using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

public partial class admingenshopping_move_dh_zh : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        btnMoveDh.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnMoveDh, null) + ";");
        btnMoveZh.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnMoveZh, null) + ";");
    }

    protected void btnMoveZh_Click(object sender, EventArgs e)
    {
        try
        {
            using (DataTable dtStates = GetDataTable("Select * From Genericart.dbo.AreaHead Where delMark=0"))
            {
                if (dtStates.Rows.Count > 0)
                {
                    int frCount = 0;
                    foreach (DataRow row in dtStates.Rows)
                    {
                        if (!IsRecordExist("Select ZonalHdId From GenericartEcom.dbo.ZonalHead Where ZonalHdName='" + row["areaHdName"].ToString() + "'"))
                        {
                            int maxId = NextId("ZonalHead", "ZonalHdId");
                            ExecuteQuery("Insert Into GenericartEcom.dbo.ZonalHead (ZonalHdId, ZonalHdName, ZonalHdFirmName, ZonalHdStateId, ZonalHdDistrictId, ZonalHdCityName, ZonalHdAddress, " +
                                " ZonalHdPincode, ZonalHdMobileNo, ZonalHdPhoneNo, ZonalHdEmail, ZonalHdUserId, ZonalHdPass, ZonalHdDob, ZonalHdPhoto, DelMark) Values " +
                                " (" + maxId + ", '" + row["areaHdName"] + "', '" + row["firmName"] + "', " + row["areaHdStateId"] + ", 0, '" + row["cityName"] +
                                "', '" + row["areaHdAddress"] + "', '" + row["pincode"] + "', '" + row["mobileNo"] + "', '" + row["phoneNo"] +
                                "', '" + row["email"] + "', '" + row["userId"] + "', '" + row["areaHdPass"] + "', '" + row["dob"] + "', '" + row["photo"] + "', '" + row["delMark"] + "')");

                            frCount++;
                        }
                    }

                    errMsg = c.ErrNotification(1, " " + frCount + " ZH Shifted Successfully..!!");
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    protected void btnZhDist_Click(object sender, EventArgs e)
    {
        try
        {
            using (DataTable dtStates = GetDataTable("Select * From generica_genericDBSQL.dbo.AreaHeadDistricts Where districtId IS NOT NULL"))
            {
                if (dtStates.Rows.Count > 0)
                {
                    int frCount = 0;
                    foreach (DataRow row in dtStates.Rows)
                    {
                        //if (!IsRecordExist("Select ZHDId From GenericEcommData.dbo.ZonalHeadDistricts Where ZonalHdId='" + row["areaHdId"].ToString() + "'"))
                        //{
                        ExecuteQuery("Insert Into admin_GenericEcommData.dbo.ZonalHeadDistricts (ZHDId, ZonalHdId, DistrictId, DelMark) Values " +
                                " (" + row["AHDId"] + ", " + row["areaHdId"] + ", " + row["districtId"] + ", " + row["delMark"] + ")");

                            frCount++;
                        //}
                    }

                    errMsg = c.ErrNotification(1, " " + frCount + " ZH Districts Shifted Successfully..!!");
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    protected void btnMoveDh_Click(object sender, EventArgs e)
    {
        try
        {
            using (DataTable dtStates = GetDataTable("Select * From Genericart.dbo.DistrictHead Where delMark=0"))
            {
                if (dtStates.Rows.Count > 0)
                {
                    int frCount = 0;
                    foreach (DataRow row in dtStates.Rows)
                    {
                        if (!IsRecordExist("Select DistHdId From GenericartEcom.dbo.DistrictHead Where DistHdName='" + row["distHdName"].ToString() + "'"))
                        {
                            int maxId = NextId("DistrictHead", "DistHdId");
                            ExecuteQuery("Insert Into GenericartEcom.dbo.DistrictHead (DistHdId, DistHdName, DistHdFirmName, DistHdStateId, DistHdDistrictId, DistHdCityName, DistHdAddress, " +
                                " DistHdPincode, DistHdMobileNo, DistHdPhoneNo, DistHdEmail, DistHdUserId, DistHdPass, DistHdDob, DistHdPhoto, DelMark) Values " +
                                " (" + maxId + ", '" + row["distHdName"] + "', '" + row["firmName"] + "', " + row["distHdStateId"] + ", " + row["distHdDistrictId"] +
                                ", '" + row["cityName"] + "', '" + row["distHdAddress"] + "', '" + row["pincode"] + "', '" + row["mobileNo"] + "', '" + row["phoneNo"] +
                                "', '" + row["email"] + "', '" + row["userId"] + "', '" + row["distHdPass"] + "', '" + row["dob"] + "', '" + row["photo"] + "', '" + row["delMark"] + "')");

                            frCount++;
                        }
                    }

                    errMsg = c.ErrNotification(1, " " + frCount + " DH Shifted Successfully..!!");
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    protected void btnMgmt_Click(object sender, EventArgs e)
    {
        try
        {
            using (DataTable dtStates = GetDataTable("Select * From Genericart.dbo.ManagementTeam Where delMark=0"))
            {
                if (dtStates.Rows.Count > 0)
                {
                    int frCount = 0;
                    foreach (DataRow row in dtStates.Rows)
                    {
                        if (!IsRecordExist("Select EmpId From admin_GenericEcommData.dbo.ManagementTeam Where EmpName='" + row["empName"].ToString() + "'"))
                        {
                            ExecuteQuery("Insert Into admin_GenericEcommData.dbo.ManagementTeam (EmpId, EmpName, EmpStateId, EmpDistId, EmpCityName, EmpDesignation, EmpMobileNo, " +
                                " EmpEmail, EmpPhoto, EmpLevel, EmpAddress, EmpPassword, LoginAuth, MgmtUserId, DelMark) Values " +
                                " (" + row["empId"] + ", '" + row["empName"] + "', " + row["stateId"] + ", " + row["distId"] + ", '" + row["cityName"] +
                                "', '" + row["designation"] + "', '" + row["mobileNo"] + "', '" + row["email"] + "', '" + row["empPhoto"] +
                                "', " + row["empLevel"] + ", '" + row["empAddress"] + "', '" + row["empPassword"] + "', " + row["loginAuth"] + ", '" + row["mgmtUserId"] + "', '" + row["delMark"] + "')");

                            frCount++;
                        }
                    }

                    errMsg = c.ErrNotification(1, " " + frCount + " Management Shifted Successfully..!!");
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