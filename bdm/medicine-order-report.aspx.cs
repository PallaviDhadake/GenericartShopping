using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class management_medicine_order_report : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtFDate.Text = "01" + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            c.FillComboBox("ZonalHdName", "ZonalHdId", "ZonalHead", "DelMark=0", "ZonalHdName", 0, ddrZh);
            //FillGrid();
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

                string dateCondition = "( (CONVERT(varchar(20), OrdAssignDate, 112) >= CONVERT(varchar(20), CAST('" + fromDate + "' as DATETIME), 112) ) AND "
                    + " (CONVERT(varchar(20), OrdAssignDate, 112) <= CONVERT(varchar(20), CAST('" + toDate + "' as DATETIME), 112)))";

                //string ordIds = GetOrderIds(dateCondition);

                string ordIds = "0";
                if (ddrZh.SelectedIndex > 0)
                {
                    if (ddrDh.SelectedIndex > 0)
                    {
                        ordIds = GetOrderIds(dateCondition, 2, Convert.ToInt32(ddrDh.SelectedValue));
                    }
                    else
                    {
                        ordIds = GetOrderIds(dateCondition, 1, Convert.ToInt32(ddrZh.SelectedValue));
                    }
                }

                if (ordIds != "0")
                {
                    strQuery = "Select Distinct a.FK_DetailProductID, a.OrdDetailSKU, b.ProductName, " +
                        " (Select Count(FK_DetailOrderID) From OrdersDetails Where FK_DetailProductID=a.FK_DetailProductID AND FK_DetailOrderID IN (" + ordIds + ")) as totalOrders, " +
                        " (Select Sum(OrdDetailQTY) From OrdersDetails Where FK_DetailProductID=a.FK_DetailProductID AND FK_DetailOrderID IN (" + ordIds + ")) as totalQty, " +
                        " d.UnitName, c.MfgName From OrdersDetails a Inner Join ProductsData b On a.FK_DetailProductID=b.ProductID " +
                        " Inner Join Manufacturers c On b.FK_MfgID=c.MfgId Inner Join UnitProducts d On b.FK_UnitID=d.UnitID " +
                        " Where a.FK_DetailOrderID IN (" + ordIds + ")";

                    using (DataTable dtMedOrd = c.GetDataTable(strQuery))
                    {
                        gvOrder.DataSource = dtMedOrd;
                        gvOrder.DataBind();

                        if (gvOrder.Rows.Count > 0)
                        {
                            gvOrder.UseAccessibleHeader = true;
                            gvOrder.HeaderRow.TableSection = TableRowSection.TableHeader;
                        }
                    }
                }
                else
                {
                    errMsg = c.ErrNotification(2, "There are no any orders present in selected date range or selected head");
                    return;
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

    private string GetOrderIds(string whereCon, int headType, int headId)
    {
        try
        {
            string strQuery = "";
            if (headType == 1) // zonal head
            {
                strQuery = "Select FK_OrderID From OrdersAssign Where Fk_FranchID IN (Select FranchID From FranchiseeData Where FranchActive=1 AND FK_ZonalHdId=" + headId + ") AND OrdReAssign=0 AND OrdAssignStatus IN (1, 5, 6, 7) AND " + whereCon;
            }
            else
            { // district head
                strQuery = "Select FK_OrderID From OrdersAssign Where Fk_FranchID IN (Select FranchID From FranchiseeData Where FranchActive=1 AND FK_DistHdId=" + headId + ") AND OrdReAssign=0 AND OrdAssignStatus IN (1, 5, 6, 7) AND " + whereCon;
            }
            using (DataTable dtOrd = c.GetDataTable(strQuery))
            {
                string ids = "";
                if (dtOrd.Rows.Count > 0)
                {
                    foreach (DataRow row in dtOrd.Rows)
                    {
                        if (ids == "")
                            ids = row["FK_OrderID"].ToString();
                        else
                            ids = ids + "," + row["FK_OrderID"].ToString();
                    }

                    return ids;
                }
                else
                {
                    return "0";
                }
            }
        }
        catch (Exception)
        {
            return "0";
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

            if (ddrZh.SelectedIndex == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select Zonal Head');", true);
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

    protected void ddrZh_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddrZh.SelectedIndex > 0)
            {
                string zhDistIds = "";
                using (DataTable dtZhDist = c.GetDataTable("Select DistrictId From ZonalHeadDistricts Where ZonalHdId=" + ddrZh.SelectedValue))
                {
                    if (dtZhDist.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtZhDist.Rows)
                        {
                            if (zhDistIds != "")
                                zhDistIds = zhDistIds + "," + row["DistrictId"].ToString();
                            else
                                zhDistIds = row["DistrictId"].ToString();
                        }
                    }
                }
                c.FillComboBox("DistHdName", "DistHdId", "DistrictHead", "DelMark=0 AND DistHdDistrictId IN (" + zhDistIds + ")", "DistHdName", 0, ddrDh);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "ddrZh_SelectedIndexChanged", ex.Message.ToString());
            return;
        }
    }
}