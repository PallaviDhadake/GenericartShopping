using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
public partial class obp_gobp_treeView : System.Web.UI.Page
{
    iClass c = new iClass();
    public string joinlevelstr;
    protected void Page_Load(object sender, EventArgs e)
    {
        //GetProductData();
        GetProductData1();
    }


    public void GetProductData1()
    {
        try
        {
            StringBuilder strMarkup = new StringBuilder();
            string myOBPUserId = c.GetReqData("OBPData", "OBP_UserID", "OBP_ID=" + Convert.ToInt32(Session["adminObp"])).ToString();

            using (DataTable dtobpdata = c.GetDataTable("Select OBP_UserID,  OBP_Ref_UserId, OBP_JoinLevel, OBP_ApplicantName from OBPData where OBP_DelMark=0 AND OBP_Ref_UserId='" + myOBPUserId + "'"))
            {
                if (dtobpdata.Rows.Count > 0)
                {
                    string mainUserJoinLevel = c.GetReqData("OBPData", "OBP_JoinLevel", "OBP_UserID='"+ myOBPUserId + "'").ToString();
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


                    int maxJoinLevel =Convert.ToInt32(mainUserJoinLevel) + 5; // Change this to the maximum join level you have

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
            c.ErrorLogHandler(this.ToString(), "btnSave_Click", ex.Message.ToString());
            return;
        }
    }


    //public void GetProductData()
    //{
    //    try
    //    {
    //        StringBuilder strMarkup = new StringBuilder();
    //        // Get Parent / Referral OBP Name with SESSION Id
    //        string myOBPUserId = c.GetReqData("OBPData", "OBP_UserID", "OBP_ID=" + Convert.ToInt32(Session["adminObp"])).ToString();

    //        using (DataTable dtobpdata = c.GetDataTable("Select OBP_Ref_UserId, OBP_JoinLevel, OBP_ApplicantName from OBPData where OBP_DelMark=0 AND OBP_Ref_UserId='"+ myOBPUserId + "'"))
    //        {

    //            if (dtobpdata.Rows.Count > 0)
    //            {
    //                // strMarkup.Append("<div class=\"row gy-5\">");
    //                foreach (DataRow row in dtobpdata.Rows)
    //                {

    //                    if (row["OBP_JoinLevel"].ToString() == "1")
    //                    {

    //                        strMarkup.Append("<div class=\"joinlevel\">");
    //                        strMarkup.Append("<div class=\"p-2\">");
    //                        strMarkup.Append("<p class=\"text-white bold_weight levelnum\">" + row["OBP_JoinLevel"].ToString() + "</p>");
    //                        strMarkup.Append("</div>");
    //                        strMarkup.Append("</div>");

    //                        strMarkup.Append("<div class=\"row justify-content-center\">");
    //                        foreach (DataRow prow in dtobpdata.Rows)
    //                        {
    //                            strMarkup.Append("<div class=\"col-3\">");
    //                            strMarkup.Append("<img src=\"../images/icons/gobp-treeView.png\" class=\"img-fluid mr-3 mt-2\" />");
    //                            strMarkup.Append("<p class=\"bold_weight mb-0\">" + prow["OBP_Ref_UserId"].ToString() + "</p>");
    //                            strMarkup.Append("<span class=\"bold_weight\">" + prow["OBP_ApplicantName"] + "</span>");
    //                            strMarkup.Append("</div>");
    //                            strMarkup.Append("<span class=\"space15\"></span>");
    //                        }
    //                        strMarkup.Append("</div>");
    //                    }
    //                    if (row["OBP_JoinLevel"].ToString() == "2")
    //                    {

    //                        strMarkup.Append("<div class=\"joinlevel\">");
    //                        strMarkup.Append("<div class=\"p-2\">");
    //                        strMarkup.Append("<p class=\"text-white bold_weight levelnum\">" + row["OBP_JoinLevel"].ToString() + "</p>");
    //                        strMarkup.Append("</div>");
    //                        strMarkup.Append("</div>");

    //                        strMarkup.Append("<div class=\"row justify-content-center\">");
    //                        foreach (DataRow prow in dtobpdata.Rows)
    //                        {
    //                            strMarkup.Append("<div class=\"col-3\">");
    //                            strMarkup.Append("<img src=\"../images/icons/gobp-treeView.png\" class=\"img-fluid mr-3 mt-2\" />");
    //                            strMarkup.Append("<p class=\"bold_weight mb-0\">" + prow["OBP_Ref_UserId"].ToString() + "</p>");
    //                            strMarkup.Append("<span class=\"bold_weight\">" + prow["OBP_ApplicantName"] + "</span>");
    //                            strMarkup.Append("</div>");
    //                            strMarkup.Append("<span class=\"space15\"></span>");
    //                        }
    //                        strMarkup.Append("</div>");
    //                    }

                       
    //                    if (row["OBP_JoinLevel"].ToString() == "3")
    //                    {

    //                        strMarkup.Append("<div class=\"joinlevel\">");
    //                        strMarkup.Append("<div class=\"p-2\">");
    //                        strMarkup.Append("<p class=\"text-white bold_weight levelnum\">" + row["OBP_JoinLevel"].ToString() + "</p>");
    //                        strMarkup.Append("</div>");
    //                        strMarkup.Append("</div>");

    //                        strMarkup.Append("<div class=\"row justify-content-center\">");
    //                        foreach (DataRow prow in dtobpdata.Rows)
    //                        {
    //                            strMarkup.Append("<div class=\"col-3\">");
    //                            strMarkup.Append("<img src=\"../images/icons/gobp-treeView.png\" class=\"img-fluid mr-3 mt-2\" />");
    //                            strMarkup.Append("<p class=\"bold_weight mb-0\">" + prow["OBP_Ref_UserId"].ToString() + "</p>");
    //                            strMarkup.Append("<span class=\"bold_weight\">" + prow["OBP_ApplicantName"] + "</span>");
    //                            strMarkup.Append("</div>");
    //                            strMarkup.Append("<span class=\"space15\"></span>");
    //                        }
    //                        strMarkup.Append("</div>");
    //                    }
    //                    if (row["OBP_JoinLevel"].ToString() == "4")
    //                    {

    //                        strMarkup.Append("<div class=\"joinlevel\">");
    //                        strMarkup.Append("<div class=\"p-2\">");
    //                        strMarkup.Append("<p class=\"text-white bold_weight levelnum\">" + row["OBP_JoinLevel"].ToString() + "</p>");
    //                        strMarkup.Append("</div>");
    //                        strMarkup.Append("</div>");

    //                        strMarkup.Append("<div class=\"row justify-content-center\">");
    //                        foreach (DataRow prow in dtobpdata.Rows)
    //                        {
    //                            strMarkup.Append("<div class=\"col-3\">");
    //                            strMarkup.Append("<img src=\"../images/icons/gobp-treeView.png\" class=\"img-fluid mr-3 mt-2\" />");
    //                            strMarkup.Append("<p class=\"bold_weight mb-0\">" + prow["OBP_Ref_UserId"].ToString() + "</p>");
    //                            strMarkup.Append("<span class=\"bold_weight\">" + prow["OBP_ApplicantName"] + "</span>");
    //                            strMarkup.Append("</div>");
    //                            strMarkup.Append("<span class=\"space15\"></span>");
    //                        }
    //                        strMarkup.Append("</div>");
    //                    }

    //                }
    //                joinlevelstr = strMarkup.ToString();

    //            }
    //            else
    //            {
    //                joinlevelstr = "<span class=\"infoClr\">Nothing to display come back later.....</span>";
    //            }
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
    //        c.ErrorLogHandler(this.ToString(), "btnSave_Click", ex.Message.ToString());
    //        return;
    //    }
    //}
}