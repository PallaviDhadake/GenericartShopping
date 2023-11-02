using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class admingenshopping_upload_products : System.Web.UI.Page
{
    public string errMsg;
    MasterClass m = new MasterClass();
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        BtnSubmit.Attributes.Add("onclick", " this.disabled = true; this.value='Processing...'; " + ClientScript.GetPostBackEventReference(BtnSubmit, null) + ";");
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (!fuFile.HasFile)
            {
                errMsg = c.ErrNotification(2, "Select File");
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
                    string rootPath = m.ReturnHttp();

                    string filename = "product-list";
                    string filePath = "~/upload/";
                    string path = Server.MapPath((filePath) + filename + fExt);

                    fuFile.SaveAs(Server.MapPath(filePath) + filename + fExt);

                    string csvPath = Server.MapPath("~/upload/product-list.csv");
                    string csvData = File.ReadAllText(csvPath);

                    foreach (string row in csvData.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            string[] arrCells = row.Split(',');

                            if (arrCells.Length == 11)
                            {
                                string prodName = arrCells[2].ToString() != "" ? arrCells[2].ToString() : "";
                                string gmplCode = arrCells[3].ToString().Trim() != "" ? arrCells[3].ToString().Trim() : "";
                                string prodSubCat = arrCells[4].ToString().Trim() != "" ? arrCells[4].ToString().Trim() : "";
                                string brand = arrCells[5].ToString().Trim() != "" ? arrCells[5].ToString().Trim() : "";
                                string disease = arrCells[6].ToString().Trim() != "" ? arrCells[6].ToString().Trim() : "";
                                string unit = arrCells[7].ToString().Trim() != "" ? arrCells[7].ToString().Trim() : "";
                                string packaging = arrCells[8].ToString().Trim() != "" ? arrCells[8].ToString().Trim() : "";
                                string mrp = arrCells[9].ToString().Trim() != "" ? arrCells[9].ToString().Trim() : "1";
                                string saleRate = arrCells[10].ToString().Trim() != "" ? arrCells[10].ToString().Trim() : "1";

                                //int itemGroupId = 1;

                                int unitId = 0;
                                if (unit != "")
                                {
                                    if (c.IsRecordExist("Select UnitID From UnitProducts Where UnitName='" + unit + "' AND delMark=0"))
                                    {
                                        unitId = Convert.ToInt32(c.GetReqData("UnitProducts", "UnitID", "UnitName='" + unit + "' AND delMark=0"));
                                    }
                                    else
                                    {
                                        int unitMaxId = c.NextId("UnitProducts", "UnitID");
                                        c.ExecuteQuery("Insert Into UnitProducts (UnitID, UnitName, delMark) Values (" + unitMaxId + ", '" + unit + "', 0)");
                                    }
                                }

                                int brandId = 0;
                                if (brand != "")
                                {
                                    if (c.IsRecordExist("Select MfgId From Manufacturers Where MfgName='" + brand + "' AND delMark=0"))
                                    {
                                        brandId = Convert.ToInt32(c.GetReqData("Manufacturers", "MfgId", "MfgName='" + brand + "' AND delMark=0"));
                                    }
                                    else
                                    {
                                        int brandMaxID = c.NextId("Manufacturers", "MfgId");
                                        c.ExecuteQuery("Insert Into Manufacturers (MfgId, MfgName, delMark) Values (" + brandMaxID + ", '" + brand + "', 0)");
                                    }
                                }

                                if (prodName != "")
                                {
                                    int maxID = c.NextId("ProductsData", "ProductID");
                                    c.ExecuteQuery("Insert Into ProductsData (ProductID, FK_MfgID, FK_UnitID, ProductSKU, ProductName, PriceMRP, " +
                                        " PriceSale, PackagingType, ProductLongDesc, ProductShortDesc, ProductPhoto, FK_CategoryID, " +
                                        " FK_SubCategoryID, ProductStock, ProductActive, PrescriptionFlag, ProductViews, ProductMetaDesc, " +
                                        " FeaturedFlag, BestSellerFlag, delmark) Values (" + maxID + ", " + brandId + ", " + unitId + ", '" + gmplCode +
                                        "', '" + prodName + "', " + Convert.ToDouble(mrp) + ", " + Convert.ToDouble(saleRate) + ", '" + packaging +
                                        "', 'NA', 'NA', 'no-photo.png', 2, 7, 10, 1, 0, 0, 'NA', 0, 0, 0)");

                                    //MasterClass.NonQuery("Insert Into Item (Name, ItemGroupAuto, UnitAuto, BrandAuto, PurchaseRate, SaleRate, " +
                                    //    " Description, Status, Validity, BusinessValue, BarCode, IGSTPercent, CGSTPercent, SGSTPercent, " +
                                    //    " QuantityPerItem, MRP, DiseaseAuto, ContentAuto, ShortDescription, SubUnit) Values ('" + prodName +
                                    //    "', " + itemGroupId + ", " + unitId + ", " + brandId + ", 0, " + Convert.ToDouble(saleRate) +
                                    //    ", 'NA', 1, 0, 0, '" + gmplCode + "', 0, 0, 0, 1, " + Convert.ToDouble(mrp) + ", " + diseaseId +
                                    //    ", 0, 'NA', '" + packaging + "')");
                                }
                            }
                        }
                    }

                    errMsg = c.ErrNotification(1, "Data Added Successfully..!!");
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