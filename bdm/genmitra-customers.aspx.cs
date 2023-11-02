using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class bdm_genmitra_customers : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            c.FillComboBox("GMitraName", "GMitraID", "GenericMitra", "GMitraStatus=1", "GMitraName", 0, ddrGenMitra);

            FillGrid();
        }
    }

    private void FillGrid()
    {
        try
        {
            string strQuery = "";
            if (ddrGenMitra.SelectedIndex > 0)
            {
                strQuery = "Select a.CustomrtID, CONVERT(varchar(20), a.CustomerJoinDate, 103) as JoinDate, a.CustomerName, a.CustomerMobile, a.CustomerEmail, isnull(a.DeviceType, '-') as DeviceType, a.CustomerPassword From CustomersData a Where a.delMark=0 AND a.FK_GenMitraID=" + ddrGenMitra.SelectedValue + " Order By a.CustomrtID DESC";
            }
            else
            {
                strQuery = "Select a.CustomrtID, CONVERT(varchar(20), a.CustomerJoinDate, 103) as JoinDate, a.CustomerName, a.CustomerMobile, a.CustomerEmail, isnull(a.DeviceType, '-') as DeviceType, a.CustomerPassword From CustomersData a Where a.delMark=0 AND a.FK_GenMitraID IS NOT NULL Order By a.CustomrtID DESC";
            }
            using (DataTable dtCust = c.GetDataTable(strQuery))
            {
                gvCust.DataSource = dtCust;
                gvCust.DataBind();

                if (gvCust.Rows.Count > 0)
                {
                    gvCust.UseAccessibleHeader = true;
                    gvCust.HeaderRow.TableSection = TableRowSection.TableHeader;
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
    protected void btnShow_Click(object sender, EventArgs e)
    {
        if (ddrGenMitra.SelectedIndex == 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select Generic Mitra');", true);
            return;
        }

        FillGrid();
    }
}