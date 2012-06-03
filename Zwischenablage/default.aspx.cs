using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zwischenablage.app;

namespace Zwischenablage
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Always check, if files have to be deleted
            FileManager.TriggerDeletion();

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
                catch (Exception)
                {

                }
            }
        }        
    }
}