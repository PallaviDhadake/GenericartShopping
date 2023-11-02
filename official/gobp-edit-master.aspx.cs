using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Sql;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

public partial class official_gobp_edit_master : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg, addrProof, idProof, resume, profile, myDistrictHead;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            c.FillComboBox("stateName", "stateId", "StatesData", "", "stateName", 0, ddrState);
            FillOpbypes();

            // Fill District Heads

            //object disthdid = c.GetReqData("", "", "");

            //c.FillComboBox("distHdName", "distHdId", "DistrictHead", "DistHdUserId IN ('nandurbarsf', 'GMDH0099')", "distHdName", 0, ddrDistrictHead);
            c.FillComboBox("distHdName", "distHdId", "DistrictHead", "DelMark=0", "distHdName", 0, ddrDistrictHead);

            if (Request.QueryString["id"] != null)
            {
                GetOBPData(Convert.ToInt32(Request.QueryString["id"]));
            }
        }
    }

    protected void ddrState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddrState.SelectedIndex != 0)
            {
                c.FillComboBox("DistrictName", "DistrictId", "DistrictsData", "StateId=" + ddrState.SelectedValue, "DistrictName", 0, ddrDistrict);
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

    protected void btnSave_Click(object sender, EventArgs e)
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
            //txtShopCode.Text = txtShopCode.Text.Trim().Replace("'", "");

            txtBankNameInfo.Text = txtBankNameInfo.Text.Trim().Replace("'", "");
            txtAccName.Text = txtAccName.Text.Trim().Replace("'", "");
            txtAccNumber.Text = txtAccNumber.Text.Trim().Replace("'", "");
            txtIFSC.Text = txtIFSC.Text.Trim().Replace("'", "");

            txtParentGobp.Text = txtParentGobp.Text.ToUpper();


            if (txtAdd.Text == "" || txtAge.Text == "" || txtAmount.Text == "" || txtBank.Text == "" || txtBirthDate.Text == "" || txtEmail.Text == "" || txtHolderName.Text == "" || txtMobile.Text == "" || txtName.Text == "" || txtownrOccuption.Text == "" || txtTrDate.Text == "" || txtUTR.Text == "" || txtWpNo.Text == "" || txtBankNameInfo.Text == "" || txtAccName.Text == "" || txtAccNumber.Text == "" || txtIFSC.Text == "" || ddrState.SelectedIndex == 0 || ddrOpbType.SelectedIndex == 0 || ddrDistrict.SelectedIndex == 0 || ddrDistrictHead.SelectedIndex==0)
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

            if (txtSalesInc.Text != "")
            {
                if (!c.IsNumeric(txtSalesInc.Text))
                {
                    errMsg = c.ErrNotification(2, "Sales Amount must be numeric value");
                    return;
                }
            }

            if (txtReferal.Text != "")
            {
                if (!c.IsNumeric(txtReferal.Text))
                {
                    errMsg = c.ErrNotification(2, "Referral Amount must be numeric value");
                    return;
                }
            }

            if (txtPurchase.Text != "")
            {
                if (!c.IsNumeric(txtPurchase.Text))
                {
                    errMsg = c.ErrNotification(2, "Total purchase amount must be numeric value");
                    return;
                }
            }

            int maxId = Convert.ToInt32(Request.QueryString["id"]);

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
            string status = chkactivateAc.Checked == true ? "Active" : "Pending";
            string encrId = c.EncryptData(txtEmail.Text + maxId.ToString());


            //int dhdId = 0; int zhdId = 0;
            //if (Request.QueryString["dhId"] != null)
            //    dhdId = Convert.ToInt32(Request.QueryString["dhId"]);

            //if (Request.QueryString["zhId"] != null)
            //    zhdId = Convert.ToInt32(Request.QueryString["zhId"]);

            double salesInc = txtSalesInc.Text != "" ? Convert.ToDouble(txtSalesInc.Text) : 0;
            double referral = txtReferal.Text != "" ? Convert.ToDouble(txtReferal.Text) : 0;
            double totPuch = txtPurchase.Text != "" ? Convert.ToDouble(txtPurchase.Text) : 0;

            string accTypeOption = rdbSaving.Checked == true ? "Saving" : "Current";

            DateTime joinDate = DateTime.Now;
            if (txtJoinDate.Text != "")
            {
                string[] arrJDate = txtJoinDate.Text.Split('/');
                joinDate = Convert.ToDateTime(arrJDate[1] + "/" + arrJDate[0] + "/" + arrJDate[2]);
            }

            //if (c.IsRecordExist("Select OBP_ShopCode From OBPData Where OBP_ShopCode='" + txtShopCode.Text + "' AND  OBP_ID!="+Request.QueryString["id"]+""))
            //{
            //    errMsg = c.ErrNotification(2, "This OBP Code alreday exist");
            //    return;
            //}

            //========= Corporate Commission validation starts ==========
            if(txtCorpComm.Text == "" || txtCorpComm.Text == "0")
            {
                if (ddrOpbType.SelectedValue == "4")
                {
                    errMsg = c.ErrNotification(2, "Corporate GOBP should have proper commission value.");
                    return;
                }
                else
                {
                    txtCorpComm.Text = "0";
                }
            }
            //========= Corporate Commission validation ends ==========


            c.ExecuteQuery("Update OBPData set OBP_FKTypeID=" + ddrOpbType.SelectedValue + ", OBP_TypeFirm='" + gobpType +
                "', OBP_ApplicantName='" + txtName.Text + "', OBP_BirthDate='" + bDate + "', OBP_Age=" + age +
                ", OBP_MaritalStatus='" + marital + "',  OBP_EmailId='" + txtEmail.Text + "', OBP_MobileNo='" + txtMobile.Text +
                "', OBP_WhatsApp='" + txtWpNo.Text + "', OBP_Address='" + txtAdd.Text + "', OBP_StateID=" + ddrState.SelectedValue +
                ", OBP_DistrictID=" + ddrDistrict.SelectedValue + ", OBP_City='" + ddrCity.SelectedItem.Text +
                "', OBP_OwnerEdu='" + gobpEd + "', OBP_OwnerOccup='" + txtownrOccuption.Text + "', OBP_LegalMatter='" + anyLegal +
                "', OBP_ResidenceFrom='" + resFrom + "', OBP_UTRNum='" + txtUTR.Text + "',  OBP_BankName='" + txtBank.Text +
                "',  OBP_UserPWD='" + txtPassword.Text + "',  OBP_TransDate='" + trDate +
                "', OBP_AccHolder='" + txtHolderName.Text + "', OBP_PaidAmt=" + txtAmount.Text + ", OBP_ShopName='" + txtshopName.Text +
                "', OBP_SalesIncent=" + salesInc + ", OBP_Referral=" + referral + ", OBP_TotalPurchase=" + totPuch +
                ", OBP_Remark='" + txtRemark.Text + "', OBP_StatusFlag='" + status + "',  OBP_BankAccType='" + accTypeOption + "', OBP_BankNameInfo='" + txtBankNameInfo.Text + "', OBP_BankAccByName='" + txtAccName.Text +
                "', OBP_BankAccNumber='" + txtAccNumber.Text + "', OBP_BankIFSC='" + txtIFSC.Text + "', OBP_CorpCommission=" + txtCorpComm.Text + " Where OBP_ID=" + maxId);

            object DistUserId = c.GetReqData("DistrictHead", "DistHdUserId", "distHdId=" + ddrDistrictHead.SelectedValue);

            object DistrictHeadName = c.GetReqData("DistrictHead", "DistHdName", "distHdId="+ ddrDistrictHead.SelectedValue);

            if (ddrDistrictHead.SelectedIndex > 0)
            {
                c.ExecuteQuery("Update OBPData Set OBP_DH_UserId='" + DistUserId + "', OBP_DH_Name='"+ DistrictHeadName + "' Where OBP_ID=" + maxId);
            }


            if(c.IsRecordExist("Select DistHdId from DistrictHead where DistHdUserId='" + DistUserId + "'"))
            {
               
                object MLMFlagObject = c.GetReqData("DistrictHead", "IsMLM", "DistHdUserId='" + DistUserId + "'");
                string MLMFlag1 = MLMFlagObject != null ? MLMFlagObject.ToString() : null;



                if (MLMFlag1 != null && MLMFlag1.ToString() == "1")
                {
                    c.ExecuteQuery("Update OBPData set IsMLM=1 where OBP_ID=" + maxId);
                }

            }

            // Check Parent GOBP is Valid OR Not ??
            // GMDH0099 : This is DH User Id of "Laxman Ambi"
            if (txtParentGobp.Text != "")
            {
                if (c.IsRecordExist("Select OBP_ID From OBPData Where OBP_DH_UserId='GMDH0099' AND OBP_UserID='" + txtParentGobp.Text + "' AND IsMLM=1"))
                {
                    //Update Join level 

                  //  object ObpUserid = c.GetReqData("OBPData", "OBP_UserID", "OBP_ID=" + Session["adminObp"]).ToString();

                    object joinlevel = Convert.ToInt32(c.GetReqData("OBPData", "OBP_JoinLevel", "OBP_UserID='" + txtUserId.Text + "'"));

                    int myJoinlevel = Convert.ToInt32(joinlevel);

                    int obpjoinlevel = 0;

                    if (myJoinlevel > 0)
                    {
                        obpjoinlevel = myJoinlevel + 1;
                    }

                    c.ExecuteQuery("Update OBPData set OBP_Ref_UserId='" + txtParentGobp.Text + "', OBP_JoinLevel="+ obpjoinlevel + " where OBP_ID=" + maxId);

                    // Calculate GOBP Recruitement Commission Chain (Avoided duplicate entries)
                    if(c.IsRecordExist("Select ObpComId From OBPCommission Where ObpRefUserId='" + txtUserId.Text + "'") == false)
                    {
                        c.GOBP_Recruit_CommissionChain(txtParentGobp.Text, txtUserId.Text);
                    }
                }
                else
                {
                    errMsg = c.ErrNotification(2, "You are entered invalid GOBP Code OR DH selection is invalid !");
                    return;
                }

            }

            //if (txtUserId.Text != "")
            //{
            //    if (!c.IsRecordExist("Select OBP_ShopCode From OBPData Where OBP_UserID IS NOT NULL AND OBP_StatusFlag='Active' AND OBP_ID=" + maxId))
            //    {
            //        c.ExecuteQuery("Update OBPData Set OBP_StatusFlag='Active' Where OBP_ID=" + maxId);
            //    }

            //    if (!IsRecordExist("Select OBP_ID From OBPData Where OBP_UserID='" + txtUserId.Text + "' AND OBP_StatusFlag='Active' AND OBP_DelMark=0 AND OBP_FKTypeID=" + ddrOpbType.SelectedValue))
            //    {
            //        int ecomMaxId = NextId("OBPData", "OBP_ID");

            //        object dhId = c.GetReqData("OBPData", "OBP_DhId", "OBP_ID=" + maxId);
            //        string dhUname = "-"; string dhName = "-";
            //        if (dhId != DBNull.Value && dhId != null && dhId.ToString() != "")
            //        {
            //            dhUname = c.GetReqData("DistrictHead", "userId", "distHdId=" + Convert.ToInt32(dhId)).ToString();
            //            dhName = c.GetReqData("DistrictHead", "distHdName", "distHdId=" + Convert.ToInt32(dhId)).ToString();
            //        }

            //        //if (c.IsRecordExist("Select OBP_ShopCode From OBPData Where OBP_ShopCode='"+ txtShopCode .Text+ "'"))
            //        //{

            //        //    errMsg = c.ErrNotification(2, "This OBP Code alreday exist");
            //        //    return;

            //        //}

            //        ExecuteQuery("Insert Into OBPData (OBP_ID, OBP_JoinDate, OBP_FKTypeID, OBP_ApplicantName, " +
            //            " OBP_EmailId, OBP_MobileNo, OBP_UserID, OBP_UserPWD, OBP_StatusFlag, OBP_DelMark, OBP_DH_UserId, OBP_DH_Name " +
            //            " ) Values (" + ecomMaxId + ", '" + joinDate + "', " + ddrOpbType.SelectedValue + ", '" + txtName.Text +
            //            "', '" + txtEmail.Text + "', '" + txtMobile.Text + "', '" + txtUserId.Text +
            //            "', 123456, 'Active', 0, '" + dhUname + "', '" + dhName + "')");
            //    }
            //}

            if (fuAddProof.HasFile)
            {
                string origPath = "~/upload/gobpData/addressProof/original/";
                string normalPath = "~/upload/gobpData/addressProof/";

                fuAddProof.SaveAs(Server.MapPath(origPath) + addrProof);
                //File.Copy(Server.MapPath(origPath) + addrProof, Server.MapPath(normalPath) + addrProof);
                c.ImageOptimizer(addrProof, origPath, normalPath, 700, true);

                File.Delete(Server.MapPath(origPath) + addrProof);

                c.ExecuteQuery("Update OBPData Set OBP_AddProof1='" + addrProof + "' Where OBP_ID=" + maxId);
            }

            if (fuAddProof1.HasFile)
            {
                string origPath = "~/upload/gobpData/addressProof/original/";
                string normalPath = "~/upload/gobpData/addressProof/";

                fuAddProof1.SaveAs(Server.MapPath(origPath) + addrProof1);
                //File.Copy(Server.MapPath(origPath) + addrProof1, Server.MapPath(normalPath) + addrProof1);
                c.ImageOptimizer(addrProof1, origPath, normalPath, 700, true);

                File.Delete(Server.MapPath(origPath) + addrProof1);

                c.ExecuteQuery("Update OBPData Set OBP_AddProof2='" + addrProof1 + "' Where OBP_ID=" + maxId);
            }

            if (fulIdProof.HasFile)
            {
                string origPath = "~/upload/gobpData/idProof/original/";
                string normalPath = "~/upload/gobpData/idProof/";

                fulIdProof.SaveAs(Server.MapPath(origPath) + idProof);

                //File.Copy(Server.MapPath(origPath) + idProof, Server.MapPath(normalPath) + idProof);
                c.ImageOptimizer(idProof, origPath, normalPath, 700, true);
                //c.ImageOptimizer(idProof, origPath, normalPath, 800, true);

                File.Delete(Server.MapPath(origPath) + idProof);
                c.ExecuteQuery("Update OBPData Set OBP_IDProof1='" + idProof + "' Where OBP_ID=" + maxId);
            }

            if (fulIdProof1.HasFile)
            {
                string origPath = "~/upload/gobpData/idProof/original/";
                string normalPath = "~/upload/gobpData/idProof/";

                fulIdProof1.SaveAs(Server.MapPath(origPath) + idProof1);

                //File.Copy(Server.MapPath(origPath) + idProof1, Server.MapPath(normalPath) + idProof1);
                c.ImageOptimizer(idProof1, origPath, normalPath, 700, true);
                File.Delete(Server.MapPath(origPath) + idProof1);

                c.ExecuteQuery("Update OBPData Set OBP_IDProof2='" + idProof1 + "' Where OBP_ID=" + maxId);
            }

            if (fuprofilePic.HasFile)
            {
                c.ExecuteQuery("Update OBPData Set OBP_ProfilePic='" + profilePic + "' Where OBP_ID=" + maxId);
            }

            if (fuResume.HasFile)
            {
                c.ExecuteQuery("Update OBPData Set OBP_Resume='" + obpResume + "' Where OBP_ID=" + maxId);
            }


            //=================================INSERT GOBP DATA TO ECOM============================================//

            //AddOBPtoECOMM(maxId);

            //================================ INSERT GOBP DATA TO ECOM END========================================//



            errMsg = c.ErrNotification(1, "Data Updated Successfully..!!");

            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('gobp-registration-master.aspx', 2000);", true);

            ddrOpbType.SelectedIndex = ddrState.SelectedIndex = ddrDistrict.SelectedIndex = ddrCity.SelectedIndex = 0;
            txtAdd.Text = txtAge.Text = txtAmount.Text = txtBank.Text = txtBirthDate.Text = txtEmail.Text = txtHolderName.Text = txtMobile.Text = "";
            txtName.Text = txtownrOccuption.Text = txtTrDate.Text = txtUTR.Text = txtWpNo.Text = txtshopName.Text = "";

            rdb10Yr.Checked = rdb5Yr.Checked = rdbBelow5Yr.Checked = rdbEduGraduate.Checked = rdbEduOther.Checked = rdbEduPG.Checked = false;
            rdbEduTenth.Checked = rdbMarried.Checked = rdbMatterNo.Checked = rdbMatterYes.Checked = rdbOther.Checked = rdbPartner.Checked = false;
            rdbProprietor.Checked = rdbSingle.Checked = rdbTrust.Checked = false;

            //if (Request.QueryString["type"] != null)
            //{
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('gobp-registration-master.aspx?type=" + Request.QueryString["type"] + "', 2000);", true);
            //}
            //else
            //{
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('gobp-registration-master.aspx', 2000);", true);
            //}
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            c.ExecuteQuery("Update OBPData Set OBP_DelMark=1 Where OBP_ID=" + Request.QueryString["id"]);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'GOBP Data Deleted', type: 'success'});", true);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('gobp-registration-master.aspx', 2000);", true);
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["type"] != null)
        {
            Response.Redirect("gobp-registration-master.aspx?type=" + Request.QueryString["type"], false);
        }
        else
        {
            Response.Redirect("gobp-registration-master.aspx", false);
        }
    }

    private void GetOBPData(int obpIdX)
    {
        try
        {
            using (DataTable dtEnq = c.GetDataTable("Select * From OBPData Where OBP_ID=" + obpIdX))
            {
                if (dtEnq.Rows.Count > 0)
                {
                    lblId.Text = obpIdX.ToString();
                    DataRow row = dtEnq.Rows[0];

                    if (row["OBP_JoinDate"] != DBNull.Value && row["OBP_JoinDate"].ToString() != "" && row["OBP_JoinDate"] != null)
                    {
                        txtJoinDate.Text = Convert.ToDateTime(row["OBP_JoinDate"]).ToString("dd/MM/yyyy");
                    }

                    txtName.Text = row["OBP_ApplicantName"] != DBNull.Value ? row["OBP_ApplicantName"].ToString() : "";


                    switch (row["OBP_TypeFirm"].ToString())
                    {
                        case "Proprietor":
                            rdbProprietor.Checked = true;
                            break;
                        case "Partner":
                            rdbPartner.Checked = true;
                            break;
                        case "Trust":
                            rdbTrust.Checked = true;
                            break;
                        case "Other":
                            rdbOther.Checked = true;
                            break;
                    }

                    txtshopName.Text = row["OBP_ShopName"] != DBNull.Value ? row["OBP_ShopName"].ToString() : "";



                    txtAdd.Text = row["OBP_Address"] != DBNull.Value ? row["OBP_Address"].ToString() : "";

                    //txtShopCode.Text = row["OBP_ShopCode"] != DBNull.Value ? row["OBP_ShopCode"].ToString() : "";

                    ddrState.SelectedValue = row["OBP_StateID"].ToString();
                    c.FillComboBox("DistrictName", "DistrictId", "DistrictsData ", "StateId=" + ddrState.SelectedValue, "", 0, ddrDistrict);
                    ddrDistrict.SelectedValue = row["OBP_DistrictID"] != DBNull.Value ? row["OBP_DistrictID"].ToString() : "0";

                    c.FillComboBox("cityName", "cityId", "CityData ", "FK_DistId=" + ddrDistrict.SelectedValue, "", 0, ddrCity);
                    ddrCity.SelectedItem.Text = row["OBP_City"] != DBNull.Value ? row["OBP_City"].ToString() : "0";

                    txtMobile.Text = row["OBP_MobileNo"] != DBNull.Value ? row["OBP_MobileNo"].ToString() : "";
                    txtEmail.Text = row["OBP_EmailId"] != DBNull.Value ? row["OBP_EmailId"].ToString() : "";
                    txtWpNo.Text = row["OBP_WhatsApp"] != DBNull.Value ? row["OBP_WhatsApp"].ToString() : "";

                    txtUserId.Text = row["OBP_UserID"] != DBNull.Value ? row["OBP_UserID"].ToString() : "";
                    txtPassword.Text = row["OBP_UserPWD"] != DBNull.Value ? row["OBP_UserPWD"].ToString() : "";
                    txtSalesInc.Text = row["OBP_SalesIncent"] != DBNull.Value ? row["OBP_SalesIncent"].ToString() : "";
                    txtReferal.Text = row["OBP_Referral"] != DBNull.Value ? row["OBP_Referral"].ToString() : "";
                    txtPurchase.Text = row["OBP_TotalPurchase"] != DBNull.Value ? row["OBP_TotalPurchase"].ToString() : "";
                    txtRemark.Text = row["OBP_Remark"] != DBNull.Value ? row["OBP_Remark"].ToString() : "";

                    txtBankNameInfo.Text = row["OBP_BankNameInfo"] != DBNull.Value ? row["OBP_BankNameInfo"].ToString() : "";
                    txtAccName.Text = row["OBP_BankAccByName"] != DBNull.Value ? row["OBP_BankAccByName"].ToString() : "";
                    txtAccNumber.Text = row["OBP_BankAccNumber"] != DBNull.Value ? row["OBP_BankAccNumber"].ToString() : "";
                    txtIFSC.Text = row["OBP_BankIFSC"] != DBNull.Value ? row["OBP_BankIFSC"].ToString() : "";
                    txtParentGobp.Text=row["OBP_Ref_UserId"] != DBNull.Value ? row["OBP_BankIFSC"].ToString() : "";
                    /////
                    object DistUserId1 = c.GetReqData("OBPData", "OBP_DH_UserId", "OBP_ID=" + obpIdX);

                    object DistUserId = c.GetReqData("DistrictHead", "DistHdUserId", "distHdId=" + ddrDistrictHead.SelectedValue);

                    object DhHeadId = c.GetReqData("DistrictHead", "DistHdId", "DistHdUserId='" + DistUserId1 + "'");


                    //object DistUserId = c.GetReqData("DistrictHead", "distHdId", "distHdId=" + ddrDistrictHead.SelectedValue);

                    ddrDistrictHead.SelectedValue = DhHeadId.ToString();

                    if (row["OBP_BirthDate"] != DBNull.Value && row["OBP_BirthDate"].ToString() != "" && row["OBP_BirthDate"].ToString() != null)
                    {
                        txtBirthDate.Text = Convert.ToDateTime(row["OBP_BirthDate"]).ToString("dd/MM/yyyy");
                    }

                    if (row["OBP_StatusFlag"] != DBNull.Value && row["OBP_StatusFlag"].ToString() != "" && row["OBP_StatusFlag"].ToString() != null)
                    {
                        if (row["OBP_StatusFlag"].ToString() == "Active")
                            chkactivateAc.Checked = true;
                        else
                            chkactivateAc.Checked = false;
                    }
                    else
                    {
                        chkactivateAc.Checked = false;
                    }


                    txtAge.Text = row["OBP_Age"] != DBNull.Value ? row["OBP_Age"].ToString() : "";

                    switch (row["OBP_OwnerEdu"].ToString())
                    {
                        case "10 - 12th":
                            rdbEduTenth.Checked = true;
                            break;
                        case "Graduate":
                            rdbEduGraduate.Checked = true;
                            break;
                        case "Post Graduate":
                            rdbEduPG.Checked = true;
                            break;
                        case "Other":
                            rdbEduOther.Checked = true;
                            break;
                    }

                    txtownrOccuption.Text = row["OBP_OwnerOccup"] != DBNull.Value ? row["OBP_OwnerOccup"].ToString() : "";

                    switch (row["OBP_LegalMatter"].ToString())
                    {
                        case "YES":
                            rdbMatterYes.Checked = true;
                            break;
                        case "NO":
                            rdbMatterNo.Checked = true;
                            break;
                    }


                    ddrOpbType.SelectedValue = row["OBP_FKTypeID"].ToString();

                    switch (row["OBP_ResidenceFrom"].ToString())
                    {
                        case "0 - 5 Years":
                            rdbBelow5Yr.Checked = true;
                            break;
                        case "5 - 10 Years":
                            rdb5Yr.Checked = true;
                            break;
                        case "More than 10 Years":
                            rdb10Yr.Checked = true;
                            break;
                    }


                    switch (row["OBP_MaritalStatus"].ToString())
                    {
                        case "NO":
                            rdbSingle.Checked = true;
                            break;
                        case "Yes":
                            rdbMarried.Checked = true;
                            break;
                    }

                    if (row["OBP_AddProof1"] != DBNull.Value && row["OBP_AddProof1"].ToString() != "" && row["OBP_AddProof1"].ToString() != null && row["OBP_AddProof1"].ToString() != "no-photo.png")
                    {
                        StringBuilder strAddr = new StringBuilder();

                        //enqData[27] = "<a href=\"../upload/addrProof/" + row["addressProof"].ToString() + "\" target=\"_blank\" title=\"View Larger\" ><img src=\"../upload/addrProof/" + row["addressProof"].ToString() + "\" style=\"width:150px;\" /></a>";
                        if (row["OBP_AddProof2"] != DBNull.Value && row["OBP_AddProof2"].ToString() != "" && row["OBP_AddProof2"].ToString() != null && row["OBP_AddProof2"].ToString() != "no-photo.png")
                        {
                            strAddr.Append("<div style=\"width:50%; float:right;\">");
                            strAddr.Append("<div style=\"padding:5px;\">");
                            strAddr.Append("<a href=\"../upload/gobpData/addressProof/" + row["OBP_AddProof1"].ToString() + "\" target=\"_blank\" title=\"View Larger\" ><img src=\"../upload/gobpData/addressProof/" + row["OBP_AddProof1"].ToString() + "\" style=\"width:100%;\" /></a>");
                            strAddr.Append("</div>");
                            strAddr.Append("</div>");
                            strAddr.Append("<div style=\"width:50%; float:right;\">");
                            strAddr.Append("<div style=\"padding:5px;\">");
                            strAddr.Append("<a href=\"../upload/gobpData/addressProof/" + row["OBP_AddProof2"].ToString() + "\" target=\"_blank\" title=\"View Larger\" ><img src=\"../upload/gobpData/addressProof/" + row["OBP_AddProof2"].ToString() + "\" style=\"width:100%;\" /></a>");
                            strAddr.Append("</div>");
                            strAddr.Append("</div>");
                            strAddr.Append("<div class=\"float_clear\"></div>");
                        }
                        else
                        {
                            strAddr.Append("<a href=\"../upload/gobpData/addressProof/" + row["OBP_AddProof1"].ToString() + "\" target=\"_blank\" title=\"View Larger\" ><img src=\"../upload/gobpData/addressProof/" + row["OBP_AddProof1"].ToString() + "\" style=\"width:50%;\" /></a>");
                        }

                        addrProof = strAddr.ToString();
                    }
                    else
                    {
                        addrProof = "<img src=\"../upload/gobpData/addressProof/no-photo.png\" style=\"width:150px;\" />";
                    }

                    if (row["OBP_IDProof1"] != DBNull.Value && row["OBP_IDProof1"].ToString() != "" && row["OBP_IDProof1"].ToString() != null)
                    {
                        StringBuilder strId = new StringBuilder();

                        if (row["OBP_IDProof2"] != DBNull.Value && row["OBP_IDProof2"].ToString() != "" && row["OBP_IDProof2"].ToString() != null)
                        {
                            strId.Append("<div style=\"width:50%; float:right;\">");
                            strId.Append("<div style=\"padding:5px;\">");
                            strId.Append("<a href=\"../upload/gobpData/idProof/" + row["OBP_IDProof1"].ToString() + "\" target=\"_blank\" title=\"View Larger\" ><img src=\"../upload/gobpData/idProof/" + row["OBP_IDProof1"].ToString() + "\" style=\"width:100%;\" /></a>");
                            strId.Append("</div>");
                            strId.Append("</div>");
                            strId.Append("<div style=\"width:50%; float:right;\">");
                            strId.Append("<div style=\"padding:5px;\">");
                            strId.Append("<a href=\"../upload/gobpData/idProof/" + row["OBP_IDProof2"].ToString() + "\" target=\"_blank\" title=\"View Larger\" ><img src=\"../upload/gobpData/idProof/" + row["OBP_IDProof2"].ToString() + "\" style=\"width:100%;\" /></a>");
                            strId.Append("</div>");
                            strId.Append("</div>");
                            strId.Append("<div class=\"float_clear\"></div>");
                        }
                        else
                        {
                            strId.Append("<a href=\"../upload/gobpData/idProof/" + row["OBP_IDProof1"].ToString() + "\" target=\"_blank\" title=\"View Larger\" ><img src=\"../upload/gobpData/idProof/" + row["OBP_IDProof1"].ToString() + "\" style=\"width:50%;\" /></a>");
                        }

                        idProof = strId.ToString();
                    }
                    else
                    {
                        idProof = "<img src=\"../upload/gobpData/idProof/no-photo.png\" style=\"width:150px;\" />";
                    }

                    txtUTR.Text = row["OBP_UTRNum"] != DBNull.Value ? row["OBP_UTRNum"].ToString() : "";
                    txtBank.Text = row["OBP_BankName"] != DBNull.Value ? row["OBP_BankName"].ToString() : "";
                    txtTrDate.Text = row["OBP_TransDate"] != DBNull.Value ? Convert.ToDateTime(row["OBP_TransDate"]).ToString("dd/MM/yyyy") : "";
                    txtHolderName.Text = row["OBP_AccHolder"] != DBNull.Value ? row["OBP_AccHolder"].ToString() : "";
                    txtAmount.Text = row["OBP_PaidAmt"] != DBNull.Value ? row["OBP_PaidAmt"].ToString() : "";

                    if (row["OBP_ProfilePic"] != DBNull.Value && row["OBP_ProfilePic"].ToString() != "" && row["OBP_ProfilePic"].ToString() != null)
                    {
                        profile = "<img src=\"../upload/gobpData/profilePhoto/" + row["OBP_ProfilePic"].ToString() + "\" style=\"width:150px;\" />";
                    }
                    else
                    {
                        profile = "<img src=\"../upload/gobpData/profilePhoto/no-photo.png\" style=\"width:150px;\" />";
                    }

                    if (row["OBP_Resume"] != DBNull.Value && row["OBP_Resume"].ToString() != "" && row["OBP_Resume"].ToString() != null)
                    {
                        resume = "<a href=\"../upload/gobpData/resume/" + row["OBP_Resume"].ToString() + "\"  class=\"pdfLink\"\" />View Resume</a>";
                    }
                    else
                    {
                        resume = "-";
                    }

                    //Show District Head Info (If any)
                    //if (Convert.ToInt32(row["OBP_DhId"]) > 0 && row["OBP_DhId"].ToString() != null)
                    //{
                    //    //myDistrictHead = c.GetReqData("DistrictHead", "distHdName", "distHdId=" + Convert.ToInt32(row["OBP_DhId"])).ToString();
                    //    myDistrictHead = "<span class=\"stCompleted tiny\">Existing: " + c.GetReqData("DistrictHead", "distHdName", "distHdId=" + Convert.ToInt32(row["OBP_DhId"])).ToString() + "</span>";
                    //}
                    //else
                    //{
                    //    myDistrictHead = "<span class=\"stSnooze tiny\" >No Distric Head assigned before</span>";
                    //}

                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }



    public string OpenConnection1()
    {
        return System.Web.Configuration.WebConfigurationManager.ConnectionStrings["GenCartDATAEcom"].ConnectionString;
    }

    public int NextId(string tableName, string fieldName)
    {
        try
        {
            int retValue = 1;
            SqlConnection con = new SqlConnection(OpenConnection1());
            con.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr = default(SqlDataReader);
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select MAX(" + fieldName + ") as maxNo From " + tableName;
            dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if ((dr["maxNo"]) != System.DBNull.Value)
                    {
                        retValue = Convert.ToInt32(dr["maxNo"]) + 1;
                    }
                    else
                    {
                        retValue = 1;
                    }
                }
            }
            else
            {
                retValue = 1;
            }
            dr.Close();
            cmd.Dispose();
            con.Close();
            con = null;
            return retValue;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public bool IsRecordExist(string strQuery)
    {
        try
        {

            bool rValue = false;
            SqlConnection con = new SqlConnection(OpenConnection1());
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = default(SqlDataReader);

            cmd.CommandText = strQuery;
            dr = cmd.ExecuteReader();

            rValue = dr.HasRows;
            dr.Close();
            cmd.Dispose();
            con.Close();
            con = null;

            return rValue;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    public void ExecuteQuery(string strQuery)
    {
        try
        {
            SqlConnection con = new SqlConnection(OpenConnection1());
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = strQuery;
            cmd.CommandTimeout = 1200000;
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
            con = null;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }


    //private void AddOBPtoECOMM(int maxId)
    //{
    //    ecommClass cEcomm = new ecommClass();
    //    GobpInfo cOBPInfo = new GobpInfo();

    //    int maxIdX = cEcomm.NextId("OBPData", "OBP_ID");

    //    int GobpId = Convert.ToInt32(Request.QueryString["id"]);


    //    cOBPInfo.OBPData(GobpId);

    //    // Check already DH data present in ECOMM DH table?
    //    if (cEcomm.IsRecordExist("Select OBP_ID From OBPData Where OBP_UserID='" + txtUserId.Text + "'") == false)
    //    {

    //        cEcomm.ExecuteQuery("Insert Into OBPData(OBP_ID, OBP_FKTypeID, OBP_ApplicantName, OBP_EmailId, OBP_MobileNo, OBP_UserID, OBP_UserPWD, OBP_StatusFlag, " +
    //            "OBP_DelMark, OBP_DH_UserId, OBP_BankAccType, OBP_BankNameInfo, OBP_BankAccByName, OBP_BankAccNumber, OBP_BankIFSC) Values(" + maxIdX + ", " + cOBPInfo.OBPFkTypeId + ", '" + cOBPInfo.OBPAppliName + "'" +
    //            ", '" + cOBPInfo.OBPEmailId + "', '" + cOBPInfo.OBPMobileNo + "', '" + cOBPInfo.OBPShopCode + "', '" + cOBPInfo.OBPStatusFlag + "', 0, '" + cOBPInfo.OBPDHID + "'" +
    //            ", '" + cOBPInfo.OBPDistrict + "', '" + cOBPInfo.OBPBankAccType + "', '" + cOBPInfo.OBPBankNameInfo + "', '" + cOBPInfo.OBPBankAccByName + "', '" + cOBPInfo.OBPBankAccNumber + "', '" + cOBPInfo.OBPBankIFSC + "')");


    //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Record Added Successfully..!!', type: 'success'});", true);

    //    }
    //    else
    //    {

    //        cEcomm.ExecuteQuery("Update OBPData set OBP_FKTypeID=" + cOBPInfo.OBPFkTypeId + ", OBP_ApplicantName='" + cOBPInfo.OBPAppliName + "', OBP_EmailId='" + cOBPInfo.OBPEmailId + "', OBP_MobileNo='" + cOBPInfo.OBPMobileNo + "', OBP_UserID='" + cOBPInfo.OBPShopCode + "',  OBP_BankAccType='" + cOBPInfo.OBPBankAccType + "', " +
    //            "OBP_BankNameInfo='" + cOBPInfo.OBPBankNameInfo + "', OBP_BankAccByName='" + cOBPInfo.OBPBankAccByName + "', OBP_BankAccNumber='" + cOBPInfo.OBPBankAccNumber + "', OBP_BankIFSC='" + cOBPInfo.OBPBankIFSC + "' Where OBP_UserID='" + txtUserId.Text + "' ");

    //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Record Updated Successfully..!!', type: 'success'});", true);

    //    }

    //}

    //protected void btnEcomm_Click(object sender, EventArgs e)
    //{
    //    //DistrictHeadInfo cDHinfo = new DistrictHeadInfo();
    //    //ecommClass cEcomm = new ecommClass();

    //    int GobpId = Convert.ToInt32(Request.QueryString["id"]);
    //    AddOBPtoECOMM(GobpId);
    //}


    //protected void ddrDistrictHead_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    object DistUserId = c.GetReqData("DistrictHead", "DistHdUserId", "distHdId=" + ddrDistrictHead.SelectedValue);
       
    //    txtParesntgobp.Text = DistUserId.ToString();
    //}
}