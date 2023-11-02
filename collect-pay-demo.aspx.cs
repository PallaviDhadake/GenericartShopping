using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestSharp;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public partial class collect_pay_demo : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSubmit_Click1(object sender, EventArgs e)
    {
        try
        {
            var client = new RestClient("https://www.genericartmedicine.com/api_ecom/collectPay");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("CustomrtID", "1");
            request.AddParameter("upi", txtUpi.Text);
            request.AddParameter("note", "Testing");
            request.AddParameter("amount", txtAmount.Text);
            IRestResponse response = client.Execute(request);
            Response.Write("Response : " + response.Content);

            //dynamic data = JObject.Parse(response);

            //JObject json = JObject.Parse(response.Content);



            string json = "{\"Name\":\"John Doe\",\"Occupation\":\"gardener\"," +
                            "\"DateOfBirth\":{\"year\":1995,\"month\":11,\"day\":30}}";

            //string user = JsonSerializer.Deserialize<String>(json);

            //Console.WriteLine(user);

            //Console.WriteLine(user.Name);
            //Console.WriteLine(user.Occupation);
            //Console.WriteLine(user.DateOfBirth);
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}
