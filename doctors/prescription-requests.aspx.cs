using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Services;
using System.IO;

public partial class doctors_prescription_requests : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] ordData = new string[20]; //10
    public string rxId, uploadedRx, deviceType;
    protected void Page_Load(object sender, EventArgs e)
    {
        // PreReqStatus=0 > Pending, 1 > Approved, 2 > Denied , 3 > Rx Uploaded
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    viewRx.Visible = false;
                    editRx.Visible = true;
                    GetRequestDetails(Convert.ToInt32(Request.QueryString["id"]));
                    rxId = Request.QueryString["id"].ToString();
                }
                else
                {
                    viewRx.Visible = true;
                    editRx.Visible = false;
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

            if (Request.QueryString["type"] != null)
            {
                if (Request.QueryString["type"] == "new")
                {
                    strQuery = "Select PreReqID, Convert(varchar(20), PreReqDate, 103) as reqDate, FK_CustomerID, PreReqName, PreReqAge, PreReqMobile, PreReqDisease, PreReqStatus, isnull(DeviceType, '-') as DeviceType From PrescriptionRequest Where FK_DoctorID=" + Session["adminDoctor"] + " AND PreReqStatus=1 Order By PreReqID DESC";
                }
                else if (Request.QueryString["type"] == "uploaded")
                {
                    strQuery = "Select PreReqID, Convert(varchar(20), PreReqDate, 103) as reqDate, FK_CustomerID, PreReqName, PreReqAge, PreReqMobile, PreReqDisease, PreReqStatus, isnull(DeviceType, '-') as DeviceType From PrescriptionRequest Where FK_DoctorID=" + Session["adminDoctor"] + " AND PreReqStatus=3 Order By PreReqID DESC";
                }
            }
            else
            {
                strQuery = "Select PreReqID, Convert(varchar(20), PreReqDate, 103) as reqDate, FK_CustomerID, PreReqName, PreReqAge, PreReqMobile, PreReqDisease, PreReqStatus, isnull(DeviceType, '-') as DeviceType From PrescriptionRequest Where FK_DoctorID=" + Session["adminDoctor"] + " Order By PreReqID DESC";
            }

            using (DataTable dtReq = c.GetDataTable(strQuery))
            {
                gvRx.DataSource = dtReq;
                gvRx.DataBind();
                if (gvRx.Rows.Count > 0)
                {
                    gvRx.UseAccessibleHeader = true;
                    gvRx.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvRx_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litView = (Literal)e.Row.FindControl("litView");
                litView.Text = "<a href=\"prescription-requests.aspx?id=" + e.Row.Cells[0].Text + "\" class=\"gView\" title=\"View/Edit\"></a>";

                Literal litForword = (Literal)e.Row.FindControl("litForword");

                // PreReqStatus=0 > Pending, 1 > Approved, 2 > Denied , 3 > Rx Uploaded
                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                switch (e.Row.Cells[1].Text)
                {

                    //case "0":
                    //    litStatus.Text = "<div class=\"ordNew\">New</div>";
                    //    break;
                    case "1":
                        litStatus.Text = "<div class=\"ordNew\">New</div>";
                        break;
                    case "2":
                        litStatus.Text = "<div class=\"ordDenied\">Denied</div>";
                        break;
                    case "3":
                        litStatus.Text = "<div class=\"ordShipped\">Prescription Uploaded</div>";
                        litForword.Text = "<a href=\"shop-list.aspx?rxId=" + e.Row.Cells[0].Text + "\" class=\"btn btn-sm btn-primary\" target=\"_blank\" title=\"Forawrd Prescription to Shop\"><i class=\"fas fa-share\"></i> Forward Prescription</a>";
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
            using (DataTable dtReq = c.GetDataTable("Select PreReqID, PreReqDate, FK_CustomerID, PreReqName, PreReqAge, PreReqGender, PreReqMobile, PreReqDisease, PreReqMedines, DeviceType, PreReqAddr, FK_FrachID, PreReqFor, PreReqType, FK_DoctorID, PreReqStatus From PrescriptionRequest Where PreReqID=" + reqIdX))
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

                    if (c.IsRecordExist("Select PreItemId From PrescriptionItems Where FK_PreReqID=" + Request.QueryString["id"]))
                    {
                        FillMedGrid();
                    }

                    if (c.IsRecordExist("Select PreUploadID From PrescriptionUploads Where FK_PreReqID=" + Request.QueryString["id"]))
                    {
                        GetUploadedPrescription(Convert.ToInt32(Request.QueryString["id"]));
                    }

                    ordData[10] = row["PreReqAddr"] != DBNull.Value && row["PreReqAddr"] != null && row["PreReqAddr"].ToString() != "" ? row["PreReqAddr"].ToString() : "-";
                    deviceType = row["DeviceType"] != DBNull.Value && row["DeviceType"] != null && row["DeviceType"].ToString() != "" ? row["DeviceType"].ToString() : "";
                    rxId = Request.QueryString["id"].ToString();
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

    [WebMethod]
    //[System.Web.Script.Services.ScriptMethod(UseHttpGet = false, ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static List<string> GetMedName(string medName)
    {
        iClass c = new iClass();
        List<string> medResult = new List<string>();
        using (DataTable dtMed = c.GetDataTable("Select TOP 100 ProductID, ProductName From ProductsData Where ProductName LIKE'" + medName + "%'"))
        {
            if (dtMed.Rows.Count > 0)
            {
                foreach (DataRow row in dtMed.Rows)
                {
                    medResult.Add(string.Format("{0}#{1}", row["ProductName"], row["ProductID"]));
                }
            }
            else
            {
                medResult.Add("Match not found");
            }

            return medResult;
        }
    }

    [WebMethod]
    public static string GetMedInfo(string medId)
    {
        //return "intellect" + medId.ToString();
        
        HttpContext.Current.Session["medData"] = medId;

        return medId;
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            GetRequestDetails(Convert.ToInt32(Request.QueryString["id"]));

            txtMedName.Text = txtMedName.Text.Trim().Replace("'", "");
            txtQty.Text = txtQty.Text.Trim().Replace("'", "");
            txtDose1.Text = txtDose1.Text.Trim().Replace("'", "");
            txtDose2.Text = txtDose2.Text.Trim().Replace("'", "");
            txtDose3.Text = txtDose3.Text.Trim().Replace("'", "");
            txtNote.Text = txtNote.Text.Trim().Replace("'", "");
            txtDays.Text = txtDays.Text.Trim().Replace("'", "");

            if (txtMedName.Text == "" || txtQty.Text == "" || txtDose1.Text == "" || txtDose2.Text == "" || txtDose3.Text == "" || txtNote.Text == "" || txtDays.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * marked fields are compulsory');", true);
                return;
            }

            if (!c.IsNumeric(txtQty.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Quantity Must be numeric value');", true);
                return;
            }

            int maxId = c.NextId("PrescriptionItems", "PreItemId");
            int medId = Convert.ToInt32(Session["medData"]);

            c.ExecuteQuery("Insert Into PrescriptionItems(PreItemId, FK_PreReqID, FK_PreProductID, PreItemQty, PreItemDose1, PreItemDose2, " +
                " PreItemDose3, PreItemNote, PreItemDays) Values (" + maxId + ", " + Request.QueryString["id"] + ", " + medId + ", " + txtQty.Text +
                ", '" + txtDose1.Text + "', '" + txtDose2.Text + "', '" + txtDose3.Text + "', '" + txtNote.Text + "', '" + txtDays.Text + "')");

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Medicine Added');", true);

            txtMedName.Text = txtQty.Text = txtDose1.Text = txtDose2.Text = txtDose3.Text = txtNote.Text = txtDays.Text = "";

            FillMedGrid();
            txtMedName.Focus();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnAdd_Click", ex.Message.ToString());
            return;
        }
    }

    private void FillMedGrid()
    {
        try
        {
            using (DataTable dtMedItem = c.GetDataTable("Select a.PreItemId, a.FK_PreReqID, a.FK_PreProductID, a.PreItemQty, a.PreItemDose1, a.PreItemDose2, a.PreItemDose3, a.PreItemNote, isnull(a.PreItemDays, 'NA') as PreItemDays, b.ProductName From PrescriptionItems a Inner Join ProductsData b On a.FK_PreProductID=b.ProductID Where a.FK_PreReqID=" + Request.QueryString["id"]))
            {
                gvRxItems.DataSource = dtMedItem;
                gvRxItems.DataBind();
                if (gvRxItems.Rows.Count > 0)
                {
                    gvRxItems.UseAccessibleHeader = true;
                    gvRxItems.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FillMedGrid", ex.Message.ToString());
            return;
        }
    }

    protected void gvRxItems_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GetRequestDetails(Convert.ToInt32(Request.QueryString["id"]));
            GridViewRow gRow = (GridViewRow)((Button)e.CommandSource).NamingContainer;
            if (e.CommandName == "gvDel")
            {
                c.ExecuteQuery("Delete From PrescriptionItems Where FK_PreReqID=" + Request.QueryString["id"] + " AND PreItemId=" + gRow.Cells[0].Text);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Medicine Deleted');", true);
                FillMedGrid();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvRxItems_RowCommand", ex.Message.ToString());
            return;
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            rxId = Request.QueryString["id"].ToString();
            string imgName = "";
            if (fuRx.HasFile)
            {
                string fExt = Path.GetExtension(fuRx.FileName).ToString().ToLower();
                if (fExt == ".jpg" || fExt == ".jpeg" || fExt == ".png")
                {
                    imgName = "med-rx-" + Request.QueryString["id"] + fExt;
                    ImageUploadProcess(imgName);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Only .jpg, .jpeg or .png files are allowed');", true);
                    //return;
                }
                int maxID = c.NextId("PrescriptionUploads", "PreUploadID");

                if (c.IsRecordExist("Select PreUploadID From PrescriptionUploads Where FK_PreReqID=" + Request.QueryString["id"]))
                {
                    int uploadId = Convert.ToInt32(c.GetReqData("PrescriptionUploads", "PreUploadID", "FK_PreReqID=" + Request.QueryString["id"]));
                    c.ExecuteQuery("Update PrescriptionUploads Set PreUploadCopy='" + imgName + "', PreUploadDate='" + DateTime.Now + "' Where PreUploadID=" + uploadId);
                }
                else
                {
                    c.ExecuteQuery("Insert Into PrescriptionUploads (PreUploadID, PreUploadDate, FK_PreReqID, PreUploadCopy) " +
                        " Values (" + maxID + ", '" + DateTime.Now + "', " + Request.QueryString["id"] + ", '" + imgName + "')");

                    c.ExecuteQuery("Update PrescriptionRequest Set PreReqStatus=3 Where PreReqID=" + Request.QueryString["id"]);
                }

                GetUploadedPrescription(Convert.ToInt32(Request.QueryString["id"]));
                

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Prescription Uploaded');", true);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('prescription-requests.aspx', 2000);", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select File to upload');", true);
                return;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnUpload_Click", ex.Message.ToString());
            return;
        }
    }

    private void ImageUploadProcess(string imgName)
    {
        try
        {
            string normalImgPath = "~/upload/docRx/";

            fuRx.SaveAs(Server.MapPath(normalImgPath) + imgName);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "ImageUploadProcess", ex.Message.ToString());
            return;
        }
    }

    private void GetUploadedPrescription(int rxIdX)
    {
        try
        {
            using (DataTable dtRx = c.GetDataTable("Select PreUploadID, PreUploadCopy From PrescriptionUploads Where FK_PreReqID=" + rxIdX))
            {
                if (dtRx.Rows.Count > 0)
                {
                    StringBuilder strMarkup = new StringBuilder();
                    int bCount = 0;
                    foreach (DataRow row in dtRx.Rows)
                    {
                        strMarkup.Append("<div class=\"imgBox\">");
                        strMarkup.Append("<div class=\"pad-5\">");
                        strMarkup.Append("<div class=\"border1\">");
                        strMarkup.Append("<div class=\"pad-5\">");
                        strMarkup.Append("<div class=\"imgContainer\">");
                        strMarkup.Append("<a href=\"" + Master.rootPath + "upload/docRx/" + row["PreUploadCopy"] + "\" data-fancybox=\"rxGroup\"><img src=\"" + Master.rootPath + "upload/docRx/" + row["PreUploadCopy"] + "\" class=\"width100\" /></a>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");

                        bCount++;

                        if ((bCount % 4) == 0)
                        {
                            strMarkup.Append("<div class=\"float_clear\"></div>");
                        }
                    }
                    strMarkup.Append("<div class=\"float_clear\"></div>");
                    uploadedRx = strMarkup.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetUploadedPrescription", ex.Message.ToString());
            return;
        }
    }
}