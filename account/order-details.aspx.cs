using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Net;
using System.Net.Http;
using System.IO;
//using PickupRegistrationServiceRef;
using ServiceFinderQueryRef;
using WayBillGenerationRef;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public partial class account_order_details : System.Web.UI.Page
{
    iClass c = new iClass();
    public string customerId, errMsg, orderCount, orderPayMode, shippingCharges, prescriptionStr, paymentslip, rdrUrl, mreq, assignDetails, gobpName;
    public string deviceType;

    public string[] ordData = new string[30]; //18
    public string[] ordCustData = new string[10];
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["ordId"] != null)
            {
                if (Request.QueryString["ref"] != null)
                {
                    if (Request.QueryString["ref"] == "rx")
                    {
                        rdrUrl = "prescription-orders-report.aspx";
                    }
                }
                else
                {
                    rdrUrl = "refund-request-report.aspx";
                }
                GetOrdersData(Convert.ToInt32(Request.QueryString["ordId"]));

                int orderID = Convert.ToInt32(Request.QueryString["ordId"]);
                string query = "SELECT [OrderStatus] FROM [dbo].[OrdersData] WHERE [OrderID] = " + orderID.ToString();
                string orderStatus = c.returnAggregate(query).ToString();

                rbtregtype.SelectedValue = orderStatus.ToString();
                
                object mreFlag = c.GetReqData("OrdersData", "MreqFlag", "OrderID=" + Request.QueryString["ordId"]);
                if (mreFlag != DBNull.Value && mreFlag != null && mreFlag.ToString() != "")
                {
                    if (mreFlag.ToString() == "1")
                    {
                        mreq = "<span class=\"medium clrProcessing bold_weight\">Customer Marked This Order as Monthly Order</span><span class=\"space10\"></span>";
                    }
                }

                //FillGrid();
            }
        }
    }

    public void GetOrdersData(int Idx)
    {
        try
        {
            using (DataTable dtOrder = c.GetDataTable("Select * From OrdersData Where OrderID =" + Idx))
            {
                if (dtOrder.Rows.Count > 0)
                {
                    DataRow bRow = dtOrder.Rows[0];
                    ordData[0] = Idx.ToString();

                    if (bRow["OrderType"] != DBNull.Value && bRow["OrderType"] != null && bRow["OrderType"].ToString() != "")
                    {
                        if (bRow["OrderType"].ToString() == "2")
                        {
                            cartProd.Visible = false;
                        }
                        else
                        {
                            cartProd.Visible = true;
                        }
                    }

                    ordData[1] = Convert.ToDateTime(bRow["OrderDate"]).ToString("dd/MM/yyyy");
                    ordData[11] = Convert.ToDateTime(bRow["OrderDate"]).ToString("hh:mm tt");

                    ordData[2] = bRow["OrderAmount"] != DBNull.Value && bRow["OrderAmount"] != null && bRow["OrderAmount"].ToString() != "" ? Convert.ToDouble(bRow["OrderAmount"]).ToString("0.00") : "NA";
                    ordData[3] = bRow["OrderShipName"].ToString();

                    ordData[20] = bRow["UPIID"] != DBNull.Value && bRow["UPIID"] != null && bRow["UPIID"].ToString() != "" ? bRow["UPIID"].ToString() : "-";

                    ordData[21] = bRow["OrderSalesBillNumber"] != DBNull.Value && bRow["OrderSalesBillNumber"] != null && bRow["OrderSalesBillNumber"].ToString() != "" ? bRow["OrderSalesBillNumber"].ToString() : "-";

                    if (bRow["FK_AddressId"] != DBNull.Value && bRow["FK_AddressId"] != null && bRow["FK_AddressId"].ToString() != "")
                    {
                        using (DataTable dtCustAddr = c.GetDataTable("Select AddressID, AddressFKCustomerID, AddressName, AddressFull, AddressCity, AddressState, AddressPincode, AddressCountry From CustomersAddress Where AddressID=" + bRow["FK_AddressId"]))
                        {
                            if (dtCustAddr.Rows.Count > 0)
                            {
                                DataRow row = dtCustAddr.Rows[0];

                                ordData[4] = row["AddressFull"].ToString() + " <span class=\"bold_weight\" style=\"display:inline-block !important;\"> (" + row["AddressName"].ToString() + ")</span>";

                                ordData[6] = row["AddressCity"].ToString();
                                ordData[7] = row["AddressState"].ToString();
                                ordData[8] = row["AddressPincode"].ToString();
                                ordData[9] = row["AddressCountry"].ToString();
                            }
                        }
                    }
                    else
                    {
                        ordData[4] = bRow["OrderShipAddress"] != DBNull.Value && bRow["OrderShipAddress"] != null && bRow["OrderShipAddress"].ToString() != "" ? bRow["OrderShipAddress"].ToString() : "-";
                        // ordData[5] = bRow["OrderShipAddress2"] != DBNull.Value && bRow["OrderShipAddress2"] != null && bRow["OrderShipAddress2"].ToString() != "" ? bRow["OrderShipAddress2"].ToString() : "-";
                        ordData[6] = bRow["OrderCity"].ToString();
                        ordData[7] = bRow["OrderState"].ToString();
                        ordData[8] = bRow["OrderZipCode"].ToString();
                        ordData[9] = bRow["OrderCountry"].ToString();
                    }
                    ordData[10] = bRow["OrderContactInfo"].ToString();


                    if (bRow["OrderPayMode"] != DBNull.Value && bRow["OrderPayMode"] != null && bRow["OrderPayMode"].ToString() != "")
                    {
                        if (bRow["OrderPaymentTxnId"] != DBNull.Value && bRow["OrderPaymentTxnId"] != null && bRow["OrderPaymentTxnId"].ToString() != "")
                        {
                            ordData[12] = "Online Payment";
                            if (c.IsRecordExist("Select OPL_id From online_payment_logs Where OPL_merchantTranId='" + bRow["OrderPaymentTxnId"].ToString() + "' AND OPL_customer_id=" + bRow["FK_OrderCustomerID"]))
                            {
                                int OPL_id = Convert.ToInt32(c.GetReqData("online_payment_logs", "OPL_id", "OPL_merchantTranId='" + bRow["OrderPaymentTxnId"].ToString() + "' AND OPL_customer_id=" + bRow["FK_OrderCustomerID"]));
                                string payStatus = c.GetReqData("online_payment_logs", "OPL_transtatus", "OPL_id=" + OPL_id).ToString();
                                ordData[13] = payStatus.ToString();
                            }
                            else
                            {
                                ordData[13] = "NA";
                            }
                        }
                        else
                        {
                            switch (Convert.ToInt32(bRow["OrderPayMode"]))
                            {
                                case 1:
                                    ordData[12] = "Cash On Delivery";
                                    ordData[13] = bRow["OrderPayStatus"].ToString() == "0" ? "UnPaid" : "Paid";
                                    break;
                            }
                        }
                    }

                    ordData[14] = bRow["OrderNote"] != DBNull.Value && bRow["OrderNote"] != null && bRow["OrderNote"].ToString() != "" ? bRow["OrderNote"].ToString() : "-";

                    deviceType = bRow["DeviceType"] != DBNull.Value && bRow["DeviceType"] != null && bRow["DeviceType"].ToString() != "" ? bRow["DeviceType"].ToString() : "";

                    ordData[15] = c.returnAggregate("Select SUM(OrdDetailAmount) From OrdersDetails Where FK_DetailOrderID=" + Idx).ToString("0.00");



                    if (Convert.ToDouble(ordData[15].ToString()) > 500)
                    {
                        //shippingCharges = "Shipping Charges = &#8377; 0.00";
                    }
                    else
                    {
                        if (bRow["DeliveryType"] != DBNull.Value && bRow["DeliveryType"] != null && bRow["DeliveryType"].ToString() != "")
                        {
                            if (bRow["DeliveryType"].ToString() == "2")
                            {
                                shippingCharges = "Shipping Charges = &#8377; 0.00 <br/>";
                            }
                            else if (bRow["DeliveryType"].ToString() == "1")
                            {
                                shippingCharges = "Shipping Charges = &#8377; 30.00 <br/>";
                            }
                            else
                            {
                                shippingCharges = "Shipping Charges = &#8377; 0.00 <br/>";
                            }
                        }
                        else
                        {
                            //shippingCharges = "Shipping Charges = &#8377; 30.00 <br/>";
                            shippingCharges = "";
                        }
                    }
                    customerId = bRow["FK_OrderCustomerID"].ToString();

                    object obpId = c.GetReqData("CustomersData", "FK_ObpID", "CustomrtID=" + customerId);
                    if (obpId != DBNull.Value && obpId != null && obpId.ToString() != "" && obpId.ToString() != "0")
                    {
                        gobpName = "GOBP : " + c.GetReqData("OBPData", "OBP_ApplicantName", "OBP_ID=" + obpId).ToString();
                    }

                    if (bRow["DeliveryType"] != DBNull.Value && bRow["DeliveryType"] != null && bRow["DeliveryType"].ToString() != "")
                    {
                        switch (Convert.ToInt32(bRow["DeliveryType"]))
                        {
                            case 1: ordData[16] = "Home Delivery"; break;
                            case 2: ordData[16] = "Self Pickup"; break;
                        }
                    }
                    else
                    {
                        ordData[16] = "Not Selected";
                    }


                    //orderStatus.SelectedValue = bRow["OrderStatus"].ToString();

                    using (DataTable dtCust = c.GetDataTable("Select isnull(CustomerName, '-') as CustomerName, isnull(CustomerMobile, '-') as CustomerMobile, isnull(CustomerEmail, '-') as CustomerEmail, CustomerAddress, CustomerCity, CustomerState, CustomerPincode From CustomersData Where CustomrtID = " + customerId))
                    {
                        if (dtCust.Rows.Count > 0)
                        {
                            DataRow row = dtCust.Rows[0];

                            ordCustData[0] = row["CustomerName"].ToString();
                            ordCustData[1] = row["CustomerMobile"].ToString();
                            ordCustData[2] = row["CustomerEmail"].ToString();

                            //if (c.IsRecordExist("Select AddressID From CustomersAddress Where AddressFKCustomerID=" + customerId + " AND AddressID=" + bRow["FK_AddressId"]))
                            if (bRow["FK_AddressId"] != DBNull.Value && bRow["FK_AddressId"] != null && bRow["FK_AddressId"].ToString() != "")
                            {
                                using (DataTable dtCustAddr = c.GetDataTable("Select AddressID, AddressName, AddressFull, AddressCity, AddressState, AddressPincode, AddressCountry From CustomersAddress Where AddressFKCustomerID=" + customerId + " AND AddressID=" + bRow["FK_AddressId"]))
                                {
                                    if (dtCustAddr.Rows.Count > 0)
                                    {
                                        DataRow cRow = dtCustAddr.Rows[0];

                                        ordCustData[3] = cRow["AddressCity"].ToString();
                                        ordCustData[4] = cRow["AddressState"].ToString();
                                        ordCustData[5] = cRow["AddressPincode"].ToString();
                                        ordCustData[6] = cRow["AddressFull"].ToString() + "<span class=\"bold_weight\" style=\"display:inline-block !important;\"> (" + cRow["AddressName"].ToString() + ")</span>";
                                    }
                                }
                            }
                            else
                            {
                                ordCustData[3] = row["CustomerCity"].ToString();
                                ordCustData[4] = row["CustomerState"].ToString();
                                ordCustData[5] = row["CustomerPincode"].ToString();
                                ordCustData[6] = row["CustomerAddress"].ToString();
                            }
                        }
                    }

                    // OrderAssignStatus 0 > Pending, 1 > Accepted, 2 > Rejected, 5 > In Process, 6 > Shipped, 7 > Delivered
                    int ordAssignStatus = Convert.ToInt32(c.GetReqData("OrdersAssign", "OrdAssignStatus", "FK_OrderID=" + Idx));


                    if (bRow["OrderStatus"].ToString() == "2")
                    {
                        ordCanc.Visible = true;
                        if (bRow["FK_ReasonID"] != DBNull.Value && bRow["FK_ReasonID"] != null && bRow["FK_ReasonID"].ToString() != "")
                        {
                            string reason = c.GetReqData("CancelReasons", "ReasonTitle", "ReasonID=" + bRow["FK_ReasonID"]).ToString();
                            ordData[17] = reason.ToString();
                        }
                        else
                        {
                            ordData[17] = "Reason not found";
                        }
                    }

                    if (bRow["OrderStatus"].ToString() == "10")
                    {
                        ordReturned.Visible = true;
                        ordData[18] = c.GetReqData("OrdersAssign", "ReturnReason", "FK_OrderID=" + Request.QueryString["ordId"] + " AND ReturnReason IS NOT NULL AND ReturnReason<>''").ToString();
                    }


                }
            }


            using (DataTable dtProduct = c.GetDataTable("Select a.OrdDetailID, a.FK_DetailProductID, a.OrdDetailQTY, 'Rs. ' + Convert(varchar(20), a.OrdDetailPrice)  as OrigPrice, 'Rs. ' + Convert(varchar(20), a.OrdDetailAmount) as OrdAmount , a.OrdDetailSKU, b.ProductName from OrdersDetails a Inner Join ProductsData b on a.FK_DetailProductID = b.ProductID where a.FK_DetailOrderID =" + Idx))
            {

                gvOrderDetails.DataSource = dtProduct;
                gvOrderDetails.DataBind();

                if (gvOrderDetails.Rows.Count > 0)
                {
                    gvOrderDetails.UseAccessibleHeader = true;
                    gvOrderDetails.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }

            orderCount = c.returnAggregate("Select COUNT(a.OrderID) From OrdersData a Join CustomersData b On b.CustomrtID = a.FK_OrderCustomerID Where b.CustomrtID=" + customerId + " AND b.delMark = 0 AND a.OrderStatus<>2").ToString();

            if (c.IsRecordExist("Select PrescriptionID From OrderPrescriptions Where FK_OrderID=" + Idx))
            {
                StringBuilder strMarkup = new StringBuilder();

                using (DataTable dtRx = c.GetDataTable("Select PrescriptionID, PrescriptionName, PrescriptionStatus From OrderPrescriptions Where FK_OrderID=" + Idx))
                {
                    if (dtRx.Rows.Count > 0)
                    {
                        int bCount = 0;
                        string rPath = c.ReturnHttp();
                        strMarkup.Append("<div class=\"card-header\"><h3 class=\"large colorLightBlue\">Prescription : </h3></div>");
                        strMarkup.Append("<div class=\"card-body\">");
                        foreach (DataRow prow in dtRx.Rows)
                        {
                            strMarkup.Append("<div class=\"imgBox txtCenter\">");
                            strMarkup.Append("<div class=\"pad-5\">");
                            strMarkup.Append("<div class=\"border1 posRelative\">");
                            if (prow["PrescriptionStatus"].ToString() == "2")
                            {
                                strMarkup.Append("<div class=\"absRejected\">Rejected</div>");
                            }
                            if (prow["PrescriptionStatus"].ToString() == "1")
                            {
                                strMarkup.Append("<div class=\"absAccepted\">Accepted</div>");
                            }
                            strMarkup.Append("<div class=\"pad-5\">");
                            strMarkup.Append("<div class=\"imgContainer\">");
                            if (prow["PrescriptionName"].ToString().Contains("http"))
                            {
                                strMarkup.Append("<a href=\"" + prow["PrescriptionName"].ToString() + "\" data-fancybox=\"rxGroup\"><img src=\"" + prow["PrescriptionName"].ToString() + "\" class=\"width100\" /></a>");
                            }
                            else
                            {
                                strMarkup.Append("<a href=\"" + rPath + "upload/prescriptions/" + prow["PrescriptionName"] + "\" data-fancybox=\"rxGroup\"><img src=\"" + rPath + "upload/prescriptions/" + prow["PrescriptionName"] + "\" class=\"width100\" /></a>");
                            }
                            strMarkup.Append("</div>");
                            strMarkup.Append("</div>");
                            strMarkup.Append("</div>");
                            strMarkup.Append("<span class=\"space5\"></span>");
                            if (prow["PrescriptionStatus"].ToString() == "0")
                            {
                                if (Request.QueryString["ref"] != null)
                                {
                                    if (Request.QueryString["ref"] == "rx")
                                    {
                                        //strMarkup.Append("<a href=\"order-details.aspx?id=" + Request.QueryString["id"] + "&type=accept&prId=" + prow["PrescriptionID"].ToString() + "&ref=rx\" class=\"buttonBlue mrgRgt10\">Accept</a>");
                                        //strMarkup.Append("<a href=\"order-details.aspx?id=" + Request.QueryString["id"] + "&type=reject&prId=" + prow["PrescriptionID"].ToString() + "&ref=rx\" class=\"buttonDel\">Reject</a>");
                                    }
                                }
                                else
                                {
                                    //strMarkup.Append("<a href=\"order-details.aspx?id=" + Request.QueryString["id"] + "&type=accept&prId=" + prow["PrescriptionID"].ToString() + "\" class=\"buttonBlue mrgRgt10\">Accept</a>");
                                    //strMarkup.Append("<a href=\"order-details.aspx?id=" + Request.QueryString["id"] + "&type=reject&prId=" + prow["PrescriptionID"].ToString() + "\" class=\"buttonDel\">Reject</a>");
                                }
                            }
                            strMarkup.Append("</div>");
                            strMarkup.Append("</div>");

                            bCount++;

                            if ((bCount % 3) == 0)
                            {
                                strMarkup.Append("<div class=\"float_clear\"></div>");
                            }
                        }
                        strMarkup.Append("<div class=\"float_clear\"></div>");
                        strMarkup.Append("</div>");
                    }
                }
                strMarkup.Append("<span class=\"space20\"></span>");
                prescriptionStr = strMarkup.ToString();
            }

            orderPayMode = c.returnAggregate("SELECT [OrderPayMode] FROM [dbo].[OrdersData] WHERE [OrderID] = " + Idx).ToString();
            if (Convert.ToInt32(orderPayMode) == 2 || Convert.ToInt32(orderPayMode) == 0)
            {

                //Payment Slip (Updated On 01-08-2023 by Shreyasha Patil)
                if (c.IsRecordExist("Select OrderID From OrdersData Where OrderID=" + Idx))
                {
                    StringBuilder strMarkup = new StringBuilder();

                    using (DataTable dtRx = c.GetDataTable("Select OrderID, PaymentSlip From OrdersData Where OrderID=" + Idx))
                    {
                        if (dtRx.Rows.Count > 0)
                        {
                            int bCount = 0;
                            string rPath = c.ReturnHttp();
                            strMarkup.Append("<div class=\"card-header\"><h3 class=\"large colorLightBlue\">Payment Slip : </h3></div>");
                            strMarkup.Append("<div class=\"card-body\">");
                            foreach (DataRow prow in dtRx.Rows)
                            {
                                strMarkup.Append("<div class=\"imgBox txtCenter\">");
                                strMarkup.Append("<div class=\"pad-5\">");
                                strMarkup.Append("<div class=\"border1 posRelative\">");
                                strMarkup.Append("<div class=\"pad-5\">");
                                strMarkup.Append("<div class=\"imgContainer\">");
                                //strMarkup.Append("<a href=\"" + rPath + "upload/prescriptions/" + prow["PrescriptionName"] + "\" data-fancybox=\"rxGroup\"><img src=\"" + rPath + "upload/prescriptions/" + prow["PrescriptionName"] + "\" class=\"width100\" /></a>");
                                if (prow["PaymentSlip"].ToString().Contains("http"))
                                {
                                    strMarkup.Append("<a href=\"" + prow["PaymentSlip"].ToString() + "\" data-fancybox=\"rxGroup\"><img src=\"" + prow["PaymentSlip"].ToString() + "\" class=\"width100\" /></a>");
                                }
                                else
                                {
                                    strMarkup.Append("<a href=\"" + rPath + "upload/paymentslip/" + prow["PaymentSlip"] + "\" data-fancybox=\"rxGroup\"><img src=\"" + rPath + "upload/paymentslip/" + prow["PaymentSlip"] + "\" class=\"width100\" /></a>");
                                }
                                strMarkup.Append("</div>");
                                strMarkup.Append("</div>");
                                strMarkup.Append("</div>");
                                strMarkup.Append("</div>");
                                strMarkup.Append("</div>");

                                bCount++;

                                if ((bCount % 3) == 0)
                                {
                                    strMarkup.Append("<div class=\"float_clear\"></div>");
                                }
                            }
                            strMarkup.Append("<div class=\"float_clear\"></div>");
                            strMarkup.Append("</div>");
                        }
                    }
                    strMarkup.Append("<span class=\"space20\"></span>");
                    paymentslip = strMarkup.ToString();
                }
            }

            // Order assignment details
            using (DataTable dtOrdAssign = c.GetDataTable("Select a.OrdAssignID, a.OrdAssignDate, a.FK_OrderID, a.FK_ReasonID, a.Fk_FranchID, " +
                " a.OrdAssignStatus, a.OrdReAssign, c.FranchName, c.FranchShopCode From OrdersAssign a Inner Join OrdersData b " +
                " On a.FK_OrderID=b.OrderID Inner Join FranchiseeData c On a.Fk_FranchID=c.FranchID Where a.FK_OrderID=" + Idx))
            {
                if (dtOrdAssign.Rows.Count > 0)
                {
                    StringBuilder strMarkup = new StringBuilder();

                    strMarkup.Append("<div class=\"card-header\">");
                    strMarkup.Append("<h3 class=\"large colorLightBlue\">Order Assigning Details</h3>");
                    strMarkup.Append("</div>");
                    strMarkup.Append("<div class=\"card-body\">");
                    strMarkup.Append("<table class=\"table table-bordered\">");
                    strMarkup.Append("<tr>");
                    strMarkup.Append("<th>Order Id</th>");
                    strMarkup.Append("<th>Assigned Date</th>");
                    strMarkup.Append("<th>Days Passed</th>");
                    strMarkup.Append("<th>Assigned To</th>");
                    strMarkup.Append("<th>Shop Code</th>");
                    strMarkup.Append("<th>Status</th>");
                    strMarkup.Append("<th>Order Cancel Reason</th>");
                    strMarkup.Append("<th>Re-assigned</th>");
                    strMarkup.Append("<th>Admin <br>Delete</th>");
                    strMarkup.Append("</tr>");

                    foreach (DataRow aRow in dtOrdAssign.Rows)
                    {
                        strMarkup.Append("<tr>");
                        strMarkup.Append("<td>#" + aRow["FK_OrderID"].ToString() + "</td>");
                        strMarkup.Append("<td>" + Convert.ToDateTime(aRow["OrdAssignDate"]).ToString("dd/MM/yyyy hh:mm tt") + "</td>");
                        strMarkup.Append("<td>" + c.GetTimeSpan(Convert.ToDateTime(aRow["OrdAssignDate"])) + "</td>");
                        strMarkup.Append("<td>" + aRow["FranchName"].ToString() + "</td>");
                        strMarkup.Append("<td>" + aRow["FranchShopCode"].ToString() + "</td>");
                        string ordAssignStatus1 = "", classname = "";
                        switch (Convert.ToInt32(aRow["OrdAssignStatus"]))
                        {
                            case 0: ordAssignStatus1 = "Pending"; classname = "clrPending"; break;
                            case 1: ordAssignStatus1 = "Accepted"; classname = "clrAccepted"; break;
                            case 2: ordAssignStatus1 = "Rejected"; classname = "clrRejected"; break;
                            case 5: ordAssignStatus1 = "In Process"; classname = "clrProcessing"; break;
                            case 6: ordAssignStatus1 = "Shipped"; classname = "clrShipped"; break;
                            case 7: ordAssignStatus1 = "Delivered"; classname = "clrDelivered"; break;
                        }
                        string actLink = "";
                        if (aRow["OrdAssignStatus"].ToString() == "2" && aRow["OrdReAssign"].ToString() == "0")
                        {
                            actLink = "<br/><a href=\"assign-order.aspx?id=" + Request.QueryString["ordId"] + "&type=delAct&assId=" + aRow["OrdAssignID"] + "\" class=\"clrShipped\">Delete Activity</a>";
                        }
                        strMarkup.Append("<td class=\"" + classname + "\">" + ordAssignStatus1.ToString() + actLink + "</td>");
                        if (aRow["FK_ReasonID"] != DBNull.Value && aRow["FK_ReasonID"] != null || aRow["FK_ReasonID"].ToString() != "")
                        {
                            if (aRow["FK_ReasonID"].ToString() != "0")
                            {
                                string reason = c.GetReqData("CancelReasons", "ReasonTitle", "ReasonID=" + aRow["FK_ReasonID"].ToString()).ToString();
                                strMarkup.Append("<td><span style=\"color:red; font-weight:600;\">" + reason + "</span></td>");
                            }
                            else
                            {
                                strMarkup.Append("<td>-</td>");
                            }
                        }
                        else
                        {
                            strMarkup.Append("<td>-</td>");
                        }
                        string ordReassign = aRow["OrdReAssign"].ToString() == "0" ? "No" : "Yes";
                        strMarkup.Append("<td>" + ordReassign.ToString() + "</td>");
                        if (aRow["OrdAssignStatus"].ToString() == "6" || aRow["OrdAssignStatus"].ToString() == "7" || aRow["OrdReAssign"].ToString() == "1")
                        {
                            strMarkup.Append("<td><a class=\"\"></a></td>");
                        }
                        else
                        {
                            strMarkup.Append("<td><a href=\"assign-order.aspx?id=" + Request.QueryString["ordId"] + "&activity=adminDel&assignId=" + aRow["OrdAssignID"] + "\" class=\"deleteProd\" OnClick=\"return confirm('Are you sure you want to delete this?');\"></a></td>");
                        }
                        strMarkup.Append("</tr>");
                    }

                    strMarkup.Append("</table>");
                    strMarkup.Append("</div>");

                    assignDetails = strMarkup.ToString();
                }
            }
        }
        catch (Exception ex)
        {

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetOrdersData", ex.Message.ToString());
            return;

        }
    }

    protected void gvOrderDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (c.IsRecordExist("Select ProdOptionID From ProductOptions Where FK_ProductID=" + e.Row.Cells[1].Text))
                {
                    int ProdOptId = Convert.ToInt32(c.GetReqData("OrderProductOptions", "FK_ProdOptionID", "FK_OrdDetailID=" + e.Row.Cells[0].Text));
                    int optGroupId = Convert.ToInt32(c.GetReqData("ProductOptions", "FK_OptionGroupID", "ProdOptionID=" + ProdOptId));
                    int optId = Convert.ToInt32(c.GetReqData("ProductOptions", "FK_OptionID", "ProdOptionID=" + ProdOptId));
                    string groupName = c.GetReqData("OptionGroups", "OptionGroupName", "OptionGroupID=" + optGroupId).ToString();
                    string optName = c.GetReqData("OptionsData", "OptionName", "OptionID=" + optId + " AND FK_OptionGroupID=" + optGroupId).ToString();
                    e.Row.Cells[2].Text += "<span class=\"space10\"></span> <span class=\"space1\"></span><span class=\"text-bold text-primary\">" + groupName + " : " + optName + "</span>";
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvOrderDetails_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    protected void btnRefund_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["ordId"] != null && rbtregtype.SelectedValue == "14")
            {
                c.ExecuteQuery("UPDATE [dbo].[OrdersData] SET [OrderStatus] = 14 WHERE [OrderID] = " + Request.QueryString["ordId"].ToString());
            }
            else if (Request.QueryString["ordId"] != null && rbtregtype.SelectedValue == "15")
            {
                c.ExecuteQuery("UPDATE [dbo].[OrdersData] SET [OrderStatus] = 15 WHERE [OrderID] = " + Request.QueryString["ordId"].ToString());
            }

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Refund Process Updated Successfully');", true);

            if (Request.QueryString["ordId"] != null)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('refund-request-report.aspx?type=" + Request.QueryString["type"] + "&ordId=" + Request.QueryString["ordId"] + "', 2000);", true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('refund-request-report.aspx?type=" + Request.QueryString["ordId"] + "', 2000);", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnRefund_Click", ex.Message.ToString());
            return;
        }
    }

    //protected void rbtregtype_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {

    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occured While Processing');", true);
    //        c.ErrorLogHandler(this.ToString(), "rbtregtype_SelectedIndexChanged", ex.Message.ToString());
    //        return;
    //    }
    //}
}