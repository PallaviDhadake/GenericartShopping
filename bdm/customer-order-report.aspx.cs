using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class bdm_customer_order_report : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] ordData = new String[10];
    public string fyDateRange;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillGrid();

            if (txtFromDate.Text != string.Empty && txtToDate.Text != string.Empty)
            {
                DateTime fromDate;
                string[] arrFromDate = txtFromDate.Text.Split('/');
                fromDate = Convert.ToDateTime(arrFromDate[1] + "/" + arrFromDate[0] + "/" + arrFromDate[2]);

                DateTime toDate;
                string[] arrToDate = txtToDate.Text.Split('/');
                toDate = Convert.ToDateTime(arrToDate[1] + "/" + arrToDate[0] + "/" + arrToDate[2]);
                litDate.Text = fromDate.ToString("dd/MM/yyyy") + " - " + toDate.ToString("dd/MM/yyyy");
            }
            else
            {
                string dateRange = c.GetFinancialYear();
                string[] arrDateRange = dateRange.ToString().Split('#');
                DateTime myFromDate = Convert.ToDateTime(arrDateRange[0]);
                DateTime myToDate = Convert.ToDateTime(arrDateRange[1]);
                litDate.Text = myFromDate.ToString("dd/MM/yyyy") + " - " + DateTime.Now.ToString("dd/MM/yyyy");
            }
        }
    }

    private void FillGrid()
    {
        try
        {
            int orderStatus = 0;

            string dateRange = c.GetFinancialYear();
            string[] arrDateRange = dateRange.ToString().Split('#');
            DateTime myFromDate = Convert.ToDateTime(arrDateRange[0]);
            DateTime myToDate = Convert.ToDateTime(arrDateRange[1]);

            SqlConnection con = new SqlConnection(c.OpenConnection());
            con.Open();
            SqlCommand cmd = new SqlCommand("GetOrdersData", con);
            cmd.CommandType = CommandType.StoredProcedure;

            if (txtFromDate.Text == string.Empty && txtToDate.Text == string.Empty)
            {
                cmd.Parameters.AddWithValue("@OrdStatus", orderStatus);
                cmd.Parameters.AddWithValue("@FromDate", myFromDate);
                cmd.Parameters.AddWithValue("@ToDate", DateTime.Now);
            }
            else if (txtFromDate.Text != string.Empty && txtToDate.Text != string.Empty)
            {
                DateTime fromDate;
                string[] arrFromDate = txtFromDate.Text.Split('/');
                fromDate = Convert.ToDateTime(arrFromDate[1] + "/" + arrFromDate[0] + "/" + arrFromDate[2]);

                DateTime toDate;
                string[] arrToDate = txtToDate.Text.Split('/');
                toDate = Convert.ToDateTime(arrToDate[1] + "/" + arrToDate[0] + "/" + arrToDate[2]);

                cmd.Parameters.AddWithValue("@OrdStatus", orderStatus);
                cmd.Parameters.AddWithValue("@FromDate", fromDate);
                cmd.Parameters.AddWithValue("@ToDate", toDate);
            }

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                gvOrder.DataSource = dt;
                gvOrder.DataBind();
            }
            else
            {
                gvOrder.DataSource = null;
                gvOrder.DataBind();
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
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                if (Convert.ToInt32(e.Row.Cells[1].Text) > 0)
                {
                    litAnch.Text = "<a href=\"order-details.aspx?id=" + e.Row.Cells[0].Text + "\" class=\"gAnch\" title=\"View/Edit\"></a>";
                }

                string favShopOrder = "";
                object favShopId = c.GetReqData("CustomersData", "CustomerFavShop", "CustomrtID=" + e.Row.Cells[2].Text);
                if (favShopId != DBNull.Value && favShopId != null && favShopId.ToString() != "")
                {
                    favShopOrder = "<br/><span style=\"font-size:0.8em; color:#fa0ac2; line-height:0.5; font-weight:600;\">Favourite Shop Order</span>";
                }


                // orderStatus=0 > added to cart, 1 > Order placed (New/pending), 2 > CANCELL ORDER BY CUSTOMER , 3 > Accepted, 4 > Denied, 5 > Processing , 6 > Shipped , 7 > deliverd
                // 8 > Re-assigned(rejected by 0001), 9 > Rejected by shop for reason aorder amount low, 10 > OrderReturned
                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                switch (e.Row.Cells[1].Text)
                {

                    case "0":
                        litStatus.Text = "<div class=\"ordNew\">Order In Cart</div>" + favShopOrder;
                        break;
                    case "1":
                        litStatus.Text = "<div class=\"ordNew\">New</div>" + favShopOrder;
                        break;
                    case "2":
                        litStatus.Text = "<div class=\"ordCancCust\">Cancel By Customer</div>";
                        break;
                    case "3":
                        litStatus.Text = "<div class=\"ordAccepted\">Accepted</div>" + favShopOrder;
                        break;
                    case "4":
                        string frCode = "", frInfo = "";
                        if (c.IsRecordExist("Select OrdAssignID From OrdersAssign Where OrdAssignStatus=2 AND FK_OrderID=" + e.Row.Cells[0].Text + ""))
                        {
                            int frId = Convert.ToInt32(c.GetReqData("OrdersAssign", "Top 1 Fk_FranchID", "OrdAssignStatus=2 AND FK_OrderID=" + e.Row.Cells[0].Text + " Order By OrdAssignID DESC"));
                            frCode = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + frId).ToString();
                            frInfo = " Rejected By " + frCode;
                        }
                        litStatus.Text = "<div class=\"ordDenied\">Denied By Admin " + frInfo + "</div>" + favShopOrder;
                        break;
                    case "5":
                        litStatus.Text = "<div class=\"ordProcessing\">Processing</div>" + favShopOrder;
                        break;
                    case "6":
                        litStatus.Text = "<div class=\"ordShipped\">Shipped</div>" + favShopOrder;
                        break;
                    case "7":
                        litStatus.Text = "<div class=\"ordDelivered\">Delivered</div>" + favShopOrder;
                        break;
                    case "8":
                        litStatus.Text = "<div class=\"ordDenied\">Rejected By GMMH0001</div>" + favShopOrder;
                        break;
                    case "9":
                        int shopId = Convert.ToInt32(c.GetReqData("OrdersAssign", "Top 1 Fk_FranchID", "OrdAssignStatus=2 AND FK_OrderID=" + e.Row.Cells[0].Text + " Order By OrdAssignID DESC"));
                        string shopCode = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + shopId).ToString();
                        litStatus.Text = "<div class=\"ordProcessing\">Rejected by " + shopCode + " - Order Amount Low</div>" + favShopOrder;
                        break;
                    case "10":
                        litStatus.Text = "<div class=\"ordDenied\">Returned By Customer</div>" + favShopOrder;
                        break;
                    case "11":
                        litStatus.Text = "<div class=\"ordDenied\">Rejected by Doctor</div>" + favShopOrder;
                        break;
                    case "12":
                        litStatus.Text = "<div class=\"ordDenied\">No Response to Call</div>" + favShopOrder;
                        break;
                    case "13":
                        litStatus.Text = "<div class=\"ordDenied\">Refund Request by Customer</div>" + favShopOrder;
                        break;
                    case "14":
                        litStatus.Text = "<div class=\"ordDenied\">Refund Request in Process</div>" + favShopOrder;
                        break;
                    case "15":
                        litStatus.Text = "<div class=\"ordDenied\">Refund Request Completed</div>" + favShopOrder;
                        break;
                }
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
            DateTime fromDate = DateTime.Now;
            string[] arrFromDate = txtFromDate.Text.Split('/');
            if (c.IsDate(arrFromDate[1] + "/" + arrFromDate[0] + "/" + arrFromDate[2]) == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid From Date');", true);
                return;
            }

            DateTime toDate = DateTime.Now;
            string[] arrToDate = txtToDate.Text.Split('/');
            if (c.IsDate(arrToDate[1] + "/" + arrToDate[0] + "/" + arrToDate[2]) == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid ToDate');", true);
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
}