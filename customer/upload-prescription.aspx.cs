using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;

public partial class customer_upload_prescription : System.Web.UI.Page
{
    iClass c = new iClass();
    public string prescriStr, orId;
    protected void Page_Load(object sender, EventArgs e)
    {
        btnUploadRx.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnUploadRx, null) + ";");
        if (Request.QueryString["orderId"] != null)
        {
            orId = Request.QueryString["orderId"].ToString();
            GetUploadedPrescription(Convert.ToInt32(Request.QueryString["orderId"]));

            if (Request.QueryString["type"] != null && Request.QueryString["prId"] != null)
            {
                if (c.IsRecordExist("Select PrescriptionID From OrderPrescriptions Where FK_OrderID=" + Request.QueryString["orderId"] + " AND PrescriptionID=" + Request.QueryString["prId"] + " AND PrescriptionStatus=0"))
                {
                    // delete prescription
                    c.ExecuteQuery("Delete From OrderPrescriptions Where FK_OrderID=" + Request.QueryString["orderId"] + " AND PrescriptionID=" + Request.QueryString["prId"]);
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Prescription Deleted');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Something Went Wrong');", true);
                }
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('upload-prescription?orderId=" + Request.QueryString["orderId"] + "', 2000);", true);
            }
        }
    }

    protected void btnUploadRx_Click(object sender, EventArgs e)
    {
        try
        {
            int orderId = Convert.ToInt32(Request.QueryString["orderId"]);

            string imgName = "";
            int prescriptionId = c.NextId("OrderPrescriptions", "PrescriptionID");
            if (fuPrescription.HasFile)
            {
                string fExt = Path.GetExtension(fuPrescription.FileName).ToString().ToLower();
                if (fExt == ".jpg" || fExt == ".jpeg" || fExt == ".png")
                {
                    imgName = "med-rx-" + prescriptionId + fExt;
                    ImageUploadProcess(imgName);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Only .jpg, .jpeg or .png files are allowed');", true);
                    //return;
                }

                c.ExecuteQuery("Insert Into OrderPrescriptions (PrescriptionID, FK_OrderID, PrescriptionName, PrescriptionStatus) " +
                    " Values (" + prescriptionId + ", " + orderId + ", '" + imgName + "', 0)");

                GetUploadedPrescription(orderId);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select image to upload prescription');", true);
                //return;
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('" + Master.rootPath + "upload-prescription?orderId=" + Request.QueryString["orderId"] + "', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnUploadRx_Click", ex.Message.ToString());
            return;
        }
    }

    private void ImageUploadProcess(string imgName)
    {
        try
        {
            string origImgPath = "~/upload/prescriptions/original/";
            string normalImgPath = "~/upload/prescriptions/";

            fuPrescription.SaveAs(Server.MapPath(origImgPath) + imgName);

            c.ImageOptimizer(imgName, origImgPath, normalImgPath, 800, true);

            //Delete rew image from server
            File.Delete(Server.MapPath(origImgPath) + imgName);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "ImageUploadProcess", ex.Message.ToString());
            return;
        }
    }

    private void GetUploadedPrescription(int ordIdX)
    {
        try
        {
            using (DataTable dtRx = c.GetDataTable("Select PrescriptionID, PrescriptionName, PrescriptionStatus From OrderPrescriptions Where FK_OrderID=" + ordIdX + " AND PrescriptionStatus=0"))
            {
                if (dtRx.Rows.Count > 0)
                {
                    StringBuilder strMarkup = new StringBuilder();
                    int bCount = 0;
                    string rPath = c.ReturnHttp();
                    foreach (DataRow row in dtRx.Rows)
                    {
                        strMarkup.Append("<div class=\"imgBox\">");
                        strMarkup.Append("<div class=\"pad-5\">");
                        strMarkup.Append("<div class=\"border1 posRelative\">");
                        if (row["PrescriptionStatus"].ToString() == "2")
                        {
                            strMarkup.Append("<div class=\"absRejected fontRegular\">Rejected</div>");
                        }
                        if (row["PrescriptionStatus"].ToString() == "1")
                        {
                            strMarkup.Append("<div class=\"absAccepted fontRegular\">Accepted</div>");
                        }
                        if (row["PrescriptionStatus"].ToString() == "0")
                        {
                            strMarkup.Append("<div class=\"absPending fontRegular\">Pending for Approval</div>");
                        }
                        strMarkup.Append("<div class=\"pad-5\">");
                        strMarkup.Append("<div class=\"imgContainer\">");
                        strMarkup.Append("<a href=\"" + rPath + "upload/prescriptions/" + row["PrescriptionName"] + "\" data-fancybox=\"rxGroup\"><img src=\"" + rPath + "upload/prescriptions/" + row["PrescriptionName"] + "\" class=\"width100\" /></a>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("<a href=\"upload-prescription?orderId=" + Request.QueryString["orderId"] + "&type=delete&prId=" + row["PrescriptionID"] + "\" title=\"Delete Prescription\"  class=\"closeAnch\"></a>");
                        strMarkup.Append("</div>");

                        bCount++;

                        if ((bCount % 4) == 0)
                        {
                            strMarkup.Append("<div class=\"float_clear\"></div>");
                        }
                    }
                    strMarkup.Append("<div class=\"float_clear\"></div>");
                    prescriStr = strMarkup.ToString();
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