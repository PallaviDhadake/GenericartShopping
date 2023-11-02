using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class districthead_dashboard : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] arrCounts = new string[15]; //9
    protected void Page_Load(object sender, EventArgs e)
    {
        GetCount();
    }

    private void GetCount()
    {
        try
        {
            string DhUserId = (c.GetReqData("DistrictHead", "DistHdUserId", "DistHdId=" + Session["adminDH"] + "").ToString());


            arrCounts[0] = c.returnAggregate("Select count(OBP_ID) From OBPData Where OBP_DH_UserId='" + DhUserId + "'").ToString();

            //arrCounts[1] = c.returnAggregate("Select Count(ProductID) From ProductsData Where delMark=0 AND ProductActive=1").ToString();
        }
        catch(Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetCount", ex.Message.ToString());
            return;
        }
    }
}