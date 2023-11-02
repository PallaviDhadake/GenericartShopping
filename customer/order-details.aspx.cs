using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Web.Services;

public partial class customer_order_details : System.Web.UI.Page
{
    iClass c = new iClass();
    public string ordStr, starRating;

    public static string orderId { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["orderId"] != null)
                {
                    GetOrderDetails(Convert.ToInt32(Request.QueryString["orderId"]));
                    orderId = Request.QueryString["orderId"].ToString();
                    object rating = c.GetReqData("OrdersData", "OrderRating", "OrderID=" + Request.QueryString["orderId"]);
                    if (rating != DBNull.Value && rating != null && rating.ToString() != "")
                    {
                        starRating = rating.ToString();
                        txtRating.Text = starRating.ToString();
                    }

                    if (Request.QueryString["type"] != null && Request.QueryString["prId"] != null)
                    {
                        if (c.IsRecordExist("Select PrescriptionID From OrderPrescriptions Where FK_OrderID=" + Request.QueryString["orderId"] + " AND PrescriptionID=" + Request.QueryString["prId"] + " AND PrescriptionStatus=2"))
                        {
                            // delete prescription
                            c.ExecuteQuery("Delete From OrderPrescriptions Where FK_OrderID=" + Request.QueryString["orderId"] + " AND PrescriptionID=" + Request.QueryString["prId"]);
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Prescription Deleted');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Something Went Wrong');", true);
                        }
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('order-details?orderId=" + Request.QueryString["orderId"] + "', 2000);", true);
                    }
                }
                else
                {
                    Response.Redirect(Master.rootPath + "my-orders", false);
                }
            }
        }
        catch (Exception ex)
        {
            ordStr = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    private void GetOrderDetails(int ordIdX)
    {
        try
        {
            using (DataTable dtOrderInfo = c.GetDataTable("Select OrderID, OrderDate, OrderAmount, OrderStatus, OrderShipAddress, FK_AddressId From OrdersData Where OrderID=" + ordIdX))
            {
                if (dtOrderInfo.Rows.Count > 0)
                {
                    DataRow row = dtOrderInfo.Rows[0];

                    StringBuilder strMarkup = new StringBuilder();

                    strMarkup.Append("<h2 class=\"clrLightBlack semiBold\">Request Number : #" + row["OrderID"].ToString() + "</h2>");
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

                    if (c.IsRecordExist("Select PrescriptionID From OrderPrescriptions Where FK_OrderID=" + ordIdX))
                    {
                        strMarkup.Append("<h3 class=\"clrLightBlack semiBold\">Your Prescription :</h3>");
                        using (DataTable dtRx = c.GetDataTable("Select PrescriptionID, PrescriptionName, PrescriptionStatus From OrderPrescriptions Where FK_OrderID=" + ordIdX))
                        {
                            if (dtRx.Rows.Count > 0)
                            {
                                int bCount = 0;
                                string rPath = c.ReturnHttp();
                                foreach (DataRow prow in dtRx.Rows)
                                {
                                    strMarkup.Append("<div class=\"imgBox\">");
                                    strMarkup.Append("<div class=\"pad-5\">");
                                    strMarkup.Append("<div class=\"border1 posRelative\">");
                                    if (prow["PrescriptionStatus"].ToString() == "2")
                                    {
                                        strMarkup.Append("<div class=\"absRejected fontRegular\">Rejected</div>");
                                        strMarkup.Append("<a href=\"" + Master.rootPath + "order-details?orderId=" + Request.QueryString["orderId"] + "&type=delete&prId=" + prow["PrescriptionID"] + "\" title=\"Delete Prescription\"  class=\"closeAnch\"></a>");
                                    }
                                    if (prow["PrescriptionStatus"].ToString() == "1")
                                    {
                                        strMarkup.Append("<div class=\"absAccepted fontRegular\">Accepted</div>");
                                    }
                                    if (prow["PrescriptionStatus"].ToString() == "0")
                                    {
                                        strMarkup.Append("<div class=\"absPending fontRegular\">Pending for Approval</div>");
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
                                    //strMarkup.Append("<a href=\"" + rPath + "upload/prescriptions/" + prow["PrescriptionName"] + "\" data-fancybox=\"rxGroup\"><img src=\"" + rPath + "upload/prescriptions/" + prow["PrescriptionName"] + "\" class=\"width100\" /></a>");
                                    strMarkup.Append("</div>");
                                    strMarkup.Append("</div>");
                                    strMarkup.Append("</div>");
                                    strMarkup.Append("</div>");
                                    strMarkup.Append("</div>");

                                    bCount++;

                                    if ((bCount % 2) == 0)
                                    {
                                        strMarkup.Append("<div class=\"float_clear\"></div>");
                                    }
                                }
                                strMarkup.Append("<div class=\"float_clear\"></div>");
                                if (c.IsRecordExist("Select PrescriptionID From OrderPrescriptions Where FK_OrderID=" + ordIdX + " AND PrescriptionStatus=2"))
                                {
                                    strMarkup.Append("<span class=\"space30\"></span>");
                                    strMarkup.Append("<a href=\"" + Master.rootPath + "upload-prescription?orderId=" + Request.QueryString["orderId"] + "\" class=\"blueAnch dspInlineBlock small fontRegular\">Upload Prescription</a>");
                                }
                            }
                        }
                        strMarkup.Append("<span class=\"space20\"></span>");
                    }

                    // timeline
                    strMarkup.Append("<ul class=\"timeline\">");
                    using (DataTable dtOrdTrack = c.GetDataTable("Select OrdTracID, OrdTracDate, FK_OrdAssignID, OrdTracType, OrdTracMessage From OrderTracking Where FK_OrderID=" + ordIdX))
                    {
                        if (dtOrdTrack.Rows.Count > 0)
                        {
                            string actClass = "active";
                            string trackType = "";
                            foreach (DataRow ordTrackRow in dtOrdTrack.Rows)
                            {
                                //***OrderTracking***
                                // OrdTeacType 1 > Confirmed, 2 > Inprocess, 3 > Shipped, 4 > Delivered
                                switch (Convert.ToInt32(ordTrackRow["OrdTracType"].ToString()))
                                {
                                    case 1: trackType = "Confirmed"; break;
                                    case 2: trackType = "Inprocess"; break;
                                    case 3: trackType = "Out For Delivery"; break;
                                    case 4: trackType = "Your request has been delivered"; break;
                                }
                                strMarkup.Append("<li class=\"" + actClass + "\">");
                                strMarkup.Append("<span>" + Convert.ToDateTime(ordTrackRow["OrdTracDate"]).ToString("dd MMM, yyyy") + ", <br/><span class=\"themeClrPrime\">" + trackType + "</span></span>");
                                string[] arrMsg = ordTrackRow["OrdTracMessage"].ToString().Split('#');
                                string finalMsg = "";
                                foreach (string msg in arrMsg)
                                {
                                    if (finalMsg == "")
                                        finalMsg = msg;
                                    else
                                        finalMsg = finalMsg + "<br/>" + msg;
                                }
                                //strMarkup.Append("<p class=\"fontRegular clrGrey\">" + ordTrackRow["OrdTracMessage"].ToString() + "</p>");
                                strMarkup.Append("<p class=\"fontRegular clrGrey\">" + finalMsg.ToString().Replace("order", "request") + "</p>");
                                strMarkup.Append("</li>");
                            }
                        }
                    }
                    strMarkup.Append("</ul>");
                    strMarkup.Append("<span class=\"lineSeperator\"></span>");

                    // delivery address
                    
                    if (row["FK_AddressId"] != DBNull.Value && row["FK_AddressId"] != null && row["FK_AddressId"].ToString() != "")
                    {
                        strMarkup.Append("<h3 class=\"clrLightBlack semiBold mrg_B_10 regular\">Delivery Address</h3>");
                        strMarkup.Append("<div class=\"float_left width70\">");
                        string addrName = c.GetReqData("CustomersAddress", "AddressName", "AddressID=" + row["FK_AddressId"].ToString()).ToString();
                        string fullAddr = c.GetReqData("CustomersAddress", "AddressFull", "AddressID=" + row["FK_AddressId"].ToString()).ToString();
                        strMarkup.Append("<span class=\"clrGrey fontRegular small dispBlk\"><span class=\"themeClrSec fontRegular\">(" + addrName + ")</span><br/>" + fullAddr.ToString() + "</span>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("<div class=\"float_clear\"></div>");
                        strMarkup.Append("<span class=\"lineSeperator\"></span>");
                    }
                    
                    ordStr = strMarkup.ToString();

                    
                }
            }
        }
        catch (Exception ex)
        {
            ordStr = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    [WebMethod]
    public static string GetOrderRating(string val)
    {
        iClass c = new iClass();
        c.ExecuteQuery("Update OrdersData Set OrderRating=" + Convert.ToInt32(val) + " Where OrderID=" + orderId);
        return val.ToString();
    }
}