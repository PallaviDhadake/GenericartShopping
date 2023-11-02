using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Razorpay.Api;

public partial class supportteam_add_new_customer : System.Web.UI.Page
{
    public string pgTitle, errMsg, disImg, newOrdUrl;
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            txtName.Text = txtName.Text.Trim().Replace("'", "");

            if (txtName.Text == "" || txtMobile.Text == "" || txtAddress.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All fields are mandatory');", true);
                return;
            }

            //check mobile no duplication
            if (c.IsRecordExist("SELECT [CustomrtID] FROM [dbo].[CustomersData] WHERE [CustomerMobile] = '" + txtMobile.Text + "'") == true)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'This Mobile No. Already registered');", true);
                return;
            }

            //Save data to Database
            int CustMaxId = c.NextId("[CustomersData]", "[CustomrtID]");
            c.ExecuteQuery("Insert Into [dbo].[CustomersData] ([CustomrtID], [CustomerJoinDate], [CustomerName], [CustomerMobile], [CustomerPassword],[CustomerAddress], " +
                           " [MobileVerify], [EmailVerify], [CustomerActive], [delMark], [DeviceType], [FK_TeamMemberId]) Values (" + CustMaxId +
                           ", '" + DateTime.Now + "', '" + txtName.Text + "', '" + txtMobile.Text +
                           "', '123456' , LEFT('" + txtAddress.Text + "',50) , 1, 1, 1, 0, 'Support Team', " + Session["adminSupport"] + ")");

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Record Saved.');", true);
            txtName.Text = txtMobile.Text = txtAddress.Text = "";
            txtName.Focus();

            string redirectUrl = "submit-po.aspx?custId=" + CustMaxId + "&type=newOrd";
            Response.Redirect(redirectUrl, false);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSubmit_Click", ex.Message.ToString());
            return;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    { 
    
    }    
}