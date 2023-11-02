using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class admingenshopping_assign_enquiry : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] ordData = new string[20]; //7
    public string shippingCharges, modal, rdrUrl;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    rdrUrl = "enquiry-report.aspx";
                    GetEnqInfo(Convert.ToInt32(Request.QueryString["id"]));

                    // EnqAssignStatus 0 > Pending, 1 > Accepted, 2 > Rejected
                    if (c.IsRecordExist("Select EnqAssignID From SavingEnqAssign Where FK_CalcID=" + Request.QueryString["id"] + " AND EnqAssignStatus=1"))
                    {
                        shopList.Visible = false;
                    }

                    if (Request.QueryString["type"] != null && Request.QueryString["assId"] != null)
                    {
                        //update assign status = 0
                        c.ExecuteQuery("Update SavingEnqAssign Set EnqAssignStatus=0 Where FK_CalcID=" + Request.QueryString["id"] + " AND EnqAssignID=" + Request.QueryString["assId"]);
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Activity Deleted');", true);
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('assign-enquiry.aspx?id=" + Request.QueryString["id"] + "', 2000);", true);
                    }

                    // admin delete activity
                    if (Request.QueryString["activity"] != null && Request.QueryString["assignId"] != null)
                    {
                        //delete entry from assign table and make it as new order accepted by admin
                        c.ExecuteQuery("Delete From SavingEnqAssign Where EnqAssignID=" + Request.QueryString["assignId"]);
                        c.ExecuteQuery("Update SavingCalc Set EnqStatus=2 Where CalcID=" + Request.QueryString["id"]);
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Activity Deleted');", true);
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('assign-enquiry.aspx?id=" + Request.QueryString["id"] + "', 2000);", true);
                    }
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

    private void GetEnqInfo(int enqIdX)
    {
        try
        {
            using (DataTable dtOrder = c.GetDataTable("Select * From SavingCalc Where CalcID =" + enqIdX))
            {
                if (dtOrder.Rows.Count > 0)
                {
                    DataRow bRow = dtOrder.Rows[0];
                    ordData[0] = "#" + enqIdX.ToString();

                    ordData[1] = Convert.ToDateTime(bRow["CalcDate"]).ToString("dd/MM/yyyy hh:mm tt");
                    if (bRow["FK_AddressId"] != DBNull.Value && bRow["FK_AddressId"] != null && bRow["FK_AddressId"].ToString() != "")
                    {
                        using (DataTable dtCustAddr = c.GetDataTable("Select AddressID, AddressFKCustomerID, AddressName, AddressFull, AddressCity, AddressState, AddressPincode, AddressCountry From CustomersAddress Where AddressID=" + bRow["FK_AddressId"]))
                        {
                            if (dtCustAddr.Rows.Count > 0)
                            {
                                DataRow row = dtCustAddr.Rows[0];

                                ordData[2] = row["AddressCity"].ToString();
                                ordData[3] = row["AddressPincode"].ToString();
                            }
                        }
                    }
                    

                    // enq details grid view
                    using (DataTable dtProduct = c.GetDataTable("Select a.BrandMedicine, 'Rs. ' + Convert(varchar(20), a.BrandPrice)  as BrandPrice, 'Rs. ' + Convert(varchar(20), a.GenericPrice) as GenericPrice , a.GenericMedicine, a.SavingAmount, Convert(varchar(20), a.SavingPercent) + '%' as SavingPercent from SavingCalcItems a where a.FK_CalcID =" + enqIdX))
                    {

                        gvOrderDetails.DataSource = dtProduct;
                        gvOrderDetails.DataBind();
                    }

                    // enq assignment details
                    using (DataTable dtOrdAssign = c.GetDataTable("Select a.EnqAssignID, a.EnqAssignDate, a.FK_CalcID, a.Fk_FranchID, " +
                        " a.EnqAssignStatus, a.FK_ReasonID, a.EnqReAssign, c.FranchName, c.FranchShopCode From SavingEnqAssign a Inner Join dbo.SavingCalc b " +
                        " On a.FK_CalcID=b.CalcID Inner Join FranchiseeData c On a.Fk_FranchID=c.FranchID Where a.FK_CalcID=" + enqIdX))
                    {
                        if (dtOrdAssign.Rows.Count > 0)
                        {
                            StringBuilder strMarkup = new StringBuilder();

                            strMarkup.Append("<div class=\"card-header\">");
                            strMarkup.Append("<h3 class=\"medium colorLightBlue\">Enquiry Assigning Details</h3>");
                            strMarkup.Append("</div>");
                            strMarkup.Append("<div class=\"card-body\">");
                            strMarkup.Append("<table class=\"table table-bordered\">");
                            strMarkup.Append("<tr>");
                            strMarkup.Append("<th>Enquiry Id</th>");
                            strMarkup.Append("<th>Assigned Date</th>");
                            strMarkup.Append("<th>Days Passed</th>");
                            strMarkup.Append("<th>Assigned To</th>");
                            strMarkup.Append("<th>Shop Code</th>");
                            strMarkup.Append("<th>Status</th>");
                            strMarkup.Append("<th>Enq Cancel Reason</th>");
                            strMarkup.Append("<th>Re-assigned</th>");
                            strMarkup.Append("<th>Admin <br>Delete</th>");
                            strMarkup.Append("</tr>");

                            foreach (DataRow aRow in dtOrdAssign.Rows)
                            {
                                strMarkup.Append("<tr>");
                                strMarkup.Append("<td>#" + aRow["FK_CalcID"].ToString() + "</td>");
                                strMarkup.Append("<td>" + Convert.ToDateTime(aRow["EnqAssignDate"]).ToString("dd/MM/yyyy hh:mm tt") + "</td>");
                                strMarkup.Append("<td>" + c.GetTimeSpan(Convert.ToDateTime(aRow["EnqAssignDate"])) + "</td>");
                                strMarkup.Append("<td>" + aRow["FranchName"].ToString() + "</td>");
                                strMarkup.Append("<td>" + aRow["FranchShopCode"].ToString() + "</td>");
                                string ordAssignStatus = "", classname = "";
                                switch (Convert.ToInt32(aRow["EnqAssignStatus"]))
                                {
                                    case 0: ordAssignStatus = "Pending"; classname = "clrPending"; break;
                                    case 1: ordAssignStatus = "Accepted"; classname = "clrAccepted"; break;
                                    case 2: ordAssignStatus = "Rejected"; classname = "clrRejected"; break;
                                    case 5: ordAssignStatus = "In Process"; classname = "clrProcessing"; break;
                                    case 6: ordAssignStatus = "Shipped"; classname = "clrShipped"; break;
                                    case 7: ordAssignStatus = "Delivered"; classname = "clrDelivered"; break;
                                }
                                string actLink = "";
                                if (aRow["EnqAssignStatus"].ToString() == "2" && aRow["EnqReAssign"].ToString() == "0")
                                {
                                    actLink = "<br/><a href=\"assign-enquiry.aspx?id=" + Request.QueryString["id"] + "&type=delAct&assId=" + aRow["EnqAssignID"] + "\" class=\"clrShipped\">Delete Activity</a>";
                                }
                                strMarkup.Append("<td class=\"" + classname + "\">" + ordAssignStatus.ToString() + actLink + "</td>");
                                if (aRow["FK_ReasonID"] != DBNull.Value && aRow["FK_ReasonID"] != null || aRow["FK_ReasonID"].ToString() != "")
                                {
                                    if (aRow["FK_ReasonID"].ToString() != "0")
                                    {
                                        string reason = c.GetReqData("CancelReasons", "ReasonTitle", "ReasonID=" + aRow["FK_ReasonID"].ToString()).ToString();
                                        strMarkup.Append("<td><span style=\"color:red; font-weight:600;\">" + reason + "</span></td>");
                                    }
                                    else
                                    {
                                        strMarkup.Append("<td>-</td>");
                                    }
                                }
                                else
                                {
                                    strMarkup.Append("<td>-</td>");
                                }
                                string ordReassign = aRow["EnqReAssign"].ToString() == "0" ? "No" : "Yes";
                                strMarkup.Append("<td>" + ordReassign.ToString() + "</td>");
                                if (aRow["EnqAssignStatus"].ToString() == "6" || aRow["EnqAssignStatus"].ToString() == "7" || aRow["EnqReAssign"].ToString() == "1")
                                {
                                    strMarkup.Append("<td><a class=\"\"></a></td>");
                                }
                                else
                                {
                                    strMarkup.Append("<td><a href=\"assign-enquiry.aspx?id=" + Request.QueryString["id"] + "&activity=adminDel&assignId=" + aRow["EnqAssignID"] + "\" class=\"deleteProd\" OnClick=\"return confirm('Are you sure you want to delete this?');\"></a></td>");
                                }
                                strMarkup.Append("</tr>");
                            }

                            strMarkup.Append("</table>");
                            strMarkup.Append("</div>");

                            ordData[7] = strMarkup.ToString();
                        }
                    }

                    // Fill Shop List
                    FillShopGrid();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetEnqInfo", ex.Message.ToString());
            return;
        }
    }

    private void FillShopGrid()
    {
        try
        {
            object addrID = c.GetReqData("SavingCalc", "FK_AddressId", "CalcID=" + Request.QueryString["id"]);
            string zipcode = "", strQuery = "";
            if (addrID != DBNull.Value && addrID != null && addrID.ToString() != "")
            {
                int addressId = Convert.ToInt32(addrID);
                zipcode = c.GetReqData("CustomersAddress", "AddressPincode", "AddressID=" + addressId).ToString();
            }
            else
            {
                zipcode = "-";
            }

            if (rdbRelevant.Checked == true)
            {
                strQuery = "Select FranchID, FranchName, FranchShopCode, FranchMobile From FranchiseeData Where FranchPinCode='" + zipcode + "' AND FranchActive=1 AND SUBSTRING(FranchShopCode,1,4)<>'GMDR' AND SUBSTRING(FranchShopCode,1,4)<>'GMOS'";
            }
            else
            {
                strQuery = "Select FranchID, FranchName, FranchShopCode, FranchMobile From FranchiseeData Where FranchActive=1 AND SUBSTRING(FranchShopCode,1,4)<>'GMDR' AND SUBSTRING(FranchShopCode,1,4)<>'GMOS'";
            }

            //using (DataTable dtShops = c.GetDataTable("Select FranchID, FranchName, FranchShopCode, FranchMobile From FranchiseeData Where FranchPinCode='" + zipcode + "' AND FranchActive=1"))
            using (DataTable dtShops = c.GetDataTable(strQuery))
            {
                gvShopList.DataSource = dtShops;
                gvShopList.DataBind();
                if (gvShopList.Rows.Count > 0)
                {
                    gvShopList.UseAccessibleHeader = true;
                    gvShopList.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FillShopGrid", ex.Message.ToString());
            return;
        }
    }

    protected void gvShopList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Button btnAssign = (Button)e.Row.FindControl("cmdAssign");
                //if (c.IsRecordExist("Select EnqAssignID From SavingEnqAssign Where FK_CalcID=" + Request.QueryString["id"] + " AND Fk_FranchID=" + e.Row.Cells[0].Text))
                //{
                //    btnAssign.Enabled = false;
                //    btnAssign.Attributes["style"] = "background:none; display:none;";
                //}

                Literal litTotalOrd = (Literal)e.Row.FindControl("litTotalOrd");
                litTotalOrd.Text = c.returnAggregate("Select Count(EnqAssignID) From SavingEnqAssign Where Fk_FranchID=" + e.Row.Cells[0].Text + " AND EnqReAssign=0 AND EnqAssignStatus=1").ToString();

                Literal litLastOrdDate = (Literal)e.Row.FindControl("litLastOrdDate");
                object lastOrderDate = c.GetReqData("SavingEnqAssign", "Top 1 EnqAssignDate", "Fk_FranchID=" + e.Row.Cells[0].Text + " AND EnqReAssign=0 AND EnqAssignStatus=1 Order By EnqAssignID DESC");
                if (lastOrderDate != DBNull.Value && lastOrderDate != null && lastOrderDate.ToString() != "")
                {
                    litLastOrdDate.Text = Convert.ToDateTime(lastOrderDate).ToString("dd/MM/yyyy hh:mm tt");
                }
                else
                {
                    litLastOrdDate.Text = "Not Found";
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvShopList_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    protected void gvShopList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridViewRow gRow = (GridViewRow)((Button)e.CommandSource).NamingContainer;
            if (e.CommandName == "gvAssign")
            {
                // EnqStatus= 0 > New, 1 > Accepted by admin & assigned to franch, 2 > Converted , 3 > Not Converted
                // EnqAssignStatus 0 > Pending, 1 > Accepted, 2 > Rejected
                c.ExecuteQuery("Update SavingCalc Set EnqStatus=2 Where CalcID=" + Request.QueryString["id"]);
                c.ExecuteQuery("Update SavingEnqAssign Set EnqReAssign=1 Where FK_CalcID=" + Request.QueryString["id"]);
                int maxId = c.NextId("SavingEnqAssign", "EnqAssignID");
                c.ExecuteQuery("Insert Into SavingEnqAssign (EnqAssignID, EnqAssignDate, FK_CalcID, Fk_FranchID, EnqAssignStatus, " +
                    " EnqReAssign) Values (" + maxId + ", '" + DateTime.Now + "', " + Request.QueryString["id"] + ", " + gRow.Cells[0].Text + ", 0, 0)");

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Enquiry Assigned Successfully..!!');", true);
                GetEnqInfo(Convert.ToInt32(Request.QueryString["id"]));
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('assign-enquiry.aspx?id=" + Request.QueryString["id"] + "', 2000);", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvShopList_RowCommand", ex.Message.ToString());
            return;
        }
    }
    protected void rdbRelevant_CheckedChanged(object sender, EventArgs e)
    {
        FillShopGrid();
        GetEnqInfo(Convert.ToInt32(Request.QueryString["id"]));
    }
    protected void rdbAll_CheckedChanged(object sender, EventArgs e)
    {
        FillShopGrid();
        GetEnqInfo(Convert.ToInt32(Request.QueryString["id"]));
    }
}