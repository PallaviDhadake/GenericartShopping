using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class obpmanager_registered_current_month_gobp : System.Web.UI.Page
{
    iClass c = new iClass();
    public string pgTitle, enqCount, headInfo, clsName, followupHistory;
    public string[] enqData = new string[50];//42
    public string[] countData = new string[10];
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            btnCancel.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnCancel, null) + ";");

            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    readFrEnquiry.Visible = true;
                    viewFrEnquiry.Visible = false;
                    lblId.Text = Convert.ToInt32(Request.QueryString["id"]).ToString();
                    GetGOBPEnqData(Convert.ToInt32(Request.QueryString["id"]));
                    GetFollowupHistory();
                    GetCount();
                }
                else
                {
                    viewFrEnquiry.Visible = true;
                    readFrEnquiry.Visible = false;

                    FillDDR();
                    FillGrid();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "Page_Load", ex.Message.ToString());
            return;
        }
    }

    private void FillDDR()
    {
        try
        {
            DateTime endDate = DateTime.Now;
            DateTime startDate = DateTime.Now;

            endDate = DateTime.Now.AddMonths(0);
            startDate = endDate.AddMonths(-11);

            SqlConnection con = new SqlConnection(c.OpenConnection());

            SqlDataAdapter da = new SqlDataAdapter("DECLARE @start DATE = '" + startDate + "'  , @end DATE = '" + endDate + "'  ;WITH Numbers (Number) AS " +
                        " (SELECT ROW_NUMBER() OVER (ORDER BY OBJECT_ID) FROM sys.all_objects) " +
                        " SELECT DATENAME(MONTH,DATEADD(MONTH, Number - 1, @start)) +  ' - ' + DATENAME(YEAR,DATEADD(MONTH, Number - 1, @start)) as monthY, " +
                        " MONTH(DATEADD(MONTH, Number - 1, @start)) MonthId FROM Numbers  a WHERE Number - 1 <= DATEDIFF(MONTH, @start, @end) ", con);

            DataSet ds = new DataSet();
            da.Fill(ds, "myCombo");

            ddrMonth.DataSource = ds.Tables[0];
            ddrMonth.DataTextField = ds.Tables[0].Columns["monthY"].ColumnName.ToString();
            ddrMonth.DataValueField = ds.Tables[0].Columns["MonthId"].ColumnName.ToString();
            ddrMonth.DataBind();

            ddrMonth.Items.Insert(0, "<-Select->");
            ddrMonth.Items[0].Value = "0";

            con.Close();
            con = null;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FIllDDR", ex.Message.ToString());
            return;
        }
    }

    private void GetCount()
    {
        try
        {
            int obpid = Convert.ToInt32(Request.QueryString["id"].ToString());
            if (Request.QueryString["type"] == "thismonth")
            {
                countData[0] = c.returnAggregate("SELECT ISNULL(COUNT([CustomrtID]),0) FROM [dbo].[CustomersData] WHERE [delMark] = 0 AND [CustomerActive] = 1 AND YEAR([CustomerJoinDate]) = YEAR(GETDATE()) AND MONTH([CustomerJoinDate]) = MONTH(GETDATE()) AND [FK_ObpID] = " + obpid).ToString();
            }
            else
            {
                countData[0] = c.returnAggregate("SELECT ISNULL(COUNT([CustomrtID]),0) FROM [dbo].[CustomersData] WHERE [delMark] = 0 AND [CustomerActive] = 1 AND [FK_ObpID] = " + obpid).ToString();
            }

            if (Request.QueryString["type"] == "thismonth")
            {
                countData[1] = c.returnAggregate(@"SELECT ISNULL(COUNT(a.[CustomrtId]),0) FROM [dbo].[CustomersData] AS a 
                                                   LEFT JOIN [dbo].[OBPData] AS b ON a.[FK_ObpID] = b.[OBP_ID] WHERE a.[delMark] = 0 AND a.[CustomerActive] = 1 AND YEAR([CustomerJoinDate]) = YEAR(GETDATE()) AND MONTH([CustomerJoinDate]) = MONTH(GETDATE()) AND a.[FK_ObpID] = " + obpid).ToString();
            }
            else
            {
                countData[1] = c.returnAggregate(@"SELECT ISNULL(COUNT(DISTINCT a.[OrderID]),0) FROM [dbo].[OrdersData] AS a
                                                   LEFT JOIN [dbo].[OBPData] AS b ON a.[GOBPId] = b.[OBP_ID] WHERE b.[OBP_DelMark] = 0 AND b.[OBP_ID] = " + obpid).ToString();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetCount", ex.Message.ToString());
            return;
        }
    }

    private void FillGrid()
    {
        try
        {
            string strQuery = "";
            string[] arrYear = ddrMonth.SelectedItem.Text.ToString().Split('-');
            string yearData = arrYear[1].ToString();

            if (Request.QueryString["type"] == "thismonth")
            {
                if (Request.QueryString["gobpregtype"] == "newgobpmonth")
                {
                    if (Convert.ToInt32(ddrMonth.SelectedValue) != 0)
                    {
                        strQuery = @"SELECT
                                a.[OBP_ID],
                                a.[OBP_ApplicantName],
                                CONVERT(VARCHAR(20), a.[OBP_JoinDate], 103) as joinDate,
                                a.[OBP_MobileNo],
                                a.[OBP_StatusFlag],
                                a.[OBP_DH_Name] as dh,
                                (SELECT COUNT(FL_ID) FROM [dbo].[FollowupOBP] WHERE [FK_OBPManID] = " + Session["adminObpManager"] + " AND [FK_OBPId] = a.[OBP_ID]) as flCount,"
                                    + "ISNULL((SELECT [DistHdMobileNo] FROM [dbo].[DistrictHead] WHERE [DistHdUserId] = a.[OBP_DH_UserId]), '-') as dhContact,"
                                    + "(SELECT COUNT([CustomrtID]) FROM [dbo].[CustomersData] WHERE [delMark] = 0 AND [CustomerActive] = 1 AND [FK_ObpID] = a.[OBP_ID]) as custCount"
                                + " FROM [dbo].[OBPData] a"
                                + " WHERE a.[OBP_DelMark] = 0 AND (YEAR(a.[OBP_JoinDate]) = " + yearData + " AND MONTH(a.[OBP_JoinDate]) = '" + Convert.ToInt32(ddrMonth.SelectedValue) + "')";
                    }
                    else
                    {
                        strQuery = @"SELECT 
                                    a.[OBP_ID], 
                            	    a.[OBP_ApplicantName], 
                            	    CONVERT(VARCHAR(20), a.[OBP_JoinDate], 103) as joinDate, 
                            	    a.[OBP_MobileNo], 
                            	    a.[OBP_StatusFlag], 
                            	    a.[OBP_DH_Name] as dh, 
                            	    (SELECT COUNT(FL_ID) FROM [dbo].[FollowupOBP] WHERE [FK_OBPManID] = " + Session["adminObpManager"] + " AND [FK_OBPId] = a.[OBP_ID]) as flCount,"
                                        + "ISNULL((SELECT [DistHdMobileNo] FROM [dbo].[DistrictHead] WHERE [DistHdUserId] = a.[OBP_DH_UserId]), '-') as dhContact, "
                                        + "(SELECT COUNT([CustomrtID]) FROM [dbo].[CustomersData] WHERE [delMark] = 0 AND [CustomerActive] = 1 AND [FK_ObpID] = a.[OBP_ID]) as custCount"
                                    + " FROM[dbo].[OBPData] a"
                                    + " WHERE YEAR(a.[OBP_JoinDate]) = YEAR(GETDATE()) AND MONTH(a.[OBP_JoinDate]) = MONTH(GETDATE())"
                                    + " AND a.[OBP_DelMark] = 0";
                    }
                }
            }
            else if (Request.QueryString["type"] == "thismonthactive")
            {
                if (Request.QueryString["gobpregtype"] == "newgobpmonth")
                {
                    if (Convert.ToInt32(ddrMonth.SelectedValue) != 0)
                    {
                        strQuery = @"SELECT DISTINCT
                                    a.[OBP_ID], 
                            	    a.[OBP_ApplicantName], 
                            	    CONVERT(VARCHAR(20), a.[OBP_JoinDate], 103) as joinDate, 
                            	    a.[OBP_MobileNo], 
                            	    a.[OBP_StatusFlag], 
                            	    a.[OBP_DH_Name] as dh, 
                            	    (SELECT COUNT(FL_ID) FROM [dbo].[FollowupOBP] WHERE [FK_OBPManID] = " + Session["adminObpManager"] + " AND [FK_OBPId] = a.[OBP_ID]) as flCount,"
                                        + "ISNULL((SELECT [DistHdMobileNo] FROM [dbo].[DistrictHead] WHERE [DistHdUserId] = a.[OBP_DH_UserId]), '-') as dhContact,"
                                        + "(SELECT COUNT([CustomrtID]) FROM [dbo].[CustomersData] WHERE [delMark] = 0 AND[CustomerActive] = 1 AND [FK_ObpID] = a.[OBP_ID]) as custCount"
                                    + " FROM [dbo].[OBPData] a"
                                    + " LEFT JOIN [dbo].[CustomersData] b ON a.[OBP_ID] = b.[FK_ObpID]"
                                    + " WHERE b.[FK_ObpID] IS NOT NULL"
                                    + " AND b.[FK_ObpID] > 0"
                                    + " AND a.[OBP_DelMark] = 0 AND YEAR(a.[OBP_JoinDate]) = " + yearData + " AND MONTH(a.[OBP_JoinDate]) = '" + Convert.ToInt32(ddrMonth.SelectedValue) + "'";
                    }
                    else
                    {
                        strQuery = @"SELECT DISTINCT
                                    a.[OBP_ID],
                                    a.[OBP_ApplicantName],
                                    CONVERT(VARCHAR(20), a.[OBP_JoinDate], 103) as joinDate,
                                    a.[OBP_MobileNo],
                                    a.[OBP_StatusFlag],
                                    a.[OBP_DH_Name] as dh,
                                    (SELECT COUNT([FL_ID]) FROM [dbo].[FollowupOBP] WHERE [FK_OBPManID] = " + Session["adminObpManager"] + " AND [FK_OBPId] = a.[OBP_ID]) as flCount,"
                                        + "ISNULL((SELECT [DistHdMobileNo] FROM [dbo].[DistrictHead] WHERE [DistHdUserId] = a.[OBP_DH_UserId]), '-') as dhContact,"
                                        + "(SELECT COUNT([CustomrtID]) FROM [dbo].[CustomersData] WHERE [delMark] = 0 AND [CustomerActive] = 1 AND [FK_ObpID] = a.[OBP_ID]) as custCount"
                                    + " FROM [dbo].[OBPData] a"
                                    + " LEFT JOIN [dbo].[CustomersData] b ON a.[OBP_ID] = b.[FK_ObpID]"
                                    + " WHERE b.[FK_ObpID] IS NOT NULL"
                                    + " AND b.[FK_ObpID] > 0"
                                    + " AND YEAR(a.[OBP_JoinDate]) = YEAR(GETDATE()) AND MONTH(a.[OBP_JoinDate]) = MONTH(GETDATE())"
                                    + " AND a.[OBP_DelMark] = 0";
                    }
                }
            }
            else if (Request.QueryString["type"] == "thismonthinactive")
            {
                if (Request.QueryString["gobpregtype"] == "newgobpmonth")
                {
                    if (Convert.ToInt32(ddrMonth.SelectedValue) != 0)
                    {
                        strQuery = @"SELECT DISTINCT
                                    a.[OBP_ID], 
                            	    a.[OBP_ApplicantName], 
                            	    CONVERT(VARCHAR(20), a.[OBP_JoinDate], 103) as joinDate, 
                            	    a.[OBP_MobileNo], 
                            	    a.[OBP_StatusFlag], 
                            	    a.[OBP_DH_Name] as dh, 
                            	    (SELECT COUNT(FL_ID) FROM [dbo].[FollowupOBP] WHERE [FK_OBPManID] = " + Session["adminObpManager"] + " AND [FK_OBPId] = a.[OBP_ID]) as flCount,"
                                        + "ISNULL((SELECT [DistHdMobileNo] FROM [dbo].[DistrictHead] WHERE [DistHdUserId] = a.[OBP_DH_UserId]), '-') as dhContact,"
                                        + "(SELECT COUNT([CustomrtID]) FROM [dbo].[CustomersData] WHERE [delMark] = 0 AND [CustomerActive] = 1 AND [FK_ObpID] = a.[OBP_ID]) as custCount"
                                    + " FROM[dbo].[OBPData] a"
                                    + " FULL OUTER JOIN[dbo].[CustomersData] b ON a.[OBP_ID] = b.[FK_ObpID]"
                                    + " WHERE b.[FK_ObpID] IS NULL"
                                    + " AND a.[OBP_ID] IS NOT NULL"
                                    + " AND a.[OBP_DelMark] = 0"
                                    + " AND YEAR(a.[OBP_JoinDate]) = " + yearData + " AND MONTH(a.[OBP_JoinDate]) = '" + Convert.ToInt32(ddrMonth.SelectedValue) + "'";
                    }
                    else
                    {
                        strQuery = @"SELECT DISTINCT
                                    a.[OBP_ID],
                                    a.[OBP_ApplicantName],
                                    CONVERT(VARCHAR(20), a.[OBP_JoinDate], 103) as joinDate,
                                    a.[OBP_MobileNo],
                                    a.[OBP_StatusFlag],
                                    a.[OBP_DH_Name] as dh,
                                    (SELECT COUNT([FL_ID]) FROM [dbo].[FollowupOBP] WHERE [FK_OBPManID] = " + Session["adminObpManager"] + " AND [FK_OBPId] = a.[OBP_ID]) as flCount,"
                                        + "ISNULL((SELECT [DistHdMobileNo] FROM [dbo].[DistrictHead] WHERE [DistHdUserId] = a.[OBP_DH_UserId]), '-') as dhContact,"
                                        + "(SELECT COUNT([CustomrtID]) FROM [dbo].[CustomersData] WHERE [delMark] = 0 AND [CustomerActive] = 1 AND [FK_ObpID] = a.[OBP_ID]) as custCount"
                                    + " FROM [dbo].[OBPData] a"
                                    + " FULL OUTER JOIN [dbo].[CustomersData] b ON a.[OBP_ID] = b.[FK_ObpID]"
                                    + " WHERE b.[FK_ObpID] IS NULL"
                                    + " AND a.[OBP_ID] IS NOT NULL"
                                    + " AND YEAR(a.[OBP_JoinDate]) = YEAR(GETDATE()) AND MONTH(a.[OBP_JoinDate]) = MONTH(GETDATE())"
                                    + " AND a.[OBP_DelMark] = 0";
                    }
                }
            }
            
            using (DataTable dtFrEnq = c.GetDataTable(strQuery))
            {
                gvGOBP.DataSource = dtFrEnq;
                gvGOBP.DataBind();
                if (dtFrEnq.Rows.Count > 0)
                {
                    gvGOBP.UseAccessibleHeader = true;
                    gvGOBP.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FillGrid", ex.Message.ToString());
            return;
        }
    }

    protected void gvGOBP_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litView = (Literal)e.Row.FindControl("litView");

                litView.Text = "<a href=\"registered-gobp.aspx?id=" + e.Row.Cells[0].Text + "\" class=\"gView\" ></a>";

            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Error Occoured while processing', type: 'error'});", true);
            c.ErrorLogHandler(this.ToString(), "gvGOBP_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    private void GetGOBPEnqData(int enqIdX)
    {
        try
        {
            using (DataTable dtEnq = c.GetDataTable("Select * From OBPData Where OBP_ID=" + enqIdX))
            {
                if (dtEnq.Rows.Count > 0)
                {
                    lblId.Text = enqIdX.ToString();
                    DataRow row = dtEnq.Rows[0];

                    if (row["OBP_JoinDate"] != DBNull.Value && row["OBP_JoinDate"] != "" && row["OBP_JoinDate"] != null)
                    {
                        enqData[1] = Convert.ToDateTime(row["OBP_JoinDate"]).ToString("dd/MM/yyyy");
                    }

                    enqData[2] = row["OBP_ApplicantName"] != DBNull.Value ? row["OBP_ApplicantName"].ToString() : "";


                    enqData[8] = row["OBP_UserID"] != DBNull.Value ? row["OBP_UserID"].ToString() : "";
                    enqData[4] = row["OBP_DH_Name"] != DBNull.Value ? row["OBP_DH_Name"].ToString() : "";
                    object mobNo = c.GetReqData("DistrictHead", "DistHdMobileNo", "DistHdUserId='" + row["OBP_DH_UserId"] + "'");
                    if (mobNo != DBNull.Value && mobNo != null && mobNo.ToString() != "")
                    {
                        enqData[5] = mobNo.ToString();
                    }
                    enqData[12] = row["OBP_MobileNo"] != DBNull.Value ? row["OBP_MobileNo"].ToString() : "";
                    enqData[13] = row["OBP_EmailId"] != DBNull.Value ? row["OBP_EmailId"].ToString() : "";
                    string obpType = "";
                    switch (Convert.ToInt32(row["OBP_FKTypeID"]))
                    {
                        case 1: obpType = "30K"; break;
                        case 2: obpType = "60K"; break;
                        case 3: obpType = "1Lac"; break;
                    }
                    enqData[0] = obpType;


                    int custCoumt = Convert.ToInt32(c.returnAggregate("Select Count(CustomrtID) From CustomersData Where delMark=0 AND CustomerActive=1 AND FK_ObpID=" + enqIdX));

                    enqData[3] = "<a href=\"gobp-customers.aspx?gobpId=" + enqIdX + "\" class=\"text-dark text-bold text-md\" target=\"_blank\">" + custCoumt + "</a>";
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Error Occoured while processing', type: 'error'});", true);
            c.ErrorLogHandler(this.ToString(), "GetFrEnqData", ex.Message.ToString());
            return;
        }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        if (ddrMonth.SelectedIndex == 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Select Month & Year to fetch data', type: 'warning'});", true);
            return;
        }
        FillGrid();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("registered-gobp.aspx");
    }


    private void GetFollowupHistory()
    {
        try
        {
            int gobpIdX = Convert.ToInt32(Request.QueryString["id"]);
            using (DataTable dtFlHistory = c.GetDataTable("Select * From FollowupOBP Where FK_OBPId=" + gobpIdX + " Order By FL_ID DESC"))
            {
                if (dtFlHistory.Rows.Count > 0)
                {
                    StringBuilder strMarkup = new StringBuilder();
                    foreach (DataRow row in dtFlHistory.Rows)
                    {
                        strMarkup.Append("<div class=\"user-block\">");
                        strMarkup.Append("<span class=\"username\">");
                        string flBy = c.GetReqData("OBPManager", "OBPManName", "OBPManID=" + row["FK_OBPManID"]).ToString();
                        strMarkup.Append("<a href=\"#\">" + flBy + "</a>");
                        strMarkup.Append("</span>");
                        strMarkup.Append("<span class=\"description\">Follow Up on - " + Convert.ToDateTime(row["FL_Date"]).ToString("dd MMM yyyy hh:mm tt") + "</span>");
                        strMarkup.Append("</div>");
                        //strMarkup.Append("<h6 class=\"text-indigo\">" + row["FlupRemarkStatus"].ToString() + "</h6>");
                        strMarkup.Append("<p class=\"text-bold\">" + row["FL_Remark"].ToString() + "</p>");
                        //strMarkup.Append("<i class=\"nav-icon fas fa-clock\"></i><span>Next Follow Up: " + Convert.ToDateTime(row["FlupNextDate"]).ToString("dd MMM yyyy") + ", Time: " + row["FlupNextTime"].ToString() + "</span>");
                        strMarkup.Append("<hr />");
                    }

                    followupHistory = strMarkup.ToString();
                }
                else
                {
                    followupHistory = "<span class=\"text-orange text-bold\">No Followup History Found</span>";
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetFollowupHistory", ex.Message.ToString());
            return;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            txtRemark.Text = txtRemark.Text.Trim().Replace("'", "");

            if (txtRemark.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Remark');", true);
                return;
            }

            int maxId = c.NextId("FollowupOBP", "FL_ID");
            int gobpIdX = Convert.ToInt32(Request.QueryString["id"]);
            c.ExecuteQuery("Insert Into FollowupOBP (FL_ID, FL_Date, FK_OBPManID, FK_OBPId, FL_Remark) Values (" + maxId + ", '" + DateTime.Now +
                "', " + Session["adminObpManager"] + ", " + gobpIdX + ", '" + txtRemark.Text + "')");

            txtRemark.Text = "";

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Followup Saved');", true);

            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('registered-gobp.aspx?id=" + Request.QueryString["id"] + "', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSave_Click", ex.Message.ToString());
            return;
        }
    }
}