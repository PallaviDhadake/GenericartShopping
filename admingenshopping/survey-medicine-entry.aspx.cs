using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

public partial class admingenshopping_survey_medicine_entry : System.Web.UI.Page
{
    iClass c = new iClass();
    public string errMsg, csvUploadDate, errMsg2;
    public int rowNo = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSubmit.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(btnSubmit, null) + ";");

        if (!IsPostBack)
        {
            DateTime lastModified = System.IO.File.GetLastWriteTime(Server.MapPath("~/upload/survey-medicine.csv"));
            csvUploadDate = lastModified.ToString("dd/MM/yyyy hh:mm tt");
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (!fuFile.HasFile)
            {
                errMsg = c.ErrNotification(2, "Select File to Fetch Data");
                

                //double a = 20.60;

                //errMsg = Math.Round(a, 2).ToString();
                return;
            }
            else
            {
                string fExt = Path.GetExtension(fuFile.FileName).ToLower();
                if (fExt != ".csv")
                {
                    errMsg = c.ErrNotification(2, "Invalid file extension. Only .csv files are allowed.");
                    return;
                }
                else
                {
                    //string path = Path.GetFileName(Server.MapPath(fuFile.FileName));
                    string rootPath = c.ReturnHttp();

                    string filename = "survey-medicine";
                    string filePath = "~/upload/";
                    string path = Server.MapPath((filePath) + filename + fExt);

                    fuFile.SaveAs(Server.MapPath(filePath) + filename + fExt);

                    string csvPath = Server.MapPath("~/upload/survey-medicine.csv");
                    string csvData = File.ReadAllText(csvPath);

                    string rowNumbers = "";
                    int successRows = 0;
                    string emptyRows = "";
                    int emptyCount = 0;
                    string sRows = "";
                    string duplicateRows = "";
                    int dRows = 0;

                    //if (c.IsRecordExist("Select MedicineRowID From SurveyMedicines"))
                    //{
                    //    c.ExecuteQuery("Delete From SurveyMedicines");
                    //}
                    int scannedRows = 0;
                    foreach (string row in csvData.Split('\n'))
                    {
                        scannedRows++;
                        if (!string.IsNullOrEmpty(row))
                        {
                            rowNo++;
                            string[] arrCells = row.Split(',');
                            if (arrCells.Length == 7)
                            {
                                string brandName = arrCells[1].ToString() != "" ? arrCells[1].ToString() : "";
                                if (brandName != "")
                                {
                                    if (!c.IsRecordExist("Select MedicineRowID From SurveyMedicines Where BrandName='" + brandName + "'"))
                                    {
                                        int maxId = c.NextId("SurveyMedicines", "MedicineRowID");

                                        string contentName = arrCells[0].ToString() != "" ? arrCells[0].ToString() : "NA";
                                        //string brandName = arrCells[1].ToString() != "" ? arrCells[1].ToString() : "NA";
                                        string companyName = arrCells[3].ToString() != "" ? arrCells[3].ToString() : "NA";
                                        string packaging = arrCells[2].ToString() != "" ? arrCells[2].ToString() : "NA";
                                        string genericcode = arrCells[5].ToString() != "" ? arrCells[5].ToString() : "NA";
                                        double brandPrice = arrCells[4].ToString() != "" ? Convert.ToDouble(Regex.Replace(arrCells[4].ToString(), "[^-?0-9\\.]+", " ")) : 0;
                                        double genPrice = arrCells[6].ToString() != "" ? Convert.ToDouble(Regex.Replace(arrCells[6].ToString(), "[^-?0-9\\.]+", " ")) : 0;

                                        double finalBrandPrice = Math.Round(brandPrice, 2);
                                        double finalGenPrice = Math.Round(genPrice, 2);

                                        if (brandPrice != 0 && genPrice != 0)
                                        {
                                            //c.ExecuteQuery("Insert Into SurveyMedicines (MedicineRowID, ContentName, BrandName, CompanyName, Packaging, " +
                                            //    " PriceBrand, GenericCode, PriceGeneric) Values (" + maxId + ", '" + contentName +
                                            //    "', '" + brandName + "', '" + companyName + "', '" + packaging +
                                            //    "', " + Convert.ToDouble(brandPrice.ToString("0.00")) + ", '" + genericcode +
                                            //    "', " + Convert.ToDouble(genPrice.ToString("0.00")) + ")");

                                            c.ExecuteQuery("Insert Into SurveyMedicines (MedicineRowID, ContentName, BrandName, CompanyName, Packaging, " +
                                                " PriceBrand, GenericCode, PriceGeneric) Values (" + maxId + ", '" + contentName +
                                                "', '" + brandName + "', '" + companyName + "', '" + packaging +
                                                "', " + finalBrandPrice + ", '" + genericcode +
                                                "', " + finalGenPrice + ")");

                                            successRows++;

                                            if (sRows == "")
                                            {
                                                sRows = rowNo.ToString();
                                            }
                                            else
                                            {
                                                sRows = sRows + ", " + rowNo.ToString();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dRows++;
                                        if (duplicateRows == "")
                                        {
                                            duplicateRows = rowNo.ToString();
                                        }
                                        else
                                        {
                                            duplicateRows = duplicateRows + ", " + rowNo.ToString();
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

                    errMsg = c.ErrNotification(1, "Data Fetched Successfully");
                    errMsg2 = "Total Rows : " + scannedRows.ToString() + "<br/>Successfully Inserted Rows :" + successRows + "<br/>Defected Rows : " + rowNumbers.ToString() + " <br/> Empty Rows : " + emptyCount.ToString() + "<br/> Duplicate Rows : " + dRows.ToString();
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "waitAndMove('survey-medicine-entry.aspx', 2000);", true);
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, "Error at Row no : " + rowNo + ", " + ex.Message.ToString());
            return;
        }
    }
}