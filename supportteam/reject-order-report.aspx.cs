using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Razorpay.Api;
using System.Activities.Expressions;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

public partial class supportteam_reject_order_report : System.Web.UI.Page
{
    iClass c = new iClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                string dateRange = c.GetFinancialYear();
                string[] arrDateRange = dateRange.ToString().Split('#');
                DateTime myFromDate = Convert.ToDateTime(arrDateRange[0]);
                DateTime myToDate = Convert.ToDateTime(arrDateRange[1]);
                litDate.Text = "(" + myFromDate.ToString("dd/MM/yyyy") + " - " + DateTime.Now.ToString("dd/MM/yyyy") + ")";
                FillGrid();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
                c.ErrorLogHandler(this.ToString(), "Page_Load", ex.Message.ToString());
                return;
            }
        }
    }

    private void FillGrid()
    {
        try
        {
            string strQuery = "";
            string dateRange = c.GetFinancialYear();
            string[] arrDateRange = dateRange.ToString().Split('#');
            DateTime myFromDate = Convert.ToDateTime(arrDateRange[0]);
            DateTime myToDate = Convert.ToDateTime(arrDateRange[1]);

            strQuery = @"SELECT 
                            a.[FK_OrderCustomerID] AS FK_OrderCustomerID, 
                            a.[OrderID] AS OrderID,
                            MAX(a.[OrderStatus]) AS OrderStatus, 
                            MAX(CONVERT(varchar(20), a.[FollowupNextDate], 103)) AS FollowupNextDate,
                            MAX(DATEDIFF(DAY, a.[FollowupLastDate], GETDATE())) AS DateDiff,
                            MAX(a.[DeviceType]) AS DeviceType,
                            MAX('#' + CAST(a.[OrderID] as VARCHAR(50)) + ' - ' + CONVERT(VARCHAR(20), a.[OrderDate], 103) + ' - ' + CAST(a.[OrderAmount] as VARCHAR(20)) + '/-' + CASE WHEN a.[OrderType] = 1 THEN 'Regular Order' ELSE 'Prescription Order' END) AS ordInfo,
                            MAX(CONVERT(VARCHAR(20), a.[FollowupLastDate], 103) + ' - ' + CONVERT(VARCHAR(20), a.[FollowupNextDate], 103)) AS flLastDate, 
                            MAX(b.[CustomerName]) AS CustomerName, 
                            MAX(b.[CustomerMobile]) AS CustomerMobile,
                            (SELECT COUNT([FlupID]) FROM [dbo].[FollowupOrders] WHERE [FK_CustomerId] = a.[FK_OrderCustomerID] AND [FK_OrderId] = a.[OrderID]) AS flCount,
                            ISNULL((SELECT TOP 1 c.[TeamPersonName] FROM [dbo].[SupportTeam] c INNER JOIN [dbo].[FollowupOrders] d ON c.[TeamID] = d.[FK_TeamMemberId] WHERE d.[FK_CustomerId] = a.[FK_OrderCustomerID] AND d.[FK_OrderId] = a.[OrderID] ORDER BY d.[FlupID] DESC), 'NA') AS flBy,
                            --(CASE WHEN MAX(a.[OrderPayStatus]) = 1 THEN 'PAID' WHEN MAX(a.[OrderPayStatus]) = 0 THEN 'UNPAID' ELSE '-' END) AS transactionStatus
	                        MAX(a.[OrderPayStatus]) AS transactionStatus
                        FROM [dbo].[OrdersData] a
                        INNER JOIN [dbo].[CustomersData] b ON a.[FK_OrderCustomerID] = b.[CustomrtID]
                        LEFT JOIN [dbo].[FollowupOrders] e ON a.[OrderID] = e.[FK_OrderId]
                        WHERE (a.[FollowupStatus] = 'Active' OR a.[FollowupStatus] IS NULL)
                        AND a.[OrderStatus] IN (2, 4, 8, 9, 11)
                        AND CONVERT(VARCHAR(20), a.[OrderDate], 112) >= CONVERT(VARCHAR(20), CAST('" + myFromDate + "' AS DATETIME), 112)"
                        + " AND CONVERT(VARCHAR(20), a.[OrderDate], 112) <= CONVERT(VARCHAR(20), CAST('" + DateTime.Now + "' AS DATETIME))"
                        + " GROUP BY a.[OrderID], a.[FK_OrderCustomerID], a.[OrderPayStatus]"
                        + " ORDER BY MAX(a.[FollowupNextDate]) DESC";

            using (DataTable dtFlOrd = c.GetDataTable(strQuery))
            {
                gvOrdFlup.DataSource = dtFlOrd;
                gvOrdFlup.DataBind();

                if (gvOrdFlup.Rows.Count > 0)
                {
                    gvOrdFlup.UseAccessibleHeader = true;
                    gvOrdFlup.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvOrdFlup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                switch (e.Row.Cells[2].Text)
                {
                    case "2":
                        litStatus.Text = "<div class=\"ordNew\">Cancelled by Customer\r\n</div>";
                        break;

                    case "4":
                        litStatus.Text = "<div class=\"ordShipped\">Denied by Admin\r\n</div>";
                        break;

                    case "8":
                        litStatus.Text = "<div class=\"ordAccepted\">Rejected by GMMH0001\r\n</div>";
                        break;

                    case "9":
                        litStatus.Text = "<div class=\"ordProcessing\">Rejected by Shop\r\n</div>";
                        break;

                    case "11":
                        litStatus.Text = "<div class=\"ordDenied\">Rejected by Doctor\r\n</div>";
                        break;
                }

                Literal litTranStatus = (Literal)e.Row.FindControl("litTranStatus");
                switch (e.Row.Cells[3].Text)
                {
                    case "0":
                        litTranStatus.Text = "<div class=\"ordDenied\">UNPAID\r\n</div>";
                        break;

                    case "1":
                        litTranStatus.Text = "<div class=\"ordDelivered\">PAID\r\n</div>";
                        break;
                }

                Literal litFlUp = (Literal)e.Row.FindControl("litFlUp");
                
                litFlUp.Text = "<a href=\"order-details.aspx?ordId=" + e.Row.Cells[1].Text + "\" class=\"gView\" title=\"View/Edit\"></a>";
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvOrdFlup_RowDataBound", ex.Message.ToString());
            return;
        }
    }
}