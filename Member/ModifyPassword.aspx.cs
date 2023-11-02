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
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
          
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
                DataTable dt = MasterClass.Query("select auto, password, id from member where id='" + Convert.ToString(Request.Cookies["MemberValue"].Value) + "' ");
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(dt.Rows[0]["password"]) == TxtCurrentPassword.Text)
                    {
                        int i = MasterClass.NonQuery("update member set password= '" + Convert.ToString(TxtConfirmPassword.Text) + "' where id='" + Convert.ToString(Request.Cookies["MemberValue"].Value) + "'");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Password Changed Successfully')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Wrong Current Password')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Wrong Current Password')", true);
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
}
