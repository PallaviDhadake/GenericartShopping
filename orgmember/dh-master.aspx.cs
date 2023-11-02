using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class orgmember_dh_master : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            FillGrid();
        }

    }

    private void FillGrid()
    {
        try
        {
            object OrgZonalmem = c.GetReqData("ZonalHead", "ZonalHdUserId", "ZonalHdId=" + Session["adminorgMember"]);

            string strQuery = "";
            //strQuery = "Select DistHdId, DistHdName, DistHdCityName, DistHdMobileNo From DistrictHead Where DelMark=0 AND OrgMemberZH='" + OrgZonalmem + "'";

            //strQuery = "Select a.DistHdId, a.DistHdName, a.DistHdCityName, a.DistHdMobileNo, (Select Count(b.OBP_ID) From OBPData Where OBP_DH_UserId = '" + OrgZonalmem + "') AS ObpCount From DistrictHead Where DelMark = 0 AND OrgMemberZH = '" + OrgZonalmem + "'";

           // strQuery = "Select DistHdId, DistHdName, DistHdCityName, DistHdMobileNo, (Select Count(OBP_ID) From OBPData Where OBP_DH_UserId = '" + OrgZonalmem + "') AS ObpCount From DistrictHead Where DelMark =0 AND OrgMemberZH = OrgZonalmem";

            strQuery = "Select a.DistHdId, a.DistHdUserId, a.DistHdName, a.DistHdCityName, a.DistHdMobileNo, a.OrgMemberZH, (Select Count(OBP_ID) From OBPData Where a.DistHdUserId = OBP_DH_UserId) As ObpCount From DistrictHead a Where a.DelMark=0 AND a.OrgMemberZH = '" + OrgZonalmem + "'";


            using (DataTable dtFrEnq = c.GetDataTable(strQuery))
            {
                gvDisHead.DataSource = dtFrEnq;
                gvDisHead.DataBind();
                if (dtFrEnq.Rows.Count > 0)
                {
                    gvDisHead.UseAccessibleHeader = true;
                    gvDisHead.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvDisHead_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                object DistUserId = c.GetReqData("DistrictHead", "DistHdUserId", "DistHdId="+e.Row.Cells[0].Text);

                Literal litView = (Literal)e.Row.FindControl("litView");
                //litView.Text = "<a href=\"gobp-master.aspx?distheadid=" + e.Row.Cells[0].Text + "\"class=\"gView\" title=\"View/Edit\"></a>";

                litView.Text = "<a href=\"gobp-master.aspx?distheadid='" + DistUserId + "'\"class=\"gView\" title=\"View/Edit\"></a>";
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvDisHead_RowDataBound", ex.Message.ToString());
            return;
        }
    }
}