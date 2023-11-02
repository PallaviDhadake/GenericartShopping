using Razorpay.Api;
using System;
using System.Activities.Expressions;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GOBPDH_gobpdh_report : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        Bind_GOBP_Order();

        if (txtFromDate.Text != string.Empty && txtToDate.Text != string.Empty)
        {
            DateTime fromDate;
            string[] arrFromDate = txtFromDate.Text.Split('/');
            fromDate = Convert.ToDateTime(arrFromDate[1] + "/" + arrFromDate[0] + "/" + arrFromDate[2]);

            DateTime toDate;
            string[] arrToDate = txtToDate.Text.Split('/');
            toDate = Convert.ToDateTime(arrToDate[1] + "/" + arrToDate[0] + "/" + arrToDate[2]);
            litDate.Text = fromDate.ToString("dd/MM/yyyy") + " - " + toDate.ToString("dd/MM/yyyy");
        }
        else
        {
            string dateRange = c.GetFinancialYear();
            string[] arrDateRange = dateRange.ToString().Split('#');
            DateTime myFromDate = Convert.ToDateTime(arrDateRange[0]);
            DateTime myToDate = Convert.ToDateTime(arrDateRange[1]);
            litDate.Text = myFromDate.ToString("dd/MM/yyyy") + " - " + DateTime.Now.ToString("dd/MM/yyyy");
        }
    }

    private void Bind_GOBP_Order()
    {
        try
        {
            string gobpuser = c.GetReqData("[dbo].[DistrictHead]", "[DistHdUserId]", "[DistHdId] = " + Session["adminGOBPDH"]).ToString();

            string strQuery = "";

            string dateRange = c.GetFinancialYear();
            string[] arrDateRange = dateRange.ToString().Split('#');
            DateTime myFromDate = Convert.ToDateTime(arrDateRange[0]);
            DateTime myToDate = Convert.ToDateTime(arrDateRange[1]);

            if (txtFromDate.Text != string.Empty && txtToDate.Text != string.Empty)
            {
                DateTime fromDate;
                string[] arrFromDate = txtFromDate.Text.Split('/');
                fromDate = Convert.ToDateTime(arrFromDate[1] + "/" + arrFromDate[0] + "/" + arrFromDate[2]);

                DateTime toDate;
                string[] arrToDate = txtToDate.Text.Split('/');
                toDate = Convert.ToDateTime(arrToDate[1] + "/" + arrToDate[0] + "/" + arrToDate[2]);

                strQuery = @"WITH FinancialYearData AS (
                                 SELECT
                                     OD.[GOBPId],
                                     COUNT(OD.[OrderID]) AS TotalOrders,
                                     SUM(OD.[OrderAmount]) AS TotalAmount
                                 FROM [dbo].[OrdersData] AS OD
                                 WHERE
                                     OD.[GOBPId] IS NOT NULL AND OD.[GOBPId] > 0
                                     AND OD.[OrderDate] >= '" + fromDate + "' AND OD.[OrderDate] <= '" + toDate + "'"
                                 + "GROUP BY OD.[GOBPId] "
                             + ") "
                             + " SELECT DISTINCT"
                                 + " MAX(OP.[OBP_ID]) AS GOBPID,"
                                 + " MAX(OP.[OBP_EmpId]) AS OBP_EmpId,"
                                 + " MAX(OP.[OBP_UserID]) AS GOBPUser,"
                                 + " MAX(OP.[OBP_ApplicantName]) AS Name,"
                                 + " MAX(OP.[OBP_MobileNo]) AS MobileNo,"
                                 + " COUNT(DISTINCT CD.[CustomrtID]) as Customers,"
                                 + " ISNULL(FYD.TotalOrders, 0) AS TotalOrders,"
                                 + " ISNULL(FYD.TotalAmount, 0) AS TotalAmount"
                             + " FROM[dbo].[OBPData] as OP"
                             + " LEFT JOIN[dbo].[CustomersData] AS CD ON OP.[OBP_ID] = CD.[FK_ObpID]"
                             + " LEFT JOIN FinancialYearData AS FYD ON OP.[OBP_ID] = FYD.[GOBPId]"
                             + " WHERE"
                                 + " OP.[OBP_DH_UserId] = '" + gobpuser + "'"
                                 + " AND(FYD.TotalOrders > 0 OR FYD.TotalAmount > 0)"
                             + " GROUP BY OP.[OBP_ApplicantName], OP.[OBP_ID], OP.[OBP_MobileNo], FYD.TotalOrders, FYD.TotalAmount";
            }
            else if (txtFromDate.Text == string.Empty && txtToDate.Text == string.Empty)
            {
                strQuery = @"WITH FinancialYearData AS (
                                 SELECT
                                     OD.[GOBPId],
                                     COUNT(OD.[OrderID]) AS TotalOrders,
                                     SUM(OD.[OrderAmount]) AS TotalAmount
                                 FROM [dbo].[OrdersData] AS OD
                                 WHERE
                                     OD.[GOBPId] IS NOT NULL AND OD.[GOBPId] > 0
                                     AND OD.[OrderDate] >= '" + myFromDate + "' AND OD.[OrderDate] <= '" + DateTime.Now + "'"
                                 + "GROUP BY OD.[GOBPId] "
                             + ") "                             
                             + " SELECT DISTINCT"
                                 + " MAX(OP.[OBP_ID]) AS GOBPID,"
                                 + " MAX(OP.[OBP_UserID]) AS GOBPUser,"
                                 + " MAX(OP.[OBP_EmpId]) AS OBP_EmpId,"
                                 + " MAX(OP.[OBP_ApplicantName]) AS Name,"
                                 + " MAX(OP.[OBP_MobileNo]) AS MobileNo,"
                                 + " COUNT(DISTINCT CD.[CustomrtID]) as Customers,"
                                 + " ISNULL(FYD.TotalOrders, 0) AS TotalOrders,"
                                 + " ISNULL(FYD.TotalAmount, 0) AS TotalAmount"
                             + " FROM[dbo].[OBPData] as OP"
                             + " LEFT JOIN[dbo].[CustomersData] AS CD ON OP.[OBP_ID] = CD.[FK_ObpID]"
                             + " LEFT JOIN FinancialYearData AS FYD ON OP.[OBP_ID] = FYD.[GOBPId]"                             
                             + " WHERE"
                                 + " OP.[OBP_DH_UserId] = '" + gobpuser + "'"
                                 + " AND(FYD.TotalOrders > 0 OR FYD.TotalAmount > 0)"
                             + " GROUP BY OP.[OBP_ApplicantName], OP.[OBP_ID], OP.[OBP_MobileNo], FYD.TotalOrders, FYD.TotalAmount";
            }

            using (DataTable dtgobpOrd = c.GetDataTable(strQuery))
            {
                gvGOBP.DataSource = dtgobpOrd;
                gvGOBP.DataBind();
                if (dtgobpOrd.Rows.Count > 0)
                {
                    gvGOBP.UseAccessibleHeader = true;
                    gvGOBP.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "Bind_GOBP_Order", ex.Message.ToString());
            return;
        }
    }

    protected void gvGOBP_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litView = (Literal)e.Row.FindControl("litView");

                litView.Text = "<a href=\"gobpdh-detail.aspx?id=" + e.Row.Cells[0].Text + "\" class=\"gView\" ></a>";
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvGOBP_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime fromDate = DateTime.Now;
            string[] arrFromDate = txtFromDate.Text.Split('/');
            if (c.IsDate(arrFromDate[1] + "/" + arrFromDate[0] + "/" + arrFromDate[2]) == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid From Date');", true);
                return;
            }

            DateTime toDate = DateTime.Now;
            string[] arrToDate = txtToDate.Text.Split('/');
            if (c.IsDate(arrToDate[1] + "/" + arrToDate[0] + "/" + arrToDate[2]) == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid ToDate');", true);
            }

            Bind_GOBP_Order();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnShow_Click", ex.Message.ToString());
            return;
        }
    }
}