using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class franchisee_dashboard : System.Web.UI.Page
{
    iClass c = new iClass();
    public string shopId;


    public string[] arrCounts = new string[15]; //9
    public string name;
    protected void Page_Load(object sender, EventArgs e)
    {
        //int id =Convert.ToInt32(Session["adminFranchisee"].ToString());
        //name = c.GetReqData("FranchiseeData", "FranchName", "FranchID="+id).ToString();
        GetCount();
        shopId= Session["adminFranchisee"].ToString();
    }

    private void GetCount()
    {
        try
        {
            string fId = Session["adminFranchisee"].ToString();
            string shopCode = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + fId).ToString();
            arrCounts[0] = c.returnAggregate("Select Count(a.OrdAssignID) From OrdersAssign a Inner Join OrdersData b On a.FK_OrderID = b.OrderID Inner Join CustomersData c On b.FK_OrderCustomerID = c.CustomrtID  Where a.OrdAssignStatus=0 AND a.OrdReAssign=0 AND b.OrderType=1 And a.Fk_FranchID=" + fId + " AND b.OrderStatus<>2 AND a.OrdReAssign=0").ToString();
            arrCounts[1] = c.returnAggregate("Select Count(a.OrdAssignID) From OrdersAssign a Inner Join OrdersData b On a.FK_OrderID = b.OrderID Where a.OrdAssignStatus=5 AND b.OrderStatus<>2 And a.Fk_FranchID=" + fId).ToString();
            
            arrCounts[3] = c.returnAggregate("Select Count(OrdAssignID) From OrdersAssign Where OrdAssignStatus=6 And Fk_FranchID="+fId).ToString();
            arrCounts[4] = c.returnAggregate("Select Count(OrdAssignID) From OrdersAssign Where OrdAssignStatus=7 And Fk_FranchID="+fId).ToString();
            arrCounts[9] = c.returnAggregate("Select Count(OrdAssignID) From OrdersAssign Where OrdAssignStatus=10 And Fk_FranchID=" + fId).ToString();

            arrCounts[5] = c.returnAggregate("Select Count(a.OrdAssignID) From OrdersAssign a Inner Join OrdersData b On a.FK_OrderID = b.OrderID Inner Join CustomersData c On b.FK_OrderCustomerID = c.CustomrtID Where a.OrdAssignStatus=0 AND a.OrdReAssign=0 AND b.OrderStatus<>2 AND b.OrderType=2 And a.Fk_FranchID=" + fId).ToString();

            if (fId == "24")
            {
                arrCounts[2] = c.returnAggregate("Select Count(OrdAssignID) From OrdersAssign Where OrdAssignStatus=2 And Fk_FranchID=" + fId + " AND (CONVERT(varchar(20), OrdAssignDate, 112) >= CONVERT(varchar(20), CAST('2021-08-01' as datetime) ,112))").ToString();
                arrCounts[6] = c.returnAggregate("Select Count(a.EnqAssignID) From SavingEnqAssign a Inner Join SavingCalc b On a.FK_CalcID = b.CalcID Inner Join CustomersData c On b.FK_CustId=c.CustomrtID Where a.EnqAssignStatus=0 AND a.EnqReAssign=0 And a.Fk_FranchID=" + fId + " AND (CONVERT(varchar(20), a.EnqAssignDate, 112) >= CONVERT(varchar(20), CAST('2021-08-15' as datetime) ,112))").ToString();
            }
            else
            {
                arrCounts[2] = c.returnAggregate("Select Count(OrdAssignID) From OrdersAssign Where OrdAssignStatus=2 And Fk_FranchID=" + fId).ToString();
                arrCounts[6] = c.returnAggregate("Select Count(a.EnqAssignID) From SavingEnqAssign a Inner Join SavingCalc b On a.FK_CalcID = b.CalcID Inner Join CustomersData c On b.FK_CustId=c.CustomrtID Where a.EnqAssignStatus=0 AND a.EnqReAssign=0 And a.Fk_FranchID=" + fId).ToString();
            }
            arrCounts[7] = c.returnAggregate("Select Count(PrescFwdID) From PrescriptionForword Where FK_FranchID=" + fId + " AND PrescFwdStatus=0").ToString();
            arrCounts[8] = c.returnAggregate("Select Count(GMitraID) From GenericMitra Where GMitraShopCode='" + shopCode + "'").ToString();


            using (DataTable dtCompShop = c.GetDataTable("Select FK_FranchID From CompanyOwnShops Where DelMark=0"))
            {
                if (dtCompShop.Rows.Count > 0)
                {
                    foreach (DataRow row in dtCompShop.Rows)
                    {
                        if (Convert.ToInt32(row["FK_FranchID"]) == Convert.ToInt32(Session["adminFranchisee"]))
                        {
                            bluedart.Visible = true;
                        }
                    }
                }
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