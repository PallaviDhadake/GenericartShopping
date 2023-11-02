using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class supportteam_staff_followup_prescription_request : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            viewPrescriptionReq.Visible = false;
            FillGrid();
        }
    }
    private void FillGrid()
    {
        try
        {
            using (DataTable dtPackege = c.GetDataTable("Select PreReqID, convert(varchar(20), PreReqDate, 103) as PreReqDate, PreReqName, PreReqMobile From PrescriptionRequest"))
            {
                gvPrescriptionReq.DataSource = dtPackege;
                gvPrescriptionReq.DataBind();
                if (gvPrescriptionReq.Rows.Count > 0)
                {
                    gvPrescriptionReq.UseAccessibleHeader = true;
                    gvPrescriptionReq.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvPrescriptionReq_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"staff-followup-form.aspx?type=prereq&id=" + e.Row.Cells[0].Text + "\" class=\"btn btn-sm btn-primary\" target=\"_blank\">Follow Up</a>";

            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvPrescriptionReq_RowDataBound", ex.Message.ToString());
            return;
        }
    }
}