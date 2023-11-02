using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for OrdersData
/// </summary>
public class OrdersData
{
    public int OrderID { get; set; }
    public int FK_OrderCustomerID { get; set; }


    
    public string CustomerName { get; set; }
    public string CustomerMobile { get; set; }
    public string CustomerEmail { get; set; }

    public string totalOrdersCount { get; set; }
    public string recentOrderId { get; set; }
    public int FeedBackFlag { get; set; }
}