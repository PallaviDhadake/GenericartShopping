using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

public partial class admingenshopping_prescription_requests : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] ordData = new string[20]; //10
    public string deviceType;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    //c.FillComboBox("a.DocName + ' ('+ b.SpecialtyName +')' as doc", "a.DoctorID", "DoctorsData a Inner Join DoctorSpecialtyData b On a.FK_DocSpecialtyID=b.SpecialtyID", "a.DelMark=0 AND a.DocActive=1", "a.DocName", 0, ddrDoc);
                    FillDoctors();
                    readReq.Visible = true;
                    viewReq.Visible = false;
                    GetRequestDetails(Convert.ToInt32(Request.QueryString["id"]));

                    int reqStatus = Convert.ToInt32(c.GetReqData("PrescriptionRequest", "PreReqStatus", "PreReqID=" + Request.QueryString["id"]));
                    if (reqStatus == 3)
                    {
                        btnAssign.Visible = false;
                        btnDeny.Visible = false;
                    }
                    if (reqStatus == 1)
                    {
                        btnDeny.Visible = false;
                    }
                }
                else
                {
                    readReq.Visible = false;
                    viewReq.Visible = true;
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

    private void FillDoctors()
    {
        try
        {
            SqlConnection con = new SqlConnection(c.OpenConnection());

            SqlDataAdapter da = new SqlDataAdapter("Select a.DocName + ' ('+ b.SpecialtyName +')' as doc, a.DoctorID From DoctorsData a Inner Join DoctorSpecialtyData b On a.FK_DocSpecialtyID=b.SpecialtyID Where a.DelMark=0 AND a.DocActive=1 Order By a.DocName", con);
            DataSet ds = new DataSet();
            da.Fill(ds, "myCombo");

            ddrDoc.DataSource = ds.Tables[0];
            ddrDoc.DataTextField = ds.Tables[0].Columns["doc"].ColumnName.ToString();
            ddrDoc.DataValueField = ds.Tables[0].Columns["DoctorID"].ColumnName.ToString();
            ddrDoc.DataBind();

            ddrDoc.Items.Insert(0, "<-Select->");
            ddrDoc.Items[0].Value = "0";

            con.Close();
            con = null;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FillDoctors", ex.Message.ToString());
            return;
        }
    }

    private void FillGrid()
    {
        try
        {
            using (DataTable dtReq = c.GetDataTable("Select PreReqID, Convert(varchar(20), PreReqDate, 103) as reqDate, FK_CustomerID, PreReqName, PreReqAge, PreReqMobile, PreReqDisease, PreReqStatus, isnull(DeviceType, '-') as DeviceType From PrescriptionRequest Order By PreReqID DESC"))
            {
                gvReq.DataSource = dtReq;
                gvReq.DataBind();
                if (gvReq.Rows.Count > 0)
                {
                    gvReq.UseAccessibleHeader = true;
                    gvReq.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvReq_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litView = (Literal)e.Row.FindControl("litView");
                litView.Text = "<a href=\"prescription-requests.aspx?id=" + e.Row.Cells[0].Text + "\" class=\"gView\" title=\"View/Edit\"></a>";

                // PreReqStatus=0 > Pending, 1 > Approved, 2 > Denied , 3 > Rx Uploaded
                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                switch (e.Row.Cells[1].Text)
                {

                    case "0":
                        litStatus.Text = "<div class=\"ordNew\">New</div>";
                        break;
                    case "1":
                        litStatus.Text = "<div class=\"ordAccepted\">Accepted</div>";
                        break;
                    case "2":
                        litStatus.Text = "<div class=\"ordDenied\">Denied</div>";
                        break;
                    case "3":
                        litStatus.Text = "<div class=\"ordShipped\">Prescription Uploaded</div>";
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvReq_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    private void GetRequestDetails(int reqIdX)
    {
        try
        {
            using (DataTable dtReq = c.GetDataTable("Select PreReqID, PreReqDate, FK_CustomerID, PreReqName, PreReqAge, PreReqGender, PreReqMobile, PreReqDisease, DeviceType, PreReqMedines, FK_FrachID, PreReqFor, PreReqType, FK_DoctorID, PreReqStatus, PreReqAddr From PrescriptionRequest Where PreReqID=" + reqIdX))
            {
                if (dtReq.Rows.Count > 0)
                {
                    DataRow row = dtReq.Rows[0];
                    ordData[0] = reqIdX.ToString();
                    ordData[1] = Convert.ToDateTime(row["PreReqDate"]).ToString("dd/MM/yyyy hh:mm tt");
                    ordData[2] = row["PreReqName"].ToString();
                    ordData[3] = row["PreReqMobile"].ToString();
                    ordData[4] = row["PreReqAge"].ToString();
                    ordData[5] = row["PreReqGender"].ToString() == "1" ? "Male" : "Female";
                    ordData[6] = row["PreReqDisease"].ToString();
                    ordData[7] = Regex.Replace(row["PreReqMedines"].ToString(), @"\r\n?|\n", "<br />");
                    ordData[8] = row["PreReqFor"].ToString() == "1" ? "Self" : "Family Member";
                    if (row["PreReqType"].ToString() == "1")
                    {
                        ordData[9] = "Customer";
                    }
                    else
                    {
                        string frName = c.GetReqData("FranchiseeData", "FranchName +' (' + FranchShopCode +')'", "FranchID=" + row["FK_FrachID"]).ToString();
                        ordData[9] = "Franchisee <span class=\"text-bold\">(" + frName + ")</span>";
                    }

                    if (row["FK_DoctorID"].ToString() != "0")
                    {
                        ddrDoc.SelectedValue = row["FK_DoctorID"].ToString();
                    }
                    deviceType = row["DeviceType"] != DBNull.Value && row["DeviceType"] != null && row["DeviceType"].ToString() != "" ? row["DeviceType"].ToString() : "";
                    ordData[10] = row["PreReqAddr"] != DBNull.Value && row["PreReqAddr"] != null && row["PreReqAddr"].ToString() != "" ? row["PreReqAddr"].ToString() : "-";
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetRequestDetails", ex.Message.ToString());
            return;
        }
    }

    protected void btnAssign_Click(object sender, EventArgs e)
    {
        try
        {
            c.ExecuteQuery("Update PrescriptionRequest Set FK_DoctorID=" + ddrDoc.SelectedValue + ", PreReqStatus=1 Where PreReqID=" + Request.QueryString["id"]);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Request Accepted and assigned to doctor');", true);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('prescription-requests.aspx', 2000);", true); ;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnAssign_Click", ex.Message.ToString());
            return;
        }
    }

    protected void btnDeny_Click(object sender, EventArgs e)
    {
        try
        {
            c.ExecuteQuery("Update PrescriptionRequest Set PreReqStatus=2 Where PreReqID=" + Request.QueryString["id"]);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Request Denied');", true);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('prescription-requests.aspx', 2000);", true); ;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnDeny_Click", ex.Message.ToString());
            return;
        }
    }
}