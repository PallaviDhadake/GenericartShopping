using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class districthead_dhLogin : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string encryptString = Request.QueryString["id"].ToString();

            string descryptString = c.DecryptString(encryptString);

            int sfId = Convert.ToInt32(c.GetReqData("DistrictHead", "DistHdId", "DistHdUserId='" + descryptString + "' AND DelMark=0"));
            Session["adminDH"] = sfId;

            Response.Redirect("dashboard.aspx", false);
        }
    }

      
}