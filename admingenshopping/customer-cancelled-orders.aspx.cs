using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class admingenshopping_customer_cancelled_orders : System.Web.UI.Page
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
            using(DataTable dtOrd = c.GetDataTable("Select a.FK_OrderCustomerID, b.CustomerName, b.CustomerMobile, b.CustomerEmail, a.OrderID, Convert(varchar(20), a.OrderDate, 103) as orDate, a.OrderAmount, " +
                " isnull(c.ReasonTitle, 'NA') as ReasonTitle, a.DeviceType From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID = b.CustomrtID " +
                " LEFT Join CancelReasons c On a.FK_ReasonID = c.ReasonID where a.OrderStatus = '2' Order By OrderID DESC"))
            {
                gvFavShop.DataSource = dtOrd;
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