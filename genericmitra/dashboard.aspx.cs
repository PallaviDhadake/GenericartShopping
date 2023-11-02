using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class genericmitra_dashboard : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] arrCounts = new string[10];
    protected void Page_Load(object sender, EventArgs e)
    {
        GetCount();
    }

    private void GetCount()
    {
        try
        {
            arrCounts[0] = c.returnAggregate("Select Count(CustomrtID) From CustomersData Where delMark=0 And FK_GenMitraID=" + Session["adminGenMitra"] + " And CustomerActive=1").ToString();
            //arrCounts[1] = c.returnAggregate("Select Count(CustomrtID) From CustomersData Where delMark=0 And FK_GenMitraID=" + Session["adminGenMitra"] + " And CustomerActive=1 And CustomerFavShop Is Not Null").ToString();
            arrCounts[1] = c.returnAggregate("Select Count(distinct CustomerFavShop) From CustomersData Where FK_GenMitraID=" + Session["adminGenMitra"]).ToString();
            arrCounts[2] = c.returnAggregate("Select Sum(OrderAmount) From OrdersData Where FK_OrderCustomerID  In (Select CustomrtID From CustomersData Where FK_GenMitraID="+ Session["adminGenMitra"] +")").ToString();

            //double comissionAmt = (Convert.ToDouble(arrCounts[2]) * 5) / 100;

            //arrCounts[3] = comissionAmt.ToString("0.00");

            arrCounts[3] = c.returnAggregate("Select SUM(GMitraComTotal) From OrdersData Where GMitraId=" + Session["adminGenMitra"]).ToString();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetCount", ex.Message.ToString());
            return;
        }
    }
}