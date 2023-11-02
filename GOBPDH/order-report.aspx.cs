using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GOBPDH_order_report : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                string dateRange = c.GetFinancialYear();
                string[] arrDateRange = dateRange.ToString().Split('#');
                DateTime myFromDate = Convert.ToDateTime(arrDateRange[0]);
                DateTime myToDate = Convert.ToDateTime(arrDateRange[1]);
                litDate.Text = "(" + myFromDate.ToString("dd/MM/yyyy") + " - " + DateTime.Now.ToString("dd/MM/yyyy") + ")";
                FillGrid();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('Error Occurred While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "Page_Load", ex.Message.ToString());
            return;
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

            string gobpuser = c.GetReqData("[dbo].[DistrictHead]", "[DistHdUserId]", "[DistHdId] = " + Session["adminGOBPDH"]).ToString();

            if (Request.QueryString["type"] == "month")
            {
                strQuery = @"SELECT
                                a.[OBP_ID] AS GOBPID,
                                b.[OrderID] AS OrderID,
                                b.[OrderStatus] AS OrdStatus,
                                CONVERT(VARCHAR(20), b.[OrderDate], 103) AS ordDate,
                                ISNULL(b.[DeviceType], '-') AS DeviceType,
                                'Rs.' + CONVERT(VARCHAR(20), b.[OrderAmount]) AS OrdAmount,
                                (SELECT COUNT([FK_DetailProductID]) FROM [dbo].[OrdersDetails] WHERE [FK_DetailOrderID] = b.[OrderID]) as ProductCount,
                                c.[CustomerName],
                                STUFF((SELECT ', ' + RTRIM(LTRIM([ProductName])) FROM [dbo].[ProductsData] WHERE [ProductID] IN(SELECT [FK_DetailProductID] FROM [dbo].[OrdersDetails] WHERE [FK_DetailOrderID] = b.[OrderID]) FOR XML PATH('')), 1, 1, '' ) as CartProducts
                            FROM [dbo].[OBPData] AS a
                            INNER JOIN [dbo].[OrdersData] b ON a.[OBP_ID] = b.[GOBPId]
                            INNER JOIN [dbo].[CustomersData] c ON b.[FK_OrderCustomerID] = c.[CustomrtID]
                            WHERE b.[GOBPId] IS NOT NULL 
                            AND b.[GOBPId] > 0 AND a.[OBP_DH_UserId] = '" + gobpuser + "' AND b.[OrderStatus] <> 0 AND b.[OrderType] IS NOT NULL AND b.[OrderType] <> 0 AND YEAR(b.[OrderDate]) = YEAR('" + DateTime.Now + "') AND MONTH(b.[OrderDate]) = MONTH('" + DateTime.Now + "')";
            }
            if (Request.QueryString["type"] == "financial")
            {
                strQuery = @"SELECT
                                a.[OBP_ID] AS GOBPID,
                                b.[OrderID] AS OrderID,
                                b.[OrderStatus] AS OrdStatus,
                                CONVERT(VARCHAR(20), b.[OrderDate], 103) AS ordDate,
                                ISNULL(b.[DeviceType], '-') AS DeviceType,
                                'Rs.' + CONVERT(VARCHAR(20), b.[OrderAmount]) AS OrdAmount,
                                (SELECT COUNT([FK_DetailProductID]) FROM [dbo].[OrdersDetails] WHERE [FK_DetailOrderID] = b.[OrderID]) as ProductCount,
                                c.[CustomerName],
                                STUFF((SELECT ', ' + RTRIM(LTRIM([ProductName])) FROM [dbo].[ProductsData] WHERE [ProductID] IN(SELECT [FK_DetailProductID] FROM [dbo].[OrdersDetails] WHERE [FK_DetailOrderID] = b.[OrderID]) FOR XML PATH('')), 1, 1, '' ) as CartProducts
                            FROM [dbo].[OBPData] AS a
                            INNER JOIN [dbo].[OrdersData] b ON a.[OBP_ID] = b.[GOBPId]
                            INNER JOIN [dbo].[CustomersData] c ON b.[FK_OrderCustomerID] = c.[CustomrtID]
                            WHERE b.[GOBPId] IS NOT NULL 
                            AND b.[GOBPId] > 0 AND a.[OBP_DH_UserId] = '" + gobpuser + "' AND b.[OrderStatus] <> 0 AND b.[OrderType] IS NOT NULL AND b.[OrderType] <> 0 AND CONVERT(VARCHAR(20), b.[OrderDate], 112) >= CONVERT(VARCHAR(20), CAST('" + myFromDate + "' AS DATETIME), 112) AND CONVERT(VARCHAR(20), b.[OrderDate], 112) <= CONVERT(VARCHAR(20), CAST('" + DateTime.Now + "' AS DATETIME), 112)";
            }
            else
            {
                strQuery = @"SELECT
                                a.[OBP_ID] AS GOBPID,
                                b.[OrderID] AS OrderID,
                                b.[OrderStatus] AS OrdStatus,
                                CONVERT(VARCHAR(20), b.[OrderDate], 103) AS ordDate,
                                ISNULL(b.[DeviceType], '-') AS DeviceType,
                                'Rs.' + CONVERT(VARCHAR(20), b.[OrderAmount]) AS OrdAmount,
                                (SELECT COUNT([FK_DetailProductID]) FROM [dbo].[OrdersDetails] WHERE [FK_DetailOrderID] = b.[OrderID]) as ProductCount,
                                c.[CustomerName],
                                STUFF((SELECT ', ' + RTRIM(LTRIM([ProductName])) FROM [dbo].[ProductsData] WHERE [ProductID] IN(SELECT [FK_DetailProductID] FROM [dbo].[OrdersDetails] WHERE [FK_DetailOrderID] = b.[OrderID]) FOR XML PATH('')), 1, 1, '' ) as CartProducts
                            FROM [dbo].[OBPData] AS a
                            INNER JOIN [dbo].[OrdersData] b ON a.[OBP_ID] = b.[GOBPId]
                            INNER JOIN [dbo].[CustomersData] c ON b.[FK_OrderCustomerID] = c.[CustomrtID]
                            WHERE b.[GOBPId] IS NOT NULL 
                            AND b.[GOBPId] > 0 AND a.[OBP_DH_UserId] = '" + gobpuser + "' AND b.[OrderStatus] <> 0 AND b.[OrderType] IS NOT NULL AND b.[OrderType] <> 0";
            }

            using (DataTable dtOrder = c.GetDataTable(strQuery))
            {
                gvOrder.DataSource = dtOrder;
                gvOrder.DataBind();
                if (gvOrder.Rows.Count > 0)
                {
                    gvOrder.UseAccessibleHeader = false;
                    gvOrder.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('Error Occurred While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FillGrid", ex.Message.ToString());
            return;
        }
    }

    protected void RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[8].Text = "Status";
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Literal litStatus = (Literal)e.Row.FindControl("litStatus");
            switch (e.Row.Cells[1].Text)
            {
                case "1":
                    litStatus.Text = "<div class=\"ordNew\">New</div>";
                    break;

                case "2":
                    litStatus.Text = "<div class=\"ordCancCust\">Cancelled By Customer</div>";
                    break;

                case "3":
                    litStatus.Text = "<div class=\"ordAccepted\">Accepted</div>";
                    break;

                case "4":
                    litStatus.Text = "<div class=\"ordDenied\">Denied By Admin</div>";
                    break;

                case "5":
                    litStatus.Text = "<div class=\"ordProcessing\">Processing</div>";
                    break;

                case "6":
                    litStatus.Text = "<div class=\"ordShipped\">Shipped</div>";
                    break;

                case "7":
                    litStatus.Text = "<div class=\"ordDelivered\">Delivered</div>";
                    break;

                case "8":
                    litStatus.Text = "<div class=\"ordDenied\">Rejected By GMMH0001</div>";
                    break;

                case "9":
                    litStatus.Text = "<div class=\"ordDenied\">Rejected By Shop</div>";
                    break;

                case "10":
                    litStatus.Text = "<div class=\"ordAutoRoute\">Returned By Customer</div>";
                    break;

                case "11":
                    litStatus.Text = "<div class=\"ordDenied\">Rejected By Doctor</div>";
                    break;

                case "12":
                    litStatus.Text = "<div class=\"ordAutoRoute\">No Response To Call</div>";
                    break;
            }

            Literal litAnch = (Literal)e.Row.FindControl("litAnch");
            if (Request.QueryString["type"] != null)
                litAnch.Text = "<a href=\"order-detail.aspx?type=" + Request.QueryString["type"] + "&ordId=" + e.Row.Cells[2].Text + "\" class=\"gView\" title=\"View/Edit\"></a>";
            else
                litAnch.Text = "<a href=\"order-detail.aspx?&ordId=" + e.Row.Cells[2].Text + "\" class=\"gView\" title=\"View/Edit\"></a>";
        }
    }
}