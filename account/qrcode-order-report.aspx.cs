using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class account_qrcode_order_report : System.Web.UI.Page
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
                //litDate.Text = "(" + myFromDate.ToString("dd/MM/yyyy") + " - " + DateTime.Now.ToString("dd/MM/yyyy") + ")";
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
            
            strQuery = @"SELECT
                         a.[OrdAssignID] AS OrdAssignID,
                         b.[OrderID] AS OrderID,
                         b.[OrderSalesBillNumber],
                         b.[OrderStatus] AS OrdStatus,
                         CONVERT(VARCHAR(20), b.[OrderDate], 103) AS ordDate,
                         ISNULL(b.[DeviceType], '-') AS DeviceType,
                         'Rs.' + CONVERT(VARCHAR(20), b.[OrderAmount]) AS OrdAmount,
                         (SELECT COUNT([FK_DetailProductID]) FROM [dbo].[OrdersDetails] WHERE [FK_DetailOrderID] = b.[OrderID]) as ProductCount,
                         c.[CustomerName],
                         b.[UPIID],
                         STUFF((SELECT ', ' + RTRIM(LTRIM([ProductName])) FROM [dbo].[ProductsData] WHERE [ProductID] IN(SELECT [FK_DetailProductID] FROM [dbo].[OrdersDetails] WHERE [FK_DetailOrderID] = b.[OrderID]) FOR XML PATH('')), 1, 1, '' ) as CartProducts
                         FROM [dbo].[OrdersAssign] AS a
                         INNER JOIN [dbo].[OrdersData] b ON a.[FK_OrderID] = b.[OrderID]
                         INNER JOIN [dbo].[CustomersData] c ON b.[FK_OrderCustomerID] = c.[CustomrtID]
                         WHERE b.[UPIID] IS NOT NULL AND b.[UPIID] <> ''
                         ORDER BY b.[OrderDate] DESC";
           

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
                litAnch.Text = "<a href=\"qrcode-order-details.aspx?type=" + Request.QueryString["type"] + "&ordId=" + e.Row.Cells[2].Text + "\" class=\"gView\" title=\"View/Edit\"></a>";
            else
                litAnch.Text = "<a href=\"qrcode-order-details.aspx?ordId=" + e.Row.Cells[2].Text + "\" class=\"gView\" title=\"View/Edit\"></a>";
        }
    }
}