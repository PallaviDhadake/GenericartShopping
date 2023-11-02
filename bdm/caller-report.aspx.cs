using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class bdm_caller_report : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] arrCall = new string[5];
    public string[] arrCounts = new string[15];
    public string errMsg, callCounts, callFollowup, callEnquiry;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
        if (Request.QueryString["id"] != null)
        {
            viewCall.Visible = false;
            ViewFollowup.Visible = true;
            GetCustInfo();
            FillGridFollowup();
            FillGridFollowupEnq();
            GetCounts();
        }
        else
        {
            viewCall.Visible = true;
            ViewFollowup.Visible = false;
            FillGrid();
        }
    }

    private void FillGrid()
    {
        try
        {
            string strQuery = "";
            if (txtDate.Text != "")
            {
                // From Date
                DateTime fromDate;
                string[] arrFromDate = txtDate.Text.Split('/');
                fromDate = Convert.ToDateTime(arrFromDate[1] + "/" + arrFromDate[0] + "/" + arrFromDate[2]);

                strQuery = "Select Distinct MIN(s.TeamID) as TeamID, s.TeamPersonName, s.TeamMobile, " +
                           "(Select COUNT(FlupId) From FollowupOrders Where FK_TeamMemberId = MIN(f.FK_TeamMemberId) AND CONVERT(varchar(20), FlupDate, 103) = CONVERT(varchar(20), CAST('" + fromDate + "' as DATETIME), 103)) as TotalFollowup, " +
                           "(Select COUNT(FlupEnqId) From FollowupEnquires Where FK_TeamMemberId = MIN(f.FK_TeamMemberID) AND CONVERT(varchar(20), FlupEnqDate, 103) = CONVERT(varchar(20), CAST('" + fromDate+ "' as DATETIME), 103)) as TotalEnquiry, " +
                           "((Select COUNT(FlupId) From FollowupOrders Where FK_TeamMemberId = MIN(f.FK_TeamMemberId) AND CONVERT(varchar(20), FlupDate, 103) = CONVERT(varchar(20), CAST('" + fromDate + "' as DATETIME), 103)) + (Select COUNT(FlupEnqId) From FollowupEnquires Where FK_TeamMemberId = MIN(f.FK_TeamMemberID) AND CONVERT(varchar(20), FlupEnqDate, 103) = CONVERT(varchar(20), CAST('" + fromDate + "' as DATETIME), 103))) as TotalCalls" +
                           " From FollowupOrders as f Inner Join SupportTeam as s on f.[FK_TeamMemberId] = s.TeamID " +
                           " Where CONVERT(varchar(20), FlupDate, 103) = CONVERT(varchar(20), CAST('" + fromDate + "' as DATETIME), 103) Group By s.TeamPersonName, s.TeamMobile Order By TotalCalls DESC";

                callFollowup = c.returnAggregate("Select COUNT(FlupId) From FollowupOrders Where CONVERT(varchar(20), FlupDate, 103) = CONVERT(varchar(20), CAST('" + fromDate + "' as DATETIME), 103)").ToString();
                callEnquiry = c.returnAggregate("Select COUNT(FlupEnqId) From FollowupEnquires Where CONVERT(varchar(20), FlupEnqDate, 103) = CONVERT(varchar(20), CAST('" + fromDate + "' as DATETIME), 103)").ToString();
                callCounts = (Convert.ToInt32(callFollowup)  + Convert.ToInt32(callEnquiry)).ToString();
            }
            else
            {
                //txtDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");

                strQuery = "Select Distinct MIN(s.TeamID) as TeamID, s.TeamPersonName, s.TeamMobile, " +
                           "(Select COUNT(FlupId) From FollowupOrders Where FK_TeamMemberId = MIN(f.FK_TeamMemberId) AND CONVERT(varchar(20), FlupDate, 103) = CONVERT(varchar(20), CAST('" + DateTime.Now + "' as DATETIME), 103)) as TotalFollowup, " +
                           "(Select COUNT(FlupEnqId) From FollowupEnquires Where FK_TeamMemberId = MIN(f.FK_TeamMemberID) AND CONVERT(varchar(20), FlupEnqDate, 103) = CONVERT(varchar(20), CAST('" + DateTime.Now + "' as DATETIME), 103)) as TotalEnquiry, " +
                           "((Select COUNT(FlupId) From FollowupOrders Where FK_TeamMemberId = MIN(f.FK_TeamMemberId) AND CONVERT(varchar(20), FlupDate, 103) = CONVERT(varchar(20), CAST('" + DateTime.Now + "' as DATETIME), 103)) + (Select COUNT(FlupEnqId) From FollowupEnquires Where FK_TeamMemberId = MIN(f.FK_TeamMemberID) AND CONVERT(varchar(20), FlupEnqDate, 103) = CONVERT(varchar(20), CAST('" + DateTime.Now + "' as DATETIME), 103))) as TotalCalls" +
                           " From FollowupOrders as f Inner Join SupportTeam as s on f.[FK_TeamMemberId] = s.TeamID " +
                           " Where CONVERT(varchar(20), FlupDate, 103) = CONVERT(varchar(20), CAST('" + DateTime.Now + "' as DATETIME), 103) Group By s.TeamPersonName, s.TeamMobile Order By TotalCalls DESC";

                callFollowup = c.returnAggregate("Select COUNT(FlupID) From FollowupOrders Where CONVERT(varchar(20), FlupDate, 103) = CONVERT(varchar(20), CAST('" + DateTime.Now + "' as DATETIME), 103)").ToString();
                callEnquiry = c.returnAggregate("Select COUNT(FlupEnqId) From FollowupEnquires Where CONVERT(varchar(20), FlupEnqDate, 103) = CONVERT(varchar(20), CAST('" + DateTime.Now + "' as DATETIME), 103)").ToString();
                callCounts = (Convert.ToInt32(callFollowup) + Convert.ToInt32(callEnquiry)).ToString();
                //callCounts = c.returnAggregate("Select COUNT(FlupId) From FollowupOrders  Where CONVERT(varchar(20), FlupDate, 103) = CONVERT(varchar(20), CAST('" + DateTime.Now + "' as DATETIME), 103)").ToString();
            }

            using (DataTable dtCallData = c.GetDataTable(strQuery))
            {
                gvCall.DataSource = dtCallData;
                gvCall.DataBind();

                if (gvCall.Rows.Count > 0)
                {
                    gvCall.UseAccessibleHeader = true;
                    gvCall.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
        }
        catch(Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FillGrid", ex.Message.ToString());
            return;
        }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            // From Date
            DateTime fromDate = DateTime.Now;
            string[] arrFromDate = txtDate.Text.Split('/');
            if (c.IsDate(arrFromDate[1] + "/" + arrFromDate[0] + "/" + arrFromDate[2]) == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid Date');", true);
                return;
            }

            FillGrid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSave_Click", ex.Message.ToString());
            return;
        }
    }

    protected void gvCall_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                if (txtDate.Text != "")
                {
                    // From Date
                    DateTime fromDate;
                    string[] arrFromDate = txtDate.Text.Split('/');
                    fromDate = Convert.ToDateTime(arrFromDate[1] + "/" + arrFromDate[0] + "/" + arrFromDate[2]);

                    litAnch.Text = "<a href=\"caller-report.aspx?id=" + e.Row.Cells[0].Text + "&date=" + fromDate + "\" target=\"_blank\" class=\"frStats btn btn-sm btn-default\" data-fancybox-type=\"iframe\"><i class=\"fa fa-eye\" aria-hidden=\"true\"></i></a>";
                }
                else
                {
                    litAnch.Text = "<a href=\"caller-report.aspx?id=" + e.Row.Cells[0].Text + "&date=" + DateTime.Now + "\" target=\"_blank\" class=\"frStats btn btn-sm btn-default\" data-fancybox-type=\"iframe\"><i class=\"fa fa-eye\" aria-hidden=\"true\"></i></a>";
                }

                double column2Value = Convert.ToDouble(decimal.Parse(DataBinder.Eval(e.Row.DataItem, "TotalCalls").ToString()));
                //double total = column2Value + ;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvCall_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    private void FillGridFollowup()
    {
        try
        {
            string strQuery;

            DateTime Date;
            Date = Convert.ToDateTime(Request.QueryString["date"].ToString());

            strQuery = "Select f.FlupID, LTRIM(RIGHT(CONVERT(VARCHAR(20), f.FlupDate, 100), 7)) AS FlupTime, c.CustomerName, f.FlupRemarkStatus, f.FlupRemark," +
                       " CONVERT(Varchar(20), f.FlupNextDate, 103) AS FlupNextDate, f.FlupNextTime" +
                       " From FollowupOrders as f" +
                       " Inner Join CustomersData as c on f.[FK_CustomerId] = c.CustomrtID" +
                       " Where f.FK_TeamMemberId = '" + Request.QueryString["id"].ToString() + "' AND CONVERT(VARCHAR(20),f.FlupDate,112) = CONVERT(VARCHAR(20),CAST('" + Date + "' AS DATE),112)" +
                       " Order By CAST(FlupNextDate AS DATE) Asc";

            using (DataTable dtFlup = c.GetDataTable(strQuery))
            {
                gvFlup.DataSource = dtFlup;
                gvFlup.DataBind();
                if (gvFlup.Rows.Count > 0)
                {
                    gvFlup.UseAccessibleHeader = true;
                    gvFlup.HeaderRow.TableSection = TableRowSection.TableHeader;
                }

            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    private void FillGridFollowupEnq()
    {
        try
        {
            string strQuery;

            DateTime Date;
            Date = Convert.ToDateTime(Request.QueryString["date"].ToString());

            strQuery = "Select f.FlupEnqID, LTRIM(RIGHT(CONVERT(VARCHAR(20), f.FlupEnqDate, 100), 7)) AS FlupTime, c.CustomerName, f.FlupEnqRemarkStatus, f.FlupEnqRemark," +
                       " CONVERT(Varchar(20), f.FlupEnqNextDate, 103) AS FlupEnqNextDate, f.FlupEnqNextTime" +
                       " From FollowupEnquires as f" +
                       " Inner Join CustomersData as c on f.[FK_CustomerId] = c.CustomrtID" +
                       " Where f.FK_TeamMemberId = '" + Request.QueryString["id"].ToString() + "' AND CONVERT(VARCHAR(20),f.FlupEnqDate,112) = CONVERT(VARCHAR(20),CAST('" + Date + "' AS DATE),112)" +
                       " Order By CAST(FlupEnqNextDate AS DATE) Asc";

            using (DataTable dtEnqFlup = c.GetDataTable(strQuery))
            {
                gvEnqFlup.DataSource = dtEnqFlup;
                gvEnqFlup.DataBind();
                if (gvEnqFlup.Rows.Count > 0)
                {
                    gvEnqFlup.UseAccessibleHeader = true;
                    gvEnqFlup.HeaderRow.TableSection = TableRowSection.TableHeader;
                }

            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    private void GetCustInfo()
    {
        try
        {
            using (DataTable dtRefInfo = c.GetDataTable("Select s.TeamID, s.TeamPersonName, s.TeamMobile," +
                                                        " CONVERT(Varchar(20), f.FlupDate, 103) AS Date " +
                                                        " From SupportTeam as s" +
                                                        " Inner Join FollowupOrders as f on s.TeamID = f.FK_TeamMemberId" +
                                                        " Where s.TeamID = '" + Request.QueryString["id"].ToString() + "' AND CONVERT(VARCHAR(20),f.FlupDate,112) = CONVERT(VARCHAR(20),CAST('" + Request.QueryString["date"].ToString() + "' AS DATE),112)"))
            {
                if (dtRefInfo.Rows.Count > 0)
                {
                    DataRow row = dtRefInfo.Rows[0];

                    lblId.Text = Request.QueryString["id"].ToString();
                    lblDate.Text = row["Date"].ToString();
                    arrCall[0] = row["TeamPersonName"].ToString();
                    arrCall[1] = row["TeamMobile"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetCustInfo", ex.Message.ToString());
            return;
        }
    }

    private void GetCounts()
    {
        try
        {
            int Teamid = Convert.ToInt32(Request.QueryString["id"].ToString());
            arrCounts[0] = c.GetReqData("FollowupOrders", "Count(FlupID)", "FlupRemarkStatusID=1 AND FK_TeamMemberId = '" + Teamid + "' AND CONVERT(VARCHAR(20),FlupDate,112) = CONVERT(VARCHAR(20),CAST('" + Request.QueryString["date"].ToString() + "' AS DATE),112)").ToString();
            arrCounts[1] = c.GetReqData("FollowupEnquires", "Count(FlupEnqID)", "FlupEnqRemarkStatusID=1 AND FK_TeamMemberId = '" + Teamid + "' AND CONVERT(VARCHAR(20),FlupEnqDate,112) = CONVERT(VARCHAR(20),CAST('" + Request.QueryString["date"].ToString() + "' AS DATE),112)").ToString();
            arrCounts[2] = (Convert.ToInt32(arrCounts[0]) + Convert.ToInt32(arrCounts[1])).ToString();
            arrCounts[3] = c.GetReqData("FollowupOrders", "Count(FlupID)", "FlupRemarkStatusID IN (2,3,4,5,6,7) AND FK_TeamMemberId = '" + Teamid + "' AND CONVERT(VARCHAR(20),FlupDate,112) = CONVERT(VARCHAR(20),CAST('" + Request.QueryString["date"].ToString() + "' AS DATE),112)").ToString();
            arrCounts[4] = c.GetReqData("FollowupEnquires", "Count(FlupEnqID)", "FlupEnqRemarkStatusID IN(2,3,4,5,6,7) AND FK_TeamMemberId = '" + Teamid + "' AND CONVERT(VARCHAR(20),FlupEnqDate,112) = CONVERT(VARCHAR(20),CAST('" + Request.QueryString["date"].ToString() + "' AS DATE),112)").ToString();
            arrCounts[5] = (Convert.ToInt32(arrCounts[3]) + Convert.ToInt32(arrCounts[4])).ToString();
            arrCounts[6] = c.GetReqData("FollowupOrders", "Count(FlupID)", "FlupRemarkStatusID IN (3,6) AND FK_TeamMemberId = '" + Teamid + "' AND CONVERT(VARCHAR(20),FlupDate,112) = CONVERT(VARCHAR(20),CAST('" + Request.QueryString["date"].ToString() + "' AS DATE),112)").ToString();
            arrCounts[7] = c.GetReqData("FollowupEnquires", "Count(FlupEnqID)", "FlupEnqRemarkStatusID IN (3,6) AND FK_TeamMemberID = '" + Teamid + "' AND CONVERT(VARCHAR(20),FlupEnqDate,112) = CONVERT(VARCHAR(20),CAST('" + Request.QueryString["date"].ToString() + "' AS DATE),112)").ToString();
            arrCounts[8] = (Convert.ToInt32(arrCounts[6]) + Convert.ToInt32(arrCounts[7])).ToString();
            arrCounts[9] = c.GetReqData("FollowupOrders", "Count(FlupID)", "FlupRemarkStatusID=7 AND FK_TeamMemberId = '" + Teamid + "' AND CONVERT(VARCHAR(20),FlupDate,112) = CONVERT(VARCHAR(20),CAST('" + Request.QueryString["date"].ToString() + "' AS DATE),112)").ToString();
            arrCounts[10] = c.GetReqData("FollowupEnquires", "Count(FlupEnqID)", "FlupEnqRemarkStatusID=7 AND FK_TeamMemberID = '" + Teamid + "' AND CONVERT(VARCHAR(20),FlupEnqDate,112) = CONVERT(VARCHAR(20),CAST('" + Request.QueryString["date"].ToString() + "' AS DATE),112)").ToString();
            arrCounts[11] = (Convert.ToInt32(arrCounts[9]) + Convert.ToInt32(arrCounts[10])).ToString();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Error Occoured while processing', type: 'error'});", true);
            c.ErrorLogHandler(this.ToString(), "GetCounts", ex.Message.ToString());
            return;
        }
    }
}