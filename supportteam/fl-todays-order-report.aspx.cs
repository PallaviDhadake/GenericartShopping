using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class supportteam_fl_todays_order_report : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
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
            if (Request.QueryString["type"] == "today")
            {
                if (Request.QueryString["shop"] != null)
                {
                    if (Request.QueryString["shop"] == "own")
                    {
                        strQuery = @"SELECT DISTINCT
                                         a.[FK_OrderCustomerID] as FK_OrderCustomerID,
                                         a.[OrderID] as OrderID,
                                         MAX(a.[OrderSalesBillNumber]) as SaleBillNo,
                                         MAX(a.[OrderStatus]) as OrderStatus,
                                         MAX(CONVERT(VARCHAR(20), a.[FollowupNextDate], 103)) as FollowupNextDate,
                                         MAX(DATEDIFF(DAY, a.[FollowupLastDate], GETDATE())) as DateDiff,
                                         MAX(a.[DeviceType]) as DeviceType,
                                         MAX('#' + CAST(a.[OrderID] as VARCHAR(50)) + ' - ' + CONVERT(VARCHAR(20), a.[OrderDate], 103) + ' - ' + CAST(a.[OrderAmount] as VARCHAR(20)) + '/-' + CASE WHEN a.[OrderType] = 1 THEN 'Regular Order' ELSE 'Prescription Order' END) as ordInfo,
                                         MAX((CASE WHEN a.[DispatchDate] IS NOT NULL THEN CONVERT(VARCHAR(20), a.[DispatchDate], 103) WHEN a.[DispatchDate] IS NULL THEN 'NA' END) + ' - ' + CONVERT(VARCHAR(20), a.[EstimatedDeliveryDate], 103)) as deliverydate,
                                         MAX(CONVERT(VARCHAR(20), a.[OrderDate], 103) + ' - ' + CONVERT(VARCHAR(20), a.[FollowupNextDate], 103)) as flLastDate,
                                         MAX(b.[CustomerName]) as CustomerName, 
                                         MAX(b.[CustomerMobile]) as CustomerMobile,
                                         (SELECT COUNT([FlupID]) FROM [dbo].[FollowupOrders] WHERE [FK_CustomerId] = a.[FK_OrderCustomerID] AND [FK_OrderId] = a.[OrderID]) as flCount,
                                         ISNULL((SELECT TOP 1 c.[TeamPersonName] FROM [dbo].[SupportTeam] c INNER JOIN [dbo].[FollowupOrders] d ON c.[TeamID] = d.[FK_TeamMemberId] WHERE d.[FK_CustomerId] = a.[FK_OrderCustomerID] AND d.[FK_OrderId] = a.[OrderID] ORDER BY d.[FlupID] DESC), 'NA') as flBy
                                     FROM [dbo].[OrdersData] a 
                                     INNER JOIN [dbo].[CustomersData] b ON a.[FK_OrderCustomerID] = b.[CustomrtID]
                                     INNER JOIN [dbo].[OrdersAssign] c ON a.[OrderID] = c.[FK_OrderID]
                                     INNER JOIN [dbo].[CompanyOwnShops] d ON c.[Fk_FranchID] = d.[FK_FranchID]
                                     LEFT JOIN [dbo].[FollowupOrders] e ON a.[OrderID] = e.[FK_OrderId]
                                     WHERE a.[FollowupStatus] = 'Active'
                                     AND a.[OrderStatus] = 7
                                     AND c.[OrdReAssign] = 0
                                     AND a.OrderDate <= GETDATE()
                                     AND a.OrderDate >= DATEADD(MONTH, -6, GETDATE())
                                     AND DAY(a.OrderDate) = DATEPART(DAY, GETDATE())
                                     GROUP BY a.[OrderID],a.[FK_OrderCustomerID]";

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
                string favShopOrder = "";
                object favShopId = c.GetReqData("[dbo].[CustomersData]", "[CustomerFavShop]", "[CustomrtID] = " + e.Row.Cells[0].Text);
                if (favShopId != DBNull.Value && favShopId != null && favShopId.ToString() != "")
                {
                    favShopOrder = "<br/><span style=\"font-size:0.8em; color:#fa0ac2; line-height:0.5; font-weight:600;\">Favourite Shop Order</span>";
                }

                string ordStatus = "";
                switch (e.Row.Cells[2].Text)
                {
                    case "1":
                        ordStatus = "<div class=\"ordNew\">New</div>" + favShopOrder;
                        break;
                    case "2":
                        ordStatus = "<div class=\"ordCancCust\">Cancel By Customer</div>";
                        break;
                    case "3":
                        ordStatus = "<div class=\"ordAccepted\">Accepted By Admin</div>" + favShopOrder;
                        break;
                    case "4":
                        string frCode = "", frInfo = "";
                        if (c.IsRecordExist("SELECT [OrdAssignID] FROM [dbo].[OrdersAssign] WHERE [OrdAssignStatus] = 2 AND [FK_OrderID] = " + e.Row.Cells[1].Text + ""))
                        {
                            int frId = Convert.ToInt32(c.GetReqData("[dbo].[OrdersAssign]", "TOP 1 [Fk_FranchID]", "[OrdAssignStatus] = 2 AND [FK_OrderID] = " + e.Row.Cells[1].Text + " ORDER BY [OrdAssignID] DESC"));
                            frCode = c.GetReqData("[dbo].[FranchiseeData]", "[FranchShopCode]", "[FranchID] = " + frId).ToString();
                            frInfo = " Rejected By " + frCode;
                        }
                        ordStatus = "<div class=\"ordDenied\">Denied By Admin " + frInfo + "</div>" + favShopOrder;
                        //ordStatus = "<div class=\"ordDenied\">Denied By Admin</div>" + favShopOrder;
                        break;
                    case "5":
                        ordStatus = "<div class=\"ordProcessing\">Processing</div>" + favShopOrder;
                        break;
                    case "6":
                        ordStatus = "<div class=\"ordShipped\">Shipped</div>" + favShopOrder;
                        break;
                    case "7":
                        ordStatus = "<div class=\"ordDelivered\">Delivered</div>" + favShopOrder;
                        break;
                    case "8":
                        ordStatus = "<div class=\"ordDenied\">Rejected By GMMH0001</div>" + favShopOrder;
                        break;
                    case "9":
                        int shopId = Convert.ToInt32(c.GetReqData("[dbo].[OrdersAssign]", "TOP 1 [Fk_FranchID]", "[OrdAssignStatus] = 2 AND [FK_OrderID] = " + e.Row.Cells[1].Text + " ORDER BY [OrdAssignID] DESC"));
                        string shopCode = c.GetReqData("[dbo].[FranchiseeData]", "[FranchShopCode]", "[FranchID] = " + shopId).ToString();
                        ordStatus = "<div class=\"ordProcessing\">Rejected by " + shopCode + " - Order Amount Low</div>" + favShopOrder;
                        //ordStatus = "<div class=\"ordProcessing\">Rejected by Shop - Order Amount Low</div>" + favShopOrder;
                        break;
                    case "10":
                        ordStatus = "<div class=\"ordDenied\">Returned By Customer</div>" + favShopOrder;
                        break;
                }

                e.Row.Cells[5].Text += ordStatus.ToString();

                string strFlup = "";
                Literal litFlUp = (Literal)e.Row.FindControl("litFlUp");
                if (Request.QueryString["type"] != null)
                {
                    if (Request.QueryString["type"] == "fresh")
                    {
                        //strFlup = " <a href=\"assign-order.aspx?custId=" + e.Row.Cells[0].Text + "&ordId=" + e.Row.Cells[1].Text + "\" class=\"btn btn-sm btn-primary\" target=\"_blank\">Assign Order</a>";
                        strFlup = " <a href=\"assign-order.aspx?id=" + e.Row.Cells[1].Text + "\" class=\"btn btn-sm btn-primary\" target=\"_blank\">Assign Order</a>";
                    }
                    else
                    {
                        strFlup = " <a href=\"followup-order-detail.aspx?custId=" + e.Row.Cells[0].Text + "&ordId=" + e.Row.Cells[1].Text + "\" class=\"btn btn-sm btn-primary\" target=\"_blank\"><i class=\"fa fas fa-user-plus\"></i> &nbsp; Followup</a>";
                    }
                }
                else
                {
                    strFlup = " <a href=\"followup-order-detail.aspx?custId=" + e.Row.Cells[0].Text + "&ordId=" + e.Row.Cells[1].Text + "\" class=\"btn btn-sm btn-primary\" target=\"_blank\"><i class=\"fa fas fa-user-plus\"></i> &nbsp; Followup</a>";
                }
                //strFlup = " <span title=\"User is Currently Locked\" class=\"text-danger text-sm\"><i class=\"fa fas fa-user-lock\"></i> &nbsp; User is currently Locked</span>";
                if (c.IsRecordExist("SELECT [CustomrtID] FROM [dbo].[CustomersData] WHERE [CallBusyFlag] = 1 AND [CustomrtID] = " + e.Row.Cells[0].Text + " AND [CallBusyBy] <>" + Session["adminSupport"]))
                {
                    strFlup = " <span title=\"User is Currently Locked\" class=\"text-danger text-sm\"><i class=\"fa fas fa-user-lock\"></i> &nbsp; User is Currently Busy</span>";
                }

                litFlUp.Text = strFlup.ToString();
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