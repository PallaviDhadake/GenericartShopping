using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class supportteam_staff_followup_all_orders : System.Web.UI.Page
{
    iClass c = new iClass();
    public string ordStatus;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //viewAllOrdFollowUp.Visible = true;
            //ddrOrdStatus.SelectedIndex = 1;
            //ordStatus= ddrOrdStatus.SelectedValue;
            //FillGrid();
        }
        
    }

    private void FillGrid()
    {
        try
        {
            //int teamId = Convert.ToInt32(Session["adminSupport"]);
            //int taskId = Convert.ToInt32(c.GetReqData("SupportTeam", "TeamTaskID", "TeamID=" + teamId + ""));
            string strQuery = "";
            
            if (ordStatus != null || ordStatus != "")
            {
                switch (ordStatus)
                {
                    case "1":
                        strQuery = "Select a.OrderID, a.FK_OrderCustomerID, b.CustomerName, b.CustomerMobile, b.CustomerEmail From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID = b.CustomrtID Where a.OrderStatus=1";
                        break;
                    case "3":
                        strQuery = "Select a.OrderID, a.FK_OrderCustomerID, b.CustomerName, b.CustomerMobile, b.CustomerEmail From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID = b.CustomrtID Where a.OrderStatus=3";
                        break;
                    case "5":
                        strQuery = "Select a.OrderID, a.FK_OrderCustomerID, b.CustomerName, b.CustomerMobile, b.CustomerEmail From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID = b.CustomrtID Where a.OrderStatus=5";
                        break;
                    case "6":
                        strQuery = "Select a.OrderID, a.FK_OrderCustomerID, b.CustomerName, b.CustomerMobile, b.CustomerEmail From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID = b.CustomrtID Where a.OrderStatus=6";
                        break;

                }
            }
            



            //"Select a.OrderID, a.FK_OrderCustomerID, b.CustomerName, b.CustomerMobile, b.CustomerEmail From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID = b.CustomrtID Where a.OrderStatus=0 Or a.OrderStatus=5"








            using (DataTable dtPackege = c.GetDataTable(strQuery))
            {

                //gvAllOrd.DataSource = dtPackege;
                //gvAllOrd.DataBind();
                //if (gvAllOrd.Rows.Count > 0)
                //{
                //    gvAllOrd.UseAccessibleHeader = true;
                //    gvAllOrd.HeaderRow.TableSection = TableRowSection.TableHeader;
                //}
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FillGrid", ex.Message.ToString());
            return;
        }
    }

    protected void gvAllOrd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                //litAnch.Text = "<a href=\"add-team-master.aspx?action=edit&id=" + e.Row.Cells[0].Text + "\" class=\"gAnch\" title=\"View / Edit\"></a>";

                litAnch.Text = "<a href=\"staff-followup-form.aspx?id=" + e.Row.Cells[0].Text + "\" class=\"btn btn-sm btn-primary\" target=\"_blank\">Follow Up</a>";

            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvAllOrd_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            //if (ddrOrdStatus.SelectedIndex == 0)
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select order type');", true);
            //}
            //ordStatus = ddrOrdStatus.SelectedValue;
            //FillGrid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSearch_Click", ex.Message.ToString());
            return;
        }
    }
}