using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class doctors_profile : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg, docStr;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Page.RouteData.Values["docId"].ToString()))
                {
                    string[] arrDoc = Page.RouteData.Values["docId"].ToString().Split('-');
                    GetDoctor(Convert.ToInt32(arrDoc[arrDoc.Length - 1]));
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


    private void GetDoctor(int docIdx)
    {
        try
        {
            using (DataTable dtDoctor = c.GetDataTable("Select a.DoctorID, a.DocName, a.DocPhoto, a.DocDegree, a.DocExperience, a.DocAbout, a.ConsultationFees, b.SpecialtyName From DoctorsData a Inner Join DoctorSpecialtyData b On b.SpecialtyID = a.FK_DocSpecialtyID Where DoctorID=" + docIdx + "And a.DelMark=0"))
            {
                if (dtDoctor.Rows.Count > 0)
                {
                    DataRow row = dtDoctor.Rows[0];
                    StringBuilder strMarkup = new StringBuilder();

                    //Doctor information Markup
                    strMarkup.Append("<div class=\"bgWhite border_r_5 box-shadow\">");
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
                    strMarkup.Append("<h1 class=\"docName semiBold mrg_B_3\">" + row["DocName"].ToString() + "</h1>");
                    strMarkup.Append("<span class=\"themeClrPrime semiBold tiny\">" + row["SpecialtyName"].ToString() + "</span><br/>");
                    if (row["DocExperience"] != DBNull.Value && row["DocExperience"] != null && row["DocExperience"].ToString() != "0")
                    {
                        strMarkup.Append("<span class=\"clrGrey semiBold tiny\">" + row["DocExperience"].ToString() + " Years Experience</span><br/>");
                    }
                    else
                    {
                        strMarkup.Append("<span class=\"space20\"></span>");
                    }
                    strMarkup.Append("</div>");
                    strMarkup.Append("</div>");
                    strMarkup.Append("<div class=\"float_clear\"></div>");
                    strMarkup.Append("<div class=\"pad_15\">");
                    strMarkup.Append("<h2 class=\"themeClrPrime semiMedium semiBold mrg_B_5\">Qualification</h2>");
                    strMarkup.Append("<p class=\"clrGrey line-ht-5 small\">" + row["DocDegree"].ToString() + "</p>");
                    strMarkup.Append("<span class=\"space20\"></span>");
                    strMarkup.Append("<h2 class=\"themeClrPrime semiMedium semiBold mrg_B_5\">About " + row["DocName"].ToString() + "</h2>");
                    strMarkup.Append("<p class=\"clrGrey line-ht-5 small\">" + row["DocAbout"].ToString() + "</p>");
                    strMarkup.Append("<span class=\"space20\"></span>");
                    string bookUrl = Master.rootPath + "book-appointment/" + c.UrlGenerator(row["DocName"].ToString().ToLower()) + "-" + row["DoctorID"].ToString() + "";
                    strMarkup.Append("<a href=\"" + bookUrl + "\" class=\"blueAnch dspBlk small upperCase letter-sp-2 txtCenter semiBold \">Book Appointment</a>");

                    strMarkup.Append("</div>");
                    strMarkup.Append("</div>");
                    docStr = strMarkup.ToString();
                }
            }
        }
        catch (Exception ex)
        {

            docStr = c.ErrNotification(3, ex.Message.ToString());
            return;
        }

       
    }
}