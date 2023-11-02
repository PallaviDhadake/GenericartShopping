using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class supportteam_saving_calc_enquiry : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private void FillGrid()
    {
        try
        {
            // From Date
            DateTime enqFromDate = DateTime.Now;
            string[] arrfrDate = txtNStartDate.Text.Split('/');
            if (c.IsDate(arrfrDate[1] + "/" + arrfrDate[0] + "/" + arrfrDate[2]) == false)
            {
                errMsg = c.ErrNotification(2, "Enter Valid From Date");
                return;
            }
            else
            {
                enqFromDate = Convert.ToDateTime(arrfrDate[1] + "/" + arrfrDate[0] + "/" + arrfrDate[2]);
            }

            // To Date
            DateTime enqToDate = DateTime.Now;
            string[] arrtoDate = txtNEndDate.Text.Split('/');
            if (c.IsDate(arrtoDate[1] + "/" + arrtoDate[0] + "/" + arrtoDate[2]) == false)
            {
                errMsg = c.ErrNotification(2, "Enter Valid To Date");
                return;
            }
            else
            {
                enqToDate = Convert.ToDateTime(arrtoDate[1] + "/" + arrtoDate[0] + "/" + arrtoDate[2]);
            }

            using (DataTable dtReg = c.GetDataTable("Select a.CalcID, Convert(varchar(20), a.FollowupNextDate, 103) as flpDate, Convert(varchar(20), a.CalcDate, 103) as enqDate, b.CustomerName, b.CustomerMobile, isnull(a.DeviceType, '-') as DeviceType, " +
                    " Case When a.EnqStatus = 0 then 'Incomplete' When a.EnqStatus = 1 then 'New' When a.EnqStatus = 2 then 'Accepted & Assigned' " +
                    " When a.EnqStatus = 3 then 'Converted' When a.EnqStatus = 4 then 'Not Converted' When a.EnqStatus = 5 then 'Inprocess' " +
                    " When a.EnqStatus = 6 then 'Dispatched' When a.EnqStatus = 7 then 'Delivered' End as EnqStatus, " +
                    " Case When MreqFlag = 1 then 'Yes' Else '-' End as MonthlyEnq " +
                    " From SavingCalc a Left Join CustomersData b on a.FK_CustId = b.CustomrtID Where a.FK_CustId <> 0 AND a.FollowupStatus='Active' AND " +
                    " (CONVERT(varchar(20), a.FollowupNextDate, 112) >= CONVERT(varchar(20), CAST('" + enqFromDate + "' as datetime), 112)) AND " +
                    " (CONVERT(varchar(20), a.FollowupNextDate, 112) <= CONVERT(varchar(20), CAST('" + enqToDate + "' as datetime), 112)) Order By a.FollowupNextDate DESC"))
            {
                gvEnq.DataSource = dtReg;
                gvEnq.DataBind();

                if (gvEnq.Rows.Count > 0)
                {
                    gvEnq.UseAccessibleHeader = true;
                    gvEnq.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FllGrid", ex.Message.ToString());
            return;
        }
    }

    protected void gvEnq_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string strFlup = "";
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                strFlup = "<a href=\"enquiry-followup.aspx?id=" + e.Row.Cells[0].Text + "\" target=\"_blank\" class=\"gView\" title=\"View/Edit\"></a>";

                if (c.IsRecordExist("SELECT [CalcID] FROM [dbo].[SavingCalc] WHERE [CallBusyFlag] = 1 AND [CalcID] = " + e.Row.Cells[0].Text + " AND [CallBusyBy] <>" + Session["adminSupport"]))
                {
                    strFlup = " <span title=\"User is Currently Locked\" class=\"text-danger text-sm\"><i class=\"fa fas fa-user-lock\"></i> &nbsp; User is Currently Busy</span>";
                }

                litAnch.Text = strFlup.ToString();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvOrder_RowDataBound", ex.Message.ToString());
            return;
        }
    }


    protected void btnShow_Click(object sender, EventArgs e)
    {
        if (txtNStartDate.Text == "" && txtNEndDate.Text == "")
        {
            errMsg = c.ErrNotification(2, "Select Date Range");
            return;
        }

        FillGrid();
    }
}