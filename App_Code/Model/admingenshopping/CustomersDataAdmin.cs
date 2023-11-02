using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CustomersDataAdmin
/// </summary>
public class CustomersDataAdmin : RegisteredCustomer
{
    public string DeviceType { get; set; }
    public string CustomerPassword { get; set; }
    public string JoinDate { get; set; }

}