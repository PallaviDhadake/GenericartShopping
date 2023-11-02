using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Text;

public partial class supportteam_staff_training_videos : System.Web.UI.Page
{
    //iClass c = new iClass();
    public string videoStr, completedVideoStr;
    protected void Page_Load(object sender, EventArgs e)
    {

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

    public object GetReqData(string tableName, string fieldName, string whereCon)
    {
        try
        {
            object retValue = null;
            SqlConnection con = new SqlConnection(OpenConnection1());
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

    public bool IsRecordExist(string strQuery)
    {
        try
        {

            bool rValue = false;
            SqlConnection con = new SqlConnection(OpenConnection1());
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


    private void GetVideo()
    {
        try
        {
           
            string strQuery = "Select VideoID, VideoTitle, LEFT(VideoTitle, 25) as vTitle, VideoURL, VideoDisplayOrder, VideoEncryptCode From TrainingVideos Where DelMark=0 AND VideoLangId=" + ddrLang.SelectedValue + " AND VideoActive=1 Order By VideoDisplayOrder";


            using (DataTable dtVideos = GetDataTable(strQuery))
            {
                if (dtVideos.Rows.Count > 0)
                {
                    StringBuilder strMarkup = new StringBuilder();

                    
                    strMarkup.Append("<span class=\"space10\"></span>");
                    //int boxCount = 0;
                   
                    strMarkup.Append("<div class=\"row\">");
                    foreach (DataRow row in dtVideos.Rows)
                    {
                        
                        strMarkup.Append("<div class=\"col-lg-3 col-6\">");
                        strMarkup.Append("<div class=\"txtCenter\">");
                        string displayOrder = row["VideoDisplayOrder"].ToString();
                        strMarkup.Append("<a data-fancybox href=\"https://www.youtube.com/watch?v=" + row["VideoURL"] + "\" class=\"txtDecNone\" title=\"" + row["VideoTitle"].ToString() + "\">");
                        strMarkup.Append("<div class=\"bgVidDarkGrey\">");
                        strMarkup.Append("<img src=\"http://img.youtube.com/vi/" + row["VideoURL"] + "/0.jpg\" alt=\"" + row["VideoTitle"].ToString() + "\" class=\"width100\">");
                        strMarkup.Append("<div class=\"pad_10\">");
                        strMarkup.Append("<span class=\"clrWhite semiBold line-ht-5 regular\">" + row["vTitle"].ToString() + "</span>");
                        strMarkup.Append("</a>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");

                        strMarkup.Append("<div class=\"pad_10\">");
                        strMarkup.Append("</div>");

                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                       
                        //boxCount++;

                        //if ((boxCount % 4) == 0)
                        //{
                        //    strMarkup.Append("<span class=\"space15\"></span>");
                        //}
                    }
                    //strMarkup.Append("<div class=\"float_clear\"></div>");
                   
                    strMarkup.Append("</div>");
                    videoStr = strMarkup.ToString();
                }
            }

        }
        catch (Exception)
        {
            // ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Error Occoured while processing', type: 'error'});", true);
            //c.ErrorLogHandler(this.ToString(), "GetVideo", ex.Message.ToString());
            // return;
            throw;
        }
    }

    protected void ddrLang_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddrLang.SelectedIndex > 0)
        {
            GetVideo();
        }
        else
        {
            videoStr = "<div class=\"themeBgSec txtCenter\"><div class=\"pad_10\"><span class=\"clrWhite fontRegular semiMedium\">Select Language To View Videos</span></div></div>";
            return;
        }
    }

}