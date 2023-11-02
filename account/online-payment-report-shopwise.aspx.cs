using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.Services;

public partial class account_online_payment_report_shopwise : System.Web.UI.Page
{
    iClass c = new iClass();
    public string monthwisePaidOrders, repTitle;
    protected void Page_Load(object sender, EventArgs e)
    {
        // Forcefully redirection to new page online-payment-report-shopwise-detail.aspx 7-June-22
        Response.Redirect("online-payment-report-shopwise-detail.aspx");
        
        
        if (!IsPostBack)
        {
            if (Request.QueryString["id"] != null)
            {
                viewOrder.Visible = false;
                monthwiseOrd.Visible = true;
                MonthBind();

                string recentOrderDate = "";

                using (DataTable dtDate = c.GetDataTable("Select TOP 1 a.OrderDate From OrdersData a Inner Join OrdersAssign b " + 
                    " On b.FK_OrderID=a.OrderID Inner Join online_payment_logs c On a.OrderPaymentTxnId=c.OPL_merchantTranId " + 
                    " Where b.OrdAssignStatus=7 AND b.Fk_FranchID=" + Request.QueryString["id"] + 
                    " AND (a.OrderPaymentTxnId IS NOT NULL AND a.OrderPaymentTxnId<>'') " +
                    " AND (c.OPL_transtatus='SUCCESS' OR c.OPL_transtatus='paid') Order By a.OrderDate desc"))
                {
                    if (dtDate.Rows.Count > 0)
                    {
                        DataRow row = dtDate.Rows[0];
                        recentOrderDate = row["OrderDate"].ToString();
                    }
                    else
                    {
                        recentOrderDate = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                }
                ddrMonth.SelectedValue = Convert.ToDateTime(recentOrderDate).Month.ToString();
                txtYear.Text = Convert.ToDateTime(recentOrderDate).Year.ToString();
                GetStats(Convert.ToInt32(ddrMonth.SelectedValue), Convert.ToInt32(txtYear.Text));
            }
            else
            {
                FillGrid();

                monthwiseOrd.Visible = false;
                viewOrder.Visible = true;
            }
        }
    }

    private void MonthBind()
    {
        try
        {
            DateTimeFormatInfo info = DateTimeFormatInfo.GetInstance(null);
            for (int i = 1; i < 13; i++)
            {
                ddrMonth.Items.Add(new ListItem(info.GetMonthName(i), i.ToString()));
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "MonthBind", ex.Message.ToString());
            return;
        }
    }

    private void FillGrid()
    {
        try
        {
            //using (DataTable dtPay = c.GetDataTable("Select Distinct a.FranchID, a.FranchName, a.FranchShopCode, " +
            //        "CAST( isnull( (Select SUM(od.OrderAmount) From OrdersData od Inner Join OrdersAssign oa On od.OrderID=oa.FK_OrderID Inner Join online_payment_logs opl On od.OrderPaymentTxnId=opl.OPL_merchantTranId " +
            //        "Where oa.Fk_FranchID=a.FranchID AND oa.OrdAssignStatus=7 AND oa.OrdReAssign=0 AND (od.OrderPaymentTxnId IS NOT NULL AND od.OrderPaymentTxnId<>'') AND (opl.OPL_transtatus='SUCCESS' OR opl.OPL_transtatus='paid') ), 0) AS int) - " +
            //        "CAST( isnull((Select SUM(fp.Payout_Amount) From FranchiseePayout fp Where fp.FK_FranchID=a.FranchID AND fp.Payout_Status=3 ), 0) AS int) as unpaidAmount" +
            //        " From FranchiseeData a Inner Join OrdersAssign b On a.FranchID=b.Fk_FranchID " +
            //        " Inner Join OrdersData c On c.OrderID=b.FK_OrderID Inner Join online_payment_logs d On c.OrderPaymentTxnId=d.OPL_merchantTranId " + 
            //        " Where c.OrderStatus=7 AND b.OrdReAssign=0 AND " +
            //        " (c.OrderPaymentTxnId IS NOT NULL AND c.OrderPaymentTxnId<>'') AND (d.OPL_transtatus='SUCCESS' OR d.OPL_transtatus='paid') "))

            //05/03/2022
            //a.FranchID Not In (24, 2010, 2062, 2070, 2012, 2064, 2065, 2124, 2137, 2142, 2165) AND 
            using (DataTable dtPay = c.GetDataTable("Select Distinct a.FranchID, a.FranchName, a.FranchShopCode, " +
                    " CAST( isnull( (Select SUM(opl.OLP_RazorPayAmount) From OrdersData od Inner Join OrdersAssign oa On od.OrderID=oa.FK_OrderID Inner Join online_payment_logs opl On CAST(od.OrderID as varchar(50))=opl.OLP_order_no  " +
                    " Where oa.Fk_FranchID=a.FranchID AND od.OrderStatus=7 AND oa.OrdAssignStatus=7 AND oa.OrdReAssign=0 AND (od.OrderPaymentTxnId IS NOT NULL AND od.OrderPaymentTxnId<>'') AND (opl.OPL_transtatus='SUCCESS' OR opl.OPL_transtatus='paid') ), 0) AS float) - " +
                    " CAST( isnull((Select SUM(fp.Payout_Amount) From FranchiseePayout fp Where fp.FK_FranchID=a.FranchID AND fp.Payout_Status=3 ), 0) AS float) as unpaidAmount " +
                    " From FranchiseeData a Inner Join OrdersAssign b On a.FranchID=b.Fk_FranchID  " +
                    " Inner Join OrdersData c On c.OrderID=b.FK_OrderID Inner Join online_payment_logs d On CAST(c.OrderID as varchar(50))=d.OLP_order_no " +
                    " Where c.OrderStatus=7 AND b.OrdReAssign=0 AND b.OrdAssignStatus=7 AND " +
                    " (c.OrderPaymentTxnId IS NOT NULL AND c.OrderPaymentTxnId<>'') AND (d.OPL_transtatus='SUCCESS' OR d.OPL_transtatus='paid') "))
            {
                gvOrder.DataSource = dtPay;
                gvOrder.DataBind();

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
                Literal litView = (Literal)e.Row.FindControl("litView");
                litView.Text = "<a href=\"online-payment-report-shopwise.aspx?id=" + e.Row.Cells[0].Text + "\" class=\"gView\"></a>";
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvOrder_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            txtYear.Text = txtYear.Text.Trim().Replace("'", "");

            if (ddrMonth.SelectedIndex == 0 || txtYear.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All Fields are compulsory');", true);
                return;
            }

            if (!c.IsNumeric(txtYear.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Year must be numeric value');", true);
                return;
            }

            GetStats(Convert.ToInt32(ddrMonth.SelectedValue), Convert.ToInt32(txtYear.Text));
        }
        catch (Exception ex)
        {
            monthwisePaidOrders = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    private void GetStats(int month, int year)
    {
        try
        {
            string shopName = c.GetReqData("FranchiseeData", "FranchName", "FranchID=" + Request.QueryString["id"]).ToString();
            repTitle = "Showing Report Of " + ddrMonth.SelectedItem.Text + " " + txtYear.Text + " Of " + shopName;
            //repTitle = "Showing Report Of " + ddrMonth.SelectedItem.Text + " " + txtYear.Text;
            var lastDay = DateTime.DaysInMonth(year, month);

            DateTime firstDayDate = new DateTime(year, month, 1);
            DateTime firstSlab = firstDayDate.AddDays(9);
            TimeSpan ts1 = firstSlab - firstDayDate;
            int dayDiff1 = ts1.Days;

            DateTime secondSlabFirstDate = firstSlab.AddDays(1);
            DateTime secondSlab = firstSlab.AddDays(10);
            TimeSpan ts2 = secondSlab - secondSlabFirstDate;
            int dayDiff2 = ts2.Days;

            DateTime thirdSlabFirstDate = secondSlab.AddDays(1);
            DateTime thirdSlab = new DateTime(year, month, lastDay);
            TimeSpan ts = thirdSlab - thirdSlabFirstDate;
            int dayDiff = ts.Days;

            StringBuilder strMarkup = new StringBuilder();

            string fId = Request.QueryString["id"].ToString();
            strMarkup.Append("<table class=\"table table-bordered table-responsive-md table-striped\">");
            strMarkup.Append("<tr class=\"bg-purple\">");
            strMarkup.Append("<td class=\"semiMedium text-bold\">Duration</td>");
            strMarkup.Append("<td class=\"semiMedium text-bold\">Days</td>");
            strMarkup.Append("<td class=\"semiMedium text-bold\">Paid Amount</td>");
            strMarkup.Append("<td class=\"semiMedium text-bold\">Status</td>");
            strMarkup.Append("<td class=\"semiMedium text-bold\"></td>");
            strMarkup.Append("</tr>");
            //first slab
            strMarkup.Append("<tr>");
            strMarkup.Append("<td>" + firstDayDate.ToString("dd/MM/yyyy") + " - " + firstSlab.ToString("dd/MM/yyyy") + "</td>");
            strMarkup.Append("<td>" + dayDiff1 + "</td>");

            //previous query - sum from ordersdata table
            //string firstSlabAmount = c.returnAggregate("Select SUM(a.OrderAmount) From OrdersData a Inner Join OrdersAssign b On a.OrderID=FK_OrderID " +
            //    " Inner Join online_payment_logs d On a.OrderPaymentTxnId=d.OPL_merchantTranId Where b.Fk_FranchID=" + fId +
            //    " AND ( CONVERT(varchar(20), a.OrderDate, 112) >= CONVERT(varchar(20), CAST('" + firstDayDate + "' as datetime) , 112) " +
            //    " AND CONVERT(varchar(20), a.OrderDate, 112) <= CONVERT(varchar(20), CAST('" + firstSlab + "' as datetime), 112 ) )" +
            //    " AND a.OrderStatus IN (7) AND b.OrdAssignStatus=7 AND a.OrderPaymentTxnId IS NOT NULL AND a.OrderPaymentTxnId<>'' " +
            //    " AND (d.OPL_transtatus='SUCCESS' OR d.OPL_transtatus='paid')").ToString();

            //new query = sum from online_payment_log table date : 05/03/2022
            string firstSlabAmount = c.returnAggregateNew("Select SUM(d.OLP_RazorPayAmount) From online_payment_logs d Inner Join OrdersData a " +
                " On d.OLP_order_no=CAST(a.OrderID as varchar(50)) Inner Join OrdersAssign b On a.OrderID=b.FK_OrderID Where b.Fk_FranchID=" + fId +
                " AND ( CONVERT(varchar(20), a.OrderDate, 112) >= CONVERT(varchar(20), CAST('" + firstDayDate + "' as datetime) , 112) " +
                " AND CONVERT(varchar(20), a.OrderDate, 112) <= CONVERT(varchar(20), CAST('" + firstSlab + "' as datetime), 112 ) )" +
                " AND a.OrderStatus IN (7) AND b.OrdAssignStatus=7 AND b.OrdReAssign=0 AND a.OrderPaymentTxnId IS NOT NULL AND a.OrderPaymentTxnId<>'' " +
                " AND (d.OPL_transtatus='SUCCESS' OR d.OPL_transtatus='paid')").ToString();
            strMarkup.Append("<td>Rs. " + Convert.ToDouble(firstSlabAmount).ToString("0.00") + "</td>");

            //calculate its status & action button
            double firstSlabAmt = Convert.ToDouble(firstSlabAmount);
            string actionButton1 = "", status1 = "";
            if (firstSlabAmt > 0)
            {
                if(c.IsRecordExist("Select Payout_ID From FranchiseePayout Where FK_FranchID="+fId+
                    " AND ( CONVERT(varchar(20), Payout_FromDate, 112) >= CONVERT(varchar(20), CAST('" + firstDayDate + "' as datetime) , 112) " +
                " AND CONVERT(varchar(20), Payout_ToDate, 112) <= CONVERT(varchar(20), CAST('" + firstSlab + "' as datetime), 112 ) ) " +
                " AND Payout_Status=1 AND (Payout_AccActivityDate IS NOT NULL OR Payout_AccActivityDate<>'')"))
                {
                    status1 = "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Approved By Account</span><span class=\"space1\"></span>" +
                                "<span class=\"clrProcessing\">Pending From Payment</span><span class=\"space1\"></span>" +
                                "<span class=\"clrProcessing\">Pending From Rajah</span>";
                }
                else
                {
                    object payStatus = c.GetReqData("FranchiseePayout", "Payout_Status", "FK_FranchID=" + fId + " AND ( CONVERT(varchar(20), Payout_FromDate, 112) >= CONVERT(varchar(20), CAST('" + firstDayDate + "' as datetime) , 112) AND CONVERT(varchar(20), Payout_ToDate, 112) <= CONVERT(varchar(20), CAST('" + firstSlab + "' as datetime), 112 ) ) ");
                    if (payStatus != DBNull.Value && payStatus != null && payStatus.ToString() != "")
                    {
                        switch (Convert.ToInt32(payStatus))
                        {
                            case 1: status1 = "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Approved By Account</span><span class=\"space1\"></span>" +
                                "<span class=\"clrProcessing\">Pending From Payment</span><span class=\"space1\"></span>" +
                                "<span class=\"clrProcessing\">Pending From Rajah</span>";
                                break;
                            case 2: status1 = "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Approved By Account</span> <span class=\"space1\"></span>" +
                                "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Paid To Rajah</span><span class=\"space1\"></span>" +
                                "<span class=\"clrProcessing\">Pending From Rajah</span>";
                                break;
                            case 3: status1 = "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Approved By Account</span> <span class=\"space1\"></span>" +
                                "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Paid To Rajah</span><span class=\"space1\"></span>" +
                                "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Received & Updated By Rajah</span>"; break;
                        }
                    }
                    else
                    {
                        status1 = "<span class=\"clrProcessing\">Pending For Approval From Account</span>" +
                        "<span class=\"clrProcessing\">Pending From Payment</span>" +
                        "<span class=\"clrProcessing\">Pending From Rajah</span>";
                        string date1 = firstDayDate.ToString("dd/MM/yyyy") + " - " + firstDayDate.ToString("dd/MM/yyyy");
                        actionButton1 = "<span class=\"btn btn-md btn-success\" onclick=\"ApprovePayment('" + date1 + "', '" + Request.QueryString["id"] + "', '" + firstSlabAmt.ToString("0.00") + "');\">Approve</span>";
                    }
                }
            }

            strMarkup.Append("<td>" + status1 + "</td>");
            strMarkup.Append("<td>" + actionButton1 + "</td>");
            strMarkup.Append("</tr>");
            //second slab
            strMarkup.Append("<tr>");
            strMarkup.Append("<td>" + secondSlabFirstDate.ToString("dd/MM/yyyy") + " - " + secondSlab.ToString("dd/MM/yyyy") + "</td>");
            strMarkup.Append("<td>" + dayDiff2 + "</td>");

            //string secondSlabAmount = c.returnAggregate("Select SUM(a.OrderAmount) From OrdersData a Inner Join OrdersAssign b On a.OrderID=FK_OrderID " +
            //    " Inner Join online_payment_logs d On a.OrderPaymentTxnId=d.OPL_merchantTranId Where b.Fk_FranchID=" + fId +
            //    " AND ( CONVERT(varchar(20), a.OrderDate, 112) >= CONVERT(varchar(20), CAST('" + secondSlabFirstDate + "' as datetime) , 112) " +
            //    " AND CONVERT(varchar(20), a.OrderDate, 112) <= CONVERT(varchar(20), CAST('" + secondSlab + "' as datetime), 112 ) )" +
            //    " AND a.OrderStatus IN (7) AND b.OrdAssignStatus=7 AND a.OrderPaymentTxnId IS NOT NULL AND a.OrderPaymentTxnId<>'' " +
            //    " AND (d.OPL_transtatus='SUCCESS' OR d.OPL_transtatus='paid')").ToString();

            string secondSlabAmount = c.returnAggregateNew("Select SUM(d.OLP_RazorPayAmount) From online_payment_logs d Inner Join OrdersData a " + 
                " On d.OLP_order_no=CAST(a.OrderID as varchar(50)) Inner Join OrdersAssign b On a.OrderID=b.FK_OrderID Where b.Fk_FranchID=" + fId +
                " AND ( CONVERT(varchar(20), a.OrderDate, 112) >= CONVERT(varchar(20), CAST('" + secondSlabFirstDate + "' as datetime) , 112) " +
                " AND CONVERT(varchar(20), a.OrderDate, 112) <= CONVERT(varchar(20), CAST('" + secondSlab + "' as datetime), 112 ) )" +
                " AND a.OrderStatus IN (7) AND b.OrdAssignStatus=7 AND b.OrdReAssign=0 AND a.OrderPaymentTxnId IS NOT NULL AND a.OrderPaymentTxnId<>'' " +
                " AND (d.OPL_transtatus='SUCCESS' OR d.OPL_transtatus='paid')").ToString();

            strMarkup.Append("<td>Rs. " + Convert.ToDouble(secondSlabAmount).ToString("0.00") + "</td>");

            double secondSlabAmt = Convert.ToDouble(secondSlabAmount);
            string actionButton2 = "", status2 = "";
            if (secondSlabAmt > 0)
            {
                if (c.IsRecordExist("Select Payout_ID From FranchiseePayout Where FK_FranchID=" + fId +
                    " AND ( CONVERT(varchar(20), Payout_FromDate, 112) >= CONVERT(varchar(20), CAST('" + secondSlabFirstDate + "' as datetime) , 112) " +
                " AND CONVERT(varchar(20), Payout_ToDate, 112) <= CONVERT(varchar(20), CAST('" + secondSlab + "' as datetime), 112 ) ) " +
                " AND Payout_Status=1 AND (Payout_AccActivityDate IS NOT NULL OR Payout_AccActivityDate<>'')"))
                {
                    status2 = "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Approved By Account</span><span class=\"space1\"></span>" +
                                "<span class=\"clrProcessing\">Pending From Payment</span><span class=\"space1\"></span>" +
                                "<span class=\"clrProcessing\">Pending From Rajah</span>";
                }
                else
                {
                    object payStatus = c.GetReqData("FranchiseePayout", "Payout_Status", "FK_FranchID=" + fId + " AND ( CONVERT(varchar(20), Payout_FromDate, 112) >= CONVERT(varchar(20), CAST('" + secondSlabFirstDate + "' as datetime) , 112) AND CONVERT(varchar(20), Payout_ToDate, 112) <= CONVERT(varchar(20), CAST('" + secondSlab + "' as datetime), 112 ) ) ");
                    if (payStatus != DBNull.Value && payStatus != null && payStatus.ToString() != "")
                    {
                        switch (Convert.ToInt32(payStatus))
                        {
                            case 1: status2 = "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Approved By Account</span><span class=\"space1\"></span>" +
                                "<span class=\"clrProcessing\">Pending From Payment</span><span class=\"space1\"></span>" +
                                "<span class=\"clrProcessing\">Pending From Rajah</span>";
                                break;
                            case 2: status2 = "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Approved By Account</span> <span class=\"space1\"></span>" +
                                "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Paid To Rajah</span><span class=\"space1\"></span>" +
                                "<span class=\"clrProcessing\">Pending From Rajah</span>";
                                break;
                            case 3: status2 = "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Approved By Account</span> <span class=\"space1\"></span>" +
                                "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Paid To Rajah</span><span class=\"space1\"></span>" +
                                "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Received & Updated By Rajah</span>"; break;
                        }
                    }
                    else
                    {
                        status2 = "<span class=\"clrProcessing\">Pending For Approval From Account</span>" +
                        "<span class=\"clrProcessing\">Pending From Payment</span>" +
                        "<span class=\"clrProcessing\">Pending From Rajah</span>";
                        string date2 = secondSlabFirstDate.ToString("dd/MM/yyyy") + " - " + secondSlabFirstDate.ToString("dd/MM/yyyy");
                        actionButton2 = "<span class=\"btn btn-md btn-success\" onclick=\"ApprovePayment('" + date2 + "', '" + Request.QueryString["id"] + "', '" + secondSlabAmt.ToString("0.00") + "');\">Approve</span>";
                    }
                }
            }

            strMarkup.Append("<td>" + status2 + "</td>");
            strMarkup.Append("<td>" + actionButton2 + "</td>");
            strMarkup.Append("</tr>");


            //third slab
            strMarkup.Append("<tr>");
            strMarkup.Append("<td>" + thirdSlabFirstDate.ToString("dd/MM/yyyy") + " - " + thirdSlab.ToString("dd/MM/yyyy") + "</td>");
            strMarkup.Append("<td>" + dayDiff + "</td>");

            //string thirdSlabAmount = c.returnAggregate("Select SUM(a.OrderAmount) From OrdersData a Inner Join OrdersAssign b On a.OrderID=FK_OrderID " +
            //    " Inner Join online_payment_logs d On a.OrderPaymentTxnId=d.OPL_merchantTranId Where b.Fk_FranchID=" + fId +
            //    " AND ( CONVERT(varchar(20), a.OrderDate, 112) >= CONVERT(varchar(20), CAST('" + thirdSlabFirstDate + "' as datetime) , 112) " +
            //    " AND CONVERT(varchar(20), a.OrderDate, 112) <= CONVERT(varchar(20), CAST('" + thirdSlab + "' as datetime), 112 ) )" +
            //    " AND a.OrderStatus IN (7) AND b.OrdAssignStatus=7 AND a.OrderPaymentTxnId IS NOT NULL AND a.OrderPaymentTxnId<>'' " +
            //    " AND (d.OPL_transtatus='SUCCESS' OR d.OPL_transtatus='paid')").ToString();

            string thirdSlabAmount = c.returnAggregateNew("Select SUM(d.OLP_RazorPayAmount) From online_payment_logs d Inner Join OrdersData a " +
                " On d.OLP_order_no=CAST(a.OrderID as varchar(50)) Inner Join OrdersAssign b On a.OrderID=b.FK_OrderID Where b.Fk_FranchID=" + fId +
                " AND ( CONVERT(varchar(20), a.OrderDate, 112) >= CONVERT(varchar(20), CAST('" + thirdSlabFirstDate + "' as datetime) , 112) " +
                " AND CONVERT(varchar(20), a.OrderDate, 112) <= CONVERT(varchar(20), CAST('" + thirdSlab + "' as datetime), 112 ) )" +
                " AND a.OrderStatus IN (7) AND b.OrdAssignStatus=7 AND b.OrdReAssign=0 AND a.OrderPaymentTxnId IS NOT NULL AND a.OrderPaymentTxnId<>'' " +
                " AND (d.OPL_transtatus='SUCCESS' OR d.OPL_transtatus='paid')").ToString();


            strMarkup.Append("<td>Rs. " + Convert.ToDouble(thirdSlabAmount).ToString("0.00") + "</td>");

            double thirdSlabAmt = Convert.ToDouble(thirdSlabAmount);
            string actionButton3 = "", status3 = "";
            if (thirdSlabAmt > 0)
            {
                if (c.IsRecordExist("Select Payout_ID From FranchiseePayout Where FK_FranchID=" + fId +
                    " AND ( CONVERT(varchar(20), Payout_FromDate, 112) >= CONVERT(varchar(20), CAST('" + thirdSlabFirstDate + "' as datetime) , 112) " +
                " AND CONVERT(varchar(20), Payout_ToDate, 112) <= CONVERT(varchar(20), CAST('" + thirdSlab + "' as datetime), 112 ) ) " +
                " AND Payout_Status=1 AND (Payout_AccActivityDate IS NOT NULL OR Payout_AccActivityDate<>'')"))
                {
                    status3 = "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Approved By Account</span><span class=\"space1\"></span>" +
                                "<span class=\"clrProcessing\">Pending From Payment</span><span class=\"space1\"></span>" +
                                "<span class=\"clrProcessing\">Pending From Rajah</span>";
                }
                else
                {
                    object payStatus = c.GetReqData("FranchiseePayout", "Payout_Status", "FK_FranchID=" + fId + " AND ( CONVERT(varchar(20), Payout_FromDate, 112) >= CONVERT(varchar(20), CAST('" + thirdSlabFirstDate + "' as datetime) , 112) AND CONVERT(varchar(20), Payout_ToDate, 112) <= CONVERT(varchar(20), CAST('" + thirdSlab + "' as datetime), 112 ) ) ");
                    if (payStatus != DBNull.Value && payStatus != null && payStatus.ToString() != "")
                    {
                        switch (Convert.ToInt32(payStatus))
                        {
                            case 1: status3 = "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Approved By Account</span><span class=\"space1\"></span>" +
                                "<span class=\"clrProcessing\">Pending From Payment</span><span class=\"space1\"></span>" +
                                "<span class=\"clrProcessing\">Pending From Rajah</span>";
                                break;
                            case 2: status3 = "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Approved By Account</span> <span class=\"space1\"></span>" +
                                "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Paid To Rajah</span><span class=\"space1\"></span>" +
                                "<span class=\"clrProcessing\">Pending From Rajah</span>";
                                break;
                            case 3: status3 = "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Approved By Account</span> <span class=\"space1\"></span>" +
                                "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Paid To Rajah</span><span class=\"space1\"></span>" +
                                "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Received & Updated By Rajah</span>"; break;
                        }
                    }
                    else
                    {
                        status3 = "<span class=\"clrProcessing\">Pending For Approval From Account</span>" +
                        "<span class=\"clrProcessing\">Pending From Payment</span>" +
                        "<span class=\"clrProcessing\">Pending From Rajah</span>";
                        string date3 = thirdSlabFirstDate.ToString("dd/MM/yyyy") + " - " + thirdSlab.ToString("dd/MM/yyyy");

                        actionButton3 = "<span class=\"btn btn-md btn-success\" onclick=\"ApprovePayment('" + date3 + "', '" + Request.QueryString["id"] + "', '" + thirdSlabAmt.ToString("0.00") + "');\">Approve</span>";
                    }
                }
            }

            strMarkup.Append("<td>" + status3 + "</td>");
            strMarkup.Append("<td>" + actionButton3 + "</td>");
            strMarkup.Append("</tr>");
            strMarkup.Append("</table>");

            strMarkup.Append("<span class=\"space10\"></span>");
            strMarkup.Append("<a href=\"online-payment-report-shopwise.aspx\" class=\"btn btn-md btn-dark\">Back</a>");

            monthwisePaidOrders = strMarkup.ToString();
        }
        catch (Exception ex)
        {
            monthwisePaidOrders = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }


    [WebMethod]
    public static int UpdatePayout(string dates, string franchId, string ordAmount)
    {
        try
        {
            iClass c = new iClass();
            string[] arrDates = dates.ToString().Split('-');
            string fromDate = arrDates[0];
            string toDate = arrDates[1];

            string[] arrFDate = fromDate.Split('/');
            if (c.IsDate(arrFDate[1] + "/" + arrFDate[0] + "/" + arrFDate[2]) == false)
            {
                return 0; // invalid From date
            }

            DateTime fDate = Convert.ToDateTime(arrFDate[1] + "/" + arrFDate[0] + "/" + arrFDate[2]);

            string[] arrTDate = toDate.Split('/');
            if (c.IsDate(arrTDate[1] + "/" + arrTDate[0] + "/" + arrTDate[2]) == false)
            {
                return 2; // invalid To date
            }

            DateTime tDate = Convert.ToDateTime(arrTDate[1] + "/" + arrTDate[0] + "/" + arrTDate[2]);

            int maxId = c.NextId("FranchiseePayout", "Payout_ID");

            // Check if this is Company Own Shop then update approval of all three departments i.e. mark Payout_Status=3
            // (27/Sept by Vinayak on request of Anand Sir of Accounts dept)
            // Accounts :1
            // Payment  :2
            // Rajah    :3
            if(c.IsRecordExist("Select [CSID] From [CompanyOwnShops] where [FK_FranchID]=" + franchId) == true)
            {
                c.ExecuteQuery("Insert Into FranchiseePayout(Payout_ID, FK_FranchID, Payout_FromDate, Payout_ToDate, Payout_Amount, " +
              " Payout_Status, Payout_AccActivityDate, DelMark) Values (" + maxId + ", " + franchId + ", '" + fDate +
              "', '" + tDate + "', " + ordAmount + ", 3, '" + DateTime.Now + "', 0)");
            }
            else
            {
                c.ExecuteQuery("Insert Into FranchiseePayout(Payout_ID, FK_FranchID, Payout_FromDate, Payout_ToDate, Payout_Amount, " +
               " Payout_Status, Payout_AccActivityDate, DelMark) Values (" + maxId + ", " + franchId + ", '" + fDate +
               "', '" + tDate + "', " + ordAmount + ", 1, '" + DateTime.Now + "', 0)");
            }
           

            return 1;
        }
        catch (Exception)
        {
            return 3;
        }
    }
}