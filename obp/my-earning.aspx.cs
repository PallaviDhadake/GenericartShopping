using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class obp_my_earning : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        FillGrid();
    }
    private void FillGrid()
    {
        try
        {
            int GOBP_ID = Convert.ToInt32(Session["adminObp"]);
            using (DataTable dtCustomer = c.GetDataTable("Select OC.ObpComId, convert(varchar(20), OC.ObpComDate, 103) as ObpComDate, OC.ObpComType, OC.ObpRefUserId, OB.OBP_ApplicantName as ObpRefName, OC.ObpComLevel, OC.ObpComPercent, OC.ObpComAmount From OBPCommission OC Inner Join OBPData OB on OC.ObpRefUserId=OB.OBP_UserID AND OC.FK_Obp_ID=" + GOBP_ID + " Order By OC.ObpComDate ASC"))
            {
                gvEarnings.DataSource = dtCustomer;
                gvEarnings.DataBind();
                if (gvEarnings.Rows.Count > 0)
                {
                    gvEarnings.UseAccessibleHeader = true;
                    gvEarnings.HeaderRow.TableSection = TableRowSection.TableHeader;
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