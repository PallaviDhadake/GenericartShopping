using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class ProductEnquiryList : System.Web.UI.Page
{
    public string ShopId = "", PinCode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.Cookies["ShopType"] != null && Request.Cookies["ShopValue"] != null)
            {
                if ((Convert.ToString(Request.Cookies["ShopType"].Value)) == "Shop_Panel")
                {
                    ShopId = (Convert.ToString(Request.Cookies["ShopValue"].Value));

                    DataTable dtShop = MasterClass.Query("select Name, UserName, password, Case isnull(Status, 0) when 1 then 'Active' else 'InActive' end as StatusShow, PinCode from Associate where UserName = '" + ShopId + "'");
                    if (dtShop.Rows.Count > 0)
                    {
                        ShopId = Convert.ToString(dtShop.Rows[0]["UserName"]);
                        PinCode = Convert.ToString(dtShop.Rows[0]["PinCode"]);

                        if (Page.IsPostBack == false)
                        {
                            BtnSearch_Click(sender, e);
                        }
                    }
                }
                else
                    Response.Redirect("~/Log_Out.aspx", false);
            }
            else
                Response.Redirect("~/Log_Out.aspx", false);
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Log_Out.aspx", false);
        }
    }

    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            string AccountName = "";

            if (TxtDateFrom.Text != "")
                AccountName = AccountName + " and Dateadd(dd, 0, datediff(dd, 0, ProductEnquiry.Date)) >= Dateadd(dd, 0, datediff(dd, 0, '" + MasterClass.ConvertDate(TxtDateFrom.Text) + "')) ";
            if (TxtDateTo.Text != "")
                AccountName = AccountName + " and Dateadd(dd, 0, datediff(dd, 0, ProductEnquiry.Date)) <= Dateadd(dd, 0, datediff(dd, 0, '" + MasterClass.ConvertDate(TxtDateTo.Text) + "')) ";

            DataTable dt = MasterClass.Query("SELECT ROW_NUMBER() OVER (ORDER BY CartId DESC) AS Row, Name, Emailid, pincode, CONVERT(VARCHAR(10), MAX(Date), 103) AS Date, CartId, UserId, SUM(CONVERT(FLOAT, Quantity)) AS TotalQuantity, Member.Name, STUFF((SELECT ', ' + RTRIM(LTRIM(Item.Name)) FROM Item WHERE Item.Auto IN (SELECT SubProductEnquiry.ProductAuto FROM ProductEnquiry AS SubProductEnquiry WHERE SubProductEnquiry.CartId = ProductEnquiry.CartId AND SubProductEnquiry.EnquiryType = 'Product') FOR XML PATH('')), 1, 1, '') AS ProductName FROM ProductEnquiry LEFT JOIN Member ON ProductEnquiry.UserId = Member.Id WHERE ProductEnquiry.Status = 1 AND EnquiryType = 'Product' AND ISNULL(ShopId, '') = '' " + AccountName + " And Member.PinCode = " + PinCode + " AND ISNULL(ProductEnquiry.Cancel, 0) = 0 GROUP BY Name, Emailid, pincode, CartId, UserId ORDER BY CartId DESC");
            if (dt.Rows.Count > 0)
            {
                Repeater1.DataSource = dt;
                Repeater1.DataBind();
            }
            else
            {
                Repeater1.DataSource = null;
                Repeater1.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        try
        {
            int CartId = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "ViewThis")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", "<script>$('#myModal').modal('show'); $('#myModal').addClass('intro');</script>", false);
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup();", true);
                DataTable dtProductEnquiry = MasterClass.Query("SELECT ROW_NUMBER() OVER (ORDER BY Item.Name) AS Row, Member.[Name], SUM(CONVERT(FLOAT, Quantity)) AS Quantity, UserId, Item.Name AS ItemName, SaleRate, (Isnull(SaleRate, 0) * Isnull(SUM(CONVERT(FLOAT, Quantity)), 0)) AS Amount FROM [ProductEnquiry] LEFT JOIN Member ON [ProductEnquiry].UserId = Member.Id LEFT JOIN Item ON ProductEnquiry.ProductAuto = Item.Auto WHERE ISNULL(ProductEnquiry.Status, 0) = 1 AND EnquiryType = 'Product' AND ISNULL(ShopId, '') = '' and ISNULL(ProductEnquiry.Quantity, 0) > 0 and [CartId] = " + CartId + " group by Member.[Name], UserId, Item.Name, SaleRate ");
                if (dtProductEnquiry.Rows.Count > 0)
                {
                    RepDetail.DataSource = dtProductEnquiry;
                    RepDetail.DataBind();
                }
            }
            if (e.CommandName == "Activate")
            {
                DataTable dt = MasterClass.Query("select * from ProductEnquiry where CartId = '" + CartId + "' AND EnquiryType = 'Product' ");
                if (dt.Rows.Count > 0)
                {
                    MasterClass.NonQuery("update ProductEnquiry set Status = '1', ShopId = '" + ShopId + "' where CartId = '" + CartId + "' And EnquiryType = 'Product'");
                    BtnSearch_Click(source, e);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Approve Successfull'); window.location='ProductEnquiryList.aspx'", true);
                }
                else
                    Repeater1.Visible = false;

                Repeater1.Visible = true;
            }
        }
        catch (Exception ex)
        {
        }
    }
}