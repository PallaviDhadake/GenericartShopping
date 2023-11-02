using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class doctors_generate_prescription : System.Web.UI.Page
{
    iClass c = new iClass();
    public string rxStr, errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["rxId"] != null)
                {
                    GeneratePrescription(Convert.ToInt32(Request.QueryString["rxId"]));
                }
                else
                {
                    Response.Redirect("prescription-requests.aspx", false);
                }
            }
        }
        catch (Exception ex)
        {
            rxStr = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    private void GeneratePrescription(int rxIdX)
    {
        try
        {
            using (DataTable dtRx = c.GetDataTable("Select a.PreItemId, a.FK_PreReqID, a.FK_PreProductID, a.PreItemQty, a.PreItemDose1, a.PreItemDose2, a.PreItemDose3, a.PreItemNote, a.PreItemDays, b.ProductName From PrescriptionItems a Inner Join ProductsData b On a.FK_PreProductID=b.ProductID Where a.FK_PreReqID=" + rxIdX))
            {
                if (dtRx.Rows.Count > 0)
                {
                    StringBuilder strMarkup = new StringBuilder();
                    using (DataTable dtDoc = c.GetDataTable("Select a.DoctorID, a.DocName, a.DocMobileNum, a.DocDegree, a.DocRegNo, b.SpecialtyName From DoctorsData a Inner Join DoctorSpecialtyData b On a.FK_DocSpecialtyID=b.SpecialtyID Where a.DoctorID=" + Session["adminDoctor"]))
                    {
                        if (dtDoc.Rows.Count > 0)
                        {
                            DataRow docRow = dtDoc.Rows[0];
                            
                            strMarkup.Append("<div class=\"rxLogo\">");
                            string rootPath = c.ReturnHttp();
                            strMarkup.Append("<img src=\"" + rootPath + "images/rx.png\" />");
                            strMarkup.Append("</div>");
                            strMarkup.Append("<div class=\"rxName\">");
                            strMarkup.Append("<h3 class=\"clrBlack semiBold mrg_B_5 medium\">" + docRow["DocName"].ToString() + "</h3>");
                            strMarkup.Append("<span class=\"clrLightBlack semiBold regular\">" + docRow["DocDegree"].ToString() + "</span>");
                            strMarkup.Append("<span class=\"space5\"></span>");
                            if (docRow["DocRegNo"] != DBNull.Value && docRow["DocRegNo"] != null && docRow["DocRegNo"].ToString() != "")
                            {
                                strMarkup.Append("<span class=\"semiBold\">Reg. No. - " + docRow["DocRegNo"].ToString() + "</span>");
                            }
                            else
                            {
                                strMarkup.Append("<span class=\"clrGrey fontRegular\">" + docRow["SpecialtyName"].ToString() + "</span>");
                            }
                            strMarkup.Append("</div>");
                            strMarkup.Append("<div class=\"float_clear\"></div>");
                            strMarkup.Append("<span class=\"space15\"></span>");
                            strMarkup.Append("<span class=\"addr semiBold small\">C/o. Genericart Medicine Pvt. Ltd., Near New English School, Pandharpur Road, Miraj. Dist. Sangli (MH)</span>");
                            strMarkup.Append("<span class=\"space5\"></span>");
                            strMarkup.Append("<span class=\"call semiBold small\">Contact : " + docRow["DocMobileNum"].ToString() + "</span>");
                            strMarkup.Append("<span class=\"space15\"></span>");
                            strMarkup.Append("<div class=\"rxLine\"></div>");
                            strMarkup.Append("<span class=\"space15\"></span>");
                            strMarkup.Append("<div class=\"float_left\">");
                            string name = c.GetReqData("PrescriptionRequest", "PreReqName", "PreReqID=" + rxIdX).ToString();
                            string age = c.GetReqData("PrescriptionRequest", "PreReqAge", "PreReqID=" + rxIdX).ToString();
                            string gender = c.GetReqData("PrescriptionRequest", "PreReqGender", "PreReqID=" + rxIdX).ToString();
                            string addr = "";
                            object rxaddr = c.GetReqData("PrescriptionRequest", "PreReqAddr", "PreReqID=" + rxIdX);
                            if (rxaddr != DBNull.Value && rxaddr != null && rxaddr.ToString() != "")
                                addr = rxaddr.ToString();
                            else
                                addr = "";
                            string rxGender = gender.ToString() == "1" ? "Male" : "Female";

                            strMarkup.Append("<h4 class=\"clrDarkGrey mrg_B_5 small\">Patient Name : " + name + "</h4>");
                            strMarkup.Append("<h4 class=\"clrDarkGrey mrg_B_5 small\">Sex : " + rxGender + "</h4>");
                            //strMarkup.Append("<h4 class=\"clrDarkGrey mrg_B_5 small\">Address : " + rxGender + "</h4>");
                            strMarkup.Append("</div>");
                            strMarkup.Append("<div class=\"float_right txtRight\">");
                            strMarkup.Append("<h5 class=\"clrDarkGrey mrg_B_5 small\">Date : " + DateTime.Now.ToString("dd MMM yyyy") + "</h5>");
                            strMarkup.Append("<h4 class=\"clrDarkGrey mrg_B_5 small\">Age : " + age + "</h4>");
                            strMarkup.Append("</div>");
                            strMarkup.Append("<div class=\"float_clear\"></div>");
                            if (addr.ToString() != "")
                            {
                                strMarkup.Append("<h4 class=\"clrDarkGrey mrg_B_5 small\">Address : " + addr + "</h4>");
                            }
                            strMarkup.Append("<span class=\"space20\"></span>");

                            strMarkup.Append("<img src=\"" + rootPath + "images/rx-32.png\" />");

                            strMarkup.Append("<table class=\"rxtable\">");
                            strMarkup.Append("<thead>");
                            strMarkup.Append("<tr>");
                            strMarkup.Append("<td>Medicine Name</td>");
                            strMarkup.Append("<td>Morning<br />Dose</td>");
                            strMarkup.Append("<td>Afternoon<br />Dose</td>");
                            strMarkup.Append("<td>Evening<br />Dose</td>");
                            strMarkup.Append("<td>Days</td>");
                            strMarkup.Append("<td>Qty</td>");
                            strMarkup.Append("</tr>");
                            strMarkup.Append("</thead>");
                            foreach (DataRow row in dtRx.Rows)
                            {
                                strMarkup.Append("<tr>");
                                strMarkup.Append("<td>" + row["ProductName"].ToString() + "(Note : " + row["PreItemNote"].ToString() + ")</td>");
                                strMarkup.Append("<td>" + row["PreItemDose1"].ToString() + "</td>");
                                strMarkup.Append("<td>" + row["PreItemDose2"].ToString() + "</td>");
                                strMarkup.Append("<td>" + row["PreItemDose3"].ToString() + "</td>");
                                if (row["PreItemDays"] != DBNull.Value && row["PreItemDays"] != null && row["PreItemDays"].ToString() != "")
                                {
                                    strMarkup.Append("<td>" + row["PreItemDays"].ToString() + "</td>");
                                }
                                else
                                {
                                    strMarkup.Append("<td>-</td>");
                                }
                                strMarkup.Append("<td>" + row["PreItemQty"].ToString() + "</td>");
                                strMarkup.Append("</tr>");
                            }
                            
                            strMarkup.Append("</table>");

                            strMarkup.Append("<span class=\"space80\"></span>");
                            //strMarkup.Append("<span class=\"fontRegular clrBlack\">Stamp &amp; Doctor's Signature</span>");
                            //strMarkup.Append("<span class=\"space10\"></span>");
                            strMarkup.Append("<div class=\"rxBottomLine\"></div>");
                            strMarkup.Append("<span class=\"space50\"></span>");
                            strMarkup.Append("<span class=\"space20\"></span>");
                            strMarkup.Append("<div class=\"txtRight\">");
                            //strMarkup.Append("<span class=\"fontRegular clrBlack\">Contact : " + docRow["DocMobileNum"].ToString() + "</span>");
                            strMarkup.Append("<span class=\"fontRegular clrBlack\">Stamp &amp; Signature</span>");
                            strMarkup.Append("</div>");
                            rxStr = strMarkup.ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            rxStr = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}