using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.IO;
using System.Drawing;
using System.Configuration;

public partial class bdm_export_excel_files : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private void ExportOrdersToExcel()
    {
        try
        {
            if (txtFromDate.Text == "" && txtToDate.Text == "")
            {
                errMsg = c.ErrNotification(2, "Select Date Range To Export Report");
                return;
            }

            // From Date
            DateTime ordFromDate = DateTime.Now;
            string[] arrfrDate = txtFromDate.Text.Split('/');
            if (c.IsDate(arrfrDate[1] + "/" + arrfrDate[0] + "/" + arrfrDate[2]) == false)
            {
                errMsg = c.ErrNotification(2, "Enter Valid From Date");
                return;
            }
            else
            {
                ordFromDate = Convert.ToDateTime(arrfrDate[1] + "/" + arrfrDate[0] + "/" + arrfrDate[2]);
            }

            // To Date
            DateTime ordToDate = DateTime.Now;
            string[] arrtoDate = txtToDate.Text.Split('/');
            if (c.IsDate(arrtoDate[1] + "/" + arrtoDate[0] + "/" + arrtoDate[2]) == false)
            {
                errMsg = c.ErrNotification(2, "Enter Valid To Date");
                return;
            }
            else
            {
                ordToDate = Convert.ToDateTime(arrtoDate[1] + "/" + arrtoDate[0] + "/" + arrtoDate[2]);
            }

            // This actually makes your HTML output to be downloaded as .xls file
            Response.Clear();
            //Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=OrdersReport.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //Get Orders in data table
                //string orderQuery = "Select a.OrderID, a.OrderDate, a.OrderAmount, d.CustomerName, d.CustomerMobile, b.FranchName, b.FranchShopCode, " +
                //    " Case When a.OrderStatus = 1 then 'New' When a.OrderStatus = 2 then 'Cancel By Cust' When a.OrderStatus = 3 then 'Accepted by admin' When a.OrderStatus = 4 then 'Denied by admin' " +
                //    " When a.OrderStatus = 5 then 'Inprocess' When a.OrderStatus = 6 then 'Shipped' When a.OrderStatus = 7 then 'Delivered' " +
                //    " When a.OrderStatus = 8 then 'Rejected by GMMH0001' When a.OrderStatus = 9 then 'Rejected by shop (Low Order Amount)' End as adminstatus, " +
                //    " Case When c.OrdAssignStatus = 0 then 'Pending' When c.OrdAssignStatus = 1 then 'Accepted' When c.OrdAssignStatus = 2 then 'Rejected' " +
                //    " When c.OrdAssignStatus = 5 then 'Inprocess' When c.OrdAssignStatus = 6 then 'Shipped' When c.OrdAssignStatus = 7 then 'Delivered' End as Shopstatus, " +
                //    " Case When a.OrderType = 1 then 'Regular Order' When a.OrderType = 2 then 'Prescription Order' End as OrderType, " +
                //    " Case When MreqFlag = 1 then 'Yes' Else '-' End as MonthlyOrder, a.DeviceType " +
                //    " From OrdersData a Left Join OrdersAssign c On a.OrderID = c.FK_OrderID " +
                //    " Left Join FranchiseeData b On c.Fk_FranchID = b.FranchID Left Join CustomersData d On a.FK_OrderCustomerID = d.CustomrtID " +
                //    " Where(a.OrderStatus <> 0 AND a.FK_OrderCustomerID <> 0) AND " +
                //    " (CONVERT(varchar(20), a.OrderDate, 112) >= CONVERT(varchar(20), CAST('" + ordFromDate + "' as datetime), 112)) AND " +
                //    " (CONVERT(varchar(20), a.OrderDate, 112) <= CONVERT(varchar(20), CAST('" + ordToDate + "' as datetime), 112)) " +
                //    " Order By a.OrderDate DESC";

                string orderQuery = "Select a.OrderID, a.OrderDate, a.OrderAmount, d.CustomerName, d.CustomerMobile, b.FranchName, b.FranchShopCode, " +
                   " Case When a.OrderStatus = 1 then 'New' When a.OrderStatus = 2 then 'Cancel By Cust' When a.OrderStatus = 3 then 'Accepted by admin' When a.OrderStatus = 4 then 'Denied by admin' " +
                   " When a.OrderStatus = 5 then 'Inprocess' When a.OrderStatus = 6 then 'Shipped' When a.OrderStatus = 7 then 'Delivered' " +
                   " When a.OrderStatus = 8 then 'Rejected by GMMH0001' When a.OrderStatus = 9 then 'Rejected by shop (Low Order Amount)' When a.OrderStatus = 10 then 'Returned by Customer'" +
                   " When a.OrderStatus = 11 then 'Rejected by Doctor' When a.OrderStatus = 12 then 'No Response to Call' When a.OrderStatus = 13 then 'Refund Request by Customer' End as adminstatus, " +
                   " Case When c.OrdAssignStatus = 0 then 'Pending' When c.OrdAssignStatus = 1 then 'Accepted' When c.OrdAssignStatus = 2 then 'Rejected' " +
                   " When c.OrdAssignStatus = 5 then 'Inprocess' When c.OrdAssignStatus = 6 then 'Shipped' When c.OrdAssignStatus = 7 then 'Delivered' End as Shopstatus, " +
                   " Case When a.OrderType = 1 then 'Regular Order' When a.OrderType = 2 then 'Prescription Order' End as OrderType, " +
                   " Case When MreqFlag = 1 then 'Yes' Else '-' End as MonthlyOrder, a.DeviceType " +
                   " From OrdersData a Left Join OrdersAssign c On a.OrderID = c.FK_OrderID " +
                   " Left Join FranchiseeData b On c.Fk_FranchID = b.FranchID Left Join CustomersData d On a.FK_OrderCustomerID = d.CustomrtID " +
                   " Where(a.OrderStatus <> 0 AND a.FK_OrderCustomerID <> 0) AND " +
                   " (CONVERT(varchar(20), a.OrderDate, 112) >= CONVERT(varchar(20), CAST('" + ordFromDate + "' as datetime), 112)) AND " +
                   " (CONVERT(varchar(20), a.OrderDate, 112) <= CONVERT(varchar(20), CAST('" + ordToDate + "' as datetime), 112)) " +
                   " Order By a.OrderDate DESC";


                using (DataTable dtOrders = c.GetDataTable(orderQuery))
                {
                    if (dtOrders.Rows.Count > 0)
                    {
                        gvOrder.DataSource = dtOrders;
                        gvOrder.DataBind();

                        // To export all pages
                        gvOrder.AllowPaging = false;

                        gvOrder.HeaderRow.BackColor = Color.Black;
                        gvOrder.HeaderRow.ForeColor = Color.White;


                        foreach (TableCell cell in gvOrder.HeaderRow.Cells)
                        {
                            cell.BackColor = gvOrder.HeaderStyle.BackColor;
                        }

                        foreach (GridViewRow row in gvOrder.Rows)
                        {
                            row.BackColor = Color.White;
                            row.BorderColor = Color.LightGray;
                            row.BorderWidth = Unit.Parse("1px");
                            foreach (TableCell cell in row.Cells)
                            {
                                if (row.RowIndex % 2 == 0)
                                {
                                    cell.BackColor = Color.WhiteSmoke;
                                }
                                else
                                {
                                    cell.BackColor = Color.White;
                                }
                            }
                        }

                        gvOrder.RenderControl(hw);

                        //style to format numbers to string
                        //string style = @"<style> .textmode { } </style>";
                        //Response.Write(style);
                        Response.Output.Write(sw.ToString());
                        Response.Flush();
                        Response.End();
                    }
                }
            }

            txtFromDate.Text = txtToDate.Text = "";
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

    protected void btnExportOrders_Click(object sender, EventArgs e)
    {
        ExportOrdersToExcel();
    }

    protected void btnExportEnq_Click(object sender, EventArgs e)
    {
        ExportEnquiriesToExcel();
    }

    private void ExportEnquiriesToExcel()
    {
        try
        {
            if (txtEnqFromDate.Text == "" && txtEnqToDate.Text == "")
            {
                errMsg = c.ErrNotification(2, "Select Date Range To Export Report");
                return;
            }

            // From Date
            DateTime enqFromDate = DateTime.Now;
            string[] arrfrDate = txtEnqFromDate.Text.Split('/');
            if (c.IsDate(arrfrDate[1] + "/" + arrfrDate[0] + "/" + arrfrDate[2]) == false)
            {
                errMsg = c.ErrNotification(2, "Enter Valid From Date");
                return;
            }
            else
            {
                enqFromDate = Convert.ToDateTime(arrfrDate[1] + "/" + arrfrDate[0] + "/" + arrfrDate[2]);
            }

            // To Date
            DateTime enqToDate = DateTime.Now;
            string[] arrtoDate = txtEnqToDate.Text.Split('/');
            if (c.IsDate(arrtoDate[1] + "/" + arrtoDate[0] + "/" + arrtoDate[2]) == false)
            {
                errMsg = c.ErrNotification(2, "Enter Valid To Date");
                return;
            }
            else
            {
                enqToDate = Convert.ToDateTime(arrtoDate[1] + "/" + arrtoDate[0] + "/" + arrtoDate[2]);
            }

            // This actually makes your HTML output to be downloaded as .xls file
            Response.Clear();
            //Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=EnquiryReport.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";

            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //Get Orders in data table
                string orderQuery = "Select DISTINCT a.CalcID, a.CalcDate, b.CustomerName, b.CustomerMobile, isnull(a.DeviceType, '-') as DeviceType, " +
                    " Case When a.EnqStatus = 0 then 'Incomplete' When a.EnqStatus = 1 then 'New' When a.EnqStatus = 2 then 'Accepted & Assigned' " +
                    " When a.EnqStatus = 3 then 'Converted' When a.EnqStatus = 4 then 'Not Converted' When a.EnqStatus = 5 then 'Inprocess' " +
                    " When a.EnqStatus = 6 then 'Dispatched' When a.EnqStatus = 7 then 'Delivered' End as EnqStatus, " +
                    " Case When MreqFlag = 1 then 'Yes' Else '-' End as MonthlyEnq " +
                    " From SavingCalc a Left Join CustomersData b on a.FK_CustId = b.CustomrtID Where a.FK_CustId <> 0  AND " +
                    " (CONVERT(varchar(20), a.CalcDate, 112) >= CONVERT(varchar(20), CAST('" + enqFromDate + "' as datetime), 112)) AND " +
                    " (CONVERT(varchar(20), a.CalcDate, 112) <= CONVERT(varchar(20), CAST('" + enqToDate + "' as datetime), 112)) " +
                    " Order By a.CalcDate DESC, a.CalcID DESC";


                using (DataTable dtEnq = c.GetDataTable(orderQuery))
                {
                    if (dtEnq.Rows.Count > 0)
                    {
                        gvEnq.DataSource = dtEnq;
                        gvEnq.DataBind();

                        // To export all pages
                        gvEnq.AllowPaging = false;

                        gvEnq.HeaderRow.BackColor = Color.Black;
                        gvEnq.HeaderRow.ForeColor = Color.White;


                        foreach (TableCell cell in gvEnq.HeaderRow.Cells)
                        {
                            cell.BackColor = gvEnq.HeaderStyle.BackColor;
                        }

                        foreach (GridViewRow row in gvEnq.Rows)
                        {
                            row.BackColor = Color.White;
                            row.BorderColor = Color.LightGray;
                            row.BorderWidth = Unit.Parse("1px");
                            foreach (TableCell cell in row.Cells)
                            {
                                if (row.RowIndex % 2 == 0)
                                {
                                    cell.BackColor = Color.WhiteSmoke;
                                }
                                else
                                {
                                    cell.BackColor = Color.White;
                                }
                            }
                        }

                        gvEnq.RenderControl(hw);

                        //style to format numbers to string
                        //string style = @"<style> .textmode { } </style>";
                        //Response.Write(style);
                        Response.Output.Write(sw.ToString());
                        Response.Flush();
                        Response.End();
                    }
                }
            }

            txtEnqFromDate.Text = txtEnqToDate.Text = "";
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }


    protected void btnExportCustomers_Click(object sender, EventArgs e)
    {
        ExportCustomersToExcel();
    }

    private void ExportCustomersToExcel()
    {
        try
        {
            if (txtCustFrDate.Text == "" && txtCustToDate.Text == "")
            {
                errMsg = c.ErrNotification(2, "Select Date Range To Export Report");
                return;
            }

            // From Date
            DateTime custFromDate = DateTime.Now;
            string[] arrfrDate = txtCustFrDate.Text.Split('/');
            if (c.IsDate(arrfrDate[1] + "/" + arrfrDate[0] + "/" + arrfrDate[2]) == false)
            {
                errMsg = c.ErrNotification(2, "Enter Valid From Date");
                return;
            }
            else
            {
                custFromDate = Convert.ToDateTime(arrfrDate[1] + "/" + arrfrDate[0] + "/" + arrfrDate[2]);
            }

            // To Date
            DateTime custToDate = DateTime.Now;
            string[] arrtoDate = txtCustToDate.Text.Split('/');
            if (c.IsDate(arrtoDate[1] + "/" + arrtoDate[0] + "/" + arrtoDate[2]) == false)
            {
                errMsg = c.ErrNotification(2, "Enter Valid To Date");
                return;
            }
            else
            {
                custToDate = Convert.ToDateTime(arrtoDate[1] + "/" + arrtoDate[0] + "/" + arrtoDate[2]);
            }


            // This actually makes your HTML output to be downloaded as .xls file
            Response.Clear();
            //Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=RegisteredCustomerReport.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";

            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //Get customers in data table
                string custQuery = "Select a.CustomrtID, a.CustomerJoinDate, a.CustomerName, a.CustomerMobile, a.CustomerEmail, a.DeviceType From CustomersData a Where" +
                    " (CONVERT(varchar(20), a.CustomerJoinDate, 112) >= CONVERT(varchar(20), CAST('" + custFromDate + "' as datetime), 112)) AND " +
                    " (CONVERT(varchar(20), a.CustomerJoinDate, 112) <= CONVERT(varchar(20), CAST('" + custToDate + "' as datetime), 112)) " +
                    " Order By a.CustomerJoinDate DESC";


                using (DataTable dtCust = c.GetDataTable(custQuery))
                {
                    if (dtCust.Rows.Count > 0)
                    {
                        gvCust.DataSource = dtCust;
                        gvCust.DataBind();

                        // To export all pages
                        gvCust.AllowPaging = false;

                        gvCust.HeaderRow.BackColor = Color.Black;
                        gvCust.HeaderRow.ForeColor = Color.White;


                        foreach (TableCell cell in gvCust.HeaderRow.Cells)
                        {
                            cell.BackColor = gvCust.HeaderStyle.BackColor;
                        }

                        foreach (GridViewRow row in gvCust.Rows)
                        {
                            row.BackColor = Color.White;
                            row.BorderColor = Color.LightGray;
                            row.BorderWidth = Unit.Parse("1px");
                            foreach (TableCell cell in row.Cells)
                            {
                                if (row.RowIndex % 2 == 0)
                                {
                                    cell.BackColor = Color.WhiteSmoke;
                                }
                                else
                                {
                                    cell.BackColor = Color.White;
                                }
                            }
                        }

                        gvCust.RenderControl(hw);

                        //style to format numbers to string
                        //string style = @"<style> .textmode { } </style>";
                        //Response.Write(style);
                        Response.Output.Write(sw.ToString());
                        Response.Flush();
                        Response.End();
                    }
                }
            }

            txtCustFrDate.Text = txtCustToDate.Text = "";
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    protected void btnExportCallList_Click(object sender, EventArgs e)
    {
        ExportCallMeList();
    }

    private void ExportCallMeList()
    {
        try
        {
            if (txtCallFrDate.Text == "" && txtCallToDate.Text == "")
            {
                errMsg = c.ErrNotification(2, "Select Date Range To Export Report");
                return;
            }

            // From Date
            DateTime callFromDate = DateTime.Now;
            string[] arrfrDate = txtCallFrDate.Text.Split('/');
            if (c.IsDate(arrfrDate[1] + "/" + arrfrDate[0] + "/" + arrfrDate[2]) == false)
            {
                errMsg = c.ErrNotification(2, "Enter Valid From Date");
                return;
            }
            else
            {
                callFromDate = Convert.ToDateTime(arrfrDate[1] + "/" + arrfrDate[0] + "/" + arrfrDate[2]);
            }

            // To Date
            DateTime callToDate = DateTime.Now;
            string[] arrtoDate = txtCallToDate.Text.Split('/');
            if (c.IsDate(arrtoDate[1] + "/" + arrtoDate[0] + "/" + arrtoDate[2]) == false)
            {
                errMsg = c.ErrNotification(2, "Enter Valid To Date");
                return;
            }
            else
            {
                callToDate = Convert.ToDateTime(arrtoDate[1] + "/" + arrtoDate[0] + "/" + arrtoDate[2]);
            }


            // This actually makes your HTML output to be downloaded as .xls file
            Response.Clear();
            //Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=CallMeReport.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";

            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //Get customers in data table
                string callQuery = "Select RequestDate, RequestName, RequestMobile, RequestMedicine, DeviceType From SavingCalcRequest Where" +
                    " (CONVERT(varchar(20), RequestDate, 112) >= CONVERT(varchar(20), CAST('" + callFromDate + "' as datetime), 112)) AND " +
                    " (CONVERT(varchar(20), RequestDate, 112) <= CONVERT(varchar(20), CAST('" + callToDate + "' as datetime), 112)) " +
                    " Order By RequestDate DESC";


                using (DataTable dtCallme = c.GetDataTable(callQuery))
                {
                    if (dtCallme.Rows.Count > 0)
                    {
                        gvCallMe.DataSource = dtCallme;
                        gvCallMe.DataBind();

                        // To export all pages
                        gvCallMe.AllowPaging = false;

                        gvCallMe.HeaderRow.BackColor = Color.Black;
                        gvCallMe.HeaderRow.ForeColor = Color.White;


                        foreach (TableCell cell in gvCallMe.HeaderRow.Cells)
                        {
                            cell.BackColor = gvCallMe.HeaderStyle.BackColor;
                        }

                        foreach (GridViewRow row in gvCallMe.Rows)
                        {
                            row.BackColor = Color.White;
                            row.BorderColor = Color.LightGray;
                            row.BorderWidth = Unit.Parse("1px");
                            foreach (TableCell cell in row.Cells)
                            {
                                if (row.RowIndex % 2 == 0)
                                {
                                    cell.BackColor = Color.WhiteSmoke;
                                }
                                else
                                {
                                    cell.BackColor = Color.White;
                                }
                            }
                        }

                        gvCallMe.RenderControl(hw);

                        //style to format numbers to string
                        //string style = @"<style> .textmode { } </style>";
                        //Response.Write(style);
                        Response.Output.Write(sw.ToString());
                        Response.Flush();
                        Response.End();
                    }
                }
            }

            txtCallFrDate.Text = txtCallToDate.Text = "";
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
    protected void btnExportNotOrderedList_Click(object sender, EventArgs e)
    {
        ExportNotOrderedCustomersToExcel();
    }

    private void ExportNotOrderedCustomersToExcel()
    {
        try
        {
            if (txtNStartDate.Text == "" && txtNEndDate.Text == "")
            {
                errMsg = c.ErrNotification(2, "Select Date Range To Export Report");
                return;
            }

            // From Date
            DateTime custFromDate = DateTime.Now;
            string[] arrfrDate = txtNStartDate.Text.Split('/');
            if (c.IsDate(arrfrDate[1] + "/" + arrfrDate[0] + "/" + arrfrDate[2]) == false)
            {
                errMsg = c.ErrNotification(2, "Enter Valid From Date");
                return;
            }
            else
            {
                custFromDate = Convert.ToDateTime(arrfrDate[1] + "/" + arrfrDate[0] + "/" + arrfrDate[2]);
            }

            // To Date
            DateTime custToDate = DateTime.Now;
            string[] arrtoDate = txtNEndDate.Text.Split('/');
            if (c.IsDate(arrtoDate[1] + "/" + arrtoDate[0] + "/" + arrtoDate[2]) == false)
            {
                errMsg = c.ErrNotification(2, "Enter Valid To Date");
                return;
            }
            else
            {
                custToDate = Convert.ToDateTime(arrtoDate[1] + "/" + arrtoDate[0] + "/" + arrtoDate[2]);
            }


            // This actually makes your HTML output to be downloaded as .xls file
            Response.Clear();
            //Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=RegisteredNotOrderedCustomerReport.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";

            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //Get customers in data table
                string custQuery = "Select a.CustomrtID, a.CustomerJoinDate, a.CustomerName, a.CustomerMobile, a.CustomerEmail, a.DeviceType From CustomersData a Where" +
                    " (CONVERT(varchar(20), a.CustomerJoinDate, 112) >= CONVERT(varchar(20), CAST('" + custFromDate + "' as datetime), 112)) AND " +
                    " (CONVERT(varchar(20), a.CustomerJoinDate, 112) <= CONVERT(varchar(20), CAST('" + custToDate + "' as datetime), 112)) " +
                    " AND a.CustomrtID NOT IN (Select Distinct FK_OrderCustomerID From OrdersData) " +
                    " Order By a.CustomerJoinDate DESC";


                using (DataTable dtCust = c.GetDataTable(custQuery))
                {
                    if (dtCust.Rows.Count > 0)
                    {
                        gvNotOrdCust.DataSource = dtCust;
                        gvNotOrdCust.DataBind();

                        // To export all pages
                        gvNotOrdCust.AllowPaging = false;

                        gvNotOrdCust.HeaderRow.BackColor = Color.Black;
                        gvNotOrdCust.HeaderRow.ForeColor = Color.White;


                        foreach (TableCell cell in gvNotOrdCust.HeaderRow.Cells)
                        {
                            cell.BackColor = gvNotOrdCust.HeaderStyle.BackColor;
                        }

                        foreach (GridViewRow row in gvNotOrdCust.Rows)
                        {
                            row.BackColor = Color.White;
                            row.BorderColor = Color.LightGray;
                            row.BorderWidth = Unit.Parse("1px");
                            foreach (TableCell cell in row.Cells)
                            {
                                if (row.RowIndex % 2 == 0)
                                {
                                    cell.BackColor = Color.WhiteSmoke;
                                }
                                else
                                {
                                    cell.BackColor = Color.White;
                                }
                            }
                        }

                        gvNotOrdCust.RenderControl(hw);

                        //style to format numbers to string
                        //string style = @"<style> .textmode { } </style>";
                        //Response.Write(style);
                        Response.Output.Write(sw.ToString());
                        Response.Flush();
                        Response.End();
                    }
                }
            }

            txtNStartDate.Text = txtNEndDate.Text = "";
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
    protected void btnObpOrders_Click(object sender, EventArgs e)
    {
        ExportOBPOrdersToExcel();
    }

    private void ExportOBPOrdersToExcel()
    {
        try
        {
            if (txtObpStartDate.Text == "" && txtObpEndDate.Text == "")
            {
                errMsg = c.ErrNotification(2, "Select Date Range To Export Report");
                return;
            }

            // From Date
            DateTime obpFromDate = DateTime.Now;
            string[] arrfrDate = txtObpStartDate.Text.Split('/');
            if (c.IsDate(arrfrDate[1] + "/" + arrfrDate[0] + "/" + arrfrDate[2]) == false)
            {
                errMsg = c.ErrNotification(2, "Enter Valid From Date");
                return;
            }
            else
            {
                obpFromDate = Convert.ToDateTime(arrfrDate[1] + "/" + arrfrDate[0] + "/" + arrfrDate[2]);
            }

            // To Date
            DateTime obpToDate = DateTime.Now;
            string[] arrtoDate = txtObpEndDate.Text.Split('/');
            if (c.IsDate(arrtoDate[1] + "/" + arrtoDate[0] + "/" + arrtoDate[2]) == false)
            {
                errMsg = c.ErrNotification(2, "Enter Valid To Date");
                return;
            }
            else
            {
                obpToDate = Convert.ToDateTime(arrtoDate[1] + "/" + arrtoDate[0] + "/" + arrtoDate[2]);
            }


            // This actually makes your HTML output to be downloaded as .xls file
            Response.Clear();
            //Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=OBP_Orders_Report.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";

            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                
                string obpQuery = "Select Distinct a.OrderID, a.OrderDate, a.OrderAmount, d.CustomerName, d.CustomerMobile, " +
                    " isnull(b.FranchName, 'NA') as FranchName, isnull(b.FranchShopCode, 'NA') as FranchShopCode, " +
                    " Case When a.OrderStatus=1 then 'New' When a.OrderStatus=2 then 'Cancel By Cust' When a.OrderStatus=3 " +
                    " then 'Accepted by admin' When a.OrderStatus=4 then 'Denied by admin' When a.OrderStatus=5 then 'Inprocess' " +
                    " When a.OrderStatus=6 then 'Shipped' When a.OrderStatus=7 then 'Delivered' When a.OrderStatus=8 " +
                    " then 'Rejected by GMMH0001' When a.OrderStatus=9 then 'Rejected by shop (Low Order Amount)' End as adminstatus," +
                    " Case When c.OrdAssignStatus=0 then 'Pending' When c.OrdAssignStatus=1 then 'Accepted' When c.OrdAssignStatus=2 then 'Rejected' " +
                    " When c.OrdAssignStatus=5 then 'Inprocess' When c.OrdAssignStatus=6 then 'Shipped' When c.OrdAssignStatus=7 then 'Delivered' " +
                    " Else 'NA' End as Shopstatus, " +
                    " Case  When a.OrderType=1 then 'Regular Order' When a.OrderType=2 then 'Prescription Order' End as OrderType, " +
                    " Case When MreqFlag=1 then 'Yes' Else '-' End as MonthlyOrder, " +
                    " Case When a.OrderPayMode=1 then 'COD' Else 'Online Payment' End as PaymentMode, " +
                    " Case When a.OrderPayStatus=0 then 'Unpaid' Else 'Paid' End as PayStatus, a.DeviceType, " +
                    " e.OBP_ApplicantName, e.OBP_MobileNo, e.OBP_UserID From OrdersData a Left Join OrdersAssign c On a.OrderID=c.FK_OrderID " +
                    " Left Join FranchiseeData b On c.Fk_FranchID=b.FranchID Left Join CustomersData d On a.FK_OrderCustomerID=d.CustomrtID " +
                    " Left Join OBPData e On d.FK_ObpID=e.OBP_ID Where a.OrderStatus<>0 AND a.FK_OrderCustomerID<>0 AND (a.DeviceType='GOBP-Order' OR a.DeviceType='GOBP-Android') " +
                    " AND (CONVERT(varchar(20), a.OrderDate, 112) >= CONVERT(varchar(20), CAST('" + obpFromDate + "' as datetime), 112)) AND " +
                    " (CONVERT(varchar(20), a.OrderDate, 112) <= CONVERT(varchar(20), CAST('" + obpToDate + "' as datetime), 112)) " +
                    " Order By a.OrderDate DESC";


                using (DataTable dtobp = c.GetDataTable(obpQuery))
                {
                    if (dtobp.Rows.Count > 0)
                    {
                        gvObpOrd.DataSource = dtobp;
                        gvObpOrd.DataBind();

                        // To export all pages
                        gvObpOrd.AllowPaging = false;

                        gvObpOrd.HeaderRow.BackColor = Color.Black;
                        gvObpOrd.HeaderRow.ForeColor = Color.White;


                        foreach (TableCell cell in gvObpOrd.HeaderRow.Cells)
                        {
                            cell.BackColor = gvObpOrd.HeaderStyle.BackColor;
                        }

                        foreach (GridViewRow row in gvObpOrd.Rows)
                        {
                            row.BackColor = Color.White;
                            row.BorderColor = Color.LightGray;
                            row.BorderWidth = Unit.Parse("1px");
                            foreach (TableCell cell in row.Cells)
                            {
                                if (row.RowIndex % 2 == 0)
                                {
                                    cell.BackColor = Color.WhiteSmoke;
                                }
                                else
                                {
                                    cell.BackColor = Color.White;
                                }
                            }
                        }

                        gvObpOrd.RenderControl(hw);

                        //style to format numbers to string
                        //string style = @"<style> .textmode { } </style>";
                        //Response.Write(style);
                        Response.Output.Write(sw.ToString());
                        Response.Flush();
                        Response.End();
                    }
                    else
                    {
                        errMsg = c.ErrNotification(2, "No Data To Display");
                        return;
                    }
                }
            }

            txtObpStartDate.Text = txtObpEndDate.Text = "";
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    protected void btnGMOrders_Click(object sender, EventArgs e)
    {
        ExportGMOrdersToExcel();
    }

    private void ExportGMOrdersToExcel()
    {
        try
        {
            if (txtGMStartDate.Text == "" && txtGMEndDate.Text == "")
            {
                errMsg = c.ErrNotification(2, "Select Date Range To Export Report");
                return;
            }

            // From Date
            DateTime gmFromDate = DateTime.Now;
            string[] arrfrDate = txtGMStartDate.Text.Split('/');
            if (c.IsDate(arrfrDate[1] + "/" + arrfrDate[0] + "/" + arrfrDate[2]) == false)
            {
                errMsg = c.ErrNotification(2, "Enter Valid From Date");
                return;
            }
            else
            {
                gmFromDate = Convert.ToDateTime(arrfrDate[1] + "/" + arrfrDate[0] + "/" + arrfrDate[2]);
            }

            // To Date
            DateTime gmToDate = DateTime.Now;
            string[] arrtoDate = txtGMEndDate.Text.Split('/');
            if (c.IsDate(arrtoDate[1] + "/" + arrtoDate[0] + "/" + arrtoDate[2]) == false)
            {
                errMsg = c.ErrNotification(2, "Enter Valid To Date");
                return;
            }
            else
            {
                gmToDate = Convert.ToDateTime(arrtoDate[1] + "/" + arrtoDate[0] + "/" + arrtoDate[2]);
            }


            // This actually makes your HTML output to be downloaded as .xls file
            Response.Clear();
            //Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=GenericMitra_Orders_Report.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";

            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);


                string gmQuery = "Select Distinct a.OrderID, a.OrderDate, a.OrderAmount, d.CustomerName, d.CustomerMobile, " +
                    " isnull(b.FranchName, 'NA') as FranchName, isnull(b.FranchShopCode, 'NA') as FranchShopCode, " +
                    " Case When a.OrderStatus=1 then 'New' When a.OrderStatus=2 then 'Cancel By Cust' When a.OrderStatus=3 " +
                    " then 'Accepted by admin' When a.OrderStatus=4 then 'Denied by admin' When a.OrderStatus=5 then 'Inprocess' " +
                    " When a.OrderStatus=6 then 'Shipped' When a.OrderStatus=7 then 'Delivered' When a.OrderStatus=8 " +
                    " then 'Rejected by GMMH0001' When a.OrderStatus=9 then 'Rejected by shop (Low Order Amount)' End as adminstatus," +
                    " Case When c.OrdAssignStatus=0 then 'Pending' When c.OrdAssignStatus=1 then 'Accepted' When c.OrdAssignStatus=2 then 'Rejected' " +
                    " When c.OrdAssignStatus=5 then 'Inprocess' When c.OrdAssignStatus=6 then 'Shipped' When c.OrdAssignStatus=7 then 'Delivered' " +
                    " Else 'NA' End as Shopstatus, " +
                    " Case  When a.OrderType=1 then 'Regular Order' When a.OrderType=2 then 'Prescription Order' End as OrderType, " +
                    " Case When MreqFlag=1 then 'Yes' Else '-' End as MonthlyOrder, " +
                    " Case When a.OrderPayMode=1 then 'COD' Else 'Online Payment' End as PaymentMode, " +
                    " Case When a.OrderPayStatus=0 then 'Unpaid' Else 'Paid' End as PayStatus, a.DeviceType" +
                    " From OrdersData a Left Join OrdersAssign c On a.OrderID=c.FK_OrderID " +
                    " Left Join FranchiseeData b On c.Fk_FranchID=b.FranchID Left Join CustomersData d On a.FK_OrderCustomerID=d.CustomrtID " +
                    " Where a.OrderStatus<>0 AND a.FK_OrderCustomerID<>0 AND d.FK_GenMitraID IS NOT NULL  AND d.FK_GenMitraID<>0 " +
                    " AND (CONVERT(varchar(20), a.OrderDate, 112) >= CONVERT(varchar(20), CAST('" + gmFromDate + "' as datetime), 112)) AND " +
                    " (CONVERT(varchar(20), a.OrderDate, 112) <= CONVERT(varchar(20), CAST('" + gmToDate + "' as datetime), 112)) " +
                    " Order By a.OrderDate DESC";


                using (DataTable dtGM = c.GetDataTable(gmQuery))
                {
                    if (dtGM.Rows.Count > 0)
                    {
                        gvGenMitraOrd.DataSource = dtGM;
                        gvGenMitraOrd.DataBind();

                        // To export all pages
                        gvGenMitraOrd.AllowPaging = false;

                        gvGenMitraOrd.HeaderRow.BackColor = Color.Black;
                        gvGenMitraOrd.HeaderRow.ForeColor = Color.White;


                        foreach (TableCell cell in gvGenMitraOrd.HeaderRow.Cells)
                        {
                            cell.BackColor = gvGenMitraOrd.HeaderStyle.BackColor;
                        }

                        foreach (GridViewRow row in gvGenMitraOrd.Rows)
                        {
                            row.BackColor = Color.White;
                            row.BorderColor = Color.LightGray;
                            row.BorderWidth = Unit.Parse("1px");
                            foreach (TableCell cell in row.Cells)
                            {
                                if (row.RowIndex % 2 == 0)
                                {
                                    cell.BackColor = Color.WhiteSmoke;
                                }
                                else
                                {
                                    cell.BackColor = Color.White;
                                }
                            }
                        }

                        gvGenMitraOrd.RenderControl(hw);

                        //style to format numbers to string
                        //string style = @"<style> .textmode { } </style>";
                        //Response.Write(style);
                        Response.Output.Write(sw.ToString());
                        Response.Flush();
                        Response.End();
                    }
                    else
                    {
                        errMsg = c.ErrNotification(2, "No Data To Display");
                        return;
                    }
                }
            }

            txtGMStartDate.Text = txtGMEndDate.Text = "";
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}