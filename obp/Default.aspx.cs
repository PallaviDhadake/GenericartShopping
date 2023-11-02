using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class obp_Default : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg, rootPath;
    protected void Page_Load(object sender, EventArgs e)
    {
        // Check GOBP URL Querystring and SESSION status
        if (Request.QueryString["obpid"] != null)
        {
            //Check this UserId GOBP exist or not?
           if(c.IsRecordExist("Select OBP_ID From OBPData Where OBP_UserID='" + Request.QueryString["obpid"] + "'") == true)
            {
                Session["adminObp"] = c.GetReqData("OBPData", "OBP_ID","OBP_UserID='" + Request.QueryString["obpid"] + "'");
                Response.Redirect("welcome-obp.aspx");
            }
        }

        //  ============= Check GOBP URL WebKey and SESSION status =========
        if (Request.QueryString["webkey"] != null)
        {
            //Check this UserId GOBP exist or not?
            if (c.IsRecordExist("Select OBP_ID From OBPData Where OBP_WebKey='" + Request.QueryString["webkey"] + "'") == true)
            {
                Session["adminObp"] = c.GetReqData("OBPData", "OBP_ID", "OBP_WebKey='" + Request.QueryString["webkey"] + "'");
                Response.Redirect("welcome-obp.aspx");
            }
        }

        // Check GOBP URL Querystring for WebKey request
        if (Request.QueryString["webkey"] != null)
        {
            //Check this UserId GOBP exist or not?
            if (c.IsRecordExist("Select OBP_ID From OBPData Where OBP_WebKey='" + Request.QueryString["webkey"] + "'") == true)
            {
                Session["adminObp"] = c.GetReqData("OBPData", "OBP_ID", "OBP_WebKey='" + Request.QueryString["webkey"] + "'");
                Response.Redirect("welcome-obp.aspx");
            }
        }

        cmdSign.Attributes.Add("onclick", "this.disabled=true;this.value='Processing...';" + ClientScript.GetPostBackEventReference(cmdSign, null) + ";");
        txtUserID.Focus();

        if (!IsPostBack)
        {
            if (Session["adminObp"] != null)
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
            if (!c.IsRecordExist("Select OBP_ID From OBPData Where OBP_UserID='" + txtUserID.Text + "' AND OBP_DelMark=0"))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Invalid User Id Entered, Try Again.');", true);
                return;
            }
            if (c.GetReqData("OBPData", "OBP_UserPWD", "OBP_UserID='" + txtUserID.Text.Trim() + "' AND OBP_DelMark=0").ToString() != txtPwd.Text)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Invalid Password Entered, Try Again.');", true);
                return;
            }
            else
            {
                int genmitraId = Convert.ToInt32(c.GetReqData("OBPData", "OBP_ID", "OBP_UserID='" + txtUserID.Text + "' AND OBP_DelMark=0"));
                Session["adminObp"] = genmitraId;
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