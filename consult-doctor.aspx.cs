using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class consult_doctor : System.Web.UI.Page
{
    iClass c = new iClass();
    public string splStr, topDocStr, errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                GetSpeciality();
                GetTopDoctors();

            }
        }
        catch (Exception ex)
        {

            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
     
        
    }

    private void GetSpeciality()
    {
        try
        {
            using (DataTable dtSpeciality = c.GetDataTable("Select SpecialtyID, SpecialtyName From DoctorSpecialtyData Where delMark=0 Order By SpecialtyName"))
            {
                if (dtSpeciality.Rows.Count > 0)
                {
                    StringBuilder strMarkup = new StringBuilder();

                    foreach (DataRow row in dtSpeciality.Rows)
                    {
                        strMarkup.Append("<div class=\"slide\">");
                        strMarkup.Append("<div class=\"prodContainer txtCenter\">");
                        strMarkup.Append("<div class=\"pad_15\">");
                        strMarkup.Append("<img src=\"" + Master.rootPath + "upload/diseases/no-photo.png\" />" );
                        string splUrl = Master.rootPath + "doctors/" + c.UrlGenerator(row["SpecialtyName"].ToString().ToLower()) + "-" + row["SpecialtyID"].ToString() + "";
                        strMarkup.Append("<a href=\"" + splUrl + "\" class=\"prodName semiBold \">" + row["SpecialtyName"].ToString() + "</a>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                    }
                    splStr = strMarkup.ToString();
                }
            }
        }
        catch (Exception ex)
        {

            splStr = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    private void GetTopDoctors()
    {
        try
        {
            using (DataTable dtTopDoctors = c.GetDataTable("Select a.DoctorID, a.DocName, a.DocPhoto, a.DocExperience, a.ConsultationFees,  b.SpecialtyName From DoctorsData a Inner Join DoctorSpecialtyData b On b.SpecialtyID = a.FK_DocSpecialtyID Where DocFeatured = 1 And a.DelMark=0 Order By a.DoctorID DESC"))
            {
                if (dtTopDoctors.Rows.Count > 0)
                {
                    StringBuilder strMarkup = new StringBuilder();

                    foreach (DataRow row in dtTopDoctors.Rows)
                    {
                        strMarkup.Append("<div class=\"slide\">");
                        strMarkup.Append("<div class=\"docBox box-shadow\">");
                        strMarkup.Append("<div class=\"docImg\"> <div>");
                        if (row["DocPhoto"] != DBNull.Value && row["DocPhoto"] != null && row["DocPhoto"].ToString() != "")
                        {
                            strMarkup.Append("<img src=\"" + Master.rootPath + "upload/doctors/" + row["DocPhoto"].ToString() + "\" alt=\"" + row["DocName"].ToString() + "\" class=\"width100\" />");
                        }
                        else
                        {
                            strMarkup.Append("<img src=\"" + Master.rootPath + "upload/doctors/no-photo.png\" alt=\"" + row["DocName"].ToString() + "\" class=\"width100\" />");
                        }
                        strMarkup.Append("</div></div>");

                        strMarkup.Append("<div class=\"docInfo\">");
                        strMarkup.Append("<div class=\"pad_10\">");
                        string docUrl = Master.rootPath + "doctors-profile/" + c.UrlGenerator(row["DocName"].ToString().ToLower()) + "-" + row["DoctorID"].ToString() + "";
                        strMarkup.Append("<a href=\"" + docUrl + "\" class=\"docName semiBold mrg_B_3 \">" + row["DocName"].ToString() + "</a>");

                        strMarkup.Append("<span class=\"clrGrey semiBold tiny\">" + row["SpecialtyName"].ToString() + "</span><br/>");

                        if(row["DocExperience"] != DBNull.Value && row["DocExperience"] != null && row["DocExperience"].ToString() != "0")
                        {
                            strMarkup.Append("<span class=\"clrGrey semiBold tiny\">" + row["DocExperience"].ToString() + " Years Experience</span><br/>");
                        }
                        else
                        {
                            strMarkup.Append("<span class=\"space20\"></span>");
                        }
                        

                        strMarkup.Append("<span class=\"space10\"></span>");

                        if(row["ConsultationFees"] !=DBNull.Value && row["ConsultationFees"] != null && row["ConsultationFees"].ToString() != "")
                        {
                            strMarkup.Append("<span class=\"prod-offer-price\">&#8377;" + row["ConsultationFees"].ToString() + "</span>");
                        }
                        else
                        {
                            strMarkup.Append("<span class=\"prod-offer-price\">NA</span>");
                            //strMarkup.Append("<span class=\"space20\"></span>");
                        }


                        strMarkup.Append("<span class=\"space5\"></span>");
                        strMarkup.Append("</div>");

                        string consultUrl = Master.rootPath + "book-appointment/" + c.UrlGenerator(row["DocName"].ToString().ToLower()) + "-" + row["DoctorID"].ToString() + "";
                        strMarkup.Append("<a href=\"" + consultUrl + "\" class=\"docAnch semiBold upperCase \">Consult</a>");
                        strMarkup.Append("</div>");

                        strMarkup.Append("<div class=\"float_clear\"></div>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                    }
                    topDocStr = strMarkup.ToString();
                }
            }
        }
        catch (Exception ex)
        {

            topDocStr = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }


}