using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class supportteam_team_alert : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] callInfo = new string[10];
    protected void Page_Load(object sender, EventArgs e)
    {
        if(c.IsRecordExist("Select FlupID From FollowupOrders where FK_CustomerId=" + Convert.ToInt32(Request.QueryString["custId"])) == true)
        {
            int teamMemId = Convert.ToInt32(c.GetReqData("FollowupOrders", "FK_TeamMemberId", "CONVERT(varchar(20), FlupDate, 112) = CONVERT(varchar(20), CAST(GETDATE() as datetime), 112) AND FK_CustomerId=" + Request.QueryString["custId"]));
            callInfo[0] = c.GetReqData("CustomersData", "CustomerName", "CustomrtID=" + Convert.ToInt32(Request.QueryString["custId"])).ToString();
            callInfo[1] = c.GetReqData("SupportTeam", "TeamPersonName", "TeamID=" + teamMemId).ToString();
            callInfo[2] = c.GetReqData("FollowupOrders", "FlupDate", "CONVERT(varchar(20), FlupDate, 112) = CONVERT(varchar(20), CAST(GETDATE() as datetime), 112) AND FK_CustomerId=" + Request.QueryString["custId"]).ToString();
            callInfo[3] = Convert.ToDateTime(callInfo[2]).ToString("dd/MM/yyyy hh:mm tt");
        }
    }
}