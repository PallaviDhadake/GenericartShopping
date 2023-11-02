using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class franchisee_qc_report_requests : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] ordData = new string[20];
    public string qcRep, deviceType;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    editReq.Visible = true;
                    viewReq.Visible = false;
                    GetRequestDetails(Convert.ToInt32(Request.QueryString["id"]));
                }
                else
                {
                    editReq.Visible = false;
                    viewReq.Visible = true;
                    FillGrid();
                }
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
            using (DataTable dtReq = c.GetDataTable("Select a.QCReqID, Convert(varchar(20), a.QCReqDate, 103) as reqDate, a.FK_OrderID, a.QCReqStatus, a.QCReqBatchNo, b.ProductName, isnull(a.DeviceType, '-') as DeviceType From QCRequest a Inner Join ProductsData b On a.FK_ProductID=b.ProductID Where a.FK_FranchiseeID=" + Session["adminFranchisee"]))
            {
                gvRequests.DataSource = dtReq;
                gvRequests.DataBind();
                if (dtReq.Rows.Count > 0)
                {
                    gvRequests.UseAccessibleHeader = true;
                    gvRequests.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvRequests_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int custId = Convert.ToInt32(c.GetReqData("OrdersData", "FK_OrderCustomerID", "OrderID=" + e.Row.Cells[2].Text));

                Literal litCust = (Literal)e.Row.FindControl("litCust");
                litCust.Text = c.GetReqData("CustomersData", "CustomerName", "CustomrtID=" + custId).ToString();

                Literal litMob = (Literal)e.Row.FindControl("litMob");
                litMob.Text = c.GetReqData("CustomersData", "CustomerMobile", "CustomrtID=" + custId).ToString();

                Literal litStatus = (Literal)e.Row.FindControl("litStatus");

                if (e.Row.Cells[1].Text == "0")
                {
                    litStatus.Text = "<span class=\"ordProcessing\">Pending</span>";
                }
                else
                {
                    litStatus.Text = "<span class=\"ordDelivered\">Report Uploaded</span>";
                }

                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"qc-report-requests.aspx?id=" + e.Row.Cells[0].Text + "\" class=\"gAnch\"></a>";
            }
        }
        catch (Exception ex)
        {

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvRequests_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    private void GetRequestDetails(int reqIdX)
    {
        try
        {
            using (DataTable dtReqInfo = c.GetDataTable("Select QCReqID, QCReqDate, FK_FranchiseeID, FK_OrderID, FK_ProductID, QCReqBatchNo, QCReport, QCReqStatus, DeviceType From QCRequest Where QCReqID=" + reqIdX))
            {
                if (dtReqInfo.Rows.Count > 0)
                {
                    DataRow row = dtReqInfo.Rows[0];

                    ordData[0] = row["FK_OrderID"].ToString();
                    string ordDate = c.GetReqData("OrdersData", "OrderDate", "OrderID=" + row["FK_OrderID"]).ToString();
                    ordData[1] = Convert.ToDateTime(ordDate).ToString("dd/MM/yyyy hh:mm tt");
                    ordData[2] = Convert.ToDateTime(row["QCReqDate"]).ToString("dd/MM/yyyy hh:mm tt");
                    ordData[3] = c.GetReqData("ProductsData", "ProductName", "ProductID=" + row["FK_ProductID"]).ToString();
                    ordData[4] = row["QCReqBatchNo"].ToString();

                    int custId = Convert.ToInt32(c.GetReqData("OrdersData", "FK_OrderCustomerID", "OrderID=" + row["FK_OrderID"]));
                    ordData[5] = c.GetReqData("CustomersData", "CustomerName", "CustomrtID=" + custId).ToString();
                    ordData[6] = c.GetReqData("CustomersData", "CustomerMobile", "CustomrtID=" + custId).ToString();
                    ordData[7] = c.GetReqData("CustomersData", "CustomerEmail", "CustomrtID=" + custId).ToString();

                    if (row["QCReport"] != DBNull.Value && row["QCReport"] != null && row["QCReport"].ToString() != "")
                    {
                        qcRep = "<span class=\"clrRejected\"><i class=\"fas fa-file-pdf\"></i><a href=\"" + Master.rootPath + "upload/qc/" + row["QCReport"].ToString() + "\" class=\"\" target=\"_blank\"> View Report</a></span>";

                        btnUpload.Text = "Update Report";
                    }

                    deviceType = row["DeviceType"] != DBNull.Value && row["DeviceType"] != null && row["DeviceType"].ToString() != "" ? row["DeviceType"].ToString() : "";
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetRequestDetails", ex.Message.ToString());
            return;
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            GetRequestDetails(Convert.ToInt32(Request.QueryString["id"]));
            string fileName = "";
            if (fuReport.HasFile)
            {
                string fExt = Path.GetExtension(fuReport.FileName).ToString().ToLower();
                if (fExt == ".pdf")
                {
                    fileName = "qc-report-" + DateTime.Now.ToString("ddmmyyhhmmss") + fExt;
                    string filePath = "~/upload/qc/";
                    fuReport.SaveAs(Server.MapPath(filePath) + fileName);
                    c.ExecuteQuery("Update QCRequest Set QCReport='" + fileName + "', QCReqStatus=1 Where QCReqID=" + Request.QueryString["id"]);
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Report Uploaded Successfully');", true);
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('qc-report-requests.aspx', 2000);", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Only .pdf files are allowed');", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select File to upload');", true);
                return;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnUpload_Click", ex.Message.ToString());
            return;
        }
    }
}