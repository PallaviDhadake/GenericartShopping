using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class supportteam_banner_master : System.Web.UI.Page
{
    public string errMsg, pgTitle;
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        pgTitle = Request.QueryString["action"] == "new" ? "Add Banner" : "Edit Banner";
        btnSave.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        btnDelete.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnDelete, null) + ";");
        btnCancel.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnCancel, null) + ";");

        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["action"] != null)
                {
                    editBanner.Visible = true;
                    viewBanner.Visible = false;

                    if (Request.QueryString["action"] == "new")
                    {
                        btnSave.Text = "Save";
                        btnDelete.Visible = false;
                    }
                    else
                    {
                        btnSave.Text = "Modify";
                        btnDelete.Visible = true;
                        GetBannerData(Convert.ToInt32(Request.QueryString["id"]));
                    }
                }
                else
                {
                    viewBanner.Visible = true;
                    editBanner.Visible = false;
                    FillGrid();
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    private void FillGrid()
    {
        try
        {
            using (DataTable dtBanner = c.GetDataTable("Select bannerId, imageName, displayOrder From BannerData Where delMark=0 Order By displayOrder"))
            {
                gvBanner.DataSource = dtBanner;
                gvBanner.DataBind();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FillGrid", ex.Message.ToString());
            return;
           
        }
    }

    protected void gvBanner_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Literal litBanner = (Literal)e.Row.FindControl("litBanner");
            string rPath = c.ReturnHttp();
            litBanner.Text = "<img src=\"" + rPath + "upload/banner/" + e.Row.Cells[1].Text + "\" width=\"200\" height=\"100\" />";


            Button btnUp = (Button)e.Row.FindControl("moveUp");
            if (e.Row.Cells[3].Text == "1")
            {
                btnUp.Enabled = false;
                btnUp.Attributes["style"] = "background:none;";
            }

            Button btnDown = (Button)e.Row.FindControl("moveDown");
            int maxOrd = Convert.ToInt32(c.returnAggregate("Select MAX(displayOrder) From BannerData Where delMark=0"));
            if (Convert.ToInt32(e.Row.Cells[3].Text) == maxOrd)
            {
                btnDown.Visible = false;
            }

            Literal litAnch = (Literal)e.Row.FindControl("litAnch");
            litAnch.Text = "<a href=\"banner-master.aspx?action=edit&id=" + e.Row.Cells[0].Text + "\" class=\"gAnch\"></a>";
        }
    }

    protected void gvBanner_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvBanner.PageIndex = e.NewPageIndex;
        FillGrid();
    }
    protected void gvBanner_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridViewRow gRow = (GridViewRow)((Button)e.CommandSource).NamingContainer;
            if (e.CommandName == "Up")
            {
                int displayOrd = Convert.ToInt32(gRow.Cells[3].Text);
                int previouRow = Convert.ToInt32(gRow.Cells[0].Text);
                c.ExecuteQuery("Update BannerData Set displayOrder=" + displayOrd + " Where displayOrder=" + (displayOrd - 1));
                c.ExecuteQuery("Update BannerData Set displayOrder=" + (displayOrd - 1) + " Where bannerId=" + previouRow);
            }
            if (e.CommandName == "Down")
            {
                int displayOrd = Convert.ToInt32(gRow.Cells[3].Text);
                int previouRow = Convert.ToInt32(gRow.Cells[0].Text);
                c.ExecuteQuery("Update BannerData Set displayOrder=" + displayOrd + " Where displayOrder=" + (displayOrd + 1));
                c.ExecuteQuery("Update BannerData Set displayOrder=" + (displayOrd + 1) + " Where bannerId=" + previouRow);
            }
            if (e.CommandName == "gvDel")
            {
                int dispOrder = Convert.ToInt32(c.GetReqData("BannerData", "displayOrder", "bannerId=" + gRow.Cells[0].Text));
                if (dispOrder > 0)
                {
                    string maxDispOrder = c.returnAggregate("Select MAX(displayOrder) From BannerData Where delMark=0").ToString();
                    if (dispOrder == Convert.ToInt32(maxDispOrder))
                    {
                        c.ExecuteQuery("Delete From BannerData Where bannerId=" + gRow.Cells[0].Text);
                    }
                    string bannerIds = "";
                    using (DataTable dtDispOrderIds = c.GetDataTable("Select bannerId From BannerData Where delMark=0 AND displayOrder>" + dispOrder))
                    {
                        if (dtDispOrderIds.Rows.Count > 0)
                        {
                            foreach (DataRow row in dtDispOrderIds.Rows)
                            {
                                if (bannerIds == "")
                                    bannerIds = row["bannerId"].ToString();
                                else
                                    bannerIds = bannerIds + ", " + row["bannerId"].ToString();
                            }

                            c.ExecuteQuery("Update BannerData Set displayOrder=displayOrder-1 Where bannerId IN (" + bannerIds + ")");
                            c.ExecuteQuery("Delete From BannerData Where bannerId=" + gRow.Cells[0].Text);
                        }
                    }
                }
                else
                {
                    c.ExecuteQuery("Delete From BannerData Where bannerId=" + gRow.Cells[0].Text);
                }

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Banner Photo Removed');", true);
               
            }
            FillGrid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvBanner_RowCommand", ex.Message.ToString());
            return;

        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            txtLink.Text = txtLink.Text.Trim().Replace("'", "");

            int maxId = lblId.Text == "[New]" ? c.NextId("BannerData", "bannerId") : Convert.ToInt32(lblId.Text);
            int bannerCount = Convert.ToInt32(c.returnAggregate("Select Count(bannerId) From BannerData Where delMark=0").ToString());
            if (bannerCount > 10)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'you can not add more than 10 banners');", true);
               
                return;
            }

            if (txtLink.Text != "")
            {
                if (!txtLink.Text.Contains("http"))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid URL');", true);
                    return;
                }
            }

            if (rdbAndroid.Checked == true)
            {
                if (ddrLink.SelectedIndex == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select android Link');", true);
                    return;
                }
            }

            if (rdbProd.Checked == true)
            {
                if (txtProd.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Product Name');", true);
                    return;
                }
            }

            if (rdbCat.Checked == true)
            {
                if (txtCat.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Category Name');", true);
                    return;
                }
            }

            string dispOrder = "";
            string fileName = "";
            if (fuImg.HasFile)
            {
                string fExt = Path.GetExtension(fuImg.FileName).ToString().ToLower();
                if (fExt == ".png" || fExt == ".jpg" || fExt == ".jpeg")
                {
                    fileName = "banner-" + maxId + fExt;
                    dispOrder = c.returnAggregate("Select MAX(displayOrder) From BannerData Where delMark=0").ToString();
                    ImageUploadProcess(fileName);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Only .png, .jpg, and .jpeg files are allowed');", true);
                    return;
                }
            }
            else
            {
                if (lblId.Text == "[New]")
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select image to upload');", true);
                    return;
                   
                }
            }
            int displayOrder = 0;
            if (dispOrder != "")
            {
                displayOrder = Convert.ToInt32(dispOrder) + 1;
            }
            else
            {
                displayOrder = 0;
            }

            if (lblId.Text == "[New]")
            {
                c.ExecuteQuery("Insert Into BannerData(bannerId, imageName, displayOrder, delMark, bannerLink) Values (" + maxId + ",'" + fileName +
                    "', " + displayOrder + ", 0, '" + txtLink.Text + "')");

                if (rdbAndroid.Checked == true)
                {
                    if (ddrLink.SelectedIndex > 0)
                    {
                        c.ExecuteQuery("Update BannerData Set AndroidURL='" + ddrLink.SelectedItem.Text + "' Where bannerId=" + maxId);
                    }
                }
                if (rdbProd.Checked == true)
                {
                    if (txtProd.Text != "")
                    {
                        int prodId = Convert.ToInt32(c.GetReqData("ProductsData", "ProductID", "ProductName='" + txtProd.Text + "' AND delMark=0 AND ProductActive=1"));
                        c.ExecuteQuery("Update BannerData Set AndroidURL='Product-Page-" + prodId + "' Where bannerId=" + maxId);
                    }
                }
                if (rdbCat.Checked == true)
                {
                    if (txtCat.Text != "")
                    {
                        int catId = Convert.ToInt32(c.GetReqData("HealthProductsData", "HealthProdId", "HealthProdName='" + txtCat.Text + "' AND delMark=0"));
                        string catName = "Category-Page-" + catId + "-" + c.UrlGenerator(txtCat.Text).ToString();
                        c.ExecuteQuery("Update BannerData Set AndroidURL='" + catName + "' Where bannerId=" + maxId);
                    }
                }

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Banner added successfully...!!');", true);
                
               
            }
            else
            {
                if (fuImg.HasFile)
                {
                    int dOrder = Convert.ToInt32(c.GetReqData("BannerData", "displayOrder", "bannerId=" + maxId));
                    if (dOrder == 0)
                    {
                        c.ExecuteQuery("Update BannerData Set imageName='" + fileName + "', bannerLink='" + txtLink.Text + "', displayOrder=" + displayOrder + " Where bannerId=" + maxId);
                    }
                    else
                    {
                        c.ExecuteQuery("Update BannerData Set imageName='" + fileName + "', bannerLink='" + txtLink.Text + "'  Where bannerId=" + maxId);
                    }
                }
                else
                {
                    c.ExecuteQuery("Update BannerData Set bannerLink='" + txtLink.Text + "'  Where bannerId=" + maxId);
                }

                if (rdbAndroid.Checked == true)
                {
                    if (ddrLink.SelectedIndex > 0)
                    {
                        c.ExecuteQuery("Update BannerData Set AndroidURL='" + ddrLink.SelectedItem.Text + "' Where bannerId=" + maxId);
                    }
                }
                if (rdbProd.Checked == true)
                {
                    if (txtProd.Text != "")
                    {
                        int prodId = Convert.ToInt32(c.GetReqData("ProductsData", "ProductID", "ProductName='" + txtProd.Text + "' AND delMark=0 AND ProductActive=1"));
                        c.ExecuteQuery("Update BannerData Set AndroidURL='Product-Page-" + prodId + "' Where bannerId=" + maxId);
                    }
                }
                if (rdbCat.Checked == true)
                {
                    if (txtCat.Text != "")
                    {
                        int catId = Convert.ToInt32(c.GetReqData("HealthProductsData", "HealthProdId", "HealthProdName='" + txtCat.Text + "' AND delMark=0"));
                        string catName = "Category-Page-" + catId + "-" + c.UrlGenerator(txtCat.Text).ToString();
                        c.ExecuteQuery("Update BannerData Set AndroidURL='" + catName + "' Where bannerId=" + maxId);
                    }
                }
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Banner updated successfully...!!');", true);
               
            }

            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('banner-master.aspx', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSave_Click", ex.Message.ToString());
            return;

        }
    }

    private void ImageUploadProcess(string imgName)
    {
        try
        {
            string origImgPath = "~/upload/banner/";

            fuImg.SaveAs(Server.MapPath(origImgPath) + imgName);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "ImageUploadProcess", ex.Message.ToString());
            return;

        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            int dispOrder = Convert.ToInt32(c.GetReqData("BannerData", "displayOrder", "bannerId=" + Request.QueryString["id"]));
            if (dispOrder > 0)
            {
                string maxDispOrder = c.returnAggregate("Select MAX(displayOrder) From BannerData Where delMark=0").ToString();
                if (dispOrder == Convert.ToInt32(maxDispOrder))
                {
                    c.ExecuteQuery("Delete From BannerData Where bannerId=" + Request.QueryString["id"]);
                    //c.ExecuteQuery("Update BannerData Set delMark=1 Where bannerId=" + Request.QueryString["id"]);
                }
                string bannerIds = "";
                using (DataTable dtDispOrderIds = c.GetDataTable("Select bannerId From BannerData Where delMark=0 AND displayOrder>" + dispOrder))
                {
                    if (dtDispOrderIds.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtDispOrderIds.Rows)
                        {
                            if (bannerIds == "")
                                bannerIds = row["bannerId"].ToString();
                            else
                                bannerIds = bannerIds + ", " + row["bannerId"].ToString();
                        }

                        c.ExecuteQuery("Update BannerData Set displayOrder=displayOrder-1 Where bannerId IN (" + bannerIds + ")");
                        c.ExecuteQuery("Delete From BannerData Where bannerId=" + Request.QueryString["id"]);
                        //c.ExecuteQuery("Update BannerData Set delMark=1 Where bannerId=" + Request.QueryString["id"]);
                    }
                }
            }
            else
            {
                //c.ExecuteQuery("Update BannerData Set delMark=1 Where bannerId=" + Request.QueryString["id"]);
                c.ExecuteQuery("Delete From BannerData Where bannerId=" + Request.QueryString["id"]);
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Banner deleted successfully...!!');", true);
            
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('banner-master.aspx', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnDelete_Click", ex.Message.ToString());
            return;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("banner-master.aspx", false);
    }

    private void GetBannerData(int bIdX)
    {
        try
        {
            using (DataTable dtBanner = c.GetDataTable("Select bannerId, imageName, bannerLink, AndroidURL From BannerData Where bannerId=" + bIdX))
            {
                if (dtBanner.Rows.Count > 0)
                {
                    lblId.Text = bIdX.ToString();

                    DataRow row = dtBanner.Rows[0];

                    txtLink.Text = row["bannerLink"] != DBNull.Value && row["bannerLink"] != null && row["bannerLink"].ToString() != "" ? row["bannerLink"].ToString() : "";

                    if (row["AndroidURL"] != DBNull.Value && row["AndroidURL"] != null && row["AndroidURL"].ToString() != "")
                    {
                        if (row["AndroidURL"].ToString().Contains("Product-Page"))
                        {
                            rdbProd.Checked = true;
                            ddrLink.Enabled = false;
                            txtCat.Enabled = false;
                            string[] arrProd = row["AndroidURL"].ToString().Split('-');
                            int prodId = Convert.ToInt32(arrProd[arrProd.Length - 1]);
                            txtProd.Text = c.GetReqData("ProductsData", "ProductName", "ProductID=" + prodId).ToString();
                        }
                        else if (row["AndroidURL"].ToString().Contains("Category-Page"))
                        {
                            rdbCat.Checked = true;
                            ddrLink.Enabled = false;
                            txtProd.Enabled = false;
                            string[] arrCat = row["AndroidURL"].ToString().Split('-');
                            int catId = Convert.ToInt32(arrCat[2]);
                            txtCat.Text = c.GetReqData("HealthProductsData", "HealthProdName", "HealthProdId=" + catId).ToString();
                        }
                        else
                        {
                            rdbAndroid.Checked = true;
                            txtProd.Enabled = false;
                            txtCat.Enabled = false;

                            switch (row["AndroidURL"].ToString())
                            {
                                case "Search-Shop": ddrLink.SelectedValue = "1"; break;
                                case "Know-Your-Savings": ddrLink.SelectedValue = "2"; break;
                                case "Upload-Prescription": ddrLink.SelectedValue = "3"; break;
                                case "Doctor-Consultation": ddrLink.SelectedValue = "4"; break;
                                case "QC-Report": ddrLink.SelectedValue = "5"; break;
                                case "Medicine-Reminder": ddrLink.SelectedValue = "6"; break;
                                case "generic-mitra-info": ddrLink.SelectedValue = "7"; break;
                                case "business-opportunity": ddrLink.SelectedValue = "8"; break;

                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetBannerData", ex.Message.ToString());
            return;
        }
    }

    protected void rdbAndroid_CheckedChanged(object sender, EventArgs e)
    {
        txtProd.Enabled = false;
        txtCat.Enabled = false;
        ddrLink.Enabled = true;
    }

    protected void rdbProd_CheckedChanged(object sender, EventArgs e)
    {
        txtCat.Enabled = false;
        ddrLink.Enabled = false;
        txtProd.Enabled = true;
    }

    protected void rdbCat_CheckedChanged(object sender, EventArgs e)
    {
        txtProd.Enabled = false;
        ddrLink.Enabled = false;
        txtCat.Enabled = true;
    }

    [WebMethod]
    public static List<string> GetSearchControl(string prefix)
    {
        iClass c = new iClass();
        List<string> customers = new List<string>();
        //using (SqlConnection conn = new SqlConnection())
        //{
        //    conn.ConnectionString = MasterClass.Getconnection();
        //    using (SqlCommand cmd = new SqlCommand())
        //    {
        //        cmd.CommandText = "SELECT ProductID, ProductName as Search FROM ProductsData WHERE ProductName like @SearchText + '%' and isnull(ProductActive, 0) = 1";
        //        cmd.Parameters.AddWithValue("@SearchText", prefix);
        //        cmd.Connection = conn;
        //        conn.Open();
        //        using (SqlDataReader sdr = cmd.ExecuteReader())
        //        {
        //            while (sdr.Read())
        //            {
        //                customers.Add(string.Format("{0}-{1}", sdr["Search"], sdr["Search"]));
        //            }
        //        }
        //        conn.Close();
        //    }
        //}

        using (DataTable dtProd = c.GetDataTable("SELECT ProductID, ProductName FROM ProductsData WHERE ProductName like '" + prefix + "%' and delMark=0 AND isnull(ProductActive, 0) = 1"))
        {
            if (dtProd.Rows.Count > 0)
            {
                foreach (DataRow row in dtProd.Rows)
                {
                    customers.Add(string.Format("{0}", row["ProductName"]));
                }
            }
            else
            {
                customers.Add("Match Not Found");
            }
        }
        return customers;
    }
    //GetCategories
    [WebMethod]
    public static List<string> GetCategories(string prefix)
    {
        iClass c = new iClass();
        List<string> categories = new List<string>();
        using (DataTable dtCat = c.GetDataTable("SELECT HealthProdId, HealthProdName FROM HealthProductsData WHERE HealthProdName like '" + prefix + "%' and delMark=0"))
        {
            if (dtCat.Rows.Count > 0)
            {
                foreach (DataRow row in dtCat.Rows)
                {
                    categories.Add(string.Format("{0}", row["HealthProdName"]));
                }
            }
            else
            {
                categories.Add("Match Not Found");
            }
        }
        return categories;
    }
}