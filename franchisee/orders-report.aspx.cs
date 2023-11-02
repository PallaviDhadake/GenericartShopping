using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class franchisee_orders_report : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] ordData = new String[10];
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
            if (Request.QueryString["type"] != null)
            {
                if (Request.QueryString["type"].ToString().Contains("genericMitra"))
                {
                    //" AND c.CustomerFavShop = " + fIdx +  (Removed where condition on 12-July-23

                    string[] arrgenmitra = Request.QueryString["type"].ToString().Split('-');
                    int genmitraId = Convert.ToInt32(arrgenmitra[arrgenmitra.Length - 1]);
                    strQuery = @"SELECT
                                    a.FK_OrderID,
                                    MAX(a.OrdAssignID) AS OrdAssignID,
                                    MAX(CONVERT(varchar(20), a.OrdAssignDate, 103)) AS ordDate,
                                    MAX(ISNULL(b.DeviceType, '-')) AS DeviceType,
                                    MAX(a.OrdAssignStatus) AS OrdAssignStatus,
                                    'Rs. ' + CONVERT(varchar(20), MAX(b.OrderAmount)) AS OrdAmount,
                                    SUM(product_count) AS ProductCount,
                                    MAX(c.CustomerName) AS CustomerName,
                                    STUFF((SELECT ', ' + RTRIM(LTRIM(ProductName)) FROM ProductsData WHERE ProductID IN (SELECT FK_DetailProductID FROM OrdersDetails WHERE FK_DetailOrderID = b.OrderID) FOR XML PATH('')), 1, 1, '') AS CartProducts,
                                    (CASE WHEN MAX(b.[OrderPayMode]) = 1 THEN 'COD' WHEN MAX(b.[OrderPayMode]) = 2 THEN 'ONLINE' ELSE '-' END) AS PaymentMode
                                FROM 
                                (
                                    SELECT 
                                        FK_DetailOrderID, 
                                        COUNT(FK_DetailProductID) AS product_count
                                    FROM 
                                        OrdersDetails
                                    GROUP BY 
                                        FK_DetailOrderID
                                ) AS od
                                INNER JOIN OrdersData b ON od.FK_DetailOrderID = b.OrderID 
                                INNER JOIN OrdersAssign a ON a.FK_OrderID = b.OrderID 
                                INNER JOIN CustomersData c ON b.FK_OrderCustomerID = c.CustomrtID 
                                WHERE a.Fk_FranchID = " + fIdx +
                                    " AND c.FK_GenMitraID = " + genmitraId + 
                                " GROUP BY b.OrderID, a.FK_OrderID;";
                }
                switch (Request.QueryString["type"])
                {
                    case "new":
                        strQuery = @"Select 
                                     a.OrdAssignID, 
                                     a.FK_OrderID, 
                                     CONVERT(varchar(20), a.OrdAssignDate, 103)as ordDate, 
                                     isnull(b.DeviceType, '-') as DeviceType, a.OrdAssignStatus , 
                                     'Rs. ' + Convert(varchar(20), b.OrderAmount) as OrdAmount,
                                     (Select COUNT(FK_DetailProductID) from OrdersDetails where FK_DetailOrderID = b.OrderID ) as ProductCount, 
                                     c.CustomerName, 
                                     STUFF((Select ', ' + RTRIM(LTRIM(ProductName)) From ProductsData Where ProductID IN(Select FK_DetailProductID from OrdersDetails where FK_DetailOrderID = b.OrderID) FOR XML PATH('')), 1, 1, '' ) as CartProducts,
                                     (CASE WHEN b.[OrderPayMode] = 1 THEN 'COD' WHEN b.[OrderPayMode] = 2 THEN 'ONLINE' ELSE '-' END) AS PaymentMode
                                     From OrdersAssign a 
                                     Inner Join OrdersData b On a.FK_OrderID = b.OrderID 
                                     Inner Join CustomersData c On b.FK_OrderCustomerID = c.CustomrtID 
                                     Where a.OrdAssignStatus =0 And a.Fk_FranchID =" + fIdx + " AND b.OrderStatus<>2 AND a.OrdReAssign=0";
                        break;

                    case "accepted":
                        strQuery = @"Select 
                                     a.OrdAssignID, 
                                     a.FK_OrderID, 
                                     CONVERT(varchar(20), a.OrdAssignDate, 103)as ordDate, 
                                     isnull(b.DeviceType, '-') as DeviceType, 
                                     a.OrdAssignStatus , 
                                     'Rs. ' + Convert(varchar(20), b.OrderAmount) as OrdAmount,
                                     (Select COUNT(FK_DetailProductID) from OrdersDetails where FK_DetailOrderID = b.OrderID ) as ProductCount, 
                                     c.CustomerName, 
                                     STUFF((Select ', ' + RTRIM(LTRIM(ProductName)) From ProductsData Where ProductID IN(Select FK_DetailProductID from OrdersDetails where FK_DetailOrderID = b.OrderID) FOR XML PATH('')), 1, 1, '' ) as CartProducts,
                                     (CASE WHEN b.[OrderPayMode] = 1 THEN 'COD' WHEN b.[OrderPayMode] = 2 THEN 'ONLINE' ELSE '-' END) AS PaymentMode
                                     From OrdersAssign a 
                                     Inner Join OrdersData b On a.FK_OrderID = b.OrderID 
                                     Inner Join CustomersData c On b.FK_OrderCustomerID = c.CustomrtID 
                                     Where a.OrdAssignStatus=5 And a.Fk_FranchID =" + fIdx + " AND b.OrderStatus<>2";
                        break;

                    case "rejected":
                        if (fIdx == "24")
                        {
                            strQuery = @"Select 
                                         a.OrdAssignID, 
                                         a.FK_OrderID, 
                                         CONVERT(varchar(20), a.OrdAssignDate, 103)as ordDate, 
                                         isnull(b.DeviceType, '-') as DeviceType, 
                                         a.OrdAssignStatus , 
                                         'Rs. ' + Convert(varchar(20), b.OrderAmount) as OrdAmount,
                                         (Select COUNT(FK_DetailProductID) from OrdersDetails where FK_DetailOrderID = b.OrderID ) as ProductCount, 
                                         c.CustomerName, 
                                         STUFF((Select ', ' + RTRIM(LTRIM(ProductName)) From ProductsData Where ProductID IN(Select FK_DetailProductID from OrdersDetails where FK_DetailOrderID = b.OrderID) FOR XML PATH('')), 1, 1, '' ) as CartProducts,
                                         (CASE WHEN b.[OrderPayMode] = 1 THEN 'COD' WHEN b.[OrderPayMode] = 2 THEN 'ONLINE' ELSE '-' END) AS PaymentMode
                                         From OrdersAssign a 
                                         Inner Join OrdersData b On a.FK_OrderID = b.OrderID 
                                         Inner Join CustomersData c On b.FK_OrderCustomerID = c.CustomrtID 
                                         Where a.OrdAssignStatus=2 And a.Fk_FranchID =" + fIdx + " AND (CONVERT(varchar(20), a.OrdAssignDate, 112) >= CONVERT(varchar(20), CAST('2021-08-01' as datetime) ,112))";
                        }
                        else
                        {
                            strQuery = @"Select 
                                         a.OrdAssignID, 
                                         a.FK_OrderID, 
                                         CONVERT(varchar(20), a.OrdAssignDate, 103)as ordDate, 
                                         isnull(b.DeviceType, '-') as DeviceType, 
                                         a.OrdAssignStatus , 
                                         'Rs. ' + Convert(varchar(20), b.OrderAmount) as OrdAmount,
                                         (Select COUNT(FK_DetailProductID) from OrdersDetails where FK_DetailOrderID = b.OrderID ) as ProductCount, 
                                         c.CustomerName, 
                                         STUFF((Select ', ' + RTRIM(LTRIM(ProductName)) From ProductsData Where ProductID IN(Select FK_DetailProductID from OrdersDetails where FK_DetailOrderID = b.OrderID) FOR XML PATH('')), 1, 1, '' ) as CartProducts,
                                         (CASE WHEN b.[OrderPayMode] = 1 THEN 'COD' WHEN b.[OrderPayMode] = 2 THEN 'ONLINE' ELSE '-' END) AS PaymentMode
                                         From OrdersAssign a 
                                         Inner Join OrdersData b On a.FK_OrderID = b.OrderID 
                                         Inner Join CustomersData c On b.FK_OrderCustomerID = c.CustomrtID 
                                         Where a.OrdAssignStatus=2 And a.Fk_FranchID =" + fIdx + "";
                        }
                        break;

                    case "monthly":
                        strQuery = @"Select 
                                     a.OrdAssignID, 
                                     a.FK_OrderID, 
                                     CONVERT(varchar(20), a.OrdAssignDate, 103)as ordDate, 
                                     isnull(b.DeviceType, '-') as DeviceType, 
                                     a.OrdAssignStatus , 
                                     'Rs. ' + Convert(varchar(20), b.OrderAmount) as OrdAmount,
                                     (Select COUNT(FK_DetailProductID) from OrdersDetails where FK_DetailOrderID = b.OrderID ) as ProductCount, 
                                     c.CustomerName, 
                                     STUFF((Select ', ' + RTRIM(LTRIM(ProductName)) From ProductsData Where ProductID IN(Select FK_DetailProductID from OrdersDetails where FK_DetailOrderID = b.OrderID) FOR XML PATH('')), 1, 1, '' ) as CartProducts,
                                     (CASE WHEN b.[OrderPayMode] = 1 THEN 'COD' WHEN b.[OrderPayMode] = 2 THEN 'ONLINE' ELSE '-' END) AS PaymentMode
                                     From OrdersAssign a 
                                     Inner Join OrdersData b On a.FK_OrderID = b.OrderID 
                                     Inner Join CustomersData c On b.FK_OrderCustomerID = c.CustomrtID 
                                     Where b.MreqFlag=1 And a.Fk_FranchID =" + fIdx + "";
                        break;

                    case "dispatched":
                        strQuery = @"Select 
                                     a.OrdAssignID, 
                                     a.FK_OrderID, 
                                     CONVERT(varchar(20), a.OrdAssignDate, 103)as ordDate, 
                                     isnull(b.DeviceType, '-') as DeviceType, 
                                     a.OrdAssignStatus , 
                                     'Rs. ' + Convert(varchar(20), b.OrderAmount) as OrdAmount,
                                     (Select COUNT(FK_DetailProductID) from OrdersDetails where FK_DetailOrderID = b.OrderID ) as ProductCount, 
                                     c.CustomerName, 
                                     STUFF((Select ', ' + RTRIM(LTRIM(ProductName)) From ProductsData Where ProductID IN(Select FK_DetailProductID from OrdersDetails where FK_DetailOrderID = b.OrderID) FOR XML PATH('')), 1, 1, '' ) as CartProducts,
                                     (CASE WHEN b.[OrderPayMode] = 1 THEN 'COD' WHEN b.[OrderPayMode] = 2 THEN 'ONLINE' ELSE '-' END) AS PaymentMode
                                     From OrdersAssign a 
                                     Inner Join OrdersData b On a.FK_OrderID = b.OrderID 
                                     Inner Join CustomersData c On b.FK_OrderCustomerID = c.CustomrtID 
                                     Where a.OrdAssignStatus=6 And a.Fk_FranchID =" + fIdx + "";
                        break;

                    case "delivered":
                        strQuery = @"Select 
                                     a.OrdAssignID, 
                                     a.FK_OrderID, 
                                     CONVERT(varchar(20), a.OrdAssignDate, 103)as ordDate, 
                                     isnull(b.DeviceType, '-') as DeviceType, 
                                     a.OrdAssignStatus , 
                                     'Rs. ' + Convert(varchar(20), b.OrderAmount) as OrdAmount,
                                     (Select COUNT(FK_DetailProductID) from OrdersDetails where FK_DetailOrderID = b.OrderID ) as ProductCount, 
                                     c.CustomerName, 
                                     STUFF((Select ', ' + RTRIM(LTRIM(ProductName)) From ProductsData Where ProductID IN(Select FK_DetailProductID from OrdersDetails where FK_DetailOrderID = b.OrderID) FOR XML PATH('')), 1, 1, '' ) as CartProducts,
                                     (CASE WHEN b.[OrderPayMode] = 1 THEN 'COD' WHEN b.[OrderPayMode] = 2 THEN 'ONLINE' ELSE '-' END) AS PaymentMode
                                     From OrdersAssign a 
                                     Inner Join OrdersData b On a.FK_OrderID = b.OrderID 
                                     Inner Join CustomersData c On b.FK_OrderCustomerID = c.CustomrtID 
                                     Where a.OrdAssignStatus=7 And a.Fk_FranchID =" + fIdx + "";
                        break;

                    case "rx":
                        strQuery = @"Select 
                                     a.OrdAssignID, 
                                     a.FK_OrderID, 
                                     CONVERT(varchar(20), a.OrdAssignDate, 103)as ordDate, 
                                     isnull(b.DeviceType, '-') as DeviceType, 
                                     a.OrdAssignStatus , 
                                     'Rs. ' + Convert(varchar(20), b.OrderAmount) as OrdAmount,
                                     (Select COUNT(FK_DetailProductID) from OrdersDetails where FK_DetailOrderID = b.OrderID ) as ProductCount, 
                                     c.CustomerName, 
                                     STUFF((Select ', ' + RTRIM(LTRIM(ProductName)) From ProductsData Where ProductID IN(Select FK_DetailProductID from OrdersDetails where FK_DetailOrderID = b.OrderID) FOR XML PATH('')), 1, 1, '' ) as CartProducts,
                                     (CASE WHEN b.[OrderPayMode] = 1 THEN 'COD' WHEN b.[OrderPayMode] = 2 THEN 'ONLINE' ELSE '-' END) AS PaymentMode
                                     From OrdersAssign a 
                                     Inner Join OrdersData b On a.FK_OrderID = b.OrderID 
                                     Inner Join CustomersData c On b.FK_OrderCustomerID = c.CustomrtID 
                                     Where b.OrderType=2 AND a.OrdAssignStatus=0 And a.Fk_FranchID =" + fIdx + " AND b.OrderStatus<>2 AND a.OrdReAssign=0";
                        break;

                    case "returned":
                        strQuery = @"Select 
                                     a.OrdAssignID, 
                                     a.FK_OrderID, 
                                     CONVERT(varchar(20), a.OrdAssignDate, 103)as ordDate, 
                                     isnull(b.DeviceType, '-') as DeviceType, 
                                     a.OrdAssignStatus , 
                                     'Rs. ' + Convert(varchar(20), b.OrderAmount) as OrdAmount,
                                     (Select COUNT(FK_DetailProductID) from OrdersDetails where FK_DetailOrderID = b.OrderID ) as ProductCount, 
                                     c.CustomerName, 
                                     STUFF((Select ', ' + RTRIM(LTRIM(ProductName)) From ProductsData Where ProductID IN(Select FK_DetailProductID from OrdersDetails where FK_DetailOrderID = b.OrderID) FOR XML PATH('')), 1, 1, '' ) as CartProducts,
                                     (CASE WHEN b.[OrderPayMode] = 1 THEN 'COD' WHEN b.[OrderPayMode] = 2 THEN 'ONLINE' ELSE '-' END) AS PaymentMode
                                     From OrdersAssign a 
                                     Inner Join OrdersData b On a.FK_OrderID = b.OrderID 
                                     Inner Join CustomersData c On b.FK_OrderCustomerID = c.CustomrtID 
                                     Where a.OrdAssignStatus=10 And a.Fk_FranchID =" + fIdx + "";
                        break;
                }
            }
            else
            {
                strQuery = @"Select 
                             a.OrdAssignID, 
                             a.FK_OrderID, 
                             CONVERT(varchar(20), a.OrdAssignDate, 103)as ordDate, 
                             isnull(b.DeviceType, '-') as DeviceType, 
                             a.OrdAssignStatus , 
                             'Rs. ' + Convert(varchar(20), b.OrderAmount) as OrdAmount,
                             (Select COUNT(FK_DetailProductID) from OrdersDetails where FK_DetailOrderID = b.OrderID ) as ProductCount, 
                             c.CustomerName, 
                             STUFF((Select ', ' + RTRIM(LTRIM(ProductName)) From ProductsData Where ProductID IN(Select FK_DetailProductID from OrdersDetails where FK_DetailOrderID = b.OrderID) FOR XML PATH('')), 1, 1, '' ) as CartProducts,
                             (CASE WHEN b.[OrderPayMode] = 1 THEN 'COD' WHEN b.[OrderPayMode] = 2 THEN 'ONLINE' ELSE '-' END) AS PaymentMode
                             From OrdersAssign a 
                             Inner Join OrdersData b On a.FK_OrderID = b.OrderID 
                             Inner Join CustomersData c On b.FK_OrderCustomerID = c.CustomrtID Where a.Fk_FranchID =" + fIdx + "";
            }
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
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (Request.QueryString["type"] != null)
                {
                    if (Request.QueryString["type"] == "monthly")
                    {
                        e.Row.Cells[11].Text = "Mark as Not Monthly Order";
                    }
                    else
                    {
                        e.Row.Cells[11].Text = "";
                    }
                }
                else
                {
                    e.Row.Cells[11].Text = "";
                }
            }
          
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string ordID = c.GetReqData("OrdersAssign", "FK_OrderID", "OrdAssignID=" + e.Row.Cells[0].Text + "").ToString();

                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                if (Request.QueryString["type"] != null)
                    litAnch.Text = "<a href=\"order-details.aspx?id=" + ordID + "&type=" + Request.QueryString["type"] + "&assignId=" + e.Row.Cells[0].Text + "\" class=\"gView\" title=\"View/Edit\"></a>";
                else
                    litAnch.Text = "<a href=\"order-details.aspx?id=" + ordID + "&assignId=" + e.Row.Cells[0].Text + "\" class=\"gView\" title=\"View/Edit\"></a>";

                // OrderAssignStatus 0 > Pending, 1 > Accepted, 2 > Rejected, 5 > In Process, 6 > Shipped, 7 > Delivered
                //(3,4 not considered to match flags 5,6,7 with main OrdersData table)

                int reAssignStatus = Convert.ToInt32(c.GetReqData("OrdersAssign", "OrdReAssign", "OrdAssignID=" + e.Row.Cells[0].Text));

                int MainOrderStatus = Convert.ToInt32(c.GetReqData("OrdersData", "OrderStatus", "OrderID=" + e.Row.Cells[2].Text));

                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                switch (e.Row.Cells[1].Text)
                {
                    case "0":
                        if (c.IsRecordExist("Select OrdAssignID From OrdersAssign Where FK_OrderID=" + e.Row.Cells[2].Text + " AND Fk_FranchID=" + Session["adminFranchisee"] + " AND OrdAssignStatus=0 AND OrdReAssign=1 AND OrdAssignID<>" + e.Row.Cells[0].Text)) 
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
                LinkButton btnRemove = (LinkButton)e.Row.FindControl("cmdRemove");
                if (Request.QueryString["type"] != null)
                {
                    if (Request.QueryString["type"] == "monthly")
                    {
                        btnRemove.Visible = true;
                    }
                    else
                    {
                        btnRemove.Visible = false;
                    }
                }
                else
                {
                    btnRemove.Visible = false;
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
    protected void gvOrder_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridViewRow gRow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;

            if (e.CommandName == "gvRemove")
            {
                int OrderID = Convert.ToInt32(gRow.Cells[2].Text);

                c.ExecuteQuery("Update OrdersData Set MreqFlag=0 Where OrderID=" + OrderID);

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Removed from Monthly Order');", true);
            }

            FillGrid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvOrder_RowDataBound", ex.Message.ToString());
            return;
        }
    }
}