using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class genericmitra_order_details : System.Web.UI.Page
{
    iClass c = new iClass();
    public string customerId, errMsg, orderCount, shippingCharges, prescriptionStr, rdrUrl, mreq;
    public string deviceType;

    public string[] ordData = new string[20]; //18
    public string[] ordCustData = new string[10];
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["id"] != null)
            {
                
                GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));

                object mreFlag = c.GetReqData("OrdersData", "MreqFlag", "OrderID=" + Request.QueryString["id"]);
                if (mreFlag != DBNull.Value && mreFlag != null && mreFlag.ToString() != "")
                {
                    if (mreFlag.ToString() == "1")
                    {
                        mreq = "<span class=\"medium clrProcessing bold_weight\">Customer Marked This Order as Monthly Order</span><span class=\"space10\"></span>";
                    }
                }

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

                    ordData[2] = Convert.ToDouble(bRow["OrderAmount"]).ToString("0.00");
                    ordData[3] = bRow["OrderShipName"].ToString();

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
                        switch (Convert.ToInt32(bRow["OrderPayMode"]))
                        {
                            case 1:
                                ordData[12] = "Cash On Delivery";
                                ordData[13] = bRow["OrderPayStatus"].ToString() == "0" ? "UnPaid" : "Paid";
                                break;
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
}