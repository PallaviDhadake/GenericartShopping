using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class obpmanager_obp_daywise_report : System.Web.UI.Page
{
    iClass c = new iClass();

    public string totalcomamt;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
          txtFDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        
            FillGrid();
        }
    }


    private void FillGrid()
    {
        try
        {
            string strQuery = "";

            DateTime fromDate = DateTime.Now;


            string[] arrFromDate = txtFDate.Text.Split('/');
            fromDate = Convert.ToDateTime(arrFromDate[1] + "/" + arrFromDate[0] + "/" + arrFromDate[2]);

            string dateCondition = "((CONVERT(varchar(20), OrderDate, 112) = CONVERT(varchar(20), CAST('" + fromDate + "' as DATETIME), 112) ))";

            totalcomamt = c.returnAggregate("Select SUM(OBPComTotal) From OrdersData Where OrderStatus IN (1, 3, 5, 6, 7) AND "+ dateCondition + "").ToString();

            strQuery = @"
                    SELECT 
                        OP.[OBP_ID],
                        OP.[OBP_UserID],
                        OP.[OBP_ApplicantName],
                        OD.OrderID,
                        CD.CustomerName,
                        OD.OrderType,
                        OD.OrderStatus,
                        COUNT(OL.FK_DetailOrderID) AS ItemsCount,
                        OD.OrderAmount AS OrderAmt,
                        OD.OBPComTotal AS CommissionAmt
                    FROM OBPData AS OP
                    INNER JOIN OrdersData OD ON OP.OBP_ID = OD.GOBPId
                    INNER JOIN CustomersData CD ON CD.CustomrtID = OD.FK_OrderCustomerID INNER JOIN OrdersDetails OL on OD.OrderID=OL.FK_DetailOrderID
                    WHERE OP.OBP_DelMark = 0 AND OD.OrderStatus IN (1, 3, 5, 6, 7) AND " + dateCondition + @"
                    GROUP BY OP.[OBP_ID], OP.[OBP_UserID], OP.[OBP_ApplicantName], OD.OrderID, OD.OrderType, OD.OrderStatus, CD.CustomerName, OD.OrderAmount, OD.OBPComTotal;";


            using (DataTable dtdaywise = c.GetDataTable(strQuery))
            {
                gvGOBPDaywise.DataSource = dtdaywise;
                gvGOBPDaywise.DataBind();

                if (gvGOBPDaywise.Rows.Count > 0)
                {   
                    gvGOBPDaywise.UseAccessibleHeader = true;
                    gvGOBPDaywise.HeaderRow.TableSection = TableRowSection.TableHeader;
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


    protected void gvGOBPDaywise_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if (e.Row.Cells[7].Text == "1")
                {
                    e.Row.Cells[7].Text = "<small class=\"badge badge-secondary\">New</small>";

                }
                else if (e.Row.Cells[7].Text == "3")
                {
                    e.Row.Cells[7].Text = "<small class=\"badge badge-secondary\">Accepted</small>";

                }
                else if(e.Row.Cells[7].Text == "5")
                {
                    e.Row.Cells[7].Text = "<small class=\"badge badge-secondary\">Processing</small>";

                }
                else if(e.Row.Cells[7].Text == "6")
                {
                    e.Row.Cells[7].Text = "<small class=\"badge badge-warning\">Shipped</small>";
                   
                }
                else if (e.Row.Cells[7].Text == "7")
                {
                    e.Row.Cells[7].Text = "<small class=\"badge badge-success\">Delivered</small>";
                }


                if (e.Row.Cells[8].Text == "1")
                {
                    e.Row.Cells[8].Text = "Normal";
                }
                else if(e.Row.Cells[8].Text == "2")
                {
                    e.Row.Cells[8].Text = "<small class=\"badge badge-success\">Prescription</small>";
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

    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            FillGrid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnShow_Click", ex.Message.ToString());
            return;
        }
    }
}