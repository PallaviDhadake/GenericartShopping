using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class supportteam_customer_order_consistency : System.Web.UI.Page
{
    iClass c = new iClass();
    public string reportMarkup;
    protected void Page_Load(object sender, EventArgs e)
    {
        btnShow.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnShow, null) + ";");
        //GetReportMarkup();
        if(!IsPostBack)
        {
            FillFinancialYears();
        }
    }


    private void FillFinancialYears()
    {
        try
        {
            ddrYear.Items.Insert(0, new ListItem("<-select->"));

            DateTime dNow = DateTime.Now;
            int maxCount = -1;
            for (int i = 1; i <= 10; i++)
            {
                int fYear2 = dNow.AddYears(-maxCount).Year;
                int fYear1 = dNow.AddYears(-(maxCount + 1)).Year;


                ddrYear.Items.Insert(i, new ListItem(fYear1 + "-" + fYear2));


                maxCount++;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FillFinancialYears", ex.Message.ToString());
            return;
        }
    }
    private void GetReportMarkup()
    {
        try
        {
            //int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            //string endDate = DateTime.Now.ToString("yyyy") + DateTime.Now.ToString("MM") + daysInMonth.ToString();
            //string sqlEDate = DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/" + daysInMonth.ToString();
            //DateTime tempDate = DateTime.Now.AddMonths(-2);
            //string startDate = tempDate.ToString("yyyy") + tempDate.ToString("MM") + "01";
            //string sqlSDate = tempDate.ToString("yyyy") + "/" + tempDate.ToString("MM") + "/" + "01";

            string[] arrYear = ddrYear.SelectedValue.Split('-');

            int fStart = Convert.ToInt16(arrYear[0]);
            int fEnd = Convert.ToInt16(arrYear[1]);

            DateTime sqlSDate = new DateTime();
            DateTime sqlEDate = new DateTime();

            string startDate = "", endDate = "";

            switch (ddrQuarter.SelectedValue)
            {
                case "1":
                    sqlSDate = Convert.ToDateTime("4/1/" + fStart);
                    sqlEDate = Convert.ToDateTime("6/30/" + fStart);
                    //fromMonth = 4;
                    //toMonth = 6;

                    startDate = fStart + "0401";
                    endDate = fStart + "0630";
                    break;
                case "2":
                    sqlSDate = Convert.ToDateTime("7/1/" + fStart);
                    sqlEDate = Convert.ToDateTime("9/30/" + fStart);
                    //fromMonth = 7;
                    //toMonth = 9;
                    startDate = fStart + "0701";
                    endDate = fStart + "0930";
                    break;
                case "3":
                    sqlSDate = Convert.ToDateTime("10/1/" + fStart);
                    sqlEDate = Convert.ToDateTime("12/31/" + fStart);
                    //fromMonth = 10;
                    //toMonth = 12;
                    startDate = fStart + "1001";
                    endDate = fStart + "1231";
                    break;
                case "4":
                    sqlSDate = Convert.ToDateTime("1/1/" + fEnd);
                    sqlEDate = Convert.ToDateTime("3/31/" + fEnd);
                    //fromMonth = 1;
                    //toMonth = 3;
                    startDate = fEnd + "0101";
                    endDate = fEnd + "0331";
                    break;
            }



            using(DataTable dtCust = c.GetDataTable("DECLARE @start DATE = '" + startDate + "'  , @end DATE = '" + endDate + "'  ;WITH Numbers (Number) AS   " +
                        " (SELECT ROW_NUMBER() OVER (ORDER BY OBJECT_ID) FROM sys.all_objects)  " +
                        " SELECT DATENAME(MONTH,DATEADD(MONTH, Number - 1, @start)) Name,MONTH(DATEADD(MONTH, Number - 1, @start)) MonthId  ," +
                        " DATENAME(YEAR,DATEADD(MONTH, Number - 1, @start)) Year FROM Numbers  a WHERE Number - 1 <= DATEDIFF(MONTH, @start, @end)  "))
            {
                if (dtCust.Rows.Count > 0)
                {
                    StringBuilder strMarkup = new StringBuilder();

                    strMarkup.Append("<table id=\"datatable\" class=\"table table-striped table-bordered table-hover w-100\">");
                    strMarkup.Append("<thead class=\"thead-dark\">");
                    strMarkup.Append("<tr>");
                    strMarkup.Append("<td>Customer Name</td>");
                    strMarkup.Append("<td>Customer Mobile No.</td>");
                    foreach (DataRow row in dtCust.Rows)
                    {
                        strMarkup.Append("<td>" + row["Name"] + " " + row["Year"] + "</td>");
                    }
                    strMarkup.Append("</tr>");
                    strMarkup.Append("</thead>");

                    using (DataTable dtCustData = c.GetDataTable("Select distinct a.FK_OrderCustomerID, b.CustomerName, b.CustomerMobile " +
                        " From OrdersData a Left Join CustomersData b On a.FK_OrderCustomerID=b.CustomrtID Where " +
                        " Convert(varchar(20), a.OrderDate, 112) >= Convert(varchar(20), CAST('" + sqlSDate + "' as datetime), 112) AND " +
                        " Convert(varchar(20), a.OrderDate, 112) <= Convert(varchar(20), CAST('" + sqlEDate + "' as datetime), 112) AND a.OrderStatus=7"))
                    {
                        if (dtCustData.Rows.Count > 0)
                        {
                            foreach (DataRow cRow in dtCustData.Rows)
                            {
                                strMarkup.Append("<tr>");
                                strMarkup.Append("<td>" + cRow["CustomerName"].ToString() + "</td>");
                                strMarkup.Append("<td>" + cRow["CustomerMobile"].ToString() + "</td>");
                                foreach (DataRow row in dtCust.Rows)
                                {
                                    string ordCount = c.returnAggregate("Select Count(OrderID) From OrdersData Where " + 
                                        " FK_OrderCustomerID=" + cRow["FK_OrderCustomerID"] + " AND MONTH(OrderDate)='" + row["MonthId"] +
                                        "' AND YEAR(OrderDate)='" + row["Year"] + "' AND OrderStatus=7").ToString();
                                    strMarkup.Append("<td>" + ordCount + "</td>");
                                }
                                strMarkup.Append("</tr>");
                            }
                        }
                    }
                    strMarkup.Append("</table>");

                    reportMarkup = strMarkup.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetReportMarkup", ex.Message.ToString());
            return;
        }
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        if (ddrQuarter.SelectedIndex == 0 && ddrYear.SelectedIndex == 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'select quarter and year');", true);
            return;
        }

        GetReportMarkup();
    }
}