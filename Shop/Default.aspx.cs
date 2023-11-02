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

public partial class Default : System.Web.UI.Page
{
    public string ShopId = "", MemberName = "", MobileNo, EmailId, JoiningDate, PackageName, MemberAmount, FranchiseHide, FranchiseShow;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            FranchiseShow = "hidden";
            if (Request.Cookies["ShopType"] != null && Request.Cookies["ShopValue"] != null)
            {
                if ((Convert.ToString(Request.Cookies["ShopType"].Value)) == "Shop_Panel")
                {
                    ShopId = (Convert.ToString(Request.Cookies["ShopValue"].Value));

                    DataTable dt = MasterClass.Query("select UserName, password, Name, MobileNo, EmailId, convert(varchar(20), DOJ, 100) as JoiningDate from Associate where UserName = '" + ShopId + "'");
                    if (dt.Rows.Count > 0)
                    {
                        MemberName = Convert.ToString(dt.Rows[0]["Name"]);
                        MobileNo = Convert.ToString(dt.Rows[0]["MobileNo"]);
                        EmailId = Convert.ToString(dt.Rows[0]["EmailId"]);
                        JoiningDate = Convert.ToString(dt.Rows[0]["JoiningDate"]);
                    }
                    else
                        Response.Redirect("~/Default.aspx", false);
                }
                else
                    Response.Redirect("~/Default.aspx", false);
            }
            else
                Response.Redirect("~/Default.aspx", false);
        }
        catch (Exception ex)
        {
        }
    }
}
