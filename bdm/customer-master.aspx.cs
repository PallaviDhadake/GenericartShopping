using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class bdm_customer_master : System.Web.UI.Page
{
    public string pgTitle, errMsg, disImg;
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            txtName.Text = txtName.Text.Trim().Replace("'", "");

            if (txtName.Text == "" || txtMobile.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All fields are mandatory');", true);
                return;
            }

            //Check Mobile number duplication
            if (c.IsRecordExist("Select CustomrtID From CustomersData Where CustomerMobile='" + txtMobile.Text + "'") == true)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'This Mobile No. customer already registered.');", true);
                return;
            }
            
            // Save data to database
            int custMaxId = c.NextId("CustomersData", "CustomrtID");
            c.ExecuteQuery("Insert Into CustomersData (CustomrtID, CustomerJoinDate, CustomerName, CustomerMobile, " +
                                            " MobileVerify, EmailVerify, CustomerActive, delMark, DeviceType) Values (" + custMaxId +
                                            ", '" + DateTime.Now + "', '" + txtName.Text + "', '" + txtMobile.Text +
                                            "', 1, 1, 1, 0, 'Survey')");
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Record Saved.');", true);
            txtName.Text = txtMobile.Text = "";
            txtName.Focus();

        }
        catch(Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "BDM-Customer_Master_btnSubmit_Click", ex.Message.ToString());
            return;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }
}