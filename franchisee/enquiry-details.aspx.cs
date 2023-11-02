using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;
using ServiceFinderQueryRef;
using WayBillGenerationRef;

public partial class franchisee_enquiry_details : System.Web.UI.Page
{
    iClass c = new iClass();
    public string customerId, errMsg, orderCount, shippingCharges, prescriptionStr, rdrUrl, mreq, ordId, deviceType, pickupReqStatus;

    public string[] ordData = new string[20]; //15
    public string[] ordCustData = new string[10];
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                returnOrd.Visible = false;
                if (!c.IsRecordExist("Select EnqAssignID From SavingEnqAssign Where FK_CalcID=" + Request.QueryString["id"] + " AND Fk_FranchID=" + Session["adminFranchisee"]))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Something Went Wrong');", true);

                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('enquiry-report.aspx', 2000);", true);
                }

                int ordStatusTemp = Convert.ToInt32(c.GetReqData("SavingEnqAssign", "EnqAssignStatus", "FK_CalcID=" + Request.QueryString["id"] + " AND Fk_FranchID=" + Session["adminFranchisee"]));
                int ordReAssignTemp = Convert.ToInt32(c.GetReqData("SavingEnqAssign", "EnqReAssign", "FK_CalcID=" + Request.QueryString["id"] + "AND Fk_FranchID=" + Session["adminFranchisee"]));

                object shipName = c.GetReqData("SavingCalc", "OrderShipName", "CalcID=" + Request.QueryString["id"]);
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
                    btnReject.Visible = true;
                    dispatchForm.Visible = false;
                    btnDispatch.Visible = false;
                }
                else
                {
                    btnAccept.Visible = false;
                    //btnReject.Visible = false;

                    if (ordStatusTemp == 2)
                    {
                        dispatchForm.Visible = false;
                    }
                    else
                    {
                        dispatchForm.Visible = true;
                    }
                    if (ordStatusTemp == 1 || ordStatusTemp == 5 || ordStatusTemp == 6)
                    {
                        btnDispatch.Visible = true;
                        amountEnq.Visible = true;
                    }
                    else
                    {
                        btnDispatch.Visible = false;
                        btnReject.Visible = false;
                        amountEnq.Visible = true;
                    }

                    if (ordStatusTemp == 6)
                    {
                        btnReject.Visible = false;
                        btnUpdateAmount.Visible = false;
                    }

                    if (ordStatusTemp == 1 || ordStatusTemp == 5)
                    {
                        using (DataTable dtCompShop = c.GetDataTable("Select FK_FranchID From CompanyOwnShops Where DelMark=0"))
                        {
                            if (dtCompShop.Rows.Count > 0)
                            {
                                foreach (DataRow row in dtCompShop.Rows)
                                {
                                    if (Convert.ToInt32(row["FK_FranchID"]) == Convert.ToInt32(Session["adminFranchisee"]))
                                    {
                                        pickupReq.Visible = true;
                                    }
                                }
                            }
                        }
                    }
                }
                ordId = Request.QueryString["id"].ToString();
                GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));

                object mreFlag = c.GetReqData("SavingCalc", "MreqFlag", "CalcID=" + Request.QueryString["id"]);
                if (mreFlag != DBNull.Value && mreFlag != null && mreFlag.ToString() != "")
                {
                    if (mreFlag.ToString() == "1")
                    {
                        mreq = "<span class=\"medium clrProcessing bold_weight\">Customer Marked This Enquiry as Monthly Order</span><span class=\"space10\"></span>";
                    }
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

    public void GetOrdersData(int Idx)
    {
        try
        {
            using (DataTable dtOrder = c.GetDataTable("Select * From SavingCalc Where CalcID=" + Idx))
            {
                if (dtOrder.Rows.Count > 0)
                {
                    DataRow bRow = dtOrder.Rows[0];
                    ordData[0] = Idx.ToString();


                    ordData[1] = Convert.ToDateTime(bRow["CalcDate"]).ToString("dd/MM/yyyy");
                    ordData[11] = Convert.ToDateTime(bRow["CalcDate"]).ToString("hh:mm tt");

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

                    customerId = bRow["FK_CustId"].ToString();
                    deviceType = bRow["DeviceType"] != DBNull.Value && bRow["DeviceType"] != null && bRow["DeviceType"].ToString() != "" ? bRow["DeviceType"].ToString() : "";

                    //orderStatus.SelectedValue = bRow["OrderStatus"].ToString();

                    using (DataTable dtCust = c.GetDataTable("Select CustomerName, CustomerMobile, CustomerEmail, CustomerAddress, CustomerCity, CustomerState, CustomerPincode From CustomersData Where CustomrtID = " + customerId))
                    {
                        if (dtCust.Rows.Count > 0)
                        {
                            DataRow row = dtCust.Rows[0];

                            int enqAssignStatus = Convert.ToInt32(c.GetReqData("SavingEnqAssign", "EnqAssignStatus", "FK_CalcID=" + Request.QueryString["id"] + " AND Fk_FranchID=" + Session["adminFranchisee"]));

                            //if (enqAssignStatus == 1 || enqAssignStatus == 5 || enqAssignStatus == 6 || enqAssignStatus == 7 || enqAssignStatus == 10)  // accepted
                            if (enqAssignStatus == 0 || enqAssignStatus == 2)
                            {
                                // cust name
                                if (row["CustomerName"] != DBNull.Value && row["CustomerName"] != null && row["CustomerName"].ToString() != "")
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
                                    ordCustData[0] = custName[0].ToString() + " " + maskedName.ToString();
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
                                    ordCustData[1] = maskedMob.ToString();
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
                                    ordCustData[2] = maskedEmail.ToString();
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
                        }
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

                    if (bRow["EnqStatus"].ToString() == "7")
                    {
                        chkDelivered.Checked = true;
                        chkDelivered.Enabled = false;
                        returnOrd.Visible = true;
                    }


                    // returned
                    if (bRow["EnqStatus"].ToString() == "10")
                    {
                        string retReason = c.GetReqData("SavingEnqAssign", "ReturnReason", "EnqAssignID=" + Request.QueryString["assignId"]).ToString();
                        txtRetReason.Text = retReason;
                        btnRet.Visible = false;
                        returnOrd.Visible = true;
                    }

                    if (bRow["TotalEnqAmount"] != DBNull.Value && bRow["TotalEnqAmount"] != null && bRow["TotalEnqAmount"].ToString() != "")
                    {
                        txtFinalAmount.Text = bRow["TotalEnqAmount"].ToString();
                    }

                }
            }

            using (DataTable dtProduct = c.GetDataTable("Select a.BrandMedicine, 'Rs. ' + Convert(varchar(20), a.BrandPrice)  as BrandPrice, 'Rs. ' + Convert(varchar(20), a.GenericPrice) as GenericPrice , a.GenericMedicine, a.GenericCode, a.SavingAmount, Convert(varchar(20), a.SavingPercent) + '%' as SavingPercent from SavingCalcItems a where a.FK_CalcID =" + Idx))
            {

                gvOrderDetails.DataSource = dtProduct;
                gvOrderDetails.DataBind();
            }

            orderCount = c.returnAggregate("Select COUNT(a.CalcID) From SavingCalc a Join CustomersData b On b.CustomrtID = a.FK_CustId Where b.CustomrtID=" + customerId + " AND b.delMark = 0").ToString();

        }
        catch (Exception ex)
        {

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetOrdersData", ex.Message.ToString());
            return;

        }
    }

    protected void btnAccept_Click(object sender, EventArgs e)
    {
        try
        {
            //***EnquiryAssign***
            // EnqAssignStatus 0 > Pending, 1 > Accepted, 2 > Rejected

            // set EnqAssignStatus=5 bcoz order is in inprocess mode while it is accepted
            c.ExecuteQuery("Update SavingEnqAssign Set EnqAssignStatus=5 Where FK_CalcID=" + Request.QueryString["id"] + " AND Fk_FranchID=" + Session["adminFranchisee"]);
            
            
            // update current enq's status as converted in main SavingCalc table 
            // update current enq's status as inprocess in main SavingCalc table 
            //c.ExecuteQuery("Update SavingCalc Set EnqStatus=3 Where CalcID=" + Request.QueryString["id"]);  //old
            c.ExecuteQuery("Update SavingCalc Set EnqStatus=5 Where CalcID=" + Request.QueryString["id"]); //new 17/09/2021 (on req from sujata mam, convert all enq code as orders)


            GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Enquiry accepted successfully');", true);

            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('enquiry-report.aspx', 2000);", true);
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
    //        c.ExecuteQuery("Update SavingEnqAssign Set EnqAssignStatus=2 Where FK_CalcID=" + Request.QueryString["id"] + " AND Fk_FranchID=" + Session["adminFranchisee"]);
    //        GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));
    //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Enquiry rejected');", true);
    //        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('enquiry-report.aspx', 2000);", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
    //        c.ErrorLogHandler(this.ToString(), "btnRejected_Click", ex.Message.ToString());
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

            // update dispatch details & set EnqStatus=6 as dispatched
            c.ExecuteQuery("Update SavingCalc Set OrderShipName='" + txtShipmentName.Text + "', OrderTrackingNo='" + txtTrackingNo.Text +
                "', EstimatedDeliveryDate='" + estimatedDate + "', EnqStatus=6 Where CalcID=" + Request.QueryString["id"]);

            // set OrderAssignStatus=6 as dispatched
            c.ExecuteQuery("Update SavingEnqAssign Set EnqAssignStatus=6 Where EnqReAssign=0 AND Fk_FranchID=" + Session["adminFranchisee"] + " AND FK_CalcID=" + Request.QueryString["id"]);

           
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Dispatch Details Added');", true);
            GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));


            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('enquiry-report.aspx', 2000);", true);
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
                // Update EnqStatus=7 as delivered & PayStaus=1 as paid in SavingCalc 
                c.ExecuteQuery("Update SavingCalc Set EnqStatus=7, OrderPayStatus=1 Where CalcID=" + Request.QueryString["id"]);

                // Update EnqAssignStatus=7 as delivered in SavingEnqAssign
                c.ExecuteQuery("Update SavingEnqAssign Set EnqAssignStatus=7 Where FK_CalcID=" + Request.QueryString["id"] + " AND Fk_FranchID=" + Session["adminFranchisee"]);

                

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Enquiry Marked as Delivered');", true);
                GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));


                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('enquiry-report.aspx', 2000);", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "chkDelivered_CheckedChanged", ex.Message.ToString());
            return;
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["type"] != null)
        {
            switch (Request.QueryString["type"])
            {
                case "new":
                    Response.Redirect("enquiry-report.aspx?type=new", false);
                    break;

                case "accepted":
                    Response.Redirect("enquiry-report.aspx?type=accepted", false);
                    break;

                case "rejected":
                    Response.Redirect("enquiry-report.aspx?type=rejected", false);
                    break;

                case "monthly":
                    Response.Redirect("enquiry-report.aspx?type=monthly", false);
                    break;

                case "dispatched":
                    Response.Redirect("enquiry-report.aspx?type=dispatched", false);
                    break;

                case "delivered":
                    Response.Redirect("enquiry-report.aspx?type=delivered", false);
                    break;
                case "returned":
                    Response.Redirect("enquiry-report.aspx?type=returned", false);
                    break;
            }
        }
        else
        {
            Response.Redirect("enquiry-report.aspx", false);
        }
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

            //mark its status EnqAssignStatus=10(OrdersAssign) & OrderStatus=10 (OrdersData) in both tables as "Returned"

            c.ExecuteQuery("Update SavingEnqAssign Set EnqAssignStatus=10, ReturnReason='" + txtRetReason.Text + "' Where FK_CalcID=" + Request.QueryString["id"] + " AND EnqAssignID=" + Request.QueryString["assignId"]);

            c.ExecuteQuery("Update SavingCalc Set EnqStatus=10 Where CalcID=" + Request.QueryString["id"]);

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Enquiry Marked as Returned');", true);

            GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnRet_Click", ex.Message.ToString());
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

            int frId = Convert.ToInt32(Session["adminFranchisee"].ToString());

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
            using (DataTable dtOrders = c.GetDataTable("Select a.FK_AddressID, a.FK_CustId, a.TotalEnqAmount, b.CustomerMobile, b.CustomerEmail, LEFT(b.CustomerName, 30) as CustomerName From SavingCalc a Inner Join CustomersData b On a.FK_CustId = b.CustomrtID Where a.CalcID=" + orderId + ""))
            {
                if (dtOrders.Rows.Count > 0)
                {
                    DataRow row = dtOrders.Rows[0];

                    addressId = row["FK_AddressID"].ToString();
                    customerId = row["FK_CustId"].ToString();
                    collectableAmount = Convert.ToDouble(row["TotalEnqAmount"].ToString());
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
            consigne.ConsigneeMobile = custMobile;
            consigne.ConsigneeName = custName;
            consigne.ConsigneePincode = custPincode;

            wayBillReq.Consignee = consigne;


            //Services

            Double actualWeight = Convert.ToDouble(txtWeight.Text);
            Double declaredValue = 100;
            string creditRef = "enqref" + orderId.ToString();

            Services serv = new Services();

            serv.ActualWeight = actualWeight;
            serv.CreditReferenceNo = creditRef;
            serv.DeclaredValue = declaredValue;

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
            string custCode = "060664";
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
                string fileName = "waybillenq-" + orderId + ".pdf";
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
                "', '" + statusCode1 + "', '" + statusInfo1 + "', '" + tokenNo + "', 2)");

                for (int i = 0; i < st.Length; i++)
                {
                    strError.Append("<br/>Code :" + st[i].StatusCode);
                    strError.Append("<br/>Info :" + st[i].StatusInformation);
                }

                strError.Append("<br/>" + "AWB No : " + awNo.ToString());
                strError.Append("<br/> <a href=\"" + Master.rootPath + "upload/awb_waybills/waybillenq-" + orderId + ".pdf\" target=\"_blank\">Download Waybill PDF File</a>");
                pickupReqStatus = c.ErrNotification(1, strError.ToString());

            }

            txtPickupDate.Text = txtPickupTime.Text = txtWeight.Text = txtWidth.Text = txtHeight.Text = txtBreadth.Text = "";
            ddrPay.SelectedIndex = 0;

            wayBill.Close();
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
    protected void btnUpdateAmount_Click(object sender, EventArgs e)
    {
        try
        {
            txtFinalAmount.Text = txtFinalAmount.Text.Trim().Replace("'", "");

            if (txtFinalAmount.Text == "")
            {
                GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Final Order Amount');", true);
                return;
            }

            if (!c.IsNumeric(txtFinalAmount.Text))
            {
                GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Amount must be numeric value');", true);
                return;
            }

            c.ExecuteQuery("Update SavingCalc Set TotalEnqAmount=" + Convert.ToDouble(txtFinalAmount.Text) + " Where CalcID=" + Request.QueryString["id"]);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Amount Updated');", true);
            GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnUpdateAmount_Click", ex.Message.ToString());
            return;
        }
    }
}