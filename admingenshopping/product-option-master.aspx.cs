using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class admingenshopping_product_option_master : System.Web.UI.Page
{
    public string pgTitle, errMsg, videoPreview;
    iClass c = new iClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        pgTitle = Request.QueryString["action"] == "new" ? "Add Category Info" : "Edit Category Info";
        FillGrid();
        if (!IsPostBack)
        {
            c.FillComboBox("ProductName", "ProductID", "ProductsData", "delMark=0", "ProductName", 0, ddrProduct);


            if (Request.QueryString["action"] != null)
            {
                //ditProdOptions.Visible = true;
                //iewProdOptions .Visible = false;

                if (Request.QueryString["action"] == "new")
                {
                    btnSave.Text = "Save Info";
                    btnDelete.Visible = false;

                }
                else
                {
                    btnSave.Text = "Modify Info";
                    btnDelete.Visible = true;
                    GetProductOptionData(Convert.ToInt32(Request.QueryString["id"]));
                }
            }
            else
            {
                //viewProdOptions.Visible = true;
                //editProdOptions.Visible = false;
                //FillGrid();
            }

            ddrProduct.Focus();

        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddrProduct.SelectedIndex == 0 || ddrOptionGroup.SelectedIndex == 0 || ddrOptions.SelectedIndex == 0 || txtPriceIncreament.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * field mandatory');", true);
                return;
            }
            int maxId = lblId.Text == "[New]" ? c.NextId("ProductOptions", "ProdOptionID") : Convert.ToInt16(lblId.Text);

            Boolean prodDuplicate = lblId.Text == "[New]" ? c.IsRecordExist("Select ProdOptionID From ProductOptions Where FK_OptionID=" + ddrOptions.SelectedValue + " And FK_ProductID=" + ddrProduct.SelectedValue + " And  FK_OptionGroupID=" + ddrOptionGroup.SelectedValue + "And DelMark=0") :
                c.IsRecordExist("Select ProdOptionID From ProductOptions Where FK_OptionID=" + ddrOptions.SelectedValue + " And FK_ProductID=" + ddrProduct.SelectedValue + " And  FK_OptionGroupID=" + ddrOptionGroup.SelectedValue + " And ProdOptionID<>" + maxId + "And DelMark=0");

            if (prodDuplicate == true)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'This Product option is already exist');", true);
                return;
            }

            if (lblId.Text == "[New]")
            {
                if (c.IsRecordExist("Select ProdOptionID From ProductOptions Where FK_ProductID=" + ddrProduct.SelectedValue + " AND FK_OptionGroupID<>" + ddrOptionGroup.SelectedValue + " And Delmark=0"))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'You can select only  one variant');", true);
                    return;
                }
            }
            else
            {
                if (c.IsRecordExist("Select ProdOptionID From ProductOptions Where FK_ProductID=" + ddrProduct.SelectedValue + " AND FK_OptionGroupID<>" + ddrOptionGroup.SelectedValue + " And ProdOptionID<>'" + lblId.Text + "' AND Delmark=0"))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'You can select only  one variant');", true);
                    return;
                }
            }

            int isActive = chkIsActive.Checked == true ? 1 : 0;
            if (lblId.Text == "[New]")
            {
                c.ExecuteQuery("Insert Into ProductOptions (ProdOptionID, FK_OptionID, FK_ProductID, FK_OptionGroupID, PriceIncrement, IsActive, DelMark) " +
                    "Values(" + maxId + ", " + ddrOptions.SelectedValue + ", " + ddrProduct.SelectedValue + "," + ddrOptionGroup.SelectedValue + ", " + txtPriceIncreament.Text + ", " + isActive + ", 0)");
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Product option info added');", true);
            }
            else
            {
                c.ExecuteQuery("Update ProductOptions Set FK_OptionID=" + ddrOptions.SelectedValue + ", FK_ProductID=" + ddrProduct.SelectedValue + ", FK_OptionGroupID=" + ddrOptionGroup.SelectedValue + ", PriceIncrement=" + txtPriceIncreament.Text + ", IsActive=" + isActive + " Where ProdOptionID=" + maxId);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Product option info updated');", true);

            }

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('product-option-master.aspx?action=new', 2000);", true);
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
            c.ExecuteQuery("Update ProductOptions Set DelMark=1 Where ProdOptionID=" + Request.QueryString["id"]);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Product option deleted');", true);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('product-option-master.aspx?action=new', 2000);", true);
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
        Response.Redirect("product-option-master.aspx?action=new");
    }
    private void FillGrid()
    {
        try
        {
            string strQuery = "";
            if (Convert.ToInt32(ddrProduct.SelectedIndex) > 0)
            {
                if (Convert.ToInt32(ddrOptionGroup.SelectedValue) > 0)
                {
                    int optGrpId = Convert.ToInt32(ddrOptionGroup.SelectedValue);
                    strQuery = "Select a.ProdOptionID, a.PriceIncrement, b.ProductName, c.OptionGroupName, d.OptionName " +
                    "From ProductOptions a Inner Join ProductsData b On a.FK_ProductID = b.ProductID " +
                    "Inner Join OptionGroups c On a.FK_OptionGroupID = c.OptionGroupID " +
                    "Inner Join OptionsData d On a.FK_OptionID = d.OptionID Where a.FK_ProductID=" + ddrProduct.SelectedValue + " AND a.FK_OptionGroupID=" + optGrpId + " And a.DelMark=0";
                }
                else
                {
                    strQuery = "Select a.ProdOptionID, a.PriceIncrement, b.ProductName, c.OptionGroupName, d.OptionName " +
                    "From ProductOptions a Inner Join ProductsData b On a.FK_ProductID = b.ProductID " +
                    "Inner Join OptionGroups c On a.FK_OptionGroupID = c.OptionGroupID " +
                    "Inner Join OptionsData d On a.FK_OptionID = d.OptionID Where a.FK_ProductID=" + ddrProduct.SelectedValue + " AND a.DelMark=0";
                }
            }
            else
            {
                //strQuery = "Select a.ProdOptionID, a.PriceIncrement, b.ProductName, c.OptionGroupName, d.OptionName " +
                //"From ProductOptions a Inner Join ProductsData b On b.ProductID = a.FK_ProductID " +
                //"Inner Join OptionGroups c On c.OptionGroupID = a.FK_OptionGroupID " +
                //"Inner Join OptionsData d On d.OptionID = a.FK_OptionID";

                strQuery = "Select a.ProdOptionID, a.PriceIncrement, b.ProductName, c.OptionGroupName, d.OptionName From ProductOptions a " +
                    "Inner Join ProductsData b On a.FK_ProductID = b.ProductID " +
                    "Inner Join OptionGroups c On a.FK_OptionGroupID = c.OptionGroupID " +
                    "Inner Join OptionsData d On a.FK_OptionID = d.OptionID Where a.DelMark=0";
            }
            using (DataTable dtProduct = c.GetDataTable(strQuery))
            {
                gvProducts.DataSource = dtProduct;
                gvProducts.DataBind();
                if (gvProducts.Rows.Count > 0)
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
                litAnch.Text = "<a href=\"product-option-master.aspx?action=edit&id=" + e.Row.Cells[0].Text + "\" class=\"gAnch\" title=\"View/Edit\"></a>";
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvProducts_RowDataBound", ex.Message.ToString());
            return;
        }
    }
    private void GetProductOptionData(int prodIdx)
    {
        try
        {
            using (DataTable dtProduct = c.GetDataTable("Select * From ProductOptions Where ProdOptionID=" + prodIdx))
            {
                if (dtProduct.Rows.Count > 0)
                {
                    DataRow row = dtProduct.Rows[0];
                    lblId.Text = prodIdx.ToString();
                    ddrProduct.SelectedValue = row["FK_ProductID"].ToString();
                    c.FillComboBox("OptionGroupName", "OptionGroupID", "OptionGroups", "", "OptionGroupName", 0, ddrOptionGroup);
                    ddrOptionGroup.SelectedValue = row["FK_OptionGroupID"].ToString();
                    c.FillComboBox("OptionName", "OptionID", "OptionsData", "FK_OptionGroupID=" + ddrOptionGroup.SelectedValue, "OptionName", 0, ddrOptions);
                    ddrOptions.SelectedValue = row["FK_OptionID"].ToString();
                    txtPriceIncreament.Text = row["PriceIncrement"].ToString();

                    if (row["IsActive"] != DBNull.Value && row["IsActive"] != null && row["IsActive"].ToString() != "")
                    {
                        if (row["IsActive"].ToString() == "1")
                        {
                            chkIsActive.Checked = true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetProductOptionData", ex.Message.ToString());
            return;
        }
    }
    protected void ddrProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            c.FillComboBox("OptionGroupName", "OptionGroupID", "OptionGroups", "", "OptionGroupName", 0, ddrOptionGroup);
            FillGrid();
        }
        catch (Exception ex)
        {

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "ddrProduct_SelectedIndexChanged", ex.Message.ToString());
            return;
        }
    }
    protected void ddrOptionGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddrOptionGroup.SelectedIndex > 0)
            {
                c.FillComboBox("OptionName", "OptionID", "OptionsData", "FK_OptionGroupID=" + ddrOptionGroup.SelectedValue, "OptionName", 0, ddrOptions);
                int optgrpId = Convert.ToInt32(ddrOptionGroup.SelectedValue);
                FillGrid();
            }

        }
        catch (Exception ex)
        {

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "ddrOptionGroup_SelectedIndexChanged", ex.Message.ToString());
            return;
        }
    }
}