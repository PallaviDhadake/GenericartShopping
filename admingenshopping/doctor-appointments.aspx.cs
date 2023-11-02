using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class admingenshopping_doctor_appointments : System.Web.UI.Page
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
            strQuery = "Select DocAppID, Convert(varchar(20), DocAppDate, 103) as appDate, isnull(DeviceType, '-') as DeviceType, " +
                " Convert(varchar(20), AppSubmitDate, 103)+ ' ' + SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 13, 2)+':'+SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 16, 2)+':'+SUBSTRING(CONVERT(VARCHAR, AppSubmitDate, 100), 18, 2) AS subDate , " + 
                " DocAppName, DocAppMobile, DocAppAge, DocAppStatus, isnull(FK_DocID, 0) as FK_DocID From DoctorsAppointmentData ";
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
                litAnch.Text = "<a href=\"doctor-appointments.aspx?id=" + e.Row.Cells[0].Text + "\" class=\"gView\"></a>";

                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                switch (e.Row.Cells[1].Text)
                {
                    case "0": litStatus.Text = "<span class=\"ordNew\">New</span>"; break;
                    case "1": litStatus.Text = "<span class=\"ordAccepted\">Accepted</span>"; break;
                    case "2": litStatus.Text = "<span class=\"ordDenied\">Denied</span>"; break;
                    case "3": litStatus.Text = "<span class=\"ordShipped\">Completed</span>"; break;
                }

                Literal litDocName = (Literal)e.Row.FindControl("litDocName");
                if (e.Row.Cells[2].Text == "0")
                {
                    litDocName.Text = "";
                }
                else
                {
                    litDocName.Text = c.GetReqData("DoctorsData", "DocName", "DoctorID=" + e.Row.Cells[2].Text).ToString();
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

    private void GetAppData(int appIdX)
    {
        try
        {
            using (DataTable dtApp = c.GetDataTable("Select DocAppID, DocAppDate, DocAppName, DocAppEmail, DocAppMobile, DocAppAge, DocAppGender, DocAppDesc, DocAppAddress, DocAppPincode, PrevDocName, DocAppType, DocAppStatus, DeviceType From DoctorsAppointmentData Where DocAppID=" + appIdX))
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

}