using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for OrdersDataAdmin
/// </summary>
public class OrdersDataAdmin
{
    public int OrderID { get; set; }
    public int OrderStatus { get; set; }
    public int FK_OrderCustomerID { get; set; }
    public string orStatus { get; set; }
    public string ordDate { get; set; }
    public string CustomerName { get; set; }
    public string CustomerMobile { get; set; }
    public string OrdAmount { get; set; }
    public int ProductCount { get; set; }
    public string CartProducts { get; set; }
    public string DeviceType { get; set; }
}