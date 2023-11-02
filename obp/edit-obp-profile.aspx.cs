using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class obp_edit_obp_profile : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg, addrProof, idProof, resume, profile, myDistrictHead, rootPath;
    protected void Page_Load(object sender, EventArgs e)
    {
       

        if (!IsPostBack)
        {
            c.FillComboBox("stateName", "stateId", "StatesData", "", "stateName", 0, ddrState);
            GetOBPData(Convert.ToInt32(Session["adminObp"]));
            
        }
        lblId.Visible = false;
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

                    c.FillComboBox("DistrictName", "DistrictId", "DistrictsData", "StateId=" + ddrState.SelectedValue + "", "DistrictName", 0, ddrDistrict);
                    ddrDistrict.SelectedValue = row["OBP_DistrictID"].ToString();

                    c.FillComboBox("cityName", "cityId", "CityData ", "FK_DistId=" + ddrDistrict.SelectedValue, "", 0, ddrCity);
                    ddrCity.SelectedItem.Text = row["OBP_City"] != DBNull.Value ? row["OBP_City"].ToString() : "0";



                    txtMobile.Text = row["OBP_MobileNo"] != DBNull.Value ? row["OBP_MobileNo"].ToString() : "";
                    txtEmail.Text = row["OBP_EmailId"] != DBNull.Value ? row["OBP_EmailId"].ToString() : "";
                    txtWpNo.Text = row["OBP_WhatsApp"] != DBNull.Value ? row["OBP_WhatsApp"].ToString() : "";


                    object DistUserId1 = c.GetReqData("OBPData", "OBP_DH_UserId", "OBP_ID=" + obpIdX);



                    object DhHeadId = c.GetReqData("DistrictHead", "DistHdId", "DistHdUserId='" + DistUserId1 + "'");




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




                    switch (row["OBP_MaritalStatus"].ToString())
                    {
                        case "NO":
                            rdbSingle.Checked = true;
                            break;
                        case "Yes":
                            rdbMarried.Checked = true;
                            break;
                    }




                    //if (row["newsImage"] != DBNull.Value && row["newsImage"] != null && row["newsImage"].ToString() != "" && row["newsImage"].ToString() != "no-photo.png")
                    //{
                    //    nwsPhoto = "<img src=\"" + Master.rootPath + "upload/news/thumb/" + row["newsImage"].ToString() + "\" width=\"200\" />";
                    //    btnRemove.Visible = true;
                    //}
                    //else
                    //{
                    //    btnRemove.Visible = false;
                    //}



                    if (row["OBP_AddProof1"] != DBNull.Value && row["OBP_AddProof1"].ToString() != "" && row["OBP_AddProof1"].ToString() != null && row["OBP_AddProof1"].ToString() != "no-photo.png")
                    {
                        StringBuilder strAddr = new StringBuilder();


                        if (row["OBP_AddProof2"] != DBNull.Value && row["OBP_AddProof2"].ToString() != "" && row["OBP_AddProof2"].ToString() != null && row["OBP_AddProof2"].ToString() != "no-photo.png")
                        {
                            strAddr.Append("<div style=\"width:50%; float:right;\">");
                            strAddr.Append("<div style=\"padding:5px;\">");

                            strAddr.Append("<a href=\""+Master.rootPath+"/upload/gobpData/addressProof/" + row["OBP_AddProof1"].ToString() + "\" target=\"_blank\" title=\"View Larger\" ><img src=\"../upload/gobpData/addressProof/" + row["OBP_AddProof1"].ToString() + "\" style=\"width:100%;\" /></a>");


                            //strAddr.Append("<a href=\"../upload/gobpData/addressProof/" + row["OBP_AddProof1"].ToString() + "\" target=\"_blank\" title=\"View Larger\" ><img src=\"../upload/gobpData/addressProof/" + row["OBP_AddProof1"].ToString() + "\" style=\"width:100%;\" /></a>");
                            strAddr.Append("</div>");
                            strAddr.Append("</div>");
                            strAddr.Append("<div style=\"width:50%; float:right;\">");
                            strAddr.Append("<div style=\"padding:5px;\">");
                            //strAddr.Append("<a href=\"../upload/gobpData/addressProof/" + row["OBP_AddProof2"].ToString() + "\" target=\"_blank\" title=\"View Larger\" ><img src=\"../upload/gobpData/addressProof/" + row["OBP_AddProof2"].ToString() + "\" style=\"width:100%;\" /></a>");

                            strAddr.Append("<a href=\"" + Master.rootPath + "/upload/gobpData/addressProof/" + row["OBP_AddProof2"].ToString() + "\" target=\"_blank\" title=\"View Larger\" ><img src=\"../upload/gobpData/addressProof/" + row["OBP_AddProof2"].ToString() + "\" style=\"width:100%;\" /></a>");

                            strAddr.Append("</div>");
                            strAddr.Append("</div>");
                            strAddr.Append("<div class=\"float_clear\"></div>");
                        }
                        else
                        {
                            //strAddr.Append("<a href=\"../upload/gobpData/addressProof/" + row["OBP_AddProof1"].ToString() + "\" target=\"_blank\" title=\"View Larger\" ><img src=\"../upload/gobpData/addressProof/" + row["OBP_AddProof1"].ToString() + "\" style=\"width:50%;\" /></a>");

                            strAddr.Append("<a href=\"" + Master.rootPath + "/upload/gobpData/addressProof/" + row["OBP_AddProof1"].ToString() + "\" target=\"_blank\" title=\"View Larger\" ><img src=\"../upload/gobpData/addressProof/" + row["OBP_AddProof1"].ToString() + "\" style=\"width:100%;\" /></a>");
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


                    if (row["OBP_OwnerEdu"] != DBNull.Value && row["OBP_OwnerEdu"] != null && row["OBP_OwnerEdu"].ToString() != "")
                    {
                        if (row["OBP_OwnerEdu"].ToString() == "10 - 12th")
                        {
                            rdbEduTenth.Checked = true;
                        }
                        if (row["OBP_OwnerEdu"].ToString() == "Graduate")
                        {
                            rdbEduGraduate.Checked = true;
                        }
                        if (row["OBP_OwnerEdu"].ToString() == "Post Graduate")
                        {
                            rdbEduPG.Checked = true;
                        }
                        if (row["OBP_OwnerEdu"].ToString() == "Other")
                        {
                            rdbEduGraduate.Checked = true;
                        }
                    }


                    if (row["OBP_ResidenceFrom"] != DBNull.Value && row["OBP_ResidenceFrom"] != null && row["OBP_ResidenceFrom"].ToString() != "")
                    {
                        if (row["OBP_ResidenceFrom"].ToString() == "0 - 5 Years")
                        {
                            rdbBelow5Yr.Checked = true;
                        }
                        if (row["OBP_ResidenceFrom"].ToString() == "5 - 10 Years")
                        {
                            rdb5Yr.Checked = true;
                        }
                        if (row["OBP_ResidenceFrom"].ToString() == "More than 10 Years")
                        {
                            rdb10Yr.Checked = true;
                        }
                    }

                }
            }
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



            if (txtAdd.Text == "" || txtAmount.Text == "" || txtEmail.Text == "" || txtMobile.Text == "" || txtName.Text == "" || txtTrDate.Text == "" || txtUTR.Text == "" || txtWpNo.Text == "" || ddrState.SelectedValue == "0" || ddrDistrict.SelectedValue == "0" || ddrCity.SelectedItem.Text == "")
            {
               // GetOBPData(Convert.ToInt32(Session["adminObp"]));

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * marked fields are mendatory');", true);
                // validateEmptyFields();
                return;
            }


            if (rdbProprietor.Checked == false && rdbPartner.Checked == false && rdbTrust.Checked == false && rdbOther.Checked == false)
            {
                errMsg = c.ErrNotification(2, "Select Firm Type");
                return;
            }

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


            if (!c.EmailAddressCheck(txtEmail.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid Email Address');", true);
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



            if (txtAmount.Text != "")
            {
                if (!c.IsNumeric(txtAmount.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Amount must be numeric value');", true);
                    return;
                }
            }



            int maxId = Convert.ToInt32(Request.QueryString["id"]);

            string profilePic = "";
            if (fuprofilePic.HasFile)
            {
                string extension = System.IO.Path.GetExtension(fuprofilePic.FileName);
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
                string extension = System.IO.Path.GetExtension(fuAddProof.FileName);
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
                string extension = System.IO.Path.GetExtension(fuAddProof1.FileName);
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
                string extension = System.IO.Path.GetExtension(fulIdProof.FileName);
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
                string extension = System.IO.Path.GetExtension(fulIdProof1.FileName);
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
                string extension = System.IO.Path.GetExtension(fuResume.FileName);
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

            string encrId = c.EncryptData(txtEmail.Text + maxId.ToString());



            c.ExecuteQuery("Update OBPData set OBP_TypeFirm='" + gobpType +
                "', OBP_ApplicantName='" + txtName.Text + "',  OBP_MaritalStatus='" + marital + "',  OBP_EmailId='" + txtEmail.Text + "', OBP_MobileNo='" + txtMobile.Text +
                "', OBP_WhatsApp='" + txtWpNo.Text + "', OBP_Address='" + txtAdd.Text + "', OBP_StateID=" + ddrState.SelectedValue +
                ", OBP_DistrictID=" + ddrDistrict.SelectedValue + ", OBP_City='" + ddrCity.SelectedItem.Text +
                "', OBP_OwnerEdu='" + gobpEd + "', OBP_OwnerOccup='" + txtownrOccuption.Text + "', OBP_LegalMatter='" + anyLegal +
                "', OBP_ResidenceFrom='" + resFrom + "', OBP_UTRNum='" + txtUTR.Text + "',  OBP_BankName='" + txtBank.Text +
                "',   OBP_TransDate='" + trDate +
                "', OBP_AccHolder='" + txtHolderName.Text + "', OBP_PaidAmt=" + txtAmount.Text + ", OBP_ShopName='" + txtshopName.Text +
                "' Where OBP_ID=" + Session["adminObp"]);



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

           

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'GOBP profile udded successfully');", true);

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('dashboard.aspx', 2000);", true);

            //ddrState.SelectedIndex = ddrDistrict.SelectedIndex = ddrCity.SelectedIndex = 0;
            //txtAdd.Text = txtAmount.Text = txtBank.Text = txtEmail.Text = txtHolderName.Text = txtMobile.Text = "";
            //txtName.Text = txtownrOccuption.Text = txtTrDate.Text = txtUTR.Text = txtWpNo.Text = txtshopName.Text = "";

            //rdb10Yr.Checked = rdb5Yr.Checked = rdbBelow5Yr.Checked = rdbEduGraduate.Checked = rdbEduOther.Checked = rdbEduPG.Checked = false;
            //rdbEduTenth.Checked = rdbMarried.Checked = rdbMatterNo.Checked = rdbMatterYes.Checked = rdbOther.Checked = rdbPartner.Checked = false;
            //rdbProprietor.Checked = rdbSingle.Checked = rdbTrust.Checked = false;


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
        Response.Redirect("dashboard.aspx");
        //if (Request.QueryString["type"] != null)
        //{
        //    Response.Redirect("gobp-registration-master.aspx?type=" + Request.QueryString["type"], false);
        //}
        //else
        //{
        //    Response.Redirect("gobp-registration-master.aspx", false);
        //}
    }



}