using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
/// <summary>
/// Summary description for CustomersData
/// </summary>
public class CustomersData
{

    iClass c = new iClass();

    public DateTime CustomerJoinDate { get; set; }
    public string CustomerName { get; set; }
    public string CustomerMobile { get; set; }
    public string CustomerEmail { get; set; }
    public DateTime CustomerDOB { get; set; }
    public string CustomerCity { get; set; }
    public string CustomerState { get; set; }
    public string CustomerPincode { get; set; }
    public string CustomerActive { get; set; }
    public string DeviceType { get; set; }
    public int FK_ObpID { get; set; }
    public DateTime RBNO_NextFollowup { get; set; }
	public int CustomerFavShop { get; set; }


	public void CustomresInfo(int CustomerIdx)
    {

        string connectionString = c.OpenConnection();
        string queryString = "SELECT * FROM CustomersData WHERE CustomrtID = " + CustomerIdx;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {

            SqlCommand command = new SqlCommand(queryString, connection);
            connection.Open();

            SqlDataReader reader = command.ExecuteReader();
            object result = null;

			while (reader.Read())
			{
				if (reader["CustomerJoinDate"] == null || reader["CustomerJoinDate"] == DBNull.Value)
				{
					CustomerJoinDate = Convert.ToDateTime(null);
				}
				else
				{
					CustomerJoinDate = Convert.ToDateTime(reader["CustomerJoinDate"]);
				}
				CustomerName = reader["CustomerName"] != DBNull.Value ? reader["CustomerName"].ToString() : "";

				CustomerMobile = reader["CustomerMobile"] != DBNull.Value ? reader["CustomerMobile"].ToString() : "";

				if (reader["CustomerDOB"] == null || reader["CustomerDOB"] == DBNull.Value)
				{
					CustomerDOB = Convert.ToDateTime(null);
				}
				else
				{
					CustomerDOB = Convert.ToDateTime(reader["CustomerDOB"]);
				}

				
				CustomerEmail = reader["CustomerEmail"] != DBNull.Value ? reader["CustomerEmail"].ToString() : "";

				CustomerCity = reader["CustomerCity"] != DBNull.Value ? reader["CustomerCity"].ToString() : "";

				CustomerState = reader["CustomerState"] != DBNull.Value ? reader["CustomerState"].ToString() : "";
				CustomerPincode = reader["CustomerPincode"] != DBNull.Value ? reader["CustomerPincode"].ToString() : "";
				CustomerActive = reader["CustomerActive"] != DBNull.Value ? reader["CustomerActive"].ToString() : "";
				DeviceType = reader["DeviceType"] != DBNull.Value ? reader["DeviceType"].ToString() : "";
				FK_ObpID = (reader["FK_ObpID"] != DBNull.Value) ? Convert.ToInt32(reader["FK_ObpID"]) : 0;

				if (reader["RBNO_NextFollowup"] == null || reader["RBNO_NextFollowup"] == DBNull.Value)
				{
					RBNO_NextFollowup = Convert.ToDateTime(null);
				}
				else
				{
					RBNO_NextFollowup = Convert.ToDateTime(reader["RBNO_NextFollowup"]);
				}
				CustomerFavShop = (reader["CustomerFavShop"] != DBNull.Value) ? Convert.ToInt32(reader["CustomerFavShop"]) : 0;
			}

		}

    }


    //================== OBP DATA =================================//

    public string OBP_ApplicantName { get; set; }
    public string OBP_MobileNo { get; set; }
    public string OBP_UserID { get; set; }


	public void OBPDataInfo(int ObpIdx)
    {
		string connectionString = c.OpenConnection();
		string queryString = "SELECT * FROM OBPData WHERE OBP_ID = " + ObpIdx;

		using (SqlConnection connection = new SqlConnection(connectionString))
		{

			SqlCommand command = new SqlCommand(queryString, connection);
			connection.Open();

			SqlDataReader reader = command.ExecuteReader();
			object result = null;

			while (reader.Read())
			{

				OBP_ApplicantName = reader["OBP_ApplicantName"] != DBNull.Value ? reader["OBP_ApplicantName"].ToString() : "";
				OBP_MobileNo = reader["OBP_MobileNo"] != DBNull.Value ? reader["OBP_MobileNo"].ToString() : "";
				CustomerMobile = reader["OBP_UserID"] != DBNull.Value ? reader["OBP_UserID"].ToString() : "";

			}

		}


	}


    //=============================== Orders Data ========================================//

    public string Total_Orders { get; set; }

    public string Delivered { get; set; }

    public string InProcess { get; set; }

    public string Cancelled { get; set; }



	public void CustOrdersSatus(int CustIdx)
	{
		string connectionString = c.OpenConnection();
		//string queryString = "SELECT * FROM OBPData WHERE OBP_ID = " + ObpIdx;

		using (SqlConnection connection = new SqlConnection(connectionString))
		{

			//SqlCommand command = new SqlCommand(queryString, connection);
			connection.Open();

			//SqlDataReader reader = command.ExecuteReader();
			//object result = null;

			//while (reader.Read())
			//{

			Total_Orders = c.returnAggregate("Select Count(OrderID) From OrdersData Where FK_OrderCustomerID="+ CustIdx).ToString();

			Delivered = c.returnAggregate("Select Count(OrderID) From OrdersData Where OrderStatus=7 AND FK_OrderCustomerID=" + CustIdx).ToString();
			InProcess = c.returnAggregate("Select Count(OrderID) From OrdersData Where OrderStatus=5 AND FK_OrderCustomerID=" + CustIdx).ToString();
			Cancelled = c.returnAggregate("Select Count(OrderID) From OrdersData Where OrderStatus=2 AND FK_OrderCustomerID=" + CustIdx).ToString();


			//}

		}


	}

    //==============================Customers Favshop Data From Franchaisee Data =================================
    public string FranchName { get; set; }
    public string FranchShopCode { get; set; }
    public int FK_FranchCityId { get; set; }
    public string FranchOwnerName { get; set; }


	public void FranchaiseeData(int FranchIdx)
	{
		string connectionString = c.OpenConnection();
		string queryString = "SELECT * FROM FranchiseeData WHERE FranchID = " + FranchIdx;

		using (SqlConnection connection = new SqlConnection(connectionString))
		{

			SqlCommand command = new SqlCommand(queryString, connection);
			connection.Open();

			SqlDataReader reader = command.ExecuteReader();
			object result = null;

			while (reader.Read())
			{

				FranchName = reader["FranchName"] != DBNull.Value ? reader["FranchName"].ToString() : "";
				FranchShopCode = reader["FranchShopCode"] != DBNull.Value ? reader["FranchShopCode"].ToString() : "";
				FK_FranchCityId = (reader["FK_FranchCityId"] != DBNull.Value) ? Convert.ToInt32(reader["FK_FranchCityId"]) : 0;
				FranchOwnerName = reader["FranchOwnerName"] != DBNull.Value ? reader["FranchOwnerName"].ToString() : "";

			}

		}


	}





	public CustomersData()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}