using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GOBPDH_gobp_orders : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["type"] != null)
        {
            Bind_GOBP_Order();
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

            if (Request.QueryString["type"] != null)
            {
                if (Request.QueryString["type"] == "active")
                {
                    strQuery = @"SELECT DISTINCT 
                                	MAX(OP.[OBP_ID]) AS GOBPID,
                                	MAX(OP.[OBP_ApplicantName]) AS Name,
                                	MAX(OP.[OBP_MobileNo]) AS MobileNo,
                                	COUNT(DISTINCT CD.[CustomrtID]) as Customers,
                                	(SELECT COUNT([OrderID]) FROM [dbo].[OrdersData] WHERE [GOBPId] = MAX(OP.[OBP_ID]) AND YEAR([OrderDate]) = YEAR(GETDATE()) AND MONTH([OrderDate]) = MONTH(GETDATE())) AS MonthOrder,
                                	ISNULL((SELECT SUM([OrderAmount]) FROM [dbo].[OrdersData] WHERE [GOBPId] = MAX(OP.[OBP_ID]) AND YEAR([OrderDate]) = YEAR(GETDATE()) AND MONTH([OrderDate]) = MONTH(GETDATE())),0) AS MonthAmount,	
                                	COUNT(OD.[OrderID]) AS TotalOrders,
                                	SUM(OD.[OrderAmount]) AS TotalAmount
                                
                                FROM [dbo].[OBPData] as OP
                                LEFT JOIN [dbo].[OrdersData] AS OD ON OP.[OBP_ID] = OD.[GOBPId]
                                LEFT JOIN [dbo].[CustomersData] AS CD ON OP.[OBP_ID] = CD.[FK_ObpID]
                                
                                WHERE (OD.[GOBPId] IS NOT NULL AND OD.[GOBPId] > 0) AND OP.[OBP_DH_UserId] = '" + gobpuser + "' GROUP BY OP.[OBP_ApplicantName], OP.[OBP_ID], OP.[OBP_MobileNo]";
                }

                if (Request.QueryString["type"] == "financial")
                {
                    strQuery = @"SELECT DISTINCT 
                                	MAX(OP.[OBP_ID]) AS GOBPID,
                                	MAX(OP.[OBP_ApplicantName]) AS Name,
                                	MAX(OP.[OBP_MobileNo]) AS MobileNo,
                                	COUNT(DISTINCT CD.[CustomrtID]) as Customers,
                                	(SELECT COUNT([OrderID]) FROM [dbo].[OrdersData] WHERE [GOBPId] = MAX(OP.[OBP_ID]) AND YEAR([OrderDate]) = YEAR(GETDATE()) AND MONTH([OrderDate]) = MONTH(GETDATE())) AS MonthOrder,
                                	ISNULL((SELECT SUM([OrderAmount]) FROM [dbo].[OrdersData] WHERE [GOBPId] = MAX(OP.[OBP_ID]) AND YEAR([OrderDate]) = YEAR(GETDATE()) AND MONTH([OrderDate]) = MONTH(GETDATE())),0) AS MonthAmount,	
                                	COUNT(OD.[OrderID]) AS TotalOrders,
                                	SUM(OD.[OrderAmount]) AS TotalAmount
                                
                                FROM [dbo].[OBPData] as OP
                                LEFT JOIN [dbo].[OrdersData] AS OD ON OP.[OBP_ID] = OD.[GOBPId]
                                LEFT JOIN [dbo].[CustomersData] AS CD ON OP.[OBP_ID] = CD.[FK_ObpID]
                                
                                WHERE (OD.[GOBPId] IS NOT NULL AND OD.[GOBPId] > 0) AND [OBP_DH_UserId] = '" + gobpuser + "' AND CONVERT(VARCHAR(20), OP.[OBP_JoinDate], 112) >= CONVERT(VARCHAR(20), CAST('" + myFromDate + "' AS DATETIME), 112) AND CONVERT(VARCHAR(20), OP.[OBP_JoinDate], 112) <= CONVERT(VARCHAR(20), CAST('" + DateTime.Now + "' AS DATETIME), 112) GROUP BY OP.[OBP_ApplicantName], OP.[OBP_ID], OP.[OBP_MobileNo]";
                }

                if (Request.QueryString["type"] == "thismonth")
                {
                    strQuery = @"SELECT DISTINCT 
                                	MAX(OP.[OBP_ID]) AS GOBPID,
                                	MAX(OP.[OBP_ApplicantName]) AS Name,
                                	MAX(OP.[OBP_MobileNo]) AS MobileNo,
                                	COUNT(DISTINCT CD.[CustomrtID]) as Customers,
                                	(SELECT COUNT([OrderID]) FROM [dbo].[OrdersData] WHERE [GOBPId] = MAX(OP.[OBP_ID]) AND YEAR([OrderDate]) = YEAR(GETDATE()) AND MONTH([OrderDate]) = MONTH(GETDATE())) AS MonthOrder,
                                	ISNULL((SELECT SUM([OrderAmount]) FROM [dbo].[OrdersData] WHERE [GOBPId] = MAX(OP.[OBP_ID]) AND YEAR([OrderDate]) = YEAR(GETDATE()) AND MONTH([OrderDate]) = MONTH(GETDATE())),0) AS MonthAmount,	
                                	COUNT(OD.[OrderID]) AS TotalOrders,
                                	SUM(OD.[OrderAmount]) AS TotalAmount
                                
                                FROM [dbo].[OBPData] as OP
                                LEFT JOIN [dbo].[OrdersData] AS OD ON OP.[OBP_ID] = OD.[GOBPId]
                                LEFT JOIN [dbo].[CustomersData] AS CD ON OP.[OBP_ID] = CD.[FK_ObpID]
                                
                                WHERE (OD.[GOBPId] IS NOT NULL AND OD.[GOBPId] > 0) AND [OBP_DH_UserId] = '" + gobpuser + "' AND YEAR(OP.[OBP_JoinDate]) = YEAR('" + DateTime.Now + "') AND MONTH(OP.[OBP_JoinDate]) = MONTH('" + DateTime.Now + "') GROUP BY OP.[OBP_ApplicantName], OP.[OBP_ID], OP.[OBP_MobileNo]";
                }
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
            c.ErrorLogHandler(this.ToString(), "FillGrid", ex.Message.ToString());
            return;
        }
    }
}