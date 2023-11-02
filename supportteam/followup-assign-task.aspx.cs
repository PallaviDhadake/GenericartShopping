using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Razorpay.Api;
using System.Threading.Tasks;

public partial class supportteam_followup_assign_task : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] arrOrd = new string[20];
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillGrid();
            GetOrderData();
            GetFlupData();

            // Check if records with the current date exist in the database
            bool recordExist;

            // Database connection string
            string connectionString = c.OpenConnection();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM [dbo].[FollowupAssign] WHERE [FlpAsnDate] = @FlpAsnDate";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Set the parameter for the current date
                    command.Parameters.AddWithValue("@FlpAsnDate", DateTime.Now.Date);

                    // Execute the query and get the count of records
                    int recordCount = (int)command.ExecuteScalar();

                    // Check if there are records with the current date
                    recordExist = (recordCount > 0);
                }
            }

            if (!recordExist)
            {
                
            }
            else
            {
                btnDistribute.Visible = false;
            }
        }
    }

    private void AssignTaskNow()
    {
        // Database connection string
        string connectionString = c.OpenConnection();

        // Fetch employees from the database
        List<Team> teams = GetTeam(connectionString);

        // Fetch tasks from the database
        List<Order> orders = GetOrder(connectionString);
        // Distribute tasks among employees
        DistributeTasks(teams, orders, connectionString);
    }

    static List<Team> GetTeam(string connectionString)
    {
        List<Team> teams = new List<Team>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT [TeamID], [TeamUserID], [TeamPersonName], [TeamMobile] FROM [dbo].[SupportTeam] WHERE [TeamTaskID] IN (1,3) AND [TeamUserStatus] = 0 AND [TeamID] IN (34,30,31,32,36,38,41,42,43,44)";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int teamID = reader.GetInt32(0);
                        string username = reader.GetString(1);
                        string name = reader.GetString(2);
                        string mobile = reader.GetString(3);
                        teams.Add(new Team { TeamID = teamID, TeamUserName = username, Name = name, MobileNo = mobile });
                    }
                }
            }
        }

        return teams;
    }

    static List<Order> GetOrder(string connectionString)
    {
        List<Order> orders = new List<Order>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = @"SELECT DISTINCT a.[FK_OrderCustomerID]
                             FROM [dbo].[OrdersData] a 
                             INNER JOIN [dbo].[CustomersData] b ON a.[FK_OrderCustomerID] = b.[CustomrtID]
                             INNER JOIN [dbo].[OrdersAssign] c ON a.[OrderID] = c.[FK_OrderID] 
                             INNER JOIN [dbo].[CompanyOwnShops] d ON c.[Fk_FranchID] = d.[FK_FranchID]
                             WHERE a.[FollowupStatus] = 'Active'
                             AND a.[OrderStatus] IN (6,7)
                             AND c.[OrdReAssign] = 0
                             AND a.OrderDate <= GETDATE()
                             AND a.OrderDate >= DATEADD(MONTH, -6, GETDATE())  
                             AND DAY(a.OrderDate)=DATEPART(DAY, GETDATE())";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int custID = reader.GetInt32(0);
                        orders.Add(new Order { FK_OrderCustomerID = custID });
                    }
                }
            }
        }

        return orders;
    }

    public void GetOrderData()
    {
        try
        {            
            arrOrd[1] = c.returnAggregate(@"SELECT COUNT(DISTINCT a.[FK_OrderCustomerID])
                                            FROM [dbo].[OrdersData] a
                                            INNER JOIN [dbo].[CustomersData] b ON a.[FK_OrderCustomerID] = b.[CustomrtID]
                                            INNER JOIN [dbo].[OrdersAssign] c ON a.[OrderID] = c.[FK_OrderID]
                                            INNER JOIN [dbo].[CompanyOwnShops] d ON c.[FK_FranchID] = d.[FK_FranchID]
                                            WHERE a.[FollowupStatus] = 'Active'
                                            AND a.[OrderStatus] IN (6,7)
                                            AND c.[OrdReAssign] = 0
                                            AND a.OrderDate <= GETDATE()
                                            AND a.OrderDate >= DATEADD(MONTH, -6, GETDATE())  
                                            AND DAY(a.OrderDate)=DATEPART(DAY, GETDATE())").ToString();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetOrderData", ex.Message.ToString());
            return;
        }
    }

    protected void btnDistribute_Click(object sender, EventArgs e)
    {
        try
        {
            AssignTaskNow();
            btnDistribute.Visible = false;
            GetOrderData();
            GetFlupData();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnDistribute_Click", ex.Message.ToString());
            return;
        }
    }

    static void DistributeTasks(List<Team> teams, List<Order> orders, string connectionString)
    {
        int teamIndex = 0;

        foreach (var order in orders)
        {

            teams[teamIndex].Orders.Add(order);

            // Generate the incremented value for [FlpAsnId]
            int flassignorder = NextId("[dbo].[FollowupAssign]", "[FlpAsnId]", connectionString);

            // Get the Max OrderID from the OrdersData table
            int orderId = Convert.ToInt32(GetReqData("[dbo].[OrdersData]", "TOP 1 [OrderID]", "[FK_OrderCustomerID] = " + order.FK_OrderCustomerID + " AND [OrderDate] <= GETDATE() AND [OrderDate] >= DATEADD(MONTH, -6, GETDATE()) AND DAY([OrderDate])=DATEPART(DAY, GETDATE()) ORDER BY [OrderDate] DESC", connectionString).ToString());

            // Create the insert query with parameters
            string insertQuery = "INSERT INTO [dbo].[FollowupAssign]([FlpAsnId], [FlpAsnDate], [FK_OrderId], [Fk_CustomerID], [FK_TeamID], [FlpAsnStatus])"
                                 + "VALUES(@FlpAsnId, @FlpAsnDate, @FK_OrderId, @FK_CustomerID, @FK_TeamID, @FlpAsnStatus)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    // Set parameter values
                    command.Parameters.AddWithValue("@FlpAsnId", flassignorder);
                    command.Parameters.AddWithValue("@FlpAsnDate", DateTime.Now.Date); // Assuming you want to use the current date
                    command.Parameters.AddWithValue("@FK_OrderId", orderId);
                    command.Parameters.AddWithValue("@FK_CustomerID", order.FK_OrderCustomerID); // Replace with the actual property for OrderId
                    command.Parameters.AddWithValue("@FK_TeamID", teams[teamIndex].TeamID); // Replace with the actual property for TeamId
                    command.Parameters.AddWithValue("@FlpAsnStatus", "Pending");

                    // Execute the insert query
                    command.ExecuteNonQuery();
                }
            }

            teamIndex = (teamIndex + 1) % teams.Count;
        }
    }

    private void FillGrid()
    {
        try
        {
            string strQuery = "";

            strQuery = "SELECT [TeamUserID], [TeamPersonName], [TeamMobile] FROM [dbo].[SupportTeam] WHERE [TeamTaskID] IN (1,3) AND [TeamUserStatus] = 0 AND [TeamID] IN (34,30,31,32,36,38,41,42,43,44)";

            using (DataTable dtTeam = c.GetDataTable(strQuery))
            {
                gvTeam.DataSource = dtTeam;
                gvTeam.DataBind();
                if (gvTeam.Rows.Count > 0)
                {
                    gvTeam.UseAccessibleHeader = true;
                    gvTeam.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    private void GetFlupData()
    {
        try
        {
            string strQuery = "";

            strQuery = @"SELECT s.[TeamPersonName], COUNT(f.[Fk_CustomerID]) AS AssignedOrders
                         FROM [dbo].[SupportTeam] AS s 
                         INNER JOIN [dbo].[FollowupAssign] AS f ON s.[TeamID] = f.[FK_TeamID]
                         WHERE CONVERT(VARCHAR(20), f.[FlpAsnDate], 112) = CONVERT(VARCHAR(20), CAST('" + DateTime.Now.Date + "' AS DATETIME), 112) GROUP BY s.[TeamPersonName]";

            using (DataTable dtCall = c.GetDataTable(strQuery))
            {
                gvCall.DataSource = dtCall;
                gvCall.DataBind();
                if (gvCall.Rows.Count > 0)
                {
                    gvCall.UseAccessibleHeader = true;
                    gvCall.HeaderRow.TableSection = TableRowSection.TableHeader;
                }

            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetFlupData", ex.Message.ToString());
            return;
        }
    }


    class Team
    {
        public int TeamID { get; set; }
        public string TeamUserName { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public List<Order> Orders { get; set; }

        public Team()
        {
            Orders = new List<Order>();
        }
    }

    class Order
    {
        public int FK_OrderCustomerID { get; set; }
        public string Description { get; set; }
    }

    class StaffData
    {
        public int StaffID { get; set; }
    }

    public static int NextId(string tableName, string fieldName, string connectionString)
    {
        try
        {
            int retValue = 1;
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr = default(SqlDataReader);
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select MAX(" + fieldName + ") as maxNo From " + tableName;
            dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if ((dr["maxNo"]) != System.DBNull.Value)
                    {
                        retValue = Convert.ToInt32(dr["maxNo"]) + 1;
                    }
                    else
                    {
                        retValue = 1;
                    }
                }
            }
            else
            {
                retValue = 1;
            }
            dr.Close();
            cmd.Dispose();
            con.Close();
            con = null;
            return retValue;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static int GetReqData(string tableName, string fieldName, string whereCon, string connectionString)
    {
        try
        {
            int retValue = 0;
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = default(SqlDataReader);
            cmd.CommandText = whereCon == "" ? "Select " + fieldName + " as colName From " + tableName : "Select " + fieldName + " as colName From " + tableName + " Where " + whereCon;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                if (dr["colName"] == DBNull.Value)
                {
                    retValue = 0;
                }
                else
                {
                    retValue = Convert.ToInt32(dr["colName"]);
                }

            }
            dr.Close();
            cmd.Dispose();
            con.Close();
            con = null;
            return retValue;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}