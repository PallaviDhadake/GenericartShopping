using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;

public partial class supportteam_notification_image_gallery : System.Web.UI.Page
{
    iClass c = new iClass();
    public string imgGallery;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ShowImages();
        }
    }

    private void ShowImages()
    {
        try
        {
            //List<String> arrimgs = GetImagesPath();
            string[] arrFolderImgs = GetImagesPath().ToArray();
            int i = 1;
            if (arrFolderImgs.Length > 0)
            {
                StringBuilder strMarkup = new StringBuilder();
                strMarkup.Append("<table class=\"table table-striped table-bordered table-hover table-responsive-sm\" id=\"gvNotif\">");
                strMarkup.Append("<tr>");
                strMarkup.Append("<th>Sr. No.</th>");
                strMarkup.Append("<th>Image</th>");
                strMarkup.Append("<th>Image Path</th>");
                strMarkup.Append("<th>Copy Path</th>");
                strMarkup.Append("</tr>");
                foreach (string img in arrFolderImgs)
                {
                    strMarkup.Append("<tr>");
                    strMarkup.Append("<td>" + i + "</td>");
                    strMarkup.Append("<td><img src=\"" + Master.rootPath + "upload/notifImg/" + img + "\" width=\"100\" /></td>");
                    strMarkup.Append("<td><span id=\"url-" + i + "\">" + Master.rootPath + "upload/notifImg/" + img + "</span></td>");
                    strMarkup.Append("<td><span onclick=\"myFunction(" + i + ")\"  class=\"btn btn-md btn-primary\">Copy Url</span></td>");
                    strMarkup.Append("</tr>");
                    i++;
                }
                strMarkup.Append("</table>");
                imgGallery = strMarkup.ToString();
            }
            else
            {
                imgGallery = "<span class=\"btn btn-md btn-warning\">No any images added yet</span>";
            }
        }
        catch (Exception ex)
        {
            imgGallery = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    public List<String> GetImagesPath()
    {
        try
        {
            string path = "~/upload/notifImg/";
            DirectoryInfo Folder;
            FileInfo[] Images;

            Folder = new DirectoryInfo(MapPath(path));
            Images = Folder.GetFiles();
            List<String> imagesList = new List<String>();

            for (int i = 0; i < Images.Length; i++)
            {
                imagesList.Add(String.Format(@"{0}", Images[i].Name));
            }


            return imagesList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}