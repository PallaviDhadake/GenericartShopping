using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class customer_user_address : System.Web.UI.Page
{
    iClass c = new iClass();
    public string latlongs, errMsg, address, custZipCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                //if (Request.Cookies["userLoc"] != null)
                //{
                //    string citySate = Request.Cookies["userLoc"].Value.ToString();
                //    string[] arrLocation = citySate.Split('#');

                //    string latitude = arrLocation[3].ToString();
                //    string longitude = arrLocation[4].ToString();

                //    latlongs = latitude + "," + longitude;
                //    //latlongs = "Intellect, Systems";
                //}

                GetUserAddressInfo();

                if (Request.QueryString["type"] != null)
                {
                    addressForm.Visible = true;
                    if (Request.QueryString["type"] == "change")
                    {
                        btnSave.Text = "Update Address";
                        GetAddress(Convert.ToInt32(Request.QueryString["id"]));
                    }
                    if (Request.QueryString["type"] == "new")
                    {
                        btnSave.Text = "Save Address";
                    }
                    if (Request.QueryString["type"] == "delete")
                    {
                        c.ExecuteQuery("Update CustomersAddress Set AddressStatus=0 Where AddressID=" + Request.QueryString["id"]);
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Address Deleted');", true);
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('" + Master.rootPath + "user-address', 2000);", true);
                    }
                }
                else
                {
                    addressForm.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    private void GetAddress(int addIdX)
    {
        try
        {
            using (DataTable dtCust = c.GetDataTable("Select AddressID, AddressFull, AddressName, AddressCity, AddressState, AddressPincode, AddressCountry, AddressLatitude, AddressLongitude From CustomersAddress Where AddressStatus=1 AND AddressFKCustomerID=" + Session["genericCust"].ToString() + " AND AddressID=" + addIdX)) 
            {
                if (dtCust.Rows.Count > 0)
                {
                    DataRow row = dtCust.Rows[0];

                    txtAddress.Text = row["AddressFull"] != DBNull.Value && row["AddressFull"] != null && row["AddressFull"].ToString() != "" ? row["AddressFull"].ToString() : "";
                    txtCity.Text = row["AddressCity"] != DBNull.Value && row["AddressCity"] != null && row["AddressCity"].ToString() != "" ? row["AddressCity"].ToString() : "";
                    txtState.Text = row["AddressState"] != DBNull.Value && row["AddressState"] != null && row["AddressState"].ToString() != "" ? row["AddressState"].ToString() : "";
                    txtZipcode.Text = row["AddressPincode"] != DBNull.Value && row["AddressPincode"] != null && row["AddressPincode"].ToString() != "" ? row["AddressPincode"].ToString() : "";
                    txtCountry.Text = row["AddressCountry"] != DBNull.Value && row["AddressCountry"] != null && row["AddressCountry"].ToString() != "" ? row["AddressCountry"].ToString() : "";
                    txtLatitude.Text = row["AddressLatitude"] != DBNull.Value && row["AddressLatitude"] != null && row["AddressLatitude"].ToString() != "" ? row["AddressLatitude"].ToString() : "";
                    txtLongitude.Text = row["AddressLongitude"] != DBNull.Value && row["AddressLongitude"] != null && row["AddressLongitude"].ToString() != "" ? row["AddressLongitude"].ToString() : "";
                    if (row["AddressName"] != DBNull.Value && row["AddressName"] != null && row["AddressName"].ToString() != "")
                    {
                        switch (row["AddressName"].ToString())
                        {
                            case "Home": ddrAddrName.SelectedValue = "1"; break;
                            case "Office": ddrAddrName.SelectedValue = "2"; break;
                            case "Other": ddrAddrName.SelectedValue = "3"; break;
                        }
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

    private void GetUserAddressInfo()
    {
        try
        {
            //using (DataTable dtCust = c.GetDataTable("Select CustomrtID, CustomerAddress, CustomerCity, CustomerState, CustomerPincode, CustomerCountry From CustomersData Where CustomrtID=" + Session["genericCust"].ToString()))
            //{
            //    if (dtCust.Rows.Count > 0)
            //    {
            //        DataRow row = dtCust.Rows[0];

            //        txtAddress.Text = row["CustomerAddress"] != DBNull.Value && row["CustomerAddress"] != null && row["CustomerAddress"].ToString() != "" ? row["CustomerAddress"].ToString() : "";
            //        txtCity.Text = row["CustomerCity"] != DBNull.Value && row["CustomerCity"] != null && row["CustomerCity"].ToString() != "" ? row["CustomerCity"].ToString() : "";
            //        txtState.Text = row["CustomerState"] != DBNull.Value && row["CustomerState"] != null && row["CustomerState"].ToString() != "" ? row["CustomerState"].ToString() : "";
            //        txtZipcode.Text = row["CustomerPincode"] != DBNull.Value && row["CustomerPincode"] != null && row["CustomerPincode"].ToString() != "" ? row["CustomerPincode"].ToString() : "";
            //        txtCountry.Text = row["CustomerCountry"] != DBNull.Value && row["CustomerCountry"] != null && row["CustomerCountry"].ToString() != "" ? row["CustomerCountry"].ToString() : "";

            //        address = txtAddress.Text;
            //    }
            //}
            StringBuilder strMarkup = new StringBuilder();
            string strQuery1 = "", strQuery2 = "";
            if (c.IsRecordExist("Select AddressID From CustomersAddress Where AddressStatus=1 AND AddressFKCustomerID=" + Session["genericCust"].ToString() + " AND AddressLatitude IS NOT NULL AND AddressLatitude<>'' AND AddressLongitude IS NOT NULL AND AddressLongitude<>''"))
            {
                int addrId = Convert.ToInt32(c.GetReqData("CustomersAddress", "AddressID", "AddressStatus=1 AND AddressFKCustomerID=" + Session["genericCust"].ToString() + " AND AddressLatitude IS NOT NULL AND AddressLatitude<>'' AND AddressLongitude IS NOT NULL AND AddressLongitude<>''"));
                string latitude = c.GetReqData("CustomersAddress", "AddressLatitude", "AddressID=" + addrId).ToString();
                string longitude = c.GetReqData("CustomersAddress", "AddressLongitude", "AddressID=" + addrId).ToString();
                latlongs = latitude + "," + longitude;

                strQuery1 = "Select TOP 1 AddressID, AddressFull, AddressName, AddressCity, AddressState, AddressPincode, AddressCountry, AddressLatitude, AddressLongitude From CustomersAddress Where AddressStatus=1 AND AddressFKCustomerID=" + Session["genericCust"].ToString() + " AND AddressLatitude IS NOT NULL AND AddressLatitude<>'' AND AddressLongitude IS NOT NULL AND AddressLongitude<>'' Order By AddressID DESC";
                strQuery2 = "Select AddressID, AddressFull, AddressName, AddressCity, AddressState, AddressPincode, " +
                " AddressCountry, AddressLatitude, AddressLongitude From CustomersAddress Where AddressStatus=1 AND " +
                " AddressFKCustomerID=" + Session["genericCust"].ToString() + " AND " +
                " AddressID NOT IN (Select TOP 1 AddressID From CustomersAddress Where AddressStatus=1 AND AddressFKCustomerID=" + Session["genericCust"].ToString() + " AND AddressLatitude IS NOT NULL AND AddressLatitude<>'' AND AddressLongitude IS NOT NULL AND AddressLongitude<>'' Order By AddressID DESC) " +
                " Order By AddressID DESC";
            }
            else
            {
                if (Request.Cookies["userLoc"] != null)
                {
                    string citySate = Request.Cookies["userLoc"].Value.ToString();
                    //string[] arrLocation = citySate.Split('#');

                    //string latitude = arrLocation[3].ToString();
                    //string longitude = arrLocation[4].ToString();

                    //latlongs = latitude + "," + longitude;
                    //latlongs = "Intellect, Systems";
                }

                strQuery1 = "Select TOP 1 AddressID, AddressFull, AddressName, AddressCity, AddressState, AddressPincode, AddressCountry, AddressLatitude, AddressLongitude From CustomersAddress Where AddressStatus=1 AND AddressFKCustomerID=" + Session["genericCust"].ToString() + " Order By AddressID DESC";
                strQuery2 = "Select AddressID, AddressFull, AddressName, AddressCity, AddressState, AddressPincode, " +
                " AddressCountry, AddressLatitude, AddressLongitude From CustomersAddress Where AddressStatus=1 AND " +
                " AddressFKCustomerID=" + Session["genericCust"].ToString() + " AND " +
                " AddressID NOT IN (Select TOP 1 AddressID From CustomersAddress Where AddressStatus=1 AND AddressFKCustomerID=" + Session["genericCust"].ToString() + " Order By AddressID DESC) " +
                " Order By AddressID DESC";
            }

            //using (DataTable dtCust = c.GetDataTable("Select TOP 1 AddressID, AddressFull, AddressName, AddressCity, AddressState, AddressPincode, AddressCountry, AddressLatitude, AddressLongitude From CustomersAddress Where AddressStatus=1 AND AddressFKCustomerID=" + Session["genericCust"].ToString() + " Order By AddressID DESC"))
            using (DataTable dtCust = c.GetDataTable(strQuery1))
            {
                if (dtCust.Rows.Count > 0)
                {
                    DataRow row = dtCust.Rows[0];
                    //<div class="bgWhite border_r_5 box-shadow">
                    //    <div id="map-canvas"></div>
                    //    <div class="pad_20">
                    //        <div class="float_left width80">
                    //            <span class="home dispBlk mrg_B_10">Home</span>
                    //            <span class="fontRegular tiny clrGrey dispBlk"><%= address %></span>
                    //        </div>
                    //        <div class="float_right">
                    //            <span class="space10"></span>
                    //            <span id="btnChange" class="bookAnch semiBold" style="font-size:1em; cursor:pointer;">Change</span>
                    //        </div>
                    //        <div class="float_clear"></div>
                    //    </div>
                    //</div>
                    //<span class="space20"></span>
                    strMarkup.Append("<div class=\"bgWhite border_r_5 box-shadow\">");
                    strMarkup.Append("<div id=\"map-canvas\"></div>");
                    strMarkup.Append("<div class=\"pad_20\">");
                    strMarkup.Append("<div class=\"float_left width80\">");
                    strMarkup.Append("<span class=\"home dispBlk mrg_B_10\">" + row["AddressName"].ToString() + "</span>");
                    strMarkup.Append("<span class=\"fontRegular tiny clrGrey dispBlk\">" + row["AddressFull"].ToString() + "</span>");
                    strMarkup.Append("</div>");
                    strMarkup.Append("<div class=\"float_right\">");
                    strMarkup.Append("<a href=\"" + Master.rootPath + "user-address?type=change&id=" + row["AddressID"].ToString() + "#add\" class=\"bookAnch semiBold\" style=\"font-size:0.9em; cursor:pointer;\">Change</a>");
                    strMarkup.Append("<br/><p class=\"txtCenter tiny\">or</p> <a href=\"" + Master.rootPath + "user-address?type=delete&id=" + row["AddressID"].ToString() + "\" class=\"deleteProd\" style=\"font-size:0.9em; color:#ff0000; cursor:pointer;\" title=\"Delete Address\"></a>");
                    strMarkup.Append("</div>");
                    strMarkup.Append("<div class=\"float_clear\"></div>");
                    strMarkup.Append("</div>");
                    strMarkup.Append("</div>");
                    strMarkup.Append("<span class=\"space20\"></span>");
                }
            }

            //using (DataTable dtCustAddr = c.GetDataTable("Select AddressID, AddressFull, AddressName, AddressCity, AddressState, AddressPincode, " + 
            //    " AddressCountry, AddressLatitude, AddressLongitude From CustomersAddress Where AddressStatus=1 AND " +
            //    " AddressFKCustomerID=" + Session["genericCust"].ToString() + " AND " +
            //    " AddressID NOT IN (Select TOP 1 AddressID From CustomersAddress Where AddressStatus=1 AND AddressFKCustomerID=" + Session["genericCust"].ToString() + " Order By AddressID DESC) " +
            //    " Order By AddressID DESC"))
            using (DataTable dtCustAddr = c.GetDataTable(strQuery2))
            {
                if (dtCustAddr.Rows.Count > 0)
                {
                    string rPath = c.ReturnHttp();
                    foreach (DataRow row in dtCustAddr.Rows)
                    {
                        strMarkup.Append("<div class=\"bgWhite border_r_5 box-shadow\">");
                        strMarkup.Append("<img src=\"" + rPath + "images/address.jpg\" class=\"width100\"/>");
                        strMarkup.Append("<div class=\"pad_20\">");
                        strMarkup.Append("<div class=\"float_left width80\">");
                        strMarkup.Append("<span class=\"home dispBlk mrg_B_10\">" + row["AddressName"].ToString() + "</span>");
                        strMarkup.Append("<span class=\"fontRegular tiny clrGrey dispBlk\">" + row["AddressFull"].ToString() + "</span>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("<div class=\"float_right\">");
                        //strMarkup.Append("<span class=\"space10\"></span>");
                        //strMarkup.Append("<a href=\"" + Master.rootPath + "user-address?type=change&id=" + row["AddressID"].ToString() + "#add\" class=\"bookAnch semiBold\" style=\"font-size:1em; cursor:pointer;\">Change</a>");
                        strMarkup.Append("<a href=\"" + Master.rootPath + "user-address?type=change&id=" + row["AddressID"].ToString() + "#add\" class=\"bookAnch semiBold\" style=\"font-size:0.9em; cursor:pointer;\">Change</a>");
                        strMarkup.Append("<br/><p class=\"txtCenter tiny\">or</p> <a href=\"" + Master.rootPath + "user-address?type=delete&id=" + row["AddressID"].ToString() + "\" class=\"deleteProd\" style=\"font-size:0.9em; color:#ff0000; cursor:pointer;\" title=\"Delete Address\"></a>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("<div class=\"float_clear\"></div>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("<span class=\"space20\"></span>");
                    }
                }
            }

            address = strMarkup.ToString();
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
            txtAddress.Text = txtAddress.Text.Trim().Replace("'", "");
            txtCity.Text = txtCity.Text.Trim().Replace("'", "");
            txtCountry.Text = txtCountry.Text.Trim().Replace("'", "");
            txtState.Text = txtState.Text.Trim().Replace("'", "");
            txtZipcode.Text = txtZipcode.Text.Trim().Replace("'", "");
            txtLatitude.Text = txtLatitude.Text.Trim().Replace("'", "");
            txtLongitude.Text = txtLongitude.Text.Trim().Replace("'", "");

            if (txtAddress.Text == "" || txtCity.Text == "" || txtState.Text == "" || txtCountry.Text == "" || txtZipcode.Text == "" || ddrAddrName.SelectedIndex == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * Marked fields are Mandatory');", true);
                return;
            }

            if (txtAddress.Text.Length > 200)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Address must be less than 200 characters');", true);
                return;
            }

            int custId = Convert.ToInt32(Session["genericCust"].ToString());

            //c.ExecuteQuery("Update CustomersData Set CustomerAddress='" + txtAddress.Text + "', CustomerCity='" + txtCity.Text +
            //    "', CustomerState='" + txtState.Text + "', CustomerPincode='" + txtZipcode.Text + "', CustomerCountry='" + txtCountry.Text +
            //    "' Where CustomrtID=" + custId);

            int maxAddrId = Request.QueryString["type"] == "new" ? c.NextId("CustomersAddress", "AddressID") : Convert.ToInt32(Request.QueryString["id"]);

            if (Request.QueryString["type"] == "new")
            {
                c.ExecuteQuery("Insert Into CustomersAddress (AddressID, AddressFKCustomerID, AddressFull, AddressCity, AddressState, " +
                            " AddressPincode, AddressCountry, AddressLatitude, AddressLongitude, AddressStatus, AddressName) Values (" + maxAddrId +
                            ", " + custId + ", '" + txtAddress.Text + "', '" + txtCity.Text + "', '" + txtState.Text + "', '" + txtZipcode.Text +
                            "', '" + txtCountry.Text + "', '" + txtLatitude.Text + "', '" + txtLongitude.Text + "', 1, '" + ddrAddrName.SelectedItem.Text + "')");

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Information Added successfully..!!');", true);
            }
            else
            {
                c.ExecuteQuery("Update CustomersAddress Set AddressFull='" + txtAddress.Text + "', AddressCity='" + txtCity.Text +
                    "', AddressState='" + txtState.Text + "', AddressPincode='" + txtZipcode.Text + "', AddressCountry='" + txtCountry.Text +
                    "', AddressLatitude='" + txtLatitude.Text + "', AddressLongitude='" + txtLongitude.Text +
                    "', AddressName='" + ddrAddrName.SelectedItem.Text + "' Where AddressID=" + maxAddrId);

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Information Updated successfully..!!');", true);
            }

            GetUserAddressInfo();
            string path = Master.rootPath;
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('" + Master.rootPath + "user-address', 1000);", true);
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect(Master.rootPath + "user-address?type=new", false);
    }
}