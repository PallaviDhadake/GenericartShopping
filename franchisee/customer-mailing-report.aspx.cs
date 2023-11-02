using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class franchisee_customer_mailing_report : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] arrCall = new string[5];
    public string[] arrCounts = new string[15];
    public string errMsg, callCounts, callFollowup, callEnquiry;
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
        try
        {
            int id = Convert.ToInt32(Session["adminFranchisee"].ToString());
            string franchiseeShop = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID =" + id).ToString();

            SqlConnection con = new SqlConnection(c.OpenConnection());
            con.Open();
            SqlCommand cmd = new SqlCommand("Customer_Mailing_Report", con);
            cmd.CommandType = CommandType.StoredProcedure;

            if (txtDate.Text == string.Empty)
            {
                cmd.Parameters.AddWithValue("@State", 1);
                cmd.Parameters.AddWithValue("@FranchiseeShop", franchiseeShop);
                cmd.Parameters.AddWithValue("@Date", DateTime.Now);
            }
            else if (txtDate.Text != string.Empty)
            {
                DateTime fromDate;
                string[] arrFromDate = txtDate.Text.Split('/');
                fromDate = Convert.ToDateTime(arrFromDate[1] + "/" + arrFromDate[0] + "/" + arrFromDate[2]);

                cmd.Parameters.AddWithValue("@State", 2);
                cmd.Parameters.AddWithValue("@FranchiseeShop", franchiseeShop);
                cmd.Parameters.AddWithValue("@Date", fromDate);
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
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FillGrid", ex.Message.ToString());
            return;
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

    protected void btnPrintAdd_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime fromDate;
            string[] arrFromDate = txtDate.Text.Split('/');
            fromDate = Convert.ToDateTime(arrFromDate[1] + "/" + arrFromDate[0] + "/" + arrFromDate[2]);

            Response.Write("<script>");
            Response.Write("window.open('generate-address-label.aspx?frid=" + Session["adminFranchisee"] + "&date=" + fromDate + "','_blank')");
            Response.Write("</script>");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnPrintAdd_Click", ex.Message.ToString());
            return;
        }
    }
}