using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class genericmitra_edit_profile : System.Web.UI.Page
{
    public string disImg, pancardImg, adharcardImg, bankdocImg;
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            // Fill State
            c.FillComboBox("StateName", "StateID", "StatesData", "", "StateName", 0, ddrState);
            GetGeneriMitraData(Convert.ToInt32(Session["adminGenMitra"]));
        }
    }

    private void GetGeneriMitraData(int gmIdx)
    {
        try
        {
            using (DataTable dtGenMitra = c.GetDataTable("Select * From GenericMitra Where GMitraID=" + gmIdx + ""))
            {
                if (dtGenMitra.Rows.Count > 0)
                {
                    DataRow row = dtGenMitra.Rows[0];
                    lblId.Text = gmIdx.ToString();

                    txtName.Text = row["GMitraName"].ToString();
                    txtMobile.Text = row["GMitraMobile"].ToString();
                    txtEmail.Text = row["GMitraEmail"].ToString();
                    txtUserName.Text = row["GMitraLogin"].ToString();
                    txtPassword.Text = row["GMitraPassword"].ToString();
                    txtBankName.Text = row["GMitraBankName"].ToString();
                    txtAccName.Text = row["GMitraBankAccName"].ToString();
                    txtAccNo.Text = row["GMitraBankAccNumber"].ToString();
                    txtIfsc.Text = row["GMitraBankIFSC"].ToString();
                    txtPan.Text = row["GMitraPanCard"].ToString();
                    ddrState.SelectedValue = row["FK_StateID"].ToString();

                    c.FillComboBox("DistrictName", "DistrictId", "DistrictsData", "StateId=" + ddrState.SelectedValue + "", "DistrictName", 0, ddrDistrict);
                    ddrDistrict.SelectedValue = row["FK_DistrictID"].ToString();

                    // Fill City
                    c.FillComboBox("CityName", "CityID", "CityData", "FK_DistId=" + ddrDistrict.SelectedValue + "", "CityName", 0, ddrCity);
                    ddrCity.SelectedValue = row["FK_CityID"].ToString();
                    

                    if (row["GMitraPan"] != DBNull.Value && row["GMitraPan"] != null && row["GMitraPan"].ToString() != "")
                    {
                        if (row["GMitraPan"].ToString().Contains(".pdf"))
                        {
                            pancardImg = "<span class=\"space10\"></span><a href=\"" + Master.rootPath + "upload/genmitradocs/" + row["GMitraPan"].ToString() + "\" target=\"_blank\" ><i class=\"fas fa-file-pdf\" ></i>View Doc</a>";
                        }
                        else
                        {
                            pancardImg = "<span class=\"space10\"></span><a href=\"" + Master.rootPath + "upload/genmitradocs/" + row["GMitraPan"].ToString() + "\"  data-fancybox=\"imgGroup1\"><img src=\"" + Master.rootPath + "upload/genmitradocs/" + row["GMitraPan"].ToString() + "\" width=\"200\"/></a>";
                        }
                    }

                    if (row["GMitraAdhar"] != DBNull.Value && row["GMitraAdhar"] != null && row["GMitraAdhar"].ToString() != "")
                    {
                        if (row["GMitraAdhar"].ToString().Contains(".pdf"))
                        {
                            adharcardImg = "<span class=\"space10\"></span><a href=\"" + Master.rootPath + "upload/genmitradocs/" + row["GMitraAdhar"].ToString() + "\" target=\"_blank\"><i class=\"fas fa-file-pdf\" ></i>View Doc</a>";
                        }
                        else
                        {
                            adharcardImg = "<span class=\"space10\"></span><a href=\"" + Master.rootPath + "upload/genmitradocs/" + row["GMitraAdhar"].ToString() + "\"  data-fancybox=\"imgGroup1\"><img src=\"" + Master.rootPath + "upload/genmitradocs/" + row["GMitraAdhar"].ToString() + "\" width=\"200\" /></a>";
                        }
                    }

                    if (row["GMitraBankDoc"] != DBNull.Value && row["GMitraBankDoc"] != null && row["GMitraBankDoc"].ToString() != "")
                    {
                        if (row["GMitraBankDoc"].ToString().Contains(".pdf"))
                        {
                            bankdocImg = "<span class=\"space10\"></span><a href=\"" + Master.rootPath + "upload/genmitradocs/" + row["GMitraBankDoc"].ToString() + "\" target=\"_blank\"><i class=\"fas fa-file-pdf\" ></i>View Doc</a>";
                        }
                        else
                        {
                            bankdocImg = "<span class=\"space10\"></span><a href=\"" + Master.rootPath + "upload/genmitradocs/" + row["GMitraBankDoc"].ToString() + "\"  data-fancybox=\"imgGroup1\"><img src=\"" + Master.rootPath + "upload/genmitradocs/" + row["GMitraBankDoc"].ToString() + "\" width=\"200\" /></a>";
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetGeneriMitraData", ex.Message.ToString());
            return;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtName.Text == "" || txtMobile.Text == "" || txtEmail.Text == "" || txtUserName.Text == "" || txtPassword.Text == "" || txtBankName.Text == "" || txtAccName.Text == "" || txtAccNo.Text == "" || txtIfsc.Text == "" || txtPan.Text == "" || ddrState.SelectedIndex == 0 || ddrDistrict.SelectedIndex == 0 || ddrCity.SelectedIndex == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * fields are mandatory');", true);
                return;
            }

            int gmId = Convert.ToInt32(Session["adminGenMitra"]);

            string origImgPath = "~/upload/genmitradocs/";
            // Pan card
            string panName = "";
            if (fuPan.HasFile)
            {
                string fExt = Path.GetExtension(fuPan.FileName).ToString().ToLower();
                if (fExt == ".jpg" || fExt == ".jpeg" || fExt == ".png" || fExt == ".pdf")
                {
                    panName = "pancard-" + gmId + "-" + DateTime.Now.ToString("ddmmyyyyHHmmss") + fExt;
                    //FileInfo pan = new FileInfo(fuPan.FileName);
                    if (fuPan.PostedFile.ContentLength > 1000000) // file size in bytes (1 MB)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Pan card file size must be less than 1MB');", true);
                        return;
                    }
                    //fuPan.SaveAs(Server.MapPath(origImgPath) + panName);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Only .jpg, .jpeg, .png or .pdf files are allowed');", true);
                    return;
                }
            }
            // Adhar card
            string adharName = "";
            if (fuAdhar.HasFile)
            {
                string fExt = Path.GetExtension(fuAdhar.FileName).ToString().ToLower();
                if (fExt == ".jpg" || fExt == ".jpeg" || fExt == ".png" || fExt == ".pdf")
                {
                    adharName = "adharcard-" + gmId + "-" + DateTime.Now.ToString("ddmmyyyyHHmmss") + fExt;
                    //FileInfo adhar = new FileInfo(fuAdhar.FileName);
                    if (fuAdhar.PostedFile.ContentLength > 1000000) // file size in bytes (1 MB)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Adhar card file size must be less than 1MB');", true);
                        return;
                    }
                    //fuAdhar.SaveAs(Server.MapPath(origImgPath) + adharName);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Only .jpg, .jpeg, .png or .pdf files are allowed');", true);
                    return;
                }
            }
            // Bank doc
            string bankDoc = "";
            if (fuPassbook.HasFile)
            {
                string fExt = Path.GetExtension(fuPassbook.FileName).ToString().ToLower();
                if (fExt == ".jpg" || fExt == ".jpeg" || fExt == ".png" || fExt == ".pdf")
                {
                    bankDoc = "bankdoc-" + gmId + "-" + DateTime.Now.ToString("ddmmyyyyHHmmss") + fExt;
                    FileInfo doc = new FileInfo(fuPassbook.FileName);
                    if (fuPassbook.PostedFile.ContentLength > 1000000) // file size in bytes (1 MB)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Cheque/Passbook file size must be less than 1MB');", true);
                        return;
                    }
                    //fuPassbook.SaveAs(Server.MapPath(origImgPath) + bankDoc);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Only .jpg, .jpeg, .png or .pdf files are allowed');", true);
                    return;
                }
            }

            c.ExecuteQuery("Update GenericMitra Set GMitraName='" + txtName.Text + "', GMitraMobile='" + txtMobile.Text + "', GMitraEmail='" + txtEmail.Text + "', GMitraLogin='" + txtUserName.Text + "', GMitraPassword='" + txtPassword.Text + "', GMitraBankName='" + txtBankName.Text + "', GMitraBankAccName='" + txtAccName.Text + "', GMitraBankAccNumber='" + txtAccNo.Text + "', GMitraBankIFSC='" + txtIfsc.Text + "', GMitraPanCard='" + txtPan.Text + "', FK_StateID=" + ddrState.SelectedValue + ", FK_DistrictID=" + ddrDistrict.SelectedValue + ", FK_CityID=" + ddrCity.SelectedValue + " Where GMitraID=" + gmId + "");

            if (fuPan.HasFile && fuAdhar.HasFile && fuPassbook.HasFile)
            {
                fuPan.SaveAs(Server.MapPath(origImgPath) + panName);
                fuAdhar.SaveAs(Server.MapPath(origImgPath) + adharName);
                fuPassbook.SaveAs(Server.MapPath(origImgPath) + bankDoc);

                c.ExecuteQuery("Update GenericMitra Set GMitraPan='" + panName + "', GMitraAdhar='" + adharName + "', GMitraBankDoc='" + bankDoc + "' Where GMitraID=" + gmId);
            }
            
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Information Updated Successfully');", true);
            string url = Master.rootPath + "genericmitra/edit-profile.aspx";

            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "redirectJS", "setTimeout(function() { window.location.replace('" + url + "') }, 1500);", true);

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSave_Click", ex.Message.ToString());
            return;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("dashboard.aspx");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnCancel_Click", ex.Message.ToString());
            return;
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
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "ddrState_SelectedIndexChanged", ex.Message.ToString());
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
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "ddrDistrict_SelectedIndexChanged", ex.Message.ToString());
            return;
        }
    }
}