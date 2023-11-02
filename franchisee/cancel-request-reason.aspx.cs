using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class franchisee_cancel_request_reason : System.Web.UI.Page
{
    iClass c = new iClass();
    public string rootPath, errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        rootPath = c.ReturnHttp();
        btnSubmit.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnSubmit, null) + ";");
        if (!IsPostBack)
        {
            c.FillComboBox("ReasonTitle", "ReasonID", "CancelReasons", "ResonType=2 AND DelMark=0", "ReasonTitle", 0, ddrReasons);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddrReasons.SelectedIndex == 0)
            {
                errMsg = c.ErrNotification(2, "Select Reason to cancel order");
                return;
            }
            int shopId = Convert.ToInt32(Session["adminFranchisee"].ToString());
            int reasonId = Convert.ToInt32(ddrReasons.SelectedValue);
            c.ExecuteQuery("Update OrdersAssign Set OrdAssignStatus=2, FK_ReasonID=" + ddrReasons.SelectedValue + " Where FK_OrderID=" + Request.QueryString["id"] + " AND Fk_FranchID=" + Session["adminFranchisee"]);

            //after rejecting order by shop, assign it to default shop
            int frId = Convert.ToInt32(c.GetReqData("DefaultShop", "FranchId", "DsID=1"));

            if (frId == Convert.ToInt32(Session["adminFranchisee"]))
            {
                //if rejected by default shop it will be set as new order (re assigned) to admin
                c.ExecuteQuery("Update OrdersData Set OrderStatus=8 Where OrderID=" + Request.QueryString["id"]);
            }
            else
            {
                if (Convert.ToInt32(ddrReasons.SelectedValue) == 7 || Convert.ToInt32(ddrReasons.SelectedValue) == 8 || Convert.ToInt32(ddrReasons.SelectedValue) == 9)
                {
                    // send it to GMMH0001

                    //if (!c.IsRecordExist("Select OrdAssignID From OrdersAssign Where FK_OrderID=" + Request.QueryString["id"] + " AND Fk_FranchID=" + frId + " AND OrdAssignStatus=0"))
                    //{
                    //    c.ExecuteQuery("Update OrdersAssign Set OrdReAssign=1 Where FK_OrderID=" + Request.QueryString["id"]);
                    //    int maxId = c.NextId("OrdersAssign", "OrdAssignID");
                    //    c.ExecuteQuery("Insert Into OrdersAssign (OrdAssignID, OrdAssignDate, FK_OrderID, Fk_FranchID, OrdAssignStatus, " +
                    //        " OrdReAssign, AssignedFrom) Values (" + maxId + ", '" + DateTime.Now + "', " + Request.QueryString["id"] + ", " + frId + ", 0, 0, 'ShopLogin')");


                    //    string mobNo = c.GetReqData("FranchiseeData", "FranchMobile", "FranchID=" + frId).ToString();
                    //    string frName = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + frId).ToString();
                    //    string pendingOrdCount = c.returnAggregate("Select Count(OrdAssignID) From OrdersAssign Where OrdAssignStatus=0 AND OrdReAssign=0 AND Fk_FranchID=" + frId).ToString();
                    //    string msgData = "Dear" + frName + ", You have received new order Order No" + Request.QueryString["id"] + "from Genericart Mobile App.Total Pending Order is/are" + pendingOrdCount + " Thank you Genericart Medicine Store Wahi Kaam, Sahi Daam";
                    //    c.SendSMS(msgData, mobNo);
                    //    //c.SendSMS(msgData, "8408027474");
                    //}
                }
                else if (Convert.ToInt32(ddrReasons.SelectedValue) == 10)
                {
                    // set its status as admin action required
                    c.ExecuteQuery("Update OrdersData Set OrderStatus=9 Where OrderID=" + Request.QueryString["id"]);
                }
                else
                {
                    // set as denied by admin
                    c.ExecuteQuery("Update OrdersData Set OrderStatus=4 Where OrderID=" + Request.QueryString["id"]);
                }
            }

            errMsg = c.ErrNotification(1, "Order Rejected");
            //string pageUrl = rootPath + "customer/my-orders";
            ClientScript.RegisterStartupScript(this.GetType(), "redirect", "setTimeout(function () { if(top!=self) {top.location.href = 'orders-report.aspx';} }, 2000);", true);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('orders-report.aspx', 2000);", true);
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}