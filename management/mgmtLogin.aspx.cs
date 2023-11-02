﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class management_mgmtLogin : System.Web.UI.Page
{
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string encryptString = Request.QueryString["id"].ToString();

            string descryptString = c.DecryptString(encryptString);

            int mgId = Convert.ToInt32(c.GetReqData("ManagementTeam", "EmpId", "MgmtUserId='" + descryptString + "' AND DelMark=0"));
            Session["adminMgmt"] = mgId;

            Response.Redirect("dashboard.aspx", false);
        }
    }

    
}