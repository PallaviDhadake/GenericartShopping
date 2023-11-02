using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class account_generic_mitra_incentive_details : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] ordData = new String[10];
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {                
                FillGrid();                
            }
        }
        catch (Exception ex)
        {

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "Page_Load", ex.Message.ToString());
            return;
        }
    }

    private void FillGrid()
    {
        try
        {
            string strQuery = "";
            string[] arrYear = Request.QueryString["month"].ToString().Split('-');
            string monthData = arrYear[0].ToString();
            string yearData = arrYear[1].ToString();

            monthData = monthData.Trim();
            int monthNumber = 0;
            switch (monthData.ToLower())
            {
                case "january":
                    monthNumber = 1;
                    break;
                case "february":
                    monthNumber = 2;
                    break;
                case "march":
                    monthNumber = 3;
                    break;
                case "april":
                    monthNumber = 4;
                    break;
                case "may":
                    monthNumber = 5;
                    break;
                case "june":
                    monthNumber = 6;
                    break;
                case "july":
                    monthNumber = 7;
                    break;
                case "august":
                    monthNumber = 8;
                    break;
                case "september":
                    monthNumber = 9;
                    break;
                case "october":
                    monthNumber = 10;
                    break;
                case "november":
                    monthNumber = 11;
                    break;
                case "december":
                    monthNumber = 12;
                    break;
                default:
                    // Invalid month string
                    break;
            }

            strQuery = @"SELECT 
                             MAX(a.[OrdAssignID]) AS OrdAssignID,   
	                         a.[FK_OrderID], 
                             MAX(b.[OrderSalesBillNumber]) as SaleBill,
	                         CONVERT(VARCHAR(20), MAX(a.[OrdAssignDate]), 103)as ordDate, 
	                         MAX(ISNULL(b.[DeviceType], '-')) as DeviceType, 
	                         MAX(a.[OrdAssignStatus]) as OrdAssignStatus , 
	                         'Rs. ' + CONVERT(VARCHAR(20), MAX(b.[OrderAmount])) as OrdAmount,
	                         (SELECT COUNT([FK_DetailProductID]) FROM [dbo].[OrdersDetails] WHERE [FK_DetailOrderID] = MAX(b.[OrderID])) as ProductCount, 
	                         MAX(c.[CustomerName]) as CustomerName, 
	                         STUFF((SELECT ', ' + RTRIM(LTRIM([ProductName])) FROM [dbo].[ProductsData] WHERE [ProductID] IN (SELECT [FK_DetailProductID] FROM [dbo].[OrdersDetails] WHERE [FK_DetailOrderID] = MAX(b.[OrderID])) FOR XML PATH('')), 1, 1, '' ) as CartProducts,
	                         (CASE WHEN MAX(b.[OrderPayMode]) = 1 THEN 'COD' WHEN MAX(b.[OrderPayMode]) = 2 THEN 'ONLINE' ELSE '-' END) AS PaymentMode
                         FROM [dbo].[OrdersAssign] a 
                         INNER JOIN [dbo].[OrdersData] b ON a.[FK_OrderID] = b.[OrderID] 
                         INNER JOIN [dbo].[CustomersData] c ON b.[FK_OrderCustomerID] = c.[CustomrtID] WHERE c.[FK_GenMitraID] = " + Request.QueryString["id"] + " AND (YEAR(b.[OrderDate]) = " + yearData + " And MONTH(b.[OrderDate]) = " + monthNumber + ") GROUP BY a.[FK_OrderID]";

            using (DataTable dtOrder = c.GetDataTable(strQuery))
            {
                gvOrder.DataSource = dtOrder;
                gvOrder.DataBind();
                if (gvOrder.Rows.Count > 0)
                {
                    gvOrder.UseAccessibleHeader = true;
                    gvOrder.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvOrder_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //if (Request.QueryString["type"] != null)
                //{
                //    if (Request.QueryString["type"] == "monthly")
                //    {
                //        e.Row.Cells[11].Text = "Mark as Not Monthly Order";
                //    }
                //    else
                //    {
                //        e.Row.Cells[11].Text = "";
                //    }
                //}
                //else
                //{
                //    e.Row.Cells[11].Text = "";
                //}
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string ordID = c.GetReqData("[dbo].[OrdersAssign]", "[FK_OrderID]", "[OrdAssignID] = " + e.Row.Cells[0].Text + "").ToString();

                int reAssignStatus = Convert.ToInt32(c.GetReqData("[dbo].[OrdersAssign]", "[OrdReAssign]", "[OrdAssignID] = " + e.Row.Cells[0].Text));

                int MainOrderStatus = Convert.ToInt32(c.GetReqData("[dbo].[OrdersData]", "[OrderStatus]", "[OrderID] = " + e.Row.Cells[2].Text));

                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                switch (e.Row.Cells[1].Text)
                {
                    case "0":

                        if (c.IsRecordExist("SELECT [OrdAssignID] FROM [dbo].[OrdersAssign] WHERE [FK_OrderID] = " + e.Row.Cells[2].Text + " AND [OrdAssignStatus] = 0 AND [OrdReAssign] = 1 AND [OrdAssignID] <> " + e.Row.Cells[0].Text))
                        {
                            if (reAssignStatus == 1)
                            {
                                litStatus.Text = "<div class=\"ordAutoRoute\">Auto Routed</div>";
                            }
                            else
                            {
                                litStatus.Text = "<div class=\"ordNew\">Re Assigned</div>";
                            }
                        }
                        else
                        {
                            if (reAssignStatus == 0)
                            {
                                string frCode = "", frInfo = "";
                                if (c.IsRecordExist("SELECT [OrdAssignID] FROM [dbo].[OrdersAssign] WHERE [OrdAssignStatus] = 2 AND [FK_OrderID] = " + e.Row.Cells[2].Text + ""))
                                {
                                    int frId = Convert.ToInt32(c.GetReqData("[dbo].[OrdersAssign]", "TOP 1 [Fk_FranchID]", "[OrdAssignStatus] = 2 AND [FK_OrderID] = " + e.Row.Cells[2].Text + " ORDER BY [OrdAssignID] DESC"));
                                    frCode = c.GetReqData("[dbo].[FranchiseeData]", "[FranchShopCode]", "[FranchID] = " + frId).ToString();
                                    frInfo = " (Rejected By " + frCode + ")";
                                }
                                litStatus.Text = "<div class=\"ordNew\">New " + frInfo + "</div>";
                            }
                            if (reAssignStatus == 1)
                            {
                                litStatus.Text = "<div class=\"ordAutoRoute\">Auto Routed</div>";
                            }
                        }

                        if (MainOrderStatus == 2)
                        {
                            litStatus.Text = "<div class=\"ordCancCust\">Cancel By Customer</div>";
                        }
                        break;
                    case "1":
                        litStatus.Text = "<div class=\"ordAccepted\">Accepted</div>";
                        break;
                    case "2":
                        litStatus.Text = "<div class=\"ordDenied\">Rejected</div>";
                        break;
                    case "5":
                        litStatus.Text = "<div class=\"ordProcessing\">Inprocess</div>";
                        break;
                    case "6":
                        litStatus.Text = "<div class=\"ordShipped\">Shipped</div>";
                        break;
                    case "7":
                        litStatus.Text = "<div class=\"ordDelivered\">Delivered</div>";
                        break;
                    case "10":
                        litStatus.Text = "<div class=\"ordDenied\">Returned By Customer</div>";
                        break;

                }            
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvOrder_RowDataBound", ex.Message.ToString());
            return;
        }
    }    
}