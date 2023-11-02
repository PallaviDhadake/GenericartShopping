using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class obpmanager_MasterObpManager : System.Web.UI.MasterPage
{
    iClass c = new iClass();
    public string rootPath, welcomeMessage;
    public string[] franchiseeData = new string[5];
    protected void Page_Load(object sender, EventArgs e)
    {
       

        //if (Request.Cookies["MyUrlCookie"] != null)
        //{
        //    // Retrieve the cookie value
        //    string cookieValue = Request.Cookies["MyCookie"].Value;
        //    // Clear the cookie
        //    Response.Cookies["MyCookie"].Expires = DateTime.Now.AddDays(-1);
        //    Response.Redirect(cookieValue);

        //}
       
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        rootPath = c.ReturnHttp();

        //if (Session["adminObpManager"] == null)
        //{
        //    Response.Redirect("Default.aspx");
        //}

        if (Session["adminObpManager"] == null)
        {
            string urlRequested = Request.Url.ToString();

            // Create a new cookie
            HttpCookie myCookie = new HttpCookie("MyUrlCookie");

            // Set the value of the cookie
            myCookie.Value = urlRequested;

            // Set additional properties if needed
            //myCookie.Expires = DateTime.Now.AddDays(7);

            Response.Cookies.Add(myCookie);
            Response.Redirect("Default.aspx");
        }


        welcomeMessage = "Welcome <span class=\"greenName\" >" + c.GetReqData("OBPManager", "OBPManUserId", "OBPManID=" + Session["adminObpManager"]).ToString() + "</span>";
    }
}
