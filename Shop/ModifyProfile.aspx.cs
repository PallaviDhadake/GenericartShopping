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
using System.IO;

public partial class ModifyProfile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            TxtMobileNo.Enabled = false;
            TxtEmailId.Enabled = false;

            if (IsPostBack == false)
            {
                if (Request.Cookies["ShopValue"] != null)
                {
                    if (Request.Cookies["ShopValue"].Value != "")
                    {
                        Hf.Value = (Convert.ToString(Request.Cookies["ShopValue"].Value));

                        DataTable dt = MasterClass.Query("SELECT Auto ,Id ,IntroducerId ,Name ,FName ,MName ,Address ,Age ,Sex ,MaritalStatus ,DOb ,Emailid ,MobileNo ,BankName ,BranchName ,BankAccountNo ,IfscCode ,pan ,DOJ ,Status ,EntryDate ,EntryBy ,UserName ,Password from Associate where UserName = '" + Convert.ToString(Hf.Value) + "' ;");
                        if (dt.Rows.Count > 0)
                        {
                            TxtName.Text = Convert.ToString(dt.Rows[0]["name"]);
                            TxtFathersName.Text = Convert.ToString(dt.Rows[0]["fname"]);
                            TxtSex.Text = Convert.ToString(dt.Rows[0]["sex"]);

                            if (Convert.ToString(dt.Rows[0]["DOb"]) != "")
                                TxtDOB.Text = Convert.ToDateTime(dt.Rows[0]["DOb"]).ToString("dd-MM-yyyy");
                            else
                                TxtDOB.Text = System.DateTime.Now.ToString("dd-MM-yyyy");

                            TxtAddress.Text = Convert.ToString(dt.Rows[0]["address"]);
                            TxtEmailId.Text = Convert.ToString(dt.Rows[0]["emailid"]);
                            TxtMobileNo.Text = Convert.ToString(dt.Rows[0]["mobileno"]);
                            TxtAadharNo.Text = Convert.ToString(dt.Rows[0]["AadharNo"]);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void TxtSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            int i = MasterClass.NonQuery("update Shop set fname = '" + TxtFathersName.Text + "', DOB = '" + MasterClass.ConvertDate(TxtDOB.Text) + "', address='" + TxtAddress.Text + "', AadharNo = '" + TxtAadharNo.Text + "', Emailid = '" + TxtEmailId.Text.Trim() + "' where id = '" + Convert.ToString(Hf.Value) + "'");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Your Profile Updated Successfully');", true);
        }
        catch (Exception ex)
        {
        }
    }
}
