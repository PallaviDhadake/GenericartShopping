using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public class MasterClass
{
    public static string CustomerSessionId;

    public static string GetProjectName()
    {
        return MasterClass.StringValue("SELECT ProjectName FROM ProjectDetail order by Auto");
    }

    public static string GetMobileNo()
    {
        return MasterClass.StringValue("SELECT MobileNo FROM ProjectDetail order by Auto");
    }

    public static string GetEmail()
    {
        return MasterClass.StringValue("SELECT Email FROM ProjectDetail order by Auto");
    }

    public static string GetAddress()
    {
        return MasterClass.StringValue("SELECT Address FROM ProjectDetail order by Auto");
    }

    public static string GetWebsiteAddress()
    {
        return MasterClass.StringValue("SELECT WebsiteAddress FROM ProjectDetail order by Auto");
    }
    public static string ConvertDate(String Date)
    {
        if (Date != "")
        {
            Date = Date.Replace("/", "-");
            String[] NewDate = Date.Split('-');
            return (NewDate[1] + "-" + NewDate[0] + "-" + NewDate[2]);
        }
        else
        {
            return Date;
        }
    }

    public static int FillDDL(DropDownList DDl, string Sql, string DataTextField, string DataValueField)
    {
        int n = 0;
        SqlConnection sqlconn;
        sqlconn = new SqlConnection();
        sqlconn.ConnectionString = MasterClass.Getconnection();
        SqlCommand sqlcmd = new SqlCommand(Sql, sqlconn);
        SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DDl.DataSource = ds;
        DDl.DataTextField = DataTextField;
        DDl.DataValueField = DataValueField;
        DDl.DataBind();
        return (n);
    }

    public static int FillCheckBoxList(CheckBoxList Chk, string Sql, string DataTextField, string DataValueField)
    {
        int n = 0;
        SqlConnection sqlconn;
        sqlconn = new SqlConnection();
        sqlconn.ConnectionString = MasterClass.Getconnection();
        SqlCommand sqlcmd = new SqlCommand(Sql, sqlconn);
        SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        Chk.DataSource = ds;
        Chk.DataTextField = DataTextField;
        Chk.DataValueField = DataValueField;
        Chk.DataBind();
        return (n);
    }

    public static DataTable Query(string sqlstr)
    {
        string ConnectionString = Getconnection();
        SqlConnection cn = new SqlConnection(ConnectionString);
        SqlDataAdapter adp = new SqlDataAdapter(sqlstr, cn);
        adp.SelectCommand.CommandTimeout = 1200000;
        DataSet ds = new DataSet();
        adp.Fill(ds);
        DataTable dt = ds.Tables[0];
        return (dt);
    }

    public static string Getconnection()
    {
        //string ConnectionString = "Initial Catalog=Db_GenericartShopping;Data Source=103.120.177.78;User ID=Db_GenericartShopping;Pwd=T9yf6z0~";
        //string ConnectionString = "Initial Catalog=Db_GenericartShopping;Data Source=(local);User ID=sa;Pwd=123";
        //string ConnectionString = "Initial Catalog=Db_GenericartShopping;Data Source=(local)";
        //string ConnectionString = "Data Source = sqlexpress; integrated security = SSPI; initial catalog = Db_GenericartShopping;";

        return System.Web.Configuration.WebConfigurationManager.ConnectionStrings["GenCartDATA"].ConnectionString;
    }

    public static int NonQuery(string sqlstr)
    {
        string ConnectionString = Getconnection();
        SqlConnection cn = new SqlConnection(ConnectionString);
        cn.Open();
        SqlCommand cmd = new SqlCommand(sqlstr, cn);
        int n = cmd.ExecuteNonQuery();
        cn.Close();
        return (n);
    }

    public static int ScalerQuery(string sqlstr)
    {
        string ConnectionString = Getconnection();
        SqlConnection cn = new SqlConnection(ConnectionString);
        cn.Open();
        SqlCommand cmd = new SqlCommand(sqlstr, cn);
        int n = Convert.ToInt32(cmd.ExecuteScalar());
        cn.Close();
        return n;
    }

    public static string StringValue(string sqlstr)
    {
        string ConnectionString = Getconnection();
        SqlConnection cn = new SqlConnection(ConnectionString);
        cn.Open();
        SqlCommand cmd = new SqlCommand(sqlstr, cn);
        String n = Convert.ToString(cmd.ExecuteScalar());
        cn.Close();
        return n;
    }

    private static Random random = new Random();

    public static double GetStockAdminItemWise(int ItemAuto)
    {
        return 100000;
    }

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public static void SendSms(string MobileNo, string Msg)
    {
        try
        {
            string Abc = HttpContext.Current.Request.Url.Host;
            if (Abc != "localhost")
            {
                
            }
        }
        catch (Exception ex)
        {
        }
    }

    // <summary>
    /// Route Path of Project
    /// </summary>
    /// <param name="reqType">Type(0-Routepath/1-CSS/2-Javascript)</param>
    /// <returns>Domain Path(LocalHost/Online)</returns>
    public string ReturnHttp()
    {
        try
        {
            string rValue = null;
            string domain = "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
            string localFolder;

            if (domain.Contains("localhost") == true)
            {
                localFolder = "/";
                //localFolder = "/GenericartShopping/";
            }
            else
            {
                localFolder = "/";
            }

            rValue = domain + localFolder;

            return rValue;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Creates SEO friendly url (Removes special characters from url)
    /// </summary>
    /// <param name="oldUrl">Current Url</param>
    /// <returns>Modified Url</returns>
    public string UrlGenerator(string oldUrl)
    {
        try
        {
            if (oldUrl.Contains("$") == true)
            {
                oldUrl = oldUrl.Replace("$", "");
            }
            if (oldUrl.Contains(" ") == true)
            {
                oldUrl = oldUrl.Replace(" ", "-");
            }
            if (oldUrl.Contains("+") == true)
            {
                oldUrl = oldUrl.Replace("+", "");
            }
            if (oldUrl.Contains(";") == true)
            {
                oldUrl = oldUrl.Replace(";", "");
            }
            if (oldUrl.Contains("=") == true)
            {
                oldUrl = oldUrl.Replace("=", "");
            }
            if (oldUrl.Contains("?") == true)
            {
                oldUrl = oldUrl.Replace("?", "");
            }
            if (oldUrl.Contains("@") == true)
            {
                oldUrl = oldUrl.Replace("@", "");
            }
            if (oldUrl.Contains("<") == true)
            {
                oldUrl = oldUrl.Replace("<", "");
            }
            if (oldUrl.Contains('"') == true)
            {
                oldUrl = oldUrl.Replace('"', '-');
            }
            if (oldUrl.Contains("'") == true)
            {
                oldUrl = oldUrl.Replace("'", "-");
            }
            if (oldUrl.Contains(">") == true)
            {
                oldUrl = oldUrl.Replace(">", "");
            }
            if (oldUrl.Contains("#") == true)
            {
                oldUrl = oldUrl.Replace("#", "");
            }
            if (oldUrl.Contains("{") == true)
            {
                oldUrl = oldUrl.Replace("{", "");
            }
            if (oldUrl.Contains("}") == true)
            {
                oldUrl = oldUrl.Replace("}", "");
            }
            if (oldUrl.Contains("|") == true)
            {
                oldUrl = oldUrl.Replace("|", "");
            }
            if (oldUrl.Contains("\\") == true)
            {
                oldUrl = oldUrl.Replace("\\", "");
            }
            if (oldUrl.Contains("^") == true)
            {
                oldUrl = oldUrl.Replace("^", "");
            }
            if (oldUrl.Contains("~") == true)
            {
                oldUrl = oldUrl.Replace("~", "");
            }
            if (oldUrl.Contains("[") == true)
            {
                oldUrl = oldUrl.Replace("[", "");
            }
            if (oldUrl.Contains("]") == true)
            {
                oldUrl = oldUrl.Replace("]", "");
            }
            if (oldUrl.Contains("`") == true)
            {
                oldUrl = oldUrl.Replace("`", "");
            }
            if (oldUrl.Contains("%") == true)
            {
                oldUrl = oldUrl.Replace("%", "percent");
            }
            if (oldUrl.Contains("&") == true)
            {
                oldUrl = oldUrl.Replace("&", "and");
            }
            if (oldUrl.Contains(":") == true)
            {
                oldUrl = oldUrl.Replace(":", "");
            }
            if (oldUrl.Contains(".") == true)
            {
                oldUrl = oldUrl.Replace(".", "-");
            }
            if (oldUrl.Contains(",") == true)
            {
                oldUrl = oldUrl.Replace(",", "-");
            }
            if (oldUrl.Contains("/") == true)
            {
                oldUrl = oldUrl.Replace("/", "");
            }

            if (oldUrl.Contains("(") == true)
            {
                oldUrl = oldUrl.Replace("(", "");
            }
            if (oldUrl.Contains(")") == true)
            {
                oldUrl = oldUrl.Replace(")", "");
            }
            if (oldUrl.Contains("--") == true)
            {
                oldUrl = oldUrl.Replace("--", "-");
            }
            if (oldUrl.EndsWith("-") == true)
            {
                oldUrl = oldUrl.Substring(0, oldUrl.Length - 1);
            }

            return oldUrl.ToLower();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    /// <summary>
    /// Used to display error Messagge on Client Side
    /// </summary>
    /// <param name="errType">Error Type as byte 1 for Succes/2 for Warning/3 for Error</param>
    /// <param name="errMsgStr">Message to display</param>
    /// <returns>Markup as string</returns>
    public string ErrNotification(byte errType, string errMsgStr)
    {
        try
        {
            string rValue = "";

            switch (errType)
            {
                case 1:
                    rValue = "<span class='success'><b>Success:</b> " + errMsgStr + "</span>";
                    break;
                case 2:
                    rValue = "<span class='warning'><b>Warning:</b> " + errMsgStr + "</span>";
                    break;
                case 3:
                    rValue = "<span class='error'><b>Error:</b> " + errMsgStr + "</span>";
                    break;
            }

            return rValue;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Executes a SQL Query
    /// </summary>
    /// <param name="strQuery">SQL Query as String</param>
    public void ExecuteQuery(string strQuery)
    {
        try
        {
            SqlConnection con = new SqlConnection(Getconnection());
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

    /// <summary>
    /// Used to get single value from databse table
    /// </summary>
    /// <param name="tableName">Name of Database Table</param>
    /// <param name="fieldName">Naem of Column</param>
    /// <param name="whereCon">Where Condition</param>
    /// <returns>Value as object</returns>
    public object GetReqData(string tableName, string fieldName, string whereCon)
    {
        try
        {
            object retValue = null;
            SqlConnection con = new SqlConnection(Getconnection());
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
}