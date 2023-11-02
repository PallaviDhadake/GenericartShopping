using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Net;

/// <summary>
/// Summary description for GenericMitraInfo
/// </summary>
public class GenericMitraInfo
{
    iClass c = new iClass();

    public DateTime GMRegDate { get; set; }
    public string GMName { get; set; }
    public string GMMobile { get; set; }
    public string GMEmail { get; set; }
    public string GMPhoto { get; set; }
    public string GMBank { get; set; }
    public string GMBankAccName { get; set; }
    public string GMBankAccNum { get; set; }
    public string GMBankIFSC { get; set; }
    public string GMPanCard { get; set; }
    public string GMLogin { get; set; }
    public string GMPassword { get; set; }
    public int GMStateId { get; set; }
    public string GMStateName { get; set; }
    public int GMDistrictId { get; set; }
    public string GMDistrictName { get; set; }
    public int GMCityId { get; set; }
    public string GMCitytName { get; set; }
    public int GMStatus { get; set; }
    public string GMPancardDoc { get; set; }
    public string GMAadharDoc { get; set; }
    public string GMBankDoc { get; set; }
    public string GMShopCode { get; set; }


    public GenericMitraInfo()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public void GenericMitraData(int GenMitraIDx)
    {

        string connectionString = c.OpenConnection();
        string queryString = "SELECT GM.*, ST.StateName, DT.DistrictName, CT.CityName FROM GenericMitra GM Inner Join StatesData ST ON GM.FK_StateID=ST.StateID Inner Join DistrictsData DT On GM.FK_DistrictID=DT.DistrictId Inner Join CityData CT On GM.FK_CityID=CT.CityID WHERE GM.GMitraID = " + GenMitraIDx;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {

            SqlCommand command = new SqlCommand(queryString, connection);
            connection.Open();

            SqlDataReader reader = command.ExecuteReader();
            object result = null;

            while (reader.Read())
            {

                //GMRegDate = reader["GMitraDate"] != DBNull.Value ? reader["GMitraDate"].ToString() : "";
                GMName = reader["GMitraName"] != DBNull.Value ? reader["GMitraName"].ToString() : "";
                GMMobile = reader["GMitraMobile"] != DBNull.Value ? reader["GMitraMobile"].ToString() : "";
                GMEmail = reader["GMitraEmail"] != DBNull.Value ? reader["GMitraEmail"].ToString() : "";
                GMPhoto = reader["GMitraPhoto"] != DBNull.Value ? reader["GMitraPhoto"].ToString() : "";
                GMBank = reader["GMitraBankName"] != DBNull.Value ? reader["GMitraBankName"].ToString() : "";
                GMBankAccName = reader["GMitraBankAccName"] != DBNull.Value ? reader["GMitraBankAccName"].ToString() : "";
                GMBankAccNum = reader["GMitraBankAccNumber"] != DBNull.Value ? reader["GMitraBankAccNumber"].ToString() : "";
                GMBankIFSC = reader["GMitraBankIFSC"] != DBNull.Value ? reader["GMitraBankIFSC"].ToString() : "";
                GMPanCard = reader["GMitraPanCard"] != DBNull.Value ? reader["GMitraPanCard"].ToString() : "";
                GMLogin = reader["GMitraLogin"] != DBNull.Value ? reader["GMitraLogin"].ToString() : "";
                GMPassword = reader["GMitraPassword"] != DBNull.Value ? reader["GMitraPassword"].ToString() : "";
                GMStateId = reader["FK_StateID"] != DBNull.Value ? Convert.ToInt32(reader["FK_StateID"]) : 0;
                GMStateName = reader["StateName"] != DBNull.Value ? reader["StateName"].ToString() : "";
                GMDistrictId = reader["FK_DistrictID"] != DBNull.Value ? Convert.ToInt32(reader["FK_DistrictID"]) : 0;
                GMDistrictName = reader["DistrictName"] != DBNull.Value ? reader["DistrictName"].ToString() : "";
                GMCityId = reader["FK_CityID"] != DBNull.Value ? Convert.ToInt32(reader["FK_CityID"]) : 0;
                GMCitytName = reader["CityName"] != DBNull.Value ? reader["CityName"].ToString() : "";
                GMStatus = reader["GMitraStatus"] != DBNull.Value ? Convert.ToInt32(reader["GMitraStatus"]) : 0;
                GMPancardDoc = reader["GMitraPan"] != DBNull.Value ? reader["GMitraPan"].ToString() : "";
                GMAadharDoc = reader["GMitraAdhar"] != DBNull.Value ? reader["GMitraAdhar"].ToString() : "";
                GMBankDoc = reader["GMitraBankDoc"] != DBNull.Value ? reader["GMitraBankDoc"].ToString() : "";
                GMShopCode = reader["GMitraShopCode"] != DBNull.Value ? reader["GMitraShopCode"].ToString() : "";
            }
            reader.Close();

        }
    }
}