using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class bdm_franchisee_daily_sales_report : System.Web.UI.Page
{
    iClass c = new iClass();
    public string repTitle, ordTotalComp, ordTotalOther;
    protected void Page_Load(object sender, EventArgs e)
    {
        FillGrid();
    }

    private void FillGrid()
    {
        try
        {
            string strQuery1 = "";
            string strQuery2 = "";
            if (txtFDate.Text != "" && txtToDate.Text != "")
            {
                // From Date
                DateTime fromDate = DateTime.Now;
                string[] arrFromDate = txtFDate.Text.Split('/');
                fromDate = Convert.ToDateTime(arrFromDate[1] + "/" + arrFromDate[0] + "/" + arrFromDate[2]);

                // To Date
                DateTime toDate = DateTime.Now;
                string[] arrToDate = txtToDate.Text.Split('/');
                toDate = Convert.ToDateTime(arrToDate[1] + "/" + arrToDate[0] + "/" + arrToDate[2]);

                string dateCondition = "( (CONVERT(varchar(20), a.OrdAssignDate, 112) >= CONVERT(varchar(20), CAST('" + fromDate + "' as DATETIME), 112) ) AND "
                    + " (CONVERT(varchar(20), a.OrdAssignDate, 112) <= CONVERT(varchar(20), CAST('" + toDate + "' as DATETIME), 112)))";


                repTitle = "Showing Report of Orders From " + fromDate.ToString("dd/MM/yyyy") + " to " + toDate.ToString("dd/MM/yyyy");

                // 5-Apr-2023 Removed a.OrderAssignStatus>=0 condition and replaced it with a.OrdAssignStatus Not In(2, 5) 

                //1. Company Own Shop Report
                strQuery1 = "Select Distinct a.Fk_FranchID, Count(Distinct a.FK_OrderID) as ordersCount, " +
                    " isnull( (Select SUM(OrdDetailAmount) From OrdersDetails Where FK_DetailOrderID IN (Select distinct FK_OrderID From OrdersAssign Where Fk_FranchID=a.Fk_FranchID AND OrdAssignStatus Not In(2, 5) AND OrdReAssign=0 AND ( (CONVERT(varchar(20), OrdAssignDate, 112) >= CONVERT(varchar(20), CAST('" + fromDate + "' as DATETIME), 112) ) AND "
                    + " (CONVERT(varchar(20), OrdAssignDate, 112) <= CONVERT(varchar(20), CAST('" + toDate + "' as DATETIME), 112))) )) , 0) as orderAmount," +
                    " b.FranchShopCode, b.FranchName, isnull(c.DistrictName, '-') as DistrictName, isnull(d.CityName, '-') as CityName " +
                    " From OrdersAssign a Inner Join FranchiseeData b On a.Fk_FranchID=b.FranchID " +
                    " Inner Join CompanyOwnShops e On a.Fk_FranchID=e.FK_FranchID Left Join DistrictsData c On b.FK_FranchDistId=c.DistrictId " +
                    " Left Join CityData d On b.FK_FranchCityId=d.CityID Where a.OrdAssignStatus Not In(2, 5) AND a.OrdReAssign=0 AND " + dateCondition +
                    " Group By a.Fk_FranchID, b.FranchShopCode, b.FranchName, c.DistrictName, d.CityName";

                //2. Other Shop Report
                strQuery2 = "Select Distinct a.Fk_FranchID, Count(Distinct a.FK_OrderID) as ordersCount, " +
                    " isnull( (Select SUM(OrdDetailAmount) From OrdersDetails Where FK_DetailOrderID IN (Select distinct FK_OrderID From OrdersAssign Where Fk_FranchID=a.Fk_FranchID AND OrdAssignStatus Not In(2, 5) AND OrdReAssign=0 AND ( (CONVERT(varchar(20), OrdAssignDate, 112) >= CONVERT(varchar(20), CAST('" + fromDate + "' as DATETIME), 112) ) AND "
                    + " (CONVERT(varchar(20), OrdAssignDate, 112) <= CONVERT(varchar(20), CAST('" + toDate + "' as DATETIME), 112))) )), 0) as orderAmount, " +
                    " b.FranchShopCode, b.FranchName, isnull(c.DistrictName, '-') as DistrictName, isnull(d.CityName, '-') as CityName " +
                    " From OrdersAssign a Inner Join FranchiseeData b On a.Fk_FranchID=b.FranchID Left Join DistrictsData c " +
                    " On b.FK_FranchDistId=c.DistrictId Left Join CityData d On b.FK_FranchCityId=d.CityID Where a.OrdAssignStatus Not In(2, 5)" +
                    " AND a.OrdReAssign=0 AND " + dateCondition +
                    " AND b.FranchID NOT IN (Select Fk_FranchID From CompanyOwnShops) " +
                    "Group By a.Fk_FranchID, b.FranchShopCode, b.FranchName, c.DistrictName, d.CityName";
            }
            else
            {
                repTitle = "Showing Report of " + DateTime.Now.ToString("dd MMM yyyy");
                //1. Company Own Shop Report
                strQuery1 = "Select Distinct a.Fk_FranchID, Count(Distinct a.FK_OrderID) as ordersCount, " +
                    " isnull( (Select SUM(OrdDetailAmount) From OrdersDetails Where FK_DetailOrderID IN (Select distinct FK_OrderID From OrdersAssign Where Fk_FranchID=a.Fk_FranchID AND OrdAssignStatus Not In(2, 5) AND OrdReAssign=0 AND CONVERT(varchar(20), OrdAssignDate, 112)=Convert(varchar(20), CAST('" + DateTime.Now + "' as datetime) ,112))), 0) as orderAmount," +
                    " b.FranchShopCode, b.FranchName, isnull(c.DistrictName, '-') as DistrictName, isnull(d.CityName, '-') as CityName " +
                    " From OrdersAssign a Inner Join FranchiseeData b On a.Fk_FranchID=b.FranchID " +
                    " Inner Join CompanyOwnShops e On a.Fk_FranchID=e.FK_FranchID Left Join DistrictsData c On b.FK_FranchDistId=c.DistrictId " +
                    " Left Join CityData d On b.FK_FranchCityId=d.CityID Where a.OrdAssignStatus Not In(2, 5) AND a.OrdReAssign=0 AND " +
                    " CONVERT(varchar(20), a.OrdAssignDate, 112)=Convert(varchar(20), CAST('" + DateTime.Now + "' as datetime) ,112) " +
                    " Group By a.Fk_FranchID, b.FranchShopCode, b.FranchName, c.DistrictName, d.CityName";

                //2. Other Shop Report
                strQuery2 = "Select Distinct a.Fk_FranchID, Count(Distinct a.FK_OrderID) as ordersCount, " +
                    " isnull( (Select SUM(OrdDetailAmount) From OrdersDetails Where FK_DetailOrderID IN (Select distinct FK_OrderID From OrdersAssign Where Fk_FranchID=a.Fk_FranchID AND OrdAssignStatus Not In(2, 5) AND OrdReAssign=0 AND CONVERT(varchar(20), OrdAssignDate, 112)=Convert(varchar(20), CAST('" + DateTime.Now + "' as datetime) ,112))), 0) as orderAmount, " +
                    " b.FranchShopCode, b.FranchName, isnull(c.DistrictName, '-') as DistrictName, isnull(d.CityName, '-') as CityName " +
                    " From OrdersAssign a Inner Join FranchiseeData b On a.Fk_FranchID=b.FranchID Left Join DistrictsData c " +
                    " On b.FK_FranchDistId=c.DistrictId Left Join CityData d On b.FK_FranchCityId=d.CityID Where a.OrdAssignStatus Not In(2, 5) " +
                    " AND a.OrdReAssign=0 AND CONVERT(varchar(20), a.OrdAssignDate, 112)=Convert(varchar(20), CAST('" + DateTime.Now + "' as datetime) ,112)  " +
                    " AND b.FranchID NOT IN (Select Fk_FranchID From CompanyOwnShops) " +
                    "Group By a.Fk_FranchID, b.FranchShopCode, b.FranchName, c.DistrictName, d.CityName";

            }

            //1. Company Own Shop Report

            using (DataTable dtCompany = c.GetDataTable(strQuery1))
            {
                gvCompanyShops.DataSource = dtCompany;
                gvCompanyShops.DataBind();

                if (gvCompanyShops.Rows.Count > 0)
                {
                    gvCompanyShops.UseAccessibleHeader = true;
                    gvCompanyShops.HeaderRow.TableSection = TableRowSection.TableHeader;

                    int totOrders = 0; double totAmount = 0.0;
                    foreach (DataRow row in dtCompany.Rows)
                    {
                        totOrders = totOrders + Convert.ToInt32(row["ordersCount"]);
                        totAmount = totAmount + Convert.ToDouble(row["orderAmount"]);
                    }

                    ordTotalComp = "Total Orders : " + totOrders.ToString() + " and Total Order Amount : " + totAmount.ToString("0.00");
                }
            }

            // Other Shop Report
            using (DataTable dtOther = c.GetDataTable(strQuery2))
            {
                gvOtherShops.DataSource = dtOther;
                gvOtherShops.DataBind();

                if (gvOtherShops.Rows.Count > 0)
                {
                    gvOtherShops.UseAccessibleHeader = true;
                    gvOtherShops.HeaderRow.TableSection = TableRowSection.TableHeader;

                    int totOrders = 0; double totAmount = 0.0;
                    foreach (DataRow row in dtOther.Rows)
                    {
                        totOrders = totOrders + Convert.ToInt32(row["ordersCount"]);
                        totAmount = totAmount + Convert.ToDouble(row["orderAmount"]);
                    }

                    ordTotalOther = "Total Orders : " + totOrders.ToString() + " and Total Order Amount : " + totAmount.ToString("0.00");
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

    protected void gvCompanyShops_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Literal litAnch = (Literal)e.Row.FindControl("litAnch");
            if (txtFDate.Text != "" && txtToDate.Text != "")
            {
                // From Date
                DateTime fromDate = DateTime.Now;
                string[] arrFromDate = txtFDate.Text.Split('/');
                fromDate = Convert.ToDateTime(arrFromDate[1] + "/" + arrFromDate[0] + "/" + arrFromDate[2]);

                // To Date
                DateTime toDate = DateTime.Now;
                string[] arrToDate = txtToDate.Text.Split('/');
                toDate = Convert.ToDateTime(arrToDate[1] + "/" + arrToDate[0] + "/" + arrToDate[2]);

                string dateCondition = "( (CONVERT(varchar(20), a.OrdAssignDate, 112) >= CONVERT(varchar(20), CAST('" + fromDate + "' as DATETIME), 112) ) AND "
                    + " (CONVERT(varchar(20), a.OrdAssignDate, 112) <= CONVERT(varchar(20), CAST('" + toDate + "' as DATETIME), 112)))";

                litAnch.Text = "<a href=\"shop-order-report.aspx?shopId=" + e.Row.Cells[0].Text + "&date=" + fromDate + "-" + toDate + "\" class=\"frStats btn btn-sm btn-default\" data-fancybox-type=\"iframe\"><i class=\"fa fa-eye\" aria-hidden=\"true\"></i></a>";
            }
            else
            {
                litAnch.Text = "<a href=\"shop-order-report.aspx?shopId=" + e.Row.Cells[0].Text + "&date=" + DateTime.Now + "\" class=\"frStats btn btn-sm btn-default\" data-fancybox-type=\"iframe\"><i class=\"fa fa-eye\" aria-hidden=\"true\"></i></a>";
            }
        }
    }

    protected void gvOtherShops_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Literal litAnch = (Literal)e.Row.FindControl("litAnch");
            //litAnch.Text = "<a href=\"shop-order-report.aspx?shopId=" + e.Row.Cells[0].Text + "\" class=\"frStats btn btn-sm btn-default\" data-fancybox-type=\"iframe\"><i class=\"fa fa-eye\" aria-hidden=\"true\"></i></a>";

            Literal litAnch = (Literal)e.Row.FindControl("litAnch");
            if (txtFDate.Text != "" && txtToDate.Text != "")
            {
                // From Date
                DateTime fromDate = DateTime.Now;
                string[] arrFromDate = txtFDate.Text.Split('/');
                fromDate = Convert.ToDateTime(arrFromDate[1] + "/" + arrFromDate[0] + "/" + arrFromDate[2]);

                // To Date
                DateTime toDate = DateTime.Now;
                string[] arrToDate = txtToDate.Text.Split('/');
                toDate = Convert.ToDateTime(arrToDate[1] + "/" + arrToDate[0] + "/" + arrToDate[2]);

                string dateCondition = "( (CONVERT(varchar(20), a.OrdAssignDate, 112) >= CONVERT(varchar(20), CAST('" + fromDate + "' as DATETIME), 112) ) AND "
                    + " (CONVERT(varchar(20), a.OrdAssignDate, 112) <= CONVERT(varchar(20), CAST('" + toDate + "' as DATETIME), 112)))";

                litAnch.Text = "<a href=\"shop-order-report.aspx?shopId=" + e.Row.Cells[0].Text + "&date=" + fromDate + "-" + toDate + "\" target=\"_blank\" class=\"frStats btn btn-sm btn-default\" data-fancybox-type=\"iframe\"><i class=\"fa fa-eye\" aria-hidden=\"true\"></i></a>";
            }
            else
            {
                litAnch.Text = "<a href=\"shop-order-report.aspx?shopId=" + e.Row.Cells[0].Text + "&date=" + DateTime.Now + "\" target=\"_blank\" class=\"frStats btn btn-sm btn-default\" data-fancybox-type=\"iframe\"><i class=\"fa fa-eye\" aria-hidden=\"true\"></i></a>";
            }
        }
    }
}