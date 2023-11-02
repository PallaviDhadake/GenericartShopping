using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
public partial class supportteam_enquiry_followup : System.Web.UI.Page
{
    iClass c = new iClass();
    public string  errMsg, orderCount, shippingCharges, prescriptionStr, rdrUrl, mreq, deviceType, newOrdUrl;
    public int customerId;
    public string EnqfollowupHistory;
    public string[] ordData = new string[20]; //15
    public string[] ordCustData = new string[10];

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                c.FillComboBox("FL_Status", "FL_StatusID", "FollowupOrdersStatus", "DelMark=0 AND FL_StatusID NOT IN (3, 6, 7)", "FL_StatusID", 0, ddrRemark);

                rdrUrl = "saving-calc-enquiry.aspx";

                btnAssignOrder.Visible = false;

                int ordStatus = Convert.ToInt32(c.GetReqData("SavingCalc", "EnqStatus", "CalcID=" + Request.QueryString["id"]));
                if (ordStatus == 1 || ordStatus == 2 || ordStatus == 3 || ordStatus == 4 || ordStatus == 5 || ordStatus == 6 || ordStatus == 7 || ordStatus == 8 || ordStatus == 9) // accepted, denied, inprocess, shipped, delivered, rejected by 0001, Order amt low
                {
                    //btnAssignOrder.Visible = true;
                    btnAssignOrder.Visible = false;
                }

                if (ordStatus == 4)
                {
                    btnAssignOrder.Visible = false;
                }

                object mreFlag = c.GetReqData("SavingCalc", "MreqFlag", "CalcID=" + Request.QueryString["id"]);
                if (mreFlag != DBNull.Value && mreFlag != null && mreFlag.ToString() != "")
                {
                    if (mreFlag.ToString() == "1")
                    {
                        mreq = "<span class=\"medium clrProcessing bold_weight\">Customer Marked This Enquiry as Monthly Order</span><span class=\"space10\"></span>";
                    }
                }

