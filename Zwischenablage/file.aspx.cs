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