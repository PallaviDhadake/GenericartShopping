using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class management_fav_shop_customers : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            c.FillComboBox("ZonalHdName", "ZonalHdId", "ZonalHead", "DelMark=0", "ZonalHdName", 0, ddrZh);
            FillGrid();
        }
    }

    private void FillGrid()
    {
        try
        {
            string strQuery = "";
            if (ddrZh.SelectedIndex > 0)
            {
                if (ddrDh.SelectedIndex > 0)
                {
                    strQuery = "Select a.FranchID, a.FranchName, a.FranchShopCode, b.CityName, " +
                    " (Select Count(CustomrtID) From CustomersData Where CustomerActive=1 AND delMark=0 AND CustomerFavShop=a.FranchID) as totalFavCust" +
                    " From FranchiseeData a Inner Join CityData b On a.FK_FranchCityId=b.CityID Where a.FranchActive=1 AND FK_DistHdId=" + ddrDh.SelectedValue + "";
                }
                else
                {
                    strQuery = "Select a.FranchID, a.FranchName, a.FranchShopCode, b.CityName, " +
                    " (Select Count(CustomrtID) From CustomersData Where CustomerActive=1 AND delMark=0 AND CustomerFavShop=a.FranchID) as totalFavCust" +
                    " From FranchiseeData a Inner Join CityData b On a.FK_FranchCityId=b.CityID Where a.FranchActive=1 AND FK_ZonalHdId=" + ddrZh.SelectedValue + "";
                }
            }
            else
            {
                strQuery = "Select a.FranchID, a.FranchName, a.FranchShopCode, b.CityName, " +
                " (Select Count(CustomrtID) From CustomersData Where CustomerActive=1 AND delMark=0 AND CustomerFavShop=a.FranchID) as totalFavCust" +
                " From FranchiseeData a Inner Join CityData b On a.FK_FranchCityId=b.CityID Where a.FranchActive=1";
            }

            using (DataTable dtShop = c.GetDataTable(strQuery))
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

    protected void ddrZh_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddrZh.SelectedIndex > 0)
            {
                string zhDistIds = "";
                using (DataTable dtZhDist = c.GetDataTable("Select DistrictId From ZonalHeadDistricts Where ZonalHdId=" + ddrZh.SelectedValue))
                {
                    if (dtZhDist.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtZhDist.Rows)
                        {
                            if (zhDistIds != "")
                                zhDistIds = zhDistIds + "," + row["DistrictId"].ToString();
                            else
                                zhDistIds = row["DistrictId"].ToString();
                        }
                    }
                }
                c.FillComboBox("DistHdName", "DistHdId", "DistrictHead", "DelMark=0 AND DistHdDistrictId IN (" + zhDistIds + ")", "DistHdName", 0, ddrDh);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "ddrZh_SelectedIndexChanged", ex.Message.ToString());
            return;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddrZh.SelectedIndex == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select Zonal Head');", true);
                return;
            }

            FillGrid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSave_Click", ex.Message.ToString());
            return;
        }
    }
}