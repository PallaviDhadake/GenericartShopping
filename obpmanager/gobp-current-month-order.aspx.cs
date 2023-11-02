using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class obpmanager_gobp_current_month_order : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillDDR();
        }
        if (Request.QueryString["type"] != null)
        {            
            Bind_GOBP_Order();
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

    private void Bind_GOBP_Order()
    {
        try
        {
            string strQuery = "";
            string[] arrYear = ddrMonth.SelectedItem.Text.ToString().Split('-');
            string yearData = arrYear[1].ToString();

            if (Request.QueryString["type"] != null)
            {
                if (Request.QueryString["type"] == "active")
                {
                    if (Convert.ToInt32(ddrMonth.SelectedValue) != 0)
                    {
                        strQuery = @"SELECT DISTINCT 
                                    	MAX(OP.[OBP_ID]) AS GOBPID,
                                    	MAX(OP.[OBP_ApplicantName]) AS Name,
                                    	MAX(OP.[OBP_MobileNo]) AS MobileNo,
                                    	(SELECT COUNT(DISTINCT [CustomrtID]) FROM [dbo].[CustomersData] WHERE [FK_ObpID] = MAX(OP.[OBP_ID])) as Customers,
                                    	(SELECT COUNT([OrderID]) FROM [dbo].[OrdersData] WHERE [GOBPId] = MAX(OP.[OBP_ID])) AS MonthOrder,
                                    	ISNULL((SELECT SUM([OrderAmount]) FROM [dbo].[OrdersData] WHERE [GOBPId] = MAX(OP.[OBP_ID])),0) AS MonthAmount
                                    	
                                    FROM [dbo].[OBPData] as OP
                                    LEFT JOIN [dbo].[OrdersData] AS OD ON OP.[OBP_ID] = OD.[GOBPId]
                                    
                                    WHERE (OD.[GOBPId] IS NOT NULL AND OD.[GOBPId] > 0)
                                    AND YEAR(OP.[OBP_JoinDate]) = " + yearData + " AND MONTH(OP.[OBP_JoinDate]) = '" + Convert.ToInt32(ddrMonth.SelectedValue) + "'"

                                    + " GROUP BY OP.[OBP_ApplicantName], OP.[OBP_ID], OP.[OBP_MobileNo]";
                    }
                    else
                    {
                        strQuery = @"SELECT DISTINCT
                                        MAX(OP.[OBP_ID]) AS GOBPID,
                                        MAX(OP.[OBP_ApplicantName]) AS Name,
                                        MAX(OP.[OBP_MobileNo]) AS MobileNo,
                                        (SELECT COUNT(DISTINCT [CustomrtID]) FROM [dbo].[CustomersData] WHERE [FK_ObpID] = MAX(OP.[OBP_ID])) as Customers,
                                        (SELECT COUNT([OrderID]) FROM [dbo].[OrdersData] WHERE [GOBPId] = MAX(OP.[OBP_ID])) AS MonthOrder,
                                        ISNULL((SELECT SUM([OrderAmount]) FROM [dbo].[OrdersData] WHERE [GOBPId] = MAX(OP.[OBP_ID])),0) AS MonthAmount

                                    FROM [dbo].[OBPData] as OP
                                    LEFT JOIN [dbo].[OrdersData] AS OD ON OP.[OBP_ID] = OD.[GOBPId]

                                    WHERE (OD.[GOBPId] IS NOT NULL AND OD.[GOBPId] > 0)
                                    AND YEAR(OP.[OBP_JoinDate]) = YEAR(GETDATE()) AND MONTH(OP.[OBP_JoinDate]) = MONTH(GETDATE())

                                    GROUP BY OP.[OBP_ApplicantName], OP.[OBP_ID], OP.[OBP_MobileNo]";
                    }
                }

                if (Request.QueryString["type"] == "inactive")
                {
                    if (Convert.ToInt32(ddrMonth.SelectedValue) != 0)
                    {
                        strQuery = @"SELECT DISTINCT
                                        MAX(OP.[OBP_ID]) AS GOBPID,
                                        MAX(OP.[OBP_ApplicantName]) AS Name,
                                        MAX(OP.[OBP_MobileNo]) AS MobileNo,
                                        (SELECT COUNT(DISTINCT [CustomrtID]) FROM [dbo].[CustomersData] WHERE [FK_ObpID] = MAX(OP.[OBP_ID])) as Customers,
                                        (SELECT COUNT([OrderID]) FROM [dbo].[OrdersData] WHERE [GOBPId] = MAX(OP.[OBP_ID])) AS MonthOrder,
                                        ISNULL((SELECT SUM([OrderAmount]) FROM [dbo].[OrdersData] WHERE [GOBPId] = MAX(OP.[OBP_ID])),0) AS MonthAmount
                
                                    FROM [dbo].[OBPData] as OP
                                    FULL OUTER JOIN [dbo].[OrdersData] AS OD ON OP.[OBP_ID] = OD.[GOBPId]

                                    WHERE (OD.[GOBPId] IS NOT NULL AND OD.[GOBPId] > 0)
                                    AND YEAR(OP.[OBP_JoinDate]) = " + yearData + " AND MONTH(OP.[OBP_JoinDate]) = '" + Convert.ToInt32(ddrMonth.SelectedValue) + "'"

                                    + " GROUP BY OP.[OBP_ApplicantName], OP.[OBP_ID], OP.[OBP_MobileNo]";
                    }
                    else
                    {
                        strQuery = @"SELECT DISTINCT 
                                       MAX(OP.[OBP_ID]) AS GOBPID,
                                       MAX(OP.[OBP_ApplicantName]) AS Name,
                                       MAX(OP.[OBP_MobileNo]) AS MobileNo,
                                       (SELECT COUNT(DISTINCT [CustomrtID]) FROM [dbo].[CustomersData] WHERE [FK_ObpID] = MAX(OP.[OBP_ID])) as Customers,
                                       (SELECT COUNT([OrderID]) FROM [dbo].[OrdersData] WHERE [GOBPId] = MAX(OP.[OBP_ID])) AS MonthOrder,
                                       ISNULL((SELECT SUM([OrderAmount]) FROM [dbo].[OrdersData] WHERE [GOBPId] = MAX(OP.[OBP_ID])),0) AS MonthAmount
                                    	
                                    FROM [dbo].[OBPData] as OP
                                    FULL OUTER JOIN [dbo].[OrdersData] AS OD ON OP.[OBP_ID] = OD.[GOBPId]
                                    
                                    WHERE OD.[GOBPId] IS NULL AND OP.[OBP_ID] IS NOT NULL
                                    AND YEAR(OP.[OBP_JoinDate]) = YEAR(GETDATE())
                                    AND MONTH(OP.[OBP_JoinDate]) = MONTH(GETDATE())
                                    
                                    GROUP BY OP.[OBP_ApplicantName], OP.[OBP_ID], OP.[OBP_MobileNo]";
                    }
                }
            }
            using (DataTable dtgobpOrd = c.GetDataTable(strQuery))
            {
                gvGOBP.DataSource = dtgobpOrd;
                gvGOBP.DataBind();
                if (dtgobpOrd.Rows.Count > 0)
                {
                    gvGOBP.UseAccessibleHeader = true;
                    gvGOBP.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "Bind_GOBP_Order", ex.Message.ToString());
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
        Bind_GOBP_Order();
    }
}