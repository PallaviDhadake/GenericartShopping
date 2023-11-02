using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Services;

public partial class supportteam_saving_calc : System.Web.UI.Page
{
    iClass c = new iClass();
    public string medStr = "", totalSum, totalPercentage, errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        btnCalculate.Attributes.Add("onclick", "this.disabled=true;this.value='Processing...';" + ClientScript.GetPostBackEventReference(btnCalculate, null) + ";");

        if (!IsPostBack)
        {
            txtMedName.Focus();

            if (Request.QueryString["action"] != null)
            {
                if (Request.QueryString["action"] == "remove")
                {
                    int calcId = Convert.ToInt32(c.GetReqData("SavingCalcItems", "FK_CalcID", "CalcItemID=" + Request.QueryString["id"]));
                    Session["calc"] = calcId.ToString();
                    c.ExecuteQuery("Delete From SavingCalcItems Where CalcItemID=" + Convert.ToInt32(Request.QueryString["id"]));
                    if (Convert.ToInt32(c.returnAggregate("Select Count(CalcItemID) From SavingCalcItems Where FK_CalcID=" + Session["calc"])) <= 0)
                    {
                        c.ExecuteQuery("Delete From SavingCalc Where CalcID=" + Session["calc"]);
                        Session["calc"] = null;
                    }
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Medicine Removed');", true);
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('saving-calc.aspx', 1000);", true);
                }
            }

            

            if (Session["calc"] != null)
            {
                medTable.Visible = true;
                FillMedTable(Convert.ToInt32(Session["calc"]));
                txtMedName.Text = "";
                txtMedName.Focus();
                string mobNo = c.GetReqData("SavingCalc", "MobileNumber", "CalcID=" + Convert.ToInt32(Session["calc"])).ToString();
                txtMobile.Text = mobNo.ToString();
                Session["medData"] = null;
            }
        }
    }

    [WebMethod]
    //[System.Web.Script.Services.ScriptMethod(UseHttpGet = false, ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static List<string> GetMedName(string medName)
    {
        iClass c = new iClass();
        List<string> medResult = new List<string>();
        using (DataTable dtMed = c.GetDataTable("Select TOP 100 MedicineRowID, BrandName From SurveyMedicines Where BrandName LIKE'" + medName + "%'"))
        {
            if (dtMed.Rows.Count > 0)
            {
                foreach (DataRow row in dtMed.Rows)
                {
                    medResult.Add(string.Format("{0}#{1}", row["BrandName"], row["MedicineRowID"]));
                }
            }
            else
            {
                medResult.Add("Match not found");
            }

            return medResult;
        }
    }

    [WebMethod]
    public static string GetMedInfo(string medId)
    {
        //return "intellect" + medId.ToString();
        iClass c = new iClass();
        int medRowId = Convert.ToInt32(medId);
        string medContent = c.GetReqData("SurveyMedicines", "ContentName", "MedicineRowID=" + medRowId).ToString().Replace("-", ",");
        string brandPrice = c.GetReqData("SurveyMedicines", "PriceBrand", "MedicineRowID=" + medRowId).ToString();
        string genPrice = c.GetReqData("SurveyMedicines", "PriceGeneric", "MedicineRowID=" + medRowId).ToString();
        string packaging = c.GetReqData("SurveyMedicines", "Packaging", "MedicineRowID=" + medRowId).ToString();

        HttpContext.Current.Session["medData"] = medId + "#" + medContent + "#" + brandPrice + "#" + genPrice + "#" + packaging;

        return medId + "#" + medContent + "#" + brandPrice + "#" + genPrice + "#" + packaging;
    }

    protected void btnCalculate_Click(object sender, EventArgs e)
    {
        try
        {
            txtMobile.Text = txtMobile.Text.Trim().Replace("'", "");

            if (txtMobile.Text == "")
            {
                errMsg = c.ErrNotification(2, "Enter Mobile No.");
                return;
            }

            if (!c.ValidateMobile(txtMobile.Text))
            {
                errMsg = c.ErrNotification(2, "Enter Valid Mobile No.");
                return;
            }


            Session["calcMob"] = txtMobile.Text;
            if (Session["medData"] != null)
            {
                int maxId = 0;

                string[] arrMedData = Session["medData"].ToString().Split('#');
                int medId = Convert.ToInt32(arrMedData[0].ToString());
                double brandPrice = Convert.ToDouble(arrMedData[2].ToString());
                double genPrice = Convert.ToDouble(arrMedData[3].ToString());
                string gmplCode = c.GetReqData("SurveyMedicines", "GenericCode", "MedicineRowID=" + medId).ToString();
                string genCode = c.GetReqData("SurveyMedicines", "ContentName", "MedicineRowID=" + medId).ToString();
                string packaging = c.GetReqData("SurveyMedicines", "Packaging", "MedicineRowID=" + medId).ToString();

                //if (c.IsRecordExist("Select CalcID From SavingCalc Where MobileNumber='" + txtMobile.Text + "' AND BrandMedicine='" + txtMedName.Text + "' AND GenericCode='" + genCode + "' AND (CONVERT(varchar(20), CAST (CalcDate AS DATE), 112) = CONVERT(varchar(20), CAST ('" + DateTime.Now + "' AS DATE), 112))"))
                //{
                //    FillMedTable(txtMobile.Text);
                //    errMsg = c.ErrNotification(2, "You have already calculated for '" + txtMedName.Text + "'");
                //    return;

                //}

                double savings = brandPrice - genPrice;
                double savingPercent = (savings * 100) / brandPrice;
                string savePercent = savingPercent.ToString("0.00");

                //int mreqFlag = chkMreq.Checked == true ? 1 : 0;

                //c.ExecuteQuery("Insert Into SavingCalc (CalcID, CalcDate, MobileNumber, BrandMedicine, BrandPrice, GenericCode, GenericPrice, " +
                //    " SavingAmount, SavingPercent) Values (" + maxId + ", '" + DateTime.Now + "', '" + txtMobile.Text + "', '" + txtMedName.Text +
                //    "', " + brandPrice + ", '" + genCode + "', " + genPrice + ", " + savings + ", " + Convert.ToDouble(savePercent) + ")");

                if (c.IsRecordExist("Select CalcID From SavingCalc Where MobileNumber='" + txtMobile.Text + "' AND EnqStatus IN (0, 1) AND (CONVERT(varchar(20), CAST (CalcDate AS DATE), 112) = CONVERT(varchar(20), CAST ('" + DateTime.Now + "' AS DATE), 112))"))
                {
                    int calcId = Convert.ToInt32(c.GetReqData("SavingCalc", "CalcID", "MobileNumber='" + txtMobile.Text + "' AND EnqStatus IN (0, 1)"));
                    maxId = calcId;
                }
                else
                {
                    maxId = c.NextId("SavingCalc", "CalcID");
                    int customerId = 0;
                    if (Session["genericCust"] != null)
                        customerId = Convert.ToInt32(Session["genericCust"]);
                    else
                        customerId = 0;

                    c.ExecuteQuery("Insert Into SavingCalc (CalcID, CalcDate, MobileNumber, FK_CustId, FK_AddressID, EnqStatus, DeviceType) " +
                        " Values (" + maxId + ", '" + DateTime.Now + "', '" + txtMobile.Text + "', " + customerId + ", 0, 0, 'CS-Enq')");
                }

                if (c.IsRecordExist("Select CalcItemID From SavingCalcItems Where FK_CalcID=" + maxId + " AND BrandMedicine='" + txtMedName.Text + "' AND GenericMedicine='" + genCode + "'"))
                {
                    FillMedTable(maxId);
                    errMsg = c.ErrNotification(2, "You have already calculated for '" + txtMedName.Text + "'");
                    return;

                }

                int calcItemId = c.NextId("SavingCalcItems", "CalcItemID");

                c.ExecuteQuery("Insert Into SavingCalcItems (CalcItemID, FK_CalcID, BrandMedicine, BrandPrice, GenericMedicine, " +
                    " GenericPrice, SavingAmount, SavingPercent, GenericCode, Packaging) Values (" + calcItemId + ", " + maxId +
                    ", '" + txtMedName.Text + "', " + brandPrice + ", '" + genCode + "', " + genPrice + ", " + savings +
                    ", " + Convert.ToDouble(savePercent) + ", '" + gmplCode + "', '" + packaging + "')");


                medTable.Visible = true;
                //AddNewRecordRowToGrid(medId);
                FillMedTable(maxId);
                txtMedName.Text = "";
                txtMedName.Focus();

                Session["medData"] = null;
            }


        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    private void FillMedTable(int calcIdX)
    {
        try
        {
            using (DataTable dtMed = c.GetDataTable("Select * From SavingCalcItems Where FK_CalcID=" + calcIdX))
            {
                if (dtMed.Rows.Count > 0)
                {
                    StringBuilder strMarkup = new StringBuilder();
                    double totalSavings = 0, totalPercent = 0, totalBrandPeice = 0;
                    foreach (DataRow row in dtMed.Rows)
                    {
                        strMarkup.Append("<tr>");
                        strMarkup.Append("<td class=\"bgOrange clrWhite\" style=\"width:20%;\">Branded</td>");
                        strMarkup.Append("<td style=\"width:40%; background:#eeeeee\">" + row["BrandMedicine"].ToString() + " <span class=\"semiBold\"><br/>(" + row["Packaging"].ToString() + ")</span></td>");
                        strMarkup.Append("<td style=\"width:10%; background:#eeeeee\">&#x20b9 " + Convert.ToDouble(row["BrandPrice"]).ToString("0.00") + "</td>");
                        double savings = Convert.ToDouble(row["BrandPrice"].ToString()) - Convert.ToDouble(row["GenericPrice"].ToString());
                        double savingPercent = (savings * 100) / Convert.ToDouble(row["BrandPrice"].ToString());
                        strMarkup.Append("<td style=\"width:10%; background:#eeeeee\" rowspan=\"2\" class=\"semiBold semiMedium\">&#x20b9 " + savings.ToString("0.00") + "</td>");
                        strMarkup.Append("<td style=\"width:10%; background:#eeeeee\" rowspan=\"2\" class=\"clrGreen semiBold semiMedium\">" + savingPercent.ToString("0.00") + "%</td>");
                        strMarkup.Append("<td style=\"width:10%; background:#eeeeee\" rowspan=\"2\"><a href=\"" + Master.rootPath + "obp/saving-calc.aspx?action=remove&id=" + row["CalcItemID"].ToString() + "\" class=\"deleteProd\"></a></td>");
                        strMarkup.Append("</tr>");
                        strMarkup.Append("<tr>");
                        strMarkup.Append("<td class=\"bg-primary clrWhite\">Generic</td>");
                        strMarkup.Append("<td style=\"background:#f6f6f6\">" + row["GenericMedicine"].ToString() + "</td>");
                        strMarkup.Append("<td style=\"background:#f6f6f6\">&#x20b9 " + Convert.ToDouble(row["GenericPrice"]).ToString("0.00") + "</td>");
                        strMarkup.Append("</tr>");
                        strMarkup.Append("<tr>");
                        strMarkup.Append("<td class=\"bgWhite\"><span class=\"space5\"></span></td>");
                        strMarkup.Append("</tr>");

                        totalBrandPeice = totalBrandPeice + Convert.ToDouble(row["BrandPrice"].ToString());
                        totalSavings = totalSavings + savings;
                        totalPercent = (totalSavings * 100) / totalBrandPeice;
                    }

                    totalSum = "&#x20b9 " + totalSavings.ToString("0.00");
                    totalPercentage = totalPercent.ToString("0.00") + "%";
                    medStr = strMarkup.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    protected void btnProceed_Click(object sender, EventArgs e)
    {
        try
        {
            int custId = 0;
            if (c.IsRecordExist("Select CustomrtID From CustomersData Where CustomerMobile='" + txtMobile.Text + "' AND delMark=0 AND CustomerActive=1"))
            {
                custId = Convert.ToInt32(c.GetReqData("CustomersData", "CustomrtID", " CustomerMobile='" + txtMobile.Text + "' AND delMark=0 AND CustomerActive=1"));
            }

            int mreqFlag = chkMreq.Checked == true ? 1 : 0;
            int calcId = Convert.ToInt32(c.GetReqData("SavingCalc", "CalcID", "MobileNumber='" + txtMobile.Text + "' AND EnqStatus IN (0, 1) AND (CONVERT(varchar(20), CAST (CalcDate AS DATE), 112) = CONVERT(varchar(20), CAST ('" + DateTime.Now + "' AS DATE), 112))"));
            c.ExecuteQuery("Update SavingCalc Set MreqFlag=" + mreqFlag + ", FK_CustId=" + custId + " Where CalcID=" + calcId);
            AddressBox.Visible = true;
            Session["calcId"] = calcId.ToString();
            FillMedTable(calcId);

            
            if (c.IsRecordExist("Select AddressID From CustomersAddress Where AddressFKCustomerID=" + custId))
            {
                c.FillComboBox("AddressFull", "AddressID", "CustomersAddress", "AddressFKCustomerID=" + custId + "", "AddressFull", 0, ddrAddress);
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    protected void btnSubmitOrder_Click(object sender, EventArgs e)
    {
        try
        {
            int calcId = Convert.ToInt32(Session["calcId"]);

            txtCountry.Text = txtCountry.Text.Trim().Replace("'", "");
            txtState.Text = txtState.Text.Trim().Replace("'", "");
            txtCity.Text = txtCity.Text.Trim().Replace("'", "");
            txtPinCode.Text = txtPinCode.Text.Trim().Replace("'", "");
            txtAddress1.Text = txtAddress1.Text.Trim().Replace("'", "");

            int addressId = 0;
            if (ddrAddress.SelectedIndex > 0)
            {
                addressId = Convert.ToInt32(ddrAddress.SelectedValue);
            }
            else
            {
                if (txtAddress1.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select or add address to continue');", true);
                    return;
                }
                if (txtCountry.Text == "" ||
                    txtState.Text == "" || txtCity.Text == "" || txtPinCode.Text == "" || txtAddress1.Text == "" || ddrAddrName.SelectedIndex == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * Marked fields are Mandatory');", true);
                    return;
                }
                if (!c.IsNumeric(txtPinCode.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Pincode must be numeric value');", true);
                    return;
                }

                int maxAddrId = c.NextId("CustomersAddress", "AddressID");
                c.ExecuteQuery("Insert Into CustomersAddress (AddressID, AddressFKCustomerID, AddressFull, AddressCity, AddressState, " +
                    " AddressPincode, AddressCountry, AddressStatus, AddressName) Values (" + maxAddrId + ", " + Request.QueryString["custId"] + ", '" + txtAddress1.Text +
                    "', '" + txtCity.Text + "', '" + txtState.Text + "', '" + txtPinCode.Text + "', '" + txtCountry.Text + "', 1, '" + ddrAddrName.SelectedItem.Text + "')");
                addressId = maxAddrId;
            }

            c.ExecuteQuery("Update SavingCalc Set EnqStatus=1, FK_AddressId=" + addressId + " Where CalcID=" + calcId);
            Session["calcId"] = null;
            Session["calc"] = null;
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Enquiry Submitted Successfully !');", true);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('dashboard.aspx', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSubmitOrder_Click", ex.Message.ToString());
            return;
        }
    }
}