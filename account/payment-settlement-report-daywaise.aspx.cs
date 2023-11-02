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
using RestSharp;

public partial class account_payment_settlement_report_daywaise : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg, apiResp, pgTitle;
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
                    FillRefundSettleGrid();
                    pgTitle = "Payment Settlement";

                    heaadH2.Visible = false;
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
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    protected void gvSettlement_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridViewRow gRow = (GridViewRow)((Button)e.CommandSource).NamingContainer;
            if (e.CommandName == "gvVerify")
            {
                int settlId = Convert.ToInt32(gRow.Cells[0].Text);
                c.ExecuteQuery("Update SettlementData Set SettlementVerify=1 Where SettlementID=" + settlId);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Marked as verified');", true);
            }

            FillGrid();
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

    private void FillRefundSettleGrid()
    {
        try
        {
            using (DataTable dtSettlement = c.GetDataTable("Select OPL_id, OLP_amount, Convert(varchar(20), OPL_datetime, 103) as opDate, " +
                " (Convert(varchar(20), OPL_datetime, 103) + ' ' + (RIGHT(Convert(VARCHAR(20), OPL_datetime,100),7) )) as oplDate, " +
                " OPL_merchantTranId, OPL_transtatus, OLP_order_no, OLP_RazorPayFee, OLP_RazorPayGST, OLP_RazorPayAmount, " +
                " OLP_device_type From online_payment_logs Where refund_comment='" + Request.QueryString["setlId"] + "'"))
            {
                gvRefundSettleCount.DataSource = dtSettlement;
                gvRefundSettleCount.DataBind();

                if (dtSettlement.Rows.Count > 0)
                {
                    gvRefundSettleCount.UseAccessibleHeader = true;
                    gvRefundSettleCount.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvSettlecount_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litShop = (Literal)e.Row.FindControl("litShop");
                string orderId = e.Row.Cells[6].Text;
                int shopId = 0;
                string shopInfo = "";
                if (c.IsRecordExist("Select OrderID From OrdersData Where OrderID=" + orderId))
                {
                    
                    if (c.IsRecordExist("Select OrdAssignID From OrdersAssign Where FK_OrderID=" + orderId))
                    {
                        shopId = Convert.ToInt32(c.GetReqData("OrdersAssign", "Fk_FranchID", "FK_OrderID=" + orderId + " AND OrdAssignStatus>=0 AND OrdReAssign=0"));

                        if (shopId > 0)
                        {
                            // Get Shop Details
                            shopInfo = c.GetReqData("FranchiseeData", "FranchShopCode+', '+ FranchName", "FranchID=" + shopId).ToString();

                            // Check is this Order is Cancelled by customer (OrderStatus=2) 12-May-2022
                            if (c.IsRecordExist("Select OrderID From OrdersData Where OrderID=" + orderId + " and OrderStatus=2"))
                            {
                                shopInfo = shopInfo + "<div class=\"ordDenied\">Order Cancelled by Customer</div>";
                                litShop.Text = shopInfo.ToString();
                                return;
                            }
                            
                            string shopStatus = "";
                            int ordAssignStatus = Convert.ToInt32(c.GetReqData("OrdersAssign", "OrdAssignStatus", "FK_OrderID=" + orderId + " AND Fk_FranchID=" + shopId + " AND OrdReAssign=0"));
                            switch (ordAssignStatus)
                            {
                                case 0: shopStatus = "<div class=\"ordNew\">Accepted</div>"; break;
                                case 1:
                                    shopStatus = "<div class=\"ordAccepted\">Accepted</div>";
                                    break;
                                case 2:
                                    shopStatus = "<div class=\"ordDenied\">Rejected</div>";
                                    break;
                                case 5:
                                    shopStatus = "<div class=\"ordProcessing\">Inprocess</div>";
                                    break;
                                case 6:
                                    shopStatus = "<div class=\"ordShipped\">Shipped</div>";
                                    break;
                                case 7:
                                    shopStatus = "<div class=\"ordDelivered\">Delivered</div>";
                                    break;
                                case 10:
                                    shopStatus = "<div class=\"ordDenied\">Returned By Customer</div>";
                                    break;
                                case 13:
                                    shopStatus = "<div class=\"ordNew\">Refund Request By Customer</div>";
                                    break;
                                case 14:
                                    shopStatus = "<div class=\"ordAccepted\">Refund Inprocess</div>";
                                    break;
                                case 15:
                                    shopStatus = "<div class=\"ordProcessing\">Refund Completed</div>";
                                    break;
                            }
                            shopInfo = shopInfo + shopStatus;
                        }
                        else
                        {
                            shopInfo = "Shop (with status) Not Found againt this order";
                        }
                    }
                    else
                    {
                        

                        // Code by Vinayak 12-May-2022 (If OrderId ref. not found into OrderAssign Table, then get its STATUS from OrdersData table)
                        //shopInfo = "Order Assign Record Not Found";

                        int orderStatusFlag = Convert.ToInt32(c.GetReqData("OrdersData", "OrderStatus", "OrderID=" + orderId));
                        switch (orderStatusFlag)
                        {
                            //case 0: ordersDataStatus = "<div class=\"ordNew\">Accepted</div>"; break;
                            case 1:
                                shopInfo = "<div class=\"ordAccepted\">Order placed successfully</div>";
                                break;
                            case 2:
                                shopInfo = "<div class=\"ordDenied\">Order Cancelled by Customer</div>";
                                break;
                            case 3:
                                shopInfo = "<div class=\"ordProcessing\">Order accepted by Admin</div>";
                                break;
                            case 4:
                                shopInfo = "<div class=\"ordDenied\">Order denied by Admin</div>";
                                break;
                            case 5:
                                shopInfo = "<div class=\"ordProcessing\">Order assigned & accepted by the shop</div>";
                                break;
                            case 6:
                                shopInfo = "<div class=\"ordDelivered\">Order shipped</div>";
                                break;
                            case 7:
                                shopInfo = "<div class=\"ordDenied\">Order Delivered</div>";
                                break;
                            case 13:
                                shopInfo = "<div class=\"ordNew\">Refund Request By Customer</div>";
                                break;
                            case 14:
                                shopInfo = "<div class=\"ordAccepted\">Refund Inprocess</div>";
                                break;
                            case 15:
                                shopInfo = "<div class=\"ordProcessing\">Refund Completed</div>";
                                break;
                        }


                    }
                }
                else
                {
                    shopInfo = "Order Details Not Found";
                }
                litShop.Text = shopInfo.ToString();
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
    protected void gvRefundSettleCount_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litShop = (Literal)e.Row.FindControl("litShop");
                string orderId = e.Row.Cells[6].Text;
                int shopId = 0;
                string shopInfo = "";
                if (c.IsRecordExist("Select OrderID From OrdersData Where OrderID=" + orderId))
                {

                    if (c.IsRecordExist("Select OrdAssignID From OrdersAssign Where FK_OrderID=" + orderId))
                    {
                        shopId = Convert.ToInt32(c.GetReqData("OrdersAssign", "Fk_FranchID", "FK_OrderID=" + orderId + " AND OrdAssignStatus>=0 AND OrdReAssign=0"));

                        if (shopId > 0)
                        {
                            // Get Shop Details
                            shopInfo = c.GetReqData("FranchiseeData", "FranchShopCode+', '+ FranchName", "FranchID=" + shopId).ToString();

                            // Check is this Order is Cancelled by customer (OrderStatus=2) 12-May-2022
                            if (c.IsRecordExist("Select OrderID From OrdersData Where OrderID=" + orderId + " and OrderStatus=2"))
                            {
                                shopInfo = shopInfo + "<div class=\"ordDenied\">Order Cancelled by Customer</div>";
                                litShop.Text = shopInfo.ToString();
                                return;
                            }

                            string shopStatus = "";
                            int ordAssignStatus = Convert.ToInt32(c.GetReqData("OrdersAssign", "OrdAssignStatus", "FK_OrderID=" + orderId + " AND Fk_FranchID=" + shopId + " AND OrdReAssign=0"));
                            switch (ordAssignStatus)
                            {
                                case 0: shopStatus = "<div class=\"ordNew\">Accepted</div>"; break;
                                case 1:
                                    shopStatus = "<div class=\"ordAccepted\">Accepted</div>";
                                    break;
                                case 2:
                                    shopStatus = "<div class=\"ordDenied\">Rejected</div>";
                                    break;
                                case 5:
                                    shopStatus = "<div class=\"ordProcessing\">Inprocess</div>";
                                    break;
                                case 6:
                                    shopStatus = "<div class=\"ordShipped\">Shipped</div>";
                                    break;
                                case 7:
                                    shopStatus = "<div class=\"ordDelivered\">Delivered</div>";
                                    break;
                                case 10:
                                    shopStatus = "<div class=\"ordDenied\">Returned By Customer</div>";
                                    break;
                                case 13:
                                    shopStatus = "<div class=\"ordNew\">Refund Request By Customer</div>";
                                    break;
                                case 14:
                                    shopStatus = "<div class=\"ordAccepted\">Refund Inprocess</div>";
                                    break;
                                case 15:
                                    shopStatus = "<div class=\"ordProcessing\">Refund Completed</div>";
                                    break;
                            }
                            shopInfo = shopInfo + shopStatus;
                        }
                        else
                        {
                            shopInfo = "Shop (with status) Not Found againt this order";
                        }
                    }
                    else
                    {


                        // Code by Vinayak 12-May-2022 (If OrderId ref. not found into OrderAssign Table, then get its STATUS from OrdersData table)
                        //shopInfo = "Order Assign Record Not Found";

                        int orderStatusFlag = Convert.ToInt32(c.GetReqData("OrdersData", "OrderStatus", "OrderID=" + orderId));
                        switch (orderStatusFlag)
                        {
                            //case 0: ordersDataStatus = "<div class=\"ordNew\">Accepted</div>"; break;
                            case 1:
                                shopInfo = "<div class=\"ordAccepted\">Order placed successfully</div>";
                                break;
                            case 2:
                                shopInfo = "<div class=\"ordDenied\">Order Cancelled by Customer</div>";
                                break;
                            case 3:
                                shopInfo = "<div class=\"ordProcessing\">Order accepted by Admin</div>";
                                break;
                            case 4:
                                shopInfo = "<div class=\"ordDenied\">Order denied by Admin</div>";
                                break;
                            case 5:
                                shopInfo = "<div class=\"ordProcessing\">Order assigned & accepted by the shop</div>";
                                break;
                            case 6:
                                shopInfo = "<div class=\"ordDelivered\">Order shipped</div>";
                                break;
                            case 7:
                                shopInfo = "<div class=\"ordDenied\">Order Delivered</div>";
                                break;
                            case 13:
                                shopInfo = "<div class=\"ordNew\">Refund Request By Customer</div>";
                                break;
                            case 14:
                                shopInfo = "<div class=\"ordAccepted\">Refund Inprocess</div>";
                                break;
                            case 15:
                                shopInfo = "<div class=\"ordProcessing\">Refund Completed</div>";
                                break;
                        }


                    }
                }
                else
                {
                    shopInfo = "Order Details Not Found";
                }
                litShop.Text = shopInfo.ToString();
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    [System.Web.Services.WebMethod]
    public static string FetchSettlement(string Id)
    {
        iClass c = new iClass();
        try
        {
            //string apiUrl = String.Format("https://www.genericartmedicine.com/api_ecom/Razorpay_settlement_by_id");
            //WebRequest request = WebRequest.Create(apiUrl);
            //request.Method = "POST";
            ////request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentType = "multipart/form-data;";

            //request.Headers["20"] = "*/*";
            //request.Headers["22"] = "*";
            //request.Headers["23"] = "en-US,en;q=0.8,hi;q=0.6";
            //request.Headers["0"] = "no-cache";

            //string postData = "{\"SettlementID\":\"" + Id + "\"}";

            //using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            //{
            //    streamWriter.Write(postData);
            //    streamWriter.Flush();
            //    streamWriter.Close();

            //    var httpResponse = (HttpWebResponse)request.GetResponse();
            //    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //    {
                    
            //        var result = streamReader.ReadToEnd();
            //        //return result;

            //        string resultStr = result.ToString();
            //        if (resultStr.Contains("False") == true)
            //        {
            //            c.ErrorLogHandler("payment-settlement-report-daywaise", "FetchSettlement", resultStr);
            //            return "fail";
            //        }
            //        else
            //        {
            //            return "success";
            //        }
            //    }
            //}


            var client = new RestClient("https://www.genericartmedicine.com/api_ecom/Razorpay_settlement_by_id");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("SettlementID", "" + Id + "");
            IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);
            if (response.Content.ToString().Contains("False"))
            {
                c.ErrorLogHandler("payment-settlement-report-daywaise", "FetchSettlement", response.Content);
                return "fail";
            }
            else
            {
                return "success";
            }
        }
        catch (Exception ex)
        {
            c.ErrorLogHandler("payment-settlement-report-daywaise", "FetchSettlement", ex.Message.ToString());
            return "Error Occured!";
        }
        
        
    }
}