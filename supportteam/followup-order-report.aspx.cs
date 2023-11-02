using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Data.Sql;
using System.Data.SqlClient;

public partial class supportteam_followup_order_report : System.Web.UI.Page
{
    iClass c = new iClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                FillDDR();
                if (Request.QueryString["type"] != null)
                {
                    if (Request.QueryString["type"] == "inactive")
                    {
                        DateTime tempDate = DateTime.Now.AddMonths(-1);
                        //txtYear.Text = tempDate.Year.ToString();
                        ddrMonth.SelectedValue = tempDate.Month.ToString();
                    }
                }
                else
                {
                    //txtYear.Text = DateTime.Now.Year.ToString();
                    DateTime tempDate = DateTime.Now.AddMonths(0);
                    ddrMonth.SelectedValue = tempDate.Month.ToString();
                }
                FillGrid();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
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
            if (Request.QueryString["type"] != null)
            {
                if (Request.QueryString["type"] == "inactive")
                {
                    endDate = DateTime.Now.AddMonths(-3);
                    startDate = endDate.AddMonths(-11);
                    
                }
            }
            else
            {
                endDate = DateTime.Now.AddMonths(0);
                startDate = endDate.AddMonths(-2);
            }
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
            string strQuery = "";
            string[] arrYear = ddrMonth.SelectedItem.Text.ToString().Split('-');
            string yearData = arrYear[1].ToString();
            if (Request.QueryString["type"] != null)
            {
                if (Request.QueryString["type"] == "inactive")
                {
                    dateRange.Visible = true;

                    if (Request.QueryString["shop"] != null)
                    {
                        if (Request.QueryString["shop"] == "own")
                        {
                            strQuery = "SELECT " 
                                       + "a.[FK_OrderCustomerID] as FK_OrderCustomerID, "
                                       + "a.[OrderID] as OrderID, "
                                       + "MAX(a.[OrderSalesBillNumber]) as SaleBillNo,"
                                       + "MAX(a.[OrderStatus]) as OrderStatus, "
                                       + "MAX(CONVERT(VARCHAR(20), a.[FollowupNextDate], 103)) as FollowupNextDate,"
                                       + "MAX(DATEDIFF(DAY, a.[FollowupLastDate], GETDATE())) AS DateDiff,"
                                       + "MAX(a.[DeviceType]) as DeviceType,"
                                       + "MAX('#' + CAST(a.[OrderID] as VARCHAR(50)) + ' - ' + CONVERT(VARCHAR(20), a.[OrderDate], 103) + ' - ' + CAST(a.[OrderAmount] as VARCHAR(20)) + '/-' + CASE WHEN a.[OrderType] = 1 THEN 'Regular Order' ELSE 'Prescription Order' END) as ordInfo,"
                                       + "MAX((CASE WHEN a.[DispatchDate] IS NOT NULL THEN CONVERT(VARCHAR(20), a.[DispatchDate], 103) WHEN a.[DispatchDate] IS NULL THEN 'NA' END) + ' - ' + CONVERT(VARCHAR(20), a.[EstimatedDeliveryDate], 103)) as deliverydate,"
                                       + "MAX(CONVERT(VARCHAR(20), a.[OrderDate], 103) + ' - ' + CONVERT(VARCHAR(20), a.[FollowupNextDate], 103)) as flLastDate, "
                                       + "MAX(b.[CustomerName]) as CustomerName, "
                                       + "MAX(b.[CustomerMobile]) as CustomerMobile,"
                                       + "(SELECT COUNT([FlupID]) FROM [dbo].[FollowupOrders] WHERE [FK_CustomerId] = a.[FK_OrderCustomerID] AND [FK_OrderId] = a.[OrderID]) as flCount,"
                                       + "ISNULL((SELECT TOP 1 c.[TeamPersonName] FROM [dbo].[SupportTeam] c INNER JOIN [dbo].[FollowupOrders] d ON c.[TeamID] = d.[FK_TeamMemberId] WHERE d.[FK_CustomerId] = a.[FK_OrderCustomerID] AND d.[FK_OrderId] = a.[OrderID] ORDER BY d.[FlupID] DESC), 'NA') as flBy"                                       
                                       + " FROM [dbo].[OrdersData] a" 
                                       + " INNER JOIN [dbo].[CustomersData] b ON a.[FK_OrderCustomerID] = b.[CustomrtID]"
                                       + " INNER JOIN [dbo].[OrdersAssign] c ON a.[OrderID] = c.[FK_OrderID]"
                                       + " INNER JOIN [dbo].[CompanyOwnShops] d ON c.[Fk_FranchID] = d.[FK_FranchID]"
                                       + " LEFT JOIN [dbo].[FollowupOrders] e ON a.[OrderID] = e.[FK_OrderId]"
                                       + " WHERE a.[FollowupStatus] = 'Active'"
                                       + " AND a.[OrderStatus] = 7"
                                       //+ " AND e.FlupRemarkStatusID <> 5"
                                       + " AND c.[OrdReAssign] = 0"
                                       // +" AND a.FollowupLastDate > DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE())-6, 0) Order by a.OrderDate"))
                                       // +" AND a.FollowupNextDate < DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE())-2, 0) Order by a.OrderDate";
                                       + " AND YEAR(a.[FollowupNextDate]) = " + yearData + " AND MONTH(a.[FollowupNextDate]) = '" + Convert.ToInt32(ddrMonth.SelectedValue) + "'"
                                       + " GROUP BY a.[OrderID],a.[FK_OrderCustomerID]"
                                       + " ORDER BY MAX(a.[FollowupNextDate]) DESC";
                        }
                        else
                        {
                            strQuery = "SELECT "
                                       + "a.[FK_OrderCustomerID] as FK_OrderCustomerID, "
                                       + "a.[OrderID] as OrderID, "
                                       + "MAX(a.[OrderSalesBillNumber]) as SaleBillNo,"
                                       + "MAX(a.[OrderStatus]) as OrderStatus, "
                                       + "MAX(CONVERT(VARCHAR(20), a.[FollowupNextDate], 103)) as FollowupNextDate,"
                                       + "MAX(DATEDIFF(DAY, a.[FollowupLastDate], GETDATE())) AS DateDiff,"
                                       + "MAX(a.[DeviceType]) as DeviceType,"
                                       + "MAX('#' + CAST(a.[OrderID] as VARCHAR(50)) + ' - ' + CONVERT(VARCHAR(20), a.[OrderDate], 103) + ' - ' + CAST(a.[OrderAmount] as VARCHAR(20)) + '/-' + CASE WHEN a.[OrderType] = 1 THEN 'Regular Order' ELSE 'Prescription Order' END) as ordInfo,"
                                       + "MAX((CASE WHEN a.[DispatchDate] IS NOT NULL THEN CONVERT(VARCHAR(20), a.[DispatchDate], 103) WHEN a.[DispatchDate] IS NULL THEN 'NA' END) + ' - ' + CONVERT(VARCHAR(20), a.[EstimatedDeliveryDate], 103)) as deliverydate,"
                                       + "MAX(CONVERT(VARCHAR(20), a.[OrderDate], 103) + ' - ' + CONVERT(VARCHAR(20), a.[FollowupNextDate], 103)) as flLastDate, "
                                       + "MAX(b.[CustomerName]) as CustomerName, "
                                       + "MAX(b.[CustomerMobile]) as CustomerMobile,"
                                       + "(SELECT COUNT([FlupID]) FROM [dbo].[FollowupOrders] WHERE [FK_CustomerId] = a.[FK_OrderCustomerID] AND [FK_OrderId] = a.[OrderID]) as flCount,"
                                       + "ISNULL((SELECT TOP 1 c.[TeamPersonName] FROM [dbo].[SupportTeam] c INNER JOIN [dbo].[FollowupOrders] d ON c.[TeamID] = d.[FK_TeamMemberId] WHERE d.[FK_CustomerId] = a.[FK_OrderCustomerID] AND d.[FK_OrderId] = a.[OrderID] ORDER BY d.[FlupID] DESC), 'NA') as flBy"                                       
                                       + " FROM [dbo].[OrdersData] a"
                                       + " INNER JOIN [dbo].[CustomersData] b ON a.[FK_OrderCustomerID] = b.[CustomrtID]"
                                       + " INNER JOIN [dbo].[OrdersAssign] c ON a.[OrderID] = c.[FK_OrderID]"
                                       + " LEFT OUTER JOIN [dbo].[CompanyOwnShops] d ON c.[Fk_FranchID] = d.[FK_FranchID]"
                                       + " LEFT JOIN [dbo].[FollowupOrders] e ON a.[OrderID] = e.[FK_OrderId]"
                                       + " WHERE a.[FollowupStatus] = 'Active'"
                                       + " AND a.[OrderStatus] = 7"
                                       //+ " AND e.FlupRemarkStatusID <> 5"
                                       + " AND c.[OrdReAssign] = 0"
                                       + " AND d.[FK_FranchID] IS NULL"
                                       //+ " AND a.FollowupLastDate > DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) - 6, 0) Order by a.OrderDate"))
                                       //+ " AND a.FollowupNextDate < DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) - 2, 0) Order by a.OrderDate";
                                       + " AND YEAR(a.[FollowupNextDate]) = " + yearData + " AND MONTH(a.[FollowupNextDate]) = '" + Convert.ToInt32(ddrMonth.SelectedValue) + "'"
                                       + " GROUP BY a.[OrderID],a.[FK_OrderCustomerID]"
                                       + " ORDER BY MAX(a.[FollowupNextDate]) DESC";
                        }
                    }
                }
                else if (Request.QueryString["type"] == "fresh")
                {
                    // get last 3 days data
                    if (Request.QueryString["shop"] != null)
                    {
                        if (Request.QueryString["shop"] == "own")
                        {
                            strQuery = "SELECT "
                                       + "a.[FK_OrderCustomerID] as FK_OrderCustomerID, "
                                       + "a.[OrderID] as OrderID, "
                                       + "MAX(a.[OrderSalesBillNumber]) as SaleBillNo,"
                                       + "MAX(a.[OrderStatus]) as OrderStatus, "
                                       + "MAX(CONVERT(VARCHAR(20), a.[FollowupNextDate], 103)) as FollowupNextDate,"
                                       + "MAX(DATEDIFF(DAY, a.[FollowupLastDate], GETDATE())) AS DateDiff,"
                                       + "MAX(a.[DeviceType]) as DeviceType,"
                                       + "MAX('#' + CAST(a.[OrderID] as VARCHAR(50)) + ' - ' + CONVERT(VARCHAR(20), a.[OrderDate], 103) + ' - ' + CAST(a.[OrderAmount] as VARCHAR(20)) + '/-' + CASE WHEN a.[OrderType] = 1 THEN 'Regular Order' ELSE 'Prescription Order' END) as ordInfo,"
                                       + "MAX((CASE WHEN a.[DispatchDate] IS NOT NULL THEN CONVERT(VARCHAR(20), a.[DispatchDate], 103) WHEN a.[DispatchDate] IS NULL THEN 'NA' END) + ' - ' + CONVERT(VARCHAR(20), a.[EstimatedDeliveryDate], 103)) as deliverydate,"
                                       + "MAX(CONVERT(VARCHAR(20), a.[OrderDate], 103) + ' - ' + CONVERT(VARCHAR(20), a.[FollowupNextDate], 103)) as flLastDate, "
                                       + "MAX(b.[CustomerName]) as CustomerName, "
                                       + "MAX(b.[CustomerMobile]) as CustomerMobile,"
                                       + "(SELECT COUNT([FlupID]) FROM [dbo].[FollowupOrders] WHERE [FK_CustomerId] = a.[FK_OrderCustomerID] AND [FK_OrderId] = a.[OrderID]) as flCount,"
                                       + "ISNULL((SELECT TOP 1 c.[TeamPersonName] FROM [dbo].[SupportTeam] c INNER JOIN [dbo].[FollowupOrders] d ON c.[TeamID] = d.[FK_TeamMemberId] WHERE d.[FK_CustomerId] = a.[FK_OrderCustomerID] AND d.[FK_OrderId] = a.[OrderID] ORDER BY d.[FlupID] DESC), 'NA') as flBy"                                       
                                       + " FROM [dbo].[OrdersData] a"
                                       + " INNER JOIN [dbo].[CustomersData] b ON a.[FK_OrderCustomerID] = b.[CustomrtID]"
                                       + " LEFT JOIN [dbo].[FollowupOrders] e ON a.[OrderID] = e.[FK_OrderId]"
                                       //+ " Inner Join OrdersAssign c On a.OrderID = c.FK_OrderID Inner Join CompanyOwnShops d On c.Fk_FranchID = d.FK_FranchID " +
                                       + " WHERE (a.[FollowupStatus] = 'Active' OR a.[FollowupStatus] IS NULL)"
                                       + " AND a.[OrderStatus] = 1"
                                       //+ " AND e.FlupRemarkStatusID <> 5"
                                       + " GROUP BY a.[OrderID],a.[FK_OrderCustomerID]"
                                       + " ORDER BY MAX(a.[FollowupNextDate]) DESC";
                                //" AND c.OrdReAssign=0 ";
                            //" AND a.OrderDate >= DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE())-15, 0)";
                        }
                        else
                        {
                            strQuery = "SELECT "
                                       + "a.[FK_OrderCustomerID] as FK_OrderCustomerID, "
                                       + "a.[OrderID] as OrderID, "
                                       + "MAX(a.[OrderSalesBillNumber]) as SaleBillNo,"
                                       + "MAX(a.[OrderStatus]) as OrderStatus, "
                                       + "MAX(CONVERT(VARCHAR(20), a.[FollowupNextDate], 103)) as FollowupNextDate,"
                                       + "MAX(DATEDIFF(DAY, a.[FollowupLastDate], GETDATE())) AS DateDiff,"
                                       + "MAX(a.[DeviceType]) as DeviceType,"
                                       + "MAX('#' + CAST(a.[OrderID] as VARCHAR(50)) + ' - ' + CONVERT(VARCHAR(20), a.[OrderDate], 103) + ' - ' + CAST(a.[OrderAmount] as VARCHAR(20)) + '/-' + CASE WHEN a.[OrderType] = 1 THEN 'Regular Order' ELSE 'Prescription Order' END) as ordInfo,"
                                       + "MAX((CASE WHEN a.[DispatchDate] IS NOT NULL THEN CONVERT(VARCHAR(20), a.[DispatchDate], 103) WHEN a.[DispatchDate] IS NULL THEN 'NA' END) + ' - ' + CONVERT(VARCHAR(20), a.[EstimatedDeliveryDate], 103)) as deliverydate,"
                                       + "MAX(CONVERT(VARCHAR(20), a.[OrderDate], 103) + ' - ' + CONVERT(VARCHAR(20), a.[FollowupNextDate], 103)) as flLastDate, "
                                       + "MAX(b.[CustomerName]) as CustomerName, "
                                       + "MAX(b.[CustomerMobile]) as CustomerMobile,"
                                       + "(SELECT COUNT([FlupID]) FROM [dbo].[FollowupOrders] WHERE [FK_CustomerId] = a.[FK_OrderCustomerID] AND [FK_OrderId] = a.[OrderID]) as flCount,"
                                       + "ISNULL((SELECT TOP 1 c.[TeamPersonName] FROM [dbo].[SupportTeam] c INNER JOIN [dbo].[FollowupOrders] d ON c.[TeamID] = d.[FK_TeamMemberId] WHERE d.[FK_CustomerId] = a.[FK_OrderCustomerID] AND d.[FK_OrderId] = a.[OrderID] ORDER BY d.[FlupID] DESC), 'NA') as flBy"                                       
                                       + " FROM [dbo].[OrdersData] a"
                                       + " INNER JOIN [dbo].[CustomersData] b ON a.[FK_OrderCustomerID] = b.[CustomrtID]"
                                       + " LEFT JOIN [dbo].[FollowupOrders] e ON a.[OrderID] = e.[FK_OrderId]"
                                       //+ " Inner Join OrdersAssign c On a.OrderID = c.FK_OrderID Left Outer Join CompanyOwnShops d On c.Fk_FranchID = d.FK_FranchID " +
                                       + " WHERE(a.[FollowupStatus] = 'Active' OR a.[FollowupStatus] IS NULL)"
                                       + " AND a.[OrderStatus] = 1"
                                       + " AND e.[FlupRemarkStatusID] <> 5"
                                       + " GROUP BY a.[OrderID],a.[FK_OrderCustomerID]"
                                       + " ORDER BY MAX(a.[FollowupNextDate]) DESC";  //AND d.FK_FranchID is null 
                                //" AND c.OrdReAssign=0 " ;
                            //" AND a.OrderDate >= DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE())-15, 0)";
                        }
                    }
                }
                else if (Request.QueryString["type"] == "today")
                {
                    if (Request.QueryString["shop"] != null)
                    {
                        if (Request.QueryString["shop"] == "own")
                        {
                            strQuery = @"SELECT "
                                        + "a.[FK_OrderCustomerID] as FK_OrderCustomerID,"
                                        + "a.[OrderID] as OrderID,"
                                        + "MAX(a.[OrderSalesBillNumber]) as SaleBillNo,"
                                        + "MAX(a.[OrderStatus]) as OrderStatus,"
                                        + "MAX(CONVERT(VARCHAR(20), a.[FollowupNextDate], 103)) as FollowupNextDate,"
                                        + "MAX(DATEDIFF(DAY, a.[FollowupLastDate], GETDATE())) as DateDiff,"
                                        + "MAX(a.[DeviceType]) as DeviceType,"
                                        + "MAX('#' + CAST(a.[OrderID] as VARCHAR(50)) + ' - ' + CONVERT(VARCHAR(20), a.[OrderDate], 103) + ' - ' + CAST(a.[OrderAmount] as VARCHAR(20)) + '/-' + CASE WHEN a.[OrderType] = 1 THEN 'Regular Order' ELSE 'Prescription Order' END) as ordInfo,"
                                        + "MAX((CASE WHEN a.[DispatchDate] IS NOT NULL THEN CONVERT(VARCHAR(20), a.[DispatchDate], 103) WHEN a.[DispatchDate] IS NULL THEN 'NA' END) + ' - ' + CONVERT(VARCHAR(20), a.[EstimatedDeliveryDate], 103)) as deliverydate,"
                                        + "MAX(CONVERT(VARCHAR(20), a.[OrderDate], 103) + ' - ' + CONVERT(VARCHAR(20), a.[FollowupNextDate], 103)) as flLastDate,"
                                        + "MAX(b.[CustomerName]) as CustomerName, "
                                        + "MAX(b.[CustomerMobile]) as CustomerMobile,"
                                        + "(SELECT COUNT([FlupID]) FROM [dbo].[FollowupOrders] WHERE [FK_CustomerId] = a.[FK_OrderCustomerID] AND [FK_OrderId] = a.[OrderID]) as flCount,"
                                        + "ISNULL((SELECT TOP 1 c.[TeamPersonName] FROM [dbo].[SupportTeam] c INNER JOIN [dbo].[FollowupOrders] d ON c.[TeamID] = d.[FK_TeamMemberId] WHERE d.[FK_CustomerId] = a.[FK_OrderCustomerID] AND d.[FK_OrderId] = a.[OrderID] ORDER BY d.[FlupID] DESC), 'NA') as flBy"                                        
                                        + " FROM [dbo].[OrdersData] a"
                                        + " INNER JOIN [dbo].[CustomersData] b ON a.[FK_OrderCustomerID] = b.[CustomrtID]"
                                        + " INNER JOIN [dbo].[OrdersAssign] c ON a.[OrderID] = c.[FK_OrderID]"
                                        + " INNER JOIN [dbo].[CompanyOwnShops] d ON c.[Fk_FranchID] = d.[FK_FranchID]"                            
                                        + " LEFT JOIN [dbo].[FollowupOrders] e ON a.[OrderID] = e.[FK_OrderId]"
                                        + " WHERE a.[FollowupStatus] = 'Active'"
                                        + " AND a.[OrderStatus] = 7"
                                        + " AND c.[OrdReAssign] = 0"
                                        //+ " AND a.FollowupNextDate >= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) - 2, 0)"
                                        //+ " AND DAY(a.FollowupNextDate)= '" + DateTime.Now.Day + "'"
                                        + " AND CONVERT(VARCHAR(20), a.[FollowupNextDate], 112) = CONVERT(VARCHAR(20), CAST(GETDATE() as DATETIME), 112)"
                                        + " GROUP BY a.[OrderID],a.[FK_OrderCustomerID]"
                                        + " ORDER BY MAX(a.[FollowupNextDate]) DESC";
                        }
                        else
                        {
                            strQuery = "SELECT "
                                       + "a.[FK_OrderCustomerID] as FK_OrderCustomerID, "
                                       + "a.[OrderID] as OrderID, "
                                       + "MAX(a.[OrderSalesBillNumber]) as SaleBillNo,"
                                       + "MAX(a.[OrderStatus]) as OrderStatus, "
                                       + "MAX(CONVERT(VARCHAR(20), a.[FollowupNextDate], 103)) as FollowupNextDate,"
                                       + "MAX(DATEDIFF(DAY, a.[FollowupLastDate], GETDATE())) AS DateDiff,"
                                       + "MAX(a.[DeviceType]) as DeviceType,"
                                       + "MAX('#' + CAST(a.[OrderID] as VARCHAR(50)) + ' - ' + CONVERT(VARCHAR(20), a.[OrderDate], 103) + ' - ' + CAST(a.[OrderAmount] as VARCHAR(20)) + '/-' + CASE WHEN a.[OrderType] = 1 THEN 'Regular Order' ELSE 'Prescription Order' END) as ordInfo,"
                                       + "MAX((CASE WHEN a.[DispatchDate] IS NOT NULL THEN CONVERT(VARCHAR(20), a.[DispatchDate], 103) WHEN a.[DispatchDate] IS NULL THEN 'NA' END) + ' - ' + CONVERT(VARCHAR(20), a.[EstimatedDeliveryDate], 103)) as deliverydate,"
                                       + "MAX(CONVERT(VARCHAR(20), a.[OrderDate], 103) + ' - ' + CONVERT(VARCHAR(20), a.[FollowupNextDate], 103)) as flLastDate, "
                                       + "MAX(b.[CustomerName]) as CustomerName, "
                                       + "MAX(b.[CustomerMobile]) as CustomerMobile,"
                                       + "(SELECT COUNT([FlupID]) FROM [dbo].[FollowupOrders] WHERE [FK_CustomerId] = a.[FK_OrderCustomerID] AND [FK_OrderId] = a.[OrderID]) as flCount,"
                                       + "ISNULL((SELECT TOP 1 c.[TeamPersonName] FROM [dbo].[SupportTeam] c INNER JOIN [dbo].[FollowupOrders] d ON c.[TeamID] = d.[FK_TeamMemberId] WHERE d.[FK_CustomerId] = a.[FK_OrderCustomerID] AND d.[FK_OrderId] = a.[OrderID] ORDER BY d.[FlupID] DESC), 'NA') as flBy"                                       
                                       + " FROM [dbo].[OrdersData] a"
                                       + " INNER JOIN [dbo].[CustomersData] b ON a.[FK_OrderCustomerID] = b.[CustomrtID]"
                                       + " INNER JOIN [dbo].[OrdersAssign] c ON a.[OrderID] = c.[FK_OrderID]"
                                       + " LEFT OUTER JOIN [dbo].[CompanyOwnShops] d ON c.[Fk_FranchID] = d.[FK_FranchID]"
                                       + " LEFT JOIN [dbo].[FollowupOrders] e ON a.[OrderID] = e.[FK_OrderId]"
                                       + " WHERE a.[FollowupStatus] = 'Active'"
                                       + " AND a.[OrderStatus] = 7"
                                       //+ " AND e.FlupRemarkStatusID <> 5"
                                       + " AND c.[OrdReAssign] = 0"
                                       + " AND d.[FK_FranchID] IS NULL"
                                       + " AND a.[FollowupNextDate] >= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) - 2, 0)"
                                       + " AND DAY(a.[FollowupNextDate])= '" + DateTime.Now.Day + "'"                                   
                                       + " AND CONVERT(VARCHAR(20), a.[FollowupNextDate], 112) <= CONVERT(VARCHAR(20), CAST(GETDATE() as DATETIME), 112)"
                                       + " GROUP BY a.[OrderID],a.[FK_OrderCustomerID]"
                                       + " ORDER BY MAX(a.[FollowupNextDate]) DESC";
                        }
                    }
                }
            }
            else
            {
                dateRange.Visible = true;
                //DateTime tempDate = DateTime.Now.AddMonths(-1);
                //txtYear.Text = tempDate.Year.ToString();
                //ddrMonth.SelectedValue = tempDate.Month.ToString();

                if (Request.QueryString["shop"] != null)
                {
                    if (Request.QueryString["shop"] == "own")
                    {
                        strQuery = "SELECT "
                                   + "a.[FK_OrderCustomerID] as FK_OrderCustomerID,"
                                   + "a.[OrderID] as OrderID, "
                                   + "MAX(a.[OrderSalesBillNumber]) as SaleBillNo,"
                                   + "MAX(a.[OrderStatus]) as OrderStatus, "
                        	       + "MAX(CONVERT(VARCHAR(20), a.[FollowupNextDate], 103)) as FollowupNextDate,"
                        	       + "MAX(DATEDIFF(DAY, a.[FollowupLastDate], GETDATE())) AS DateDiff,"
                                   + "MAX(a.[DeviceType]) as DeviceType, "
                        	       + "MAX('#' + CAST(a.[OrderID] as VARCHAR(50)) + ' - ' + CONVERT(VARCHAR(20), a.[OrderDate], 103) + ' - ' + CAST(a.[OrderAmount] as VARCHAR(20)) + '/-' + CASE WHEN a.[OrderType] = 1 THEN 'Regular Order' ELSE 'Prescription Order' END) as ordInfo, "
                                   + "MAX((CASE WHEN a.[DispatchDate] IS NOT NULL THEN CONVERT(VARCHAR(20), a.[DispatchDate], 103) WHEN a.[DispatchDate] IS NULL THEN 'NA' END) + ' - ' + CONVERT(VARCHAR(20), a.[EstimatedDeliveryDate], 103)) as deliverydate,"
                                   + "MAX(CONVERT(VARCHAR(20), a.[OrderDate], 103) + ' - ' + CONVERT(VARCHAR(20), a.[FollowupNextDate], 103)) as flLastDate,  "
                                   + "MAX(b.[CustomerName]) as CustomerName, "
                        	       + "MAX(b.[CustomerMobile]) as CustomerMobile,"
                        	       + "(SELECT COUNT([FlupID]) FROM [dbo].[FollowupOrders] WHERE [FK_CustomerId] = a.[FK_OrderCustomerID] AND [FK_OrderId] = a.[OrderID]) as flCount,"
                        	       + "ISNULL((SELECT TOP 1 c.[TeamPersonName] FROM [dbo].[SupportTeam] c INNER JOIN [dbo].[FollowupOrders] d ON c.[TeamID] = d.[FK_TeamMemberId] WHERE d.[FK_CustomerId] = a.[FK_OrderCustomerID] AND d.[FK_OrderId] = a.[OrderID] ORDER BY d.[FlupID] DESC), 'NA') as flBy"                                   
                                   + " FROM [dbo].[OrdersData] a"
                                   + " INNER JOIN [dbo].[CustomersData] b ON a.[FK_OrderCustomerID] = b.[CustomrtID]"
                                   + " INNER JOIN [dbo].[OrdersAssign] c ON a.[OrderID] = c.[FK_OrderID]"
                                   + " INNER JOIN [dbo].[CompanyOwnShops] d ON c.[Fk_FranchID] = d.[FK_FranchID]"
                                   + " LEFT JOIN [dbo].[FollowupOrders] e ON a.[OrderID] = e.[FK_OrderId]"
                                   + " WHERE a.[FollowupStatus] = 'Active'"
                                   + " AND a.[OrderStatus] = 7"
                                   //+ " AND e.FlupRemarkStatusID <> 5"
                                   + " AND c.[OrdReAssign] = 0"
                                   //+ " AND a.FollowupLastDate > DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE())-6, 0) Order by a.OrderDate"))
                                   //+ " AND a.FollowupNextDate >= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE())-3, 0) Order by a.OrderDate";
                                   + " AND YEAR(a.[FollowupNextDate]) = " + yearData + " AND MONTH(a.[FollowupNextDate]) = '" + Convert.ToInt32(ddrMonth.SelectedValue) +"'"
                                   + " AND CONVERT(VARCHAR(20), a.[FollowupNextDate], 112) < CONVERT(VARCHAR(20), CAST(GETDATE() as DATETIME), 112)"
                                   + " GROUP BY a.[OrderID],a.[FK_OrderCustomerID]"
                                   + " ORDER BY MAX(a.[FollowupNextDate]) DESC";
                    }
                    else
                    {
                        strQuery = "SELECT "
                                   + "a.[FK_OrderCustomerID] as FK_OrderCustomerID, "
                                   + "a.[OrderID] as OrderID, "
                                   + "MAX(a.[OrderSalesBillNumber]) as SaleBillNo,"
                                   + "MAX(a.[OrderStatus]) OrderStatus, "
                                   + "MAX(CONVERT(VARCHAR(20), a.[FollowupNextDate], 103)) as FollowupNextDate,"
                                   + "MAX(DATEDIFF(DAY, a.[FollowupLastDate], GETDATE())) AS DateDiff,"
                                   + "MAX(a.[DeviceType]) as DeviceType,"
                                   + "MAX('#' + CAST(a.[OrderID] as VARCHAR(50)) + ' - ' + CONVERT(VARCHAR(20), a.[OrderDate], 103) + ' - ' + CAST(a.[OrderAmount] as VARCHAR(20)) + '/-' + CASE WHEN a.[OrderType] = 1 THEN 'Regular Order' ELSE 'Prescription Order' END) as ordInfo,"
                                   + "MAX((CASE WHEN a.[DispatchDate] IS NOT NULL THEN CONVERT(VARCHAR(20), a.[DispatchDate], 103) WHEN a.[DispatchDate] IS NULL THEN 'NA' END) + ' - ' + CONVERT(VARCHAR(20), a.[EstimatedDeliveryDate], 103)) as deliverydate,"
                                   + "MAX(CONVERT(VARCHAR(20), a.[OrderDate], 103) + ' - ' + CONVERT(VARCHAR(20), a.[FollowupNextDate], 103)) as flLastDate,  "
                                   + "MAX(b.[CustomerName]) as CustomerName, "
                                   + "MAX(b.[CustomerMobile]) as CustomerMobile,"
                                   + "(SELECT COUNT([FlupID]) FROM [dbo].[FollowupOrders] WHERE [FK_CustomerId] = a.[FK_OrderCustomerID] AND [FK_OrderId] = a.[OrderID]) as flCount,"
                                   + "ISNULL((SELECT TOP 1 c.[TeamPersonName] FROM [dbo].[SupportTeam] c INNER JOIN [dbo].[FollowupOrders] d ON c.[TeamID] = d.[FK_TeamMemberId] WHERE d.[FK_CustomerId] = a.[FK_OrderCustomerID] AND d.[FK_OrderId] = a.[OrderID] ORDER BY d.[FlupID] DESC), 'NA') as flBy"                                   
                                   + " FROM [dbo].[OrdersData] a"
                                   + " INNER JOIN [dbo].[CustomersData] b ON a.[FK_OrderCustomerID] = b.[CustomrtID]"
                                   + " INNER JOIN [dbo].[OrdersAssign] c On a.[OrderID] = c.[FK_OrderID]"
                                   + " LEFT OUTER JOIN [dbo].[CompanyOwnShops] d ON c.[Fk_FranchID] = d.[FK_FranchID]"
                                   + " LEFT JOIN [dbo].[FollowupOrders] e ON a.[OrderID] = e.[FK_OrderId]"
                                   + " WHERE a.[FollowupStatus] = 'Active'"
                                   + " AND a.[OrderStatus] = 7"
                                   //+ " AND e.FlupRemarkStatusID <> 5"
                                   + " AND c.[OrdReAssign] = 0"
                                   + " AND d.[FK_FranchID] IS NULL"
                                   //+ " AND a.FollowupLastDate > DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) - 6, 0) Order by a.OrderDate"))
                                   //+ " AND a.FollowupNextDate >= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) - 3, 0) Order by a.OrderDate";
                                   + " AND YEAR(a.[FollowupNextDate]) = " + yearData + " AND MONTH(a.[FollowupNextDate]) = '" + Convert.ToInt32(ddrMonth.SelectedValue) + "'"
                                   + " AND CONVERT(VARCHAR(20), a.[FollowupNextDate], 112) < CONVERT(VARCHAR(20), CAST(GETDATE() as DATETIME), 112)"
                                   + " GROUP BY a.[OrderID],a.[FK_OrderCustomerID]"
                                   + " ORDER BY MAX(a.[FollowupNextDate]) DESC";
                    }
                }
            }
            using (DataTable dtFlOrd = c.GetDataTable(strQuery))
            {
                gvOrdFlup.DataSource = dtFlOrd;
                gvOrdFlup.DataBind();

                if (gvOrdFlup.Rows.Count > 0)
                {
                    gvOrdFlup.UseAccessibleHeader = true;
                    gvOrdFlup.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvOrdFlup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string favShopOrder = "";
                object favShopId = c.GetReqData("[dbo].[CustomersData]", "[CustomerFavShop]", "[CustomrtID] = " + e.Row.Cells[0].Text);
                if (favShopId != DBNull.Value && favShopId != null && favShopId.ToString() != "")
                {
                    favShopOrder = "<br/><span style=\"font-size:0.8em; color:#fa0ac2; line-height:0.5; font-weight:600;\">Favourite Shop Order</span>";
                }

                string ordStatus = "";
                switch (e.Row.Cells[2].Text)
                {
                    case "1":
                        ordStatus = "<div class=\"ordNew\">New</div>" + favShopOrder;
                        break;
                    case "2":
                        ordStatus = "<div class=\"ordCancCust\">Cancel By Customer</div>";
                        break;
                    case "3":
                        ordStatus = "<div class=\"ordAccepted\">Accepted By Admin</div>" + favShopOrder;
                        break;
                    case "4":
                        string frCode = "", frInfo = "";
                        if (c.IsRecordExist("SELECT [OrdAssignID] FROM [dbo].[OrdersAssign] WHERE [OrdAssignStatus] = 2 AND [FK_OrderID] = " + e.Row.Cells[1].Text + ""))
                        {
                            int frId = Convert.ToInt32(c.GetReqData("[dbo].[OrdersAssign]", "TOP 1 [Fk_FranchID]", "[OrdAssignStatus] = 2 AND [FK_OrderID] = " + e.Row.Cells[1].Text + " ORDER BY [OrdAssignID] DESC"));
                            frCode = c.GetReqData("[dbo].[FranchiseeData]", "[FranchShopCode]", "[FranchID] = " + frId).ToString();
                            frInfo = " Rejected By " + frCode;
                        }
                        ordStatus = "<div class=\"ordDenied\">Denied By Admin " + frInfo + "</div>" + favShopOrder;
                        //ordStatus = "<div class=\"ordDenied\">Denied By Admin</div>" + favShopOrder;
                        break;
                    case "5":
                        ordStatus = "<div class=\"ordProcessing\">Processing</div>" + favShopOrder;
                        break;
                    case "6":
                        ordStatus = "<div class=\"ordShipped\">Shipped</div>" + favShopOrder;
                        break;
                    case "7":
                        ordStatus = "<div class=\"ordDelivered\">Delivered</div>" + favShopOrder;
                        break;
                    case "8":
                        ordStatus = "<div class=\"ordDenied\">Rejected By GMMH0001</div>" + favShopOrder;
                        break;
                    case "9":
                        int shopId = Convert.ToInt32(c.GetReqData("[dbo].[OrdersAssign]", "TOP 1 [Fk_FranchID]", "[OrdAssignStatus] = 2 AND [FK_OrderID] = " + e.Row.Cells[1].Text + " ORDER BY [OrdAssignID] DESC"));
                        string shopCode = c.GetReqData("[dbo].[FranchiseeData]", "[FranchShopCode]", "[FranchID] = " + shopId).ToString();
                        ordStatus = "<div class=\"ordProcessing\">Rejected by " + shopCode + " - Order Amount Low</div>" + favShopOrder;
                        //ordStatus = "<div class=\"ordProcessing\">Rejected by Shop - Order Amount Low</div>" + favShopOrder;
                        break;
                    case "10":
                        ordStatus = "<div class=\"ordDenied\">Returned By Customer</div>" + favShopOrder;
                        break;
                }

                e.Row.Cells[5].Text += ordStatus.ToString();

                string strFlup = "";
                Literal litFlUp = (Literal)e.Row.FindControl("litFlUp");
                if (Request.QueryString["type"] != null)
                {
                    if (Request.QueryString["type"] == "fresh")
                    {
                        //strFlup = " <a href=\"assign-order.aspx?custId=" + e.Row.Cells[0].Text + "&ordId=" + e.Row.Cells[1].Text + "\" class=\"btn btn-sm btn-primary\" target=\"_blank\">Assign Order</a>";
                        strFlup = " <a href=\"assign-order.aspx?id=" + e.Row.Cells[1].Text + "\" class=\"btn btn-sm btn-primary\" target=\"_blank\">Assign Order</a>";
                    }
                    else
                    {
                        strFlup = " <a href=\"followup-order-detail.aspx?custId=" + e.Row.Cells[0].Text + "&ordId=" + e.Row.Cells[1].Text + "\" class=\"btn btn-sm btn-primary\" target=\"_blank\"><i class=\"fa fas fa-user-plus\"></i> &nbsp; Followup</a>";
                    }
                }
                else
                {
                    strFlup = " <a href=\"followup-order-detail.aspx?custId=" + e.Row.Cells[0].Text + "&ordId=" + e.Row.Cells[1].Text + "\" class=\"btn btn-sm btn-primary\" target=\"_blank\"><i class=\"fa fas fa-user-plus\"></i> &nbsp; Followup</a>";
                }
                //strFlup = " <span title=\"User is Currently Locked\" class=\"text-danger text-sm\"><i class=\"fa fas fa-user-lock\"></i> &nbsp; User is currently Locked</span>";
                if (c.IsRecordExist("SELECT [CustomrtID] FROM [dbo].[CustomersData] WHERE [CallBusyFlag] = 1 AND [CustomrtID] = " + e.Row.Cells[0].Text + " AND [CallBusyBy] <>" + Session["adminSupport"]))
                {
                    strFlup = " <span title=\"User is Currently Locked\" class=\"text-danger text-sm\"><i class=\"fa fas fa-user-lock\"></i> &nbsp; User is Currently Busy</span>";
                }

                litFlUp.Text = strFlup.ToString();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvOrdFlup_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    private void MonthBind()
    {
        try
        {
            DateTimeFormatInfo info = DateTimeFormatInfo.GetInstance(null);
            for (int i = 1; i < 13; i++)
            {
                ddrMonth.Items.Add(new ListItem(info.GetMonthName(i), i.ToString()));
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Error occurred while processing your request', type: 'error'});", true);
            c.ErrorLogHandler(this.ToString(), "MonthBind", ex.Message.ToString());
            return;
        }
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        if (ddrMonth.SelectedIndex == 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Select Month & Year to fetch data', type: 'warning'});", true);
            return;
        }
        FillGrid();
    }
}