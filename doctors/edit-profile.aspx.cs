using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class doctors_edit_profile : System.Web.UI.Page
{
    public string pgTitle, errMsg, docImg;
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        pgTitle = "Edit Your Profile";
        btnSave.Attributes.Add("onclick", "this.disabled=true; this.value='Processing...';" + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        btnCancel.Attributes.Add("onclick", "this.disabled=true; this.value='Processing...';" + ClientScript.GetPostBackEventReference(btnCancel, null) + ";");

        if (!IsPostBack)
        {

            c.FillComboBox("StateName", "StateID", "StatesData", "FK_CountryID=101", "StateID", 0, ddrState);
            c.FillComboBox("SpecialtyName", "SpecialtyID", "DoctorSpecialtyData", "delMark=0", "SpecialtyName", 0, ddrSpeciality);
            FillExperience();
            GetDoctorData(Convert.ToInt32(Session["adminDoctor"]));

            txtName.Focus();
        }
    }

    private void GetDoctorData(int Idx)
    {
        try
        {
            using (DataTable dtProduct = c.GetDataTable("Select * From DoctorsData Where DoctorID=" + Idx))
            {
                if (dtProduct.Rows.Count > 0)
                {
                    DataRow bRow = dtProduct.Rows[0];
                    lblId.Text = Idx.ToString();

                    txtName.Text = bRow["DocName"].ToString();
                    txtMobileNo.Text = bRow["DocMobileNum"].ToString();
                    txtEmail.Text = bRow["DocEmailId"].ToString();
                    ddrState.SelectedValue = bRow["FK_DocStateID"].ToString();

                    //Fill Dropdown list of city
                    //c.FillComboBox("CityName", "CityID", "CityData", "FK_StateID=" + ddrState.SelectedValue, "CityName", 0, ddrCity);
                    //ddrCity.SelectedValue = bRow["FK_DocCityID"].ToString();

                    c.FillComboBox("DistrictName", "DistrictId", "DistrictsData", "StateId=" + ddrState.SelectedValue, "DistrictName", 0, ddrDist);
                    ddrDist.SelectedValue = bRow["FK_DocDistId"].ToString();
                    c.FillComboBox("CityName", "CityID", "CityData", "FK_DistId=" + ddrDist.SelectedValue, "CityName", 0, ddrCity);
                    ddrCity.SelectedValue = bRow["FK_DocCityID"].ToString();


                    txtPinCode.Text = bRow["DocPinCode"].ToString();
                    txtAddress.Text = bRow["DocAddress"].ToString();
                    txtDegree.Text = bRow["DocDegree"].ToString();
                    ddrSpeciality.SelectedValue = bRow["FK_DocSpecialtyID"].ToString();
                    ddrExperience.SelectedValue = bRow["DocExperience"].ToString();
                    txtAbout.Text = bRow["DocAbout"].ToString();

                    if (bRow["DocPhoto"] != DBNull.Value && bRow["DocPhoto"] != null && bRow["DocPhoto"].ToString() != "" && bRow["DocPhoto"].ToString() != "no-photo.png")
                    {
                        docImg = "<img src=\"" + Master.rootPath + "upload/doctors/" + bRow["DocPhoto"].ToString() + "\" width=\"200\" />";
                    }



                    if (bRow["ConsultationFees"] != DBNull.Value && bRow["ConsultationFees"] != null && bRow["ConsultationFees"].ToString() != "")
                    {
                        txtFees.Text = bRow["ConsultationFees"].ToString();
                    }
                }
            }
        }
        catch (Exception)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            return;
        }
    }

    protected void ddrState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //GetDoctorData(Convert.ToInt32(Session["adminDoctor"]));
            object docPhoto = c.GetReqData("DoctorsData", "DocPhoto", "DoctorID=" + Session["adminDoctor"]);
            if (docPhoto != DBNull.Value && docPhoto != null && docPhoto != "" && docPhoto != "no-photo.png")
            {
                docImg = "<img src=\"" + Master.rootPath + "upload/doctors/" + docPhoto.ToString() + "\" width=\"200\" />";
            }
            //Fill Dropdown list of Sub category selection
            if (ddrState.SelectedIndex > 0)
            {
                //c.FillComboBox("CityName", "CityID", "CityData", "FK_StateID=" + ddrState.SelectedValue, "CityName", 0, ddrCity);
                c.FillComboBox("DistrictName", "DistrictId", "DistrictsData", "StateId=" + ddrState.SelectedValue, "DistrictName", 0, ddrDist);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "ddrState_SelectedIndexChanged", ex.Message.ToString());
            return;
        }
    }

    protected void ddrDist_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            object docPhoto = c.GetReqData("DoctorsData", "DocPhoto", "DoctorID=" + Session["adminDoctor"]);
            if (docPhoto != DBNull.Value && docPhoto != null && docPhoto != "" && docPhoto != "no-photo.png")
            {
                docImg = "<img src=\"" + Master.rootPath + "upload/doctors/" + docPhoto.ToString() + "\" width=\"200\" />";
            }

            //Fill Dropdown list of Sub category selection
            if (ddrDist.SelectedIndex > 0)
            {
                c.FillComboBox("CityName", "CityID", "CityData", "FK_DistId=" + ddrDist.SelectedValue, "CityName", 0, ddrCity);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "ddrDist_SelectedIndexChanged", ex.Message.ToString());
            return;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("dashboard.aspx");
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {

            //Single quote filter
            GetAllControls(this.Controls);

            //Empty fields validation
            if (txtName.Text == "" || txtMobileNo.Text == "" || txtEmail.Text == "" || ddrState.SelectedIndex == 0 || ddrCity.SelectedIndex == 0 || ddrDist.SelectedIndex == 0 || ddrSpeciality.SelectedIndex == 0 || ddrExperience.SelectedIndex == 0 || txtDegree.Text == "" || txtFees.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * fields are mandatory');", true);
                return;
            }
            //Email Id regular expression validation
            if (!c.EmailAddressCheck(txtEmail.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter correct email id');", true);
                return;
            }

            if (!c.IsNumeric(txtFees.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Consultation Fees must be numeric value');", true);
                return;
            }

            if (txtPinCode.Text != "")
            {
                if (!c.IsNumeric(txtPinCode.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Pincode must be numeric value');", true);
                    return;
                }
            }

            if (txtAddress.Text != "")
            {
                if (txtAddress.Text.Length > 200)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Address must be less than 200 characters');", true);
                    return;
                }
            }

            int maxId = Convert.ToInt16(lblId.Text);

            string imgName = "";

            if (fuImg.HasFile)
            {
                string fExt = Path.GetExtension(fuImg.FileName).ToLower();
                if (fExt == ".png" || fExt == ".jpg" || fExt == ".jpeg")
                {
                    imgName = "doctor-" + maxId + fExt;
                    fuImg.SaveAs(Server.MapPath("~/upload/doctors/") + imgName);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Only .png, .jpg and .jpeg files are allowed');", true);
                    return;
                }
            }
            // Insert / Update data into database 


            //int docExp = txtExp.Text == "" ? 0 : Convert.ToInt32(txtExp.Text);

            c.ExecuteQuery("Update DoctorsData Set DocName='" + txtName.Text + "', DocMobileNum='" + txtMobileNo.Text +
                    "', DocEmailId='" + txtEmail.Text + "', FK_DocStateID=" + ddrState.SelectedValue +
                    ", FK_DocCityID=" + ddrCity.SelectedValue +
                    ", DocPinCode=" + Convert.ToInt32(txtPinCode.Text) + ", DocAddress='" + txtAddress.Text +
                    "', DocDegree='" + txtDegree.Text + "', FK_DocSpecialtyID=" + ddrSpeciality.SelectedValue +
                    ", DocExperience=" + ddrExperience.SelectedValue + ", DocAbout='" + txtAbout.Text +
                    "', ConsultationFees=" + Convert.ToDouble(txtFees.Text) + ", FK_DocDistId=" + ddrDist.SelectedValue + " Where DoctorID=" + maxId);

            if (fuImg.HasFile)
            {
                c.ExecuteQuery("Update DoctorsData Set DocPhoto='" + imgName + "' Where DoctorID=" + maxId);
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Doctor Info updated');", true);

            //ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('doctor-master.aspx', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSave_Click", ex.Message.ToString());
            return;

        }
    }

    public void GetAllControls(ControlCollection ctls)
    {
        foreach (Control c in ctls)
        {
            if (c is System.Web.UI.WebControls.TextBox)
            {
                TextBox tt = c as TextBox;
                //to do something by using textBox tt.
                tt.Text = tt.Text.Trim().Replace("'", "");
            }
            if (c.HasControls())
            {
                GetAllControls(c.Controls);

            }
        }
    }

    private void FillExperience()
    {
        for (int i = 1; i < 31; i++)
        {
            ddrExperience.Items.Add(i.ToString());
            ddrExperience.SelectedValue = "0";
        }
    }
}