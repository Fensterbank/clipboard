using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zwischenablage.app
{
    public class File
    {
        private int id;
        private String fileName;
        private DateTime creationDate;
        private DateTime? deletionDate;

        public int ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public String FileName
        { 
            get { return this.fileName; }
            set { this.fileName = value; }
        }

        public DateTime CreationDate
        {
            get { return this.creationDate; }
            set { this.creationDate = value; }
        }

        public DateTime? DeletionDate
        {
            get { return this.deletionDate; }
            set { this.deletionDate = value; }
        }

        public File(string fileName, DateTime? deletionDate)
        {            
            this.fileName = fileName;
            this.creationDate = DateTime.Now;
            this.deletionDate = deletionDate;
        }

        public File()
        {
        }
    
    }
}