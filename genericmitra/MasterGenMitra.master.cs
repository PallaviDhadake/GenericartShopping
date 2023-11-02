using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class genericmitra_MasterGenMitra : System.Web.UI.MasterPage
{
    iClass c = new iClass();
    public string rootPath, welcomeMessage;
    public string[] franchiseeData = new string[5];
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["adminGenMitra"] == null)
        {
            Response.Redirect("Default.aspx");
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        rootPath = c.ReturnHttp();
        if (Session["adminGenMitra"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        welcomeMessage = "Welcome <span class=\"greenName\" >" + c.GetReqData("GenericMitra", "GMitraName", "GMitraID=" + Session["adminGenMitra"]).ToString() + "</span>";
    }
}
