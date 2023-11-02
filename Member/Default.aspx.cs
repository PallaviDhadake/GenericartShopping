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

public partial class Default : System.Web.UI.Page
{
    public string MemberId = "", UserName = "", MemberAmount = "", LeftMember = "", RightMember = "", StatusShow = "", DOJ = "", EntryDate = "";
    public double BinaryIncome = 0, SponcershipIncome = 0, LoginIncome = 0, AdminCharge = 0, TDS = 0, ROIIncomeEarned = 0, ROIIncomeGenerated = 0, FRCIncome = 0;
    public double TotalTeam = 0, LeftPurchaseBV = 0, RightPurchaseBV = 0, SelfBV = 0, LeftUpliner = 0, RightUpliner = 0, LeftActiveUpliner = 0, RightActiveUpliner = 0, LeftRePurchaseBV = 0, RightRePurchaseBV = 0, SelfRePurchaseBV = 0, LeftRewardPurchaseBV = 0, RightRewardPurchaseBV = 0, RewardPurchaseSelfBV = 0, SelfBVCurrentMonth = 0, TotalBV = 0;
    public double ActivationBV = 0, LeftTotalTeam = 0, RightTotalTeam = 0, LeftActiveTotalTeam = 0, RightActiveTotalTeam = 0, TotalActiveTeam = 0, LeftCureentMonthTotalBV = 0, RightCureentMonthTotalBV = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.Cookies["MemberType"] != null && Request.Cookies["MemberValue"] != null)
            {
                if (Convert.ToString(Request.Cookies["MemberType"].Value) == "MemberPanel")
                {
                    MemberId = Convert.ToString(Request.Cookies["MemberValue"].Value);

                    DataTable dt = MasterClass.Query("select id, password, isnull(Amount, 0) as Amount, leftProfile, RightProfile, Member1Id, Member2Id, Case isnull(Status, 0) when 1 then 'Active' else 'Deactive' end as StatusShow, DOJ, EntryDate from member where id = '" + MemberId + "'");
                    if (dt.Rows.Count > 0)
                    {
                        MemberAmount = Convert.ToDouble(dt.Rows[0]["Amount"]).ToString("0.00");
                        LeftMember = Convert.ToString(dt.Rows[0]["leftProfile"]);
                        RightMember = Convert.ToString(dt.Rows[0]["RightProfile"]);
                        StatusShow = Convert.ToString(dt.Rows[0]["StatusShow"]);

                        
                        if (Convert.ToString(dt.Rows[0]["EntryDate"]) != "")
                            EntryDate = Convert.ToDateTime(dt.Rows[0]["EntryDate"]).ToString("dd/MM/yyyy");
                        else
                            EntryDate = Convert.ToDateTime(dt.Rows[0]["DOJ"]).ToString("dd/MM/yyyy");

                        DOJ = Convert.ToDateTime(dt.Rows[0]["DOJ"]).ToString("dd/MM/yyyy");

                    }
                    else
                        Response.Redirect("~/MemberLogin.aspx", false);
                }
                else
                    Response.Redirect("~/MemberLogin.aspx", false);
            }
            else
                Response.Redirect("~/MemberLogin.aspx", false);
        }
        catch (Exception ex)
        {
        }
    }
}
