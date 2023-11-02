using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admingenshopping_customer_details : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           // FillGrid();
        }
    }

    //private void FillGrid()
    //{
    //    try
    //    {
    //        using (DataTable dtCust = c.GetDataTable("Select Distinct a.CustomrtID, CONVERT(varchar(20), a.CustomerJoinDate, 103) as JoinDate, a.CustomerName, a.CustomerMobile, a.CustomerEmail, isnull(a.DeviceType, '-') as DeviceType, a.CustomerPassword From CustomersData a Inner Join CustomersAddress b On a.CustomrtID=b.AddressFKCustomerID Where a.delMark=0 Order By a.CustomrtID DESC"))
    //        {
    //            gvDetails.DataSource = dtCust;
    //            gvDetails.DataBind();
    //            if (gvDetails.Rows.Count > 0)
    //            {
    //                gvDetails.UseAccessibleHeader = true;
    //                gvDetails.HeaderRow.TableSection = TableRowSection.TableHeader;
    //            }

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
    //        c.ErrorLogHandler(this.ToString(), "FillGrid", ex.Message.ToString());
    //        return;
    //    }
    //}
    //protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            TextBox txtCustName = (TextBox)e.Row.FindControl("txtCustName");
    //            txtCustName.Text = e.Row.Cells[2].Text.ToString();

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
    //        c.ErrorLogHandler(this.ToString(), "gvDetails_RowDataBound", ex.Message.ToString());
    //        return;
    //    }
    //}

    //protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName == "gvUpdate")
    //        {
    //            GridViewRow gRow = (GridViewRow)((Button)e.CommandSource).NamingContainer;
    //            TextBox txtCustName = (TextBox)gRow.FindControl("txtCustName");
    //            int custId = Convert.ToInt32(gRow.Cells[0].Text);

    //            if (txtCustName.Text == "")
    //            {
    //                FillGrid();
    //                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Customer Name');", true);
    //                return;
    //            }

    //            c.ExecuteQuery("Update CustomersData Set CustomerName='" + txtCustName.Text + "' Where CustomrtID=" + gRow.Cells[0].Text);

    //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Customer Name Updated');", true);
    //            FillGrid();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
    //        c.ErrorLogHandler(this.ToString(), "gvDetails_RowCommand", ex.Message.ToString());
    //        return;
    //    }
    //}

    [WebMethod]
    public static void UpdateCustomerData(string customerName, string customerId)
    {

        iClass c = new iClass();
        int custId = Convert.ToInt32(customerId);
        c.ExecuteQuery("Update CustomersData Set CustomerName='" + customerName + "' Where CustomrtID=" + custId + "");

    }
}