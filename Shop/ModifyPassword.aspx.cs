using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Modify : System.Web.UI.Page
{
    public string StudentId = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.Cookies["ShopValue"] != null)
            {
                if (Request.Cookies["shopValue"].Value != "")
                    StudentId = (Convert.ToString(Request.Cookies["ShopValue"].Value));
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                DataTable dt = MasterClass.Query("select auto, password, UserName from Associate where UserName = '" + StudentId + "' ");
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(dt.Rows[0]["password"]) == TxtCurrentPassword.Text)
                    {
                        int i = MasterClass.NonQuery("update Student set password = '" + Convert.ToString(TxtConfirmPassword.Text) + "' where UserName = '" + StudentId + "'");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Password Changed Successfully')", true);
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Wrong Current Password')", true);
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Wrong Current Password')", true);
            }
        }
        catch (Exception ex)
        {
        }
    }
}
