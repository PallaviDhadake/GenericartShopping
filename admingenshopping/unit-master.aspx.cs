using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admingenshopping_unit_master : System.Web.UI.Page
{
    public string pgTitle, errMsg, videoPreview;
    iClass c = new iClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        pgTitle = Request.QueryString["action"] == "new" ? "Add Unit Info" : "Edit Unit Info";
        btnSave.Attributes.Add("onclick", "this.disabled=true; this.value='Processing...';" + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        btnDelete.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnDelete, null) + ";");
        btnCancel.Attributes.Add("onclick", "this.disabled=true; this.value='Processing...';" + ClientScript.GetPostBackEventReference(btnCancel, null) + ";");

        if (!IsPostBack)
        {
            if (Request.QueryString["action"] != null)
            {
                editProf.Visible = true;
                viewprof.Visible = false;

                if (Request.QueryString["action"] == "new")
                {
                    btnSave.Text = "Save Info";
                    btnDelete.Visible = false;

                }
                else
                {
                    btnSave.Text = "Modify Info";
                    btnDelete.Visible = true;
                    GetUnitData(Convert.ToInt32(Request.QueryString["id"]));
                }
            }
            else
            {
                viewprof.Visible = true;
                editProf.Visible = false;
                FillGrid();
            }

            txtUnitName.Focus();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            //Single quote filter
            txtUnitName.Text = txtUnitName.Text.Trim().Replace("'", "");

            //Empty fields validation
            if (txtUnitName.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Please fill the unit name');", true);
                
                return;
            }
            // Insert / Update data into database 
            int maxId = lblId.Text == "[New]" ? c.NextId("UnitProducts", "UnitID") : Convert.ToInt16(lblId.Text);

            if (lblId.Text == "[New]")
            {
                c.ExecuteQuery("Insert Into UnitProducts (UnitID, UnitName, delMark) Values(" + maxId + ", '" + txtUnitName.Text + "', 0)");
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Unit info added successfully');", true);
            }
            else
            {
                c.ExecuteQuery("Update UnitProducts Set UnitName='" + txtUnitName.Text + "' Where UnitID=" + maxId);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Unit info updated successfully');", true);
            }
            
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('unit-master.aspx', 2000);", true);

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
            if (c.IsRecordExist("Select ProductID From ProductsData Where FK_UnitID=" + Convert.ToInt32(Request.QueryString["id"])))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'You Can't Delete this record, its reference exists in ProductData');", true);
               
                return;
            }
            else
            {
                c.ExecuteQuery("Delete From UnitProducts Where UnitID=" + Convert.ToInt32(Request.QueryString["id"]));
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Unit deleted successfully');", true);
                
                Response.Redirect("unit-master.aspx");
                FillGrid();
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

        Response.Redirect("unit-master.aspx");
    }

    private void FillGrid()
    {
        try
        {
            using (DataTable dtUnit = c.GetDataTable("Select * From UnitProducts Where delMark=0 Order By UnitID DESC"))
            {
                gvUnit.DataSource = dtUnit;
                gvUnit.DataBind();
                if (dtUnit.Rows.Count > 0)
                {
                    gvUnit.UseAccessibleHeader = true;
                    gvUnit.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvUnit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"unit-master.aspx?action=edit&id=" + e.Row.Cells[0].Text + "\" class=\"gAnch\" title=\"View/Edit\"></a>";


            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvUnit_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    private void GetUnitData(int Idx)
    {
        try
        {
            using (DataTable dtSupplier = c.GetDataTable("Select * From UnitProducts Where UnitID=" + Idx))
            {
                if (dtSupplier.Rows.Count > 0)
                {
                    DataRow bRow = dtSupplier.Rows[0];
                    lblId.Text = Idx.ToString();

                    txtUnitName.Text = bRow["UnitName"].ToString();

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetUnitData", ex.Message.ToString());
            return;
        }
    }

}