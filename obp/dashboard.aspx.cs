using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;

public partial class obp_dashboard : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] arrCounts = new string[10];
    GobpInfo gInfo = new GobpInfo();
    protected void Page_Load(object sender, EventArgs e)
    {
        GetCount();
        ShowChart();

        // Check Document completion status
        gInfo.OBPData(Convert.ToInt32(Session["adminObp"]));
       

        if (gInfo.AddressProof1 == "" || gInfo.AddressProof2 == "" || gInfo.IdProof1 == "")
        {
            string script = "docReminderPopup();"; // The JavaScript function call
            ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", script, true);
        }

    }

    private void GetCount()
    {
        try
        {
            arrCounts[0] = c.returnAggregate("Select Count(CustomrtID) From CustomersData Where delMark=0 And FK_ObpID=" + Session["adminObp"] + " And CustomerActive=1").ToString();
            //arrCounts[1] = c.returnAggregate("Select Count(CustomrtID) From CustomersData Where delMark=0 And FK_GenMitraID=" + Session["adminGenMitra"] + " And CustomerActive=1 And CustomerFavShop Is Not Null").ToString();
            arrCounts[1] = c.returnAggregate("Select Count(distinct CustomerFavShop) From CustomersData Where FK_ObpID=" + Session["adminObp"]).ToString();
            arrCounts[2] = c.returnAggregate("Select Sum(OrderAmount) From OrdersData Where FK_OrderCustomerID  In (Select CustomrtID From CustomersData Where FK_ObpID=" + Session["adminObp"] + ") AND OrderStatus IN (1, 3, 5, 6, 7)").ToString();

            //double comissionAmt = (Convert.ToDouble(arrCounts[2]) * 10) / 100;
            //arrCounts[3] = comissionAmt.ToString("0.00");

            CalculateComission();
            
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetCount", ex.Message.ToString());
            return;
        }
    }

    private void CalculateComission()
    {
        try
        {
            ////Generic Products
            //string GenericProdAmount = c.returnAggregate("Select SUM(a.OrdDetailAmount) From OrdersDetails a Inner Join ProductsData b " +
            //    " On a.FK_DetailProductID=b.ProductID Inner Join OrdersData c On a.FK_DetailOrderID=c.OrderID Where " +
            //    " c.OrderID IN (Select OrderID From OrdersData Where FK_OrderCustomerID In " +
            //    " (Select CustomrtID From CustomersData Where FK_ObpID=" + Session["adminObp"] + ") AND OrderStatus IN (1, 3, 5, 6, 7)) " +
            //    " AND b.FK_CategoryID=2").ToString();


            ////Surgical Products
            //string SurgicalProdAmount = c.returnAggregate("Select SUM(a.OrdDetailAmount) From OrdersDetails a Inner Join ProductsData b " +
            //    " On a.FK_DetailProductID=b.ProductID Inner Join OrdersData c On a.FK_DetailOrderID=c.OrderID Where " +
            //    " c.OrderID IN (Select OrderID From OrdersData Where FK_OrderCustomerID In " +
            //    " (Select CustomrtID From CustomersData Where FK_ObpID=" + Session["adminObp"] + ") AND OrderStatus IN (1, 3, 5, 6, 7)) " +
            //    " AND b.FK_CategoryID=12").ToString();

            ////Ayurvedic Products
            //string AyurvedicProdAmount = c.returnAggregate("Select SUM(a.OrdDetailAmount) From OrdersDetails a Inner Join ProductsData b " +
            //    " On a.FK_DetailProductID=b.ProductID Inner Join OrdersData c On a.FK_DetailOrderID=c.OrderID Where " +
            //    " c.OrderID IN (Select OrderID From OrdersData Where FK_OrderCustomerID In " +
            //    " (Select CustomrtID From CustomersData Where FK_ObpID=" + Session["adminObp"] + ") AND OrderStatus IN (1, 3, 5, 6, 7)) " +
            //    " AND b.FK_CategoryID=10").ToString();

            ////Cosmetic Products
            //string CosmeticProdAmount = c.returnAggregate("Select SUM(a.OrdDetailAmount) From OrdersDetails a Inner Join ProductsData b " +
            //    " On a.FK_DetailProductID=b.ProductID Inner Join OrdersData c On a.FK_DetailOrderID=c.OrderID Where " +
            //    " c.OrderID IN (Select OrderID From OrdersData Where FK_OrderCustomerID In " +
            //    " (Select CustomrtID From CustomersData Where FK_ObpID=" + Session["adminObp"] + ") AND OrderStatus IN (1, 3, 5, 6, 7)) " +
            //    " AND b.FK_CategoryID=3").ToString();

            ////obp type = 1>30K, 2>60K, 3>1Lac
            //int obpType = Convert.ToInt32(c.GetReqData("OBPData", "OBP_FKTypeID", "OBP_ID=" + Session["adminObp"]));
            //int genProdPercentage = 0;

            //switch (obpType)
            //{
            //    case 1: genProdPercentage = 20; break;
            //    case 2: genProdPercentage = 25; break;
            //    case 3: genProdPercentage = 30; break;
            //}


            //double finalGenProdCommission = (Convert.ToDouble(GenericProdAmount) * genProdPercentage) / 100;
            //double finalSurgicalProdCommission = (Convert.ToDouble(SurgicalProdAmount) * 15) / 100;
            //double finalAyurvedicProdCommission = (Convert.ToDouble(AyurvedicProdAmount) * 10) / 100;
            //double finalCosmeticProdCommission = (Convert.ToDouble(CosmeticProdAmount) * 10) / 100;

            //arrCounts[3] = (finalGenProdCommission + finalSurgicalProdCommission + finalAyurvedicProdCommission + finalCosmeticProdCommission).ToString();
            arrCounts[3] = c.returnAggregate("Select SUM(OBPComTotal) From OrdersData Where GOBPId=" + Session["adminObp"]).ToString();

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "CalculateComission", ex.Message.ToString());
            return;
        }
    }

    private void ShowChart()
    {
        try
        {
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            string[] arrFYear = c.GetFinancialYear().ToString().Split('#');
            startDate = Convert.ToDateTime(arrFYear[0]);
            endDate = Convert.ToDateTime(arrFYear[1]);

            string strQuery = "Select  DATENAME(MONTH, OrderDate) + ' ' + DATENAME(YEAR, OrderDate) as comMonth, MONTH(OrderDate) cMonth, SUM(OBPComTotal) as comAmt " +
                            " From OrdersData Where GOBPId=" + Session["adminObp"] + " And (CONVERT(varchar(20), OrderDate, 112) >= CONVERT(varchar(20), CAST('" + startDate + "' as DateTime), 112) " +
                            " And CONVERT(varchar(20), OrderDate, 112) <= CONVERT(varchar(20), CAST('" + endDate + "' as DateTime), 112)) " +
                            " Group By DATENAME(MONTH, OrderDate) + ' ' + DATENAME(YEAR, OrderDate), MONTH(OrderDate), DATENAME(YEAR, OrderDate) Order By DATENAME(YEAR, OrderDate), MONTH(OrderDate)";

            DataTable dtChart = c.GetDataTable(strQuery);
            chartPerform.DataSource = dtChart;
            chartPerform.Series[0].ChartType = SeriesChartType.Column;
            chartPerform.Legends[0].Enabled = true;
            chartPerform.Series[0].XValueMember = "comMonth";
            chartPerform.Series[0].YValueMembers = "comAmt";
            chartPerform.DataBind();

        }
        catch (Exception)
        {
            //errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}