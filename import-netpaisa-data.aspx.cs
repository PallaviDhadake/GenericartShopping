using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;

public partial class import_netpaisa_data : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg2;
    public int rowNo = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
       
       
    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        try
        {

            int invalidMobile = 0;
            string rootPath = c.ReturnHttp();
           
            string csvPath = Server.MapPath("~/upload/Netpaisa-GOBP-Data1.csv");
            string csvData = File.ReadAllText(csvPath);

            string rowNumbers = "";
            int successRows = 0;
            string emptyRows = "";
            int emptyCount = 0;
            string sRows = "";
            int scannedRows = 0;


            foreach (string row in csvData.Split('\n'))
            {
                scannedRows++;
                if (!String.IsNullOrEmpty(row))
                {
                    rowNo++;
                    string[] arrCells = row.Split(',');
                    if (arrCells.Length == 5 || arrCells.Length == 6)
                    {
                        if (arrCells[1].ToString() != "")
                        {
                            if (arrCells[0].ToString() == "User")
                            {

                            }
                            else
                            {

                                string mobile = arrCells[2].ToString() != "" ? arrCells[2].ToString().Trim().Replace("'", "") : "NA";


                                if (c.ValidateMobile(mobile) == true)
                                {

                                    int MaxId = c.NextId("OBPData", "OBP_ID");

                                    string name = arrCells[0].ToString() != "" ? arrCells[0].ToString().Trim().Replace("'", "").ToUpper() : "NA";

                                    string email = arrCells[1].ToString() != "" ? arrCells[1].ToString().Trim().Replace("'", "") : "NA";

                                  
                                    string state = arrCells[3].ToString() != "" ? arrCells[3].ToString().Trim().Replace("'", "") : "NA";

                                   

                                    string obpUserId = "OBP" + MaxId.ToString("D5");

                                    string stateId = GetStateId(state);

                                    //if (stateId.Length > 2)
                                    //{
                                    //    lblStateName.Text = lblStateName.Text + ", " + stateId;
                                    //    stateId = "1";
                                    //}
                                   

                                    c.ExecuteQuery("Insert into OBPData(OBP_ID, OBP_JoinDate, OBP_FKTypeID, OBP_ApplicantName, OBP_EmailId, OBP_MobileNo, OBP_UserID , OBP_UserPWD , OBP_StatusFlag , OBP_DelMark , OBP_DH_UserId , OBP_DH_Name , OBP_StateID, OBP_City , OBP_InsertType , IsMLM , OBP_JoinLevel)Values(" + MaxId + ", '" + DateTime.Now + "', 1, '" + name + "', '" + email + "', '" + mobile + "', '" + obpUserId + "', '123456' , 'Active', 0, 'OBPDH005', 'NETPAISA', " + stateId + ", '" + state + "', 'NetPaisa', 0, 1)");

                                    successRows++;

                                    if (sRows == "")
                                        sRows = rowNo.ToString();
                                    else
                                        sRows = sRows + ", " + rowNo.ToString();

                                }
                                else
                                {
                                    invalidMobile = invalidMobile + 1;
                                    Label1.Text = invalidMobile.ToString();
                                }
                            }

                        }
                    }
                    else
                    {
                        if (rowNumbers == "")
                        {
                            rowNumbers = rowNo.ToString();
                        }
                        else
                        {
                            rowNumbers = rowNumbers + ", " + rowNo.ToString();
                        }
                    }
                }
                else
                {
                    emptyCount++;
                    if (emptyRows == "")
                    {
                        emptyRows = scannedRows.ToString();
                    }
                    else
                    {
                        emptyRows = emptyRows + ", " + scannedRows.ToString();
                    }
                }
            }


            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Data Fetched Successfully');", true);

            errMsg2 = "Data inserted successfully";

            Response.Write("Data inserted successfully");


        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "btnSubmit_Click", ex.Message.ToString());
            return;
        }
    }


    public string GetStateId(string stateName)
    {
        string stateId = "";

        if (stateName == "Kolkata")
        {
            return "28";
        }

        using (DataTable dtStates = c.GetDataTable("Select * From StatesData Where StateName='"+ stateName + "'"))
        {
            if (dtStates.Rows.Count > 0)
            {
                stateId = dtStates.Rows[0]["StateID"].ToString();
            }
            else
            {
                return stateName;
            }

        }

        return stateId;
    }
}