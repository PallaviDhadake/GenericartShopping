using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class supportteam_ordered_customer_details : System.Web.UI.Page
{
    iClass c = new iClass();
    public string customerId, errMsg, orderCount, shippingCharges, prescriptionStr, rdrUrl, mreq, apiResponse, gobpName;
    public string deviceType, paymentslip;

    public string[] ordData = new string[20]; //18
    public string[] ordCustData = new string[10];
    public string RefundAlert = "";


    protected void Page_Load(object sender, EventArgs e)
    {
        btnRefresh.Attributes.Add("onclick", "this.disabled=true; this.value='Processing...';" + ClientScript.GetPostBackEventReference(btnRefresh, null) + ";");

        if (!IsPostBack)
        {
            if (Request.QueryString["id"] != null)
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
                    rdrUrl = "order-report.aspx";
                }
                GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));
                btnAssignOrder.Visible = false;

                int ordStatus = Convert.ToInt32(c.GetReqData("OrdersData", "OrderStatus", "OrderID=" + Request.QueryString["id"]));
                if (ordStatus == 3 || ordStatus == 4 || ordStatus == 5 || ordStatus == 6 || ordStatus == 7) // accepted, denied, inprocess, shipped, delivered
                {
                    btnSubmit.Visible = false;
                    btnDeny.Visible = false;
                    btnAssignOrder.Visible = true;
                }

                if (ordStatus == 4)
                {
                    btnAssignOrder.Visible = false;
                    btnHistory.Visible = true;
                }

                object mreFlag = c.GetReqData("OrdersData", "MreqFlag", "OrderID=" + Request.QueryString["id"]);
                if (mreFlag != DBNull.Value && mreFlag != null && mreFlag.ToString() != "")
                {
                    if (mreFlag.ToString() == "1")
                    {
                        mreq = "<span class=\"medium clrProcessing bold_weight\">Customer Marked This Order as Monthly Order</span><span class=\"space10\"></span>";
                    }
                }

                //FillGrid();

                if (Request.QueryString["type"] != null && Request.QueryString["prId"] != null)
                {
                    if (Request.QueryString["type"] == "reject")
                    {
                        c.ExecuteQuery("Update OrderPrescriptions Set PrescriptionStatus=2 Where PrescriptionID=" + Request.QueryString["prId"]);
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Prescription Rejected');", true);
                    }
                    else
                    {
                        //accept
                        c.ExecuteQuery("Update OrderPrescriptions Set PrescriptionStatus=1 Where PrescriptionID=" + Request.QueryString["prId"]);
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Prescription Accepted');", true);
                    }
                    string redirect = "";
                    if (Request.QueryString["ref"] != null)
                    {
                        if (Request.QueryString["ref"] == "rx")
                        {
                            redirect = "ordered-customer-details.aspx?id=" + Request.QueryString["id"] + "&ref=rx";
                        }
                    }
                    else
                    {
                        redirect = "ordered-customer-details.aspx?id=" + Request.QueryString["id"];
                    }
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('" + redirect + "', 2000);", true);
                }
            }

            //Refund Button active status (Code by Vinayak 10/May/2022)
            btnRefund.Enabled = RefundStatus();
            btnRefund.ToolTip = btnRefund.Enabled == false ? "This Order is yet to settle" : "Initiate Refund Process";

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

                    ordData[2] = Convert.ToDouble(bRow["OrderAmount"]).ToString("0.00");
                    ordData[3] = bRow["OrderShipName"].ToString();

                    // Coins Consume
                    ordData[18] = bRow["OrderCoinsValue"] != DBNull.Value && bRow["OrderCoinsValue"] != null && bRow["OrderCoinsValue"].ToString() != "" ? bRow["OrderCoinsValue"].ToString() : "0";

                    // Shipping Charges 20-June-22
                    ordData[19] = bRow["OrderShippingAmount"] != DBNull.Value && bRow["OrderShippingAmount"] != null && bRow["OrderShippingAmount"].ToString() != "" ? bRow["OrderShippingAmount"].ToString() : "0";

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
                            //btnRefresh.Visible = true;
                            ordData[12] = "Online Payment - Razorpay";
                            if (c.IsRecordExist("Select OPL_id From online_payment_logs Where OPL_merchantTranId='" + bRow["OrderPaymentTxnId"].ToString() + "' AND OPL_customer_id=" + bRow["FK_OrderCustomerID"]))
                            {
                                int OPL_id = Convert.ToInt32(c.GetReqData("online_payment_logs", "OPL_id", "OPL_merchantTranId='" + bRow["OrderPaymentTxnId"].ToString() + "' AND OPL_customer_id=" + bRow["FK_OrderCustomerID"]));
                                string payStatus = c.GetReqData("online_payment_logs", "OPL_transtatus", "OPL_id=" + OPL_id).ToString();
                                ordData[13] = payStatus.ToString();
                            }
                            else
                            {
                                ordData[13] = bRow["OrderPayStatus"].ToString() == "0" ? "UnPaid" : "Paid";
                            }

                            if (bRow["OrderPayStatus"].ToString() == "0")
                            {
                                btnRefresh.Visible = true;
                            }
                            else
                            {
                                btnRefresh.Visible = false;
                            }
                        }
                        else
                        {
                            btnRefresh.Visible = false;
                            switch (Convert.ToInt32(bRow["OrderPayMode"]))
                            {
                                case 0:
                                    ordData[12] = "Not Selected";
                                    ordData[13] = bRow["OrderPayStatus"].ToString() == "0" ? "UnPaid" : "Paid";
                                    //if (bRow["OrderPayStatus"].ToString() == "0")
                                    //    btnRefresh.Visible = true;
                                    //else
                                    //    btnRefresh.Visible = false;
                                    break;
                                case 1:
                                    ordData[12] = "Cash On Delivery";
                                    ordData[13] = bRow["OrderPayStatus"].ToString() == "0" ? "UnPaid" : "Paid";
                                    if (bRow["OrderPayStatus"].ToString() == "0")
                                        btnRefresh.Visible = false;
                                    break;
                                case 2:
                                    ordData[12] = "Online Payment - Razorpay";
                                    ordData[13] = bRow["OrderPayStatus"].ToString() == "0" ? "UnPaid" : "Paid";
                                    //if (bRow["OrderPayStatus"].ToString() == "0")
                                    //    btnRefresh.Visible = true;
                                    //else
                                    //    btnRefresh.Visible = false;
                                    break;
                            }


                        }
                    }
                    else
                    {
                        ordData[12] = "Not Selected";
                        ordData[13] = bRow["OrderPayStatus"].ToString() == "0" ? "UnPaid" : "Paid";
                        if (bRow["OrderPaymentTxnId"] != DBNull.Value && bRow["OrderPaymentTxnId"] != null && bRow["OrderPaymentTxnId"].ToString() != "")
                        {
                            if (bRow["OrderPayStatus"].ToString() == "0")
                            {
                                btnRefresh.Visible = true;
                            }
                            else
                            {
                                btnRefresh.Visible = false;
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
                    if (ordAssignStatus == 1 || ordAssignStatus == 5 || ordAssignStatus == 6 || ordAssignStatus == 7)
                    {
                        btnAssignOrder.Text = "View Assign Details";
                    }
                    else
                    {
                        btnAssignOrder.Text = "Assign Order";
                    }

                    if (bRow["OrderStatus"].ToString() == "2")
                    {
                        ordCanc.Visible = true;
                        btnSubmit.Visible = false;
                        btnDeny.Visible = false;
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
                        btnSubmit.Visible = false;
                        btnDeny.Visible = false;
                        ordData[18] = c.GetReqData("OrdersAssign", "ReturnReason", "FK_OrderID=" + Request.QueryString["id"] + " AND ReturnReason IS NOT NULL AND ReturnReason<>''").ToString();
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
                                        strMarkup.Append("<a href=\"order-details.aspx?id=" + Request.QueryString["id"] + "&type=accept&prId=" + prow["PrescriptionID"].ToString() + "&ref=rx\" class=\"buttonBlue mrgRgt10\">Accept</a>");
                                        strMarkup.Append("<a href=\"order-details.aspx?id=" + Request.QueryString["id"] + "&type=reject&prId=" + prow["PrescriptionID"].ToString() + "&ref=rx\" class=\"buttonDel\">Reject</a>");
                                    }
                                }
                                else
                                {
                                    strMarkup.Append("<a href=\"order-details.aspx?id=" + Request.QueryString["id"] + "&type=accept&prId=" + prow["PrescriptionID"].ToString() + "\" class=\"buttonBlue mrgRgt10\">Accept</a>");
                                    strMarkup.Append("<a href=\"order-details.aspx?id=" + Request.QueryString["id"] + "&type=reject&prId=" + prow["PrescriptionID"].ToString() + "\" class=\"buttonDel\">Reject</a>");
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
        }
        catch (Exception ex)
        {

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetOrdersData", ex.Message.ToString());
            return;

        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            // orderStatus=0 > added to cart, 1 > Order placed (New/pending), 2 > Cancelled By Customer , 3 > Accepted, 4 > Denied, 5 > Processing, 6 > Shipped, 7 > Delivered

            int orderId = Convert.ToInt32(Request.QueryString["id"]);
            string imgName = "";
            int payslipId = c.NextId("OrdersPaymentSlip", "OrdPayId");
            if (fuPaymentSlip.HasFile)
            {
                string fExt = Path.GetExtension(fuPaymentSlip.FileName).ToString().ToLower();
                if (fExt == ".jpg" || fExt == ".jpeg" || fExt == ".png")
                {
                    imgName = "payment-slip-" + payslipId + fExt;
                    ImageUploadProcess(imgName);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Only .jpg, .jpeg or .png files are allowed');", true);
                    //return;
                }

                c.ExecuteQuery("Insert Into OrdersPaymentSlip (OrdPayId, OrdPayDate, OrdPaySlip, FK_OrderId) " +
                    " Values (" + payslipId + ", '" + DateTime.Now + "', '" + imgName + "', " + orderId + ")");

                GetUploadedPrescription(orderId);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select image to upload prescription');", true);
                //return;
            }

            int codFlag = chkCODType.Checked == true ? 1 : 2;

            if (chkSelfPickup.Checked == true)
            {
                codFlag = 3;
            }

            if (codFlag == 3)
            {
                //int custId = Convert.ToInt32(Request.QueryString["custId"]);
                int shipCharges = 0;
                //int ordId = Convert.ToInt32(c.GetReqData("[dbo].[OrdersData]", "TOP 1 [OrderID]", "[FK_OrderCustomerID]=" + custId + "ORDER BY [OrderDate] DESC").ToString());
                string orderAmount = c.returnAggregate("Select SUM(OrdDetailAmount) From OrdersDetails Where FK_DetailOrderID=" + Request.QueryString["id"]).ToString("0.00");
                string finalOrderAmount = "";

                shipCharges = 0;
                finalOrderAmount = (Convert.ToDouble(orderAmount.ToString()) + shipCharges).ToString("0.00");

                c.ExecuteQuery("Update OrdersData Set OrderAmount=" + Convert.ToDouble(finalOrderAmount) + ", [OrderShippingAmount] = 0 Where OrderID=" + Request.QueryString["id"]);
            }

            c.ExecuteQuery("Update OrdersData Set OrderStatus=3, OrderPayMode = " + codFlag + " Where OrderID=" + Request.QueryString["id"]);

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Order Accepted');", true);
            GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));
            string redirect = "";
            if (Request.QueryString["ref"] != null)
            {
                if (Request.QueryString["ref"] == "rx")
                {
                    redirect = "ordered-customer-details.aspx?id=" + Request.QueryString["id"] + "&ref=rx";
                }
            }
            else
            {
                redirect = "ordered-customer-details.aspx?id=" + Request.QueryString["id"];
            }

            string msgData = "Dear Customer, Your Order is confirmed by Genericart Medicine. We will notify you when it is ready to dispatch. Genericart Medicine - Wahi Kaam, Sahi Daam.";

            int custId = Convert.ToInt32(c.GetReqData("OrdersData", "FK_OrderCustomerID", "OrderID=" + Request.QueryString["id"]));
            string mobNo = c.GetReqData("CustomersData", "CustomerMobile", "CustomrtID=" + custId).ToString();

            c.SendSMS(msgData, mobNo);

            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('" + redirect + "', 2000);", true); ;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSubmit_Click", ex.Message.ToString());
            return;
        }
    }

    private void ImageUploadProcess(string imgName)
    {
        try
        {
            string origImgPath = "~/upload/paymentslip/original/";
            string normalImgPath = "~/upload/paymentslip/";

            fuPaymentSlip.SaveAs(Server.MapPath(origImgPath) + imgName);

            c.ImageOptimizer(imgName, origImgPath, normalImgPath, 800, true);

            //Delete rew image from server
            File.Delete(Server.MapPath(origImgPath) + imgName);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "ImageUploadProcess", ex.Message.ToString());
            return;
        }
    }

    private void GetUploadedPrescription(int ordIdX)
    {
        try
        {
            using (DataTable dtRx = c.GetDataTable("Select OrdPayId, OrdPaySlip From OrdersPaymentSlip Where FK_OrderId = " + ordIdX))
            {
                if (dtRx.Rows.Count > 0)
                {
                    StringBuilder strMarkup = new StringBuilder();
                    int bCount = 0;
                    foreach (DataRow row in dtRx.Rows)
                    {
                        strMarkup.Append("<div class=\"imgBox\">");
                        strMarkup.Append("<div class=\"pad-5\">");
                        strMarkup.Append("<div class=\"border1\">");
                        strMarkup.Append("<div class=\"pad-5\">");
                        strMarkup.Append("<div class=\"imgContainer\">");
                        strMarkup.Append("<a href=\"" + Master.rootPath + "upload/paymentslip/" + row["OrdPaySlip"] + "\" data-fancybox=\"rxGroup\"><img src=\"" + Master.rootPath + "upload/paymentslip/" + row["OrdPaySlip"] + "\" class=\"width100\" /></a>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                        //strMarkup.Append("<a href=\"#\" title=\"Delete Payment Slip\"  class=\"closeAnch\"></a>");
                        strMarkup.Append("</div>");

                        bCount++;

                        if ((bCount % 4) == 0)
                        {
                            strMarkup.Append("<div class=\"float_clear\"></div>");
                        }
                    }
                    strMarkup.Append("<div class=\"float_clear\"></div>");
                    paymentslip = strMarkup.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetUploadedPrescription", ex.Message.ToString());
            return;
        }
    }

    protected void btnDeny_Click(object sender, EventArgs e)
    {
        try
        {
            // orderStatus=0 > added to cart, 1 > Order placed (New/pending), 2 > Cancelled By Customer , 3 > Accepted, 4 > Denied, 5 > Processing, 6 > Shipped, 7 > Delivered

            c.ExecuteQuery("Update OrdersData Set OrderStatus=4 Where OrderID=" + Request.QueryString["id"]);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Order Denied');", true);
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('order-reports.aspx', 2000);", true);
            string redirect = "";
            if (Request.QueryString["ref"] != null)
            {
                if (Request.QueryString["ref"] == "rx")
                {
                    redirect = "prescription-orders-report.aspx";
                }
            }
            else
            {
                redirect = "order-report.aspx";
            }
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('" + redirect + "', 2000);", true); ;


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnDeny_Click", ex.Message.ToString());
            return;
        }
    }
    protected void btnAssignOrder_Click(object sender, EventArgs e)
    {
        string redirect = "";
        if (Request.QueryString["ref"] != null)
        {
            if (Request.QueryString["ref"] == "rx")
            {
                redirect = "assign-order.aspx?id=" + Request.QueryString["id"] + "&ref=rx";
            }
        }
        else
        {
            redirect = "assign-order.aspx?id=" + Request.QueryString["id"];
        }
        Response.Redirect(redirect, false);
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

    protected void btnRefreshNew_Click(object sender, EventArgs e)
    {
        try
        {
            string apiCallUrl = "https://www.genericartmedicine.com/api_ecom/transaction_new_status";
            int orderIdX = Convert.ToInt32(Request.QueryString["id"]);
            int custIdX = Convert.ToInt32(c.GetReqData("OrdersData", "FK_OrderCustomerID", "OrderID=" + orderIdX));
            int typeX = 1;  // 1 - Orders, 2 - Dr. appointment

            string apiUrl = String.Format(apiCallUrl);
            WebRequest request = WebRequest.Create(apiUrl);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            request.Headers["20"] = "*/*";
            request.Headers["22"] = "*";
            request.Headers["23"] = "en-US,en;q=0.8,hi;q=0.6";
            request.Headers["0"] = "no-cache";

            string postData = "";
            postData = "order_id=" + orderIdX + "&CustomrtID=" + custIdX + "&type=" + typeX;

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    //apiResponse = c.ErrNotification(1, "Response : " + result.ToString());

                    string statusInfo = "", cMsg = "";
                    var OrderResponses = JsonConvert.DeserializeObject<OrderResponse>(result);
                    statusInfo = OrderResponses.status;
                    cMsg = OrderResponses.messages;

                    if (statusInfo == "True")
                    {
                        apiResponse = c.ErrNotification(1, "Status : " + statusInfo.ToString() + ", Msg : " + cMsg.ToString());
                    }
                    else
                    {
                        apiResponse = c.ErrNotification(2, "Status : " + statusInfo.ToString() + ", Msg : " + cMsg.ToString());
                    }

                    // Refresh Page updates
                    GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));

                }
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnRefreshNew_Click", ex.Message.ToString());
            return;
        }
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        try
        {
            string apiCallUrl = "";
            int orderIdX = Convert.ToInt32(Request.QueryString["id"]);
            int custIdX = Convert.ToInt32(c.GetReqData("OrdersData", "FK_OrderCustomerID", "OrderID=" + orderIdX));
            int typeX = 1;  // 1 - Orders, 2 - Dr. appointment
            double ordAmountX = 0.0;
            object orderPayAmount = c.GetReqData("OrdersData", "OrderPayableAmount", "OrderID=" + orderIdX);
            if (orderPayAmount != DBNull.Value && orderPayAmount != null && orderPayAmount.ToString() != "")
            {
                ordAmountX = Convert.ToDouble(orderPayAmount);
            }

            string deviceTypeX = "";
            object device = c.GetReqData("OrdersData", "DeviceType", "OrderID=" + orderIdX);
            if (device != DBNull.Value && device != null && device.ToString() != "")
            {
                deviceTypeX = device.ToString();
            }

            object txnId = c.GetReqData("OrdersData", "OrderPaymentTxnId", "OrderID=" + orderIdX);
            if (txnId == DBNull.Value || txnId == null || txnId.ToString() == "")
            {
                string response = c.GetOrderTxnId(orderIdX, custIdX, ordAmountX, typeX, deviceTypeX).ToString();

                string statusInfo = "", cMsg = "", ordTxnId = "";
                var OrderResponses = JsonConvert.DeserializeObject<OrderResponse>(response);
                statusInfo = OrderResponses.status;
                cMsg = OrderResponses.messages;
                ordTxnId = OrderResponses.razorpay_order_id;

                if (statusInfo == "True")
                {
                    apiResponse = c.ErrNotification(1, "Status : " + statusInfo.ToString() + ", Msg : " + cMsg.ToString() + ", Order Transaction Id : " + ordTxnId);
                }
                else
                {
                    apiResponse = c.ErrNotification(2, "Status : " + statusInfo.ToString() + ", Msg : " + cMsg.ToString());
                }

                // after generating txn id, call txn status api

                if (ordTxnId.ToString() != "" || ordTxnId != null)
                {
                    if (ordTxnId.ToString().Contains("pay"))
                    {
                        string statusResp = c.GetTransactionStatus(orderIdX, custIdX, typeX).ToString();

                        string statusInfo1 = "", cMsg1 = "";
                        var OrderResponses1 = JsonConvert.DeserializeObject<OrderResponse>(statusResp);
                        statusInfo1 = OrderResponses1.status;
                        cMsg1 = OrderResponses1.messages;

                        if (statusInfo1 == "True")
                        {
                            apiResponse = c.ErrNotification(1, "Status : " + statusInfo1.ToString() + ", Msg : " + cMsg1.ToString());
                        }
                        else
                        {
                            apiResponse = c.ErrNotification(2, "Status : " + statusInfo1.ToString() + ", Msg : " + cMsg1.ToString());
                        }
                    }
                    else if (ordTxnId.ToString().Contains("order"))
                    {
                        string statusTxn = c.GetRazorPayTransStatus(txnId.ToString()).ToString();

                        string statusInfo2 = "", cMsg2 = "", transStatus = "", txnIdX = "";
                        var OrderResponses2 = JsonConvert.DeserializeObject<OrderResponse>(statusTxn);
                        statusInfo2 = OrderResponses2.status;
                        cMsg2 = OrderResponses2.messages;
                        transStatus = OrderResponses2.transtatus;
                        txnIdX = OrderResponses2.txn_id;

                        if (statusInfo2 == "True")
                        {
                            if (transStatus.ToString() == "paid")
                            {
                                c.ExecuteQuery("Update OrdersData Set OrderPayStatus=1, OrderPaymentTxnId='" + txnIdX + "' Where OrderID=" + orderIdX);
                            }
                            apiResponse = c.ErrNotification(1, "Status : " + statusInfo2.ToString() + ", Msg : " + cMsg2.ToString());
                        }
                        else
                        {
                            apiResponse = c.ErrNotification(2, "Status : " + statusInfo2.ToString() + ", Msg : " + cMsg2.ToString());
                        }
                    }
                }
            }
            else
            {
                if (txnId.ToString().Contains("pay"))
                {
                    string statusResp = c.GetTransactionStatus(orderIdX, custIdX, typeX).ToString();

                    string statusInfo1 = "", cMsg1 = "";
                    var OrderResponses1 = JsonConvert.DeserializeObject<OrderResponse>(statusResp);
                    statusInfo1 = OrderResponses1.status;
                    cMsg1 = OrderResponses1.messages;

                    if (statusInfo1 == "True")
                    {
                        apiResponse = c.ErrNotification(1, "Status : " + statusInfo1.ToString() + ", Msg : " + cMsg1.ToString());
                    }
                    else
                    {
                        apiResponse = c.ErrNotification(2, "Status : " + statusInfo1.ToString() + ", Msg : " + cMsg1.ToString());
                    }
                }
                else if (txnId.ToString().Contains("order"))
                {
                    string statusTxn = c.GetRazorPayTransStatus(txnId.ToString()).ToString();

                    string statusInfo2 = "", cMsg2 = "", transStatus = "", txnIdX = "";
                    var OrderResponses2 = JsonConvert.DeserializeObject<OrderResponse>(statusTxn);
                    statusInfo2 = OrderResponses2.status;
                    cMsg2 = OrderResponses2.messages;
                    transStatus = OrderResponses2.transtatus;
                    txnIdX = OrderResponses2.txn_id;

                    if (statusInfo2 == "True")
                    {
                        if (transStatus.ToString() == "paid")
                        {
                            c.ExecuteQuery("Update OrdersData Set OrderPayStatus=1, OrderPaymentTxnId='" + txnIdX + "' Where OrderID=" + orderIdX);
                        }
                        apiResponse = c.ErrNotification(1, "Status : " + statusInfo2.ToString() + ", Msg : " + cMsg2.ToString());
                    }
                    else
                    {
                        apiResponse = c.ErrNotification(2, "Status : " + statusInfo2.ToString() + ", Msg : " + cMsg2.ToString());
                    }
                }
                ////string apiUrl = String.Format("https://www.genericartmedicine.com/api_ecom/transaction_new_status"); 
                //string apiUrl = String.Format(apiCallUrl);
                //WebRequest request = WebRequest.Create(apiUrl);
                //request.Method = "POST";
                //request.ContentType = "application/x-www-form-urlencoded";

                //request.Headers["20"] = "*/*";
                //request.Headers["22"] = "*";
                //request.Headers["23"] = "en-US,en;q=0.8,hi;q=0.6";
                //request.Headers["0"] = "no-cache";

                //string postData = "";
                //postData = "order_id=" + orderIdX + "&CustomrtID=" + custId + "&type=" + type;

                //using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                //{
                //    streamWriter.Write(postData);
                //    streamWriter.Flush();
                //    streamWriter.Close();

                //    var httpResponse = (HttpWebResponse)request.GetResponse();
                //    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                //    {
                //        var result = streamReader.ReadToEnd();
                //        //apiResponse = c.ErrNotification(1, "Response : " + result.ToString());

                //        string statusInfo = "", cMsg = "";
                //        var OrderResponses = JsonConvert.DeserializeObject<OrderResponse>(result);
                //        statusInfo = OrderResponses.status;
                //        cMsg = OrderResponses.messages;

                //        if (statusInfo == "True")
                //        {
                //            apiResponse = c.ErrNotification(1, "Status : " + statusInfo.ToString() + ", Msg : " + cMsg.ToString());
                //        }
                //        else
                //        {
                //            apiResponse = c.ErrNotification(2, "Status : " + statusInfo.ToString() + ", Msg : " + cMsg.ToString());
                //        }
                //    }
                //}
            }
            GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));
        }
        catch (Exception ex)
        {
            GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnRefresh_Click", ex.Message.ToString());
            return;
        }
    }

    protected void btnHistory_Click(object sender, EventArgs e)
    {
        string redirect = "";
        if (Request.QueryString["ref"] != null)
        {
            if (Request.QueryString["ref"] == "rx")
            {
                redirect = "assign-order.aspx?id=" + Request.QueryString["id"] + "&ref=rx&type=reject";
            }
        }
        else
        {
            redirect = "assign-order.aspx?id=" + Request.QueryString["id"] + "&type=reject";
        }
        Response.Redirect(redirect, false);
    }
    protected void btnRefund_Click(object sender, EventArgs e)
    {
        try
        {
            int OrderIdx = Convert.ToInt32(Request.QueryString["id"]);
            var client = new RestClient("https://www.genericartmedicine.com/api_ecom/refund_by_id");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("order_id", OrderIdx);
            IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);
            RefundContent.Visible = true;
            RefundAlert = response.Content;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnRefresh_Click", ex.Message.ToString());
            return;
        }
    }

    // Code wriiten by Vinayak (10/May/2022)
    public bool RefundStatus()
    {
        try
        {
            // Read data from "online_payment_logs" table of this order
            string Idx = Request.QueryString["id"];
            using (DataTable dtOrder = c.GetDataTable("Select * From online_payment_logs Where OLP_Order_no ='" + Idx + "'"))
            {
                if (dtOrder.Rows.Count > 0)
                {
                    DataRow logRow = dtOrder.Rows[0];
                    // Check OPL_transtatus paid / created? If "paid" then proceed OR keep REFUND button deactive
                    string tranStatus = logRow["OPL_transtatus"] != DBNull.Value && logRow["OPL_transtatus"] != null && logRow["OPL_transtatus"].ToString() != "" ? logRow["OPL_transtatus"].ToString() : "-";
                    if (tranStatus == "created" || tranStatus == "-")
                    {

                        RefundContent.Visible = true;
                        RefundAlert = "Refund Warning: This Order is yet to settle";
                        return false;
                    }
                    // If OPL_transtatus = paid
                    string settlementID = logRow["OLP_settlementID"] != DBNull.Value && logRow["OLP_settlementID"] != null && logRow["OLP_settlementID"].ToString() != "" ? logRow["OLP_settlementID"].ToString() : "-";

                    if (settlementID == "-")
                    {

                        RefundContent.Visible = true;
                        RefundAlert = "Refund Warning: This Order is yet to settle";
                        return false;
                    }
                    // Check refund_UTR Status
                    string refundUTR = logRow["refund_UTR"] != DBNull.Value && logRow["refund_UTR"] != null && logRow["refund_UTR"].ToString() != "" ? logRow["refund_UTR"].ToString() : "-";

                    if (refundUTR == "-")
                    {
                        return true;
                    }
                    else
                    {
                        RefundContent.Visible = true;
                        RefundAlert = "Refund Processed, URT No.: " + refundUTR;
                        return false;
                    }
                }
                return false;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', " + ex.Message.ToString() + ");", true);
            return false;
        }
    }
}