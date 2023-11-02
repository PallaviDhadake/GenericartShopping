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
        txtShopCode.Focus();

        if (!IsPostBack)
        {
            if (Session["adminFranchisee"] != null)
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
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter ShopCode & Password.');", true);

                return;
            }
            if (!c.IsRecordExist("Select FranchID From FranchiseeData Where FranchShopCode='" + txtShopCode.Text + "'"))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Invalid ShopCode Entered, Try Again.');", true);

                return;
            }
            else if (c.GetReqData("FranchiseeData", "FranchPassword", "FranchShopCode='" + txtShopCode.Text.Trim() + "'").ToString() != txtPwd.Text)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Invalid Password Entered. Try Again.');", true);

                return;
            }
            else
            {
                int franchId = Convert.ToInt32(c.GetReqData("FranchiseeData", "FranchID", "FranchActive=1 AND FranchShopCode='" + txtShopCode.Text + "'"));
                Session["adminFranchisee"] = franchId;

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