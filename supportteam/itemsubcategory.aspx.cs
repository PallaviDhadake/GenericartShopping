using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class supportteam_itemsubcategory : System.Web.UI.Page
{
    public string pgTitle, errMsg;
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {


        //pgTitle = Request.QueryString["action"] == "new" ? "Add Sub Category Info" : "Edit Sub Category Info";
        if (Request.QueryString["action"] != null)
        {
            if(Request.QueryString["action"] == "new")
            {
                pgTitle = "Add Sub Category Info";
            }
            else
            {
                pgTitle = "Edit Sub Category Info";
            }
        }
        else
        {
            pgTitle = "Add Sub Category Info";
        }
        btnSave.Attributes.Add("onclick", "this.disabled=true; this.value='Processing...';" + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        btnDelete.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnDelete, null) + ";");
        btnCancel.Attributes.Add("onclick", "this.disabled=true; this.value='Processing...';" + ClientScript.GetPostBackEventReference(btnCancel, null) + ";");

        if (!IsPostBack)
        {
            //Fill Dropdown list of Parent category selection
            c.FillComboBox("ProductCatName", "ProductCatID", "ProductCategory", "delMark=0 AND ParentCatID=0", "ProductCatName", 0, ddrMainCat);

            if (Request.QueryString["action"] != null)
            {

                if (Request.QueryString["action"] == "new")
                {
                    btnSave.Text = "Save Info";
                    btnDelete.Visible = false;

                }
                else
                {
                    btnSave.Text = "Modify Info";
                    btnDelete.Visible = true;
                    GetCategoryData(Convert.ToInt32(Request.QueryString["id"]));
                }
            }
            else
            {
                btnDelete.Visible = false;
                btnCancel.Visible = false;

                //viewprof.Visible = true;
                //editProf.Visible = false;
                //FillGrid();
            }
          

            FillGrid();
            ddrMainCat.Focus();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //Single quote filter
        txtCategory.Text = txtCategory.Text.Trim().Replace("'", "");

        //Empty fields validation
        if (txtCategory.Text == "" || ddrMainCat.SelectedIndex == 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * field are required');", true);
            
            txtCategory.Focus();
            return;
        }

        // Insert / Update data into database 
        int maxId = lblId.Text == "[New]" ? c.NextId("ProductCategory", "ProductCatID") : Convert.ToInt16(lblId.Text);

        if (lblId.Text == "[New]")
        {
            c.ExecuteQuery("Insert Into ProductCategory(ProductCatID, ProductCatName, ParentCatID, ChildCatFlag, delMark) Values(" + maxId + ", '" + txtCategory.Text + "'," + ddrMainCat.SelectedValue + " , 0, 0)");
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Sub Category info added');", true);
            
        }
        else
        {
            c.ExecuteQuery("Update ProductCategory set ProductCatName='" + txtCategory.Text + "', ParentCatID=" + ddrMainCat.SelectedValue + " Where ProductCatID=" + maxId);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Sub Category info Updated');", true);
            
        }
        // Show fresh updated Gridview data
        FillGrid();
        //Clear text fields
        ResetControl();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (c.IsRecordExist("Select ProductCatID From ProductCategory Where ParentCatID=" + Convert.ToInt32(Request.QueryString["id"])))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'You Can't Delete this record, its reference exists in Child Category');", true);
                
                return;
            }
            else if (c.IsRecordExist("Select ProductID from ProductsData Where FK_SubCategoryID=" + Convert.ToInt32(Request.QueryString["id"])))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'You Can't Delete this record, its reference exists in ProductData');", true);
                
                return;
            }
            else
            {
                c.ExecuteQuery("Delete From ProductCategory Where ProductCatID=" + Convert.ToInt32(Request.QueryString["id"]));
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Category deleted successfully');", true);
               
                FillGrid();
                ResetControl();
            }
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
        if (Request.QueryString["action"] == "new")
        {
            Response.Redirect("dashboard.aspx");
        }
        else
        {
            Response.Redirect("itemsubcategory.aspx?action=new");
        }
    }

    //Show Data into Gridview starts here
    private void FillGrid()
    {
        try
        {
            using (DataTable dtCategory = c.GetDataTable("Select a.ProductCatID, a.ProductCatName, a.ParentCatID, (Select ProductCatName From ProductCategory Where delMark=0 AND ProductCatID=a.ParentCatID) as ParentCatName From ProductCategory a Where a.delMark=0 AND a.ParentCatID<>0 AND a.ChildCatFlag=0 Order By a.ProductCatName"))
            {
                gvCategory.DataSource = dtCategory;
                gvCategory.DataBind();
                if (gvCategory.Rows.Count > 0)
                {
                    gvCategory.UseAccessibleHeader = true;
                    gvCategory.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvCategory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"itemsubcategory.aspx?action=edit&id=" + e.Row.Cells[0].Text + "\" class=\"gAnch\" title=\"View/Edit\"></a>";

                Literal litSubProd = (Literal)e.Row.FindControl("litSubProd");
                litSubProd.Text = "<a href=\"related-products.aspx?subCatId=" + e.Row.Cells[0].Text + "\" class=\"fas fa-1x fa-cart-plus\" target=\"_blank\" title=\"Add Related Products\"></a>";
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvCategory_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    protected void gvCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCategory.PageIndex = e.NewPageIndex;
        FillGrid();
    }

    private void GetCategoryData(int Idx)
    {
        try
        {
            using (DataTable dtCategory = c.GetDataTable("Select * From ProductCategory Where ProductCatID=" + Idx))
            {
                if (dtCategory.Rows.Count > 0)
                {
                    DataRow bRow = dtCategory.Rows[0];
                    lblId.Text = Idx.ToString();
                    txtCategory.Text = bRow["ProductCatName"].ToString();
                    ddrMainCat.SelectedValue = bRow["ParentCatID"].ToString();
                }
                txtCategory.Focus();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvCategory_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    private void ResetControl()
    {
        ddrMainCat.Focus();
        ddrMainCat.SelectedIndex = 0;
        txtCategory.Text = "";
        lblId.Text = "[New]";
    }
}