using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class customer_user_info : System.Web.UI.Page
{
    iClass c = new iClass();
    public string firstChar, sessionName, bgColor, rootPath, joinDate;
    public string[] arrFavShop = new string[5];
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //string custName = c.GetReqData("CustomersData", "CustomerName", "CustomrtID=" + Session["genericCust"]).ToString();
            //firstChar = custName.Substring(0, 1);
            //string[] arrSessionName = custName.ToString().Split(' ');
            //sessionName = custName.ToString();
            GetCustomerInfo();

            if (c.IsRecordExist("Select CustomrtID From CustomersData Where CustomerFavShop IS NOT NULL AND CustomerFavShop<>'' AND CustomerFavShop<>0 AND CustomrtID=" + Session["genericCust"]))
            {
                favShop.Visible = true;
                int franchId = Convert.ToInt32(c.GetReqData("CustomersData", "CustomerFavShop", "CustomrtID=" + Session["genericCust"]));
                GetFavShopInfo(franchId);
            }
        }
    }

    private void GetCustomerInfo()
    {
        string custName = c.GetReqData("CustomersData", "CustomerName", "CustomrtID=" + Session["genericCust"]).ToString();
        firstChar = custName.Substring(0, 1);
        string[] arrSessionName = custName.ToString().Split(' ');
        sessionName = custName.ToString();
        // array of colors
        string[] colors = { "#881798", "#e3008c", "#ffb900", "#107c10", "#da3b01", "#e3008c", "#00b7c3", "#0078d7", "#744da9", "#ff4343" };
        Random rNo = new Random();
        int index = rNo.Next(colors.Length);
        bgColor = "background:" + colors[index].ToString();
        string regDate = c.GetReqData("CustomersData", "CustomerJoinDate", "CustomrtID=" + Session["genericCust"]).ToString();

        joinDate = Convert.ToDateTime(regDate).ToString("dd MMM, yyyy");

        txtName.Text = custName.ToString();

        object emailCustomer = c.GetReqData("CustomersData", "CustomerEmail", "CustomrtID=" + Session["genericCust"]);

        if (emailCustomer != DBNull.Value && emailCustomer != null && emailCustomer.ToString() != "")
        {
            txtEmail.Text = emailCustomer.ToString();
        }

        txtMobile.Text = c.GetReqData("CustomersData", "CustomerMobile", "CustomrtID=" + Session["genericCust"]).ToString();

        object dob = c.GetReqData("CustomersData", "CustomerDOB", "CustomrtID=" + Session["genericCust"]);
        if (dob != DBNull.Value && dob != null && dob.ToString() != "")
        {
            txtDOB.Text = Convert.ToDateTime(dob).ToString("dd/MM/yyyy");

            int age = 0;
            age = DateTime.Now.Year - Convert.ToDateTime(dob).Year;
            if (DateTime.Now.DayOfYear < Convert.ToDateTime(dob).DayOfYear)
                age = age - 1;

            txtAge.Text = age.ToString();
        }
    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            txtName.Text = txtName.Text.Trim().Replace("'", "");
            txtEmail.Text = txtEmail.Text.Trim().Replace("'", "");
            txtMobile.Text = txtMobile.Text.Trim().Replace("'", "");
            txtDOB.Text = txtDOB.Text.Trim().Replace("'", "");
            txtAge.Text = txtAge.Text.Trim().Replace("'", "");

            if (txtName.Text == "" || txtEmail.Text == "" || txtMobile.Text == "" || txtDOB.Text == "" || txtAge.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * Fields are mandatory');", true);
                return;
            }

            string[] arrDate = txtDOB.Text.Split('/');
            if (c.IsDate(arrDate[1] + "/" + arrDate[0] + "/" + arrDate[2]) == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid Date of Birth');", true);
                return;
            }
            DateTime dob = Convert.ToDateTime(arrDate[1] + "/" + arrDate[0] + "/" + arrDate[2]);

            if (!c.ValidateMobile(txtMobile.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid Mobile Number');", true);
                return;
            }

            if (!c.EmailAddressCheck(txtEmail.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid Email Address');", true);
                return;
            }

            if (!c.IsNumeric(txtAge.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Age must be numeric value');", true);
                return;
            }

            int custId = Convert.ToInt32(Session["genericCust"]);

            c.ExecuteQuery("Update CustomersData Set CustomerName='" + txtName.Text + "', CustomerMobile='" + txtMobile.Text +
                "', CustomerEmail='" + txtEmail.Text + "', CustomerDOB='" + dob + "' Where CustomrtID=" + custId);


            GetCustomerInfo();
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Information updated Successfully..!!');", true);

            //txtName.Text = txtEmail.Text = txtMobile.Text = txtDOB.Text = txtAge.Text = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSubmit_Click", ex.Message.ToString());
            return;
        }
    }

    private void GetFavShopInfo(int franchIdX)
    {
        try
        {
            using (DataTable dtShop = c.GetDataTable("Select FranchID, FranchName, FranchMobile, FranchAddress From FranchiseeData Where FranchID=" + franchIdX))
            {
                if (dtShop.Rows.Count > 0)
                {
                    DataRow row = dtShop.Rows[0];

                    arrFavShop[0] = row["FranchName"].ToString();
                    arrFavShop[1] = row["FranchAddress"].ToString();
                    arrFavShop[2] = row["FranchMobile"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetFavShopInfo", ex.Message.ToString());
            return;
        }
    }
}