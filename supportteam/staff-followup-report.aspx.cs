using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class supportteam_staff_followup_report : System.Web.UI.Page
{
    iClass c = new iClass();
    public string pageLink, pageName;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            viewFeedback.Visible = true;
            FillGrid();
        }
    }
    private void FillGrid()
    {
        try
        {
            int teamId = Convert.ToInt32(Session["adminSupport"]);
            int taskId = Convert.ToInt32(c.GetReqData("SupportTeam", "TeamTaskID", "TeamID="+ teamId +""));
            string strQuery = "";
            switch (taskId)
            {
                case 1:
                    pageLink = "staff-followup-new.aspx";
                    pageName = "Registered Customer Follow Up";
                    strQuery = "Select a.FeedBkID, convert(varchar(20), a.FeedBkDate, 103) as FeedBkDate, a.FeedBkTaskID, a.FeedBkRating, b.CustomerName From FeedbackData a Inner Join CustomersData b On a.FK_CustomerID=b.CustomrtID Where FK_TeamID=" + teamId + " And FeedBkTaskID=" + taskId + "";
                    break;
                case 2:
                    pageLink = "staff-followup-delivered-order.aspx";
                    pageName = "Delivered Orders Follow Up";
                    strQuery = "Select a.FeedBkID, convert(varchar(20), a.FeedBkDate, 103) as FeedBkDate, a.FeedBkTaskID, a.FeedBkRating, b.CustomerName From FeedbackData a Inner Join CustomersData b On a.FK_CustomerID=b.CustomrtID Where FK_TeamID=" + teamId + " And FeedBkTaskID=" + taskId + "";
                    break;
                case 3:
                    pageLink = "staff-followup-all-orders.aspx";
                    pageName = "All Orders Follow Up";
                    strQuery = "Select a.FeedBkID, convert(varchar(20), a.FeedBkDate, 103) as FeedBkDate, a.FeedBkTaskID, a.FeedBkRating, b.CustomerName From FeedbackData a Inner Join CustomersData b On a.FK_CustomerID=b.CustomrtID Where FK_TeamID=" + teamId + " And FeedBkTaskID=" + taskId + "";
                    break;
                case 4:
                    pageLink = "staff-followup-lab-appointment.aspx";
                    pageName = "Lab Appointment Follow Up";
                    strQuery = "Select a.FeedBkID, convert(varchar(20), a.FeedBkDate, 103) as FeedBkDate, a.FeedBkTaskID, a.FeedBkRating, b.LabAppName As CustomerName From FeedbackData a Inner Join LabAppointments b On a.FeedBkTransID=b.LabAppID Where FK_TeamID=" + teamId + " And FeedBkTaskID=" + taskId + "";
                    break;
                case 5:
                    pageLink = "staff-followup-doctors-appointment.aspx";
                    pageName = "Doctor Appointment Follow Up";
                    strQuery = "Select a.FeedBkID, convert(varchar(20), a.FeedBkDate, 103) as FeedBkDate, a.FeedBkTaskID, a.FeedBkRating, b.DocAppName As CustomerName From FeedbackData a Inner Join DoctorsAppointmentData b On a.FeedBkTransID=b.DocAppID Where FK_TeamID=" + teamId + " And FeedBkTaskID=" + taskId + "";
                    break;
                case 6:
                    pageLink = "staff-followup-prescription-request.aspx";
                    pageName = "Prescription Request Follow Up";
                    strQuery = "Select a.FeedBkID, convert(varchar(20), a.FeedBkDate, 103) as FeedBkDate, a.FeedBkTaskID, a.FeedBkRating, b.PreReqName As CustomerName From FeedbackData a Inner Join PrescriptionRequest b On a.FeedBkTransID=b.PreReqID Where FK_TeamID=" + teamId + " And FeedBkTaskID=" + taskId + "";
                    break;
                case 8:
                    pageLink = "staff-followup-comp-owned-shoporder.aspx";
                    pageName = "Company Owned Shop Orders Follow Up";
                    strQuery = "Select a.FeedBkID, convert(varchar(20), a.FeedBkDate, 103) as FeedBkDate, a.FeedBkTaskID, a.FeedBkRating, b.CustomerName From FeedbackData a Inner Join CustomersData b On a.FK_CustomerID=b.CustomrtID Where FK_TeamID=" + teamId + " And FeedBkTaskID=" + taskId + "";
                    break;

            }

            using (DataTable dtPackege = c.GetDataTable(strQuery))
            {

                gvFeedback.DataSource = dtPackege;
                gvFeedback.DataBind();
                if (gvFeedback.Rows.Count > 0)
                {
                    gvFeedback.UseAccessibleHeader = true;
                    gvFeedback.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvFeedback_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"staff-followup-form.aspx?action=edit&feedbackId=" + e.Row.Cells[0].Text + "\" class=\"gAnch\" title=\"View / Edit\" target=\"_blank\"></a>";

                Literal litTaskId = (Literal)e.Row.FindControl("litTaskId");
                int teamId = Convert.ToInt32(Session["adminSupport"]);
                int taskId = Convert.ToInt32(c.GetReqData("SupportTeam", "TeamTaskID", "TeamID=" + teamId + ""));
                switch (taskId)
                {
                    case 1:
                        litTaskId.Text = "Registered customer";
                        break;
                    case 2:
                        litTaskId.Text = "Delivered order";
                        break;
                    case 3:
                        litTaskId.Text = "All order";
                        break;
                    case 4:
                        litTaskId.Text = "Lab appointment";
                        break;
                    case 5:
                        litTaskId.Text = "Doctor appointment";
                        break;
                    case 6:
                        litTaskId.Text = "Prescription request";
                        break;
                    case 8:
                        litTaskId.Text = "Company Owned Shop Orders";
                        break;


                }

                Literal litRatings = (Literal)e.Row.FindControl("litRatings");
                int rating = Convert.ToInt32(e.Row.Cells[1].Text);
                switch (rating)
                {
                    case 1:
                        litRatings.Text = "Excellent";
                        break;
                    case 2:
                        litRatings.Text = "Very Good";
                        break;
                    case 3:
                        litRatings.Text = "Good";
                        break;
                    case 4:
                        litRatings.Text = "Fair";
                        break;
                    case 5:
                        litRatings.Text = "Poor";
                        break;
                    case 6:
                        litRatings.Text = "Very Poor";
                        break;

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvFeedback_RowDataBound", ex.Message.ToString());
            return;
        }
    }
}