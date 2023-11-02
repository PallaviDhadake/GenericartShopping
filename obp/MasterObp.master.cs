using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class obp_MasterObp : System.Web.UI.MasterPage
{
    iClass c = new iClass();
    public string rootPath, welcomeMessage, DHname;
    public string[] franchiseeData = new string[5];
    public string hideMeClass = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        rootPath = c.ReturnHttp();
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "sessionPingTrigger();", true);

        if (Session["adminObp"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        TypeMLM.Visible = c.IsRecordExist("Select OBP_ID From OBPData Where IsMLM=1 AND OBP_ID=" + Session["adminObp"]);
        hideMeClass = (c.IsRecordExist("Select OBP_ID From OBPData Where OBP_FKTypeID=4 AND OBP_ID=" + Session["adminObp"])) ? "showMeBro" : "hideMeBro";
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        rootPath = c.ReturnHttp();
        ScriptManager1.Services.Add(new ServiceReference(rootPath + "WSSession.asmx"));

        if (Session["adminObp"] == null)
        {
            Response.Redirect("Default.aspx");
        }

        DHname = "<span class=\"greenName\">( " + c.GetReqData("OBPData", "OBP_ApplicantName", "OBP_ID=" + Session["adminObp"].ToString()) + " )</span>";

        welcomeMessage = "Welcome <span class=\"greenName\" >" + c.GetReqData("OBPData", "OBP_UserID", "OBP_ID=" + Session["adminObp"]).ToString() + "</span>" + " " + DHname;


        //welcomeMessage = "Welcome <span class=\"greenName\" >" + c.GetReqData("[dbo].[DistrictHead]", "[DistHdUserId]", "[DistHdId]=" + Session["adminObp"]).ToString() + "</span>" + " " + DHname;
    }


}
