using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for coupon
/// </summary>
public class coupon
{
	public string CouponID { get; set; }
	public string CouponImg { get; set; }
	public string CouponTitle { get; set; }
	public string CouponInfo { get; set; }
	public string CouponTerms { get; set; }
	public string CouponCode { get; set; }
	public string CouponType { get; set; }
	public string CouponRefType { get; set; }
	public string CouponProductName { get; set; }
	public int CouponRefId { get; set; }
	public string CouponProductOffer { get; set; }
	public int CouponProductId { get; set; }
	public int CouponProductQty { get; set; }
	public int CouponPercentage { get; set; }
	public double CouponCompareVal { get; set; }
	public double CouponMinAmount { get; set; }
	public double CouponMaxAmount { get; set; }
	public double CouponMaxAllow { get; set; }
	public double CouponUsedAmount { get; set; }
	public string CouponStartDate { get; set; }
	public string CouponEndDate { get; set; }
	public string CouponDisplay { get; set; }
	public coupon()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}