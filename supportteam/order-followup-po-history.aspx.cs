using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class supportteam_order_followup_po_history : System.Web.UI.Page
{
    iClass c = new iClass();
    public string pgTitle, err, orderslink, followupHistory;
    public string[] arrCust = new string[30];
    public string[] lastCall = new string[30];
    public string[] OrdDeatils = new string[30];
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            c.FillComboBox("FL_Status", "FL_StatusID", "FollowupOrdersStatus", "DelMark=0 AND FL_StatusID NOT IN (3, 6, 7)", "FL_StatusID", 0, ddrRemark);
        }
        ShowCustomersData();
      
        GetFollowupHistory();
        orderslink = "order-followup-po.aspx?&custId=" + Request.QueryString["custId"];
    }


    private void ShowCustomersData()
    {

        CustomersData custinfo = new CustomersData();

        int CustIdx = Convert.ToInt32(Request.QueryString["custId"]);

        custinfo.CustomresInfo(CustIdx);

        arrCust[0] = custinfo.CustomerName;
        arrCust[1] = custinfo.CustomerMobile;

        custinfo.FranchaiseeData(custinfo.CustomerFavShop);

      
        string FavShopCity = (custinfo.FK_FranchCityId != 0)
   ? c.GetReqData("CityData", "CityName", "CityID=" + custinfo.FK_FranchCityId).ToString()
   : "NA";
        string FranchName = !string.IsNullOrEmpty(custinfo.FranchName) ? custinfo.FranchName : "Not Found";

        string FranchShopCode = !string.IsNullOrEmpty(custinfo.FranchShopCode) ? custinfo.FranchShopCode : "Not Found";

        arrCust[2] = FranchName + " ( ShopCode - " + FranchShopCode + " ) , City - " + FavShopCity + "";

        custinfo.CustOrdersSatus(CustIdx);

        string TotalOrders = !string.IsNullOrEmpty(custinfo.Total_Orders) ? custinfo.Total_Orders : "NA";
        string Delivered = !string.IsNullOrEmpty(custinfo.Delivered) ? custinfo.Delivered : "NA";
        string InProcess = !string.IsNullOrEmpty(custinfo.InProcess) ? custinfo.InProcess : "NA";
        string Cancelled = !string.IsNullOrEmpty(custinfo.Cancelled) ? custinfo.Cancelled : "NA";

        arrCust[3] = TotalOrders;
        arrCust[4] = Delivered;
        arrCust[5] = InProcess;
        arrCust[6] = Cancelled;

        //OBP Deatils
        custinfo.OBPDataInfo(custinfo.FK_ObpID);

        if (custinfo.FK_ObpID == 0)
        {
            nogobp.Visible = true;
            arrCust[10] = "No GOBP Found";

        }
        else
        {
            mobno.Visible = true;
            string ObpUserId = custinfo.FK_ObpID == 0 ? "NA" : custinfo.OBP_UserID;
            arrCust[7] = custinfo.OBP_ApplicantName;
            arrCust[8] = custinfo.OBP_MobileNo;
            arrCust[9] = "( " + ObpUserId + " )";
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
            using (DataTable dtFlHistory = c.GetDataTable("Select * From FollowupOrders Where FK_CustomerId=" + custIdX + " Order By FlupID DESC"))
            {
                if (dtFlHistory.Rows.Count > 0)
                {
                    int ncount = 0;
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
                        strMarkup.Append("<i class=\"nav-icon fas fa-clock mr-2\"></i><span>Next Follow Up: " + Convert.ToDateTime(row["FlupNextDate"]).ToString("dd MMM yyyy") + ", Time: " + row["FlupNextTime"].ToString() + "</span>");
                        //strMarkup.Append("<hr />");

                        if (ncount < dtFlHistory.Rows.Count)
                        {
                            strMarkup.Append("<hr />");
                        }
                        ncount++;
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


    protected void btnSave_Click(object sender, EventArgs e)
    {
          try
            {
                txtRemark.Text = txtRemark.Text.Trim().Replace("'", "");
                txtCalendar.Text = txtCalendar.Text.Trim().Replace("'", "");
                txtTime.Text = txtTime.Text.Trim().Replace("'", "");

                if (ddrRemark.SelectedIndex == 0 || txtRemark.Text == "" || txtCalendar.Text == "" || txtTime.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All Fields are Compulsory');", true);
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

                int maxId = c.NextId("FollowupOrders", "FlupID");

                //int ordId = 0;
                //if (Request.QueryString["ordId"] != null)
                //{
                //    ordId = Convert.ToInt32(Request.QueryString["ordId"]);
                //}

                c.ExecuteQuery("Insert Into FollowupOrders (FlupID, FlupDate, FK_CustomerId, FK_TeamMemberId, " +
                    " FlupRemarkStatusID, FlupRemarkStatus, FlupRemark, FlupNextDate, FlupNextTime, FlupRptOrderId, " +
                    " FlupStatus) Values (" + maxId + ", '" + DateTime.Now + "', " + Request.QueryString["custId"] +
                    ",  " + Session["adminSupport"] + ", " + ddrRemark.SelectedValue +
                    ", '" + ddrRemark.SelectedItem.Text + "', '" + txtRemark.Text + "', '" + flDate + "', '" + txtTime.Text + "', 0, 'Open')");

                // Update CustomersData => RBNO_NextFollowup with next Next Followup date for "Regsitered But Not Ordered" Follow up case.
                if (Request.QueryString["custId"] != null)
                {
                    c.ExecuteQuery("Update CustomersData set RBNO_NextFollowup='" + flDate + "' Where CustomrtID=" + Request.QueryString["custId"]);
                }

                //if (Request.QueryString["ordId"] != null)
                //{
                //    c.ExecuteQuery("Update OrdersData Set FollowupLastDate='" + DateTime.Now + "', FollowupNextDate='" + flDate + "', FollowupNextTime='" + txtTime.Text + "', FollowupStatus='Active' Where OrderID=" + Request.QueryString["ordId"]);
                //}

                c.ExecuteQuery("Update CustomersData Set CallBusyFlag=0, CallBusyBy=NULL Where CustomrtID=" + Request.QueryString["custId"]);

                ddrRemark.SelectedIndex = 0;
                txtTime.Text = txtRemark.Text = txtCalendar.Text = "";

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Followup Saved');", true);

                //if (Request.QueryString["ordId"] != null)
                //{
                //    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('followup-order-detail.aspx?custId=" + Request.QueryString["custId"] + "&ordId=" + Request.QueryString["ordId"] + "', 2000);", true);
                //}
                //else
                //{
                //    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('followup-order-detail.aspx?custId=" + Request.QueryString["custId"] + "', 2000);", true);
                //}
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
                c.ErrorLogHandler(this.ToString(), "btnSave_Click", ex.Message.ToString());
                return;
            }
        
       
    }
}