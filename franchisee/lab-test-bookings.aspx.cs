using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class franchisee_lab_test_bookings : System.Web.UI.Page
{
    iClass c = new iClass();
    public string appUrl;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillGrid();
        }

        string frCode = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + Session["adminFranchisee"]).ToString();
        appUrl = Master.rootPath + "book-lab-test?code=" + frCode;
    }

    private void FillGrid()
    {
        try
        {
            string shopCode = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + Session["adminFranchisee"]).ToString();
            using (DataTable dtCust = c.GetDataTable("Select LabAppID, CONVERT(varchar(20), LabAppDate, 103) as appDate, LabAppName, LabAppStatus, LabAppAge, Case When LabAppGender=1 Then 'Male' Else 'Female' End as gender, LabAppMobile, LabAppAddress, LabAppPincode, LabAppEmail, isnull(FK_LabTestID, 0) as FK_LabTestID, isnull(DeviceType, '-') as DeviceType From LabAppointments Where LabRefShopCode='" + shopCode + "' Order By LabAppDate DESC, LabAppID DESC")) 
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
}