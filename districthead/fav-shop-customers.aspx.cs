using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class districthead_fav_shop_customers : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillGrid();
        }
    }

    private void FillGrid()
    {
        try
        {
            using(DataTable dtShop = c.GetDataTable("Select a.FranchID, a.FranchName, a.FranchShopCode, b.CityName, " +
                " (Select Count(CustomrtID) From CustomersData Where CustomerActive=1 AND delMark=0 AND CustomerFavShop=a.FranchID) as totalFavCust" +
                " From FranchiseeData a Inner Join CityData b On a.FK_FranchCityId=b.CityID Where a.FranchActive=1 AND a.FK_DistHdId=" + Session["adminDH"] + ""))
            {
                gvOrder.DataSource = dtShop;
                gvOrder.DataBind();

                if (gvOrder.Rows.Count > 0)
                {
                    gvOrder.UseAccessibleHeader = true;
                    gvOrder.HeaderRow.TableSection = TableRowSection.TableHeader;
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
}