using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Text;

public partial class PrescriptionList : System.Web.UI.Page
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

                    DataTable dt = MasterClass.Query("select Name, UserName, password, Case isnull(Status, 0) when 1 then 'Active' else 'InActive' end as StatusShow, PinCode from Associate where UserName = '" + ShopId + "'");
                    if (dt.Rows.Count > 0)
                    {
                        if (ShopId == Convert.ToString(dt.Rows[0]["UserName"]))
                        {
                            ShopId = Convert.ToString(dt.Rows[0]["UserName"]);
                            PinCode = Convert.ToString(dt.Rows[0]["PinCode"]);

                            BtnSearch_Click(sender, e);
                        }
                        else
                            Response.Redirect("~/Log_Out.aspx", false);
                    }
                    else
                        Response.Redirect("~/Log_Out.aspx", false);
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

            string Sql = "SELECT ProductEnquiry. Auto, Name, Emailid, pincode, UserId, Date, CartId, UploadPrescription, ShopId, ISNULL(ProductEnquiry.Status, 0) AS ProductEnquiryStatus FROM ProductEnquiry LEFT JOIN Member ON ProductEnquiry.UserId = Member.Id WHERE EnquiryType = 'Prescription' AND ProductEnquiry.Status = 0 AND ISNULL(ShopId, '') = '' AND ISNULL(ProductEnquiry.Cancel, 0) = 0 " + AccountName + " And Member.PinCode = " + PinCode + "";
            DataTable dt = MasterClass.Query(Sql);
            if (dt.Rows.Count > 0)
            {
                Repeater1.DataSource = dt;
                Repeater1.DataBind();
            }
            else
            {
                LblMassageShow.Text = "No Record Found";
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
            if (e.CommandName == "Activate")
            {
                HF1.Value = Convert.ToString(e.CommandArgument);
                DataTable dt = MasterClass.Query("select * from ProductEnquiry where auto = '" + Convert.ToString(HF1.Value) + "' ");
                if (dt.Rows.Count > 0)
                {
                    MasterClass.NonQuery("update ProductEnquiry set Status = '1', ShopId = '" + ShopId + "' where auto = '" + Convert.ToString(HF1.Value) + "'");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Approve Successfull');", true);
                    Response.Redirect("PrescriptionList.aspx");
                }
                else
                    Repeater1.Visible = false;

                Repeater1.Visible = true;
            }
        }
        catch (Exception)
        {
        }
    }
}

