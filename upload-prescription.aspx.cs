using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;

public partial class upload_prescription : System.Web.UI.Page
{
    iClass c = new iClass();
    public string prescriStr, addrStr;
    public int ordId;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                int customerId;
                if (Session["genericCust"] == null)
                {
                    Response.Redirect(Master.rootPath + "login?ref=rx", false);
                }
                else
                {
                    customerId = Convert.ToInt32(Session["genericCust"]);
                    if (Session["ordId"] != null)
                    {
                        ordId = Convert.ToInt32(Session["ordId"]);
                    }
                    else
                    {
                        ordId = c.NextId("OrdersData", "OrderID");
                        Session["ordId"] = ordId.ToString();
                    }

                    GetMembeDetails(customerId);

                    if (c.IsRecordExist("Select PrescriptionID From OrderPrescriptions Where FK_OrderID=" + ordId))
                    {
                        GetUploadedPrescription(ordId);
                    }

                    if (c.IsRecordExist("Select AddressID From CustomersAddress Where AddressFKCustomerID=" + customerId))
                    {
                        newAddr.Visible = false;
                        existingAddr.Visible = true;
                        GetCustomerAddress(customerId, ordId);
                    }
                    else
                    {
                        newAddr.Visible = true;
                        existingAddr.Visible = false;
                        chkAddNew.Checked = true;
                    }
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

    private void GetCustomerAddress(int custIdX, int orderIdX)
    {
        try
        {
            using (DataTable dtCustAddr = c.GetDataTable("Select AddressID, AddressFull, AddressName, AddressCity, AddressPincode From CustomersAddress Where AddressStatus=1 AND AddressFKCustomerID=" + custIdX))
            {
                if (dtCustAddr.Rows.Count > 0)
                {
                    StringBuilder strMarkup = new StringBuilder();
                    int bCount = 0;
                    foreach (DataRow row in dtCustAddr.Rows)
                    {
                        strMarkup.Append("<div class=\"w50 float_left\">");
                        strMarkup.Append("<div style=\"padding:10px 30px 10px 0;\">");
                        strMarkup.Append("<span id=\"addr-" + row["AddressID"].ToString() + "\">");
                        if (!c.IsRecordExist("Select OrderID From OrdersData Where OrderID=" + orderIdX + " AND FK_AddressId=" + row["AddressID"].ToString()))
                        {
                            strMarkup.Append("<a href=\"" + Master.rootPath + "add-addr/" + row["AddressID"].ToString() + "-" + orderIdX + "\" class=\"addrBorder txtDecNone\"  style=\"display:block;\">");
                            strMarkup.Append("<div class=\"box-shadow bgWhite border_r_5\">");
                            strMarkup.Append("<div class=\"pad_10\" style=\"cursor:pointer\">");
                            strMarkup.Append("<span class=\"themeClrPrime fontRegular\">" + row["AddressName"].ToString() + "</span>");
                            strMarkup.Append("<span class=\"space5\"></span>");
                            strMarkup.Append("<span class=\"clrGrey small fontRegular line-ht-5\">" + row["AddressFull"].ToString() + "</span>");
                            strMarkup.Append("<span class=\"space10\"></span>");
                            strMarkup.Append("<span class=\"clrGrey small fontRegular line-ht-5\">" + row["AddressCity"].ToString() + ", " + row["AddressPincode"].ToString() + "</span>");
                            strMarkup.Append("</div>");
                            strMarkup.Append("</div>");
                            strMarkup.Append("</a>");
                        }
                        else
                        {
                            strMarkup.Append("<a class=\"addrBorderBlue txtDecNone\"  style=\"display:block;\">");
                            strMarkup.Append("<div class=\"box-shadow bgWhite border_r_5\">");
                            strMarkup.Append("<div class=\"pad_10\" style=\"cursor:pointer\">");
                            strMarkup.Append("<span class=\"themeClrPrime fontRegular\">" + row["AddressName"].ToString() + "</span>");
                            strMarkup.Append("<span class=\"space5\"></span>");
                            strMarkup.Append("<span class=\"clrGrey small fontRegular line-ht-5\">" + row["AddressFull"].ToString() + "</span>");
                            strMarkup.Append("<span class=\"space10\"></span>");
                            strMarkup.Append("<span class=\"clrGrey small fontRegular line-ht-5\">" + row["AddressCity"].ToString() + ", " + row["AddressPincode"].ToString() + "</span>");
                            strMarkup.Append("<span class=\"space10\"></span>");
                            strMarkup.Append("<span class=\"semiBold themeClrPrime\">Address Selected</span>");
                            strMarkup.Append("</div>");
                            strMarkup.Append("</div>");
                            strMarkup.Append("</a>");
                        }
                        strMarkup.Append("</span>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");

                        bCount++;

                        if ((bCount % 2) == 0)
                        {
                            strMarkup.Append("<div class=\"float_clear\"></div>");
                        }
                    }
                    strMarkup.Append("<div class=\"float_clear\"></div>");
                    addrStr = strMarkup.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetCustomerAddress", ex.Message.ToString());
            return;
        }
    }

    private void GetMembeDetails(int memIdX)
    {
        try
        {
            using (DataTable dtMem = c.GetDataTable("Select CustomrtID, CustomerEmail, CustomerMobile, CustomerName, CustomerCity, " +
                " CustomerState, CustomerPincode, CustomerCountry, CustomerAddress From CustomersData Where CustomrtID=" + memIdX))
            {
                if (dtMem.Rows.Count > 0)
                {
                    DataRow row = dtMem.Rows[0];

                    txtMobile.Text = row["CustomerMobile"].ToString();

                    txtName.Text = row["CustomerName"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetMembeDetails", ex.Message.ToString());
            return;
        }
    }

    protected void btnUploadRx_Click(object sender, EventArgs e)
    {
        try
        {
            int orderId = Convert.ToInt32(Session["ordId"]);

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

                // OrderType = 1 > Normal Orders, 2 > Prescription Orders
                if (!c.IsRecordExist("Select OrderID From OrdersData Where OrderID=" + orderId))
                {
                    c.ExecuteQuery("Insert Into OrdersData (OrderID, FK_OrderCustomerID, OrderDate, OrderAmount, OrderStatus, OrderType, FK_AddressId, DeviceType) " +
                    " Values (" + orderId + ", " + Session["genericCust"] + ", '" + DateTime.Now + "', 0, 0, 2, 0, 'Web')");
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
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('" + Master.rootPath + "upload-prescription', 2000);", true);
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
            using (DataTable dtRx = c.GetDataTable("Select PrescriptionID, PrescriptionName From OrderPrescriptions Where FK_OrderID=" + ordIdX))
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
                        strMarkup.Append("<a href=\"" + Master.rootPath + "upload/prescriptions/" + row["PrescriptionName"] + "\" data-fancybox=\"rxGroup\"><img src=\"" + Master.rootPath + "upload/prescriptions/" + row["PrescriptionName"] + "\" class=\"width100\" /></a>");
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

    protected void chkAddNew_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAddNew.Checked == true)
        {
            newAddr.Visible = true;
            existingAddr.Visible = false;
        }
        else
        {
            newAddr.Visible = false;
            existingAddr.Visible = true;
            Response.Redirect(Master.rootPath + "upload-prescription", false);
        }
    }

    protected void btnShipping_Click(object sender, EventArgs e)
    {
        try
        {
            txtMobile.Text = txtMobile.Text.Trim().Replace("'", "");
            txtName.Text = txtName.Text.Trim().Replace("'", "");
            txtCountry.Text = txtCountry.Text.Trim().Replace("'", "");
            txtState.Text = txtState.Text.Trim().Replace("'", "");
            txtCity.Text = txtCity.Text.Trim().Replace("'", "");
            txtPinCode.Text = txtPinCode.Text.Trim().Replace("'", "");
            txtAddress1.Text = txtAddress1.Text.Trim().Replace("'", "");

            int orderId = Convert.ToInt32(Session["ordId"]);
            int customerId = Convert.ToInt32(Session["genericCust"]);
            int mreqFlag = chkMreq.Checked == true ? 1 : 0;

            if (chkAddNew.Checked == false)
            {
                if (c.IsRecordExist("Select OrderID From OrdersData Where OrderID=" + orderId + " AND (FK_AddressId IS NULL OR FK_AddressId=0)"))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select Address To Proceed');", true);
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('" + Master.rootPath + "upload-prescription', 1000);", true);
                }
                else
                {
                    c.ExecuteQuery("Update OrdersData Set OrderContactInfo='" + txtMobile.Text + "', OrderStatus=1, OrderPayMode=1, " +
                        " OrderPayStatus=0, MreqFlag=" + mreqFlag + " Where OrderID=" + orderId);
                    //Response.Redirect(Master.rootPath + "upload-prescription", false);
                    //Session["ordId"] = null;
                    //ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Your Request Submitted Successfully..!!');", true);
                    //ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('" + Master.rootPath + "customer/my-orders', 1000);", true);
                }
            }
            else
            {

                if (txtName.Text == "" || txtCountry.Text == "" ||
                    txtState.Text == "" || txtCity.Text == "" || txtPinCode.Text == "" || txtAddress1.Text == "" || ddrAddrName.SelectedIndex == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * Marked fields are Mandatory');", true);
                    return;
                }
                if (txtMobile.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Contact Information');", true);
                    return;
                }
                if (c.ValidateMobile(txtMobile.Text) == false)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter valid mobile number');", true);
                    return;
                }
                if (!c.IsNumeric(txtPinCode.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Pincode must be numeric value');", true);
                    return;
                }

                // insert new address of cust in custAddress tbl

                if (chkAddNew.Checked == true)
                {
                    int maxAddrId = c.NextId("CustomersAddress", "AddressID");
                    c.ExecuteQuery("Insert Into CustomersAddress (AddressID, AddressFKCustomerID, AddressFull, AddressCity, AddressState, " +
                        " AddressPincode, AddressCountry, AddressStatus, AddressName) Values (" + maxAddrId + ", " + customerId + ", '" + txtAddress1.Text +
                        "', '" + txtCity.Text + "', '" + txtState.Text + "', '" + txtPinCode.Text + "', '" + txtCountry.Text + "', 1, '" + ddrAddrName.SelectedItem.Text + "')");

                    // update addressId in OrdersData tbl
                    c.ExecuteQuery("Update OrdersData Set OrderContactInfo='" + txtMobile.Text + "', FK_AddressId=" + maxAddrId +
                        ", OrderStatus=1, OrderPayMode=1, OrderPayStatus=0, MreqFlag=" + mreqFlag + " Where OrderID=" + orderId);

                    //Session["ordId"] = null;

                    //ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Your Request Submitted Successfully..!!');", true);
                    //ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('" + Master.rootPath + "customer/my-orders', 1000);", true);
                }

                
            }

            if (c.IsRecordExist("Select CustomrtID From CustomersData Where CustomerFavShop IS NOT NULL AND CustomerFavShop<>'' AND CustomerFavShop<>0 AND CustomrtID=" + customerId))
            {
                int franchId = Convert.ToInt32(c.GetReqData("CustomersData", "CustomerFavShop", "CustomrtID=" + customerId));

                // set order status to 3 -> as it is accepted by admin 
                c.ExecuteQuery("Update OrdersData Set OrderStatus=3 Where OrderID=" + orderId);

                if (!c.IsRecordExist("Select OrdAssignID From OrdersAssign Where FK_OrderID=" + orderId + " AND Fk_FranchID=" + franchId + " AND  OrdAssignStatus=0"))
                {
                    // insert entry in OrderAssign table, it is directly assigned to customers fav shop
                    c.ExecuteQuery("Update OrdersAssign Set OrdReAssign=1 Where FK_OrderID=" + orderId);
                    int maxId = c.NextId("OrdersAssign", "OrdAssignID");
                    c.ExecuteQuery("Insert Into OrdersAssign (OrdAssignID, OrdAssignDate, FK_OrderID, Fk_FranchID, OrdAssignStatus, " +
                        " OrdReAssign, AssignedFrom) Values (" + maxId + ", '" + DateTime.Now + "', " + orderId + ", " + franchId + ", 0, 0, 'website')");
                }

                // send sms to shop

                string frName = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + franchId).ToString();
                string mobNo = c.GetReqData("FranchiseeData", "FranchMobile", "FranchID=" + franchId).ToString();
                string pendingOrdCount = c.returnAggregate("Select Count(OrdAssignID) From OrdersAssign Where OrdAssignStatus=0 AND OrdReAssign=0 AND Fk_FranchID=" + franchId).ToString();
                string msgData = "Dear " + frName + ", You have received new order Order No " + orderId + " from Genericart Mobile App.Total Pending Order is/are " + pendingOrdCount + " Thank you Genericart Medicine Store Wahi Kaam, Sahi Daam";
                c.SendSMS(msgData, mobNo);
            }

            Session["ordId"] = null;
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Your Request Submitted Successfully..!!');", true);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('" + Master.rootPath + "customer/my-orders', 1000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnShipping_Click", ex.Message.ToString());
            return;
        }
    }
}