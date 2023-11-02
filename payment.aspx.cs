using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Razorpay.Api;
using System.Web.Security;

using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Data;

public partial class payment : System.Web.UI.Page
{
    iClass c = new iClass();
    public string orderId, rootPath, custName, custMob, custEmail, ordAmount, errMsg, oAmt;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            rootPath = c.ReturnHttp();
            if (Request.QueryString["orderId"] != null)
            {
                if (Request.QueryString["oplId"] != null)
                {
                    int custId = Convert.ToInt32(c.GetReqData("OrdersData", "FK_OrderCustomerID", "OrderID=" + Request.QueryString["orderId"]));

                    using (DataTable dtCustInfo = c.GetDataTable("Select CustomrtID, CustomerName, CustomerMobile, CustomerEmail From CustomersData Where CustomrtID=" + custId))
                    {
                        if (dtCustInfo.Rows.Count > 0)
                        {
                            DataRow row = dtCustInfo.Rows[0];

                            custName = row["CustomerName"].ToString();
                            custMob = row["CustomerMobile"].ToString();
                            custEmail = row["CustomerEmail"].ToString();
                        }
                    }

                    //double orAmount = Convert.ToDouble(c.GetReqData("OrdersData", "OrderAmount", "OrderID=" + Request.QueryString["orderId"]));
                    //OrderPayableAmount
                    double orAmount = Convert.ToDouble(c.GetReqData("OrdersData", "OrderPayableAmount", "OrderID=" + Request.QueryString["orderId"]));
                    double rzOrAmount = orAmount * 100;
                    ordAmount = rzOrAmount.ToString();
                    oAmt = orAmount.ToString();


                    Dictionary<string, object> input = new Dictionary<string, object>();
                    input.Add("amount", rzOrAmount); // this amount should be same as transaction amount
                    input.Add("currency", "INR");
                    input.Add("receipt", "12121");
                    input.Add("payment_capture", 1);

                    //string key = "rzp_live_P0bPwAWuVII6Nq";
                    //string secret = "jUK1eLXVsGNRVjaRQDV51y5y";

                    string key = "rzp_live_XLBk42M2vXMaGC";
                    string secret = "qsZYzZDmiJQ7alsb6GHw6si4";

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                    RazorpayClient client = new RazorpayClient(key, secret);

                    Razorpay.Api.Order order = client.Order.Create(input);
                    orderId = order["id"].ToString();

                    //update orderId in onlin_payment_log table as bankRRN no
                    c.ExecuteQuery("Update online_payment_logs Set OPL_BankRRN='" + orderId + "' Where OPL_id=" + Request.QueryString["oplId"]);

                    //add orderid(bank rrn no) and opl_id in cookies
                    Response.Cookies["rzData"].Value = Request.QueryString["oplId"] + "#" + orderId + "#" + Request.QueryString["orderId"];
                    Response.Cookies["rzData"].Expires = DateTime.Now.AddDays(1);
                }
            }
            else
            {
                //Response.Redirect(rootPath + "checkout/payment", false);
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}