using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class franchisee_bluedart_waybills : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillGrid();
        }
    }

    private void FillGrid()
    {
        try
        {
            using (DataTable dtWaybill = c.GetDataTable("Select a.WayBillID, Convert(varchar, a.WayBillDate, 0) as wDate, a.FK_AssignID, " +
                    " a.FK_OrderID, Convert(varchar, a.ShipmentPickupDate, 13) as shipDate, a.AWBNo, a.CCRCRDREF, a.TokenNumber, " +
                    " Case When WayBillType=1 then 'Order' Else 'Enquiry' End as wBillCat " +
                    " From WaybillResult a Inner Join OrdersAssign b On a.FK_AssignID=b.OrdAssignID " +
                    " Where b.Fk_FranchID=" + Session["adminFranchisee"] + " UNION " +

                    " Select a.WayBillID, Convert(varchar, a.WayBillDate, 0) as wDate, a.FK_AssignID, " +
                    " a.FK_OrderID, Convert(varchar, a.ShipmentPickupDate, 13) as shipDate, a.AWBNo, a.CCRCRDREF, a.TokenNumber, " +
                    " Case When WayBillType=1 then 'Order' Else 'Enquiry' End as wBillCat " +
                    " From WaybillResult a Inner Join SavingEnqAssign b On a.FK_AssignID=b.EnqAssignID " +
                    " Where b.Fk_FranchID=" + Session["adminFranchisee"]))
            {
                gvWaybills.DataSource = dtWaybill;
                gvWaybills.DataBind();

                if (gvWaybills.Rows.Count > 0)
                {
                    gvWaybills.UseAccessibleHeader = true;
                    gvWaybills.HeaderRow.TableSection = TableRowSection.TableHeader;
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
    protected void gvWaybills_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litPdf = (Literal)e.Row.FindControl("litPdf");

                if (e.Row.Cells[8].Text == "Order")
                {
                    string fPath = Server.MapPath("~/upload/awb_waybills/waybill-" + e.Row.Cells[2].Text + ".pdf");
                    if (File.Exists(fPath))
                    {
                        litPdf.Text = "<a href=\"" + Master.rootPath + "upload/awb_waybills/waybill-" + e.Row.Cells[2].Text + ".pdf\" target=\"_blank\">View PDF</a>";
                    }
                    else
                    {
                        litPdf.Text = "";
                    }
                }
                else
                {
                    string fPath = Server.MapPath("~/upload/awb_waybills/waybillenq-" + e.Row.Cells[2].Text + ".pdf");
                    if (File.Exists(fPath))
                    {
                        litPdf.Text = "<a href=\"" + Master.rootPath + "upload/awb_waybills/waybillenq-" + e.Row.Cells[2].Text + ".pdf\" target=\"_blank\">View PDF</a>";
                    }
                    else
                    {
                        litPdf.Text = "";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvWaybills_RowDataBound", ex.Message.ToString());
            return;
        }
    }
}