using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class franchisee_cancel_request_reason_enq : System.Web.UI.Page
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
            c.ExecuteQuery("Update SavingEnqAssign Set EnqAssignStatus=2, FK_ReasonID=" + ddrReasons.SelectedValue + " Where FK_CalcID=" + Request.QueryString["id"] + " AND Fk_FranchID=" + Session["adminFranchisee"]);

            //after rejecting order by shop, assign it to default shop
            int frId = Convert.ToInt32(c.GetReqData("DefaultShop", "FranchId", "DsID=1"));

            if (frId == Convert.ToInt32(Session["adminFranchisee"]))
            {
                //if rejected by default shop it will be set as new order (re assigned) to admin
                c.ExecuteQuery("Update SavingCalc Set EnqStatus=8 Where CalcID=" + Request.QueryString["id"]);
            }
            else
            {
                if (Convert.ToInt32(ddrReasons.SelectedValue) == 7 || Convert.ToInt32(ddrReasons.SelectedValue) == 8 || Convert.ToInt32(ddrReasons.SelectedValue) == 9)
                {
                    // send it to GMMH0001

                    if (!c.IsRecordExist("Select EnqAssignID From SavingEnqAssign Where FK_CalcID=" + Request.QueryString["id"] + " AND Fk_FranchID=" + frId + " AND EnqAssignStatus=0"))
                    {
                        c.ExecuteQuery("Update SavingEnqAssign Set EnqReAssign=1 Where FK_CalcID=" + Request.QueryString["id"]);
                        int maxId = c.NextId("SavingEnqAssign", "EnqAssignID");
                        c.ExecuteQuery("Insert Into SavingEnqAssign (EnqAssignID, EnqAssignDate, FK_CalcID, Fk_FranchID, EnqAssignStatus, " +
                            " EnqReAssign) Values (" + maxId + ", '" + DateTime.Now + "', " + Request.QueryString["id"] + ", " + frId + ", 0, 0)");

                    }
                }
                else if (Convert.ToInt32(ddrReasons.SelectedValue) == 10)
                {
                    // set its status as admin action required
                    c.ExecuteQuery("Update SavingCalc Set EnqStatus=9 Where CalcID=" + Request.QueryString["id"]);
                }
                else
                {
                    // set as denied by admin
                    c.ExecuteQuery("Update SavingCalc Set EnqStatus=4 Where CalcID=" + Request.QueryString["id"]);
                }
            }

            errMsg = c.ErrNotification(1, "Enquiry Rejected");
            ClientScript.RegisterStartupScript(this.GetType(), "redirect", "setTimeout(function () { if(top!=self) {top.location.href = 'enquiry-report.aspx';} }, 2000);", true);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('enquiry-report.aspx', 2000);", true);
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}