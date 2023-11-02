using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class supportteam_customerwise_distribution : System.Web.UI.Page
{
    iClass c = new iClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["type"] != null)
                {
                }
                else
                {
                    DateTime tempDate = DateTime.Now.AddMonths(0);
                }
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

            if (Request.QueryString["type"] == "today")
            {
                if (Request.QueryString["shop"] != null)
                {
                    if (Request.QueryString["shop"] == "own")
                    {
                        strQuery = @"SELECT 
                                     a.[FlpAsnId],   
                                     a.[Fk_CustomerID],
                                     a.[FK_OrderId],
                                     b.[CustomerName],
                                     b.[CustomerMobile],
                                     b.[CallGoodTime],
                                     (SELECT COUNT([OrderID]) FROM [dbo].[OrdersData] 
                                        WHERE [FK_OrderCustomerID] = a.[Fk_CustomerID] AND OrderStatus IN (6,7) AND OrderDate <= GETDATE()
                                            AND OrderDate >= DATEADD(MONTH, -6, GETDATE())  
                                            AND DAY(OrderDate)=DATEPART(DAY, GETDATE())) AS OrderCount
                                    FROM [dbo].[FollowupAssign] AS a 
                                    INNER JOIN [dbo].[OrdersData] AS c ON a.[FK_OrderId] = c.[OrderID]
                                    INNER JOIN [dbo].[CustomersData] AS b ON a.[Fk_CustomerID] = b.[CustomrtID]
                                    WHERE CONVERT(VARCHAR(20), a.[FlpAsnDate], 112) = CONVERT(VARCHAR(20), CAST(GETDATE() as DATETIME), 112)                                    
                                    AND a.[FlpAsnStatus] IN ('Pending', 'Postpone') AND c.[OrderStatus] IN (6,7)
                                    AND a.[FK_TeamID] = " + Session["adminSupport"];
                    }
                }
            }

            using (DataTable dtFlOrd = c.GetDataTable(strQuery))
            {
                gvOrdFlup.DataSource = dtFlOrd;
                gvOrdFlup.DataBind();

                if (gvOrdFlup.Rows.Count > 0)
                {
                    gvOrdFlup.UseAccessibleHeader = true;
                    gvOrdFlup.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvOrdFlup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string strFlup = "";
                Literal litFlUp = (Literal)e.Row.FindControl("litFlUp");

                strFlup = " <a href=\"followup-customer-report.aspx?custId=" + e.Row.Cells[1].Text + "\" class=\"gView\" target=\"_blank\" title=\"View\"></a>";

                if (c.IsRecordExist("SELECT [CustomrtID] FROM [dbo].[CustomersData] WHERE [CallBusyFlag] = 1 AND [CustomrtID] = " + e.Row.Cells[1].Text + " AND [CallBusyBy] <>" + Session["adminSupport"]))
                {
                    strFlup = " <span title=\"User is Currently Locked\" class=\"text-danger text-sm\"><i class=\"fa fas fa-user-lock\"></i> &nbsp; User is Currently Busy With " + Session["adminSupport"] + "</span>";
                }

                litFlUp.Text = strFlup.ToString();

                DropDownList ddlFlupStatus = (DropDownList)e.Row.FindControl("ddlFlupStatus");

                string strFlupStatus = "";
                strFlupStatus = c.GetReqData("[dbo].[FollowupAssign]", "[FlpAsnStatus]", "[Fk_CustomerID]=" + e.Row.Cells[1].Text + " AND CONVERT(VARCHAR(20), [FlpAsnDate], 112) = CONVERT(VARCHAR(20), CAST(GETDATE() as DATETIME), 112)").ToString();
                ddlFlupStatus.SelectedItem.Text = strFlupStatus.ToString();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvOrdFlup_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            Button btn = (Button)sender; // The button that triggered the click event
            GridViewRow gRow = (GridViewRow)btn.NamingContainer;

            int custId = Convert.ToInt32(gRow.Cells[1].Text);

            DropDownList ddlFlupStatus = (DropDownList)gRow.FindControl("ddlFlupStatus");

            if (ddlFlupStatus != null)
            {
                // Access the selected value from the DropDownList
                string selectedValue = ddlFlupStatus.SelectedItem.Text;

                // Use the selected value in your SQL query
                c.ExecuteQuery("UPDATE [dbo].[FollowupAssign] SET [FlpAsnStatus] = '" + selectedValue + "' WHERE [Fk_CustomerID] = " + custId + " AND CONVERT(VARCHAR(20), [FlpAsnDate], 112) = CONVERT(VARCHAR(20), CAST(GETDATE() as DATETIME), 112)");

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Followup Status Updated Successfully ');", true);

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('dashboard.aspx', 2000);", true);
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions and display an error message
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occurred While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnUpdate_Click", ex.Message.ToString());
        }
    }
}