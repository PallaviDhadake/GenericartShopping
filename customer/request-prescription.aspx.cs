using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class customer_request_prescription : System.Web.UI.Page
{
    iClass c = new iClass();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                FillGrid();
                if (rdbMyself.Checked == true)
                {
                    GetCustInfo();
                }
                else
                {
                    txtName.Text = txtMobile.Text = txtAge.Text = "";
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

    private void GetCustInfo()
    {
        try
        {
            using (DataTable dtCust = c.GetDataTable("Select CustomrtID, CustomerName, CustomerMobile, CustomerDOB From CustomersData Where CustomrtID=" + Session["genericCust"]))
            {
                if (dtCust.Rows.Count > 0)
                {
                    DataRow row = dtCust.Rows[0];

                    txtName.Text = row["CustomerName"].ToString();
                    txtMobile.Text = row["CustomerMobile"].ToString();
                    if (row["CustomerDOB"] != DBNull.Value && row["CustomerDOB"] != null && row["CustomerDOB"].ToString() != "")
                    {
                        int custAge = CalculateAge(Convert.ToDateTime(row["CustomerDOB"]));
                        txtAge.Text = custAge.ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetCustInfo", ex.Message.ToString());
            return;
        }
    }

    private int CalculateAge(DateTime dateOfBirth)
    {
        int age = 0;
        age = DateTime.Now.Year - dateOfBirth.Year;
        if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
            age = age - 1;

        return age;
    }

    protected void rdbMyself_CheckedChanged(object sender, EventArgs e)
    {
        if (rdbMyself.Checked == true)
        {
            GetCustInfo();
        }
    }
    protected void rdbFamily_CheckedChanged(object sender, EventArgs e)
    {
        txtName.Text = txtMobile.Text = txtAge.Text = "";
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            txtName.Text = txtName.Text.Trim().Replace("'", "");
            txtMobile.Text = txtMobile.Text.Trim().Replace("'", "");
            txtAge.Text = txtAge.Text.Trim().Replace("'", "");
            txtDisease.Text = txtDisease.Text.Trim().Replace("'", "");
            txtMedicine.Text = txtMedicine.Text.Trim().Replace("'", "");
            txtAddr.Text = txtAddr.Text.Trim().Replace("'", "");

            if (txtName.Text == "" || txtMobile.Text == "" || txtAge.Text == "" || txtDisease.Text == "" || txtMedicine.Text == "" || txtAddr.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * Marked fields are Mandatory');", true);
                return;
            }

            if (!c.IsNumeric(txtAge.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Age must be numeric value');", true);
                return;
            }

            if (!c.ValidateMobile(txtMobile.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid mobile number');", true);
                return;
            }

            int rxReqFor = 0;
            if (rdbMyself.Checked == true)
                rxReqFor = 1;
            else if (rdbFamily.Checked == true)
                rxReqFor = 2;


            int maxId = c.NextId("PrescriptionRequest", "PreReqID");

            c.ExecuteQuery("Insert Into PrescriptionRequest (PreReqID, PreReqDate, FK_CustomerID, PreReqName, PreReqAge, PreReqGender, PreReqMobile, " +
                " PreReqDisease, PreReqMedines, FK_FrachID, PreReqFor, PreReqType, FK_DoctorID, PreReqStatus, PreReqAddr, DeviceType) Values (" + maxId + ", '" + DateTime.Now +
                "', " + Session["genericCust"] + ", '" + txtName.Text + "', " + txtAge.Text + ", " + ddrGender.SelectedValue + ", '" + txtMobile.Text +
                "', '" + txtDisease.Text + "', '" + txtMedicine.Text + "', 0, " + rxReqFor + ", 1, 5, 1, '" + txtAddr.Text + "', 'Web')");

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Your Prescription Request Submittted successfully..!!');", true);

            txtName.Text = txtAge.Text = txtMobile.Text = txtDisease.Text = txtMedicine.Text = "";
            ddrGender.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSave_Click", ex.Message.ToString());
            return;
        }
    }

    private void FillGrid()
    {
        try
        {
            using (DataTable dtRx = c.GetDataTable("Select PreReqID, Convert(varchar(20), PreReqDate, 103) as reqDate, FK_CustomerID, PreReqName, PreReqDisease, PreReqStatus From PrescriptionRequest Where FK_CustomerID=" + Session["genericCust"] + " Order By PreReqID DESC"))
            {
                gvRxReq.DataSource = dtRx;
                gvRxReq.DataBind();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FillGrid", ex.Message.ToString());
            return;
        }
    }

    protected void gvRxReq_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string rPath = c.ReturnHttp();
                // PreReqStatus=0 > Pending, 1 > Approved, 2 > Denied , 3 > Rx Uploaded
                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                switch (e.Row.Cells[1].Text)
                {

                    case "0":
                        litStatus.Text = "<div class=\"ordNew\">New</div>";
                        break;
                    case "1":
                        litStatus.Text = "<div class=\"ordProcessing\">Pending</div>";
                        break;
                    case "2":
                        litStatus.Text = "<div class=\"ordDenied\">Denied</div>";
                        break;
                    case "3":
                        string rxName = c.GetReqData("PrescriptionUploads", "PreUploadCopy", "FK_PreReqID=" + e.Row.Cells[0].Text).ToString();
                        string rxUploadedDate = c.GetReqData("PrescriptionUploads", "PreUploadDate", "FK_PreReqID=" + e.Row.Cells[0].Text).ToString();
                        string rxDate = Convert.ToDateTime(rxUploadedDate).ToString("dd/MM/yyyy");
                        litStatus.Text = "<a href=\"" + rPath + "upload/docRx/" + rxName + "\" class=\"ordAccepted txtDecNone\" target=\"_blank\">Download Prescription (Uploaded On : " + rxDate + ")</a>";
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvRxReq_RowDataBound", ex.Message.ToString());
            return;
        }
    }
}