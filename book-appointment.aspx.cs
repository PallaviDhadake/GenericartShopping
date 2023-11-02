using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class book_appointment : System.Web.UI.Page
{
    iClass c = new iClass();
    public string docStr, consultStr, about, errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                //if (!String.IsNullOrEmpty(Page.RouteData.Values["doctorId"].ToString()))
                //{
                //    string[] arrCat = Page.RouteData.Values["doctorId"].ToString().Split('-');
                //    GetDoctor(Convert.ToInt32(arrCat[arrCat.Length - 1]));
                //}
                //else
                //{
                //    Response.Redirect(Master.rootPath, false);
                //}

                if (Request.QueryString["code"] != null)
                {
                    txtShopCode.Text = Request.QueryString["code"];
                }
            }
        }
        catch (Exception ex)
        {

           errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }

    }

    private void GetDoctor(int doctorIdx)
    {
        try
        {
            using (DataTable dtDoctor = c.GetDataTable("Select a.DoctorID, a.DocName, a.DocPhoto, a.DocExperience, a.DocAbout, a.ConsultationFees, b.SpecialtyName From DoctorsData a Inner Join DoctorSpecialtyData b On b.SpecialtyID = a.FK_DocSpecialtyID Where DoctorID=" + doctorIdx + "And a.DelMark=0"))
            {
                if (dtDoctor.Rows.Count > 0)
                {
                    DataRow row = dtDoctor.Rows[0];
                    StringBuilder strMarkup = new StringBuilder();
                    StringBuilder strConsult = new StringBuilder();

                    //Doctor information Markup
                    strMarkup.Append("<div class=\"bgWhite border_r_5 box-shadow\">");
                    strMarkup.Append("<div class=\"width50\">");
                    strMarkup.Append("<div class=\"pad_15\">");
                    if (row["DocPhoto"] != DBNull.Value && row["DocPhoto"] != null && row["DocPhoto"].ToString() != "")
                    {
                        strMarkup.Append("<img src=\"" + Master.rootPath + "upload/doctors/" + row["DocPhoto"].ToString() + "\" alt=\"" + row["DocName"].ToString() + "\" class=\"width100\" />");
                    }
                    else
                    {
                        strMarkup.Append("<img src=\"" + Master.rootPath + "upload/doctors/no-photo.png\" alt=\"" + row["DocName"].ToString() + "\" class=\"width100\" />");
                    }

                    strMarkup.Append("</div>");
                    strMarkup.Append("</div>");

                    strMarkup.Append("<div class=\"width50\">");
                    strMarkup.Append("<div class=\"pad_TB_15\">");
                    strMarkup.Append("<h1 class=\"docName semiBold\">" + row["DocName"].ToString() + "</h1>");
                    strMarkup.Append("<span class=\"themeClrPrime semiBold tiny\">" + row["SpecialtyName"].ToString() + "</span><br/>");

                    if (row["DocExperience"] != DBNull.Value && row["DocExperience"] != null && row["DocExperience"].ToString() != "0")
                    {
                        strMarkup.Append("<span class=\"clrGrey semiBold tiny\">" + row["DocExperience"].ToString() + " Years Experience</span><br/>");
                    }
                    else
                    {
                        strMarkup.Append("<span class=\"space20\"></span>");
                    }

                    strMarkup.Append("<span class=\"space5\"></span>");

                    if (row["DocAbout"] != DBNull.Value && row["DocAbout"] != null && row["DocAbout"].ToString() != "")
                    {
                        string temp = row["DocAbout"].ToString();

                        if (temp.Length > 43)
                        {
                            about = string.Concat(temp.Substring(0, 41), "...");
                            strMarkup.Append("<p class=\"clrGrey tiny line-ht-5 fontRegular\">" + about + "</p>");
                        }
                        else
                        {
                            about = temp;
                            strMarkup.Append("<p class=\"clrGrey tiny line-ht-5 fontRegular\">" + about + "</p>");
                        }
                    }
                    strMarkup.Append("<span class=\"space5\"></span>");
                    string docUrl = Master.rootPath + "doctors-profile/" + c.UrlGenerator(row["DocName"].ToString().ToLower()) + "-" + row["DoctorID"].ToString() + "";
                    strMarkup.Append("<a href=\"" + docUrl + "\" class=\"readMore\" style=\"font-size:0.8em; font-weight:600;\">Check Profile</a>");
                    strMarkup.Append("</div>");
                    strMarkup.Append("</div>");
                    strMarkup.Append("<div class=\"float_clear\"></div>");
                    strMarkup.Append("</div>");
                    docStr = strMarkup.ToString();


                    //Appointment Summary Markup
                    strConsult.Append("<div class=\"bgWhite border_r_5 box-shadow\">");
                    strConsult.Append("<div class=\"pad_15\">");
                    strConsult.Append("<h2 class=\" clrLightBlack semiBold \">Appointment Summary</h2>");
                    strConsult.Append("<span class=\"lineSeperator\"></span>");
                    if (row["ConsultationFees"] != DBNull.Value && row["ConsultationFees"] != null && row["ConsultationFees"].ToString() != "")
                    {
                        strConsult.Append("<span class=\"semiBold clrLightBlack small float_left\">Consultation Fees : </span> <span class=\"fontRegular small float_right\">&#8377;" + row["ConsultationFees"] + "</span>");
                        strConsult.Append("<div class=\"float_clear\"></div>");
                        strConsult.Append("<span class=\"lineSeperator\"></span>");
                        strConsult.Append("<span class=\"semiBold clrLightBlack small float_left\">Total Value : </span><span class=\"fontRegular small float_right\">&#8377;" + row["ConsultationFees"] + "</span>");
                    }
                    else
                    {
                        strConsult.Append("<span class=\"semiBold clrLightBlack small float_left\">Consultation Fees : </span> <span class=\"fontRegular small float_right\">NA</span>");
                        strConsult.Append("<div class=\"float_clear\"></div>");
                        strConsult.Append("<span class=\"lineSeperator\"></span>");
                        strConsult.Append("<span class=\"semiBold clrLightBlack small float_left\">Total Value : </span><span class=\"fontRegular small float_right\">NA</span>");
                        //strMarkup.Append("<span class=\"space20\"></span>");
                    }


                    //strConsult.Append("<span class=\"semiBold clrLightBlack small float_left\">Consultation Fees : </span> <span class=\"fontRegular small float_right\">&#8377;" + row["ConsultationFees"] + "</span>");
                    //strConsult.Append("<div class=\"float_clear\"></div>");
                    //strConsult.Append("<span class=\"lineSeperator\"></span>");
                    //strConsult.Append("<span class=\"semiBold clrLightBlack small float_left\">Total Value : </span><span class=\"fontRegular small float_right\">&#8377;" + row["ConsultationFees"] + "</span>");

                    strConsult.Append("<div class=\"float_clear\"></div>");
                    strConsult.Append("</div>");
                    strConsult.Append("</div>");
                    consultStr = strConsult.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
       
    }



    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            txtName.Text = txtName.Text.Trim().Replace("'", "");
            txtEmail.Text = txtEmail.Text.Trim().Replace("'", "");
            txtMobile.Text = txtMobile.Text.Trim().Replace("'", "");
            txtAge.Text = txtAge.Text.Trim().Replace("'", "");
            txtAddress.Text = txtAddress.Text.Trim().Replace("'", "");
            txtPinCode.Text = txtPinCode.Text.Trim().Replace("'", "");
            txtProblem.Text = txtProblem.Text.Trim().Replace("'", "");
            txtDate.Text = txtDate.Text.Trim().Replace("'", "");
            txtDocName.Text = txtDocName.Text.Trim().Replace("'", "");

            txtShopCode.Text = txtShopCode.Text.Trim().Replace("'", "");

            //string[] arrCat = Page.RouteData.Values["doctorId"].ToString().Split('-');
            //int docId = Convert.ToInt32(arrCat[arrCat.Length - 1]);
            //GetDoctor(docId);
            int docId = 0;

            if (txtName.Text == "" || txtEmail.Text == "" || txtMobile.Text == "" || txtAge.Text == "" || txtAddress.Text == "" || txtPinCode.Text == "" || txtProblem.Text == "" || txtDate.Text == "" || txtDocName.Text == "" || ddrGender.SelectedIndex == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * Marked fields are Mandatory');", true);
                return;
            }

            if (!c.IsNumeric(txtAge.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Age must be numeric value');", true);
                return;
            }

            if (!c.IsNumeric(txtPinCode.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Pincode must be numeric value');", true);
                return;
            }

            if (!c.EmailAddressCheck(txtEmail.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid Email Address');", true);
                return;
            }

            if (!c.ValidateMobile(txtMobile.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid mobile number');", true);
                return;
            }

            if (txtAddress.Text.Length > 200)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Address must be less than 200 characters');", true);
                return;
            }

            if (txtShopCode.Text != "")
            {
                if (!c.IsRecordExist("Select FranchID From FranchiseeData Where FranchShopCode='" + txtShopCode.Text + "' AND FranchActive=1"))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Invalid Shop Code entered');", true);
                    return;
                }
            }

            DateTime appDate = DateTime.Now;
            string[] arrTDate = txtDate.Text.Split('/');
            if (c.IsDate(arrTDate[1] + "/" + arrTDate[0] + "/" + arrTDate[2]) == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid Appointment Date');", true);
                return;
            }
            else
            {
                appDate = Convert.ToDateTime(arrTDate[1] + "/" + arrTDate[0] + "/" + arrTDate[2]);
            }

            int apptype = 0;
            if (rdbMyself.Checked == true)
                apptype = 1;
            else if (rdbFamily.Checked == true)
                apptype = 2;
                

            //string docName = c.GetReqData("DoctorsData", "DocName", "DoctorID=" + docId).ToString();

            //if (c.IsRecordExist("Select DocAppID From DoctorsAppointmentData Where DocAppMobile='" + txtMobile.Text + "' AND FK_DocID=" + docId + " AND DocAppType=" + apptype + " AND ( CONVERT(varchar(20), CAST (DocAppDate AS DATE), 112) = CONVERT(varchar(20), CAST ('" + appDate + "' AS DATE), 112) )")) 
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Appointment on " + txtDate.Text + " is already booked by " + txtMobile.Text + " number with Dr. " + docName + "');", true);
            //    return;
            //}
            if (c.IsRecordExist("Select DocAppID From DoctorsAppointmentData Where DocAppMobile='" + txtMobile.Text + "' AND DocAppType=" + apptype + " AND ( CONVERT(varchar(20), CAST (DocAppDate AS DATE), 112) = CONVERT(varchar(20), CAST ('" + appDate + "' AS DATE), 112) )"))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Appointment on " + txtDate.Text + " is already booked by " + txtMobile.Text + " number ');", true);
                return;
            }

            int maxId = c.NextId("DoctorsAppointmentData", "DocAppID");

            c.ExecuteQuery("Insert Into DoctorsAppointmentData (DocAppID, DocAppDate, DocAppName, DocAppEmail, DocAppMobile, DocAppAge, DocAppGender, " +
                " DocAppDesc, DocAppAddress, DocAppPincode, FK_DocID, PrevDocName, DocAppType, DocAppStatus, AppSubmitDate, DeviceType, DocRefShopCode) Values (" + maxId + ", '" + appDate + "', '" + txtName.Text + "', '" + txtEmail.Text +
                "', '" + txtMobile.Text + "', " + txtAge.Text + ", " + ddrGender.SelectedValue + ", '" + txtProblem.Text + "', '" + txtAddress.Text + "', '" + txtPinCode.Text + "', " + docId +
                ", '" + txtDocName.Text + "', " + apptype + ", 0, '" + DateTime.Now + "', 'Web', '" + txtShopCode.Text + "')");

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Doctors appointment booked successfully..!!');", true);

            txtName.Text = txtAge.Text = txtMobile.Text = txtAddress.Text = txtPinCode.Text = txtEmail.Text = txtDate.Text = txtProblem.Text = txtDocName.Text = "";
            ddrGender.SelectedIndex = 0;

        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}