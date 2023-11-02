using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

/// <summary>
/// Summary description for ecommClass
/// </summary>
public class ecommClass
{
    public ecommClass()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public string OpenConnection()
    {
        return System.Web.Configuration.WebConfigurationManager.ConnectionStrings["GenCartDATAReg"].ConnectionString;
    }

    public int NextId(string tableName, string fieldName)
    {
        try
        {
            int retValue = 1;
            SqlConnection con = new SqlConnection(OpenConnection());
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

    public bool IsRecordExist(string strQuery)
    {
        try
        {

            bool rValue = false;
            SqlConnection con = new SqlConnection(OpenConnection());
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = default(SqlDataReader);

            cmd.CommandText = strQuery;
            dr = cmd.ExecuteReader();

            rValue = dr.HasRows;
            dr.Close();
            cmd.Dispose();
            con.Close();
            con = null;

            return rValue;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public void ExecuteQuery(string strQuery)
    {
        try
        {
            SqlConnection con = new SqlConnection(OpenConnection());
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = strQuery;
            cmd.CommandTimeout = 1200000;
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
            con = null;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    public object GetReqData(string tableName, string fieldName, string whereCon)
    {
        try
        {
            object retValue = null;
            SqlConnection con = new SqlConnection(OpenConnection());
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
                    retValue = null;
                }
                else
                {
                    retValue = dr["colName"];
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

    public DataTable GetDataTable(string strQuery)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(OpenConnection()))
            {
                SqlDataAdapter da = new SqlDataAdapter(strQuery, con);
                da.SelectCommand.CommandTimeout = 1200000;
                using (DataTable dt = new DataTable())
                {
                    da.Fill(dt);
                    con.Close();
                    con.Dispose();
                    return dt;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    public long returnAggregate(string strQuery)
    {
        try
        {
            long rValue = 0;
            SqlConnection con = new SqlConnection(OpenConnection());
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = strQuery;

            object result = cmd.ExecuteScalar();

            if (result.GetType() != typeof(DBNull))
            {
                rValue = Convert.ToInt32(result);
            }
            else
            {
                rValue = 0;

            }

            con.Close();
            con = null;
            cmd.Dispose();
            return rValue;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}