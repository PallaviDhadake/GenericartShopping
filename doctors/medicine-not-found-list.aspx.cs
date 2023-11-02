using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class doctors_medicine_not_found_list : System.Web.UI.Page
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
            using (DataTable dtReq = c.GetDataTable("Select RequestID, Convert(varchar(20), RequestDate, 103) as reqDate, isnull(RequestName, '-') as RequestName, RequestMedicine, RequestMobile, isnull(DeviceType, '-') as DeviceType From SavingCalcRequest Order By RequestID DESC"))
            {
                gvMedList.DataSource = dtReq;
                gvMedList.DataBind();
                if (gvMedList.Rows.Count > 0)
                {
                    gvMedList.UseAccessibleHeader = true;
                    gvMedList.HeaderRow.TableSection = TableRowSection.TableHeader;
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