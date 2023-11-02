using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class supportteam_staff_followup_new : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {

        viewNewFollowup.Visible = false;
        FillGrid();

    }

    private void FillGrid()
    {
        try
        {
            
            using (DataTable dtPackege = c.GetDataTable("Select Distinct a.CustomrtID, a.CustomerName, a.CustomerEmail, a.CustomerMobile From CustomersData a Left Join OrdersData b On a.CustomrtID=b.FK_OrderCustomerID Where a.CustomrtID Not In(Select Distinct FK_OrderCustomerID From OrdersData)"))
            {

                gvNewFollowUp.DataSource = dtPackege;
                gvNewFollowUp.DataBind();
                if (gvNewFollowUp.Rows.Count > 0)
                {
                    gvNewFollowUp.UseAccessibleHeader = true;
                    gvNewFollowUp.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvNewFollowUp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                //litAnch.Text = "<a href=\"staff-followup-form.aspx?id=" + e.Row.Cells[0].Text + "\" class=\"btn btn-sm btn-primary\">Follow Up</a>";
                if (c.IsRecordExist("Select FeedBkID From FeedbackData Where FK_CustomerID=" + e.Row.Cells[0].Text + " AND FeedBkTaskID=1"))
                {
                    litAnch.Text = "<span class=\"btn btn-sm btn-success\"><i class=\"fa fa-check\"></i> Follow Up Recored</span>";
                }
                else
                {
                    litAnch.Text = "<a href=\"staff-followup-form.aspx?type=regcust&id=" + e.Row.Cells[0].Text + "\" class=\"btn btn-sm btn-primary\" target=\"_blank\">Follow Up</a>";
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