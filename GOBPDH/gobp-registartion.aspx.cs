using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GOBPDH_gobp_registartion : System.Web.UI.Page
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
            txtParentGobp.Text = txtParentGobp.Text.Trim().Replace("'", "");

            txtParentGobp.Text = txtParentGobp.Text.ToUpper();

            if (txtAdd.Text == "" || txtAge.Text == "" || txtAmount.Text == "" || txtBank.Text == "" || txtBirthDate.Text == "" || txtEmail.Text == "" || txtHolderName.Text == "" || txtMobile.Text == "" || txtName.Text == "" || txtownrOccuption.Text == "" || txtTrDate.Text == "" || txtUTR.Text == "" || txtWpNo.Text == "" || ddrState.SelectedIndex == 0 || ddrOpbType.SelectedIndex == 0 || ddrDistrict.SelectedIndex == 0 || ddrCity.SelectedIndex == 0)
            {
                //errMsg = c.ErrNotification(2, "All * marked fields are mendatory");
                //return;

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * marked fields are mendatory');", true);
                return;
            }


            if (rdbProprietor.Checked == false && rdbPartner.Checked == false && rdbTrust.Checked == false && rdbOther.Checked == false)
            {

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select Firm Type.');", true);
                return;
            }

            //if (rdbReferralYes.Checked == false && rdbReferralYes.Checked == false)
            //{
            //    errMsg = c.ErrNotification(2, "Select Referral");
            //    return;
            //}



            if (rdbMatterYes.Checked == false && rdbMatterNo.Checked == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select if any Illegal matters');", true);
                return;
            }

            if (rdbBelow5Yr.Checked == false && rdb5Yr.Checked == false && rdb10Yr.Checked == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select Residence From');", true);
                return;
            }

            if (rdbMarried.Checked == false && rdbSingle.Checked == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select Marital Status');", true);
                return;
            }



            if (chkTerms.Checked == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'You must agree to Terms & Conditions to proceed');", true);
                return;
            }


            if (!c.EmailAddressCheck(txtEmail.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter valid email address');", true);
                return;
            }

            if (!c.ValidateMobile(txtMobile.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter valid mobile number');", true);
                return;
            }

            if (txtWpNo.Text != "")
            {
                if (!c.ValidateMobile(txtWpNo.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter valid WhatsApp number');", true);
                    return;
                }
            }

            if (txtAdd.Text != "")
            {
                if (txtAdd.Text.Length > 300)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Residential Address should be less than 300 characters.');", true);
                    return;
                }
            }

            DateTime bDate = DateTime.Now;
            if (txtBirthDate.Text != "")
            {
                string[] arrDate = txtBirthDate.Text.Split('/');
                if (c.IsDate(arrDate[1] + "/" + arrDate[0] + "/" + arrDate[2]) == false)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter valid Date of Birth');", true);
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
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter valid Transaction Date');", true);
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
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Age must be numeric value');", true);
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
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Amount must be numeric value');", true);
                    return;
                }
            }

            int maxId = c.NextId("OBPData", "OBP_ID");

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
            string encrId = c.EncryptData(txtEmail.Text + maxId.ToString());


            int dhdId = 0; int zhdId = 0;
            if (Request.QueryString["dhId"] != null)
                dhdId = Convert.ToInt32(Request.QueryString["dhId"]);

            if (Request.QueryString["zhId"] != null)
                zhdId = Convert.ToInt32(Request.QueryString["zhId"]);

            string shopcode = "OBP" + maxId.ToString("D5");

            string gobpid = c.GetReqData("[dbo].[DistrictHead]", "[DistHdUserId]", "[DistHdId] = " + Session["adminGOBPDH"]).ToString();

            string gobpname = c.GetReqData("[dbo].[DistrictHead]", "[DistHdName]", "[DistHdId] = " + Session["adminGOBPDH"]).ToString();


            //object obpId = c.GetReqData("OBPData", "OBP_ID", "OBP_DH_UserId='"+ gobpid + "'").ToString();

            //object ObpUserid = c.GetReqData("OBPData", "OBP_UserID", "OBP_ID=" + obpId).ToString();

            //object joinlevel = Convert.ToInt32(c.GetReqData("OBPData", "OBP_JoinLevel", "OBP_UserID='" + ObpUserid + "'"));
            //int myJoinlevel = Convert.ToInt32(joinlevel);

            //int obpjoinlevel = 0;

            //if (myJoinlevel > 0)
            //{
            //    obpjoinlevel = myJoinlevel + 1;
            //}

            c.ExecuteQuery("Insert Into OBPData(OBP_ID, OBP_JoinDate, OBP_FKTypeID, OBP_TypeFirm, OBP_DH_UserId, OBP_DH_Name, " +
                " OBP_ApplicantName, OBP_BirthDate, OBP_Age, OBP_MaritalStatus, OBP_EmailId, OBP_MobileNo, OBP_WhatsApp, OBP_Address, " +
                " OBP_StateID, OBP_DistrictID, OBP_City, OBP_OwnerEdu, OBP_OwnerOccup, OBP_LegalMatter, OBP_ResidenceFrom, OBP_UTRNum, OBP_BankName, OBP_TransDate, " +
                " OBP_AccHolder, OBP_PaidAmt, OBP_IsClosed, OBP_StatusFlag, OBP_DelMark,  OBP_ZhId, OBP_ShopName, OBP_UserPWD, OBP_UserID)Values(" + maxId +
                ",  '" + DateTime.Now + "', " + ddrOpbType.SelectedValue + ", '" + gobpType + "', '" + gobpid + "', '" + gobpname + "' ,'" + txtName.Text +
                "', '" + bDate + "', " + age + ", '" + marital + "', '" + txtEmail.Text + "', '" + txtMobile.Text +
                "', '" + txtWpNo.Text + "', '" + txtAdd.Text + "', " + ddrState.SelectedValue + ", " + ddrDistrict.SelectedValue +
                ", '" + ddrCity.SelectedItem.Text + "', '" + gobpEd + "', '" + txtownrOccuption.Text + "', '" + anyLegal + "', '" + resFrom + "', '" + txtUTR.Text +
                "', '" + txtBank.Text + "', '" + trDate + "', '" + txtHolderName.Text + "', " + Convert.ToDouble(txtAmount.Text) + ", 0, " +
                " 'Pending', 0,  " + zhdId + ", '" + txtshopName.Text + "', '123456', '" + shopcode + "')");


            // Check Parent GOBP is Valid OR Not ??
            // GMDH0099 : This is DH User Id of "Laxman Ambi"
            if (txtParentGobp.Text != "")
            {
                if (c.IsRecordExist("Select OBP_ID From OBPData Where OBP_UserID='" + txtParentGobp.Text + "' AND IsMLM=1"))
                {
                    //Update Join level 

                    //  object ObpUserid = c.GetReqData("OBPData", "OBP_UserID", "OBP_ID=" + Session["adminObp"]).ToString();

                    object joinlevel = Convert.ToInt32(c.GetReqData("OBPData", "OBP_JoinLevel", "OBP_UserID='" + shopcode + "'"));

                    int myJoinlevel = Convert.ToInt32(joinlevel);

                    int obpjoinlevel = 0;

                    if (myJoinlevel > 0)
                    {
                        obpjoinlevel = myJoinlevel + 1;
                    }

                    c.ExecuteQuery("Update OBPData set OBP_Ref_UserId='" + txtParentGobp.Text + "', OBP_JoinLevel=" + obpjoinlevel + " where OBP_ID=" + maxId);

                    // Calculate GOBP Recruitement Commission Chain (Avoided duplicate entries)
                    //if (c.IsRecordExist("Select ObpComId From OBPCommission Where ObpRefUserId='" + txtUserId.Text + "'") == false)
                    //{
                    //    c.GOBP_Recruit_CommissionChain(txtParentGobp.Text, txtUserId.Text);
                    //}
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'You are entered invalid GOBP Code');", true);
                    return;

                    //errMsg = c.ErrNotification(2, "You are entered invalid GOBP Code OR DH selection is invalid !");
                    //return;
                }

            }

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Registration Done Successfully..!!');", true);

            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('registered-gobp.aspx', 2000);", true);

            //Response.Redirect("registered-gobp.aspx");
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}