using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
public partial class GOBPDH_gobp_lookup_details : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] arrGobpData = new string[50];
    public string[] arrShopInfo = new string[30];
    public string followupHistory, rootPath, joinlevelstr;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetGOBPData();
            GetGOBPLookup(Convert.ToInt32(Request.QueryString["id"]));
            //GetFollowupHistory();
            OBPJoinLevelTree();
        }
       
    }

    private void GetGOBPData()
    {
        try
        {

            GobpInfo obpinfo = new GobpInfo();

            int ObpId = Convert.ToInt32(Request.QueryString["id"]);

            obpinfo.OBPData(ObpId);

            arrGobpData[0] = obpinfo.ApplicantName;
            arrGobpData[1] = obpinfo.TypeFirm;
            arrGobpData[2] = obpinfo.Address;
            arrGobpData[3] = obpinfo.Age.ToString();
            arrGobpData[4] = obpinfo.MaritalStatus;
            arrGobpData[5] = obpinfo.EmailId;
            arrGobpData[6] = obpinfo.MobileNo;
            arrGobpData[7] = obpinfo.WhatsAppNo;
            object state = c.GetReqData("StatesData", "StateName", "StateID="+obpinfo.State);
            arrGobpData[8] = state.ToString();
            object District = c.GetReqData("DistrictsData", "DistrictName", "DistrictId=" + obpinfo.District);
            arrGobpData[9] = District.ToString();
            arrGobpData[10] = obpinfo.City;
            arrGobpData[11] = obpinfo.OwnerEducation;
            arrGobpData[12] = obpinfo.OwnerOccupation;
            arrGobpData[13] = obpinfo.LegalMatter;
            arrGobpData[14] = obpinfo.ResidenceFrom;

            string resume = obpinfo.Resume;
            arrGobpData[16] = "<a href=\""+Master.rootPath + "upload/gobpData/resume/"+ resume + "\" class=\"text-decoration-none text-info\" target=\"_blank\">View Resume</a>";

            string address = obpinfo.AddressProof1;
            arrGobpData[18] = "<img src=\"" + Master.rootPath + "upload/gobpData/addressProof/" + address + " \" style=\"width:200px\">";

            string address1 = obpinfo.AddressProof2;
            arrGobpData[35] = "<img src=\"" + Master.rootPath + "upload/gobpData/addressProof/" + address1 + " \" style=\"width:200px\">";

            string IdProof1 = obpinfo.IdProof1;
            arrGobpData[19] = "<img src=\"" + Master.rootPath + "upload/gobpData/idProof/" + IdProof1 + " \" style=\"width:200px\">";

            string IdProof2 = obpinfo.IdProof2;
            arrGobpData[20] = "<img src=\"" + Master.rootPath + "upload/gobpData/idProof/" + IdProof2 + " \" style=\"width:200px\">";

            string profilephoto = obpinfo.ProfilePhoto;
            arrGobpData[15] = "<img src=\"" + Master.rootPath + "upload/gobpData/profilePhoto/" + profilephoto + "\" style=\"width:200px\">";

            // arrGobpData[15] = "<img src=\""+obpinfo.ProfilePhotoPath+"\" style=\"width:200px\">";
            //arrGobpData[16] = "<img src=\"" + obpinfo.ResumePath + "\" style=\"width:200px\">";
            //arrGobpData[17] = "<img src=\"" + obpinfo.imagePath + "\" style=\"width:200px\">";
            // arrGobpData[18] = "<img src=\"" + obpinfo.AddressProof2Path + "\" style=\"width:200px\">";
            //  arrGobpData[19] = "<img src=\"" + obpinfo.Idproof1Path + "\" style=\"width:200px\">";
            //arrGobpData[20] = "<img src=\"" + obpinfo.Idproof2Path + "\" style=\"width:200px\">";

            arrGobpData[21] = obpinfo.UTRNumber;
            arrGobpData[22] = obpinfo.BankName;
            arrGobpData[23] = obpinfo.AccountHolder;
            arrGobpData[24] = obpinfo.PaidAmmount.ToString();
            arrGobpData[25] = obpinfo.TransDate.ToString("dd/MM/yyyy");
            arrGobpData[26] = obpinfo.BirthDate.ToString("dd/MM/yyyy");

            string obpType = "";
            switch (Convert.ToInt32(obpinfo.TypeId))
            {
                case 1: obpType = "30K"; break;
                case 2: obpType = "60K"; break;
                case 3: obpType = "1Lac"; break;
            }
            arrGobpData[27] = obpType;
            arrGobpData[28] = obpinfo.JoinDate.ToString("dd/MM/yyyy");
            arrGobpData[29] = obpinfo.OBPUserID.ToString();
            arrGobpData[30] = obpinfo.DistrictHeadName.ToString();
            object mobNo = c.GetReqData("DistrictHead", "DistHdMobileNo", "DistHdUserId='"+ obpinfo.OBPDH_UserId + "'");
            if (mobNo != DBNull.Value && mobNo != null && mobNo.ToString() != "")
            {
                arrGobpData[31] = mobNo.ToString();
            }

            arrGobpData[32] = obpinfo.IsMLM.ToString();

            //string mlmtype = "";
            //if (obpinfo.IsMLM.ToString() == "1")
            //{
            //    mlmtype = "Yes";
               
            //}
            //else
            //{
            //    mlmtype = "No";
            //    arrGobpData[32] = mlmtype;
            //}

            arrGobpData[33] = obpinfo.OBPZHUserId;
            arrGobpData[34] = obpinfo.OBPRefUserId;
            arrGobpData[36] = obpinfo.JoinLevel.ToString();
            arrGobpData[37] = obpinfo.ShopName.ToString();
            arrGobpData[38] = obpinfo.ShopCode.ToString();

            //arrGobpData[35]=
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Error Occoured while processing', type: 'error'});", true);
            c.ErrorLogHandler(this.ToString(), "GetGOBPData", ex.Message.ToString());
            return;
        }
    }

    private void GetGOBPLookup(int obpid)
    {
        try
        {
            GOBPLookup gobpLoopup = c.GetGOBPLookupDetails(obpid);
            arrShopInfo[0] = gobpLoopup.YearlyGOBPSummary;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Error Occoured while processing', type: 'error'});", true);
            c.ErrorLogHandler(this.ToString(), "GetGOBPLookup", ex.Message.ToString());
            return;
        }
    }


    public void OBPJoinLevelTree()
    {
        try
        {
            StringBuilder strMarkup = new StringBuilder();
           // string myOBPUserId = c.GetReqData("OBPData", "OBP_UserID", "OBP_ID=" + Convert.ToInt32(Session["adminObp"])).ToString();

            string myOBPUserId = c.GetReqData("OBPData", "OBP_UserID", "OBP_ID=" + Request.QueryString["id"]).ToString();

            using (DataTable dtobpdata = c.GetDataTable("Select OBP_UserID,  OBP_Ref_UserId, OBP_JoinLevel, OBP_ApplicantName from OBPData where OBP_DelMark=0 AND OBP_Ref_UserId='" + myOBPUserId + "'"))
            {
                if (dtobpdata.Rows.Count > 0)
                {
                    string mainUserJoinLevel = c.GetReqData("OBPData", "OBP_JoinLevel", "OBP_UserID='" + myOBPUserId + "'").ToString();
                    //strMarkup.Append("<div class=\"text-center\">");
                    strMarkup.Append("<div class=\"mainjoinlevel\">");
                    strMarkup.Append("<div class=\"p-2\">");
                    strMarkup.Append("<p class=\"text-white bold_weight levelnum\">" + mainUserJoinLevel + "</p>");
                    strMarkup.Append("</div>");
                    strMarkup.Append("</div>");
                    strMarkup.Append("<span class=\"space10\"></span>");
                    // strMarkup.Append("<div style=\"margin-right:-22px !important;\">");
                    strMarkup.Append("<img src=\"../images/icons/main-treeviewUser.png\" class=\"mb-2 mt-2 \"/>");
                    strMarkup.Append("<p class=\"bold_weight\">It's You</span>");
                    strMarkup.Append("<span class=\"space20\"></span>");
                    //strMarkup.Append("</div>");
                    //strMarkup.Append("</div>");


                    int maxJoinLevel = Convert.ToInt32(mainUserJoinLevel) + 5; // Change this to the maximum join level you have

                    for (int joinLevel = 1; joinLevel <= maxJoinLevel; joinLevel++)
                    {
                        DataRow[] filteredRows = dtobpdata.Select("OBP_JoinLevel = '" + joinLevel + "'");

                        if (filteredRows.Length > 0)
                        {

                            strMarkup.Append("<div class=\"joinlevel\">");
                            strMarkup.Append("<div class=\"p-2\">");
                            strMarkup.Append("<p class=\"text-white bold_weight levelnum\">" + joinLevel + "</p>");
                            strMarkup.Append("</div>");
                            strMarkup.Append("</div>");
                            strMarkup.Append("<span class=\"space10\"></span>");
                            strMarkup.Append("<div class=\"row justify-content-center\">");
                            foreach (DataRow prow in filteredRows)
                            {
                                strMarkup.Append("<div class=\"col-3\">");
                                strMarkup.Append("<img src=\"../images/icons/gobp-treeView.png\" class=\"img-fluid  mt-2 mb-2\" />");
                                strMarkup.Append("<p class=\"bold_weight mb-0\">" + prow["OBP_UserID"].ToString() + "</p>");
                                strMarkup.Append("<span class=\"bold_weight\">" + prow["OBP_ApplicantName"] + "</span>");
                                strMarkup.Append("</div>");
                                strMarkup.Append("<span class=\"space15\"></span>");
                            }
                            strMarkup.Append("</div>");
                        }
                    }

                    joinlevelstr = strMarkup.ToString();
                }
                else
                {
                    joinlevelstr = "<span class=\"infoClr\">Nothing to display come back later.....</span>";
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occurred While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "OBPJoinLevelTree", ex.Message.ToString());
            return;
        }
    }


    //private void GetFollowupHistory()
    //{
    //    try
    //    {
    //        int gobpIdX = Convert.ToInt32(Request.QueryString["id"]);
    //        using (DataTable dtFlHistory = c.GetDataTable("Select * From FollowupOBP Where FK_OBPId=" + gobpIdX + " Order By FL_ID DESC"))
    //        {
    //            if (dtFlHistory.Rows.Count > 0)
    //            {
    //                StringBuilder strMarkup = new StringBuilder();
    //                foreach (DataRow row in dtFlHistory.Rows)
    //                {
    //                    strMarkup.Append("<div class=\"user-block\">");
    //                    strMarkup.Append("<span class=\"username\">");
    //                    string flBy = c.GetReqData("OBPManager", "OBPManName", "OBPManID=" + row["FK_OBPManID"]).ToString();
    //                    strMarkup.Append("<a href=\"#\">" + flBy + "</a>");
    //                    strMarkup.Append("</span>");
    //                    strMarkup.Append("<span class=\"description\">Follow Up on - " + Convert.ToDateTime(row["FL_Date"]).ToString("dd MMM yyyy hh:mm tt") + "</span>");
    //                    strMarkup.Append("</div>");
    //                    //strMarkup.Append("<h6 class=\"text-indigo\">" + row["FlupRemarkStatus"].ToString() + "</h6>");
    //                    strMarkup.Append("<p class=\"text-bold\">" + row["FL_Remark"].ToString() + "</p>");
    //                    //strMarkup.Append("<i class=\"nav-icon fas fa-clock\"></i><span>Next Follow Up: " + Convert.ToDateTime(row["FlupNextDate"]).ToString("dd MMM yyyy") + ", Time: " + row["FlupNextTime"].ToString() + "</span>");
    //                    strMarkup.Append("<hr />");
    //                }

    //                followupHistory = strMarkup.ToString();
    //            }
    //            else
    //            {
    //                followupHistory = "<span class=\"text-orange text-bold\">No Followup History Found</span>";
    //                return;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
    //        c.ErrorLogHandler(this.ToString(), "GetFollowupHistory", ex.Message.ToString());
    //        return;
    //    }
    //}


    //private void GetGOBPLookup(int obpid)
    //{
    //    try
    //    {
    //        GOBPLookup gobpLoopup = c.GetGOBPLookupDetails(obpid);

    //        GobpInfo obpinfo = c.GetCustLookupDetails(obpid);


    //        arrShopInfo[0] = gobpLoopup.YearlyGOBPSummary;
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Error Occoured while processing', type: 'error'});", true);
    //        c.ErrorLogHandler(this.ToString(), "GetGOBPLookup", ex.Message.ToString());
    //        return;
    //    }
    //}

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("registered-gobp.aspx");
    }

    //protected void btnSave_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        txtRemark.Text = txtRemark.Text.Trim().Replace("'", "");

    //        if (txtRemark.Text == "")
    //        {
    //            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Enter Remark');", true);
    //            return;
    //        }

    //        int maxId = c.NextId("FollowupOBP", "FL_ID");
    //        int gobpIdX = Convert.ToInt32(Request.QueryString["id"]);

    //        //c.ExecuteQuery("Insert Into FollowupOBP (FL_ID, FL_Date, FK_OBPManID, FK_OBPId, FL_Remark) Values (" + maxId + ", '" + DateTime.Now + "',  " + Session["adminObpManager"] + ",  " + gobpIdX + ",  '" + txtRemark.Text + "')");

    //        c.ExecuteQuery("Insert Into FollowupOBP (FL_ID, FL_Date, FK_OBPManID, FK_OBPId, FL_Remark) Values (" + maxId + ", '" + DateTime.Now + "', " + Session["adminObpManager"] + ", " + gobpIdX + ", '" + txtRemark.Text + "') ");

    //        txtRemark.Text = "";

    //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Followup Saved');", true);

    //        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('gobp-lookup-details.aspx?id=" + Request.QueryString["id"] + "', 2000);", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
    //        c.ErrorLogHandler(this.ToString(), "btnSave_Click", ex.Message.ToString());
    //        return;
    //    }
    //}
}