using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class franchisee_edit_profile : System.Web.UI.Page
{
    iClass c = new iClass();
    public string pgTitle, adharPhoto, panPhoto;
    protected void Page_Load(object sender, EventArgs e)
    {
        txtFrAddress.Attributes.Add("Maxlength", "300");
        pgTitle = "Edit your profile";
        if (!IsPostBack)
        {
            c.FillComboBox("StateName", "StateID", "StatesData", "FK_CountryID=101", "StateName", 0, ddrFrState);
            int frIdx =Convert.ToInt32(Session["adminFranchisee"].ToString());
            GetFranchiseeData(frIdx);
        }
       
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {

            if (txtFrName.Text == "" || txtFrOwnerName.Text == "" || txtFrPincode.Text == "" || ddrFrState.SelectedIndex == 0 || txtFrEmail.Text == "" || txtFrMobile.Text == "" || txtFrAddress.Text == "" )
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * Fields are mandatory');", true);
                return;
            }

            if (c.ValidateMobile(txtFrMobile.Text) == false)
            {

                //8007934383
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter valid mobile no');", true);
                return;
            }

            if(c.EmailAddressCheck(txtFrEmail.Text) == false)
            {
                //ujwalswastgen2016@gmail.com
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter valid Email address');", true);
                return;
            }

            int frIdx = Convert.ToInt32(Session["adminFranchisee"].ToString());
            string adharImgName = "";
            string panImgName = "";
            if (adharImg.HasFile)
            {
                string fExt = Path.GetExtension(adharImg.FileName).ToString().ToLower();
                if (fExt == ".jpg" || fExt == ".jpeg" || fExt == ".png")
                {
                    adharImgName = "adhaar-photo-" + frIdx + fExt;
                    adharImageUploadProcess(adharImgName);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Only .jpg, .jpeg or .png files are allowed');", true);
                    return;
                }
            }
            else
            {
                
            }

            if (panImg.HasFile)
            {
                string fExt = Path.GetExtension(panImg.FileName).ToString().ToLower();
                if (fExt == ".jpg" || fExt == ".jpeg" || fExt == ".png")
                {
                    panImgName = "pan-photo-" + frIdx + fExt;
                    panImageUploadProcess(panImgName);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Only .jpg, .jpeg or .png files are allowed');", true);
                    return;
                }
            }
            else
            {
               
            }

            c.ExecuteQuery("Update FranchiseeData Set FranchName='"+ txtFrName.Text + "', FranchOwnerName='"+ txtFrOwnerName.Text + "', FK_FranchStateId="+ ddrFrState.SelectedValue + ", FranchPinCode='"+ txtFrPincode.Text + "', FranchEmail='"+ txtFrEmail.Text + "', FranchMobile='"+ txtFrMobile.Text + "', FranchAddress='"+ txtFrAddress.Text + "', FranchBankName='"+ txtFrBankName.Text + "', FranchBankBranch='"+ txtBankBranch.Text + "', FranchBankAccName='"+ txtBankAccName.Text + "', FranchBankAccNum='"+ txtBankAccNo.Text + "', FranchBankIFSC='"+ txtBankIfsc.Text + "' Where FranchID="+ frIdx );
            if (adharImg.HasFile)
            {
                c.ExecuteQuery("Update FranchiseeData Set FranchAadharCard='" + adharImgName + "' Where FranchID=" + frIdx);
            }
            if (panImg.HasFile)
            {
                c.ExecuteQuery("Update FranchiseeData Set FranchPanCard='" + panImgName + "' Where FranchID=" + frIdx);
            }

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Frainchisee Info Updated');", true);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('dashboard.aspx', 2000);", true);
        }
        catch (Exception ex)
        {

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSave_Click", ex.Message.ToString());
            return;
        }
    }

    private void GetFranchiseeData(int frId)
    {
        try
        {
            using (DataTable dtFranchisee = c.GetDataTable("Select * From FranchiseeData Where FranchID="+ frId))
            {
                if (dtFranchisee.Rows.Count > 0)
                {
                    DataRow row = dtFranchisee.Rows[0];

                    txtFrName.Text = row["FranchName"].ToString();
                    txtFrCode.Text = row["FranchShopCode"].ToString();
                    txtFrDate.Text =Convert.ToDateTime(row["FranchRegDate"].ToString()).ToString("dd/MM/yyyy");
                    txtFrOwnerName.Text = row["FranchOwnerName"].ToString();
                    txtFrPincode.Text = row["FranchPinCode"].ToString();
                    txtFrEmail.Text = row["FranchEmail"].ToString();
                    txtFrMobile.Text = row["FranchMobile"].ToString();
                    txtFrAddress.Text = row["FranchAddress"].ToString();
                    txtFrBankName.Text = row["FranchBankName"] != DBNull.Value && row["FranchBankName"] != null && row["FranchBankName"].ToString() != "" ? row["FranchBankName"].ToString() : "-";
                    txtBankBranch.Text = row["FranchBankBranch"] != DBNull.Value && row["FranchBankBranch"] != null && row["FranchBankBranch"].ToString() != "" ? row["FranchBankBranch"].ToString() : "-";
                    txtBankAccName.Text = row["FranchBankAccName"] != DBNull.Value && row["FranchBankAccName"] != null && row["FranchBankAccName"].ToString() != "" ? row["FranchBankAccName"].ToString() : "-";
                    txtBankAccNo.Text = row["FranchBankAccNum"] != DBNull.Value && row["FranchBankAccNum"] != null && row["FranchBankAccNum"].ToString() != "" ? row["FranchBankAccNum"].ToString() : "-";
                    txtBankIfsc.Text = row["FranchBankIFSC"] != DBNull.Value && row["FranchBankIFSC"] != null && row["FranchBankIFSC"].ToString() != "" ? row["FranchBankIFSC"].ToString() : "-";
                    ddrFrState.SelectedValue = row["FK_FranchStateId"].ToString();

                    if (row["FranchAadharCard"] != DBNull.Value && row["FranchAadharCard"] != null && row["FranchAadharCard"].ToString() != "")
                    {
                        adharPhoto = "<img src=\"" + Master.rootPath + "upload/adhaarcard/thumb/" + row["FranchAadharCard"].ToString() + "\" width=\"200\" />";
                    }
                    if (row["FranchPanCard"] != DBNull.Value && row["FranchPanCard"] != null && row["FranchPanCard"].ToString() != "")
                    {
                        panPhoto = "<img src=\"" + Master.rootPath + "upload/pancard/thumb/" + row["FranchPanCard"].ToString() + "\" width=\"200\" />";
                    }

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetFranchiseeData", ex.Message.ToString());
            return;
        }
    }

    private void adharImageUploadProcess(string imgName)
    {
        try
        {
            string origImgPath = "~/upload/";
            string normalImgPath = "~/upload/adhaarcard/";
            string thumbImgPath = "~/upload/adhaarcard/thumb/";

            adharImg.SaveAs(Server.MapPath(origImgPath) + imgName);

            c.ImageOptimizer(imgName, origImgPath, normalImgPath, 800, true);
            c.ImageOptimizer(imgName, normalImgPath, thumbImgPath, 400, true);

            //Delete rew image from server
            File.Delete(Server.MapPath(origImgPath) + imgName);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "adharImageUploadProcess", ex.Message.ToString());
            return;
        }
    }
    private void panImageUploadProcess(string imgName)
    {
        try
        {
            string origImgPath = "~/upload/";
            string normalImgPath = "~/upload/pancard/";
            string thumbImgPath = "~/upload/pancard/thumb/";

            panImg.SaveAs(Server.MapPath(origImgPath) + imgName);

            c.ImageOptimizer(imgName, origImgPath, normalImgPath, 800, true);
            c.ImageOptimizer(imgName, normalImgPath, thumbImgPath, 400, true);

            //Delete rew image from server
            File.Delete(Server.MapPath(origImgPath) + imgName);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "panImageUploadProcess", ex.Message.ToString());
            return;
        }
    }
}