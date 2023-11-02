using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Text;

public partial class franchisee_received_prescriptions : System.Web.UI.Page
{
    iClass c = new iClass();
    public static string[] rxData = new string[10];
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
            string strQuery = "";

            if (Request.QueryString["type"] != null)
            {
                switch (Request.QueryString["type"])
                {
                    case "new": strQuery = "Select a.PrescFwdID, Convert(varchar(20), a.PrescFwdDate, 103) as frwdDate, a.PrescImg, a.FK_PreReqID, a.PrescFwdStatus, b.DocName, c.PreReqName From PrescriptionForword a Inner Join DoctorsData b On a.FK_DoctorID=b.DoctorID Inner Join PrescriptionRequest c On a.FK_PreReqID=c.PreReqID Where a.PrescFwdStatus=0 AND a.FK_FranchID=" + Session["adminFranchisee"]; break;
                }
            }
            else
            {
                strQuery = "Select a.PrescFwdID, Convert(varchar(20), a.PrescFwdDate, 103) as frwdDate, a.PrescImg, a.FK_PreReqID, a.PrescFwdStatus, b.DocName, c.PreReqName From PrescriptionForword a Inner Join DoctorsData b On a.FK_DoctorID=b.DoctorID Inner Join PrescriptionRequest c On a.FK_PreReqID=c.PreReqID Where a.FK_FranchID=" + Session["adminFranchisee"];
            }
            using (DataTable dtRx = c.GetDataTable(strQuery))
            {
                gvRx.DataSource = dtRx;
                gvRx.DataBind();

                if (dtRx.Rows.Count > 0)
                {
                    gvRx.UseAccessibleHeader = true;
                    gvRx.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvRx_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litDownloadRx = (Literal)e.Row.FindControl("litDownloadRx");
                litDownloadRx.Text = "<a href=\"" + Master.rootPath + "upload/docRx/" + e.Row.Cells[2].Text + "\" target=\"_blank\" class=\"btn btn-sm btn-success\"><i class=\"fas fa-download\"></i> Download Prescription</a>";

                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                switch (e.Row.Cells[1].Text)
                {
                    case "0": litStatus.Text = "<span class=\"ordProcessing\">Pending</span>"; break;
                    case "1": litStatus.Text = "<span class=\"ordAccepted\">Completed</span>"; break;
                    case "2": litStatus.Text = "<span class=\"ordDenied\">Rejected</span>"; break;
                }

                e.Row.Cells[6].Text = "<button type=\"button\" class=\"btn btn-sm btn-primary\" data-toggle=\"modal\" data-target=\"#modal-lg\" onclick=\"GetCustDetails(" + e.Row.Cells[3].Text + ");\"><i class=\"fas fa-user\"></i> " + e.Row.Cells[6].Text + "</button>";
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvRx_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    [WebMethod]
    public static string GetRxCustInfo(string preReqId)
    {
        iClass c = new iClass();
        StringBuilder strMarkup = new StringBuilder();
        using (DataTable dtRx = c.GetDataTable("Select PreReqID, PreReqDate, PreReqName, PreReqAge, PreReqGender, PreReqMobile, PreReqDisease, PreReqAddr From PrescriptionRequest Where PreReqID=" + preReqId))
        {
            if (dtRx.Rows.Count > 0)
            {
                DataRow row = dtRx.Rows[0];

                strMarkup.Append("<table class=\"form_table\">");
                strMarkup.Append("<tr>");
                strMarkup.Append("<td><span class=\"formLable bold_weight\">Id :</span></td>");
                strMarkup.Append("<td><span class=\"formLable\">" + row["PreReqID"].ToString() + "</span> </td>");
                strMarkup.Append("</tr>");

                strMarkup.Append("<tr>");
                strMarkup.Append("<td style=\"width: 40%;\"><span class=\"formLable bold_weight\">Prescription Request Date :</span></td>");
                strMarkup.Append("<td style=\"width: 60%;\"><span class=\"formLable\">" + Convert.ToDateTime(row["PreReqDate"]).ToString("dd/MM/yyyy") + "</span> </td>");
                strMarkup.Append("</tr>");

                strMarkup.Append("<tr>");
                strMarkup.Append("<td><span class=\"formLable bold_weight\">Name :</span></td>");
                strMarkup.Append("<td><span class=\"formLable\">" + row["PreReqName"].ToString() + "</span> </td>");
                strMarkup.Append("</tr>");

                strMarkup.Append("<tr>");
                strMarkup.Append("<td><span class=\"formLable bold_weight\">Mobile No. :</span></td>");
                strMarkup.Append("<td><span class=\"formLable\">" + row["PreReqMobile"].ToString() + "</span> </td>");
                strMarkup.Append("</tr>");

                strMarkup.Append("<tr>");
                strMarkup.Append("<td><span class=\"formLable bold_weight\">Age :</span></td>");
                strMarkup.Append("<td><span class=\"formLable\">" + row["PreReqAge"].ToString() + "</span> </td>");
                strMarkup.Append("</tr>");

                strMarkup.Append("<tr>");
                string gender = row["PreReqGender"].ToString() == "1" ? "Male" : "Female";
                strMarkup.Append("<td><span class=\"formLable bold_weight\">Gender :</span></td>");
                strMarkup.Append("<td><span class=\"formLable\">" + gender + "</span> </td>");
                strMarkup.Append("</tr>");

                strMarkup.Append("<tr>");
                strMarkup.Append("<td><span class=\"formLable bold_weight\">Address :</span></td>");
                strMarkup.Append("<td><span class=\"formLable\">" + row["PreReqAddr"].ToString() + "</span> </td>");
                strMarkup.Append("</tr>");

                strMarkup.Append("<tr>");
                strMarkup.Append("<td><span class=\"formLable bold_weight\">Disease :</span></td>");
                strMarkup.Append("<td><span class=\"formLable\">" + row["PreReqDisease"].ToString() + "</span> </td>");
                strMarkup.Append("</tr>");

                strMarkup.Append("</table>");

                if (!c.IsRecordExist("Select PrescFwdID From PrescriptionForword Where FK_PreReqID=" + row["PreReqID"].ToString() + " AND PrescFwdStatus=1"))
                {
                    strMarkup.Append("<span class=\"space20\"></span>");
                    strMarkup.Append("<button type=\"button\" id=\"completeOrd\" class=\"btn btn-md btn-success\" onclick=\"MarkAsComplete(" + row["PreReqID"].ToString() + ")\">Mark as Complete Order</button>");
                }
            }
        }

        return strMarkup.ToString();
    }

    [WebMethod]
    public static string CompleteOrder(string presReqId)
    {
        iClass c = new iClass();
        //adminFranchisee
        c.ExecuteQuery("Update PrescriptionForword Set PrescFwdStatus=1 Where FK_PreReqID=" + presReqId + " AND FK_FranchID=" + HttpContext.Current.Session["adminFranchisee"]);
        return presReqId;
    }
}