using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

public partial class WebServices : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

   
    [WebMethod]
    public static string[] GetSearchControl(string prefix)
    {
        string City = "";
        if (HttpContext.Current.Request.Cookies["City"] != null)
        {
            if (Convert.ToString(HttpContext.Current.Request.Cookies["City"]) != "")
            {
                City = " and City.City = '" + Convert.ToString(HttpContext.Current.Request.Cookies["City"].Value) + "'";
            }
        }

        List<string> customers = new List<string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = MasterClass.Getconnection();
            using (SqlCommand cmd = new SqlCommand())
            {
                //cmd.CommandText = "SELECT ProductID, ProductName as Search FROM ProductsData WHERE delMark=0 AND ProductName like @SearchText + '%' and isnull(ProductActive, 0) = 1";
                cmd.CommandText = "SELECT ProductID, ProductName as Search FROM ProductsData WHERE delMark=0 AND ProductName like '%" + prefix + "%' and ProductActive = 1";
                cmd.Parameters.AddWithValue("@SearchText", prefix);
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        customers.Add(string.Format("{0}#{1}", sdr["Search"], sdr["Search"]));
                    }
                }
                conn.Close();
            }
        }
        return customers.ToArray();
    }

    [WebMethod]
    public static string[] GetCities(string prefix)
    {
        List<string> city = new List<string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = MasterClass.Getconnection();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "SELECT TOP 15 CityID, CityName as Search FROM CityData WHERE CityName like @SearchText + '%'";
                cmd.Parameters.AddWithValue("@SearchText", prefix);
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        city.Add(string.Format("{0}-{1}", sdr["Search"], sdr["Search"]));
                    }
                }
                conn.Close();
            }
        }
        return city.ToArray();
    }

}