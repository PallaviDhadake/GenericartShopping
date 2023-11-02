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

public partial class PurchaseList : System.Web.UI.Page
{
    public string MemberId = ""; 

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.Cookies["MemberType"] != null && Request.Cookies["MemberValue"] != null)
            {
                if (Convert.ToString(Request.Cookies["MemberType"].Value) == "MemberPanel")
                {
                    MemberId = Convert.ToString(Request.Cookies["MemberValue"].Value);

                    if (IsPostBack == false)
                    {
                        TxtDateFrom.Text = (System.DateTime.Now).ToString("dd/MM/yyyy");
                        TxtDateTo.Text = (System.DateTime.Now).ToString("dd/MM/yyyy");
                        BtnSearch_Click(sender, e);
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            string AccountName = "";

            if (TxtDateFrom.Text != "")
                AccountName = AccountName + " and DATEADD(dd, 0,DATEDIFF(dd, 0, receivedate)) >= DATEADD(dd, 0,DATEDIFF(dd, 0, '" + MasterClass.ConvertDate(TxtDateFrom.Text) + "')) ";
            if (TxtDateTo.Text != "")
                AccountName = AccountName + " and DATEADD(dd, 0,DATEDIFF(dd, 0, receivedate)) <= DATEADD(dd, 0,DATEDIFF(dd, 0, '" + MasterClass.ConvertDate(TxtDateTo.Text) + "')) ";

            string Sql = "select stock.auto, ROW_NUMBER() OVER(ORDER BY stock.billno) AS Row, Member.Name AS EmployeeName, Member.Id, convert(nvarchar(15), stock.receivedate,103) as receivedate, stock.billno, convert(nvarchar(15), stock.billdate,103) as billdate, convert(decimal(10,2), stock.billamount) as billamount, convert(decimal(10,2), stock.others) as others ,convert(decimal(10,2), stock.netamount) as netamount, isnull(Stock.businessvalue, 0) as businessvalue, Case isnull(Active, 0) when 0 then 'Pending' else 'Approved' end as StatusShow from stock left join dbo.Member on stock.AccountName = Member.auto where stock.auto <> 0 " + AccountName + " and Member.Id = '" + MemberId + "' and trancode = 52 order by stock.billno";
            DataTable dt = MasterClass.Query(Sql);
            if (dt.Rows.Count > 0)
            {
                Repeater1.DataSource = dt;
                Repeater1.DataBind();

                double totalMarks = dt.Select().Sum(p => Convert.ToDouble(p["netamount"]));
                (Repeater1.Controls[Repeater1.Controls.Count - 1].Controls[0].FindControl("lblTotal") as Label).Text = totalMarks.ToString("0.00");

                double TotalBV = dt.Select().Sum(p => Convert.ToDouble(p["businessvalue"]));
                (Repeater1.Controls[Repeater1.Controls.Count - 1].Controls[0].FindControl("LblTotalBV") as Label).Text = TotalBV.ToString("");
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
            string Auto = Convert.ToString(e.CommandArgument);

            //if (e.CommandName == "View")
            //{
            //    Response.Redirect("BillReceipt.aspx?BillId=" + (Convert.ToString(Auto)) + "", false);
            //}
            //else if (e.CommandName == "CustomerWisePrint")
            //{
            //    Response.Redirect("EmployeeBillReceipt.aspx?BillId=" + (Convert.ToString(Auto)) + "", false);
            //}
        }
        catch (Exception ex)
        {
        }
    }
}
