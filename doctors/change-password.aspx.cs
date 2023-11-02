using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class doctors_change_password : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        txtConfirm.Text = txtConfirm.Text.Trim().Replace("'", "");
        txtNew.Text = txtNew.Text.Trim().Replace("'", "");
        txtOld.Text = txtOld.Text.Trim().Replace("'", "");

        string temp = Session["adminDoctor"].ToString();

        if (txtOld.Text == "" || txtNew.Text == "" || txtConfirm.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Old and New password');", true);
            return;
        }
        if (txtOld.Text == txtNew.Text)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Old password and New passwords cannot be same');", true);
            return;
        }
        try
        {
            if (txtOld.Text == c.GetReqData("DoctorsData", "DocPassword", "DoctorID=" + temp).ToString())
            {
                if (txtNew.Text.Length < 6)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Password should be at least 6 characters long');", true);
                    return;
                }
                else
                {
                    if (txtNew.Text == txtConfirm.Text)
                    {
                        c.ExecuteQuery("Update DoctorsData Set DocPassword='" + txtNew.Text + "' Where DoctorID=" + temp);
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Password successfully changed');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'New password confirmation failed');", true);
                        return;
                    }
                    txtNew.Text = txtOld.Text = txtConfirm.Text = "";
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