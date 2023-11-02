using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class customer_request_qc_report : System.Web.UI.Page
{
    iClass c = new iClass();
    public string ordStr;
    public int franchId;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["order"] != null)
            {
                string decryptOrderId = c.DecryptString(Request.QueryString["order"].ToString());
                GetOrderDetails(Convert.ToInt32(decryptOrderId));
                FillProductGrid();
            }
        }
    }

    private void GetOrderDetails(int ordIdX)
    {
        try
        {
            using (DataTable dtOrderInfo = c.GetDataTable("Select OrderID, OrderDate, OrderAmount, OrderStatus, OrderShipAddress, FK_AddressId From OrdersData Where OrderID=" + ordIdX))
            {
                if (dtOrderInfo.Rows.Count > 0)
                {
                    DataRow row = dtOrderInfo.Rows[0];

                    StringBuilder strMarkup = new StringBuilder();

                    strMarkup.Append("<h2 class=\"clrLightBlack semiBold\">Request Number : #" + row["OrderID"].ToString() + "</h2>");
                    strMarkup.Append("<span class=\"lineSeperator\"></span>");

                    strMarkup.Append("<div class=\"float_left width70\">");
                    strMarkup.Append("<span class=\"clrGrey fontRegular tiny dispBlk mrg_B_3\">Request Date : " + Convert.ToDateTime(row["OrderDate"]).ToString("dd/MM/yyyy") + "</span>");
                    string products = "";
                    using (DataTable dtProds = c.GetDataTable("Select a.ProductID, a.ProductName From ProductsData a Inner Join OrdersDetails b On a.ProductID=b.FK_DetailProductID Where b.FK_DetailOrderID=" + ordIdX))
                    {
                        if (dtProds.Rows.Count > 0)
                        {
                            foreach (DataRow prodRow in dtProds.Rows)
                            {
                                if (products == "")
                                    products = prodRow["ProductName"].ToString();
                                else
                                    products = products + ", " + prodRow["ProductName"].ToString();
                            }
                        }
                    }
                    strMarkup.Append("<span class=\"clrGrey fontRegular tiny dispBlk\">contains " + products + "</span>");
                    strMarkup.Append("</div>");
                    strMarkup.Append("<div class=\"float_right\">");
                    strMarkup.Append("<span class=\"tiny clrGrey fontRegular\">Amt. </span>");
                    strMarkup.Append("<span class=\"regular clrLightBlack semiBold\">&#8377; " + row["OrderAmount"].ToString() + "</span>");
                    strMarkup.Append("</div>");
                    strMarkup.Append("<div class=\"float_clear\"></div>");
                    //strMarkup.Append("<span class=\"space20\"></span>");

                    ordStr = strMarkup.ToString();


                }
            }
        }
        catch (Exception ex)
        {
            ordStr = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    private void FillProductGrid()
    {
        try
        {
            string decryptOrderId = c.DecryptString(Request.QueryString["order"].ToString());
            int orderId = Convert.ToInt32(decryptOrderId);
            franchId = Convert.ToInt32(c.GetReqData("OrdersAssign", "FK_FranchID", "FK_OrderID=" + orderId));
            using (DataTable dtProd = c.GetDataTable("Select a.ProductID, a.ProductName From ProductsData a Inner Join OrdersDetails b On a.ProductID=b.FK_DetailProductID Where b.FK_DetailOrderID=" + orderId))
            {
                gvProducts.DataSource = dtProd;
                gvProducts.DataBind();
            }
        }
        catch (Exception ex)
        {
            ordStr = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    protected void gvProducts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string decryptOrderId = c.DecryptString(Request.QueryString["order"].ToString());
                int orderId = Convert.ToInt32(decryptOrderId);

                TextBox txtBatchNo = (TextBox)e.Row.FindControl("txtBatchNo");
                object batchno = c.GetReqData("QCRequest", "QCReqBatchNo", "FK_OrderID=" + orderId + " AND FK_FranchiseeID=" + franchId + " AND FK_ProductID=" + e.Row.Cells[0].Text);
                if (batchno != DBNull.Value && batchno != null && batchno.ToString() != "")
                {
                    txtBatchNo.Text = batchno.ToString();
                }


                Button btnReq = (Button)e.Row.FindControl("cmdRequest");
                Button btnDownload = (Button)e.Row.FindControl("cmdDownload");
                btnDownload.Visible = false;

                if (c.IsRecordExist("Select QCReqID From QCRequest Where FK_OrderID=" + orderId + " AND FK_FranchiseeID=" + franchId + " AND FK_ProductID=" + e.Row.Cells[0].Text)) 
                {
                    if (c.IsRecordExist("Select QCReqID From QCRequest Where FK_OrderID=" + orderId + " AND FK_FranchiseeID=" + franchId + " AND FK_ProductID=" + e.Row.Cells[0].Text + "AND  QCReqStatus=0"))
                    {
                        btnReq.Enabled = false;
                        btnReq.CssClass = "pendingBtn";
                        btnReq.Text = "Pending";
                    }
                    else
                    {
                        btnReq.Visible = false;
                        btnDownload.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ordStr = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string decryptOrderId = c.DecryptString(Request.QueryString["order"].ToString());
            int orderId = Convert.ToInt32(decryptOrderId);

            GridViewRow gRow = (GridViewRow)((Button)e.CommandSource).NamingContainer;
            franchId = Convert.ToInt32(c.GetReqData("OrdersAssign", "FK_FranchID", "FK_OrderID=" + orderId));
            TextBox txtBatchNo = (TextBox)gRow.FindControl("txtBatchNo");

            if (e.CommandName == "gvRequest")
            {
                if (txtBatchNo.Text == "")
                {
                    FillProductGrid();
                    GetOrderDetails(Convert.ToInt32(orderId));
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Batch No.');", true);
                    return;
                }

                int maxId = c.NextId("QCRequest", "QCReqID");

                c.ExecuteQuery("Insert Into QCRequest (QCReqID, QCReqDate, FK_FranchiseeID, FK_OrderID, FK_ProductID, " +
                    " QCReqBatchNo, QCReqStatus, DeviceType) Values (" + maxId + ", '" + DateTime.Now + "', " + franchId + ", " + orderId +
                    ", " + gRow.Cells[0].Text + ", '" + txtBatchNo.Text + "', 0, 'Web')");

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Request Sent');", true);
                FillProductGrid();
                GetOrderDetails(Convert.ToInt32(orderId));
            }

            if (e.CommandName == "gvDownload")
            {
                if (c.IsRecordExist("Select QCReqID From QCRequest Where FK_OrderID=" + orderId + " AND FK_FranchiseeID=" + franchId + " AND FK_ProductID=" + gRow.Cells[0].Text + " AND  QCReqBatchNo='" + txtBatchNo.Text + "' AND  QCReqStatus=1")) 
                {
                    string rPath = c.ReturnHttp();
                    int QcReqID = Convert.ToInt32(c.GetReqData("QCRequest", "QCReqID", "FK_OrderID=" + orderId + " AND FK_FranchiseeID=" + franchId + " AND FK_ProductID=" + gRow.Cells[0].Text + "AND QCReqBatchNo='" + txtBatchNo.Text + "' AND QCReqStatus=1"));
                    string repName = c.GetReqData("QCRequest", "QCReport", "QCReqID=" + QcReqID).ToString();

                    Response.Redirect(rPath + "upload/qc/" + repName.ToString(), false);

                    //FillProductGrid();
                    //GetOrderDetails(Convert.ToInt32(Request.QueryString["order"]));
                }
            }
        }
        catch (Exception ex)
        {
            ordStr = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}