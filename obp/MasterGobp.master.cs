using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class obp_MasterGobp : System.Web.UI.MasterPage
{
    iClass c = new iClass();
    public string firstChar, sessionName, bgColor, rootPath, DHname, welcomeMessage;
    protected void Page_Load(object sender, EventArgs e)
    {
        // Important code line
        this.Page.Header.DataBind();

        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "sessionPingTrigger();", true);
        if (!IsPostBack)
        {
            //rootPath = c.ReturnHttp();
            if (Session["adminObp"] == null)
            {
                Response.Redirect(Master.rootPath + "login", false);
            }
            else
            {
                GetGobpInfo();
            }
        }
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        rootPath = c.ReturnHttp();

       
    }

    public void GetGobpInfo()
    {
        string custName = c.GetReqData("OBPData", "OBP_ApplicantName", "OBP_ID=" + Session["adminObp"]).ToString();
        firstChar = custName.Substring(0, 1);
        //firstChar = custName;
        //string[] arrSessionName = custName.ToString().Split(' ');
        sessionName = c.GetReqData("OBPData", "OBP_UserID", "OBP_ID=" + Session["adminObp"]).ToString();
        // array of colors
        string[] colors = { "#881798", "#e3008c", "#ffb900", "#107c10", "#da3b01", "#e3008c", "#00b7c3", "#0078d7", "#744da9", "#ff4343" };
        Random rNo = new Random();
        int index = rNo.Next(colors.Length);
        bgColor = "background:" + colors[index].ToString();
    }
}
