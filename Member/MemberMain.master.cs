using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

public partial class MemberMain : System.Web.UI.MasterPage
{
    public string MemberId = "", UserName = "", ImageShow = "", RankName = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.Cookies["MemberType"] != null && Request.Cookies["MemberValue"] != null)
            {
                if (Convert.ToString(Request.Cookies["MemberType"].Value) == "MemberPanel")
                {
                    MemberId = Convert.ToString(Request.Cookies["MemberValue"].Value);
                    ImageShow = "images/user.png";

                    DataTable dt = MasterClass.Query("select Name, id, password from member where ltrim(rtrim(id)) = '" + MemberId + "'");
                    if (dt.Rows.Count > 0)
                    {
                        if (Convert.ToString(Request.Cookies["MemberValue"].Value) == Convert.ToString(dt.Rows[0]["id"]).Trim())
                        {
                            MemberId = Convert.ToString(dt.Rows[0]["id"]);
                            UserName = Convert.ToString(dt.Rows[0]["Name"]);

                        }
                        else
                            Response.Redirect("~/Log_Out.aspx", false);
                    }
                    else
                        Response.Redirect("~/Log_Out.aspx", false);
                }
                else
                    Response.Redirect("~/Log_Out.aspx", false);
            }
            else
                Response.Redirect("~/Log_Out.aspx", false);
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Log_Out.aspx", false);
        }
    }
}
