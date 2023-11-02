using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class registration : System.Web.UI.Page
{
    public string proceedMsg, verifyMsg, errMsg, loginUrl;
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        btnProceed.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnProceed, null) + ";");
        btnVerify.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnVerify, null) + ";");
        btnResend.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnResend, null) + ";");
        btnCancel.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnCancel, null) + ";");

        if (!IsPostBack)
        {
            proceedData.Visible = true;

            if (Request.QueryString["ref"] != null)
            {
                if (Request.QueryString["ref"] == "calc")
                {
                    if (Session["calcMob"] != null)
                    {
                        txtMobile.Text = Session["calcMob"].ToString();
                        txtMobile.Enabled = false;
                    }
                }
                else if (Request.QueryString["ref"] == "cart")
                {
                    loginUrl = Master.rootPath + "login?ref=cart";
                }
            }
            else
            {
                loginUrl = Master.rootPath + "login";
            }

            if (Session["userMob"] == null || Session["verifyCode"] == null)
            {
                proceedData.Visible = true;
            }
            else
            {
                if (String.IsNullOrEmpty(Page.RouteData.Values["regId"].ToString()))
                {

                }
                else
                {
                    string regId = Page.RouteData.Values["regId"].ToString();
                    if (regId == "verify")
                    {
                        otpVerify.Visible = true;
                        proceedData.Visible = false;
                    }
                }
            }

            //if (Session["genericCust"] != null)
            //{
            //    Response.Redirect(Master.rootPath + "customer/user-info", false);
            //}
        }
    }

    protected void btnProceed_Click(object sender, EventArgs e)
    {
        try
        {
            txtName.Text = txtName.Text.Trim().Replace("'", "");
            txtMobile.Text = txtMobile.Text.Trim().Replace("'", "");
            //txtEmail.Text = txtEmail.Text.Trim().Replace("'", "");
            //txtCountry.Text = txtCountry.Text.Trim().Replace("'", "");
            //txtState.Text = txtState.Text.Trim().Replace("'", "");
            //txtCity.Text = txtCity.Text.Trim().Replace("'", "");
            //txtPinCode.Text = txtPinCode.Text.Trim().Replace("'", "");
            //txtAddress1.Text = txtAddress1.Text.Trim().Replace("'", "");
            //txtPassword.Text = txtPassword.Text.Trim().Replace("'", "");

            string finalOtp = new Random().Next(1000, 9999).ToString();

            if (txtName.Text == "" || txtMobile.Text == "")
            {
                proceedMsg = c.ErrNotification(2, "All * fields are compulsory");
                return;
            }
            else if (c.ValidateMobile(txtMobile.Text) == false)
            {
                proceedMsg = c.ErrNotification(2, "Enter Valid Mobile No");
                return;
            }
            //else if (c.EmailAddressCheck(txtEmail.Text) == false)
            //{
            //    proceedMsg = c.ErrNotification(2, "Enter Valid Email Address");
            //    return;
            //}
            // To check if mob already registered
            else if (c.IsRecordExist("Select CustomrtID From CustomersData Where CustomerMobile='" + txtMobile.Text + "' AND delMark=0"))
            {
                proceedMsg = c.ErrNotification(2, "This Mobile No is already registered with us, you can directly <a href=\"" + Master.rootPath + "login\" class=\"forgetLink\">login</a> by using Mobile No.");
                //return;
                string lUrl = "";
                if (Request.QueryString["ref"] != null)
                {
                    lUrl = Master.rootPath + "login?ref=" + Request.QueryString["ref"] + "&mob=" + txtMobile.Text + "";
                }
                else
                {
                    lUrl = Master.rootPath + "login?mob=" + txtMobile.Text + "";
                }
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('" + lUrl + "', 1000);", true);
                return;
            }
            //else if (c.IsRecordExist("Select CustomrtID From CustomersData Where CustomerEmail='" + txtEmail.Text + "' AND delMark=0"))
            //{
            //    proceedMsg = c.ErrNotification(2, "This Email Id is already registered with us, you can directly <a href=\"" + Master.rootPath + "login\" class=\"forgetLink\">login</a> by using Mobile No.");
            //    return;
            //}
            else
            {
                Session["userMob"] = txtMobile.Text;
                Session["verifyCode"] = finalOtp;
                //Session["otherInfo"] = txtName.Text + "#" + txtMobile.Text + "#" + txtEmail.Text + "#" + txtPassword.Text;
                Session["otherInfo"] = txtName.Text + "#" + txtMobile.Text;
            }

            string msgData = "Dear " + txtName.Text + ", Your Verification Code is " + Session["verifyCode"].ToString() + " Genericart Medicine - Wahi Kaam, Sahi Daam";
            c.SendSMS(msgData, Session["userMob"].ToString());

            proceedData.Visible = false;

            if (Request.QueryString["ref"] != null)
            {
                switch (Request.QueryString["ref"])
                {
                    case "cart":
                        if (Request.QueryString["remind"] != null)
                        {
                            Response.Redirect(Master.rootPath + "registration/verify?ref=cart&remind=" + Request.QueryString["remind"], false); 
                        }
                        else
                        {
                            Response.Redirect(Master.rootPath + "registration/verify?ref=cart", false); 
                        }
                        break;
                    case "rx": Response.Redirect(Master.rootPath + "registration/verify?ref=rx", false); break;
                    case "calc": Response.Redirect(Master.rootPath + "registration/verify?ref=calc", false); break;
                }
            }
            else
            {
                Response.Redirect(Master.rootPath + "registration/verify", false);
            }

            //DateTime cDate = DateTime.Now;
            //string currentDate = cDate.ToString("yyyy-MM-dd HH:mm:ss.fff");

            //HttpCookie cartItemCookie = Request.Cookies["ordId"];
            //int orderId = 0;
            //if (cartItemCookie != null)
            //{
            //    string[] arrOrd = cartItemCookie.Value.Split('#');
            //    orderId = Convert.ToInt32(arrOrd[0].ToString());
            //}

            // Insert basic user details in db
            //int maxId = c.NextId("CustomersData", "CustomrtID");

            //c.ExecuteQuery("Insert Into CustomersData (CustomrtID, CustomerJoinDate, CustomerName, CustomerMobile, CustomerEmail " +
            //    ", CustomerPassword, CustomerAddress, CustomerCity, CustomerState, CustomerPincode, CustomerCountry, MobileVerify, " +
            //    " EmailVerify, CustomerActive, delMark) Values (" + maxId + ", '" + currentDate + "', '" + txtName.Text +
            //    "', '" + txtMobile.Text + "', '" + txtEmail.Text + "', '" + txtPassword.Text + "', '" + txtAddress1.Text +
            //    "', '" + txtCity.Text + "', '" + txtState.Text + "', '" + txtPinCode.Text + "', '" + txtCountry.Text +
            //    "', 1, 1, 1, 0)");

            //c.ExecuteQuery("Insert Into CustomersData (CustomrtID, CustomerJoinDate, CustomerName, CustomerMobile, CustomerEmail " +
            //    ", CustomerPassword, MobileVerify, EmailVerify, CustomerActive, delMark) Values (" + maxId + ", '" + currentDate +
            //    "', '" + txtName.Text + "', '" + txtMobile.Text + "', '" + txtEmail.Text + "', '" + txtPassword.Text +
            //    "', 1, 1, 1, 0)");

            //Session["genericCust"] = maxId.ToString();


            //if (orderId > 0)
            //{
            //    string newordCustId = orderId.ToString() + "#" + Session["genericCust"].ToString();
            //    Response.Cookies["ordId"].Value = newordCustId;
            //    Response.Cookies["ordId"].Expires = DateTime.Now.AddDays(30);

            //    c.ExecuteQuery("Update OrdersData Set FK_OrderCustomerID=" + Session["genericCust"] + " Where OrderID=" + orderId);

            //}

            //if (Request.QueryString["ref"] != null)
            //{
            //    switch (Request.QueryString["ref"])
            //    {
            //        case "cart": Response.Redirect(Master.rootPath + "checkout", false); break;
            //        case "rx": Response.Redirect(Master.rootPath + "upload-prescription", false); break;
            //    }
            //}
            //else
            //{
            //    Response.Redirect(Master.rootPath + "customer/user-info", false);
            //}
            //else
            //{
            //    Session["userEmail"] = txtEmail.Text;
            //    Session["verifyCode"] = finalOtp;
            //    Session["otherInfo"] = txtName.Text + "#" + txtMobile.Text + "#" + txtCountry.Text + "#" + txtState.Text + "#" + txtCity.Text + "#" + txtPinCode.Text + "#" + txtAddress1.Text + "#" + txtPassword.Text;
            //}

            //string msgData = "Your Verification Code is " + Session["verifyCode"].ToString() + ", <br/> - Genericart Online Medicine Shopping";
            //c.SendMail("info@intellect-systems.com", "Genericart Medicine", Session["userEmail"].ToString(), msgData, "Verification Code", "", true, "", "");
            ////c.SendSMS(msgData, Session["userMob"].ToString());

            //proceedData.Visible = false;
            ////otpVerify.Visible = true;

            //Response.Redirect(Master.rootPath + "registration/verify?ref=cart", false);
        }
        catch (Exception ex)
        {
            proceedMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
    protected void btnVerify_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["userMob"] == null || Session["verifyCode"] == null)
            {
                Response.Redirect(Master.rootPath + "registration", false);
            }
            else
            {
                DateTime cDate = DateTime.Now;
                string currentDate = cDate.ToString("yyyy-MM-dd HH:mm:ss.fff");

                HttpCookie cartItemCookie = Request.Cookies["ordId"];
                int orderId = 0;
                if (cartItemCookie != null)
                {
                    string[] arrOrd = cartItemCookie.Value.Split('#');
                    orderId = Convert.ToInt32(arrOrd[0].ToString());
                }

                if (txtVerify.Text == "")
                {
                    verifyMsg = c.ErrNotification(2, "Enter Verification Code");
                    return;
                }
                else if (txtVerify.Text != Session["verifyCode"].ToString())
                {
                    verifyMsg = c.ErrNotification(2, "Wrong Verification Code Entered");
                    return;
                }
                else
                {
                    // Insert basic user details in db
                    int maxId = c.NextId("CustomersData", "CustomrtID");
                    string[] arrMemberInfo = Session["otherInfo"].ToString().Split('#');
                    //string[] arrMemberInfo = Session["otherInfo"].ToString().Split('#');

                    //c.ExecuteQuery("Insert Into CustomersData (CustomrtID, CustomerJoinDate, CustomerEmail, CustomerMobile, CustomerName " +
                    //    ", CustomerCity, CustomerState, CustomerPincode, CustomerCountry, CustomerAddress, MobileVerify, EmailVerify, CustomerPassword, delMark) Values " +
                    //    " (" + maxId + ", '" + currentDate + "', '" + Session["userEmail"] + "', '" + arrMemberInfo[1].ToString() + "', '" + arrMemberInfo[0].ToString() +
                    //    "', '" + arrMemberInfo[4].ToString() + "', '" + arrMemberInfo[3].ToString() + "', '" + arrMemberInfo[5].ToString() + "', '" + arrMemberInfo[2].ToString() +
                    //    "', '" + arrMemberInfo[6].ToString() + "', 1, 1, '" + arrMemberInfo[7].ToString() + "', 0)");

                    c.ExecuteQuery("Insert Into CustomersData (CustomrtID, CustomerJoinDate, CustomerName, CustomerMobile, " +
                        " MobileVerify, EmailVerify, CustomerActive, delMark, DeviceType, CustomerPassword) Values (" + maxId + ", '" + currentDate +
                        "', N'" + arrMemberInfo[0].ToString() + "', '" + arrMemberInfo[1].ToString() + "', 1, 1, 1, 0, 'Web', '123456')");

                    Session["genericCust"] = maxId.ToString();


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
                        }
                    }
                    else
                    {
                        Response.Redirect(Master.rootPath + "customer/user-info", false);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
    protected void btnResend_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["userMob"] == null || Session["verifyCode"] == null)
            {
                Response.Redirect(Master.rootPath + "registration", false);
            }
            else
            {
                //string msgData = "Your Verification Code is " + Session["verifyCode"].ToString() + ", <br/> - Genericart Online Generic Medicine Shopping";
                //c.SendSMS(msgData, Session["userMob"].ToString());
                //c.SendMail("info@intellect-systems.com", "Genericart Shopping", Session["userEmail"].ToString(), msgData, "Verification Code", "", true, "", "");

                string msgData = "Dear " + txtName.Text + ", Your Verification Code is " + Session["verifyCode"].ToString() + " Genericart Medicine - Wahi Kaam, Sahi Daam";
                c.SendSMS(msgData, Session["userMob"].ToString());

                verifyMsg = c.ErrNotification(1, "Verification Code is Re-sent to your mobile number");
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Session["userMob"] = null;
        Session["verifyCode"] = null;
        Response.Redirect(Master.rootPath + "registration", false);
    }
}