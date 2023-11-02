using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Drawing;
using System.Configuration;
using Razorpay.Api;

public partial class obpmanager_gobp_incentive_report : System.Web.UI.Page
{
    iClass c = new iClass();

    int customerCount = 0;
    int orderCount = 0;
    double orderAmount = 0.0;
    double orderTaxAmount = 0.0;
    public string errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                FillDDR();
                if (ddrMonth.SelectedIndex != 0)
                {

                }
                else
                {
                    DateTime tempDate = DateTime.Now.AddMonths(0);
                    ddrMonth.SelectedValue = tempDate.Month.ToString();
                }
                FillGrid();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occured While Processing');", true);
                c.ErrorLogHandler(this.ToString(), "Page_Load", ex.Message.ToString());
                return;
            }
        }
    }

    private void FillDDR()
    {
        try
        {
            DateTime endDate = DateTime.Now;
            DateTime startDate = DateTime.Now;

            endDate = DateTime.Now.AddMonths(0);
            startDate = endDate.AddMonths(-5);

            SqlConnection con = new SqlConnection(c.OpenConnection());

            SqlDataAdapter da = new SqlDataAdapter("DECLARE @start DATE = '" + startDate + "'  , @end DATE = '" + endDate + "'  ;WITH Numbers (Number) AS " +
                        " (SELECT ROW_NUMBER() OVER (ORDER BY OBJECT_ID) FROM sys.all_objects) " +
                        " SELECT DATENAME(MONTH,DATEADD(MONTH, Number - 1, @start)) +  ' - ' + DATENAME(YEAR,DATEADD(MONTH, Number - 1, @start)) as monthY, " +
                        " MONTH(DATEADD(MONTH, Number - 1, @start)) MonthId FROM Numbers  a WHERE Number - 1 <= DATEDIFF(MONTH, @start, @end) ", con);

            DataSet ds = new DataSet();
            da.Fill(ds, "myCombo");

            ddrMonth.DataSource = ds.Tables[0];
            ddrMonth.DataTextField = ds.Tables[0].Columns["monthY"].ColumnName.ToString();
            ddrMonth.DataValueField = ds.Tables[0].Columns["MonthId"].ColumnName.ToString();
            ddrMonth.DataBind();

            ddrMonth.Items.Insert(0, "<-Select Month->");
            ddrMonth.Items[0].Value = "0";

            con.Close();
            con = null;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FIllDDR", ex.Message.ToString());
            return;
        }
    }

    private void FillGrid()
    {
        try
        {
            string strQuery = "";
            string[] arrYear = ddrMonth.SelectedItem.Text.ToString().Split('-');
            string yearData = arrYear[1].ToString();

            if (ddrMonth.SelectedIndex != 0)
            {
                strQuery = @"SELECT 
                                OP.[OBP_ID],
                                OP.[OBP_ApplicantName],
                                OP.[OBP_UserID],
                                ISNULL((SELECT COUNT([CustomrtID]) FROM [dbo].[CustomersData] WHERE [delMark] = 0 AND [CustomerActive] = 1 
                                     AND [FK_ObpID] = OP.[OBP_ID]), 0) as custCount,
                                ISNULL((SELECT COUNT([OrderID]) FROM [dbo].[OrdersData] WHERE [GOBPId] = OP.[OBP_ID] 
                                     AND(YEAR([OrderDate]) = " + yearData + " And MONTH([OrderDate]) = '" + Convert.ToInt32(ddrMonth.SelectedValue) + "') AND [OrderStatus]Not in ('0', '2', '4', '8', '9', '10', '11') "
                                    + "GROUP BY [GOBPId]), 0) as TotalOrder, "
                                    + " ROUND(ISNULL((SELECT SUM(OrderAmount) From OrdersData where GOBPId = OP.[OBP_ID] AND MONTH([OrderDate]) = " + Convert.ToInt32(ddrMonth.SelectedValue) + " AND YEAR([OrderDate]) = " + yearData + " AND [OrderStatus] Not in ('0', '2', '4', '8', '9', '10', '11')), 0), 2) as OrderTotalAmount, "
                                    + " ISNULL((SELECT SUM(OBPComTotal) From OrdersData where GOBPId = OP.[OBP_ID] AND MONTH([OrderDate]) = " + Convert.ToInt32(ddrMonth.SelectedValue) + " AND YEAR([OrderDate]) = " + yearData + " AND [OrderStatus] Not in ('0', '2', '4', '8', '9', '10', '11')), 0) as OrderAmount"
                                    + " FROM [dbo].[OBPData] as OP"
                                    + " ORDER BY TotalOrder DESC, OrderAmount DESC";
            }
            else
            {
                strQuery = @"SELECT 
                                OP.[OBP_ID],
                                OP.[OBP_ApplicantName],
                                OP.[OBP_UserID],
                                ISNULL((SELECT COUNT([CustomrtID]) FROM [dbo].[CustomersData] WHERE [delMark] = 0 AND [CustomerActive] = 1 
                                     AND [FK_ObpID] = OP.[OBP_ID]), 0) as custCount,
                                ISNULL((SELECT COUNT([OrderID]) FROM [dbo].[OrdersData] WHERE [GOBPId] = OP.[OBP_ID] 
                                     AND MONTH([OrderDate]) = MONTH(GETDATE()) AND YEAR([OrderDate]) = YEAR(GETDATE()) AND [OrderStatus] Not in ('0', '2', '4', '8', '9', '10', '11')
                                     GROUP BY [GOBPId]), 0) as TotalOrder,
                                ISNULL((SELECT SUM(OrderAmount) From OrdersData where GOBPId = OP.[OBP_ID] AND MONTH([OrderDate]) =  MONTH(GETDATE()) AND YEAR([OrderDate]) = YEAR(GETDATE()) AND[OrderStatus] Not in ('0', '2', '4', '8', '9', '10', '11')), 0) as OrderTotalAmount,
                                ISNULL((SELECT SUM(OBPComTotal) From OrdersData where GOBPId = OP.[OBP_ID] AND MONTH([OrderDate]) =  MONTH(GETDATE()) AND YEAR([OrderDate]) = YEAR(GETDATE()) AND[OrderStatus] Not in ('0', '2', '4', '8', '9', '10', '11')), 0) as OrderAmount
                            FROM [dbo].[OBPData] as OP
                            ORDER BY TotalOrder DESC, OrderAmount DESC";
            }

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
            c.ErrorLogHandler(this.ToString(), "FillGrid", ex.Message.ToString());
            return;
        }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        if (ddrMonth.SelectedIndex == 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Select Month & Year to fetch data', type: 'warning'});", true);
            return;
        }
        FillGrid();
    }

    protected void gvGOBPIncentive_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            string[] arrYear = ddrMonth.SelectedItem.Text.ToString().Split('-');
            string yearData = arrYear[1].ToString();


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"customer_details.aspx?id=" + e.Row.Cells[0].Text + "&month=" + ddrMonth.SelectedItem.Text + "\" target=\"_blank\" class=\"frStats btn btn-sm btn-default\" data-fancybox-type=\"iframe\"><i class=\"fa fa-eye\" aria-hidden=\"true\"></i></a>";
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                customerCount += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "custCount"));
                orderCount += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TotalOrder"));
                orderAmount += Math.Round(Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "OrderAmount")), 2);
                orderTaxAmount += Math.Round(Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "OrderTotalAmount")), 2);

                //double roundedDecimal = Math.Round(number, 2);

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Text = "Total";
                e.Row.Cells[1].Font.Bold = true;

                e.Row.Cells[2].Text = customerCount.ToString();
                e.Row.Cells[2].Font.Bold = true;

                e.Row.Cells[3].Text = orderCount.ToString();
                e.Row.Cells[3].Font.Bold = true;

                e.Row.Cells[4].Text = orderTaxAmount.ToString();
                e.Row.Cells[4].Font.Bold = true;

                e.Row.Cells[5].Text = orderAmount.ToString();
                e.Row.Cells[5].Font.Bold = true;

                e.Row.Cells[6].Text = "";
                e.Row.Cells[6].Font.Bold = true;

            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvGOBPIncentive_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    protected void btnExportOrders_Click(object sender, EventArgs e)
    {
        ExportOrdersToExcel();
    }

    private void ExportOrdersToExcel()
    {
        try
        {
            string[] arrYear = ddrMonth.SelectedItem.Text.ToString().Split('-');
            string yearData = arrYear[1].ToString();

            // This actually makes your HTML output to be downloaded as .xls file
            Response.Clear();
            //Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename = GOBP_Incentive.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                string headerText = "GOBP Incentive Report - " + ddrMonth.SelectedItem.Text;
                AddExcelHeader(hw, headerText);

                //Get Orders in data table
                string GOBPReport = @"SELECT 
                                OP.[OBP_ID],
                                OP.[OBP_ApplicantName],
                                OP.[OBP_UserID],
                                ISNULL((SELECT COUNT([CustomrtID]) FROM [dbo].[CustomersData] WHERE [delMark] = 0 AND [CustomerActive] = 1 
                                     AND [FK_ObpID] = OP.[OBP_ID]), 0) as custCount,
                                ISNULL((SELECT COUNT([OrderID]) FROM [dbo].[OrdersData] WHERE [GOBPId] = OP.[OBP_ID] 
                                     AND(YEAR([OrderDate]) = " + yearData + " And MONTH([OrderDate]) = '" + Convert.ToInt32(ddrMonth.SelectedValue) + "') AND [OrderStatus] Not in ('0', '2', '4', '8', '9', '10', '11') "
                                     + "GROUP BY [GOBPId]), 0) as TotalOrder, "
                                     + " ROUND(ISNULL((SELECT SUM(OrderAmount) From OrdersData where GOBPId = OP.[OBP_ID] AND MONTH([OrderDate]) = " + Convert.ToInt32(ddrMonth.SelectedValue) + " AND YEAR([OrderDate]) = " + yearData + " AND [OrderStatus] Not in ('0', '2', '4', '8', '9', '10', '11')), 0), 2) as OrderTotalAmount, "
                                     + " ISNULL((SELECT SUM(OBPComTotal) From OrdersData where GOBPId = OP.[OBP_ID] AND MONTH([OrderDate]) = " + Convert.ToInt32(ddrMonth.SelectedValue) + " AND YEAR([OrderDate]) = " + yearData + " AND [OrderStatus] Not in ('0', '2', '4', '8', '9', '10', '11')), 0) as OrderAmount"
                                    + " FROM [dbo].[OBPData] as OP"
                                    + " ORDER BY TotalOrder DESC, OrderAmount DESC";

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

                            gvGOBPIncentive.Columns[7].Visible = false;
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
        writer.Write("<td colspan='7' style='font-weight:bold; font-size:14px; text-align:center; background-color: orange;'>");
        writer.Write(headerText);
        writer.Write("</td>");
        writer.Write("</tr>");
        writer.Write("</table>");
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
}