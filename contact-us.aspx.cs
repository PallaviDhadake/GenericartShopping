using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Text;

public partial class contact_us : System.Web.UI.Page
{
    public string errMsg;
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSubmit.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnSubmit, null) + ";");
    }

    protected void UpdatePanel1_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "grecaptcha.render('recaptcha', {'sitekey': '6LcNBIUUAAAAADbX-_n6UhdJhtAxQDgiypcyZqSN'});", true);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            txtName.Text = txtName.Text.Trim().Replace("'", "");
            txtEmail.Text = txtEmail.Text.Trim().Replace("'", "");
            txtMobile.Text = txtMobile.Text.Trim().Replace("'", "");
            txtMsg.Text = txtMsg.Text.Trim().Replace("'", "");

            if (txtName.Text == "" || txtEmail.Text == "" || txtMobile.Text == "" || txtMsg.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * Marked fields are Mandatory');", true);
                return;
            }
            if (c.EmailAddressCheck(txtEmail.Text) == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid Email Address');", true);
                return;
            }
            if (c.ValidateMobile(txtMobile.Text) == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid Mobile No.');", true);
                return;
            }

            string EncodedResponse = Request.Form["g-Recaptcha-Response"];
            bool IsCaptchaValid = (ReCaptchaClass.Validate(EncodedResponse) == "True" ? true : false);

            if (!IsCaptchaValid)
            {
                //InValid Request
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Captcha Verification Failed');", true);
                return;
            }

            StringBuilder strMarkup = new StringBuilder();

            strMarkup.Append("Dear Sir, <br/><br/>");
            strMarkup.Append("You have a new Feedback <br/><br/>");
            strMarkup.Append("Details..<br/><br/>");
            strMarkup.Append("Name - <b>" + txtName.Text + "</b> <br/><br/>");
            strMarkup.Append("Email Id - <b>" + txtEmail.Text + "</b> <br/><br/>");
            strMarkup.Append("Mobile No - <b>" + txtMobile.Text + "</b> <br/><br/>");
            strMarkup.Append("Message - <b>" + txtMsg.Text + "</b> <br/>");

            //IT HELP DESK GMPL  helpdesk@genericartmedicine.com
            //c.SendMail("info@genericart.in", "Genericart Medicine", "prajaktap204@gmail.com", strMarkup.ToString(), "New Feedback at Genericart Medicine", "", true, "", "");
            c.SendMail("info@genericart.in", "Genericart Medicine", "helpdesk@genericartmedicine.com", strMarkup.ToString(), "New Feedback at Genericart Medicine", "", true, "", "");

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Thank you for your feedback. We will contact you soon.');", true);
            errMsg = c.ErrNotification(1, "");

            txtName.Text = txtEmail.Text = txtMobile.Text = txtMsg.Text = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSubmit_Click", ex.Message.ToString());
            return;
        }
    }
}