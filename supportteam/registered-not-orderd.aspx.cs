using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class supportteam_registered_not_orderd : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //FillGrid();
        }

        //string script = @"<script type='text/javascript'>
        //              function submitForm() {
        //                __doPostBack('<%= btnShow.ClientID %>', '');
        //              }
        //            </script>";
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "submitForm", script);
    }

    private void FillGrid()
    {
        try
        {
            // From Date
            DateTime custFromDate = DateTime.Now;
            string[] arrfrDate = txtNStartDate.Text.Split('/');
            if (c.IsDate(arrfrDate[1] + "/" + arrfrDate[0] + "/" + arrfrDate[2]) == false)
            {
                errMsg = c.ErrNotification(2, "Enter Valid From Date");
                return;
            }
            else
            {
                custFromDate = Convert.ToDateTime(arrfrDate[1] + "/" + arrfrDate[0] + "/" + arrfrDate[2]);
            }

            // To Date
            DateTime custToDate = DateTime.Now;
            string[] arrtoDate = txtNEndDate.Text.Split('/');
            if (c.IsDate(arrtoDate[1] + "/" + arrtoDate[0] + "/" + arrtoDate[2]) == false)
            {
                errMsg = c.ErrNotification(2, "Enter Valid To Date");
                return;
            }
            else
            {
                custToDate = Convert.ToDateTime(arrtoDate[1] + "/" + arrtoDate[0] + "/" + arrtoDate[2]);
            }

            //using (DataTable dtReg = c.GetDataTable("Select a.CustomrtID, Convert(varchar(20), a.CustomerJoinDate, 103) as CustomerJoinDate, " +
            //    " a.CustomerName, a.CustomerMobile, a.CustomerEmail, a.DeviceType, " +
            //    " (Select Count(FlupID) From FollowupOrders Where FK_CustomerId=a.CustomrtID AND FK_OrderId=0) as flcount " +
            //    " From CustomersData a Left Outer Join OrdersData b On a.CustomrtID=b.FK_OrderCustomerID Where" +
            //    " (CONVERT(varchar(20), a.CustomerJoinDate, 112) >= CONVERT(varchar(20), CAST('" + custFromDate + "' as datetime), 112)) AND " +
            //    " (CONVERT(varchar(20), a.CustomerJoinDate, 112) <= CONVERT(varchar(20), CAST('" + custToDate + "' as datetime), 112)) " +
            //    " AND (b.FK_OrderCustomerID is null or b.FK_OrderCustomerID=0)  " +
            //    " Order By a.CustomerJoinDate DESC"))


            //using (DataTable dtReg = c.GetDataTable("Select a.CustomrtID, Convert(varchar(20), a.RBNO_NextFollowup, 103) as RBNO_NextFollowup, " +
            //    " a.CustomerName, a.CustomerMobile, a.CustomerEmail, a.DeviceType, " +
            //    " (Select Count(FlupID) From FollowupOrders Where FK_CustomerId=a.CustomrtID AND FK_OrderId=0) as flcount " +
            //    " From CustomersData a Left Outer Join OrdersData b On a.CustomrtID=b.FK_OrderCustomerID  Where" +
            //    " (CONVERT(varchar(20), a.RBNO_NextFollowup, 112) >= CONVERT(varchar(20), CAST('" + custFromDate + "' as datetime), 112)) AND " +
            //    " (CONVERT(varchar(20), a.RBNO_NextFollowup, 112) <= CONVERT(varchar(20), CAST('" + custToDate + "' as datetime), 112)) " +
            //    " AND (b.FK_OrderCustomerID is null or b.FK_OrderCustomerID=0)  " +
            //    " Order By a.RBNO_NextFollowup DESC"))

            using (DataTable dtReg = c.GetDataTable("Select a.CustomrtID, Convert(varchar(20), a.RBNO_NextFollowup, 103) as RBNO_NextFollowup, " +
           " a.CustomerName, a.CustomerMobile, a.CustomerEmail, a.DeviceType, " +
           " (Select Count(FlupID) From FollowupOrders Where FK_CustomerId=a.CustomrtID AND FK_OrderId=0) as flcount " +
           " From CustomersData a Left Outer Join OrdersData b On a.CustomrtID=b.FK_OrderCustomerID LEFT JOIN SavingCalc c ON a.CustomrtID = c.FK_CustId where c.FK_CustId IS NULL AND" +
           " (CONVERT(varchar(20), a.RBNO_NextFollowup, 112) >= CONVERT(varchar(20), CAST('" + custFromDate + "' as datetime), 112)) AND " +
           " (CONVERT(varchar(20), a.RBNO_NextFollowup, 112) <= CONVERT(varchar(20), CAST('" + custToDate + "' as datetime), 112)) " +
           " AND (b.FK_OrderCustomerID is null or b.FK_OrderCustomerID=0)  " +
           " Order By a.RBNO_NextFollowup DESC"))

            {
                gvNotOrdCust.DataSource = dtReg;
                gvNotOrdCust.DataBind();

                if (gvNotOrdCust.Rows.Count > 0)
                {
                    gvNotOrdCust.UseAccessibleHeader = true;
                    gvNotOrdCust.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FllGrid", ex.Message.ToString());
            return;
        }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        //if (txtNStartDate.Text == "" && txtNEndDate.Text == "")
        //{
        //    errMsg = c.ErrNotification(2, "Select Date Range");
        //    return;
        //}

        //FillGrid();
    }

    protected void Submit(object sender, EventArgs e)
    {
        //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Button clicked.');", true);
        if (txtNStartDate.Text == "" && txtNEndDate.Text == "")
        {
            errMsg = c.ErrNotification(2, "Select Date Range");
            return;
        }
        FillGrid();

    }

    protected void gvNotOrdCust_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Check Favaourite Shop if ANY?
                object favShopCode = c.GetReqData("CustomersData", "CustomerFavShop", "CustomrtID=" + Convert.ToInt32(e.Row.Cells[0].Text));
                int myFavShopCode = (favShopCode != null) ? Convert.ToInt32(favShopCode) : 0;

                Literal litFavShop = (Literal)e.Row.FindControl("litFavShop");
                if (myFavShopCode > 0) 
                {
                    string FranchShopCode = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + myFavShopCode).ToString();
                    litFavShop.Text = "<span class=\"badge badge-success\" style=\"font-size: 14px;\">" + FranchShopCode + "</span>";
                }

                //if (favShopCode != null)
                //{
                //    myFavShopCode = Convert.ToInt32(favShopCode);
                //}
                //else
                //{
                //    myFavShopCode = 0;
                //}
                string strFlup = "";
                Literal litFlUp = (Literal)e.Row.FindControl("litFlUp");
                strFlup = "<a href=\"followup-order-detail.aspx?custId=" + e.Row.Cells[0].Text + "\" class=\"btn btn-sm btn-primary\" target=\"_blank\"><i class=\"fa fas fa-user-plus\"></i> &nbsp; Followup</a>";

                if (c.IsRecordExist("SELECT [CustomrtID] FROM [dbo].[CustomersData] WHERE [CallBusyFlag] = 1 AND [CustomrtID] = " + e.Row.Cells[0].Text + " AND [CallBusyBy] <>" + Session["adminSupport"]))
                {
                    strFlup = " <span title=\"User is Currently Locked\" class=\"text-danger text-sm\"><i class=\"fa fas fa-user-lock\"></i> &nbsp; User is Currently Busy</span>";
                }

                litFlUp.Text = strFlup.ToString();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvNotOrdCust_RowDataBound", ex.Message.ToString());
            return;
        }
    }
}