using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class doctors_MasterDoctor : System.Web.UI.MasterPage
{
    iClass c = new iClass();
    public string docName, rootPath;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["adminDoctor"] == null)
        {
            Response.Redirect("default.aspx");
        }

        if (Session["adminDoctor"].ToString() == "5")
        {
            shop.Visible = true;
            medList.Visible = true;
        }
        else
        {
            shop.Visible = false;
            medList.Visible = false;
        }

        if (Session["adminDoctor"].ToString() == "6")
        {
            labtest.Visible = true;
        }
        else
        {
            labtest.Visible = false;
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        rootPath = c.ReturnHttp();
        if (Session["adminDoctor"] == null)
        {
            Response.Redirect("default.aspx");
        }
    }
}
