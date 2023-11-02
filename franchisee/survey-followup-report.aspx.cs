using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

public partial class franchisee_survey_followup_report : System.Web.UI.Page
{
    iClass c = new iClass();
    public string gvCount1, gvCount2, convertCount, nonConvertCount, totalSurveys;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillGrid();
        }
    }

    private void FillGrid()
    {
        try
        {
            

            int genShopId = 0;
            string shopCode = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + Session["adminFranchisee"]).ToString();
            using (DataTable dtId = GetDataTable("Select frId From FranchiseeData Where frShopCode='" + shopCode + "' AND delMark=0 ANd frStatus=1 AND isClosed=0 AND legalBlock=0"))
            {
                if (dtId.Rows.Count > 0)
                {
                    DataRow frRow = dtId.Rows[0];
                    genShopId = Convert.ToInt32(frRow["frId"]);
                }
            }

            totalSurveys = GetReqData("MedicineSurvey a Inner Join CustomersSurvey b On a.customerId=b.CustomerId", "Count(a.surveyId)", "a.franchId=" + genShopId).ToString();
            convertCount = GetReqData("MedicineSurvey", "Count(surveyId)", "franchId=" + genShopId + " AND convertType=1").ToString();
            nonConvertCount = GetReqData("MedicineSurvey", "Count(surveyId)", "franchId=" + genShopId + " AND convertType=2").ToString();

            if (genShopId > 0)
            {

                using (DataTable dtCust = GetDataTable("Select Distinct a.CustomerId, a.CustomerName, a.CustomerMobile, 0 as sortCol, " +
                        " (Select Count(s.surveyId) From MedicineSurvey s Inner Join CustomersSurvey c On s.CustomerId=c.customerId Where s.customerId=a.CustomerId AND s.convertType=1 AND s.franchId=" + genShopId + ") as totalMonthlySurvey, " +
                        " isnull((Select TOP 1 CONVERT(varchar(20), s.reOrdDate, 103) From MedicineSurvey s Inner Join CustomersSurvey c On s.CustomerId=c.customerId Where s.customerId=a.CustomerId AND s.convertType=1 AND s.franchId=" + genShopId + " Order By s.surveyDate DESC), '-') as reOrdDate, " +
                        " isnull((Select TOP 1 CONVERT(varchar(20), s.surveyId, 103) From MedicineSurvey s Inner Join CustomersSurvey c On s.CustomerId=c.customerId Where s.customerId=a.CustomerId AND s.convertType=1 AND s.franchId=" + genShopId + " Order By s.surveyDate DESC), '-') as surveyId " +
                        " From CustomersSurvey a Inner Join MedicineSurvey b On a.CustomerId=b.customerId " +
                        " Where b.convertType=1 AND b.franchId=" + genShopId + " AND (DAY(reOrdDate)=Day(GETDATE()) OR reOrdDay=Day(GETDATE())) " +
                        " Order By totalMonthlySurvey DESC"))
                {
                    gvSurvey.DataSource = dtCust;
                    gvSurvey.DataBind();

                    gvCount2 = dtCust.Rows.Count.ToString();
                    gvCount1 = c.returnAggregate("Select Count(Distinct a.CustomrtID) From CustomersData a Inner Join OrdersData b On a.CustomrtID=b.FK_OrderCustomerID Inner Join OrdersAssign c On b.OrderID=c.FK_OrderID Where b.MreqFlag=1 AND b.OrderStatus=7 AND c.Fk_FranchID=" + Session["adminFranchisee"] + " AND c.OrdAssignStatus=7 AND DAY(b.OrderDate)=DAY(GETDATE())").ToString();

                    if (gvSurvey.Rows.Count > 0)
                    {
                        gvSurvey.UseAccessibleHeader = true;
                        gvSurvey.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Something Went Wrong');", true);
                return;
            }
        }
        catch (Exception ex)
        {

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FillGrid", ex.Message.ToString());
            return;
        }
    }

    protected void gvSurvey_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litDays = (Literal)e.Row.FindControl("litDays");

                if (e.Row.Cells[4].Text != "-") 
                {
                    string[] arrOrdDate = e.Row.Cells[4].Text.ToString().Split('/');
                    DateTime ordDate = Convert.ToDateTime(arrOrdDate[1] + "/" + arrOrdDate[0] + "/" + arrOrdDate[2]);
                    litDays.Text = c.GetTimeSpan(ordDate);
                }
                else
                {
                    litDays.Text = "NA";
                }

                //e.Row.Cells[4].Text = "<a href=\"order-details.aspx?id=" + orderId + "\" class=\"\" target=\"_blank\">" + e.Row.Cells[4].Text + "</a>";

                //Feedback / Follow Up Anchor
                Literal litAnchor = (Literal)e.Row.FindControl("litAnch");
                string orderId = e.Row.Cells[7].Text;
                // Get total count of follou upd done before
                string litText = "";
                if (c.IsRecordExist("Select FollowupID From MonthlyOrderFollowUp Where FK_OrderID=" + orderId + " AND FK_FranchiseeID=" + Session["adminFranchisee"] + " AND FollowupCategory=2"))
                {
                    int followupType = Convert.ToInt32(c.GetReqData("MonthlyOrderFollowUp", "FollowupType", "FK_OrderID=" + orderId + " AND FK_FranchiseeID=" + Session["adminFranchisee"] + " AND FollowupCategory=2"));
                    switch (followupType)
                    {
                        case 1: litText = "Wrongly select as monthly order"; e.Row.Cells[8].Text += "3"; break;
                        case 2: litText = "Not interested for monthly order"; e.Row.Cells[8].Text += "2"; break;
                        case 3: litText = "Order Done, Call back in next month"; e.Row.Cells[8].Text += "4"; e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#f2d9d9"); break;
                        //case 4: litText = "Phone not picked"; e.Row.Cells[8].Text += "1"; break;
                        case 4: litText = "Followup Again"; e.Row.Cells[8].Text += "1"; break;
                    }
                }
                else
                {
                    litText = "Follow Up (0)";
                }

                //litAnchor.Text = "<a href=\"#\" class=\"gFeedback\" data-toggle=\"modal\" data-target=\"#modal-lg\" data-whatever=\"" + orderId + "\">Follow Up (" + c.returnAggregate("Select count(FollowupId) From MonthlyOrderFollowUp Where FK_OrderID=" + orderId ).ToString() + ")</a>";
                litAnchor.Text = "<a href=\"#\" class=\"gFeedback\" data-toggle=\"modal\" data-target=\"#modal-lg\" data-whatever=\"" + orderId + "\">" + litText.ToString() + "</a>";
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvSurvey_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static Boolean SurveyFollowUp(int OrderIdRef, int OptionSelected)
    {
        iClass c = new iClass();
        int maxId = c.NextId("MonthlyOrderFollowUp", "FollowupID");
        
        //FollowupCategory=1 (Orders) / FollowupCategory=2 (survey)
        c.ExecuteQuery("Insert Into MonthlyOrderFollowUp(FollowupID, FollowupDate, FK_FranchiseeID, FK_OrderID, FollowupType, FollowupCategory) Values(" + maxId + ", '" + DateTime.Now + "', " + Convert.ToInt32(HttpContext.Current.Session["adminFranchisee"]) + ", " + OrderIdRef + ", " + OptionSelected + ", 2) ");
       
        return true;
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
}