using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class supportteam_staff_followup_count : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            viewCount.Visible = true;
            FillGrid();
        }
    }
    private void FillGrid()
    {
        try
        {
            DateTime todayDate = DateTime.Now;
            string currentDate = todayDate.ToString();

            using (DataTable dtStaff = c.GetDataTable("Select  a.TeamID, a.TeamTaskID, a.TeamUserID, a.TeamPersonName,(Select count(FeedBkID) From FeedbackData Where FK_TeamID = a.TeamID And FeedBkTaskID = a.TeamTaskID) As OverAllCount,(Select count(FeedBkID) From FeedbackData Where FK_TeamID = a.TeamID And FeedBkTaskID = a.TeamTaskID And Cast(FeedBkDate as date) = '" + currentDate + "') As TodaysCount From SupportTeam a Where a.TeamUserStatus=0 And a.TeamAuthority <> 1"))
            {

                gvStaffFollowCount.DataSource = dtStaff;
                gvStaffFollowCount.DataBind();
                if (gvStaffFollowCount.Rows.Count > 0)
                {
                    gvStaffFollowCount.UseAccessibleHeader = true;
                    gvStaffFollowCount.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvStaffFollowCount_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litTask = (Literal)e.Row.FindControl("litTask");
                switch (e.Row.Cells[1].Text)
                {
                    case "0":
                        litTask.Text = "<div>NA</div>";
                        break;
                    case "1":
                        litTask.Text = "<div>Registered customer list</div>";
                        break;
                    case "2":
                        litTask.Text = "<div>Delivered order list</div>";
                        break;
                    case "3":
                        litTask.Text = "<div>All order list</div>";
                        break;
                    case "4":
                        litTask.Text = "<div>Lab appointment list</div>";
                        break;
                    case "5":
                        litTask.Text = "<div>Doctor appointment list</div>";
                        break;
                    case "6":
                        litTask.Text = "<div>Prescription request list</div>";
                        break;
                    case "7":
                        litTask.Text = "<div>Purchase Department</div>";
                        break;
                    case "8":
                        litTask.Text = "<div>Company Owned Shop Orders</div>";
                        break;
                }

            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvNewFollowUp_RowDataBound", ex.Message.ToString());
            return;
        }
    }
}