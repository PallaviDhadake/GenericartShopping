using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class supportteam_manufacturers : System.Web.UI.Page
{
    public string pgTitle, errMsg, videoPreview;
    iClass c = new iClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        pgTitle = Request.QueryString["action"] == "new" ? "Add Manufacturer Info" : "Edit Manufacturer Info";
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
                    GetSupplierData(Convert.ToInt32(Request.QueryString["id"]));
                }
            }
            else
            {
                viewprof.Visible = true;
                editProf.Visible = false;
                FillGrid();
            }

            txtMfgName.Focus();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            //Single quote filter
            txtMfgName.Text = txtMfgName.Text.Trim().Replace("'", "");
           
            //Empty fields validation
            if (txtMfgName.Text == "")
            {
              

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * field are Require');", true);
                return;
            }
            // Insert / Update data into database 
            int maxId = lblId.Text == "[New]" ? c.NextId("Manufacturers", "MfgId") : Convert.ToInt16(lblId.Text);

            if (lblId.Text == "[New]")
            {
                c.ExecuteQuery("Insert Into Manufacturers (MfgId, MfgName, delmark) Values(" + maxId + ",'" + txtMfgName.Text + "', 0)");

            

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Manufacturer info added');", true);

            }
            else
            {
                c.ExecuteQuery("Update Manufacturers Set MfgName='" + txtMfgName.Text + "' Where MfgId=" + maxId);

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Manufacturer info updated');", true);


            }
            //ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "waitAndMove('supplier-master.aspx', 2000);", true);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('manufacturers.aspx', 2000);", true);

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
            if (c.IsRecordExist("Select ProductID From ProductsData Where FK_MfgID=" + Convert.ToInt32(Request.QueryString["id"])))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'You Can't Delete this record, its reference exists in ProductData');", true);
                
                return;
            }
            else
            {
                c.ExecuteQuery("Delete From Manufacturers Where MfgId=" + Convert.ToInt32(Request.QueryString["id"]));
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Manufacturer deleted successfully');", true);
                
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

        Response.Redirect("dashboard.aspx");
    }

    private void FillGrid()
    {
        try
        {
            using (DataTable dtManufacturer = c.GetDataTable("Select * From Manufacturers Where delmark=0 Order By MfgId DESC"))
            {
                gvManufacturer.DataSource = dtManufacturer;
                gvManufacturer.DataBind();
                if (dtManufacturer.Rows.Count > 0)
                {
                    gvManufacturer.UseAccessibleHeader = true;
                    gvManufacturer.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvManufacturer_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"manufacturers.aspx?action=edit&id=" + e.Row.Cells[0].Text + "\" class=\"gAnch\" title=\"View/Edit\"></a>";

            
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvManufacturer_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    private void GetSupplierData(int Idx)
    {
        try
        {
            using (DataTable dtMfr = c.GetDataTable("Select * From Manufacturers Where MfgId=" + Idx))
            {
                if (dtMfr.Rows.Count > 0)
                {
                    DataRow bRow = dtMfr.Rows[0];
                    lblId.Text = Idx.ToString();

                    txtMfgName.Text = bRow["MfgName"].ToString();
                    
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetSupplierData", ex.Message.ToString());
            return;
        }
    }


}