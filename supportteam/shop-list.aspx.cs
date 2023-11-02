using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class supportteam_shop_list : System.Web.UI.Page
{
    iClass c = new iClass();
    public string pgTitle, errMsg, frAdhar, frPan, PageTitle;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                FillGrid();
                if (Request.QueryString["rxId"] != null)
                {
                    string rxReqName = c.GetReqData("PrescriptionRequest", "PreReqName", "PreReqID=" + Request.QueryString["rxId"]).ToString();
                    PageTitle = "Forward " + rxReqName + "'s Prescription to Shop";
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "Page_Load", ex.Message.ToString());
            return;
        }
    }

    private void FillGrid()
    {
        try
        {
            using (DataTable dtShops = c.GetDataTable("Select a.FranchID, a.FranchShopCode, a.FranchName, a.FranchOwnerName, a.FranchMobile, a.FranchPassword, a.FranchAddress, a.FranchPinCode, b.CityName From FranchiseeData a Inner Join CityData b On a.FK_FranchCityId=b.CityID Where a.FranchActive=1"))
            {
                gvShops.DataSource = dtShops;
                gvShops.DataBind();

                if (dtShops.Rows.Count > 0)
                {
                    gvShops.UseAccessibleHeader = true;
                    gvShops.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "FillGrid", ex.Message.ToString());
            return;
        }
    }

    protected void gvShops_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    Button btnForword = (Button)e.Row.FindControl("cmdForword");
            //    if (Request.QueryString["rxId"] != null)
            //    {
            //        if (c.IsRecordExist("Select PrescFwdID From PrescriptionForword Where FK_PreReqID=" + Request.QueryString["rxId"] + " AND FK_DoctorID=" + Session["adminDoctor"] + " AND FK_FranchID=" + e.Row.Cells[0].Text))
            //        {
            //            btnForword.Visible = false;
            //        }
            //    }
            //    else
            //    {
            //        btnForword.Visible = false;
            //    }
            //}
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "gvShops_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    //protected void gvShops_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        GridViewRow gRow = (GridViewRow)((Button)e.CommandSource).NamingContainer;
    //        if (e.CommandName == "gvForowrd")
    //        {
    //            if (c.IsRecordExist("Select PrescFwdID From PrescriptionForword Where FK_PreReqID=" + Request.QueryString["rxId"] + " AND FK_DoctorID=" + Session["adminDoctor"]))
    //            {
    //                FillGrid();
    //                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'you can send prescription to only one shop');", true);
    //                return;
    //            }
    //            if (!c.IsRecordExist("Select PrescFwdID From PrescriptionForword Where FK_PreReqID=" + Request.QueryString["rxId"] + " AND FK_DoctorID=" + Session["adminDoctor"] + " AND FK_FranchID=" + gRow.Cells[0].Text))
    //            {
    //                int maxId = c.NextId("PrescriptionForword", "PrescFwdID");
    //                string rxImgName = c.GetReqData("PrescriptionUploads", "PreUploadCopy", "FK_PreReqID=" + Request.QueryString["rxId"]).ToString();

    //                c.ExecuteQuery("Insert Into PrescriptionForword (PrescFwdID, PrescFwdDate, FK_PreReqID, FK_FranchID, " +
    //                    " FK_DoctorID, PrescImg, PrescFwdStatus) Values (" + maxId + ", '" + DateTime.Now + "', " + Request.QueryString["rxId"] +
    //                    ", " + gRow.Cells[0].Text + ", " + Session["adminDoctor"] + ", '" + rxImgName + "', 0)");

    //                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Prescription sent to shop');", true);
    //            }

    //            FillGrid();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
    //        c.ErrorLogHandler(this.ToString(), "gvShops_RowCommand", ex.Message.ToString());
    //        return;
    //    }
    //}
}