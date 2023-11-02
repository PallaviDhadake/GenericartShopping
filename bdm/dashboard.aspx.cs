using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class bdm_dashboard : System.Web.UI.Page
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
            // Prepare From & To Date range as parameter (28-Apr-2023)
            string dateRange = c.GetFinancialYear();
            string[] arrDateRange = dateRange.ToString().Split('#');
            DateTime myFromDate = Convert.ToDateTime(arrDateRange[0]);
            DateTime myToDate = Convert.ToDateTime(arrDateRange[1]);

            

            //arrCounts[0] = c.returnAggregate("Select Count(a.OrderID) From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID Where a.OrderStatus<>0 AND a.OrderType IS NOT NULL AND a.OrderType<>0").ToString();
            arrCounts[0] = c.returnAggregate("Select Count(a.OrderID) From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID Where (Convert(varchar(20), a.OrderDate, 112) >= Convert(varchar(20), CAST('" + myFromDate + "' AS DATETIME), 112)) " +
                " AND (Convert(varchar(20), a.OrderDate, 112) <= Convert(varchar(20), CAST('" + myToDate + "' AS DATETIME), 112)) AND a.OrderStatus<>0 AND a.OrderType IS NOT NULL AND a.OrderType<>0").ToString();

            arrCounts[1] = c.returnAggregate("Select SUM(OrdDetailAmount) From OrdersDetails Where FK_DetailOrderID IN (Select a.OrderID From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID Where a.OrderStatus<>0 AND a.OrderType IS NOT NULL AND a.OrderType<>0)").ToString();

            arrCounts[2] = c.returnAggregate("Select Count(a.OrderID) From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID Where a.OrderStatus=7 AND a.OrderType IS NOT NULL AND a.OrderType<>0").ToString();
            arrCounts[3] = c.returnAggregate("Select SUM(OrdDetailAmount) From OrdersDetails Where FK_DetailOrderID IN (Select a.OrderID From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID Where a.OrderStatus=7 AND a.OrderType IS NOT NULL AND a.OrderType<>0)").ToString();

            arrCounts[4] = c.returnAggregate("Select Count(CustomrtID) From CustomersData Where delMark=0 AND CustomerActive=1").ToString();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FillGrid", ex.Message.ToString());
            return;

        }
    }
}