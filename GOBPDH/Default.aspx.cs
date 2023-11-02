using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GOBPDH_Default : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg, rootPath;
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdSign.Attributes.Add("onclick", "this.disabled=true,this.value='Processing...';" + ClientScript.GetPostBackEventReference(cmdSign, null) + ".");
        txtUserID.Focus();

        if (!IsPostBack)
        {
            if (Session["adminGOBPDH"] != null)
            {
                Response.Redirect("dashboard.aspx");
            }
        }
    }

    protected void cmdSign_Click(object sender, EventArgs e)
    {
        try
        {
            txtUserID.Text = txtUserID.Text.Trim().Replace("'", "");
            txtPwd.Text = txtPwd.Text.Trim().Replace("'", "");

            if (txtUserID.Text == "" || txtPwd.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter User Id & Password.');", true);
                return;
            }
            if (!c.IsRecordExist("SELECT [DistHdId] FROM [dbo].[DistrictHead] WHERE [DistHdUserId] ='" + txtUserID.Text + "' AND [IsOrgDH] = 1"))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Invalid User Id Entered, Try Again.');", true);
                return;
            }

            if (!c.IsRecordExist("SELECT [DistHdId] FROM [dbo].[DistrictHead] WHERE [DistHdUserId] = '" + txtUserID.Text + "' AND [IsOrgDH] = 1 AND [DelMark] = 0"))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Your Account Has Been Suspended Or Deleted.');", true);
                return;
            }
            else if (c.GetReqData("[dbo].[DistrictHead]", "[DistHdPass]", "[DistHdUserId] = '" + txtUserID.Text.Trim() + "' AND [IsOrgDH] = 1").ToString() != txtPwd.Text)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Invalid Password Entered, Try Again.');", true);
                return;
            }
            else
            {
                int gobpId = Convert.ToInt32(c.GetReqData("[dbo].[DistrictHead]", "[DistHdId]", "[DistHdUserId] ='" + txtUserID.Text + "' AND [IsOrgDH] = 1"));
                Session["adminGOBPDH"] = gobpId;
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