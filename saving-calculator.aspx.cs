using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class saving_calculator : System.Web.UI.Page
{
    iClass c = new iClass();
    public string medStr = "", totalSum, totalPercentage, errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        btnCalculate.Attributes.Add("onclick", "this.disabled=true;this.value='Processing...';" + ClientScript.GetPostBackEventReference(btnCalculate, null) + ";");
        if (Session["genericCust"] != null)
        {
            string mob = c.GetReqData("CustomersData", "CustomerMobile", "CustomrtID=" + Session["genericCust"]).ToString();
            txtMobile.Text = mob.ToString();
        }
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
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('" + Master.rootPath + "saving-calculator', 1000);", true);
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
                        " Values (" + maxId + ", '" + DateTime.Now + "', '" + txtMobile.Text + "', " + customerId + ", 0, 0, 'Web')");
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
                        strMarkup.Append("<td style=\"width:40%;\">" + row["BrandMedicine"].ToString() + " <span class=\"semiBold\"><br/>(" + row["Packaging"].ToString() + ")</span></td>");
                        strMarkup.Append("<td style=\"width:10%;\">&#x20b9 " + Convert.ToDouble(row["BrandPrice"]).ToString("0.00") + "</td>");
                        double savings = Convert.ToDouble(row["BrandPrice"].ToString()) - Convert.ToDouble(row["GenericPrice"].ToString());
                        double savingPercent = (savings * 100) / Convert.ToDouble(row["BrandPrice"].ToString());
                        strMarkup.Append("<td style=\"width:10%;\" rowspan=\"2\" class=\"semiBold semiMedium\">&#x20b9 " + savings.ToString("0.00") + "</td>");
                        strMarkup.Append("<td style=\"width:10%;\" rowspan=\"2\" class=\"clrGreen semiBold semiMedium\">" + savingPercent.ToString("0.00") + "%</td>");
                        strMarkup.Append("<td style=\"width:10%;\" rowspan=\"2\"><a href=\"" + Master.rootPath + "saving-calculator?action=remove&id=" + row["CalcItemID"].ToString() + "\" class=\"deleteProd\"></a></td>");
                        strMarkup.Append("</tr>");
                        strMarkup.Append("<tr>");
                        strMarkup.Append("<td class=\"themeBgPrime clrWhite\">Generic</td>");
                        strMarkup.Append("<td>" + row["GenericMedicine"].ToString() + "</td>");
                        strMarkup.Append("<td>&#x20b9 " + Convert.ToDouble(row["GenericPrice"]).ToString("0.00") + "</td>");
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

    //private void FillMedTable(string mobileNo)
    //{
    //    try
    //    {
    //        using (DataTable dtMed = c.GetDataTable("Select * From SavingCalc Where MobileNumber='" + mobileNo + "' AND (CONVERT(varchar(20), CAST (CalcDate AS DATE), 112) = CONVERT(varchar(20), CAST ('" + DateTime.Now + "' AS DATE), 112))"))
    //        {
    //            if (dtMed.Rows.Count > 0)
    //            {
    //                StringBuilder strMarkup = new StringBuilder();
    //                double totalSavings = 0, totalPercent = 0, totalBrandPeice = 0;
    //                foreach (DataRow row in dtMed.Rows)
    //                {
    //                    strMarkup.Append("<tr>");
    //                    strMarkup.Append("<td class=\"bgOrange clrWhite\" style=\"width:20%;\">Branded</td>");
    //                    strMarkup.Append("<td style=\"width:40%;\">" + row["BrandMedicine"].ToString() + "</td>");
    //                    strMarkup.Append("<td style=\"width:10%;\">&#x20b9 " + row["BrandPrice"].ToString() + "</td>");
    //                    double savings = Convert.ToDouble(row["BrandPrice"].ToString()) - Convert.ToDouble(row["GenericPrice"].ToString());
    //                    double savingPercent = (savings * 100) / Convert.ToDouble(row["BrandPrice"].ToString());
    //                    strMarkup.Append("<td style=\"width:10%;\" rowspan=\"2\" class=\"semiBold semiMedium\">&#x20b9 " + savings + "</td>");
    //                    strMarkup.Append("<td style=\"width:10%;\" rowspan=\"2\" class=\"clrGreen semiBold semiMedium\">" + savingPercent.ToString("0.00") + "%</td>");
    //                    strMarkup.Append("</tr>");
    //                    strMarkup.Append("<tr>");
    //                    strMarkup.Append("<td class=\"themeBgPrime clrWhite\">Generic</td>");
    //                    strMarkup.Append("<td>" + row["GenericCode"].ToString() + "</td>");
    //                    strMarkup.Append("<td>&#x20b9 " + row["GenericPrice"].ToString() + "</td>");
    //                    strMarkup.Append("</tr>");
    //                    strMarkup.Append("<tr>");
    //                    strMarkup.Append("<td class=\"bgWhite\"><span class=\"space5 \"></span></td>");
    //                    strMarkup.Append("</tr>");

    //                    totalBrandPeice = totalBrandPeice + Convert.ToDouble(row["BrandPrice"].ToString());
    //                    totalSavings = totalSavings + savings;
    //                    totalPercent = (totalSavings * 100) / totalBrandPeice;
    //                }

    //                totalSum = "&#x20b9 " + totalSavings.ToString();
    //                totalPercentage = totalPercent.ToString("0.00") + "%";
    //                medStr = strMarkup.ToString();
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        errMsg = c.ErrNotification(3, ex.Message.ToString());
    //        return;
    //    }
    //}

    protected void btnProceed_Click(object sender, EventArgs e)
    {
        try
        {
            int mreqFlag = chkMreq.Checked == true ? 1 : 0;
            int calcId = Convert.ToInt32(c.GetReqData("SavingCalc", "CalcID", "MobileNumber='" + txtMobile.Text + "' AND EnqStatus IN (0, 1) AND (CONVERT(varchar(20), CAST (CalcDate AS DATE), 112) = CONVERT(varchar(20), CAST ('" + DateTime.Now + "' AS DATE), 112))"));
            c.ExecuteQuery("Update SavingCalc Set MreqFlag=" + mreqFlag + " Where CalcID=" + calcId);

            if (Session["genericCust"] != null)
            {
                Response.Redirect(Master.rootPath + "enquiry-checkout", false);
            }
            else
            {
                Response.Redirect(Master.rootPath + "login?ref=calc", false);
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}