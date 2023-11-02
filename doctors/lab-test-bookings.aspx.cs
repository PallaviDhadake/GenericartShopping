using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class doctors_lab_test_bookings : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg, deviceType;
    public string[] ordData = new string[20]; //11
    protected void Page_Load(object sender, EventArgs e)
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

    private void FillGrid()
    {
        try
        {
            using (DataTable dtCust = c.GetDataTable("Select LabAppID, CONVERT(varchar(20), LabAppDate, 103) as appDate, LabAppStatus, LabAppName, LabAppAge, Case When LabAppGender=1 Then 'Male' Else 'Female' End as gender, LabAppMobile, LabAppAddress, LabAppPincode, LabAppEmail, isnull(FK_LabTestID, 0) as FK_LabTestID, isnull(DeviceType, '-') as DeviceType From LabAppointments Order By LabAppDate DESC, LabAppID DESC"))
            {
                gvDetails.DataSource = dtCust;
                gvDetails.DataBind();
                if (gvDetails.Rows.Count > 0)
                {
                    gvDetails.UseAccessibleHeader = true;
                    gvDetails.HeaderRow.TableSection = TableRowSection.TableHeader;
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
    protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                object labName = c.GetReqData("LabTestData", "LabTestName", "LabTestID=" + e.Row.Cells[1].Text);

                Literal litLabTest = (Literal)e.Row.FindControl("litLabTest");
                string labtestName = "";
                if (labName != DBNull.Value && labName != null && labName.ToString() != "")
                {
                    labtestName = labName.ToString();
                }
                else
                {
                    labtestName = "-";
                }

                //e.Row.Cells[10].Text = labName.ToString();
                litLabTest.Text = labtestName.ToString();

                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"lab-test-bookings.aspx?id=" + e.Row.Cells[0].Text + "\" class=\"gView\"></a>";

                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                switch (e.Row.Cells[2].Text)
                {
                    case "0": litStatus.Text = "<span class=\"ordNew\">New</span>"; break;
                    case "1": litStatus.Text = "<span class=\"ordAccepted\">Completed</span>"; break;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvDetails_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    private void GetAppData(int appIdX)
    {
        try
        {
            using (DataTable dtApp = c.GetDataTable("Select * From LabAppointments Where LabAppID=" + appIdX))
            {
                if (dtApp.Rows.Count > 0)
                {
                    DataRow row = dtApp.Rows[0];

                    ordData[0] = appIdX.ToString();
                    ordData[1] = Convert.ToDateTime(row["LabAppDate"]).ToString("dd/MM/yyyy");
                    ordData[2] = row["LabAppName"].ToString();
                    ordData[3] = row["LabAppAge"].ToString();
                    ordData[4] = row["LabAppGender"].ToString() == "1" ? "Male" : "Female";
                    ordData[5] = row["LabAppEmail"].ToString();
                    ordData[6] = row["LabAppMobile"].ToString();
                    ordData[7] = row["LabAppAddress"].ToString();
                    ordData[8] = row["LabAppPincode"] != DBNull.Value && row["LabAppPincode"] != null && row["LabAppPincode"].ToString() != "" ? row["LabAppPincode"].ToString() : "";
                    if (row["FK_LabTestID"] != DBNull.Value && row["FK_LabTestID"] != null && row["FK_LabTestID"].ToString() != "")
                    {
                        ordData[9] = c.GetReqData("LabTestData", "LabTestName", "LabTestID=" + row["FK_LabTestID"]).ToString();
                    }

                    if (row["LabAppStatus"].ToString() == "1") // completed
                    {
                        btnCompleted.Visible = false;
                    }

                    deviceType = row["DeviceType"] != DBNull.Value && row["DeviceType"] != null && row["DeviceType"].ToString() != "" ? row["DeviceType"].ToString() : "";
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
            c.ExecuteQuery("Update LabAppointments Set LabAppStatus=1 Where LabAppID=" + Request.QueryString["id"]);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Appointment Marked as Completed');", true);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('lab-test-bookings.aspx', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnCompleted_Click", ex.Message.ToString());
            return;
        }
    }
}