using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.Services;

public partial class supportteam_followup_order_detail : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] arrCust = new string[10];
   
    public string[] arrLastCall = new string[10];
    public string[] arrOrd = new string[20];
    public string[] arrOrdInfo = new string[10];
    public string[] arrGobp = new string[5];    
    public string followupHistory, lockMsg, ordConvertBy, newOrdUrl, lookupUrl, callMsg, myCustId, callTime;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            myCustId = Request.QueryString["custId"].ToString();
            c.FillComboBox("FL_Status", "FL_StatusID", "FollowupOrdersStatus", "DelMark=0 AND FL_StatusID NOT IN (3, 6, 7)", "FL_StatusID", 0, ddrRemark);
            FillDDR();
            GetOrderInfo();

            if (LastCallResponse() == true)
            {
                string script = "lastCallPopup();"; // The JavaScript function call
                ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", script, true);
            }
            
        }

        if (Request.QueryString["custId"] != null)
        {
            lookupUrl = "cust-lookup.aspx?custId=" + Request.QueryString["custId"];
            GetCustInfo();

            newOrdUrl = "<a href=\"submit-po.aspx?custId=" + Request.QueryString["custId"] + "&type=newOrd\" target=\"_blank\" class=\"btn btn-md btn-warning\">New Order</a>";
            btnRepeat.Enabled = false;

            // Check if there any call done to this customer TODAY only.

            if (c.IsRecordExist("Select FlupID From FollowupOrders Where FlupRemarkStatusID<>2 AND CONVERT(varchar(20), FlupDate, 112) = CONVERT(varchar(20), CAST(GETDATE() as datetime), 112) AND FK_CustomerId=" + Request.QueryString["custId"]))
            {
                int teamMemId = Convert.ToInt32(c.GetReqData("FollowupOrders", "FK_TeamMemberId", "CONVERT(varchar(20), FlupDate, 112) = CONVERT(varchar(20), CAST(GETDATE() as datetime), 112) AND FK_CustomerId=" + Request.QueryString["custId"]));
                string teamName = c.GetReqData("SupportTeam", "TeamPersonName", "TeamID=" + teamMemId).ToString();
                string flDateTime = c.GetReqData("FollowupOrders", "FlupDate", "CONVERT(varchar(20), FlupDate, 112) = CONVERT(varchar(20), CAST(GETDATE() as datetime), 112) AND FK_CustomerId=" + Request.QueryString["custId"]).ToString();
                string flTime = Convert.ToDateTime(flDateTime).ToString("dd/MM/yyyy hh:mm tt");
                
                callMsg = "<span class=\"text-success text-md text-bold\">This Customer has get call TODAY at " + flTime + " by " + teamName + "</span>";
                Response.Redirect("team-alert.aspx?custId=" + Request.QueryString["custId"]);
            }

            GetFollowupHistory();

            if (Request.QueryString["ordId"] != null)
            {
                GetOrdersData();
                GetCallStatus();
                btnRepeat.Enabled = true;
                if (c.IsRecordExist("Select FlupID From FollowupOrders Where FlupRptOrderId=" + Request.QueryString["ordId"]))
                {
                    int teamMemId = Convert.ToInt32(c.GetReqData("FollowupOrders", "FK_TeamMemberId", "FlupRptOrderId=" + Request.QueryString["ordId"]));
                    ordConvertBy = "This Order is Converted By - " + c.GetReqData("SupportTeam", "TeamPersonName", "TeamID=" + teamMemId).ToString();
                }

                newOrdUrl = "<a href=\"submit-po.aspx?custId=" + Request.QueryString["custId"] + "&ordId=" + Request.QueryString["ordId"] + "&type=newOrd\" target=\"_blank\" class=\"btn btn-md btn-warning\">New Order</a>";
            }
        }

      
    }

    private void FillDDR()
    {
        try
        {
            SqlConnection con = new SqlConnection(c.OpenConnection());

            SqlDataAdapter da = new SqlDataAdapter("Select a.FranchShopCode+' - '+b.FranchName as frName, a.FK_FranchID From CompanyOwnShops a Inner Join FranchiseeData b On a.FranchShopCode=b.FranchShopCode Where a.DelMark=0 Order By frName", con);
            DataSet ds = new DataSet();
            da.Fill(ds, "myCombo");

            ddrShops.DataSource = ds.Tables[0];
            ddrShops.DataTextField = ds.Tables[0].Columns["frName"].ColumnName.ToString();
            ddrShops.DataValueField = ds.Tables[0].Columns["FK_FranchID"].ColumnName.ToString();
            ddrShops.DataBind();

            ddrShops.Items.Insert(0, "<-Select->");
            ddrShops.Items[0].Value = "0";

            

            con.Close();
            con = null;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Error Occoured while processing', type: 'error'});", true);
            c.ErrorLogHandler(this.ToString(), "FillDDR", ex.Message.ToString());
            return;
        }
    }

    private void GetCallStatus()
    {
        try
        {
            object busyFlag = c.GetReqData("CustomersData", "CallBusyFlag", "CustomrtID=" + Request.QueryString["custId"]);
            if (busyFlag != DBNull.Value && busyFlag != null && busyFlag.ToString() != "")
            {
                if (Convert.ToInt32(busyFlag) == 0)
                {
                    btnLock.Text = "Lock Call";
                }
                else
                {
                    int busyBy = Convert.ToInt32(c.GetReqData("CustomersData", "CallBusyBy", "CustomrtID=" + Request.QueryString["custId"]));
                    if (busyBy == Convert.ToInt32(Session["adminSupport"]))
                    {
                        btnLock.Text = "Unlock Call";
                    }
                    else
                    {
                        string flBy = c.GetReqData("SupportTeam", "TeamPersonName", "TeamID=" + busyBy).ToString();
                        lockMsg = "<span class=\"text-red text-bold text-lg\">This User is busy with " + flBy + "</span>";
                        btnLock.Visible = false;

                        btnRepeat.Visible = false;
                        btnEditRepeat.Visible = false;
                        //btnNew.Visible = false;
                        btnSave.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetCallStatus", ex.Message.ToString());
            return;
        }
    }


    /// <summary>
    ///  Collect Last call response information for Modal-Popup 19-Aug-23 by Vinayak
    /// </summary>
    private bool LastCallResponse()
    {
        try
        {
            int custIdX = Convert.ToInt32(Request.QueryString["custId"]);
            using (DataTable dtCust = c.GetDataTable("Select top 1 FlupDate, FlupRemark, FlupResponse" +
              " From FollowupOrders Where FK_CustomerId=" + custIdX + " Order By FlupID desc"))
            {
                if (dtCust.Rows.Count > 0)
                {
                    DataRow row = dtCust.Rows[0];
                    arrLastCall[0] = row["FlupDate"].ToString();
                    arrLastCall[1] = row["FlupRemark"].ToString();
                    arrLastCall[2] = row["FlupResponse"].ToString();
                    arrLastCall[3] = "The last call to this customer was on " + arrLastCall[0] + ", and customer's remark was as follows. <br><p class=\"text-primary\">" + arrLastCall[1] + "<br></p>";
                    switch (arrLastCall[2])
                    {
                        case "happy":
                            arrLastCall[4] = "<img src=\"../images/icons/happy.png\" style=\"width: 28px\" />";
                            break;
                        case "neutral":
                            arrLastCall[4] = "<img src=\"../images/icons/neutral.png\" style=\"width: 28px\" />";
                            break;
                        case "upset":
                            arrLastCall[4] = "<img src=\"../images/icons/upset.png\" style=\"width: 28px\" />";
                            break;
                        case "angry":
                            arrLastCall[4] = "<img src=\"../images/icons/angry.png\" style=\"width: 28px\" />";
                            break;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetCustInfo", ex.Message.ToString());
            return false;
        }
    }
    private void GetCustInfo()
    {
        try
        {
            int custIdX = Convert.ToInt32(Request.QueryString["custId"]);
            using (DataTable dtCust = c.GetDataTable("Select a.CustomerName, a.CustomerMobile, isnull(b.FranchName, 'Not Found') as favShop, " +
                " isnull(b.FranchShopCode, 'ShopCode - NA') as FranchShopCode, isnull(c.CityName, 'City - NA') as CityName, isnull(a.CallGoodTime, 'NA') as CallGoodTime" +
                " From CustomersData a Left Join FranchiseeData b On a.CustomerFavShop=b.FranchID Left Join CityData c On b.FK_FranchCityId=c.CityID Where " +
                " a.CustomerActive=1 AND a.delMark=0 AND a.CustomrtID=" + custIdX))
            {
                if (dtCust.Rows.Count > 0)
                {
                    DataRow row = dtCust.Rows[0];
                    arrCust[0] = row["CustomerName"].ToString();
                    arrCust[1] = row["CustomerMobile"].ToString();
                    arrCust[2] = row["favShop"].ToString() + " (" + row["FranchShopCode"].ToString() + "), " + row["cityName"].ToString();
                    callTime = row["CallGoodTime"].ToString();
                }
            }

            //OrderInfo

            arrCust[3] = c.returnAggregate("Select Count(OrderID) From OrdersData Where FK_OrderCustomerID=" + custIdX + " AND OrderStatus<>0").ToString();
            arrCust[4] = c.returnAggregate("Select Count(OrderID) From OrdersData Where FK_OrderCustomerID=" + custIdX + " AND OrderStatus=7").ToString();
            arrCust[5] = c.returnAggregate("Select Count(OrderID) From OrdersData Where FK_OrderCustomerID=" + custIdX + " AND OrderStatus IN (3, 5, 6)").ToString();
            arrCust[6] = c.returnAggregate("Select Count(OrderID) From OrdersData Where FK_OrderCustomerID=" + custIdX + " AND OrderStatus=2").ToString();
            //arrCust[7] = c.returnAggregate("Select Count(OrderID) From OrdersData Where FK_OrderCustomerID=" + custIdX + " AND OrderStatus=1").ToString(); //new
            //arrCust[8] = c.returnAggregate("Select Count(OrderID) From OrdersData Where FK_OrderCustomerID=" + custIdX + " AND OrderStatus=(4, 9)").ToString(); //denied by shops and denied by admin


            //obp details
            object obpId = c.GetReqData("CustomersData", "FK_ObpID", "CustomrtID=" + Request.QueryString["custId"]);
            if (obpId != DBNull.Value && obpId != null && obpId.ToString() != "")
            {
                using (DataTable dtObp = c.GetDataTable("Select OBP_ID, OBP_JoinDate, OBP_ApplicantName, OBP_EmailId, OBP_MobileNo From OBPData Where OBP_DelMark=0 AND OBP_ID=" + obpId))
                {
                    DataRow oRow = dtObp.Rows[0];
                    arrGobp[0] = oRow["OBP_ApplicantName"].ToString();
                    arrGobp[1] = oRow["OBP_MobileNo"].ToString();
                    arrGobp[2] = oRow["OBP_EmailId"].ToString();
                }
            }
            else
            {
                arrGobp[0] = "NA";
                arrGobp[1] = "NA";
                arrGobp[2] = "NA";
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetCustInfo", ex.Message.ToString());
            return;
        }
    }

    private void GetOrdersData()
    {
        try
        {
            int OrdIdX = Convert.ToInt32(Request.QueryString["ordId"]);
            arrOrd[0] = OrdIdX.ToString();
            arrOrd[1] = Convert.ToDateTime(c.GetReqData("OrdersData", "OrderDate", "OrderID=" + OrdIdX)).ToString("dd MMM yyyy");
            arrOrd[2] = c.GetReqData("OrdersData", "OrderAmount", "OrderID=" + OrdIdX).ToString();                        

            //arrOrd[10] = c.GetReqData("OrdersData", "UPIID", "OrderID=" + OrdIdX).ToString();
            //arrOrd[11] = c.GetReqData("OrdersData", "TransactionNo", "OrderID=" + OrdIdX).ToString();

            object frID = c.GetReqData("OrdersAssign", "Fk_FranchID", "FK_OrderID=" + OrdIdX + " AND OrdReAssign=0");
            if (frID != DBNull.Value && frID != null && frID.ToString() != "")
            {
                arrOrd[3] = c.GetReqData("FranchiseeData", "FranchName+', '+FranchShopCode", "FranchID=" + frID).ToString();
                int ordStatus = Convert.ToInt32(c.GetReqData("OrdersAssign", "OrdAssignStatus", "FK_OrderID=" + OrdIdX + " AND OrdReAssign=0"));
                string shopStatus = "";
                switch (ordStatus)
                {
                    case 0: shopStatus = "New"; break;
                    case 1:
                        shopStatus = "<div class=\"ordAccepted\">Accepted</div>";
                        break;
                    case 2:
                        shopStatus = "<div class=\"ordDenied\">Rejected</div>";
                        break;
                    case 5:
                        shopStatus = "<div class=\"ordProcessing\">Inprocess</div>";
                        break;
                    case 6:
                        shopStatus = "<div class=\"ordShipped\">Shipped</div>";
                        break;
                    case 7:
                        shopStatus = "<div class=\"ordDelivered\">Delivered</div>";
                        break;
                    case 10:
                        shopStatus = "<div class=\"ordDenied\">Returned By Customer</div>";
                        break;
                }
                arrOrd[4] = shopStatus;

                arrOrd[5] = c.returnAggregate("Select Count(FlupID) From FollowupOrders Where FK_OrderId=" + OrdIdX + " AND FK_CustomerId=" + Request.QueryString["custId"]).ToString();

                //long dispatchDateLong = c.returnAggregate("SELECT [DispatchDate] FROM [dbo].[OrdersData] WHERE [OrderID] = " + OrdIdX);
                //string dispatchDateStr;

                //DateTime dispatchDate = DateTime.MinValue;
                //if (dispatchDateLong != 0)
                //{
                //    dispatchDate = new DateTime(dispatchDateLong);
                //}

                //if (dispatchDate != DateTime.MinValue)
                //{
                //    dispatchDateStr = dispatchDate.ToString("dd MMM yyyy");
                //}
                //else
                //{
                //    dispatchDateStr = "NA";
                //}

                //arrOrd[7] = dispatchDateStr;
                arrOrd[7] = Convert.ToDateTime(c.GetReqData("[dbo].[OrdersData]", "[DispatchDate]", "[OrderID] = " + OrdIdX)).ToString("dd MMM yyyy");
                arrOrd[6] = Convert.ToDateTime(c.GetReqData("[dbo].[OrdersData]", "[EstimatedDeliveryDate]", "[OrderID] = " + OrdIdX)).ToString("dd MMM yyyy");

                object deviceType = c.GetReqData("OrdersData", "DeviceType", "OrderID=" + OrdIdX);
                if (deviceType != DBNull.Value && deviceType != null && deviceType.ToString() != "")
                {
                    if (deviceType.ToString().Contains("GOBP-Order"))
                    {
                        object obpId = c.GetReqData("CustomersData", "FK_ObpID", "CustomrtID=" + Request.QueryString["custId"]);
                        if (obpId != DBNull.Value && obpId != null && obpId.ToString() != "")
                        {
                            using (DataTable dtObp = c.GetDataTable("Select OBP_ID, OBP_JoinDate, OBP_ApplicantName, OBP_EmailId, OBP_MobileNo From OBPData Where OBP_DelMark=0 AND OBP_ID=" + obpId))
                            {
                                DataRow oRow = dtObp.Rows[0];
                                arrGobp[0] = oRow["OBP_ApplicantName"].ToString();
                                arrGobp[1] = oRow["OBP_MobileNo"].ToString();
                                arrGobp[2] = oRow["OBP_EmailId"].ToString();
                            }
                        }
                    }
                    else
                    {
                        arrGobp[0] = "NA";
                        arrGobp[1] = "NA";
                        arrGobp[2] = "NA";
                    }
                }
            }

            using (DataTable dtProduct = c.GetDataTable("Select a.OrdDetailID, a.FK_DetailProductID, a.OrdDetailQTY, 'Rs. ' + Convert(varchar(20), a.OrdDetailPrice)  as OrigPrice, 'Rs. ' + Convert(varchar(20), a.OrdDetailAmount) as OrdAmount , a.OrdDetailSKU, b.ProductName from OrdersDetails a Inner Join ProductsData b on a.FK_DetailProductID = b.ProductID where a.FK_DetailOrderID =" + OrdIdX))
            {
                gvOrderDetails.DataSource = dtProduct;
                gvOrderDetails.DataBind();

                if (gvOrderDetails.Rows.Count > 0)
                {
                    gvOrderDetails.UseAccessibleHeader = true;
                    gvOrderDetails.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetOrdersData", ex.Message.ToString());
            return;
        }
    }

    protected void gvOrderDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (c.IsRecordExist("Select ProdOptionID From ProductOptions Where FK_ProductID=" + e.Row.Cells[1].Text))
                {
                    int ProdOptId = Convert.ToInt32(c.GetReqData("OrderProductOptions", "FK_ProdOptionID", "FK_OrdDetailID=" + e.Row.Cells[0].Text));
                    int optGroupId = Convert.ToInt32(c.GetReqData("ProductOptions", "FK_OptionGroupID", "ProdOptionID=" + ProdOptId));
                    int optId = Convert.ToInt32(c.GetReqData("ProductOptions", "FK_OptionID", "ProdOptionID=" + ProdOptId));
                    string groupName = c.GetReqData("OptionGroups", "OptionGroupName", "OptionGroupID=" + optGroupId).ToString();
                    string optName = c.GetReqData("OptionsData", "OptionName", "OptionID=" + optId + " AND FK_OptionGroupID=" + optGroupId).ToString();
                    e.Row.Cells[2].Text += "<span class=\"space10\"></span> <span class=\"space1\"></span><span class=\"text-bold text-primary\">" + groupName + " : " + optName + "</span>";
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvOrderDetails_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    private void GetFollowupHistory()
    {
        try
        {
            int custIdX = Convert.ToInt32(Request.QueryString["custId"]);
            int OrdIdX = 0;
            if (Request.QueryString["ordId"] != null)
            {
                OrdIdX = Convert.ToInt32(Request.QueryString["ordId"]);
            }
            using (DataTable dtFlHistory = c.GetDataTable("Select * From FollowupOrders Where FK_CustomerId=" + custIdX + " AND FK_OrderId=" + OrdIdX + " Order By FlupID DESC"))
            {
                if (dtFlHistory.Rows.Count > 0)
                {
                    StringBuilder strMarkup = new StringBuilder();
                    foreach (DataRow row in dtFlHistory.Rows)
                    {
                        strMarkup.Append("<div class=\"user-block\">");
                        strMarkup.Append("<span class=\"username\">");
                        string flBy = c.GetReqData("SupportTeam", "TeamPersonName", "TeamID=" + row["FK_TeamMemberId"]).ToString();
                        strMarkup.Append("<a href=\"#\">" + flBy + "</a>");
                        strMarkup.Append("</span>");
                        strMarkup.Append("<span class=\"description\">Follow Up on - " + Convert.ToDateTime(row["FlupDate"]).ToString("dd MMM yyyy hh:mm tt") + "</span>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("<h6 class=\"text-indigo\">" + row["FlupRemarkStatus"].ToString() + "</h6>");
                        strMarkup.Append("<p class=\"text-bold\">" + row["FlupRemark"].ToString() + "</p>");
                        strMarkup.Append("<i class=\"nav-icon fas fa-clock\"></i><span>Next Follow Up: " + Convert.ToDateTime(row["FlupNextDate"]).ToString("dd MMM yyyy") + ", Time: " + row["FlupNextTime"].ToString() + "</span>");
                        strMarkup.Append("<hr />");
                    }

                    followupHistory = strMarkup.ToString();
                }
                else
                {
                    followupHistory = "<span class=\"text-orange text-bold\">No Followup History Found</span>";
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetFollowupHistory", ex.Message.ToString());
            return;
        }
    }

    protected void btnEditRepeat_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("edit-new-order.aspx?" + "custid=" + Request.QueryString["custId"].ToString() + "&ordId=" + Request.QueryString["ordId"].ToString());
        }
        catch(Exception ex)
        {
            return;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string customerResponse = "undefined";

            txtRemark.Text = txtRemark.Text.Trim().Replace("'", "");
            txtCalendar.Text = txtCalendar.Text.Trim().Replace("'", "");
            txtTime.Text = txtTime.Text.Trim().Replace("'", "");

            if (ddrRemark.SelectedIndex == 0 || txtRemark.Text == "" || txtCalendar.Text == "" || txtTime.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All Fields are Compulsory');", true);
                return;
            }

            // =========== Customer's Call Response Emoji values ============= (18-Aug-23 by Vinayak)
            // Check if is not "Not Connected"
            if(ddrRemark.SelectedIndex > 1)
            {
                foreach (Control control in RadioButtonGrp.Controls)
                {
                    if (control is RadioButton)
                    {
                        RadioButton radioButton = (RadioButton)control;
                        if (radioButton.Checked)
                        {
                            customerResponse = radioButton.Text.ToLower();
                        }
                    }
                }
            }



            DateTime flDate = DateTime.Now;
            string[] arrTDate = txtCalendar.Text.Split('/');
            if (c.IsDate(arrTDate[1] + "/" + arrTDate[0] + "/" + arrTDate[2]) == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid Date');", true);
                return;
            }
            else
            {
                flDate = Convert.ToDateTime(arrTDate[1] + "/" + arrTDate[0] + "/" + arrTDate[2]);
            }

            int maxId = c.NextId("FollowupOrders", "FlupID");

            int ordId = 0;
            if (Request.QueryString["ordId"] != null)
            {
                ordId = Convert.ToInt32(Request.QueryString["ordId"]);
            }

            c.ExecuteQuery("Insert Into FollowupOrders (FlupID, FlupDate, FK_CustomerId, FK_OrderId, FK_TeamMemberId, " +
                " FlupRemarkStatusID, FlupRemarkStatus, FlupRemark, FlupNextDate, FlupNextTime, FlupRptOrderId, " +
                " FlupStatus, FlupResponse) Values (" + maxId + ", '" + DateTime.Now + "', " + Request.QueryString["custId"] +
                ", " + ordId + ", " + Session["adminSupport"] + ", " + ddrRemark.SelectedValue + 
                ", '" + ddrRemark.SelectedItem.Text + "', '" + txtRemark.Text + "', '" + flDate + "', '" + txtTime.Text + "', 0, 'Open', '" + customerResponse + "')");

            // Update CustomersData => RBNO_NextFollowup with next Next Followup date for "Regsitered But Not Ordered" Follow up case.
            if (Request.QueryString["custId"] != null)
            {
                c.ExecuteQuery("Update CustomersData set RBNO_NextFollowup='" + flDate + "' Where CustomrtID=" + Request.QueryString["custId"]);
            }

            if (Request.QueryString["ordId"] != null)
            {
                c.ExecuteQuery("Update OrdersData Set FollowupLastDate='" + DateTime.Now + "', FollowupNextDate='" + flDate + "', FollowupNextTime='" + txtTime.Text + "', FollowupStatus='Active' Where OrderID=" + Request.QueryString["ordId"]);
            }

            c.ExecuteQuery("Update CustomersData Set CallBusyFlag=0, CallBusyBy=NULL Where CustomrtID=" + Request.QueryString["custId"]);

            ddrRemark.SelectedIndex = 0;
            txtTime.Text = txtRemark.Text = txtCalendar.Text = "";

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Followup Saved');", true);

            if (Request.QueryString["ordId"] != null)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('followup-order-detail.aspx?custId=" + Request.QueryString["custId"] + "&ordId=" + Request.QueryString["ordId"] + "', 2000);", true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('followup-order-detail.aspx?custId=" + Request.QueryString["custId"] + "', 2000);", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSave_Click", ex.Message.ToString());
            return;
        }
    }

    protected void btnLock_Click(object sender, EventArgs e)
    {
        try
        {
            object busyFlag = c.GetReqData("CustomersData", "CallBusyFlag", "CustomrtID=" + Request.QueryString["custId"]);
            if (busyFlag != DBNull.Value && busyFlag != null && busyFlag.ToString() != "")
            {
                if (Convert.ToInt32(busyFlag) == 0)
                {
                    c.ExecuteQuery("Update CustomersData Set CallBusyFlag=1, CallBusyBy=" + Session["adminSupport"] + " Where CustomrtID=" + Request.QueryString["custId"]);
                    btnLock.Text = "Unlock Call"; // Change the button text
                }
                else
                {
                    c.ExecuteQuery("Update CustomersData Set CallBusyFlag=0, CallBusyBy=NULL Where CustomrtID=" + Request.QueryString["custId"]);
                    btnLock.Text = "Lock Call"; // Change the button text
                }
            }
            else
            {
                c.ExecuteQuery("Update CustomersData Set CallBusyFlag=1, CallBusyBy=" + Session["adminSupport"] + " Where CustomrtID=" + Request.QueryString["custId"]);
                btnLock.Text = "Unlock Call"; // Change the button text
            }

            if (Request.QueryString["ordId"] != null)
            {
                Response.Redirect("followup-order-detail.aspx?custId=" + Request.QueryString["custId"] + "&ordId=" + Request.QueryString["ordId"], false);
            }
            else
            {
                Response.Redirect("followup-order-detail.aspx?custId=" + Request.QueryString["custId"], false);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occurred While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnLock_Click", ex.Message.ToString());
            return;
        }
    }

    protected void btnFlup_Click(object sender, EventArgs e)
    {
        try
        {            
            Response.Redirect("order-followup-po.aspx?custId=" + Request.QueryString["custId"] + "", false);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnFlup_Click", ex.Message.ToString());
            return;
        }
    }

    protected void btnRepeat_Click(object sender, EventArgs e)
    {
        try
        {
            int OrderId = Convert.ToInt32(Request.QueryString["ordId"]);
            int maxId = c.NextId("OrdersData", "OrderID");

            //Assign Order
            int shopToAssign = 0;
            if (chkConfirm.Checked == true)
            {
                //assign order to prev shop
                int prevShopId = Convert.ToInt32(c.GetReqData("OrdersAssign", "TOP 1 Fk_FranchID", "FK_OrderID=" + OrderId + " AND OrdReAssign=0 Order By OrdAssignDate desc"));
                shopToAssign = prevShopId;
            }
            else
            {
                if (ddrShops.SelectedIndex == 0 && txtShopCode.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter shop code or select shop to assign order');", true);
                    return;
                }

                if (ddrShops.SelectedIndex > 0)
                {
                    shopToAssign = Convert.ToInt32(ddrShops.SelectedValue);
                }
                else
                {
                    if (txtShopCode.Text != "")
                    {
                        if (!c.IsRecordExist("Select FranchID From FranchiseeData Where FranchShopCode='" + txtShopCode.Text + "' AND FranchActive=1"))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Shop with above shop code is not exists');", true);
                            return;
                        }
                        else
                        {
                            shopToAssign = Convert.ToInt32(c.GetReqData("FranchiseeData", "FranchID", "FranchShopCode='" + txtShopCode.Text + "' AND FranchActive=1"));
                        }
                    }
                }
            }

            int mreq = chkMonthly.Checked == true ? 1 : 0;

            //update prev order followup status as inactive
            c.ExecuteQuery("Update OrdersData Set FollowupStatus='Inactive' Where OrderID=" + OrderId);

            //Insert new repeat order info
            int addressId = Convert.ToInt32(c.GetReqData("OrdersData", "FK_AddressId", "OrderID=" + OrderId));
            string orderAmount = c.returnAggregate("Select SUM(OrdDetailAmount) From OrdersDetails Where FK_DetailOrderID=" + OrderId).ToString("0.00");
            int shippingcharge = 0;
            string finalOrderAmount = "";

            if (Convert.ToDouble(orderAmount) >= 500)
            {
                shippingcharge = 0;
                finalOrderAmount = (Convert.ToDouble(orderAmount.ToString()) + shippingcharge).ToString("0.00");
            }
            else
            {
                shippingcharge = 30;
                finalOrderAmount = (Convert.ToDouble(orderAmount.ToString()) + shippingcharge).ToString("0.00");
            }
            
            DateTime nextFlDate = DateTime.Now.AddDays(26);
            c.ExecuteQuery("Insert Into OrdersData (OrderID, FK_OrderCustomerID, OrderDate, OrderStatus, OrderType, DeviceType, OrderPayStatus, " +
                " MreqFlag, FollowupLastDate, FollowupNextDate, FollowupNextTime, FollowupStatus, FK_AddressId, OrderAmount, OrderShippingAmount) Values (" + maxId +
                ", " + Request.QueryString["custId"] + ", '" + DateTime.Now + "', 3, 1, 'FL-Repeat-Order', 0, " + mreq + ", '" + DateTime.Now +
                "', '" + nextFlDate + "', '11:00 AM', 'Active', " + addressId + ", " + finalOrderAmount + ", " + shippingcharge + ")");

            using (DataTable dtOrdDetails = c.GetDataTable("Select OrdDetailID, FK_DetailOrderID, FK_DetailProductID, OrdDetailQTY, OrdDetailPrice, " + 
                " OrdDetailAmount, OrdDetailSKU From OrdersDetails Where FK_DetailOrderID=" + OrderId))
            {
                if (dtOrdDetails.Rows.Count > 0)
                {
                    foreach (DataRow row in dtOrdDetails.Rows)
                    {
                        int maxDetailId = c.NextId("OrdersDetails", "OrdDetailID");

                        c.ExecuteQuery("Insert Into OrdersDetails (OrdDetailID, FK_DetailOrderID, FK_DetailProductID, OrdDetailQTY, OrdDetailPrice, " +
                            " OrdDetailAmount, OrdDetailSKU) Values (" + maxDetailId + ", " + maxId + ", " + row["FK_DetailProductID"] +
                            ", " + row["OrdDetailQTY"] + ", " + row["OrdDetailPrice"] + ", " + row["OrdDetailAmount"] + ", '" + row["OrdDetailSKU"] + "')");
                    }
                }
            }

            //insert into followup table
            int maxFlId = c.NextId("FollowupOrders", "FlupID");
            c.ExecuteQuery("Update FollowupOrders Set FlupStatus='Closed' Where FK_OrderId=" + Request.QueryString["ordId"] + 
                " AND FK_CustomerId=" + Request.QueryString["custId"]);

            c.ExecuteQuery("Insert Into FollowupOrders (FlupID, FlupDate, FK_CustomerId, FK_OrderId, FK_TeamMemberId, " +
                " FlupRemarkStatusID, FlupRemarkStatus, FlupRemark, FlupNextDate, FlupNextTime, FlupRptOrderId, " +
                " FlupStatus) Values (" + maxFlId + ", '" + DateTime.Now + "', " + Request.QueryString["custId"] +
                ", " + maxId + ", " + Session["adminSupport"] + ", 3, 'Connected: Repeated Order', " + 
                " 'Repeat Order Placed', '" + nextFlDate + "', '11:00 AM', " + maxId + ", 'Open')");

            c.ExecuteQuery("Update CustomersData Set CallBusyFlag=0, CallBusyBy=NULL Where CustomrtID=" + Request.QueryString["custId"]);

            // insert into OrdersAssign table
            int maxAssignId = c.NextId("OrdersAssign", "OrdAssignID");
            c.ExecuteQuery("Insert Into OrdersAssign (OrdAssignID, OrdAssignDate, FK_OrderID, Fk_FranchID, OrdAssignStatus, " +
                " OrdReAssign, AssignedFrom) Values (" + maxAssignId + ", '" + DateTime.Now + "', " + maxId + ", " + shopToAssign + 
                ", 0, 0, 'FL-Repeat')");

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Repeat Order Placed');", true);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('followup-order-report.aspx', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnRepeat_Click", ex.Message.ToString());
            return;
        }
    }

    private void GetOrderInfo()
     {
        try
        {
            int days = 4;
            DateTime TodaysDate = DateTime.Now;
            int ToDate = DateTime.Now.Day;
            using (DataTable dtOrder = c.GetDataTable(@"Select Top 1 OrderId, OrderAmount, OrderDate, FollowupLastDate 
                                From [dbo].[OrdersData] Where FK_OrderCustomerID = " + Request.QueryString["custId"].ToString() + " Order by FollowupLastDate DESC"))
            {
                if (dtOrder.Rows.Count > 0)
                {
                    DataRow bRow = dtOrder.Rows[0];

                    DateTime date =Convert.ToDateTime(bRow["FollowupLastDate"].ToString());
                    if (TodaysDate < date)
                    {
                        int difference = Convert.ToInt32((date - TodaysDate).TotalDays);
                    }
                    else
                    {
                        int difference = Convert.ToInt32((TodaysDate - date).TotalDays);
                    }

                    if (ToDate < days)
                    {
                        OrderInfo.Visible = true;
                        DateTime OrderDate = Convert.ToDateTime(bRow["OrderDate"].ToString());
                        arrOrdInfo[0] = bRow["OrderDate"].ToString() + "( "+ OrderDate.Day + " days ago)";
                        arrOrdInfo[1] = bRow["OrderId"].ToString();
                        arrOrdInfo[2] = bRow["OrderAmount"].ToString();
                    }
                    else
                    {
                        OrderInfo.Visible = false;
                    }                    
                }                
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetOrderInfo", ex.Message.ToString());
            return;
        }
    }

    [WebMethod]
    public static string UpdateGoodTime(string timeStr, string myCustomerId)
    {
        try
        {
            iClass c = new iClass();
            c.ExecuteQuery("Update CustomersData set CallGoodTime='" + timeStr + "' Where CustomrtID=" + Convert.ToInt32(myCustomerId));
            return timeStr;
        }

        catch(Exception ex)
        {
            return ex.ToString();
        }
    }
}