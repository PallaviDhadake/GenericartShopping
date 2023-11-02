using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class products_purchased_by_cust : System.Web.UI.Page
{
    iClass c = new iClass();
    public string backLink, errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["custId"] != null)
            {
                FillGrid(Convert.ToInt32(Request.QueryString["custId"]));
                backLink = "customer-lookup.aspx?custId=" + Request.QueryString["custId"];
            }
        }
    }

    private void FillGrid(int custId)
    {
        try
        {
            string strQuery = "Select distinct e.FK_DetailProductID, a.ProductName, a.ProductSKU, a.PriceMRP, a.PriceSale, CONVERT(varchar(20), f.OrderID) + ' - ' + CONVERT(varchar(20), f.OrderDate, 103) as OrdDetails, " + 
                " b.ProductCatName, c.UnitName, d.MfgName From ProductsData a Inner Join ProductCategory b On a.FK_SubCategoryID=b.ProductCatID " +
                " Inner Join UnitProducts c On a.FK_UnitID=c.UnitID Inner Join Manufacturers d On a.FK_MfgID=d.MfgId " + 
                " Inner Join OrdersDetails e On e.FK_DetailProductID=a.ProductID  " +
                " Inner Join OrdersData f On e.FK_DetailOrderID=f.OrderID " +
                " Where a.delMark=0 AND f.FK_OrderCustomerID=" + custId + " AND f.OrderStatus IN (6, 7)";

            using (DataTable dtProd = c.GetDataTable(strQuery))
            {
                gvMedicine.DataSource = dtProd;
                gvMedicine.DataBind();

                if (dtProd.Rows.Count > 0)
                {
                    gvMedicine.UseAccessibleHeader = true;
                    gvMedicine.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}