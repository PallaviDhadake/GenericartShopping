using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class obpmanager_yearly_dashboard : System.Web.UI.Page
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
            arrCounts[0] = c.returnAggregate(@"SELECT COUNT([OBP_ID]) 
                                               FROM [dbo].[OBPData] 
                                               WHERE YEAR([OBP_JoinDate]) = YEAR(GETDATE()) AND [OBP_DelMark] = 0").ToString();

            arrCounts[1] = c.returnAggregate(@"SELECT COUNT(DISTINCT OP.[OBP_ID])
                                               FROM [dbo].[OBPData] as OP
                                               LEFT JOIN [dbo].[CustomersData] AS CD ON OP.[OBP_ID] = CD.[FK_ObpID]
                                               WHERE CD.[FK_ObpID] IS NOT NULL
                                               AND CD.[FK_ObpID] > 0 AND YEAR(OP.[OBP_JoinDate]) = YEAR(GETDATE()) AND OP.[OBP_DelMark] = 0").ToString();

            arrCounts[2] = c.returnAggregate(@"SELECT COUNT(DISTINCT OP.[OBP_ID])
                                               FROM [dbo].[OBPData] as OP
                                               FULL OUTER JOIN [dbo].[CustomersData] AS CD ON OP.[OBP_ID] = CD.[FK_ObpID]
                                               WHERE CD.[FK_ObpID] IS NULL AND YEAR(OP.[OBP_JoinDate]) = YEAR(GETDATE()) AND OP.[OBP_DelMark] = 0").ToString();

            arrCounts[3] = c.returnAggregate(@"SELECT SUM(OD.[OrderAmount])
                                               FROM [dbo].[OBPData] AS OP
                                               LEFT JOIN [dbo].[OrdersData] AS OD ON OP.[OBP_ID] = OD.[GOBPId]
                                               WHERE OD.[GOBPId] IS NOT NULL
                                               ANd OD.[GOBPId] > 0 AND YEAR(OP.[OBP_JoinDate]) = YEAR(GETDATE()) AND OP.[OBP_DelMark] = 0 AND OD.[OrderStatus] = 7").ToString();

            arrCounts[4] = c.returnAggregate(@"SELECT COUNT(OD.[OrderID])
                                               FROM [dbo].[OBPData] AS OP
                                               LEFT JOIN [dbo].[OrdersData] AS OD ON OP.[OBP_ID] = OD.[GOBPId]
                                               WHERE OD.[GOBPId] IS NOT NULL
                                               ANd OD.[GOBPId] > 0 AND YEAR(OP.[OBP_JoinDate]) = YEAR(GETDATE()) AND OP.[OBP_DelMark] = 0 AND OD.[OrderStatus] = 7").ToString();

            arrCounts[5] = 0.ToString();

            arrCounts[6] = c.returnAggregate(@"SELECT COUNT([OBP_ID]) 
                                               FROM [dbo].[OBPData] 
                                               WHERE YEAR([OBP_JoinDate]) < YEAR(GETDATE()) AND [OBP_DelMark] = 0").ToString();

            arrCounts[7] = c.returnAggregate(@"SELECT COUNT(DISTINCT OP.[OBP_ID])
                                               FROM [dbo].[OBPData] as OP
                                               LEFT JOIN [dbo].[CustomersData] AS CD ON OP.[OBP_ID] = CD.[FK_ObpID]
                                               WHERE CD.[FK_ObpID] IS NOT NULL
                                               AND CD.[FK_ObpID] > 0 AND YEAR(OP.[OBP_JoinDate]) < YEAR(GETDATE()) AND OP.[OBP_DelMark] = 0").ToString();

            arrCounts[8] = c.returnAggregate(@"SELECT COUNT(DISTINCT OP.[OBP_ID])
                                               FROM [dbo].[OBPData] as OP
                                               FULL OUTER JOIN [dbo].[CustomersData] AS CD ON OP.[OBP_ID] = CD.[FK_ObpID]
                                               WHERE CD.[FK_ObpID] IS NULL AND YEAR(OP.[OBP_JoinDate]) < YEAR(GETDATE()) AND OP.[OBP_DelMark] = 0").ToString();

            arrCounts[9] = c.returnAggregate(@"SELECT SUM(OD.[OrderAmount])
                                               FROM [dbo].[OBPData] AS OP
                                               LEFT JOIN [dbo].[OrdersData] AS OD ON OP.[OBP_ID] = OD.[GOBPId]
                                               WHERE OD.[GOBPId] IS NOT NULL
                                               ANd OD.[GOBPId] > 0 AND OD.[OrderStatus] = 7 AND YEAR(OP.[OBP_JoinDate]) < YEAR(GETDATE()) AND OP.[OBP_DelMark] = 0
                                               AND YEAR(OD.[OrderDate]) = YEAR(GETDATE())").ToString();

            arrCounts[10] = c.returnAggregate(@"SELECT COUNT(OD.[OrderID])
                                                FROM [dbo].[OBPData] AS OP
                                                LEFT JOIN [dbo].[OrdersData] AS OD ON OP.[OBP_ID] = OD.[GOBPId]
                                                WHERE OD.[GOBPId] IS NOT NULL
                                                ANd OD.[GOBPId] > 0 AND OD.[OrderStatus] = 7 AND YEAR(OP.[OBP_JoinDate]) < YEAR(GETDATE()) AND OP.[OBP_DelMark] = 0
                                                AND YEAR(OD.[OrderDate]) = YEAR(GETDATE())").ToString();

            arrCounts[11] = 0.ToString();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetCount", ex.Message.ToString());
            return;
        }
    }
}