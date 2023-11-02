using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class obpmanager_gobp_referral_incentive_report : System.Web.UI.Page
{
    iClass c = new iClass();

    double incentiveReport = 0.0;
    public string errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occured While Processing');", true);
                c.ErrorLogHandler(this.ToString(), "Page_Load", ex.Message.ToString());
                return;
            }
        }
    }

    private void FillGrid()
    {
        try
        {
            string strQuery = "";
            //string[] arrYear = ddrMonth.SelectedItem.Text.ToString().Split('-');
            //string yearData = arrYear[1].ToString();

            if (txtUsername.Text != null)
            {
                strQuery = @"SELECT
                             a.[ObpComId] AS OBPID,
                             a.[ObpRefUserId] AS OBP_Username,
                             (SELECT [OBP_ApplicantName] FROM [dbo].[OBPData] WHERE [OBP_UserID] = a.[ObpRefUserId]) AS Name,
                             a.[FK_OrderId] AS OrderID,
                             CONVERT(VARCHAR(20), a.[ObpComDate], 103) AS Date,
                             a.[ObpComLevel] AS CommLevel,
                             a.[ObpComPercent] AS ComPercent,
                             a.[ObpComAmount] AS ComAmount,
                             a.[ObpComType] AS CommType
                         
                         FROM [dbo].[OBPCommission] AS a
                         WHERE a.[ObpComType] IN ('Sales-Direct','Sales-Ref')
                         AND a.[ObpUserId] = '" + txtUsername.Text + "'";
            }

            using (DataTable dtGOBPIncentive = c.GetDataTable(strQuery))
            {
                gvGOBPIncentive.DataSource = dtGOBPIncentive;
                gvGOBPIncentive.DataBind();

                if (gvGOBPIncentive.Rows.Count > 0)
                {
                    gvGOBPIncentive.UseAccessibleHeader = true;
                    gvGOBPIncentive.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FillGrid", ex.Message.ToString());
            return;
        }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        if (txtUsername.Text == null)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Enter OBP Username to check the incentive report', type: 'warning'});", true);
            return;
        }
        FillGrid();
    }

    protected void gvGOBPIncentive_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                incentiveReport += Math.Round(Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "ComAmount")), 2);
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[6].Text = "Total";
                e.Row.Cells[6].Font.Bold = true;

                e.Row.Cells[7].Text = incentiveReport.ToString();
                e.Row.Cells[7].Font.Bold = true;

                e.Row.Cells[8].Text = "";
                e.Row.Cells[6].Font.Bold = true;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvGOBPIncentive_RowDataBound", ex.Message.ToString());
            return;
        }
    }
}