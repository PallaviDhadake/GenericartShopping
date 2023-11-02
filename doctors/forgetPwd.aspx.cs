using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class doctors_forgetPwd : System.Web.UI.Page
{
    public string errMsg, rootPath;
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdRecover.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(cmdRecover, null) + ";");
    }

    protected void cmdRecover_Click(object sender, EventArgs e)
    {
        try
        {
            txtMobile.Text = txtMobile.Text.Trim().Replace("'", "");
            if (txtMobile.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter mobile number.');", true);
                return;
            }
            if (c.IsRecordExist("Select * From DoctorsData Where DocMobileNum='" + txtMobile.Text.Trim() + "'") == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Invalid mobile number.');", true);
                return;
            }
            
            string frMobile = txtMobile.Text.Trim();
            string frPwd = c.GetReqData("DoctorsData", "DocPassword", "DocMobileNum='" + frMobile + "'").ToString();
            string frUserName = c.GetReqData("DoctorsData", "DocMobileNum", "DocMobileNum='" + frMobile + "'").ToString();
            string frOwnerName = c.GetReqData("DoctorsData", "DocName", "DocMobileNum='" + frMobile + "'").ToString();
            string msgData = "Hello " + frOwnerName + ", Your User Id : " + frUserName + " Password : " + frPwd + " Genericart Medicine - Wahi Kaam Sahi Daam";
            c.SendSMS(msgData, frMobile);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Account details send to your email.');", true);
            txtMobile.Text = "";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('default.aspx', 2000);", true);
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}