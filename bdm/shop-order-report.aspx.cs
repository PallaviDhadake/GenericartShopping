using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class bdm_shop_order_report : System.Web.UI.Page
{
    iClass c = new iClass();
    public string shopName, errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["shopId"] != null)
            {
                FillGrid();
            }
        }
    }

    private void FillGrid()
    {
        try
        {
            shopName = c.GetReqData("FranchiseeData", "FranchName", "FranchID=" + Request.QueryString["shopId"]).ToString();
            string dateCondition = "";
            if (Request.QueryString["date"] != null)
            {
                if (Request.QueryString["date"].ToString().Contains('-'))
                {
                    string[] arrDates = Request.QueryString["date"].ToString().Split('-');
                    string fromDate = arrDates[0].ToString();
                    string toDate = arrDates[1].ToString();
                    dateCondition = " ( CONVERT(varchar(20), a.OrdAssignDate, 112) >= CONVERT(varchar(20), CAST('" + fromDate + "' as DATETIME), 112) ) AND "
                    + " (CONVERT(varchar(20), a.OrdAssignDate, 112) <= CONVERT(varchar(20), CAST('" + toDate + "' as DATETIME), 112))";
                }
                else
                {
                    dateCondition = "( CONVERT(varchar(20), a.OrdAssignDate, 112) = CONVERT(varchar(20), CAST('" + DateTime.Now + "' as DATETIME), 112) ) ";
                }
            }

            //string strQuery = "Select a.OrdAssignID, a.FK_OrderID, CONVERT(varchar(20), a.OrdAssignDate, 103)as ordDate, " +
            //    " isnull(b.DeviceType, '-') as DeviceType, a.OrdAssignStatus , 'Rs. ' + Convert(varchar(20), b.OrderAmount) as OrdAmount, " +
            //    " c.CustomerName From OrdersAssign a Inner Join OrdersData b On a.FK_OrderID = b.OrderID Inner Join CustomersData c " +
            //    " On b.FK_OrderCustomerID = c.CustomrtID Where a.OrdReAssign=0 AND a.OrdAssignStatus>=0 AND a.Fk_FranchID=" + Request.QueryString["shopId"] + " AND " + dateCondition;

            string strQuery = "Select a.OrdAssignID, a.FK_OrderID, CONVERT(varchar(20), a.OrdAssignDate, 103)as ordDate, " +
               " isnull(b.DeviceType, '-') as DeviceType, a.OrdAssignStatus , 'Rs. ' + Convert(varchar(20), b.OrderAmount) as OrdAmount, " +
               " c.CustomerName From OrdersAssign a Inner Join OrdersData b On a.FK_OrderID = b.OrderID Inner Join CustomersData c " +
               " On b.FK_OrderCustomerID = c.CustomrtID Where a.OrdAssignStatus Not In(2, 5) AND a.OrdReAssign=0 AND a.OrdAssignStatus>=0 AND a.Fk_FranchID=" + Request.QueryString["shopId"] + " AND " + dateCondition;


            using (DataTable dtOrder = c.GetDataTable(strQuery))
            {
                gvOrder.DataSource = dtOrder;
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
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    protected void gvOrder_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // OrderAssignStatus 0 > Pending, 1 > Accepted, 2 > Rejected, 5 > In Process, 6 > Shipped, 7 > Delivered
                //(3,4 not considered to match flags 5,6,7 with main OrdersData table)

                int reAssignStatus = Convert.ToInt32(c.GetReqData("OrdersAssign", "OrdReAssign", "OrdAssignID=" + e.Row.Cells[0].Text));

                int MainOrderStatus = Convert.ToInt32(c.GetReqData("OrdersData", "OrderStatus", "OrderID=" + e.Row.Cells[2].Text));

                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                switch (e.Row.Cells[1].Text)
                {
                    case "0":
                        if (c.IsRecordExist("Select OrdAssignID From OrdersAssign Where FK_OrderID=" + e.Row.Cells[2].Text + " AND Fk_FranchID=" + Request.QueryString["shopId"] + " AND OrdAssignStatus=0 AND OrdReAssign=1 AND OrdAssignID<>" + e.Row.Cells[0].Text))
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
                                if (c.IsRecordExist("Select OrdAssignID From OrdersAssign Where OrdAssignStatus=2 AND FK_OrderID=" + e.Row.Cells[2].Text + ""))
                                {
                                    int frId = Convert.ToInt32(c.GetReqData("OrdersAssign", "Top 1 Fk_FranchID", "OrdAssignStatus=2 AND FK_OrderID=" + e.Row.Cells[2].Text + " Order By OrdAssignID DESC"));
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
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}