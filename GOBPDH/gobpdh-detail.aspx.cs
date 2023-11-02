using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GOBPDH_gobpdh_detail : System.Web.UI.Page
{
    iClass c = new iClass();
    public string pgTitle, enqCount, headInfo, clsName, followupHistory;
    public string[] enqData = new string[50];//42
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["id"] != null)
        {
            GetGOBPEnqData(Convert.ToInt32(Request.QueryString["id"]));
            Bind_Order_Detail();
        }
    }

    private void GetGOBPEnqData(int enqIdX)
    {
        try
        {
            using (DataTable dtEnq = c.GetDataTable("Select * From OBPData Where OBP_ID=" + enqIdX))
            {
                if (dtEnq.Rows.Count > 0)
                {
                    lblId.Text = enqIdX.ToString();
                    DataRow row = dtEnq.Rows[0];

                    if (row["OBP_JoinDate"] != DBNull.Value && row["OBP_JoinDate"] != "" && row["OBP_JoinDate"] != null)
                    {
                        enqData[1] = Convert.ToDateTime(row["OBP_JoinDate"]).ToString("dd/MM/yyyy");
                    }

                    enqData[2] = row["OBP_ApplicantName"] != DBNull.Value ? row["OBP_ApplicantName"].ToString() : "";


                    enqData[8] = row["OBP_UserID"] != DBNull.Value ? row["OBP_UserID"].ToString() : "";
                    enqData[14] = row["OBP_EmpId"] != DBNull.Value ? row["OBP_EmpId"].ToString() : "";
                    //enqData[4] = row["OBP_DH_Name"] != DBNull.Value ? row["OBP_DH_Name"].ToString() : "";
                    //object mobNo = c.GetReqData("DistrictHead", "DistHdMobileNo", "DistHdUserId='" + row["OBP_DH_UserId"] + "'");
                    //if (mobNo != DBNull.Value && mobNo != null && mobNo.ToString() != "")
                    //{
                    //    enqData[5] = mobNo.ToString();
                    //}
                    enqData[12] = row["OBP_MobileNo"] != DBNull.Value ? row["OBP_MobileNo"].ToString() : "";
                    enqData[13] = row["OBP_EmailId"] != DBNull.Value ? row["OBP_EmailId"].ToString() : "";
                    //string obpType = "";
                    //switch (Convert.ToInt32(row["OBP_FKTypeID"]))
                    //{
                    //    case 1: obpType = "30K"; break;
                    //    case 2: obpType = "60K"; break;
                    //    case 3: obpType = "1Lac"; break;
                    //}
                    //enqData[0] = obpType;


                    //int custCoumt = Convert.ToInt32(c.returnAggregate("Select Count(CustomrtID) From CustomersData Where delMark=0 AND CustomerActive=1 AND FK_ObpID=" + enqIdX));

                    //enqData[3] = "<a href=\"gobp-customers.aspx?gobpId=" + enqIdX + "\" class=\"text-dark text-bold text-md\" target=\"_blank\">" + custCoumt + "</a>";
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Error Occoured while processing', type: 'error'});", true);
            c.ErrorLogHandler(this.ToString(), "GetFrEnqData", ex.Message.ToString());
            return;
        }
    }

    private void Bind_Order_Detail()
    {
        try
        {
            string strQuery = "";

            strQuery = @"SELECT	
                         	 CONVERT(VARCHAR(20), MAX(OD.[OrderDate]), 103) AS Date,
                         	 OD.[OrderID],
                         	 MAX(CD.[CustomerName]) AS Name,
                         	 COUNT(OT.[FK_DetailOrderID]) AS Items,
                         	 MAX(OD.[OrderAmount]) AS OrderAmount,
                         	 MAX(OD.[OrderStatus]) AS OrderStatus                         
                         FROM [dbo].[OBPData] AS OB
                         INNER JOIN [dbo].[OrdersData] AS OD ON OB.[OBP_ID] = OD.[GOBPId]
                         INNER JOIN [dbo].[CustomersData] AS CD ON OD.[FK_OrderCustomerID] = CD.[CustomrtID]
                         INNER JOIN [dbo].[OrdersDetails] AS OT ON OD.[OrderID] = OT.[FK_DetailOrderID]
                         WHERE OB.[OBP_ID] = " + Request.QueryString["id"] + " GROUP BY OD.[OrderID]";

            using (DataTable dtgobpOrd = c.GetDataTable(strQuery))
            {
                gvOrder.DataSource = dtgobpOrd;
                gvOrder.DataBind();
                if (dtgobpOrd.Rows.Count > 0)
                {
                    gvOrder.UseAccessibleHeader = true;
                    gvOrder.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "Bind_Order_Detail", ex.Message.ToString());
            return;
        }
    }

    protected void gvOrder_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {               
                // orderStatus=0 > added to cart, 1 > Order placed (New/pending), 2 > CANCELL ORDER BY CUSTOMER , 3 > Accepted, 4 > Denied, 5 > Processing , 6 > Shipped , 7 > deliverd
                // 8 > Re-assigned(rejected by 0001), 9 > Rejected by shop for reason aorder amount low, 10 > OrderReturned
                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                switch (e.Row.Cells[1].Text)
                {

                    case "0":
                        litStatus.Text = "<div class=\"ordNew\">Order In Cart</div>";
                        break;
                    case "1":
                        litStatus.Text = "<div class=\"ordNew\">New</div>";
                        break;
                    case "2":
                        litStatus.Text = "<div class=\"ordCancCust\">Cancel By Customer</div>";
                        break;
                    case "3":
                        litStatus.Text = "<div class=\"ordAccepted\">Accepted</div>";
                        break;
                    case "4":
                        string frCode = "", frInfo = "";
                        if (c.IsRecordExist("Select OrdAssignID From OrdersAssign Where OrdAssignStatus=2 AND FK_OrderID=" + e.Row.Cells[0].Text + ""))
                        {
                            int frId = Convert.ToInt32(c.GetReqData("OrdersAssign", "Top 1 Fk_FranchID", "OrdAssignStatus=2 AND FK_OrderID=" + e.Row.Cells[0].Text + " Order By OrdAssignID DESC"));
                            frCode = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + frId).ToString();
                            frInfo = " Rejected By " + frCode;
                        }
                        litStatus.Text = "<div class=\"ordDenied\">Denied By Admin " + frInfo + "</div>";
                        break;
                    case "5":
                        litStatus.Text = "<div class=\"ordProcessing\">Processing</div>";
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
                        int shopId = Convert.ToInt32(c.GetReqData("OrdersAssign", "Top 1 Fk_FranchID", "OrdAssignStatus=2 AND FK_OrderID=" + e.Row.Cells[0].Text + " Order By OrdAssignID DESC"));
                        string shopCode = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + shopId).ToString();
                        litStatus.Text = "<div class=\"ordProcessing\">Rejected by " + shopCode + " - Order Amount Low</div>";
                        break;
                    case "10":
                        litStatus.Text = "<div class=\"ordDenied\">Returned By Customer</div>";
                        break;
                    case "11":
                        litStatus.Text = "<div class=\"ordDenied\">Rejected by Doctor</div>";
                        break;
                    case "12":
                        litStatus.Text = "<div class=\"ordDenied\">No Response to Call</div>";
                        break;
                    case "13":
                        litStatus.Text = "<div class=\"ordDenied\">Refund Request by Customer</div>";
                        break;
                    case "14":
                        litStatus.Text = "<div class=\"ordDenied\">Refund Request in Process</div>";
                        break;
                    case "15":
                        litStatus.Text = "<div class=\"ordDenied\">Refund Request Completed</div>";
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("gobpdh-report.aspx");
    }
}