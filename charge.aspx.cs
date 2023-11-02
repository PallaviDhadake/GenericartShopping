using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Razorpay.Api;

public partial class charge : System.Web.UI.Page
{
    iClass c = new iClass();
    public string payId;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // get values from cookies
            int oplId = 0, orderId = 0;
            string bankrrn = "";
            HttpCookie ordCookie = Request.Cookies["rzData"];
            if (ordCookie != null)
            {
                string[] arrOrd = ordCookie.Value.Split('#');
                oplId = Convert.ToInt32(arrOrd[0].ToString());
                //bankrrn = Convert.ToInt32(arrOrd[1].ToString());
                bankrrn = arrOrd[1].ToString();
                orderId = Convert.ToInt32(arrOrd[2].ToString());
            }

            double orAmount = Convert.ToDouble(c.GetReqData("OrdersData", "OrderAmount", "OrderID=" + orderId));
            double rzOrAmount = orAmount * 100;

            string paymentId = Request.Form["razorpay_payment_id"];

            Dictionary<string, object> input = new Dictionary<string, object>();
            input.Add("amount", rzOrAmount); // this amount should be same as transaction amount

            //string key = "rzp_live_P0bPwAWuVII6Nq";
            //string secret = "jUK1eLXVsGNRVjaRQDV51y5y";

            string key = "rzp_live_XLBk42M2vXMaGC";
            string secret = "qsZYzZDmiJQ7alsb6GHw6si4";

            RazorpayClient client = new RazorpayClient(key, secret);

            Dictionary<string, string> attributes = new Dictionary<string, string>();

            attributes.Add("razorpay_payment_id", paymentId);
            attributes.Add("razorpay_order_id", Request.Form["razorpay_order_id"]);
            attributes.Add("razorpay_signature", Request.Form["razorpay_signature"]);

            Utils.verifyPaymentSignature(attributes);

            //             please use the below code to refund the payment 
            //             Refund refund = new Razorpay.Api.Payment((string) paymentId).Refund();

            //Console.WriteLine(paymentId);
            payId = paymentId.ToString();

            //update paymentId as merchanttransId in online payment log table
            c.ExecuteQuery("Update online_payment_logs Set OPL_merchantTranId='" + paymentId.ToString() + "', OPL_transtatus='paid' Where OPL_id=" + oplId);
            c.ExecuteQuery("Update OrdersData Set OrderPayStatus=1, OrderPaymentTxnId='" + paymentId.ToString() + "' Where OrderID=" + orderId);
            //c.ExecuteQuery("Update OrdersData Set OrderPayStatus=1 Where OrderID=" + orderId);

            payId = "<span style=\"height:50px; display:block; width:100%;\"></span><div style=\"text-align:center\"><span style=\"font-size:2.5em; color:green; font-weight:bold\">Payment Successfull ! <br/> Your Order Placed Successfully..!!</span></div>";
        }
        catch (Exception ex)
        {
            payId = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}