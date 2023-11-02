using Razorpay.Api;
using System;
using System.Activities.Expressions;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class supportteam_dashboard : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] managersDataCount = new string[10];
    public string[] arrCounts = new string[15]; //9
    public string[] arrRefunds = new string[10];
    //public string[] staffData = new string[10];
    //public string[] staffDataCount = new string[10];
    public string[] purchaseData = new string[5];
    public string[] arrFlUp = new string[20]; //10
    public string dataCount, cardName, todaysCount, overallCount, currDay;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (Session["adminSupport"] != null)
            {                
                if (Session["adminSupport"].ToString() == "1")
                {
                    todistribute.Visible = true;
                    today.Visible = false;
                    teamManager.Visible = true;                    
                    teamStaff.Visible = false;
                    purchaseDeptID.Visible = false;
                    trainee.Visible = false;
                    payment.Visible = false;
                    GetManagersDataCount();
                }
                else
                {
                    CheckUserTask();
                }
                GetFollowupStats();
                GetRefundCount();
                GetTeamOrder();
            }
        }
    }

    private void GetRefundCount()
    {
        try
        {
            string dateRange = c.GetFinancialYear();
            string[] arrDateRange = dateRange.ToString().Split('#');
            DateTime myFromDate = Convert.ToDateTime(arrDateRange[0]);
            DateTime myToDate = Convert.ToDateTime(arrDateRange[1]);

            arrRefunds[0] = c.returnAggregate(@"SELECT COUNT(OrderID) FROM [dbo].[OrdersData] WHERE [OrderStatus] = 13 
                                    AND (Convert(varchar(20), [OrderDate], 112) >= Convert(varchar(20), CAST('" + myFromDate + "' AS DATETIME), 112)) AND (Convert(varchar(20), [OrderDate], 112) <= Convert(varchar(20), CAST('" + DateTime.Now + "' AS DATETIME), 112))").ToString();
            arrRefunds[1] = c.returnAggregate(@"SELECT COUNT(OrderID) FROM [dbo].[OrdersData] WHERE [OrderStatus] = 14
                                    AND (Convert(varchar(20), [OrderDate], 112) >= Convert(varchar(20), CAST('" + myFromDate + "' AS DATETIME), 112)) AND (Convert(varchar(20), [OrderDate], 112) <= Convert(varchar(20), CAST('" + DateTime.Now + "' AS DATETIME), 112))").ToString();
            arrRefunds[2] = c.returnAggregate(@"SELECT COUNT(OrderID) FROM [dbo].[OrdersData] WHERE [OrderStatus] = 15 
                                    AND (Convert(varchar(20), [OrderDate], 112) >= Convert(varchar(20), CAST('" + myFromDate + "' AS DATETIME), 112)) AND (Convert(varchar(20), [OrderDate], 112) <= Convert(varchar(20), CAST('" + DateTime.Now + "' AS DATETIME), 112))").ToString();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetRefundCount", ex.Message.ToString());
            return;
        }
    }

    private void GetFollowupStats()
    {
        try
        {
            string dateRange = c.GetFinancialYear();
            string[] arrDateRange = dateRange.ToString().Split('#');
            DateTime myFromDate = Convert.ToDateTime(arrDateRange[0]);
            DateTime myToDate = Convert.ToDateTime(arrDateRange[1]);

            currDay = DateTime.Now.Day.ToString();

            //DateTime pendDate = DateTime.Now.AddMonths(-1);
            DateTime pendDate = DateTime.Now;

            int pdaysInMonth = DateTime.DaysInMonth(pendDate.Year, pendDate.Month);
            string pnendDate = pendDate.Month.ToString() + "/" + pdaysInMonth + "/" + pendDate.Year.ToString();
            DateTime pstartDate = pendDate.AddMonths(-2);

            string pnstartDate = pstartDate.Month.ToString() + "/" + "01/" + pstartDate.Year.ToString();

            arrFlUp[6] = c.returnAggregate("Select Count(a.FK_OrderCustomerID) " +
               " From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID " +
               " Inner Join OrdersAssign c On a.OrderID=c.FK_OrderID Inner Join CompanyOwnShops d On c.Fk_FranchID=d.FK_FranchID " +
               " Where a.FollowupStatus='Active' AND  a.OrderStatus=7 " +
               " AND c.OrdReAssign=0 " +
               " AND Convert(varchar(20), a.FollowupNextDate, 112) >= Convert(varchar(20), CAST('" + pnstartDate + "' as datetime), 112) " +
               " AND Convert(varchar(20), a.FollowupNextDate, 112) < Convert(varchar(20), CAST('" + pendDate + "' as datetime), 112)").ToString();

              // " AND a.FollowupNextDate >= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE())-3, 0) AND " +
              //" CONVERT(varchar(20), FollowupNextDate, 112) < CONVERT(varchar(20), CAST(GETDATE() as datetime) ,112)").ToString(); //pending

              //arrFlUp[7] = c.returnAggregate("Select Count(a.FK_OrderCustomerID) " +
              //    " From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID " +
              //    " Inner Join OrdersAssign c On a.OrderID=c.FK_OrderID " +
              //    " Where a.FollowupStatus='Active' AND a.OrderStatus IN (5,6,7) AND c.Fk_FranchID IN (24, 2010, 2062, 2070, 2012, 2065, 2124, 2137, 2142, 2165) " +
              //    " AND c.OrdReAssign=0 " +
              //    " AND a.FollowupNextDate <= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE())-3, 0)").ToString(); //inactive


            DateTime endDate = DateTime.Now.AddMonths(-3);
            int idaysInMonth = DateTime.DaysInMonth(endDate.Year, endDate.Month);
            string iendDate = endDate.Month.ToString() + "/" + idaysInMonth + "/" + endDate.Year.ToString();
            DateTime startDate = endDate.AddMonths(-11);
            string istartDate = startDate.Month.ToString() + "/" + "01/" + startDate.Year.ToString();
            arrFlUp[7] = c.returnAggregate("Select Count(a.FK_OrderCustomerID) " +
                " From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID " +
                " Inner Join OrdersAssign c On a.OrderID=c.FK_OrderID Inner Join CompanyOwnShops d On c.Fk_FranchID=d.FK_FranchID " +
                " Where a.FollowupStatus='Active' AND  a.OrderStatus=7 " +
                " AND c.OrdReAssign=0 " +
                " AND Convert(varchar(20), a.FollowupNextDate, 112) >= Convert(varchar(20), CAST('" + istartDate + "' as datetime), 112) " +
                " AND Convert(varchar(20), a.FollowupNextDate, 112) <= Convert(varchar(20), CAST('" + iendDate + "' as datetime), 112)").ToString();
            
            //" AND a.FollowupNextDate <= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE())-3, 0)").ToString(); //inactive
            ////arrFlUp[2] = c.returnAggregate("Select Count(a.FK_OrderCustomerID) " +
            ////    " From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID " +
            ////    " Where a.FollowupStatus='Active' AND a.OrderStatus<>0 " +
            ////    " AND a.OrderDate >= DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE())-15, 0)").ToString(); //fresh
            ///
            arrFlUp[4] = c.returnAggregate("Select Count(a.OrderID) From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID" +
                // " Inner Join OrdersAssign c On a.OrderID=c.FK_OrderID  " +  //Inner Join CompanyOwnShops d On c.Fk_FranchID=d.FK_FranchID
                " Where a.OrderStatus=1 AND (a.FollowupStatus='Active' OR a.FollowupStatus IS NULL)  " +
                " ").ToString(); // frash/new orders  // AND c.OrdReAssign=0

            arrFlUp[11] = c.returnAggregate(@"SELECT COUNT(a.[OrderID]) FROM [dbo].[OrdersData] AS a
                                              INNER JOIN [dbo].[CustomersData] AS b ON a.[FK_OrderCustomerID] = b.[CustomrtID]
                                              WHERE a.[OrderStatus] IN (4, 8, 9, 11) AND (a.[FollowupStatus] = 'Active' OR a.[FollowupStatus] IS NULL)
                                              AND CONVERT(VARCHAR(20), a.[OrderDate], 112) >= CONVERT(VARCHAR(20), CAST('" + myFromDate + "' AS DATETIME), 112)" +
                                              " AND CONVERT(VARCHAR(20), a.[OrderDate], 112) <= CONVERT(VARCHAR(20), CAST('" + DateTime.Now + "' AS DATETIME))").ToString();

            //arrFlUp[5] = c.returnAggregate("Select Count(a.FK_OrderCustomerID) " +
            //    " From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID " +
            //    " Inner Join OrdersAssign c On a.OrderID=c.FK_OrderID Inner Join CompanyOwnShops d On c.Fk_FranchID=d.FK_FranchID Left Join FollowupOrders e On a.OrderID=e.FK_OrderId" +
            //    " Where a.FollowupStatus='Active' AND a.OrderStatus=7 " +
            //    " AND c.OrdReAssign=0 " +
            //    " AND a.FollowupNextDate >= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE())-2, 0)  AND " +
            //    " DAY(a.FollowupNextDate)='" + DateTime.Now.Day + "'  " +
            //    " AND CONVERT(varchar(20), a.FollowupNextDate, 112) <= CONVERT(varchar(20), CAST(GETDATE() as datetime) ,112)").ToString(); //today's flup            

            //Other shops excluding company own shops
            arrFlUp[0] = c.returnAggregate("Select Count(a.FK_OrderCustomerID) " +
                " From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID " +
                " Inner Join OrdersAssign c On a.OrderID=c.FK_OrderID Left Outer Join CompanyOwnShops d On c.Fk_FranchID=d.FK_FranchID " +
                " Where a.FollowupStatus='Active' AND  a.OrderStatus=7  " +
                " AND c.OrdReAssign=0  AND d.FK_FranchID is null " +
                " AND Convert(varchar(20), a.FollowupNextDate, 112) >= Convert(varchar(20), CAST('" + pnstartDate + "' as datetime), 112) " +
                " AND Convert(varchar(20), a.FollowupNextDate, 112) < Convert(varchar(20), CAST('" + pendDate + "' as datetime), 112)").ToString();

            //" AND a.FollowupNextDate >= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE())-3, 0) AND " +
            //" CONVERT(varchar(20), FollowupNextDate, 112) < CONVERT(varchar(20), CAST(GETDATE() as datetime) ,112)").ToString(); //pending

            arrFlUp[1] = c.returnAggregate("Select Count(a.FK_OrderCustomerID) " +
                " From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID " +
                " Inner Join OrdersAssign c On a.OrderID=c.FK_OrderID Left Outer Join CompanyOwnShops d On c.Fk_FranchID=d.FK_FranchID" +
                " Where a.FollowupStatus='Active' AND a.OrderStatus=7  " +
                " AND c.OrdReAssign=0 AND d.FK_FranchID is null " +
                " AND Convert(varchar(20), a.FollowupNextDate, 112) >= Convert(varchar(20), CAST('" + istartDate + "' as datetime), 112) " +
                " AND Convert(varchar(20), a.FollowupNextDate, 112) <= Convert(varchar(20), CAST('" + iendDate + "' as datetime), 112)").ToString();
            
            //" AND a.FollowupNextDate <= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE())-3, 0)").ToString(); //inactive
            //arrFlUp[2] = c.returnAggregate("Select Count(a.OrderID) From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID" +
            //    " Inner Join OrdersAssign c On a.OrderID=c.FK_OrderID Left Outer Join CompanyOwnShops d On c.Fk_FranchID=d.FK_FranchID " +
            //    " Where a.OrderStatus=1 AND (a.FollowupStatus='Active' OR a.FollowupStatus IS NULL) " + //AND  d.FK_FranchID is null 
            //   "  ").ToString(); // frash/new orders   //AND c.OrdReAssign=0

            arrFlUp[3] = c.returnAggregate("Select Count(a.FK_OrderCustomerID) " +
                " From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID " +
                " Inner Join OrdersAssign c On a.OrderID=c.FK_OrderID Left Outer Join CompanyOwnShops d On c.Fk_FranchID=d.FK_FranchID Left Join FollowupOrders e On a.OrderID=e.FK_OrderId" +
                " Where a.FollowupStatus='Active' AND a.OrderStatus=7 " +
                " AND c.OrdReAssign=0  AND d.FK_FranchID is null " +
                " AND a.FollowupNextDate >= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE())-2, 0)  AND " +
                " DAY(a.FollowupNextDate)='" + DateTime.Now.Day + "'  " +
                " AND CONVERT(varchar(20), a.FollowupNextDate, 112) <= CONVERT(varchar(20), CAST(GETDATE() as datetime) ,112)").ToString(); //today's flup


            //arrFlUp[0] = c.returnAggregate("Select Count(a.FK_OrderCustomerID) " +
            //    " From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID " +
            //    " Where a.FollowupStatus='Active' AND a.OrderStatus<>0 " +
            //    " AND a.FollowupNextDate >= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE())-3, 0) AND " + 
            //    " CONVERT(varchar(20), FollowupNextDate, 112) < CONVERT(varchar(20), CAST(GETDATE() as datetime) ,112)").ToString(); //pending

            //arrFlUp[1] = c.returnAggregate("Select Count(a.FK_OrderCustomerID) " +
            //    " From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID " +
            //    " Where a.FollowupStatus='Active' AND a.OrderStatus<>0 " +
            //    " AND a.FollowupNextDate <= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE())-3, 0)").ToString(); //inactive

            ////arrFlUp[2] = c.returnAggregate("Select Count(a.FK_OrderCustomerID) " +
            ////    " From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID " +
            ////    " Where a.FollowupStatus='Active' AND a.OrderStatus<>0 " +
            ////    " AND a.OrderDate >= DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE())-15, 0)").ToString(); //fresh
            //arrFlUp[2] = c.returnAggregate("Select Count(a.OrderID) From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID" +
            //    " Where a.OrderStatus=1 AND (a.FollowupStatus='Active' OR a.FollowupStatus IS NULL) ").ToString(); // frash/new orders

            //arrFlUp[3] = c.returnAggregate("Select Count(a.FK_OrderCustomerID) " +
            //    " From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID " +
            //    " Where a.FollowupStatus='Active' AND a.OrderStatus<>0 " +
            //    " AND a.FollowupNextDate >= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE())-2, 0)  AND " +
            //    " DAY(a.FollowupNextDate)='"+DateTime.Now.Day+"'  " + 
            //    " AND CONVERT(varchar(20), a.FollowupNextDate, 112) <= CONVERT(varchar(20), CAST(GETDATE() as datetime) ,112)").ToString(); //today's flup

            // Registerd but not ordered
            //arrFlUp[8] = c.returnAggregate("Select Count(a.CustomrtID) From CustomersData a " + 
            //    " Left Outer Join OrdersData b On a.CustomrtID=b.FK_OrderCustomerID where (b.FK_OrderCustomerID is null or b.FK_OrderCustomerID=0)").ToString();


            // If RBNO_NextFollowup found NULL, then here we update it with CustomerJoinDate by default (28th March 23, by Vinayak)
            c.ExecuteQuery("Update CustomersData set RBNO_NextFollowup = CustomerJoinDate Where RBNO_NextFollowup IS NULL");

            arrFlUp[8] = c.returnAggregate("Select Count(a.CustomrtID) From CustomersData a " +
               " Left Outer Join OrdersData b On a.CustomrtID=b.FK_OrderCustomerID LEFT JOIN SavingCalc c ON a.CustomrtID = c.FK_CustId where c.FK_CustId IS NULL AND (b.FK_OrderCustomerID is null or b.FK_OrderCustomerID=0) AND CONVERT(varchar(20), a.RBNO_NextFollowup, 112) <= CONVERT(varchar(20), CAST(GETDATE() as datetime) ,112)").ToString();

            // If in SavingCalc table found FollowupLastDate, FollowupNextDate, FollowupNextTime, FollowupStatus columns NULL, then we update it with CalcDate by default (31th March 23, by Vinayak)
            c.ExecuteQuery("Update SavingCalc set FollowupLastDate = CalcDate, FollowupNextDate = CalcDate, FollowupNextTime='11:00 AM', FollowupStatus='Active' Where FollowupNextDate IS NULL");

            // enquiries
            arrFlUp[9] = c.returnAggregate("Select Count(DISTINCT a.CalcID) From SavingCalc a Left Join CustomersData b on a.FK_CustId = b.CustomrtID " +
                 " Where a.FollowupStatus='Inactive' AND a.FK_CustId <> 0 AND CONVERT(varchar(20), a.FollowupNextDate, 112) <= CONVERT(varchar(20), CAST(GETDATE() as datetime) ,112)").ToString();

            // customer consistency count
            int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            string sqlEDate = DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/" + daysInMonth.ToString();
            DateTime tempDate = DateTime.Now.AddMonths(-2);
            string sqlSDate = tempDate.ToString("yyyy") + "/" + tempDate.ToString("MM") + "/" + "01";
            arrFlUp[10] = c.returnAggregate("Select Count(distinct a.FK_OrderCustomerID) " +
                        " From OrdersData a Left Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID Where " +
                        " Convert(varchar(20), a.OrderDate, 112) >= Convert(varchar(20), CAST('" + sqlSDate + "' as datetime), 112) AND " +
                        " Convert(varchar(20), a.OrderDate, 112) <= Convert(varchar(20), CAST('" + sqlEDate + "' as datetime), 112) AND a.OrderStatus=7").ToString();

            arrFlUp[12] = c.returnAggregate(@"Select Count(a.OrderID) From OrdersData a 
                                              Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID  
                                              Where a.OrderStatus=1 AND a.OrderType=1 
                                              AND (b.CustomerFavShop IS NULL OR b.CustomerFavShop=0 OR b.CustomerFavShop IS NOT NULL OR b.CustomerFavShop<>0)").ToString();

            arrFlUp[13] = c.returnAggregate(@"Select Count(a.OrderID) From OrdersData a 
                                              Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID 
                                              Where a.OrderStatus=1 AND a.OrderType=2 
                                              AND a.FK_OrderCustomerID IS NOT NULL 
                                              AND a.FK_OrderCustomerID<>0 
                                              AND (b.CustomerFavShop IS NULL OR b.CustomerFavShop=0 OR b.CustomerFavShop IS NOT NULL OR b.CustomerFavShop<>0)").ToString();

            arrFlUp[15] = c.returnAggregate(@"SELECT COUNT(a.[OrderID]) 
                                              FROM [dbo].[OrdersData] a 
                                              INNER JOIN [dbo].[CustomersData] b ON a.[FK_OrderCustomerID] = b.[CustomrtID]
                                              LEFT JOIN [dbo].[OrdersAssign] c ON a.[OrderID] = c.[FK_OrderID]
                                              WHERE a.[OrderStatus] = 3 
                                              AND a.[FollowupStatus] = 'Active'
                                              AND c.[FK_OrderID] IS NULL 
                                              AND NOT EXISTS (
                                                      SELECT 1
                                                      FROM [dbo].[OrdersAssign] oa
                                                      WHERE oa.[FK_OrderID] = a.[OrderID]
                                              )").ToString();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetFollowupStats", ex.Message.ToString());
            return;
        }
    }

    private void CheckUserTask()
    {
        try
        {
            int teamId = Convert.ToInt32(Session["adminSupport"]);
            int taskId = Convert.ToInt32(c.GetReqData("SupportTeam", "TeamTaskID", "TeamID=" + teamId + ""));
            switch (taskId)
            {
                case 1:
                    teamManager.Visible = false;
                    teamStaff.Visible = true;
                    purchaseDeptID.Visible = false;
                    trainee.Visible = false;
                    payment.Visible = false;
                    GetStaffData();
                    GetTodaysFeedBkCount(taskId);
                    break;
                case 2:
                    teamManager.Visible = false;
                    teamStaff.Visible = true;
                    purchaseDeptID.Visible = false;
                    trainee.Visible = false;
                    payment.Visible = false;
                    GetStaffData();
                    GetTodaysFeedBkCount(taskId);
                    break;
                case 3:
                    teamManager.Visible = false;
                    teamStaff.Visible = true;
                    purchaseDeptID.Visible = false;
                    trainee.Visible = false;
                    payment.Visible = false;
                    GetStaffData();
                    GetTodaysFeedBkCount(taskId);
                    break;
                case 4:
                    teamManager.Visible = false;
                    teamStaff.Visible = true;
                    purchaseDeptID.Visible = false;
                    trainee.Visible = false;
                    payment.Visible = false;
                    GetStaffData();
                    GetTodaysFeedBkCount(taskId);
                    break;
                case 5:
                    teamManager.Visible = false;
                    teamStaff.Visible = true;
                    purchaseDeptID.Visible = false;
                    trainee.Visible = false;
                    payment.Visible = false;
                    GetStaffData();
                    GetTodaysFeedBkCount(taskId);
                    break;
                case 6:
                    teamManager.Visible = false;
                    teamStaff.Visible = true;
                    purchaseDeptID.Visible = false;
                    trainee.Visible = false;
                    payment.Visible = false;
                    GetStaffData();
                    GetTodaysFeedBkCount(taskId);
                    break;
                case 7:
                    purchaseDeptID.Visible = true;
                    teamStaff.Visible = false;
                    teamManager.Visible = false;
                    trainee.Visible = false;
                    payment.Visible = false;
                    GetPurchaseDeptCountData();
                    GetTodaysFeedBkCount(taskId);
                    break;
                case 8:
                    teamManager.Visible = false;
                    teamStaff.Visible = true;
                    purchaseDeptID.Visible = false;
                    trainee.Visible = false;
                    payment.Visible = false;
                    GetStaffData();
                    GetTodaysFeedBkCount(taskId);
                    break;
                case 9:
                    teamManager.Visible = false;
                    teamStaff.Visible = false;
                    purchaseDeptID.Visible = false;
                    trainee.Visible = true;
                    payment.Visible = false;
                    break;
                case 10:
                    teamManager.Visible = false;
                    teamStaff.Visible = false;
                    purchaseDeptID.Visible = false;
                    trainee.Visible = false;
                    payment.Visible = true;
                    break;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "CheckUserTask", ex.Message.ToString());
            return;
        }
    }

    protected void GetTeamOrder()
    {
        try
        {
            if (Session["adminSupport"].ToString() == "1")
            {
                todistribute.Visible = true;
                today.Visible = false;
                arrFlUp[16] = c.returnAggregate("SELECT COUNT(DISTINCT a.[FK_OrderCustomerID]) " +
                " FROM [dbo].[OrdersData] a " +
                " INNER JOIN [dbo].[CustomersData] b ON a.[FK_OrderCustomerID] = b.[CustomrtID] " +
                " INNER JOIN [dbo].[OrdersAssign] c ON a.[OrderID] = c.[FK_OrderID] " +
                " INNER JOIN[dbo].[CompanyOwnShops] d ON c.[FK_FranchID] = d.[FK_FranchID] " +
                " WHERE a.[FollowupStatus] = 'Active' " +
                " AND a.[OrderStatus] IN(6, 7) " +
                " AND c.[OrdReAssign] = 0 " +
                " AND a.OrderDate <= GETDATE() " +
                " AND a.OrderDate >= DATEADD(MONTH, -6, GETDATE()) " +
                " AND DAY(a.OrderDate) = DATEPART(DAY, GETDATE())").ToString(); //today's flup
            }
            else
            {
                todistribute.Visible = false;
                today.Visible = true;
                arrFlUp[5] = c.returnAggregate("Select COUNT(FA.FlpAsnId) " +
                " From FollowupAssign FA " +
                " Inner Join OrdersData OD on FA.FK_OrderId=OD.OrderID " +
                " Where CONVERT(VARCHAR(20), FA.FlpAsnDate, 112) = CONVERT(VARCHAR(20), CAST('" + DateTime.Now.Date + "' AS DATETIME), 112)" +
                " AND FA.FK_TeamID=" + Session["adminSupport"].ToString() + " AND OD.[FollowupStatus] = 'Active' AND [FlpAsnStatus] IN ('Pending', 'Postpone')").ToString(); //today's flup
            }
        }
        catch (Exception ex)
        { 
            
        }
    }

    protected void GetCount()
    {
        try
        {
            arrCounts[0] = c.returnAggregate("Select Count(a.OrderID) From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID  Where a.OrderStatus=1 AND a.OrderType=1 AND (b.CustomerFavShop IS NULL OR b.CustomerFavShop=0)").ToString(); //new orders
            arrCounts[8] = c.returnAggregate("Select Count(a.OrderID) From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID  Where a.OrderStatus=1 AND a.OrderType=1 AND (b.CustomerFavShop IS NOT NULL OR b.CustomerFavShop<>0)").ToString(); // new fav shop orders
            arrCounts[6] = c.returnAggregate("Select Count(a.OrderID) From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID Where a.OrderStatus<>0 AND a.OrderType IS NOT NULL AND a.OrderType<>0").ToString();

            arrCounts[4] = c.returnAggregate("Select Count(a.OrderID) From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID Where a.OrderStatus=1 AND a.OrderType=2 AND a.FK_OrderCustomerID IS NOT NULL AND a.FK_OrderCustomerID<>0 AND (b.CustomerFavShop IS NULL OR b.CustomerFavShop=0)").ToString(); // new rx orders
            arrCounts[9] = c.returnAggregate("Select Count(a.OrderID) From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID Where a.OrderStatus=1 AND a.OrderType=2 AND a.FK_OrderCustomerID IS NOT NULL AND a.FK_OrderCustomerID<>0 AND (b.CustomerFavShop IS NOT NULL OR b.CustomerFavShop<>0)").ToString(); // new fav shop rx orders

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FillGrid", ex.Message.ToString());
            return;
        }
    }

    private void GetManagersDataCount()
    {
        try
        {
            //active team members
            managersDataCount[0] = c.returnAggregate("Select Count(TeamID) From SupportTeam Where TeamAuthority=2 And TeamUserStatus=0").ToString();
            //blocked team members
            managersDataCount[1] = c.returnAggregate("Select Count(TeamID) From SupportTeam Where TeamAuthority=2 And TeamUserStatus=1").ToString();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetManagersDataCount", ex.Message.ToString());
            return;
        }
    }

    private void GetStaffData()
    {
        try
        {
            int teamId = Convert.ToInt32(Session["adminSupport"]);
            int taskId = Convert.ToInt32(c.GetReqData("SupportTeam", "TeamTaskID", "TeamID=" + teamId + ""));
            dataCount = c.returnAggregate("Select Count(FeedBkID) From FeedbackData Where FK_TeamID=" + Session["adminSupport"] + " And FeedBkTaskID=" + taskId + "").ToString();

            switch (taskId)
            {
                case 1:
                    cardName = "Registered Customer Follow Up completed till now";
                    break;
                case 2:
                    cardName = "Delivered Order Follow Up completed till now";
                    break;
                case 3:
                    cardName = "All Order Follow Up completed till now";
                    break;
                case 4:
                    cardName = "Lab Appointment Follow Up completed till now";
                    break;
                case 5:
                    cardName = "Doctor Appointment Follow Up completed till now";
                    break;
                case 6:
                    cardName = "Prescription Request Follow Up completed till now";
                    break;
                case 8:
                    cardName = "Company Owned Shop Orders Follow Up completed till now";
                    break;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetStaffData", ex.Message.ToString());
            return;
        }
    }

    private void GetPurchaseDeptCountData()
    {
        try
        {
            purchaseData[0] = c.returnAggregate("Select Count(ProductID) From ProductsData Where delMark=0 AND ProductActive=1").ToString();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetPurchaseDeptCountData", ex.Message.ToString());
            return;
        }
    }

    private void GetTodaysFeedBkCount(int taskId)
    {
        try
        {
            DateTime todayDate = DateTime.Now;
            string currentDate = todayDate.ToString();
            todaysCount = c.returnAggregate("select count(*) from FeedbackData where cast(FeedBkDate as date)='" + currentDate + "' And FeedBkTaskID=" + taskId + " And FK_TeamID=" + Session["adminSupport"] + "").ToString();
            overallCount = c.returnAggregate("select count(*) from FeedbackData where FeedBkTaskID=" + taskId + "").ToString();
            //switch (taskId)
            //{
            //    case 1:
            //        todaysCount = c.returnAggregate("select count(*) from FeedbackData where cast(FeedBkDate as date)='"+ currentDate +"' And FeedBkTaskID=1").ToString();
            //        overallCount = c.returnAggregate("select count(*) from FeedbackData where FeedBkTaskID="+ taskId +"").ToString();
            //        break;
            //}
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetTodaysFeedBkCount", ex.Message.ToString());
            return;
        }
    }
}