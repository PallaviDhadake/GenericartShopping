using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class districthead_medicine_order_report_shopwise : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtFDate.Text = "01" + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            FillGrid();
        }
    }

    private void FillGrid()
    {
        try
        {
            string strQuery = "";
            if (txtFDate.Text != "" && txtToDate.Text != "")
            {
                // From Date
                DateTime fromDate = DateTime.Now;
                string[] arrFromDate = txtFDate.Text.Split('/');
                fromDate = Convert.ToDateTime(arrFromDate[1] + "/" + arrFromDate[0] + "/" + arrFromDate[2]);

                // To Date
                DateTime toDate = DateTime.Now;
                string[] arrToDate = txtToDate.Text.Split('/');
                toDate = Convert.ToDateTime(arrToDate[1] + "/" + arrToDate[0] + "/" + arrToDate[2]);

                string dateCondition = "( (CONVERT(varchar(20), a.OrdAssignDate, 112) >= CONVERT(varchar(20), CAST('" + fromDate + "' as DATETIME), 112) ) AND "
                    + " (CONVERT(varchar(20), a.OrdAssignDate, 112) <= CONVERT(varchar(20), CAST('" + toDate + "' as DATETIME), 112)))";

                strQuery = "Select Distinct a.Fk_FranchID, b.FranchName, b.FranchShopCode, " +
                        " (Select Count(Distinct FK_OrderID) From OrdersAssign Where Fk_FranchID=a.Fk_FranchID AND " + dateCondition + ") as totalOrders, " +
                        " (Select SUM(OrdDetailAmount) From OrdersDetails Where FK_DetailOrderID IN (Select Distinct FK_OrderID From OrdersAssign Where Fk_FranchID=a.Fk_FranchID AND OrdAssignStatus=7 AND OrdReAssign=0  AND " + dateCondition + ")) as ordAmount, " +
                        " (Select SUM(OrdDetailAmount) From OrdersDetails Where FK_DetailOrderID IN (Select Distinct FK_OrderID From OrdersAssign Where Fk_FranchID=a.Fk_FranchID AND OrdAssignStatus<>2 AND OrdReAssign=0 AND " + dateCondition + ")) as totalOrdAmount, " +
                        " (Select SUM(OrdDetailAmount) From OrdersDetails Where FK_DetailOrderID IN (Select Distinct FK_OrderID From OrdersAssign Where Fk_FranchID=a.Fk_FranchID AND OrdAssignStatus=2 AND (OrdReAssign=1 OR OrdReAssign=0) AND " + dateCondition + ")) as rejOrdAmount, " +
                        " (Select Count(Distinct FK_OrderID) From OrdersAssign Where Fk_FranchID=a.Fk_FranchID AND OrdAssignStatus=0 AND OrdReAssign=0 AND " + dateCondition + ") as pendingOrd, " +
                        " (Select Count(Distinct FK_OrderID) From OrdersAssign Where Fk_FranchID=a.Fk_FranchID AND OrdAssignStatus=5 AND OrdReAssign=0 AND " + dateCondition + ") as inprocOrd, " +
                        " (Select Count(Distinct FK_OrderID) From OrdersAssign Where Fk_FranchID=a.Fk_FranchID AND OrdAssignStatus=6 AND OrdReAssign=0 AND " + dateCondition + ") as shippedOrd, " +
                        " (Select Count(Distinct FK_OrderID) From OrdersAssign Where Fk_FranchID=a.Fk_FranchID AND OrdAssignStatus=7 AND OrdReAssign=0 AND " + dateCondition + ") as deliveredOrd, " +
                        " (Select Count(Distinct FK_OrderID) From OrdersAssign Where Fk_FranchID=a.Fk_FranchID AND OrdAssignStatus=2 AND (OrdReAssign=1 OR OrdReAssign=0) AND " + dateCondition + ") as rejectedOrd " +
                        " From OrdersAssign a Inner Join FranchiseeData b On a.Fk_FranchID=b.FranchID " +
                        " Where b.FranchActive=1 AND b.FK_DistHdId=" + Session["adminDH"] + " AND " + dateCondition;

                //strQuery = "Select Distinct a.Fk_FranchID, b.FranchName, b.FranchShopCode, " +
                //        " (Select Count(FK_OrderID) From OrdersAssign Where Fk_FranchID=a.Fk_FranchID) as totalOrders, " +
                //        " (Select SUM(OrdDetailAmount) From OrdersDetails Where FK_DetailOrderID IN (Select FK_OrderID From OrdersAssign Where Fk_FranchID=a.Fk_FranchID AND OrdAssignStatus=7 AND OrdReAssign=0)) as ordAmount, " +
                //        " (Select SUM(OrdDetailAmount) From OrdersDetails Where FK_DetailOrderID IN (Select FK_OrderID From OrdersAssign Where Fk_FranchID=a.Fk_FranchID AND OrdAssignStatus<>2 AND OrdReAssign=0)) as totalOrdAmount, " +
                //        " (Select Count(FK_OrderID) From OrdersAssign Where Fk_FranchID=a.Fk_FranchID AND OrdAssignStatus=0 AND OrdReAssign=0) as pendingOrd, " +
                //        " (Select Count(FK_OrderID) From OrdersAssign Where Fk_FranchID=a.Fk_FranchID AND OrdAssignStatus=5 AND OrdReAssign=0) as inprocOrd, " +
                //        " (Select Count(FK_OrderID) From OrdersAssign Where Fk_FranchID=a.Fk_FranchID AND OrdAssignStatus=6 AND OrdReAssign=0) as shippedOrd, " +
                //        " (Select Count(FK_OrderID) From OrdersAssign Where Fk_FranchID=a.Fk_FranchID AND OrdAssignStatus=7 AND OrdReAssign=0) as deliveredOrd, " +
                //        " (Select Count(FK_OrderID) From OrdersAssign Where Fk_FranchID=a.Fk_FranchID AND OrdAssignStatus=2 AND (OrdReAssign=1 OR OrdReAssign=0)) as rejectedOrd " +
                //        " From OrdersAssign a Inner Join FranchiseeData b On a.Fk_FranchID=b.FranchID " +
                //        " Where b.FranchActive=1 AND FK_DistHdId=" + Session["adminDH"] + " AND " + dateCondition;

                using (DataTable dtMedOrd = c.GetDataTable(strQuery))
                {
                    gvOrder.DataSource = dtMedOrd;
                    gvOrder.DataBind();

                    if (gvOrder.Rows.Count > 0)
                    {
                        gvOrder.UseAccessibleHeader = true;
                        gvOrder.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }
                    else
                    {
                        errMsg = c.ErrNotification(2, "There are no any orders present in selected date range");
                        return;
                    }
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            // From Date
            DateTime fromDate = DateTime.Now;
            string[] arrFromDate = txtFDate.Text.Split('/');
            if (c.IsDate(arrFromDate[1] + "/" + arrFromDate[0] + "/" + arrFromDate[2]) == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid From Date');", true);
                return;
            }

            // To Date
            DateTime toDate = DateTime.Now;
            string[] arrToDate = txtToDate.Text.Split('/');
            if (c.IsDate(arrToDate[1] + "/" + arrToDate[0] + "/" + arrToDate[2]) == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid To Date');", true);
                return;
            }

            FillGrid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSave_Click", ex.Message.ToString());
            return;
        }
    }
}