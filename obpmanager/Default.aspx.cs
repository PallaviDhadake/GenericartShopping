using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class obpmanager_Default : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg, rootPath;
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdSign.Attributes.Add("onclick", "this.disabled=true;this.value='Processing...';" + ClientScript.GetPostBackEventReference(cmdSign, null) + ";");
        txtUserID.Focus();

        if (!IsPostBack)
        {
            if (Session["adminObpManager"] != null)
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
            if (!c.IsRecordExist("Select OBPManID From OBPManager Where OBPManUserId='" + txtUserID.Text + "' AND DelMark=0"))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Invalid User Id Entered, Try Again.');", true);
                return;
            }
            if (c.GetReqData("OBPManager", "OBPManPass", "OBPManUserId='" + txtUserID.Text.Trim() + "' AND DelMark=0").ToString() != txtPwd.Text)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Invalid Password Entered, Try Again.');", true);
                return;
            }
            else
            {
                int genmitraId = Convert.ToInt32(c.GetReqData("OBPManager", "OBPManID", "OBPManUserId='" + txtUserID.Text + "' AND DelMark=0"));
                Session["adminObpManager"] = genmitraId;


                if (Request.Cookies["MyUrlCookie"] != null)
                {
                    // Retrieve the cookie value
                    string cookieValue = Request.Cookies["MyUrlCookie"].Value;
                    // Clear the cookie
                    Response.Cookies["MyUrlCookie"].Expires = DateTime.Now.AddDays(-1);
                    Response.Redirect(cookieValue);

                }
                else
                {
                    Response.Redirect("monthly-dashboard.aspx", false);
                }
                
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