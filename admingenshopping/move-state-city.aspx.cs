using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

public partial class admingenshopping_move_state_city : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        btnMoveState.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnMoveState, null) + ";");
        btnMoveDist.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnMoveDist, null) + ";");
        btnMoveCity.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnMoveCity, null) + ";");
    }

    protected void btnMoveState_Click(object sender, EventArgs e)
    {
        try
        {
            using (DataTable dtStates = GetDataTable("Select * From StatesData Where delmark=0"))
            {
                if (dtStates.Rows.Count > 0)
                {
                    int frCount = 0;
                    foreach (DataRow row in dtStates.Rows)
                    {
                        if (!IsRecordExist("Select StateID From StatesData Where StateName='" + row["stateName"].ToString() + "'"))
                        //if (!IsRecordExist("Select StateID From GenericEcommData.dbo.StatesData Where StateName='" + row["stateName"].ToString() + "'"))
                        {
                            //int maxId = NextId("GenericEcommData.dbo.StatesData", "StateID");
                            ExecuteQuery("Insert Into StatesData (StateID, StateName, FK_CountryID) Values (" + row["stateId"].ToString() + 
                                ", '" + row["stateName"].ToString() + "', 101)");

                            frCount++;
                        }
                    }

                    errMsg = c.ErrNotification(1, " " + frCount + " States Shifted Successfully..!!");
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
    protected void btnMoveDist_Click(object sender, EventArgs e)
    {
        try
        {
            using (DataTable dtStates = GetDataTable("Select * From DistrictsData Where delmark=0"))
            {
                if (dtStates.Rows.Count > 0)
                {
                    int frCount = 0;
                    foreach (DataRow row in dtStates.Rows)
                    {
                        if (!IsRecordExist("Select DistrictId From DistrictsData Where DistrictName='" + row["districtName"].ToString() + "'"))
                        //if (!IsRecordExist("Select DistrictId From GenericEcommData.dbo.DistrictsData Where DistrictName='" + row["districtName"].ToString() + "'"))
                        {
                            ExecuteQuery("Insert Into DistrictsData (DistrictId, DistrictName, StateId, DelMark) Values " +
                                " (" + row["districtId"].ToString() + ", '" + row["districtName"].ToString() + "', " + row["stateId"] + ", 0)");

                            frCount++;
                        }
                    }

                    errMsg = c.ErrNotification(1, " " + frCount + " Districts Shifted Successfully..!!");
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    protected void btnMoveCity_Click(object sender, EventArgs e)
    {
        try
        {
            using (DataTable dtStates = GetDataTable("Select * From CityData Where delmark=0"))
            {
                if (dtStates.Rows.Count > 0)
                {
                    int frCount = 0;
                    foreach (DataRow row in dtStates.Rows)
                    {
                        if (!IsRecordExist("Select CityID From CityData Where CityName='" + row["cityName"].ToString() + "'"))
                        //if (!IsRecordExist("Select CityID From GenericEcommData.dbo.CityData Where CityName='" + row["cityName"].ToString() + "'"))
                        {
                            ExecuteQuery("Insert Into CityData (CityID, CityName, FK_DistId) Values " +
                                " (" + row["cityId"].ToString() + ", '" + row["cityName"].ToString() + "', " + row["districtId"] + ")");

                            frCount++;
                        }
                    }

                    errMsg = c.ErrNotification(1, " " + frCount + " Cities Shifted Successfully..!!");
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
    

    public string OpenConnection()
    {
        return System.Web.Configuration.WebConfigurationManager.ConnectionStrings["GenCartDATA"].ConnectionString;
    }

    public string OpenConnection1()
    {
        return System.Web.Configuration.WebConfigurationManager.ConnectionStrings["GenCartDATAReg"].ConnectionString;
    }

    public DataTable GetDataTable(string strQuery)
    {
        try
        {
            SqlConnection con = new SqlConnection(OpenConnection1());

            SqlDataAdapter da = new SqlDataAdapter(strQuery, con);
            DataTable dt = new DataTable();

            da.Fill(dt);

            con.Close();
            con = null;

            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }

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
    
}