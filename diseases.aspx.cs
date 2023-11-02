using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class diseases : System.Web.UI.Page
{
    iClass c = new iClass();
    public string diseaseProdStr, paginationHtml;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Page.RouteData.Values["disId"].ToString()))
                {
                    string[] arrCat = Page.RouteData.Values["disId"].ToString().Split('-');
                    GetDiseaseProducts(Convert.ToInt32(arrCat[arrCat.Length - 1]));
                }
                else
                {
                    Response.Redirect(Master.rootPath, false);
                }
            }
        }
        catch (Exception ex)
        {
            diseaseProdStr = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    private void GetDiseaseProducts(int diseaseIdX)
    {
        try
        {
            int frmRec, toRec, pageNo;

            if (String.IsNullOrEmpty(Page.RouteData.Values["pgId"].ToString()))
            {
                pageNo = 1;
            }
            else
            {
                pageNo = Convert.ToInt32(Page.RouteData.Values["pgId"].ToString());
            }

            int catProdDisplay = 12;

            frmRec = (pageNo - 1) * catProdDisplay + 1;
            toRec = pageNo * catProdDisplay;
            string diseaseName = c.GetReqData("DiseaseData", "DiseaseName", "DiseaseId=" + diseaseIdX).ToString();
            this.Title = "Generic Medicines for " + diseaseName + " | Genericart Online Generic Medicine Shopping";

            string disProdIds = "";
            // Get Products of diseases
            using (DataTable dtDisprod = c.GetDataTable("Select FK_ProductID From DiseaseProducts Where FK_DiseaseID=" + diseaseIdX))
            {
                if (dtDisprod.Rows.Count > 0)
                {
                    foreach (DataRow disRow in dtDisprod.Rows)
                    {
                        if (disProdIds == "")
                            disProdIds = disRow["FK_ProductID"].ToString();
                        else
                            disProdIds = disProdIds + ", " + disRow["FK_ProductID"].ToString();
                    }
                }
            }

            if (disProdIds != "")
            {
                string strQuery = "Select * From (Select *, ROW_NUMBER() Over(Order By ProductName ASC) AS NUMBER From (Select a.ProductID, a.ProductName, a.ProductShortDesc, a.ProductPhoto, a.FK_SubCategoryID, a.PrescriptionFlag, a.FK_MfgID, a.FK_UnitID, a.PackagingType, a.PriceMRP, a.PriceSale, CONVERT(DECIMAL(20, 2), (100 - (a.PriceSale * 100) / a.PriceMRP)) AS DiscountPercent From ProductsData a Where a.ProductID IN (" + disProdIds + ") AND a.delMark=0 AND a.ProductActive=1) a ) AS TBL Where NUMBER Between (" + frmRec + ") AND (" + toRec + ")";

                using (DataTable dtCatProd = c.GetDataTable(strQuery))
                {
                    DataTable dtTotalCatProd = c.GetDataTable("Select ProductID, ProductName From ProductsData Where ProductID IN (" + disProdIds + ") AND delMark=0 AND ProductActive=1 Order By ProductName ASC");
                    int totalCatProd = dtTotalCatProd.Rows.Count;
                    int pageCount = totalCatProd % catProdDisplay > 0 ? (totalCatProd / catProdDisplay) + 1 : (totalCatProd / catProdDisplay);
                    if (pageCount >= 1)
                    {
                        paginationHtml = paginationMaker(pageCount, pageNo);
                    }
                    StringBuilder strMarkup = new StringBuilder();

                    strMarkup.Append("<h1 class=\"pageH2 clrLightBlack semiBold mrg_B_10\">" + diseaseName + " Products</h1>");
                    if (dtCatProd.Rows.Count > 0)
                    {

                        int boxCount = 0;

                        foreach (DataRow row in dtCatProd.Rows)
                        {
                            strMarkup.Append("<div class=\"col_1_4\">");
                            strMarkup.Append("<div class=\"pad_10\">");
                            strMarkup.Append("<div class=\"prodContainer posRelative\" title=\"" + row["ProductName"].ToString() + "\">");
                            strMarkup.Append("<div class=\"pad_15\">");
                            if (row["PrescriptionFlag"] != DBNull.Value && row["PrescriptionFlag"] != null && row["PrescriptionFlag"].ToString() != "")
                            {
                                if (row["PrescriptionFlag"].ToString() == "1")
                                {
                                    strMarkup.Append("<div class=\"medPrescription\" title=\"Prescription Required\">Rx</div>");
                                }
                            }
                            strMarkup.Append("<div class=\"txtCenter\">");
                            if (row["ProductPhoto"] != DBNull.Value && row["ProductPhoto"] != null && row["ProductPhoto"].ToString() != "")
                            {
                                strMarkup.Append("<div class=\"proFixSize\">");
                                strMarkup.Append("<img src=\"" + Master.rootPath + "upload/products/thumb/" + row["ProductPhoto"].ToString() + "\" alt=\"" + row["ProductName"].ToString() + "\" class=\"width100\" />");
                                strMarkup.Append("</div>");
                            }
                            else
                            {
                                strMarkup.Append("<div class=\"proFixSize\">");
                                strMarkup.Append("<img src=\"" + Master.rootPath + "upload/products/thumb/no-photo.png\" alt=\"" + row["ProductName"].ToString() + "\" class=\"width100\" />");
                                strMarkup.Append("</div>");
                            }
                            strMarkup.Append("</div>");
                            strMarkup.Append("<span class=\"prodLine\"></span>");
                            strMarkup.Append("<span class=\"space15\"></span>");
                            string subCatName = c.GetReqData("ProductCategory", "ProductCatName", "ProductCatID=" + row["FK_SubCategoryID"].ToString()).ToString();
                            string prodUrl = Master.rootPath + "products/" + c.UrlGenerator(subCatName) + "/" + c.UrlGenerator(row["ProductName"].ToString().ToLower()) + "-" + row["ProductID"].ToString();
                            string pName = row["ProductName"].ToString().Length > 25 ? row["ProductName"].ToString().Substring(0, 25) + "..." : row["ProductName"].ToString();
                            strMarkup.Append("<a href=\"" + prodUrl + "\" class=\"prodName semiBold\">" + pName + "</a>");
                            strMarkup.Append("<span class=\"space10\"></span>");
                            string prodUnit = c.GetReqData("UnitProducts", "UnitName", "UnitID=" + row["FK_UnitID"]).ToString();
                            string prodManufacturer = c.GetReqData("Manufacturers", "MfgName", "MfgId=" + row["FK_MfgID"]).ToString();
                            strMarkup.Append("<span class=\"small clrGrey line-ht-5 dspBlk semiBold\">" + prodUnit + " Of " + row["PackagingType"].ToString() + "</span>");
                            strMarkup.Append("<span class=\"space10\"></span>");
                            string shortDesc = row["ProductShortDesc"].ToString().Length > 35 ? row["ProductShortDesc"].ToString().Substring(0, 35) + "..." : row["ProductShortDesc"].ToString();
                            strMarkup.Append("<p class=\"clrGrey line-ht-5 tiny mrg_B_15\">" + shortDesc + "</p>");
                            strMarkup.Append("<span class=\"prod-offer-price\">&#8377; " + row["PriceSale"].ToString() + "</span>");
                            strMarkup.Append("<span class=\"prod-price\">&#8377; " + row["PriceMRP"].ToString() + "</span>");
                            strMarkup.Append("<span class=\"prod-discount\">" + row["DiscountPercent"].ToString() + "%</span>");
                            strMarkup.Append("</div>");
                            strMarkup.Append("</div>");
                            strMarkup.Append("</div>");
                            strMarkup.Append("</div>");

                            boxCount++;

                            if ((boxCount % 4) == 0)
                            {
                                strMarkup.Append("<div class=\"float_clear\"></div>");
                            }
                        }
                        strMarkup.Append("<div class=\"float_clear\"></div>");


                    }
                    else
                    {
                        strMarkup.Append("<div class=\"themeBgPrime\"><div class=\"pad_10\"><span class=\"clrWhite fontRegular\">No any products to display</span></div></div>");
                    }

                    diseaseProdStr = strMarkup.ToString();
                }
            }
            else
            {
                diseaseProdStr = "<div class=\"themeBgPrime\"><div class=\"pad_10\"><span class=\"clrWhite fontRegular\">No any products to display</span></div></div>";
                return;
            }
        }
        catch (Exception ex)
        {
            diseaseProdStr = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    public string paginationMaker(int totalPages, int pageReq)
    {
        string rValue = "";

        if (Convert.ToInt32(Page.RouteData.Values["pgId"].ToString()) > totalPages)
        {
            Response.Redirect(Master.rootPath + "diseases/" + Page.RouteData.Values["disId"].ToString() + "/1", false);
        }

        string pageinationUrl = "", actClass = "";
        for (int i = 1; i <= totalPages; i++)
        {
            if (String.IsNullOrEmpty(Page.RouteData.Values["disId"].ToString()))
            {
                //pageinationUrl = Master.rootPath + "diseases/" + i.ToString();
            }
            else
            {
                pageinationUrl = Master.rootPath + "diseases/" + Page.RouteData.Values["disId"].ToString() + "/" + i.ToString();
            }

            if (!String.IsNullOrEmpty(Page.RouteData.Values["pgId"].ToString()))
            {
                if (Convert.ToInt32(Page.RouteData.Values["pgId"].ToString()) == i)
                {
                    actClass = "act";
                }
                else
                {
                    actClass = "";
                }
            }
            else
            {
                if (i == 1)
                {
                    actClass = "act";
                }
                else
                {
                    actClass = "";
                }
            }
            if (rValue == "")
            {
                rValue = "<li><a class=\"" + actClass + "\" href=\"" + pageinationUrl + "\">" + i.ToString() + "</a></li>";
            }
            else
            {
                rValue = rValue + "<li><a class=\"" + actClass + "\" href=\"" + pageinationUrl + "\">" + i.ToString() + "</a></li>";
            }
        }
        rValue = "<span class=\"space30\"></span><ul class=\"bootPag\">" + rValue + "<li class=\"clear\"></li></ul>";
        return rValue;
    }
}