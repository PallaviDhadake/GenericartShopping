using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;


/// <summary>
/// Summary description for supportTeamWebServices
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class supportTeamWebServices : System.Web.Services.WebService
{
    iClass c = new iClass();

    [WebMethod]
    public void GetLabAppointment(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch)
    {
        int displayLength = iDisplayLength;
        int displayStart = iDisplayStart;
        int sortCol = iSortCol_0;
        string sortDir = sSortDir_0;
        string search = sSearch;
        string cs = ConfigurationManager.ConnectionStrings["GenCartDATA"].ConnectionString;
       
        List<LabAppointmentData> listLabAppointmentData = new List<LabAppointmentData>();
       // List<gcTables> listLabAppointmentDatas = new List<gcTables>();


        //List<Model.Customer> listCustomers = new List<Model.Customer>();
        int filteredCount = 0;
        int totalOrders = 0;
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand("spGetLabAppointmentData", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter paramDisplayLength = new SqlParameter()
            {
                ParameterName = "@DisplayLength",
                Value = displayLength
            };
            cmd.Parameters.Add(paramDisplayLength);

            SqlParameter paramDisplayStart = new SqlParameter()
            {
                ParameterName = "@DisplayStart",
                Value = displayStart
            };
            cmd.Parameters.Add(paramDisplayStart);

            SqlParameter paramSortCol = new SqlParameter()
            {
                ParameterName = "@SortCol",
                Value = sortCol
            };
            cmd.Parameters.Add(paramSortCol);

            SqlParameter paramSortDir = new SqlParameter()
            {
                ParameterName = "@SortDir",
                Value = sortDir
            };
            cmd.Parameters.Add(paramSortDir);

            SqlParameter paramSearchString = new SqlParameter()
            {
                ParameterName = "@Search",
                Value = string.IsNullOrEmpty(search) ? null : search
            };
            cmd.Parameters.Add(paramSearchString);

            con.Open();
            cmd.CommandTimeout = 240;
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                LabAppointmentData labAppointmentData = new LabAppointmentData();
               // gcTables labAppointmentData = new gcTables();

                labAppointmentData.LabAppID = Convert.ToInt32(rdr["LabAppID"]);
                labAppointmentData.LabAppName = rdr["LabAppName"].ToString();
                labAppointmentData.LabAppMobile = rdr["LabAppMobile"].ToString();
                labAppointmentData.LabTestName = rdr["LabTestName"].ToString();
                labAppointmentData.LabAppDate = rdr["LabAppDate"].ToString();
                filteredCount = Convert.ToInt32(rdr["TotalCount"]);

                listLabAppointmentData.Add(labAppointmentData);
            }
        }

        var result = new
        {
            iTotalRecords = GetLabAppointmentTotalCount(),
            iTotalDisplayRecords = filteredCount,
            aaData = listLabAppointmentData
        };

        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Write(js.Serialize(result));



    }
    private int GetLabAppointmentTotalCount()
    {
        int totalCustomerCount = 0;
        string cs = ConfigurationManager.ConnectionStrings["GenCartDATA"].ConnectionString;
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new
                SqlCommand("select Count(LabAppID) From LabAppointments", con);
            con.Open();
            totalCustomerCount = (int)cmd.ExecuteScalar();
        }
        return totalCustomerCount;
    }

     [WebMethod]
    public void GetOrders(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, int orderStatus, string fromDate, string toDate)
    {
        int displayLength = iDisplayLength;
        int displayStart = iDisplayStart;
        int sortCol = iSortCol_0;
        string sortDir = sSortDir_0;
        string search = sSearch;
        int ordStatus = orderStatus;
        string cs = ConfigurationManager.ConnectionStrings["GenCartDATA"].ConnectionString;
       
       // List<LabAppointmentData> listLabAppointmentDatas = new List<LabAppointmentData>();
        List<OrdersData> listOrdersData = new List<OrdersData>();


        //List<Model.Customer> listCustomers = new List<Model.Customer>();
        int filteredCount = 0;
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand("spGetOrders", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter paramDisplayLength = new SqlParameter()
            {
                ParameterName = "@DisplayLength",
                Value = displayLength
            };
            cmd.Parameters.Add(paramDisplayLength);

            SqlParameter paramDisplayStart = new SqlParameter()
            {
                ParameterName = "@DisplayStart",
                Value = displayStart
            };
            cmd.Parameters.Add(paramDisplayStart);

            SqlParameter paramSortCol = new SqlParameter()
            {
                ParameterName = "@SortCol",
                Value = sortCol
            };
            cmd.Parameters.Add(paramSortCol);

            SqlParameter paramSortDir = new SqlParameter()
            {
                ParameterName = "@SortDir",
                Value = sortDir
            };
            cmd.Parameters.Add(paramSortDir);

            SqlParameter paramSearchString = new SqlParameter()
            {
                ParameterName = "@Search",
                Value = string.IsNullOrEmpty(search) ? null : search
            };
            cmd.Parameters.Add(paramSearchString);

            SqlParameter paramOrderStatus = new SqlParameter()
            {
                ParameterName = "@OrderStatus",
                Value = ordStatus
            };
            cmd.Parameters.Add(paramOrderStatus);

            if (fromDate != "" && toDate != "")
            {
                SqlParameter paramFromDate = new SqlParameter()
                {
                    ParameterName = "@StartDate",
                    Value = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd")
                };
                cmd.Parameters.Add(paramFromDate);

                SqlParameter paramToDate = new SqlParameter()
                {
                    ParameterName = "@EndDate",
                    Value = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd")
                };
                cmd.Parameters.Add(paramToDate);

            }

            con.Open();
            cmd.CommandTimeout = 30;
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                //LabAppointmentData labAppointmentData = new LabAppointmentData();
                OrdersData order = new OrdersData();

                //order.OrderID = Convert.ToInt32(rdr["OrderID"]);
                order.FK_OrderCustomerID = Convert.ToInt32(rdr["FK_OrderCustomerID"]);
                order.CustomerName = rdr["CustomerName"].ToString();
                order.CustomerMobile = rdr["CustomerMobile"].ToString();
                order.CustomerEmail = rdr["CustomerEmail"].ToString();
                order.totalOrdersCount = rdr["totalOrdersCount"].ToString();
                order.recentOrderId = rdr["recentOrderId"].ToString();
                filteredCount = Convert.ToInt32(rdr["TotalCount"]);

                listOrdersData.Add(order);
            }
        }

        var result = new
        {
            iTotalRecords = GetOrdersCountTotalCount(ordStatus),
            iTotalDisplayRecords = filteredCount,
            aaData = listOrdersData
        };

        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Write(js.Serialize(result));



    }
    private int GetOrdersCountTotalCount(int ordStatus)
    {
        int totalCustomerCount = 0;
        string cs = ConfigurationManager.ConnectionStrings["GenCartDATA"].ConnectionString;
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new
                SqlCommand("Select Count(a.OrderID)From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID = b.CustomrtID Where a.OrderStatus ="+ ordStatus + "; ", con);
            con.Open();
            totalCustomerCount = (int)cmd.ExecuteScalar();
        }
        return totalCustomerCount;
    }


    [WebMethod]
    public void GetRegisteredCustomer(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, int teamId, int feedbackStatus)
    {
        int displayLength = iDisplayLength;
        int displayStart = iDisplayStart;
        int sortCol = iSortCol_0;
        string sortDir = sSortDir_0;
        string search = sSearch;
        int Id = teamId;
        int status = feedbackStatus;

        string cs = ConfigurationManager.ConnectionStrings["GenCartDATA"].ConnectionString;

        // List<LabAppointmentData> listLabAppointmentDatas = new List<LabAppointmentData>();
        List<RegisteredCustomer> listRegisteredCustomers = new List<RegisteredCustomer>();


        //List<Model.Customer> listCustomers = new List<Model.Customer>();
        int filteredCount = 0;
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand("spGetRegisteredCustomers", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter paramDisplayLength = new SqlParameter()
            {
                ParameterName = "@DisplayLength",
                Value = displayLength
            };
            cmd.Parameters.Add(paramDisplayLength);

            SqlParameter paramDisplayStart = new SqlParameter()
            {
                ParameterName = "@DisplayStart",
                Value = displayStart
            };
            cmd.Parameters.Add(paramDisplayStart);

            SqlParameter paramSortCol = new SqlParameter()
            {
                ParameterName = "@SortCol",
                Value = sortCol
            };
            cmd.Parameters.Add(paramSortCol);

            SqlParameter paramSortDir = new SqlParameter()
            {
                ParameterName = "@SortDir",
                Value = sortDir
            };
            cmd.Parameters.Add(paramSortDir);

            SqlParameter paramSearchString = new SqlParameter()
            {
                ParameterName = "@Search",
                Value = string.IsNullOrEmpty(search) ? null : search
            };
            cmd.Parameters.Add(paramSearchString);
            SqlParameter paramTeamId = new SqlParameter()
            {
                ParameterName = "@TeamId",
                Value = Id
            };
            cmd.Parameters.Add(paramTeamId);

            SqlParameter paramFeedbackStatus = new SqlParameter()
            {
                ParameterName = "@FeedbackFlag",
                Value = status
            };
            cmd.Parameters.Add(paramFeedbackStatus);

            con.Open();
            cmd.CommandTimeout = 30;
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                //LabAppointmentData labAppointmentData = new LabAppointmentData();
                RegisteredCustomer customer = new RegisteredCustomer();

                customer.CustomrtID = Convert.ToInt32(rdr["CustomrtID"]);
                customer.FeedBackFlag = Convert.ToInt32(rdr["FeedBackFlag"]);

                customer.CustomerName = rdr["CustomerName"].ToString();
                customer.CustomerMobile = rdr["CustomerMobile"].ToString();
                customer.CustomerEmail = rdr["CustomerEmail"].ToString();
                customer.CustomerJoinDate = rdr["CustomerJoinDate"].ToString();
                customer.CustAddress = rdr["CustAddress"].ToString();
                customer.CustCity = rdr["CustCity"].ToString();

                filteredCount = Convert.ToInt32(rdr["TotalCount"]);

                listRegisteredCustomers.Add(customer);
            }
        }

        var result = new
        {
            iTotalRecords = GetRegisteredCustomerCount(),
            iTotalDisplayRecords = filteredCount,
            aaData = listRegisteredCustomers
        };

        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Write(js.Serialize(result));

    }

    private int GetRegisteredCustomerCount()
    {
        int totalCustomerCount = 0;
        string cs = ConfigurationManager.ConnectionStrings["GenCartDATA"].ConnectionString;
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new
                SqlCommand("Select Distinct Count(a.CustomrtID) From CustomersData a Left Join OrdersData b On a.CustomrtID = b.FK_OrderCustomerID Where a.CustomrtID Not In(Select Distinct FK_OrderCustomerID From OrdersData)", con);
            con.Open();
            totalCustomerCount = (int)cmd.ExecuteScalar();
        }
        return totalCustomerCount;
    }


    [WebMethod]
    public void GetPrescriptionRequestData(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch)
    {
        int displayLength = iDisplayLength;
        int displayStart = iDisplayStart;
        int sortCol = iSortCol_0;
        string sortDir = sSortDir_0;
        string search = sSearch;
        //string cs = ConfigurationManager.ConnectionStrings["GenCartDATA"].ConnectionString;
        string cs = c.OpenConnection();

        List<PrescriptionRequestData> listPrescriptionRequestData = new List<PrescriptionRequestData>();
        // List<gcTables> listLabAppointmentDatas = new List<gcTables>();



        int filteredCount = 0;

        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand("spGetPrescriptionRequestData", con);
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
                PrescriptionRequestData prescriptionRequest = new PrescriptionRequestData();
                // gcTables labAppointmentData = new gcTables();

                prescriptionRequest.PreReqID = Convert.ToInt32(rdr["PreReqID"]);
                prescriptionRequest.PreReqDate = rdr["PreReqDate"].ToString();
                prescriptionRequest.PreReqName = rdr["PreReqName"].ToString();
                prescriptionRequest.PreReqMobile = rdr["PreReqMobile"].ToString();
                prescriptionRequest.PreReqDisease = rdr["PreReqDisease"].ToString();
                filteredCount = Convert.ToInt32(rdr["TotalCount"]);

                listPrescriptionRequestData.Add(prescriptionRequest);
            }
        }

        var result = new
        {
            //iTotalRecords = GetLabAppointmentTotalCount(),
            iTotalDisplayRecords = filteredCount,
            aaData = listPrescriptionRequestData
        };

        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Write(js.Serialize(result));
    }

    [WebMethod]
    public void GetDoctorsAppointmentData(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch)
    {
        int displayLength = iDisplayLength;
        int displayStart = iDisplayStart;
        int sortCol = iSortCol_0;
        string sortDir = sSortDir_0;
        string search = sSearch;
        //string cs = ConfigurationManager.ConnectionStrings["GenCartDATA"].ConnectionString;
        string cs = c.OpenConnection();


        List<DoctorsAppointmentData> listDoctorsAppointmentData = new List<DoctorsAppointmentData>();
        int filteredCount = 0;

        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand("spDoctorsAppointmentData", con);
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
                DoctorsAppointmentData appointment = new DoctorsAppointmentData();
                // gcTables labAppointmentData = new gcTables();

                appointment.DocAppID = Convert.ToInt32(rdr["DocAppID"]);
                appointment.AppSubmitDate = rdr["AppSubmitDate"].ToString();
                appointment.DocAppDate = rdr["DocAppDate"].ToString();
                appointment.DocAppName = rdr["DocAppName"].ToString();
                appointment.DocAppMobile = rdr["DocAppMobile"].ToString();
                appointment.PrevDocName = rdr["PrevDocName"].ToString();
                filteredCount = Convert.ToInt32(rdr["TotalCount"]);

                listDoctorsAppointmentData.Add(appointment);
            }
        }

        var result = new
        {
            //iTotalRecords = GetLabAppointmentTotalCount(),
            iTotalDisplayRecords = filteredCount,
            aaData = listDoctorsAppointmentData
        };

        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Write(js.Serialize(result));
    }


    [WebMethod]
    public void GetShopwiseCustomerData(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch, int orderStatus, int frId, int teamId, string fromDate, string toDate)
    {
        int displayLength = iDisplayLength;
        int displayStart = iDisplayStart;
        int sortCol = iSortCol_0;
        string sortDir = sSortDir_0;
        string search = sSearch;
        int ordStatus = orderStatus;
        int frID = frId;
        int teamID = teamId;
        //string cs = ConfigurationManager.ConnectionStrings["GenCartDATA"].ConnectionString;
        string cs = c.OpenConnection();


        List<OrdersData> listShopwiseCustomersData = new List<OrdersData>();
        int filteredCount = 0;

        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand("getShopwiseCustomers", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@DisplayLength", displayLength);
            cmd.Parameters.AddWithValue("@DisplayStart", displayStart);
            cmd.Parameters.AddWithValue("@SortCol", sortCol);
            cmd.Parameters.AddWithValue("@SortDir", sortDir);
            string searchval = string.IsNullOrEmpty(search) ? null : search;
            cmd.Parameters.AddWithValue("@Search", searchval);
            cmd.Parameters.AddWithValue("@OrderStatus", ordStatus);
            cmd.Parameters.AddWithValue("@FranchId", frID);
            cmd.Parameters.AddWithValue("@TeamId", teamID);

            if (fromDate != "" && toDate != "")
            {
                SqlParameter paramFromDate = new SqlParameter()
                {
                    ParameterName = "@StartDate",
                    Value = Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd")
                };
                cmd.Parameters.Add(paramFromDate);

                SqlParameter paramToDate = new SqlParameter()
                {
                    ParameterName = "@EndDate",
                    Value = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd")
                };
                cmd.Parameters.Add(paramToDate);
            }

            con.Open();
            cmd.CommandTimeout = 30;
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                OrdersData customer = new OrdersData();
                // gcTables labAppointmentData = new gcTables();
                customer.FK_OrderCustomerID = Convert.ToInt32(rdr["FK_OrderCustomerID"]);
                customer.CustomerName = rdr["CustomerName"].ToString();
                customer.CustomerMobile = rdr["CustomerMobile"].ToString();
                customer.CustomerEmail = rdr["CustomerEmail"].ToString();
                customer.totalOrdersCount = rdr["totalOrders"].ToString();
                customer.recentOrderId = rdr["recentOrderId"].ToString();
                customer.FeedBackFlag = Convert.ToInt32(rdr["FeedBackFlag"]);
                filteredCount = Convert.ToInt32(rdr["TotalCount"]);

                listShopwiseCustomersData.Add(customer);
            }
        }

        var result = new
        {
            //iTotalRecords = GetLabAppointmentTotalCount(),
            iTotalDisplayRecords = filteredCount,
            aaData = listShopwiseCustomersData
        };

        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Write(js.Serialize(result));
    }


}
