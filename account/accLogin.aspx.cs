using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class account_accLogin : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string encryptString = Request.QueryString["id"].ToString();

            string descryptString = c.DecryptString(encryptString);

            int accId = Convert.ToInt32(c.GetReqData("AccountUser", "auId", "auUserName='" + descryptString + "' AND delMark=0"));
            Session["adminAcc"] = accId;

            Response.Redirect("dashboard.aspx", false);
        }
    }

    
}