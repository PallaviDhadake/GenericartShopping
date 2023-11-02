using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class orgmember_change_password : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        txtOldPass.Text = txtOldPass.Text.Trim().Replace("'", "");
        txtNewPass.Text = txtNewPass.Text.Trim().Replace("'", "");
        txtConfirmPass.Text = txtConfirmPass.Text.Trim().Replace("'", "");

        if (txtOldPass.Text == "" || txtNewPass.Text == "" || txtConfirmPass.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter old and new password');", true);
            return;
        }
        if (txtOldPass.Text == txtNewPass.Text)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Old and new password cannot be same');", true);
            return;
        }

        try
        {
            if (txtOldPass.Text == c.GetReqData("ZonalHead", "ZonalHdPass", "ZonalHdId=" + Session["adminorgMember"]).ToString())
            {
                if (txtNewPass.Text.Length < 6)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Password should be atleast 6 character long');", true);
                    return;
                }
                else
                {
                    if (txtNewPass.Text == txtConfirmPass.Text)
                    {
                        c.ExecuteQuery("Update ZonalHead Set ZonalHdPass='" + txtNewPass.Text + "' Where ZonalHdId=" + Session["adminorgMember"]);

                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Password successfully changed');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'New password confirmation failed');", true);
                        return;
                    }
                    txtNewPass.Text = txtConfirmPass.Text = "";
                    txtOldPass.Text = "";
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Old password did not match');", true);
                return;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSubmit_Click", ex.Message.ToString());
            return;
        }
    }
}