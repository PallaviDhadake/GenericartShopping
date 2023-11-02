using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class supportteam_cust_lookup : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg, lookupUrl, poUrl, editorder;
    protected void Page_Load(object sender, EventArgs e)
    {
        repBtn.Visible = false;
        if (Request.QueryString["custId"] != null)
        {
            string custMob = c.GetReqData("CustomersData", "CustomerMobile", "CustomrtID=" + Request.QueryString["custId"]).ToString();
            txtMob.Text = custMob;            
        }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            txtMob.Text = txtMob.Text.Trim().Replace("'", "");

            if (txtMob.Text == "")
            {
                errMsg = c.ErrNotification(2, "Enter Registered Mobile No");
                return;
            }

            if (!c.IsRecordExist("Select CustomrtID From CustomersData Where CustomerMobile='" + txtMob.Text + "' AND delMark=0 AND CustomerActive=1"))
            {
                errMsg = c.ErrNotification(2, "Enter valid customer mobile number");
                return;
            }

            int custId = Convert.ToInt32(c.GetReqData("CustomersData", "CustomrtID", "CustomerMobile='" + txtMob.Text + "' AND delMark=0 AND CustomerActive=1"));
            repBtn.Visible = true;
            lookupUrl = Master.rootPath + "customer-lookup.aspx?custId=" + custId;
            poUrl = Master.rootPath + "supportteam/submit-po.aspx?custId=" + custId + "&type=newOrd";

            editorder = "Edit & Repeat Order";

            FillGrid();
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    private void FillGrid()
    {
        try
        {
            string dateRange = c.GetFinancialYear();
            string[] arrDateRange = dateRange.ToString().Split('#');
            DateTime myFromDate = Convert.ToDateTime(arrDateRange[0]);
            DateTime myToDate = Convert.ToDateTime(arrDateRange[1]);

            string strQuery = "";

            strQuery = @"SELECT TOP 1
                            a.[FK_OrderCustomerID] AS FK_OrderCustomerID,
                            a.[OrderID] AS OrderID,
                            MAX(b.[CustomerName]) as CustomerName, 
                            MAX(b.[CustomerMobile]) as CustomerMobile,
                            CONVERT(VARCHAR(20), MAX(a.[OrderDate]), 103) AS ordDate,
                            MAX(CONVERT(VARCHAR(20), a.[OrderDate], 103) + ' - ' + CONVERT(VARCHAR(20), a.[FollowupNextDate], 103)) AS FlLastDate,
                            LEFT (STUFF((SELECT ', ' + RTRIM(LTRIM([ProductName])) FROM [dbo].[ProductsData] WHERE [ProductID] IN (SELECT [FK_DetailProductID] FROM [dbo].[OrdersDetails] WHERE [FK_DetailOrderID] = a.[OrderID]) FOR XML PATH('')), 1, 1, '' ) , 200) AS ProductName,
                            'Rs. ' + CONVERT(VARCHAR(20), MAX(a.[OrderAmount])) AS OrdAmount,
                            MAX(DATEDIFF(DAY, a.[FollowupLastDate], GETDATE())) AS DateDiff,
                            MAX(a.[OrderStatus]) AS OrderStatus
                        FROM [dbo].[OrdersData] AS a
                        INNER JOIN [dbo].[CustomersData] b ON a.[FK_OrderCustomerID] = b.[CustomrtID]
                        WHERE a.[FollowupStatus] = 'Active'
                        AND b.[CustomerMobile] = '" + txtMob.Text + "'" +
                        " AND a.[OrderStatus] = 7" +
                        " AND CONVERT(VARCHAR(20), a.[FollowupNextDate], 112) >= CONVERT(VARCHAR(20), CAST('" + myFromDate + "' as DATETIME), 112)" +
                        " AND CONVERT(VARCHAR(20), a.[FollowupNextDate], 112) <= CONVERT(VARCHAR(20), CAST('" + DateTime.Now + "' as DATETIME), 112) " +
                        " GROUP BY a.[OrderID], a.[FK_OrderCustomerID]" +
                        " ORDER BY MAX(a.[FollowupNextDate]) DESC";

            using (DataTable dtEditOrd = c.GetDataTable(strQuery))
            {
                gvEditOrder.DataSource = dtEditOrd;
                gvEditOrder.DataBind();

                if (gvEditOrder.Rows.Count > 0)
                {
                    gvEditOrder.UseAccessibleHeader = true;
                    gvEditOrder.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvEditOrder_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                switch (e.Row.Cells[1].Text)
                {
                    case "7":
                        litStatus.Text = "<div class=\"ordDelivered\">Delivered</div>";
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FillGrid", ex.Message.ToString());
            return;
        }
    }

    protected void EditOrder_Click(object sender, EventArgs e)
    {
        //int custId = Convert.ToInt32(c.GetReqData("[dbo].[CustomersData]", " [CustomrtID]", " [CustomerMobile] = '" + txtMob.Text + "'").ToString());
        if (Request.QueryString["custId"] != null)
        {
            int ordId = Convert.ToInt32(c.GetReqData("[dbo].[OrdersData]", "TOP 1 [OrderID]", "[FK_OrderCustomerID] = " + Request.QueryString["custId"] + " ORDER BY [FollowupNextDate] DESC").ToString());

            Response.Redirect("edit-new-order.aspx?" + "custid=" + Request.QueryString["custId"].ToString() + "&ordId=" + ordId);
        }
        else
        {
            int custId = Convert.ToInt32(c.GetReqData("[dbo].[CustomersData]", "[CustomrtID]", "[CustomerMobile] = '" + txtMob.Text + "'").ToString());

            int ordId = Convert.ToInt32(c.GetReqData("[dbo].[OrdersData]", " TOP 1 [OrderID]", "[FK_OrderCustomerID]=" + custId + " ORDER BY [FollowupNextDate] DESC").ToString());

            Response.Redirect("edit-new-order.aspx?" + "custid=" + custId + "&ordId=" + ordId);
        }
    }
}