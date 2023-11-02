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

public partial class obpmanager_dashboard : System.Web.UI.Page
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
            arrCounts[0] = c.returnAggregate(@"SELECT COUNT([OBP_ID]) From [dbo].[OBPData]").ToString();

            arrCounts[1] = c.returnAggregate(@"SELECT COUNT(DISTINCT OP.[OBP_ID]) 
                                               FROM [dbo].[OBPData] as OP 
                                               LEFT JOIN [dbo].[CustomersData] AS CD ON OP.[OBP_ID] = CD.[FK_ObpID]
                                               WHERE CD.[FK_ObpID] IS NOT NULL 
                                               AND CD.[FK_ObpID] > 0 ").ToString();

            arrCounts[2] = c.returnAggregate(@"SELECT COUNT(DISTINCT OP.[OBP_ID]) 
                                               FROM [dbo].[OBPData] as OP 
                                               FULL OUTER JOIN [dbo].[CustomersData] AS CD ON OP.[OBP_ID] = CD.[FK_ObpID]
                                               WHERE CD.[FK_ObpID] IS NULL ").ToString();

            arrCounts[3] = c.returnAggregate(@"SELECT COUNT([OBP_ID]) FROM [dbo].[OBPData] WHERE YEAR([OBP_JoinDate]) = YEAR(GETDATE()) AND MONTH([OBP_JoinDate]) = MONTH(GETDATE())").ToString();

            arrCounts[4] = c.returnAggregate(@"SELECT COUNT(DISTINCT OP.[OBP_ID])
                                               FROM [dbo].[OBPData] as OP
                                               LEFT JOIN [dbo].[CustomersData] AS CD ON OP.[OBP_ID] = CD.[FK_ObpID]
                                               WHERE CD.[FK_ObpID] IS NOT NULL
                                               AND CD.[FK_ObpID] > 0 AND YEAR(OP.[OBP_JoinDate]) = YEAR(GETDATE()) AND MONTH(OP.[OBP_JoinDate]) = MONTH(GETDATE())").ToString();

            arrCounts[5] = c.returnAggregate(@"SELECT COUNT(DISTINCT OP.[OBP_ID])
                                               FROM [dbo].[OBPData] as OP
                                               FULL OUTER JOIN [dbo].[CustomersData] AS CD ON OP.[OBP_ID] = CD.[FK_ObpID]
                                               WHERE CD.[FK_ObpID] IS NULL AND YEAR(OP.[OBP_JoinDate]) = YEAR(GETDATE()) AND MONTH(OP.[OBP_JoinDate]) = MONTH(GETDATE())").ToString();

            arrCounts[6] = c.returnAggregate(@"SELECT COUNT(DISTINCT OP.[OBP_ID]) 
                                               FROM [dbo].[OBPData] as OP 
                                               LEFT JOIN [dbo].[OrdersData] AS OD ON OP.[OBP_ID] = OD.[GOBPId]
                                               WHERE OD.[GOBPId] IS NOT NULL 
                                               AND OD.[GOBPId] > 0 ").ToString();

            arrCounts[7] = c.returnAggregate(@"SELECT SUM(OD.[OBPComTotal])
                                               FROM [dbo].[OBPData] AS OP
                                               LEFT JOIN [dbo].[OrdersData] AS OD ON OP.[OBP_ID] = OD.[GOBPId]
                                               WHERE OD.[GOBPId] IS NOT NULL
                                               ANd OD.[GOBPId] > 0").ToString();

            arrCounts[8] = c.returnAggregate(@"SELECT COUNT(DISTINCT OP.[OBP_ID]) 
                                               FROM [dbo].[OBPData] as OP 
                                               FULL OUTER JOIN [dbo].[OrdersData] AS OD ON OP.[OBP_ID] = OD.[GOBPId]
                                               WHERE OD.[GOBPId] IS NULL").ToString();

            arrCounts[9] = c.returnAggregate(@"SELECT COUNT(DISTINCT OP.[OBP_ID]) 
                                               FROM [dbo].[OBPData] as OP 
                                               LEFT JOIN [dbo].[OrdersData] AS OD ON OP.[OBP_ID] = OD.[GOBPId]
                                               WHERE OD.[GOBPId] IS NOT NULL 
                                               AND OD.[GOBPId] > 0 AND YEAR(OP.[OBP_JoinDate]) = YEAR(GETDATE()) AND MONTH(OP.[OBP_JoinDate]) = MONTH(GETDATE())").ToString();

            arrCounts[10] = c.returnAggregate(@"SELECT SUM(OD.[OBPComTotal])
                                               FROM [dbo].[OBPData] AS OP
                                               LEFT JOIN [dbo].[OrdersData] AS OD ON OP.[OBP_ID] = OD.[GOBPId]
                                               WHERE OD.[GOBPId] IS NOT NULL
                                               ANd OD.[GOBPId] > 0 AND YEAR(OP.[OBP_JoinDate]) = YEAR(GETDATE()) AND MONTH(OP.[OBP_JoinDate]) = MONTH(GETDATE())").ToString();

            arrCounts[11] = c.returnAggregate(@"SELECT COUNT(DISTINCT OP.[OBP_ID]) 
                                               FROM [dbo].[OBPData] as OP 
                                               FULL OUTER JOIN [dbo].[OrdersData] AS OD ON OP.[OBP_ID] = OD.[GOBPId]
                                               WHERE OD.[GOBPId] IS NULL AND YEAR(OP.[OBP_JoinDate]) = YEAR(GETDATE()) AND MONTH(OP.[OBP_JoinDate]) = MONTH(GETDATE())").ToString();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetCount", ex.Message.ToString());
            return;
        }
    }
}