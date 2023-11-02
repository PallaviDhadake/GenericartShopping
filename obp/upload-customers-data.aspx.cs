using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class obp_upload_customers_data : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg, errMsg2;
    public int rowNo = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        btnUploadCust.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnUploadCust, null) + ";");
    }

    protected void btnUploadCust_Click(object sender, EventArgs e)
    {
        try
        {
            if (!fuFileCust.HasFile)
            {
                errMsg = c.ErrNotification(2, "Select File to Fetch Data");
                return;
            }
            else
            {
                string fExt = Path.GetExtension(fuFileCust.FileName).ToLower();
                if (fExt != ".csv")
                {
                    errMsg = c.ErrNotification(2, "Invalid file extension. Only .csv files are allowed.");
                    return;
                }
                else
                {
                    string rootPath = c.ReturnHttp();

                    string filename = "cust-data";
                    string filePath = "~/upload/";
                    string path = Server.MapPath((filePath) + filename + fExt);

                    fuFileCust.SaveAs(Server.MapPath(filePath) + filename + fExt);

                    string csvPath = Server.MapPath("~/upload/cust-data.csv");
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
                        if (!string.IsNullOrEmpty(row))
                        {
                            rowNo++;
                            string[] arrCells = row.Split(',');
                            if (arrCells.Length == 2)
                            {
                                if (arrCells[0].ToString() != "")
                                {
                                    int maxId = c.NextId("CustomersData", "CustomrtID");

                                    string custName = arrCells[0].ToString().Trim().Replace("'", "");
                                    string custMob = arrCells[1].ToString().Trim().Replace("'", "");

                                    c.ExecuteQuery("Insert Into CustomersData (CustomrtID, CustomerJoinDate, CustomerName, CustomerMobile, " +
                                            " CustomerPassword, MobileVerify, EmailVerify, CustomerActive, delMark, CustomerFavShop, DeviceType, " +
                                            " FK_GenMitraID) Values (" + maxId + ", '" + DateTime.Now + "', '" + custName +
                                            "', '" + custMob + "', '123456', 1, 1, 1, 0, 24, 'G-Web', 48)");

                                    int maxAddrId = c.NextId("CustomersAddress", "AddressID");
                                    c.ExecuteQuery("Insert Into CustomersAddress (AddressID, AddressFKCustomerID, AddressCity, " +
                                           " AddressState, AddressCountry, AddressStatus) Values (" + maxAddrId + ", " + maxId + 
                                           ", 'Pune', 'Maharashtra', 'India', 1)");

                                    successRows++;

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

                    errMsg = c.ErrNotification(1, "Data Fetched Successfully");
                    errMsg2 = "Total Rows : " + scannedRows.ToString() + "<br/>Successfully Inserted Rows :" + successRows + 
                        "<br/>Defected Rows : " + rowNumbers.ToString() + " <br/> Empty Rows : " + emptyCount.ToString();
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