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
    public string ImageShow = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            TxtMobileNo.Enabled = false;
            TxtEmailId.Enabled = false;

            if (IsPostBack == false)
            {
                ImageShow = "images/user.png";

                int i = MasterClass.FillDDL(TxtCountry, "Select auto, Name from Country order by Name ", "Name", "Name");
                TxtCountry_SelectedIndexChanged(sender, e);
                TxtState_SelectedIndexChanged(sender, e);

                CmbRelation.Items.Clear();
                CmbRelation.Items.Add("Father");
                CmbRelation.Items.Add("Husband");
                CmbRelation.Items.Add("Wife");
                CmbRelation.Items.Add("Mother");
                CmbRelation.Items.Add("Brother");
                CmbRelation.Items.Add("Sister");

                if (Request.Cookies["MemberValue"] != null)
                {
                    i = MasterClass.FillDDL(TxtCity, "Select auto, Name from City order by Name ", "Name", "Name");
                    if (Request.Cookies["MemberValue"].Value != "")
                    {
                        Hf.Value = Convert.ToString(Request.Cookies["MemberValue"].Value);
                        DataTable dt = MasterClass.Query("Select name, fName, Address, AadharNo, introducerid, Age, bankname, branchname, bankaccountno, Sex, convert(nvarchar(10), DOB, 103) as DOB, Emailid, Country, Mobileno, bankAccountno, Photo, Level, Member1id, Member2id, State, City, Pincode, Nominee, relation, amount, ifsccode, pan, MemberPhoto from member where id= '" + Convert.ToString(Hf.Value) + "' ;");
                        if (dt.Rows.Count > 0)
                        {
                            TxtName.Text = Convert.ToString(dt.Rows[0]["name"]);
                            TxtFathersName.Text = Convert.ToString(dt.Rows[0]["fname"]);
                            TxtIntroducerID.Text = Convert.ToString(dt.Rows[0]["introducerid"]);
                            TxtSex.Text = Convert.ToString(dt.Rows[0]["sex"]);

                            TxtPAN.Text = Convert.ToString(dt.Rows[0]["pan"]);
                            TxtAddress.Text = Convert.ToString(dt.Rows[0]["address"]);
                            TxtPinCode.Text = Convert.ToString(dt.Rows[0]["pincode"]);
                            TxtEmailId.Text = Convert.ToString(dt.Rows[0]["emailid"]);
                            TxtMobileNo.Text = Convert.ToString(dt.Rows[0]["mobileno"]);
                            TxtBankName.Text = Convert.ToString(dt.Rows[0]["bankname"]);
                            TxtBranchName.Text = Convert.ToString(dt.Rows[0]["branchname"]);
                            TxtAccountNo.Text = Convert.ToString(dt.Rows[0]["bankaccountno"]);
                            TxtIFSCCode.Text = Convert.ToString(dt.Rows[0]["ifsccode"]);
                            TxtNomineeName.Text = Convert.ToString(dt.Rows[0]["nominee"]);
                            TxtAadharNo.Text = Convert.ToString(dt.Rows[0]["AadharNo"]);

                            if (Convert.ToString(dt.Rows[0]["country"]) != "")
                            {
                                TxtCountry.SelectedValue = Convert.ToString(dt.Rows[0]["country"]);
                                TxtCountry_SelectedIndexChanged(sender, e);

                                if (Convert.ToString(dt.Rows[0]["State"]) != "")
                                {
                                    TxtState.SelectedValue = Convert.ToString(dt.Rows[0]["state"]);
                                    TxtState_SelectedIndexChanged(sender, e);

                                    if (Convert.ToString(dt.Rows[0]["City"]) != "")
                                    {
                                        TxtCity.SelectedValue = Convert.ToString(dt.Rows[0]["city"]);
                                    }
                                }
                            }

                            if (Convert.ToString(dt.Rows[0]["MemberPhoto"]) != "")
                            {
                                ImageShow = "../ImageUpload/" + Convert.ToString(dt.Rows[0]["MemberPhoto"]);
                            }

                            if (Convert.ToString(dt.Rows[0]["relation"]) != "")
                            {
                                CmbRelation.SelectedValue = Convert.ToString(dt.Rows[0]["relation"]);
                            }

                            if (Convert.ToString(dt.Rows[0]["DOb"]) != "")
                                TxtDOB.Text = Convert.ToDateTime(dt.Rows[0]["DOb"]).ToString("dd-MM-yyyy");
                            else
                                TxtDOB.Text = System.DateTime.Now.ToString("dd-MM-yyyy");

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
            int i = MasterClass.NonQuery("update member set fname='" + TxtFathersName.Text + "', DOB = '" + MasterClass.ConvertDate(TxtDOB.Text) + "', address='" + TxtAddress.Text + "', city='" + TxtCity.Text + "', pincode='" + TxtPinCode.Text + "', state='" + TxtState.Text + "',  bankaccountno='" + TxtAccountNo.Text + "', bankname = '" + TxtBankName.Text + "' ,branchname = '" + TxtBranchName.Text + "', ifsccode = '" + TxtIFSCCode.Text + "', country = '" + TxtCountry.Text + "', pan = '" + TxtPAN.Text + "', nominee = '" + TxtNomineeName.Text + "', relation = '" + CmbRelation.SelectedItem.Text + "', AadharNo = '" + TxtAadharNo.Text + "' where id='" + Convert.ToString(Hf.Value) + "'");

            if (ImageUpload.HasFile == true)
            {
                string extention = (Path.GetExtension(ImageUpload.PostedFile.FileName)).ToLower();
                if ((((extention == ".jpg") || (extention == ".jpeg")) || ((extention == ".gif") || (extention == ".png"))))
                {
                    string ImageName = Convert.ToString(Hf.Value) + extention;
                    MasterClass.NonQuery("update member set MemberPhoto = '" + ImageName + "' where id = '" + Convert.ToString(Hf.Value) + "'");
                    ImageUpload.SaveAs(Server.MapPath(Path.Combine("../ImageUpload/", ImageName)));
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Your Profile Updated Successfully'); window.location='ModifyProfile.aspx'", true);
        }
        catch (Exception ex)
        {
        }
    }

    protected void TxtCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (TxtCountry.Text != "")
        {
            int i = MasterClass.FillDDL(TxtState, "Select State.Name from State left join Country on State.countryAuto = Country.auto where country.Name = '" + TxtCountry.SelectedValue + "' order by Name ", "Name", "Name");
        }
    }

    protected void TxtState_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (TxtState.Text != "")
        {
            int i = MasterClass.FillDDL(TxtCity, "Select City.Name from City left join state on City.stateAuto = state.auto where state.Name = '" + TxtState.SelectedValue + "' order by Name ", "Name", "Name");
        }
    }
}
