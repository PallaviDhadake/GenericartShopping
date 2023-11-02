using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class franchisee_shopwise_order_rating : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                FillGrid();
            }
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
            string strQuery = "";
            string fIdx = Session["adminFranchisee"].ToString();
            strQuery = "Select a.OrdAssignID,CONVERT(varchar(20), a.OrdAssignDate, 103)as ordDate, a.OrdAssignStatus , isnull(b.OrderRating, 0) as rating, '#'+Convert(varchar(20), b.OrderID) as ordId, 'Rs. ' + Convert(varchar(20), b.OrderAmount) as OrdAmount,(Select COUNT(FK_DetailProductID) from OrdersDetails where FK_DetailOrderID = b.OrderID ) as ProductCount, c.CustomerName, STUFF((Select ', ' + RTRIM(LTRIM(ProductName)) From ProductsData Where ProductID IN(Select FK_DetailProductID from OrdersDetails where FK_DetailOrderID = b.OrderID) FOR XML PATH('')), 1, 1, '' ) as CartProducts From OrdersAssign a Inner Join OrdersData b On a.FK_OrderID = b.OrderID Inner Join CustomersData c On b.FK_OrderCustomerID = c.CustomrtID Where a.Fk_FranchID =" + fIdx + " AND a.OrdAssignStatus<>2 AND a.OrdReAssign=0";
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

                string ordID = c.GetReqData("OrdersAssign", "FK_OrderID", "OrdAssignID=" + e.Row.Cells[0].Text + "").ToString();

                // OrderAssignStatus 0 > Pending, 1 > Accepted, 2 > Rejected, 5 > In Process, 6 > Shipped, 7 > Delivered
                //(3,4 not considered to match flags 5,6,7 with main OrdersData table)
                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                switch (e.Row.Cells[1].Text)
                {
                    case "0":
                        litStatus.Text = "<div class=\"ordNew\">New</div>";
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

                }

                // Rating
                Literal litRating = (Literal)e.Row.FindControl("litRating");
                string rating = "";
                switch (e.Row.Cells[2].Text)
                {
                    case "0": rating = "NA";
                        break;
                    case "1": rating = "1 Star";
                        break;
                    case "2": rating = "2 Star";
                        break;
                    case "3": rating = "3 Star";
                        break;
                    case "4": rating = "4 Star";
                        break;
                    case "5": rating = "5 Star";
                        break;
                }

                litRating.Text = rating.ToString();
            }
        }
        catch (Exception ex)
        {


            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvOrder_RowDataBound", ex.Message.ToString());
            return;
        }
    }
}