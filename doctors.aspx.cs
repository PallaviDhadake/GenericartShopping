using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class doctors : System.Web.UI.Page
{
    iClass c = new iClass();
    public string  errMsg, docStr;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Page.RouteData.Values["specId"].ToString()))
                {
                    string[] arrCat = Page.RouteData.Values["specId"].ToString().Split('-');
                    GetDoctors(Convert.ToInt32(arrCat[arrCat.Length - 1]));
                }
                else
                {
                    Response.Redirect(Master.rootPath, false);
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    private void GetDoctors(int specIdx)
    {
        try
        {
            using (DataTable dtDoctors = c.GetDataTable("Select a.DoctorID, a.DocName, a.DocPhoto, a.DocExperience, a.ConsultationFees, b.SpecialtyName From DoctorsData a Inner Join DoctorSpecialtyData b On b.SpecialtyID = a.FK_DocSpecialtyID Where SpecialtyID=" + specIdx + "And a.DelMark=0 Order By DocName "))
            {

                string speciality = c.GetReqData("DoctorSpecialtyData", "SpecialtyName", "SpecialtyID=" + specIdx + "").ToString();
                StringBuilder strMarkup = new StringBuilder();
                strMarkup.Append("<h1 class=\"clrLightBlack semiBold pageH2 mrg_B_10\">" + speciality + "</h1>");

                if (dtDoctors.Rows.Count > 0)
                {
                    int boxCount = 0;
                    foreach (DataRow row in dtDoctors.Rows)
                    {
                        strMarkup.Append("<div class=\"col_1_3\">");
                        strMarkup.Append("<div class=\"pad_10\">");
                        strMarkup.Append("<div class=\"docBox box-shadow\">");
                        strMarkup.Append("<div class=\"width50\">");
                        strMarkup.Append("<div class=\"pad_15\">");
                        if (row["DocPhoto"] != DBNull.Value && row["DocPhoto"] != null && row["DocPhoto"].ToString() != "")
                        {
                            strMarkup.Append("<img src=\"" + Master.rootPath + "upload/doctors/" + row["DocPhoto"].ToString() + "\" alt=\"" + row["DocName"].ToString() + "\" class=\"width100 border_r_5\" />");
                        }
                        else
                        {
                            strMarkup.Append("<img src=\"" + Master.rootPath + "upload/doctors/no-photo.png\" alt=\"" + row["DocName"].ToString() + "\" class=\"width100 border_r_5\" />");
                        }
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");

                        strMarkup.Append("<div class=\"width50\">");
                        strMarkup.Append("<div class=\"pad_15\">");
                        string docUrl = Master.rootPath + "doctors-profile/" + c.UrlGenerator(row["DocName"].ToString().ToLower()) + "-" + row["DoctorID"].ToString() + "";
                        strMarkup.Append("<a href=\"" + docUrl + "\" class=\"docName semiBold mrg_B_3 \">" + row["DocName"].ToString() + "</a>");
                        strMarkup.Append("<span class=\"clrGrey semiBold tiny\">" + row["SpecialtyName"].ToString() + "</span><br/>");
                        if (row["DocExperience"] != DBNull.Value && row["DocExperience"] != null && row["DocExperience"].ToString() != "0")
                        {
                            strMarkup.Append("<span class=\"clrGrey semiBold tiny\">" + row["DocExperience"].ToString() + " Years Experience</span>");
                        }
                        else
                        {
                            strMarkup.Append("<span class=\"space20\"></span>");
                        }
                        strMarkup.Append("<span class=\"space10\"></span>");

                        if (row["ConsultationFees"] != DBNull.Value && row["ConsultationFees"] != null && row["ConsultationFees"].ToString() != "")
                        {
                            strMarkup.Append("<span class=\"prod-offer-price\">&#8377;" + row["ConsultationFees"].ToString() + "</span>");
                        }
                        else
                        {
                            strMarkup.Append("<span class=\"prod-offer-price\">NA</span>");
                            //strMarkup.Append("<span class=\"space15\"></span>");
                        }



                        //strMarkup.Append("<span class=\"prod-offer-price\">&#8377;" + row["ConsultationFees"] +"</span>");
                        strMarkup.Append("<span class=\"space5\"></span>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");

                        strMarkup.Append("<div class=\"float_clear\"></div>");
                        strMarkup.Append("<div class=\"width50 txtCenter\">");
                        strMarkup.Append("<a href=\"" + docUrl + "\" class=\"docProfileAnch semiBold upperCase \">Profile</a>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("<div class=\"width50 txtCenter\">");
                        string consultUrl = Master.rootPath + "book-appointment/" + c.UrlGenerator(row["DocName"].ToString().ToLower()) + "-" + row["DoctorID"].ToString() + "";
                        strMarkup.Append("<a href=\"" + consultUrl + "\" class=\"docAnch semiBold upperCase \">Consult</a>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("<div class=\"float_clear\"></div>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");

                        boxCount++;
                        if ((boxCount % 3) == 0)
                        {
                            strMarkup.Append("<div class=\"float_clear\"></div>");
                        }

                    }
                    strMarkup.Append("<div class=\"float_clear\"></div>");


                }
                else
                {
                    strMarkup.Append("<div class=\"themeBgPrime\"><div class=\"pad_10\"><span class=\"clrWhite fontRegular\">No any doctors to display</span></div></div>");
                }
                docStr = strMarkup.ToString();
            }
        }
        catch (Exception ex)
        {

            docStr = c.ErrNotification(3, ex.Message.ToString());
            return;
        }

       
    }

}