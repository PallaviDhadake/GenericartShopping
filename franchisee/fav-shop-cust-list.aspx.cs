using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class franchisee_fav_shop_cust_list : System.Web.UI.Page
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

            using (DataTable dtFavShop = c.GetDataTable("Select a.CustomrtID, a.CustomerName, a.CustomerMobile, Convert(varchar(20), a.CustomerJoinDate, 103) as regDate, " +
                " (Select Count(OrdAssignID) From OrdersAssign Where Fk_FranchID=" + Session["adminFranchisee"] + " AND FK_OrderID IN (Select OrderID From OrdersData Where FK_OrderCustomerID=a.CustomrtID AND OrderStatus IN (1, 5, 6, 7)) AND OrdReAssign=0 AND OrdAssignStatus IN (1, 5, 6, 7)) as ordCountCust " +
                " From CustomersData a " +
                " Where a.delMark=0 AND a.CustomerFavShop=" + Session["adminFranchisee"] + " Order By CustomrtID DESC"))
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