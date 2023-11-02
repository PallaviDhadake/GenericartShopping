using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.IO;

public partial class gobp_registration : System.Web.UI.Page
{
    public string errMsg;
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSubmit.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnSubmit, null) + ";");

        if (!IsPostBack)
        {
            c.FillComboBox("stateName", "stateId", "StatesData", "", "stateName", 0, ddrState);
            FillOpbypes();
        }
    }

    protected void ddrState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddrState.SelectedIndex != 0)
            {
                c.FillComboBox("DistrictName", "DistrictId", "DistrictsData", "stateId=" + ddrState.SelectedValue, "DistrictName", 0, ddrDistrict);
            }
            else
            {
                errMsg = c.ErrNotification(2, "Please Select State");
                return;
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }


    protected void ddrDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddrDistrict.SelectedIndex != 0)
            {
                c.FillComboBox("cityName", "cityId", "CityData", "FK_DistId=" + ddrDistrict.SelectedValue, "cityName", 0, ddrCity);
            }
            else
            {
                errMsg = c.ErrNotification(2, "Please Select district");
                return;
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    private void FillOpbypes()
    {
        try
        {
            SqlConnection con = new SqlConnection(c.OpenConnection());

            SqlDataAdapter da = new SqlDataAdapter("Select OBPTypeName + ' (' + Convert(varchar(20), OBPAmount) + ')' as opbType, OBPTypeID From OBPTypes Where DelMark=0 Order By OBPTypeName", con);
            DataSet ds = new DataSet();
            da.Fill(ds, "myCombo");

            ddrOpbType.DataSource = ds.Tables[0];
            ddrOpbType.DataTextField = ds.Tables[0].Columns["opbType"].ColumnName.ToString();
            ddrOpbType.DataValueField = ds.Tables[0].Columns["OBPTypeID"].ColumnName.ToString();
            ddrOpbType.DataBind();

            ddrOpbType.Items.Insert(0, "<-Select->");
            ddrOpbType.Items[0].Value = "0";


            con.Close();
            con = null;
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
       
        try
        {
            txtshopName.Text = txtshopName.Text.Trim().Replace("'", "");
            txtName.Text = txtName.Text.Trim().Replace("'", "");
            txtBirthDate.Text = txtBirthDate.Text.Trim().Replace("'", "");
            txtAge.Text = txtAge.Text.Trim().Replace("'", "");
            txtEmail.Text = txtEmail.Text.Trim().Replace("'", "");
            txtMobile.Text = txtMobile.Text.Trim().Replace("'", "");
            txtWpNo.Text = txtWpNo.Text.Trim().Replace("'", "");
            txtAdd.Text = txtAdd.Text.Trim().Replace("'", "");
            txtownrOccuption.Text = txtownrOccuption.Text.Trim().Replace("'", "");
            txtUTR.Text = txtUTR.Text.Trim().Replace("'", "");
            txtBank.Text = txtBank.Text.Trim().Replace("'", "");
            txtTrDate.Text = txtTrDate.Text.Trim().Replace("'", "");
            txtHolderName.Text = txtHolderName.Text.Trim().Replace("'", "");
            txtAmount.Text = txtAmount.Text.Trim().Replace("'", "");

            if (txtAdd.Text == "" || txtAge.Text == "" || txtAmount.Text == "" || txtBank.Text == "" || txtBirthDate.Text == "" || txtEmail.Text == "" || txtHolderName.Text == "" || txtMobile.Text == "" || txtName.Text == "" || txtownrOccuption.Text == "" || txtTrDate.Text == "" || txtUTR.Text == "" || txtWpNo.Text == "" || ddrState.SelectedIndex == 0 || ddrOpbType.SelectedIndex == 0 || ddrDistrict.SelectedIndex == 0 || ddrCity.SelectedIndex == 0)
            {
                errMsg = c.ErrNotification(2, "All * marked fields are mendatory");
                return;
            }


            if (rdbProprietor.Checked == false && rdbPartner.Checked == false && rdbTrust.Checked == false && rdbOther.Checked == false)
            {
                errMsg = c.ErrNotification(2, "Select Firm Type");
                return;
            }

            //if (rdbReferralYes.Checked == false && rdbReferralYes.Checked == false)
            //{
            //    errMsg = c.ErrNotification(2, "Select Referral");
            //    return;
            //}



            if (rdbMatterYes.Checked == false && rdbMatterNo.Checked == false)
            {
                errMsg = c.ErrNotification(2, "Select if Any Illegal Matters");
                return;
            }

            if (rdbBelow5Yr.Checked == false && rdb5Yr.Checked == false && rdb10Yr.Checked == false)
            {
                errMsg = c.ErrNotification(2, "Select Residence From");
                return;
            }

            if (rdbMarried.Checked == false && rdbSingle.Checked == false)
            {
                errMsg = c.ErrNotification(2, "Select Marital Status");
                return;
            }



            if (chkTerms.Checked == false)
            {
                errMsg = c.ErrNotification(2, "You must agree to Terms & Conditions to proceed");
                return;
            }


            if (!c.EmailAddressCheck(txtEmail.Text))
            {
                errMsg = c.ErrNotification(2, "Enter valid email address");
                return;
            }

            if (!c.ValidateMobile(txtMobile.Text))
            {
                errMsg = c.ErrNotification(2, "Enter valid mobile number");
                return;
            }

            if (txtWpNo.Text != "")
            {
                if (!c.ValidateMobile(txtWpNo.Text))
                {
                    errMsg = c.ErrNotification(2, "Enter valid WhatsApp number");
                    return;
                }
            }

            if (txtAdd.Text != "")
            {
                if (txtAdd.Text.Length > 300)
                {
                    errMsg = c.ErrNotification(2, "Residential Address should be less than 300 characters");
                    return;
                }
            }

            DateTime bDate = DateTime.Now;
            if (txtBirthDate.Text != "")
            {
                string[] arrDate = txtBirthDate.Text.Split('/');
                if (c.IsDate(arrDate[1] + "/" + arrDate[0] + "/" + arrDate[2]) == false)
                {
                    errMsg = c.ErrNotification(2, "Enter valid Date of Birth");
                    return;
                }
                else
                {
                    bDate = Convert.ToDateTime(arrDate[1] + "/" + arrDate[0] + "/" + arrDate[2]);
                }
            }

            DateTime trDate = DateTime.Now;
            string[] arrTDate = txtTrDate.Text.Split('/');
            if (c.IsDate(arrTDate[1] + "/" + arrTDate[0] + "/" + arrTDate[2]) == false)
            {
                errMsg = c.ErrNotification(2, "Enter valid Transaction Date");
                return;
            }
            else
            {
                trDate = Convert.ToDateTime(arrTDate[1] + "/" + arrTDate[0] + "/" + arrTDate[2]);
            }

            int age = 0;
            if (txtAge.Text != "")
            {
                if (!c.IsNumeric(txtAge.Text))
                {
                    errMsg = c.ErrNotification(2, "Age must be numeric value");
                    return;
                }
                else
                {
                    age = Convert.ToInt16(txtAge.Text);
                }
            }

            if (txtAmount.Text != "")
            {
                if (!c.IsNumeric(txtAmount.Text))
                {
                    errMsg = c.ErrNotification(2, "Amount must be numeric value");
                    return;
                }
            }

            int maxId = c.NextId("OBPData", "OBP_ID");

            string profilePic = "";
            if (fuprofilePic.HasFile)
            {
                string fExt = Path.GetExtension(fuprofilePic.FileName).ToLower();
                if (new[] { ".jpg", ".jpeg", ".png" }.Contains(fExt))
                {
                    profilePic = "gobp-profile-" + maxId.ToString() + fExt;
                    fuprofilePic.SaveAs(Server.MapPath("~/upload/gobpData/profilePhoto/") + profilePic);
                }
                else
                {
                    errMsg = c.ErrNotification(2, "Only .jpg, .jpeg, .png  files are allowed for profile pic");
                    return;
                }
            }
            else
            {
                profilePic = "NA";
            }

            string addrProof = "", addrProof1 = "";
            if (fuAddProof.HasFile)
            {
                string fExt = Path.GetExtension(fuAddProof.FileName).ToLower();
                if (new[] { ".jpg", ".jpeg", ".png" }.Contains(fExt))
                {
                    addrProof = "addr-proof-" + maxId.ToString() + fExt;
                }
                else
                {
                    errMsg = c.ErrNotification(2, "Only .jpg, .jpeg, .png  files are allowed for Address proof");
                    return;
                }
            }
            else
            {
                errMsg = c.ErrNotification(2, "Upload address proof");
                return;
            }


            if (fuAddProof1.HasFile)
            {
                string fExt = Path.GetExtension(fuAddProof1.FileName).ToLower();
                if (new[] { ".jpg", ".jpeg", ".png" }.Contains(fExt))
                {
                    addrProof1 = "addr-proof-1-" + maxId.ToString() + fExt;
                }
                else
                {
                    errMsg = c.ErrNotification(2, "Only .jpg, .jpeg, .png files are allowed for Address proof");
                    return;
                }
            }

            string idProof = "", idProof1 = "";
            if (fulIdProof.HasFile)
            {
                string fExt = Path.GetExtension(fulIdProof.FileName).ToLower();
                if (new[] { ".jpg", ".jpeg", ".png" }.Contains(fExt))
                {
                    idProof = "id-proof-" + maxId.ToString() + fExt;
                }
                else
                {
                    errMsg = c.ErrNotification(2, "Only .jpg, .jpeg, .png files are allowed for Id proof");
                    return;
                }
            }
            else
            {
                errMsg = c.ErrNotification(2, "Upload ID proof");
                return;
            }


            if (fulIdProof1.HasFile)
            {
                string fExt = Path.GetExtension(fulIdProof1.FileName).ToLower();
                if (new[] { ".jpg", ".jpeg", ".png" }.Contains(fExt))
                {
                    idProof1 = "id-proof-1-" + maxId.ToString() + fExt;
                }
                else
                {
                    errMsg = c.ErrNotification(2, "Only .jpg, .jpeg, .png  files are allowed for Id proof");
                    return;
                }
            }

            string obpResume = "";
            if (fuResume.HasFile)
            {
                string fExt = Path.GetExtension(fuResume.FileName).ToLower();
                if (new[] { ".pdf" }.Contains(fExt))
                {
                    obpResume = "obp-resume-" + maxId.ToString() + fExt;
                    fuResume.SaveAs(Server.MapPath("~/upload/gobpData/resume/") + obpResume);
                }
                else
                {
                    errMsg = c.ErrNotification(2, "Only .pdf  files are allowed for resume");
                    return;
                }
            }
            else
            {
                obpResume = "NA";
            }


            string gobpType = "NA";
            if (rdbProprietor.Checked == true)
            {
                gobpType = "Proprietor";
            }
            else if (rdbPartner.Checked == true)
            {
                gobpType = "Partner";
            }
            else if (rdbTrust.Checked == true)
            {
                gobpType = "Trust";
            }
            else
            {
                gobpType = "Other";
            }


            string gobpEd = "NA";
            if (rdbEduTenth.Checked == true)
            {
                gobpEd = "10th - 12th";
            }
            else if (rdbEduGraduate.Checked == true)
            {
                gobpEd = "Graduate";
            }
            else if (rdbEduPG.Checked == true)
            {
                gobpEd = "Post Graduate";
            }
            else
            {
                gobpEd = "Other";
            }

            string resFrom = "NA";
            if (rdbBelow5Yr.Checked == true)
            {
                resFrom = "0 - 5 Years";
            }
            else if (rdb5Yr.Checked == true)
            {
                resFrom = "5 - 10 Years";
            }
            else
            {
                resFrom = "More than 10 Years";
            }

            string anyLegal = rdbMatterYes.Checked == true ? "YES" : "NO";
            string marital = rdbMarried.Checked == true ? "Yes" : "NO";
            // int refrral = rdbReferralYes.Checked == true ? 1 : 0;
            string encrId = c.EncryptData(txtEmail.Text + maxId.ToString());


            int dhdId = 0; int zhdId = 0;
            if (Request.QueryString["dhId"] != null)
                dhdId = Convert.ToInt32(Request.QueryString["dhId"]);

            if (Request.QueryString["zhId"] != null)
                zhdId = Convert.ToInt32(Request.QueryString["zhId"]);

            //string shopcode = "OBP000" + maxId + "";

            //object DistHeadId = c.GetReqData("DistrictHead", "DistHdId", "OBP_DH_Name='" + txtName.Text +"'");

           // object DistUserId = c.GetReqData("DistrictHead", "DistHdUserId", "DistHdId=" + DistHeadId).ToString();

            // object DhHeadId = c.GetReqData("DistrictHead", "DistHdId", "DistHdUserId='" + DhuserId + "'");

            // int obpNumber = c.NextId("OBPData", "OBP_ID");
           
            string shopcode = "OBP" + maxId.ToString("D5");


            //string shopcode = DistUserId.ToString();

            c.ExecuteQuery("Insert Into OBPData(OBP_ID, OBP_JoinDate, OBP_FKTypeID, OBP_TypeFirm, " +
                " OBP_ApplicantName, OBP_BirthDate, OBP_Age, OBP_MaritalStatus, OBP_EmailId, OBP_MobileNo, OBP_WhatsApp, OBP_Address, " +
                " OBP_StateID, OBP_DistrictID, OBP_City, OBP_OwnerEdu, OBP_OwnerOccup, OBP_ProfilePic, OBP_AddProof1, " +
                " OBP_IDProof1, OBP_Resume, OBP_LegalMatter, OBP_ResidenceFrom, OBP_UTRNum, OBP_BankName, OBP_TransDate, " +
                " OBP_AccHolder, OBP_PaidAmt, OBP_IsClosed, OBP_StatusFlag, OBP_DelMark,  OBP_ZhId, OBP_ShopName, OBP_UserPWD, OBP_UserID)Values(" + maxId +
                ",  '" + DateTime.Now + "', " + ddrOpbType.SelectedValue + ", '" + gobpType + "', '" + txtName.Text +
                "', '" + bDate + "', " + age + ", '" + marital + "', '" + txtEmail.Text + "', '" + txtMobile.Text +
                "', '" + txtWpNo.Text + "', '" + txtAdd.Text + "', " + ddrState.SelectedValue + ", " + ddrDistrict.SelectedValue +
                ", '" + ddrCity.SelectedItem.Text + "', '" + gobpEd + "', '" + txtownrOccuption.Text + "', '" + profilePic + "', '" + addrProof +
                "', '" + idProof + "', '" + obpResume + "', '" + anyLegal + "', '" + resFrom + "', '" + txtUTR.Text +
                "', '" + txtBank.Text + "', '" + trDate + "', '" + txtHolderName.Text + "', " + Convert.ToDouble(txtAmount.Text) + ", 0, " +
                " 'Pending', 0,  " + zhdId + ", '" + txtshopName.Text + "', '123456', '" + shopcode + "')");


            if (fuAddProof.HasFile)
            {
                string origPath = "~/upload/gobpData/addressProof/original/";
                string normalPath = "~/upload/gobpData/addressProof/";

                fuAddProof.SaveAs(Server.MapPath(origPath) + addrProof);
                File.Copy(Server.MapPath(origPath) + addrProof, Server.MapPath(normalPath) + addrProof);

                File.Delete(Server.MapPath(origPath) + addrProof);
            }

            if (fuAddProof1.HasFile)
            {
                string origPath = "~/upload/gobpData/addressProof/original/";
                string normalPath = "~/upload/gobpData/addressProof/";

                fuAddProof1.SaveAs(Server.MapPath(origPath) + addrProof1);
                File.Copy(Server.MapPath(origPath) + addrProof1, Server.MapPath(normalPath) + addrProof1);

                File.Delete(Server.MapPath(origPath) + addrProof1);

                c.ExecuteQuery("Update OBPData Set OBP_AddProof2='" + addrProof1 + "' Where OBP_ID=" + maxId);
            }

            if (fulIdProof.HasFile)
            {
                string origPath = "~/upload/gobpData/idProof/original/";
                string normalPath = "~/upload/gobpData/idProof/";

                fulIdProof.SaveAs(Server.MapPath(origPath) + idProof);

                File.Copy(Server.MapPath(origPath) + idProof, Server.MapPath(normalPath) + idProof);

                //c.ImageOptimizer(idProof, origPath, normalPath, 800, true);

                File.Delete(Server.MapPath(origPath) + idProof);
            }

            if (fuAddProof1.HasFile)
            {
                string origPath = "~/upload/gobpData/idProof/original/";
                string normalPath = "~/upload/gobpData/idProof/";

                fuAddProof1.SaveAs(Server.MapPath(origPath) + idProof1);

                File.Copy(Server.MapPath(origPath) + idProof1, Server.MapPath(normalPath) + idProof1);
                File.Delete(Server.MapPath(origPath) + idProof1);

                c.ExecuteQuery("Update OBPData Set OBP_IDProof2='" + idProof1 + "' Where OBP_ID=" + maxId);
            }

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Registration Done Successfully!!');", true);
            //Response.Redirect(Master.rootPath + "gobp-print-preview.aspx?id=" + maxId, false);

            // errMsg = c.ErrNotification(1, "Registration Done Successfully..!!");

            //ddrState.SelectedIndex = ddrDistrict.SelectedIndex = ddrCity.SelectedIndex = 0;
            //txtAdd.Text = txtAge.Text = txtAmount.Text = txtBank.Text = txtBirthDate.Text = txtEmail.Text = txtHolderName.Text = txtMobile.Text = "";
            //txtName.Text = txtownrOccuption.Text = txtTrDate.Text = txtUTR.Text = txtWpNo.Text = txtshopName.Text = "";

            //rdb10Yr.Checked = rdb5Yr.Checked = rdbBelow5Yr.Checked = rdbEduGraduate.Checked = rdbEduOther.Checked = rdbEduPG.Checked = false;
            //rdbEduTenth.Checked = rdbMarried.Checked = rdbMatterNo.Checked = rdbMatterYes.Checked = rdbOther.Checked = rdbPartner.Checked = false;
            //rdbProprietor.Checked = rdbSingle.Checked = rdbTrust.Checked = false;


            //Response.Redirect(Master.rootPath + "gobp-print-preview?id=" + maxId, false);
            //ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Registration Done Successfully..!!');", true);
            //ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('gobp-print-preview.aspx?id='" + maxId + ", 2000);", true);


        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

}