using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;

public partial class obp_add_gobp : System.Web.UI.Page
{
    iClass c = new iClass();
    public string rootPath, profilepic, resume, addproof, addproof1, idproof, idproof1;
    protected void Page_Load(object sender, EventArgs e)
    {

        btnSubmit.Attributes.Add("onclick", "this.disabled=true; this.value='Processing...';" + ClientScript.GetPostBackEventReference(btnSubmit, null) + ";");
        btnCancel.Attributes.Add("onclick", "this.disabled=true; this.value='Processing...';" + ClientScript.GetPostBackEventReference(btnCancel, null) + ";");

        if (!IsPostBack)
        {
            //c.FillComboBox("DistHdName", "DistHdId", "DistrictHead", "DelMark=0 AND IsOrgDH=1 AND IsMLM=1", "DistHdName", 0, ddrDH);
            c.FillComboBox("StateName", "StateID", "StatesData", "StateID>0", "StateName", 0, ddrState);

            int obpNumber = c.NextId("OBPData", "OBP_ID");
            //txtUserName.Text = "OBP" +  obpNumber.ToString("D5");
            //txtObpName.Focus();

            if (Request.QueryString["action"] != null)
            {
                editdata.Visible = true;
                viewdata.Visible = false;

                if (Request.QueryString["action"] == "new")
                {
                    btnSubmit.Text = "Save Info";
                    joinlevel.Visible = false;
                }
                else
                {
                    btnSubmit.Text = "Modify Info";
                    GetGobpData(Convert.ToInt32(Request.QueryString["id"]));
                }
            }
            else
            {
                viewdata.Visible = true;
                editdata.Visible = false;
                FillGrid();
            }

            // Define variables
            int count = 15;
            int selectedValue = 0;

            // Loop through and populate dropdown with count number of items
            for (int i = 1; i <= count; i++)
            {
                ddrJoinLevel.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            // Set first item to "Show All" if selected value is 0
            if (selectedValue == 0)
            {
                ddrJoinLevel.Items[0].Text = "Show All";
            }
        }
    }


    //protected void ddrDH_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    // Event handling code goes here
    //    //if(ddrDH.SelectedIndex > 0)
    //    //{
    //    //    txtDHUserName.Text = c.GetReqData("DistrictHead", "DistHdUserId", "DistHdId=" + Convert.ToInt32(ddrDH.SelectedValue)).ToString();
    //    //}

    //}

    private void validateEmptyFields()
    {
        ClientScript.RegisterStartupScript(GetType(), "HighlightValidation", @"
                    function highlightInvalidInputs() {
                        
                        var inputs = document.querySelectorAll('.required-input');
                        for (var i = 0; i < inputs.length; i++) {
                            var input = inputs[i];
                            if (input.value.trim() === '') {
                                input.classList.add('invalid');
                            }
                        }
                    }

                    highlightInvalidInputs();
                ", true);
    }
    protected void ddrState_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Event handling code goes here
        if (ddrState.SelectedIndex > 0)
        {
            c.FillComboBox("DistrictName", "DistrictId", "DistrictsData", "StateId=" + Convert.ToInt32(ddrState.SelectedValue), "DistrictName", 0, ddrDistrict);
        }

    }

    private void FillGrid()
    {
        try
        {
            // Get Parent / Referral OBP Name with SESSION Id
            string myOBPUserId = c.GetReqData("OBPData", "OBP_UserID", "OBP_ID=" + Convert.ToInt32(Session["adminObp"])).ToString();

            int myJoinCount = Convert.ToInt32(ddrJoinLevel.SelectedValue);

            string strQuery = "";

            if (myJoinCount > 0)
            {
                strQuery = @"Select OBP_ID, OBP_UserID, OBP_ApplicantName, OBP_MobileNo, OBP_City, OBP_StatusFlag, OBP_JoinLevel From OBPData Where OBP_DelMark=0 AND OBP_Ref_UserId='" + myOBPUserId + "' AND OBP_JoinLevel=" + myJoinCount;
            }
            else
            {
                strQuery = @"Select OBP_ID, OBP_UserID, OBP_ApplicantName, OBP_MobileNo, OBP_City, OBP_StatusFlag, OBP_JoinLevel From OBPData Where OBP_DelMark=0 AND OBP_Ref_UserId='" + myOBPUserId + "'";
            }

            using (DataTable dtProduct = c.GetDataTable(strQuery))
            {
                gvAddGOBP.DataSource = dtProduct;
                gvAddGOBP.DataBind();
                if (dtProduct.Rows.Count > 0)
                {
                    gvAddGOBP.UseAccessibleHeader = true;
                    gvAddGOBP.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
           
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FillGrid", ex.Message.ToString());
            return;
        }
    }

    protected void gvAddGOBP_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"gobp-info.aspx?action=edit&id=" + e.Row.Cells[0].Text + "\" class=\"gView\" title=\"View/Edit\"></a>";

                //litAnch.Text = "<a href=\"add-gobp.aspx?action=edit&id=" + e.Row.Cells[0].Text + "\" class=\"gView\" title=\"View/Edit\"></a>";
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvAddGOBP_RowDataBound", ex.Message.ToString());
            return;
        }
    }


    private void GetGobpData(int OBPIdx)
    {
        try
        {
            using (DataTable dtProduct = c.GetDataTable("Select * From OBPData Where OBP_ID=" + OBPIdx))
            {
                if (dtProduct.Rows.Count > 0)
                {
                    DataRow bRow = dtProduct.Rows[0];
                    lblId.Text = OBPIdx.ToString();

                    txtAdd.Text = bRow["OBP_Address"].ToString();
                   // txtAge.Text = bRow["OBP_Age"].ToString();
                    txtAmount.Text = bRow["OBP_PaidAmt"].ToString();
                    txtBank.Text = bRow["OBP_BankName"].ToString();
                   // txtBirthDate.Text = Convert.ToDateTime(bRow["OBP_BirthDate"]).ToString("dd/MM/yyyy");
                    txtEmail.Text = bRow["OBP_EmailId"].ToString();
                    txtHolderName.Text = bRow["OBP_AccHolder"].ToString();
                    txtMobile.Text = bRow["OBP_MobileNo"].ToString();
                    txtName.Text = bRow["OBP_ApplicantName"].ToString();
                    txtownrOccuption.Text = bRow["OBP_OwnerOccup"].ToString();
                    txtshopName.Text = bRow["OBP_ShopName"].ToString();
                    txtTrDate.Text = Convert.ToDateTime(bRow["OBP_TransDate"]).ToString("dd/MM/yyyy");
                    txtUTR.Text = bRow["OBP_UTRNum"].ToString();
                    txtWpNo.Text = bRow["OBP_WhatsApp"].ToString();


                    if (bRow["OBP_ProfilePic"] != DBNull.Value && bRow["OBP_ProfilePic"] != null && bRow["OBP_ProfilePic"].ToString() != "" && bRow["OBP_ProfilePic"].ToString() != "no-photo.png")
                    {
                        profilepic = "<img src=\"" + Master.rootPath + "upload/gobpData/profilePhoto/" + bRow["OBP_ProfilePic"].ToString() + "\" width=\"200\" />";

                    }

                    if (bRow["OBP_Resume"] != DBNull.Value && bRow["OBP_Resume"] != null && bRow["OBP_Resume"].ToString() != "" && bRow["OBP_Resume"].ToString() != "no-photo.png")
                    {
                        resume = "<a href=\"" + Master.rootPath + "upload/gobpData/resume/" + bRow["OBP_Resume"].ToString() + "\" target=\"_blank\">View Resume</a>";

                        //resume = "<img src=\"" + Master.rootPath + "upload/gobpData/profilePhoto/" + bRow["newsImage"].ToString() + "\" width=\"200\" />";

                    }

                    if (bRow["OBP_AddProof1"] != DBNull.Value && bRow["OBP_AddProof1"] != null && bRow["OBP_AddProof1"].ToString() != "" && bRow["OBP_AddProof1"].ToString() != "no-photo.png")
                    {
                        
                        addproof = "<img src=\"" + Master.rootPath + "upload/gobpData/addressProof/" + bRow["OBP_AddProof1"].ToString() + "\" width=\"200\" />";

                    }

                    if (bRow["OBP_AddProof2"] != DBNull.Value && bRow["OBP_AddProof2"] != null && bRow["OBP_AddProof2"].ToString() != "" && bRow["OBP_AddProof2"].ToString() != "no-photo.png")
                    {
                        
                        addproof1 = "<img src=\"" + Master.rootPath + "upload/gobpData/addressProof/" + bRow["OBP_AddProof2"].ToString() + "\" width=\"200\" />";

                    }

                    if (bRow["OBP_IDProof1"] != DBNull.Value && bRow["OBP_IDProof1"] != null && bRow["OBP_IDProof1"].ToString() != "" && bRow["OBP_IDProof1"].ToString() != "no-photo.png")
                    {
                        
                        idproof = "<img src=\"" + Master.rootPath + "upload/gobpData/idProof/" + bRow["OBP_IDProof1"].ToString() + "\" width=\"200\" />";

                    }


                    if (bRow["OBP_IDProof2"] != DBNull.Value && bRow["OBP_IDProof2"] != null && bRow["OBP_IDProof2"].ToString() != "" && bRow["OBP_IDProof2"].ToString() != "no-photo.png")
                    {
                        idproof1 = "<img src=\"" + Master.rootPath + "upload/gobpData/idProof/" + bRow["OBP_IDProof2"].ToString() + "\" width=\"200\" />";
                    }


                    if (bRow["OBP_TypeFirm"] != DBNull.Value && bRow["OBP_TypeFirm"] != null && bRow["OBP_TypeFirm"].ToString() != "")
                    {
                        if (bRow["OBP_TypeFirm"].ToString() == "Proprietor")
                        {
                            rdbProprietor.Checked = true;
                        }
                        if (bRow["OBP_TypeFirm"].ToString() == "Partner")
                        {
                            rdbPartner.Checked = true;
                        }
                        if (bRow["OBP_TypeFirm"].ToString() == "Trust")
                        {
                            rdbTrust.Checked = true;
                        }
                        if (bRow["OBP_TypeFirm"].ToString() == "Other")
                        {
                            rdbOther.Checked = true;
                        }

                    }


                    if (bRow["OBP_MaritalStatus"] != DBNull.Value && bRow["OBP_MaritalStatus"] != null && bRow["OBP_MaritalStatus"].ToString() != "")
                    {
                        if (bRow["OBP_MaritalStatus"].ToString() == "Single")
                        {
                            rdbSingle.Checked = true;
                        }
                        if (bRow["OBP_MaritalStatus"].ToString() == "Married")
                        {
                            rdbMarried.Checked = true;
                        }
                    }



                    if (bRow["OBP_OwnerEdu"] != DBNull.Value && bRow["OBP_OwnerEdu"] != null && bRow["OBP_OwnerEdu"].ToString() != "")
                    {
                        if (bRow["OBP_OwnerEdu"].ToString() == "10 - 12th")
                        {
                            rdbEduTenth.Checked = true;
                        }
                        if (bRow["OBP_OwnerEdu"].ToString() == "Graduate")
                        {
                            rdbEduGraduate.Checked = true;
                        }
                        if (bRow["OBP_OwnerEdu"].ToString() == "Post Graduate")
                        {
                            rdbEduPG.Checked = true;
                        }
                        if (bRow["OBP_OwnerEdu"].ToString() == "Other")
                        {
                            rdbEduGraduate.Checked = true;
                        }
                    }

                    if (bRow["OBP_LegalMatter"] != DBNull.Value && bRow["OBP_LegalMatter"] != null && bRow["OBP_LegalMatter"].ToString() != "")
                    {
                        if (bRow["OBP_LegalMatter"].ToString() == "Yes")
                        {
                            rdbMatterYes.Checked = true;
                        }
                        if (bRow["OBP_LegalMatter"].ToString() == "NO")
                        {
                            rdbMatterNo.Checked = true;
                        }
                    }

                    if (bRow["OBP_ResidenceFrom"] != DBNull.Value && bRow["OBP_ResidenceFrom"] != null && bRow["OBP_ResidenceFrom"].ToString() != "")
                    {
                        if (bRow["OBP_ResidenceFrom"].ToString() == "0 - 5 Years")
                        {
                            rdbBelow5Yr.Checked = true;
                        }
                        if (bRow["OBP_ResidenceFrom"].ToString() == "5 - 10 Years")
                        {
                            rdb5Yr.Checked = true;
                        }
                        if (bRow["OBP_ResidenceFrom"].ToString() == "More than 10 Years")
                        {
                            rdb10Yr.Checked = true;
                        }
                    }


                    //txtCity.Text = bRow["OBP_City"].ToString();
                    //txtDHUserName.Text = bRow["OBP_DH_UserId"].ToString();
                    //txtEmail.Text = bRow["OBP_EmailId"].ToString();
                    //txtMobile.Text = bRow["OBP_MobileNo"].ToString();
                    //txtObpName.Text = bRow["OBP_ApplicantName"].ToString();
                    //txtWhatsApp.Text = bRow["OBP_WhatsApp"].ToString();

                    object DhuserId = c.GetReqData("OBPData", "OBP_DH_UserId", "OBP_ID=" + OBPIdx);

                    object DhHeadId = c.GetReqData("DistrictHead", "DistHdId", "DistHdUserId='" + DhuserId + "'");

                    //  ddrDH.SelectedValue = DhHeadId.ToString();
                    ddrState.SelectedValue = bRow["OBP_StateID"].ToString();
                    c.FillComboBox("DistrictName", "DistrictId", "DistrictsData", "StateId=" + ddrState.SelectedValue, "DistrictName", 0, ddrDistrict);
                    ddrDistrict.SelectedValue = bRow["OBP_DistrictID"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetDoctorData", ex.Message.ToString());
            return;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            //Remove colon
            //txtObpName.Text = txtObpName.Text.Trim().Replace("'", "");
            //txtMobile.Text = txtMobile.Text.Trim().Replace("'", "");
            //txtEmail.Text = txtEmail.Text.Trim().Replace("'", "");
            //txtWhatsApp.Text = txtWhatsApp.Text.Trim().Replace("'", "");
            //txtCity.Text = txtCity.Text.Trim().Replace("'", "");

            //Empty frield validation
            //if (txtObpName.Text == "" || txtMobile.Text == "" || txtEmail.Text == "" || txtCity.Text == "" || txtWhatsApp.Text == "" || ddrDH.SelectedIndex == 0 | ddrState.SelectedIndex == 0 | ddrDistrict.SelectedIndex == 0)
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * Fields are mandatory');", true);
            //    return;
            //}

            txtshopName.Text = txtshopName.Text.Trim().Replace("'", "");
            txtName.Text = txtName.Text.Trim().Replace("'", "");
            //txtBirthDate.Text = txtBirthDate.Text.Trim().Replace("'", "");
           // txtAge.Text = txtAge.Text.Trim().Replace("'", "");
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

            if (txtAdd.Text == "" || txtAmount.Text == "" ||  txtEmail.Text == "" || txtMobile.Text == "" || txtName.Text == "" || txtTrDate.Text == "" || txtUTR.Text == "" || txtWpNo.Text == "" || ddrState.SelectedIndex == 0 || ddrDistrict.SelectedIndex == 0 || ddrCity.SelectedIndex == 0)
            {
                //errMsg = c.ErrNotification(2, "All * marked fields are mendatory");
                //return;

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * marked fields are mendatory');", true);
                validateEmptyFields();
                return;
            }


            if (rdbProprietor.Checked == false && rdbPartner.Checked == false && rdbTrust.Checked == false && rdbOther.Checked == false)
            {

                //ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select Firm Type.');", true);
                //return;
                rdbOther.Checked = true;
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


            if (c.ValidateMobile(txtMobile.Text) == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid Mobile No');", true);
                return;
            }
            if (c.EmailAddressCheck(txtEmail.Text) == false)
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

            //DateTime bDate = DateTime.Now;
            //if (txtBirthDate.Text != "")
            //{
            //    string[] arrDate = txtBirthDate.Text.Split('/');
            //    if (c.IsDate(arrDate[1] + "/" + arrDate[0] + "/" + arrDate[2]) == false)
            //    {
            //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter valid Date of Birth');", true);
            //        return;
            //    }
            //    else
            //    {
            //        bDate = Convert.ToDateTime(arrDate[1] + "/" + arrDate[0] + "/" + arrDate[2]);
            //    }
            //}

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

            //int age = 0;
            //if (txtAge.Text != "")
            //{
            //    if (!c.IsNumeric(txtAge.Text))
            //    {
            //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Age must be numeric value');", true);
            //        return;
            //    }
            //    else
            //    {
            //        age = Convert.ToInt16(txtAge.Text);
            //    }
            //}

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

            //Address , Id proof , Resume Upload Control
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
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Only .jpg, .jpeg, .png  files are allowed for profile pic');", true);
                    return;
                    //errMsg = c.ErrNotification(2, "Only .jpg, .jpeg, .png  files are allowed for profile pic");
                    //return;
                }
            }
            else
            {
                profilePic = "NA";
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
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Only .jpg, .jpeg, .png  files are allowed for Address proof');", true);
                    return;

                }
            }
            else
            {
                //ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Upload address proof');", true);
                //return;
                addrProof = "NA";
              

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
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Only .jpg, .jpeg, .png files are allowed for Address proof');", true);
                    return;
                    //errMsg = c.ErrNotification(2, "Only .jpg, .jpeg, .png files are allowed for Address proof");
                    //return;
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
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Only .jpg, .jpeg, .png files are allowed for Id proof');", true);
                    return;
                    //errMsg = c.ErrNotification(2, "Only .jpg, .jpeg, .png files are allowed for Id proof");
                    //return;
                }
            }
            else
            {
                //ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Upload ID proof');", true);
                //return;
                //errMsg = c.ErrNotification(2, "Upload ID proof");
                //return;
                addrProof1 = "NA";
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
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Only .jpg, .jpeg, .png  files are allowed for Id proof');", true);
                    return;
                    //errMsg = c.ErrNotification(2, "Only .jpg, .jpeg, .png  files are allowed for Id proof");
                    //return;
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
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Only .pdf  files are allowed for resume');", true);
                    return;
                    //errMsg = c.ErrNotification(2, "Only .pdf  files are allowed for resume");
                    //return;
                }
            }
            else
            {
                obpResume = "NA";
            }


            int dhdId = 0; int zhdId = 0;
            if (Request.QueryString["dhId"] != null)
                dhdId = Convert.ToInt32(Request.QueryString["dhId"]);

            if (Request.QueryString["zhId"] != null)
                zhdId = Convert.ToInt32(Request.QueryString["zhId"]);

            string shopcode = "OBP" + maxId.ToString("D5");

            // Get District-Head information
            string dhUserId = c.GetReqData("[dbo].[OBPData]", "[OBP_DH_UserId]", "[OBP_ID] = " + Session["adminObp"]).ToString();
            string dhUserName = c.GetReqData("[dbo].[OBPData]", "[OBP_DH_Name]", "[OBP_ID] = " + Session["adminObp"]).ToString();

            //Join Level

            object ObpUserid = c.GetReqData("OBPData", "OBP_UserID", "OBP_ID=" + Session["adminObp"]).ToString();

            object joinlevel = Convert.ToInt32(c.GetReqData("OBPData", "OBP_JoinLevel", "OBP_UserID='" + ObpUserid + "'"));
            int myJoinlevel = Convert.ToInt32(joinlevel);

            int obpjoinlevel = 0;

            if (myJoinlevel > 0)
            {
                obpjoinlevel = myJoinlevel + 1;
            }


            string myParentOBP = c.GetReqData("OBPData", "OBP_UserID", "OBP_ID=" + Convert.ToInt32(Session["adminObp"])).ToString();

            // string ObpUserId=  "OBP" + maxId.ToString("D5");

            //if (lblId.Text == "[New]")
            //{

                c.ExecuteQuery("Insert Into OBPData(OBP_ID, OBP_JoinDate, OBP_FKTypeID, OBP_TypeFirm, OBP_DH_UserId, OBP_DH_Name, " +
                " OBP_ApplicantName, OBP_MaritalStatus, OBP_EmailId, OBP_MobileNo, OBP_WhatsApp, OBP_Address, " +
                " OBP_StateID, OBP_DistrictID, OBP_City, OBP_OwnerEdu, OBP_OwnerOccup, OBP_LegalMatter, OBP_ResidenceFrom, OBP_UTRNum, OBP_BankName, OBP_TransDate, " +
                " OBP_AccHolder, OBP_PaidAmt, OBP_IsClosed, OBP_StatusFlag, OBP_DelMark, OBP_ShopName, OBP_UserPWD, OBP_UserID, OBP_JoinLevel, OBP_ProfilePic, OBP_AddProof1, OBP_IDProof1, OBP_Resume, OBP_Ref_UserId, IsMLM)Values(" + maxId +
                ",  '" + DateTime.Now + "', 1, '" + gobpType + "', '" + dhUserId + "', '" + dhUserName + "' ,'" + txtName.Text +
                "', '" + marital + "', '" + txtEmail.Text + "', '" + txtMobile.Text +
                "', '" + txtWpNo.Text + "', '" + txtAdd.Text + "', " + ddrState.SelectedValue + ", " + ddrDistrict.SelectedValue +
                ", '" + ddrCity.SelectedItem.Text + "', '" + gobpEd + "', '" + txtownrOccuption.Text + "', '" + anyLegal + "', '" + resFrom + "', '" + txtUTR.Text +
                "', '" + txtBank.Text + "', '" + trDate + "', '" + txtHolderName.Text + "', " + Convert.ToDouble(txtAmount.Text) + ", 0, " +
                " 'Pending', 0, '" + txtshopName.Text + "', '123456', '" + shopcode + "', " + obpjoinlevel + ",  '" + profilePic + "', '" + addrProof +
                "', '" + idProof + "', '" + obpResume + "',  '" + myParentOBP + "', 1)");


                if (fuAddProof.HasFile)
                {
                    string extension = System.IO.Path.GetExtension(fuAddProof.FileName);
                    string origPath = "~/upload/gobpData/addressProof/original/";
                    string normalPath = "~/upload/gobpData/addressProof/";

                    fuAddProof.SaveAs(Server.MapPath(origPath) + addrProof);
                    File.Copy(Server.MapPath(origPath) + addrProof, Server.MapPath(normalPath) + addrProof);

                    File.Delete(Server.MapPath(origPath) + addrProof);
                }

                if (fuAddProof1.HasFile)
                {
                    string extension = System.IO.Path.GetExtension(fuAddProof1.FileName);
                    string origPath = "~/upload/gobpData/addressProof/original/";
                    string normalPath = "~/upload/gobpData/addressProof/";

                    fuAddProof1.SaveAs(Server.MapPath(origPath) + addrProof1);
                    File.Copy(Server.MapPath(origPath) + addrProof1, Server.MapPath(normalPath) + addrProof1);

                    File.Delete(Server.MapPath(origPath) + addrProof1);

                    c.ExecuteQuery("Update OBPData Set OBP_AddProof2='" + addrProof1 + "' Where OBP_ID=" + maxId);
                }

                if (fulIdProof.HasFile)
                {
                    string extension = System.IO.Path.GetExtension(fulIdProof.FileName);
                    string origPath = "~/upload/gobpData/idProof/original/";
                    string normalPath = "~/upload/gobpData/idProof/";

                    fulIdProof.SaveAs(Server.MapPath(origPath) + idProof);

                    File.Copy(Server.MapPath(origPath) + idProof, Server.MapPath(normalPath) + idProof);

                    //c.ImageOptimizer(idProof, origPath, normalPath, 800, true);

                    File.Delete(Server.MapPath(origPath) + idProof);
                }

                if (fuAddProof1.HasFile)
                {
                    string extension = System.IO.Path.GetExtension(fuAddProof1.FileName);
                    string origPath = "~/upload/gobpData/idProof/original/";
                    string normalPath = "~/upload/gobpData/idProof/";

                    fuAddProof1.SaveAs(Server.MapPath(origPath) + idProof1);

                    File.Copy(Server.MapPath(origPath) + idProof1, Server.MapPath(normalPath) + idProof1);
                    File.Delete(Server.MapPath(origPath) + idProof1);

                    c.ExecuteQuery("Update OBPData Set OBP_IDProof2='" + idProof1 + "' Where OBP_ID=" + maxId);
                }

                OBP_Commission_Calc();

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'GOBP info added successfully');", true);
            //}
            //else
            //{
            //    c.ExecuteQuery("Update OBPData Set OBP_ApplicantName='" + txtObpName.Text + "', OBP_EmailId='" + txtEmailId.Text +
            //            "', OBP_MobileNo='" + txtMobile.Text + "', OBP_DH_Name='" + ddrDH.SelectedItem.Text +
            //            "', OBP_WhatsApp='" + txtWhatsApp.Text +
            //            "', OBP_StateID=" + ddrState.SelectedValue + ", OBP_DistrictID=" + ddrDistrict.SelectedValue +
            //            ", OBP_City='" + txtCity.Text + "' Where OBP_ID=" + maxObpId);
            //}




            // =========== Mobile Number duplication validation =========
            //if (lblId.Text == "[New]")
            //{
            //    if (c.IsRecordExist("Select OBP_ID From OBPData Where OBP_MobileNo='" + txtMobile.Text + "' AND OBP_DelMark=0"))
            //    {
            //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'This Mobile No is already registered');", true);
            //        return;
            //    }

            //    // GOBP User Id duplication check
            //    //if (c.IsRecordExist("Select OBP_ID From OBPData Where OBP_UserID='" + txtUserName.Text + "' AND OBP_DelMark=0"))
            //    //{
            //    //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'This GOBP User Id is already exist.');", true);
            //    //    return;
            //    //}
            //}
            //else
            //{
            //    if (c.IsRecordExist("Select OBP_ID From OBPData Where OBP_MobileNo='" + txtMobile.Text + "' AND OBP_DelMark=0 AND OBP_ID<>" + lblId.Text))
            //    {
            //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'This Mobile No is already registered');", true);
            //        return;
            //    }
            //}

            // ============= New data insertion ===============
            //   int maxObpId = lblId.Text == "[New]" ? c.NextId("OBPData", "OBP_ID") : Convert.ToInt32(lblId.Text);

            // Get Parent / Referral OBP Name with SESSION Id
            //string myParentOBP = c.GetReqData("OBPData", "OBP_UserID", "OBP_ID=" + Convert.ToInt32(Session["adminObp"])).ToString();

            // Again extract fresh OBP-UserId number
            //int obpNumber = c.NextId("OBPData", "OBP_ID");
            //txtUserName.Text = "OBP" + obpNumber.ToString("D5");




            //if (lblId.Text == "[New]")
            //{
            //    string queryInsert = @"Insert into OBPData(OBP_ID, OBP_JoinDate, OBP_FKTypeID, OBP_ApplicantName, OBP_EmailId, OBP_MobileNo, 
            //    OBP_UserID, OBP_UserPWD, OBP_StatusFlag, OBP_DelMark, OBP_DH_UserId, OBP_DH_Name, OBP_WhatsApp, OBP_StateID, OBP_DistrictID,
            //    OBP_City, OBP_Ref_UserId, IsMLM, OBP_JoinLevel)
            //    Values(" + maxObpId + ", '" + DateTime.Now + "', 2, '" + txtObpName.Text + "', '" + txtEmailId.Text + "', '" + txtMobile.Text + "', " +
            //    "'" + txtUserName.Text + "', '123456', 'Active', 0, '" + txtDHUserName.Text + "', '" + ddrDH.SelectedItem.Text + "', '" + txtWhatsApp.Text + "', " 
            //    + ddrState.SelectedValue + ", " + ddrDistrict.SelectedValue + ", '" + txtCity.Text + "', '" + myParentOBP + "', 1, "+ obpjoinlevel + ")";

            //    c.ExecuteQuery(queryInsert);

            //    // Add OBP-Commission table entry for Recruit-Direct

            //}

            //else
            //{
            //    c.ExecuteQuery("Update OBPData Set OBP_ApplicantName='" + txtObpName.Text + "', OBP_EmailId='" + txtEmailId.Text +
            //        "', OBP_MobileNo='" + txtMobile.Text + "', OBP_DH_Name='" + ddrDH.SelectedItem.Text +
            //        "', OBP_WhatsApp='" + txtWhatsApp.Text +
            //        "', OBP_StateID=" + ddrState.SelectedValue + ", OBP_DistrictID=" + ddrDistrict.SelectedValue +
            //        ", OBP_City='" + txtCity.Text + "' Where OBP_ID=" + maxObpId);

            //  ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'GOBP info updated successfully');", true);
            //}

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('add-gobp.aspx', 2000);", true);

            //txtCity.Text = txtDHUserName.Text = txtEmailId.Text = txtMobile.Text = txtObpName.Text = txtUserName.Text = txtWhatsApp.Text = "";
        }

