using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CustomerLookup
/// </summary>
public class CustomerLookup
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerMob { get; set; }
    public string CustomerDOB { get; set; }
    public string CustomerJoinDate { get; set; }
    public string CustomerFavShop { get; set; }
    public string TotalOrders { get; set; }
    public string LastOrderDate { get; set; }
    public string ProductsPurchased { get; set; }
    public string TotalOrderAmount { get; set; }
    public string AvgOrderAmount { get; set; }
    public string YearlyOrderSummary { get; set; }

	public CustomerLookup()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}