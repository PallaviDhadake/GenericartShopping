using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class franchisee_MasterFranchisee : System.Web.UI.MasterPage
{
    iClass c = new iClass();
    public string rootPath, shopCode;
    public string[] franchiseeData = new string[5];
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["adminFranchisee"] == null)
        {
            Response.Redirect("default.aspx");
        }
        int id =Convert.ToInt32(Session["adminFranchisee"].ToString());
        // shopCode = c.GetReqData("FranchiseeData", "FranchShopCode", "FranchID="+id).ToString();

        GetFranchisee(id);

       

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        rootPath = c.ReturnHttp();
        if (Session["adminFranchisee"] == null)
        {
            Response.Redirect("default.aspx");
        }

    }

    private void GetFranchisee(int frId)
    {
        try
        {
            using (DataTable dtFranchisee = c.GetDataTable("Select Convert(varchar(20), FranchRegDate, 107) as RegDate, FranchShopCode, FranchName, FranchOwnerName From FranchiseeData Where FranchID=" + frId))
            {
                if (dtFranchisee.Rows.Count > 0)
                {
                    DataRow row = dtFranchisee.Rows[0];

                    franchiseeData[0] = row["FranchShopCode"].ToString();
                    franchiseeData[1] = row["FranchName"].ToString();
                    franchiseeData[2] = row["FranchOwnerName"].ToString();
                    franchiseeData[3] = row["RegDate"].ToString();
                }
            }
        }
        catch (Exception ex)
        {

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetFranchisee", ex.Message.ToString());
            return;
        }
    }
}
