using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class customer_MasterCustomer : System.Web.UI.MasterPage
{
    iClass c = new iClass();
    public string firstChar, sessionName, bgColor, rootPath;
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "sessionPingTrigger();", true);
        if (!IsPostBack)
        {
            //rootPath = c.ReturnHttp();
            if (Session["genericCust"] == null)
            {
                Response.Redirect(Master.rootPath + "login", false);   
            }
            else
            {
                GetCustomerInfo();
            }
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["genericCust"] == null)
        {
            Response.Redirect(Master.rootPath + "login", false);
        }
        else
        {
            GetCustomerInfo();
        }
    }

    
    public void GetCustomerInfo()
    {
        string custName = c.GetReqData("CustomersData", "CustomerName", "CustomrtID=" + Session["genericCust"]).ToString();
        firstChar = custName.Substring(0, 1);
        string[] arrSessionName = custName.ToString().Split(' ');
        sessionName = arrSessionName[0].ToString();
        // array of colors
        string[] colors = { "#881798", "#e3008c", "#ffb900", "#107c10", "#da3b01", "#e3008c", "#00b7c3", "#0078d7", "#744da9", "#ff4343" };
        Random rNo = new Random();
        int index = rNo.Next(colors.Length);
        bgColor = "background:" + colors[index].ToString();
    }
}
