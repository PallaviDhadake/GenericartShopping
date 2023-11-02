using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class admingenshopping_assign_heads_shop : System.Web.UI.Page
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
            using (DataTable dtShops = c.GetDataTable("Select a.FranchID, a.FranchShopCode, a.FranchName, a.FranchMobile, a.FranchPassword, a.FranchAddress, isnull(b.CityName, 'NA') as CityName From FranchiseeData a Left Join CityData b On a.FK_FranchCityId=b.CityID Where a.FranchActive=1 AND FK_DistHdId IS NULL AND FK_ZonalHdId IS NULL"))
            {
                gvShops.DataSource = dtShops;
                gvShops.DataBind();

                if (dtShops.Rows.Count > 0)
                {
                    gvShops.UseAccessibleHeader = true;
                    gvShops.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvShops_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litAnch = (Literal)e.Row.FindControl("litAnch");
                litAnch.Text = "<a href=\"shop-list.aspx?action=edit&id=" + e.Row.Cells[0].Text + "&from=assign\" class=\"gAnch\" title=\"View/Edit\"></a>";
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvShops_RowDataBound", ex.Message.ToString());
            return;
        }
    }
}