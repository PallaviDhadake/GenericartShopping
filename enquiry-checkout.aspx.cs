using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class enquiry_checkout : System.Web.UI.Page
{
    iClass c = new iClass();
    public string addrStr;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                int customerId;
                if (Session["genericCust"] == null)
                {
                    Response.Redirect(Master.rootPath + "login?ref=calc", false);
                }
                else
                {
                    customerId = Convert.ToInt32(Session["genericCust"]);
                    
                    GetMembeDetails(customerId);
                    int calcId = Convert.ToInt32(c.GetReqData("SavingCalc", "CalcID", "FK_CustId=" + customerId + " AND (CONVERT(varchar(20), CAST (CalcDate AS DATE), 112) = CONVERT(varchar(20), CAST ('" + DateTime.Now + "' AS DATE), 112))"));
                    Session["calcId"] = calcId.ToString();

                    if (c.IsRecordExist("Select AddressID From CustomersAddress Where AddressFKCustomerID=" + customerId))
                    {
                        newAddr.Visible = false;
                        existingAddr.Visible = true;
                        
                        GetCustomerAddress(customerId, calcId);
                    }
                    else
                    {
                        newAddr.Visible = true;
                        existingAddr.Visible = false;
                        chkAddNew.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "Page_Load", ex.Message.ToString());
            return;
        }
    }

    private void GetCustomerAddress(int custIdX, int calcIdX)
    {
        try
        {
            using (DataTable dtCustAddr = c.GetDataTable("Select AddressID, AddressFull, AddressName, AddressCity, AddressPincode From CustomersAddress Where AddressStatus=1 AND AddressFKCustomerID=" + custIdX))
            {
                if (dtCustAddr.Rows.Count > 0)
                {
                    StringBuilder strMarkup = new StringBuilder();
                    int bCount = 0;
                    foreach (DataRow row in dtCustAddr.Rows)
                    {
                        strMarkup.Append("<div class=\"w50 float_left\">");
                        strMarkup.Append("<div style=\"padding:10px 30px 10px 0;\">");
                        strMarkup.Append("<span id=\"addr-" + row["AddressID"].ToString() + "\">");
                        if (!c.IsRecordExist("Select CalcID From SavingCalc Where CalcID=" + calcIdX + " AND FK_AddressID=" + row["AddressID"].ToString()))
                        {
                            strMarkup.Append("<a href=\"" + Master.rootPath + "add-addr/" + row["AddressID"].ToString() + "-" + calcIdX + "\" class=\"addrBorder txtDecNone\"  style=\"display:block;\">");
                            strMarkup.Append("<div class=\"box-shadow bgWhite border_r_5\">");
                            strMarkup.Append("<div class=\"pad_10\" style=\"cursor:pointer\">");
                            strMarkup.Append("<span class=\"themeClrPrime fontRegular\">" + row["AddressName"].ToString() + "</span>");
                            strMarkup.Append("<span class=\"space5\"></span>");
                            strMarkup.Append("<span class=\"clrGrey small fontRegular line-ht-5\">" + row["AddressFull"].ToString() + "</span>");
                            strMarkup.Append("<span class=\"space10\"></span>");
                            strMarkup.Append("<span class=\"clrGrey small fontRegular line-ht-5\">" + row["AddressCity"].ToString() + ", " + row["AddressPincode"].ToString() + "</span>");
                            strMarkup.Append("</div>");
                            strMarkup.Append("</div>");
                            strMarkup.Append("</a>");
                        }
                        else
                        {
                            strMarkup.Append("<a class=\"addrBorderBlue txtDecNone\"  style=\"display:block;\">");
                            strMarkup.Append("<div class=\"box-shadow bgWhite border_r_5\">");
                            strMarkup.Append("<div class=\"pad_10\" style=\"cursor:pointer\">");
                            strMarkup.Append("<span class=\"themeClrPrime fontRegular\">" + row["AddressName"].ToString() + "</span>");
                            strMarkup.Append("<span class=\"space5\"></span>");
                            strMarkup.Append("<span class=\"clrGrey small fontRegular line-ht-5\">" + row["AddressFull"].ToString() + "</span>");
                            strMarkup.Append("<span class=\"space10\"></span>");
                            strMarkup.Append("<span class=\"clrGrey small fontRegular line-ht-5\">" + row["AddressCity"].ToString() + ", " + row["AddressPincode"].ToString() + "</span>");
                            strMarkup.Append("<span class=\"space10\"></span>");
                            strMarkup.Append("<span class=\"semiBold themeClrPrime\">Address Selected</span>");
                            strMarkup.Append("</div>");
                            strMarkup.Append("</div>");
                            strMarkup.Append("</a>");
                        }
                        strMarkup.Append("</span>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");

                        bCount++;

                        if ((bCount % 2) == 0)
                        {
                            strMarkup.Append("<div class=\"float_clear\"></div>");
                        }
                    }
                    strMarkup.Append("<div class=\"float_clear\"></div>");
                    addrStr = strMarkup.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetCustomerAddress", ex.Message.ToString());
            return;
        }
    }

    private void GetMembeDetails(int memIdX)
    {
        try
        {
            using (DataTable dtMem = c.GetDataTable("Select CustomrtID, CustomerEmail, CustomerMobile, CustomerName, CustomerCity, " +
                " CustomerState, CustomerPincode, CustomerCountry, CustomerAddress From CustomersData Where CustomrtID=" + memIdX))
            {
                if (dtMem.Rows.Count > 0)
                {
                    DataRow row = dtMem.Rows[0];

                    txtMobile.Text = row["CustomerMobile"].ToString();

                    txtName.Text = row["CustomerName"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetMembeDetails", ex.Message.ToString());
            return;
        }
    }

    protected void chkAddNew_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAddNew.Checked == true)
        {
            newAddr.Visible = true;
            existingAddr.Visible = false;
        }
        else
        {
            newAddr.Visible = false;
            existingAddr.Visible = true;
            Response.Redirect(Master.rootPath + "enquiry-checkout", false);
        }
    }
    protected void btnShipping_Click(object sender, EventArgs e)
    {
        try
        {
            txtMobile.Text = txtMobile.Text.Trim().Replace("'", "");
            txtName.Text = txtName.Text.Trim().Replace("'", "");
            txtCountry.Text = txtCountry.Text.Trim().Replace("'", "");
            txtState.Text = txtState.Text.Trim().Replace("'", "");
            txtCity.Text = txtCity.Text.Trim().Replace("'", "");
            txtPinCode.Text = txtPinCode.Text.Trim().Replace("'", "");
            txtAddress1.Text = txtAddress1.Text.Trim().Replace("'", "");

            int calcId = Convert.ToInt32(Session["calcId"]);
            int customerId = Convert.ToInt32(Session["genericCust"]);

            if (chkAddNew.Checked == false)
            {
                if (c.IsRecordExist("Select CalcID From SavingCalc Where CalcID=" + calcId + " AND (FK_AddressId IS NULL OR FK_AddressId=0)"))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select Address To Proceed');", true);
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('" + Master.rootPath + "enquiry-checkout', 1000);", true);
                }
                else
                {
                    if (c.IsRecordExist("Select CustomrtID From CustomersData Where CustomerFavShop IS NOT NULL AND CustomerFavShop<>'' AND CustomerFavShop<>0 AND CustomrtID=" + customerId))
                    {
                        // route direct to fav shop
                        int franchId = Convert.ToInt32(c.GetReqData("CustomersData", "CustomerFavShop", "CustomrtID=" + customerId));

                        // set enq status to 2 -> as it is accepted by admin & assign to shop
                        c.ExecuteQuery("Update SavingCalc Set EnqStatus=2 Where CalcID=" + calcId);

                        if (!c.IsRecordExist("Select EnqAssignID From SavingEnqAssign Where FK_CalcID=" + calcId + " AND Fk_FranchID=" + franchId + " AND  EnqAssignStatus=0"))
                        {
                            // insert entry in EnqiryAssign table, it is directly assigned to customers fav shop
                            c.ExecuteQuery("Update SavingEnqAssign Set EnqReAssign=1 Where FK_CalcID=" + calcId);
                            int maxId = c.NextId("SavingEnqAssign", "EnqAssignID");
                            c.ExecuteQuery("Insert Into SavingEnqAssign (EnqAssignID, EnqAssignDate, FK_CalcID, Fk_FranchID, EnqAssignStatus, " +
                                " EnqReAssign) Values (" + maxId + ", '" + DateTime.Now + "', " + calcId + ", " + franchId + ", 0, 0)");

                        }

                    }
                    else
                    {
                        c.ExecuteQuery("Update SavingCalc Set EnqStatus=1 Where CalcID=" + calcId);
                    }
                    Session["calcId"] = null;
                    //ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Your Enquiry Submitted Successfully..!!');", true);
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "$(document).ready(function () {$('#slide').popup('show');});", true);
                    //ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('" + Master.rootPath + "', 6000);", true);
                }
            }
            else
            {

                if (txtName.Text == "" || txtCountry.Text == "" ||
                    txtState.Text == "" || txtCity.Text == "" || txtPinCode.Text == "" || txtAddress1.Text == "" || ddrAddrName.SelectedIndex == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * Marked fields are Mandatory');", true);
                    return;
                }
                if (txtMobile.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Contact Information');", true);
                    return;
                }
                if (c.ValidateMobile(txtMobile.Text) == false)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter valid mobile number');", true);
                    return;
                }
                if (!c.IsNumeric(txtPinCode.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Pincode must be numeric value');", true);
                    return;
                }
                // insert new address of cust in custAddress tbl

                if (chkAddNew.Checked == true)
                {
                    int maxAddrId = c.NextId("CustomersAddress", "AddressID");
                    c.ExecuteQuery("Insert Into CustomersAddress (AddressID, AddressFKCustomerID, AddressFull, AddressCity, AddressState, " +
                        " AddressPincode, AddressCountry, AddressStatus, AddressName) Values (" + maxAddrId + ", " + customerId + ", '" + txtAddress1.Text +
                        "', '" + txtCity.Text + "', '" + txtState.Text + "', '" + txtPinCode.Text + "', '" + txtCountry.Text + "', 1, '" + ddrAddrName.SelectedItem.Text + "')");


                    if (c.IsRecordExist("Select CustomrtID From CustomersData Where CustomerFavShop IS NOT NULL AND CustomerFavShop<>'' AND CustomerFavShop<>0 AND CustomrtID=" + customerId))
                    {
                        // route direct to fav shop
                        int franchId = Convert.ToInt32(c.GetReqData("CustomersData", "CustomerFavShop", "CustomrtID=" + customerId));

                        // set enq status to 2 -> as it is accepted by admin & assign to shop
                        c.ExecuteQuery("Update SavingCalc Set FK_AddressId=" + maxAddrId + ", EnqStatus=2 Where CalcID=" + calcId);

                        if (!c.IsRecordExist("Select EnqAssignID From SavingEnqAssign Where FK_CalcID=" + calcId + " AND Fk_FranchID=" + franchId + " AND  EnqAssignStatus=0"))
                        {
                            // insert entry in EnqiryAssign table, it is directly assigned to customers fav shop
                            c.ExecuteQuery("Update SavingEnqAssign Set EnqReAssign=1 Where FK_CalcID=" + calcId);
                            int maxId = c.NextId("SavingEnqAssign", "EnqAssignID");
                            c.ExecuteQuery("Insert Into SavingEnqAssign (EnqAssignID, EnqAssignDate, FK_CalcID, Fk_FranchID, EnqAssignStatus, " +
                                " EnqReAssign) Values (" + maxId + ", '" + DateTime.Now + "', " + calcId + ", " + franchId + ", 0, 0)");
                        }
                    }
                    else
                    {
                        // update addressId in SavingCalc tbl
                        c.ExecuteQuery("Update SavingCalc Set FK_AddressId=" + maxAddrId + ", EnqStatus=1 Where CalcID=" + calcId);
                    }
                    Session["ordId"] = null;

                    //ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Your Enquiry Submitted Successfully..!!');", true);
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "$(document).ready(function () {$('#slide').popup('show');});", true);
                    //ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('" + Master.rootPath + "', 6000);", true);
                }
            }

            string totalBrandPrice = c.returnAggregate("Select SUM(BrandPrice) From SavingCalcItems Where FK_CalcID=" + calcId).ToString("0.00");
            string totalSavings = c.returnAggregate("Select SUM(BrandPrice - GenericPrice) From SavingCalcItems Where FK_CalcID=" + calcId).ToString("0.00");
            double totalPercent = (Convert.ToDouble(totalSavings) * 100) / Convert.ToDouble(totalBrandPrice);

            string strMsg = "Dear Sir / Madam, Your Total Saving is of Rs. " + totalSavings + " i.e Saving of " + totalPercent.ToString("0.00") + "%, Thank you for your enquiry..!! Genericart Shop coordinator will be in touch with you shortly. Genericart Medicine Store - Wahi Kaam, Sahi Daam Toll Free No. 9090308585";
            
            string custMobNo = c.GetReqData("CustomersData", "CustomerMobile", "CustomrtID=" + customerId).ToString();
            c.SendSMS(strMsg, custMobNo);

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('" + Master.rootPath + "', 6000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnShipping_Click", ex.Message.ToString());
            return;
        }
    }
}