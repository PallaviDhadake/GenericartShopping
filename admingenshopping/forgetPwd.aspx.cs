using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admingenshopping_forgetPwd : System.Web.UI.Page
{
    public string errMsg, rootPath;
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdRecover.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(cmdRecover, null) + ";");
    }

    protected void cmdRecover_Click(object sender, EventArgs e)
    {
        txtEmail.Text = txtEmail.Text.Trim().Replace("'", "");
        if (txtEmail.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Email Address.');", true);
            return;
        }
        if (c.IsRecordExist("Select * From Users Where emailId='" + txtEmail.Text.Trim() + "'") == false)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Invalid Email Address.');", true);
            
            return;
        }

        string userName = txtEmail.Text.Trim();
        string userPwd = c.GetReqData("Users", "userPwd", "emailId='" + userName + "'").ToString();
        string msgData = "<p><b>User Email : </b>" + userName + "</p><br/><p><b>Password :</b>" + userPwd + "</p>";

        //c.sendMail("info@nandadeepeyehospital.org", "Nandadeep Eye Hospital", txtEmail.Text.Trim(), msgData, "Account Credentials", "", true, "", "");

        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Account details send to your email.');", true);
        
        txtEmail.Text = "";
        Response.Redirect("Default.aspx", false);
    }
}