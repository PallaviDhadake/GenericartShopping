using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;

public partial class checkout : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] arrConInfo = new string[5];
    public string addrStr, prescriStr, errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            btnPlaceOrder.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnPlaceOrder, null) + ";");

            // Check Payment mode option buttons according to GOBP login Session
            string obpSessionToken = Session["adminObp"] as string;

            if (Request.Cookies["MyOBPCookie"] != null)
            {
                rdbCash.Visible = true;
                rdbCash.Checked = true;
                rdbOnline.Visible = false;
                rdbOnline.Checked = false;
                // Clear the cookie
                //Response.Cookies["MyUrlCookie"].Expires = DateTime.Now.AddDays(-1);
            }
            else
            {
                rdbCash.Visible = false;
                rdbCash.Checked = false;
                rdbOnline.Visible = Visible;
                rdbOnline.Checked = Visible;
            }

            if (!IsPostBack)
            {
                //uploadPrescription.Visible = false;

                int orderId = 0, customerId = 0;
                HttpCookie ordCookie = Request.Cookies["ordId"];
                if (ordCookie != null)
                {
                    string[] arrOrd = ordCookie.Value.Split('#');
                    orderId = Convert.ToInt32(arrOrd[0].ToString());
                    customerId = Convert.ToInt32(arrOrd[1].ToString());
                }

                if (String.IsNullOrEmpty(Page.RouteData.Values["chkId"].ToString()))
                {
                    informationForm.Visible = true;
                    payment.Visible = false;
                    paymentDetails.Visible = false;

                    if (orderId > 0)
                    {
                        if (Request.QueryString["id"] != null)
                        {
                            c.ExecuteQuery("Delete From OrderPrescriptions Where PrescriptionID=" + Convert.ToInt32(Request.QueryString["id"]));
                            GetUploadedPrescription(orderId);
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('" + Master.rootPath + "checkout', 1000);", true);
                        }

                        // CHECK ANY OF MEDICINE FROM CART REQUIRE PRESCRIPTION
                        if (c.IsRecordExist("Select a.OrdDetailID, a.FK_DetailProductID From OrdersDetails a Inner Join ProductsData b " + 
                            " on a.FK_DetailProductID=b.ProductID Where a.FK_DetailOrderID=" + orderId + " AND b.PrescriptionFlag=1"))
                        {
                            uploadPrescription.Visible = true;
                            GetUploadedPrescription(orderId);   
                        }
                        else
                        {
                            uploadPrescription.Visible = false;
                        }
                        int memberId = Convert.ToInt32(c.GetReqData("OrdersData", "FK_OrderCustomerID", "OrderID=" + orderId));
                        //if (c.IsRecordExist("Select OrderID From OrdersData Where OrderShipAddress IS NOT NULL AND OrderShipAddress<>'' AND OrderID=" + orderId))
                        //{
                        //    GetOrderInfo(orderId);
                        //}
                        if (c.IsRecordExist("Select OrderID From OrdersData Where FK_AddressId IS NOT NULL AND OrderShipAddress<>'' AND OrderID=" + orderId))
                        {
                            GetOrderInfo(orderId);
                        }
                        else
                        {
                            if (memberId > 0)
                            {
                                GetMembeDetails(memberId);

                                if (c.IsRecordExist("Select AddressID From CustomersAddress Where AddressFKCustomerID=" + customerId))
                                {
                                    newAddr.Visible = false;
                                    existingAddr.Visible = true;
                                    GetCustomerAddress(customerId, orderId);
                                }
                                else
                                {
                                    newAddr.Visible = true;
                                    existingAddr.Visible = false;
                                    chkAddNew.Checked = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    informationForm.Visible = false;
                    uploadPrescription.Visible = false;
                    payment.Visible = true;
                    paymentDetails.Visible = true;
                    GetBriefOrderInfo(orderId);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "Page_Load", ex.Message.ToString());
            return;
        }
    }

    private void GetCustomerAddress(int custIdX, int orderIdX)
    {
        try
        {
            using (DataTable dtCustAddr = c.GetDataTable("Select AddressID, AddressFull, AddressName, AddressCity, AddressPincode From CustomersAddress Where AddressStatus=1 AND AddressFKCustomerID=" + custIdX))
            {
                if (dtCustAddr.Rows.Count > 0)
                {
                    StringBuilder strMarkup = new StringBuilder();
                    int bCount = 0;
                    foreach (DataRow row in dtCustAddr.Rows)
                    {
                        //strMarkup.Append("<span id=\"addr-" + row["AddressID"].ToString() + "\">");
                        //strMarkup.Append("<div class=\"w50 float_left\">");
                        //strMarkup.Append("<div class=\"pad_15\">");
                        //strMarkup.Append("<div class=\"box-shadow bgWhite border_r_5\">");
                        //strMarkup.Append("<a href=\"" + Master.rootPath + "add-addr/" + row["AddressID"].ToString() + "-" + orderIdX + "\" class=\"addrBorder txtDecNone\">");
                        //strMarkup.Append("<div class=\"pad_10\" style=\"cursor:pointer\">");
                        //strMarkup.Append("<span class=\"clrGrey small fontRegular line-ht-5\">" + row["AddressFull"].ToString() + "</span>");
                        //strMarkup.Append("</div>");
                        //strMarkup.Append("</a>");
                        //strMarkup.Append("</div>");
                        //strMarkup.Append("</div>");
                        //strMarkup.Append("</div>");
                        //strMarkup.Append("</span>");
                        strMarkup.Append("<div class=\"w50 float_left\">");
                        strMarkup.Append("<div style=\"padding:10px 30px 10px 0;\">");
                        strMarkup.Append("<span id=\"addr-" + row["AddressID"].ToString() + "\">");
                        if (!c.IsRecordExist("Select OrderID From OrdersData Where OrderID=" + orderIdX + " AND FK_AddressId=" + row["AddressID"].ToString()))
                        {
                            strMarkup.Append("<a href=\"" + Master.rootPath + "add-addr/" + row["AddressID"].ToString() + "-" + orderIdX + "\" class=\"addrBorder txtDecNone\"  style=\"display:block;\">");
                            strMarkup.Append("<div class=\"box-shadow bgWhite border_r_5\">");
                            strMarkup.Append("<div class=\"pad_10\" style=\"cursor:pointer\">");
                            strMarkup.Append("<span class=\"themeClrPrime fontRegular\">" + row["AddressName"].ToString() + "</span>");
                            strMarkup.Append("<span class=\"space5\"></span>");
                            strMarkup.Append("<span class=\"clrGrey small fontRegular line-ht-5\">" + row["AddressFull"].ToString() + "</span>");
                            strMarkup.Append("<span class=\"space10\"></span>");
                            strMarkup.Append("<span class=\"clrGrey small fontRegular line-ht-5\">" + row["AddressCity"].ToString() + ", " + row["AddressPincode"].ToString() + "</span>");
                            strMarkup.Append("</div>");
                            strMarkup.Append("</div>");
                            strMarkup.Append("</a>");
                        }
                        else
                        {
                            strMarkup.Append("<a class=\"addrBorderBlue txtDecNone\"  style=\"display:block;\">");
                            strMarkup.Append("<div class=\"box-shadow bgWhite border_r_5\">");
                            strMarkup.Append("<div class=\"pad_10\" style=\"cursor:pointer\">");
                            strMarkup.Append("<span class=\"themeClrPrime fontRegular\">" + row["AddressName"].ToString() + "</span>");
                            strMarkup.Append("<span class=\"space5\"></span>");
                            strMarkup.Append("<span class=\"clrGrey small fontRegular line-ht-5\">" + row["AddressFull"].ToString() + "</span>");
                            strMarkup.Append("<span class=\"space10\"></span>");
                            strMarkup.Append("<span class=\"clrGrey small fontRegular line-ht-5\">" + row["AddressCity"].ToString() + ", " + row["AddressPincode"].ToString() + "</span>");
                            strMarkup.Append("<span class=\"space10\"></span>");
                            strMarkup.Append("<span class=\"semiBold themeClrPrime\">Address Selected</span>");
                            strMarkup.Append("</div>");
                            strMarkup.Append("</div>");
                            strMarkup.Append("</a>");
                        }
                        strMarkup.Append("</span>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");

                        bCount++;

                        if ((bCount % 2) == 0)
                        {
                            strMarkup.Append("<div class=\"float_clear\"></div>");
                        }
                    }
                    strMarkup.Append("<div class=\"float_clear\"></div>");
                    addrStr = strMarkup.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetCustomerAddress", ex.Message.ToString());
            return;
        }
    }

    private void GetMembeDetails(int memIdX)
    {
        try
        {
            using (DataTable dtMem = c.GetDataTable("Select CustomrtID, CustomerEmail, CustomerMobile, CustomerName, CustomerCity, " +
                " CustomerState, CustomerPincode, CustomerCountry, CustomerAddress From CustomersData Where CustomrtID=" + memIdX))
            {
                if (dtMem.Rows.Count > 0)
                {
                    DataRow row = dtMem.Rows[0];

                    txtMobile.Text = row["CustomerMobile"].ToString();

                    txtName.Text = row["CustomerName"].ToString();
                    //txtAddress1.Text = row["CustomerAddress"].ToString();
                    //txtCountry.Text = row["CustomerCountry"].ToString();
                    //txtState.Text = row["CustomerState"].ToString();
                    //txtCity.Text = row["CustomerCity"].ToString();
                    //txtPinCode.Text = row["CustomerPincode"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetMembeDetails", ex.Message.ToString());
            return;
        }
    }

    private void GetOrderInfo(int orderIdX)
    {
        try
        {
            using (DataTable dtMem = c.GetDataTable("Select OrderContactInfo, FK_OrderCustomerID, OrderShipAddress, OrderCity, OrderState, " +
                " OrderZipCode, OrderCountry From OrdersData Where OrderID=" + orderIdX))
            {
                if (dtMem.Rows.Count > 0)
                {
                    DataRow row = dtMem.Rows[0];

                    txtMobile.Text = row["OrderContactInfo"].ToString();

                    txtName.Text = c.GetReqData("CustomersData", "CustomerName", "CustomrtID=" + row["FK_OrderCustomerID"]).ToString();

                    txtAddress1.Text = row["OrderShipAddress"].ToString();
                    //txtAddress2.Text = row["OrderShipAddress2"] != DBNull.Value && row["OrderShipAddress2"] != null && row["OrderShipAddress2"].ToString() != "" ? row["OrderShipAddress2"].ToString() : "";
                    txtCountry.Text = row["OrderCountry"].ToString();
                    txtState.Text = row["OrderState"].ToString();
                    txtCity.Text = row["OrderCity"].ToString();
                    txtPinCode.Text = row["OrderZipCode"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetOrderInfo", ex.Message.ToString());
            return;
        }
    }

    protected void btnShipping_Click(object sender, EventArgs e)
    {
        try
        {
            txtMobile.Text = txtMobile.Text.Trim().Replace("'", "");
            txtName.Text = txtName.Text.Trim().Replace("'", "");
            txtCountry.Text = txtCountry.Text.Trim().Replace("'", "");
            txtState.Text = txtState.Text.Trim().Replace("'", "");
            txtCity.Text = txtCity.Text.Trim().Replace("'", "");
            txtPinCode.Text = txtPinCode.Text.Trim().Replace("'", "");
            txtAddress1.Text = txtAddress1.Text.Trim().Replace("'", "");
            //txtAddress2.Text = txtAddress2.Text.Trim().Replace("'", "");

            int orderId = 0, customerId = 0;
            HttpCookie ordCookie = Request.Cookies["ordId"];
            if (ordCookie != null)
            {
                string[] arrOrd = ordCookie.Value.Split('#');
                orderId = Convert.ToInt32(arrOrd[0].ToString());
                customerId = Convert.ToInt32(arrOrd[1].ToString());
            }


            if (chkAddNew.Checked == false) 
            {
                if (c.IsRecordExist("Select OrderID From OrdersData Where OrderID=" + orderId + " AND  (FK_AddressId IS NULL OR FK_AddressId=0)"))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select Address To Proceed');", true);
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('" + Master.rootPath + "checkout', 1000);", true);
                }
                else
                {
                    c.ExecuteQuery("Update OrdersData Set OrderContactInfo='" + txtMobile.Text + "' Where OrderID=" + orderId);
                    Response.Redirect(Master.rootPath + "checkout/payment", false);
                }
            }
            else
            {

                if (txtName.Text == "" || txtCountry.Text == "" ||
                    txtState.Text == "" || txtCity.Text == "" || txtPinCode.Text == "" || txtAddress1.Text == "" || ddrAddrName.SelectedIndex == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * Marked fields are Mandatory');", true);
                    return;
                }
                if (txtMobile.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Contact Information');", true);
                    return;
                }
                if (c.ValidateMobile(txtMobile.Text) == false)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter valid mobile number');", true);
                    return;
                }
                if (!c.IsNumeric(txtPinCode.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Pincode must be numeric value');", true);
                    return;
                }

                //c.ExecuteQuery("Update OrdersData Set OrderShipAddress='" + txtAddress1.Text + "', OrderCity='" + txtCity.Text +
                //    "', OrderState='" + txtState.Text + "', OrderZipCode='" + txtPinCode.Text + "', OrderCountry='" + txtCountry.Text +
                //    "', OrderContactInfo='" + txtEmail.Text + "' Where OrderID=" + orderId);

                // insert new address of cust in custAddress tbl

                if (chkAddNew.Checked == true)
                {
                    int maxAddrId = c.NextId("CustomersAddress", "AddressID");
                    c.ExecuteQuery("Insert Into CustomersAddress (AddressID, AddressFKCustomerID, AddressFull, AddressCity, AddressState, " +
                        " AddressPincode, AddressCountry, AddressStatus, AddressName) Values (" + maxAddrId + ", " + customerId + ", '" + txtAddress1.Text +
                        "', '" + txtCity.Text + "', '" + txtState.Text + "', '" + txtPinCode.Text + "', '" + txtCountry.Text + "', 1, '" + ddrAddrName.SelectedItem.Text + "')");

                    // update addressId in OrdersData tbl
                    c.ExecuteQuery("Update OrdersData Set OrderContactInfo='" + txtMobile.Text + "', FK_AddressId=" + maxAddrId + " Where OrderID=" + orderId);

                    Response.Redirect(Master.rootPath + "checkout/payment", false);
                }
            }

            
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnShipping_Click", ex.Message.ToString());
            return;
        }
    }

    private void GetBriefOrderInfo(int orderIdX)
    {
        try
        {
            arrConInfo[0] = c.GetReqData("OrdersData", "OrderContactInfo", "OrderID=" + orderIdX).ToString();
            //arrConInfo[1] = c.GetReqData("OrdersData", "OrderShipAddress", "OrderID=" + orderIdX).ToString();
            int orderAddrId = Convert.ToInt32(c.GetReqData("OrdersData", "FK_AddressId", "OrderID=" + orderIdX));
            string addrName = c.GetReqData("CustomersAddress", "AddressName", "AddressID=" + orderAddrId).ToString();
            arrConInfo[1] = c.GetReqData("CustomersAddress", "AddressFull", "AddressID=" + orderAddrId).ToString() + ", <span class=\"themeClrSec fontRegular\">(" + addrName + ")</span>";

            //arrConInfo[2] = c.returnAggregate("Select SUM(OrdDetailPrice) From OrdersDetails Where FK_DetailOrderID=" + orderIdX).ToString("0.00");
            arrConInfo[2] = c.returnAggregate("Select SUM(OrdDetailAmount) From OrdersDetails Where FK_DetailOrderID=" + orderIdX).ToString("0.00");
            if (Convert.ToDouble(arrConInfo[2].ToString()) > 500)
            {
                arrConInfo[3] = "0.00";
            }
            else
            {
                if (rdbPickup.Checked == true)
                {
                    arrConInfo[3] = "0.00";
                }
                else
                {
                    arrConInfo[3] = "30.00"; //30.00
                }
            }

            arrConInfo[4] = (Convert.ToDouble(arrConInfo[2].ToString()) + Convert.ToDouble(arrConInfo[3].ToString())).ToString("0.00");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetBriefOrderInfo", ex.Message.ToString());
            return;
        }
    }
    protected void btnPlaceOrder_Click(object sender, EventArgs e)
    {
        try
        {
            int orderId = 0, customerId = 0;
            HttpCookie ordCookie = Request.Cookies["ordId"];
            if (ordCookie != null)
            {
                string[] arrOrd = ordCookie.Value.Split('#');
                orderId = Convert.ToInt32(arrOrd[0].ToString());
                customerId = Convert.ToInt32(arrOrd[1].ToString());
            }

            GetBriefOrderInfo(orderId);

            //if (rdbCash.Checked == false && rdbOnline.Checked == false)
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select Payment Mode');", true);
            //    return;
            //}
            if (rdbOnline.Checked == false && rdbCash.Checked == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select Payment Mode');", true);
                return;
            }

            if (rdbHome.Checked == false && rdbPickup.Checked == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select Delivery Method');", true);
                return;
            }

            if (txtNote.Text != "")
            {
                if (txtNote.Text.Length > 200)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Note must be less than 200 characters');", true);
                    return;
                }
            }

            int payMode = 0;
            if (rdbCash.Checked == true)
            {
                payMode = 1;
            }
            else if (rdbOnline.Checked == true)
            {
                payMode = 2;
            }

            int deliveryMode = 0;

            if (rdbHome.Checked == true)
            {
                deliveryMode = 1;
            }
            else if (rdbPickup.Checked == true)
            {
                deliveryMode = 2;
            }

            if (Convert.ToDouble(arrConInfo[4].ToString()) < 100)
            {
                if (deliveryMode == 1)
                {
                    errMsg = "<span class=\"small semiBold dispBlk\" style=\"color:red; margin-bottom:20px;\">Sorry..! We can not proceed orders under Rs. 100 for Home Delivery, You can select Self Pickup option to proceed order</span>";
                    return;
                }
            }

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

            

            if (c.IsRecordExist("Select CustomrtID From CustomersData Where CustomerFavShop IS NOT NULL AND CustomerFavShop<>'' AND CustomerFavShop<>0 AND CustomrtID=" + customerId))
            {
                // route order direct to shop

                int franchId = Convert.ToInt32(c.GetReqData("CustomersData", "CustomerFavShop", "CustomrtID=" + customerId));

                // set order status to 3 -> as it is accepted by admin 
                //c.ExecuteQuery("Update OrdersData Set OrderAmount=" + Convert.ToDouble(arrConInfo[4].ToString()) + ", OrderPayMode=" + payMode +
                //    ", OrderPayStatus=0, OrderStatus=3, OrderNote='" + txtNote.Text + "', DeliveryType=" + deliveryMode + ", OrderShippingAmount=" + arrConInfo[3].ToString() + " Where OrderID=" + orderId);


                //changed on 28 sept 2022
                c.ExecuteQuery("Update OrdersData Set OrderAmount=" + Convert.ToDouble(arrConInfo[2].ToString()) +
                    ", OrderPayableAmount=" + Convert.ToDouble(arrConInfo[4].ToString()) + ", OrderPayMode=" + payMode +
                    ", OrderPayStatus=0, OrderStatus=3, OrderNote='" + txtNote.Text + "', DeliveryType=" + deliveryMode +
                    ", OrderShippingAmount=" + arrConInfo[3].ToString() + ", GOBPId=" + gobpId + " Where OrderID=" + orderId);

                if (!c.IsRecordExist("Select OrdAssignID From OrdersAssign Where FK_OrderID=" + orderId + " AND Fk_FranchID=" + franchId + " AND  OrdAssignStatus=0"))
                {
                    // insert entry in OrderAssign table, it is directly assigned to customers fav shop
                    c.ExecuteQuery("Update OrdersAssign Set OrdReAssign=1 Where FK_OrderID=" + orderId);
                    int maxId = c.NextId("OrdersAssign", "OrdAssignID");
                    c.ExecuteQuery("Insert Into OrdersAssign (OrdAssignID, OrdAssignDate, FK_OrderID, Fk_FranchID, OrdAssignStatus, " +
                        " OrdReAssign, AssignedFrom) Values (" + maxId + ", '" + DateTime.Now + "', " + orderId + ", " + franchId + ", 0, 0, 'website')");
                }

                // send sms to shop

                string frName = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + franchId).ToString();
                string mobNo = c.GetReqData("FranchiseeData", "FranchMobile", "FranchID=" + franchId).ToString();
                string pendingOrdCount = c.returnAggregate("Select Count(OrdAssignID) From OrdersAssign Where OrdAssignStatus=0 AND OrdReAssign=0 AND Fk_FranchID=" + franchId).ToString();
                string msgData = "Dear " + frName + ", You have received new order Order No " + orderId + " from Genericart Mobile App.Total Pending Order is/are " + pendingOrdCount + " Thank you Genericart Medicine Store Wahi Kaam, Sahi Daam";
                c.SendSMS(msgData, mobNo);
                //c.SendSMS(msgData, "8408027474");
            }
            else
            {
                // orderStatus=0 > added to cart, 1 > Order placed (New/pending), 2 > Cancelled By Customer , 3 > Accepted, 4 > Denied, 5 > Processing, 6 > Shipped, 7 > Delivered
                
                //c.ExecuteQuery("Update OrdersData Set OrderAmount=" + Convert.ToDouble(arrConInfo[4].ToString()) + ", OrderPayMode=" + payMode +
                //    ", OrderPayStatus=0, OrderStatus=1, OrderNote='" + txtNote.Text + "', DeliveryType=" + deliveryMode + " Where OrderID=" + orderId);



                c.ExecuteQuery("Update OrdersData Set OrderAmount=" + Convert.ToDouble(arrConInfo[2].ToString()) + 
                    ",  OrderPayableAmount=" + Convert.ToDouble(arrConInfo[4].ToString()) + ", OrderPayMode=" + payMode +
                    ", OrderPayStatus=0, OrderStatus=1, OrderNote='" + txtNote.Text + "', DeliveryType=" + deliveryMode +
                    ", OrderShippingAmount=" + arrConInfo[3].ToString() + ", GOBPId=" + gobpId + " Where OrderID=" + orderId);
            }

            //if (gobpId != 0)
            //{
            //    double gobpCom = c.GetOBPComission(orderId);
            //    c.ExecuteQuery("Update OrdersData Set OBPComTotal=" + gobpCom + " Where OrderID=" + orderId);
            //}

            //Generic Mitra
            if (gMitraId != 0)
            {
                double gMitraCom = c.CalCulateGMitraComission(orderId);
                c.ExecuteQuery("Update OrdersData Set GMitraComTotal=" + gMitraCom + ", GMitraId=" + gMitraId + " Where OrderID=" + orderId);
            }


            if (rdbCash.Checked == true)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Your Request Submitted Successfully..!!');", true);

                Response.Cookies["ordId"].Expires = DateTime.Now.AddDays(-1);
                // Check it GOBP Cookie is ACTIVE, then make cookie and customer session NULL and again activate GOBP session and redirect GOBP dashboard.
                
                if (Request.Cookies["MyOBPCookie"] != null)
                {
                    Response.Cookies["MyOBPCookie"].Expires = DateTime.Now.AddDays(-1);
                    //Clear Customer Session
                    Session["genericCust"] = null;
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('" + Master.rootPath + "obp/dashboard.aspx', 2000);", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('" + Master.rootPath + "', 2000);", true);
                }
                
            }
            else
            {
                if (!c.IsRecordExist("Select OPL_id From online_payment_logs Where OPL_customer_id=" + customerId + " AND OLP_order_no=" + orderId))
                {
                    int maxId = c.NextId("online_payment_logs", "OPL_id");
                    c.ExecuteQuery("Insert Into online_payment_logs (OPL_id, OPL_datetime, OPL_customer_id, OLP_amount, OPL_transtatus, " +
                        " OLP_device_type, OLP_order_no, OLP_order_type) Values (" + maxId + ", '" + DateTime.Now + "', " + customerId +
                        ", " + Convert.ToDouble(arrConInfo[4].ToString()) + ", 'created', 'Web', '" + orderId + "', 1)");


                    Response.Redirect(Master.rootPath + "payment?orderId=" + orderId + "&oplId=" + maxId, false);

                    Response.Cookies["ordId"].Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies["MyOBPCookie"].Expires = DateTime.Now.AddDays(-1);
                }
            }
            
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnPlaceOrder_Click", ex.Message.ToString());
            return;
        }
    }

    protected void chkAddNew_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAddNew.Checked == true)
        {
            newAddr.Visible = true;
            existingAddr.Visible = false;
        }
        else
        {
            newAddr.Visible = false;
            existingAddr.Visible = true;
            Response.Redirect(Master.rootPath + "checkout", false);
        }
    }
    protected void btnUploadRx_Click(object sender, EventArgs e)
    {
        try
        {
            int orderId = 0;
            HttpCookie ordCookie = Request.Cookies["ordId"];
            if (ordCookie != null)
            {
                string[] arrOrd = ordCookie.Value.Split('#');
                orderId = Convert.ToInt32(arrOrd[0].ToString());
            }

            string imgName = "";
            int prescriptionId = c.NextId("OrderPrescriptions", "PrescriptionID");
            if (fuPrescription.HasFile)
            {
                string fExt = Path.GetExtension(fuPrescription.FileName).ToString().ToLower();
                if (fExt == ".jpg" || fExt == ".jpeg" || fExt == ".png")
                {
                    imgName = "med-rx-" + prescriptionId + fExt;
                    ImageUploadProcess(imgName);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Only .jpg, .jpeg or .png files are allowed');", true);
                    //return;
                }

                c.ExecuteQuery("Insert Into OrderPrescriptions (PrescriptionID, FK_OrderID, PrescriptionName, PrescriptionStatus) " + 
                    " Values (" + prescriptionId + ", " + orderId + ", '" + imgName + "',0)");

                GetUploadedPrescription(orderId);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select image to upload prescription');", true);
                //return;
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('" + Master.rootPath + "checkout', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnUploadRx_Click", ex.Message.ToString());
            return;
        }
    }

    private void ImageUploadProcess(string imgName)
    {
        try
        {
            string origImgPath = "~/upload/prescriptions/original/";
            string normalImgPath = "~/upload/prescriptions/";

            fuPrescription.SaveAs(Server.MapPath(origImgPath) + imgName);

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
            using (DataTable dtRx = c.GetDataTable("Select PrescriptionID, PrescriptionName From OrderPrescriptions Where FK_OrderID=" + ordIdX))
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
                        strMarkup.Append("<a href=\"" + Master.rootPath + "upload/prescriptions/" + row["PrescriptionName"] + "\" data-fancybox=\"rxGroup\"><img src=\"" + Master.rootPath + "upload/prescriptions/" + row["PrescriptionName"] + "\" class=\"width100\" /></a>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("<a href=\"checkout?id=" + row["PrescriptionID"] + "\" title=\"Delete Prescription\"  class=\"closeAnch\"></a>");
                        strMarkup.Append("</div>");

                        bCount++;

                        if ((bCount % 4) == 0)
                        {
                            strMarkup.Append("<div class=\"float_clear\"></div>");
                        }
                    }
                    strMarkup.Append("<div class=\"float_clear\"></div>");
                    prescriStr = strMarkup.ToString();
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

    protected void rdbPickup_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int orderId = 0, customerId = 0;
            HttpCookie ordCookie = Request.Cookies["ordId"];
            if (ordCookie != null)
            {
                string[] arrOrd = ordCookie.Value.Split('#');
                orderId = Convert.ToInt32(arrOrd[0].ToString());
                customerId = Convert.ToInt32(arrOrd[1].ToString());
            }

            GetBriefOrderInfo(orderId);

            
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "rdbPickup_CheckedChanged", ex.Message.ToString());
            return;
        }
    }
    protected void rdbHome_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int orderId = 0, customerId = 0;
            HttpCookie ordCookie = Request.Cookies["ordId"];
            if (ordCookie != null)
            {
                string[] arrOrd = ordCookie.Value.Split('#');
                orderId = Convert.ToInt32(arrOrd[0].ToString());
                customerId = Convert.ToInt32(arrOrd[1].ToString());
            }

            GetBriefOrderInfo(orderId);


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "rdbHome_CheckedChanged", ex.Message.ToString());
            return;
        }
    }
}