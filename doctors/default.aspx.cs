using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class franchisee_default : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg, rootPath;
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdSign.Attributes.Add("onclick", "this.disabled=true;this.value='Processing...';" + ClientScript.GetPostBackEventReference(cmdSign, null) + ";");
        txtMob.Focus();

        if (!IsPostBack)
        {
            if (Session["adminDoctor"] != null)
            {
                Response.Redirect("dashboard.aspx");
            }
        }
    }

    protected void cmdSign_Click(object sender, EventArgs e)
    {
        try
        {
            txtMob.Text = txtMob.Text.Trim().Replace("'", "");
            txtPwd.Text = txtPwd.Text.Trim().Replace("'", "");

            if (txtMob.Text == "" || txtPwd.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Mobile No & Password.');", true);

                return;
            }
            if (!c.IsRecordExist("Select DoctorID From DoctorsData Where DocMobileNum='" + txtMob.Text + "'"))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Invalid Mobile No Entered, Try Again.');", true);

                return;
            }
            else if (c.GetReqData("DoctorsData", "DocPassword", "DocMobileNum='" + txtMob.Text.Trim() + "'").ToString() != txtPwd.Text)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Invalid Password Entered. Try Again.');", true);

                return;
            }
            else
            {
                int doc = Convert.ToInt32(c.GetReqData("DoctorsData", "DoctorID", "DocMobileNum='" + txtMob.Text + "'"));
                Session["adminDoctor"] = doc;

                Response.Redirect("dashboard.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "cmdSign_Click", ex.Message.ToString());
            return;
        }
    }

}