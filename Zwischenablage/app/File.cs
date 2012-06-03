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

namespace Zwischenablage.app
{
    public class File
    {
        private Random rnd;
        private int id;
        private String fileName;
        private DateTime creationDate;
        private DateTime? deletionDate;
        private String mimeType;
        private Int64 fileSize;

        public String PhysicalPath
        {
            get { return HttpRuntime.AppDomainAppPath + "clipboard\\" + this.fileName; }
        }

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

        public Int64 FileSize
        {
            get { return this.fileSize; }
            set { this.fileSize = value; }
        }

        public String FileSizeString
        {
            get
            {
                Double size = fileSize;
                String suffix = "Bytes";
                if (size > 1024)
                {
                    size = size / 1024;
                    suffix = "KiB";
                }

                if (size > 1024)
                {
                    size = size / 1024;
                    suffix = "MiB";
                }
                if (size > 1024)
                {
                    size = size / 1024;
                    suffix = "GiB";
                }

                return String.Format("{0:0.00}", size) + " " + suffix;
            }
        }

        public String MimeType
        {
            get { return this.mimeType; }
            set { this.mimeType = value; }
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

        public File(string fileName, String mimeType, Int64 fileSize, DateTime? deletionDate)
        {
            this.rnd = new Random(DateTime.Now.Millisecond);
            this.id = rnd.Next(1000, 1000000);
            this.fileName = fileName;
            this.mimeType = mimeType;
            this.creationDate = DateTime.Now;
            this.deletionDate = deletionDate;
            this.fileSize = fileSize;
        }

        public File()
        {
        }

        public void CreateNewID()
        {
            this.id = rnd.Next(1000, 1000000);
        }

        public void Delete() {
            if (this.id != -1 && this.id != 1)
            {
                FileManager.deleteFileByID(this.id);
                System.IO.File.Delete(this.PhysicalPath);
            }
        }

        public String GetMimeTypeImageURL
        {
            get
            {
                String imageURL = "img/mimetypes/";
                String[] parts;
                switch (this.mimeType)
                {
                    case "application/gzip":
                    case "application/x-gtar":
                    case "application/x-tar":
                    case "application/zip":
                    case "application/x-7z-compressed":
                    case "application/x-compress":
                    case "application/x-stuffit":
                        imageURL += "tar.png";
                        break;
                    case "application/msexcel":
                    case "application/vnd.oasis.opendocument.spreadsheet":
                    case "application/x-vnd.oasis.opendocument.spreadsheet":
                        imageURL += "kchart_chrt.png";
                        break;
                    case "application/mspowerpoint":
                    case "application/vnd.oasis.opendocument.presentation":
                    case "application/x-vnd.oasis.opendocument.presentation":
                        imageURL += "pps.png";
                        break;
                    case "application/msword":
                    case "application/vnd.oasis.opendocument.text":
                    case "application/x-vnd.oasis.opendocument.text":
                    case "application/rtf":
                    case "text/richtext":
                    case "text/rtf":
                        imageURL += "doc.png";
                        break;
                    case "application/octet-stream":
                    case "application/x-macbinary":
                        parts = this.fileName.Split('.');
                        if (parts[parts.Length - 1].ToLower().Equals(".exe"))
                        {
                            imageURL += "exec_wine.png";
                        }
                        else if (parts[parts.Length - 1].ToLower().Equals(".ttf"))
                        {
                            imageURL += "font_truetype.png";
                        }
                        else
                        {
                            imageURL += "binary.png";
                        }
                        break;
                    case "application/x-msdownload":
                    case "application/x-exe":
                    case "application/exe":
                    case "application/x-winexe":
                    case "application/msdos-windows":
                        imageURL += "exec_wine.png";
                        break;                        
                    case "application/pdf":
                    case "application/postscript":
                        imageURL += "pdf.png";
                        break;
                    case "application/xhtml+xml":
                    case "application/x-www-form-urlencoded":
                    case "text/html":
                        imageURL += "html.png";
                        break;
                    case "application/x-httpd-php":
                        imageURL += "php.png";
                        break;
                    case "application/x-latex":
                    case "application/x-tex":
                        imageURL += "tex.png";
                        break;
                    case "application/x-sh":
                        imageURL += "shellscript.png";
                        break;
                    case "application/x-shockwave-flash":
                        imageURL += "video.png";
                        break;
                    case "audio/x-aiff":
                    case "audio/x-mpeg":
                    case "audio/x-wav":
                    case "audio/mp4":
                    case "audio/mp3":
                    case "audio/mpg":
                    case "audio/mpeg":
                    case "audio/x-matroska":
                    case "audio/ogg":
                    case "application/ogg":
                        imageURL += "sound.png";
                        break;
                    case "audio/x-midi":
                        imageURL += "midi.png";
                        break;
                    case "audio/x-pn-realaudio-plugin":
                    case "audio/x-pn-realaudio":
                        imageURL += "real.png";
                        break;
                    case "audio/x-qt-stream":
                    case "video/quicktime":
                        imageURL += "quicktime";
                        break;
                    case "drawing/x-dwf":
                        imageURL += "swf.png";
                        break;
                    case "image/tiff":
                    case "image/x-portable-pixmap":
                    case "image/x-icon":
                    case "image/x-rgb":
                    case "image/bmp":
                    case "image/x-bmp":
                    case "image/x-bitmap":
                    case "image/x-xbitmap":
                    case "image/x-win-bitmap":
                    case "image/x-windows-bmp":
                    case "image/ms-bmp":
                    case "image/x-ms-bmp":
                    case "application/bmp":
                    case "application/x-bmp":
                    case "application/x-win-bitmap":
                        imageURL += "image.png";
                        break;
                    case "message/rfc822":
                        imageURL += "message.png";
                        break;
                    case "image/xcf":
                        imageURL += "xcf.png";
                        break;
                    case "message/news":
                        imageURL += "news.png";
                        break;
                    case "text/comma-separated-values":
                    case "text/css":
                    case "text/tab-separated-values":
                    case "text/xml":
                    case "text/x-speech":
                        imageURL += "txt.png";
                        break;
                    case "text/plain":
                        parts = this.fileName.Split('.');
                        if (parts[parts.Length - 1].ToLower().Equals(".c"))
                        {
                            imageURL += "source_c.png";
                        }
                        else if (parts[parts.Length - 1].ToLower().Equals(".cpp"))
                        {
                            imageURL += "source_cpp.png";
                        }
                        else if (parts[parts.Length - 1].ToLower().Equals(".h"))
                        {
                            imageURL += "source_h.png";
                        }
                        else if (parts[parts.Length - 1].ToLower().Equals(".py"))
                        {
                            imageURL += "source_py.png";
                        }
                        else if (parts[parts.Length - 1].ToLower().Equals(".java"))
                        {
                            imageURL += "java_src.png";
                        }
                        else
                        {
                            imageURL += "txt.png";
                        }
                        break;
                    case "video/mpeg":
                    case "video/mp4":
                    case "video/x-msvideo":
                    case "video/x-sgi-movie":
                    case "video/mp4v-es":
                    case "video/x-matroska":
                    case "video/x-mpeg":
                    case "video/ogg":
                    case "video/webm":
                        imageURL += "video.png";
                        break;
                    case "application/pgp":
                    case "application/pgp-keys":
                    case "application/pgp-signature":
                    case "application/x-pgp-plugin":
                    case "application/pgp-encrypted":
                        imageURL += "encrypted.png";
                        break;
                    case "application/x-rpm":
                    case "application/x-redhat":
                        imageURL += "rpm.png";
                        break;
                    case "application/x-deb":
                        imageURL += "deb.png";
                        break;
                    default:
                        imageURL += "empty.png";
                        break;
                }
                return imageURL;
            }
        }
    
    }
}