        catch (Exception ex)
        {
            c.ErrorLogHandler(this.ToString(), "btnSubmit_Click", ex.Message.ToString());
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error occured while processing.');", true);
        }
    }

    private void OBP_Commission_Calc()
    {
        try
        {
            string obpUserId = c.GetReqData("OBPData", "OBP_UserID", "OBP_ID").ToString();

            // ======= Level-1 Own Commission =======

            int maxCommId = c.NextId("OBPCommission", "ObpComId");
            int myObpId = Convert.ToInt32(Session["adminObp"]);
            string myObpUserID = c.GetReqData("OBPData", "OBP_UserID", "OBP_ID=" + Convert.ToInt32(Session["adminObp"])).ToString();
            int maxId = c.NextId("OBPData", "OBP_ID");
            int myParentOBP_ID = 0;
            int myGrandParentOBP_ID = 0;
            string shopcode = "OBP" + maxId.ToString("D5");
            // ======= Level-1 Commission =======
            string queryInsert = @"Insert into OBPCommission(ObpComId, ObpComDate, ObpComType, ObpUserId, FK_Obp_ID, ObpRefUserId, ObpComLevel, ObpComPercent, ObpComAmount)
                Values(" + maxCommId + ", '" + DateTime.Now + "', 'Recruit-Direct', '" + myObpUserID + "', " + myObpId + ", '" + shopcode + "', 1, 20, 6000)";

            c.ExecuteQuery(queryInsert);

            // ======= Level-2 Parent's Commission =======
            // Check if any PARENT OBP present for this Session-OBP
            object parentOBP = c.GetReqData("OBPData", "OBP_Ref_UserId", "OBP_ID=" + Convert.ToInt32(Session["adminObp"]));

            if (parentOBP != null)
            {
                myParentOBP_ID = Convert.ToInt32(c.GetReqData("OBPData", "OBP_ID", "OBP_UserID='" + parentOBP.ToString() + "'"));

                maxCommId = c.NextId("OBPCommission", "ObpComId");
                queryInsert = @"Insert into OBPCommission(ObpComId, ObpComDate, ObpComType, ObpUserId, FK_Obp_ID, ObpRefUserId, ObpComLevel, ObpComPercent, ObpComAmount)
                Values(" + maxCommId + ", '" + DateTime.Now + "', 'Recruit-Ref', '" + parentOBP.ToString() + "', " + myParentOBP_ID + ", '" + shopcode + "', 2, 10, 3000)";

                c.ExecuteQuery(queryInsert);
            }

            // ======= Level-3 Grand-Parent's Commission =======
            if (parentOBP != null)
            {
                // Check if any GRAND-PARENT OBP present for my Parent-OBP
                object GrandParentOBP = c.GetReqData("OBPData", "OBP_Ref_UserId", "OBP_ID=" + myParentOBP_ID);

                if (GrandParentOBP != null)
                {
                    myGrandParentOBP_ID = Convert.ToInt32(c.GetReqData("OBPData", "OBP_ID", "OBP_UserID='" + GrandParentOBP.ToString() + "'"));

                    maxCommId = c.NextId("OBPCommission", "ObpComId");
                    queryInsert = @"Insert into OBPCommission(ObpComId, ObpComDate, ObpComType, ObpUserId, FK_Obp_ID, ObpRefUserId, ObpComLevel, ObpComPercent, ObpComAmount)
                    Values(" + maxCommId + ", '" + DateTime.Now + "', 'Recruit-Ref', '" + GrandParentOBP.ToString() + "', " + myGrandParentOBP_ID + ", '" + shopcode + "', 3, 5, 1500)";

                    c.ExecuteQuery(queryInsert);
                }

            }

        }
        catch (Exception ex)
        {
            //errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("add-gobp.aspx");

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
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Please Select district');", true);
                return;
                //errMsg = c.ErrNotification(2, "Please Select district");
                //return;
            }
        }
        catch (Exception ex)
        {
            c.ErrorLogHandler(this.ToString(), "btnSubmit_Click", ex.Message.ToString());
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error occured while processing.');", true);
        }
    }

    protected void btnCancel_Click1(object sender, EventArgs e)
    {
        Response.Redirect("add-gobp.aspx");
    }

    protected void BtnShow_Click(object sender, EventArgs e)
    {
        try
        {
            FillGrid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSave_Click", ex.Message.ToString());
            return;
        }
    }
}