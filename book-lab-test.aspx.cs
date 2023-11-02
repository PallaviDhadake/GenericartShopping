using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class book_lab_test : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                c.FillComboBox("LabTestName", "LabTestID", "LabTestData", "DelMark=0", "LabTestName", 0, ddrLab);

                if (Request.QueryString["code"] != null)
                {
                    txtShopCode.Text = Request.QueryString["code"];
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
            txtName.Text = txtName.Text.Trim().Replace("'", "");
            txtAge.Text = txtAge.Text.Trim().Replace("'", "");
            txtMobile.Text = txtMobile.Text.Trim().Replace("'", "");
            txtAddress.Text = txtAddress.Text.Trim().Replace("'", "");
            txtPinCode.Text = txtPinCode.Text.Trim().Replace("'", "");
            txtEmail.Text = txtEmail.Text.Trim().Replace("'", "");
            txtDate.Text = txtDate.Text.Trim().Replace("'", "");

            txtShopCode.Text = txtShopCode.Text.Trim().Replace("'", "");

            if (txtName.Text == "" || txtAge.Text == "" || txtMobile.Text == "" || txtAddress.Text == "" || txtPinCode.Text == "" || txtEmail.Text == "" || txtDate.Text == "" || ddrGender.SelectedIndex == 0 || ddrLab.SelectedIndex == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * Marked fields are Mandatory');", true);
                return;
            }

            if (!c.IsNumeric(txtAge.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Age must be numeric value');", true);
                return;
            }

            if (!c.IsNumeric(txtPinCode.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Pincode must be numeric value');", true);
                return;
            }

            if (!c.EmailAddressCheck(txtEmail.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid Email Address');", true);
                return;
            }

            if (!c.ValidateMobile(txtMobile.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid mobile number');", true);
                return;
            }

            if (txtAddress.Text.Length > 200)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Address must be less than 200 characters');", true);
                return;
            }

            if (txtShopCode.Text != "")
            {
                if (!c.IsRecordExist("Select FranchID From FranchiseeData Where FranchShopCode='" + txtShopCode.Text + "' AND FranchActive=1"))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Invalid Shop Code entered');", true);
                    return;
                }
            }

            DateTime appDate = DateTime.Now;
            string[] arrTDate = txtDate.Text.Split('/');
            if (c.IsDate(arrTDate[1] + "/" + arrTDate[0] + "/" + arrTDate[2]) == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid Appointment Date');", true);
                return;
            }
            else
            {
                appDate = Convert.ToDateTime(arrTDate[1] + "/" + arrTDate[0] + "/" + arrTDate[2]);
            }

            if (c.IsRecordExist("Select LabAppID From LabAppointments Where LabAppMobile='" + txtMobile.Text + "' AND FK_LabTestID=" + ddrLab.SelectedValue + " AND ( CONVERT(varchar(20), CAST (LabAppDate AS DATE), 112) = CONVERT(varchar(20), CAST ('" + appDate + "' AS DATE), 112) )")) 
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Appointment on " + txtDate.Text + " is already booked by " + txtMobile.Text + " number for " + ddrLab.SelectedItem.Text + "');", true);
                return;
            }

            int maxId = c.NextId("LabAppointments", "LabAppID");

            c.ExecuteQuery("Insert Into LabAppointments (LabAppID, LabAppDate, LabAppName, LabAppAge, LabAppGender, LabAppMobile, LabAppAddress, " +
                " LabAppPincode, LabAppEmail, FK_LabTestID, LabAppStatus, DeviceType, LabRefShopCode) Values (" + maxId + ", '" + appDate + "', '" + txtName.Text + "', " + txtAge.Text +
                ", " + ddrGender.SelectedValue + ", '" + txtMobile.Text + "', '" + txtAddress.Text + "', '" + txtPinCode.Text + "', '" + txtEmail.Text +
                "', " + ddrLab.SelectedValue + ", 0, 'Web', '" + txtShopCode.Text + "')");


            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Lab test appointment booked successfully..!!');", true);

            txtName.Text = txtAge.Text = txtMobile.Text = txtAddress.Text = txtPinCode.Text = txtEmail.Text = txtDate.Text = "";
            ddrLab.SelectedIndex = 0;
            ddrGender.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}