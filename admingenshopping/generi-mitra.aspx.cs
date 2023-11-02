using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admingenshopping_generi_mitra : System.Web.UI.Page
{
    public string pgTitle, errMsg, disImg, pancardImg, adharcardImg, bankdocImg;
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["action"] != null)
            {
                // Fill State
                c.FillComboBox("StateName", "StateID", "StatesData", "", "StateName", 0, ddrState);

                if (Request.QueryString["action"] == "edit")
                {
                    editGeneriMitra.Visible = true;
                    //viewGeneriMitra.Visible = false;
                    

                    pgTitle = "Edit Generi Mitra";
                    ButtonVisibility();
                    GetGeneriMitraData(Convert.ToInt32(Request.QueryString["id"]));
                }

            }
            else
            {
               // viewGeneriMitra.Visible = true;
                editGeneriMitra.Visible = false;
                FillGrid();
            }

        }
    }
    private void ButtonVisibility()
    {
        try
        {
            if (Request.QueryString["action"] == "edit")
            {
                int gmId = Convert.ToInt32(Request.QueryString["id"]);
                if (c.IsRecordExist("Select GMitraID From GenericMitra Where GMitraID=" + gmId + " And GMitraStatus=1 "))
                {
                    btnBlock.Text = "Block";
                }
                else if (c.IsRecordExist("Select GMitraID From GenericMitra Where GMitraID=" + gmId + " And GMitraStatus=2"))
                {
                    btnBlock.Text = "Unblock";
                }
                
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "ButtonVisibility", ex.Message.ToString());
            return;
        }
    }
    private void FillGrid()
    {
        try
        {
            using (DataTable dtGeneriMitra = c.GetDataTable("Select a.GMitraID, a.GMitraStatus, Convert(varchar(20), a.GMitraDate, 103) as GMitraDate, a.GMitraName, a.GMitraMobile, b.StateName, c.DistrictName, d.CityName From GenericMitra a Inner Join StatesData b On a.FK_StateID = b.StateID Inner Join DistrictsData c On a.FK_DistrictID = c.DistrictId Inner Join CityData d On a.FK_CityID = d.CityID "))
            {
                //gvGeneriMitra.DataSource = dtGeneriMitra;
                //gvGeneriMitra.DataBind();
                //if (gvGeneriMitra.Rows.Count > 0)
                //{
                //    gvGeneriMitra.UseAccessibleHeader = true;
                //    gvGeneriMitra.HeaderRow.TableSection = TableRowSection.TableHeader;
                //}
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FillGrid", ex.Message.ToString());
            return;
        }
    }

    protected void gvGeneriMitra_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"generi-mitra.aspx?action=edit&id=" + e.Row.Cells[0].Text + "\" class=\"gAnch\"></a>";

                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                int status = Convert.ToInt32(e.Row.Cells[1].Text);
                switch (status)
                {
                    case 0:
                        litStatus.Text = "<span class=\"stDogerBlue\">New</span>";
                        break;
                    case 1:
                        litStatus.Text = "<span class=\"stGreen\">Active</span>";
                        break;
                    case 2:
                        litStatus.Text = "<span class=\"stGrey\">Blocked</span>";
                        break;
                    case 3:
                        litStatus.Text = "<span class=\"stRed\">Deleted</span>";
                        break;

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvGeneriMitra_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    private void GetGeneriMitraData(int gmIdx)
    {
        try
        {
            using (DataTable dtGenMitra = c.GetDataTable("Select * From GenericMitra Where GMitraID=" + gmIdx + ""))
            {
                if (dtGenMitra.Rows.Count > 0)
                {
                    DataRow row = dtGenMitra.Rows[0];
                    lblId.Text = gmIdx.ToString();

                    txtName.Text = row["GMitraName"].ToString();
                    txtMobile.Text = row["GMitraMobile"].ToString();
                    txtEmail.Text = row["GMitraEmail"].ToString();
                    txtUserName.Text = row["GMitraLogin"].ToString();
                    txtPassword.Text = row["GMitraPassword"].ToString();
                    txtBankName.Text = row["GMitraBankName"].ToString();
                    txtAccName.Text = row["GMitraBankAccName"].ToString();
                    txtAccNo.Text = row["GMitraBankAccNumber"].ToString();
                    txtIfsc.Text = row["GMitraBankIFSC"].ToString();
                    txtPan.Text = row["GMitraPanCard"].ToString();
                    ddrState.SelectedValue = row["FK_StateID"].ToString();

                    c.FillComboBox("DistrictName", "DistrictId", "DistrictsData", "StateId=" + ddrState.SelectedValue + "", "DistrictName", 0, ddrDistrict);
                    ddrDistrict.SelectedValue = row["FK_DistrictID"].ToString();

                    // Fill City
                    c.FillComboBox("CityName", "CityID", "CityData", "FK_DistId=" + ddrDistrict.SelectedValue + "", "CityName", 0, ddrCity);
                    ddrCity.SelectedValue = row["FK_CityID"].ToString();
                    int gmStatus = Convert.ToInt32(row["GMitraStatus"].ToString());
                    switch (gmStatus)
                    {
                        case 0:
                            btnBlock.Visible = false;
                            break;
                        case 1:
                            btnBlock.Visible = true;
                            chkApprove.Checked = true;
                            chkApprove.Enabled = false;
                            break;
                        case 2:
                            btnBlock.Visible = true;
                            chkApprove.Checked = true;
                            chkApprove.Enabled = false;
                            break;
                        
                    }

                    if (row["GMitraPan"] != DBNull.Value && row["GMitraPan"] != null && row["GMitraPan"].ToString() != "")
                    {
                        if (row["GMitraPan"].ToString().Contains(".pdf"))
                        {
                            pancardImg = "<a href=\"" + Master.rootPath + "upload/genmitradocs/" + row["GMitraPan"].ToString() + "\" target=\"_blank\" ><i class=\"fas fa-file-pdf\" ></i>View Doc</a>";
                        }
                        else
                        {
                            pancardImg = "<a href=\"" + Master.rootPath + "upload/genmitradocs/" + row["GMitraPan"].ToString() + "\"  data-fancybox=\"imgGroup1\"><img src=\"" + Master.rootPath + "upload/genmitradocs/" + row["GMitraPan"].ToString() + "\" width=\"200\"/></a>";
                        }
                    }

                    if (row["GMitraAdhar"] != DBNull.Value && row["GMitraAdhar"] != null && row["GMitraAdhar"].ToString() != "")
                    {
                        if (row["GMitraAdhar"].ToString().Contains(".pdf"))
                        {
                            adharcardImg = "<a href=\"" + Master.rootPath + "upload/genmitradocs/" + row["GMitraAdhar"].ToString() + "\" target=\"_blank\"><i class=\"fas fa-file-pdf\" ></i>View Doc</a>";
                        }
                        else
                        {
                            adharcardImg = "<a href=\"" + Master.rootPath + "upload/genmitradocs/" + row["GMitraAdhar"].ToString() + "\"  data-fancybox=\"imgGroup1\"><img src=\"" + Master.rootPath + "upload/genmitradocs/" + row["GMitraAdhar"].ToString() + "\" width=\"200\" /></a>";
                        }
                    }

                    if (row["GMitraBankDoc"] != DBNull.Value && row["GMitraBankDoc"] != null && row["GMitraBankDoc"].ToString() != "")
                    {
                        if (row["GMitraBankDoc"].ToString().Contains(".pdf"))
                        {
                            bankdocImg = "<a href=\"" + Master.rootPath + "upload/genmitradocs/" + row["GMitraBankDoc"].ToString() + "\" target=\"_blank\"><i class=\"fas fa-file-pdf\" ></i>View Doc</a>";
                        }
                        else
                        {
                            bankdocImg = "<a href=\"" + Master.rootPath + "upload/genmitradocs/" + row["GMitraBankDoc"].ToString() + "\"  data-fancybox=\"imgGroup1\"><img src=\"" + Master.rootPath + "upload/genmitradocs/" + row["GMitraBankDoc"].ToString() + "\" width=\"200\" /></a>";
                        }
                    }

                    if (row["GMitraShopCode"] != DBNull.Value && row["GMitraShopCode"] != null && row["GMitraShopCode"].ToString() != "")
                    {
                        txtShopCode.Text = row["GMitraShopCode"].ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetGeneriMitraData", ex.Message.ToString());
            return;
        }
    }


    protected void ddrState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // Fill District
            c.FillComboBox("DistrictName", "DistrictId", "DistrictsData", "StateId=" + ddrState.SelectedValue + "", "DistrictName", 0, ddrDistrict);
            GetPageInfo();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "ddrState_SelectedIndexChanged", ex.Message.ToString());
            return;
        }
    }

    protected void ddrDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // Fill City
            c.FillComboBox("CityName", "CityID", "CityData", "FK_DistId=" + ddrDistrict.SelectedValue + "", "CityName", 0, ddrCity);
            GetPageInfo();


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "ddrDistrict_SelectedIndexChanged", ex.Message.ToString());
            return;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtName.Text == "" || txtMobile.Text == "" || txtEmail.Text == "" || txtUserName.Text == "" || txtPassword.Text == "" || txtBankName.Text == "" || txtAccName.Text == "" || txtAccNo.Text == "" || txtIfsc.Text == "" || txtPan.Text == "" || ddrState.SelectedIndex == 0 || ddrDistrict.SelectedIndex == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * fields are mandatory');", true);
                return;
            }
            int gmId = Convert.ToInt32(lblId.Text);

            if (c.IsRecordExist("Select GMitraID From GenericMitra Where GMitraMobile='" + txtMobile.Text + "' And GMitraID<>" + gmId + " AND GMitraStatus=1")) 
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'User Is Allready Registered');", true);
                return;
            }

            int gmStatus = chkApprove.Checked == true ? 1 : 0;

            c.ExecuteQuery("Update GenericMitra Set GMitraName='" + txtName.Text + "', GMitraMobile='" + txtMobile.Text + "', GMitraEmail='" + txtEmail.Text + "', GMitraLogin='" + txtUserName.Text + "', GMitraPassword='" + txtPassword.Text + "', GMitraBankName='" + txtBankName.Text + "', GMitraBankAccName='" + txtAccName.Text + "', GMitraBankAccNumber='" + txtAccNo.Text + "', GMitraBankIFSC='" + txtIfsc.Text + "', GMitraPanCard='" + txtPan.Text + "', FK_StateID=" + ddrState.SelectedValue + ", FK_DistrictID=" + ddrDistrict.SelectedValue + ", FK_CityID=" + ddrCity.SelectedValue + ", GMitraStatus=" + gmStatus + ", GMitraShopCode='" + txtShopCode.Text + "' Where GMitraID=" + gmId + "");
            GetPageInfo();
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Information Updated Successfully');", true);

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('generi-mitra.aspx', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSave_Click", ex.Message.ToString());
            return;
        }
    }

    private void GetPageInfo()
    {
        try
        {
            pgTitle = "Edit Generi Mitra";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetPageInfo", ex.Message.ToString());
            return;
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            //Status : 3 --> Deleted
            c.ExecuteQuery("Update GenericMitra Set GMitraStatus=3 Where GMitraID=" + Convert.ToInt32(Request.QueryString["id"]) + "");
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Generi Mitra deleted successfully');", true);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('generi-mitra.aspx', 2000);", true);

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnDelete_Click", ex.Message.ToString());
            return;
        }
    }

    protected void btnBlock_Click(object sender, EventArgs e)
    {
        try
        {
            int gmId = Convert.ToInt32(Request.QueryString["id"]);
            int gmStatus =Convert.ToInt16(c.GetReqData("GenericMitra", "GMitraStatus", "GMitraID="+ gmId +""));
            switch (gmStatus)
            {
                case 1 :
                    c.ExecuteQuery("Update GenericMitra Set GMitraStatus=2 Where GMitraID=" + gmId + "");
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'User blocked successfully');", true);

                    break;
                case 2:
                    c.ExecuteQuery("Update GenericMitra Set GMitraStatus=1 Where GMitraID=" + gmId + "");
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'User unblocked successfully');", true);
                    break;
            }

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('generi-mitra.aspx', 2000);", true);

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnBlock_Click", ex.Message.ToString());
            return;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("generi-mitra.aspx");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnCancel_Click", ex.Message.ToString());
            return;
        }
    }
}