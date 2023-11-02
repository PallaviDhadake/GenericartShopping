using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

/// <summary>
/// Summary description for adminShoppingWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class adminShoppingWebService : System.Web.Services.WebService
{

    iClass c = new iClass();

    [WebMethod]
    public void GetCustomerData(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch)
    {
        int displayLength = iDisplayLength;
        int displayStart = iDisplayStart;
        int sortCol = iSortCol_0;
        string sortDir = sSortDir_0;
        string search = sSearch;
        //string cs = ConfigurationManager.ConnectionStrings["GenCartDATA"].ConnectionString;
        string cs = c.OpenConnection();


        List<CustomersDataAdmin> listCustomersData = new List<CustomersDataAdmin>();
        int filteredCount = 0;

        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand("getCustomersData", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@DisplayLength", displayLength);
            cmd.Parameters.AddWithValue("@DisplayStart", displayStart);
            cmd.Parameters.AddWithValue("@SortCol", sortCol);
            cmd.Parameters.AddWithValue("@SortDir", sortDir);
            string searchval = string.IsNullOrEmpty(search) ? null : search;
            cmd.Parameters.AddWithValue("@Search", searchval);

            con.Open();
            cmd.CommandTimeout = 30;
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                CustomersDataAdmin customer = new CustomersDataAdmin();
                // gcTables labAppointmentData = new gcTables();

                customer.CustomrtID = Convert.ToInt32(rdr["CustomrtID"]);
                customer.JoinDate = rdr["JoinDate"].ToString();
                customer.CustomerName = rdr["CustomerName"].ToString();
                customer.CustomerMobile = rdr["CustomerMobile"].ToString();
                customer.CustomerEmail = rdr["CustomerEmail"].ToString();
                customer.DeviceType = rdr["DeviceType"].ToString();
                customer.CustomerPassword = rdr["CustomerPassword"].ToString();
                filteredCount = Convert.ToInt32(rdr["TotalCount"]);

                listCustomersData.Add(customer);
            }
        }

        var result = new
        {
            //iTotalRecords = GetLabAppointmentTotalCount(),
            iTotalDisplayRecords = filteredCount,
            aaData = listCustomersData
        };

        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Write(js.Serialize(result));
    }

    [WebMethod]
    public void GetGeneriMitraCustomerData(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, int GmStatus)
    {
        int displayLength = iDisplayLength;
        int displayStart = iDisplayStart;
        int sortCol = iSortCol_0;
        string sortDir = sSortDir_0;
        string search = sSearch;
        int gmStatus = GmStatus;
        //string cs = ConfigurationManager.ConnectionStrings["GenCartDATA"].ConnectionString;
        string cs = c.OpenConnection();


        List<GeneriMitraCustomer> listCustomersData = new List<GeneriMitraCustomer>();
        int filteredCount = 0;

        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand("GetGeneriMitraCustomers", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@DisplayLength", displayLength);
            cmd.Parameters.AddWithValue("@DisplayStart", displayStart);
            cmd.Parameters.AddWithValue("@SortCol", sortCol);
            cmd.Parameters.AddWithValue("@SortDir", sortDir);
            string searchval = string.IsNullOrEmpty(search) ? null : search;
            cmd.Parameters.AddWithValue("@Search", searchval);
            cmd.Parameters.AddWithValue("@GmStatus", gmStatus);

            con.Open();
            cmd.CommandTimeout = 30;
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                GeneriMitraCustomer customer = new GeneriMitraCustomer();
                // gcTables labAppointmentData = new gcTables();

                customer.GMitraID = Convert.ToInt32(rdr["GMitraID"]);
                customer.GMitraStatus = Convert.ToInt32(rdr["GMitraStatus"]);
                customer.GMitraDate = rdr["GMitraDate"].ToString();
                customer.GMitraName = rdr["GMitraName"].ToString();
                customer.GMitraEmail = rdr["GMitraEmail"].ToString();
                customer.GMitraMobile = rdr["GMitraMobile"].ToString();
                customer.StateName = rdr["StateName"].ToString();
                customer.DistrictName = rdr["DistrictName"].ToString();
                customer.CityName = rdr["CityName"].ToString();
                filteredCount = Convert.ToInt32(rdr["TotalCount"]);

                listCustomersData.Add(customer);
            }
        }

        var result = new
        {
            //iTotalRecords = GetLabAppointmentTotalCount(),
            iTotalDisplayRecords = filteredCount,
            aaData = listCustomersData
        };

        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Write(js.Serialize(result));
    }

    [WebMethod]
    public void GetAllOrdersData(int orderStatus)
    {
        //OrdStatus 0->All Orders, 1->new orders without fav shop, 2->cancelled by cust, 3->accepted by admin, 4->denied by admin,
        //5->inprocess, 6->shipped, 7->delivered, 8->rejected by gmmh0001, 9->ord amount low, 10->returned by cust
        //111->new orders with fav shop, 222->monthly, 333->denied by shops

        
        //string cs = ConfigurationManager.ConnectionStrings["GenCartDATA"].ConnectionString;
        string cs = c.OpenConnection();

        List<OrdersDataAdmin> listOrdersDataAdmin = new List<OrdersDataAdmin>();
        int filteredCount = 0;

        // Prepare From & To Date range as parameter (28-Apr-2023)
        string dateRange = c.GetFinancialYear();
        string[] arrDateRange = dateRange.ToString().Split('#');
        DateTime myFromDate = Convert.ToDateTime(arrDateRange[0]);
        DateTime myToDate = Convert.ToDateTime(arrDateRange[1]);

        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand("GetOrdersData", con);
            cmd.CommandType = CommandType.StoredProcedure;

            //cmd.Parameters.AddWithValue("@DisplayLength", displayLength);
            //cmd.Parameters.AddWithValue("@DisplayStart", displayStart);
            //cmd.Parameters.AddWithValue("@SortCol", sortCol);
            //cmd.Parameters.AddWithValue("@SortDir", sortDir);
            //string searchval = string.IsNullOrEmpty(search) ? null : search;
            //cmd.Parameters.AddWithValue("@Search", searchval);
            cmd.Parameters.AddWithValue("@OrdStatus", orderStatus);
            cmd.Parameters.AddWithValue("@FromDate", myFromDate);
            cmd.Parameters.AddWithValue("@ToDate", myToDate);

            con.Open();
            cmd.CommandTimeout = 30;
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                OrdersDataAdmin order = new OrdersDataAdmin();

                order.OrderID = Convert.ToInt32(rdr["OrderID"]);
                order.OrderStatus = Convert.ToInt32(rdr["OrderStatus"]);
                order.FK_OrderCustomerID = Convert.ToInt32(rdr["FK_OrderCustomerID"]);
                order.orStatus = rdr["orStatus"].ToString();
                order.ordDate = rdr["ordDate"].ToString();
                order.CustomerName = rdr["CustomerName"].ToString();
                order.CustomerMobile = rdr["CustomerMobile"].ToString();
                order.OrdAmount = rdr["OrderAmount"].ToString();
                order.ProductCount = Convert.ToInt32(rdr["ProductCount"].ToString());
                order.CartProducts = rdr["CartProducts"].ToString();
                order.DeviceType = rdr["DeviceType"].ToString();
                filteredCount = Convert.ToInt32(rdr["TotalCount"]);

                listOrdersDataAdmin.Add(order);
            }
        }

        //var result = new
        //{
        //    iTotalDisplayRecords = filteredCount,
        //    aaData = listOrdersDataAdmin
        //};

        JavaScriptSerializer js = new JavaScriptSerializer();
        js.MaxJsonLength = Int32.MaxValue;
        Context.Response.Write(js.Serialize(listOrdersDataAdmin));
        
    }

    [WebMethod]
    public void GetEmployees()
    {
        string cs = c.OpenConnection();
        List<ManagementTeam> mgTeam = new List<ManagementTeam>();

        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand("spGetManagementTeam", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();

            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                ManagementTeam managementTeam = new ManagementTeam();
                managementTeam.EmpId = Convert.ToInt32(rdr["EmpId"]);
                managementTeam.EmpName = rdr["EmpName"].ToString();
                managementTeam.EmpCityName = rdr["EmpCityName"].ToString();
                managementTeam.EmpDesignation = rdr["EmpDesignation"].ToString();
                managementTeam.EmpMobileNo = rdr["EmpMobileNo"].ToString();
                managementTeam.EmpEmail = rdr["EmpEmail"].ToString();

                mgTeam.Add(managementTeam);
            }
        }

        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Write(js.Serialize(mgTeam));
    }


    [WebMethod]
    public void GetOnlinePayReport(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch)
    {
        //int temp = orderStatus;
        int displayLength = iDisplayLength;
        int displayStart = iDisplayStart;
        int sortCol = iSortCol_0;
        string sortDir = sSortDir_0;
        string search = sSearch;

        string cs = c.OpenConnection();

        List<OLPReport> listOLPData = new List<OLPReport>();
        int filteredCount = 0;

        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand("GetOLPData", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@DisplayLength", displayLength);
            cmd.Parameters.AddWithValue("@DisplayStart", displayStart);
            cmd.Parameters.AddWithValue("@SortCol", sortCol);
            cmd.Parameters.AddWithValue("@SortDir", sortDir);
            string searchval = string.IsNullOrEmpty(search) ? null : search;
            cmd.Parameters.AddWithValue("@Search", searchval);

            con.Open();
            cmd.CommandTimeout = 30;
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                OLPReport olp = new OLPReport();
                // gcTables labAppointmentData = new gcTables();

                olp.OrderID = Convert.ToInt32(rdr["OrderID"]);
                olp.OrderStatus = Convert.ToInt32(rdr["OrderStatus"]);
                olp.ordDate = rdr["ordDate"].ToString();
                olp.custInfo = rdr["custInfo"].ToString();
                olp.ordAmount = rdr["ordAmount"].ToString();
                olp.ordPaidAmount = rdr["ordPaidAmount"].ToString();
                olp.FranchName = rdr["FranchName"].ToString();
                olp.FranchShopcode = rdr["FranchShopcode"].ToString();
                olp.Shopstatus = rdr["Shopstatus"].ToString();
                olp.OrderPaymentTxnId = rdr["OrderPaymentTxnId"].ToString();
                olp.OPL_transtatus = rdr["OPL_transtatus"].ToString();
                olp.OLP_device_type = rdr["OLP_device_type"].ToString();

                filteredCount = Convert.ToInt32(rdr["TotalCount"]);

                listOLPData.Add(olp);
            }
        }

        var result = new
        {
            //iTotalRecords = GetLabAppointmentTotalCount(),
            iTotalDisplayRecords = filteredCount,
            aaData = listOLPData
        };

        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Write(js.Serialize(result));
    }
}
