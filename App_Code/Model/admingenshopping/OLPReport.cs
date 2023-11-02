using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for OLPReport
/// </summary>
public class OLPReport
{
    public int OrderID { get; set; }
    public int OrderStatus { get; set; }
    public string ordDate { get; set; }
    public string custInfo { get; set; }
    public string ordAmount { get; set; }
    public string ordPaidAmount { get; set; }
    public string FranchName { get; set; }
    public string FranchShopcode { get; set; }
    public string Shopstatus { get; set; }
    public string OrderPaymentTxnId { get; set; }
    public string OPL_transtatus { get; set; }
    public string OLP_device_type { get; set; }
}