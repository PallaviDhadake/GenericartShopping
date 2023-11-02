using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class register_genmitra : System.Web.UI.Page
{
    iClass c = new iClass();
    public string pgTitle, errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        btnRegister.Attributes.Add("onclick", "this.disabled=true; this.value='Processing...';" + ClientScript.GetPostBackEventReference(btnRegister, null) + ";");
        if (!IsPostBack)
        {
            txtName.Focus();
            // Fill State
            c.FillComboBox("StateName", "StateID", "StatesData", "", "StateName", 0, ddrState);
        }
    }

    protected void ddrState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // Fill District
            c.FillComboBox("DistrictName", "DistrictId", "DistrictsData", "StateId=" + ddrState.SelectedValue + "", "DistrictName", 0, ddrDistrict);
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
            // Fill City
            c.FillComboBox("CityName", "CityID", "CityData", "FK_DistId=" + ddrDistrict.SelectedValue + "", "CityName", 0, ddrCity);
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    protected void btnRegister_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtName.Text == "" || txtMobile.Text == "" || txtEmail.Text == "" || txtBankName.Text == "" || ddrState.SelectedIndex == 0 || ddrDistrict.SelectedIndex == 0 )
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * fields are mandatory');", true);
                return;
            }
            //if (c.IsRecordExist("Select GMitraID From GenericMitra Where GMitraMobile='" + txtMobile.Text + "' AND GMitraStatus=1"))
            if (c.IsRecordExist("Select GMitraID From GenericMitra Where GMitraMobile='" + txtMobile.Text + "' AND GMitraStatus<>3"))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'User with this mobile no Is Already Registered');", true);
                return;
            }
            //if (c.IsRecordExist("Select GMitraID From GenericMitra Where GMitraEmail='" + txtEmail.Text + "' AND GMitraStatus=1"))
            if (c.IsRecordExist("Select GMitraID From GenericMitra Where GMitraEmail='" + txtEmail.Text + "' AND GMitraStatus<>3"))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'User with this email id Is Already Registered');", true);
                return;
            }
            int maxId =  c.NextId("GenericMitra", "GMitraID");

            string frshopCode = "";
            if (Request.QueryString["frId"] != null)
            {
                frshopCode = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + Request.QueryString["frId"]).ToString();
            }

            string origImgPath = "~/upload/genmitradocs/";
            // Pan card
            string panName = "";
            if (fuPan.HasFile)
            {
                string fExt = Path.GetExtension(fuPan.FileName).ToString().ToLower();
                if (fExt == ".jpg" || fExt == ".jpeg" || fExt == ".png" || fExt == ".pdf")
                {
                    panName = "pancard-" + maxId + "-" + DateTime.Now.ToString("ddmmyyyyHHmmss") + fExt;
                    //FileInfo pan = new FileInfo(fuPan.FileName);
                    if (fuPan.PostedFile.ContentLength > 1000000) // file size in bytes (1 MB)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Pan card file size must be less than 1MB');", true);
                        return;
                    }
                    //fuPan.SaveAs(Server.MapPath(origImgPath) + panName);
                    fuPan.SaveAs(Server.MapPath(origImgPath) + panName);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Only .jpg, .jpeg, .png or .pdf files are allowed');", true);
                    return;
                }
            }
            //else
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select Pan Card Copy to upload');", true);
            //    return;
            //}
            // Adhar card
            string adharName = "";
            if (fuAdhar.HasFile)
            {
                string fExt = Path.GetExtension(fuAdhar.FileName).ToString().ToLower();
                if (fExt == ".jpg" || fExt == ".jpeg" || fExt == ".png" || fExt == ".pdf")
                {
                    adharName = "adharcard-" + maxId + "-" + DateTime.Now.ToString("ddmmyyyyHHmmss") + fExt;
                    //FileInfo adhar = new FileInfo(fuAdhar.FileName);
                    if (fuAdhar.PostedFile.ContentLength > 1000000) // file size in bytes (1 MB)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Adhar card file size must be less than 1MB');", true);
                        return;
                    }
                    //fuAdhar.SaveAs(Server.MapPath(origImgPath) + adharName);
                    fuAdhar.SaveAs(Server.MapPath(origImgPath) + adharName);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Only .jpg, .jpeg, .png or .pdf files are allowed');", true);
                    return;
                }
            }
            //else
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select Adhar Card Copy to upload');", true);
            //    return;
            //}
            // Bank doc
            string bankDoc = "";
            if (fuPassbook.HasFile)
            {
                string fExt = Path.GetExtension(fuPassbook.FileName).ToString().ToLower();
                if (fExt == ".jpg" || fExt == ".jpeg" || fExt == ".png" || fExt == ".pdf")
                {
                    bankDoc = "bankdoc-" + maxId + "-" + DateTime.Now.ToString("ddmmyyyyHHmmss") + fExt;
                    FileInfo doc = new FileInfo(fuPassbook.FileName);
                    if (fuPassbook.PostedFile.ContentLength > 1000000) // file size in bytes (1 MB)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Cheque/Passbook file size must be less than 1MB');", true);
                        return;
                    }
                    //fuPassbook.SaveAs(Server.MapPath(origImgPath) + bankDoc);
                    fuPassbook.SaveAs(Server.MapPath(origImgPath) + bankDoc);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Only .jpg, .jpeg, .png or .pdf files are allowed');", true);
                    return;
                }
            }
            //else
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select Cheque/Passbook Copy to upload');", true);
            //    return;
            //}


            DateTime cDate = DateTime.Now;
            string currentDate = cDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string imgName = "no-photo.png";
            string userId = txtMobile.Text;
            string password = "123456";

            int GMitraStatus = Request.QueryString["frId"] != null ? 1 : 0;

            c.ExecuteQuery("Insert Into GenericMitra (GMitraID, GMitraDate, GMitraName, GMitraMobile, GMitraEmail, GMitraPhoto, GMitraBankName, " +
                " GMitraBankAccName, GMitraBankAccNumber, GMitraBankIFSC, GMitraPanCard, GMitraLogin, GMitraPassword, FK_StateID, FK_DistrictID, " +
                " FK_CityID, GMitraStatus, GMitraShopCode) Values( " + maxId + ", '" + currentDate + "', '" + txtName.Text +
                "', '" + txtMobile.Text + "', '" + txtEmail.Text + "', '" + imgName + "', '" + txtBankName.Text + "', '" + txtAccName.Text +
                "', '" + txtAccNo.Text + "', '" + txtIfsc.Text + "', '" + txtPan.Text + "', '" + userId + "', '" + password +
                "', " + ddrState.SelectedValue + ", " + ddrDistrict.SelectedValue + ", " + ddrCity.SelectedValue + ", " + GMitraStatus +
                ", '" + frshopCode + "' ) ");

            if (fuPan.HasFile)
            {
                //fuPan.SaveAs(Server.MapPath(origImgPath) + panName);
                //fuAdhar.SaveAs(Server.MapPath(origImgPath) + adharName);
                //fuPassbook.SaveAs(Server.MapPath(origImgPath) + bankDoc);

                c.ExecuteQuery("Update GenericMitra Set GMitraPan='" + panName + "' Where GMitraID=" + maxId);
            }

            if (fuAdhar.HasFile)
            {
                c.ExecuteQuery("Update GenericMitra Set GMitraAdhar='" + adharName + "' Where GMitraID=" + maxId);
            }

            if (fuPassbook.HasFile)
            {
                c.ExecuteQuery("Update GenericMitra Set GMitraBankDoc='" + bankDoc + "' Where GMitraID=" + maxId);
            }


            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Registration Successfull..');", true);

            //string url = Master.rootPath + "register-genmitra";
            string url = Master.rootPath;

            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('" + url + "') }, 1500);", true);


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnRegister_Click", ex.Message.ToString());
            return;
        }
    }

   
}