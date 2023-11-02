using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class admingenshopping_assign_gmplcode_to_enquiry : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        btnAssign.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnAssign, null) + ";");
    }
    protected void btnAssign_Click(object sender, EventArgs e)
    {
        try
        {
            using (DataTable dtCode = c.GetDataTable("Select * From SavingCalcItems Where SUBSTRING(GenericMedicine, 1,3)<>'GMP'"))
            {
                if (dtCode.Rows.Count > 0)
                {
                    foreach (DataRow row in dtCode.Rows)
                    {
                        string gmplCode = c.GetReqData("SurveyMedicines", "GenericCode", "BrandName='" + row["BrandMedicine"].ToString() + "' AND ContentName='" + row["GenericMedicine"].ToString() + "'").ToString();

                        c.ExecuteQuery("Update SavingCalcItems Set GenericCode='" + gmplCode + "' Where CalcItemID=" + row["CalcItemID"]);

                    }

                    errMsg = c.ErrNotification(1, "Code Assigned Successfully..!!");

                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}