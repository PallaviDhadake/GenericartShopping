using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class generic_mitra_info : System.Web.UI.Page
{
    iClass c = new iClass();
    public string rootPath, currentYear;
    protected void Page_Load(object sender, EventArgs e)
    {
        rootPath = c.ReturnHttp();
        currentYear = DateTime.Now.Year.ToString();
    }
}