using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class admingenshopping_shop_list : System.Web.UI.Page
{
    iClass c = new iClass();
    public string pgTitle, errMsg, frAdhar, frPan;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["action"] != null)
                {
                    c.FillComboBox("StateName", "StateID", "StatesData", "FK_CountryID=101", "StateID", 0, ddrState);
                    c.FillComboBox("ZonalHdName", "ZonalHdId", "ZonalHead", "DelMark=0", "ZonalHdName", 0, ddrZH);

                    editFranch.Visible = true;
                    viewFranch.Visible = false;
                    if (Request.QueryString["action"] == "new")
                    {
                        pgTitle = "Add Franchisee Info";
                        btnDelete.Visible = false;
                    }
                    else
                    {
                        pgTitle = "Modify Franchisee Info";
                        btnDelete.Visible = true;
                        GetShopInfo(Convert.ToInt32(Request.QueryString["id"]));
                    }
                }
                else
                {
                    editFranch.Visible = false;
                    viewFranch.Visible = true;
                    FillGrid();
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

    protected void ddrZH_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddrZH.SelectedIndex > 0)
            {
                string zhDistIds = "";
                using (DataTable dtZhDist = c.GetDataTable("Select DistrictId From ZonalHeadDistricts Where ZonalHdId=" + ddrZH.SelectedValue))
                {
                    if (dtZhDist.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtZhDist.Rows)
                        {
                            if (zhDistIds != "")
                                zhDistIds = zhDistIds + "," + row["DistrictId"].ToString();
                            else
                                zhDistIds = row["DistrictId"].ToString();
                        }

                        c.FillComboBox("DistHdName", "DistHdId", "DistrictHead", "DelMark=0 AND DistHdDistrictId IN (" + zhDistIds + ")", "DistHdName", 0, ddrDH);

                    }
                }
                //c.FillComboBox("DistHdName", "DistHdId", "DistrictHead", "DelMark=0 AND DistHdDistrictId IN (" + zhDistIds + ")", "DistHdName", 0, ddrDH);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "ddrZh_SelectedIndexChanged", ex.Message.ToString());
            return;
        }
    }
    private void FillGrid()
    {
        try
        {
            using (DataTable dtShops = c.GetDataTable("Select a.FranchID, a.FranchShopCode, a.FranchName, a.FranchMobile, a.FranchPassword, a.FranchAddress, isnull(b.CityName, 'NA') as CityName From FranchiseeData a Left Join CityData b On a.FK_FranchCityId=b.CityID Where a.FranchActive=1"))
            {
                gvShops.DataSource = dtShops;
                gvShops.DataBind();

                if (dtShops.Rows.Count > 0)
                {
                    gvShops.UseAccessibleHeader = true;
                    gvShops.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void ddrState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Fill Dropdown list of Sub category selection
            if (ddrState.SelectedIndex > 0)
            {
                //c.FillComboBox("CityName", "CityID", "CityData", "FK_StateID=" + ddrState.SelectedValue, "CityName", 0, ddrCity);
                c.FillComboBox("DistrictName", "DistrictId", "DistrictsData", "StateId=" + ddrState.SelectedValue, "DistrictName", 0, ddrDist);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "ddrState_SelectedIndexChanged", ex.Message.ToString());
            return;
        }
    }

    protected void ddrDist_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Fill Dropdown list of Sub category selection
            if (ddrDist.SelectedIndex > 0)
            {
                c.FillComboBox("CityName", "CityID", "CityData", "FK_DistId=" + ddrDist.SelectedValue, "CityName", 0, ddrCity);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "ddrDist_SelectedIndexChanged", ex.Message.ToString());
            return;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {

            //Single quote filter
            GetAllControls(this.Controls);

            //Empty fields validation
            if (txtShopName.Text == "" || txtOwner.Text == "" || txtShopCode.Text == "" || txtRegDate.Text == "" || 
                txtMobileNo.Text == "" || txtEmail.Text == "" || ddrState.SelectedIndex == 0 || ddrCity.SelectedIndex == 0 || 
                ddrDist.SelectedIndex == 0 || txtPinCode.Text == "" || txtAddress.Text == "" || txtLatLongs.Text == "" || 
                txtPassword.Text == "" || txtBankName.Text == "" || txtBranch.Text == "" || txtBankAccName.Text == "" || 
                txtAccNo.Text == "" || txtIfsc.Text == "" || ddrZH.SelectedIndex == 0 || ddrDH.SelectedIndex == 0) 
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * fields are mandatory');", true);
                return;
            }
            //Email Id regular expression validation
            if (!c.EmailAddressCheck(txtEmail.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter correct email id');", true);
                return;
            }

            if (!c.ValidateMobile(txtMobileNo.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter valid mobile number');", true);
                return;
            }

            if (txtPinCode.Text != "")
            {
                if (!c.IsNumeric(txtPinCode.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Pincode must be numeric value');", true);
                    return;
                }
            }

            if (txtAddress.Text != "")
            {
                if (txtAddress.Text.Length > 300)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Address must be less than 300 characters');", true);
                    return;
                }
            }

            DateTime regDate = DateTime.Now;
            string[] arrTDate = txtRegDate.Text.Split('/');
            if (c.IsDate(arrTDate[1] + "/" + arrTDate[0] + "/" + arrTDate[2]) == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid Registration Date');", true);
                return;
            }
            else
            {
                regDate = Convert.ToDateTime(arrTDate[1] + "/" + arrTDate[0] + "/" + arrTDate[2]);
            }

            int maxId = lblId.Text == "[New]" ? c.NextId("FranchiseeData", "FranchID") : Convert.ToInt16(lblId.Text);

            //string imgName = "";

            //if (fuImg.HasFile)
            //{
            //    string fExt = Path.GetExtension(fuImg.FileName).ToLower();
            //    if (fExt == ".png" || fExt == ".jpg" || fExt == ".jpeg")
            //    {
            //        imgName = "doctor-" + maxId + fExt;
            //        fuImg.SaveAs(Server.MapPath("~/upload/doctors/") + imgName);
            //    }
            //    else
            //    {
            //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Only .png, .jpg and .jpeg files are allowed');", true);
            //        return;
            //    }
            //}
            // Insert / Update data into database 


            int legalBlock = chkLegal.Checked == true ? 1 : 0;
            //int docExp = txtExp.Text == "" ? 0 : Convert.ToInt32(txtExp.Text);

            if (lblId.Text == "[New]")
            {
                if (c.IsRecordExist("Select FranchID From FranchiseeData Where FranchShopCode='" + txtShopCode.Text + "' And FranchActive=1"))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Shop with this shop code already exists');", true);
                    return;
                }
            }
            else
            {
                if (c.IsRecordExist("Select FranchID From FranchiseeData Where FranchShopCode='" + txtShopCode.Text + "' And FranchActive=1 And FranchID<>" + lblId.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Shop with this shop code already exists');", true);
                    return;
                }
            }

            if (lblId.Text == "[New]")
            {
                c.ExecuteQuery("Insert Into FranchiseeData (FranchID, FranchRegDate, FranchShopCode, FranchName, FranchOwnerName, FK_FranchStateId, FK_FranchCityId, " +
                    " FranchPinCode, FranchAddress, FranchLatLong, FranchEmail, FranchMobile, FranchPassword, FranchBankName, FranchBankBranch, FranchBankAccName, " +
                    " FranchBankAccNum, FranchBankIFSC, FranchActive, FranchLegalBlock, FK_FranchDistId) Values(" + maxId + ", '" + regDate + "', '" + txtShopCode.Text + "','" + txtShopName.Text +
                    "', '" + txtOwner.Text + "', " + ddrState.SelectedValue + ", " + ddrCity.SelectedValue + ", '" + txtPinCode.Text + "', '" + txtAddress.Text +
                    "', '" + txtLatLongs.Text + "', '" + txtEmail.Text + "', '" + txtMobileNo.Text + "', '" + txtPassword.Text +
                    "', '" + txtBankName.Text + "', '" + txtBranch.Text + "', '" + txtBankAccName.Text + "', '" + txtAccNo.Text + "', '" + txtIfsc.Text + "', 1, " + legalBlock + ", " + ddrDist.SelectedValue + ")");

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Franchisee Info Added');", true);

            }
            else
            {
                c.ExecuteQuery("Update FranchiseeData Set FranchRegDate='" + regDate + "', FranchShopCode='" + txtShopCode.Text + "', FranchName='" + txtShopName.Text +
                    "', FranchOwnerName='" + txtOwner.Text + "',  FK_FranchStateId=" + ddrState.SelectedValue + ", FK_FranchCityId=" + ddrCity.SelectedValue +
                    ", FranchPinCode='" + txtPinCode.Text + "', FranchAddress='" + txtAddress.Text + "', FranchLatLong='" + txtLatLongs.Text +
                    "', FranchEmail='" + txtEmail.Text + "', FranchMobile='" + txtMobileNo.Text + "', FranchPassword='" + txtPassword.Text +
                    "', FranchBankName='" + txtBankName.Text + "', FranchBankBranch='" + txtBranch.Text + "', FranchBankAccName='" + txtBankAccName.Text +
                    "', FranchBankAccNum='" + txtAccNo.Text + "', FranchBankIFSC='" + txtIfsc.Text + "', FranchLegalBlock=" + legalBlock +
                    ", FK_FranchDistId=" + ddrDist.SelectedValue + ", FK_ZonalHdId=" + ddrZH.SelectedValue +
                    ", FK_DistHdId=" + ddrDH.SelectedValue + " Where FranchID=" + maxId);

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Franchisee Info updated');", true);

            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('shop-list.aspx', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSave_Click", ex.Message.ToString());
            return;

        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            c.ExecuteQuery("Update FranchiseeData Set FranchActive=0 Where FranchID=" + Request.QueryString["id"]);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Franchisee Info Deleted');", true);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('doctor-master.aspx', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnDelete_Click", ex.Message.ToString());
            return;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["from"] != null)
        {
            Response.Redirect("assign-heads-shop.aspx", false);
        }
        else
        {
            Response.Redirect("shop-list.aspx", false);
        }
    }

    public void GetAllControls(ControlCollection ctls)
    {
        foreach (Control c in ctls)
        {
            if (c is System.Web.UI.WebControls.TextBox)
            {
                TextBox tt = c as TextBox;
                //to do something by using textBox tt.
                tt.Text = tt.Text.Trim().Replace("'", "");
            }
            if (c.HasControls())
            {
                GetAllControls(c.Controls);

            }
        }
    }

    protected void gvShops_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"shop-list.aspx?action=edit&id=" + e.Row.Cells[0].Text + "\" class=\"gAnch\" title=\"View/Edit\"></a>";
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvShops_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    private void GetShopInfo(int frIdX)
    {
        try
        {
            using (DataTable dtFr = c.GetDataTable("Select * From FranchiseeData Where FranchID=" + frIdX))
            {
                if (dtFr.Rows.Count > 0)
                {
                    DataRow row = dtFr.Rows[0];

                    lblId.Text = frIdX.ToString();

                    txtShopName.Text = row["FranchName"].ToString();
                    txtOwner.Text = row["FranchOwnerName"].ToString();
                    txtShopCode.Text = row["FranchShopCode"].ToString();
                    txtRegDate.Text = Convert.ToDateTime(row["FranchRegDate"]).ToString("dd/MM/yyyy");
                    txtMobileNo.Text = row["FranchMobile"].ToString();
                    txtEmail.Text = row["FranchEmail"].ToString();
                    txtPinCode.Text = row["FranchPinCode"].ToString();
                    txtAddress.Text = row["FranchAddress"].ToString();
                    txtLatLongs.Text = row["FranchLatLong"].ToString();
                    txtPassword.Text = row["FranchPassword"].ToString();
                    txtBankName.Text = row["FranchBankName"].ToString();
                    txtBranch.Text = row["FranchBankBranch"].ToString();
                    txtBankAccName.Text = row["FranchBankAccName"].ToString();
                    txtAccNo.Text = row["FranchBankAccNum"].ToString();
                    txtIfsc.Text = row["FranchBankIFSC"].ToString();
                    ddrState.SelectedValue = row["FK_FranchStateId"].ToString();
                    c.FillComboBox("DistrictName", "DistrictId", "DistrictsData", "StateId=" + ddrState.SelectedValue, "DistrictName", 0, ddrDist);
                    ddrDist.SelectedValue = row["FK_FranchDistId"].ToString();
                    c.FillComboBox("CityName", "CityID", "CityData", "FK_DistId=" + ddrDist.SelectedValue, "CityName", 0, ddrCity);
                    ddrCity.SelectedValue = row["FK_FranchCityId"].ToString();
                    chkLegal.Checked = row["FranchLegalBlock"].ToString() == "1" ? true : false;

                    if (row["FK_ZonalHdId"] != DBNull.Value && row["FK_ZonalHdId"] != null && row["FK_ZonalHdId"].ToString() != "")
                    {
                        ddrZH.SelectedValue = row["FK_ZonalHdId"].ToString();

                        string zhDistIds = "";
                        using (DataTable dtZhDist = c.GetDataTable("Select DistrictId From ZonalHeadDistricts Where ZonalHdId=" + ddrZH.SelectedValue))
                        {
                            if (dtZhDist.Rows.Count > 0)
                            {
                                foreach (DataRow zrow in dtZhDist.Rows)
                                {
                                    if (zhDistIds != "")
                                        zhDistIds = zhDistIds + "," + zrow["DistrictId"].ToString();
                                    else
                                        zhDistIds = zrow["DistrictId"].ToString();
                                }
                            }
                        }
                        c.FillComboBox("DistHdName", "DistHdId", "DistrictHead", "DelMark=0 AND DistHdDistrictId IN (" + zhDistIds + ")", "DistHdName", 0, ddrDH);

                        ddrDH.SelectedValue = row["FK_DistHdId"].ToString();
                    }

                    //if (row["FranchAadharCard"] != DBNull.Value && row["FranchAadharCard"] != null && row["FranchAadharCard"].ToString() != "")
                    //{
                    //    frAdhar = "<img src=\"\" />"
                    //}
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetShopInfo", ex.Message.ToString());
            return;
        }
    }
}