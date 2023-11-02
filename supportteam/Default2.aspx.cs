using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class supportteam_Default2 : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg, rootPath;
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdSign.Attributes.Add("onclick", "this.disabled=true;this.value='Processing...';" + ClientScript.GetPostBackEventReference(cmdSign, null) + ";");
        txtTeamUserID.Focus();

        if (!IsPostBack)
        {
            if (Session["GeneratedOTP"] != null)
            {
                Response.Redirect("dashboard.aspx");
            }
        }
    }

    protected void btnRequest_Click(object sender, EventArgs e)
    {
        try
        {
            OTPGenerator OTPGenerator = new OTPGenerator();

            // Generate the OTP
            string otp = OTPGenerator.GenerateOTP(6);

            // Store the OTP in a Session variable
            Session["GeneratedOTP"] = otp;

            c.ExecuteQuery("UPDATE [dbo].[SupportTeam] SET [LoginReqDate] = '" + DateTime.Now + "' ,[LoginOTP] = " + otp + " ,[LoginValidity] = CONCAT(CAST(GETDATE() AS DATE),':','7:00PM') ,[LoginStatus] = 'Request' WHERE [TeamUserID] = '" + txtTeamUserID.Text + "'");

            // Create a new cookie for LoginValidity
            HttpCookie loginValidityCookie = new HttpCookie("LoginValidity", "[LoginValidity]");

            // Set the Cookies Value to the LoginValidity value you want to store 
            loginValidityCookie.Value = "[LoginValidity]";

            // Set the Expiration time for the cookies (adjust as needed)
            loginValidityCookie.Expires = DateTime.Now.AddHours(9);

            // Add the cookie to the response
            Response.Cookies.Add(loginValidityCookie);

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'OTP Has been send to the Admin.');", true);
            pwd.Visible = true;
            request.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnRequest_Click", ex.Message.ToString());
            return;
        }
    }

    protected void cmdSign_Click(object sender, EventArgs e)
    {
        try
        {
            c.ExecuteQuery("UPDATE [dbo].[SupportTeam] SET [LoginStatus] = 'Active' WHERE [TeamUserID] = '" + txtTeamUserID.Text + "'");
            Response.Redirect("dashboard.aspx", false);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "cmdSign_Click", ex.Message.ToString());
            return;
        }
    }
}