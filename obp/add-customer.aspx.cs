using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class obp_add_customer : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["action"] != null)
            {
                //initialBox.Visible = true;
                //editCust.Visible = true;
                viewCust.Visible = false;
                if (Request.QueryString["action"] == "new")
                {
                    btnSave.Text = "Save Info";
                    editCust.Visible = false;
                    initialBox.Visible = true; ;
                }
                else
                {
                    editCust.Visible = true;
                    initialBox.Visible = false;
                    btnSave.Text = "Modify Info";
                    GetCustData(Convert.ToInt32(Request.QueryString["id"]));
                    //ButtonsVisibility();
                }
            }
            else
            {
                editCust.Visible = false;
                initialBox.Visible = false;
                viewCust.Visible = true;
                FillGrid();
            }
            txtCountry.Text="India";
        }
    }

    private void FillGrid()
    {
        try
        {
            int GOBP = Convert.ToInt32(Session["adminObp"]);
            using (DataTable dtCustomer = c.GetDataTable("Select CustomrtID, isnull(CustomerFavShop, 0) as CustomerFavShop, convert(varchar(20), CustomerJoinDate, 103) as CustomerJoinDate, CustomerName, CustomerMobile, CustomerEmail From CustomersData Where delMark=0 And CustomerActive=1 And FK_ObpID=" + GOBP + ""))
            {
                gvCustomer.DataSource = dtCustomer;
                gvCustomer.DataBind();
                if (gvCustomer.Rows.Count > 0)
                {
                    gvCustomer.UseAccessibleHeader = true;
                    gvCustomer.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            txtName.Text = txtName.Text.Trim().Replace("'", "");
            txtMobile.Text = txtMobile.Text.Trim().Replace("'", "");
            txtEmail.Text = txtEmail.Text.Trim().Replace("'", "");
            txtCountry.Text = txtCountry.Text.Trim().Replace("'", "");
            txtState.Text = txtState.Text.Trim().Replace("'", "");
            txtCity.Text = txtCity.Text.Trim().Replace("'", "");
            txtPinCode.Text = txtPinCode.Text.Trim().Replace("'", "");
            txtAddress.Text = txtAddress.Text.Trim().Replace("'", "");

            if (txtName.Text == "" || txtMobile.Text == "" || txtCountry.Text == "" || txtState.Text == "" || txtCity.Text == "" || txtAddress.Text == "" || ddrAddrType.SelectedIndex == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * Fields are mandatory');", true);
                return;
            }
            if (c.ValidateMobile(txtMobile.Text) == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid Mobile No');", true);
                return;
            }
            if (c.EmailAddressCheck(txtEmail.Text) == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid Email Address');", true);
                return;
            }
            if (lblId.Text == "[New]")
            {
                if (c.IsRecordExist("Select CustomrtID From CustomersData Where CustomerMobile='" + txtMobile.Text + "' AND delMark=0"))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'This Mobile No is already registered');", true);
                    return;
                }
            }
            else
            {
                if (c.IsRecordExist("Select CustomrtID From CustomersData Where CustomerMobile='" + txtMobile.Text + "' AND delMark=0 AND CustomrtID<>" + lblId.Text)) 
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'This Mobile No is already registered');", true);
                    return;
                }
            }


            int maxCustId = lblId.Text == "[New]" ? c.NextId("CustomersData", "CustomrtID") : Convert.ToInt32(lblId.Text);

            int GOBPID = Convert.ToInt32(Session["adminObp"]);
            DateTime cDate = DateTime.Now;
            string currentDate = cDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string password = "123456";
            string addrType = ddrAddrType.Items[ddrAddrType.SelectedIndex].Text;
            int shopId;
            if (lblId.Text == "[New]")
            {
                c.ExecuteQuery("Insert Into CustomersData(CustomrtID, CustomerJoinDate, CustomerName, CustomerMobile, CustomerEmail, CustomerPassword, " +
                " CustomerActive, DeviceType, MobileVerify, EmailVerify, delMark, FK_ObpID) Values(" + maxCustId + ", '" +
                currentDate + "', '" + txtName.Text + "', '" + txtMobile.Text + "', '" + txtEmail.Text + "', '" + password + "', 1, 'GOBP-Web', 1, " +
                " 1, 0, " + GOBPID + ") ");

                int maxAddrId = c.NextId("CustomersAddress", "AddressID");
                c.ExecuteQuery("Insert Into CustomersAddress (AddressID, AddressFKCustomerID, AddressName,  AddressFull, AddressCity, " +
                    " AddressState, AddressPincode, AddressCountry, AddressStatus) Values (" + maxAddrId + ", " + maxCustId + ", '" +
                    addrType + "', '" + txtAddress.Text + "', '" + txtCity.Text + "', '" + txtState.Text + "', '" + txtPinCode.Text +
                    "', '" + txtCountry.Text + "', 1)");

                
                //if (txtShopCode.Text != "")
                //{
                //    if (c.IsRecordExist("Select FranchID From FranchiseeData Where FranchShopCode='" + txtShopCode.Text + "' And FranchActive=1"))
                //    {
                //        shopId = Convert.ToInt32(c.GetReqData("FranchiseeData", "FranchID", "FranchShopCode= '" + txtShopCode.Text + "' And FranchActive=1 "));
                //    }
                //    else
                //    {
                //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Shop with this shopcode does not exist');", true);
                //        return;
                //    }
                //    c.ExecuteQuery("Update CustomersData Set CustomerFavShop=" + shopId + " Where CustomrtID=" + maxCustId + "");
                //}
                //else
                //{
                //    c.ExecuteQuery("Update CustomersData Set CustomerFavShop=24 Where CustomrtID=" + maxCustId + "");
                //}

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Customer info added successfully');", true);
            }
            else
            {
                c.ExecuteQuery("Update CustomersData Set CustomerName='" + txtName.Text + "', CustomerMobile='" + txtMobile.Text + 
                    "', CustomerEmail='" + txtEmail.Text + "' Where CustomrtID=" + maxCustId);

                //if (txtShopCode.Text != "")
                //{
                //    if (c.IsRecordExist("Select FranchID From FranchiseeData Where FranchShopCode='" + txtShopCode.Text + "' And FranchActive=1"))
                //    {
                //        shopId = Convert.ToInt32(c.GetReqData("FranchiseeData", "FranchID", "FranchShopCode= '" + txtShopCode.Text + "' And FranchActive=1 "));
                //    }
                //    else
                //    {
                //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Shop with this shopcode does not exist');", true);
                //        return;
                //    }
                //    c.ExecuteQuery("Update CustomersData Set CustomerFavShop=" + shopId + " Where CustomrtID=" + maxCustId + "");
                //}

                int addrId = Convert.ToInt32(c.GetReqData("CustomersAddress", "TOP 1 AddressID", "AddressFKCustomerID=" + maxCustId + " Order By AddressID DESC"));

                c.ExecuteQuery("Update CustomersAddress Set AddressName='" + ddrAddrType.SelectedItem.Text + "', AddressFull='" + txtAddress.Text + 
                    "', AddressCity='" + txtCity.Text + "', AddressState='" + txtState.Text + "', AddressPincode='" + txtPinCode.Text + 
                    "', AddressCountry='" + txtCountry.Text + "' Where AddressID=" + addrId + " AND AddressFKCustomerID=" + maxCustId);

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Customer info updated successfully');", true);                
            }

            HttpCookie obpCookies = new HttpCookie("obpCookies");
            obpCookies.Value = Session["adminObp"].ToString();
            Response.Cookies.Add(obpCookies);

            Response.Redirect(c.ReturnHttp() + "saving-calculator");

            //ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('add-customer.aspx', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSave_Click", ex.Message.ToString());
            return;
        }
    }

    protected void gvCustomer_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {                
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"add-customer.aspx?action=edit&id=" + e.Row.Cells[0].Text + "\" class=\"gAnch\"></a>";
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvCustomer_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    protected void gvCustomer_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridViewRow gRow = (GridViewRow)((Button)e.CommandSource).NamingContainer;
            int custId = Convert.ToInt32(gRow.Cells[0].Text);

            if (e.CommandName == "gvSubmit")
            {
                Session["genericCust"] = custId;

                //Response.Redirect("https://ecommerce.genericartmedicine.com/categories/medicine-7/1");

                // Create a new cookie
                HttpCookie myObpCookie = new HttpCookie("MyOBPCookie");
                myObpCookie.Value = Session["adminObp"].ToString();
                Response.Cookies.Add(myObpCookie);

                Response.Redirect (c.ReturnHttp() + "categories/medicine-7/1");
            }

            if (e.CommandName == "gvKnowMore")
            {
                Session["genericCust"] = custId;

                // Create a new cookie
                HttpCookie obpCookies = new HttpCookie("obpCookies");
                obpCookies.Value = Session["adminObp"].ToString();
                Response.Cookies.Add(obpCookies);

                Response.Redirect(c.ReturnHttp() + "saving-calculator");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvCustomer_RowCommand", ex.Message.ToString());
            return;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("add-customer.aspx");
    }

    private void GetCustData(int custIdX)
    {
        try
        {
            using (DataTable dtCust = c.GetDataTable("Select * From CustomersData Where CustomrtID=" + custIdX))
            {
                if (dtCust.Rows.Count > 0)
                {
                    DataRow row = dtCust.Rows[0];

                    lblId.Text = custIdX.ToString();

                    //txtShopCode.Text = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID=" + row["CustomerFavShop"].ToString() + "").ToString();
                    txtName.Text = row["CustomerName"].ToString();
                    txtMobile.Text = row["CustomerMobile"].ToString();
                    txtEmail.Text = row["CustomerEmail"] != DBNull.Value && row["CustomerEmail"] != null && row["CustomerEmail"].ToString() != "" ? row["CustomerEmail"].ToString() : "";

                    using (DataTable dtAddr = c.GetDataTable("Select Top 1 AddressName, AddressFull, AddressCity, AddressState, AddressPincode, AddressCountry From CustomersAddress Where AddressFKCustomerID=" + custIdX + " Order By AddressID DESC"))
                    {
                        if (dtAddr.Rows.Count > 0)
                        {
                            DataRow aRow = dtAddr.Rows[0];

                            txtCountry.Text = aRow["AddressCountry"] != DBNull.Value && aRow["AddressCountry"] != null && aRow["AddressCountry"].ToString() != "" ? aRow["AddressCountry"].ToString() : "";
                            txtState.Text = aRow["AddressState"] != DBNull.Value && aRow["AddressState"] != null && aRow["AddressState"].ToString() != "" ? aRow["AddressState"].ToString() : "";
                            txtCity.Text = aRow["AddressCity"] != DBNull.Value && aRow["AddressCity"] != null && aRow["AddressCity"].ToString() != "" ? aRow["AddressCity"].ToString() : "";
                            txtPinCode.Text = aRow["AddressPincode"] != DBNull.Value && aRow["AddressPincode"] != null && aRow["AddressPincode"].ToString() != "" ? aRow["AddressPincode"].ToString() : "";
                            txtAddress.Text = aRow["AddressFull"] != DBNull.Value && aRow["AddressFull"] != null && aRow["AddressFull"].ToString() != "" ? aRow["AddressFull"].ToString() : "";
                            if (aRow["AddressName"] != DBNull.Value && aRow["AddressName"] != null && aRow["AddressName"].ToString() != "")
                            {
                                switch (aRow["AddressName"].ToString())
                                {
                                    case "Home": ddrAddrType.SelectedValue = "1"; break;
                                    case "Office": ddrAddrType.SelectedValue = "2"; break;
                                    case "Other": ddrAddrType.SelectedValue = "3"; break;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetCustData", ex.Message.ToString());
            return;
        }
    }
    protected void btnSub_Click(object sender, EventArgs e)
    {
        try
        {
            txtMob.Text = txtMob.Text.Trim().Replace("'", "");

            if (txtMob.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Mobile number');", true);
                return;
            }

            if (!c.ValidateMobile(txtMob.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid Mobile number');", true);
                return;
            }

            if (c.IsRecordExist("Select CustomrtID From CustomersData Where CustomerMobile='" + txtMob.Text + "' AND CustomerActive=1 AND  delMark=0"))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Customer with this mobile no is already exists');", true);
                return;
            }
            else
            {
                initialBox.Visible = false;
                editCust.Visible = true;
                txtMobile.Text = txtMob.Text;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetCustData", ex.Message.ToString());
            return;
        }
    }
}