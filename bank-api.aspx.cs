using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using RestSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


public partial class bank_api : System.Web.UI.Page
{
    public string responseMsg, apiResponse;
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        TestJsonApi();
    }

    private void TestJsonApi()
    {
        //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
        ////string WebApiUrl = string.Format("https://apibankingsandbox.icicibank.com/api/MerchantAPI/UPI/v0/CollectPay2/407423");
        //string WebApiUrl = string.Format("https://apibankingonesandbox.icicibank.com/api/MerchantAPI/UPI/v0/CollectPay2/407423");

        ////string WebApiUrl = string.Format("https://www.genericartmedicine.com/api_ecom/collectPay");

        //WebRequest requestobject = WebRequest.Create(WebApiUrl);
        //requestobject.Method = "POST";
        ////requestobject.ContentType = "application/json";
        //requestobject.ContentType = "text/plain";

        ////requestobject.Headers.Add("accept:*/*, accept-encoding:*, accept-language:en-US,en;q=0.8,hi;q=0.6, cache-control:no-cache");

        //requestobject.Headers["20"] = "*/*";
        //requestobject.Headers["22"] = "*";
        //requestobject.Headers["23"] = "en-US,en;q=0.8,hi;q=0.6";
        //requestobject.Headers["0"] = "no-cache";

        ////string postData = "{\"payerVa\" : \"testo17@icici\",\"amount\" : \"5.00\",\"note\" : \"collect-pay-request\",\"collectByDate\" :\"30/09/2021 06:30 PM\",\"merchantId\" : \"407423\",\"merchantName\":\"GenericMedicine\"\"subMerchantId\" : \"12234\",\"GenericMedicine\" : \"Test\",\"terminalId\" : \"5411\",\"merchantTranId\" : \"ABCD290806\",\"billNumber\" : \"sdf234234\"}";
        //string postData = "{ \"CustomrtID\" : \"1\",\"upi\" : \"testo17@icici\",\"note\" : \"Testing\",\"amount\" :\"100\" }";

        //using (var streamWriter = new StreamWriter(requestobject.GetRequestStream()))
        //{
        //    streamWriter.Write(postData);
        //    streamWriter.Flush();
        //    streamWriter.Close();



        //    var httpResponse = (HttpWebResponse)requestobject.GetResponse();

        //    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //    {

        //        var result = streamReader.ReadToEnd();
        //        responseMsg = "Response : " + result.ToString();
        //    }


        //}
        //============================================================================================================================================================================
        //var client = new RestClient("https://www.genericartmedicine.com/api_ecom/collectPay");
        //client.Timeout = -1;
        //var request = new RestRequest(Method.POST);
        //request.AlwaysMultipartFormData = true;
        //request.AddParameter("CustomrtID", "1");
        //request.AddParameter("upi", "nileshkothari34@oksbi");
        //request.AddParameter("note", "Testing");
        //request.AddParameter("amount", "1");
        //IRestResponse response = client.Execute(request);
        //Response.Write("Response : " + response.Content);

        //=========================================================================================================================================================================================

        //var client = new RestClient("http://49.248.119.22:41503/ws_04m_create_sord");
        //client.Timeout = -1;
        //var request = new RestRequest(Method.POST);
        //request.AlwaysMultipartFormData = true;
        
        //request.AddParameter("cIndex", "create");
        //request.AddParameter("order_no", "4");
        //request.AddParameter("order_date", "2022-02-10 15:34:33 PM");
        ////request.AddParameter("order_delivery_date", "2022-02-22T18:59:39.087Z");
        ////request.AddParameter("order_valid_date", "2022-02-22T18:59:39.087Z");
        //request.AddParameter("act_code", "GC281");
        //request.AddParameter("customer_name", "GENERICART MEDICINE PVT LTD");
        //request.AddParameter("customer_mobile", "2269024802");
        //request.AddParameter("shipping_name", "WELLNESS FOREVER HEALTHTECH PVT LTD");
        //request.AddParameter("shipping_add1", "A-4 KRISHNA MILLS COMPOUND");
        //request.AddParameter("shipping_add2", "SONAPUR LANE");
        //request.AddParameter("shipping_add3", "L.B.S. MARG AT NAHUR");
        //request.AddParameter("shipping_city", "MUMBAI");
        //request.AddParameter("shipping_pin", "400078");
        //request.AddParameter("shipping_state", "MAHARASHTRA");
        //request.AddParameter("shipping_mobile_no", "2269024802");
        //request.AddParameter("billing_name", "WELLNESS FOREVER HEALTHTECH PVT LTD");
        //request.AddParameter("billing_add1", "A-4 KRISHNA MILLS COMPOUND");
        //request.AddParameter("billing_add2", "SONAPUR LANE");
        //request.AddParameter("billing_add3", "L.B.S. MARG AT NAHUR");
        //request.AddParameter("billing_city", "MUMBAI");
        //request.AddParameter("billing_pin", "400078");
        //request.AddParameter("billing_state", "MAHARASHTRA");
        //request.AddParameter("billing_mobile_no", "2269024802");
        //request.AddParameter("discount_per", "0");
        //request.AddParameter("urgent", "NO");
        //request.AddParameter("inter_sale", "0");
        //request.AddParameter("disc_rs", "1");
        //request.AddParameter("adminSettlement", "0.00");
        //request.AddParameter("partial_conversion", "1");
        //request.AddParameter("order_value", "0.00");
        //request.AddParameter("item[]['item_id']", "378432");
        //request.AddParameter("item[]['item_branch_id']", "503");
        //request.AddParameter("item[]['item_qty']", "1");
        //request.AddParameter("item[]['item_price']", "0");
        //request.AddParameter("item[]['item_discount']", "0");
        //request.AddParameter("item[]['disc_per']", "0");
        //request.AddParameter("item[]['item_id']", "383875");
        //request.AddParameter("item[]['item_branch_id']", "503");
        //request.AddParameter("item[]['item_qty']", "2");
        //request.AddParameter("item[]['item_price']", "0");
        //request.AddParameter("item[]['item_discount']", "0");
        //request.AddParameter("item[]['disc_per']", "0");
        //IRestResponse response = client.Execute(request);
        //Response.Write("Response : " + response.Content);

        //======================================================================================================================================================

        //var client = new RestClient("https://www.genericartmedicine.com/api_ecom/transaction_new_status");
        //client.Timeout = -1;
        //var request = new RestRequest(Method.POST);
        //request.AlwaysMultipartFormData = true;
        //request.AddParameter("order_id", "716");
        //request.AddParameter("CustomrtID", "1");
        //request.AddParameter("type", "2");
        //IRestResponse response = client.Execute(request);
        ////Console.WriteLine(response.Content);
        //Response.Write("Response : " + response.Content);

        string apiUrl = String.Format("https://www.genericartmedicine.com/api_ecom/transaction_new_status"); //testing url
        WebRequest request = WebRequest.Create(apiUrl);
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        //request.ContentType = "text/plain";

        request.Headers["20"] = "*/*";
        request.Headers["22"] = "*";
        request.Headers["23"] = "en-US,en;q=0.8,hi;q=0.6";
        request.Headers["0"] = "no-cache";

        int orderIdX = 716;
        int custId = 1;
        int type = 2;

        string postData = "";
        postData = "order_id=" + orderIdX + "&CustomrtID=" + custId + "&type=" + type;

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



                apiResponse = c.ErrNotification(1, "Status : " + statusInfo.ToString() + ",<br/> Msg : " + cMsg.ToString());

                //if (statusInfo == "True")
                //{
                //    apiResponse = apiResponse + "<br/> resonse got";
                //}
            }
        }
    }
}

public class OrderResponse
{
    public string status { get; set; }
    public string messages { get; set; }
}