using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class districthead_MasterDH : System.Web.UI.MasterPage
{
    iClass c = new iClass();
    public string rootPath, welcomeMessage;
    public string[] franchiseeData = new string[5];
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["adminDH"] == null)
        {
            Response.Redirect("Default.aspx");
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        rootPath = c.ReturnHttp();
        if (Session["adminDH"] == null)
        {
            Response.Redirect("default.aspx");
        }
        welcomeMessage = "Welcome <span class=\"greenName\" >" + c.GetReqData("DistrictHead", "DistHdName", "DistHdId=" + Session["adminDH"]).ToString() + "</span>";
    }
}
