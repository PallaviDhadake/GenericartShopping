using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class supportteam_refund_request_report : System.Web.UI.Page
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


            if (Request.QueryString["type"] != null)
            {
                switch (Request.QueryString["type"])
                {
                    case "Refund-Request":
                        strQuery = @"SELECT
                                     b.[OrderID] AS OrderID,
                                     b.[OrderStatus] AS OrdStatus,
                                     CONVERT(VARCHAR(20), b.[OrderDate], 103) AS OrdDate,
                                     ISNULL(b.[DeviceType], '-') AS DeviceType,
                                     'Rs.' + CONVERT(VARCHAR(20), b.[OrderAmount]) AS OrdAmount,
                                     (SELECT COUNT([FK_DetailProductID]) FROM [dbo].[OrdersDetails] WHERE [FK_DetailOrderID] = b.[OrderID]) as ProductCount,
                                     c.[CustomerName],
                                     STUFF((SELECT ', ' + RTRIM(LTRIM([ProductName])) FROM [dbo].[ProductsData] WHERE [ProductID] IN(SELECT [FK_DetailProductID] FROM [dbo].[OrdersDetails] WHERE [FK_DetailOrderID] = b.[OrderID]) FOR XML PATH('')), 1, 1, '') as CartProducts
                                     FROM [dbo].[OrdersData] AS b 
                                     INNER JOIN [dbo].[CustomersData] AS c ON b.[FK_OrderCustomerID] = c.[CustomrtID]
                                     WHERE b.[OrderStatus] = 13
                                     AND (CONVERT(VARCHAR(20), b.[OrderDate], 112) >= CONVERT(VARCHAR(20), CAST('" + myFromDate + "' AS DATETIME), 112)) AND (CONVERT(VARCHAR(20), b.[OrderDate], 112) <= CONVERT(VARCHAR(20), CAST('" + DateTime.Now + "' AS DATETIME), 112))";
                        break;

                    case "Request-Inprocess":
                        strQuery = @"SELECT
                                     b.[OrderID] AS OrderID,
                                     b.[OrderStatus] AS OrdStatus,
                                     CONVERT(VARCHAR(20), b.[OrderDate], 103) AS OrdDate,
                                     ISNULL(b.[DeviceType], '-') AS DeviceType,
                                     'Rs.' + CONVERT(VARCHAR(20), b.[OrderAmount]) AS OrdAmount,
                                     (SELECT COUNT([FK_DetailProductID]) FROM [dbo].[OrdersDetails] WHERE [FK_DetailOrderID] = b.[OrderID]) as ProductCount,
                                     c.[CustomerName],
                                     STUFF((SELECT ', ' + RTRIM(LTRIM([ProductName])) FROM [dbo].[ProductsData] WHERE [ProductID] IN(SELECT [FK_DetailProductID] FROM [dbo].[OrdersDetails] WHERE [FK_DetailOrderID] = b.[OrderID]) FOR XML PATH('')), 1, 1, '') as CartProducts
                                     FROM [dbo].[OrdersData] AS b 
                                     INNER JOIN [dbo].[CustomersData] AS c ON b.[FK_OrderCustomerID] = c.[CustomrtID]
                                     WHERE b.[OrderStatus] = 14
                                     AND (CONVERT(VARCHAR(20), b.[OrderDate], 112) >= CONVERT(VARCHAR(20), CAST('" + myFromDate + "' AS DATETIME), 112)) AND (CONVERT(VARCHAR(20), b.[OrderDate], 112) <= CONVERT(VARCHAR(20), CAST('" + DateTime.Now + "' AS DATETIME), 112))";
                        break;

                    case "Request-Completed":
                        strQuery = @"SELECT
                                     b.[OrderID] AS OrderID,
                                     b.[OrderStatus] AS OrdStatus,
                                     CONVERT(VARCHAR(20), b.[OrderDate], 103) AS OrdDate,
                                     ISNULL(b.[DeviceType], '-') AS DeviceType,
                                     'Rs.' + CONVERT(VARCHAR(20), b.[OrderAmount]) AS OrdAmount,
                                     (SELECT COUNT([FK_DetailProductID]) FROM [dbo].[OrdersDetails] WHERE [FK_DetailOrderID] = b.[OrderID]) as ProductCount,
                                     c.[CustomerName],
                                     STUFF((SELECT ', ' + RTRIM(LTRIM([ProductName])) FROM [dbo].[ProductsData] WHERE [ProductID] IN(SELECT [FK_DetailProductID] FROM [dbo].[OrdersDetails] WHERE [FK_DetailOrderID] = b.[OrderID]) FOR XML PATH('')), 1, 1, '') as CartProducts
                                     FROM [dbo].[OrdersData] AS b 
                                     INNER JOIN [dbo].[CustomersData] AS c ON b.[FK_OrderCustomerID] = c.[CustomrtID]
                                     WHERE b.[OrderStatus] = 15
                                     AND (CONVERT(VARCHAR(20), b.[OrderDate], 112) >= CONVERT(VARCHAR(20), CAST('" + myFromDate + "' AS DATETIME), 112)) AND (CONVERT(VARCHAR(20), b.[OrderDate], 112) <= CONVERT(VARCHAR(20), CAST('" + DateTime.Now + "' AS DATETIME), 112))";
                        break;
                }
            }
            else
            {
                strQuery = @"SELECT
                             b.[OrderID] AS OrderID,
                             b.[OrderStatus] AS OrdStatus,
                             CONVERT(VARCHAR(20), b.[OrderDate], 103) AS OrdDate,
                             ISNULL(b.[DeviceType], '-') AS DeviceType,
                             'Rs.' + CONVERT(VARCHAR(20), b.[OrderAmount]) AS OrdAmount,
                             (SELECT COUNT([FK_DetailProductID]) FROM [dbo].[OrdersDetails] WHERE [FK_DetailOrderID] = b.[OrderID]) as ProductCount,
                             c.[CustomerName],
                             STUFF((SELECT ', ' + RTRIM(LTRIM([ProductName])) FROM [dbo].[ProductsData] WHERE [ProductID] IN(SELECT [FK_DetailProductID] FROM [dbo].[OrdersDetails] WHERE [FK_DetailOrderID] = b.[OrderID]) FOR XML PATH('')), 1, 1, '') as CartProducts
                             FROM [dbo].[OrdersData] AS b
                             INNER JOIN [dbo].[CustomersData] AS c ON b.[FK_OrderCustomerID] = c.[CustomrtID]
                             WHERE b.[OrderStatus] IN (13,14,15)
                             AND (CONVERT(VARCHAR(20), b.[OrderDate], 112) >= CONVERT(VARCHAR(20), CAST('" + myFromDate + "' AS DATETIME), 112)) AND (CONVERT(VARCHAR(20), b.[OrderDate], 112) <= CONVERT(VARCHAR(20), CAST('" + DateTime.Now + "' AS DATETIME), 112))";
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
                else
                {
                    gvOrder.UseAccessibleHeader = true;
                    gvOrder.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('Error Occurred While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "Page_Load", ex.Message.ToString());
            return;
        }
    }

    protected void RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[7].Text = "Status";
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Literal litStatus = (Literal)e.Row.FindControl("litStatus");
            switch (e.Row.Cells[0].Text)
            {
                case "13":
                    litStatus.Text = "<div class=\"ordNew\">Refund Request</div>";
                    break;

                case "14":
                    litStatus.Text = "<div class=\"ordCancCust\">Refund Inprocess</div>";
                    break;

                case "15":
                    litStatus.Text = "<div class=\"ordAccepted\">Refund Completed</div>";
                    break;
            }

            //string ordID = c.GetReqData("OrdersData", "FK_OrderID", "OrdAssignID=" + e.Row.Cells[0].Text + "").ToString();

            Literal litAnch = (Literal)e.Row.FindControl("litAnch");
            if (Request.QueryString["type"] != null)
                litAnch.Text = "<a href=\"refund-request-details.aspx?type=" + Request.QueryString["type"] + "&ordId=" + e.Row.Cells[1].Text + "\" class=\"gView\" title=\"View/Edit\"></a>";
            else
                litAnch.Text = "<a href=\"refund-request-details.aspx?ordId=" + e.Row.Cells[1].Text + "\" class=\"gView\" title=\"View/Edit\"></a>";
        }
    }
}