using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Web.UI.DataVisualization.Charting;


public partial class bdm_callers_chart : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        

        if (!IsPostBack)
        {
            c.FillComboBox("TeamPersonName", "TeamID", "SupportTeam", "TeamAuthority=2 AND TeamTaskID IN (1,3)", "TeamPersonName", 0, ddrCallers);
            ShowPerformanceChart("");
        }
    }

    protected void ShowPerformanceChart(string whereCond)
    {
        try
        {
            iClass c = new iClass();
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            string[] arrFYear = c.GetFinancialYear().ToString().Split('#');
            startDate = Convert.ToDateTime(arrFYear[0]);
            endDate = Convert.ToDateTime(arrFYear[1]);
            string sqlQuery = "";

            if (whereCond == "")
                sqlQuery = "Select DATENAME(MONTH, FlupDate) + ' ' + DATENAME(YEAR, FlupDate) as comMonth, MONTH(FlupDate) cMonth, COUNT(FlupID) as comAmt  From FollowupOrders Where (CONVERT(varchar(20), FlupDate, 112) >= CONVERT(varchar(20), CAST('" + startDate + "' as DateTime), 112) And CONVERT(varchar(20), FlupDate, 112) <= CONVERT(varchar(20), CAST('" + endDate + "' as DateTime), 112)) Group By DATENAME(MONTH, FlupDate) + ' ' + DATENAME(YEAR, FlupDate), MONTH(FlupDate), DATENAME(YEAR, FlupDate) Order By DATENAME(YEAR, FlupDate), MONTH(FlupDate)";
            else
                sqlQuery = "Select DATENAME(MONTH, FlupDate) + ' ' + DATENAME(YEAR, FlupDate) as comMonth, MONTH(FlupDate) cMonth, COUNT(FlupID) as comAmt  From FollowupOrders Where (CONVERT(varchar(20), FlupDate, 112) >= CONVERT(varchar(20), CAST('" + startDate + "' as DateTime), 112) And CONVERT(varchar(20), FlupDate, 112) <= CONVERT(varchar(20), CAST('" + endDate + "' as DateTime), 112)) AND " + whereCond + " Group By DATENAME(MONTH, FlupDate) + ' ' + DATENAME(YEAR, FlupDate), MONTH(FlupDate), DATENAME(YEAR, FlupDate) Order By DATENAME(YEAR, FlupDate), MONTH(FlupDate)";

            DataTable dtChart = c.GetDataTable(sqlQuery);
            chartPerform.DataSource = dtChart;
            chartPerform.Series[0].ChartType = SeriesChartType.Column;
            chartPerform.Legends[0].Enabled = true;
            chartPerform.Series[0].XValueMember = "comMonth";
            chartPerform.Series[0].YValueMembers = "comAmt";
            chartPerform.DataBind();
        }
        catch(Exception ex)
        {
            return;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string whereCondStr = "";
            if (rdbConverted.Checked == true)
            {
                whereCondStr = "FlupRemarkStatusID IN (3, 7, 8)";

            }

            if (ddrCallers.SelectedIndex > 0)
            {
                whereCondStr = whereCondStr != "" ? whereCondStr + " AND FK_TeamMemberId = " + ddrCallers.SelectedValue.ToString() : "FK_TeamMemberId = " + ddrCallers.SelectedValue.ToString();
            }

            ShowPerformanceChart(whereCondStr);
        }
        catch (Exception ex)
        {
        }
    }
}