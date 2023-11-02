using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Web.Script.Services;

public partial class admingenshopping_related_products : System.Web.UI.Page
{
    iClass c = new iClass();
    public string subCat, errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSave.Attributes.Add("onclick", "this.disabled=true; this.value='Processing...';" + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        subCat = c.GetReqData("ProductCategory", "ProductCatName", "ProductCatID=" + Request.QueryString["subCatId"] + " AND delMark=0").ToString();
        txtSubCat.Text = subCat;
        //FillGrid();

        if (!IsPostBack)
        {
            object subCatProd = c.GetReqData("ProductCategory", "RelatedProdId", "ProductCatID=" + Request.QueryString["subCatId"]);
            if (subCatProd != DBNull.Value && subCatProd != null && subCatProd.ToString() != "")
            {
                using (DataTable dtProd = c.GetDataTable("Select ProductID, ProductName, ProductSKU From ProductsData Where ProductID IN (" + subCatProd.ToString() + ")"))
                {
                    if (dtProd.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtProd.Rows)
                        {
                            if (txtProduct.Text == "")
                                txtProduct.Text = row["ProductName"].ToString();
                            else
                                txtProduct.Text = txtProduct.Text + "," + row["ProductName"].ToString();
                        }
                    }
                }
            }
        }
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<string> GetProducts(string prefix)
    {
        iClass c = new iClass();
        List<string> products = new List<string>();
        using (DataTable dtProd = c.GetDataTable("SELECT ProductID, ProductName FROM ProductsData WHERE ProductName like '" + prefix + "%' and delMark=0 AND isnull(ProductActive, 0) = 1"))
        {
            if (dtProd.Rows.Count > 0)
            {
                foreach (DataRow row in dtProd.Rows)
                {
                    products.Add(string.Format("{0}", row["ProductName"]));
                }
            }
            else
            {
                products.Add("Match Not Found");
            }
        }
        return products;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            txtProduct.Text = txtProduct.Text.Trim().Replace("'", "");

            if (txtProduct.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select Product');", true);
                return;
            }

            int prodId = 0;
            string prodIds = "";

            string[] arrProds = txtProduct.Text.Split(',');

            for (int i = 0; i < arrProds.Length; i++)
            {
                prodId = Convert.ToInt32(c.GetReqData("ProductsData", "ProductID", "ProductName='" + arrProds[i].Trim().ToString() + "'"));
                if (prodIds == "")
                    prodIds = prodId.ToString();
                else
                    prodIds = prodIds + "," + prodId.ToString();
            }

            c.ExecuteQuery("Update ProductCategory set RelatedProdId='" + prodIds + "' Where ProductCatID=" + Request.QueryString["subCatId"]);

            //FillGrid();

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Related Products Added for Sub Category..!!');", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSave_Click", ex.Message.ToString());
            return;
        }
    }

    private void FillGrid()
    {
        try
        {
            object subCatProd = c.GetReqData("ProductCategory", "RelatedProdId", "ProductCatID=" + Request.QueryString["subCatId"]);
            if (subCatProd != DBNull.Value && subCatProd != null && subCatProd.ToString() != "")
            {
                using (DataTable dtProd = c.GetDataTable("Select ProductID, ProductName, ProductSKU From ProductsData Where ProductID IN (" + subCatProd.ToString() + ")"))
                {
                    gvSubCatProd.DataSource = dtProd;
                    gvSubCatProd.DataBind();

                    if (gvSubCatProd.Rows.Count > 0)
                    {
                        gvSubCatProd.UseAccessibleHeader = true;
                        gvSubCatProd.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }
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

    protected void gvSubCatProd_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
}