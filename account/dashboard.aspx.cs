using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class account_dashboard : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] arrCounts = new string[10]; //4
    protected void Page_Load(object sender, EventArgs e)
    {
        GetCount();
    }

    protected void GetCount()
    {
        try
        {
            string dateRange = c.GetFinancialYear();
            string[] arrDateRange = dateRange.ToString().Split('#');
            DateTime myFromDate = Convert.ToDateTime(arrDateRange[0]);
            DateTime myToDate = Convert.ToDateTime(arrDateRange[1]);

            //arrCounts[0] = c.returnAggregate("Select Count(a.OrderID) From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID Where a.OrderStatus<>0 AND a.OrderType IS NOT NULL AND a.OrderType<>0").ToString();
            arrCounts[0] = c.returnAggregate("SELECT COUNT(OrderID) FROM [dbo].[OrdersData] WHERE [OrderPayStatus] = 1 AND [OrderStatus] = 3 AND YEAR([OrderDate]) = YEAR('"+ myFromDate + "') AND MONTH([OrderDate]) = MONTH('" + DateTime.Now + "')").ToString();
            arrCounts[1] = c.returnAggregate("SELECT COUNT(OrderID) FROM [dbo].[OrdersData] WHERE [OrderPayStatus] = 1 AND [OrderStatus] = 5 AND YEAR([OrderDate]) = YEAR('"+ myFromDate + "') AND MONTH([OrderDate]) = MONTH('" + DateTime.Now + "')").ToString();
            arrCounts[2] = c.returnAggregate("SELECT COUNT(OrderID) FROM [dbo].[OrdersData] WHERE [OrderPayStatus] = 1 AND [OrderStatus] = 6 AND YEAR([OrderDate]) = YEAR('"+ myFromDate + "') AND MONTH([OrderDate]) = MONTH('" + DateTime.Now + "')").ToString();
            arrCounts[3] = c.returnAggregate("SELECT COUNT(OrderID) FROM [dbo].[OrdersData] WHERE [OrderPayStatus] = 1 AND [OrderStatus] = 7 AND YEAR([OrderDate]) = YEAR('"+ myFromDate + "') AND MONTH([OrderDate]) = MONTH('" + DateTime.Now + "')").ToString();
            arrCounts[4] = c.returnAggregate(@"SELECT COUNT(OrderID) FROM [dbo].[OrdersData] WHERE [OrderStatus] = 13 
                                    AND (Convert(varchar(20), [OrderDate], 112) >= Convert(varchar(20), CAST('" + myFromDate + "' AS DATETIME), 112)) AND (Convert(varchar(20), [OrderDate], 112) <= Convert(varchar(20), CAST('" + DateTime.Now + "' AS DATETIME), 112))").ToString();
            arrCounts[5] = c.returnAggregate(@"SELECT COUNT(OrderID) FROM [dbo].[OrdersData] WHERE [OrderStatus] = 14
                                    AND (Convert(varchar(20), [OrderDate], 112) >= Convert(varchar(20), CAST('" + myFromDate + "' AS DATETIME), 112)) AND (Convert(varchar(20), [OrderDate], 112) <= Convert(varchar(20), CAST('" + DateTime.Now + "' AS DATETIME), 112))").ToString();
            arrCounts[6] = c.returnAggregate(@"SELECT COUNT(OrderID) FROM [dbo].[OrdersData] WHERE [OrderStatus] = 15 
                                    AND (Convert(varchar(20), [OrderDate], 112) >= Convert(varchar(20), CAST('" + myFromDate + "' AS DATETIME), 112)) AND (Convert(varchar(20), [OrderDate], 112) <= Convert(varchar(20), CAST('" + DateTime.Now + "' AS DATETIME), 112))").ToString();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetCount", ex.Message.ToString());
            return;

        }
    }
}