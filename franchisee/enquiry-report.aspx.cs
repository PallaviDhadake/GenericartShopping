using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class franchisee_enquiry_report : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
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
            string fIdx = Session["adminFranchisee"].ToString();
            if (Request.QueryString["type"] != null)
            {
                switch (Request.QueryString["type"])
                {
                    case "new":
                        if (fIdx == "24")
                        {
                            strQuery = "Select a.EnqAssignID, a.FK_CalcID, CONVERT(varchar(20), a.EnqAssignDate, 103)as ordDate, a.EnqAssignStatus , (Select COUNT(CalcItemID) from SavingCalcItems where FK_CalcID = b.CalcID ) as ProductCount, c.CustomerName, STUFF((Select ', ' + RTRIM(LTRIM(GenericMedicine)) From SavingCalcItems Where FK_CalcID IN (b.CalcID) FOR XML PATH('')), 1, 1, '' ) as EnqProducts From SavingEnqAssign a Inner Join SavingCalc b On a.FK_CalcID = b.CalcID Inner Join CustomersData c On b.FK_CustId = c.CustomrtID Where a.EnqAssignStatus=0 AND a.EnqReAssign=0 And a.Fk_FranchID=" + fIdx + " AND (CONVERT(varchar(20), a.EnqAssignDate, 112) >= CONVERT(varchar(20), CAST('2021-08-15' as datetime) ,112))";
                        }
                        else
                        {
                            strQuery = "Select a.EnqAssignID, a.FK_CalcID, CONVERT(varchar(20), a.EnqAssignDate, 103)as ordDate, a.EnqAssignStatus , (Select COUNT(CalcItemID) from SavingCalcItems where FK_CalcID = b.CalcID ) as ProductCount, c.CustomerName, STUFF((Select ', ' + RTRIM(LTRIM(GenericMedicine)) From SavingCalcItems Where FK_CalcID IN (b.CalcID) FOR XML PATH('')), 1, 1, '' ) as EnqProducts From SavingEnqAssign a Inner Join SavingCalc b On a.FK_CalcID = b.CalcID Inner Join CustomersData c On b.FK_CustId = c.CustomrtID Where a.EnqAssignStatus=0 AND a.EnqReAssign=0 And a.Fk_FranchID=" + fIdx + "";
                        }
                        break;

                    case "accepted":
                        strQuery = "Select a.EnqAssignID, a.FK_CalcID, CONVERT(varchar(20), a.EnqAssignDate, 103)as ordDate, a.EnqAssignStatus , (Select COUNT(CalcItemID) from SavingCalcItems where FK_CalcID = b.CalcID ) as ProductCount, c.CustomerName, STUFF((Select ', ' + RTRIM(LTRIM(GenericMedicine)) From SavingCalcItems Where FK_CalcID IN (b.CalcID) FOR XML PATH('')), 1, 1, '' ) as EnqProducts From SavingEnqAssign a Inner Join SavingCalc b On a.FK_CalcID = b.CalcID Inner Join CustomersData c On b.FK_CustId = c.CustomrtID Where a.EnqAssignStatus IN (1,5) And a.Fk_FranchID=" + fIdx + "";
                        break;

                    case "rejected":
                        strQuery = "Select a.EnqAssignID, a.FK_CalcID, CONVERT(varchar(20), a.EnqAssignDate, 103)as ordDate, a.EnqAssignStatus , (Select COUNT(CalcItemID) from SavingCalcItems where FK_CalcID = b.CalcID ) as ProductCount, c.CustomerName, STUFF((Select ', ' + RTRIM(LTRIM(GenericMedicine)) From SavingCalcItems Where FK_CalcID IN (b.CalcID) FOR XML PATH('')), 1, 1, '' ) as EnqProducts From SavingEnqAssign a Inner Join SavingCalc b On a.FK_CalcID = b.CalcID Inner Join CustomersData c On b.FK_CustId = c.CustomrtID Where a.EnqAssignStatus=2 And a.Fk_FranchID=" + fIdx + "";
                        break;

                    case "monthly":
                        strQuery = "Select a.EnqAssignID, a.FK_CalcID, CONVERT(varchar(20), a.EnqAssignDate, 103)as ordDate, a.EnqAssignStatus , (Select COUNT(CalcItemID) from SavingCalcItems where FK_CalcID = b.CalcID ) as ProductCount, c.CustomerName, STUFF((Select ', ' + RTRIM(LTRIM(GenericMedicine)) From SavingCalcItems Where FK_CalcID IN (b.CalcID) FOR XML PATH('')), 1, 1, '' ) as EnqProducts From SavingEnqAssign a Inner Join SavingCalc b On a.FK_CalcID = b.CalcID Inner Join CustomersData c On b.FK_CustId = c.CustomrtID Where b.MreqFlag=1 And a.Fk_FranchID=" + fIdx + "";
                        break;

                    case "dispatched":
                        strQuery = "Select a.EnqAssignID, a.FK_CalcID, CONVERT(varchar(20), a.EnqAssignDate, 103)as ordDate, a.EnqAssignStatus , (Select COUNT(CalcItemID) from SavingCalcItems where FK_CalcID = b.CalcID ) as ProductCount, c.CustomerName, STUFF((Select ', ' + RTRIM(LTRIM(GenericMedicine)) From SavingCalcItems Where FK_CalcID IN (b.CalcID) FOR XML PATH('')), 1, 1, '' ) as EnqProducts From SavingEnqAssign a Inner Join SavingCalc b On a.FK_CalcID = b.CalcID Inner Join CustomersData c On b.FK_CustId = c.CustomrtID Where a.EnqAssignStatus=6 And a.Fk_FranchID=" + fIdx + "";
                        break;

                    case "delivered":
                        strQuery = "Select a.EnqAssignID, a.FK_CalcID, CONVERT(varchar(20), a.EnqAssignDate, 103)as ordDate, a.EnqAssignStatus , (Select COUNT(CalcItemID) from SavingCalcItems where FK_CalcID = b.CalcID ) as ProductCount, c.CustomerName, STUFF((Select ', ' + RTRIM(LTRIM(GenericMedicine)) From SavingCalcItems Where FK_CalcID IN (b.CalcID) FOR XML PATH('')), 1, 1, '' ) as EnqProducts From SavingEnqAssign a Inner Join SavingCalc b On a.FK_CalcID = b.CalcID Inner Join CustomersData c On b.FK_CustId = c.CustomrtID Where a.EnqAssignStatus=7 And a.Fk_FranchID=" + fIdx + "";
                        break;

                    case "returned":
                        strQuery = "Select a.EnqAssignID, a.FK_CalcID, CONVERT(varchar(20), a.EnqAssignDate, 103)as ordDate, a.EnqAssignStatus , (Select COUNT(CalcItemID) from SavingCalcItems where FK_CalcID = b.CalcID ) as ProductCount, c.CustomerName, STUFF((Select ', ' + RTRIM(LTRIM(GenericMedicine)) From SavingCalcItems Where FK_CalcID IN (b.CalcID) FOR XML PATH('')), 1, 1, '' ) as EnqProducts From SavingEnqAssign a Inner Join SavingCalc b On a.FK_CalcID = b.CalcID Inner Join CustomersData c On b.FK_CustId = c.CustomrtID Where a.EnqAssignStatus=10 And a.Fk_FranchID=" + fIdx + "";
                        break;
                }
            }
            else
            {
                strQuery = "Select a.EnqAssignID, a.FK_CalcID, CONVERT(varchar(20), a.EnqAssignDate, 103)as ordDate, a.EnqAssignStatus , (Select COUNT(CalcItemID) from SavingCalcItems where FK_CalcID = b.CalcID ) as ProductCount, c.CustomerName, STUFF((Select ', ' + RTRIM(LTRIM(GenericMedicine)) From SavingCalcItems Where FK_CalcID IN (b.CalcID) FOR XML PATH('')), 1, 1, '' ) as EnqProducts From SavingEnqAssign a Inner Join SavingCalc b On a.FK_CalcID = b.CalcID Inner Join CustomersData c On b.FK_CustId = c.CustomrtID Where a.Fk_FranchID=" + fIdx + "";
            }
            using (DataTable dtOrder = c.GetDataTable(strQuery))
            {
                gvOrder.DataSource = dtOrder;
                gvOrder.DataBind();
                if (gvOrder.Rows.Count > 0)
                {
                    gvOrder.UseAccessibleHeader = true;
                    gvOrder.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvOrder_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string ordID = c.GetReqData("SavingEnqAssign", "FK_CalcID", "EnqAssignID=" + e.Row.Cells[0].Text + "").ToString();

                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                //litAnch.Text = "<a href=\"enquiry-details.aspx?id=" + ordID + "&assignId=" + e.Row.Cells[0].Text + "\" class=\"gView\" title=\"View/Edit\"></a>";
                if (Request.QueryString["type"] != null)
                    litAnch.Text = "<a href=\"enquiry-details.aspx?id=" + ordID + "&type=" + Request.QueryString["type"] + "&assignId=" + e.Row.Cells[0].Text + "\" class=\"gView\" title=\"View/Edit\"></a>";
                else
                    litAnch.Text = "<a href=\"enquiry-details.aspx?id=" + ordID + "&assignId=" + e.Row.Cells[0].Text + "\" class=\"gView\" title=\"View/Edit\"></a>";


                // EnqAssignStatus 0 > Pending, 1 > Accepted, 2 > Rejected
                int reAssignStatus = Convert.ToInt32(c.GetReqData("SavingEnqAssign", "EnqReAssign", "EnqAssignID=" + e.Row.Cells[0].Text));
                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                switch (e.Row.Cells[1].Text)
                {
                    case "0":
                        //litStatus.Text = "<div class=\"ordNew\">New</div>";
                        if (c.IsRecordExist("Select EnqAssignID From SavingEnqAssign Where FK_CalcID=" + e.Row.Cells[2].Text + " AND Fk_FranchID=" + Session["adminFranchisee"] + " AND EnqAssignStatus=0 AND EnqReAssign=1 AND EnqAssignID<>" + e.Row.Cells[0].Text))
                        {
                            if (reAssignStatus == 1)
                            {
                                litStatus.Text = "<div class=\"ordAutoRoute\">Auto Routed</div>";
                            }
                            else
                            {
                                litStatus.Text = "<div class=\"ordNew\">Re Assigned</div>";
                            }
                        }
                        else
                        {
                            if (reAssignStatus == 0)
                            {
                                string frCode = "", frInfo = "";
                                if (c.IsRecordExist("Select EnqAssignID From SavingEnqAssign Where EnqAssignStatus=2 AND FK_CalcID=" + e.Row.Cells[2].Text + ""))
                                {
                                    int frId = Convert.ToInt32(c.GetReqData("SavingEnqAssign", "Top 1 Fk_FranchID", "EnqAssignStatus=2 AND FK_CalcID=" + e.Row.Cells[2].Text + " Order By EnqAssignID DESC"));
                                    frCode = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + frId).ToString();
                                    frInfo = " (Rejected By " + frCode + ")";
                                }
                                litStatus.Text = "<div class=\"ordNew\">New " + frInfo + "</div>";
                            }
                            if (reAssignStatus == 1)
                            {
                                litStatus.Text = "<div class=\"ordAutoRoute\">Auto Routed</div>";
                            }
                        }
                        break;
                    case "1":
                        litStatus.Text = "<div class=\"ordAccepted\">Accepted</div>";
                        break;
                    case "2":
                        litStatus.Text = "<div class=\"ordDenied\">Rejected</div>";
                        break;
                    case "5":
                        litStatus.Text = "<div class=\"ordProcessing\">Inprocess</div>";
                        break;
                    case "6":
                        litStatus.Text = "<div class=\"ordShipped\">Shipped</div>";
                        break;
                    case "7":
                        litStatus.Text = "<div class=\"ordDelivered\">Delivered</div>";
                        break;
                    case "10":
                        litStatus.Text = "<div class=\"ordDenied\">Returned By Customer</div>";
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvOrder_RowDataBound", ex.Message.ToString());
            return;
        }
    }
}