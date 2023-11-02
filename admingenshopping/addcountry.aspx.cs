using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admingenshopping_addcountry : System.Web.UI.Page
{
    iClass c = new iClass();
    public string pgTitle, errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        pgTitle = Request.QueryString["action"] == "new" ? "Add Country Info" : "Edit Country Info";
        btnSave.Attributes.Add("onclick", "this.disabled=true; this.value='Processing...';" + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        btnDelete.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnDelete, null) + ";");
        btnCancel.Attributes.Add("onclick", "this.disabled=true; this.value='Processing...';" + ClientScript.GetPostBackEventReference(btnCancel, null) + ";");

        if (!IsPostBack)
        {
            if (Request.QueryString["action"] != null)
            {
                editCountry.Visible = true;
                viewCountry.Visible = false;

                if (Request.QueryString["action"] == "new")
                {
                    btnSave.Text = "Save Info";
                    btnDelete.Visible = false;

                }
                else
                {
                    btnSave.Text = "Modify Info";
                    btnDelete.Visible = true;
                    GetCountryData(Convert.ToInt32(Request.QueryString["id"]));
                }
            }
            else
            {
                viewCountry.Visible = true;
                editCountry.Visible = false;
                FillGrid();
            }

            txtName.Focus();
        }
        //try
        //{
        //    viewCountry.Visible = true;
        //    FillGrid();
        //}
        //catch (Exception)
        //{

        //    throw;
        //}
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            //Single quote filter
            txtName.Text = txtName.Text.Trim().Replace("'", "");

            //Empty fields validation
            if (txtName.Text == "")
            {


                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Please Enter Country Name');", true);
                return;
            }
            // Insert / Update data into database 
            int maxId = lblId.Text == "[New]" ? c.NextId("CountryData", "CountryID") : Convert.ToInt16(lblId.Text);

            if (lblId.Text == "[New]")
            {
                c.ExecuteQuery("Insert Into CountryData (CountryID, CountryName) Values(" + maxId + ",'" + txtName.Text + "')");

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Country added successfully');", true);
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "showNotification({message: 'Supplier Info Added', type: 'success'});", true);
            }
            else
            {
                c.ExecuteQuery("Update CountryData Set CountryName='" + txtName.Text + "' Where CountryID=" + maxId);

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Country Updated successfully');", true);
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "showNotification({message: 'Supplier Info Updated', type: 'success'});", true);
            }
            //ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "waitAndMove('supplier-master.aspx', 2000);", true);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('addcountry.aspx', 2000);", true);

        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            //ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Error occurred while processing', type: 'error'});", true);
            return;
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            c.ExecuteQuery("Delete From CountryData Where CountryID=" + Convert.ToInt32(Request.QueryString["id"]));
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Country deleted successfully');", true);
            FillGrid();
            //if (c.IsRecordExist("Select ProductID From ProductsData Where FK_MfgID=" + Convert.ToInt32(Request.QueryString["id"])))
            //{
            //    errMsg = c.ErrNotification(2, "You Can't Delete this record, its reference exists in ProductData");
            //    return;
            //}
            //else
            //{
            //    c.ExecuteQuery("Delete From Manufacturers Where MfgId=" + Convert.ToInt32(Request.QueryString["id"]));
            //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Manufacturer deleted successfully', type: 'success'});", true);
            //    FillGrid();
            //}

        }
        catch (Exception)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Error Occoured while processing', type: 'error'});", true);
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
            using (DataTable dtTesti = c.GetDataTable("Select ROW_NUMBER() OVER (ORDER BY CountryName) AS sn, CountryID, CountryName from CountryData order by CountryName"))
            {
                gvCountry.DataSource = dtTesti;
                gvCountry.DataBind();
                if (gvCountry.Rows.Count > 0)
                {
                    gvCountry.UseAccessibleHeader = true;
                    gvCountry.HeaderRow.TableSection = TableRowSection.TableHeader;
                }

            }
        }
        catch (Exception)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Error occurred while processing', type: 'error'});", true);
            return;
        }
    }

    protected void gvCountry_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"addcountry.aspx?action=edit&id=" + e.Row.Cells[0].Text + "\" class=\"gAnch\"></a>";
            }
        }
        catch (Exception)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Error while processing', type: 'error'});", true);
            return;
        }
    }

    protected void GetCountryData(int Idx)
    {
        try
        {
            using (DataTable dtMfr = c.GetDataTable("Select * From CountryData Where CountryID=" + Idx))
            {
                if (dtMfr.Rows.Count > 0)
                {
                    DataRow bRow = dtMfr.Rows[0];
                    lblId.Text = Idx.ToString();

                    txtName.Text = bRow["CountryName"].ToString();

                }
            }
        }
        catch (Exception)
        {

            throw;
        }
    }



}