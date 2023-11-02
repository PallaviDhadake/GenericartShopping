using System;
using System.Activities.Expressions;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GOBPDH_dashboard : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] arrCounts = new string[20];
    protected void Page_Load(object sender, EventArgs e)
    {
        GetCount();
    }

    private void GetCount()
    {
        try
        {
            string dateRange = c.GetFinancialYear();
            string[] arrDateRange = dateRange.ToString().Split('#');
            DateTime myFromDate = Convert.ToDateTime(arrDateRange[0]);
            DateTime myToDate = Convert.ToDateTime(arrDateRange[1]);

            string gobpuser = c.GetReqData("[dbo].[DistrictHead]", "[DistHdUserId]", "[DistHdId] = " + Session["adminGOBPDH"]).ToString();

            arrCounts[0] = c.returnAggregate(@"SELECT COUNT([OBP_ID]) FROM [dbo].[OBPData] WHERE [OBP_DH_UserId] = '" + gobpuser + "' AND [OBP_DelMark] = 0").ToString();
            
            arrCounts[1] = c.returnAggregate(@"SELECT COUNT(DISTINCT OP.[OBP_ID]) 
                                               FROM [dbo].[OBPData] as OP 
                                               LEFT JOIN [dbo].[OrdersData] AS OD ON OP.[OBP_ID] = OD.[GOBPId] 
                                               WHERE OD.[GOBPId] IS NOT NULL 
                                               AND OD.[GOBPId] > 0 AND OP.[OBP_DH_UserId] = '" + gobpuser + "'").ToString();
             
            arrCounts[2] = 0.ToString();

            arrCounts[3] = c.returnAggregate(@"SELECT COUNT([OBP_ID]) FROM [dbo].[OBPData] WHERE [OBP_DH_UserId] = '" + gobpuser + "' AND [OBP_DelMark] = 0 AND CONVERT(VARCHAR(20), [OBP_JoinDate], 112) >= CONVERT(VARCHAR(20), CAST('" + myFromDate + "' AS DATETIME), 112) AND CONVERT(VARCHAR(20), [OBP_JoinDate], 112) <= CONVERT(VARCHAR(20), CAST('" + DateTime.Now + "' AS DATETIME), 112)").ToString();

            arrCounts[4] = 0.ToString();

            arrCounts[5] = c.returnAggregate(@"SELECT COUNT(DISTINCT OP.[OBP_ID]) 
                                               FROM [dbo].[OBPData] as OP 
                                               LEFT JOIN [dbo].[OrdersData] AS OD ON OP.[OBP_ID] = OD.[GOBPId] 
                                               WHERE OD.[GOBPId] IS NOT NULL 
                                               AND OD.[GOBPId] > 0 AND OP.[OBP_DH_UserId] = '" + gobpuser + "' AND YEAR(OP.[OBP_JoinDate]) = YEAR('" + DateTime.Now + "') AND MONTH(OP.[OBP_JoinDate]) = MONTH('" + DateTime.Now + "')").ToString();

            arrCounts[6] = 0.ToString();

            arrCounts[7] = 0.ToString();

            arrCounts[8] = 0.ToString();

            arrCounts[9] = 0.ToString();

            arrCounts[10] = 0.ToString();

            arrCounts[11] = 0.ToString();

            arrCounts[12] = 0.ToString();

            arrCounts[13] = c.returnAggregate(@"SELECT COUNT(DISTINCT OD.[OrderID]) 
                                                FROM [dbo].[OBPData] as OP 
                                                LEFT JOIN [dbo].[OrdersData] AS OD ON OP.[OBP_ID] = OD.[GOBPId] 
                                                WHERE OD.[GOBPId] IS NOT NULL 
                                                AND OD.[GOBPId] > 0 AND OP.[OBP_DH_UserId] = '" + gobpuser + "' AND OD.[OrderStatus] <> 0 AND OD.[OrderType] IS NOT NULL AND OD.[OrderType] <> 0").ToString();

            arrCounts[14] = c.returnAggregate(@"SELECT COUNT(DISTINCT OD.[OrderID]) 
                                                FROM [dbo].[OBPData] as OP 
                                                LEFT JOIN [dbo].[OrdersData] AS OD ON OP.[OBP_ID] = OD.[GOBPId] 
                                                WHERE OD.[GOBPId] IS NOT NULL 
                                                AND OD.[GOBPId] > 0 AND OP.[OBP_DH_UserId] = '" + gobpuser + "' AND YEAR(OD.[OrderDate]) = YEAR('" + DateTime.Now + "') AND MONTH(OD.[OrderDate]) = MONTH('" + DateTime.Now + "') AND OD.[OrderStatus] <> 0 AND OD.[OrderType] IS NOT NULL AND OD.[OrderType] <> 0").ToString();

            arrCounts[15] = c.returnAggregate(@"SELECT COUNT(DISTINCT OD.[OrderID]) 
                                                FROM [dbo].[OBPData] as OP 
                                                LEFT JOIN [dbo].[OrdersData] AS OD ON OP.[OBP_ID] = OD.[GOBPId] 
                                                WHERE OD.[GOBPId] IS NOT NULL 
                                                AND OD.[GOBPId] > 0 AND OP.[OBP_DH_UserId] = '" + gobpuser + "' AND CONVERT(VARCHAR(20), OD.[OrderDate], 112) >= CONVERT(VARCHAR(20), CAST('" + myFromDate + "' AS DATETIME), 112) AND CONVERT(VARCHAR(20), OD.[OrderDate], 112) <= CONVERT(VARCHAR(20), CAST('" + DateTime.Now + "' AS DATETIME), 112) AND OD.[OrderStatus] <> 0 AND OD.[OrderType] IS NOT NULL AND OD.[OrderType] <> 0").ToString();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('Error Occurred While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetCount", ex.Message.ToString());
            return;
        }
    }
}