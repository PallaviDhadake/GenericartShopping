using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class supportteam_Default : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg, rootPath;
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdSign.Attributes.Add("onclick", "this.disabled=true;this.value='Processing...';" + ClientScript.GetPostBackEventReference(cmdSign, null) + ";");
        txtUserID.Focus();

        if (!IsPostBack)
        {
            if (Session["adminGenMitra"] != null)
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
            if (!c.IsRecordExist("Select GMitraID From GenericMitra Where GMitraLogin='" + txtUserID.Text + "'"))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Invalid User Id Entered, Try Again.');", true);
                return;
            }

            int genMitraStaus = Convert.ToInt32(c.GetReqData("GenericMitra", "GMitraStatus", " GMitraLogin='" + txtUserID.Text + "'"));
            switch (genMitraStaus)
            {
                case 0:
                    break;
                case 1:
                    if (c.GetReqData("GenericMitra", "GMitraPassword", "GMitraLogin='" + txtUserID.Text.Trim() + "'").ToString() != txtPwd.Text)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Invalid Password Entered, Try Again.');", true);
                        return;
                    }
                    else
                    {
                        int genmitraId = Convert.ToInt32(c.GetReqData("GenericMitra", "GMitraID", "GMitraLogin='" + txtUserID.Text + "'"));
                        Session["adminGenMitra"] = genmitraId;
                        Response.Redirect("dashboard.aspx", false);
                    }
                    break;
                case 2:
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Your Account Has Been Suspended.');", true);
                    return;
                case 3:
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Your Account Has Been Deleted.');", true);
                    return;
            }


            //if (!c.IsRecordExist("Select GMitraID From GenericMitra Where GMitraID='" + txtUserID.Text + "' And TeamUserStatus=1"))
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Your Account Has Been Suspended Or Deleted.');", true);
            //    return;
            //}
            //else if (c.GetReqData("SupportTeam", "TeamPassword", "TeamUserID='" + txtUserID.Text.Trim() + "'").ToString() != txtPwd.Text)
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Invalid Password Entered, Try Again.');", true);
            //    return;
            //}
            //else
            //{
            //    int teamId = Convert.ToInt32(c.GetReqData("SupportTeam", "TeamID", "TeamUserID='" + txtUserID.Text + "'"));
            //    Session["adminSupport"] = teamId;
            //    Response.Redirect("dashboard.aspx", false);
            //}
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "cmdSign_Click", ex.Message.ToString());
            return;
        }
    }
}