using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class admingenshopping_cust_fav_shop_report : System.Web.UI.Page
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
            //Literal litTotalOrd = (Literal)e.Row.FindControl("litTotalOrd");
            //litTotalOrd.Text = c.returnAggregate("Select Count(OrdAssignID) From OrdersAssign Where Fk_FranchID=" + e.Row.Cells[0].Text + " AND OrdReAssign=0 AND OrdAssignStatus IN (1, 5, 6, 7)").ToString();


            using (DataTable dtFavShop = c.GetDataTable("Select a.CustomrtID, a.CustomerName, a.CustomerMobile, a.CustomerFavShop, " +
                " b.FranchName, b.FranchShopCode, " +
                " (Select Count(OrdAssignID) From OrdersAssign Where Fk_FranchID=a.CustomerFavShop AND OrdReAssign=0 AND OrdAssignStatus IN (1, 5, 6, 7)) as ordCount, " +
                " (Select Count(OrdAssignID) From OrdersAssign Where Fk_FranchID=a.CustomerFavShop AND FK_OrderID IN (Select OrderID From OrdersData Where FK_OrderCustomerID=a.CustomrtID AND OrderStatus IN (1, 5, 6, 7)) AND OrdReAssign=0 AND OrdAssignStatus IN (1, 5, 6, 7)) as ordCountCust " +
                " From CustomersData a Inner Join FranchiseeData b On a.CustomerFavShop=b.FranchID " +
                " Where a.delMark=0 Order By CustomrtID DESC"))
            {
                gvFavShop.DataSource = dtFavShop;
                gvFavShop.DataBind();

                if (gvFavShop.Rows.Count > 0)
                {
                    gvFavShop.UseAccessibleHeader = true;
                    gvFavShop.HeaderRow.TableSection = TableRowSection.TableHeader;
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
}