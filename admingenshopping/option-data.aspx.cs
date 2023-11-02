using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class admingenshopping_option_data : System.Web.UI.Page
{
    public string pgTitle, errMsg;
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSave.Attributes.Add("onclick", "this.disabled=true; this.value='Processing...';" + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        btnDelete.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnDelete, null) + ";");
        btnCancel.Attributes.Add("onclick", "this.disabled=true; this.value='Processing...';" + ClientScript.GetPostBackEventReference(btnCancel, null) + ";");

        if (!IsPostBack)
        {
            //  pgTitle = lblId.Text == "[New]" ? "Add Option Info" : "Edit Option Info";
            //Fill Dropdown list of Parent category selection
            c.FillComboBox("OptionGroupName", "OptionGroupID", "OptionGroups", "", "OptionGroupName", 0, ddrOptGroup);

            if (Request.QueryString["id"] != null)
            {
                pgTitle = "Edit Option Info";
                btnSave.Text = "Modify Info";
                btnDelete.Visible = true;
                GetOptionData(Convert.ToInt32(Request.QueryString["id"]));
            }
            else
            {
                pgTitle = "Add Option Info";
                btnSave.Text = "Save Info";
                btnDelete.Visible = false;
                FillGrid();
            }

            FillGrid();
            ddrOptGroup.Focus();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            //Single quote filter
            txtOptName.Text = txtOptName.Text.Trim().Replace("'", "");

            //Empty fields validation
            if (txtOptName.Text == "" || ddrOptGroup.SelectedIndex == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * field are mandatory');", true);
                //ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message:'All * fields are mandetory',type:'warning'});", true);
                txtOptName.Focus();
                return;
            }

            // Insert / Update data into database 
            int maxId = lblId.Text == "[New]" ? c.NextId("OptionsData", "OptionID") : Convert.ToInt16(lblId.Text);

            if (lblId.Text == "[New]")
            {
                if (c.IsRecordExist("Select OptionID From OptionsData Where FK_OptionGroupID=" + ddrOptGroup.SelectedValue + " AND OptionName='" + txtOptName.Text + "'"))
                {
                    FillGrid();
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Option Already Exists');", true);
                    return;
                }
            }
            else
            {
                if (c.IsRecordExist("Select OptionID From OptionsData Where FK_OptionGroupID=" + ddrOptGroup.SelectedValue + " AND OptionName='" + txtOptName.Text + "' AND OptionID<>'" + lblId.Text + "'"))
                {
                    FillGrid();
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Option Already Exists');", true);
                    return;
                }
            }

            string dispOrder = c.returnAggregate("Select MAX(OptionDisplayOrder) From OptionsData Where FK_OptionGroupID=" + ddrOptGroup.SelectedValue).ToString();
            int displayOrder = 0;
            if (dispOrder != "")
            {
                displayOrder = Convert.ToInt32(dispOrder) + 1;
            }
            else
            {
                displayOrder = 0;
            }

            if (lblId.Text == "[New]")
            {
                c.ExecuteQuery("Insert Into OptionsData(OptionID, OptionName, FK_OptionGroupID, OptionDisplayOrder) Values(" + maxId + ", '" + txtOptName.Text + "'," + ddrOptGroup.SelectedValue + ", " + displayOrder + ")");
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Option added');", true);
            }
            else
            {
                c.ExecuteQuery("Update OptionsData Set OptionName='" + txtOptName.Text + "', FK_OptionGroupID=" + ddrOptGroup.SelectedValue + ", OptionDisplayOrder=" + displayOrder + " Where OptionID=" + maxId);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Option updated');", true);
            }
            // Show fresh updated Gridview data
            FillGrid();
            //Clear text fields
            ResetControl();
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('option-data.aspx', 2000);", true);
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
            if (c.IsRecordExist("Select ProdOptionID From ProductOptions Where FK_OptionID=" + Convert.ToInt32(Request.QueryString["id"])))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'You cant delete this product, its reference exist in product options');", true);
                return;
            }
            else
            {
                int dispOrder = Convert.ToInt32(c.GetReqData("OptionsData", "OptionDisplayOrder", "OptionID=" + Request.QueryString["id"]));
                if (dispOrder > 0)
                {
                    string maxDispOrder = c.returnAggregate("Select MAX(OptionDisplayOrder) From OptionsData Where FK_OptionGroupID=" + ddrOptGroup.SelectedValue).ToString();
                    if (dispOrder == Convert.ToInt32(maxDispOrder))
                    {
                        c.ExecuteQuery("Delete From OptionsData Where OptionID=" + Request.QueryString["id"]);
                    }
                    string optionIds = "";
                    using (DataTable dtDispOrderIds = c.GetDataTable("Select OptionID From OptionsData Where FK_OptionGroupID=" + ddrOptGroup.SelectedValue + " AND OptionDisplayOrder>" + dispOrder))
                    {
                        if (dtDispOrderIds.Rows.Count > 0)
                        {
                            foreach (DataRow row in dtDispOrderIds.Rows)
                            {
                                if (optionIds == "")
                                    optionIds = row["OptionID"].ToString();
                                else
                                    optionIds = optionIds + ", " + row["OptionID"].ToString();
                            }

                            c.ExecuteQuery("Update OptionsData Set OptionDisplayOrder=OptionDisplayOrder-1 Where OptionID IN (" + optionIds + ")");
                            c.ExecuteQuery("Delete From OptionsData Where OptionID=" + Request.QueryString["id"]);
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'option deleted successfully');", true);
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('option-data.aspx', 2000);", true);
                        }
                    }
                }
                //c.ExecuteQuery("Delete From OptionsData Where OptionID=" + Convert.ToInt32(Request.QueryString["id"]));
                //ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'option deleted successfully');", true);
                //ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Category deleted successfully', type: 'success'});", true);
                FillGrid();
                ResetControl();
            }
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
        if (Request.QueryString["action"] == "new")
        {
            Response.Redirect("dashboard.aspx");
        }
        else
        {
            Response.Redirect("option-data.aspx");
        }
    }

    //Show Data into Gridview starts here
    private void FillGrid()
    {
        try
        {
            string strQuery = "";

            if (Convert.ToInt32(ddrOptGroup.SelectedValue) > 0)
            {
                int optGrpId = Convert.ToInt32(ddrOptGroup.SelectedValue);
                strQuery = "Select a.OptionID, a.OptionName, a.OptionDisplayOrder, " +
                           "(Select OptionGroupName From OptionGroups Where OptionGroupID = a.FK_OptionGroupID) as optionGroup " +
                           "From OptionsData a Where FK_OptionGroupID=" + optGrpId + " Order By OptionDisplayOrder";
            }
            else
            {
                strQuery = "Select a.OptionID, a.OptionName, a.OptionDisplayOrder, " +
                "(Select OptionGroupName From OptionGroups Where OptionGroupID = a.FK_OptionGroupID) as optionGroup " +
                "From OptionsData a Order By OptionDisplayOrder";
            }

            using (DataTable dtOption = c.GetDataTable(strQuery))
            {
                gvOption.DataSource = dtOption;
                gvOption.DataBind();
                if (gvOption.Rows.Count > 0)
                {
                    gvOption.UseAccessibleHeader = true;
                    gvOption.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvOption_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"option-data.aspx?action=edit&id=" + e.Row.Cells[0].Text + "\" class=\"gAnch\" title=\"View/Edit\"></a>";

                Button btnUp = (Button)e.Row.FindControl("moveUp");
                if (e.Row.Cells[3].Text == "1")
                {
                    btnUp.Enabled = false;
                    btnUp.Attributes["style"] = "background:none;";
                }

                Button btnDown = (Button)e.Row.FindControl("moveDown");
                int maxOrd = Convert.ToInt32(c.returnAggregate("Select MAX(OptionDisplayOrder) From OptionsData Where FK_OptionGroupID=" + ddrOptGroup.SelectedValue));
                if (Convert.ToInt32(e.Row.Cells[3].Text) == maxOrd)
                {
                    btnDown.Visible = false;
                }
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvOption_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    private void GetOptionData(int Idx)
    {
        try
        {
            using (DataTable dtOption = c.GetDataTable("Select * From OptionsData Where OptionID=" + Idx))
            {
                if (dtOption.Rows.Count > 0)
                {
                    DataRow bRow = dtOption.Rows[0];
                    lblId.Text = Idx.ToString();
                    txtOptName.Text = bRow["OptionName"].ToString();
                    ddrOptGroup.SelectedValue = bRow["FK_OptionGroupID"].ToString();
                }
                txtOptName.Focus();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetOptionData", ex.Message.ToString());
            return;
        }
    }

    private void ResetControl()
    {
        txtOptName.Focus();
        txtOptName.Text = "";
        lblId.Text = "[New]";
    }

    protected void gvOption_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridViewRow gRow = (GridViewRow)((Button)e.CommandSource).NamingContainer;
            if (e.CommandName == "Up")
            {
                int displayOrd = Convert.ToInt32(gRow.Cells[3].Text);
                int previouRow = Convert.ToInt32(gRow.Cells[0].Text);
                c.ExecuteQuery("Update OptionsData Set OptionDisplayOrder=" + displayOrd + " Where FK_OptionGroupID=" + ddrOptGroup.SelectedValue + " AND OptionDisplayOrder=" + (displayOrd - 1));
                c.ExecuteQuery("Update OptionsData Set OptionDisplayOrder=" + (displayOrd - 1) + " Where OptionID=" + previouRow);
            }

            if (e.CommandName == "Down")
            {
                int displayOrd = Convert.ToInt32(gRow.Cells[3].Text);
                int previouRow = Convert.ToInt32(gRow.Cells[0].Text);
                c.ExecuteQuery("Update OptionsData Set OptionDisplayOrder=" + displayOrd + " Where FK_OptionGroupID=" + ddrOptGroup.SelectedValue + " AND OptionDisplayOrder=" + (displayOrd + 1));
                c.ExecuteQuery("Update OptionsData Set OptionDisplayOrder=" + (displayOrd + 1) + " Where OptionID=" + previouRow);
            }
            FillGrid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvOption_RowCommand", ex.Message.ToString());
            return;
        }
    }

    protected void ddrOptGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
        txtOptName.Text = "";
    }
}