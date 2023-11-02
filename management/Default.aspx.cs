using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class management_Default : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg, rootPath;
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdSign.Attributes.Add("onclick", "this.disabled=true;this.value='Processing...';" + ClientScript.GetPostBackEventReference(cmdSign, null) + ";");
        txtShopCode.Focus();

        if (!IsPostBack)
        {
            if (Session["adminMgmt"] != null)
            {
                Response.Redirect("dashboard.aspx");
            }
        }
    }

    protected void cmdSign_Click(object sender, EventArgs e)
    {
        try
        {
            txtShopCode.Text = txtShopCode.Text.Trim().Replace("'", "");
            txtPwd.Text = txtPwd.Text.Trim().Replace("'", "");

            if (txtShopCode.Text == "" || txtPwd.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter User Id & Password.');", true);

                return;
            }
            if (!c.IsRecordExist("Select EmpId From ManagementTeam Where MgmtUserId='" + txtShopCode.Text + "'"))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Invalid User Id Entered, Try Again.');", true);

                return;
            }
            else if (c.GetReqData("ManagementTeam", "EmpPassword", "MgmtUserId='" + txtShopCode.Text.Trim() + "'").ToString() != txtPwd.Text)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Invalid Password Entered. Try Again.');", true);

                return;
            }
            else
            {
                int franchId = Convert.ToInt32(c.GetReqData("ManagementTeam", "EmpId", "MgmtUserId='" + txtShopCode.Text + "'"));
                Session["adminMgmt"] = franchId;

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