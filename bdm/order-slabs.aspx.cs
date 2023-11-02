using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;

public partial class bdm_order_slabs : System.Web.UI.Page
{
    iClass c = new iClass();
    public string ordTitle;
    //public static string[] shopData = new string[10];
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            FillGrid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "Page_Load", ex.Message.ToString());
            return;
        }
    }

    private void FillGrid()
    {
        try
        {
            string strQuery1 = "";
            string strQuery2 = "";
            string strQuery3 = "";
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


                ordTitle = "Showing Report of Orders From " + fromDate.ToString("dd/MM/yyyy") + " to " + toDate.ToString("dd/MM/yyyy");

                //1. Zero Orders
                strQuery1 = "Select d.FranchID, d.FranchShopCode, d.FranchName, 0 as ordersCount, " +
                    " isnull(b.DistrictName, '-') as DistrictName, isnull(c.CityName, '-') as CityName" +
                    " From FranchiseeData d Left Join DistrictsData b On d.FK_FranchDistId=b.DistrictId Left Join CityData c " +
                    " On d.FK_FranchCityId=c.CityID Where d.FranchActive=1 " +
                    " AND d.FranchID NOT IN (Select Distinct a.Fk_FranchID From OrdersAssign a Where " + dateCondition + ")";

                //2. 1 - 100 Orders
                strQuery2 = "Select Distinct a.Fk_FranchID, Count(Distinct a.FK_OrderID) as ordersCount, b.FranchShopCode, b.FranchName, " +
                    " isnull(c.DistrictName, '-') as DistrictName, isnull(d.CityName, '-') as CityName " +
                    " From OrdersAssign a Inner Join FranchiseeData b On a.Fk_FranchID=b.FranchID Left Join DistrictsData c " +
                    " On b.FK_FranchDistId=c.DistrictId Left Join CityData d On b.FK_FranchCityId=d.CityID Where a.OrdAssignStatus=7 " +
                    " AND a.OrdReAssign=0 AND " + dateCondition + " Group By a.Fk_FranchID, b.FranchShopCode, b.FranchName,  " +
                    " c.DistrictName, d.CityName Having Count(Distinct a.FK_OrderID)>=1 AND Count(Distinct a.FK_OrderID)<=100 Order By ordersCount";

                //3. Above 100 Orders
                strQuery3 = "Select Distinct a.Fk_FranchID, Count(Distinct a.FK_OrderID) as ordersCount, b.FranchShopCode, b.FranchName, " +
                    " isnull(c.DistrictName, '-') as DistrictName, isnull(d.CityName, '-') as CityName " +
                    " From OrdersAssign a Inner Join FranchiseeData b On a.Fk_FranchID=b.FranchID Left Join DistrictsData c " +
                    " On b.FK_FranchDistId=c.DistrictId Left Join CityData d On b.FK_FranchCityId=d.CityID Where a.OrdAssignStatus=7 " +
                    " AND a.OrdReAssign=0 AND " + dateCondition + " Group By a.Fk_FranchID, b.FranchShopCode, b.FranchName, " +
                    " c.DistrictName, d.CityName Having Count(Distinct a.FK_OrderID)>100 Order By ordersCount";
                
            }
            else
            {
                ordTitle = "Showing Report of Overall Orders";
                //1. Zero Orders
                //strQuery1 = "Select FranchID, FranchShopCode, FranchName, 0 as ordersCount From FranchiseeData Where FranchActive=1 " +
                //    " AND FranchID NOT IN (Select Distinct Fk_FranchID From OrdersAssign)";

                strQuery1 = "Select d.FranchID, d.FranchShopCode, d.FranchName, 0 as ordersCount, " +
                    " isnull(b.DistrictName, '-') as DistrictName, isnull(c.CityName, '-') as CityName " +
                   " From FranchiseeData d Left Join DistrictsData b On d.FK_FranchDistId=b.DistrictId Left Join CityData c " +
                   " On d.FK_FranchCityId=c.CityID Where d.FranchActive=1 " +
                   " AND d.FranchID NOT IN (Select Distinct a.Fk_FranchID From OrdersAssign a)";

                //2. 1 - 100 Orders
                strQuery2 = "Select Distinct a.Fk_FranchID, Count(Distinct a.FK_OrderID) as ordersCount, b.FranchShopCode, b.FranchName, " + 
                    " isnull(c.DistrictName, '-') as DistrictName, isnull(d.CityName, '-') as CityName " +
                    " From OrdersAssign a Inner Join FranchiseeData b On a.Fk_FranchID=b.FranchID Left Join DistrictsData c " + 
                    " On b.FK_FranchDistId=c.DistrictId Left Join CityData d On b.FK_FranchCityId=d.CityID Where a.OrdAssignStatus=7 " +
                    " AND a.OrdReAssign=0 Group By a.Fk_FranchID, b.FranchShopCode, b.FranchName, c.DistrictName, d.CityName " +
                    " Having Count(Distinct a.FK_OrderID)>=1 AND Count(Distinct a.FK_OrderID)<=100 Order By ordersCount";

                //3. Above 100 Orders
                strQuery3 = "Select Distinct a.Fk_FranchID, Count(Distinct a.FK_OrderID) as ordersCount, b.FranchShopCode, b.FranchName, " + 
                    " isnull(c.DistrictName, '-') as DistrictName, isnull(d.CityName, '-') as CityName " +
                    " From OrdersAssign a Inner Join FranchiseeData b On a.Fk_FranchID=b.FranchID Left Join DistrictsData c " +
                    " On b.FK_FranchDistId=c.DistrictId Left Join CityData d On b.FK_FranchCityId=d.CityID Where a.OrdAssignStatus=7 " +
                    " AND a.OrdReAssign=0 Group By a.Fk_FranchID, b.FranchShopCode, b.FranchName, c.DistrictName, d.CityName " +
                    " Having Count(Distinct a.FK_OrderID)>100 Order By ordersCount";
            }

            // zero orders
            using (DataTable dtZeroOrd = c.GetDataTable(strQuery1))
            {
                gvZeroOrders.DataSource = dtZeroOrd;
                gvZeroOrders.DataBind();

                if (gvZeroOrders.Rows.Count > 0)
                {
                    gvZeroOrders.UseAccessibleHeader = true;
                    gvZeroOrders.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }

            // 1-100 orders
            using (DataTable dtHundredOrders = c.GetDataTable(strQuery2))
            {
                gvLess100.DataSource = dtHundredOrders;
                gvLess100.DataBind();

                if (gvLess100.Rows.Count > 0)
                {
                    gvLess100.UseAccessibleHeader = true;
                    gvLess100.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }

            // above 100 orders
            using (DataTable dtAboveHundredOrders = c.GetDataTable(strQuery3))
            {
                gvAbove100.DataSource = dtAboveHundredOrders;
                gvAbove100.DataBind();

                if (gvAbove100.Rows.Count > 0)
                {
                    gvAbove100.UseAccessibleHeader = true;
                    gvAbove100.HeaderRow.TableSection = TableRowSection.TableHeader;
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
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtFDate.Text == "" || txtToDate.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * marked fields are compulsory');", true);
                return;
            }

            // From Date
            DateTime fromDate = DateTime.Now;
            string[] arrFromDate = txtFDate.Text.Split('/');
            if (c.IsDate(arrFromDate[1] + "/" + arrFromDate[0] + "/" + arrFromDate[2]) == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid From Date');", true);
                return;
            }

            // To Date
            DateTime toDate = DateTime.Now;
            string[] arrToDate = txtToDate.Text.Split('/');
            if (c.IsDate(arrToDate[1] + "/" + arrToDate[0] + "/" + arrToDate[2]) == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid To Date');", true);
                return;
            }

            FillGrid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnShow_Click", ex.Message.ToString());
            return;
        }
    }

    protected void gvZeroOrders_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Literal litAnch = (Literal)e.Row.FindControl("litAnch");
            litAnch.Text = "<button type=\"button\" class=\"btn btn-sm btn-default\" data-toggle=\"modal\" data-target=\"#modal-lg\" onclick=\"getShopInfo('" + e.Row.Cells[0].Text + "')\"><i class=\"fa fa-eye\" aria-hidden=\"true\"></i></button>";
        }
    }

    protected void gvLess100_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Literal litAnch = (Literal)e.Row.FindControl("litAnch");
            litAnch.Text = "<button type=\"button\" class=\"btn btn-sm btn-default\" data-toggle=\"modal\" data-target=\"#modal-lg\" onclick=\"getShopInfo('" + e.Row.Cells[0].Text + "')\"><i class=\"fa fa-eye\" aria-hidden=\"true\"></i></button>";
        }
    }

    protected void gvAbove100_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Literal litAnch = (Literal)e.Row.FindControl("litAnch");
            litAnch.Text = "<button type=\"button\" class=\"btn btn-sm btn-default\" data-toggle=\"modal\" data-target=\"#modal-lg\" onclick=\"getShopInfo('" + e.Row.Cells[0].Text + "')\"><i class=\"fa fa-eye\" aria-hidden=\"true\"></i></button>";
        }
    }


    [WebMethod]
    public static string[] GetShopDetails(string shopIdX)
    {
        iClass c = new iClass();
        
        string[] shopData = new string[10];

        using (DataTable dtShop = c.GetDataTable("Select FranchID, FranchShopCode, FranchName, FranchOwnerName, FranchPinCode, FranchAddress, " +
            " FranchEmail, FranchMobile From FranchiseeData Where FranchID=" + shopIdX))
        {
            if (dtShop.Rows.Count > 0)
            {
                DataRow row = dtShop.Rows[0];

                shopData[0] = row["FranchName"].ToString();
                shopData[1] = row["FranchOwnerName"].ToString();
                shopData[2] = row["FranchShopCode"].ToString();
                shopData[3] = row["FranchMobile"].ToString();
                shopData[4] = row["FranchEmail"].ToString();
                shopData[5] = row["FranchAddress"].ToString();
                shopData[6] = row["FranchPinCode"].ToString();
            }
        }

        return shopData;
    }
}