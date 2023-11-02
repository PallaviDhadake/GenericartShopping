using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;
public partial class official_gobp_registration_master : System.Web.UI.Page
{
    iClass c = new iClass();
    public string rootPath, pgTitle;
    public string[] enqData = new string[50];
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            btnCancel.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnCancel, null) + ";");

            if (!IsPostBack)
            {

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
                        //GetOBPCustCom();
                    }
                    else
                    {
                        viewFrEnquiry.Visible = true;
                        readFrEnquiry.Visible = false;


                        FillGrid();
                    }


                    FillGrid();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "showNotification({message: 'Error Occoured while processing', type: 'error'});", true);
            c.ErrorLogHandler(this.ToString(), "Page_Load", ex.Message.ToString());
            return;
        }
    }

    private void FillGrid()
    {
        try
        {
            // string DhUserId = (c.GetReqData("DistrictHead", "DistHdUserId", "DistHdId=" + Session["adminDH"] + "").ToString());

            string strQuery = "";
            if (Request.QueryString["type"] == "new" || Request.QueryString["type"] == null)
            {
                //string strQuery = "";
                strQuery = @"Select a.OBP_ID, isnull(OBP_UserID, '-') as OBP_UserID,  a.OBP_City, a.OBP_ApplicantName, c.OBPAmount, Convert(varchar(20), a.OBP_JoinDate, 103) as joinDate, a.OBP_MobileNo 
                         From OBPData a  
                         Inner Join OBPTypes c On a.OBP_FKTypeID=c.OBPTypeID Where a.OBP_DelMark=0 AND a.OBP_StatusFlag='Pending'
                          ORDER BY a.OBP_ID DESC";

            }
            else if(Request.QueryString["type"] == "active")
            {
                
                strQuery = @"Select a.OBP_ID, isnull(OBP_UserID, '-') as OBP_UserID,  a.OBP_City, a.OBP_ApplicantName, c.OBPAmount, Convert(varchar(20), a.OBP_JoinDate, 103) as joinDate, a.OBP_MobileNo 
                         From OBPData a  
                         Inner Join OBPTypes c On a.OBP_FKTypeID=c.OBPTypeID Where a.OBP_DelMark=0 AND a.OBP_StatusFlag='Active'
                          ORDER BY a.OBP_ID DESC";
            }



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
                Literal litEdit = (Literal)e.Row.FindControl("litEdit");
                Literal litView = (Literal)e.Row.FindControl("litView");

                if (Request.QueryString["type"] != null)
                {
                    litEdit.Text = "<a href=\"gobp-edit-master.aspx?type=" + Request.QueryString["type"] + "&action=edit&id=" + e.Row.Cells[0].Text + "\" class=\"gAnch\" title=\"Edit  Data\"></a>";
                    litView.Text = "<a href=\"gobp-registration-master.aspx?type=" + Request.QueryString["type"] + "&id=" + e.Row.Cells[0].Text + "\" class=\"gView\" title=\"Edit Franchisee Data\"></a>";
                }
                else
                {
                    litView.Text = "<a href=\"gobp-registration-master.aspx?id=" + e.Row.Cells[0].Text + "\" class=\"gView\" title=\"Read Franchisee Enquiry Data\"></a>";
                    litEdit.Text = "<a href=\"gobp-edit-master.aspx?action=edit&id=" + e.Row.Cells[0].Text + "\" class=\"gAnch\" title=\"Edit Franchisee Data\"></a>";
                }

                //string gobpStatus = "";
                //switch (e.Row.Cells[8].Text)
                //{
                //    case "Pending":
                //        gobpStatus = "<span class=\"stSnooze tiny\" >Pending</span>";
                //        break;
                //    case "Active":
                //        gobpStatus = "<span class=\"stCompleted tiny\" >Active</span>";
                //        break;
                //    case "Blacked":
                //        gobpStatus = "<span class=\"stPending tiny\" >Blocked</span>";
                //        break;
                //}

                //e.Row.Cells[8].Text = gobpStatus.ToString();



                //Check Data present in Ecom or not
                //Literal litEcom = new Literal();
                //litEcom = (Literal)e.Row.FindControl("litEcom");

                //object UserId = "";

                //if (UserId != DBNull.Value && UserId != null && UserId.ToString() != "")
                //{
                //    UserId = c.GetReqData("OBPData", "OBP_UserID", "OBP_ID=" + e.Row.Cells[0].Text).ToString();
                //}

                //object UserId = c.GetReqData("OBPData", "OBP_ShopCode", "OBP_ID=" + e.Row.Cells[3].Text);

                //if (UserId != null)
                //{
                //UserId = "";
                //if (ecom.IsRecordExist("Select OBP_ID From OBPData Where OBP_UserID='" + e.Row.Cells[3].Text + "'"))
                //{
                //    litEcom.Text = "<img src=\"../images/icons/tick.png\">";
                //}
                //else
                //{
                //    litEcom.Text = "<img src=\"../images/icons/close.png\">";
                //}
                
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
                    enqData[4] = row["OBP_TypeFirm"] != DBNull.Value ? row["OBP_TypeFirm"].ToString() : "";

                   

                    enqData[3] = row["OBP_ShopName"] != DBNull.Value ? row["OBP_ShopName"].ToString() : "";


                    //enqData[4] = row["frShopAddress"] != DBNull.Value ? row["frShopAddress"].ToString() : "";

                    if (row["OBP_Address"] != DBNull.Value)
                    {
                        if (row["OBP_Address"].ToString().Length > 60)
                        {
                            string p1, p2;
                            p1 = row["OBP_Address"].ToString().Substring(0, 60);
                            int len = row["OBP_Address"].ToString().Length;

                            p2 = row["OBP_Address"].ToString().Substring(61, (row["OBP_Address"].ToString().Length - 61));
                            enqData[5] = p1 + "<br/>-" + p2;
                        }
                        else
                        {
                            enqData[5] = row["OBP_Address"].ToString();
                        }
                    }

                    //enqData[8] = row["OBP_ShopCode"] != DBNull.Value ? row["OBP_ShopCode"].ToString() : "";
                    enqData[8] = row["OBP_UserID"] != DBNull.Value ? row["OBP_UserID"].ToString() : "";
                    //enqData[6] = row["frLatLong"] != DBNull.Value ? row["frLatLong"].ToString() : "";

                    enqData[9] = c.GetReqData("StatesData", "stateName", "stateId=" + row["OBP_StateID"]).ToString();
                    enqData[10] = c.GetReqData("DistrictsData", "districtName", "districtId=" + row["OBP_DistrictID"]).ToString();
                    //enqData[9] = c.GetReqData("CityData", "cityName", "cityId=" + row["cityId"]).ToString();
                    enqData[11] = row["OBP_City"] != DBNull.Value ? row["OBP_City"].ToString() : "";

                    //enqData[10] = row["pinCode"] != DBNull.Value ? row["pinCode"].ToString() : "";
                    enqData[12] = row["OBP_MobileNo"] != DBNull.Value ? row["OBP_MobileNo"].ToString() : "";
                    //enqData[12] = row["frLandline"] != DBNull.Value ? row["frLandline"].ToString() : "";
                    enqData[13] = row["OBP_EmailId"] != DBNull.Value ? row["OBP_EmailId"].ToString() : "";
                    enqData[14] = row["OBP_WhatsApp"] != DBNull.Value ? row["OBP_WhatsApp"].ToString() : "";
                    //enqData[15] = row["frOwnerAddress"] != DBNull.Value ? row["frOwnerAddress"].ToString() : "";

                    if (row["OBP_BirthDate"] != DBNull.Value && row["OBP_BirthDate"].ToString() != "" && row["OBP_BirthDate"].ToString() != null)
                    {
                        enqData[6] = Convert.ToDateTime(row["OBP_BirthDate"]).ToString("dd/MM/yyyy");
                    }


                    enqData[7] = row["OBP_Age"] != DBNull.Value ? row["OBP_Age"].ToString() : "";
                    enqData[15] = row["OBP_OwnerEdu"] != DBNull.Value ? row["OBP_OwnerEdu"].ToString() : "";
                   
                    enqData[16] = row["OBP_OwnerOccup"] != DBNull.Value ? row["OBP_OwnerOccup"].ToString() : "";
                    
                    enqData[17] = row["OBP_LegalMatter"] != DBNull.Value ? row["OBP_LegalMatter"].ToString() : "";

                    enqData[18] = row["OBP_ResidenceFrom"] != DBNull.Value ? row["OBP_ResidenceFrom"].ToString() : "";
                    enqData[0] = c.GetReqData("OBPTypes", "OBPTypeName", "OBPTypeID=" + row["OBP_FKTypeID"]).ToString();

                    
                    enqData[19] = row["OBP_MaritalStatus"] != DBNull.Value ? row["OBP_MaritalStatus"].ToString() : "";
                    //enqData[24] = row["frDistance"] != DBNull.Value ? row["frDistance"].ToString() : "";
                    //enqData[25] = row["frInvest"].ToString() == "1" ? "Yes" : "No";

                    int pFlag = 0;
                    if (Request.QueryString["printDoc"] != null)
                    {
                        if (Request.QueryString["printDoc"].ToString() == "1")
                        {
                            pFlag = 1;
                        }
                    }


                    if (row["OBP_AddProof1"] != DBNull.Value && row["OBP_AddProof1"].ToString() != "" && row["OBP_AddProof1"].ToString() != null && row["OBP_AddProof1"].ToString() != "no-photo.png")
                    {
                        StringBuilder strAddr = new StringBuilder();

                        //enqData[27] = "<a href=\"../upload/addrProof/" + row["addressProof"].ToString() + "\" target=\"_blank\" title=\"View Larger\" ><img src=\"../upload/addrProof/" + row["addressProof"].ToString() + "\" style=\"width:150px;\" /></a>";
                        if (row["OBP_AddProof2"] != DBNull.Value && row["OBP_AddProof2"].ToString() != "" && row["OBP_AddProof2"].ToString() != null && row["OBP_AddProof2"].ToString() != "no-photo.png")
                        {
                            strAddr.Append("<div style=\"width:50%; float:right;\">");
                            strAddr.Append("<div style=\"padding:5px;\">");
                            strAddr.Append("<a href=\"../upload/gobpData/addressProof/" + row["OBP_AddProof1"].ToString() + "\" target=\"_blank\" title=\"View Larger\" ><img src=\"../upload/gobpData/addressProof/" + row["OBP_AddProof1"].ToString() + "\" style=\"width:100%;\" /></a>");
                            strAddr.Append("</div>");
                            strAddr.Append("</div>");
                            strAddr.Append("<div style=\"width:50%; float:right;\">");
                            strAddr.Append("<div style=\"padding:5px;\">");
                            strAddr.Append("<a href=\"../upload/gobpData/addressProof/" + row["OBP_AddProof2"].ToString() + "\" target=\"_blank\" title=\"View Larger\" ><img src=\"../upload/gobpData/addressProof/" + row["OBP_AddProof2"].ToString() + "\" style=\"width:100%;\" /></a>");
                            strAddr.Append("</div>");
                            strAddr.Append("</div>");
                            strAddr.Append("<div class=\"float_clear\"></div>");
                        }
                        else
                        {
                            strAddr.Append("<a href=\"../upload/gobpData/addressProof/" + row["OBP_AddProof1"].ToString() + "\" target=\"_blank\" title=\"View Larger\" ><img src=\"../upload/gobpData/addressProof/" + row["OBP_AddProof1"].ToString() + "\" style=\"width:50%;\" /></a>");
                        }

                        if (pFlag == 1)
                        {
                            // Owner Name
                            strAddr.Append("<br />");
                            strAddr.Append("<span class=\"space20\"></span>");
                            strAddr.Append("<div style=\"width:40%; float:right;\">");
                            strAddr.Append("<div style=\"border:2px solid #000; padding-top: 17px; \">");
                            strAddr.Append("<div class=\"pad_LR_10 pad_TB_15\" style=\" padding-bottom: 0.9em; padding-left: 0.9em; \" >");
                            strAddr.Append("<span class=\"bold_weight dspBlk small mrgBtm10\">Name : " + enqData[2] + "</span>");
                            strAddr.Append("<br />");
                            strAddr.Append("<br />");
                            strAddr.Append("<span class=\"space10\"></span>");
                            strAddr.Append("<span class=\"bold_weight dspBlk small\">Signature : </span>");
                            strAddr.Append("</div>");
                            strAddr.Append("</div>");
                            strAddr.Append("</div>");
                            strAddr.Append("<div class=\"float_clear\"></div>");
                        }

                        enqData[25] = strAddr.ToString();
                    }
                    else
                    {
                        enqData[25] = "<img src=\"../upload/gobpData/addressProof/no-photo.png\" style=\"width:150px;\" />";
                    }

                    if (row["OBP_IDProof1"] != DBNull.Value && row["OBP_IDProof1"].ToString() != "" && row["OBP_IDProof1"].ToString() != null)
                    {
                        StringBuilder strId = new StringBuilder();
                        
                        if (row["OBP_IDProof2"] != DBNull.Value && row["OBP_IDProof2"].ToString() != "" && row["OBP_IDProof2"].ToString() != null)
                        {
                            strId.Append("<div style=\"width:50%; float:right;\">");
                            strId.Append("<div style=\"padding:5px;\">");
                            strId.Append("<a href=\"../upload/gobpData/idProof/" + row["OBP_IDProof1"].ToString() + "\" target=\"_blank\" title=\"View Larger\" ><img src=\"../upload/gobpData/idProof/" + row["OBP_IDProof1"].ToString() + "\" style=\"width:100%;\" /></a>");
                            strId.Append("</div>");
                            strId.Append("</div>");
                            strId.Append("<div style=\"width:50%; float:right;\">");
                            strId.Append("<div style=\"padding:5px;\">");
                            strId.Append("<a href=\"../upload/gobpData/idProof/" + row["OBP_IDProof2"].ToString() + "\" target=\"_blank\" title=\"View Larger\" ><img src=\"../upload/gobpData/idProof/" + row["OBP_IDProof2"].ToString() + "\" style=\"width:100%;\" /></a>");
                            strId.Append("</div>");
                            strId.Append("</div>");
                            strId.Append("<div class=\"float_clear\"></div>");
                        }
                        else
                        {
                            strId.Append("<a href=\"../upload/gobpData/idProof/" + row["OBP_IDProof1"].ToString() + "\" target=\"_blank\" title=\"View Larger\" ><img src=\"../upload/gobpData/idProof/" + row["OBP_IDProof1"].ToString() + "\" style=\"width:50%;\" /></a>");
                        }

                        if (pFlag == 1)
                        {
                            // Owner Name
                            strId.Append("<br />");
                            strId.Append("<span class=\"space20\"></span>");
                            strId.Append("<div style=\"width:40%; float:right;\">");
                            strId.Append("<div style=\"border:2px solid #000; padding-top: 17px; \">");
                            strId.Append("<div class=\"pad_LR_10 pad_TB_15\" style=\" padding-bottom: 0.9em; padding-left: 0.9em; \" >");
                            strId.Append("<span class=\"bold_weight dspBlk small mrgBtm10\">Name : " + enqData[2] + "</span>");
                            strId.Append("<br />");
                            strId.Append("<br />");
                            strId.Append("<span class=\"space10\"></span>");
                            strId.Append("<span class=\"bold_weight dspBlk small\">Signature : </span>");
                            strId.Append("</div>");
                            strId.Append("</div>");
                            strId.Append("</div>");
                            strId.Append("<div class=\"float_clear\"></div>");
                        }

                        enqData[26] = strId.ToString();
                    }
                    else
                    {
                        enqData[26] = "<img src=\"../upload/gobpData/idProof/no-photo.png\" style=\"width:150px;\" />";
                    }

                    enqData[20] = row["OBP_UTRNum"] != DBNull.Value ? row["OBP_UTRNum"].ToString() : "";
                    enqData[21] = row["OBP_BankName"] != DBNull.Value ? row["OBP_BankName"].ToString() : "";
                    enqData[22] = row["OBP_TransDate"] != DBNull.Value ? Convert.ToDateTime(row["OBP_TransDate"]).ToString("dd/MM/yyyy") : "";
                    enqData[23] = row["OBP_AccHolder"] != DBNull.Value ? row["OBP_AccHolder"].ToString() : "";
                    enqData[24] = row["OBP_PaidAmt"] != DBNull.Value ? row["OBP_PaidAmt"].ToString() : "";

                    

                    if (row["OBP_ProfilePic"] != DBNull.Value && row["OBP_ProfilePic"].ToString() != "" && row["OBP_ProfilePic"].ToString() != null && row["OBP_ProfilePic"].ToString() != "NA")
                    {
                        enqData[27] = "<img src=\"../upload/gobpData/profilePhoto/" + row["OBP_ProfilePic"].ToString() + "\" style=\"width:150px;\" />";
                    }
                    else
                    {
                        enqData[27] = "<img src=\"../upload/gobpData/profilePhoto/no-photo.png\" style=\"width:150px;\" />";
                    }

                    if (row["OBP_Resume"] != DBNull.Value && row["OBP_Resume"].ToString() != "" && row["OBP_Resume"].ToString() != null && row["OBP_Resume"].ToString() != "NA")
                    {
                        enqData[28] = "<a href=\"../upload/gobpData/resume/" + row["OBP_Resume"].ToString() + "\"  class=\"pdfLink\"\" />View Resume</a>";
                    }
                    else
                    {
                        enqData[28] = "-";
                    }

                    if (pFlag == 1)
                    {
                        ////////////////////////////////////////////////////////////////////////////////////////

                        photo.Visible = true;
                        StringBuilder strMarkup = new StringBuilder();
                        strMarkup.Append("<div class=\"pagebreak\"></div>");
                        strMarkup.Append("<table style=\"border: 2px solid black;padding:10px;background:#f8f8f8\">");
                        strMarkup.Append("<tr><td style=\"font-weight:bold; text-align:center; font-size:1.2em; margin-bottom:10px;\">Terms and Conditions</td></tr>");
                        strMarkup.Append("<tr><td style=\"width:30%;\">I understand and agree that the statements in this proposal form shall be the basis of the contract between me and Genericart Medicine Pvt Ltd.</td></tr>");
                        strMarkup.Append("<tr><td>I further declare that the statements in this proposal are true and I have disclosed all information which might be material to the company. I declare that I have read the OBP agreement and understood the terms and conditions associated risks and benefits which I propose to take.</td></tr>");
                        strMarkup.Append("<tr><td>I declare that the amount paid has not been generated from the proceeds of any criminal activities / offences and I shall abide by and conform to the Prevention of Money Laundering Act, 2002 or any other applicable laws.</td></tr>");
                        strMarkup.Append("<tr><td>I declare that the company has disclosed and explained all the information related to shop and I declare that I have understood the same before signing this proposal form.</td></tr>");
                        strMarkup.Append("<tr><td>I also hereby agree and authorized the Company to access my data maintained by the Unique Identification Authority of India (UIDAI) for KYC verification and other eKYC services purpose.</td></tr>");
                        strMarkup.Append("<tr><td>I herewith declare that I have understood & read all your term, condition & FAQ of agreement as well as I am aware that the OBP fee is non refundable in any case. I am willing to purchase franchise accordingly. Kindly prepare agreement per details mentioned above.</td></tr>");
                        strMarkup.Append("<tr><td>I herewith declare that I do not have any criminal background as well as there were no civil judicial cases pending/running against me.</td></tr>");
                        strMarkup.Append("<tr><td style=\"font-weight:bold\">I have understood all above points which are already present in agreement.</td></tr>");
                        strMarkup.Append("<tr><td>Terms & Conditions accepted will be considered as “Signature”.</td></tr>");
                        //strMarkup.Append("<tr><td style=\" text-align:right;\"><img src=\"http://www.genericartmedicine.com/images/signature.png\" style=\"width:200px;\" /></td></tr>");
                        strMarkup.Append("</table>");



                        // Owner Name
                        strMarkup.Append("<br />");
                        strMarkup.Append("<span class=\"space20\"></span>");
                        strMarkup.Append("<div style=\"width:40%; float:right;\">");
                        strMarkup.Append("<div style=\"border:2px solid #000; padding-top: 17px; \">");
                        strMarkup.Append("<div class=\"pad_LR_10 pad_TB_15\" style=\" padding-bottom: 0.9em; padding-left: 0.9em; \" >");
                        strMarkup.Append("<span class=\"bold_weight dspBlk small mrgBtm10\">Name : " + enqData[2] + "</span>");
                        strMarkup.Append("<br />");
                        strMarkup.Append("<br />");
                        strMarkup.Append("<span class=\"space10\"></span>");
                        strMarkup.Append("<span class=\"bold_weight dspBlk small\">Signature : </span>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                       

                        enqData[35] = strMarkup.ToString();
                    }


                    //btnPrint.Visible = pFlag == 1 ? false : true;
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
        if (Request.QueryString["type"] != null)
        {
            Response.Redirect("gobp-registration-master.aspx?type=" + Request.QueryString["type"] + "");
        }
        else
        {
            Response.Redirect("gobp-registration-master.aspx");
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Response.Redirect("gobp-registration-master.aspx?id=" + Request.QueryString["id"] + "&printDoc=1", false);
    }
}