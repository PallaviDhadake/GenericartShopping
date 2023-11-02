using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class account_generic_mitra_incentive_report : System.Web.UI.Page
{
    iClass c = new iClass();

    double orderSum = 0;
    double commission = 0;
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
            startDate = endDate.AddMonths(-4);

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

            ddrMonth.Items.Insert(0, "<-Select->");
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
            string genmitraIds = "0";
            using (DataTable dtCust = c.GetDataTable(@"SELECT 
                                                           [CustomrtID],
                                                           [FK_GenMitraID]
                                                       FROM[dbo].[CustomersData]
                                                       WHERE[FK_GenMitraID] IS NOT NULL
                                                       AND[FK_GenMitraID] <> ''
                                                       AND[FK_GenMitraID] <> 0"))
            {
                if (dtCust.Rows.Count > 0)
                {
                    foreach (DataRow row in dtCust.Rows)
                    {
                        if (row["FK_GenMitraID"] != DBNull.Value && row["FK_GenMitraID"] != null && row["FK_GenMitraID"].ToString() != "")
                        {
                            if (genmitraIds == "")
                                genmitraIds = row["FK_GenMitraID"].ToString();
                            else
                                genmitraIds = genmitraIds + "," + row["FK_GenMitraID"].ToString();
                        }
                        else
                        {
                            genmitraIds = "0";
                        }
                    }
                }
            }

            string strQuery = "";
            string[] arrYear = ddrMonth.SelectedItem.Text.ToString().Split('-');
            string yearData = arrYear[1].ToString();

            if (ddrMonth.SelectedIndex != 0)
            {
                strQuery = @"SELECT 
                                   a.[GMitraID],
                                   CONVERT(VARCHAR(20), a.[GMitraDate], 103) as rDate,
                                   a.[GMitraName],
                                   a.[GMitraMobile],
                             	   a.[GMitraShopCode],
                             	   (SELECT [FranchName] FROM [dbo].[FranchiseeData] WHERE [FranchShopCode] = a.[GMitraShopCode]) as franchiseeName,
                                   ISNULL((SELECT COUNT([CustomrtID]) FROM [dbo].[CustomersData] WHERE [delMark] = 0 AND [CustomerActive] = 1 AND [FK_GenMitraID] = a.[GMitraID]),0) as custCount,
                             	   ISNULL((Select COUNT(DISTINCT c.[OrderID]) FROM [dbo].[OrdersData] c INNER JOIN [dbo].[CustomersData] b ON c.[FK_OrderCustomerID] = b.[CustomrtID] WHERE c.[OrderStatus] <> 0 AND b.[FK_GenMitraID] = a.[GMitraID] AND MONTH(c.[OrderDate]) = " + Convert.ToInt32(ddrMonth.SelectedValue) + " AND YEAR(c.[OrderDate]) = " + yearData + "), 0) as TotalOrder,"
                                    + "ISNULL((SELECT SUM([OrderAmount]) FROM [dbo].[OrdersData] WHERE [FK_OrderCustomerID] In (SELECT [CustomrtID] FROM [dbo].[CustomersData] WHERE [FK_GenMitraID] = a.[GMitraID] AND MONTH([OrderDate]) = " + Convert.ToInt32(ddrMonth.SelectedValue) + " AND YEAR([OrderDate]) = " + yearData + ")), 0) as ordValue,"
                                    + "ISNULL((((SELECT SUM([OrderAmount]) FROM [dbo].[OrdersData] WHERE [FK_OrderCustomerID] IN (SELECT [CustomrtID] FROM [dbo].[CustomersData] WHERE [FK_GenMitraID] = a.[GMitraID] AND MONTH([OrderDate]) = " + Convert.ToInt32(ddrMonth.SelectedValue) + " AND YEAR([OrderDate]) = " + yearData + ")) * 20) / 100) ,0) as Commission"
                             + " FROM[dbo].[GenericMitra] a"
                             + " WHERE a.[GMitraStatus] = 1"
                             + " AND a.[GMitraShopCode] IN ('GMMH1000', 'GMMH0437', 'GMMH0001', 'GMMH0585', 'GMMH0443', 'GMMH0428')";
            }
            else
            {
                strQuery = @"SELECT 
                                   a.[GMitraID],
                                   CONVERT(VARCHAR(20), a.[GMitraDate], 103) as rDate,
                                   a.[GMitraName],
                                   a.[GMitraMobile],
                             	   a.[GMitraShopCode],
                             	   (SELECT [FranchName] FROM [dbo].[FranchiseeData] WHERE [FranchShopCode] = a.[GMitraShopCode]) as franchiseeName,
                                   ISNULL((SELECT COUNT([CustomrtID]) FROM [dbo].[CustomersData] WHERE [delMark] = 0 AND [CustomerActive] = 1 AND [FK_GenMitraID] = a.[GMitraID]),0) as custCount,
                             	   ISNULL((Select COUNT(DISTINCT c.[OrderID]) FROM [dbo].[OrdersData] c INNER JOIN [dbo].[CustomersData] b ON c.[FK_OrderCustomerID] = b.[CustomrtID] WHERE c.[OrderStatus] <> 0 AND b.[FK_GenMitraID] = a.[GMitraID] AND MONTH(c.[OrderDate]) = MONTH(GETDATE()) AND YEAR(c.[OrderDate]) = YEAR(GETDATE())), 0) as TotalOrder,
                             	   ISNULL((SELECT SUM([OrderAmount]) FROM [dbo].[OrdersData] WHERE [FK_OrderCustomerID] In (SELECT [CustomrtID] FROM [dbo].[CustomersData] WHERE [FK_GenMitraID] = a.[GMitraID] AND MONTH([OrderDate]) = MONTH(GETDATE()) AND YEAR([OrderDate]) = YEAR(GETDATE()))), 0) as ordValue,
                             	   ISNULL((((SELECT SUM([OrderAmount]) FROM [dbo].[OrdersData] WHERE [FK_OrderCustomerID] IN (SELECT [CustomrtID] FROM [dbo].[CustomersData] WHERE [FK_GenMitraID] = a.[GMitraID] AND MONTH([OrderDate]) = MONTH(GETDATE()) AND YEAR([OrderDate]) = YEAR(GETDATE()))) * 20) / 100) ,0) as Commission
                             FROM[dbo].[GenericMitra] a
                             WHERE a.[GMitraStatus] = 1
                             AND a.[GMitraShopCode] IN ('GMMH1000', 'GMMH0437', 'GMMH0001', 'GMMH0585', 'GMMH0443', 'GMMH0428')";
            }

            using (DataTable dtGenMitra = c.GetDataTable(strQuery))
            {
                gvGenMitra.DataSource = dtGenMitra;
                gvGenMitra.DataBind();

                if (gvGenMitra.Rows.Count > 0)
                {
                    gvGenMitra.UseAccessibleHeader = true;
                    gvGenMitra.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvGenMitra_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            string[] arrYear = ddrMonth.SelectedItem.Text.ToString().Split('-');
            string yearData = arrYear[1].ToString();


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"generic-mitra-incentive-details.aspx?id=" + e.Row.Cells[0].Text + "&month=" + ddrMonth.SelectedItem.Text + "\" class=\"frStats btn btn-sm btn-default\" data-fancybox-type=\"iframe\"><i class=\"fa fa-eye\" aria-hidden=\"true\"></i></a>";
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                orderSum += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ordValue"));
                commission += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Commission"));
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[4].Text = "Total";
                e.Row.Cells[4].Font.Bold = true;

                e.Row.Cells[5].Text = "";
                e.Row.Cells[5].Font.Bold = true;

                e.Row.Cells[6].Text = "";
                e.Row.Cells[6].Font.Bold = true;

                e.Row.Cells[7].Text = orderSum.ToString();
                e.Row.Cells[7].Font.Bold = true;

                e.Row.Cells[8].Text = commission.ToString();
                e.Row.Cells[8].Font.Bold = true;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvGenMitra_RowDataBound", ex.Message.ToString());
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
            Response.AddHeader("content-disposition", "attachment;filename = Generic_Mitra_Incentive.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                string headerText = "Generic Mitra Incentive Report - " + ddrMonth.SelectedItem.Text;
                AddExcelHeader(hw, headerText);

                //Get Orders in data table
                string GenMitraReport = @"SELECT 
                                   a.[GMitraID],
                                   CONVERT(VARCHAR(20), a.[GMitraDate], 103) as rDate,
                                   a.[GMitraName],
                                   a.[GMitraMobile],
                             	   a.[GMitraShopCode],
                             	   (SELECT [FranchName] FROM [dbo].[FranchiseeData] WHERE [FranchShopCode] = a.[GMitraShopCode]) as franchiseeName,
                                   ISNULL((SELECT COUNT([CustomrtID]) FROM [dbo].[CustomersData] WHERE [delMark] = 0 AND [CustomerActive] = 1 AND [FK_GenMitraID] = a.[GMitraID]),0) as custCount,
                             	   ISNULL((Select COUNT(DISTINCT c.[OrderID]) FROM [dbo].[OrdersData] c INNER JOIN [dbo].[CustomersData] b ON c.[FK_OrderCustomerID] = b.[CustomrtID] WHERE c.[OrderStatus] <> 0 AND b.[FK_GenMitraID] = a.[GMitraID] AND MONTH(c.[OrderDate]) = " + Convert.ToInt32(ddrMonth.SelectedValue) + " AND YEAR(c.[OrderDate]) = " + yearData + "), 0) as TotalOrder,"
                                    + "ISNULL((SELECT SUM([OrderAmount]) FROM [dbo].[OrdersData] WHERE [FK_OrderCustomerID] In (SELECT [CustomrtID] FROM [dbo].[CustomersData] WHERE [FK_GenMitraID] = a.[GMitraID] AND MONTH([OrderDate]) = " + Convert.ToInt32(ddrMonth.SelectedValue) + " AND YEAR([OrderDate]) = " + yearData + ")), 0) as ordValue,"
                                    + "ISNULL((((SELECT SUM([OrderAmount]) FROM [dbo].[OrdersData] WHERE [FK_OrderCustomerID] IN (SELECT [CustomrtID] FROM [dbo].[CustomersData] WHERE [FK_GenMitraID] = a.[GMitraID] AND MONTH([OrderDate]) = " + Convert.ToInt32(ddrMonth.SelectedValue) + " AND YEAR([OrderDate]) = " + yearData + ")) * 20) / 100) ,0) as Commission"
                             + " FROM[dbo].[GenericMitra] a"
                             + " WHERE a.[GMitraStatus] = 1"
                             + " AND a.[GMitraShopCode] IN ('GMMH1000', 'GMMH0437', 'GMMH0001', 'GMMH0585', 'GMMH0443', 'GMMH0428')";

                using (DataTable dtGenMitraIncentive = c.GetDataTable(GenMitraReport))
                {
                    if (dtGenMitraIncentive.Rows.Count > 0)
                    {
                        gvGenMitra.DataSource = dtGenMitraIncentive;
                        gvGenMitra.DataBind();

                        // To export all pages
                        gvGenMitra.AllowPaging = false;

                        gvGenMitra.HeaderRow.BackColor = Color.Black;
                        gvGenMitra.HeaderRow.ForeColor = Color.White;


                        foreach (TableCell cell in gvGenMitra.HeaderRow.Cells)
                        {
                            cell.BackColor = gvGenMitra.HeaderStyle.BackColor;
                        }

                        foreach (GridViewRow row in gvGenMitra.Rows)
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

                            gvGenMitra.Columns[0].Visible = false;
                            gvGenMitra.Columns[10].Visible = false;
                        }

                        gvGenMitra.RenderControl(hw);

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
        writer.Write("<td colspan='9' style='font-weight:bold; font-size:14px; text-align:center; background-color: orange;'>");
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