using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class orgmember_gobp_master : System.Web.UI.Page
{
    iClass c = new iClass();
    public string DistrictHeadName;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //object OrgZonalmem = c.GetReqData("ZonalHead", "ZonalHdUserId", "ZonalHdId=" + Session["adminorgMember"]);

            //string DistId = c.GetReqData("DistrictHead", "DistHdUserId", "OrgMemberZH='"+ OrgZonalmem +"'").ToString();

            DistrictHeadName = c.GetReqData("DistrictHead", "DistHdName", "DistHdUserId="+ Request.QueryString["distheadid"] + "").ToString();

        }
       

        FillGrid();
    }


    private void FillGrid()
    {
        try
        {
            object OrgZonalmem = c.GetReqData("ZonalHead", "ZonalHdUserId", "ZonalHdId=" + Session["adminorgMember"]);

            object DistUserId = Request.QueryString["distheadid"].ToString();

            string strQuery = "";
            
            strQuery = "SELECT a.OBP_ID, a.OBP_UserID, a.OBP_ApplicantName, COUNT(b.CustomrtID) AS customers," +
                "(Select COUNT(b.CustomrtID) From CustomersData b Where b.FK_ObpID = a.OBP_ID AND (MONTH(b.CustomerJoinDate) = " + DateTime.Now.Month + " AND YEAR(b.CustomerJoinDate) = " + DateTime.Now.Year + "))AS MonthCustomer " +
                "FROM OBPData a LEFT OUTER JOIN CustomersData b ON a.OBP_ID = b.FK_ObpID WHERE a.OBP_DH_UserId = " + DistUserId + " GROUP BY a.OBP_ID, a.OBP_UserID, a.OBP_ApplicantName";

            using (DataTable dtFrEnq = c.GetDataTable(strQuery))
            {
                gvGobpData.DataSource = dtFrEnq;
                gvGobpData.DataBind();
                if (dtFrEnq.Rows.Count > 0)
                {
                    gvGobpData.UseAccessibleHeader = true;
                    gvGobpData.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FillGrid", ex.Message.ToString());
            return;
        }
    }

}