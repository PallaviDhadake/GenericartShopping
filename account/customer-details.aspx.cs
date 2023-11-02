using PickupRegistrationServiceRef;
using Razorpay.Api;
using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Globalization;

public partial class account_customer_details : System.Web.UI.Page
{
    iClass c = new iClass();

    double billAmount = 0;
    double SGST = 0;
    double CGST = 0;
    double totalGST = 0;
    double amount = 0;
    double incentiveAmount = 0;
    public string[] custData = new string[5];
    public string errMsg;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                if (Request.QueryString["id"].ToString() != null)
                {
                    GetCustDetail(Convert.ToInt32(Request.QueryString["id"]));
                    FillGrid();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occured While Processing');", true);
                c.ErrorLogHandler(this.ToString(), "Page_Load", ex.Message.ToString());
                return;
            }
        }
    }

    private void GetCustDetail(int obpid)
    {
        try
        {
            using (DataTable dtEnq = c.GetDataTable("Select * From OBPData Where OBP_ID=" + obpid))
            {
                if (dtEnq.Rows.Count > 0)
                {
                    DataRow row = dtEnq.Rows[0];

                    custData[0] = row["OBP_ApplicantName"] != DBNull.Value ? row["OBP_ApplicantName"].ToString() : "";

                    custData[1] = Request.QueryString["month"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetCustDetail", ex.Message.ToString());
            return;
        }
    }

    private void FillGrid()
    {
        try
        {
            string strQuery;
            string[] arrYear = Request.QueryString["month"].ToString().Split('-');
            string monthData = arrYear[0].ToString();
            string yearData = arrYear[1].ToString();

            monthData = monthData.Trim();
            int monthNumber = 0;
            switch (monthData.ToLower())
            {
                case "january":
                    monthNumber = 1;
                    break;
                case "february":
                    monthNumber = 2;
                    break;
                case "march":
                    monthNumber = 3;
                    break;
                case "april":
                    monthNumber = 4;
                    break;
                case "may":
                    monthNumber = 5;
                    break;
                case "june":
                    monthNumber = 6;
                    break;
                case "july":
                    monthNumber = 7;
                    break;
                case "august":
                    monthNumber = 8;
                    break;
                case "september":
                    monthNumber = 9;
                    break;
                case "october":
                    monthNumber = 10;
                    break;
                case "november":
                    monthNumber = 11;
                    break;
                case "december":
                    monthNumber = 12;
                    break;
                default:
                    // Invalid month string
                    break;
            }

            //strQuery = @"SELECT
            //                CD.[CustomerName] AS CustomerName,
            //             OD.[OrderId] AS SaleBill,
            //             OD.[OrderAmount] AS TotalSaleBill,                                            
            //                PD.[ProductName] AS ProductList,
            //                ODT.[OrdDetailAmount] AS BillAmount,
            //                ISNULL(ROUND((ODT.[OrdDetailAmount] - (PD.[TaxLessAmount] * ODT.[OrdDetailQTY])),2),0) AS TotalGST,
            //                ISNULL((PD.[TaxLessAmount] * ODT.[OrdDetailQTY]),0) AS Amount,
            //                ODT.[OBPComPercent] AS GOBPIncentive,
            //                ISNULL(ROUND(((ODT.[OBPComPercent] / CAST(100 AS DECIMAL(10,2)) * (PD.[TaxLessAmount] * ODT.[OrdDetailQTY]))),2),0) AS IncentiveAmount                                        

            //            FROM [dbo].[OrdersData] AS OD
            //            join [dbo].[OrdersDetails] AS ODT on ODT.[FK_DetailOrderID] = OD.[OrderID]
            //            join [dbo].[CustomersData] AS CD on OD.[FK_OrderCustomerID] = CD.[CustomrtID]
            //            join [dbo].[ProductsData] AS PD on ODT.[FK_DetailProductID] = PD.[ProductID]
            //            join [dbo].[OBPData] AS OBP on OD.[GOBPId] = OBP.[OBP_ID]
            //            where OD.[GOBPId] IS NOT NULL and OD.[GOBPId] != '0' and ODT.[OBPComPercent] IS NOT NULL and ODT.[OBPComPercent] > 0
            //            AND(YEAR(OD.[OrderDate]) = " + yearData + " And MONTH(OD.[OrderDate]) = " + monthNumber + ") AND OBP.[OBP_ID] = " + Request.QueryString["id"].ToString() + " order by PD.[ProductName] ASC";

            strQuery = @"SELECT
                            CD.[CustomerName] AS CustomerName,
	                        OD.[OrderId] AS OrderID,
                            OD.[OrderSalesBillNumber] AS SaleBill,
	                        OD.[OrderAmount] AS TotalSaleBill,                                            
                            PD.[ProductName] AS ProductList,
                            ODT.[OrdDetailAmount] AS BillAmount,
                            ISNULL(ROUND((ODT.[OrdDetailAmount] - (PD.[TaxLessAmount] * ODT.[OrdDetailQTY])),2),0) AS TotalGST,
                            ISNULL((PD.[TaxLessAmount] * ODT.[OrdDetailQTY]),0) AS Amount,
                            ODT.[OBPComPercent] AS GOBPIncentive,
                            ISNULL(ODT.OBPComAmt, 0) AS IncentiveAmount                                        
                            
                        FROM [dbo].[OrdersData] AS OD
                        join [dbo].[OrdersDetails] AS ODT on ODT.[FK_DetailOrderID] = OD.[OrderID]
                        join [dbo].[CustomersData] AS CD on OD.[FK_OrderCustomerID] = CD.[CustomrtID]
                        join [dbo].[ProductsData] AS PD on ODT.[FK_DetailProductID] = PD.[ProductID]
                        join [dbo].[OBPData] AS OBP on OD.[GOBPId] = OBP.[OBP_ID]
                        where OD.[GOBPId] IS NOT NULL and OD.[GOBPId] != '0' 
                        AND [OrderStatus] Not in ('0', '2', '4', '8', '9', '10') AND (YEAR(OD.[OrderDate]) = " + yearData + " And MONTH(OD.[OrderDate]) = " + monthNumber + ") AND OBP.[OBP_ID] = " + Request.QueryString["id"].ToString() + " order by PD.[ProductName] ASC";

            using (DataTable dtGOBPIncentive = c.GetDataTable(strQuery))
            {
                gvGOBPIncentive.DataSource = dtGOBPIncentive;
                gvGOBPIncentive.DataBind();

                if (gvGOBPIncentive.Rows.Count > 0)
                {
                    gvGOBPIncentive.UseAccessibleHeader = true;
                    gvGOBPIncentive.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "Page_Load", ex.Message.ToString());
            return;
        }
    }

    protected void gvGOBPIncentive_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!Convert.IsDBNull(DataBinder.Eval(e.Row.DataItem, "BillAmount")))
                {
                    billAmount += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "BillAmount"));
                }

                if (!Convert.IsDBNull(DataBinder.Eval(e.Row.DataItem, "TotalGST")))
                {
                    totalGST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "TotalGST"));
                }

                if (!Convert.IsDBNull(DataBinder.Eval(e.Row.DataItem, "Amount")))
                {
                    amount += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Amount"));
                }

                if (!Convert.IsDBNull(DataBinder.Eval(e.Row.DataItem, "IncentiveAmount")))
                {
                    incentiveAmount += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "IncentiveAmount"));
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[4].Text = "Total";
                e.Row.Cells[4].Font.Bold = true;

                e.Row.Cells[5].Text = billAmount.ToString();
                e.Row.Cells[5].Font.Bold = true;

                e.Row.Cells[6].Text = totalGST.ToString();
                e.Row.Cells[6].Font.Bold = true;

                e.Row.Cells[7].Text = amount.ToString();
                e.Row.Cells[7].Font.Bold = true;

                e.Row.Cells[8].Text = "";
                e.Row.Cells[8].Font.Bold = true;

                e.Row.Cells[9].Text = incentiveAmount.ToString();
                e.Row.Cells[9].Font.Bold = true;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvGOBPIncentive_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

    protected void btnExportOrders_Click(object sender, EventArgs e)
    {
        ExportOrdersToExcel();
    }

    private void ExportOrdersToExcel()
    {
        try
        {
            string[] arrYear = Request.QueryString["month"].ToString().Split('-');
            string monthData = arrYear[0].Trim();
            string yearData = arrYear[1].Trim();

            int monthNumber = GetMonthNumber(monthData);

            // This actually makes your HTML output to be downloaded as .xls file
            Response.Clear();
            //Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename = GOBP_Incentive_Report.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                string headerText = "GOBP Incentive Report - " + Request.QueryString["month"].ToString();
                AddExcelHeader(hw, headerText);

                GetCustDetail(Convert.ToInt32(Request.QueryString["id"]));

                string custName = custData[0].ToString();
                AddExcelHeader1(hw, custName);

                //Get Orders in data table
                string GOBPReport = @"SELECT
                                            CD.[CustomerName] AS CustomerName,
	                                        OD.[OrderId] AS OrderID,
                                            OD.[OrderSalesBillNumber] AS SaleBill,
	                                        OD.[OrderAmount] AS TotalSaleBill,                                            
                                            PD.[ProductName] AS ProductList,
                                            ODT.[OrdDetailAmount] AS BillAmount,
                                            ISNULL(ROUND((ODT.[OrdDetailAmount] - (PD.[TaxLessAmount] * ODT.[OrdDetailQTY])),2),0) AS TotalGST,
                                            ISNULL((PD.[TaxLessAmount] * ODT.[OrdDetailQTY]),0) AS Amount,
                                            ODT.[OBPComPercent] AS GOBPIncentive,
                                            ISNULL(ROUND(((ODT.[OBPComPercent] / CAST(100 AS DECIMAL(10,2)) * (PD.[TaxLessAmount] * ODT.[OrdDetailQTY]))),2),0) AS IncentiveAmount                                            
                                            
                                        FROM [dbo].[OrdersData] AS OD
                                        join [dbo].[OrdersDetails] AS ODT on ODT.[FK_DetailOrderID] = OD.[OrderID]
                                        join [dbo].[CustomersData] AS CD on OD.[FK_OrderCustomerID] = CD.[CustomrtID]
                                        join [dbo].[ProductsData] AS PD on ODT.[FK_DetailProductID] = PD.[ProductID]
                                        join [dbo].[OBPData] AS OBP on OD.[GOBPId] = OBP.[OBP_ID]
                                        where OD.[GOBPId] IS NOT NULL and OD.[GOBPId] != '0' and ODT.[OBPComPercent] IS NOT NULL and ODT.[OBPComPercent] > 0
                                        AND(YEAR(OD.[OrderDate]) = " + yearData + " And MONTH(OD.[OrderDate]) = " + monthNumber + ") AND OBP.[OBP_ID] = " + Request.QueryString["id"].ToString() + " order by PD.[ProductName] ASC";


                using (DataTable dtGOBPIncentive = c.GetDataTable(GOBPReport))
                {
                    if (dtGOBPIncentive.Rows.Count > 0)
                    {
                        gvGOBPIncentive.DataSource = dtGOBPIncentive;
                        gvGOBPIncentive.DataBind();

                        // To export all pages
                        gvGOBPIncentive.AllowPaging = false;

                        gvGOBPIncentive.HeaderRow.BackColor = Color.Black;
                        gvGOBPIncentive.HeaderRow.ForeColor = Color.White;


                        foreach (TableCell cell in gvGOBPIncentive.HeaderRow.Cells)
                        {
                            cell.BackColor = gvGOBPIncentive.HeaderStyle.BackColor;
                        }

                        foreach (GridViewRow row in gvGOBPIncentive.Rows)
                        {
                            row.BackColor = Color.White;
                            row.BorderColor = Color.LightGray;
                            row.BorderWidth = Unit.Parse("1px");
                            foreach (TableCell cell in row.Cells)
                            {
                                if (row.RowIndex % 2 == 0)
                                {
                                    cell.BackColor = Color.WhiteSmoke;
                                }
                                else
                                {
                                    cell.BackColor = Color.White;
                                }
                            }
                        }

                        gvGOBPIncentive.RenderControl(hw);

                        //style to format numbers to string
                        //string style = @"<style> .textmode { } </style>";
                        //Response.Write(style);
                        Response.Output.Write(sw.ToString());
                        Response.Flush();
                        Response.End();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    private void AddExcelHeader(HtmlTextWriter writer, string headerText)
    {
        writer.Write("<table>");
        writer.Write("<tr>");
        writer.Write("<td colspan='9' style='font-weight:bold; font-size:14px; text-align:center; background-color:orange;'>");
        writer.Write(headerText);
        writer.Write("</td>");
        writer.Write("</tr>");
        writer.Write("</table>");
    }

    private void AddExcelHeader1(HtmlTextWriter writer, string custName)
    {
        writer.Write("<table>");
        writer.Write("<tr>");
        writer.Write("<td colspan='9' style='font-weight:bold; font-size:14px; text-align:center; background-color:orange;'>");
        writer.Write(custName);
        writer.Write("</td>");
        writer.Write("</tr>");
        writer.Write("</table>");
    }

    private int GetMonthNumber(string monthData)
    {
        switch (monthData.ToLower())
        {
            case "january":
                return 1;
            case "february":
                return 2;
            case "march":
                return 3;
            case "april":
                return 4;
            case "may":
                return 5;
            case "june":
                return 6;
            case "july":
                return 7;
            case "august":
                return 8;
            case "september":
                return 9;
            case "october":
                return 10;
            case "november":
                return 11;
            case "december":
                return 12;
            default:
                return 0; // Invalid month string
        }
    }
}