using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class admingenshopping_saving_calculator_list : System.Web.UI.Page
{
    iClass c = new iClass();
    public string totalSum, totalPercentage, errMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillGrid();
        }
    }

    private void FillGrid()
    {
        try
        {
            using (DataTable dtSavingCalc = c.GetDataTable("Select CalcID, Convert(varchar(20), CalcDate, 103) as cDate, MobileNumber, " +
                " BrandMedicine, NCHAR(8377)+convert(varchar(20), BrandPrice) as BrandPrice, GenericCode, " +
                " NCHAR(8377)+convert(varchar(20), GenericPrice) as GenericPrice, NCHAR(8377)+convert(varchar(20), SavingAmount) as SavingAmount, " +
                " convert(varchar(20), SavingPercent)+'%' as netSaving From SavingCalc Order By CalcDate DESC, CalcID DESC"))
            {
                gvCalc.DataSource = dtSavingCalc;
                gvCalc.DataBind();

                if (gvCalc.Rows.Count > 0)
                {
                    gvCalc.UseAccessibleHeader = true;
                    gvCalc.HeaderRow.TableSection = TableRowSection.TableHeader;
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