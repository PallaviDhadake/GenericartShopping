using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class customer_Default : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        //  ============= Check Customer URL WebKey and SESSION status =========
        if (Request.QueryString["webkey"] != null)
        {
            //Check this UserId GOBP exist or not?
            if (c.IsRecordExist("Select CustomrtID From CustomersData Where Customer_WebKey='" + Request.QueryString["webkey"] + "'") == true)
            {
                Session["genericCust"] = c.GetReqData("CustomersData", "CustomrtID", "Customer_WebKey='" + Request.QueryString["webkey"] + "'");
                Response.Redirect("user-info.aspx");
            }
            else
            {
                Response.Redirect("~/login");
            }
        }
    }
}