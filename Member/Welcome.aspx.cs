using System;
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

public partial class Welcome : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["Value"] != null)
            {
                Hf.Value = Convert.ToString(Request.QueryString["Value"]);
                DataTable dt = MasterClass.Query("Select id, Package.Name as Packagetype, CONVERT(NVARCHAR(10), doj, 103) AS DOJ, Member.name, Address, introducerid, Emailid, Mobileno, Pan, AadharNo, (Select Member1.Name from Member as Member1 where Member1.Id = Member.IntroducerId) as IntroducerName from member LEFT JOIN pin ON dbo.Member.PinId = dbo.Pin.PinId LEFT JOIN dbo.Package ON dbo.Package.Auto = pin.package where id = '" + Convert.ToString(Hf.Value) + "' ;");
                if (dt.Rows.Count > 0)
                {
                    LblID.Text = Convert.ToString(dt.Rows[0]["id"]);
                    LblHeadName.Text = Convert.ToString(dt.Rows[0]["name"]);
                    LblName.Text = Convert.ToString(dt.Rows[0]["name"]);
                    LblDOJ.Text = Convert.ToString(dt.Rows[0]["DOJ"]);
                    LblPackage.Text = Convert.ToString(dt.Rows[0]["Packagetype"]);
                    //LblEmailId.Text = Convert.ToString(dt.Rows[0]["Emailid"]);
                    LblAddress.Text = Convert.ToString(dt.Rows[0]["Address"]);
                    //LblPanNo.Text = Convert.ToString(dt.Rows[0]["Pan"]);
                    //LblAadharNo.Text = Convert.ToString(dt.Rows[0]["AadharNo"]);
                    LblSponsorId.Text = Convert.ToString(dt.Rows[0]["IntroducerId"]);
                    LblSponsorName.Text = Convert.ToString(dt.Rows[0]["IntroducerName"]);
                }
            }
            else if (Request.Cookies["MemberValue"] != null)
            {
                if (Request.Cookies["MemberValue"].Value != "")
                {
                    Hf.Value = Convert.ToString(Request.Cookies["MemberValue"].Value);
                    DataTable dt = MasterClass.Query("Select id, Package.Name as Packagetype, CONVERT(NVARCHAR(10), member.entrydate, 103) AS entrydate, CONVERT(NVARCHAR(10), member.DOJ, 103) AS DOJ, Member.name, Address, introducerid, Emailid, Mobileno, Pan, AadharNo, (Select Member1.Name from Member as Member1 where Member1.Id = Member.IntroducerId) as IntroducerName from member LEFT JOIN pin ON dbo.Member.PinId = dbo.Pin.PinId LEFT JOIN dbo.Package ON dbo.Package.Auto = pin.package where id = '" + Convert.ToString(Hf.Value) + "' ;");
                    if (dt.Rows.Count > 0)
                    {
                        LblID.Text = Convert.ToString(dt.Rows[0]["id"]);
                        LblHeadName.Text = Convert.ToString(dt.Rows[0]["name"]);
                        LblName.Text = Convert.ToString(dt.Rows[0]["name"]);
                        LblDOJ.Text = Convert.ToString(dt.Rows[0]["entrydate"]);
                        LblTopUpDate.Text = Convert.ToString(dt.Rows[0]["DOJ"]);
                        LblPackage.Text = Convert.ToString(dt.Rows[0]["Packagetype"]);
                       // LblEmailId.Text = Convert.ToString(dt.Rows[0]["Emailid"]);
                        LblAddress.Text = Convert.ToString(dt.Rows[0]["Address"]);
                        //LblPanNo.Text = Convert.ToString(dt.Rows[0]["Pan"]);
                        //LblAadharNo.Text = Convert.ToString(dt.Rows[0]["AadharNo"]);
                        LblSponsorId.Text = Convert.ToString(dt.Rows[0]["IntroducerId"]);
                        LblSponsorName.Text = Convert.ToString(dt.Rows[0]["IntroducerName"]);
                    }
                }
                else
                {
                    Response.Redirect("Default.aspx");
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

}