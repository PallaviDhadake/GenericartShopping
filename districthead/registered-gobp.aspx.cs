using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
public partial class districthead_registered_gobp : System.Web.UI.Page
{
    iClass c = new iClass();
    public string pgTitle, enqCount, headInfo, clsName, followupHistory;
    public string[] enqData = new string[50];//42
    public string[] countData = new string[10];
    public string[] custcountData = new string[30];
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            btnCancel.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnCancel, null) + ";");

            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    readFrEnquiry.Visible = true;
                    viewFrEnquiry.Visible = false;
                    lblId.Text = Convert.ToInt32(Request.QueryString["id"]).ToString();
                    GetGOBPEnqData(Convert.ToInt32(Request.QueryString["id"]));
                   // GetFollowupHistory();
                   // GetCount();
                    GetOBPCustCom();
                }
                else
                {
                    viewFrEnquiry.Visible = true;
                    readFrEnquiry.Visible = false;


                    FillGrid();
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


    private void GetOBPCustCom()
    {
        try
        {
            int obpid = Convert.ToInt32(Request.QueryString["id"].ToString());
            //Total Customer
            custcountData[0] = c.returnAggregate("SELECT ISNULL(COUNT([CustomrtID]),0) FROM [dbo].[CustomersData] WHERE [delMark] = 0 AND [CustomerActive] = 1 AND [FK_ObpID] = " + obpid).ToString();

            //This Month Customer
            custcountData[1] = c.returnAggregate("SELECT ISNULL(COUNT([CustomrtID]),0) FROM [dbo].[CustomersData] WHERE [delMark] = 0 AND [CustomerActive] = 1 AND YEAR([CustomerJoinDate]) = YEAR(GETDATE()) AND MONTH([CustomerJoinDate]) = MONTH(GETDATE()) AND [FK_ObpID] = " + obpid).ToString();

            //Last Month Customer
            custcountData[2] = c.returnAggregate("SELECT ISNULL(COUNT([CustomrtID]),0) FROM [dbo].[CustomersData] WHERE[delMark] = 0 AND[CustomerActive] = 1 AND YEAR([CustomerJoinDate]) = YEAR(DATEADD(MONTH, -1, GETDATE()))AND MONTH([CustomerJoinDate]) = MONTH(DATEADD(MONTH, -1, GETDATE())) AND[FK_ObpID] = " + obpid).ToString();

            //Here commission query
            //Total Commisson
            custcountData[3] = c.returnAggregate("SELECT ROUND(ISNULL(SUM(OBPComTotal), 0),1) FROM OrdersData WHERE GOBPId = " + obpid).ToString();

            //This Month Commission
            custcountData[4] = c.returnAggregate("SELECT ROUND(ISNULL(SUM(OBPComTotal),0),1) FROM OrdersData WHERE  YEAR([OrderDate]) = YEAR(GETDATE()) AND MONTH([OrderDate]) = MONTH(GETDATE()) AND GOBPId =" + obpid).ToString();

            //Last month Commission
            custcountData[5] = c.returnAggregate("SELECT ROUND(ISNULL(SUM(OBPComTotal),0),1) FROM OrdersData WHERE YEAR([OrderDate]) = YEAR(DATEADD(MONTH, -1, GETDATE()))AND MONTH([OrderDate]) = MONTH(DATEADD(MONTH, -1, GETDATE())) AND GOBPId =" + obpid).ToString();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetOBPCustCom", ex.Message.ToString());
            return;
        }
    }


    private void FillGrid()
    {
        try
        {
            string DhUserId =(c.GetReqData("DistrictHead", "DistHdUserId", "DistHdId="+Session["adminDH"] +"").ToString());

            string strQuery = "";
            strQuery = @"SELECT 
                                a.[OBP_ID], 
                            	a.[OBP_ApplicantName], 
                            	CONVERT(VARCHAR(20), a.[OBP_JoinDate], 103) as joinDate, 
                            	a.[OBP_MobileNo], 
                            	a.[OBP_StatusFlag], 
                            	a.[OBP_DH_Name] as dh,"
                                 + "(SELECT COUNT([CustomrtID]) FROM[dbo].[CustomersData] WHERE[delMark] = 0 AND[CustomerActive] = 1 AND [FK_ObpID] = a.[OBP_ID]) as custCount"
                             + " From [dbo].[OBPData] a Where OBP_DelMark=0 AND OBP_StatusFlag='Active' AND OBP_DH_UserId='"+ DhUserId + "'";

            using (DataTable dtFrEnq = c.GetDataTable(strQuery))
            {
                gvGOBP.DataSource = dtFrEnq;
                gvGOBP.DataBind();
                if (dtFrEnq.Rows.Count > 0)
                {
                    gvGOBP.UseAccessibleHeader = true;
                    gvGOBP.HeaderRow.TableSection = TableRowSection.TableHeader;
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

    protected void gvGOBP_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litView = (Literal)e.Row.FindControl("litView");

                litView.Text = "<a href=\"registered-gobp.aspx?id=" + e.Row.Cells[0].Text + "\" class=\"gView\" ></a>";

            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Error Occoured while processing', type: 'error'});", true);
            c.ErrorLogHandler(this.ToString(), "gvGOBP_RowDataBound", ex.Message.ToString());
            return;
        }
    }

    private void GetGOBPEnqData(int enqIdX)
    {
        try
        {
            using (DataTable dtEnq = c.GetDataTable("Select * From OBPData Where OBP_ID=" + enqIdX))
            {
                if (dtEnq.Rows.Count > 0)
                {
                    lblId.Text = enqIdX.ToString();
                    DataRow row = dtEnq.Rows[0];

                    if (row["OBP_JoinDate"] != DBNull.Value && row["OBP_JoinDate"] != "" && row["OBP_JoinDate"] != null)
                    {
                        enqData[1] = Convert.ToDateTime(row["OBP_JoinDate"]).ToString("dd/MM/yyyy");
                    }

                    enqData[2] = row["OBP_ApplicantName"] != DBNull.Value ? row["OBP_ApplicantName"].ToString() : "";


                    enqData[8] = row["OBP_UserID"] != DBNull.Value ? row["OBP_UserID"].ToString() : "";
                    enqData[4] = row["OBP_DH_Name"] != DBNull.Value ? row["OBP_DH_Name"].ToString() : "";
                    object mobNo = c.GetReqData("DistrictHead", "DistHdMobileNo", "DistHdUserId='" + row["OBP_DH_UserId"] + "'");
                    if (mobNo != DBNull.Value && mobNo != null && mobNo.ToString() != "")
                    {
                        enqData[5] = mobNo.ToString();
                    }
                    enqData[12] = row["OBP_MobileNo"] != DBNull.Value ? row["OBP_MobileNo"].ToString() : "";
                    enqData[13] = row["OBP_EmailId"] != DBNull.Value ? row["OBP_EmailId"].ToString() : "";
                    string obpType = "";
                    switch (Convert.ToInt32(row["OBP_FKTypeID"]))
                    {
                        case 1: obpType = "30K"; break;
                        case 2: obpType = "60K"; break;
                        case 3: obpType = "1Lac"; break;
                    }
                    enqData[0] = obpType;


                    int custCoumt = Convert.ToInt32(c.returnAggregate("Select Count(CustomrtID) From CustomersData Where delMark=0 AND CustomerActive=1 AND FK_ObpID=" + enqIdX));

                    enqData[3] = "<a href=\"gobp-customers.aspx?gobpId=" + enqIdX + "\" class=\"text-dark text-bold text-md\" target=\"_blank\">" + custCoumt + "</a>";
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Error Occoured while processing', type: 'error'});", true);
            c.ErrorLogHandler(this.ToString(), "GetFrEnqData", ex.Message.ToString());
            return;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("registered-gobp.aspx");
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

    }
}