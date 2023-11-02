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

public partial class ShopMain : System.Web.UI.MasterPage
{
    public string ShopId = "", UserName = "", StatusShow = "", FranchiseHide = "";
    public double WalletAmount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.Cookies["ShopType"] != null && Request.Cookies["ShopValue"] != null)
            {
                if ((Convert.ToString(Request.Cookies["ShopType"].Value)) == "Shop_Panel")
                {
                    ShopId = (Convert.ToString(Request.Cookies["ShopValue"].Value));

                    DataTable dt = MasterClass.Query("select Name, UserName, password, Case isnull(Status, 0) when 1 then 'Active' else 'InActive' end as StatusShow from Associate where UserName = '" + ShopId + "'");
                    if (dt.Rows.Count > 0)
                    {
                        if (ShopId == Convert.ToString(dt.Rows[0]["UserName"]))
                        {
                            ShopId = Convert.ToString(dt.Rows[0]["UserName"]);
                            UserName = Convert.ToString(dt.Rows[0]["Name"]);
                            StatusShow = Convert.ToString(dt.Rows[0]["StatusShow"]);
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
