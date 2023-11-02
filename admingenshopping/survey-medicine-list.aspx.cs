using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class admingenshopping_survey_medicine_list : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg, pgTitle, totalCount;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            btnSave.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

            totalCount = c.returnAggregate("Select Count(MedicineRowID) From SurveyMedicines").ToString();

            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    editMed.Visible = true;
                    viewMed.Visible = false;

                    btnSave.Text = "Modify Info";
                    pgTitle = "Edit Brand Medicines";
                    GetMedInfo(Convert.ToInt32(Request.QueryString["id"]));
                }
                else
                {
                    editMed.Visible = false;
                    viewMed.Visible = true;
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

    private void FillGrid()
    {
        try
        {
            string strQuery = "";
            if (txtMedContent.Text == "" && txtGmpCode.Text == "")
            {
                strQuery = "Select TOP 500 MedicineRowID, ContentName, BrandName, CompanyName, Packaging, PriceBrand, PriceGeneric, isnull(GenericCode, '') as genCode From SurveyMedicines Order By MedicineRowID DESC";
            }
            else
            {
                if (txtMedContent.Text != "")
                {
                    if (rdbBasic.Checked == true)
                    {
                        strQuery = "Select MedicineRowID, ContentName, BrandName, CompanyName, Packaging, PriceBrand, PriceGeneric, isnull(GenericCode, '') as genCode From SurveyMedicines Where ContentName LIKE '" + txtMedContent.Text + "%' Order By MedicineRowID DESC";
                    }
                    else
                    {
                        strQuery = "Select MedicineRowID, ContentName, BrandName, CompanyName, Packaging, PriceBrand, PriceGeneric, isnull(GenericCode, '') as genCode From SurveyMedicines Where ContentName LIKE '%" + txtMedContent.Text + "%' Order By MedicineRowID DESC";
                    }
                }
                else if (txtGmpCode.Text != "")
                {
                    strQuery = "Select MedicineRowID, ContentName, BrandName, CompanyName, Packaging, PriceBrand, PriceGeneric, isnull(GenericCode, '') as genCode From SurveyMedicines Where GenericCode='" + txtGmpCode.Text + "'  Order By MedicineRowID DESC";
                }
            }
            using (DataTable dtSurveyMed = c.GetDataTable(strQuery))
            {
                gvSurveyMed.DataSource = dtSurveyMed;
                gvSurveyMed.DataBind();

                if (dtSurveyMed.Rows.Count > 0)
                {
                    gvSurveyMed.UseAccessibleHeader = true;
                    gvSurveyMed.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvSurveyMed_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"survey-medicine-list.aspx?id=" + e.Row.Cells[0].Text + "\" class=\"gAnch\"></a>";
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvSurveyMed_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            txtContent.Text = txtContent.Text.Trim().Replace("'", "");
            txtBrandCompany.Text = txtBrandCompany.Text.Trim().Replace("'", "");
            txtBrandName.Text = txtBrandName.Text.Trim().Replace("'", "");
            txtBrandPrice.Text = txtBrandPrice.Text.Trim().Replace("'", "");
            txtGenericPrice.Text = txtGenericPrice.Text.Trim().Replace("'", "");
            txtPackaging.Text = txtPackaging.Text.Trim().Replace("'", "");
            txtGenCode.Text = txtGenCode.Text.Trim().Replace("'", "");

            if (txtContent.Text == "" || txtBrandName.Text == "" || txtBrandPrice.Text == "" || txtPackaging.Text == "" || txtBrandCompany.Text == "" || txtGenericPrice.Text == "" || txtGenCode.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * Fields are mandatory');", true);
                return;
            }

            if (!c.IsNumeric(txtBrandPrice.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Brand Price Must be Numeric Value');", true);
                return;
            }

            if (txtGenericPrice.Text != "")
            {
                if (!c.IsNumeric(txtGenericPrice.Text))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Generic Price Must be Numeric Value');", true);
                    return;
                }
            }

            if (c.IsRecordExist("Select MedicineRowID From SurveyMedicines Where BrandName='" + txtBrandName.Text + "' AND MedicineRowID<>" + lblId.Text))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'This brand name already exists');", true);
                return;
            }

            if (c.IsRecordExist("Select MedicineRowID From SurveyMedicines Where ContentName<>'" + txtContent.Text + "' AND GenericCode='" + txtGenCode.Text + "'"))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'This Generic code with another content name already exists');", true);
                return;
            }

            int maxId = Convert.ToInt32(lblId.Text);

            //c.ExecuteQuery("Update SurveyMedicines Set ContentName='" + txtContent.Text + "', BrandName='" + txtBrandName.Text +
            //        "', CompanyName='" + txtBrandCompany.Text + "', Packaging='" + txtPackaging.Text +
            //        "', PriceBrand=" + Convert.ToDouble(txtBrandPrice.Text) + ", PriceGeneric=" + Convert.ToDouble(txtGenericPrice.Text) +
            //        ", GenericCode='" + txtGenCode.Text + "' Where MedicineRowID=" + maxId);
            c.ExecuteQuery("Update SurveyMedicines Set ContentName='" + txtContent.Text + "', BrandName='" + txtBrandName.Text +
                    "', CompanyName='" + txtBrandCompany.Text + "', Packaging='" + txtPackaging.Text +
                    "', PriceBrand=" + txtBrandPrice.Text + ", PriceGeneric=" + txtGenericPrice.Text +
                    ", GenericCode='" + txtGenCode.Text + "' Where MedicineRowID=" + maxId);

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Brand Medicine Updated');", true);

            txtBrandPrice.Text = txtBrandName.Text = txtBrandCompany.Text = txtContent.Text = txtGenericPrice.Text = txtPackaging.Text = txtGenCode.Text = "";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('survey-medicine-list.aspx', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSubmit_Click", ex.Message.ToString());
            return;
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("survey-medicine-list.aspx");
    }

    private void GetMedInfo(int medIdX)
    {
        try
        {
            using (DataTable dtBrandMed = c.GetDataTable("Select * From SurveyMedicines Where MedicineRowID=" + medIdX))
            {
                if (dtBrandMed.Rows.Count > 0)
                {
                    DataRow row = dtBrandMed.Rows[0];

                    lblId.Text = medIdX.ToString();

                    txtContent.Text = row["ContentName"].ToString();
                    txtBrandName.Text = row["BrandName"].ToString();
                    txtPackaging.Text = row["Packaging"].ToString();
                    txtBrandPrice.Text = row["PriceBrand"].ToString();
                    txtBrandCompany.Text = row["CompanyName"] != DBNull.Value && row["CompanyName"] != null && row["CompanyName"].ToString() != "" ? row["CompanyName"].ToString() : "";
                    txtGenericPrice.Text = row["PriceGeneric"] != DBNull.Value && row["PriceGeneric"] != null && row["PriceGeneric"].ToString() != "" ? row["PriceGeneric"].ToString() : "";
                    txtGenCode.Text = row["GenericCode"] != DBNull.Value && row["GenericCode"] != null && row["GenericCode"].ToString() != "" ? row["GenericCode"].ToString() : "";
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetMedInfo", ex.Message.ToString());
            return;
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            c.ExecuteQuery("Delete From SurveyMedicines Where MedicineRowID=" + Request.QueryString["id"]);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Brand Medicine Deleted');", true);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('survey-medicine-list.aspx', 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnDelete_Click", ex.Message.ToString());
            return;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            txtMedContent.Text = txtMedContent.Text.Trim().Replace("'", "");

            if (txtMedContent.Text == "" && txtGmpCode.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter either Content Name or GMP Code to search');", true);
                return;
            }

            FillGrid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSearch_Click", ex.Message.ToString());
            return;
        }
    }

    protected void btnDeleteMed_Click(object sender, EventArgs e)
    {
        try
        {
            int sCount = 0;
            string medStr = "";

            foreach (GridViewRow gRow in gvSurveyMed.Rows)
            {
                CheckBox chk = (CheckBox)gRow.FindControl("chkSelect");
                if (chk.Checked)
                {
                    sCount++;
                    medStr = medStr == "" ? gRow.Cells[0].Text : medStr + ", " + gRow.Cells[0].Text;
                }
            }

            if (sCount <= 0)
            {
                FillGrid();
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Select Medicine to delete', type: 'warning'});", true);
                return;
            }
            else
            {
                //c.ExecuteQuery("Delete From SurveyMedicines Where MedicineRowID IN (" + medStr + ")");
                c.ExecuteQuery("Delete From SurveyMedicines Where MedicineRowID IN (" + medStr + ")");
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Medicines Data Deleted', type: 'success'});", true);
                FillGrid();
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Error occurred while processing', type: 'error'});", true);
            c.ErrorLogHandler(this.ToString(), "btnDeleteMed_Click", ex.Message.ToString());
            return;
        }
    }
}