using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Zwischenablage.app
{
    public class FileDal
    {
        #region SQL Statements
        private const string getAllFilesStatement = "SELECT * FROM files";
        private const string getFileByIDStatement = "SELECT * FROM files WHERE id = @id";
        private const string getLastIDStatement = "SELECT MAX(id) AS id FROM files";
        private const string createFileStatement = "INSERT INTO [files] ([filename],[creationDate],[deletionDate])  VALUES (@filename, @creationDate, @deletionDate )";        
        private const string deleteFileStatement = "DELETE FROM files WHERE id = @id";
        #endregion

        #region Methods
        public static List<File> getAllFiles()
        {
            DataTable dt = SQLHelper.ExecuteDataTable(getAllFilesStatement);
            return convertToFiles(dt);
        }

        public static File getFileByID(int id)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);
            DataTable dt = SQLHelper.ExecuteDataTable(getFileByIDStatement, parameters);
            return convertToFile(dt.Rows[0]);
        }

        public static File createFile(File entry)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@filename", entry.FileName);
            parameters.Add("@creationDate", entry.CreationDate);
            parameters.Add("@deletionDate", entry.DeletionDate);            

            try
            {
                SQLHelper.ExecuteDataTable(createFileStatement, parameters);
            }
            catch (Exception)
            {
                return null;
            }
            DataTable dt = SQLHelper.ExecuteDataTable(getLastIDStatement);
            entry.ID = Convert.ToInt32(dt.Rows[0]["id"]);
            return entry;
        }

       
        public static Boolean deleteFileByID(int id)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);
            try
            {
                SQLHelper.ExecuteDataTable(deleteFileStatement, parameters);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Converter Methods

        /// <summary>
        /// Konvertiert eine DataTable zu einer Liste von NewsPosts
        /// </summary>
        private static List<File> convertToFiles(DataTable dt)
        {
            List<File> entries = new List<File>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                entries.Add(convertToFile(dt.Rows[i]));
            }
            return entries;
        }

        /// <summary>
        /// Konvertiert eine DataRow zu einem NewsPost
        /// </summary>
        private static File convertToFile(DataRow dr)
        {
            File entry = new File();

            if (!dr["id"].Equals(DBNull.Value)) entry.ID = (int)dr["id"];
            if (!dr["filename"].Equals(DBNull.Value)) entry.FileName = (String)dr["filename"];
            if (!dr["creationDate"].Equals(DBNull.Value)) entry.CreationDate = (DateTime)dr["creationDate"];
            if (!dr["deletionDate"].Equals(DBNull.Value)) entry.DeletionDate = (DateTime)dr["deletionDate"];            
            return entry;
        }
        #endregion
    }
}