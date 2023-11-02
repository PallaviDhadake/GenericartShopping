using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class customer_lookup : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg;
    public string[] arrShopInfo = new string[20];
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["custId"] != null)
            {
                GetLookupInfo(Convert.ToInt32(Request.QueryString["custId"]));
            }
        }
    }

    private void GetLookupInfo(int custId)
    {
        try
        {
            CustomerLookup cLookup = c.GetCustLookupDetails(custId);

            arrShopInfo[0] = cLookup.CustomerName;
            arrShopInfo[1] = cLookup.CustomerEmail;
            arrShopInfo[2] = cLookup.CustomerMob;
            arrShopInfo[3] = cLookup.CustomerDOB;
            arrShopInfo[4] = cLookup.CustomerJoinDate;
            arrShopInfo[5] = cLookup.CustomerFavShop;
            arrShopInfo[6] = cLookup.TotalOrders;
            arrShopInfo[7] = cLookup.LastOrderDate;
            arrShopInfo[8] = cLookup.ProductsPurchased;
            arrShopInfo[9] = cLookup.TotalOrderAmount;
            arrShopInfo[10] = cLookup.AvgOrderAmount;
            arrShopInfo[11] = cLookup.YearlyOrderSummary;
        }
        catch (Exception ex)
        {
            errMsg = ex.Message.ToString();
            return;
        }
    }
}