using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class franchisee_frLogin : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string encryptString = Request.QueryString["shopCode"].ToString();

            string descryptString = DecryptString(encryptString);

            int franchId = Convert.ToInt32(c.GetReqData("FranchiseeData", "FranchID", "FranchShopCode='" + descryptString + "' AND FranchActive=1"));
            Session["adminFranchisee"] = franchId;

            if (Request.QueryString["to"] != null)
            {
                if (Request.QueryString["to"] == "clientFollowup")
                {
                    Response.Redirect("monthly-order-followup.aspx", false);
                }
            }
            else
            {
                Response.Redirect("dashboard.aspx", false);
            }
        }
    }

    public string DecryptString(string encrString)
    {
        byte[] b;
        string decrypted;
        try
        {
            b = Convert.FromBase64String(encrString);
            decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
        }
        catch (FormatException fe)
        {
            decrypted = "";
        }
        return decrypted;
    }  
}