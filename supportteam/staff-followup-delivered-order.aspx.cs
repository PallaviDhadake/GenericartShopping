using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class supportteam_staff_followup_delivered_order : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        //viewDeliveredOrdFollowup.Visible = true;
        FillGrid();
    }

    private void FillGrid()
    {
        try
        {
            //int teamId = Convert.ToInt32(Session["adminSupport"]);
            //int taskId = Convert.ToInt32(c.GetReqData("SupportTeam", "TeamTaskID", "TeamID=" + teamId + ""));

            using (DataTable dtPackege = c.GetDataTable("Select a.OrderID, a.FK_OrderCustomerID, b.CustomerName, b.CustomerMobile, b.CustomerEmail From OrdersData a Inner Join CustomersData b On a.FK_OrderCustomerID = b.CustomrtID Where a.OrderStatus = 7"))
            {

                //gvDeliveredOrd.DataSource = dtPackege;
                //gvDeliveredOrd.DataBind();
                //if (gvDeliveredOrd.Rows.Count > 0)
                //{
                //    gvDeliveredOrd.UseAccessibleHeader = true;
                //    gvDeliveredOrd.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvDeliveredOrd_RowDataBound(object sender, GridViewRowEventArgs e)
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
            c.ErrorLogHandler(this.ToString(), "gvDeliveredOrd_RowDataBound", ex.Message.ToString());
            return;
        }
    }

}