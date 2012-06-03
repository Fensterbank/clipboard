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
using System.Web.UI;
using System.Web.UI.WebControls;
using Zwischenablage.app;

namespace Zwischenablage
{
    public partial class file : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FileManager.TriggerDeletion();
            try
            {
                int FileID = Convert.ToInt32(Request.QueryString["id"]);
                File foundFile = FileManager.getFileByID(FileID);

                if (!System.IO.File.Exists(foundFile.PhysicalPath)) {
                    throw new Exception("File not found in path");
                }
                litTitle.Text = foundFile.FileName;
                lnkImage.Text = foundFile.FileName;
                lnkImage.NavigateUrl = "/clipboard/" + foundFile.FileName;
                litInfoUploadedDate.Text = foundFile.CreationDate.ToShortDateString() + ", " + foundFile.CreationDate.ToShortTimeString();
                litInfoFileSize.Text = foundFile.FileSizeString;
                litFilename.Text = foundFile.FileName;

                switch (foundFile.MimeType)
                {
                    case "image/jpeg":
                    case "image/png":
                    case "image/gif":                        
                        lnkImage.ImageUrl = "/clipboard/" + foundFile.FileName;                                                                                               
                        break;
                    default:
                        lnkImage.ImageUrl = foundFile.GetMimeTypeImageURL;
                        divThumbnail.Attributes.Add("style", "width: 270px; margin-left: auto; margin-right: auto;");
                        break;
                }
                mvMain.SetActiveView(viewShow);
            }
            catch (Exception)
            {
                mvMain.SetActiveView(viewError);
                litTitle.Text = "Sorry dude, file not found!";
            }

        }
    }
}