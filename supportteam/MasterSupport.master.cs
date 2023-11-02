using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class supportteam_MasterSupport : System.Web.UI.MasterPage
{
    iClass c = new iClass();
    public string rootPath, welcomeMessage;
    protected void Page_Load(object sender, EventArgs e)
    {
        rootPath = c.ReturnHttp();
        if (Session["adminSupport"] == null)
        {
            Response.Redirect("Default.aspx");
        }

        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "sessionPingTrigger();", true);
        //if (Session["adminSupport"].ToString() == "1")
        //{
        //    teamLead.Visible = true;
        //    teamStaff.Visible = false;

        //}
        //else
        //{
        //    teamLead.Visible = false;
        //    teamStaff.Visible = true;
        //}
        checkUserRole();
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        rootPath = c.ReturnHttp();
        if (Session["adminSupport"] == null)
        {
            Response.Redirect("default.aspx");
        }
        welcomeMessage = "Welcome <span class=\"greenName\" >" + c.GetReqData("SupportTeam", "TeamPersonName", "TeamID=" + Session["adminSupport"]).ToString() + "</span>";
        
        ScriptManager1.Services.Add(new ServiceReference(rootPath + "WebServices/ShoppingWebService.asmx"));
        ScriptManager1.Services.Add(new ServiceReference(rootPath + "WebServices/supportTeamWebServices.asmx"));
        ScriptManager1.Services.Add(new ServiceReference(rootPath + "WebServices/adminShoppingWebService.asmx"));
    }

    private void checkUserRole()
    {
        try
        {
            int teamId = Convert.ToInt32(Session["adminSupport"]);
            int taskId = Convert.ToInt32(c.GetReqData("SupportTeam", "TeamTaskID", "TeamID="+ teamId +""));
            if (Session["adminSupport"].ToString() == "1")
            {
                teamLead.Visible = true;
                teamStaff.Visible = false;
                purchaseDept.Visible = false;

            }
            else
            {
                if (taskId == 7)
                {
                    teamLead.Visible = false;
                    purchaseDept.Visible = true;
                    teamStaff.Visible = false;
                }
                else
                {
                    teamLead.Visible = false;
                    teamStaff.Visible = true;
                    purchaseDept.Visible = false;
                }
                
            }
        }
        catch (Exception ex)
        {

            throw;
        }
    }
}
