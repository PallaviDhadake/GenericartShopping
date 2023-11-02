using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class doctors_my_appointments : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg, deviceType;
    public string[] ordData = new string[20]; //11

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    viewApp.Visible = false;
                    readApp.Visible = true;
                    GetAppData(Convert.ToInt32(Request.QueryString["id"]));
                }
                else
                {
                    viewApp.Visible = true;
                    readApp.Visible = false;
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
            string strQuery = "";
            //if (Session["adminDoctor"].ToString() == "5" || Session["adminDoctor"].ToString() == "6")
            //{
            //    if (Request.QueryString["type"] == "new")
            //    {
            //        strQuery = "Select DocAppID, Convert(varchar(20), DocAppDate, 103) as appDate, Convert(varchar(20), AppSubmitDate, 103)+ ' ' + SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 13, 2)+':'+SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 16, 2)+':'+SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 18, 2) AS subDate , DocAppName, DocAppMobile, DocAppAge, DocAppStatus, isnull(DeviceType, '-') as DeviceType From DoctorsAppointmentData Where (FK_DocID=" + Session["adminDoctor"] + " OR FK_DocID=0 ) AND DocAppStatus=0";
            //    }
            //    else
            //    {
            //        strQuery = "Select DocAppID, Convert(varchar(20), DocAppDate, 103) as appDate, Convert(varchar(20), AppSubmitDate, 103)+ ' ' + SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 13, 2)+':'+SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 16, 2)+':'+SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 18, 2) AS subDate , DocAppName, DocAppMobile, DocAppAge, DocAppStatus, isnull(DeviceType, '-') as DeviceType From DoctorsAppointmentData Where ( FK_DocID=" + Session["adminDoctor"] + " OR FK_DocID=0 )";
            //    }
            //}
            //else
            //{
            //    if (Request.QueryString["type"] == "new")
            //    {
            //        strQuery = "Select DocAppID, Convert(varchar(20), DocAppDate, 103) as appDate, Convert(varchar(20), AppSubmitDate, 103)+ ' ' + SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 13, 2)+':'+SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 16, 2)+':'+SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 18, 2) AS subDate , DocAppName, DocAppMobile, DocAppAge, DocAppStatus, isnull(DeviceType, '-') as DeviceType From DoctorsAppointmentData Where FK_DocID=" + Session["adminDoctor"] + " AND DocAppStatus=0";
            //    }
            //    else
            //    {
            //        strQuery = "Select DocAppID, Convert(varchar(20), DocAppDate, 103) as appDate, Convert(varchar(20), AppSubmitDate, 103)+ ' ' + SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 13, 2)+':'+SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 16, 2)+':'+SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 18, 2) AS subDate , DocAppName, DocAppMobile, DocAppAge, DocAppStatus, isnull(DeviceType, '-') as DeviceType From DoctorsAppointmentData Where FK_DocID=" + Session["adminDoctor"];
            //    }
            //}
            if (Session["adminDoctor"].ToString() == "5")
            {
                // dr. shruti
                if (Request.QueryString["type"] == "paid")
                {
                    strQuery = "Select Distinct a.DocAppID, Convert(varchar(20), a.DocAppDate, 103) as appDate, Convert(varchar(20), a.AppSubmitDate, 103)+ ' ' + SUBSTRING(CONVERT(VARCHAR, a.AppSubmitDate, 100), 13, 2)+':'+SUBSTRING(CONVERT(VARCHAR, a.AppSubmitDate, 100), 16, 2)+':'+SUBSTRING(CONVERT(VARCHAR, a.AppSubmitDate, 100), 18, 2) AS subDate , a.DocAppName, a.DocAppMobile, a.DocAppAge, a.DocAppStatus, isnull(a.DeviceType, '-') as DeviceType From DoctorsAppointmentData a Inner Join online_payment_logs b On a.Doc_txn_id = b.OPL_merchantTranId Where b.OPL_transtatus='paid' AND a.Doc_pay_amount>0 AND ( a.FK_DocID=" + Session["adminDoctor"] + " OR a.FK_DocID=0 )";
                    //for local test//strQuery = "Select DocAppID, Convert(varchar(20), DocAppDate, 103) as appDate, Convert(varchar(20), AppSubmitDate, 103)+ ' ' + SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 13, 2)+':'+SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 16, 2)+':'+SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 18, 2) AS subDate , DocAppName, DocAppMobile, DocAppAge, DocAppStatus, isnull(DeviceType, '-') as DeviceType From DoctorsAppointmentData Where ( FK_DocID=" + Session["adminDoctor"] + " OR FK_DocID=0 )";
                }
                else
                {
                    strQuery = "Select Distinct a.DocAppID, Convert(varchar(20), a.DocAppDate, 103) as appDate, Convert(varchar(20), a.AppSubmitDate, 103)+ ' ' + SUBSTRING(CONVERT(VARCHAR, a.AppSubmitDate, 100), 13, 2)+':'+SUBSTRING(CONVERT(VARCHAR, a.AppSubmitDate, 100), 16, 2)+':'+SUBSTRING(CONVERT(VARCHAR, a.AppSubmitDate, 100), 18, 2) AS subDate , a.DocAppName, a.DocAppMobile, a.DocAppAge, a.DocAppStatus, isnull(a.DeviceType, '-') as DeviceType From DoctorsAppointmentData a Inner Join online_payment_logs b On a.Doc_txn_id = b.OPL_merchantTranId Where b.OPL_transtatus='paid' AND a.Doc_pay_amount>0 AND ( a.FK_DocID=" + Session["adminDoctor"] + " OR a.FK_DocID=0 )";
                    //for local test//strQuery = "Select DocAppID, Convert(varchar(20), DocAppDate, 103) as appDate, Convert(varchar(20), AppSubmitDate, 103)+ ' ' + SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 13, 2)+':'+SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 16, 2)+':'+SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 18, 2) AS subDate , DocAppName, DocAppMobile, DocAppAge, DocAppStatus, isnull(DeviceType, '-') as DeviceType From DoctorsAppointmentData Where ( FK_DocID=" + Session["adminDoctor"] + " OR FK_DocID=0 )";
                }
            }
            else if (Session["adminDoctor"].ToString() == "6")
            {
                // dr. vinita
                if (Request.QueryString["type"] == "new")
                {
                    strQuery = "Select Distinct DocAppID, Convert(varchar(20), DocAppDate, 103) as appDate, Convert(varchar(20), AppSubmitDate, 103)+ ' ' + SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 13, 2)+':'+SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 16, 2)+':'+SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 18, 2) AS subDate , DocAppName, DocAppMobile, DocAppAge, DocAppStatus, isnull(DeviceType, '-') as DeviceType From DoctorsAppointmentData Where Doc_txn_id IS NULL AND (FK_DocID=" + Session["adminDoctor"] + " OR FK_DocID=0 ) AND DocAppStatus=0";
                }
                else
                {
                    strQuery = "Select Distinct DocAppID, Convert(varchar(20), DocAppDate, 103) as appDate, Convert(varchar(20), AppSubmitDate, 103)+ ' ' + SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 13, 2)+':'+SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 16, 2)+':'+SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 18, 2) AS subDate , DocAppName, DocAppMobile, DocAppAge, DocAppStatus, isnull(DeviceType, '-') as DeviceType From DoctorsAppointmentData Where Doc_txn_id IS NULL AND ( FK_DocID=" + Session["adminDoctor"] + " OR FK_DocID=0 )";
                }
            }
            else
            {
                if (Request.QueryString["type"] == "new")
                {
                    strQuery = "Select Distinct DocAppID, Convert(varchar(20), DocAppDate, 103) as appDate, Convert(varchar(20), AppSubmitDate, 103)+ ' ' + SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 13, 2)+':'+SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 16, 2)+':'+SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 18, 2) AS subDate , DocAppName, DocAppMobile, DocAppAge, DocAppStatus, isnull(DeviceType, '-') as DeviceType From DoctorsAppointmentData Where Doc_txn_id IS NULL AND  FK_DocID=" + Session["adminDoctor"] + " AND DocAppStatus=0";
                }
                else
                {
                    strQuery = "Select Distinct DocAppID, Convert(varchar(20), DocAppDate, 103) as appDate, Convert(varchar(20), AppSubmitDate, 103)+ ' ' + SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 13, 2)+':'+SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 16, 2)+':'+SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 18, 2) AS subDate , DocAppName, DocAppMobile, DocAppAge, DocAppStatus, isnull(DeviceType, '-') as DeviceType From DoctorsAppointmentData Where Doc_txn_id IS NULL AND  FK_DocID=" + Session["adminDoctor"];
                }
            }
            using (DataTable dtApp = c.GetDataTable(strQuery))
            {
                gvAppointment.DataSource = dtApp;
                gvAppointment.DataBind();

                if (gvAppointment.Rows.Count > 0)
                {
                    gvAppointment.UseAccessibleHeader = true;
                    gvAppointment.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvAppointment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"my-appointments.aspx?id=" + e.Row.Cells[0].Text + "\" class=\"gView\"></a>";

                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                string status = "";
                if (Session["adminDoctor"].ToString() == "5")
                {
                    status = "<br/><span class=\"text-bold text-indigo\">(Paid)</span>";
                }
                switch (e.Row.Cells[1].Text)
                {
                    case "0": litStatus.Text = "<span class=\"ordNew\">New</span>" + status + ""; break;
                    case "1": litStatus.Text = "<span class=\"ordAccepted\">Accepted</span>" + status; break;
                    case "2": litStatus.Text = "<span class=\"ordDenied\">Denied</span>" + status; break;
                    case "3": litStatus.Text = "<span class=\"ordShipped\">Completed</span>" + status; break;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvAppointment_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            c.ExecuteQuery("Update DoctorsAppointmentData Set DocAppStatus=1 Where DocAppID=" + Request.QueryString["id"]);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Appointment Accepted');", true);
            

            //string msgData = "Dear Customer, Your Order is confirmed by Genericart Medicine. We will notify you when it is ready to dispatch. Genericart Medicine - Wahi Kaam, Sahi Daam.";

            //int custId = Convert.ToInt32(c.GetReqData("OrdersData", "FK_OrderCustomerID", "OrderID=" + Request.QueryString["id"]));
            //string mobNo = c.GetReqData("CustomersData", "CustomerMobile", "CustomrtID=" + custId).ToString();

            //c.SendSMS(msgData, mobNo);

            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('my-appointments.aspx', 2000);", true); ;
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('my-appointments.aspx', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSubmit_Click", ex.Message.ToString());
            return;
        }
    }

    protected void btnDeny_Click(object sender, EventArgs e)
    {
        try
        {
            rejectApp.Visible = true;

            //if (txtDenyReason.Text == "")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Appointment Deny Reason');", true);
            //    return;
            //}

            //if (txtDenyReason.Text != "")
            //{
            //    c.ExecuteQuery("Update DoctorsAppointmentData Set DocAppStatus=2, AppDenyReason='" + txtDenyReason.Text + "' Where DocAppID=" + Request.QueryString["id"]);
            //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Appointment Denied');", true);
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('my-appointments.aspx', 2000);", true);
            //}
            //c.ExecuteQuery("Update DoctorsAppointmentData Set DocAppStatus=2 Where DocAppID=" + Request.QueryString["id"]);
            //ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Appointment Denied');", true);
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('my-appointments.aspx', 2000);", true);
            GetAppData(Convert.ToInt32(Request.QueryString["id"]));
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnDeny_Click", ex.Message.ToString());
            return;
        }
    }

    private void GetAppData(int appIdX)
    {
        try
        {
            using (DataTable dtApp = c.GetDataTable("Select DocAppID, DocAppDate, DocAppName, DocAppEmail, DocAppMobile, DocAppAge, DeviceType, DocAppGender, DocAppDesc, DocAppAddress, DocAppPincode, PrevDocName, DocAppType, DocAppStatus, Doc_txn_id, Doc_pay_amount From DoctorsAppointmentData Where DocAppID=" + appIdX))
            {
                if (dtApp.Rows.Count > 0)
                {
                    DataRow row = dtApp.Rows[0];

                    ordData[0] = appIdX.ToString();
                    ordData[1] = Convert.ToDateTime(row["DocAppDate"]).ToString("dd/MM/yyyy");
                    ordData[2] = row["DocAppName"].ToString();
                    ordData[3] = row["DocAppAge"].ToString();
                    ordData[4] = row["DocAppGender"].ToString() == "1" ? "Male" : "Female";
                    ordData[5] = row["DocAppEmail"].ToString();
                    ordData[6] = row["DocAppMobile"].ToString();
                    ordData[7] = row["DocAppAddress"].ToString();
                    ordData[8] = row["DocAppPincode"] != DBNull.Value && row["DocAppPincode"] != null && row["DocAppPincode"].ToString() != "" ? row["DocAppPincode"].ToString() : "";
                    ordData[9] = row["DocAppDesc"].ToString();
                    ordData[10] = row["PrevDocName"] != DBNull.Value && row["PrevDocName"] != null && row["PrevDocName"].ToString() != "" ? row["PrevDocName"].ToString() : "";
                    ordData[11] = row["DocAppType"].ToString() == "1" ? "Self" : "Family Member";

                    if (row["DocAppStatus"].ToString() == "1") // accepted
                    {
                        btnCompleted.Visible = true;
                        btnSubmit.Visible = false;
                    }
                    if (row["DocAppStatus"].ToString() == "2") // denied 
                    {
                        btnCompleted.Visible = false;
                        btnSubmit.Visible = false;
                        btnDeny.Visible = false;
                    }
                    if (row["DocAppStatus"].ToString() == "3") // 3 > completed
                    {
                        btnCompleted.Visible = false;
                        btnSubmit.Visible = false;
                        btnDeny.Visible = false;
                    }

                    deviceType = row["DeviceType"] != DBNull.Value && row["DeviceType"] != null && row["DeviceType"].ToString() != "" ? row["DeviceType"].ToString() : "";

                    if (row["Doc_txn_id"] != DBNull.Value && row["Doc_txn_id"] != null && row["Doc_txn_id"].ToString() != "")
                    {
                        if (Session["adminDoctor"].ToString() == "5")
                        {
                            paidApp.Visible = true;
                        }
                        string payStatus = c.GetReqData("online_payment_logs", "OPL_transtatus", "OPL_merchantTranId='" + row["Doc_txn_id"].ToString() + "'").ToString();
                        ordData[13] = payStatus;
                        ordData[12] = row["Doc_pay_amount"].ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetAppData", ex.Message.ToString());
            return;
        }
    }

    protected void btnCompleted_Click(object sender, EventArgs e)
    {
        try
        {
            c.ExecuteQuery("Update DoctorsAppointmentData Set DocAppStatus=3 Where DocAppID=" + Request.QueryString["id"]);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Appointment Marked as Completed');", true);
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('my-appointments.aspx', 2000);", true);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('my-appointments.aspx', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnCompleted_Click", ex.Message.ToString());
            return;
        }
    }
    protected void btnSubmitReason_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtDenyReason.Text == "")
            {
                GetAppData(Convert.ToInt32(Request.QueryString["id"]));
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Appointment Deny Reason');", true);
                return;
            }

            if (txtDenyReason.Text != "")
            {
                c.ExecuteQuery("Update DoctorsAppointmentData Set DocAppStatus=2, AppDenyReason='" + txtDenyReason.Text + "' Where DocAppID=" + Request.QueryString["id"]);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Appointment Denied');", true);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('my-appointments.aspx', 2000);", true);
            }
            //c.ExecuteQuery("Update DoctorsAppointmentData Set DocAppStatus=2 Where DocAppID=" + Request.QueryString["id"]);
            //ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Appointment Denied');", true);
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('my-appointments.aspx', 2000);", true);
            
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSubmitReason_Click", ex.Message.ToString());
            return;
        }
    }
}