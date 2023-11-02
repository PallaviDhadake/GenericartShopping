using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class login : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg, regLink;
    protected void Page_Load(object sender, EventArgs e)
    {
        btnLogin.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnLogin, null) + ";");
        if (!IsPostBack)
        {
            txtUserId.Focus();

            if (Session["genericCust"] != null)
            {
                Response.Redirect(Master.rootPath + "customer/user-info", false);
            }

            if (Request.QueryString["ref"] != null)
            {
                switch (Request.QueryString["ref"])
                {
                    case "cart":
                        if (Request.QueryString["remind"] != null)
                        {
                            regLink = Master.rootPath + "registration?ref=cart&remind=" + Request.QueryString["remind"].ToString();
                        }
                        else
                        {
                            regLink = Master.rootPath + "registration?ref=cart"; 
                        }
                        break;
                    case "rx": regLink = Master.rootPath + "registration?ref=rx"; break;
                    case "calc": regLink = Master.rootPath + "registration?ref=calc"; break;
                }
            }
            else
            {
                regLink = Master.rootPath + "registration";
            }

            if (Request.QueryString["mob"] != null)
            {
                txtUserId.Text = Request.QueryString["mob"].ToString();
            }
        }
    }

    protected void UpdatePanel1_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "grecaptcha.render('recaptcha', {'sitekey': '6LcNBIUUAAAAADbX-_n6UhdJhtAxQDgiypcyZqSN'});", true);
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtUserId.Text == "" || txtPassword.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * Marked fields are Mandatory');", true);
                return;
            }

            
            //Removed pasword field on req from sujata mam
            //if (txtUserId.Text == "")
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * Marked fields are Mandatory');", true);
            //    return;
            //}


            string EncodedResponse = Request.Form["g-Recaptcha-Response"];
            bool IsCaptchaValid = (ReCaptchaClass.Validate(EncodedResponse) == "True" ? true : false);

            //if (!IsCaptchaValid)
            //{
            //    //InValid Request
            //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Captcha Verification Failed');", true);
            //    return;
            //}

            HttpCookie cartItemCookie = Request.Cookies["ordId"];
            int orderId = 0;
            if (cartItemCookie != null)
            {
                string[] arrOrd = cartItemCookie.Value.Split('#');
                orderId = Convert.ToInt32(arrOrd[0].ToString());
            }

            bool isnum = System.Text.RegularExpressions.Regex.IsMatch(txtUserId.Text, "[ ^ 0-9]");

            if (isnum)
            {
                if (c.IsRecordExist("Select CustomrtID From CustomersData Where CustomerMobile='" + txtUserId.Text + "' AND delMark=1"))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Sorry..! This account is deleted');", true);
                    return;
                }
                if (!c.IsRecordExist("Select CustomrtID From CustomersData Where CustomerMobile='" + txtUserId.Text + "'"))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Invalid Mobile Number Entered');", true);
                    return;
                }

                else if (c.GetReqData("CustomersData", "CustomerPassword", "CustomerMobile='" + txtUserId.Text + "'").ToString() != txtPassword.Text)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Invalid Password Entered');", true);
                    return;
                }

                else
                {
                    Session["genericCust"] = c.GetReqData("CustomersData", "CustomrtID", "CustomerMobile='" + txtUserId.Text + "'");
                    Response.Redirect(Master.rootPath + "customer/user-info", false);
                }
            }
            else
            {
                if (c.IsRecordExist("Select CustomrtID From CustomersData Where CustomerEmail='" + txtUserId.Text + "' AND delMark=1"))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Sorry..! This account is deleted');", true);
                    return;
                }
                if (!c.IsRecordExist("Select CustomrtID From CustomersData Where CustomerEmail='" + txtUserId.Text + "'"))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Invalid Email Address Entered');", true);
                    return;
                }
                //else if (c.GetReqData("CustomersData", "CustomerPassword", "CustomerEmail='" + txtUserId.Text + "'").ToString() != txtPassword.Text)
                //{
                //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Invalid Password Entered');", true);
                //    return;
                //}
                else
                {
                    Session["genericCust"] = c.GetReqData("CustomersData", "CustomrtID", "CustomerEmail='" + txtUserId.Text + "'");
                    Response.Redirect(Master.rootPath + "customer/user-info", false);
                }
            }

            if (orderId > 0)
            {
                string newordCustId = orderId.ToString() + "#" + Session["genericCust"].ToString();
                Response.Cookies["ordId"].Value = newordCustId;
                Response.Cookies["ordId"].Expires = DateTime.Now.AddDays(30);

                c.ExecuteQuery("Update OrdersData Set FK_OrderCustomerID=" + Session["genericCust"] + " Where OrderID=" + orderId);
            }

            if (Request.QueryString["ref"] != null)
            {
                switch (Request.QueryString["ref"])
                {
                    case "cart":
                        if (Request.QueryString["remind"] != null)
                        {
                            string remindIds = "";
                            if (Request.QueryString["remind"].ToString().Contains('-'))
                            {
                                remindIds = Request.QueryString["remind"].ToString().Replace('-', ',');
                            }
                            else
                            {
                                remindIds = Request.QueryString["remind"].ToString();
                            }
                            c.ExecuteQuery("Update PillReminder Set FK_CustomerID=" + Session["genericCust"] + " Where RemindID IN (" + remindIds + ")");
                        }
                        Response.Redirect(Master.rootPath + "checkout", false); 
                        break;
                    case "rx": Response.Redirect(Master.rootPath + "upload-prescription", false); break;
                    case "calc":
                        string custMob = c.GetReqData("CustomersData", "CustomerMobile", "CustomrtID=" + Session["genericCust"]).ToString();
                        int calcID = Convert.ToInt32(c.GetReqData("SavingCalc", "CalcID", "MobileNumber='" + custMob + "' AND EnqStatus=0 AND (CONVERT(varchar(20), CAST (CalcDate AS DATE), 112) = CONVERT(varchar(20), CAST ('" + DateTime.Now + "' AS DATE), 112))"));
                        c.ExecuteQuery("Update SavingCalc Set FK_CustId=" + Session["genericCust"].ToString() + " Where CalcID=" + calcID);
                        Response.Redirect(Master.rootPath + "enquiry-checkout", false); 
                        break;
                    case "favShop":
                        string shopId = Request.QueryString["shop"].ToString();
                        c.ExecuteQuery("Update CustomersData Set CustomerFavShop=" + shopId + " Where CustomrtID=" + Session["genericCust"]);
                        Response.Redirect(Master.rootPath + "nearest-shops?shop=" + Request.QueryString["shop"].ToString(), false);
                        break;
                }
            }
            else
            {
                Response.Redirect(Master.rootPath + "customer/user-info", false);
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}