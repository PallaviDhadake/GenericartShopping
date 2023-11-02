using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

public partial class bdm_fl_gobp_flup_overview_report : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            FillGrid();
        }
    }

    private void FillGrid()
    {
        SqlConnection con = new SqlConnection(c.OpenConnection());
        con.Open();
        SqlCommand cmd = new SqlCommand("FL_GOBP_Enq_Overview_Report", con);
        cmd.CommandTimeout = 120000000;
        cmd.CommandType = CommandType.StoredProcedure;

        if (txtDate.Text == string.Empty)
        {
            cmd.Parameters.AddWithValue("@State", 1);
            cmd.Parameters.AddWithValue("@DATE", DBNull.Value);
        }
        else if (txtDate.Text != string.Empty)
        {
            DateTime fromDate;
            string[] arrFromDate = txtDate.Text.Split('/');
            fromDate = Convert.ToDateTime(arrFromDate[1] + "/" + arrFromDate[0] + "/" + arrFromDate[2]);

            cmd.Parameters.AddWithValue("@State", 2);
            cmd.Parameters.AddWithValue("@DATE", fromDate);
        }

        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        sda.Fill(dt);

        if (dt.Rows.Count > 0)
        {
            gvCall.DataSource = dt;
            gvCall.DataBind();
        }
        else
        {
            gvCall.DataSource = null;
            gvCall.DataBind();
        }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime fromDate = DateTime.Now;
            string[] arrFromDate = txtDate.Text.Split('/');
            if (c.IsDate(arrFromDate[1] + "/" + arrFromDate[0] + "/" + arrFromDate[2]) == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Valid Date');", true);
                return;
            }

            FillGrid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnShow_Click", ex.Message.ToString());
            return;
        }
    }

    protected void gvCall_OnDataBound(object sender, EventArgs e)
    {
        try
        {
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableHeaderCell cell = new TableHeaderCell();
            cell.Text = "";
            cell.ColumnSpan = 2;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.Text = "Online Sales";
            cell.ColumnSpan = 3;
            cell.HorizontalAlign = HorizontalAlign.Center;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.Text = "GOBP";
            cell.ColumnSpan = 3;
            cell.HorizontalAlign = HorizontalAlign.Center;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.Text = "Follow up";
            cell.ColumnSpan = 3;
            cell.HorizontalAlign = HorizontalAlign.Center;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.Text = "Enquiry";
            cell.ColumnSpan = 3;
            cell.HorizontalAlign = HorizontalAlign.Center;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.Text = "Total";
            cell.ColumnSpan = 3;
            cell.HorizontalAlign = HorizontalAlign.Center;
            row.Controls.Add(cell);

            row.BackColor = ColorTranslator.FromHtml("#3AC0F2");
            gvCall.HeaderRow.Parent.Controls.AddAt(0, row);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvCall_OnDataBound", ex.Message.ToString());
            return;
        }
    }
}