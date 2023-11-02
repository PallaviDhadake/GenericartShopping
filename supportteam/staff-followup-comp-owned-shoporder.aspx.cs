using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class supportteam_staff_followup_comp_owned_shoporder : System.Web.UI.Page
{
    iClass c = new iClass();
    public string pageLink, pageName;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            viewCompOwnShop.Visible = true;
            FillGrid();
        }
    }
    private void FillGrid()
    {
        try
        {
            using (DataTable dtCompOwnShops = c.GetDataTable("Select a.CSID, a.FK_FranchID, a.FranchShopCode, (Select Count(FK_OrderID) From OrdersAssign Where Fk_FranchID=a.FK_FranchID And OrdReAssign=0) As totalOrd From CompanyOwnShops a Where a.FK_FranchID In(24, 2010, 2062, 2070)"))
            {
                if (dtCompOwnShops.Rows.Count > 0)
                {
                    gvCompOwnShop.DataSource = dtCompOwnShops;
                    gvCompOwnShop.DataBind();
                    if (gvCompOwnShop.Rows.Count > 0)
                    {
                        gvCompOwnShop.UseAccessibleHeader = true;
                        gvCompOwnShop.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }
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
    protected void gvCompOwnShop_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litOrders = (Literal)e.Row.FindControl("litOrders");
                string frId = e.Row.Cells[1].Text;
                string totalOrder = c.GetReqData("OrdersAssign", "Count(FK_OrderID)", "Fk_FranchID=" + frId + " And OrdReAssign=0").ToString();

                litOrders.Text = "<a href=\"staff-followup-compowned-shopwise-orders.aspx?franchId=" + frId + "\" class=\"text-dark\" title=\"Total Orders\" target=\"_blank\"> " + totalOrder + " </a>";


            }
               
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvCompOwnShop_RowDataBound", ex.Message.ToString());
            return;
        }
    }
}