using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Data;
using System.Drawing;

public partial class distributor_online_payment_report : System.Web.UI.Page
{
    public string errMsg;
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private void ExportOrdersToExcel()
    {
        try
        {
            // This actually makes your HTML output to be downloaded as .xls file
            Response.Clear();
            //Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=OnlineOrdersReport.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //Get Orders in data table
                string orderQuery = "Select a.OrderID, a.FK_OrderCustomerID, Convert(varchar(20), a.OrderDate, 103) as ordDate, " +
                    " 'Rs. '+Convert(varchar(20), a.OrderAmount) as ordAmount, a.OrderStatus, a.OrderPaymentTxnId, e.OPL_transtatus, " +
                    " isnull(e.OLP_device_type, '-') as OLP_device_type, b.CustomerName + ', '+ b.CustomerMobile as custInfo, " +
                    " Case When c.OrdAssignStatus = 0 then 'Pending' When c.OrdAssignStatus = 1 then 'Accepted' When c.OrdAssignStatus = 2 then 'Rejected' " +
                    " When c.OrdAssignStatus = 5 then 'Inprocess' When c.OrdAssignStatus = 6 then 'Shipped' When c.OrdAssignStatus = 7 then 'Delivered' End as Shopstatus, " +
                    " d.FranchName, d.FranchShopcode From OrdersData a  Inner Join CustomersData b on a.FK_OrderCustomerID = b.CustomrtID " +
                    " Inner Join OrdersAssign c on a.OrderID=c.FK_OrderID Inner Join FranchiseeData d On c.Fk_FranchID=d.FranchID " +
                    " Inner Join online_payment_logs e On a.OrderPaymentTxnId=e.OPL_merchantTranId Where a.OrderStatus <> 0 AND c.OrdReAssign=0 AND " +
                    " (a.OrderPaymentTxnId IS NOT NULL AND a.OrderPaymentTxnId<>'') AND (e.OPL_transtatus='SUCCESS' OR e.OPL_transtatus='paid') ";

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

    protected void btnExport_Click(object sender, EventArgs e)
    {
        ExportOrdersToExcel();
    }
}