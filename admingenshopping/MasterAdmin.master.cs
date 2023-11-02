using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admingenshopping_MasterAdmin : System.Web.UI.MasterPage
{
    public string rootPath;
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        rootPath = c.ReturnHttp();
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "sessionPingTrigger();", true);

        if (Session["adminMaster"] == null)
        {
            Response.Redirect("default.aspx");
        }
      
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        rootPath = c.ReturnHttp();
        ScriptManager1.Services.Add(new ServiceReference(rootPath + "WSSession.asmx"));
    }


}
