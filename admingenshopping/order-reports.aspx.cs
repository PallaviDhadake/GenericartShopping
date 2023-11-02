using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class admingenshopping_order_reports : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] ordData = new String[10];
    public string fyDateRange;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //FillGrid();

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
            string strQuery = "";
            if (Request.QueryString["type"] != null)
            {
                switch (Request.QueryString["type"])
                {
                    //case "new":
                    //    strQuery = "Select DISTINCT a.OrderID, a.FK_OrderCustomerID, a.OrderDate, convert(varchar(20), a.OrderDate, 103) as ordDate, b.CustomerName, b.CustomerMobile,  'Rs. ' + Convert(varchar(20), a.OrderAmount) as OrdAmount, isnull(a.DeviceType, '-') as DeviceType, (Select COUNT(FK_DetailProductID) From " +
                    //        "OrdersDetails Where FK_DetailOrderID= a.OrderID ) As ProductCount, LEFT (STUFF((Select ', ' + RTRIM(LTRIM(ProductName)) From ProductsData Where ProductID IN (Select FK_DetailProductID from OrdersDetails where FK_DetailOrderID = a.OrderID) FOR XML PATH('')), 1, 1, '' ) , 200) as CartProducts, a.OrderStatus From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID = b.CustomrtID Where a.OrderStatus = 1 AND a.OrderType=1 Order By ordDate DESC, a.OrderID DESC";
                    //    break;

                    case "new":
                        strQuery = "Select DISTINCT a.OrderID, a.FK_OrderCustomerID, a.OrderDate, convert(varchar(20), a.OrderDate, 103) as ordDate, b.CustomerName, b.CustomerMobile,  'Rs. ' + Convert(varchar(20), a.OrderAmount) as OrdAmount, isnull(a.DeviceType, '-') as DeviceType, (Select COUNT(FK_DetailProductID) From " +
                            "OrdersDetails Where FK_DetailOrderID= a.OrderID ) As ProductCount, LEFT (STUFF((Select ', ' + RTRIM(LTRIM(ProductName)) From ProductsData Where ProductID IN (Select FK_DetailProductID from OrdersDetails where FK_DetailOrderID = a.OrderID) FOR XML PATH('')), 1, 1, '' ) , 200) as CartProducts, a.OrderStatus From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID = b.CustomrtID Where a.OrderStatus = 1 AND a.OrderType=1 AND (b.CustomerFavShop IS NULL OR b.CustomerFavShop=0) Order By ordDate DESC, a.OrderID DESC";
                        break;

                    case "new-fav":
                        strQuery = "Select DISTINCT a.OrderID, a.FK_OrderCustomerID, a.OrderDate, convert(varchar(20), a.OrderDate, 103) as ordDate, b.CustomerName, b.CustomerMobile,  'Rs. ' + Convert(varchar(20), a.OrderAmount) as OrdAmount, isnull(a.DeviceType, '-') as DeviceType, (Select COUNT(FK_DetailProductID) From " +
                            "OrdersDetails Where FK_DetailOrderID= a.OrderID ) As ProductCount, LEFT (STUFF((Select ', ' + RTRIM(LTRIM(ProductName)) From ProductsData Where ProductID IN (Select FK_DetailProductID from OrdersDetails where FK_DetailOrderID = a.OrderID) FOR XML PATH('')), 1, 1, '' ) , 200) as CartProducts, a.OrderStatus From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID = b.CustomrtID Where a.OrderStatus = 1 AND a.OrderType=1 AND (b.CustomerFavShop IS NOT NULL OR b.CustomerFavShop<>0) Order By ordDate DESC, a.OrderID DESC";
                        break;

                    case "accepted":
                        strQuery = "Select DISTINCT a.OrderID, a.FK_OrderCustomerID, a.OrderDate, convert(varchar(20), a.OrderDate, 103) as ordDate, b.CustomerName, b.CustomerMobile, 'Rs. ' + Convert(varchar(20), a.OrderAmount) as OrdAmount, isnull(a.DeviceType, '-') as DeviceType, (Select COUNT(FK_DetailProductID) From " +
                            "OrdersDetails Where FK_DetailOrderID= a.OrderID ) As ProductCount, LEFT (STUFF((Select ', ' + RTRIM(LTRIM(ProductName)) From ProductsData Where ProductID IN (Select FK_DetailProductID from OrdersDetails where FK_DetailOrderID = a.OrderID) FOR XML PATH('')), 1, 1, '' ) , 200) as CartProducts, a.OrderStatus From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID = b.CustomrtID Where a.OrderStatus = 3 AND a.OrderType=1 Order By ordDate DESC, a.OrderID DESC";
                        break;
                    case "denied":
                        strQuery = "Select DISTINCT a.OrderID, a.FK_OrderCustomerID, a.OrderDate, convert(varchar(20), a.OrderDate, 103) as ordDate, b.CustomerName, b.CustomerMobile, 'Rs. ' + Convert(varchar(20), a.OrderAmount) as OrdAmount, isnull(a.DeviceType, '-') as DeviceType, (Select COUNT(FK_DetailProductID) From " +
                            "OrdersDetails Where FK_DetailOrderID= a.OrderID ) As ProductCount, LEFT (STUFF((Select ', ' + RTRIM(LTRIM(ProductName)) From ProductsData Where ProductID IN (Select FK_DetailProductID from OrdersDetails where FK_DetailOrderID = a.OrderID) FOR XML PATH('')), 1, 1, '' ) , 200) as CartProducts, a.OrderStatus From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID = b.CustomrtID Where a.OrderStatus = 4 AND a.OrderType=1 Order By ordDate DESC, a.OrderID DESC";
                        break;
                    case "processing":
                        strQuery = "Select DISTINCT a.OrderID, a.FK_OrderCustomerID, a.OrderDate, convert(varchar(20), a.OrderDate, 103) as ordDate, b.CustomerName, b.CustomerMobile, 'Rs. ' + Convert(varchar(20), a.OrderAmount) as OrdAmount, isnull(a.DeviceType, '-') as DeviceType, (Select COUNT(FK_DetailProductID) From " +
                            "OrdersDetails Where FK_DetailOrderID= a.OrderID ) As ProductCount, LEFT (STUFF((Select ', ' + RTRIM(LTRIM(ProductName)) From ProductsData Where ProductID IN (Select FK_DetailProductID from OrdersDetails where FK_DetailOrderID = a.OrderID) FOR XML PATH('')), 1, 1, '' ) , 200) as CartProducts, a.OrderStatus From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID = b.CustomrtID Where a.OrderStatus = 5 AND a.OrderType=1 Order By ordDate DESC, a.OrderID DESC";
                        break;
                    case "shipped":
                        strQuery = "Select DISTINCT a.OrderID, a.FK_OrderCustomerID, a.OrderDate, convert(varchar(20), a.OrderDate, 103) as ordDate,b.CustomerName, b.CustomerMobile, 'Rs. ' + Convert(varchar(20), a.OrderAmount) as OrdAmount, isnull(a.DeviceType, '-') as DeviceType, (Select COUNT(FK_DetailProductID) From " +
                            "OrdersDetails Where FK_DetailOrderID= a.OrderID ) As ProductCount, LEFT (STUFF((Select ', ' + RTRIM(LTRIM(ProductName)) From ProductsData Where ProductID IN (Select FK_DetailProductID from OrdersDetails where FK_DetailOrderID = a.OrderID) FOR XML PATH('')), 1, 1, '' ) , 200) as CartProducts, a.OrderStatus From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID = b.CustomrtID Where a.OrderStatus = 6 AND a.OrderType=1 Order By ordDate DESC, a.OrderID DESC";
                        break;
                    case "delivered":
                        strQuery = "Select DISTINCT a.OrderID, a.FK_OrderCustomerID, a.OrderDate, convert(varchar(20), a.OrderDate, 103) as ordDate, b.CustomerName, b.CustomerMobile, 'Rs. ' + Convert(varchar(20), a.OrderAmount) as OrdAmount, isnull(a.DeviceType, '-') as DeviceType, (Select COUNT(FK_DetailProductID) From " +
                            "OrdersDetails Where FK_DetailOrderID= a.OrderID ) As ProductCount, LEFT (STUFF((Select ', ' + RTRIM(LTRIM(ProductName)) From ProductsData Where ProductID IN (Select FK_DetailProductID from OrdersDetails where FK_DetailOrderID = a.OrderID) FOR XML PATH('')), 1, 1, '' ) , 200) as CartProducts, a.OrderStatus From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID = b.CustomrtID Where a.OrderStatus = 7 AND a.OrderType=1 Order By ordDate DESC, a.OrderID DESC";
                        break;
                    case "monthly":
                        strQuery = "Select DISTINCT a.OrderID, a.FK_OrderCustomerID, a.OrderDate, convert(varchar(20), a.OrderDate, 103) as ordDate, b.CustomerName, b.CustomerMobile, 'Rs. ' + Convert(varchar(20), a.OrderAmount) as OrdAmount, isnull(a.DeviceType, '-') as DeviceType, (Select COUNT(FK_DetailProductID) From " +
                            "OrdersDetails Where FK_DetailOrderID= a.OrderID ) As ProductCount, LEFT (STUFF((Select ', ' + RTRIM(LTRIM(ProductName)) From ProductsData Where ProductID IN (Select FK_DetailProductID from OrdersDetails where FK_DetailOrderID = a.OrderID) FOR XML PATH('')), 1, 1, '' ) , 200) as CartProducts, a.OrderStatus From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID = b.CustomrtID Where a.OrderStatus<>0 AND a.MreqFlag=1 AND a.OrderType=1 Order By ordDate DESC, a.OrderID DESC";
                        break;
                    //deniedbyshop
                    case "deniedbyshop":
                        strQuery = "Select DISTINCT a.OrderID, a.FK_OrderCustomerID, a.OrderDate, convert(varchar(20), a.OrderDate, 103) as ordDate, b.CustomerName, b.CustomerMobile, 'Rs. ' + Convert(varchar(20), a.OrderAmount) as OrdAmount, isnull(a.DeviceType, '-') as DeviceType, (Select COUNT(FK_DetailProductID) From " +
                            "OrdersDetails Where FK_DetailOrderID= a.OrderID ) As ProductCount, LEFT (STUFF((Select ', ' + RTRIM(LTRIM(ProductName)) From ProductsData Where ProductID IN (Select FK_DetailProductID from OrdersDetails where FK_DetailOrderID = a.OrderID) FOR XML PATH('')), 1, 1, '' ) , 200) as CartProducts, a.OrderStatus From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID = b.CustomrtID Inner Join OrdersAssign c On a.OrderID=c.FK_OrderID Where a.OrderStatus<>0 AND a.OrderType=1 AND c.OrdAssignStatus=2 AND c.OrdReAssign=0 Order By ordDate DESC, a.OrderID DESC";
                        break;
                    //cancelled b cust
                    case "cancelCust":
                        strQuery = "Select DISTINCT a.OrderID, a.FK_OrderCustomerID, a.OrderDate, convert(varchar(20), a.OrderDate, 103) as ordDate, b.CustomerName, b.CustomerMobile, 'Rs. ' + Convert(varchar(20), a.OrderAmount) as OrdAmount, isnull(a.DeviceType, '-') as DeviceType, (Select COUNT(FK_DetailProductID) From " +
                            "OrdersDetails Where FK_DetailOrderID= a.OrderID ) As ProductCount, LEFT (STUFF((Select ', ' + RTRIM(LTRIM(ProductName)) From ProductsData Where ProductID IN (Select FK_DetailProductID from OrdersDetails where FK_DetailOrderID = a.OrderID) FOR XML PATH('')), 1, 1, '' ) , 200) as CartProducts, a.OrderStatus From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID = b.CustomrtID Where a.OrderStatus = 2 AND a.OrderType=1 Order By ordDate DESC, a.OrderID DESC";
                        break;
                    case "returned":
                        strQuery = "Select DISTINCT a.OrderID, a.FK_OrderCustomerID, a.OrderDate, convert(varchar(20), a.OrderDate, 103) as ordDate, b.CustomerName, b.CustomerMobile, 'Rs. ' + Convert(varchar(20), a.OrderAmount) as OrdAmount, isnull(a.DeviceType, '-') as DeviceType, (Select COUNT(FK_DetailProductID) From " +
                            "OrdersDetails Where FK_DetailOrderID= a.OrderID ) As ProductCount, LEFT (STUFF((Select ', ' + RTRIM(LTRIM(ProductName)) From ProductsData Where ProductID IN (Select FK_DetailProductID from OrdersDetails where FK_DetailOrderID = a.OrderID) FOR XML PATH('')), 1, 1, '' ) , 200) as CartProducts, a.OrderStatus From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID = b.CustomrtID Where a.OrderStatus = 10 Order By ordDate DESC, a.OrderID DESC";
                        break;
                }
            }
            else
            {
                strQuery = "Select DISTINCT a.OrderID, a.FK_OrderCustomerID, a.OrderDate, convert(varchar(20), a.OrderDate, 103) as ordDate , b.CustomerName, b.CustomerMobile,  " +
                    " 'Rs. ' + Convert(varchar(20), a.OrderAmount) as OrdAmount,  isnull(a.DeviceType, '-') as DeviceType, " +
                    " (Select COUNT(FK_DetailProductID) from OrdersDetails where FK_DetailOrderID = a.OrderID ) as ProductCount, " +
                    " LEFT (STUFF((Select ', ' + RTRIM(LTRIM(ProductName)) From ProductsData Where ProductID IN (Select FK_DetailProductID from OrdersDetails where FK_DetailOrderID = a.OrderID) FOR XML PATH('')), 1, 1, '' ) , 200) as CartProducts, a.OrderStatus " +
                    " From OrdersData a Inner Join CustomersData b on a.FK_OrderCustomerID = b.CustomrtID Where a.OrderStatus <> 0 AND a.OrderType=1" +
                    " Order By a.OrderDate DESC, a.OrderID DESC";

                //strQuery = "Select DISTINCT a.OrderID, a.OrderDate, convert(varchar(20), a.OrderDate, 103) as ordDate , b.CustomerName,  " +
                //    " 'Rs. ' + Convert(varchar(20), a.OrderAmount) as OrdAmount, " +
                //    " (Select COUNT(FK_DetailProductID) from OrdersDetails where FK_DetailOrderID = a.OrderID ) as ProductCount, " +
                //    " LEFT (STUFF((Select ', ' + RTRIM(LTRIM(ProductName)) From ProductsData Where ProductID IN (Select FK_DetailProductID from OrdersDetails where FK_DetailOrderID = a.OrderID) FOR XML PATH('')), 1, 1, '' ) , 200) as CartProducts, a.OrderStatus " +
                //    " From OrdersData a Inner Join CustomersData b on a.FK_OrderCustomerID = b.CustomrtID Where a.OrderStatus <> 0 AND a.OrderType=1" +
                //    " Order By a.OrderDate DESC, a.OrderID DESC";
            }

            using (DataTable dtOrder = c.GetDataTable(strQuery))
            {
                //gvOrder.DataSource = dtOrder;
                //gvOrder.DataBind();
                //if (gvOrder.Rows.Count > 0)
                //{
                //    gvOrder.UseAccessibleHeader = true;
                //    gvOrder.HeaderRow.TableSection = TableRowSection.TableHeader;
                //}

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
                litAnch.Text = "<a href=\"order-details.aspx?id=" + e.Row.Cells[0].Text + "\" class=\"gView\" title=\"View/Edit\"></a>";

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
                    case "11":
                        litStatus.Text = "<div class=\"ordDenied\">Rejected by Doctor</div>" + favShopOrder;
                        break;
                    case "12":
                        litStatus.Text = "<div class=\"ordDenied\">No Response to Call</div>" + favShopOrder;
                        break;
                    case "13":
                        litStatus.Text = "<div class=\"ordDenied\">Refund Request by Customer</div>" + favShopOrder;
                        break;
                    case "14":
                        litStatus.Text = "<div class=\"ordDenied\">Refund Request in Process</div>" + favShopOrder;
                        break;
                    case "15":
                        litStatus.Text = "<div class=\"ordDenied\">Refund Request Completed</div>" + favShopOrder;
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