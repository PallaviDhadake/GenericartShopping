using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Script.Services;

public partial class admingenshopping_disease_product_master : System.Web.UI.Page
{
    public string pgTitle, errMsg;
    iClass c = new iClass();


    protected void Page_Load(object sender, EventArgs e)
    {
        pgTitle = Request.QueryString["action"] == "new" ? "Add Unit Info" : "Edit Unit Info";
        btnSave.Attributes.Add("onclick", "this.disabled=true; this.value='Processing...';" + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            txtProduct.Focus();
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
       
        //Single quote filter
        txtProduct.Text = txtProduct.Text.Trim().Replace("'", "");

        //Empty fields validation
        if (txtProduct.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'All * field are required');", true);

            txtProduct.Focus();
            return;
        }

        // Insert / Update data into database 
        int productId = 0;
        object prodID = c.GetReqData("ProductsData", "ProductID", "ProductName='"+ txtProduct.Text +"'");
        if (prodID != DBNull.Value && prodID != null && prodID.ToString()!= "")
        {
            productId = Convert.ToInt32(prodID);
        }
        int maxId = 0;
        int disId = 0;
        string[] arrDisease = txtDisease.Text.Split(',');
        if (lblId.Text== "[New]")
        {
            //TextBox1.Text = txtDisease.Text;
            // c.ExecuteQuery("Insert Into DiseaseProducts(DisProID, FK_ProductID) Values(" + maxId + ", " + prodID + ")");
           
            for (int i = 0; i < arrDisease.Length; i++)
            {
                maxId = c.NextId("DiseaseProducts", "DisProID");
                disId = Convert.ToInt32(c.GetReqData("DiseaseData", "DiseaseId", "DiseaseName='" + arrDisease[i].ToString().Trim() + "'"));
                if (!c.IsRecordExist("Select DisProID From DiseaseProducts Where FK_ProductID=" + productId + " AND FK_DiseaseID=" + disId)) 
                {
                    c.ExecuteQuery("Insert Into DiseaseProducts(DisProID, FK_ProductID, FK_DiseaseID) Values(" + maxId + ", " + productId + ", " + disId + ")");
                }
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Disease Added for Product Successfully..!!');", true);
        }
        //else
        //{
        //    maxId = Convert.ToInt32(lblId.Text);
        //    c.ExecuteQuery("Delete From DiseaseProducts Where FK_ProductID=" + maxId);
        //    for (int i = 0; i < arrDisease.Length; i++)
        //    {
        //        maxId = c.NextId("DiseaseProducts", "DisProID");
        //        disId = Convert.ToInt32(c.GetReqData("DiseaseData", "DiseaseId", "DiseaseName='" + arrDisease[i].ToString() + "'"));
        //        if (!c.IsRecordExist("Select DisProID From DiseaseProducts Where FK_ProductID=" + prodID + " AND FK_DiseaseID=" + disId))
        //        {
        //            c.ExecuteQuery("Insert Into DiseaseProducts(DisProID, FK_ProductID, FK_DiseaseID) Values(" + maxId + ", " + prodID + ", " + disId + ")");
        //        }

        //    }
        //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Entry updated');", true);
        //}

        FillGrid(productId);
        ResetControl();
    }


    protected void txtProduct_TextChanged(object sender, EventArgs e)
    {
        object prodID = c.GetReqData("ProductsData", "ProductID", "ProductName='" + txtProduct.Text + "'");
        int productId = 0;
        if (prodID != DBNull.Value && prodID != null && prodID.ToString() != "")
        {
            productId = Convert.ToInt32(prodID);
            FillGrid(productId);
        }

    }

    private void FillGrid(int prodId)
    {
        try
        {
            using (DataTable dtDiseaseProduct = c.GetDataTable("SELECT a.FK_ProductID, a.FK_DiseaseID, b.ProductName, c.DiseaseName FROM DiseaseProducts a INNER JOIN ProductsData b On b.ProductID = a.FK_ProductID INNER JOIN DiseaseData c On c.DiseaseId = a.FK_DiseaseID WHERE FK_ProductID=" + prodId))
            {
                gvDiseaseProd.DataSource = dtDiseaseProduct;
                gvDiseaseProd.DataBind();
                if(gvDiseaseProd.Rows.Count > 0)
                {
                    gvDiseaseProd.UseAccessibleHeader = true;
                    gvDiseaseProd.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    private void ResetControl()
    {
        txtProduct.Focus();
        txtDisease.Text = "";
    }

    protected void gvDiseaseProd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridViewRow gRow = (GridViewRow)((Button)e.CommandSource).NamingContainer;
            if (e.CommandName == "gvDel")
            {
                c.ExecuteQuery("Delete From DiseaseProducts Where FK_ProductID=" + gRow.Cells[0].Text + "AND FK_DiseaseID=" + gRow.Cells[1].Text);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Disease successfully deleted');", true);
            }
            
            FillGrid(Convert.ToInt32(gRow.Cells[0].Text));

        }
        catch (Exception ex)
        {

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvDiseaseProd_RowCommand", ex.Message.ToString());
            return;
        }
    }

    [WebMethod]
    public static List<string> GetSearchControl(string prefix)
    {
        iClass c = new iClass();
        List<string> customers = new List<string>();
        //using (SqlConnection conn = new SqlConnection())
        //{
        //    conn.ConnectionString = MasterClass.Getconnection();
        //    using (SqlCommand cmd = new SqlCommand())
        //    {
        //        cmd.CommandText = "SELECT ProductID, ProductName as Search FROM ProductsData WHERE ProductName like @SearchText + '%' and isnull(ProductActive, 0) = 1";
        //        cmd.Parameters.AddWithValue("@SearchText", prefix);
        //        cmd.Connection = conn;
        //        conn.Open();
        //        using (SqlDataReader sdr = cmd.ExecuteReader())
        //        {
        //            while (sdr.Read())
        //            {
        //                customers.Add(string.Format("{0}-{1}", sdr["Search"], sdr["Search"]));
        //            }
        //        }
        //        conn.Close();
        //    }
        //}

        using(DataTable dtProd = c.GetDataTable("SELECT ProductID, ProductName FROM ProductsData WHERE ProductName like '" + prefix + "%' and delMark=0 AND isnull(ProductActive, 0) = 1"))
        {
            if (dtProd.Rows.Count > 0)
            {
                foreach(DataRow row in dtProd.Rows)
                {
                    customers.Add(string.Format("{0}", row["ProductName"]));
                }
            }
            else
            {
                customers.Add("Match Not Found");
            }
        }
        return customers;
    }
    //GetCategories
    [WebMethod]
    public static List<string> GetCategories(string prefix)
    {
        iClass c = new iClass();
        List<string> categories = new List<string>();
        using (DataTable dtCat = c.GetDataTable("SELECT HealthProdId, HealthProdName FROM HealthProductsData WHERE HealthProdName like '" + prefix + "%' and delMark=0"))
        {
            if (dtCat.Rows.Count > 0)
            {
                foreach (DataRow row in dtCat.Rows)
                {
                    categories.Add(string.Format("{0}", row["HealthProdName"]));
                }
            }
            else
            {
                categories.Add("Match Not Found");
            }
        }
        return categories;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<string> GetDiseases(string prefix)
    {
        iClass c = new iClass();
        List<string> diseases = new List<string>();
        using (DataTable dtDisease = c.GetDataTable("SELECT DiseaseId, DiseaseName FROM DiseaseData WHERE delMark=0 AND DiseaseName like '" + prefix + "%'"))
        {
            if (dtDisease.Rows.Count > 0)
            {
                foreach (DataRow row in dtDisease.Rows)
                {
                    diseases.Add(string.Format("{0}", row["DiseaseName"]));
                }
            }
            else
            {
                diseases.Add("Match Not Found");
            }
        }
        return diseases;
    }
}