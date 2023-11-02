using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class franchisee_generic_mitra_details : System.Web.UI.Page
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
            string genmitraIds = "0";
            using (DataTable dtCust = c.GetDataTable("Select CustomrtID, FK_GenMitraID From CustomersData Where CustomerFavShop=" + Session["adminFranchisee"] + " AND FK_GenMitraID IS NOT NULL AND FK_GenMitraID<>'' AND FK_GenMitraID<>0"))
            {
                if (dtCust.Rows.Count > 0)
                {
                    foreach (DataRow row in dtCust.Rows)
                    {
                        if (row["FK_GenMitraID"] != DBNull.Value && row["FK_GenMitraID"] != null && row["FK_GenMitraID"].ToString() != "")
                        {
                            if (genmitraIds == "")
                                genmitraIds = row["FK_GenMitraID"].ToString();
                            else
                                genmitraIds = genmitraIds + "," + row["FK_GenMitraID"].ToString();
                        }
                        else
                        {
                            genmitraIds = "0";
                        }
                    }
                }
            }

            string frshopCode = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + Session["adminFranchisee"]).ToString();

            //using (DataTable dtGenMitra = c.GetDataTable("Select a.GMitraID, Convert(varchar(20), a.GMitraDate, 103) as rDate, a.GMitraName, a.GMitraMobile, " +
            //    " (Select Count(CustomrtID) From CustomersData Where delMark = 0 And FK_GenMitraID = a.GMitraID And CustomerActive = 1 AND CustomerFavShop=" + Session["adminFranchisee"] + ") as custCount, " +
            //    " isnull( (Select Sum(OrderAmount) From OrdersData Where FK_OrderCustomerID  In(Select CustomrtID From CustomersData Where FK_GenMitraID = a.GMitraID AND CustomerFavShop=" + Session["adminFranchisee"] + ")), 0) as ordValue " +
            //    " From GenericMitra a Where a.GMitraStatus = 1 AND a.GMitraShopCode='" + frshopCode + "' " + 
            //    " Union " +
            //    " Select a.GMitraID, Convert(varchar(20), a.GMitraDate, 103) as rDate, a.GMitraName, a.GMitraMobile, " +
            //    " (Select Count(CustomrtID) From CustomersData Where delMark = 0 And FK_GenMitraID = a.GMitraID And CustomerActive = 1 AND CustomerFavShop=" + Session["adminFranchisee"] + ") as custCount, " +
            //    " isnull( (Select Sum(OrderAmount) From OrdersData Where FK_OrderCustomerID  In(Select CustomrtID From CustomersData Where FK_GenMitraID = a.GMitraID AND CustomerFavShop=" + Session["adminFranchisee"] + ")), 0) as ordValue " +
            //    " From GenericMitra a Where a.GMitraStatus = 1 AND a.GMitraID IN (" + genmitraIds + ")")) 

            using (DataTable dtGenMitra = c.GetDataTable("Select a.GMitraID, Convert(varchar(20), a.GMitraDate, 103) as rDate, a.GMitraName, a.GMitraMobile, " +
                " (Select Count(CustomrtID) From CustomersData Where delMark = 0 And FK_GenMitraID = a.GMitraID And CustomerActive = 1) as custCount, " +
                " isnull( (Select Sum(OrderAmount) From OrdersData Where FK_OrderCustomerID  In(Select CustomrtID From CustomersData Where FK_GenMitraID = a.GMitraID)), 0) as ordValue " +
                " From GenericMitra a Where a.GMitraStatus = 1 AND a.GMitraShopCode='" + frshopCode + "'"))

            {
                gvGenMitra.DataSource = dtGenMitra;
                gvGenMitra.DataBind();
                if (gvGenMitra.Rows.Count > 0)
                {
                    gvGenMitra.UseAccessibleHeader = true;
                    gvGenMitra.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvGenMitra_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litComission = (Literal)e.Row.FindControl("litComission");

                double OrdValue = Convert.ToDouble(e.Row.Cells[5].Text);
                double comissionAmt = (OrdValue * 20) / 100;  // Previously here was 5%

                litComission.Text = comissionAmt.ToString("0.00");


                Literal litOrders = (Literal)e.Row.FindControl("litOrders");
                //string ordCount = c.returnAggregate("Select Count(DISTINCT a.OrderID) From OrdersData a Inner Join CustomersData b on a.FK_OrderCustomerID = b.CustomrtID Where a.OrderStatus <> 0 AND b.FK_GenMitraID=" + e.Row.Cells[0].Text + " AND b.CustomerFavShop=" + Session["adminFranchisee"]).ToString();
                string ordCount = c.returnAggregate("Select Count(DISTINCT a.OrderID) From OrdersData a Inner Join CustomersData b on a.FK_OrderCustomerID = b.CustomrtID Where a.OrderStatus <> 0 AND b.FK_GenMitraID=" + e.Row.Cells[0].Text).ToString();
                if (Convert.ToInt32(ordCount) > 0)
                {
                    litOrders.Text = "<a href=\"orders-report.aspx?type=genericMitra-" + e.Row.Cells[0].Text + "\" class=\"link-info\" target=\"_blank\">View " + ordCount + " Orders</a>";
                }
                else
                {
                    litOrders.Text = ordCount.ToString();
                }

                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                //litStatus.Text = "<span class=\"ordDenied\">UnPaid</span> / <span class=\"ordDelivered\">Paid</span>";
                litStatus.Text = "<span class=\"ordDenied\">UnPaid</span>";

                Literal litPay = (Literal)e.Row.FindControl("litPay");
                litPay.Text = "<a href=\"#\" class=\"badge badge-pill badge-info\">Pay Comission</a>";

                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"generic-mitra-info.aspx?genmitraID=" + e.Row.Cells[0].Text + "\" class=\"gView\" title=\"View/Edit\" target=\"_blank\"></a>";

            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvGenMitra_RowDataBound", ex.Message.ToString());
            return;
        }
    }
}