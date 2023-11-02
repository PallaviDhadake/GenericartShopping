using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class orgmember_Default : System.Web.UI.Page
{
    iClass c = new iClass();
    public string rootPath, errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdSign.Attributes.Add("onclick", "this.disabled=true;this.value='Processing...';" + ClientScript.GetPostBackEventReference(cmdSign, null) + ";");
        txtUserName.Focus();

        if (!IsPostBack)
        {
            if (Session["adminorgMember"] != null)
            {
                Response.Redirect("dashboard.aspx");
            }
        }
    }

    protected void cmdSign_Click(object sender, EventArgs e)
    {
        try
        {
            txtUserName.Text = txtUserName.Text.Trim().Replace("'", "");
            txtPwd.Text = txtPwd.Text.Trim().Replace("'", "");

            if (txtUserName.Text == "" || txtPwd.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter User Id & Password.');", true);
                return;
            }
            if (!c.IsRecordExist("Select ZonalHdId From ZonalHead Where ZonalHdUserId='" + txtUserName.Text + "'"))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Invalid User Id Entered, Try Again.');", true);
                return;
            }
            if (c.GetReqData("ZonalHead", "ZonalHdPass", "ZonalHdUserId='" + txtUserName.Text.Trim() + "'").ToString() != txtPwd.Text)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Invalid Password Entered. Try Again.');", true);
                return;
            }

            object isOrgMem = c.GetReqData("ZonalHead", "IsOrgMember", "ZonalHdUserId='" + txtUserName.Text + "'");

            if(isOrgMem != DBNull.Value && isOrgMem != null && isOrgMem.ToString() != "")
            {

                if (isOrgMem.ToString() == "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Invalid User. Try Again.');", true);
                    return;
                }
                else
                {
                    int zonalhdid = Convert.ToInt32(c.GetReqData("ZonalHead", "ZonalHdId", "ZonalHdUserId='" + txtUserName.Text + "'"));
                    Session["adminorgMember"] = zonalhdid;
                    Response.Redirect("dashboard.aspx", false);
                }

            }
            else
            {

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