using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using Razorpay.Api;
using System.Activities.Statements;
using System.Reflection;
using System.Security.Cryptography;

public partial class admingenshopping_product_master : System.Web.UI.Page
{
    iClass c = new iClass();
    public string pgTitle, prodPhoto, rdrUrl, prodMsg, photoUrl;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            pgTitle = Request.QueryString["action"] == "new" ? "Add Product Info" : "Edit Product Info";
            btnSave.Attributes.Add("onclick", "this.disabled=true; this.value='Processing...';" + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
            btnDelete.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnDelete, null) + ";");
            btnCancel.Attributes.Add("onclick", "this.disabled=true; this.value='Processing...';" + ClientScript.GetPostBackEventReference(btnCancel, null) + ";");

            if (!IsPostBack)
            {
                txtProdName.Focus();
                
                if (Request.QueryString["action"] != null)
                {
                    editProduct.Visible = true;
                    viewProduct.Visible = false;

                    btnRemove.Visible = false;

                    // Fill Manufacturer (Med Company)
                    c.FillComboBox("MfgName", "MfgId", "Manufacturers", "delmark=0", "MfgName", 0, ddrMfr);
                    // Fill Med Units
                    c.FillComboBox("UnitName", "UnitID", "UnitProducts", "delMark=0", "UnitName", 0, ddrUnit);
                    // Fill Product Main Category
                    c.FillComboBox("ProductCatName", "ProductCatID", "ProductCategory", "delMark=0 AND ParentCatID=0", "ProductCatName", 0, ddrMainCategory);


                    if (Request.QueryString["action"] == "new")
                    {
                        btnSave.Text = "Save Info";
                        btnDelete.Visible = false;

                    }
                    else
                    {
                        btnPhoto.Visible = true;
                        btnSave.Text = "Modify Info";
                        btnDelete.Visible = true;
                        GetProductData(Convert.ToInt32(Request.QueryString["id"]));
                    }
                }
                else
                {
                    viewProduct.Visible = true;
                    editProduct.Visible = false;
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
            //using(DataTable dtProd = c.GetDataTable("Select a.ProductID, a.ProductName, a.ProductSKU, a.PriceMRP, a.PriceSale, a.ProductStock, " + 
            //    " b.ProductCatName, c.UnitName, d.MfgName From ProductsData a Inner Join ProductCategory b On a.FK_SubCategoryID=b.ProductCatID " +
            //    " Inner Join UnitProducts c On a.FK_UnitID=c.UnitID Inner Join Manufacturers d On a.FK_MfgID=d.MfgId Where a.delMark=0 AND a.ProductActive=1 Order By a.ProductID DESC"))
            using (DataTable dtProd = c.GetDataTable(@"SELECT 
                                                            a.[ProductID],
                                                            a.[ProductName],
                                                            a.[ProductSKU],
                                                            a.[ProductEntryCode],
                                                            a.[PriceMRP],
                                                            a.[PriceSale],
                                                            a.[ProductStock],
                                                            b.[ProductCatName],
                                                            c.[UnitName],
                                                            d.[MfgName]
                                                        FROM [dbo].[ProductsData] a
                                                        INNER JOIN [dbo].[ProductCategory] b ON a.[FK_SubCategoryID] = b.[ProductCatID]
                                                        INNER JOIN [dbo].[UnitProducts] c ON a.[FK_UnitID] = c.[UnitID]
                                                        INNER JOIN [dbo].[Manufacturers] d ON a.[FK_MfgID] = d.[MfgId]
                                                        WHERE a.[delMark] = 0
                                                        ORDER BY a.[ProductID] DESC"))
            {
                gvProducts.DataSource = dtProd;
                gvProducts.DataBind();

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
            c.ErrorLogHandler(this.ToString(), "FillGrid", ex.Message.ToString());
            return;
        }
    }


    protected void gvProducts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"product-master.aspx?action=edit&id=" + e.Row.Cells[0].Text + "\" class=\"gAnch\" title=\"View/Edit\"></a>";

                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                int prodActive = Convert.ToInt32(c.GetReqData("ProductsData", "ProductActive", "ProductID = " + e.Row.Cells[0].Text));
                if (prodActive == 1)
                    litStatus.Text = "<span class=\"stGreen\">Active</span>";
                else
                    litStatus.Text = "<span class=\"stGrey\">Inactive</span>";
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvProducts_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    protected void gvProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvProducts.PageIndex = e.NewPageIndex;
        FillGrid();
    }

    public void GetAllControls(ControlCollection ctls)
    {
        foreach (Control c in ctls)
        {
            if (c is System.Web.UI.WebControls.TextBox)
            {
                TextBox tt = c as TextBox;
                //to do something by using textBox tt.
                tt.Text = tt.Text.Trim().Replace("'", "");
            }
            if (c.HasControls())
            {
                GetAllControls(c.Controls);

            }
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            //Single quote filter
            GetAllControls(this.Controls);

            //Empty fields validation
            if (txtProdSKU.Text == "" || txtProdName.Text == "" || txtPriceMrp.Text == "" || txtPriceSale.Text == "" || 
                txtStock.Text == "" || txtPackaging.Text == "" || txtShortDesc.Text == "" || txtLongDesc.Text=="" || 
                ddrMfr.SelectedIndex==0 || ddrUnit.SelectedIndex == 0 || ddrMainCategory.SelectedIndex==0 || txtProdEnCode.Text == "" ||
                ddrSubCategory.SelectedIndex == 0 || ddrPercent.SelectedIndex == 0 || txtTaxLess.Text == "") 
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * Fields are mandatory');", true);
                return;
            }

            // Insert / Update data into database 
            int maxId = lblId.Text == "[New]" ? c.NextId("ProductsData", "ProductID") : Convert.ToInt16(lblId.Text);

            // Check Product code duplication attempt
            Boolean proCodeDuplicate = lblId.Text == "[New]" ? c.IsRecordExist("Select ProductSKU From ProductsData Where ProductSKU='" + txtProdSKU.Text + "' AND delMark=0") : c.IsRecordExist("Select ProductSKU From ProductsData Where ProductSKU='" + txtProdSKU.Text + "' AND delMark=0 AND ProductID<>" + maxId);
            if (proCodeDuplicate == true)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'This Product SKU Code already exist for another product');", true);
                return;
            }

            //Check Product Entry Code duplication attempt
            Boolean proCodeEntry = lblId.Text == "[New]" ? c.IsRecordExist("Select ProductEntryCode From ProductsData Where ProductEntryCode = '" + txtProdEnCode.Text + "' AND delMark=0") : c.IsRecordExist("Select ProductEntryCode From ProductsData Where ProductEntryCode = '" + txtProdEnCode.Text + "' AND delMark = 0 AND ProductID <> " + maxId);
            if (proCodeEntry == true)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'This Product Entry Code already exist for another product');", true);
                return;
            }

            DateTime cDate = DateTime.Now;
            string currentDate = cDate.ToString("yyyy-MM-dd HH:mm:ss.fff");

            int prescriptionFlag = chkPrescription.Checked == true ? 1 : 0;
            int activeFlag = chkActive.Checked == true ? 1 : 0;
            int featuredFlag = chkFeatured.Checked == true ? 1 : 0;
            int bestSeller = chkBestSeller.Checked == true ? 1 : 0;

            int pillReminder = chkReminder.Checked == true ? 1 : 0;
            int notOnlineSale = chkNotOnline.Checked == true ? 1 : 0;

            string imgName = "";
            if (fuImg.HasFile)
            {
                string fExt = Path.GetExtension(fuImg.FileName).ToString().ToLower();
                if (fExt == ".jpg" || fExt == ".jpeg" || fExt == ".png" || fExt == ".webp")
                {
                    imgName = "product-photo-" + maxId + DateTime.Now.ToString("ddmmyyyyHHmmss") + fExt;
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
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Only .jpg, .jpeg, .webp or .png files are allowed');", true);
                    return;
                }
            }
            else
            {
                if (lblId.Text == "[New]")
                {
                    imgName = "no-photo.png";
                }
            }

            if (txtShortDesc.Text != "")
            {
                if (txtShortDesc.Text.Length > 100)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Short description must be less than 100 characters');", true);
                    return;
                }
            }

            if (txtMetaDesc.Text != "")
            {
                if (txtMetaDesc.Text.Length > 200)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Product Meta description must be less than 200 characters');", true);
                    return;
                }
            }

            double mrpPrice = Math.Round(Convert.ToDouble(txtPriceMrp.Text), 2);
            double salePrice = Math.Round(Convert.ToDouble(txtPriceSale.Text), 2);

            if (salePrice > mrpPrice)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'saling parice is grater than mrp price');", true);
                return;
            }

            string dispOrder = "";
            dispOrder = c.returnAggregate("Select MAX(ProductDisplayOrder) From ProductsData Where delMark=0").ToString();
            int displayOrder = 0;
            if (chkBestSeller.Checked == true)
            {
                if (dispOrder != "")
                {
                    displayOrder = Convert.ToInt32(dispOrder) + 1;
                }
                else
                {
                    displayOrder = 0;
                }
            }
            else
            {
                displayOrder = 0;
            }

            if (lblId.Text == "[New]")
            {
                c.ExecuteQuery("Insert Into ProductsData (ProductID, FK_MfgID, FK_UnitID, ProductSKU, ProductEntryCode, ProductName, PriceMRP, PriceSale, " +
                    " PackagingType, ProductLongDesc, ProductShortDesc, FK_CategoryID, FK_SubCategoryID, ProductStock, ProductActive, " +
                    " PrescriptionFlag, ProductViews, ProductMetaDesc, FeaturedFlag, BestSellerFlag, delmark, ProductPhoto, RemindFlag, " +
                    " ProductDisplayOrder, IsNotForOnlineSale, TaxPercent, TaxLessAmount) Values (" + maxId +
                    ", " + ddrMfr.SelectedValue + ", " + ddrUnit.SelectedValue + ", '" + txtProdSKU.Text + "', '" + txtProdEnCode.Text + "','" + txtProdName.Text +
                    "', " + mrpPrice + ", " + salePrice + ", '" + txtPackaging.Text +
                    "', '" + txtLongDesc.Text + "', '" + txtShortDesc.Text + "', " + ddrMainCategory.SelectedValue + ", " + ddrSubCategory.SelectedValue +
                    ", " + Convert.ToInt32(txtStock.Text) + ", " + activeFlag + ", " + prescriptionFlag + ", 0, '" + txtMetaDesc.Text +
                    "', " + featuredFlag + ", " + bestSeller + ", 0, '" + imgName + "', " + pillReminder + ", " + displayOrder +
                    ", " + notOnlineSale + ", " + ddrPercent.SelectedValue + ", " + txtTaxLess.Text + ")");

                //ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Product Info Added');", true);

                rdrUrl = "product-photos.aspx?prId=" + maxId;
                prodMsg = "Product Info Added Successfully..!!";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "$(document).ready(function () {$('#slide').popup('show');});", true);
            }
            else
            {
                c.ExecuteQuery("Update ProductsData Set FK_MfgID=" + ddrMfr.SelectedValue + ", FK_UnitID=" + ddrUnit.SelectedValue +
                    ", ProductSKU='" + txtProdSKU.Text + "', ProductEntryCode='" + txtProdEnCode.Text + "',ProductName='" + txtProdName.Text + "', PriceMRP=" + mrpPrice +
                    ", PriceSale=" + salePrice + ", PackagingType='" + txtPackaging.Text +
                    "', ProductLongDesc='" + txtLongDesc.Text + "', ProductShortDesc='" + txtShortDesc.Text +
                    "', FK_CategoryID=" + ddrMainCategory.SelectedValue + ", FK_SubCategoryID=" + ddrSubCategory.SelectedValue +
                    ", ProductStock=" + Convert.ToInt32(txtStock.Text) + ", ProductActive=" + activeFlag + ", PrescriptionFlag=" + prescriptionFlag +
                    ", ProductMetaDesc='" + txtMetaDesc.Text + "', FeaturedFlag=" + featuredFlag + ", BestSellerFlag=" + bestSeller +
                    ", RemindFlag=" + pillReminder + ", IsNotForOnlineSale=" + notOnlineSale +
                    ", TaxPercent=" + ddrPercent.SelectedValue + ", TaxLessAmount=" + txtTaxLess.Text + " Where ProductID=" + maxId);

                if (fuImg.HasFile)
                {
                    c.ExecuteQuery("Update ProductsData Set ProductPhoto='" + imgName + "' Where ProductID=" + maxId);
                }

                //ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Product Info Updated');", true);
                rdrUrl = "product-photos.aspx?prId=" + maxId;
                prodMsg = "Product Info Updated Successfully..!!";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "$(document).ready(function () {document.getElementById('slide').style.display = \"none\"; $('#slide').popup('show');});", true);
            }

            //ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('product-master.aspx', 2000);", true);
            
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSave_Click", ex.Message.ToString());
            return;
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            c.ExecuteQuery("Update ProductsData Set delMark=1 Where ProductID=" + Request.QueryString["id"]);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Product Deleted');", true);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('product-master.aspx', 2000);", true);
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
        Response.Redirect("product-master.aspx");
    }

    private void GetProductData(int prodIdx)
    {
        try
        {
            using (DataTable dtProd = c.GetDataTable("Select * From ProductsData Where ProductID=" + prodIdx))
            {
                if (dtProd.Rows.Count > 0)
                {
                    DataRow row = dtProd.Rows[0];
                    lblId.Text = prodIdx.ToString();
                    txtProdSKU.Text = row["ProductSKU"].ToString();
                    txtProdEnCode.Text = row["ProductEntryCode"].ToString();
                    txtProdName.Text = row["ProductName"].ToString();
                    txtPriceMrp.Text = row["PriceMRP"].ToString();
                    txtPriceSale.Text = row["PriceSale"].ToString();
                    txtShortDesc.Text = row["ProductShortDesc"].ToString();
                    txtLongDesc.Text = row["ProductLongDesc"].ToString();
                    txtPackaging.Text = row["PackagingType"].ToString();
                    ddrMainCategory.SelectedValue = row["FK_CategoryID"].ToString();
                    ddrMfr.SelectedValue = row["FK_MfgID"].ToString();
                    ddrUnit.SelectedValue = row["FK_UnitID"].ToString();

                    btnPhoto.Visible = true;

                    //Fill Dropdown list of Sub category selection
                    c.FillComboBox("ProductCatName", "ProductCatID", "ProductCategory", "delMark=0 AND ParentCatID=" + ddrMainCategory.SelectedValue, "ProductCatName", 0, ddrSubCategory);
                    ddrSubCategory.SelectedValue = row["FK_SubCategoryID"].ToString();


                    txtStock.Text = row["ProductStock"].ToString();

                    txtMetaDesc.Text = row["ProductMetaDesc"] != DBNull.Value && row["ProductMetaDesc"] != null && row["ProductMetaDesc"].ToString() != "" ? row["ProductMetaDesc"].ToString() : "";

                    if (row["FeaturedFlag"] != DBNull.Value && row["FeaturedFlag"] != null && row["FeaturedFlag"].ToString() != "")
                    {
                        if (row["FeaturedFlag"].ToString() == "1")
                        {
                            chkFeatured.Checked = true;
                        }
                    }

                    if (row["BestSellerFlag"] != DBNull.Value && row["BestSellerFlag"] != null && row["BestSellerFlag"].ToString() != "")
                    {
                        if (row["BestSellerFlag"].ToString() == "1")
                        {
                            chkBestSeller.Checked = true;
                        }
                    }

                    if (row["ProductActive"] != DBNull.Value && row["ProductActive"] != null && row["ProductActive"].ToString() != "")
                    {
                        if (row["ProductActive"].ToString() == "1")
                        {
                            chkActive.Checked = true;
                        }
                    }

                    if (row["PrescriptionFlag"] != DBNull.Value && row["PrescriptionFlag"] != null && row["PrescriptionFlag"].ToString() != "")
                    {
                        if (row["PrescriptionFlag"].ToString() == "1")
                        {
                            chkPrescription.Checked = true;
                        }
                    }

                    if (row["RemindFlag"] != DBNull.Value && row["RemindFlag"] != null && row["RemindFlag"].ToString() != "")
                    {
                        if (row["RemindFlag"].ToString() == "1")
                        {
                            chkReminder.Checked = true;
                        }
                    }

                    if (row["IsNotForOnlineSale"] != DBNull.Value && row["IsNotForOnlineSale"] != null && row["IsNotForOnlineSale"].ToString() != "")
                    {
                        if (row["IsNotForOnlineSale"].ToString() == "1")
                        {
                            chkNotOnline.Checked = true;
                        }
                    }

                    if (row["ProductPhoto"] != DBNull.Value && row["ProductPhoto"] != null && row["ProductPhoto"].ToString() != "")
                    {
                        //if (row["ProductPhoto"].ToString().Contains(".webp"))
                        //{
                        //    //prodPhoto = "<picture id=\"picture\">  <source srcset=\"https://www.gstatic.com/webp/gallery/1.sm.webp\" type=\"image/webp\">  <source srcset=\"https://www.gstatic.com/webp/gallery/1.sm.jpg\" type=\"image/jpeg\">   <img loading=\"lazy\" src=\"https://www.gstatic.com/webp/gallery/1.sm.jpg\" alt=\"Alt Text!\"></picture>";
                        //    //prodPhoto = "<picture id=\"picture\">  <source srcset=\"" + row["ProductPhoto"].ToString() + "\" type=\"image/webp\">  <source srcset=\"https://www.gstatic.com/webp/gallery/1.sm.jpg\" type=\"image/jpeg\">   <img loading=\"lazy\" src=\"" + row["ProductPhoto"].ToString() + "\" alt=\"Alt Text!\"></picture>";
                        //}
                        //else
                        //{
                        //    prodPhoto = "<img src=\"" + Master.rootPath + "upload/products/thumb/" + row["ProductPhoto"].ToString() + "\" width=\"200\" />";
                        //}
                        prodPhoto = "<img src=\"" + Master.rootPath + "upload/products/thumb/" + row["ProductPhoto"].ToString() + "\" width=\"200\" />";
                        if (row["ProductPhoto"].ToString() == "no-photo.png")
                        {
                            btnRemove.Visible = false;
                        }
                        else
                        {
                            btnRemove.Visible = true;
                        }
                    }

                    ddrPercent.SelectedValue = row["TaxPercent"] != DBNull.Value && row["TaxPercent"] != null && row["TaxPercent"].ToString() != "" ? row["TaxPercent"].ToString() : "-1";
                    txtTaxLess.Text = row["TaxLessAmount"] != DBNull.Value && row["TaxLessAmount"] != null && row["TaxLessAmount"].ToString() != "" ? row["TaxLessAmount"].ToString() : "";
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetProductData", ex.Message.ToString());
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
            string thumbRawImgPath = "~/upload/products/thumbRaw/";

            fuImg.SaveAs(Server.MapPath(origImgPath) + imgName);

            //c.ImageOptimizer(imgName, origImgPath, normalImgPath, 800, true);
            //c.ImageOptimizer(imgName, normalImgPath, thumbImgPath, 400, true);

            c.ImageOptimizer(imgName, origImgPath, normalImgPath, 800, true);
            c.ImageOptimizer(imgName, normalImgPath, thumbRawImgPath, 600, true);
            c.CenterCropImage(imgName, thumbRawImgPath, thumbImgPath, 500, 500);

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
    protected void btnRemove_Click(object sender, EventArgs e)
    {
        try
        {
            c.ExecuteQuery("Update ProductsData Set ProductPhoto='no-photo.png' Where ProductID=" + Request.QueryString["id"]);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Product Photo Removed');", true);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('product-master.aspx', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnRemove_Click", ex.Message.ToString());
            return;
        }
    }
    protected void btnPhoto_Click(object sender, EventArgs e)
    {
        Response.Redirect("product-photos.aspx?prId=" + Request.QueryString["id"], false);
    }

    protected void ddrPercent_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddrPercent.SelectedIndex > 0)
            {
                double sellPrice = Convert.ToDouble(txtPriceSale.Text);

                double tempPercent = Convert.ToDouble(ddrPercent.SelectedValue) / 100.00;
                double tempVar = tempPercent + 1;

                double taxlessPrice = Convert.ToDouble(sellPrice.ToString("0.00")) / tempVar;

                txtTaxLess.Text = taxlessPrice.ToString("0.00");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "ddrPercent_SelectedIndexChanged", ex.Message.ToString());
            return;
        }
    }
}