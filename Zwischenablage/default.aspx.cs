﻿/*
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
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Zwischenablage.app;

namespace Zwischenablage
{
    public partial class _default : System.Web.UI.Page
    {
        String _UploadPassword;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Always check, if files have to be deleted
            FileManager.TriggerDeletion();

            _UploadPassword = ConfigurationManager.AppSettings["UploadPassword"];


            if (!String.IsNullOrWhiteSpace(_UploadPassword))
            {
                phUploadPassword.Visible = true;
            }
            else
            {
                phUploadPassword.Visible = false;
            }


            if (!this.IsPostBack)
            {
                form1.Attributes.Add("class", "well form-horizontal");
                mvMain.SetActiveView(viewUpload);
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (upFile.PostedFile != null && upFile.PostedFile.ContentLength > 0)
            {                
                String fileName = upFile.PostedFile.FileName;
                fileName = fileName.Replace(' ', '_').Replace("ß", "ss").Replace("ü", "ue").Replace("ö", "oe").Replace("ä", "ae");
                String SaveLoc = Server.MapPath("clipboard") + "\\" + fileName;

                try
                {
                    if (!String.IsNullOrWhiteSpace(_UploadPassword) && !tbUploadPassword.Text.Equals(_UploadPassword))
                    {
                        throw new UnauthorizedAccessException("Incorrect upload password");
                    }

                    upFile.PostedFile.SaveAs(SaveLoc);                    
                    DateTime? deletionDate = null;

                    switch (Convert.ToInt32(ddlDeleteAfter.SelectedValue))
                    {
                        case 1:
                            // Delete after 30 Minutes                            
                            deletionDate = DateTime.Now.AddMinutes(30);
                            break;
                        case 2:                            
                            // Delete after 6 hours                            
                            deletionDate = DateTime.Now.AddHours(6);
                            break;
                        case 3:
                            // Delete after 1 day
                            deletionDate = DateTime.Now.AddDays(1);
                            break;
                        case 4:
                            // Delete after 1 week                            
                            deletionDate = DateTime.Now.AddDays(7);
                            break;
                        case 5:
                            // Delete after 1 month
                            deletionDate = DateTime.Now.AddMonths(1);
                            break;
                        case 6:                            
                            // Never delete
                            break;
                    }


                    File importedFile = new File(fileName, upFile.PostedFile.ContentType, upFile.PostedFile.ContentLength, deletionDate);
                    int fileID = FileManager.createFile(importedFile).ID;
                    mvMain.SetActiveView(viewResult);
                    tbDirectLink.Text = "http://clipboard.bolvin.de/clipboard/" + fileName;                    
                    tbPageLink.Text = "http://clipboard.bolvin.de/file.aspx?id=" + fileID;
                    form1.Attributes.Add("class", "well form-horizontal");
                }
                catch (Exception ex)
                {
  
                }
            }
        }        
    }
}