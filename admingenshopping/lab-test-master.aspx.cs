using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class admingenshopping_lab_test_master : System.Web.UI.Page
{
    public string pgTitle, errMsg;
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["action"] != null)
        {
            if (Request.QueryString["action"] == "new")
            {
                pgTitle = "Add Lab Info";
            }
            else
            {
                pgTitle = "Edit Lab Info";
            }
        }
        else
        {
            pgTitle = "Lab Test Master";
        }
        btnSave.Attributes.Add("onclick", "this.disabled=true; this.value='Processing...';" + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        btnDelete.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnDelete, null) + ";");
        btnCancel.Attributes.Add("onclick", "this.disabled=true; this.value='Processing...';" + ClientScript.GetPostBackEventReference(btnCancel, null) + ";");

        if (!IsPostBack)
        {
            if (Request.QueryString["action"] != null)
            {
                //editProf.Visible = true;
                //viewprof.Visible = false;

                if (Request.QueryString["action"] == "new")
                {
                    btnSave.Text = "Save Info";
                    btnDelete.Visible = false;

                }
                else
                {
                    btnSave.Text = "Modify Info";
                    btnDelete.Visible = true;
                    GetLabData(Convert.ToInt32(Request.QueryString["id"]));
                }
            }
            else
            {
                //viewprof.Visible = true;
                //editProf.Visible = false;
                //FillGrid();
                btnDelete.Visible = false;
                btnCancel.Visible = false;
            }

            FillGrid();
            txtLabName.Focus();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            //Single quote filter
            txtLabName.Text = txtLabName.Text.Trim().Replace("'", "");

            //Empty fields validation
            if (txtLabName.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * field are required');", true);
                txtLabName.Focus();
                return;
            }

            // Insert / Update data into database 
            int maxId = lblId.Text == "[New]" ? c.NextId("LabTestData", "LabTestID") : Convert.ToInt16(lblId.Text);

            if (lblId.Text == "[New]")
            {
                c.ExecuteQuery("Insert Into LabTestData(LabTestID, LabTestName, DelMark) Values(" + maxId + ", '" + txtLabName.Text + "', 0)");
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Lab info added');", true);
            }
            else
            {
                c.ExecuteQuery("Update LabTestData set LabTestName='" + txtLabName.Text + "' Where LabTestID=" + maxId);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Lab info updated');", true);
            }
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
            c.ExecuteQuery("Update LabTestData Set DelMark=1 Where LabTestID=" + Convert.ToInt32(Request.QueryString["id"]));
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Lab info deleted successfully');", true);

            FillGrid();
            ResetControl();
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
            Response.Redirect("lab-test-master.aspx?action=new");
        }

    }

    //Show Data into Gridview starts here
    private void FillGrid()
    {
        try
        {
            using (DataTable dtCategory = c.GetDataTable("Select LabTestID, LabTestName From LabTestData Where DelMark=0 Order By LabTestID DESC"))
            {
                gvLabs.DataSource = dtCategory;
                gvLabs.DataBind();
                if (gvLabs.Rows.Count > 0)
                {
                    gvLabs.UseAccessibleHeader = true;
                    gvLabs.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvLabs_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"lab-test-master.aspx?action=edit&id=" + e.Row.Cells[0].Text + "\" class=\"gAnch\" title=\"View/Edit\"></a>";
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvLabs_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    private void GetLabData(int Idx)
    {
        try
        {
            using (DataTable dtCategory = c.GetDataTable("Select * From LabTestData Where LabTestID=" + Idx))
            {
                if (dtCategory.Rows.Count > 0)
                {
                    DataRow bRow = dtCategory.Rows[0];
                    lblId.Text = Idx.ToString();
                    txtLabName.Text = bRow["LabTestName"].ToString();
                }
                txtLabName.Focus();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetCategoryData", ex.Message.ToString());
            return;
        }
    }

    //Reset Page controls
    private void ResetControl()
    {
        txtLabName.Focus();
        txtLabName.Text = "";
        lblId.Text = "[New]";
    }
}