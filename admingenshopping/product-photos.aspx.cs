using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;

public partial class admingenshopping_product_photos : System.Web.UI.Page
{
    public string pgTitle, errMsg, videoPreview;
    public string photoMarkup;
    protected iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            //Fill Dropdown list of Products selection
            c.FillComboBox("ProductName", "ProductID", "ProductsData", "delMark=0", "ProductName", 0, ddrProduct);

            if (Request.QueryString["prId"] != null)
            {
                ddrProduct.SelectedValue = Request.QueryString["prId"].ToString();
                GetAlbumPhotos(Convert.ToInt32(ddrProduct.SelectedValue));
            }

            if (!IsPostBack)
            {
                //Check Delete photo query string request
                if (Request.QueryString["id"] != null)
                {
                    ddrProduct.SelectedValue = Convert.ToInt32(c.GetReqData("ProductPhotos", "FK_ProductID", "ProductPhotoID=" + Request.QueryString["id"])).ToString();
                    c.ExecuteQuery("Delete From ProductPhotos Where ProductPhotoID=" + Convert.ToInt32(Request.QueryString["id"]));
                    GetAlbumPhotos(Convert.ToInt32(ddrProduct.SelectedValue));

                    errMsg = c.ErrNotification(1, "Photo Deleted");
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myJsFunction", "waitAndMove('product-photos.aspx?prId=" + ddrProduct.SelectedValue + "', 1000);", true);
                }

                // check default pic querystring
                if (Request.QueryString["defaultPic"] != null)
                {
                    ddrProduct.SelectedValue = Convert.ToInt32(c.GetReqData("ProductPhotos", "FK_ProductID", "ProductPhotoID=" + Request.QueryString["defaultPic"])).ToString();
                    string photoName = c.GetReqData("ProductPhotos", "PhotoName", "ProductPhotoID=" + Request.QueryString["defaultPic"]).ToString();
                    c.ExecuteQuery("Update ProductsData Set ProductPhoto='" + photoName + "' Where ProductID=" + ddrProduct.SelectedValue);
                    GetAlbumPhotos(Convert.ToInt32(ddrProduct.SelectedValue));
                    errMsg = c.ErrNotification(1, "Default Photo Set Successfully..!!");
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "myJsFunction", "waitAndMove('product-photos.aspx', 1000);", true);
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myJsFunction", "waitAndMove('product-photos.aspx?prId=" + ddrProduct.SelectedValue + "', 1000);", true);
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    protected void ddrProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddrProduct.SelectedIndex > 0)
        {
            GetAlbumPhotos(Convert.ToInt32(ddrProduct.SelectedValue));
        }
        else
        {
            photoMarkup = "No photos added for this Album";
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddrProduct.SelectedIndex == 0)
            {
                errMsg = c.ErrNotification(2, "Select Product to add photos");
                return;
            }
            if (fuImg.HasFile)
            {
                string fExt = Path.GetExtension(fuImg.FileName).ToString().ToLower();
                if (fExt == ".jpg" || fExt == ".jpeg" || fExt == ".png" || fExt == ".webp")
                {
                    int maxId = c.NextId("ProductPhotos", "ProductPhotoID");
                    string imgName = "product-photo-" + maxId + DateTime.Now.ToString("ddMMyyyyHHmmss") + fExt;
                    c.ExecuteQuery("Insert into ProductPhotos(ProductPhotoID, FK_ProductID, PhotoName, DefaultFlag) Values(" + maxId +
                        ", " + ddrProduct.SelectedValue + ", '" + imgName + "', 0) ");

                    if (fExt != ".webp")
                    {
                        ImageUploadProcess(imgName);
                    }
                    else
                    {
                        string normalImgPath = "~/upload/products/";
                        string thumbImgPath = "~/upload/products/thumb/";

                        fuImg.SaveAs(Server.MapPath(normalImgPath) + imgName);
                        fuImg.SaveAs(Server.MapPath(thumbImgPath) + imgName);
                    }

                    errMsg = c.ErrNotification(1, "Image uploaded");

                    GetAlbumPhotos(Convert.ToInt32(ddrProduct.SelectedValue));

                }
                else
                {
                    errMsg = c.ErrNotification(2, "Only .jpg, .jpeg, .webp or .png files are allowed");
                    return;
                }
            }
            else
            {
                errMsg = c.ErrNotification(2, "Select image to upload");
                return;
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    private void ImageUploadProcess(string imgName)
    {
        try
        {
            string origImgPath = "~/upload/";
            string normalImgPath = "~/upload/products/";
            string thumbImgPath = "~/upload/products/thumb/";

            fuImg.SaveAs(Server.MapPath(origImgPath) + imgName);

            c.ImageOptimizer(imgName, origImgPath, normalImgPath, 800, true);
            c.ImageOptimizer(imgName, normalImgPath, thumbImgPath, 400, true);

            //Delete rew image from server
            File.Delete(Server.MapPath(origImgPath) + imgName);
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    private void GetAlbumPhotos(int photoIdX)
    {
        try
        {
            string rootPath = c.ReturnHttp();
            //DataTable dtPhotos = new DataTable();
            //dtPhotos = c.GetDataTable("Select * From ProductPhotos Where FK_ProductID=" + photoIdX);

            using (DataTable dtPhotos = c.GetDataTable("Select * From ProductPhotos Where FK_ProductID=" + photoIdX))
            {
                if (dtPhotos.Rows.Count > 0)
                {
                    StringBuilder strMarkup = new StringBuilder();
                    foreach (DataRow row in dtPhotos.Rows)
                    {
                        strMarkup.Append("<div class=\"imgBox\">");
                        strMarkup.Append("<div class=\"pad-5\">");
                        strMarkup.Append("<div class=\"border1\">");
                        strMarkup.Append("<div class=\"pad-5\">");
                        strMarkup.Append("<div class=\"imgContainer\">");
                        strMarkup.Append("<img src=\"" + rootPath + "upload/products/thumb/" + row["PhotoName"] + "\" class=\"w100\" />");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");

                        strMarkup.Append("<span class=\"space5\">");
                        //strMarkup.Append("<div class=\"txtCenter\"><a href=\"product-photos.aspx?defaultPic=" + row["ProductPhotoID"] + "\" class=\"btn btn-sm btn-primary\">Set as default photo</a></div>");

                        strMarkup.Append("<a href=\"product-photos.aspx?id=" + row["ProductPhotoID"] + "\" title=\"Delete Photo\"  class=\"closeAnch\"></a>");
                        strMarkup.Append("</div>");
                    }
                    strMarkup.Append("<div class=\"float_clear\"></div>");
                    strMarkup.Append("<span class=\"space30\">");
                    photoMarkup = strMarkup.ToString();
                }
                else
                {
                    photoMarkup = "No photos added for this Album";
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}