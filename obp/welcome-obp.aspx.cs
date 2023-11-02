using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class obp_welcome_obp : System.Web.UI.Page
{
    iClass c = new iClass();
    public string firstChar, sessionName, bgColor, rootPath, joinDate;
    public string[] arrFavShop = new string[5];
    public string[] arrCounts = new string[10];
    protected void Page_Load(object sender, EventArgs e)
    {
        // Important code line
        this.Page.Header.DataBind();

        if (!IsPostBack)
        {
            GetGobpInfo();
            GetOrderCount();
        }
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        rootPath = c.ReturnHttp();
    }

    private void GetOrderCount()
    {
        try
        {
            arrCounts[0] = c.returnAggregate(@"SELECT COUNT([OrderID]) FROM [dbo].[OrdersData] WHERE [OrderStatus] = 7 AND [GOBPId] = " + Session["adminObp"]).ToString();

            arrCounts[1] = c.returnAggregate(@"SELECT SUM([OrderAmount]) FROM [dbo].[OrdersData] WHERE [OrderStatus] = 7 AND [GOBPId] = " + Session["adminObp"]).ToString();

            arrCounts[2] = c.returnAggregate(@"SELECT SUM([OBPComTotal]) FROM [dbo].[OrdersData] WHERE [OrderStatus] = 7 AND [GOBPId] = " + Session["adminObp"]).ToString();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetOrderCount", ex.Message.ToString());
            return;
        }
    }

    private void GetGobpInfo()
    {
        string custName = c.GetReqData("[dbo].[OBPData]", "[OBP_ApplicantName]", "[OBP_ID] = " + Session["adminObp"]).ToString();
        firstChar = custName.Substring(0, 1);
        string[] arrSessionName = custName.ToString().Split(' ');
        sessionName = custName.ToString();
        // array of colors
        string[] colors = { "#881798", "#e3008c", "#ffb900", "#107c10", "#da3b01", "#e3008c", "#00b7c3", "#0078d7", "#744da9", "#ff4343" };
        Random rNo = new Random();
        int index = rNo.Next(colors.Length);
        bgColor = "background:" + colors[index].ToString();
        string regDate = c.GetReqData("[dbo].[OBPData]", "[OBP_JoinDate]", "[OBP_ID] = " + Session["adminObp"]).ToString();
        joinDate = Convert.ToDateTime(regDate).ToString("dd MMM, yyyy");
    }
}