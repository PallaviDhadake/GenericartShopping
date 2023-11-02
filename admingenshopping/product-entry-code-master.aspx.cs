using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
public partial class admingenshopping_product_entry_code_master : System.Web.UI.Page
{
    iClass c = new iClass();
    public string dtProd, allprodcode, entercode;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillGrid();
            GetCount();
        }

      
    }

    protected void GetCount()
    {
        try
        {

            allprodcode = c.returnAggregate("Select Count(ProductID) From ProductsData WHERE delmark=0 AND ProductEntryCode IS NULL").ToString();

            entercode = c.returnAggregate("Select COUNT(ProductID) From ProductsData WHERE delmark=0 AND ProductEntryCode IS NOT NULL").ToString();

        }
        catch (Exception ex)
        {

        }
    }

    private void FillGrid()
    {
        try
        {
            //using (DataTable dtProd = c.GetDataTable("Select TOP 100  a.ProductID, a.ProductName, a.ProductSKU, a.PriceMRP, a.PriceSale, " +
            //    " c.UnitName From ProductsData a Inner Join ProductCategory b On a.FK_SubCategoryID=b.ProductCatID " +
            //    " Inner Join UnitProducts c On a.FK_UnitID=c.UnitID Inner Join Manufacturers d On a.FK_MfgID=d.MfgId Where a.delMark=0 Order By a.ProductID DESC"))

            using (DataTable dtProd = c.GetDataTable("Select a.ProductID, a.ProductName, a.ProductSKU, a.PriceMRP, a.PriceSale, c.UnitName from ProductsData a Inner join UnitProducts c on a.FK_UnitID=c.UnitID Where a.delMark=0 Order By a.ProductID DESC"))

            {
                gvProducts.DataSource = dtProd;
                gvProducts.DataBind();

                if (dtProd.Rows.Count > 0)
                {
                    gvProducts.UseAccessibleHeader = true;
                    gvProducts.HeaderRow.TableSection = TableRowSection.TableHeader;
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




    protected void gvProducts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Literal litStatus = (Literal)e.Row.FindControl("litStatus");
                int prodActive = Convert.ToInt32(c.GetReqData("ProductsData", "ProductActive", "ProductID=" + e.Row.Cells[0].Text));
                if (prodActive == 1)
                    litStatus.Text = "<span class=\"stGreen\">Active</span>";
                else
                    litStatus.Text = "<span class=\"stGrey\">Inactive</span>";



                Literal litTextBox = (Literal)e.Row.FindControl("litTextBox");
                string myTextbox = "txtProd" + e.Row.Cells[0].Text;


                //Product Entry code
                object EntryCode = c.GetReqData("ProductsData", "ProductEntryCode", "ProductID=" + e.Row.Cells[0].Text);
               // object ProductCode = EntryCode != null ? EntryCode.ToString() : null;

                // TextBox txtProdEntryCode = (TextBox)e.Row.FindControl("litTextBox");
                //  txtProdEntCode.Text= prodEntCode.ToString();

                if (EntryCode != DBNull.Value && EntryCode != null && EntryCode.ToString() != "")
                {
                    //litTextBox.Text = EntryCode.ToString();
                   // litTextBox.Text = EntryCode.ToString();
                    litTextBox.Text = "<input type=\"text\" id=\"" + myTextbox + "\" value=\"" + EntryCode + "\"  class=\"form-control\" />";
                }
                else
                {
                    litTextBox.Text = "<input type=\"text\" id=\"" + myTextbox + "\" class=\"form-control\" placeholder=\"Entry Code\" />";
                }

                
                Literal litButton = (Literal)e.Row.FindControl("litButton");

                litButton.Text = "<a class=\"btn btn-primary text-white\" onclick=\"UpdateProdCode('" + myTextbox + "', '" + e.Row.Cells[0].Text + "');\">Update</a>";

                

            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvProducts_RowDataBound", ex.Message.ToString());
            return;
        }
    }


    [WebMethod]
    public static string SaveProdCode(int prodIdx, string prodEntryCode)
    {
        try
        {
            iClass c = new iClass();
            int MaxId = c.NextId("ProductsData", "ProductID");

            prodEntryCode = prodEntryCode.Trim().Replace(",", "");
            prodEntryCode = prodEntryCode.ToUpper();

           if (c.IsRecordExist("Select ProductID From ProductsData Where ProductEntryCode='"+ prodEntryCode + "' AND ProductID!="+ prodIdx))
            {
                return "2";
            }
            else
            {
                c.ExecuteQuery("Update ProductsData set ProductEntryCode='" + prodEntryCode + "' Where ProductID=" + prodIdx);

                return "1";
            }

           
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
    }

}