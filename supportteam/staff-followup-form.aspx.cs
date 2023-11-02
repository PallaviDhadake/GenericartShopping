using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class supportteam_staff_followup_form : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] custumerInfo = new string[5];
    public string taskName, apiResponse, poUrl, custLookupLink;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (Request.QueryString["id"] != null)
            {
                poUrl = "submit-po.aspx?custId=" + Request.QueryString["id"];
                custLookupLink = Master.rootPath + "customer-lookup.aspx?custId=" + Request.QueryString["id"];
            } 
            if (Request.QueryString["action"] != null)
            {
                GetFeedBackData(Convert.ToInt32(Request.QueryString["feedbackId"]));

            }

            GetCustomerInfo();
        }
    }

    private void GetCustomerInfo()
    {
        try
        {
            //int customerId = Convert.ToInt32(c.GetReqData("FeedbackData", "FK_CustomerID", "FeedBkID=" + feedackId + ""));
            int teamId = Convert.ToInt32(Session["adminSupport"]);
            int taskId = Convert.ToInt32(c.GetReqData("SupportTeam", "TeamTaskID", "TeamID=" + teamId + ""));

            string type = Request.QueryString["type"];
            int id = Convert.ToInt32(Request.QueryString["Id"]);

            switch (type)
            {
                case "regcust":
                    custumerInfo[0] = c.GetReqData("CustomersData", "CustomerName", "CustomrtID=" + id + "").ToString();
                    custumerInfo[1] = c.GetReqData("CustomersData", "CustomerMobile", "CustomrtID=" + id + "").ToString();
                    taskName = "Registered Customers";
                    break;
                case "deliverord":
                    custumerInfo[0] = c.GetReqData("CustomersData", "CustomerName", "CustomrtID=" + id + "").ToString();
                    custumerInfo[1] = c.GetReqData("CustomersData", "CustomerMobile", "CustomrtID=" + id + "").ToString();
                    taskName = "Delivered Orders";
                    break;
                case "allord":
                    custumerInfo[0] = c.GetReqData("CustomersData", "CustomerName", "CustomrtID=" + id + "").ToString();
                    custumerInfo[1] = c.GetReqData("CustomersData", "CustomerMobile", "CustomrtID=" + id + "").ToString();
                    taskName = "All Orders";
                    break;
                case "labapp":
                    custumerInfo[0] = c.GetReqData("LabAppointments", "LabAppName", "LabAppID=" + id + "").ToString();
                    custumerInfo[1] = c.GetReqData("LabAppointments", "LabAppMobile", "LabAppID=" + id + "").ToString();
                    taskName = "Lab Appointment";
                    break;
                case "docapp":
                    custumerInfo[0] = c.GetReqData("DoctorsAppointmentData", "DocAppName", "DocAppID=" + id + "").ToString();
                    custumerInfo[1] = c.GetReqData("DoctorsAppointmentData", "DocAppMobile", "DocAppID=" + id + "").ToString();
                    taskName = "Doctor Appointment";
                    break;
                case "prereq":
                    custumerInfo[0] = c.GetReqData("PrescriptionRequest", "PreReqName", "PreReqID=" + id + "").ToString();
                    custumerInfo[1] = c.GetReqData("PrescriptionRequest", "PreReqMobile", "PreReqID=" + id + "").ToString();
                    taskName = "Prescription Request";
                    break;

                case "shopwisecustid":
                    custumerInfo[0] = c.GetReqData("CustomersData", "CustomerName", "CustomrtID=" + id + "").ToString();
                    custumerInfo[1] = c.GetReqData("CustomersData", "CustomerMobile", "CustomrtID=" + id + "").ToString();
                    taskName = "Company Owned Shop Orders";
                    break;


            }

            if (Request.QueryString["action"] == "edit")
            {
                int custid = Convert.ToInt32(c.GetReqData("FeedbackData", "FK_CustomerID", "FeedBkID=" + Request.QueryString["feedbackId"]));
                custumerInfo[0] = c.GetReqData("CustomersData", "CustomerName", "CustomrtID=" + custid + "").ToString();
                custumerInfo[1] = c.GetReqData("CustomersData", "CustomerMobile", "CustomrtID=" + custid + "").ToString();
                taskName = "Registered Customers";
            }
            //if (customerId != 0)
            //{
            //    using (DataTable dtCustInfo = c.GetDataTable("Select CustomerName, CustomerMobile From CustomersData Where CustomrtID=" + customerId + ""))
            //    {
            //        if (dtCustInfo.Rows.Count > 0)
            //        {
            //            DataRow row = dtCustInfo.Rows[0];

            //            custumerInfo[0] = row["CustomerName"].ToString();
            //            custumerInfo[1] = row["CustomerMobile"].ToString();

            //        }
            //    }
            //}

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSave_Click", ex.Message.ToString());
            return;
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {

        try
        {
            string strQuery = "";
            string returnLink = "";

            if (RadioButton1.Checked == false && RadioButton2.Checked == false && RadioButton3.Checked == false && RadioButton4.Checked == false && RadioButton5.Checked == false && RadioButton6.Checked == false || txtComment.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * fields are mandetory');", true);
                return;
            }
            //1=Excellent, 2=very good, 3=good
            //4=fair, 5=poor, 6=very poor
            int rating = 0;
            if (RadioButton1.Checked)
                rating = 1;
            if (RadioButton2.Checked)
                rating = 2;
            if (RadioButton3.Checked)
                rating = 3;
            if (RadioButton4.Checked)
                rating = 4;
            if (RadioButton5.Checked)
                rating = 5;
            if (RadioButton6.Checked)
                rating = 6;


            //int maxId = c.NextId("FeedbackData", "FeedBkID");
            int maxId = Request.QueryString["action"] == "edit" ? Convert.ToInt32(Request.QueryString["feedbackId"]) : c.NextId("FeedbackData", "FeedBkID");
            int franchInterest = chkFranch.Checked == true ? 1 : 0;

            if (Request.QueryString["action"] == "edit")
            {
                c.ExecuteQuery("Update FeedbackData Set FeedBkRating=" + rating + ", FeedBkText='" + txtComment.Text + "', FranchInterest=" + franchInterest + " Where FeedBkID=" + maxId + "");
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Feedback Updated Successfully');", true);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('staff-followup-report.aspx', 2000);", true);
            }
            else
            {
                DateTime cDate = DateTime.Now;
                string currentDate = cDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                int teamId = Convert.ToInt32(Session["adminSupport"]);
                int taskId = Convert.ToInt32(c.GetReqData("SupportTeam", "TeamTaskID", "TeamID=" + teamId + ""));
                //int franchInterest = chkFranch.Checked == true ? 1 : 0;

                int feedBackTransID = 0;

                switch (taskId)
                {
                    case 1:
                        feedBackTransID = 0;
                        if (c.IsRecordExist("Select FeedBkID From FeedbackData Where FK_CustomerID=" + Request.QueryString["id"] + " And FeedBkTaskID=" + taskId + ""))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Feedback is already recorded');", true);
                            GetCustomerInfo();
                            return;
                        }
                        strQuery = "Insert Into FeedbackData(FeedBkID, FeedBkDate, FK_TeamID, FK_CustomerID, FeedBkTaskID, FeedBkTransID, FeedBkRating, FeedBkText, FranchInterest)Values(" + maxId + ", '" + currentDate + "', " + teamId + ", " + Request.QueryString["id"] + ", " + taskId + ", 0, " + rating + ", '" + txtComment.Text + "', " + franchInterest + ")";
                        returnLink = "staff-followup-new.aspx";
                        break;
                    case 2:
                        feedBackTransID = Convert.ToInt32(c.GetReqData("OrdersData", "OrderID", "FK_OrderCustomerID=" + Request.QueryString["id"] + ""));
                        if (c.IsRecordExist("Select FeedBkID From FeedbackData Where FK_CustomerID=" + Request.QueryString["id"] + " And FeedBkTaskID=" + taskId + " And FeedBkTransID=" + feedBackTransID + ""))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Feedback is already recorded');", true);
                            GetCustomerInfo();
                            return;
                        }
                        strQuery = "Insert Into FeedbackData(FeedBkID, FeedBkDate, FK_TeamID, FK_CustomerID, FeedBkTaskID, FeedBkTransID, FeedBkRating, FeedBkText, FranchInterest)Values(" + maxId + ", '" + currentDate + "', " + teamId + ", " + Request.QueryString["id"] + ", " + taskId + ", " + feedBackTransID + ", " + rating + ", '" + txtComment.Text + "', " + franchInterest + ")";
                        returnLink = "staff-followup-delivered-order.aspx";
                        break;
                    case 3:
                        feedBackTransID = Convert.ToInt32(c.GetReqData("OrdersData", "Top 1 OrderID", "FK_OrderCustomerID=" + Request.QueryString["id"] + " Order by OrderDate desc"));
                        if (c.IsRecordExist("Select FeedBkID From FeedbackData Where FK_CustomerID=" + Request.QueryString["id"] + " And FeedBkTaskID=" + taskId + " And FeedBkTransID=" + feedBackTransID + ""))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Feedback is already recorded');", true);
                            GetCustomerInfo();
                            return;
                        }
                        strQuery = "Insert Into FeedbackData(FeedBkID, FeedBkDate, FK_TeamID, FK_CustomerID, FeedBkTaskID, FeedBkTransID, FeedBkRating, FeedBkText, FranchInterest)Values(" + maxId + ", '" + currentDate + "', " + teamId + ", " + Request.QueryString["id"] + ", " + taskId + ", " + feedBackTransID + ", " + rating + ", '" + txtComment.Text + "', " + franchInterest + ")";
                        returnLink = "staff-followup-all-orders.aspx";
                        break;
                    case 4:
                        if (c.IsRecordExist("Select FeedBkID From FeedbackData Where FeedBkTaskID=" + taskId + " And FeedBkTransID=" + Request.QueryString["id"] + ""))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Feedback is already recorded');", true);
                            GetCustomerInfo();
                            return;
                        }
                        strQuery = "Insert Into FeedbackData(FeedBkID, FeedBkDate, FK_TeamID, FK_CustomerID, FeedBkTaskID, FeedBkTransID, FeedBkRating, FeedBkText, FranchInterest)Values(" + maxId + ", '" + currentDate + "', " + teamId + ", 0, " + taskId + ", " + Request.QueryString["id"] + ", " + rating + ", '" + txtComment.Text + "', " + franchInterest + ")";
                        returnLink = "staff-followup-lab-appointment.aspx";
                        break;
                    case 5:
                        if (c.IsRecordExist("Select FeedBkID From FeedbackData Where FeedBkTaskID=" + taskId + " And FeedBkTransID=" + Request.QueryString["id"] + ""))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Feedback is already recorded');", true);
                            GetCustomerInfo();
                            return;
                        }
                        strQuery = "Insert Into FeedbackData(FeedBkID, FeedBkDate, FK_TeamID, FK_CustomerID, FeedBkTaskID, FeedBkTransID, FeedBkRating, FeedBkText, FranchInterest)Values(" + maxId + ", '" + currentDate + "', " + teamId + ", 0, " + taskId + ", " + Request.QueryString["id"] + ", " + rating + ", '" + txtComment.Text + "', " + franchInterest + ")";
                        returnLink = "staff-followup-doctors-appointment.aspx";
                        break;
                    case 6:
                        int customerId = Convert.ToInt32(c.GetReqData("PrescriptionRequest", "FK_CustomerID", "PreReqID=" + Request.QueryString["id"] + ""));
                        if (c.IsRecordExist("Select FeedBkID From FeedbackData Where FK_CustomerID=" + customerId + " And FeedBkTaskID=" + taskId + " And FeedBkTransID=" + Request.QueryString["id"] + ""))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Feedback is already recorded');", true);
                            GetCustomerInfo();
                            return;
                        }
                        strQuery = "Insert Into FeedbackData(FeedBkID, FeedBkDate, FK_TeamID, FK_CustomerID, FeedBkTaskID, FeedBkTransID, FeedBkRating, FeedBkText, FranchInterest)Values(" + maxId + ", '" + currentDate + "', " + teamId + ", " + customerId + " , " + taskId + ", " + Request.QueryString["id"] + ", " + rating + ", '" + txtComment.Text + "', " + franchInterest + ")";
                        returnLink = "staff-followup-prescription-request.aspx";
                        break;
                    case 8:
                        feedBackTransID = Convert.ToInt32(c.GetReqData("OrdersData", "Top 1 OrderID", "FK_OrderCustomerID=" + Request.QueryString["id"] + " Order by OrderDate desc"));
                        if (c.IsRecordExist("Select FeedBkID From FeedbackData Where FK_CustomerID=" + Request.QueryString["id"] + " And FeedBkTaskID=" + taskId + " And FeedBkTransID=" + feedBackTransID + ""))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Feedback is already recorded');", true);
                            GetCustomerInfo();
                            return;
                        }
                        strQuery = "Insert Into FeedbackData(FeedBkID, FeedBkDate, FK_TeamID, FK_CustomerID, FeedBkTaskID, FeedBkTransID, FeedBkRating, FeedBkText, FranchInterest)Values(" + maxId + ", '" + currentDate + "', " + teamId + ", " + Request.QueryString["id"] + " , " + taskId + ", " + feedBackTransID + ", " + rating + ", '" + txtComment.Text + "', " + franchInterest + ")";
                        returnLink = "staff-followup-compowned-shopwise-orders.aspx?franchId=" + Request.QueryString["frId"] + "";
                        break;

                }

                c.ExecuteQuery(strQuery);
                GetCustomerInfo();
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Feedback Added Successfully');", true);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('" + returnLink + "', 2000);", true);
            }


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSave_Click", ex.Message.ToString());
            return;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            int teamId = Convert.ToInt32(Session["adminSupport"]);
            int taskId = Convert.ToInt32(c.GetReqData("SupportTeam", "TeamTaskID", "TeamID=" + teamId + ""));
            string returnLink = "";
            switch (taskId)
            {
                case 1:
                    returnLink = "staff-followup-new.aspx";
                    break;
                case 2:
                    returnLink = "staff-followup-delivered-order.aspx";
                    break;
                case 3:
                    returnLink = "staff-followup-all-orders.aspx";
                    break;
                case 4:
                    returnLink = "staff-followup-lab-appointment.aspx";
                    break;
                case 5:
                    returnLink = "staff-followup-doctors-appointment.aspx";
                    break;
                case 6:
                    returnLink = "staff-followup-prescription-request.aspx";
                    break;
                case 8:
                    returnLink = "staff-followup-comp-owned-shoporder.aspx";
                    break;

            }

            if (Request.QueryString["action"] != null)
            {
                returnLink = "staff-followup-report.aspx";
            }

            Response.Redirect(returnLink, false);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnCancel_Click", ex.Message.ToString());
            return;
        }

    }


    private void GetFeedBackData(int feedbackIdX)
    {
        try
        {
            using (DataTable dtFeedbk = c.GetDataTable("Select * From FeedbackData Where FeedBkID=" + feedbackIdX))
            {
                if (dtFeedbk.Rows.Count > 0)
                {
                    DataRow row = dtFeedbk.Rows[0];

                    if (row["FeedBkRating"] != DBNull.Value && row["FeedBkRating"] != null && row["FeedBkRating"].ToString() != "")
                    {
                        switch (Convert.ToInt32(row["FeedBkRating"]))
                        {
                            case 1: RadioButton1.Checked = true; break;
                            case 2: RadioButton2.Checked = true; break;
                            case 3: RadioButton3.Checked = true; break;
                            case 4: RadioButton4.Checked = true; break;
                            case 5: RadioButton5.Checked = true; break;
                            case 6: RadioButton6.Checked = true; break;
                        }
                    }

                    if (row["FeedBkText"] != DBNull.Value && row["FeedBkText"] != null && row["FeedBkText"].ToString() != "")
                    {
                        txtComment.Text = row["FeedBkText"].ToString();
                    }

                    if (row["FranchInterest"] != DBNull.Value && row["FranchInterest"] != null && row["FranchInterest"].ToString() != "")
                    {
                        chkFranch.Checked = row["FranchInterest"].ToString() == "1" ? true : false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetFeedBackData", ex.Message.ToString());
            return;
        }
    }

    protected void btnCall_Click(object sender, EventArgs e)
    {
        try
        {
            GetCustomerInfo();
            string mobNo = c.GetReqData("CustomersData", "CustomerMobile", "CustomrtID=" + Request.QueryString["id"]).ToString();
            string agentNo = "";

            int teamId = Convert.ToInt32(Session["adminSupport"]);
            object AgentCaller = c.GetReqData("SupportTeam", "TeamAgentNum", "TeamID=" + teamId);

            if (AgentCaller != DBNull.Value && AgentCaller != null && AgentCaller.ToString() != "")
            {
                //string agentNo = "7559490407"; //manasi
                agentNo = AgentCaller.ToString();

                apiResponse = c.Servetel_ClickToCall(agentNo, mobNo);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Agent Number is not assigned.', type: 'error'});", true);
            }

           
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Error Occoured while processing', type: 'error'});", true);
            c.ErrorLogHandler(this.ToString(), "btnCall_Click", ex.Message.ToString());
            return;
        }
    }
}