using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Activities.Expressions;

public partial class supportteam_callers_dashboard : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] arrFlup = new string[30];
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetCount();
        }
    }

    public void GetCount()
    {
        try
        {
            arrFlup[0] = c.returnAggregate("SELECT COUNT([FlupId]) FROM [dbo].[FollowupOrders] WHERE [FK_TeamMemberId] = " + Session["adminSupport"]).ToString();
            arrFlup[1] = c.returnAggregate("SELECT COUNT([FlupEnqId]) FROM [dbo].[FollowupEnquires] WHERE [FK_TeamMemberId] = " + Session["adminSupport"]).ToString();
            arrFlup[2] = (Convert.ToInt32(arrFlup[0]) + Convert.ToInt32(arrFlup[1])).ToString();
            arrFlup[3] = c.returnAggregate("SELECT COUNT([FlupId]) FROM [dbo].[FollowupOrders] WHERE [FK_TeamMemberId] = " + Session["adminSupport"] + " AND CONVERT(VARCHAR(20), [FlupDate], 112) = CONVERT(VARCHAR(20), CAST('" + DateTime.Now + "' AS DATETIME), 112)").ToString();
            arrFlup[4] = c.returnAggregate("SELECT COUNT([FlupEnqId]) FROM [dbo].[FollowupEnquires] WHERE [FK_TeamMemberId] = " + Session["adminSupport"] + " AND CONVERT(VARCHAR(20), [FlupEnqDate], 112) = CONVERT(VARCHAR(20), CAST('" + DateTime.Now + "' AS DATETIME), 112)").ToString();
            arrFlup[5] = (Convert.ToInt32(arrFlup[3]) + Convert.ToInt32(arrFlup[4])).ToString();
            arrFlup[6] = c.returnAggregate("SELECT SUM([OrderAmount]) FROM [dbo].[OrdersData] WHERE [OrderID] IN (Select [FK_OrderID] FROM [dbo].[FollowupOrders] Where [FK_TeamMemberId] = " + Session["adminSupport"] + " AND [FlupRemarkStatusID] IN (3,7))").ToString();
            arrFlup[7] = c.returnAggregate("SELECT COUNT([FlupID]) FROM [dbo].[FollowupOrders] WHERE [FK_TeamMemberId] = " + Session["adminSupport"] + " AND [FlupRemarkStatusID] IN (3,6,7)").ToString();
            arrFlup[8] = c.returnAggregate("SELECT COUNT([OrderID]) FROM [dbo].[OrdersData] WHERE [OrderID] IN (SELECT [FK_OrderID] FROM [dbo].[FollowupOrders] WHERE [FK_TeamMemberId] = " + Session["adminSupport"] + " AND [FlupRemarkStatusID] IN (3,7))").ToString();

            //------------------------------ Monthly Calls ------------------------------ 

            arrFlup[15] = c.returnAggregate("SELECT COUNT([FlupId]) FROM [dbo].[FollowupOrders] WHERE [FK_TeamMemberId] = " + Session["adminSupport"] + " AND YEAR([FlupDate]) = YEAR('" + DateTime.Now + "') AND MONTH([FlupDate]) = MONTH('" + DateTime.Now + "')").ToString();
            arrFlup[16] = c.returnAggregate("SELECT COUNT([FlupEnqId]) FROM [dbo].[FollowupEnquires] WHERE [FK_TeamMemberId] = " + Session["adminSupport"] + " AND YEAR([FlupEnqDate]) = YEAR('" + DateTime.Now + "') AND MONTH([FlupEnqDate]) = MONTH('" + DateTime.Now + "')").ToString();
            arrFlup[17] = (Convert.ToInt32(arrFlup[15]) + Convert.ToInt32(arrFlup[16])).ToString();
            arrFlup[18] = c.returnAggregate("SELECT SUM([OrderAmount]) FROM [dbo].[OrdersData] WHERE [OrderID] IN (Select[FK_OrderID] FROM[dbo].[FollowupOrders] Where[FK_TeamMemberId] = " + Session["adminSupport"] + " AND[FlupRemarkStatusID] IN(3, 7) AND YEAR([FlupDate]) = YEAR('" + DateTime.Now + "') AND MONTH([FlupDate]) = MONTH('" + DateTime.Now + "'))").ToString();
            arrFlup[19] = c.returnAggregate("SELECT COUNT([FlupID]) FROM [dbo].[FollowupOrders] WHERE [FK_TeamMemberId] = " + Session["adminSupport"] + " AND [FlupremarkStatusID] IN (3,6,7) AND YEAR([FlupDate]) = YEAR('" + DateTime.Now + "') AND MONTH([FlupDate]) = MONTH('" + DateTime.Now + "')").ToString();
            arrFlup[20] = c.returnAggregate("SELECT COUNT([OrderID]) FROM [dbo].[OrdersData] WHERE [OrderID] IN (SELECT [FK_OrderID] FROM [dbo].[FollowupOrders] WHERE [FK_TeamMemberId] = " + Session["adminSupport"] + " AND [FlupRemarkStatusID] IN (3,7) AND YEAR([flupDate]) = YEAR('" + DateTime.Now + "') AND MONTH([FlupDate]) = MONTH('" + DateTime.Now + "'))").ToString();

            //------------------------------ Todays Calls ------------------------------ 

            arrFlup[21] = c.returnAggregate("SELECT COUNT([FlupId]) FROM [dbo].[FollowupOrders] WHERE [FK_TeamMemberId] = " + Session["adminSupport"] + " AND CONVERT(VARCHAR(20), [FlupDate], 112) = CONVERT(VARCHAR(20), CAST('" + DateTime.Now + "' AS DATETIME), 112)").ToString();
            arrFlup[22] = c.returnAggregate("SELECT COUNT([FlupEnqId]) FROM [dbo].[FollowupEnquires] WHERE [FK_TeamMemberId] = " + Session["adminSupport"] + " AND CONVERT(VARCHAR(20), [FlupEnqDate], 112) = CONVERT(VARCHAR(20), CAST('" + DateTime.Now + "' AS DATETIME), 112)").ToString();
            arrFlup[23] = (Convert.ToInt32(arrFlup[21]) + Convert.ToInt32(arrFlup[22])).ToString();
            arrFlup[24] = c.returnAggregate("SELECT SUM([OrderAmount]) FROM [dbo].[OrdersData] WHERE [OrderID] IN (SELECT [FK_OrderID] FROM [dbo].[FollowupOrders] WHERE [FK_TeamMemberId]= " + Session["adminSupport"] + " AND [FlupRemarkStatusID] IN (3,7) AND CONVERT(VARCHAR(20), [FlupDate], 112) = CONVERT(VARCHAR(20), CAST('" + DateTime.Now + "' AS DATETIME), 112))").ToString();
            arrFlup[25] = c.returnAggregate("SELECT COUNT([FlupId]) FROM [dbo].[FollowupOrders] WHERE [FK_TeamMemberId] = " + Session["adminSupport"] + " AND [FlupRemarkStatusID] IN (3,6,7) AND CONVERT(VARCHAR(20), [FlupDate], 112) = CONVERT(VARCHAR(20), CAST('" + DateTime.Now + "' AS DATETIME), 112)").ToString();
            arrFlup[26] = c.returnAggregate("SELECT COUNT([OrderID]) FROM [dbo].[OrdersData] WHERE [OrderID] IN (SELECT [FK_OrderID] FROM [dbo].[FollowupOrders] WHERE [FK_TeamMemberId] = " + Session["adminSupport"] + " AND [FlupRemarkStatusID] IN (3,7) AND CONVERT(VARCHAR(20), [FlupDate], 112) = CONVERT(VARCHAR(20), CAST('" + DateTime.Now + "' AS DATETIME), 112))").ToString();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetCount", ex.Message.ToString());
            return;
        }
    }
}