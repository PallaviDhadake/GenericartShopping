using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class doctors_dashboard : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] arrCounts = new string[10];
    public string strUrl;
    protected void Page_Load(object sender, EventArgs e)
    {
        GetCount();
    }

    private void GetCount()
    {
        try
        {
            string docId = Session["adminDoctor"].ToString();
            arrCounts[0] = c.returnAggregate("Select Count(PreReqID) From PrescriptionRequest Where PreReqStatus=1 And FK_DoctorID=" + docId).ToString();
            arrCounts[1] = c.returnAggregate("Select Count(PreReqID) From PrescriptionRequest Where PreReqStatus=3 And FK_DoctorID=" + docId).ToString();
            arrCounts[3] = c.returnAggregate(@"Select Count(OrderID) From OrdersData a Inner Join CustomersData b
                                                On a.FK_OrderCustomerID = b.CustomrtID Where a.OrderStatus = 1 AND a.OrderType = 2 
                                                AND (b.CustomerFavShop IS NULL OR b.CustomerFavShop = 0)").ToString();

            arrCounts[4] = c.returnAggregate("Select Count(a.OrderID) From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID Where a.OrderStatus=1 AND a.OrderType=2 AND a.FK_OrderCustomerID IS NOT NULL AND a.FK_OrderCustomerID<>0 AND (b.CustomerFavShop IS NOT NULL OR b.CustomerFavShop<>0)").ToString(); // new fav shop rx orders

            //arrCounts[3] = c.returnAggregate("Select Count(OrdAssignID) From OrdersAssign Where OrdAssignStatus=6 And Fk_FranchID=" + fId).ToString();
            //arrCounts[4] = c.returnAggregate("Select Count(OrdAssignID) From OrdersAssign Where OrdAssignStatus=7 And Fk_FranchID=" + fId).ToString();

            if (Session["adminDoctor"].ToString() == "5")
            {
                strUrl = "my-appointments.aspx?type=paid";
                arrCounts[2] = c.returnAggregate("Select Count(a.DocAppID) From DoctorsAppointmentData a Inner Join online_payment_logs b On a.Doc_txn_id = b.OPL_merchantTranId Where b.OPL_transtatus='paid' AND a.Doc_pay_amount>0 AND ( a.FK_DocID=" + Session["adminDoctor"] + " OR a.FK_DocID=0 )").ToString();
            }
            else if (Session["adminDoctor"].ToString() == "6")
            {
                strUrl = "my-appointments.aspx?type=new";
                arrCounts[2] = c.returnAggregate("Select Count(DocAppID) From DoctorsAppointmentData Where Doc_txn_id IS NULL AND (FK_DocID=" + Session["adminDoctor"] + " OR FK_DocID=0 ) AND DocAppStatus=0").ToString();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetCount", ex.Message.ToString());
            return;
        }
    }
}