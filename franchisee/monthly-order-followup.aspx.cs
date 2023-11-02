using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class franchisee_monthly_order_followup : System.Web.UI.Page
{
    iClass c = new iClass();
    public string gvCount1, gvCount2;
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

            using(DataTable dtCust = c.GetDataTable("Select Distinct a.CustomrtID, a.CustomerName, a.CustomerMobile, 0 as sortCol," +
                    " (Select Count(o.OrderID) From OrdersData o Inner Join OrdersAssign oa On o.OrderID=oa.FK_OrderID Where o.FK_OrderCustomerID=a.CustomrtID AND o.MreqFlag=1 AND o.OrderStatus=7 AND oa.Fk_FranchID=" + Session["adminFranchisee"] + ") as totalMonthlyOrders, " +
                    //" (Select TOP 1 Convert(varchar(20), o.OrderID) + ' - ' + CONVERT(varchar(20), o.OrderDate, 103) From OrdersData o Inner Join OrdersAssign oa On o.OrderID=oa.FK_OrderID Where o.FK_OrderCustomerID=a.CustomrtID AND o.MreqFlag=1 AND o.OrderStatus=7 AND oa.Fk_FranchID=" + Session["adminFranchisee"] + " Order By o.OrderDate DESC) as recentOrderId, " +
                    " (Select TOP 1 o.OrderID From OrdersData o Inner Join OrdersAssign oa On o.OrderID=oa.FK_OrderID Where o.FK_OrderCustomerID=a.CustomrtID AND o.MreqFlag=1 AND o.OrderStatus=7 AND oa.Fk_FranchID=" + Session["adminFranchisee"] + " Order By o.OrderDate DESC) as recentOrderId, " +
                    " (Select TOP 1 CONVERT(varchar(20), o.OrderDate, 103) From OrdersData o Inner Join OrdersAssign oa On o.OrderID=oa.FK_OrderID Where o.FK_OrderCustomerID=a.CustomrtID AND o.MreqFlag=1 AND o.OrderStatus=7 AND oa.Fk_FranchID=" + Session["adminFranchisee"] + " Order By o.OrderDate DESC) as recentOrderDate " +
                    " From CustomersData a Inner Join OrdersData b On a.CustomrtID=b.FK_OrderCustomerID Inner Join OrdersAssign c On b.OrderID=c.FK_OrderID " +
                    " Where b.MreqFlag=1 AND b.OrderStatus=7 AND c.Fk_FranchID=" + Session["adminFranchisee"] +
                    " AND c.OrdAssignStatus=7 AND DAY(b.OrderDate)=DAY(GETDATE()) Order By totalMonthlyOrders DESC"))
            {
                gvOrder.DataSource = dtCust;
                gvOrder.DataBind();

                gvCount1 = dtCust.Rows.Count.ToString();
                gvCount2 = returnAggregate("Select Count(Distinct a.CustomerId) From CustomersSurvey a Inner Join MedicineSurvey b On a.CustomerId=b.customerId Where b.convertType=1 AND b.franchId=" + genShopId + " AND (DAY(reOrdDate)=Day(GETDATE()) OR reOrdDay=Day(GETDATE()) )").ToString();

                if (gvOrder.Rows.Count > 0)
                {
                    gvOrder.UseAccessibleHeader = true;
                    gvOrder.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvOrder_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litDays = (Literal)e.Row.FindControl("litDays");

                //string[] arrOrder = e.Row.Cells[4].Text.Split('-');

                //string orderId = arrOrder[0].ToString();
                //string orderDate = arrOrder[1].ToString();

                string orderId = e.Row.Cells[4].Text.ToString();
                string orderDate = e.Row.Cells[5].Text.ToString();

                string[] arrOrdDate = orderDate.ToString().Split('/');
                DateTime ordDate = Convert.ToDateTime(arrOrdDate[1] + "/" + arrOrdDate[0] + "/" + arrOrdDate[2]);
                litDays.Text = c.GetTimeSpan(ordDate);

                e.Row.Cells[4].Text = "<a href=\"order-details.aspx?id=" + orderId + "\" class=\"\" target=\"_blank\">" + e.Row.Cells[4].Text + "</a>";

                //Feedback / Follow Up Anchor
                Literal litAnchor = (Literal)e.Row.FindControl("litAnch");
                // Get total count of follou upd done before
                string litText = "";
                if (c.IsRecordExist("Select FollowupID From MonthlyOrderFollowUp Where FK_OrderID=" + orderId + " AND FK_FranchiseeID=" + Session["adminFranchisee"] + " AND FollowupCategory=1"))
                {
                    int followupType = Convert.ToInt32(c.GetReqData("MonthlyOrderFollowUp", "FollowupType", "FK_OrderID=" + orderId + " AND FK_FranchiseeID=" + Session["adminFranchisee"] + " AND FollowupCategory=1"));
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
            c.ErrorLogHandler(this.ToString(), "gvOrder_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static Boolean OrderFollowUp(int OrderIdRef, int OptionSelected)
    {
        iClass c = new iClass();
        int maxId = c.NextId("MonthlyOrderFollowUp", "FollowupID");
        //FollowupCategory=1 (Orders) / FollowupCategory=2 (survey)

        c.ExecuteQuery("Insert Into MonthlyOrderFollowUp(FollowupID, FollowupDate, FK_FranchiseeID, FK_OrderID, FollowupType, " +
            " FollowupCategory) Values(" + maxId + ", '" + DateTime.Now + "', " +
            Convert.ToInt32(HttpContext.Current.Session["adminFranchisee"]) + ", " + OrderIdRef + ", " + OptionSelected +
            ", 1) ");

        if (OptionSelected == 1 || OptionSelected == 2)
        {
            c.ExecuteQuery("Update OrdersData Set MreqFlag=0 Where OrderID=" + OrderIdRef);
        }
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

    public long returnAggregate(string strQuery)
    {
        try
        {
            long rValue = 0;
            SqlConnection con = new SqlConnection(OpenConnection1());
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