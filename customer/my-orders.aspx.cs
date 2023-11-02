using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class customer_my_orders : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg, orderStr, completeOrderStr, myvariable;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                
                if (Request.QueryString["type"] != null && Request.QueryString["id"] != null)
                {
                   
                   
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "$(document).ready(function () {$('#cancelReason').popup('show');});", true);
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "showCancleReasons()", true);
                    //c.ExecuteQuery("Update OrdersData Set OrderStatus=2 Where OrderID=" + Request.QueryString["id"]);
                    //ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Order Canceled');", true);
                    //ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('" + Master.rootPath + "my-orders', 2000);", true);
                }

                GetOrders(1); // for ongoing orders
                GetOrders(2); // for completed orders
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    private void GetOrders(int ordStatus)
    {
        try
        {
            string strQuery = "";
            if (ordStatus == 1)
            {
                strQuery = "Select OrderID, OrderStatus From OrdersData Where FK_OrderCustomerID=" + Session["genericCust"];

            }
            else
            {
                strQuery = "Select OrderID, OrderStatus From OrdersData Where FK_OrderCustomerID=" + Session["genericCust"] + " AND OrderStatus=7";
            }

            using (DataTable dtOrders = c.GetDataTable(strQuery))
            {
                StringBuilder strMarkup = new StringBuilder();
                if (dtOrders.Rows.Count > 0)
                {
                    if (ordStatus == 1)
                    {
                        // check for pending (Upapproved by admin) Orders
                        if (c.IsRecordExist("Select OrderID From OrdersData Where FK_OrderCustomerID=" + Session["genericCust"] + " AND OrderStatus=1 Order By OrderID DESC"))
                        {
                            strMarkup.Append("<h3 class=\"clrLightBlack semiBold mrg_B_20\">Not Yet Approved</h3>");
                            using (DataTable dtNotApproved = c.GetDataTable("Select OrderID From OrdersData Where FK_OrderCustomerID=" + Session["genericCust"] + " AND OrderStatus=1 Order By OrderID DESC"))
                            {
                                if (dtNotApproved.Rows.Count > 0)
                                {
                                    foreach (DataRow notapprovedOrdRow in dtNotApproved.Rows)
                                    {
                                        strMarkup.Append(GetOrderMarkup(Convert.ToInt32(notapprovedOrdRow["OrderID"])));
                                    }
                                }
                            }
                        }

                        // check for approved by admin Orders
                        if (c.IsRecordExist("Select OrderID From OrdersData Where FK_OrderCustomerID=" + Session["genericCust"] + " AND OrderStatus IN (3, 5, 6) Order By OrderID DESC"))
                        {
                            strMarkup.Append("<h3 class=\"clrLightBlack semiBold mrg_B_20\">Approved</h3>");
                            using (DataTable dtApproved = c.GetDataTable("Select OrderID From OrdersData Where FK_OrderCustomerID=" + Session["genericCust"] + " AND OrderStatus IN (3, 5, 6) Order By OrderID DESC"))
                            {
                                if (dtApproved.Rows.Count > 0)
                                {
                                    foreach (DataRow approvedOrdRow in dtApproved.Rows)
                                    {
                                        strMarkup.Append(GetOrderMarkup(Convert.ToInt32(approvedOrdRow["OrderID"])));
                                    }
                                }
                            }
                        }

                        // check for denied by admin Orders
                        if (c.IsRecordExist("Select OrderID From OrdersData Where FK_OrderCustomerID=" + Session["genericCust"] + " AND OrderStatus=4 Order By OrderID DESC"))
                        {
                            strMarkup.Append("<h3 class=\"clrLightBlack semiBold mrg_B_20\">Denied</h3>");
                            using (DataTable dtApproved = c.GetDataTable("Select OrderID From OrdersData Where FK_OrderCustomerID=" + Session["genericCust"] + " AND OrderStatus=4 Order By OrderID DESC"))
                            {
                                if (dtApproved.Rows.Count > 0)
                                {
                                    foreach (DataRow approvedOrdRow in dtApproved.Rows)
                                    {
                                        strMarkup.Append(GetOrderMarkup(Convert.ToInt32(approvedOrdRow["OrderID"])));
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // delivered orders
                        foreach (DataRow row in dtOrders.Rows)
                        {
                            strMarkup.Append(GetOrderMarkup(Convert.ToInt32(row["OrderID"])));
                        }
                    }
                    if (ordStatus == 1)
                    {
                        orderStr = strMarkup.ToString();
                    }
                    else
                    {
                        completeOrderStr = strMarkup.ToString();
                    }
                }
                else
                {
                    if (ordStatus == 1)
                    {
                        orderStr = "<div class=\"themeBgPrime\"><div class=\"pad_10\"><span class=\"clrWhite fontRegular\">No any ongoing orders to display</span></div></div>";
                        return;
                    }
                    else
                    {
                        completeOrderStr = "<div class=\"themeBgPrime\"><div class=\"pad_10\"><span class=\"clrWhite fontRegular\">No any completed orders to display</span></div></div>";
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (ordStatus == 1)
            {
                orderStr = c.ErrNotification(3, ex.Message.ToString());
                return;
            }
            else
            {
                completeOrderStr = c.ErrNotification(3, ex.Message.ToString());
                return;
            }
        }
    }

    private string GetOrderMarkup(int ordIdX)
    {
        try
        {
            using (DataTable dtOrderInfo = c.GetDataTable("Select OrderID, OrderDate, OrderAmount, OrderStatus From OrdersData Where OrderID=" + ordIdX))
            {
                if (dtOrderInfo.Rows.Count > 0)
                {
                    DataRow row = dtOrderInfo.Rows[0];

                    StringBuilder strMarkup = new StringBuilder();

                    strMarkup.Append("<div class=\"bgWhite border_r_5 box-shadow posRelative\">");
                    strMarkup.Append("<div class=\"pad_15\">");
                    strMarkup.Append("<div class=\"absCircle\">");

                    string classname = "", orderStatus = "", color = "";
                    switch (Convert.ToInt32(row["OrderStatus"]))
                    {
                        case 1: classname = "blueCircle"; orderStatus = "Pending"; color = "#0420fb"; break;
                        case 3: classname = "pinkCircle"; orderStatus = "Approved"; color = "#ff998b"; break;
                        case 4: classname = "redCircle"; orderStatus = "Denied"; color = "#ff0000"; break;
                        case 5: classname = "yellowCircle"; orderStatus = "In Process"; color = "#f6c863"; break;
                        case 6: classname = "purpleCircle"; orderStatus = "Shipped"; color = "#6835e8"; break;
                        case 7: classname = "greenCircle"; orderStatus = "Delivered"; color = "#42cb63"; break;
                    }

                    strMarkup.Append("<div class=\"" + classname + "\"></div>");
                    strMarkup.Append("<div class=\"float_clear\"></div>");
                    strMarkup.Append("<span class=\"tiny semiBold clrDarkGrey\" style=\"color:" + color + "\">" + orderStatus + "</span>");
                    strMarkup.Append("</div>");
                    string orderUrl = Master.rootPath + "order-details?orderId=" + row["OrderID"].ToString();
                    strMarkup.Append("<a href=\"" + orderUrl + "\" class=\"orderAnch semiBold\">Request Number : #" + row["OrderID"].ToString() + "</a>");
                    strMarkup.Append("<span class=\"lineSeperator\"></span>");
                    strMarkup.Append("<div class=\"float_left width70\">");
                    strMarkup.Append("<span class=\"clrGrey fontRegular tiny dispBlk mrg_B_3\">Request Date : " + Convert.ToDateTime(row["OrderDate"]).ToString("dd/MM/yyyy") + "</span>");
                    string products = "";
                    using (DataTable dtProds = c.GetDataTable("Select a.ProductID, a.ProductName From ProductsData a Inner Join OrdersDetails b On a.ProductID=b.FK_DetailProductID Where b.FK_DetailOrderID=" + ordIdX))
                    {
                        if (dtProds.Rows.Count > 0)
                        {
                            foreach (DataRow prodRow in dtProds.Rows)
                            {
                                if (products == "")
                                    products = prodRow["ProductName"].ToString();
                                else
                                    products = products + ", " + prodRow["ProductName"].ToString();
                            }

                            if (c.IsRecordExist("Select PrescriptionID From OrderPrescriptions Where FK_OrderID=" + ordIdX))
                            {
                                products = products + ", Prescription";
                            }
                        }
                        else
                        {
                            if (c.IsRecordExist("Select PrescriptionID From OrderPrescriptions Where FK_OrderID=" + ordIdX))
                            {
                                products = "Prescription";
                            }
                        }
                    }
                    
                    strMarkup.Append("<span class=\"clrGrey fontRegular tiny dispBlk\">contains " + products + "</span>");
                    strMarkup.Append("</div>");
                    strMarkup.Append("<div class=\"float_right\">");
                    strMarkup.Append("<span class=\"tiny clrGrey fontRegular\">Amt. </span>");
                    strMarkup.Append("<span class=\"regular clrLightBlack semiBold\">&#8377; " + row["OrderAmount"].ToString() + "</span>");
                    strMarkup.Append("</div>");
                    strMarkup.Append("<div class=\"float_clear\"></div>");
                    strMarkup.Append("<span class=\"space20\"></span>");
                    string rPath = c.ReturnHttp();
                    switch (orderStatus)
                    {   
                        case "Pending":
                            strMarkup.Append("<a href=\"" + rPath + "contact-us\" class=\"blueAnch dspInlineBlk semiBold small mrg_R_5\">Contact Us</a>");
                            strMarkup.Append("<a href=\"" + Master.rootPath + "cancel-request-reason.aspx?id=" + ordIdX + "\" class=\"pinkAnch dspInlineBlk semiBold small cancel-links\" data-fancybox-type=\"iframe\">Cancel Request</a>");
                            break;

                        case "Approved":
                            strMarkup.Append("<a href=\"" + orderUrl + "\" class=\"blueAnch dspInlineBlk semiBold small mrg_R_5\">Track Request</a>");
                            object favShop = c.GetReqData("CustomersData", "CustomerFavShop", "CustomrtID=" + Session["genericCust"]);
                            if (favShop != DBNull.Value && favShop != null && favShop.ToString() != "" && favShop.ToString() != "0")
                            {
                                //if fav shop order, then allow cust to cancel order before fav shop accepts it
                                int ordAssignStataus = Convert.ToInt32(c.GetReqData("OrdersAssign", "OrdAssignStatus", "FK_OrderID=" + ordIdX + " AND OrdReAssign=0"));
                                if (ordAssignStataus == 0)
                                {
                                    strMarkup.Append("<a href=\"" + Master.rootPath + "cancel-request-reason.aspx?id=" + ordIdX + "\" class=\"pinkAnch dspInlineBlk semiBold small cancel-links\" data-fancybox-type=\"iframe\">Cancel Request</a>");
                                }
                            }
                            break;

                        case "Denied":
                            strMarkup.Append("<a href=\"" + rPath + "contact-us\" class=\"blueAnch dspInlineBlk semiBold small mrg_R_5\">Contact Us</a>");
                            break;

                        case "In Process":
                            strMarkup.Append("<a href=\"" + orderUrl + "\" class=\"blueAnch dspInlineBlk semiBold small mrg_R_5\">Track Request</a>");
                            strMarkup.Append("<a href=\"" + rPath + "contact-us\" class=\"pinkAnch dspInlineBlk semiBold small mrg_R_5\">Contact Us</a>");
                            break;

                        case "Shipped":
                            strMarkup.Append("<a href=\"" + orderUrl + "\" class=\"blueAnch dspInlineBlk semiBold small mrg_R_5\">Track Request</a>");
                            strMarkup.Append("<a href=\"" + rPath + "contact-us\" class=\"pinkAnch dspInlineBlk semiBold small mrg_R_5\">Contact Us</a>");
                            break;

                        case "Delivered":
                            strMarkup.Append("<a href=\"" + orderUrl + "\" class=\"blueAnch dspInlineBlk semiBold small mrg_R_5\">View Request</a>");
                            break;
                    }

                    int ordStatus = Convert.ToInt32(c.GetReqData("OrdersData", "OrderStatus", "OrderID=" + ordIdX));
                    if (ordStatus == 7)
                    {
                        strMarkup.Append("<span class=\"space10\"></span>");
                        string encryptOrderId = c.EnryptString(ordIdX.ToString());
                        strMarkup.Append("<a href=\"request-qc-report?order=" + encryptOrderId + "\" class=\"pinkAnch dspInlineBlk semiBold small mrg_R_5\">Request QC Report</a>");
                    }
                    
                    strMarkup.Append("</div>");
                    strMarkup.Append("</div>");
                    strMarkup.Append("<span class=\"space30\"></span>");

                    return strMarkup.ToString();
                }
                else
                {
                    return "";
                }
            }
        }
        catch (Exception ex)
        {
            return c.ErrNotification(3, ex.Message.ToString());
        }
    }

    //protected void btnSubmit_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Error Occoured While Processing');", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
    //        c.ErrorLogHandler(this.ToString(), "btnSubmit_Click", ex.Message.ToString());
    //        return;
    //    }
    //}
  
  
}