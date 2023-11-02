using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class official_Dashboard : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] arrCounts = new string[30];
    protected void Page_Load(object sender, EventArgs e)
    {
        GetCount();
    }

    protected void GetCount()
    {
        try
        {

            //object OrgZonalmem = c.GetReqData("ZonalHead", "ZonalHdUserId", "ZonalHdId=" + Session["adminorgMember"]);

            arrCounts[0] = c.returnAggregate("Select Count(OBP_ID) From OBPData where OBP_DelMark = 0 AND OBP_StatusFlag='Pending'").ToString();

            arrCounts[1] = c.returnAggregate("Select Count(OBP_ID) From OBPData where OBP_DelMark = 0 AND OBP_StatusFlag='Active'").ToString();

            //arrCounts[1] = c.returnAggregate("SELECT COUNT(OBP_ID) FROM OBPData WHERE OBP_DH_UserId='" + OrgZonalmem + "'").ToString();

            //arrCounts[2] = c.returnAggregate("Select Count(newsId) From NewsData where DelMark=0").ToString();

            //arrCounts[3] = c.returnAggregate("Select Count(EvntId) From EventsGallery where DelMark=0").ToString();

            //arrCounts[4] = c.returnAggregate("Select Count(ProductId) From ProductsData where DelMark=0").ToString();
            ////arrcounts[4] = c.returnaggregate("select count(hosp_reqid) from hosprequirements where hosp_reqstatus=0 and delmark=0").tostring();
            //arrCounts[5] = c.returnAggregate("select count(BannerID) from BannersData where delmark=0 ").ToString();
            ////arrcounts[6] = c.returnaggregate("select count(reqid) from requirementsdata where delmark=0").tostring();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetCount", ex.Message.ToString());
            return;

        }
    }


}