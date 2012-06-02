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
            if (!this.IsPostBack)
            {
                form1.Attributes.Add("class", "well");
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
                    File importedFile = new File(fileName, null);
                    int fileID = FileDal.createFile(importedFile).ID;
                    mvMain.SetActiveView(viewResult);
                    tbDirectLink.Text = "http://clipboard.bolvin.de/clipboard/" + fileName;
                    tbPageLink.Text = "http://clipboard.bolvin.de/show.aspx?id=" + fileID;
                    form1.Attributes.Add("class", "well form-horizontal");
                }
                catch (Exception)
                {

                }
            }
        }        
    }
}