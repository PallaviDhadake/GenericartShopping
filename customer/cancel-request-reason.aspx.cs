using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

public partial class customer_cancel_request_reason : System.Web.UI.Page
{
    iClass c = new iClass();
    public string rootPath, errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //c.FillComboBox("ReasonTitle", "ReasonID", "CancelReasons", "ResonType=1 AND DelMark=0 AND ReasonTitle IS NOT NULL AND ReasonTitle<>''", "ReasonTitle", 0, ddrReasons);
            FillReasons();
        }
    }

    private void FillReasons()
    {
        try
        {
            SqlConnection con = new SqlConnection(c.OpenConnection());

            SqlDataAdapter da = new SqlDataAdapter("Select Distinct(ReasonTitle) as rsTitle, ReasonID From CancelReasons Where ResonType=1 AND DelMark=0 AND ReasonTitle IS NOT NULL AND ReasonTitle<>' ' AND ReasonID NOT IN (Select ReasonID From CancelReasons Where ReasonTitle like '%?%') Order By ReasonTitle", con);
            DataSet ds = new DataSet();
            da.Fill(ds, "myCombo");

            ddrReasons.DataSource = ds.Tables[0];
            ddrReasons.DataTextField = ds.Tables[0].Columns["rsTitle"].ColumnName.ToString();
            ddrReasons.DataValueField = ds.Tables[0].Columns["ReasonID"].ColumnName.ToString();
            ddrReasons.DataBind();

            ddrReasons.Items.Insert(0, "<-Select->");
            ddrReasons.Items[0].Value = "0";
            
            con.Close();
            con = null;
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        rootPath = c.ReturnHttp();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddrReasons.SelectedIndex == 0)
            {
                errMsg = c.ErrNotification(2, "Select Reason to cancel request");
                return;
            }
            int reasonId = Convert.ToInt32(ddrReasons.SelectedValue);
            c.ExecuteQuery("Update OrdersData Set OrderStatus=2, FK_ReasonID=" + reasonId + " Where OrderID=" + Request.QueryString["id"]);
            errMsg = c.ErrNotification(1, "Your Request has been cancelled..!!");
            string pageUrl = rootPath + "customer/my-orders";
            ClientScript.RegisterStartupScript(this.GetType(), "redirect", "setTimeout(function () { if(top!=self) {top.location.href = '" + pageUrl + "';} }, 2000);", true);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "CallMyFunction", "waitAndMove('" + rootPath + "my-orders', 2000);", true);
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}