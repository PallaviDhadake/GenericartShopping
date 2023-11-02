using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

public partial class obp_add_customer_excel : System.Web.UI.Page
{
    iClass c = new iClass();
    public int rowNo = 0;
    public string errMsg1, errMsg2;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnUploadMedProd_Click(object sender, EventArgs e)
    {
        try
        {
            if (!fuObpList.HasFile)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'CSV File section is mandatory.');", true);
                return;
            }
            string fExt = Path.GetExtension(fuObpList.FileName).ToLower();
            if (fExt != ".csv")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Invalid file extension. Only .csv files are allowed.');", true);
                return;
            }

            string rootPath = c.ReturnHttp();
            string formattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //string filename = "cust-list-" + Session["adminObp"].ToString() + "-" + formattedDate;
            string filename = "cust-list-" + Session["adminObp"].ToString();
            filename = filename.Replace(" ", "-");
            
            string filePath = "~/upload/customer-csv/";
            string srvMapPath = filePath + filename + fExt;
            string path = Server.MapPath(srvMapPath);
            fuObpList.SaveAs(Server.MapPath(filePath) + filename + fExt);


            // Now connect to the file and read it
            string csvPath = Server.MapPath("~/upload/customer-csv/" + filename + fExt);
            string csvData = File.ReadAllText(csvPath);

            string rowNumbers = "";
            int successRows = 0;
            string emptyRows = "";
            int emptyCount = 0;
            string sRows = "";
            int scannedRows = 0;
            int duplicateRec = 0;

            foreach (string row in csvData.Split('\n'))
            {
                scannedRows++;
                if (!string.IsNullOrEmpty(row))
                {
                    rowNo++;
                    string[] arrCells = row.Split(',');
                    if (arrCells.Length == 2)
                    {
                        if (arrCells[1].ToString() != "")
                        {
                                // Check Mobile Number duplication
                            if(c.IsRecordExist("Select CustomrtID From CustomersData where CustomerMobile='" + arrCells[1].ToString().Trim() + "'") == false)
                            {
                                int maxId = c.NextId("CustomersData", "CustomrtID");
                                string custName = arrCells[0].ToString() != "" ? arrCells[0].ToString().Trim().Replace("'", "") : "NA";
                                string custMobile = arrCells[1].ToString() != "" ? arrCells[1].ToString().Trim().Replace("'", "") : "NA";
                                c.ExecuteQuery("Insert Into CustomersData(CustomrtID, CustomerJoinDate, CustomerName, CustomerMobile, MobileVerify, EmailVerify, CustomerActive, delMark, DeviceType, FK_ObpID) Values (" + maxId + ", '" + DateTime.Now + "', '" + custName + "', '" + custMobile + "', 1, 1, 1, 0, 'Corp Excel', " + Convert.ToInt32(Session["adminObp"]) + ")");
                                successRows++;
                            }
                            else
                            {
                                duplicateRec++;
                            }
                            
                            if (sRows == "")
                                sRows = rowNo.ToString();
                            else
                            sRows = sRows + ", " + rowNo.ToString();
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

            errMsg2 = "Total Rows : " + scannedRows.ToString() +
                       "<br/>Successfully Inserted Rows :" + successRows +
                       "<br/>Defected Rows : " + rowNumbers.ToString() +
                       "<br/> Empty Rows : " + emptyCount.ToString() +
                       "<br/> Duplcate record attempts : " + duplicateRec.ToString();

            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Customers data uploaded successfully.');", true);
        }
        catch(Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', '" + ex.Message.ToString() + "');", true);
            return;
        }
    }
}