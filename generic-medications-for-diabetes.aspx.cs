using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class generic_medications_for_diabetes : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Add Description Meta Tag.
        HtmlMeta description = new HtmlMeta();
        description.HttpEquiv = "description";
        description.Name = "description";
        description.Content = "Generic drugs tolazamide, chlorpropamide, exenatide etc. are used for diabetes treatment. Find all information about prices, uses, side effects & composition";
        this.Page.Header.Controls.Add(description);
        this.Page.Header.DataBind();
    }
}