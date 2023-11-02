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


public partial class account_online_payment_report_shopwise_detail : System.Web.UI.Page
{
    iClass c = new iClass();
    public string monthwisePaidOrders, repTitle;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["id"] != null)
            {
                viewOrder.Visible = false;
                monthwiseOrd.Visible = true;
                MonthBind();

                string recentOrderDate = "";
                DateTime recentMyDate = new DateTime();
                recentMyDate = DateTime.Now;

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
                        recentMyDate = Convert.ToDateTime(row["OrderDate"]);
                    }
                    else
                    {
                        recentOrderDate = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                }
                //ddrMonth.SelectedValue = Convert.ToDateTime(recentOrderDate).Month.ToString();
                int month = recentMyDate.Month;
                ddrMonth.SelectedValue = month.ToString();

                int year = recentMyDate.Year;
                //txtYear.Text = Convert.ToDateTime(recentOrderDate).Year.ToString();
                txtYear.Text = year.ToString();
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

    private void FillGrid()
    {
        try
        {
            //05/03/2022
            //using (DataTable dtPay = c.GetDataTable("Select Distinct a.FranchID, a.FranchName, a.FranchShopCode, " +
            //        " CAST( isnull( (Select SUM(opl.OLP_RazorPayAmount) From OrdersData od Inner Join OrdersAssign oa On od.OrderID=oa.FK_OrderID Inner Join online_payment_logs opl On CAST(od.OrderID as varchar(50))=opl.OLP_order_no  " +
            //        " Where oa.Fk_FranchID=a.FranchID AND od.OrderStatus=7 AND oa.OrdAssignStatus=7 AND oa.OrdReAssign=0 AND (od.OrderPaymentTxnId IS NOT NULL AND od.OrderPaymentTxnId<>'') AND (opl.OPL_transtatus='SUCCESS' OR opl.OPL_transtatus='paid') ), 0) AS float) - " +
            //        " CAST( isnull((Select SUM(fp.Payout_Amount) From FranchiseePayout fp Where fp.FK_FranchID=a.FranchID AND fp.Payout_Status=3 ), 0) AS float) as unpaidAmount " +
            //        " From FranchiseeData a Inner Join OrdersAssign b On a.FranchID=b.Fk_FranchID  " +
            //        " Inner Join OrdersData c On c.OrderID=b.FK_OrderID Inner Join online_payment_logs d On CAST(c.OrderID as varchar(50))=d.OLP_order_no " +
            //        " Where c.OrderStatus=7 AND b.OrdReAssign=0 AND b.OrdAssignStatus=7 AND " +
            //        " (c.OrderPaymentTxnId IS NOT NULL AND c.OrderPaymentTxnId<>'') AND (d.OPL_transtatus='SUCCESS' OR d.OPL_transtatus='paid') "))

            using (DataTable dtPay = c.GetDataTable("Select Distinct a.FranchID, a.FranchName, a.FranchShopCode, " +
                   " ROUND( isnull( (Select SUM(opl.OLP_RazorPayAmount) From OrdersData od Inner Join OrdersAssign oa On od.OrderID=oa.FK_OrderID Inner Join online_payment_logs opl On CAST(od.OrderID as varchar(50))=opl.OLP_order_no  " +
                   " Where oa.Fk_FranchID=a.FranchID AND od.OrderStatus=7 AND oa.OrdAssignStatus=7 AND oa.OrdReAssign=0 AND (od.OrderPaymentTxnId IS NOT NULL AND od.OrderPaymentTxnId<>'') AND (opl.OPL_transtatus='SUCCESS' OR opl.OPL_transtatus='paid') ), 0), 2) - " +
                   " ROUND( isnull((Select SUM(fp.Payout_Amount) From FranchiseePayout fp Where fp.FK_FranchID=a.FranchID AND fp.Payout_Status=3 ), 0), 2) as unpaidAmount " +
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
                litView.Text = "<a href=\"online-payment-report-shopwise-detail.aspx?id=" + e.Row.Cells[0].Text + "\" class=\"gView\"></a>";
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
            string shopName = c.GetReqData("FranchiseeData", "FranchName", "FranchID=" + Request.QueryString["id"]).ToString();
            repTitle = "Showing Report Of " + ddrMonth.SelectedItem.Text + " " + txtYear.Text + " Of " + shopName;
            //repTitle = "Showing Report Of " + ddrMonth.SelectedItem.Text + " " + txtYear.Text;
            var lastDay = DateTime.DaysInMonth(year, month);

            DateTime firstDayDate = new DateTime(year, month, 1);
            DateTime lastDayDate = new DateTime(year, month, lastDay);

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
            strMarkup.Append("<td class=\"semiMedium text-bold\">Date</td>");
            strMarkup.Append("<td class=\"semiMedium text-bold\">Paid Amount</td>");
            strMarkup.Append("<td class=\"semiMedium text-bold\">Status</td>");
            strMarkup.Append("<td class=\"semiMedium text-bold\"></td>");
            strMarkup.Append("</tr>");

            using (DataTable dtPayment = c.GetDataTable("Select a.OrderDate, d.OLP_RazorPayAmount, d.OLP_order_no From online_payment_logs d Inner Join OrdersData a " +
                " On d.OLP_order_no=CAST(a.OrderID as varchar(50)) Inner Join OrdersAssign b On a.OrderID=b.FK_OrderID Where b.Fk_FranchID=" + fId +
                " AND ( CONVERT(varchar(20), a.OrderDate, 112) >= CONVERT(varchar(20), CAST('" + firstDayDate + "' as datetime) , 112) " +
                " AND CONVERT(varchar(20), a.OrderDate, 112) <= CONVERT(varchar(20), CAST('" + lastDayDate + "' as datetime), 112 ) )" +
                " AND a.OrderStatus IN (7) AND b.OrdAssignStatus=7 AND b.OrdReAssign=0 AND a.OrderPaymentTxnId IS NOT NULL AND a.OrderPaymentTxnId<>'' " +
                " AND (d.OPL_transtatus='SUCCESS' OR d.OPL_transtatus='paid') order by a.OrderDate"))
            {
                if (dtPayment.Rows.Count > 0)
                {
                    foreach (DataRow row in dtPayment.Rows)
                    {
                        strMarkup.Append("<tr>");
                        strMarkup.Append("<td>" + row["OrderDate"].ToString() + "</td>");
                        strMarkup.Append("<td>" + Convert.ToDouble(row["OLP_RazorPayAmount"]).ToString("0.00") + "</td>");
                       
                        // =============== Status Markup Starts ==============
                        double firstSlabAmt = Convert.ToDouble(row["OLP_RazorPayAmount"]);
                        string orderNumber = row["OLP_order_no"].ToString();

                        string actionButton1 = "", status1 = "";

                        string strRajah = "-";
                        string shopCode = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + fId).ToString();
                        if (shopCode.Contains("GMMH"))
                        {
                            strRajah = "Rajah";
                        }
                        else
                        {
                            strRajah = "Karnataka Godown";
                        }

                        if (firstSlabAmt > 0)
                        {
                            if (c.IsRecordExist("Select Payout_ID From FranchiseePayout Where FK_FranchID=" + fId +
                                " AND CONVERT(varchar(20), Payout_FromDate, 112) = CONVERT(varchar(20), CAST('" + row["OrderDate"] + "' as datetime) , 112) " +
                                " AND Payout_Status=1 And Payout_Amount=" + firstSlabAmt + " AND (Payout_AccActivityDate IS NOT NULL OR Payout_AccActivityDate<>'')"))
                            {
                                object distId = c.GetReqData("FranchiseePayout", "FK_DistributorID", "FK_FranchID=" + fId + " AND ( CONVERT(varchar(20), Payout_FromDate, 112) = CONVERT(varchar(20), CAST('" + row["OrderDate"] + "' as datetime) , 112)) AND Payout_Status=1 And Payout_Amount=" + firstSlabAmt + " AND (Payout_AccActivityDate IS NOT NULL OR Payout_AccActivityDate<>'')");
                                if (distId != DBNull.Value && distId != null && distId.ToString() != "" && distId.ToString() != "0")
                                {
                                    if (Convert.ToInt32(distId) == 1)
                                        strRajah = "Rajah";
                                    else
                                        strRajah = "Karnataka Godown";
                                }

                                status1 = "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Approved By Account</span><span class=\"space1\"></span>" +
                                            "<span class=\"clrProcessing\">Pending From Payment</span><span class=\"space1\"></span>" +
                                            "<span class=\"clrProcessing\">Pending From " + strRajah + "</span>";
                            }
                            else
                            {
                                object payStatus = c.GetReqData("FranchiseePayout", "Payout_Status", "FK_FranchID=" + fId + " AND ( CONVERT(varchar(20), Payout_FromDate, 112) = CONVERT(varchar(20), CAST('" + row["OrderDate"] + "' as datetime) , 112)) And Payout_Amount=" + firstSlabAmt + "");

                                object distId = c.GetReqData("FranchiseePayout", "FK_DistributorID", "FK_FranchID=" + fId + " AND ( CONVERT(varchar(20), Payout_FromDate, 112) = CONVERT(varchar(20), CAST('" + row["OrderDate"] + "' as datetime) , 112)) And Payout_Amount=" + firstSlabAmt + "");
                                if (distId != DBNull.Value && distId != null && distId.ToString() != "" && distId.ToString() != "0")
                                {
                                    if (Convert.ToInt32(distId) == 1)
                                        strRajah = "Rajah";
                                    else
                                        strRajah = "Karnataka Godown";
                                }

                                if (payStatus != DBNull.Value && payStatus != null && payStatus.ToString() != "")
                                {
                                    switch (Convert.ToInt32(payStatus))
                                    {
                                        case 1: status1 = "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Approved By Account</span><span class=\"space1\"></span>" +
                                            "<span class=\"clrProcessing\">Pending From Payment</span><span class=\"space1\"></span>" +
                                            "<span class=\"clrProcessing\">Pending From " + strRajah + "</span>";
                                            break;
                                        case 2: status1 = "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Approved By Account</span> <span class=\"space1\"></span>" +
                                            "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Paid To " + strRajah + "</span><span class=\"space1\"></span>" +
                                            "<span class=\"clrProcessing\">Pending From " + strRajah + "</span>";
                                            break;
                                        case 3: status1 = "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Approved By Account</span> <span class=\"space1\"></span>" +
                                            "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Paid To " + strRajah + "</span><span class=\"space1\"></span>" +
                                            "<i class=\"fas fa-check\"></i><span class=\"text-success text-bold\"> Received & Updated By " + strRajah + "</span>"; break;
                                    }
                                }
                                else
                                {
                                    status1 = "<span class=\"clrProcessing\">Pending For Approval From Account</span>" +
                                    "<span class=\"clrProcessing\">Pending From Payment</span>" +
                                    "<span class=\"clrProcessing\">Pending From " + strRajah + "</span>";

                                    //string date1 = firstDayDate.ToString("dd/MM/yyyy") + " - " + firstDayDate.ToString("dd/MM/yyyy");
                                    DateTime myDate = Convert.ToDateTime(row["OrderDate"].ToString());
                                    string date1 = myDate.ToString("dd/MM/yyyy") + " - " + myDate.ToString("dd/MM/yyyy");

                                    actionButton1 = "<span class=\"btn btn-md btn-success\" onclick=\"ApprovePayment('" + date1 + "', '" + Request.QueryString["id"] + "', '" + firstSlabAmt.ToString("0.00") + "', '" + orderNumber + "'); this.disabled = true;\">Approve</span>";
                                }
                            }
                        }

                        strMarkup.Append("<td>" + status1 + "</td>");
                        // =============== Status Markup Ends ==============

                        strMarkup.Append("<td>" + actionButton1 + "</td>");

                        strMarkup.Append("</tr>");

                    }
                }
            }
            
            strMarkup.Append("</table>");

            strMarkup.Append("<span class=\"space10\"></span>");
            strMarkup.Append("<a href=\"online-payment-report-shopwise-detail.aspx\" class=\"btn btn-md btn-dark\">Back</a>");

            monthwisePaidOrders = strMarkup.ToString();
        }
        catch (Exception ex)
        {
            monthwisePaidOrders = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    [WebMethod]
    public static int UpdatePayout(string dates, string franchId, string ordAmount, string orderNumber)
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

            int distId = 0;
            string shopCode = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + franchId).ToString();
            if (shopCode.Contains("GMMH"))
            {
                distId = 1;
            }
            else
            {
                distId = 2;
            }

            int payStatusFlag = 1;
            // Check is this Company own shop, if YES then directly make its payStatus=3, else 1 (Pn request of Anand Sir-Accounts Dept) 1/Nov/2023
            // i.e. Approval from all Depts. ACCOUNT, PURCHASE & RAJAH at one click
            if (c.IsRecordExist("Select CSID From CompanyOwnShops where FK_FranchID=" + franchId) == true)
            { 
                payStatusFlag = 3;
                c.ExecuteQuery("Insert Into FranchiseePayout(Payout_ID, FK_FranchID, Payout_FromDate, Payout_ToDate, Payout_Amount, " +
                " Payout_Status, Payout_AccActivityDate, Payout_PurchActivityDate, Payout_RajahActivityDate, DelMark, Payout_Order_Number, FK_DistributorID) Values (" + maxId + ", " + franchId + ", '" + fDate +
                "', '" + tDate + "', " + ordAmount + ", " + payStatusFlag + ", '" + DateTime.Now + "', '" + DateTime.Now + "', '" + DateTime.Now + "', 0, '" + orderNumber + "', " + distId + ")");
            }
            else
            { 
            c.ExecuteQuery("Insert Into FranchiseePayout(Payout_ID, FK_FranchID, Payout_FromDate, Payout_ToDate, Payout_Amount, " +
                " Payout_Status, Payout_AccActivityDate, DelMark, Payout_Order_Number, FK_DistributorID) Values (" + maxId + ", " + franchId + ", '" + fDate +
                "', '" + tDate + "', " + ordAmount + ", " + payStatusFlag + ", '" + DateTime.Now + "', 0, '" + orderNumber + "', " + distId + ")");
            }
            return 1;
        }
        catch (Exception)
        {
            return 3;
        }
    }
}