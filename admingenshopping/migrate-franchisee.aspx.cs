using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

public partial class admingenshopping_migrate_franchisee : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        BtnSubmit.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(BtnSubmit, null) + ";");
        btnUpdateHeads.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnUpdateHeads, null) + ";");
    }


    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            using (DataTable dtFr = GetDataTable("Select * From generica_genericDBSQL.dbo.FranchiseeData Where delMark=0 AND isClosed=0 AND frAnniversary IS NOT NULL AND frAnniversary<>'' AND SUBSTRING(frShopCode,1,4)<>'GMDR' AND SUBSTRING(frShopCode,1,4)<>'GMOS' AND SUBSTRING(frShopCode,1,4)<>'TRFR' AND SUBSTRING(frShopCode,1,4)<>'TRAN' AND SUBSTRING(frShopCode,1,4)<>'CANC'"))
            {
                if (dtFr.Rows.Count > 0)
                {
                    int frCount = 0;
                    foreach (DataRow row in dtFr.Rows)
                    {
                        if (!IsRecordExist("Select FranchID From admin_GenericEcommData.dbo.FranchiseeData Where FranchShopCode='" + row["frShopCode"].ToString() + "'"))
                        {
                            int maxId = NextId("admin_GenericEcommData.dbo.FranchiseeData", "FranchID");
                            ExecuteQuery("Insert Into admin_GenericEcommData.dbo.FranchiseeData (FranchID, FranchRegDate, FranchShopCode, " +
                                " FranchName, FranchOwnerName, FK_FranchStateId, FK_FranchCityId, FranchPinCode, FranchAddress, " +
                                " FranchLatLong, FranchEmail, FranchMobile, FranchPassword, FranchBankName, FranchBankAccName, " +
                                " FranchActive, FranchLegalBlock) Values (" + maxId + ", '" + row["frRegDate"].ToString() +
                                "', '" + row["frShopCode"].ToString() + "', '" + row["frProposedName"].ToString() + "', '" + row["frAppName"].ToString() +
                                "', 0, 0, '" + row["pinCode"].ToString() + "', '" + row["frShopAddress"].ToString() + "', '" + row["frLatLong"].ToString() +
                                "', '" + row["frEmail"].ToString() + "', '" + row["frMobile"].ToString() + "', '123456', '" + row["bankName"].ToString() +
                                "', '" + row["accHolder"].ToString() + "', 1, " + row["legalBlock"].ToString() + ")");

                            frCount++;
                        }
                    }

                    errMsg = c.ErrNotification(1, " " + frCount + " Franchisee Shifted Successfully..!!");
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
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            using (DataTable dtFr = GetDataTable("Select * From generica_genericDBSQL.dbo.FranchiseeData Where delMark=0 AND isClosed=0 AND frAnniversary IS NOT NULL AND frAnniversary<>'' AND SUBSTRING(frShopCode,1,4)<>'GMDR' AND SUBSTRING(frShopCode,1,4)<>'GMOS' AND SUBSTRING(frShopCode,1,4)<>'TRFR' AND SUBSTRING(frShopCode,1,4)<>'TRAN' AND SUBSTRING(frShopCode,1,4)<>'CANC'"))
            {
                if (dtFr.Rows.Count > 0)
                {
                    int frCount = 0;
                    foreach (DataRow row in dtFr.Rows)
                    {
                        if (IsRecordExist("Select FranchID From admin_GenericEcommData.dbo.FranchiseeData Where FranchShopCode='" + row["frShopCode"].ToString() + "'"))
                        //if (IsRecordExist("Select FranchID From GenericEcommData.dbo.FranchiseeData Where FranchShopCode='" + row["frShopCode"].ToString() + "'"))
                        {
                            //int frId = NextId("admin_GenericEcommData.dbo.FranchiseeData", "FranchID");
                            int frId = Convert.ToInt32(GetReqData("FranchiseeData", "FranchID", "FranchShopCode='" + row["frShopCode"].ToString() + "'"));
                            //ExecuteQuery("Update admin_GenericEcommData.dbo.FranchiseeData Set FranchShopCode='" + row["frShopCode"].ToString() + "', FranchName='" + row["frProposedName"].ToString() +
                            //    "', FranchOwnerName='" + row["frAppName"].ToString() + "', FranchPinCode='" + row["pinCode"].ToString() +
                            //    "', FranchAddress='" + row["frShopAddress"].ToString() + "', FranchLatLong='" + row["frLatLong"].ToString() +
                            //    "', FranchEmail='" + row["frEmail"].ToString() + "', FranchMobile='" + row["frMobile"].ToString() +
                            //    "', FranchBankName='" + row["bankName"].ToString() + "', FranchBankAccName='" + row["accHolder"].ToString() +
                            //    "', FranchLegalBlock=" + row["legalBlock"].ToString() + " Where FranchID=" + frId);
                            ExecuteQuery("Update admin_GenericEcommData.dbo.FranchiseeData Set FK_FranchStateId=" + row["stateId"] + 
                                ", FK_FranchCityId=" + row["cityId"] + ", FK_FranchDistId=" + row["distId"] + " Where FranchID=" + frId);

                            frCount++;
                        }
                    }

                    errMsg = c.ErrNotification(1, " " + frCount + " Franchisee Updated Successfully..!!");
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    protected void btnUpdateHeads_Click(object sender, EventArgs e)
    {
        try
        {
            using (DataTable dtFr = c.GetDataTable("Select * From FranchiseeData Where FranchActive=1"))
            {
                if (dtFr.Rows.Count > 0)
                {
                    int frCount = 0;
                    foreach (DataRow row in dtFr.Rows)
                    {

                        int distHdId = Convert.ToInt32(GetReqData("DistrictHead", "DistHdId", "DelMark=0 AND DistHdDistrictId='" + row["FK_FranchDistId"].ToString() + "'"));
                        int zonalHdId = Convert.ToInt32(GetReqData("ZonalHeadDistricts", "ZonalHdId", "DelMark=0 AND DistrictId='" + row["FK_FranchDistId"].ToString() + "'"));
                        ExecuteQuery("Update FranchiseeData Set FK_DistHdId=" + distHdId + ", FK_ZonalHdId=" + zonalHdId + " Where FranchID=" + row["FranchID"]);

                        frCount++;
                        
                    }

                    errMsg = c.ErrNotification(1, " " + frCount + " Franchisee Head Updated Successfully..!!");
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
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