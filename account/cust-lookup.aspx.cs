using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class account_cust_lookup : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg, lookupUrl;
    protected void Page_Load(object sender, EventArgs e)
    {
        repBtn.Visible = false;
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            txtMob.Text = txtMob.Text.Trim().Replace("'", "");

            if (txtMob.Text == "")
            {
                errMsg = c.ErrNotification(2, "Enter Registered Mobile No");
                return;
            }

            if (!c.IsRecordExist("Select CustomrtID From CustomersData Where CustomerMobile='" + txtMob.Text + "' AND delMark=0 AND CustomerActive=1"))
            {
                errMsg = c.ErrNotification(2, "Enter valid customer mobile number");
                return;
            }

            int custId = Convert.ToInt32(c.GetReqData("CustomersData", "CustomrtID", "CustomerMobile='" + txtMob.Text + "' AND delMark=0 AND CustomerActive=1"));
            repBtn.Visible = true;
            lookupUrl = Master.rootPath + "customer-lookup.aspx?custId=" + custId;
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}