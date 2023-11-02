using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public partial class supportteam_payment_settlement_report_daywaise : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg, apiResp;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            btnFetch.Attributes.Add("onclick", "this.disabled=true; this.value='Processing...';" + ClientScript.GetPostBackEventReference(btnFetch, null) + ";");

            if (!IsPostBack)
            {
                if (Request.QueryString["setlId"] != null)
                {
                    settleGrid.Visible = false;
                    settleCountGrid.Visible = true;

                    FillSettleGrid();
                }
                else
                {
                    settleGrid.Visible = true;
                    settleCountGrid.Visible = false;

                    FillGrid();
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    private void FillGrid()
    {
        try
        {
            using (DataTable dtSettlement = c.GetDataTable("Select SettlementID, OrderSettlementID, Convert(varchar(20), SettlementDate, 103) as SettDate," +
                " (Convert(varchar(20), SettlementDate, 103) + ' ' + (RIGHT(Convert(VARCHAR(20), SettlementDate,100),7) )) as sDate, " +
                " SettlementFee, SettlemetAmount, SettlemetTotalAmount, SettlementGST, SettlementVerify, UTRNo, SettlementCount From SettlementData"))
            {
                gvSettlement.DataSource = dtSettlement;
                gvSettlement.DataBind();

                if (dtSettlement.Rows.Count > 0)
                {
                    gvSettlement.UseAccessibleHeader = true;
                    gvSettlement.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    protected void gvSettlement_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[10].Text = "<a href=\"payment-settlement-report-daywaise.aspx?setlId=" + e.Row.Cells[4].Text + "\" target=\"_blank\" class=\"badge badge-pill badge-info\">" + e.Row.Cells[10].Text + "</a>";

                Button btnVerify = (Button)e.Row.FindControl("cmdVerify");
                if (e.Row.Cells[1].Text == "1")
                {
                    btnVerify.Enabled = false;
                    btnVerify.Text = "Verified";
                    btnVerify.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }


    private void FillSettleGrid()
    {
        try
        {
            using (DataTable dtSettlement = c.GetDataTable("Select OPL_id, OLP_amount, Convert(varchar(20), OPL_datetime, 103) as opDate, " +
                " (Convert(varchar(20), OPL_datetime, 103) + ' ' + (RIGHT(Convert(VARCHAR(20), OPL_datetime,100),7) )) as oplDate, " +
                " OPL_merchantTranId, OPL_transtatus, OLP_order_no, OLP_RazorPayFee, OLP_RazorPayGST, OLP_RazorPayAmount, " +
                " OLP_device_type From online_payment_logs Where OLP_SettlementID='" + Request.QueryString["setlId"] + "'"))
            {
                gvSettlecount.DataSource = dtSettlement;
                gvSettlecount.DataBind();

                if (dtSettlement.Rows.Count > 0)
                {
                    gvSettlecount.UseAccessibleHeader = true;
                    gvSettlecount.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    protected void btnFetch_Click(object sender, EventArgs e)
    {
        try
        {
            string settlementStatus = c.GetSettlementStatus().ToString();

            string statusInfo = "", cMsg = "";
            var OrderResponses = JsonConvert.DeserializeObject<OrderResponse>(settlementStatus);
            statusInfo = OrderResponses.status;
            cMsg = OrderResponses.messages;

            if (statusInfo == "True")
            {
                apiResp = c.ErrNotification(1, "Status : " + statusInfo.ToString() + ", Msg : " + cMsg.ToString());
            }
            else
            {
                apiResp = c.ErrNotification(2, "Status : " + statusInfo.ToString() + ", Msg : " + cMsg.ToString());
            }

            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('payment-settlement-report-daywaise.aspx', 1000);", true);
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}