                GetEnqFollowupHistory();
            }
            if (Request.QueryString["id"] != null)
            {
                GetOrdersData(Convert.ToInt32(Request.QueryString["id"]));
                GetEnqFollowupHistory();
            }
        }
        catch (Exception ex)
        {

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "Page_Load", ex.Message.ToString());
            return;

        }
    }

    public void GetOrdersData(int Idx)
    {
        try
        {
            using (DataTable dtOrder = c.GetDataTable("Select * From SavingCalc Where CalcID=" + Idx))
            {
                if (dtOrder.Rows.Count > 0)
                {
                    DataRow bRow = dtOrder.Rows[0];
                    ordData[0] = Idx.ToString();


                    ordData[1] = Convert.ToDateTime(bRow["CalcDate"]).ToString("dd/MM/yyyy");
                    ordData[11] = Convert.ToDateTime(bRow["CalcDate"]).ToString("hh:mm tt");
                    deviceType = bRow["DeviceType"] != DBNull.Value && bRow["DeviceType"] != null && bRow["DeviceType"].ToString() != "" ? bRow["DeviceType"].ToString() : "";

                    if (bRow["FK_AddressId"] != DBNull.Value && bRow["FK_AddressId"] != null && bRow["FK_AddressId"].ToString() != "")
                    {
                        using (DataTable dtCustAddr = c.GetDataTable("Select AddressID, AddressFKCustomerID, AddressName, AddressFull, AddressCity, AddressState, AddressPincode, AddressCountry From CustomersAddress Where AddressID=" + bRow["FK_AddressId"]))
                        {
                            if (dtCustAddr.Rows.Count > 0)
                            {
                                DataRow row = dtCustAddr.Rows[0];

                                ordData[4] = row["AddressFull"].ToString() + " <span class=\"bold_weight\" style=\"display:inline-block !important;\"> (" + row["AddressName"].ToString() + ")</span>";

                                ordData[6] = row["AddressCity"].ToString();
                                ordData[7] = row["AddressState"].ToString();
                                ordData[8] = row["AddressPincode"].ToString();
                                ordData[9] = row["AddressCountry"].ToString();
                            }
                        }
                    }

                    customerId = Convert.ToInt32(bRow["FK_CustId"]);

                    // Generate NEW ORDER Anchor here
                    newOrdUrl = "<a href=\"submit-po.aspx?custId=" + customerId + "&type=newOrd&enqref=" + Request.QueryString["id"] + "\" target=\"_blank\" class=\"btn btn-md btn-warning\">New Order</a>";

                    if (bRow["EnqStatus"].ToString() == "10")
                    {
                        ordReturned.Visible = true;
                        ordData[18] = c.GetReqData("SavingEnqAssign", "ReturnReason", "FK_CalcID=" + Request.QueryString["id"] + " AND ReturnReason IS NOT NULL AND ReturnReason<>''").ToString();
                    }


                    //orderStatus.SelectedValue = bRow["OrderStatus"].ToString();

                    using (DataTable dtCust = c.GetDataTable("Select CustomrtID, CustomerName, CustomerMobile, CustomerEmail, CustomerAddress, CustomerCity, CustomerState, CustomerPincode From CustomersData Where CustomrtID = " + customerId))
                    {
                        if (dtCust.Rows.Count > 0)
                        {
                            DataRow row = dtCust.Rows[0];

                            ordCustData[0] = row["CustomrtID"].ToString();
                            ordCustData[1] = row["CustomerName"].ToString();
                            ordCustData[2] = row["CustomerMobile"].ToString();
                            ordCustData[3] = row["CustomerEmail"].ToString();


                            if (bRow["FK_AddressId"] != DBNull.Value && bRow["FK_AddressId"] != null && bRow["FK_AddressId"].ToString() != "")
                            {
                                using (DataTable dtCustAddr = c.GetDataTable("Select AddressID, AddressName, AddressFull, AddressCity, AddressState, AddressPincode, AddressCountry From CustomersAddress Where AddressFKCustomerID=" + customerId + " AND AddressID=" + bRow["FK_AddressId"]))
                                {
                                    if (dtCustAddr.Rows.Count > 0)
                                    {
                                        DataRow cRow = dtCustAddr.Rows[0];

                                        ordCustData[3] = cRow["AddressCity"].ToString();
                                        ordCustData[4] = cRow["AddressState"].ToString();
                                        ordCustData[5] = cRow["AddressPincode"].ToString();
                                        ordCustData[6] = cRow["AddressFull"].ToString() + "<span class=\"bold_weight\" style=\"display:inline-block !important;\"> (" + cRow["AddressName"].ToString() + ")</span>";
                                    }
                                }
                            }
                        }
                    }
                }
            }

            using (DataTable dtProduct = c.GetDataTable("Select a.BrandMedicine, 'Rs. ' + Convert(varchar(20), a.BrandPrice)  as BrandPrice, 'Rs. ' + Convert(varchar(20), a.GenericPrice) as GenericPrice, a.CalcItemQty, a.GenericMedicine, a.GenericCode, a.SavingAmount, Convert(varchar(20), a.SavingPercent) + '%' as SavingPercent from SavingCalcItems a where a.FK_CalcID =" + Idx))
            {

                gvOrderDetails.DataSource = dtProduct;
                gvOrderDetails.DataBind();
            }

            orderCount = c.returnAggregate("Select COUNT(a.CalcID) From SavingCalc a Join CustomersData b On b.CustomrtID = a.FK_CustId Where b.CustomrtID=" + customerId + " AND b.delMark = 0").ToString();
        }
        catch (Exception ex)
        {

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetOrdersData", ex.Message.ToString());
            return;

        }
    }
    protected void btnAssignOrder_Click(object sender, EventArgs e)
    {
       // Response.Redirect("assign-enquiry.aspx?id=" + Request.QueryString["id"], false);
    }

    protected void btnLock_Click(object sender, EventArgs e)
    {
        try
        {
            object busyFlag = c.GetReqData("SavingCalc", "CallBusyFlag", "CalcID=" + Request.QueryString["id"]);
            if (busyFlag != DBNull.Value && busyFlag != null && busyFlag.ToString() != "")
            {
                if (Convert.ToInt32(busyFlag) == 0)
                {
                    c.ExecuteQuery("Update SavingCalc Set CallBusyFlag=1, CallBusyBy=" + Session["adminSupport"] + " Where CalcID=" + Request.QueryString["id"]);
                    btnLock.Text = "Unlock Call"; // Change the button text
                }
                else
                {
                    c.ExecuteQuery("Update SavingCalc Set CallBusyFlag=0, CallBusyBy=NULL Where CalcID=" + Request.QueryString["id"]);
                    btnLock.Text = "Lock Call"; // Change the button text
                }
            }
            else
            {
                c.ExecuteQuery("Update SavingCalc Set CallBusyFlag=1, CallBusyBy=" + Session["adminSupport"] + " Where CalcID=" + Request.QueryString["id"]);
                btnLock.Text = "Unlock Call"; // Change the button text
            }
            
            Response.Redirect("enquiry-followup.aspx?id=" + Request.QueryString["id"], false);            
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occurred While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnLock_Click", ex.Message.ToString());
            return;
        }
    }

    private void GetEnqFollowupHistory()
    {
        try
        {
            customerId = Convert.ToInt32(c.GetReqData("SavingCalc", "FK_CustId", "CalcID='" + Request.QueryString["id"].ToString() + "'"));

            //using (DataTable dtEnqFlHistory = c.GetDataTable("Select * From FollowupEnquires Where FK_CustomerId = '" + customerId + "' Order By FlupEnqID DESC"))
            using (DataTable dtEnqFlHistory = c.GetDataTable("Select * From FollowupEnquires Where FK_EnqId=" + Convert.ToInt32(Request.QueryString["id"].ToString()) + " Order By FlupEnqID DESC"))

                
            {
                if (dtEnqFlHistory.Rows.Count > 0)
                {
                    StringBuilder strMarkup = new StringBuilder();
                    foreach (DataRow row in dtEnqFlHistory.Rows)
                    {
                        strMarkup.Append("<div class=\"user-block\">");
                        strMarkup.Append("<span class=\"username\">");
                        string enqflby = c.GetReqData("SupportTeam", "TeamPersonName", "TeamID=" + row["FK_teamMemberId"]).ToString();
                        strMarkup.Append("<a href=\"#\">" + enqflby + "</a>");
                        strMarkup.Append("</span>");
                        strMarkup.Append("<span class=\"description\">Follow Up on - " + Convert.ToDateTime(row["FlupEnqDate"]).ToString("dd MMM yyyy hh:mm tt") + "</span>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("<h6 class=\"text-indigo\">" + row["FlupEnqRemarkStatus"].ToString() + "</h6>");
                        strMarkup.Append("<p class=\"text-bold\">" + row["FlupEnqRemark"].ToString() + "</p>");
                        strMarkup.Append("<i class=\"nav-icon fas fa-clock\"></i><span>Next Follow Up: " + Convert.ToDateTime(row["FlupEnqNextDate"]).ToString("dd MMM yyyy") + ", Time: " + row["FlupEnqNextTime"].ToString() + "</span>");
                        strMarkup.Append("<hr />");
                    }

                    EnqfollowupHistory = strMarkup.ToString();
                }
                else
                {
                    EnqfollowupHistory = "<span class=\"text-orange text-bold\">No Enquiry Followup History Found</span>";
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetEnqFollowupHistory", ex.Message.ToString());
            return;
        }
    }

    // Code by Vinayak 16-Aug-23
    protected void btnInactive_Click(object sender, EventArgs e)
    {
        try
        {
            c.ExecuteQuery("Update SavingCalc set FollowupStatus='Inactive' where CalcID=" + Convert.ToInt32(Request.QueryString["id"]));
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Enquiry has marked as Inactive for follow up');", true);
        }
        catch(Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnInactive_Click", ex.Message.ToString());
            return;
        }
    }

        protected void btnSave_Click(object sender, EventArgs e)
        {
        try
        {
            customerId = Convert.ToInt32(c.GetReqData("SavingCalc", "FK_CustId", "CalcID='" + Request.QueryString["id"].ToString() + "'"));

            txtRemark.Text = txtRemark.Text.Trim().Replace("'", "");
            txtCalendar.Text = txtCalendar.Text.Trim().Replace("'", "");
            txtTime.Text = txtTime.Text.Trim().Replace("'", "");

            if (ddrRemark.SelectedIndex == 0 || txtRemark.Text == "" || txtCalendar.Text == "" || txtTime.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All Fields are compulsary');", true);
                return;
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

            int enqId = c.NextId("FollowupEnquires", "FlupEnqID");

            c.ExecuteQuery("Insert Into FollowupEnquires (FlupEnqID, FlupEnqDate, FK_EnqId, FK_CustomerId, FK_TeamMemberId, " +
                    " FlupEnqRemarkStatusID, FlupEnqRemarkStatus, FlupEnqRemark, FlupEnqNextDate, FlupEnqNextTime, FlupEnqRptOrderId, " +
                    " FlupEnqStatus) Values (" + enqId + ", '" + DateTime.Now + "', " + Convert.ToInt32(Request.QueryString["id"]) +
                    ", " + customerId + ", " + Session["adminSupport"] + ", " + ddrRemark.SelectedValue +
                    ", '" + ddrRemark.SelectedItem.Text + "', '" + txtRemark.Text + "', '" + flDate + "', '" + txtTime.Text + "', 0, 'Open')");

            //c.ExecuteQuery("Update SavingCalc set FollowupLastDate='" + DateTime.Now + "', FollowupNextDate='" + flDate + "', FollowupNextTime='" + txtTime.Text + "', FollowupStatus='Active' Where CalcID='" + Request.QueryString["id"].ToString() + "'");
            c.ExecuteQuery("Update SavingCalc set FollowupLastDate='" + DateTime.Now + "', FollowupNextDate='" + flDate + "', FollowupNextTime='" + txtTime.Text + "', FollowupStatus='Active' Where CalcID=" + Convert.ToInt32(Request.QueryString["id"].ToString()));

            c.ExecuteQuery("Update CustomersData Set CallBusyFlag=0, CallBusyBy=NULL Where CustomrtID=" + customerId);

            ddrRemark.SelectedIndex = 0;
            txtTime.Text = txtRemark.Text = txtCalendar.Text = "";

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Enquiry Followup Saved');", true);
            GetEnqFollowupHistory();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSave_Click", ex.Message.ToString());
            return;
        }
    }
}