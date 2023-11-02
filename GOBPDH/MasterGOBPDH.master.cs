using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GOBPDH_MasterGOBPDH : System.Web.UI.MasterPage
{
    iClass c = new iClass();
    public string rootPath, welcomeMessage, DHname;
    public string[] franchiseeData = new string[5];
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["adminGOBPDH"] == null)
        {
            Response.Redirect("Default.aspx");
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        rootPath = c.ReturnHttp();
        if (Session["adminGOBPDH"] == null)
        {
            Response.Redirect("Default.aspx");
        }

        DHname = "<span class=\"greenName\">( " + c.GetReqData("DistrictHead", "DistHdName", "DistHdId=" + Session["adminGOBPDH"].ToString()) + " )</span>";

        welcomeMessage = "Welcome <span class=\"greenName\" >" + c.GetReqData("[dbo].[DistrictHead]", "[DistHdUserId]", "[DistHdId]=" + Session["adminGOBPDH"]).ToString() + "</span>" + " " +  DHname;
    }
}
