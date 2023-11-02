using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GOBPDH_gobp_customers : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["gobpId"] != null)
                {
                    FillGrid();
                }
                else
                {
                    Response.Redirect("registered-customers.aspx", false);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "Page_Load", ex.Message.ToString());
            return;
        }
    }

    private void FillGrid()
    {
        try
        {
            string strQuery = "";
            strQuery = "SELECT a.[CustomrtID], a.[CustomerName], a.[CustomerMobile], " +
                " isnull((Select SUM([OrderAmount]) From [dbo].[OrdersData] Where [FK_OrderCustomerID] = a.[CustomrtID] AND [OrderStatus] = 7), 0) as custPurchase " +
                " From [dbo].[CustomersData] a Where a.[delMark] = 0 AND a.[CustomerActive] = 1 AND [FK_ObpID] = " + Request.QueryString["gobpId"];
            using (DataTable dtFrEnq = c.GetDataTable(strQuery))
            {
                gvGOBP.DataSource = dtFrEnq;
                gvGOBP.DataBind();
                if (dtFrEnq.Rows.Count > 0)
                {
                    gvGOBP.UseAccessibleHeader = true;
                    gvGOBP.HeaderRow.TableSection = TableRowSection.TableHeader;
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