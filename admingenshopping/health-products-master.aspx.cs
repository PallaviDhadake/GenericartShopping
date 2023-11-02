using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class admingenshopping_health_products_master : System.Web.UI.Page
{
    public string pgTitle, errMsg, hpImg;
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            btnSubmit.Attributes.Add("onclick", "this.disabled = true; this.value = 'Processing...';" + ClientScript.GetPostBackEventReference(btnSubmit, null) + ";");
            pgTitle = Request.QueryString["action"] != null ? Request.QueryString["action"] == "new" ? "Add New Health Product" : "Edit Health Product" : "Health Product Master";
            if (!IsPostBack)
            {


                if (Request.QueryString["action"] != null)
                {
                    txtName.Focus();
                    editDisease.Visible = true;
                    viewDisease.Visible = false;
                    if (Request.QueryString["action"] == "new")
                    {
                        btnDelete.Visible = false;
                        btnSubmit.Text = "Save Info";

                        assignProd.Visible = false;
                    }
                    else
                    {
                        // Fill Product Main Category
                        c.FillComboBox("ProductCatName", "ProductCatID", "ProductCategory", "delMark=0 AND ParentCatID=0", "ProductCatName", 0, ddrMainCategory);

                        GetHealthProduct(Convert.ToInt32(Request.QueryString["id"]));
                        btnDelete.Visible = true;
                        btnSubmit.Text = "Modify Info";
                        assignProd.Visible = true;
                    }
                }
                else
                {
                    editDisease.Visible = false;
                    viewDisease.Visible = true;
                    FillGrid();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "Page_Load", ex.Message.ToString());
            return;

        }
    }

    private void FillGrid()
    {
        try
        {
            using (DataTable dtHp = c.GetDataTable("Select HealthProdId, HealthProdName From HealthProductsData Where delMark=0 Order By HealthProdId DESC"))
            {
                gvHealthProd.DataSource = dtHp;
                gvHealthProd.DataBind();
                if (dtHp.Rows.Count > 0)
                {
                    gvHealthProd.UseAccessibleHeader = true;
                    gvHealthProd.HeaderRow.TableSection = TableRowSection.TableHeader;
                }

            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FillGrid", ex.Message.ToString());
            return;

        }
    }

    protected void gvHealthProd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"health-products-master.aspx?action=edit&id=" + e.Row.Cells[0].Text + "\" class=\"gAnch\"></a>";
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvHealthProd_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            txtName.Text = txtName.Text.Trim().Replace("'", "");


            if (txtName.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * marked fields are mandatory');", true);

                return;
            }

            int maxId = Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : c.NextId("HealthProductsData", "HealthProdId");

            string imgName = "";

            if (fuImg.HasFile)
            {
                string fExt = Path.GetExtension(fuImg.FileName).ToLower();
                if (fExt == ".png" || fExt == ".jpg" || fExt == ".jpeg")
                {
                    imgName = "hp-" + maxId + fExt;
                    ImageUploadProcess(imgName);
                    //fuImg.SaveAs(Server.MapPath("~/upload/diseases/") + imgName);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Only .png, .jpg and .jpeg files are allowed');", true);

                    return;
                }
            }


            if (lblId.Text == "[New]")
            {
                imgName = imgName == "" ? "no-photo.png" : imgName;
                c.ExecuteQuery("Insert Into HealthProductsData (HealthProdId, HealthProdName, HealthProdCoverPhoto, delMark) Values(" + maxId +
                    ", N'" + txtName.Text + "', '" + imgName + "', 0)");

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Health Product added successfully');", true);
            }
            else
            {
                c.ExecuteQuery("Update HealthProductsData Set HealthProdName=N'" + txtName.Text +
                    "' Where HealthProdId=" + maxId);

                if (fuImg.HasFile)
                {
                    c.ExecuteQuery("Update HealthProductsData Set HealthProdCoverPhoto='" + imgName + "' Where HealthProdId=" + maxId);
                }

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Health Product updated successfuly');", true);
            }

            txtName.Text = "";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('health-products-master.aspx', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSubmit_Click", ex.Message.ToString());
            return;
        }
    }

    private void GetHealthProduct(int hpIdX)
    {
        try
        {
            using (DataTable dtTest = c.GetDataTable("Select * From HealthProductsData Where HealthProdId=" + hpIdX))
            {
                if (dtTest.Rows.Count > 0)
                {
                    DataRow row = dtTest.Rows[0];

                    lblId.Text = hpIdX.ToString();

                    txtName.Text = row["HealthProdName"].ToString();


                    if (row["HealthProdCoverPhoto"] != DBNull.Value && row["HealthProdCoverPhoto"] != null && row["HealthProdCoverPhoto"].ToString() != "" && row["HealthProdCoverPhoto"].ToString() != "no-photo.png")
                    {
                        hpImg = "<img src=\"" + Master.rootPath + "upload/healthProducts/" + row["HealthProdCoverPhoto"].ToString() + "\" width=\"200\" />";
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Wrong Record Selected');", true);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetHealthProduct", ex.Message.ToString());
            return;
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            //c.ExecuteQuery("Delete From DiseaseData Where DiseaseId=" + Convert.ToInt32(lblId.Text));
            c.ExecuteQuery("Update HealthProductsData Set delMark=1 Where HealthProdId=" + Convert.ToInt32(lblId.Text));
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Health Product info deleted');", true);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('health-products-master.aspx', 2000);", true);
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
        Response.Redirect("health-products-master.aspx");
    }

    private void ImageUploadProcess(string imgName)
    {
        try
        {
            string origImgPath = "~/upload/healthProducts/original";
            string normalImgPath = "~/upload/healthProducts/";
            string thumbRawImgPath = "~/upload/healthProducts/thumbRaw/";


            fuImg.SaveAs(Server.MapPath(origImgPath) + imgName);

            c.ImageOptimizer(imgName, origImgPath, thumbRawImgPath, 700, true);
            c.CenterCropImage(imgName, thumbRawImgPath, normalImgPath, 600, 600);

            //Delete rew image from server
            File.Delete(Server.MapPath(origImgPath) + imgName);
            File.Delete(Server.MapPath(thumbRawImgPath) + imgName);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "ImageUploadProcess", ex.Message.ToString());
            return;
        }
    }

    protected void ddrMainCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Fill Dropdown list of Sub category selection
            if (ddrMainCategory.SelectedIndex > 0)
            {
                c.FillComboBox("ProductCatName", "ProductCatID", "ProductCategory", "delMark=0 AND ParentCatID=" + ddrMainCategory.SelectedValue, "ProductCatName", 0, ddrSubCategory);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "ddrMainCategory_SelectedIndexChanged", ex.Message.ToString());
            return;
        }
    }
    protected void ddrSubCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddrSubCategory.SelectedIndex > 0)
            {
                FillProductGrid();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "ddrSubCategory_SelectedIndexChanged", ex.Message.ToString());
            return;
        }
    }

    private void FillProductGrid()
    {
        try
        {
            using (DataTable dtProd = c.GetDataTable("Select a.ProductID, a.ProductName, a.ProductSKU, a.PriceMRP, a.PriceSale, a.ProductStock, " +
                " b.ProductCatName, c.UnitName, d.MfgName From ProductsData a Inner Join ProductCategory b On a.FK_SubCategoryID=b.ProductCatID " +
                " Inner Join UnitProducts c On a.FK_UnitID=c.UnitID Inner Join Manufacturers d On a.FK_MfgID=d.MfgId Where a.delMark=0 " +
                " AND a.ProductActive=1 AND a.FK_CategoryID=" + ddrMainCategory.SelectedValue +
                " AND a.FK_SubCategoryID=" + ddrSubCategory.SelectedValue + " Order By a.ProductID DESC"))
            {
                gvProducts.DataSource = dtProd;
                gvProducts.DataBind();


                foreach (GridViewRow gRow in gvProducts.Rows)
                {
                    CheckBox chk = (CheckBox)gRow.FindControl("chkSelect");
                    if (c.IsRecordExist("Select HealthProID From HealthDataProducts Where FK_HealthProdId=" + Request.QueryString["id"] + " And FK_ProductID=" + gRow.Cells[0].Text))
                        chk.Checked = true;
                    else
                        chk.Checked = false;
                }


                if (dtProd.Rows.Count > 0)
                {
                    gvProducts.UseAccessibleHeader = true;
                    gvProducts.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FillProductGrid", ex.Message.ToString());
            return;
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            int pCount = 0;

            string prIdStr = "";

            foreach (GridViewRow gRow in gvProducts.Rows)
            {
                CheckBox chk = (CheckBox)gRow.FindControl("chkSelect");
                if (chk.Checked)
                {
                    pCount++;
                    prIdStr = prIdStr == "" ? gRow.Cells[0].Text : prIdStr + ", " + gRow.Cells[0].Text;
                }
            }

            if (pCount <= 0)
            {
                errMsg = c.ErrNotification(2, "Select Products to assign");
                return;
            }

            string[] arrProd = prIdStr.Split(',');
            // added following query (reason - avoid duplicate record insertion)
            //c.ExecuteQuery("Delete From DiseaseProducts Where FK_DiseaseID=" + Request.QueryString["id"]);
            foreach (string prId in arrProd)
            {
                int maxId = c.NextId("HealthDataProducts", "HealthProID");

                if (!c.IsRecordExist("Select HealthProID From HealthDataProducts Where FK_ProductID=" + prId + " AND FK_HealthProdId=" + Request.QueryString["id"]))
                {
                    c.ExecuteQuery("Insert Into HealthDataProducts (HealthProID, FK_ProductID, FK_HealthProdId) Values (" +
                        maxId + ", " + prId + ", " + Request.QueryString["id"] + ")");
                }
            }

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Products added to Health Products');", true);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('health-products-master.aspx?action=edit&id=" + Request.QueryString["id"] + "', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnAdd_Click", ex.Message.ToString());
            return;
        }
    }

    protected void gvProducts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnDel = (Button)e.Row.FindControl("cmdDelete");
                CheckBox chk = (CheckBox)e.Row.FindControl("chkSelect");
                if (c.IsRecordExist("Select HealthProID From HealthDataProducts Where FK_ProductID=" + e.Row.Cells[0].Text + " AND FK_HealthProdId=" + Request.QueryString["id"]))
                {
                    btnDel.Visible = true;
                    chk.Visible = false;
                }
                else
                {
                    btnDel.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvProducts_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridViewRow gRow = (GridViewRow)((Button)e.CommandSource).NamingContainer;

            if (e.CommandName == "gvDel")
            {
                c.ExecuteQuery("Delete From HealthDataProducts Where FK_ProductID=" + gRow.Cells[0].Text + " AND FK_HealthProdId=" + Request.QueryString["id"]);
            }

            FillProductGrid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvProducts_RowCommand", ex.Message.ToString());
            return;
        }
    }
}