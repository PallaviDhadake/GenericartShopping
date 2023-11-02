using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class obpmanager_gobp_incentive_current_month : System.Web.UI.Page
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
            string strQuery = "";

            if (Request.QueryString["type"] != null)
            {
                if (Request.QueryString["type"] == "active")
                {
                    strQuery = @"SELECT DISTINCT 
                                	MAX(OP.[OBP_ID]) AS GOBPID,
                                	MAX(OP.[OBP_ApplicantName]) AS Name,
                                	MAX(OP.[OBP_MobileNo]) AS MobileNo,
                                	(SELECT COUNT(DISTINCT [CustomrtID]) FROM [dbo].[CustomersData] WHERE [FK_ObpID] = MAX(OP.[OBP_ID])) as Customers,
                                	(SELECT COUNT([OrderID]) FROM [dbo].[OrdersData] WHERE [GOBPId] = MAX(OP.[OBP_ID])) AS MonthOrder,
                                	ISNULL((SELECT SUM([OrderAmount]) FROM [dbo].[OrdersData] WHERE [GOBPId] = MAX(OP.[OBP_ID])),0) AS MonthAmount,
                                    (SELECT SUM([OBPComTotal]) FROM [dbo].[OrdersData] WHERE [GOBPId] = MAX(OP.[OBP_ID])) AS IncentiveAmount
                                	
                                FROM [dbo].[OBPData] as OP
                                LEFT JOIN [dbo].[OrdersData] AS OD ON OP.[OBP_ID] = OD.[GOBPId]
                                
                                WHERE (OD.[GOBPId] IS NOT NULL AND OD.[GOBPId] > 0)
                                
                                GROUP BY OP.[OBP_ApplicantName], OP.[OBP_ID], OP.[OBP_MobileNo]";
                }

                if (Request.QueryString["type"] == "inactive")
                {
                    strQuery = @"SELECT DISTINCT 
                                    MAX(OP.[OBP_ID]) AS GOBPID,
                                    MAX(OP.[OBP_ApplicantName]) AS Name,
                                    MAX(OP.[OBP_MobileNo]) AS MobileNo,
                                    (SELECT COUNT(DISTINCT [CustomrtID]) FROM [dbo].[CustomersData] WHERE [FK_ObpID] = MAX(OP.[OBP_ID])) as Customers,
                                    (SELECT COUNT([OrderID]) FROM [dbo].[OrdersData] WHERE [GOBPId] = MAX(OP.[OBP_ID])) AS MonthOrder,
                                    ISNULL((SELECT SUM([OrderAmount]) FROM [dbo].[OrdersData] WHERE [GOBPId] = MAX(OP.[OBP_ID])),0) AS MonthAmount,
                                    (SELECT SUM([OBPComTotal]) FROM [dbo].[OrdersData] WHERE [GOBPId] = MAX(OP.[OBP_ID])) AS IncentiveAmount
                                 	
                                 FROM [dbo].[OBPData] as OP
                                 FULL OUTER JOIN [dbo].[OrdersData] AS OD ON OP.[OBP_ID] = OD.[GOBPId]
                                 
                                 WHERE OD.[GOBPId] IS NULL AND OP.[OBP_ID] IS NOT NULL
                                 AND YEAR(OP.[OBP_JoinDate]) = YEAR(GETDATE())
                                 AND MONTH(OP.[OBP_JoinDate]) = MONTH(GETDATE())
                                 
                                 GROUP BY OP.[OBP_ApplicantName], OP.[OBP_ID], OP.[OBP_MobileNo]";
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