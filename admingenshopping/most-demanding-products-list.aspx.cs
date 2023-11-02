using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class admingenshopping_most_demanding_products_list : System.Web.UI.Page
{
    iClass c = new iClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["act"] != null)
            {
                if (Request.QueryString["act"] == "button")
                {
                    btnSetOrder.Visible = true;
                }
            }
            else
            {
                btnSetOrder.Visible = false;
            }
            FillGrid();
        }
    }

    private void FillGrid()
    {
        try
        {
            using (DataTable dtProd = c.GetDataTable("Select a.ProductID, a.ProductName, a.ProductSKU, a.PriceMRP, a.PriceSale, a.ProductDisplayOrder, " +
                " isnull(a.ProductPhoto, '-') as ProductPhoto, b.ProductCatName, d.MfgName From ProductsData a Inner Join ProductCategory b On a.FK_SubCategoryID=b.ProductCatID " +
                " Inner Join Manufacturers d On a.FK_MfgID=d.MfgId Where a.delMark=0 AND a.BestSellerFlag=1 Order By a.ProductID DESC"))
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
                Button btnUp = (Button)e.Row.FindControl("moveUp");
                if (e.Row.Cells[7].Text == "1")
                {
                    btnUp.Enabled = false;
                    btnUp.Attributes["style"] = "background:none;";
                }

                Button btnDown = (Button)e.Row.FindControl("moveDown");
                int maxOrd = Convert.ToInt32(c.returnAggregate("Select MAX(ProductDisplayOrder) From ProductsData Where delMark=0 AND BestSellerFlag=1"));
                if (Convert.ToInt32(e.Row.Cells[7].Text) == maxOrd)
                {
                    btnDown.Visible = false;
                }

                Literal litProduct = (Literal)e.Row.FindControl("litProduct");
                if (e.Row.Cells[1].Text != "-")
                {
                    litProduct.Text = "<img src=\"" + Master.rootPath + "upload/products/thumb/" + e.Row.Cells[1].Text + "\" width=\"150\" />";
                }
                else
                {
                    litProduct.Text = "NA";
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
            if (e.CommandName == "Up")
            {
                int displayOrd = Convert.ToInt32(gRow.Cells[7].Text);
                int previouRow = Convert.ToInt32(gRow.Cells[0].Text);
                c.ExecuteQuery("Update ProductsData Set ProductDisplayOrder=" + displayOrd + " Where ProductDisplayOrder=" + (displayOrd - 1));
                c.ExecuteQuery("Update ProductsData Set ProductDisplayOrder=" + (displayOrd - 1) + " Where ProductID=" + previouRow); 
                
            }
            if (e.CommandName == "Down")
            {
                int displayOrd = Convert.ToInt32(gRow.Cells[7].Text);
                int previouRow = Convert.ToInt32(gRow.Cells[0].Text);
                c.ExecuteQuery("Update ProductsData Set ProductDisplayOrder=" + displayOrd + " Where ProductDisplayOrder=" + (displayOrd + 1));
                c.ExecuteQuery("Update ProductsData Set ProductDisplayOrder=" + (displayOrd + 1) + " Where ProductID=" + previouRow);
            }

            FillGrid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvProducts_RowCommand", ex.Message.ToString());
            return;
        }
    }


    protected void btnSetOrder_Click(object sender, EventArgs e)
    {
        try
        {
            using (DataTable dtProd = c.GetDataTable("Select a.ProductID, a.ProductName, a.ProductSKU, a.PriceMRP, a.PriceSale, a.ProductDisplayOrder, " +
                " b.ProductCatName, d.MfgName From ProductsData a Inner Join ProductCategory b On a.FK_SubCategoryID=b.ProductCatID " +
                " Inner Join Manufacturers d On a.FK_MfgID=d.MfgId Where a.delMark=0 AND a.BestSellerFlag=1 Order By a.ProductID DESC"))
            {
                if (dtProd.Rows.Count > 0)
                {
                    int rowCount = 1;
                    foreach (DataRow row in dtProd.Rows)
                    {
                        c.ExecuteQuery("Update ProductsData Set ProductDisplayOrder=" + rowCount + " Where ProductID=" + row["ProductID"]);
                        rowCount++;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSetOrder_Click", ex.Message.ToString());
            return;
        }
    }
}