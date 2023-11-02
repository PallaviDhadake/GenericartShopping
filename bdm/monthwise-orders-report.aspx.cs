using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class bdm_monthwise_orders_report : System.Web.UI.Page
{
    iClass c = new iClass();
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
            string startDate = "01/01/" + DateTime.Now.Year.ToString();
            DateTime now = DateTime.Now;    
            DateTime firstDayCurrentMonth = new DateTime(now.Year, now.Month, 1);
            DateTime lastDayLastMonth = firstDayCurrentMonth.AddDays(-1);

            using(DataTable dtOrd = c.GetDataTable("Select  DATENAME(MONTH, OrderDate) + ' ' + DATENAME(YEAR, OrderDate) as OrderMonth, " + 
                " MONTH(OrderDate) NumOfMonth, Count(OrderID) as OrderCount, SUM(OrderAmount) as OrderAmount " +
                " From OrdersData Where (CONVERT(varchar(20), OrderDate, 112) >= CONVERT(varchar(20), CAST('" + startDate + "' as DateTime), 112) " +
                " And CONVERT(varchar(20), OrderDate, 112) <= CONVERT(varchar(20), CAST('" + lastDayLastMonth + "' as DateTime), 112)) " +
                " AND OrderStatus<>0 AND FK_OrderCustomerID<>0 " +
                " Group By DATENAME(MONTH, OrderDate) + ' ' + DATENAME(YEAR, OrderDate), MONTH(OrderDate), YEAR(OrderDate) " +
                " Order By YEAR(OrderDate), MONTH(OrderDate)"))
            {
                gvOrder.DataSource = dtOrd;
                gvOrder.DataBind();

                if (gvOrder.Rows.Count > 0)
                {
                    gvOrder.UseAccessibleHeader = true;
                    gvOrder.HeaderRow.TableSection = TableRowSection.TableHeader;

                    // total orders count
                    int totalOrders = dtOrd.AsEnumerable().Sum(row => row.Field<int>("OrderCount"));
                    gvOrder.FooterRow.Cells[1].Text = "Total";
                    gvOrder.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                    gvOrder.FooterRow.Cells[2].Text = totalOrders.ToString("0");

                    // total order amount
                    double totalOrderAmount = dtOrd.AsEnumerable().Sum(row => row.Field<double>("OrderAmount"));
                    gvOrder.FooterRow.Cells[3].Text = totalOrderAmount.ToString("0.00");
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
}