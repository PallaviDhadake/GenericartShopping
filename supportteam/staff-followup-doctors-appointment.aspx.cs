using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class supportteam_staff_followup_doctors_appointment : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            viewDocAppointment.Visible = false;
            FillGrid();
        }
        
    }
    private void FillGrid()
    {
        try
        {
            using (DataTable dtPackege = c.GetDataTable("Select DocAppID, DocAppName, DocAppMobile From DoctorsAppointmentData"))
            {
                gvDocApp.DataSource = dtPackege;
                gvDocApp.DataBind();
                if (gvDocApp.Rows.Count > 0)
                {
                    gvDocApp.UseAccessibleHeader = true;
                    gvDocApp.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvDocApp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"staff-followup-form.aspx?type=docapp&id=" + e.Row.Cells[0].Text + "\" class=\"btn btn-sm btn-primary\" target=\"_blank\">Follow Up</a>";

            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvDocApp_RowDataBound", ex.Message.ToString());
            return;
        }
    }
}