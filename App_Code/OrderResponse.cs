using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for OrderResponse
/// </summary>
public class OrderResponse
{
    public string status { get; set; }
    public string messages { get; set; }
    public string razorpay_order_id { get; set; }
    public string transtatus { get; set; }
    public string txn_id { get; set; }
}