/*
    Clipboard - A simple file and image uploader and viewer
    Copyright (C) 2012 Frédéric Bolvin

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>. 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Zwischenablage.app
{
    public class FileManager
    {
        #region SQL Statements
        private const string getAllFilesStatement = "SELECT * FROM files";
        private const string getFileByIDStatement = "SELECT * FROM files WHERE id = @id";
        private const string checkFreeIDStatement = "SELECT id FROM files WHERE id = @id";
        private const string createFileStatement = "INSERT INTO [files] ([id], [filename],[mimeType], [fileSize], [creationDate],[deletionDate])  VALUES (@id, @filename, @mimeType, @fileSize, @creationDate, @deletionDate )";        
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
            if (dt.Rows.Count == 0)
            {
                throw new Exception("File ID not found in database");
            }
            else
            {
                return convertToFile(dt.Rows[0]);
            }
        }

        public static File createFile(File entry)
        {            
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", entry.ID);
            parameters.Add("@filename", entry.FileName);
            parameters.Add("@creationDate", entry.CreationDate);
            parameters.Add("@deletionDate", entry.DeletionDate);
            parameters.Add("@mimeType", entry.MimeType);
            parameters.Add("@fileSize", entry.FileSize);   

            try
            {
                DataTable dt = SQLHelper.ExecuteDataTable(checkFreeIDStatement, parameters);

                while (dt.Rows.Count > 0)
                {
                    entry.CreateNewID();
                    parameters["@id"] = entry.ID;
                    dt = SQLHelper.ExecuteDataTable(checkFreeIDStatement, parameters);
                }

                SQLHelper.ExecuteDataTable(createFileStatement, parameters);
            }
            catch (Exception)
            {
                return null;
            }                       
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
            if (!dr["mimeType"].Equals(DBNull.Value)) entry.MimeType = (String)dr["mimeType"];
            if (!dr["fileSize"].Equals(DBNull.Value)) entry.FileSize = (Int64)dr["fileSize"];
            if (!dr["creationDate"].Equals(DBNull.Value)) entry.CreationDate = (DateTime)dr["creationDate"];
            if (!dr["deletionDate"].Equals(DBNull.Value)) entry.DeletionDate = (DateTime)dr["deletionDate"];            
            return entry;
        }
        #endregion

        #region Logic

        public static void TriggerDeletion()
        {
            List<File> fileList = FileManager.getAllFiles();
            DateTime now = DateTime.Now;

            foreach (File f in fileList)
            {                
                if (f.DeletionDate != null && now > f.DeletionDate)
                {
                    f.Delete();                    
                }
            }
        }


        #endregion
    }
}