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

public partial class doctors_order_details : System.Web.UI.Page
{
    iClass c = new iClass();
    public string customerId, orderCount, shippingCharges, prescriptionStr, mreq, ordId, deviceType, pickupReqStatus, apiResponse, gobpName;

    public string[] ordData = new string[20]; //14
    public string[] ordCustData = new string[10];
    public string favShopInfo = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        btnRefresh.Attributes.Add("onclick", "this.disabled=true; this.value='Processing...';" + ClientScript.GetPostBackEventReference(btnRefresh, null) + ";");
        //btnSendReq.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnSendReq, null) + ";");
        btnAddOfflinePayment.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnAddOfflinePayment, null) + ";");

        if (!IsPostBack)
        {
            returnOrd.Visible = false;
            //if (!c.IsRecordExist("Select OrdAssignID From OrdersAssign Where FK_OrderID=" + Request.QueryString["id"]))
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Something Went Wrong');", true);

            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('prescription-order.aspx', 2000);", true);
            //}

            int ordStatusTemp = Convert.ToInt32(c.GetReqData("OrdersAssign", "OrdAssignStatus", "FK_OrderID=" + Request.QueryString["id"]));
            int ordReAssignTemp = Convert.ToInt32(c.GetReqData("OrdersAssign", "OrdReAssign", "FK_OrderID=" + Request.QueryString["id"]));

            object shipName = c.GetReqData("OrdersData", "OrderShipName", "OrderID=" + Request.QueryString["id"]);
            if (shipName != DBNull.Value && shipName != null && shipName.ToString() != "")
            {
                chkDelivered.Visible = true;
            }
            else
            {
                chkDelivered.Visible = false;
            }

            if (ordStatusTemp == 0 && ordReAssignTemp == 0)
            {
                btnAccept.Visible = true;
                //btnReject.Visible = true;
                btnReject.Visible = false;
                dispatchForm.Visible = false;
                btnDispatch.Visible = false;
            }
            else
            {
                btnAccept.Visible = true;

                if (ordStatusTemp == 2)
                {
                    dispatchForm.Visible = false;
                }
                else
                {
                    dispatchForm.Visible = true;
                }
                if (ordStatusTemp == 5 || ordStatusTemp == 6)
                {
                    btnDispatch.Visible = true;
                }
                else
                {
                    btnDispatch.Visible = false;
                    btnReject.Visible = false;
                }

                if (ordStatusTemp == 6)
                {
                    btnReject.Visible = false;
                }

                //if (ordStatusTemp == 5)
                //{
                //    using (DataTable dtCompShop = c.GetDataTable("Select FK_FranchID From CompanyOwnShops Where DelMark=0"))
                //    {
                //        if (dtCompShop.Rows.Count > 0)
                //        {
                //            foreach (DataRow row in dtCompShop.Rows)
                //            {
                //                if (Convert.ToInt32(row["FK_FranchID"]) == 24)
                //                {
                //                    pickupReq.Visible = true;
                //                }
                //            }
                //        }
                //    }
                //}
            }
            ordId = Request.QueryString["id"].ToString();
            GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));

            object mreFlag = c.GetReqData("OrdersData", "MreqFlag", "OrderID=" + Request.QueryString["id"]);
            if (mreFlag != DBNull.Value && mreFlag != null && mreFlag.ToString() != "")
            {
                if (mreFlag.ToString() == "1")
                {
                    mreq = "<span class=\"medium clrProcessing bold_weight\">Customer Marked This Order as Monthly Order</span><span class=\"space10\"></span>";
                }
            }

            FillQuantity();

            c.FillComboBox("FranchName", "FranchID", "FranchiseeData", "FranchID IN (24,2010,2062,2070,2012,2065,2124,2137,2142,2165)", "FranchName", 0, ddrFranchisee);
        }
    }
    private void GetOrdersData(int ordIdx)
    {
        try
        {
            using (DataTable dtOrder = c.GetDataTable("Select * From OrdersData Where OrderID =" + ordIdx))
            {
                if (dtOrder.Rows.Count > 0)
                {
                    DataRow bRow = dtOrder.Rows[0];

                    if (bRow["OrderType"] != DBNull.Value && bRow["OrderType"] != null && bRow["OrderType"].ToString() != "")
                    {
                        if (bRow["OrderType"].ToString() == "2")
                        {
                            //cartProd.Visible = false;
                            rxAmount.Visible = true;
                            medUpdate.Visible = true;

                            if (bRow["OrderStatus"] != DBNull.Value && bRow["OrderStatus"] != null && bRow["OrderStatus"].ToString() != "")
                            {
                                if (Convert.ToInt32(bRow["OrderStatus"]) > 6)
                                {
                                    medUpdate.Visible = false;
                                }
                            }

                            if (bRow["OrderAmount"] != DBNull.Value && bRow["OrderAmount"] != null && bRow["OrderAmount"].ToString() != "")
                            {
                                txtRxOrdAmount.Text = bRow["OrderAmount"].ToString();
                                txtRxOrdAmount.Enabled = false;
                            }
                        }
                        else
                        {
                            cartProd.Visible = true;
                            rxAmount.Visible = false;
                            medUpdate.Visible = false;
                        }
                    }

                    ordData[0] = ordIdx.ToString();
                    ordData[1] = Convert.ToDateTime(bRow["OrderDate"]).ToString("dd/MM/yyyy");
                    ordData[11] = Convert.ToDateTime(bRow["OrderDate"]).ToString("hh:mm tt");

                    ordData[2] = Convert.ToDouble(bRow["OrderAmount"]).ToString("0.00");
                    ordData[3] = bRow["OrderShipName"].ToString();
                    ordData[4] = bRow["OrderShipAddress"] != DBNull.Value && bRow["OrderShipAddress"] != null && bRow["OrderShipAddress"].ToString() != "" ? bRow["OrderShipAddress"].ToString() : "-";

                    // Coins Consume
                    ordData[18] = bRow["OrderCoinsValue"] != DBNull.Value && bRow["OrderCoinsValue"] != null && bRow["OrderCoinsValue"].ToString() != "" ? bRow["OrderCoinsValue"].ToString() : "0";

                    // Shipping Charges 26-June-22
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
                            //ordData[12] = "Online Payment - Razorpay";
                            if (c.IsRecordExist("Select OPL_id From online_payment_logs Where OPL_merchantTranId='" + bRow["OrderPaymentTxnId"].ToString() + "' AND OPL_customer_id=" + bRow["FK_OrderCustomerID"]))
                            {
                                int OPL_id = Convert.ToInt32(c.GetReqData("online_payment_logs", "OPL_id", "OPL_merchantTranId='" + bRow["OrderPaymentTxnId"].ToString() + "' AND OPL_customer_id=" + bRow["FK_OrderCustomerID"]));
                                string payStatus = c.GetReqData("online_payment_logs", "OPL_transtatus", "OPL_id=" + OPL_id).ToString();
                                ordData[13] = payStatus.ToString();
                            }
                            else
                            {
                                //ordData[13] = "NA";
                                ordData[13] = bRow["OrderPayStatus"].ToString() == "0" ? "UnPaid" : "Paid";
                            }

                            if (bRow["OrderPayStatus"].ToString() == "0")
                            {
                                btnRefresh.Visible = true;
                            }
                            else
                            {
                                btnRefresh.Visible = false;
                                if (bRow["OrderPayMode"].ToString() == "3" || bRow["OrderPayMode"].ToString() == "4" || bRow["OrderPayMode"].ToString() == "5")
                                {
                                    offlinePay.Visible = true;
                                    txtUTRNo.Text = bRow["OrderPaymentTxnId"].ToString();
                                    txtUTRNo.Enabled = false;
                                    btnAddOfflinePayment.Visible = false;
                                }
                            }

                            switch (Convert.ToInt32(bRow["OrderPayMode"]))
                            {
                                case 0:
                                    ordData[12] = "Not Selected";
                                    break;
                                case 1:
                                    ordData[12] = "Cash On Delivery";
                                    break;
                                case 2:
                                    ordData[12] = "Online Payment - Razorpay";
                                    break;
                                case 3:
                                    ordData[12] = "Online Payment - By QR Code Scan";
                                    chkQR.Checked = true;
                                    chkGPay.Enabled = false;
                                    chkCash.Enabled = false;
                                    break;
                                case 4:
                                    ordData[12] = "Online Payment - By Google Pay";
                                    chkGPay.Checked = true;
                                    chkQR.Enabled = false;
                                    chkCash.Enabled = false;
                                    break;
                                case 5:
                                    ordData[12] = "By Cash - Self Pickup";
                                    chkCash.Checked = true;
                                    chkGPay.Enabled = false;
                                    chkQR.Enabled = false;
                                    break;
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
                                    break;

                                case 3:
                                    ordData[12] = "Online Payment - By QR Code Scan";
                                    ordData[13] = bRow["OrderPayStatus"].ToString() == "0" ? "UnPaid" : "Paid";
                                    break;
                                case 4:
                                    ordData[12] = "Online Payment - By Google Pay";
                                    ordData[13] = bRow["OrderPayStatus"].ToString() == "0" ? "UnPaid" : "Paid";
                                    break;
                                case 5:
                                    ordData[12] = "By Cash - Self Pickup";
                                    ordData[13] = bRow["OrderPayStatus"].ToString() == "0" ? "UnPaid" : "Paid";
                                    break;
                            }

                            if (bRow["OrderPayMode"].ToString() == "3" || bRow["OrderPayMode"].ToString() == "4" || bRow["OrderPayMode"].ToString() == "5")
                            {
                                offlinePay.Visible = true;
                                txtUTRNo.Text = bRow["OrderPaymentTxnId"] != DBNull.Value && bRow["OrderPaymentTxnId"] != null && bRow["OrderPaymentTxnId"].ToString() != "" ? bRow["OrderPaymentTxnId"].ToString() : "";
                                txtUTRNo.Enabled = false;
                                btnAddOfflinePayment.Visible = false;
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
                        else
                        {
                            offlinePay.Visible = true;
                        }
                    }

                    deviceType = bRow["DeviceType"] != DBNull.Value && bRow["DeviceType"] != null && bRow["DeviceType"].ToString() != "" ? bRow["DeviceType"].ToString() : "";
                    ordData[14] = bRow["OrderNote"] != DBNull.Value && bRow["OrderNote"] != null && bRow["OrderNote"].ToString() != "" ? bRow["OrderNote"].ToString() : "-";
                    ordData[15] = c.returnAggregate("Select SUM(OrdDetailAmount) From OrdersDetails Where FK_DetailOrderID=" + ordIdx).ToString("0.00");

                    if (Convert.ToDouble(ordData[15].ToString()) > 500)
                    {
                        //shippingCharges = "Shipping Charges = &#8377; 0.00";
                    }
                    else
                    {
                        //shippingCharges = "Shipping Charges = &#8377; 30.00 <br/>";
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

                    customerId = bRow["FK_OrderCustomerID"].ToString();

                    object obpId = c.GetReqData("CustomersData", "FK_ObpID", "CustomrtID=" + customerId);
                    if (obpId != DBNull.Value && obpId != null && obpId.ToString() != "" && obpId.ToString() != "0")
                    {
                        gobpName = "GOBP : " + c.GetReqData("OBPData", "OBP_ApplicantName", "OBP_ID=" + obpId).ToString();
                    }

                    using (DataTable dtCust = c.GetDataTable("Select CustomerName, CustomerMobile, CustomerEmail, CustomerAddress, CustomerCity, CustomerState, CustomerPincode, CustomerFavShop From CustomersData Where CustomrtID = " + customerId))
                    {
                        if (dtCust.Rows.Count > 0)
                        {
                            DataRow row = dtCust.Rows[0];

                            int ordAssignStatus = Convert.ToInt32(c.GetReqData("OrdersAssign", "OrdAssignStatus", "FK_OrderID=" + Request.QueryString["id"]));

                            if (ordAssignStatus == 0 || ordAssignStatus == 2)  // pending - rejected
                            {
                                
                                // Customer's Favourite Shop info
                                if(row["CustomerFavShop"] != DBNull.Value && row["CustomerFavShop"] != null && row["CustomerFavShop"].ToString() != "")
                                {
                                    favShopInfo = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + Convert.ToInt32(row["CustomerFavShop"])).ToString();
                                    favShopInfo = favShopInfo + " " + c.GetReqData("FranchiseeData", "FranchName", "FranchID=" + Convert.ToInt32(row["CustomerFavShop"])).ToString();
                                }
                                else
                                {
                                    favShopInfo = "No Shop Found.";
                                }
                                
                                // cust name
                                if (row["CustomerName"] != DBNull.Value && row["CustomerName"] != null && row["CustomerName"].ToString() != "")
                                {
                                    if (row["CustomerName"].ToString().Contains(' '))
                                    {
                                        string[] custName = row["CustomerName"].ToString().Split(' ');
                                        char[] charName = custName[1].ToCharArray();
                                        string maskedName = "";
                                        int nameLength = charName.Length;
                                        int i = 1;
                                        foreach (char name in charName)
                                        {
                                            if (maskedName == "")
                                            {
                                                maskedName = "X";
                                            }
                                            else
                                            {
                                                maskedName = maskedName + "X";
                                            }

                                            i++;
                                        }
                                        //Masking removed 10th Apr 23 by Vinayak on Dr.Shruti's request.
                                        //ordCustData[0] = custName[0].ToString() + " " + maskedName.ToString();
                                        ordCustData[0] = row["CustomerName"].ToString();
                                    }
                                    else
                                    {
                                        ordCustData[0] = row["CustomerName"].ToString();
                                    }
                                }
                                else
                                {
                                    ordCustData[0] = "-";
                                }

                                // cust mobile number
                                if (row["CustomerMobile"] != DBNull.Value && row["CustomerMobile"] != null && row["CustomerMobile"].ToString() != "")
                                {
                                    string custMob = row["CustomerMobile"].ToString();
                                    char[] charMob = custMob.ToCharArray();
                                    string maskedMob = "";
                                    int mobLength = charMob.Length;
                                    int j = 1;
                                    foreach (char mob in charMob)
                                    {
                                        if (maskedMob == "")
                                        {
                                            maskedMob = mob.ToString();
                                        }
                                        else
                                        {
                                            if (j > 5)
                                                maskedMob = maskedMob + "X";
                                            else
                                                maskedMob = maskedMob + mob.ToString();
                                        }

                                        j++;
                                    }
                                    //Masking removed 10th Apr 23 by Vinayak on Dr.Shruti's request.
                                    //ordCustData[1] = maskedMob.ToString();
                                    ordCustData[1] = custMob;
                                }
                                else
                                {
                                    ordCustData[1] = "-";
                                }

                                // custEmail
                                if (row["CustomerEmail"] != DBNull.Value && row["CustomerEmail"] != null && row["CustomerEmail"].ToString() != "")
                                {
                                    string custEmail = row["CustomerEmail"].ToString();
                                    char[] charEmail = custEmail.ToCharArray();
                                    string maskedEmail = "";
                                    int emailLength = charEmail.Length;
                                    int k = 1;
                                    foreach (char mail in charEmail)
                                    {
                                        if (maskedEmail == "")
                                        {
                                            maskedEmail = mail.ToString();
                                        }
                                        else
                                        {
                                            if (k > 5)
                                                maskedEmail = maskedEmail + "X";
                                            else
                                                maskedEmail = maskedEmail + mail.ToString();
                                        }

                                        k++;
                                    }
                                    //Masking removed 10th Apr 23 by Vinayak on Dr.Shruti's request.
                                    //ordCustData[2] = maskedEmail.ToString();
                                    ordCustData[2] = custEmail;
                                }
                                else
                                {
                                    ordCustData[2] = "-";
                                }

                            }
                            else
                            {
                                ordCustData[0] = row["CustomerName"].ToString();
                                ordCustData[1] = row["CustomerMobile"].ToString();
                                ordCustData[2] = row["CustomerEmail"].ToString();
                            }


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

                    //prescription
                    if (c.IsRecordExist("Select PrescriptionID From OrderPrescriptions Where FK_OrderID=" + ordIdx))
                    {
                        StringBuilder strMarkup = new StringBuilder();

                        using (DataTable dtRx = c.GetDataTable("Select PrescriptionID, PrescriptionName, PrescriptionStatus From OrderPrescriptions Where FK_OrderID=" + ordIdx))
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
                                        strMarkup.Append("<div class=\"absRejected\">Rejected by Admin</div>");
                                    }
                                    if (prow["PrescriptionStatus"].ToString() == "1")
                                    {
                                        strMarkup.Append("<div class=\"absAccepted\">Accepted by Admin</div>");
                                    }
                                    strMarkup.Append("<div class=\"pad-5\">");
                                    strMarkup.Append("<div class=\"imgContainer\">");
                                    //strMarkup.Append("<a href=\"" + rPath + "upload/prescriptions/" + prow["PrescriptionName"] + "\" data-fancybox=\"rxGroup\"><img src=\"" + rPath + "upload/prescriptions/" + prow["PrescriptionName"] + "\" class=\"width100\" /></a>");
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

                    // dispatch details
                    if (bRow["OrderShipName"] != DBNull.Value && bRow["OrderShipName"] != null && bRow["OrderShipName"].ToString() != "")
                    {
                        txtShipmentName.Text = bRow["OrderShipName"].ToString();
                    }
                    if (bRow["OrderTrackingNo"] != DBNull.Value && bRow["OrderTrackingNo"] != null && bRow["OrderTrackingNo"].ToString() != "")
                    {
                        txtTrackingNo.Text = bRow["OrderTrackingNo"].ToString();
                    }
                    if (bRow["EstimatedDeliveryDate"] != DBNull.Value && bRow["EstimatedDeliveryDate"] != null && bRow["EstimatedDeliveryDate"].ToString() != "")
                    {
                        txtDate.Text = Convert.ToDateTime(bRow["EstimatedDeliveryDate"]).ToString("dd/MM/yyyy");
                    }

                    if (bRow["OrderStatus"].ToString() == "7")
                    {
                        chkDelivered.Checked = true;
                        chkDelivered.Enabled = false;
                        returnOrd.Visible = true;
                    }

                    // canceled by cust
                    if (bRow["OrderStatus"].ToString() == "2")
                    {
                        ordCanc.Visible = true;
                        btnAccept.Visible = false;
                        btnDispatch.Visible = false;
                        btnReject.Visible = false;
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

                    // returned
                    if (bRow["OrderStatus"].ToString() == "10")
                    {
                        string retReason = c.GetReqData("OrdersAssign", "ReturnReason", "OrdAssignID=" + Request.QueryString["assignId"]).ToString();
                        txtRetReason.Text = retReason;
                        btnRet.Visible = false;
                        returnOrd.Visible = true;
                    }
                }




                using (DataTable dtProduct = c.GetDataTable("Select a.OrdDetailID, a.FK_DetailProductID, a.OrdDetailQTY, 'Rs. ' + Convert(varchar(20), a.OrdDetailPrice)  as OrigPrice, 'Rs. ' + Convert(varchar(20), a.OrdDetailAmount) as OrdAmount , a.OrdDetailSKU, b.ProductName from OrdersDetails a Inner Join ProductsData b on a.FK_DetailProductID = b.ProductID where a.FK_DetailOrderID =" + ordIdx))
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




            }
        }
        catch (Exception ex)
        {

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetOrdersData", ex.Message.ToString());
            return;
        }
    }


    protected void btnResponse_Click(object sender, EventArgs e)
    {
        try
        {
            // OrderStatus = 12 => No Response by customer
            c.ExecuteQuery("Update OrdersData set OrderStatus=12 Where OrderID=" + Request.QueryString["id"]);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Response submited successfully');", true);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('prescription-order.aspx?type=new', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnRejectOrder_Click", ex.Message.ToString());
            return;
        }
    }

    protected void btnRejectOrder_Click(object sender, EventArgs e)
    {
        try
        {
            // OrderStatus = 11 => Order Rejected by Doctor
            c.ExecuteQuery("Update OrdersData set OrderStatus=11 Where OrderID=" + Request.QueryString["id"]);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Order rejected successfully');", true);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('prescription-order.aspx?type=new', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnRejectOrder_Click", ex.Message.ToString());
            return;
        }
    }

    protected void btnAccept_Click(object sender, EventArgs e)
    {
        try
        {
            //***OrdersAssign***

            int assignShopId = 0;

            // Assign Shop selection empty validation
            if(ddrFranchisee.SelectedIndex == 0 && txtOtherShop.Text == "" )
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Assign Order SHOP selection is mandatory.');", true);
                GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));
                return;
            }

            // Check If OTHER-SHOP is mentioned into TextBox
            if (txtOtherShop.Text != "")
            {
                if (c.IsRecordExist("Select FranchID From FranchiseeData Where FranchShopCode='" + txtOtherShop.Text + "' AND FranchLegalBlock=0") == true)
                {
                    assignShopId = Convert.ToInt32(c.GetReqData("FranchiseeData", "FranchID", "FranchShopCode='" + txtOtherShop.Text + "'"));
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'This Other-Shop does not exist.');", true);
                    GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));
                    return;
                }
            }
            else
            {
                assignShopId = Convert.ToInt32(ddrFranchisee.SelectedValue);
            }

            // OrderAssignStatus 0 > Pending, 1 > Accepted, 2 > Rejected, 5 > In Process, 6 > Shipped, 7 > Delivered
            //(3,4 not considered to match flags 5,6,7 with main OrdersData table)

            int ordType = Convert.ToInt32(c.GetReqData("OrdersData", "OrderType", "OrderID=" + Request.QueryString["id"]));
            if (ordType == 2)
            {
                if (txtRxOrdAmount.Text == "")
                {
                    GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Order Amount');", true);
                    return;
                }

                if (!c.IsNumeric(txtRxOrdAmount.Text))
                {
                    GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Order Amount Must be Numeric Value');", true);
                    return;
                }

                c.ExecuteQuery("Update OrdersData Set OrderAmount=" + Convert.ToDouble(txtRxOrdAmount.Text) + " Where OrderID=" + Request.QueryString["id"]);
            }

            // Default Shop assign 2062 => GMMH0443 => GENERICART MEDICINE STORE SAWARKAR CHOWK (11-Apr-23)
            int AssignOrderId = c.NextId("OrdersAssign", "OrdAssignID");
            string DocName = c.GetReqData("DoctorsData", "DocName", "DoctorID = " + Session["adminDoctor"]).ToString();

            c.ExecuteQuery("Insert into OrdersAssign (OrdAssignID, OrdAssignDate, FK_OrderID, FK_FranchID, OrdAssignStatus, OrdReAssign, AssignedFrom) " +
               " Values (" + AssignOrderId + ", '" + DateTime.Now + "', " + Request.QueryString["id"] + ", "+ assignShopId + ", 0, 0, '" + DocName + "')");
            
            //***OrdersData***
            // update current order's status as inprocess in main OrdersData table 
            // orderStatus=0 > added to cart, 1 > Order placed (New/pending), 2 > Cancelled By Customer , 3 > Accepted, 4 > Denied, 5 > Processing, 6 > Shipped, 7 > Delivered
            c.ExecuteQuery("Update OrdersData Set OrderStatus=1 Where OrderID=" + Request.QueryString["id"]);

            //Now mark this Prescription order as successfuly DECODED by Doctor=> IsDecoded=yes  (12-Apr-23 by Vinayak)
            c.ExecuteQuery("Update OrdersData Set IsDecoded = 'yes' Where OrderID = " + Request.QueryString["id"]);

            GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));
            string custId = c.GetReqData("OrdersData", "FK_OrderCustomerID", "OrderID=" + Request.QueryString["id"]).ToString();
            string msgTitle = "Order Confirmed";
            string msgBody = "Dear Customer, Your Order #" + Request.QueryString["id"] + " is confirmed. We will notify you when it has been sent.";
            object custToken = c.GetReqData("CustomersData", "CustomerToken", "CustomrtID=" + custId);
            //if (custToken != DBNull.Value && custToken != null && custToken.ToString() != "")
            //{
            //    c.SendPushNotification(custId, msgTitle, msgBody, "");
            //}
            int maxID = c.NextId("Notification", "Notifi_id");
            c.ExecuteQuery("Insert Into Notification (Notifi_id, Notifi_CustomrtID, Notifi_heading, Notifi_msg, Notifi_status) " +
                " Values (" + maxID + ", " + custId + ", '" + msgTitle + "', '" + msgBody + "', 0)");
            c.SendPushNotification(custId, msgTitle, msgBody, "");

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Order assigned to GMMH0443 successfully');", true);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('prescription-order.aspx?type=new', 2000);", true);
        }
        catch (Exception ex)
        {

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnAccept_Click", ex.Message.ToString());
            return;
        }
    }

    //protected void btnRejected_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //c.ExecuteQuery("Update OrdersAssign Set OrdAssignStatus=2 Where FK_OrderID=" + Request.QueryString["id"] + " AND Fk_FranchID=" + Session["adminFranchisee"]);
    //        GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));
    //        cancReason.Visible = true;
    //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select reason to reject order');", true);
    //        //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('orders-report.aspx', 2000);", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
    //        c.ErrorLogHandler(this.ToString(), "btnRejected_Click", ex.Message.ToString());
    //        return;
    //    }
    //}

    //protected void btnCancOrder_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        c.ExecuteQuery("Update OrdersAssign Set OrdAssignStatus=2, FK_ReasonID=" + ddrReasons.SelectedValue + " Where FK_OrderID=" + Request.QueryString["id"] + " AND Fk_FranchID=" + Session["adminFranchisee"]);
    //        GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));
    //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Order Rejected');", true);
    //        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('orders-report.aspx', 2000);", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
    //        c.ErrorLogHandler(this.ToString(), "btnCancOrder_Click", ex.Message.ToString());
    //        return;
    //    }
    //}

    protected void btnDispatch_Click(object sender, EventArgs e)
    {
        try
        {
            txtDate.Text = txtDate.Text.Trim().Replace("'", "");
            txtShipmentName.Text = txtShipmentName.Text.Trim().Replace("'", "");
            txtTrackingNo.Text = txtTrackingNo.Text.Trim().Replace("'", "");

            if (txtDate.Text == "" || txtShipmentName.Text == "" || txtTrackingNo.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * Fields are Compulsory');", true);
                //return;
            }

            string[] arrDate = txtDate.Text.Split('/');
            if (c.IsDate(arrDate[1] + "/" + arrDate[0] + "/" + arrDate[2]) == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid Date');", true);
                return;
            }
            DateTime estimatedDate = Convert.ToDateTime(arrDate[1] + "/" + arrDate[0] + "/" + arrDate[2]);

            // update dispatch details & set orderStatus=6 as dispatched
            c.ExecuteQuery("Update OrdersData Set OrderShipName='" + txtShipmentName.Text + "', OrderTrackingNo='" + txtTrackingNo.Text +
                "', EstimatedDeliveryDate='" + estimatedDate + "', OrderStatus=6 Where OrderID=" + Request.QueryString["id"]);

            // set OrderAssignStatus=6 as dispatched
            c.ExecuteQuery("Update OrdersAssign, Fk_FranchID = 2062 Set OrdAssignStatus=6 Where OrdReAssign=0 AND FK_OrderID=" + Request.QueryString["id"]);

            if (!c.IsRecordExist("Select OrdTracID From OrderTracking Where FK_OrderID=" + Request.QueryString["id"] + " AND OrdTracType=3"))
            {
                // add entry in Order Tracking table
                int trackMaxId = c.NextId("OrderTracking", "OrdTracID");
                int ordAssignId = Convert.ToInt32(c.GetReqData("OrdersAssign", "OrdAssignID", "FK_OrderID=" + Request.QueryString["id"] + " AND OrdAssignStatus=6"));

                c.ExecuteQuery("Insert Into OrderTracking (OrdTracID, OrdTracDate, FK_OrderID, FK_OrdAssignID, OrdTracType, OrdTracMessage) " +
                    " Values (" + trackMaxId + ", '" + DateTime.Now + "', " + Request.QueryString["id"] + ", " + ordAssignId +
                    ", 3, 'Your order is out for delivery.# Courier Name : " + txtShipmentName.Text + ",# Your Order Tracking No. : " + txtTrackingNo.Text + " and #Estimated delivery Date : " + estimatedDate.ToString("dd MMM, yyyy") + "')");


                string msgData = "Dear Customer, Your Order has been dispatched with " + txtShipmentName.Text + " courier. Your Order Tracking No.: " + txtTrackingNo.Text + " Genericart Medicine - Wahi Kaam, Sahi Daam.";

                int custId = Convert.ToInt32(c.GetReqData("OrdersData", "FK_OrderCustomerID", "OrderID=" + Request.QueryString["id"]));
                string mobNo = c.GetReqData("CustomersData", "CustomerMobile", "CustomrtID=" + custId).ToString();

                c.SendSMS(msgData, mobNo);

            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Dispatch Details Added');", true);
            GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));

            string customerId = c.GetReqData("OrdersData", "FK_OrderCustomerID", "OrderID=" + Request.QueryString["id"]).ToString();
            string msgTitle = "Order Dispatched";
            string msgBody = "Dear Customer, Your Order #" + Request.QueryString["id"] + " is dispatched. Your Order is out for delivery.";
            object custToken = c.GetReqData("CustomersData", "CustomerToken", "CustomrtID=" + customerId);
            //if (custToken != DBNull.Value && custToken != null && custToken.ToString() != "")
            //{
            //    c.SendPushNotification(customerId, msgTitle, msgBody, "");
            //}

            int maxID = c.NextId("Notification", "Notifi_id");
            c.ExecuteQuery("Insert Into Notification (Notifi_id, Notifi_CustomrtID, Notifi_heading, Notifi_msg, Notifi_status) " +
                " Values (" + maxID + ", " + customerId + ", '" + msgTitle + "', '" + msgBody + "', 0)");
            c.SendPushNotification(customerId, msgTitle, msgBody, "");

            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('prescription-order.aspx?type=new', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnDispatch_Click", ex.Message.ToString());
            return;
        }
    }

    protected void chkDelivered_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkDelivered.Checked == true)
            {
                // Update OrderStatus=7 as delivered & PayStaus=1 as paid in OrdersData 
                //also update followup last date & next date (20/01/2023)
                //c.ExecuteQuery("Update OrdersData Set OrderStatus=7, OrderPayStatus=1 Where OrderID=" + Request.QueryString["id"]);
                DateTime nextFlDate = DateTime.Now.AddDays(26);
                c.ExecuteQuery("Update OrdersData Set OrderStatus=7, OrderPayStatus=1, FollowupLastDate='" + DateTime.Now +
                    "', FollowupNextDate='" + nextFlDate + "', FollowupNextTime='11:00 AM', FollowupStatus='Active' Where OrderID=" + Request.QueryString["id"]);

                // Update OrderAssignStatus=7 as delivered in OrderAssign
                c.ExecuteQuery("Update OrdersAssign, Fk_FranchID = 2062 Set OrdAssignStatus=7 Where FK_OrderID=" + Request.QueryString["id"]);

                // Add Entry of Delivered Order in OrderTracking
                if (!c.IsRecordExist("Select OrdTracID From OrderTracking Where FK_OrderID=" + Request.QueryString["id"] + " AND OrdTracType=4"))
                {
                    int trackMaxId = c.NextId("OrderTracking", "OrdTracID");
                    int ordAssignId = Convert.ToInt32(c.GetReqData("OrdersAssign", "OrdAssignID", "FK_OrderID=" + Request.QueryString["id"] + " AND OrdAssignStatus=7 AND Fk_FranchID=2062"));

                    c.ExecuteQuery("Insert Into OrderTracking (OrdTracID, OrdTracDate, FK_OrderID, FK_OrdAssignID, OrdTracType, OrdTracMessage) " +
                        " Values (" + trackMaxId + ", '" + DateTime.Now + "', " + Request.QueryString["id"] + ", " + ordAssignId +
                        ", 4, 'A shipment from order No. " + Request.QueryString["id"] + " has been delivered.# If you have not received your package yet? Let us know.')");

                }

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Order Marked as Delivered');", true);
                GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));


                string customerId = c.GetReqData("OrdersData", "FK_OrderCustomerID", "OrderID=" + Request.QueryString["id"]).ToString();
                string msgTitle = "Order Delivered";
                string msgBody = "Dear Customer, Your Order #" + Request.QueryString["id"] + " is has been delivered.";
                object custToken = c.GetReqData("CustomersData", "CustomerToken", "CustomrtID=" + customerId);
                //if (custToken != DBNull.Value && custToken != null && custToken.ToString() != "")
                //{
                //    c.SendPushNotification(customerId, msgTitle, msgBody, "");
                //}

                int maxID = c.NextId("Notification", "Notifi_id");
                c.ExecuteQuery("Insert Into Notification (Notifi_id, Notifi_CustomrtID, Notifi_heading, Notifi_msg, Notifi_status) " +
                    " Values (" + maxID + ", " + customerId + ", '" + msgTitle + "', '" + msgBody + "', 0)");
                c.SendPushNotification(customerId, msgTitle, msgBody, "");

                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('prescription-order.aspx?type=new', 2000);", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "chkDelivered_CheckedChanged", ex.Message.ToString());
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

                string ordId = Request.QueryString["id"].ToString();

                Button btnDel = (Button)e.Row.FindControl("cmdDelete");
                object ordType = c.GetReqData("OrdersData", "OrderType", "OrderID=" + ordId);
                if (ordType != DBNull.Value && ordType != null && ordType.ToString() != "")
                {
                    if (Convert.ToInt32(ordType) != 2)
                    {
                        btnDel.Visible = false;
                    }
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

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["type"] != null)
        {
            if (Request.QueryString["type"].ToString().Contains("genericMitra"))
            {
                Response.Redirect("prescription-order.aspx?type=new", false);
            }
            switch (Request.QueryString["type"])
            {
                case "new":
                    Response.Redirect("prescription-order.aspx?type=new", false);
                    break;

                case "accepted":
                    Response.Redirect("prescription-order.aspx?type=accepted", false);
                    break;

                case "rejected":
                    Response.Redirect("prescription-order.aspx?type=rejected", false);
                    break;

                case "monthly":
                    Response.Redirect("prescription-order.aspx?type=monthly", false);
                    break;

                case "dispatched":
                    Response.Redirect("prescription-order.aspx?type=dispatched", false);
                    break;

                case "delivered":
                    Response.Redirect("prescription-order.aspx?type=delivered", false);
                    break;

                case "rx":
                    Response.Redirect("prescription-order.aspx?type=rx", false);
                    break;
                case "returned":
                    Response.Redirect("prescription-order.aspx?type=returned", false);
                    break;
            }
        }
        else
        {
            Response.Redirect("prescription-order.aspx?type=new", false);
        }
    }

    private void FillQuantity()
    {
        for (int i = 1; i < 31; i++)
        {
            ddrQty.Items.Add(i.ToString());
            ddrQty.SelectedValue = "1";
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            txtMedName.Text = txtMedName.Text.Trim().Replace("'", "");
            int orderId = Convert.ToInt32(Request.QueryString["id"]);

            if (txtMedName.Text == "" || ddrQty.SelectedIndex == 0)
            {
                GetOrdersData(orderId);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * fields are compulsory');", true);
                return;
            }

            //int orderId = Convert.ToInt32(Request.QueryString["id"]);
            int prodIdX = Convert.ToInt32(c.GetReqData("ProductsData", "ProductID", "ProductName = '" + txtMedName.Text + "' and delMark=0 AND isnull(ProductActive, 0) = 1"));
            double prodAmount = Convert.ToDouble(c.GetReqData("ProductsData", "PriceSale", "ProductID=" + prodIdX));
            double ordAmount = 0, finalOrdAmount = 0, incrementPrice = 0;
            int prodOptionId = 0;
            int optionId = 0;
            int qty = Convert.ToInt32(ddrQty.SelectedValue);

            if (optionId > 0)
            {
                prodOptionId = Convert.ToInt32(c.GetReqData("ProductOptions", "ProdOptionID", "FK_ProductID=" + prodIdX + " AND FK_OptionID=" + optionId));
                incrementPrice = Convert.ToDouble(c.GetReqData("ProductOptions", "PriceIncrement", "ProdOptionID=" + prodOptionId));
                ordAmount = prodAmount + incrementPrice;
                finalOrdAmount = ordAmount * qty;
            }
            else
            {
                ordAmount = prodAmount;
                finalOrdAmount = ordAmount * qty;
            }

            string prodCode = c.GetReqData("ProductsData", "ProductSKU", "ProductID=" + prodIdX).ToString();

            int detailsMaxId = 0;
            if (!c.IsRecordExist("Select OrdDetailID From OrdersDetails Where FK_DetailOrderID=" + orderId + " AND FK_DetailProductID=" + prodIdX))
            {
                detailsMaxId = c.NextId("OrdersDetails", "OrdDetailID");

                c.ExecuteQuery("Insert Into OrdersDetails (OrdDetailID, FK_DetailOrderID, FK_DetailProductID, OrdDetailQTY, OrdDetailPrice, " +
                    " OrdDetailAmount, OrdDetailSKU)" +
                    " Values (" + detailsMaxId + ", " + orderId + ", " + prodIdX + ", " + qty + ", " + ordAmount +
                    ", " + finalOrdAmount + ", '" + prodCode + "')");
            }

            if (optionId > 0)
            {
                if (!c.IsRecordExist("Select OrderOptionID From OrderProductOptions Where FK_OrdDetailID=" + detailsMaxId + " AND FK_ProdOptionID=" + prodIdX))
                {
                    int maxOrdOptionId = c.NextId("OrderProductOptions", "OrderOptionID");
                    c.ExecuteQuery("Insert Into OrderProductOptions (OrderOptionID, FK_OrdDetailID, FK_ProdOptionID, PriceProduct, " +
                        " PriceIncrement) Values (" + maxOrdOptionId + ", " + detailsMaxId + ", " + prodOptionId + ", " + prodAmount + ", " + incrementPrice + ")");
                }
            }

            object deliveryType = c.GetReqData("OrdersData", "DeliveryType", "OrderID=" + orderId);
            int shipCharges = 0;
            if (deliveryType != DBNull.Value && deliveryType != null && deliveryType.ToString() != "")
            {
                if (deliveryType.ToString() == "2")
                {
                    shipCharges = 0;
                }
                else if (deliveryType.ToString() == "1")
                {
                    shipCharges = 30;
                }
                else
                {
                    shipCharges = 0;
                }
            }
            else
            {
                shipCharges = 0;
            }

            string orderAmount = c.returnAggregate("Select SUM(OrdDetailAmount) From OrdersDetails Where FK_DetailOrderID=" + orderId).ToString("0.00");
            string finalOrderAmount = (Convert.ToDouble(orderAmount.ToString()) + shipCharges).ToString("0.00");

            c.ExecuteQuery("Update OrdersData Set OrderAmount=" + Convert.ToDouble(finalOrderAmount) + " Where OrderID=" + orderId);

            int customerId = Convert.ToInt32(c.GetReqData("OrdersData", "FK_OrderCustomerID", "OrderID=" + orderId));
            int gobpId = 0;
            object obpId = c.GetReqData("CustomersData", "FK_ObpID", "CustomrtID=" + customerId);
            if (obpId != DBNull.Value && obpId != null && obpId.ToString() != "")
            {
                gobpId = Convert.ToInt32(obpId);
            }

            //Generic Mitra
            int gMitraId = 0;
            object GMitraId = c.GetReqData("CustomersData", "FK_GenMitraID", "CustomrtID=" + customerId);
            if (GMitraId != DBNull.Value && GMitraId != null && GMitraId.ToString() != "")
            {
                gMitraId = Convert.ToInt32(GMitraId);
            }

            if (gobpId != 0)
            {
                double gobpCom = c.GetOBPComission(orderId);
                c.ExecuteQuery("Update OrdersData Set OBPComTotal=" + gobpCom + " Where OrderID=" + orderId);
            }

            //Generic Mitra
            if (gMitraId != 0)
            {
                double gMitraCom = c.CalCulateGMitraComission(orderId);
                c.ExecuteQuery("Update OrdersData Set GMitraComTotal=" + gMitraCom + ", GMitraId=" + gMitraId + " Where OrderID=" + orderId);
            }

            GetOrdersData(orderId);

            txtMedName.Text = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnAdd_Click", ex.Message.ToString());
            return;
        }
    }
    protected void gvOrderDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridViewRow gRow = (GridViewRow)((Button)e.CommandSource).NamingContainer;
            if (e.CommandName == "gvDel")
            {
                c.ExecuteQuery("Delete From OrdersDetails Where OrdDetailID=" + gRow.Cells[0].Text);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Medicine Deleted');", true);
            }



            object deliveryType = c.GetReqData("OrdersData", "DeliveryType", "OrderID=" + Request.QueryString["id"]);
            int shipCharges = 0;
            if (deliveryType != DBNull.Value && deliveryType != null && deliveryType.ToString() != "")
            {
                if (deliveryType.ToString() == "2")
                {
                    shipCharges = 0;
                }
                else if (deliveryType.ToString() == "1")
                {
                    shipCharges = 30;
                }
                else
                {
                    shipCharges = 0;
                }
            }
            else
            {
                shipCharges = 0;
            }

            string orderAmount = c.returnAggregate("Select SUM(OrdDetailAmount) From OrdersDetails Where FK_DetailOrderID=" + Request.QueryString["id"]).ToString("0.00");
            string finalOrderAmount = (Convert.ToDouble(orderAmount.ToString()) + shipCharges).ToString("0.00");

            c.ExecuteQuery("Update OrdersData Set OrderAmount=" + Convert.ToDouble(finalOrderAmount) + " Where OrderID=" + Request.QueryString["id"]);

            GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvOrderDetails_RowCommand", ex.Message.ToString());
            return;
        }
    }

    protected void btnSendReq_Click(object sender, EventArgs e)
    {
        try
        {
            GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));
            txtPickupDate.Text = txtPickupDate.Text.Trim().Replace("'", "");
            txtPickupTime.Text = txtPickupTime.Text.Trim().Replace("'", "");
            txtWidth.Text = txtWidth.Text.Trim().Replace("'", "");
            txtHeight.Text = txtHeight.Text.Trim().Replace("'", "");
            txtBreadth.Text = txtBreadth.Text.Trim().Replace("'", "");
            txtWeight.Text = txtWeight.Text.Trim().Replace("'", "");

            if (txtPickupDate.Text == "" || txtPickupTime.Text == "" || ddrPay.SelectedIndex == 0 || txtWidth.Text == "" || txtHeight.Text == "" || txtBreadth.Text == "" || txtWeight.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * marked fields are compulsory');", true);
                return;
            }

            if (!c.IsNumeric(txtBreadth.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Breadth must be numeric value');", true);
                return;
            }

            if (!c.IsNumeric(txtWidth.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Width must be numeric value');", true);
                return;
            }

            if (!c.IsNumeric(txtHeight.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Height must be numeric value');", true);
                return;
            }

            if (!c.IsNumeric(txtWeight.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Weight must be numeric value');", true);
                return;
            }

            string[] arrPickupDate = txtPickupDate.Text.Split('/');
            if (c.IsDate(arrPickupDate[1] + "/" + arrPickupDate[0] + "/" + arrPickupDate[2]) == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid Pickup Date');", true);
                return;
            }
            DateTime pickupDate = Convert.ToDateTime(arrPickupDate[2] + "-" + arrPickupDate[1] + "-" + arrPickupDate[0]);

            int frId = 2062;

            string shopOwnerName = "", ShopAddr = "", ShopPinCode = "", ShopMob = "", shopEmail = "", consigneePinCode = "", shopName = "";
            // get basic shop details
            using (DataTable dtFrInfo = c.GetDataTable("Select a.FranchID, LEFT(a.FranchOwnerName, 30) as FranchOwnerName, LEFT(a.FranchName, 30) as FranchName, a.FranchPinCode, a.FranchAddress, a.FranchEmail, a.FranchMobile, b.CityName From FranchiseeData a Inner Join CityData b On a.FK_FranchCityId=b.CityID Where a.FranchID=" + frId))
            {
                if (dtFrInfo.Rows.Count > 0)
                {
                    DataRow row = dtFrInfo.Rows[0];

                    shopOwnerName = row["FranchOwnerName"].ToString();
                    shopName = row["FranchName"].ToString();
                    ShopPinCode = row["FranchPinCode"].ToString();
                    //ShopAddr = row["CityName"].ToString();
                    ShopAddr = row["FranchAddress"].ToString();
                    ShopMob = row["FranchMobile"].ToString();
                    shopEmail = row["FranchEmail"].ToString();
                }
            }
            int orderId = Convert.ToInt32(Request.QueryString["id"]);


            string addressId = "", customerId = "", emailId = "", custMobile = "", custName = "";
            Double collectableAmount = 0.0;
            using (DataTable dtOrders = c.GetDataTable("Select a.FK_AddressId, a.OrderAmount, a.FK_OrderCustomerID, b.CustomerMobile, b.CustomerEmail, LEFT(b.CustomerName, 30) as CustomerName From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID = b.CustomrtID Where a.OrderID=" + orderId + ""))
            {
                if (dtOrders.Rows.Count > 0)
                {
                    DataRow row = dtOrders.Rows[0];

                    addressId = row["FK_AddressId"].ToString();
                    customerId = row["FK_OrderCustomerID"].ToString();
                    collectableAmount = Convert.ToDouble(row["OrderAmount"]);
                    emailId = row["CustomerEmail"].ToString();
                    custMobile = row["CustomerMobile"].ToString();
                    custName = row["CustomerName"].ToString();

                }
            }

            string custAddress = c.GetReqData("CustomersAddress", "AddressFull", "AddressID=" + addressId + "").ToString();


            string custPincode = c.GetReqData("CustomersAddress", "AddressPincode", "AddressID=" + addressId + "").ToString();

            //==========================================================================================================================//
            //      check delivery service is availbale at delivery address pincode first and if yes then generate waybill
            //==========================================================================================================================//

            ServiceFinderQueryClient client = new ServiceFinderQueryClient();

            ServiceCenterDetailsReference pincodeDetails = new ServiceCenterDetailsReference();

            ServiceFinderQueryRef.UserProfile userPincode = new ServiceFinderQueryRef.UserProfile();
            userPincode.LoginID = "SAG60897";
            //userPincode.LicenceKey = "hzfukpgpahfoqkplnehojpu6rjkmekfh"; // demo
            userPincode.LicenceKey = "rr5sesemke7nijinfusporovnnxgsjlq"; //production
            userPincode.Api_type = "S";
            userPincode.Area = "SAG";
            userPincode.Customercode = "060664";
            userPincode.Version = "1.9";

            pincodeDetails = client.GetServicesforPincode(custPincode, userPincode);

            bool pincodeError = pincodeDetails.IsError;
            if (pincodeError == true)
            {
                pickupReqStatus = c.ErrNotification(2, pincodeDetails.ErrorMessage + " Delivery service not available at pincode " + custPincode);
                return;
            }

            // if delivery available then generate waybill

            WayBillGenerationClient wayBill = new WayBillGenerationClient();
            WayBillGenerationRequest wayBillReq = new WayBillGenerationRequest();

            //Consignee
            Consignee consigne = new Consignee();

            string custaddr1 = "addr", custaddr2 = "addr", custaddr3 = "addr";
            if (custAddress.Length <= 30)
            {
                custaddr1 = custAddress.ToString();
            }
            else if (custAddress.Length > 30 && custAddress.Length <= 60)
            {
                custaddr1 = custAddress.ToString().Substring(0, 30);
                custaddr2 = custAddress.ToString().Substring(31);
            }
            else if (custAddress.Length > 60 && custAddress.Length <= 90)
            {
                custaddr1 = custAddress.ToString().Substring(0, 30);
                custaddr2 = custAddress.ToString().Substring(30, Math.Min(custAddress.Length, 30));
                custaddr3 = custAddress.ToString().Substring(Math.Min(custAddress.Length, 60));
            }
            else
            {
                custaddr1 = custAddress.ToString().Substring(0, 30);
                custaddr2 = custAddress.ToString().Substring(30, Math.Min(custAddress.Length, 30));
                custaddr3 = custAddress.ToString().Substring(Math.Min(custAddress.Length, 60), Math.Min(custAddress.Length, 29));
            }


            consigne.ConsigneeAddress1 = custaddr1;
            consigne.ConsigneeAddress2 = custaddr2;
            consigne.ConsigneeAddress3 = custaddr3;

            consigne.ConsigneeAttention = custName;
            //consigne.ConsigneeEmailID = shopEmail;
            consigne.ConsigneeMobile = custMobile;
            consigne.ConsigneeName = custName;
            consigne.ConsigneePincode = custPincode;
            //consigne.ConsigneeTelephone = ShopMob;

            wayBillReq.Consignee = consigne;


            //ReturnAddress
            //ReturnAddress retAddr = new ReturnAddress();
            //retAddr.ReturnAddress1 = ShopAddr;
            //retAddr.ReturnAddress1 = ShopAddr;
            //retAddr.ReturnAddress2 = ShopAddr;
            //retAddr.ReturnAddress3 = ShopAddr;
            //retAddr.ReturnContact = shopOwnerName;
            //retAddr.ReturnEmailID = shopEmail;
            //retAddr.ReturnMobile = ShopMob;
            //retAddr.ReturnPincode = consigneePinCode;

            //wayBillReq.Returnadds = retAddr;


            //Services

            Double actualWeight = Convert.ToDouble(txtWeight.Text);
            Double declaredValue = 100;
            string creditRef = "orderref" + orderId.ToString();

            Services serv = new Services();

            serv.ActualWeight = actualWeight;
            serv.CreditReferenceNo = creditRef;
            serv.DeclaredValue = declaredValue;


            //List<Dimension> dList = new List<Dimension>()
            //{
            //    new Dimension{Breadth=0.1, Count=1, Height=0.1, Length=0.1}
            //};
            List<Dimension> dList = new List<Dimension>()
            {
                new Dimension{Breadth=Convert.ToDouble(txtBreadth.Text), Count=1, Height=Convert.ToDouble(txtHeight.Text), Length=Convert.ToDouble(txtWidth.Text)}
            };
            serv.Dimensions = dList.ToArray();


            List<ItemDetails> idtl = new List<ItemDetails>()
            {
                new ItemDetails{ItemValue=collectableAmount, Itemquantity=1, ItemID="Item01", ItemName="Medicines", ProductDesc1="Genericart Products"}
            };
            serv.itemdtl = idtl.ToArray();


            //serv.InvoiceNo = "ABC123";
            serv.IsForcePickup = true;
            //serv.IsPartialPickup = false;
            serv.IsReversePickup = false;
            serv.ItemCount = 1;
            //serv.PDFOutputNotRequired = true;
            serv.PDFOutputNotRequired = false;

            serv.PickupDate = pickupDate;
            string pickupTime = "";
            if (txtPickupTime.Text.Contains(':'))
            {
                string[] arrPickup = txtPickupTime.Text.Split(':');
                pickupTime = arrPickup[0].ToString() + arrPickup[1].ToString();
            }
            serv.PickupTime = pickupTime;
            serv.PieceCount = 1;
            serv.ProductCode = "A";
            string subProdCode = ddrPay.SelectedValue == "1" ? "P" : "C";
            serv.SubProductCode = subProdCode.ToString();
            double amount = ddrPay.SelectedValue == "1" ? 0 : collectableAmount;
            serv.CollectableAmount = amount;
            serv.ProductType = (int)duti.Dutiables;
            serv.RegisterPickup = true;

            serv.SpecialInstruction = "urgent";



            wayBillReq.Services = serv;


            //Shipper
            Shipper shipper = new Shipper();

            string addr1 = "addr", addr2 = "addr", addr3 = "addr";
            if (ShopAddr.Length <= 30)
            {
                addr1 = ShopAddr.ToString();
            }
            else if (ShopAddr.Length > 30 && ShopAddr.Length <= 60)
            {
                addr1 = ShopAddr.ToString().Substring(0, 30);
                addr2 = ShopAddr.ToString().Substring(31);
            }
            else if (ShopAddr.Length > 60 && ShopAddr.Length <= 90)
            {
                addr1 = ShopAddr.ToString().Substring(0, 30);
                addr2 = ShopAddr.ToString().Substring(30, Math.Min(ShopAddr.Length, 30));
                addr3 = ShopAddr.ToString().Substring(Math.Min(ShopAddr.Length, 60));
            }
            else
            {
                addr1 = ShopAddr.ToString().Substring(0, 30);
                addr2 = ShopAddr.ToString().Substring(30, Math.Min(ShopAddr.Length, 30));
                addr3 = ShopAddr.ToString().Substring(Math.Min(ShopAddr.Length, 60), Math.Min(ShopAddr.Length, 29));
            }


            shipper.CustomerAddress1 = addr1;
            shipper.CustomerAddress2 = addr2;
            shipper.CustomerAddress3 = addr3;

            //shipper.CustomerCode = "060664";
            //shipper.CustomerEmailID = emailId;
            shipper.CustomerMobile = ShopMob;
            shipper.CustomerName = shopName;
            shipper.CustomerPincode = ShopPinCode;
            shipper.IsToPayCustomer = false;

            //set Origin area depending on shop location
            int shopId = Convert.ToInt32(Session["adminFranchisee"]);
            string originArea = "SAG";
            //string custCode = "060664";
            string custCode = "119836";
            if (shopId == 24 || shopId == 2010 || shopId == 2062 || shopId == 2070)
            {
                originArea = "SAG";
                custCode = "119836";
            }
            else if (shopId == 2012)
            {
                originArea = "NBM";
                custCode = "119836";
            }
            else if (shopId == 2064 || shopId == 2065 || shopId == 2137 || shopId == 2142)
            {
                originArea = "BOM";
                custCode = "119836";
            }
            else if (shopId == 2124)
            {
                originArea = "JAI";
                custCode = "119836";
            }

            shipper.CustomerCode = custCode;
            shipper.OriginArea = originArea;
            shipper.Sender = "Genericart Medicine";
            shipper.VendorCode = "ABCD";



            wayBillReq.Shipper = shipper;

            WayBillGenerationRef.UserProfile user = new WayBillGenerationRef.UserProfile();
            user.LoginID = "SAG60897";
            //user.LicenceKey = "hzfukpgpahfoqkplnehojpu6rjkmekfh"; // demo
            user.LicenceKey = "rr5sesemke7nijinfusporovnnxgsjlq"; //production
            user.Api_type = "S";
            user.Area = "SAG";
            user.Customercode = "119836";
            user.Version = "1.9";

            WayBillGenerationResponse wayBillResponse = wayBill.GenerateWayBill(wayBillReq, user);

            bool isError = wayBillResponse.IsError;
            StringBuilder strError = new StringBuilder();

            WayBillGenerationStatus[] st = new WayBillGenerationStatus[5];

            st = wayBillResponse.Status;

            if (isError)
            {
                strError.Append("Error Occoured");

                //strError.Append("<br/>Code :" + st.StatusCode);
                //strError.Append("<br/>Info :" + st.StatusInformation);
                for (int i = 0; i < st.Length; i++)
                {
                    strError.Append("<br/>Code :" + st[i].StatusCode);
                    strError.Append("<br/>Info :" + st[i].StatusInformation);
                }


                pickupReqStatus = c.ErrNotification(2, strError.ToString());
                return;
            }
            else
            {
                string awNo = wayBillResponse.AWBNo;
                //strError.Append("<br/>Code :" + st.StatusCode);
                //strError.Append("<br/>Info :" + st.StatusInformation);

                string creditRefNo = wayBillResponse.CCRCRDREF;
                string destinationArea = wayBillResponse.DestinationArea;
                string destinationLoc = wayBillResponse.DestinationLocation;
                string error = wayBillResponse.IsError == true ? "true" : "false";
                string errorPu = wayBillResponse.IsErrorInPU == true ? "true" : "false";
                //string shipmentpickupDate = wayBillResponse.ShipmentPickupDate.ToString("dd/MM/yyyy HH:mm:ss");
                //string shipmentpickupDate = pickupDate.ToString("dd/MM/yyyy HH:mm:ss");
                string shipmentpickupDate = pickupDate.ToString("dd/MM/yyyy") + " " + txtPickupTime.Text + ":00";
                string tokenNo = wayBillResponse.TokenNumber;
                string statusCode = st[0].StatusCode;
                string statusInfo = st[0].StatusInformation;
                string statusCode1 = st[1].StatusCode;
                string statusInfo1 = st[1].StatusInformation;

                byte[] awbPrintContent = wayBillResponse.AWBPrintContent;
                string awbpdf = awbPrintContent.Length.ToString();

                string filePath = "~/upload/awb_waybills/";
                string fileName = "waybill-" + orderId + ".pdf";
                using (FileStream stream = System.IO.File.Create(Server.MapPath(filePath) + fileName))
                {
                    string base64String = Convert.ToBase64String(awbPrintContent, 0, awbPrintContent.Length);
                    byte[] byteArray = Convert.FromBase64String(base64String);
                    stream.Write(byteArray, 0, byteArray.Length);
                }

                //insert success details in database table

                int maxId = c.NextId("WayBillResult", "WayBillID");

                c.ExecuteQuery("Insert Into WayBillResult (WayBillID, WayBillDate, FK_OrderID, FK_AssignID, AWBNo, CCRCRDREF, DestinationArea, " +
                " DestinationLocation, IsError, IsErrorInPU, ShipmentPickupDate, WayBillStatusCode, WayBillStatusInfo, " +
                " WayBillStatusCode1, WayBillStatusInfo1, TokenNumber, WayBillType) Values (" + maxId + ", '" + DateTime.Now + "', '" + Request.QueryString["id"] +
                "', '" + Request.QueryString["assignId"] + "', '" + awNo + "', '" + creditRefNo + "', '" + destinationArea + "', '" + destinationLoc +
                "', '" + error + "', '" + errorPu + "', '" + shipmentpickupDate + "', '" + statusCode + "', '" + statusInfo +
                "', '" + statusCode1 + "', '" + statusInfo1 + "', '" + tokenNo + "', 1)");

                for (int i = 0; i < st.Length; i++)
                {
                    strError.Append("<br/>Code :" + st[i].StatusCode);
                    strError.Append("<br/>Info :" + st[i].StatusInformation);
                }

                strError.Append("<br/>" + "AWB No : " + awNo.ToString());
                strError.Append("<br/> <a href=\"" + Master.rootPath + "upload/awb_waybills/waybill-" + orderId + ".pdf\" target=\"_blank\">Download Waybill PDF File</a>");
                pickupReqStatus = c.ErrNotification(1, strError.ToString());

            }

            txtPickupDate.Text = txtPickupTime.Text = txtWeight.Text = txtWidth.Text = txtHeight.Text = txtBreadth.Text = "";
            ddrPay.SelectedIndex = 0;

            wayBill.Close();

            //create object of PickupRegistrationClient, PickupRegistrationRequest and pass parameters to api

            //PickupRegistrationClient client = new PickupRegistrationClient();
            //PickupRegistrationRequest regRequest = new PickupRegistrationRequest();

            //regRequest.AreaCode = "SAG";
            //regRequest.CISDDN = false;
            //regRequest.ContactPersonName = shopOwnerName;
            //regRequest.CustomerAddress1 = ShopAddr;
            //regRequest.CustomerCode = "060664";
            //regRequest.CustomerName = shopOwnerName;
            //regRequest.CustomerPincode = ShopPinCode;
            //regRequest.CustomerTelephoneNumber = ShopMob;
            //regRequest.DoxNDox = "1";
            //regRequest.EmailID = shopEmail;
            //regRequest.IsForcePickup = true;
            //regRequest.IsReversePickup = false;
            //regRequest.MobileTelNo = ShopMob;
            //regRequest.NumberofPieces = 1;
            //regRequest.OfficeCloseTime = "22:00";
            //regRequest.ProductCode = "A";
            //regRequest.RouteCode = "99";
            //regRequest.ShipmentPickupDate = pickupDate;
            //regRequest.ShipmentPickupTime = txtPickupTime.Text;
            //string[] arrSubProd = new string[1] { "E-Tailing" };
            //regRequest.SubProducts = arrSubProd;
            //regRequest.VolumeWeight = 1.2;
            //regRequest.WeightofShipment = 1.2;
            //regRequest.isToPayShipper = false;


            //UserProfile user = new UserProfile();
            //user.LoginID = "SAG60897";
            //user.LicenceKey = "rr5sesemke7nijinfusporovnnxgsjlq"; //production
            //user.Api_type = "S";
            //user.Area = "SAG";
            //user.Customercode = "060664";
            //user.Version = "1.9";

            //PickupRegistrationResponse pickupResponse = client.RegisterPickup(regRequest, user);

            //bool isError = pickupResponse.IsError;

            //if (isError)
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured while submitting request');", true);
            //    return;
            //}
            //else
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Request Submitted Successfully');", true);
            //    //pickupReqStatus = "Request Submitted Successfully";
            //}

            //txtPickupDate.Text = txtPickupTime.Text = "";

            //client.Close();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSendReq_Click", ex.Message.ToString());
            return;
        }
    }

    enum duti
    {
        Dutiables
    }

    protected void btnRet_Click(object sender, EventArgs e)
    {
        try
        {
            txtRetReason.Text = txtRetReason.Text.Trim().Replace("'", "");

            if (txtRetReason.Text == "")
            {
                GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Reason of return order');", true);
                return;
            }

            if (txtRetReason.Text.Length > 100)
            {
                GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Reason must be less than 100 characters');", true);
                return;
            }

            //mark its status OrderAssignStatus=10(OrdersAssign) & OrderStatus=10 (OrdersData) in both tables as "Returned"

            c.ExecuteQuery("Update OrdersAssign Set OrdAssignStatus=10, ReturnReason='" + txtRetReason.Text + "' Where FK_OrderID=" + Request.QueryString["id"] + " AND OrdAssignID=" + Request.QueryString["assignId"]);

            c.ExecuteQuery("Update OrdersData Set OrderStatus=10 Where OrderID=" + Request.QueryString["id"]);

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Order Marked as Returned');", true);

            GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnRet_Click", ex.Message.ToString());
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
                    offlinePay.Visible = true;
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
                            offlinePay.Visible = true;
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
                            offlinePay.Visible = true;
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
                        offlinePay.Visible = true;
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
                        apiResponse = c.ErrNotification(1, "Status : " + statusInfo2.ToString() + ", Msg : " + cMsg2.ToString() + ", status : " + transStatus + ", txnId : " + txnIdX);
                    }
                    else
                    {
                        apiResponse = c.ErrNotification(2, "Status : " + statusInfo2.ToString() + ", Msg : " + cMsg2.ToString());
                        offlinePay.Visible = true;
                    }
                }
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

    protected void btnAddOfflinePayment_Click(object sender, EventArgs e)
    {
        try
        {
            txtUTRNo.Text = txtUTRNo.Text.Trim().Replace("'", "");

            if (txtUTRNo.Text == "")
            {
                GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter UTR No.');", true);
                return;
            }

            int payMode = 0;
            if (chkQR.Checked == true)
            {
                payMode = 3;
            }
            else if (chkGPay.Checked == true)
            {
                payMode = 4;
            }
            else if (chkCash.Checked == true)
            {
                payMode = 5;
            }
            //OrderPayMode=3 -> UPI Payment
            c.ExecuteQuery("Update OrdersData Set OrderPayMode=" + payMode + ", OrderPaymentTxnId='" + txtUTRNo.Text + "', OrderPayStatus=1 Where OrderID=" + Request.QueryString["id"]);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Order Marked as Paid');", true);

            GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));
        }
        catch (Exception ex)
        {
            GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnAddOfflinePayment_Click", ex.Message.ToString());
            return;
        }
    }
}