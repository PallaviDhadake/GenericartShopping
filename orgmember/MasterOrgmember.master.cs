using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class orgmember_MasterOrgmember : System.Web.UI.MasterPage
{
    iClass c = new iClass();
    public string rootPath;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["adminorgMember"] == null)
        {
            Response.Redirect("Default.aspx");
        }
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        rootPath = c.ReturnHttp();
        if (Session["adminorgMember"] == null)
        {
            Response.Redirect("default.aspx");
        }
        //welcomeMessage = "Welcome <span class=\"greenName\" >" + c.GetReqData("DistributorsData", "distUserName", "distId=" + Session["adminDistributor"]).ToString() + "</span>";
    }

}
