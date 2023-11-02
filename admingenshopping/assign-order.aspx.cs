using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class admingenshopping_assign_order : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] ordData = new string[20]; //7
    public string shippingCharges, modal, rdrUrl;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //cmdAssign.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(cmdAssign, null) + ";");
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
                        rdrUrl = "order-reports.aspx";
                    }

                    
                    

                    GetOrderInfo(Convert.ToInt32(Request.QueryString["id"]));

                    int custId = Convert.ToInt32(c.GetReqData("OrdersData", "FK_OrderCustomerID", "OrderID=" + Request.QueryString["id"]));
                    object custFavShop = c.GetReqData("CustomersData", "CustomerFavShop", "CustomrtID=" + custId);
                    if (custFavShop != DBNull.Value && custFavShop != null && custFavShop.ToString() != "" && custFavShop.ToString() != "0")
                    {
                        // it is customers fav shop order, hide shops list to assign order until it is rejected by fav shop
                        shopList.Visible = false;
                        if (c.IsRecordExist("Select OrdAssignID From OrdersAssign Where FK_OrderID=" + Request.QueryString["id"] + " AND OrdAssignStatus IN (2) AND Fk_FranchID=" + custFavShop.ToString()))
                        {
                            shopList.Visible = true;
                        }
                        else
                        {

                        }
                    }

                    // OrderAssignStatus 0 > Pending, 1 > Accepted, 2 > Rejected, 5 > In Process, 6 > Shipped, 7 > Delivered
                    //(3,4 not considered, bcoz to match flags 5,6,7 with main OrdersData table)
                    if (c.IsRecordExist("Select OrdAssignID From OrdersAssign Where FK_OrderID=" + Request.QueryString["id"] + " AND OrdAssignStatus IN (1,5,6,7)"))
                    {
                        shopList.Visible = false;
                    }
                    else
                    {
                        shopList.Visible = true;
                    }

                    if (Request.QueryString["type"] != null && Request.QueryString["assId"] != null)
                    {
                        //update assign status = 0
                        c.ExecuteQuery("Update OrdersAssign Set OrdAssignStatus=0, FK_ReasonID=0 Where FK_OrderID=" + Request.QueryString["id"] + " AND OrdAssignID=" + Request.QueryString["assId"]);
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Activity Deleted');", true);
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('assign-order.aspx?id=" + Request.QueryString["id"] + "', 2000);", true);
                    }

                    // admin delete activity
                    if (Request.QueryString["activity"] != null && Request.QueryString["assignId"] != null)
                    {
                        //delete entry from assign table and make it as new order accepted by admin
                        c.ExecuteQuery("Delete From OrderTracking Where FK_OrdAssignID=" + Request.QueryString["assignId"]);
                        c.ExecuteQuery("Delete From OrdersAssign Where OrdAssignID=" + Request.QueryString["assignId"]);
                        c.ExecuteQuery("Update OrdersData Set OrderStatus=3 Where OrderID=" + Request.QueryString["id"]);
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Activity Deleted');", true);
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('assign-order.aspx?id=" + Request.QueryString["id"] + "', 2000);", true);
                    }

                    if (Request.QueryString["type"] != null)
                    {
                        if (Request.QueryString["type"] == "reject")
                        {
                            shopList.Visible = false;
                        }
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

    private void GetOrderInfo(int orderIdX)
    {
        try
        {
            using (DataTable dtOrder = c.GetDataTable("Select * From OrdersData Where OrderID=" + orderIdX))
            {
                if (dtOrder.Rows.Count > 0)
                {
                    DataRow bRow = dtOrder.Rows[0];
                    ordData[0] = "#" + orderIdX.ToString();

                    if (bRow["OrderType"] != DBNull.Value && bRow["OrderType"] != null && bRow["OrderType"].ToString() != "")
                    {
                        if (bRow["OrderType"].ToString() == "2")
                        {
                            modal = "";
                        }
                        else
                        {
                            modal = "modal";
                        }
                    }
                    else
                    {
                        modal = "modal";
                    }


                    ordData[1] = Convert.ToDateTime(bRow["OrderDate"]).ToString("dd/MM/yyyy hh:mm tt");
                    //ordData[2] = bRow["OrderCity"].ToString();
                    //ordData[3] = bRow["OrderZipCode"].ToString();
                    if (bRow["FK_AddressId"] != DBNull.Value && bRow["FK_AddressId"] != null && bRow["FK_AddressId"].ToString() != "")
                    {
                        using (DataTable dtCustAddr = c.GetDataTable("Select AddressID, AddressFKCustomerID, AddressName, AddressFull, AddressCity, AddressState, AddressPincode, AddressCountry From CustomersAddress Where AddressID=" + bRow["FK_AddressId"]))
                        {
                            if (dtCustAddr.Rows.Count > 0)
                            {
                                DataRow row = dtCustAddr.Rows[0];

                                ordData[2] = row["AddressCity"].ToString();
                                ordData[3] = row["AddressPincode"].ToString();
                            }
                        }
                    }
                    else
                    {
                        ordData[2] = bRow["OrderCity"].ToString();
                        ordData[3] = bRow["OrderZipCode"].ToString();
                    }
                    ordData[4] = bRow["OrderContactInfo"].ToString();
                    ordData[5] = Convert.ToDouble(bRow["OrderAmount"]).ToString("0.00");
                    //ordData[] = bRow["OrderShipAddress"] != DBNull.Value && bRow["OrderShipAddress"] != null && bRow["OrderShipAddress"].ToString() != "" ? bRow["OrderShipAddress"].ToString() : "-";
                    //ordData[] = bRow["OrderShipAddress2"] != DBNull.Value && bRow["OrderShipAddress2"] != null && bRow["OrderShipAddress2"].ToString() != "" ? bRow["OrderShipAddress2"].ToString() : "-";
                    
                    //ordData[] = bRow["OrderState"].ToString();
                    
                    //ordData[] = bRow["OrderCountry"].ToString();
                    


                    //if (bRow["OrderPayMode"] != DBNull.Value && bRow["OrderPayMode"] != null && bRow["OrderPayMode"].ToString() != "")
                    //{
                    //    switch (Convert.ToInt32(bRow["OrderPayMode"]))
                    //    {
                    //        case 1:
                    //            ordData[] = "Cash On Delivery";
                    //            ordData[] = bRow["OrderPayStatus"].ToString() == "0" ? "UnPaid" : "Paid";
                    //            break;
                    //    }
                    //}

                    //ordData[] = bRow["OrderNote"] != DBNull.Value && bRow["OrderNote"] != null && bRow["OrderNote"].ToString() != "" ? bRow["OrderNote"].ToString() : "-";

                    ordData[6] = c.returnAggregate("Select SUM(OrdDetailAmount) From OrdersDetails Where FK_DetailOrderID=" + orderIdX).ToString("0.00");

                    if (Convert.ToDouble(ordData[6].ToString()) > 500)
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
                            else
                            {
                                shippingCharges = "Shipping Charges = &#8377; 30.00 <br/>";
                            }
                        }
                        else
                        {
                            shippingCharges = "Shipping Charges = &#8377; 30.00 <br/>";
                        }
                    }


                    // Order details grid view
                    using (DataTable dtProduct = c.GetDataTable("Select a.OrdDetailQTY, 'Rs. ' + Convert(varchar(20), a.OrdDetailPrice)  as OrigPrice, 'Rs. ' + Convert(varchar(20), a.OrdDetailAmount) as OrdAmount , a.OrdDetailSKU, b.ProductName from OrdersDetails a Inner Join ProductsData b on a.FK_DetailProductID = b.ProductID where a.FK_DetailOrderID =" + orderIdX))
                    {

                        gvOrderDetails.DataSource = dtProduct;
                        gvOrderDetails.DataBind();
                    }

                    // Order assignment details
                    using (DataTable dtOrdAssign = c.GetDataTable("Select a.OrdAssignID, a.OrdAssignDate, a.FK_OrderID, a.FK_ReasonID, a.Fk_FranchID, " + 
                        " a.OrdAssignStatus, a.OrdReAssign, c.FranchName, c.FranchShopCode From OrdersAssign a Inner Join OrdersData b " +
                        " On a.FK_OrderID=b.OrderID Inner Join FranchiseeData c On a.Fk_FranchID=c.FranchID Where a.FK_OrderID=" + orderIdX))
                    {
                        if (dtOrdAssign.Rows.Count > 0)
                        {
                            StringBuilder strMarkup = new StringBuilder();

                            strMarkup.Append("<div class=\"card-header\">");
                            strMarkup.Append("<h3 class=\"medium colorLightBlue\">Order Assigning Details</h3>");
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
                                string ordAssignStatus = "", classname = "";
                                switch (Convert.ToInt32(aRow["OrdAssignStatus"]))
                                {
                                    case 0: ordAssignStatus = "Pending"; classname = "clrPending"; break;
                                    case 1: ordAssignStatus = "Accepted"; classname = "clrAccepted"; break;
                                    case 2: ordAssignStatus = "Rejected"; classname = "clrRejected"; break;
                                    case 5: ordAssignStatus = "In Process"; classname = "clrProcessing"; break;
                                    case 6: ordAssignStatus = "Shipped"; classname = "clrShipped"; break;
                                    case 7: ordAssignStatus = "Delivered"; classname = "clrDelivered"; break;
                                }
                                string actLink = "";
                                if (aRow["OrdAssignStatus"].ToString() == "2" && aRow["OrdReAssign"].ToString() == "0")
                                {
                                    actLink = "<br/><a href=\"assign-order.aspx?id=" + Request.QueryString["id"] + "&type=delAct&assId=" + aRow["OrdAssignID"] + "\" class=\"clrShipped\">Delete Activity</a>";
                                }
                                strMarkup.Append("<td class=\"" + classname + "\">" + ordAssignStatus.ToString() + actLink + "</td>");
                                if (aRow["FK_ReasonID"]!=DBNull.Value &&aRow["FK_ReasonID"]!=null || aRow["FK_ReasonID"].ToString() != "")
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
                                    strMarkup.Append("<td><a href=\"assign-order.aspx?id=" + Request.QueryString["id"] + "&activity=adminDel&assignId=" + aRow["OrdAssignID"] + "\" class=\"deleteProd\" OnClick=\"return confirm('Are you sure you want to delete this?');\"></a></td>");
                                }
                                strMarkup.Append("</tr>");
                            }

                            strMarkup.Append("</table>");
                            strMarkup.Append("</div>");

                            ordData[7] = strMarkup.ToString();
                        }
                    }

                    // Fill Shop List
                    FillShopGrid();
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

    private void FillShopGrid()
    {
        try
        {
            object addrID = c.GetReqData("OrdersData", "FK_AddressId", "OrderID=" + Request.QueryString["id"]);
            string zipcode = "", strQuery = "";
            if (addrID != DBNull.Value && addrID != null && addrID.ToString() != "")
            {
                int addressId = Convert.ToInt32(addrID);
                zipcode = c.GetReqData("CustomersAddress", "AddressPincode", "AddressID=" + addressId).ToString();
            }
            else
            {
                zipcode = c.GetReqData("OrdersData", "OrderZipCode", "OrderID=" + Request.QueryString["id"]).ToString();
            }

            if (rdbRelevant.Checked == true)
            {
                strQuery = "Select FranchID, FranchName, FranchShopCode, FranchMobile From FranchiseeData Where FranchPinCode='" + zipcode + "' AND FranchActive=1 AND SUBSTRING(FranchShopCode,1,4)<>'GMDR' AND SUBSTRING(FranchShopCode,1,4)<>'GMOS'";
            }
            else
            {
                strQuery = "Select FranchID, FranchName, FranchShopCode, FranchMobile From FranchiseeData Where FranchActive=1 AND SUBSTRING(FranchShopCode,1,4)<>'GMDR' AND SUBSTRING(FranchShopCode,1,4)<>'GMOS'";
            }

            //using (DataTable dtShops = c.GetDataTable("Select FranchID, FranchName, FranchShopCode, FranchMobile From FranchiseeData Where FranchPinCode='" + zipcode + "' AND FranchActive=1"))
            using (DataTable dtShops = c.GetDataTable(strQuery))
            {
                gvShopList.DataSource = dtShops;
                gvShopList.DataBind();
                if (gvShopList.Rows.Count > 0)
                {
                    gvShopList.UseAccessibleHeader = true;
                    gvShopList.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FillShopGrid", ex.Message.ToString());
            return;
        }
    }

    protected void gvShopList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Button btnAssign = (Button)e.Row.FindControl("cmdAssign");
                //if (c.IsRecordExist("Select OrdAssignID From OrdersAssign Where FK_OrderID=" + Request.QueryString["id"] + " AND Fk_FranchID=" + e.Row.Cells[0].Text))
                //{
                //    btnAssign.Enabled = false;
                //    btnAssign.Attributes["style"] = "background:none; display:none;";
                //}

                Literal litTotalOrd = (Literal)e.Row.FindControl("litTotalOrd");
                litTotalOrd.Text = c.returnAggregate("Select Count(OrdAssignID) From OrdersAssign Where Fk_FranchID=" + e.Row.Cells[0].Text + " AND OrdReAssign=0 AND OrdAssignStatus IN (1, 5, 6, 7)").ToString();

                Literal litLastOrdDate = (Literal)e.Row.FindControl("litLastOrdDate");
                object lastOrderDate = c.GetReqData("OrdersAssign", "Top 1 OrdAssignDate", "Fk_FranchID=" + e.Row.Cells[0].Text + " AND OrdReAssign=0 AND OrdAssignStatus IN (1, 5, 6, 7) Order By OrdAssignID DESC");
                if (lastOrderDate != DBNull.Value && lastOrderDate != null && lastOrderDate.ToString() != "")
                {
                    litLastOrdDate.Text = Convert.ToDateTime(lastOrderDate).ToString("dd/MM/yyyy hh:mm tt");
                }
                else
                {
                    litLastOrdDate.Text = "Not Found";
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvShopList_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    protected void gvShopList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridViewRow gRow = (GridViewRow)((Button)e.CommandSource).NamingContainer;
            if (e.CommandName == "gvAssign")
            {
                // OrderAssignStatus 0 > Pending, 1 > Accepted, 2 > Rejected
                // OrderAssignStatus 0 > Pending, 1 > Accepted, 2 > Rejected, 5 > In Process, 6 > Shipped, 7 > Delivered
                //(3,4 not considered, to match flags 5,6,7 with main OrdersData table)

                // check if order is already assigned & it is rejected
                //if (c.IsRecordExist("Select OrdAssignID From OrdersAssign Where FK_OrderID=" + Request.QueryString["id"]))
                //{
                //    //int ordAssignID = Convert.ToInt32(c.GetReqData("OrdersAssign", "TOP 1 OrdAssignID", "FK_OrderID=" + Request.QueryString["id"] + " Order By OrdAssignID DESC"));
                //    //int ordAssignStatus = Convert.ToInt32(c.GetReqData("OrdersAssign", "OrdAssignStatus", "OrdAssignID=" + ordAssignID));
                //    //if (ordAssignStatus == 2 || ordAssignStatus == 0) // rejected or pending
                //    //{
                //    //    c.ExecuteQuery("Update OrdersAssign Set OrdReAssign=1 Where OrdAssignID=" + ordAssignID);
                //    //    // insert new assign entry
                //    //    int maxId = c.NextId("OrdersAssign", "OrdAssignID");
                //    //    c.ExecuteQuery("Insert Into OrdersAssign (OrdAssignID, OrdAssignDate, FK_OrderID, Fk_FranchID, OrdAssignStatus, " +
                //    //        " OrdReAssign) Values (" + maxId + ", '" + DateTime.Now + "', " + Request.QueryString["id"] + ", " + gRow.Cells[0].Text + ", 0, 0)");

                //    //}
                //}
                //else
                //{
                //    //int maxId = c.NextId("OrdersAssign", "OrdAssignID");
                //    //c.ExecuteQuery("Insert Into OrdersAssign (OrdAssignID, OrdAssignDate, FK_OrderID, Fk_FranchID, OrdAssignStatus, " +
                //    //    " OrdReAssign) Values (" + maxId + ", '" + DateTime.Now + "', " + Request.QueryString["id"] + ", " + gRow.Cells[0].Text + ", 0, 0)");

                //}


                //if (!c.IsRecordExist("Select OrdAssignID From OrdersAssign Where FK_OrderID=" + Request.QueryString["id"] + " AND Fk_FranchID=" + gRow.Cells[0].Text + " AND  OrdAssignStatus=0")) 
                //{
                    c.ExecuteQuery("Update OrdersAssign Set OrdReAssign=1 Where FK_OrderID=" + Request.QueryString["id"]);
                    int maxId = c.NextId("OrdersAssign", "OrdAssignID");
                    c.ExecuteQuery("Insert Into OrdersAssign (OrdAssignID, OrdAssignDate, FK_OrderID, Fk_FranchID, OrdAssignStatus, " +
                        " OrdReAssign, AssignedFrom) Values (" + maxId + ", '" + DateTime.Now + "', " + Request.QueryString["id"] + ", " + gRow.Cells[0].Text + ", 0, 0, 'Admin')");

                    string frName = gRow.Cells[2].Text.ToString();
                    string mobNo = gRow.Cells[3].Text.ToString();
                    string pendingOrdCount = c.returnAggregate("Select Count(OrdAssignID) From OrdersAssign Where OrdAssignStatus=0 AND OrdReAssign=0 AND Fk_FranchID=" + gRow.Cells[0].Text).ToString();
                    string msgData = "Dear " + frName + ", You have received new order Order No " + Request.QueryString["id"] + " from Genericart Mobile App.Total Pending Order is/are " + pendingOrdCount + " Thank you Genericart Medicine Store Wahi Kaam, Sahi Daam";
                    c.SendSMS(msgData, mobNo);
                    //c.SendSMS(msgData, "8408027474");
                //}
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Order AssignedSuccessfully..!!');", true);
                GetOrderInfo(Convert.ToInt32(Request.QueryString["id"]));
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('assign-order.aspx?id=" + Request.QueryString["id"] + "', 2000);", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvShopList_RowCommand", ex.Message.ToString());
            return;
        }
    }
    protected void rdbRelevant_CheckedChanged(object sender, EventArgs e)
    {
        FillShopGrid();
        GetOrderInfo(Convert.ToInt32(Request.QueryString["id"]));
    }
    protected void rdbAll_CheckedChanged(object sender, EventArgs e)
    {
        FillShopGrid();
        GetOrderInfo(Convert.ToInt32(Request.QueryString["id"]));
    }
}