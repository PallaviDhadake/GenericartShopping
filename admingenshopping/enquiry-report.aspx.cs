using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class admingenshopping_enquiry_report : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] ordData = new String[10];
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
            string strQuery = "";
            if (Request.QueryString["type"] != null)
            {
                switch (Request.QueryString["type"])
                {
                    case "new":
                        strQuery = "Select DISTINCT a.CalcID, a.CalcDate, convert(varchar(20), a.CalcDate, 103) as calDate , b.CustomerName, b.CustomerMobile, isnull(a.DeviceType, '-') as DeviceType, " +
                            " (Select COUNT(CalcItemID) from SavingCalcItems where FK_CalcID = a.CalcID ) as ProductCount, " +
                            " STUFF((Select ', ' + RTRIM(LTRIM(GenericMedicine)) From SavingCalcItems Where FK_CalcID IN (a.CalcID) FOR XML PATH('')), 1, 1, '' ) as EnqProducts, a.EnqStatus " +
                            " From SavingCalc a Inner Join CustomersData b on a.FK_CustId = b.CustomrtID Where a.EnqStatus = 1 " +
                            " Order By a.CalcDate DESC, a.CalcID DESC";
                        break;

                    case "accepted":
                        strQuery = "Select DISTINCT a.CalcID, a.CalcDate, convert(varchar(20), a.CalcDate, 103) as calDate , b.CustomerName, b.CustomerMobile, isnull(a.DeviceType, '-') as DeviceType,  " +
                            " (Select COUNT(CalcItemID) from SavingCalcItems where FK_CalcID = a.CalcID ) as ProductCount, " +
                            " STUFF((Select ', ' + RTRIM(LTRIM(GenericMedicine)) From SavingCalcItems Where FK_CalcID IN (a.CalcID) FOR XML PATH('')), 1, 1, '' ) as EnqProducts, a.EnqStatus " +
                            " From SavingCalc a Inner Join CustomersData b on a.FK_CustId = b.CustomrtID Where a.EnqStatus = 2 " +
                            " Order By a.CalcDate DESC, a.CalcID DESC";
                        break;
                    case "converted":
                        strQuery = "Select DISTINCT a.CalcID, a.CalcDate, convert(varchar(20), a.CalcDate, 103) as calDate , b.CustomerName, b.CustomerMobile, isnull(a.DeviceType, '-') as DeviceType, " +
                            " (Select COUNT(CalcItemID) from SavingCalcItems where FK_CalcID = a.CalcID ) as ProductCount, " +
                            " STUFF((Select ', ' + RTRIM(LTRIM(GenericMedicine)) From SavingCalcItems Where FK_CalcID IN (a.CalcID) FOR XML PATH('')), 1, 1, '' ) as EnqProducts, a.EnqStatus " +
                            " From SavingCalc a Inner Join CustomersData b on a.FK_CustId = b.CustomrtID Where a.EnqStatus = 3 " +
                            " Order By a.CalcDate DESC, a.CalcID DESC";
                        break;
                    case "notConverted":
                        strQuery = "Select DISTINCT a.CalcID, a.CalcDate, convert(varchar(20), a.CalcDate, 103) as calDate , b.CustomerName, b.CustomerMobile, isnull(a.DeviceType, '-') as DeviceType, " +
                            " (Select COUNT(CalcItemID) from SavingCalcItems where FK_CalcID = a.CalcID ) as ProductCount, " +
                            " STUFF((Select ', ' + RTRIM(LTRIM(GenericMedicine)) From SavingCalcItems Where FK_CalcID IN (a.CalcID) FOR XML PATH('')), 1, 1, '' ) as EnqProducts, a.EnqStatus " +
                            " From SavingCalc a Inner Join CustomersData b on a.FK_CustId = b.CustomrtID Where a.EnqStatus = 4 " +
                            " Order By a.CalcDate DESC, a.CalcID DESC";
                        break;
                    case "incomplete":
                        strQuery = "Select DISTINCT a.CalcID, a.CalcDate, convert(varchar(20), a.CalcDate, 103) as calDate , b.CustomerName, b.CustomerMobile, isnull(a.DeviceType, '-') as DeviceType, " +
                            " (Select COUNT(CalcItemID) from SavingCalcItems where FK_CalcID = a.CalcID ) as ProductCount, " +
                            " STUFF((Select ', ' + RTRIM(LTRIM(GenericMedicine)) From SavingCalcItems Where FK_CalcID IN (a.CalcID) FOR XML PATH('')), 1, 1, '' ) as EnqProducts, a.EnqStatus " +
                            " From SavingCalc a Inner Join CustomersData b on a.FK_CustId = b.CustomrtID Where a.EnqStatus = 0 " +
                            " Order By a.CalcDate DESC, a.CalcID DESC";
                        break;
                    case "monthly":
                        strQuery = "Select DISTINCT a.CalcID, a.CalcDate, convert(varchar(20), a.CalcDate, 103) as calDate , b.CustomerName, b.CustomerMobile, isnull(a.DeviceType, '-') as DeviceType, " +
                            " (Select COUNT(CalcItemID) from SavingCalcItems where FK_CalcID = a.CalcID ) as ProductCount, " +
                            " STUFF((Select ', ' + RTRIM(LTRIM(GenericMedicine)) From SavingCalcItems Where FK_CalcID IN (a.CalcID) FOR XML PATH('')), 1, 1, '' ) as EnqProducts, a.EnqStatus " +
                            " From SavingCalc a Inner Join CustomersData b on a.FK_CustId = b.CustomrtID Where a.EnqStatus<>0 AND a.MreqFlag = 1 " +
                            " Order By a.CalcDate DESC, a.CalcID DESC";
                        break;
                    case "dispatched":
                        strQuery = "Select DISTINCT a.CalcID, a.CalcDate, convert(varchar(20), a.CalcDate, 103) as calDate , b.CustomerName, b.CustomerMobile, isnull(a.DeviceType, '-') as DeviceType, " +
                            " (Select COUNT(CalcItemID) from SavingCalcItems where FK_CalcID = a.CalcID ) as ProductCount, " +
                            " STUFF((Select ', ' + RTRIM(LTRIM(GenericMedicine)) From SavingCalcItems Where FK_CalcID IN (a.CalcID) FOR XML PATH('')), 1, 1, '' ) as EnqProducts, a.EnqStatus " +
                            " From SavingCalc a Inner Join CustomersData b on a.FK_CustId = b.CustomrtID Where a.EnqStatus = 6 " +
                            " Order By a.CalcDate DESC, a.CalcID DESC";
                        break;

                    case "delivered":
                        strQuery = "Select DISTINCT a.CalcID, a.CalcDate, convert(varchar(20), a.CalcDate, 103) as calDate , b.CustomerName, b.CustomerMobile, isnull(a.DeviceType, '-') as DeviceType, " +
                            " (Select COUNT(CalcItemID) from SavingCalcItems where FK_CalcID = a.CalcID ) as ProductCount, " +
                            " STUFF((Select ', ' + RTRIM(LTRIM(GenericMedicine)) From SavingCalcItems Where FK_CalcID IN (a.CalcID) FOR XML PATH('')), 1, 1, '' ) as EnqProducts, a.EnqStatus " +
                            " From SavingCalc a Inner Join CustomersData b on a.FK_CustId = b.CustomrtID Where a.EnqStatus = 7 " +
                            " Order By a.CalcDate DESC, a.CalcID DESC";
                        break;

                    case "returned":
                        strQuery = "Select DISTINCT a.CalcID, a.CalcDate, convert(varchar(20), a.CalcDate, 103) as calDate , b.CustomerName, b.CustomerMobile, isnull(a.DeviceType, '-') as DeviceType, " +
                            " (Select COUNT(CalcItemID) from SavingCalcItems where FK_CalcID = a.CalcID ) as ProductCount, " +
                            " STUFF((Select ', ' + RTRIM(LTRIM(GenericMedicine)) From SavingCalcItems Where FK_CalcID IN (a.CalcID) FOR XML PATH('')), 1, 1, '' ) as EnqProducts, a.EnqStatus " +
                            " From SavingCalc a Inner Join CustomersData b on a.FK_CustId = b.CustomrtID Where a.EnqStatus = 10 " +
                            " Order By a.CalcDate DESC, a.CalcID DESC";
                        break;
                }
            }
            else
            {
                strQuery = "Select DISTINCT a.CalcID, a.CalcDate, convert(varchar(20), a.CalcDate, 103) as calDate , b.CustomerName, b.CustomerMobile, isnull(a.DeviceType, '-') as DeviceType, " +
                    " (Select COUNT(CalcItemID) from SavingCalcItems where FK_CalcID = a.CalcID ) as ProductCount, " +
                    " STUFF((Select ', ' + RTRIM(LTRIM(GenericMedicine)) From SavingCalcItems Where FK_CalcID IN (a.CalcID) FOR XML PATH('')), 1, 1, '' ) as EnqProducts, a.EnqStatus " +
                    " From SavingCalc a Inner Join CustomersData b on a.FK_CustId = b.CustomrtID " +
                    " Order By a.CalcDate DESC, a.CalcID DESC";
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
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"enquiry-details.aspx?id=" + e.Row.Cells[0].Text + "\" class=\"gView\" title=\"View/Edit\"></a>";

                // EnqStatus= 0 > New, 1 > Accepted by admin & assigned to franch, 2 > Converted , 3 > Not Converted
                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                switch (e.Row.Cells[1].Text)
                {
                    case "0":
                        litStatus.Text = "<div class=\"stGrey\">Incomplete</div>";
                        break;
                    case "1":
                        litStatus.Text = "<div class=\"ordNew\">New</div>";
                        break;
                    case "2":
                        litStatus.Text = "<div class=\"ordProcessing\">Accepted & Assigned</div>";
                        break;
                    case "3":
                        litStatus.Text = "<div class=\"ordAccepted\">Converted</div>";
                        break;
                    case "4":
                        string frCode = "", frInfo = "";
                        if (c.IsRecordExist("Select EnqAssignID From SavingEnqAssign Where EnqAssignStatus=2 AND FK_CalcID=" + e.Row.Cells[0].Text + ""))
                        {
                            int frId = Convert.ToInt32(c.GetReqData("SavingEnqAssign", "Top 1 Fk_FranchID", "EnqAssignStatus=2 AND FK_CalcID=" + e.Row.Cells[0].Text + " Order By EnqAssignID DESC"));
                            frCode = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + frId).ToString();
                            frInfo = " Rejected By " + frCode;
                        }
                        litStatus.Text = "<div class=\"ordDenied\">Not Converted" + frInfo + "</div>";
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
                    case "8":
                        litStatus.Text = "<div class=\"ordDenied\">Rejected By GMMH0001</div>";
                        break;
                    case "9":
                        int shopId = Convert.ToInt32(c.GetReqData("SavingEnqAssign", "Top 1 Fk_FranchID", "EnqAssignStatus=2 AND FK_CalcID=" + e.Row.Cells[0].Text + " Order By EnqAssignID DESC"));
                        string shopCode = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + shopId).ToString();
                        litStatus.Text = "<div class=\"ordProcessing\">Rejected by " + shopCode + " - Order Amount Low</div>";
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