using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class admingenshopping_option_group_master : System.Web.UI.Page
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
            if (Request.QueryString["id"] != null)
            {
                pgTitle = "Edit Option Group Info";
                btnSave.Text = "Modify Info";
                btnDelete.Visible = true;
                GetOptionGroupData(Convert.ToInt32(Request.QueryString["id"]));
            }
            else
            {
                pgTitle = "Add Option Group Info";
                btnSave.Text = "Save Info";
                btnDelete.Visible = false;
            }
            FillGrid();
            txtOptionGroup.Focus();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            //Single quote filter
            txtOptionGroup.Text = txtOptionGroup.Text.Trim().Replace("'", "");

            //Empty fields validation
            if (txtOptionGroup.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * field are mandatory');", true);               
                txtOptionGroup.Focus();
                pgTitle = lblId.Text == "[New]" ? "Add Option Group Info" : "Edit Option Group Info";
                return;
            }

            // Insert / Update data into database 
            int maxId = lblId.Text == "[New]" ? c.NextId("OptionGroups", "OptionGroupID") : Convert.ToInt16(lblId.Text);

            if (lblId.Text == "[New]")
            {
                if (c.IsRecordExist("Select OptionGroupID From OptionGroups Where OptionGroupName='" + txtOptionGroup.Text + "'"))
                {
                    FillGrid();
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Option group already exists');", true);
                    return;
                }
            }
            else
            {
                if (c.IsRecordExist("Select OptionGroupID From OptionGroups Where OptionGroupName='" + txtOptionGroup.Text + "' AND OptionGroupID<>" + lblId.Text))
                {
                    FillGrid();
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Option group already exists');", true);
                    return;
                }
            }

            if (lblId.Text == "[New]")
            {
                c.ExecuteQuery("Insert Into OptionGroups(OptionGroupID, OptionGroupName) Values(" + maxId + ", '" + txtOptionGroup.Text + "')");
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Option group added');", true);
            }
            else
            {
                c.ExecuteQuery("Update OptionGroups Set OptionGroupName='" + txtOptionGroup.Text + "' Where OptionGroupID=" + maxId);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Option group updated');", true);
            }
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('option-group-master.aspx', 2000);", true);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('option-group-master.aspx', 2000);", true);
            // Show fresh updated Gridview data
            FillGrid();
            //Clear text fields
            ResetControl();

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
            if (c.IsRecordExist("Select OptionID From OptionsData Where FK_OptionGroupID=" + Convert.ToInt32(Request.QueryString["id"])))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'You cant delete this record, its reference exist in option data');", true);
                return;
            }
            else if (c.IsRecordExist("Select ProdOptionID From ProductOptions Where FK_OptionGroupID=" + Convert.ToInt32(Request.QueryString["id"])))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'You cant delete this record, its reference exist in product options');", true);
                return;
            }
            else
            {
                c.ExecuteQuery("Delete From OptionGroups Where OptionGroupID=" + Convert.ToInt32(Request.QueryString["id"]));
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Option group deleted successfully');", true);
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('option-group-master.aspx', 2000);", true);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('option-group-master.aspx', 2000);", true);
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

        if (lblId.Text == "[New]")
        {
            Response.Redirect("dashboard.aspx");
        }
        else
        {
            Response.Redirect("option-group-master.aspx");
        }
    }

    //Show Data into Gridview starts here
    private void FillGrid()
    {
        try
        {
            using (DataTable dtOptGroup = c.GetDataTable("Select OptionGroupID, OptionGroupName From OptionGroups"))
            {
                gvOptionGroup.DataSource = dtOptGroup;
                gvOptionGroup.DataBind();
                if (gvOptionGroup.Rows.Count > 0)
                {
                    gvOptionGroup.UseAccessibleHeader = true;
                    gvOptionGroup.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvOptionGroup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"option-group-master.aspx?action=edit&id=" + e.Row.Cells[0].Text + "\" class=\"gAnch\" title=\"View/Edit\"></a>";
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvOptionGroup_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    protected void gvOptionGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvOptionGroup.PageIndex = e.NewPageIndex;
        FillGrid();
    }

    private void GetOptionGroupData(int Idx)
    {
        try
        {
            using (DataTable dtOptGroup = c.GetDataTable("Select * From OptionGroups Where OptionGroupID=" + Idx))
            {
                if (dtOptGroup.Rows.Count > 0)
                {
                    DataRow bRow = dtOptGroup.Rows[0];
                    lblId.Text = Idx.ToString();
                    txtOptionGroup.Text = bRow["OptionGroupName"].ToString();
                }
                txtOptionGroup.Focus();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetOptionGroupData", ex.Message.ToString());
            return;
        }
    }

    //Reset Page controls
    private void ResetControl()
    {
        txtOptionGroup.Focus();
        txtOptionGroup.Text = "";
        lblId.Text = "[New]";
    }
}