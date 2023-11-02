using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class franchisee_generic_mitra_info : System.Web.UI.Page
{
    public string[] gmInfo = new string[15]; 

    iClass c = new iClass();
    GenericMitraInfo gmC = new GenericMitraInfo();
    protected void Page_Load(object sender, EventArgs e)
    {
        string rootPathStr = c.ReturnHttp();
        if (Request.QueryString["genmitraID"] != null)
        {
            gmC.GenericMitraData(Convert.ToInt32(Request.QueryString["genmitraID"]));
            gmInfo[0] = gmC.GMName;
            gmInfo[1] = gmC.GMMobile;
            gmInfo[2] = gmC.GMEmail;
            gmInfo[3] = gmC.GMStateName;
            gmInfo[4] = gmC.GMDistrictName;
            gmInfo[5] = gmC.GMCitytName;
            gmInfo[6] = "<a href = \"" + rootPathStr + "upload/genmitradocs/" +  gmC.GMPancardDoc + "\" class=\"link-primary\" target=\"_blank\">See Document</a>";
            gmInfo[7] = "<a href = \"" + rootPathStr + "upload/genmitradocs/" + gmC.GMAadharDoc + "\" class=\"link-primary\" target=\"_blank\">See Document</a>";
            gmInfo[8] = "<a href = \"" + rootPathStr + "upload/genmitradocs/" + gmC.GMBankDoc + "\" class=\"link-primary\" target=\"_blank\">See Document</a>";
        }
    }
}