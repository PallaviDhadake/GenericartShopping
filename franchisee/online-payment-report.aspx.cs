using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Text;

public partial class franchisee_online_payment_report : System.Web.UI.Page
{
    iClass c = new iClass();
    public string monthwisePaidOrders, repTitle;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["type"] != null)
            {
                viewOrder.Visible = false;
                MonthBind();

                if (Request.QueryString["type"] == "monthwise")
                {
                    monthwiseOrd.Visible = true;

                    //ddrMonth.SelectedValue = DateTime.Now.Month.ToString();
                    //txtYear.Text = DateTime.Now.Year.ToString();
                    string recentOrderDate = "";
                    using (DataTable dtDate = c.GetDataTable("Select TOP 1 a.OrderDate From OrdersData a Inner Join OrdersAssign b " +
                    " On b.FK_OrderID=a.OrderID Inner Join online_payment_logs c On a.OrderPaymentTxnId=c.OPL_merchantTranId " +
                    " Where b.OrdAssignStatus=7 AND b.Fk_FranchID=" + Session["adminFranchisee"] + 
                    " AND (a.OrderPaymentTxnId IS NOT NULL AND a.OrderPaymentTxnId<>'') " +
                    " AND (c.OPL_transtatus='SUCCESS' OR c.OPL_transtatus='paid') Order By a.OrderDate desc"))
                    {
                        if (dtDate.Rows.Count > 0)
                        {
                            DataRow row = dtDate.Rows[0];
                            recentOrderDate = row["OrderDate"].ToString();

                            ddrMonth.SelectedValue = Convert.ToDateTime(recentOrderDate).Month.ToString();
                            txtYear.Text = Convert.ToDateTime(recentOrderDate).Year.ToString();
                        }
                        else
                        {
                            //recentOrderDate = DateTime.Now.ToString("dd/MM/yyyy");
                            ddrMonth.SelectedValue = DateTime.Now.Month.ToString();
                            txtYear.Text = DateTime.Now.Year.ToString();
                        }
                    }

                    GetStats(Convert.ToInt32(ddrMonth.SelectedValue), Convert.ToInt32(txtYear.Text));
                }
            }
            else
            {
                FillGrid();

                monthwiseOrd.Visible = false;
                viewOrder.Visible = true;
            }
        }
    }

    private void FillGrid()
    {
        try
        {
            string fId = Session["adminFranchisee"].ToString();
            using(DataTable dtPay = c.GetDataTable("Select Distinct a.FK_OrderID, a.OrdAssignID, a.Fk_FranchID, b.FK_OrderCustomerID, " +
                " Convert(varchar(20), b.OrderDate, 103) as ordDate, c.CustomerName, Convert(varchar(20), d.OPL_datetime, 103) as payDate, " +
                " b.OrderPaymentTxnId, 'Rs. ' + Convert(varchar(20), b.OrderAmount) as OrdAmount, " +
                " 'Rs. ' + Convert(varchar(20), d.OLP_amount) as OLP_amount, a.OrdAssignStatus, d.OPL_transtatus, d.OLP_device_type " +
                " From OrdersAssign a Inner Join OrdersData b On a.FK_OrderID=b.OrderID Inner Join CustomersData c " + 
                " On b.FK_OrderCustomerID=c.CustomrtID Inner Join online_payment_logs d On b.OrderPaymentTxnId=d.OPL_merchantTranId " +
                " Where a.Fk_FranchID=" + fId + " AND b.OrderStatus<>0 AND (OrderPaymentTxnId IS NOT NULL AND b.OrderPaymentTxnId<>'') " + 
                " AND (d.OPL_transtatus='SUCCESS' OR d.OPL_transtatus='paid')"))
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

                string ordID = e.Row.Cells[3].Text.ToString();

                
                // OrderAssignStatus 0 > Pending, 1 > Accepted, 2 > Rejected, 5 > In Process, 6 > Shipped, 7 > Delivered
                //(3,4 not considered to match flags 5,6,7 with main OrdersData table)

                int reAssignStatus = Convert.ToInt32(c.GetReqData("OrdersAssign", "OrdReAssign", "OrdAssignID=" + e.Row.Cells[0].Text));

                int MainOrderStatus = Convert.ToInt32(c.GetReqData("OrdersData", "OrderStatus", "OrderID=" + e.Row.Cells[3].Text));

                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                switch (e.Row.Cells[1].Text)
                {
                    case "0":
                        if (c.IsRecordExist("Select OrdAssignID From OrdersAssign Where FK_OrderID=" + e.Row.Cells[3].Text + " AND Fk_FranchID=" + Session["adminFranchisee"] + " AND OrdAssignStatus=0 AND OrdReAssign=1 AND OrdAssignID<>" + e.Row.Cells[0].Text))
                        {
                            if (reAssignStatus == 1)
                            {
                                litStatus.Text = "<div class=\"ordAutoRoute\">Auto Routed</div>";
                            }
                            else
                            {
                                litStatus.Text = "<div class=\"ordNew\">Re Assigned</div>";
                            }
                        }
                        else
                        {
                            if (reAssignStatus == 0)
                            {
                                string frCode = "", frInfo = "";
                                if (c.IsRecordExist("Select OrdAssignID From OrdersAssign Where OrdAssignStatus=2 AND FK_OrderID=" + e.Row.Cells[3].Text + ""))
                                {
                                    int frId = Convert.ToInt32(c.GetReqData("OrdersAssign", "Top 1 Fk_FranchID", "OrdAssignStatus=2 AND FK_OrderID=" + e.Row.Cells[3].Text + " Order By OrdAssignID DESC"));
                                    frCode = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + frId).ToString();
                                    frInfo = " (Rejected By " + frCode + ")";
                                }
                                litStatus.Text = "<div class=\"ordNew\">New " + frInfo + "</div>";
                            }
                            if (reAssignStatus == 1)
                            {
                                litStatus.Text = "<div class=\"ordAutoRoute\">Auto Routed</div>";
                            }
                        }

                        if (MainOrderStatus == 2)
                        {
                            litStatus.Text = "<div class=\"ordCancCust\">Cancel By Customer</div>";
                        }
                        break;
                    case "1":
                        litStatus.Text = "<div class=\"ordAccepted\">Accepted</div>";
                        break;
                    case "2":
                        litStatus.Text = "<div class=\"ordDenied\">Rejected</div>";
                        break;
                    case "5":
                        litStatus.Text = "<div class=\"ordProcessing\">Inprocess</div>";
                        break;
                    case "6":
                        litStatus.Text = "<div class=\"ordShipped\">Shipped</div>";
                        break;
                    case "7":
                        litStatus.Text = "<div class=\"ordDelivered\">Delivered</div>";
                        break;
                    case "10":
                        litStatus.Text = "<div class=\"ordDenied\">Returned By Customer</div>";
                        break;

                }

                e.Row.Cells[3].Text = "<a href=\"order-details.aspx?id=" + e.Row.Cells[3].Text + "&assignId=" + e.Row.Cells[0].Text + "\" target=\"_blank\">" + e.Row.Cells[3].Text + "</a>";
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvOrder_RowDataBound", ex.Message.ToString());
            return;
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
            //string shopName = c.GetReqData("FranchiseeData", "FranchName", "FranchID=" + Request.QueryString["id"]).ToString();
            repTitle = "Showing Report Of " + ddrMonth.SelectedItem.Text + " " + txtYear.Text;
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

            string fId = Session["adminFranchisee"].ToString();
            strMarkup.Append("<table class=\"table table-bordered table-responsive-md table-striped\">");
            strMarkup.Append("<tr class=\"bg-purple\">");
            strMarkup.Append("<td class=\"semiMedium text-bold\">Duration</td>");
            strMarkup.Append("<td class=\"semiMedium text-bold\">Days</td>");
            strMarkup.Append("<td class=\"semiMedium text-bold\">Paid Amount</td>");
            strMarkup.Append("<td class=\"semiMedium text-bold\">Status</td>");
            strMarkup.Append("</tr>");
            //first slab
            strMarkup.Append("<tr>");
            strMarkup.Append("<td>" + firstDayDate.ToString("dd/MM/yyyy") + " - " + firstSlab.ToString("dd/MM/yyyy") + "</td>");
            strMarkup.Append("<td>" + dayDiff1 + "</td>");
            //string firstSlabAmount = c.returnAggregate("Select SUM(a.OrderAmount) From OrdersData a Inner Join OrdersAssign b On a.OrderID=FK_OrderID " +
            //    " Inner Join online_payment_logs d On a.OrderPaymentTxnId=d.OPL_merchantTranId Where b.Fk_FranchID=" + fId +
            //    " AND ( CONVERT(varchar(20), a.OrderDate, 112) >= CONVERT(varchar(20), CAST('" + firstDayDate + "' as datetime) , 112) " +
            //    " AND CONVERT(varchar(20), a.OrderDate, 112) <= CONVERT(varchar(20), CAST('" + firstSlab + "' as datetime), 112 ) )" +
            //    " AND a.OrderStatus NOT IN (0, 2, 4) AND a.OrderPaymentTxnId IS NOT NULL AND a.OrderPaymentTxnId<>'' " +
            //    " AND (d.OPL_transtatus='SUCCESS' OR d.OPL_transtatus='paid')").ToString();


            //string firstSlabAmount = c.returnAggregate("Select SUM(a.OrderAmount) From OrdersData a Inner Join OrdersAssign b On a.OrderID=FK_OrderID " +
            //    " Inner Join online_payment_logs d On a.OrderPaymentTxnId=d.OPL_merchantTranId Where b.Fk_FranchID=" + fId +
            //    " AND ( CONVERT(varchar(20), a.OrderDate, 112) >= CONVERT(varchar(20), CAST('" + firstDayDate + "' as datetime) , 112) " +
            //    " AND CONVERT(varchar(20), a.OrderDate, 112) <= CONVERT(varchar(20), CAST('" + firstSlab + "' as datetime), 112 ) )" +
            //    " AND a.OrderStatus IN (7) AND a.OrderPaymentTxnId IS NOT NULL AND a.OrderPaymentTxnId<>'' " +
            //    " AND (d.OPL_transtatus='SUCCESS' OR d.OPL_transtatus='paid')").ToString();

            //new query = sum from online_payment_log table date : 05/03/2022
            string firstSlabAmount = c.returnAggregateNew("Select SUM(d.OLP_RazorPayAmount) From online_payment_logs d Inner Join OrdersData a " +
                " On d.OLP_order_no=CAST(a.OrderID as varchar(50)) Inner Join OrdersAssign b On a.OrderID=b.FK_OrderID Where b.Fk_FranchID=" + fId +
                " AND ( CONVERT(varchar(20), a.OrderDate, 112) >= CONVERT(varchar(20), CAST('" + firstDayDate + "' as datetime) , 112) " +
                " AND CONVERT(varchar(20), a.OrderDate, 112) <= CONVERT(varchar(20), CAST('" + firstSlab + "' as datetime), 112 ) )" +
                " AND a.OrderStatus IN (7) AND b.OrdAssignStatus=7 AND b.OrdReAssign=0 AND a.OrderPaymentTxnId IS NOT NULL AND a.OrderPaymentTxnId<>'' " +
                " AND (d.OPL_transtatus='SUCCESS' OR d.OPL_transtatus='paid')").ToString();

            strMarkup.Append("<td>Rs. " + Convert.ToDouble(firstSlabAmount).ToString("0.00") + "</td>");

            string status1 = "";

            if (Convert.ToDouble(firstSlabAmount) > 0)
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
                    //status3 = "<span class=\"clrProcessing\">Pending For Approval From Account</span>";
                    status1 = "<span class=\"clrProcessing\">Pending For Approval From Account</span>" +
                        "<span class=\"clrProcessing\">Pending From Payment</span>" +
                        "<span class=\"clrProcessing\">Pending From Rajah</span>";
                }
            }
            strMarkup.Append("<td>" + status1 + "</td>");
            strMarkup.Append("</tr>");
            //second slab
            strMarkup.Append("<tr>");
            strMarkup.Append("<td>" + secondSlabFirstDate.ToString("dd/MM/yyyy") + " - " + secondSlab.ToString("dd/MM/yyyy") + "</td>");
            strMarkup.Append("<td>" + dayDiff2 + "</td>");
            //string secondSlabAmount = c.returnAggregate("Select SUM(a.OrderAmount) From OrdersData a Inner Join OrdersAssign b On a.OrderID=FK_OrderID " +
            //    " Inner Join online_payment_logs d On a.OrderPaymentTxnId=d.OPL_merchantTranId Where b.Fk_FranchID=" + fId +
            //    " AND ( CONVERT(varchar(20), a.OrderDate, 112) >= CONVERT(varchar(20), CAST('" + secondSlabFirstDate + "' as datetime) , 112) " +
            //    " AND CONVERT(varchar(20), a.OrderDate, 112) <= CONVERT(varchar(20), CAST('" + secondSlab + "' as datetime), 112 ) )" +
            //    " AND a.OrderStatus NOT IN (0, 2, 4) AND a.OrderPaymentTxnId IS NOT NULL AND a.OrderPaymentTxnId<>'' " +
            //    " AND (d.OPL_transtatus='SUCCESS' OR d.OPL_transtatus='paid')").ToString();
            //string secondSlabAmount = c.returnAggregate("Select SUM(a.OrderAmount) From OrdersData a Inner Join OrdersAssign b On a.OrderID=FK_OrderID " +
            //    " Inner Join online_payment_logs d On a.OrderPaymentTxnId=d.OPL_merchantTranId Where b.Fk_FranchID=" + fId +
            //    " AND ( CONVERT(varchar(20), a.OrderDate, 112) >= CONVERT(varchar(20), CAST('" + secondSlabFirstDate + "' as datetime) , 112) " +
            //    " AND CONVERT(varchar(20), a.OrderDate, 112) <= CONVERT(varchar(20), CAST('" + secondSlab + "' as datetime), 112 ) )" +
            //    " AND a.OrderStatus IN (7) AND a.OrderPaymentTxnId IS NOT NULL AND a.OrderPaymentTxnId<>'' " +
            //    " AND (d.OPL_transtatus='SUCCESS' OR d.OPL_transtatus='paid')").ToString();

            string secondSlabAmount = c.returnAggregateNew("Select SUM(d.OLP_RazorPayAmount) From online_payment_logs d Inner Join OrdersData a " +
                " On d.OLP_order_no=CAST(a.OrderID as varchar(50)) Inner Join OrdersAssign b On a.OrderID=b.FK_OrderID Where b.Fk_FranchID=" + fId +
                " AND ( CONVERT(varchar(20), a.OrderDate, 112) >= CONVERT(varchar(20), CAST('" + secondSlabFirstDate + "' as datetime) , 112) " +
                " AND CONVERT(varchar(20), a.OrderDate, 112) <= CONVERT(varchar(20), CAST('" + secondSlab + "' as datetime), 112 ) )" +
                " AND a.OrderStatus IN (7) AND b.OrdAssignStatus=7 AND b.OrdReAssign=0 AND a.OrderPaymentTxnId IS NOT NULL AND a.OrderPaymentTxnId<>'' " +
                " AND (d.OPL_transtatus='SUCCESS' OR d.OPL_transtatus='paid')").ToString();

            strMarkup.Append("<td>Rs. " + Convert.ToDouble(secondSlabAmount).ToString("0.00") + "</td>");
            string status2 = "";
            if (Convert.ToDouble(secondSlabAmount) > 0)
            {
                object payStatus2 = c.GetReqData("FranchiseePayout", "Payout_Status", "FK_FranchID=" + fId + " AND ( CONVERT(varchar(20), Payout_FromDate, 112) >= CONVERT(varchar(20), CAST('" + secondSlabFirstDate + "' as datetime) , 112) AND CONVERT(varchar(20), Payout_ToDate, 112) <= CONVERT(varchar(20), CAST('" + secondSlab + "' as datetime), 112 ) ) ");
                if (payStatus2 != DBNull.Value && payStatus2 != null && payStatus2.ToString() != "")
                {
                    switch (Convert.ToInt32(payStatus2))
                    {
                        case 1: status2 = "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Approved By Account</span><span class=\"space1\"></span>" +
                            "<span class=\"clrProcessing\">Pending From Payment</span><span class=\"space1\"></span>" +
                            "<span class=\"clrProcessing\">Pending From Rajah</span>";
                            break;
                        case 2: status2 = "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Approved By Account</span> <span class=\"space1\"></span>" +
                            "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Paid To Rajah</span><span class=\"space1\"></span>" +
                            "<span class=\"clrProcessing\">Pending From Rajah</span>";
                            break;
                        case 3: status2 = "<i class=\"fas fa-check\"></i> <span class=\"text-success text-bold\"> Approved By Account</span><span class=\"space1\"></span>" +
                            "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Paid To Rajah</span><span class=\"space1\"></span>" +
                            "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Received & Updated By Rajah</span>"; break;
                    }
                }
                else
                {
                    //status3 = "<span class=\"clrProcessing\">Pending For Approval From Account</span>";
                    status2 = "<span class=\"clrProcessing\">Pending For Approval From Account</span>" +
                        "<span class=\"clrProcessing\">Pending From Payment</span>" +
                        "<span class=\"clrProcessing\">Pending From Rajah</span>";
                }
            }
            strMarkup.Append("<td>" + status2 + "</td>");
            strMarkup.Append("</tr>");
            //third slab
            strMarkup.Append("<tr>");
            strMarkup.Append("<td>" + thirdSlabFirstDate.ToString("dd/MM/yyyy") + " - " + thirdSlab.ToString("dd/MM/yyyy") + "</td>");
            strMarkup.Append("<td>" + dayDiff + "</td>");
            //string thirdSlabAmount = c.returnAggregate("Select SUM(a.OrderAmount) From OrdersData a Inner Join OrdersAssign b On a.OrderID=FK_OrderID " +
            //    " Inner Join online_payment_logs d On a.OrderPaymentTxnId=d.OPL_merchantTranId Where b.Fk_FranchID=" + fId +
            //    " AND ( CONVERT(varchar(20), a.OrderDate, 112) >= CONVERT(varchar(20), CAST('" + thirdSlabFirstDate + "' as datetime) , 112) " +
            //    " AND CONVERT(varchar(20), a.OrderDate, 112) <= CONVERT(varchar(20), CAST('" + thirdSlab + "' as datetime), 112 ) )" +
            //    " AND a.OrderStatus NOT IN (0, 2, 4) AND a.OrderPaymentTxnId IS NOT NULL AND a.OrderPaymentTxnId<>'' " +
            //    " AND (d.OPL_transtatus='SUCCESS' OR d.OPL_transtatus='paid')").ToString();
            //string thirdSlabAmount = c.returnAggregate("Select SUM(a.OrderAmount) From OrdersData a Inner Join OrdersAssign b On a.OrderID=FK_OrderID " +
            //    " Inner Join online_payment_logs d On a.OrderPaymentTxnId=d.OPL_merchantTranId Where b.Fk_FranchID=" + fId +
            //    " AND ( CONVERT(varchar(20), a.OrderDate, 112) >= CONVERT(varchar(20), CAST('" + thirdSlabFirstDate + "' as datetime) , 112) " +
            //    " AND CONVERT(varchar(20), a.OrderDate, 112) <= CONVERT(varchar(20), CAST('" + thirdSlab + "' as datetime), 112 ) )" +
            //    " AND a.OrderStatus IN (7) AND a.OrderPaymentTxnId IS NOT NULL AND a.OrderPaymentTxnId<>'' " +
            //    " AND (d.OPL_transtatus='SUCCESS' OR d.OPL_transtatus='paid')").ToString();

            string thirdSlabAmount = c.returnAggregateNew("Select SUM(d.OLP_RazorPayAmount) From online_payment_logs d Inner Join OrdersData a " +
                " On d.OLP_order_no=CAST(a.OrderID as varchar(50)) Inner Join OrdersAssign b On a.OrderID=b.FK_OrderID Where b.Fk_FranchID=" + fId +
                " AND ( CONVERT(varchar(20), a.OrderDate, 112) >= CONVERT(varchar(20), CAST('" + thirdSlabFirstDate + "' as datetime) , 112) " +
                " AND CONVERT(varchar(20), a.OrderDate, 112) <= CONVERT(varchar(20), CAST('" + thirdSlab + "' as datetime), 112 ) )" +
                " AND a.OrderStatus IN (7) AND b.OrdAssignStatus=7 AND b.OrdReAssign=0 AND a.OrderPaymentTxnId IS NOT NULL AND a.OrderPaymentTxnId<>'' " +
                " AND (d.OPL_transtatus='SUCCESS' OR d.OPL_transtatus='paid')").ToString();

            strMarkup.Append("<td>Rs. " + Convert.ToDouble(thirdSlabAmount).ToString("0.00") + "</td>");
            string status3 = "";
            if (Convert.ToDouble(thirdSlabAmount) > 0)
            {
                object payStatus3 = c.GetReqData("FranchiseePayout", "Payout_Status", "FK_FranchID=" + fId + " AND ( CONVERT(varchar(20), Payout_FromDate, 112) >= CONVERT(varchar(20), CAST('" + thirdSlabFirstDate + "' as datetime) , 112) AND CONVERT(varchar(20), Payout_ToDate, 112) <= CONVERT(varchar(20), CAST('" + thirdSlab + "' as datetime), 112 ) ) ");
                if (payStatus3 != DBNull.Value && payStatus3 != null && payStatus3.ToString() != "")
                {
                    switch (Convert.ToInt32(payStatus3))
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
                    //status3 = "<span class=\"clrProcessing\">Pending For Approval From Account</span>";
                    status3 = "<span class=\"clrProcessing\">Pending For Approval From Account</span>" +
                        "<span class=\"clrProcessing\">Pending From Payment</span>" +
                        "<span class=\"clrProcessing\">Pending From Rajah</span>";
                }
            }
            strMarkup.Append("<td>" + status3 + "</td>");
            strMarkup.Append("</tr>");
            strMarkup.Append("</table>");

            monthwisePaidOrders = strMarkup.ToString();
        }
        catch (Exception ex)
        {
            monthwisePaidOrders = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}