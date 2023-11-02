using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class forgot_pwd : System.Web.UI.Page
{
    
    iClass c = new iClass();
    public string errMsg, rootPath;
    protected void Page_Load(object sender, EventArgs e)
    {
        btnRecover.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnRecover, null) + ";");
    }

    protected void btnRecover_Click(object sender, EventArgs e)
    {
        try
        {
            txtUserId.Text = txtUserId.Text.Trim().Replace("'", "");
            if (txtUserId.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter mobile number.');", true);
                return;
            }
            if (c.IsRecordExist("Select * From CustomersData Where CustomerMobile='" + txtUserId.Text.Trim() + "'") == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Invalid mobile number.');", true);
                return;
            }

            string userId = txtUserId.Text.Trim();
            string frPwd = c.GetReqData("CustomersData", "CustomerPassword", "CustomerMobile='" + userId + "'").ToString();
            string frUserName = c.GetReqData("CustomersData", "CustomerName", "CustomerMobile='" + userId + "'").ToString();
            string msgData = "Hello " + frUserName + ", Your User Id : " + userId + " Password : " + frPwd + " Genericart Medicine - Wahi Kaam, Sahi Daam";
            c.SendSMS(msgData, userId);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Account details send to your mobile no.');", true);
            txtUserId.Text = "";
            string path = Master.rootPath + "login";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('" + path + "', 2000);", true);
           
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}