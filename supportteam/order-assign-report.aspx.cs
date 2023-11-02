using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class supportteam_order_assign_report : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] ordData = new String[10];
    public string fyDateRange;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillGrid();

            //// Prepare From & To Date range as parameter (28-Apr-2023)
            string dateRange = c.GetFinancialYear();
            string[] arrDateRange = dateRange.ToString().Split('#');
            DateTime myFromDate = Convert.ToDateTime(arrDateRange[0]);
            DateTime myToDate = Convert.ToDateTime(arrDateRange[1]);
            fyDateRange = myFromDate.ToString("dd/MM/yyyy") + " - " + DateTime.Now.ToString("dd/MM/yyyy");

        }
    }

    private void FillGrid()
    {
        try
        {
            string dateRange = c.GetFinancialYear();
            string[] arrDateRange = dateRange.ToString().Split('#');
            DateTime myFromDate = Convert.ToDateTime(arrDateRange[0]);
            DateTime myToDate = Convert.ToDateTime(arrDateRange[1]);

            string strQuery = "";
            if (Request.QueryString["type"] != null)
            {
                switch (Request.QueryString["type"])
                {
                    case "accept":
                        strQuery = @"SELECT DISTINCT 
                                        a.[OrderID],
                                        a.[FK_OrderCustomerID],
                                        a.[OrderDate],
                                        CONVERT(VARCHAR(20), a.[OrderDate], 103) AS ordDate,
                                        b.[CustomerName],
                                        b.[CustomerMobile], 
                                        'Rs. ' + CONVERT(VARCHAR(20), a.[OrderAmount]) AS OrdAmount,
                                        ISNULL(a.[DeviceType], '-') AS DeviceType,   
                                        a.[OrderStatus]
                                     FROM[dbo].[OrdersData] a
                                     INNER JOIN[dbo].[CustomersData] b ON a.[FK_OrderCustomerID] = b.[CustomrtID]
                                     LEFT JOIN [dbo].[OrdersAssign] c ON a.[OrderID] = c.[FK_OrderID]
                                     WHERE a.[OrderStatus] = 3
                                     AND c.[FK_OrderID] IS NULL 
                                     ORDER BY a.[OrderID] DESC";
                        break;
                }
            }
            //else
            //{
            //    strQuery = @"SELECT DISTINCT 
            //                    a.[OrderID],
            //                    a.[FK_OrderCustomerID],
            //                    a.[OrderDate],
            //                    CONVERT(VARCHAR(20), a.[OrderDate], 103) AS ordDate,
            //                    b.[CustomerName],
            //                    b.[CustomerMobile], 
            //                    'Rs. ' + CONVERT(VARCHAR(20), a.[OrderAmount]) AS OrdAmount,
            //                    ISNULL(a.[DeviceType], '-') AS DeviceType,   
            //                    a.[OrderStatus]
            //                 FROM[dbo].[OrdersData] a
            //                 INNER JOIN[dbo].[CustomersData] b ON a.[FK_OrderCustomerID] = b.[CustomrtID]
            //                 LEFT JOIN [dbo].[OrdersAssign] c ON a.[OrderID] = c.[FK_OrderID]
            //                 WHERE a.[OrderStatus] = 3
            //                 AND a.[FollowupStatus] = 'Active'
            //                 AND c.[FK_OrderID] IS NULL 
            //                 ORDER BY a.[OrderID] DESC";
            //}

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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"ordered-customer-details.aspx?id=" + e.Row.Cells[0].Text + "\" class=\"gView\" title=\"View/Edit\"></a>";

                string favShopOrder = "";
                object favShopId = c.GetReqData("CustomersData", "CustomerFavShop", "CustomrtID=" + e.Row.Cells[2].Text);
                if (favShopId != DBNull.Value && favShopId != null && favShopId.ToString() != "")
                {
                    favShopOrder = "<br/><span style=\"font-size:0.8em; color:#fa0ac2; line-height:0.5; font-weight:600;\">Favourite Shop Order</span>";
                }

                // orderStatus=0 > added to cart, 1 > Order placed (New/pending), 2 > CANCELL ORDER BY CUSTOMER , 3 > Accepted, 4 > Denied, 5 > Processing , 6 > Shipped , 7 > deliverd
                // 8 > Re-assigned(rejected by 0001), 9 > Rejected by shop for reason aorder amount low, 10 > OrderReturned
                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                switch (e.Row.Cells[1].Text)
                {
                    case "1":
                        litStatus.Text = "<div class=\"ordNew\">New</div>" + favShopOrder;
                        break;
                    case "2":
                        litStatus.Text = "<div class=\"ordCancCust\">Cancel By Customer</div>";
                        break;
                    case "3":
                        litStatus.Text = "<div class=\"ordAccepted\">Accepted</div>" + favShopOrder;
                        break;
                    case "4":
                        string frCode = "", frInfo = "";
                        if (c.IsRecordExist("Select OrdAssignID From OrdersAssign Where OrdAssignStatus=2 AND FK_OrderID=" + e.Row.Cells[0].Text + ""))
                        {
                            int frId = Convert.ToInt32(c.GetReqData("OrdersAssign", "Top 1 Fk_FranchID", "OrdAssignStatus=2 AND FK_OrderID=" + e.Row.Cells[0].Text + " Order By OrdAssignID DESC"));
                            frCode = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + frId).ToString();
                            frInfo = " Rejected By " + frCode;
                        }
                        litStatus.Text = "<div class=\"ordDenied\">Denied By Admin " + frInfo + "</div>" + favShopOrder;
                        break;
                    case "5":
                        litStatus.Text = "<div class=\"ordProcessing\">Processing</div>" + favShopOrder;
                        break;
                    case "6":
                        litStatus.Text = "<div class=\"ordShipped\">Shipped</div>" + favShopOrder;
                        break;
                    case "7":
                        litStatus.Text = "<div class=\"ordDelivered\">Delivered</div>" + favShopOrder;
                        break;
                    case "8":
                        litStatus.Text = "<div class=\"ordDenied\">Rejected By GMMH0001</div>" + favShopOrder;
                        break;
                    case "9":
                        int shopId = Convert.ToInt32(c.GetReqData("OrdersAssign", "Top 1 Fk_FranchID", "OrdAssignStatus=2 AND FK_OrderID=" + e.Row.Cells[0].Text + " Order By OrdAssignID DESC"));
                        string shopCode = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + shopId).ToString();
                        litStatus.Text = "<div class=\"ordProcessing\">Rejected by " + shopCode + " - Order Amount Low</div>" + favShopOrder;
                        break;
                    case "10":
                        litStatus.Text = "<div class=\"ordDenied\">Returned By Customer</div>" + favShopOrder;
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

    [WebMethod]
    public static string GetFavShopStatus(string custId)
    {
        iClass c = new iClass();
        object favShopId = c.GetReqData("CustomersData", "CustomerFavShop", "CustomrtID=" + custId);
        string favShopOrderStatus = "";
        if (favShopId != DBNull.Value && favShopId != null && favShopId.ToString() != "")
        {
            favShopOrderStatus = "<br/><span style=\"font-size:0.8em; color:#fa0ac2; line-height:0.5; font-weight:600;\">Favourite Shop Order</span>";
        }
        else
        {
            favShopOrderStatus = "";
        }
        return favShopOrderStatus.ToString();
    }
}