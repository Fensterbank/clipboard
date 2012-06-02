using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Globalization;

namespace Zwischenablage.app
{
    /// <summary>
    /// Kapselt den kompletten SQL Server Datenbankzugriff.
    /// </summary>
    internal static class SQLHelper
    {
        private static readonly string ConnectionString = LoadConnectionString();

        private static string LoadConnectionString()
        {
            // Lade den Datenbank Connection String aus der Konfigurationsdatei
            // der Anwendung.
            ConnectionStringSettings settings = ConfigurationManager.
                ConnectionStrings["DatabaseConnection"];


            // Werfe eine Ausnahme, wenn der Connection String nicht gefunden
            // werden konnte.
            if (settings == null)
                throw new Exception("Connection String is missing");

            // Den Connection String zurückgeben.
            return settings.ConnectionString;
        }

        private static SqlConnection GetOpenConnection()
        {
            // Eine neue, offene SqlConnection zurückgeben.
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }

        public static DataTable ExecuteDataTable(string selectCommand)
        {
            // Die Anfrage an die Überladung weiterreichen.
            return ExecuteDataTable(selectCommand, null);
        }

        public static DataTable ExecuteDataTable(string selectCommand, IDictionary<string, object> parameters)
        {
            // Validation des Parameters 'selectCommand'.
            if (selectCommand == null)
                throw new ArgumentNullException("selectCommand");

            // Einen try-Block verwenden, um alle Ausnahmen abzufangen und nur
            // einen einzigen Ausnahmetyp zu werfen: SqlError.
            try
            {
                // Eine offene Verbindung anfordern. Das using Konstrukt
                // verwenden, da SqlConnection die Schnittstelle IDisposable
                // implementiert.
                using (SqlConnection connection = SQLHelper.GetOpenConnection())
                {
                    // Einen neuen Datenbankbefehl erstellen. Das using
                    // Konstrukt verwenden, da SqlConnection die Schnittstelle
                    // IDisposable implementiert.
                    using (SqlCommand command = new SqlCommand(selectCommand,
                        connection))
                    {
                        // Alle Parameter zu dem Befehl hinzufügen.
                        AddParameters(command, parameters);

                        // Einen Datenadapter zur Befüllung der DataTable
                        // erstellen. Das using Konstrukt verwenden, da
                        // SqlDataAdapter die Schnittstelle IDisposable
                        // implementiert.
                        using (SqlDataAdapter adapter = new SqlDataAdapter(
                            command))
                        {
                            // Eine DataTable erstellen, die gefüllt wird.
                            DataTable dataTable = new DataTable();
                            dataTable.Locale = CultureInfo.InvariantCulture;

                            // Die DataTable befüllen.
                            adapter.Fill(dataTable);

                            // Die gefüllte DataTable zurückgeben.
                            return dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Eine entsprechende Fehlermeldung laden und einen SqlError
                // als Ausnahme werfen.
                throw ex;
            }
        }

        /// <summary>
        /// Ruft eine Stored Procedure auf dem SQL-Server aus
        /// </summary>
        public static DataTable RunStoredProcedure(string procedureName, IDictionary<string, object> parameters)
        {
            // Validation des Parameters 'procedureName'.
            if (procedureName == null)
                throw new ArgumentNullException("procedureName");

            // SqlCommand vorbereiten 
            SqlConnection con = GetOpenConnection();
            SqlCommand cmd = new SqlCommand(procedureName, con);
            cmd.CommandType = CommandType.StoredProcedure;

            // Parameter-Auflistung füllen 
            AddParameters(cmd, parameters);

            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                DataTable dataTable = new DataTable();
                dataTable.Locale = CultureInfo.InvariantCulture;
                adapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static object ExecuteStoredProcedure(string procedureName, IDictionary<string, object> parameters)
        {
            // Validation des Parameters 'procedureName'.
            if (procedureName == null)
                throw new ArgumentNullException("procedureName");

            // Einen try-Block verwenden, um alle Ausnahmen abzufangen und nur
            // einen einzigen Ausnahmetyp zu werfen: SqlError.
            try
            {
                // Eine offene Verbindung anfordern. Das using Konstrukt
                // verwenden, da SqlConnection die Schnittstelle IDisposable
                // implementiert.
                using (SqlConnection connection = GetOpenConnection())
                {
                    // Eine Transaktion erstellen. Das using Konstrukt
                    // verwenden, da SqlTransaction die Schnittstelle
                    // IDisposable implementiert.
                    using (SqlTransaction transaction = connection.
                        BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        // Objekt für das Entgegennehmen des Ergebnisses der
                        // gespeicherten Prozedur.
                        object result;

                        // Einen neuen Datenbankbefehl erstellen. Das using
                        // Konstrukt verwenden, da SqlConnection die
                        // Schnittstelle IDisposable implementiert.
                        using (SqlCommand command = new SqlCommand(
                            procedureName, connection, transaction))
                        {
                            // Alle Parameter zu dem Befehl hinzufügen.
                            AddParameters(command, parameters);

                            // Den Befehlstyp auf gespeicherte Prozedur stellen
                            // und diese ausführen.
                            command.CommandType = CommandType.StoredProcedure;
                            result = command.ExecuteScalar();
                        }

                        // Die Transaktion committen (es wird automatisch ein
                        // Rollback durchgeführt, wenn dieser Aufruf fehlt).
                        transaction.Commit();

                        // Das Ergebnis der gespeicherten Prozedur zurückgeben.
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                // Eine entsprechende Fehlermeldung laden und einen SqlError
                // als Ausnahme werfen.
                throw ex;
            }
        }

        public static object GetNullableDBValue(object value)
        {
            // Wenn in der Datenbank der Wert NULL steht, gibt der .NET SQL
            // Server Provider DBNull.Value zurück. Da DBNull.Value jedoch
            // nicht auf andere Typen gecastet werden kann, an dieser Stelle
            // den .NET NULL Wert zurückgeben.
            if (value == DBNull.Value)
                return null;

            // Es ist kein DBNull Wert. Den Wert so zurückgeben wie er ist.
            return value;
        }

        public static void GetNullableDBValue(object target, object value)
        {
            // Wenn in der Datenbank der Wert NULL steht, gibt der .NET SQL
            // Server Provider DBNull.Value zurück. Da DBNull.Value jedoch
            // nicht auf andere Typen gecastet werden kann, an dieser Stelle
            // den .NET NULL Wert zurückgeben.
            if (value != DBNull.Value)
                target = value;
        }

        private static void AddParameters(SqlCommand command, IDictionary<string, object> parameters)
        {
            // Parameter zu dem Befehl nur hinzufügen, wenn Parameter
            // existieren.
            if (parameters != null)
            {
                // Durch alle Schlüssel laufen (es ist ein Wörterbuch).
                foreach (string key in parameters.Keys)
                {
                    // Den vorbereiteten Wert des Parameters zur Parameters
                    // Sammlung des Befehls hinzufügen.
                    object value = PrepareParameter(parameters[key]);
                    command.Parameters.AddWithValue(key, value);
                }
            }
        }

        private static object PrepareParameter(object value)
        {
            // NULL durch DBNull.Value ersetzen.
            if (value == null)
                return DBNull.Value;

            // DateTime Wertebereiche validieren (nur beim Typ DateTime).
            if (value.GetType() == typeof(DateTime))
            {
                // Entschachteln des DateTime Wertes aus value.
                DateTime dateTime = (DateTime)value;

                // Wenn der Datumswert vor SQL Server datetime Minimum liegt,
                // SqlDateTime.MinValue zurückgeben.
                if (dateTime < SqlDateTime.MinValue.Value)
                    return SqlDateTime.MinValue;

                // Wenn der Datumswert nach SQL Server datetime Maximum liegt,
                // SqlDateTime.MaxValue zurückgeben.
                if (dateTime > SqlDateTime.MaxValue.Value)
                    return SqlDateTime.MaxValue;
            }

            // Keine Veränderung, den Wert so zurückgeben, wie er ist.
            return value;
        }
    }
}