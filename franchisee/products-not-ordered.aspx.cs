using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class franchisee_products_not_ordered : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillGrid();
        }
    }

    private void FillGrid()
    {
        try
        {
            // " (CONVERT(varchar(20), a.OrderDate, 112) >= CONVERT(varchar(20), CAST('" + ordFromDate + "' as datetime), 112)) AND " +
            //        " (CONVERT(varchar(20), a.OrderDate, 112) <= CONVERT(varchar(20), CAST('" + ordToDate + "' as datetime), 112)) " +
            using (DataTable dtProd = c.GetDataTable("Select a.ProductID, a.ProductName, a.ProductSKU, a.PriceMRP, a.PriceSale, a.ProductStock, " +
                " b.ProductCatName, c.UnitName, d.MfgName From ProductsData a Inner Join ProductCategory b On a.FK_SubCategoryID=b.ProductCatID " +
                " Inner Join UnitProducts c On a.FK_UnitID=c.UnitID Inner Join Manufacturers d On a.FK_MfgID=d.MfgId Where a.delMark=0 " +
                " AND a.ProductActive=1 AND a.ProductID NOT IN (Select Distinct FK_DetailProductID From OrdersDetails) Order By a.ProductID DESC"))
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
                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                int prodActive = Convert.ToInt32(c.GetReqData("ProductsData", "ProductActive", "ProductID=" + e.Row.Cells[0].Text));
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

    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtFromDate.Text == "" && txtToDate.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select Date Range To Export Report');", true);
                return;
            }

            // From Date
            DateTime ordFromDate = DateTime.Now;
            string[] arrfrDate = txtFromDate.Text.Split('/');
            if (c.IsDate(arrfrDate[1] + "/" + arrfrDate[0] + "/" + arrfrDate[2]) == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid From Date');", true);
                return;
            }
            else
            {
                ordFromDate = Convert.ToDateTime(arrfrDate[1] + "/" + arrfrDate[0] + "/" + arrfrDate[2]);
            }

            // To Date
            DateTime ordToDate = DateTime.Now;
            string[] arrtoDate = txtToDate.Text.Split('/');
            if (c.IsDate(arrtoDate[1] + "/" + arrtoDate[0] + "/" + arrtoDate[2]) == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid To Date');", true);
                return;
            }
            else
            {
                ordToDate = Convert.ToDateTime(arrtoDate[1] + "/" + arrtoDate[0] + "/" + arrtoDate[2]);
            }

            FillGrid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnShow_Click", ex.Message.ToString());
            return;
        }
    }
}