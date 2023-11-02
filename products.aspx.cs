using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Web.Services;

public partial class products : System.Web.UI.Page
{
    iClass c = new iClass();
    public string[] arrProdInfo = new string[10]; //6
    public string errMsg, prodAvailable;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (String.IsNullOrEmpty(Page.RouteData.Values["prodId"].ToString()))
                {
                    if (String.IsNullOrEmpty(Page.RouteData.Values["prCatId"].ToString()))
                    {
                        Response.Redirect(Master.rootPath, false);
                    }
                    else
                    {
                        Response.Redirect(Master.rootPath, false);
                    }
                }
                else
                {
                    string[] arrProdId = Page.RouteData.Values["prodId"].ToString().Split('-');
                    GetProductInfo(Convert.ToInt32(arrProdId[arrProdId.Length-1]));
                    FillQuantity();

                    //check option available for product
                    if (c.IsRecordExist("Select ProdOptionID From ProductOptions Where FK_ProductID=" + Convert.ToInt32(arrProdId[arrProdId.Length - 1]) + " AND DelMark=0 AND IsActive=1"))
                    {
                        prodOption.Visible = true;
                        using (DataTable dtProdOptions = c.GetDataTable("Select ProdOptionID, FK_ProductID, FK_OptionGroupID, FK_OptionID From ProductOptions Where FK_ProductID=" + Convert.ToInt32(arrProdId[arrProdId.Length - 1]) + " AND DelMark=0 AND IsActive=1"))
                        {
                            string options = "", optionGroupId = "";
                            foreach (DataRow optionRow in dtProdOptions.Rows)
                            {
                                if (options == "")
                                    options = optionRow["FK_OptionID"].ToString();
                                else
                                    options = options + "," + optionRow["FK_OptionID"].ToString();

                                optionGroupId = optionRow["FK_OptionGroupID"].ToString();
                            }

                            arrProdInfo[5] = c.GetReqData("OptionGroups", "OptionGroupName", "OptionGroupID=" + optionGroupId).ToString();
                            //c.FillComboBox("OptionName", "OptionID", "OptionsData", "FK_OptionGroupID=" + optionGroupId + " AND OptionID IN (" + options + ")", "OptionDisplayOrder", 0, ddrOption);
                            GetProductOptions(options, optionGroupId, "", arrProdId[arrProdId.Length - 1]);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    private void GetProductOptions(string options, string optGrpId, string optId, string prodId)
    {
        try
        {
            using (DataTable dtOpt = c.GetDataTable("Select OptionID, OptionName From OptionsData Where FK_OptionGroupID=" + optGrpId + " AND OptionID IN (" + options + ") Order By OptionDisplayOrder"))
            {
                if (dtOpt.Rows.Count > 0)
                {
                    StringBuilder strMarkup = new StringBuilder();
                    strMarkup.Append("<div id=\"tree\">");
                    strMarkup.Append("<ul class=\"prodOption\">");
                    string classname = "";
                    foreach (DataRow row in dtOpt.Rows)
                    {
                        if (optId != "")
                        {
                            if (row["OptionID"].ToString() == optId)
                                classname = "act";
                        }
                        strMarkup.Append("<li><a href=\"javascript:void(0);\" id=\"" + row["OptionID"].ToString() + "\" class=\"" + classname + "\" onClick=\"GetOption(" + row["OptionID"].ToString() + ", " + prodId + ");\">" + row["OptionName"].ToString() + "</a></li>");
                    }
                    strMarkup.Append("</ul>");
                    strMarkup.Append("</div>");

                    arrProdInfo[4] = strMarkup.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    private void GetProductInfo(int prodIdX)
    {
        try
        {
            c.ExecuteQuery("Update ProductsData Set ProductViews=ProductViews+1 Where ProductID=" + prodIdX);

            using (DataTable dtProd = c.GetDataTable("Select ProductID, FK_MfgID, FK_UnitID, ProductSKU, ProductName, PriceMRP, PriceSale, FK_SubCategoryID, " + 
                " CONVERT(DECIMAL(20, 2), (100 - (PriceSale * 100) / PriceMRP)) AS DiscountPercent, PackagingType, ProductLongDesc, " +
                " ProductShortDesc, ProductPhoto, ProductStock, PrescriptionFlag, ProductMetaDesc, IsNotForOnlineSale From ProductsData Where ProductID=" + prodIdX))
            {
                if (dtProd.Rows.Count > 0)
                {
                    StringBuilder strMarkup = new StringBuilder();
                    DataRow row = dtProd.Rows[0];

                    // prodImg
                    if (c.IsRecordExist("Select ProductPhotoID From ProductPhotos Where FK_ProductID=" + prodIdX))
                    {
                        StringBuilder strImg = new StringBuilder();
                        strImg.Append("<div class=\"clearfix pContainer\">");
                        strImg.Append("<ul id=\"image-gallery\" class=\"gallery list-unstyled cS-hidden\">");
                        strImg.Append("<li data-thumb=\"" + Master.rootPath + "upload/products/thumb/" + row["ProductPhoto"].ToString() + "\">");
                        strImg.Append("<img src=\"" + Master.rootPath + "upload/products/" + row["ProductPhoto"].ToString() + "\" alt=\"\" class=\"width100\" />");
                        strImg.Append("</li>");
                        using (DataTable dtProdPhotos = c.GetDataTable("Select ProductPhotoID, PhotoName From ProductPhotos Where FK_ProductID=" + prodIdX))
                        {
                            if (dtProd.Rows.Count > 0)
                            {
                                foreach (DataRow pRow in dtProdPhotos.Rows)
                                {
                                    strImg.Append("<li data-thumb=\"" + Master.rootPath + "upload/products/thumb/" + pRow["PhotoName"].ToString() + "\">");
                                    strImg.Append("<img src=\"" + Master.rootPath + "upload/products/" + pRow["PhotoName"].ToString() + "\" alt=\"" + row["ProductName"].ToString() + " - Genericart Products\" class=\"width100\" />");
                                    strImg.Append("</li>");
                                }
                            }
                        }
                        strImg.Append("</ul>");
                        strImg.Append("</div>");
                        arrProdInfo[0] = strImg.ToString();
                    }
                    else
                    {
                        arrProdInfo[0] = "<img src=\"" + Master.rootPath + "upload/products/" + row["ProductPhoto"].ToString() + "\" alt=\"\" class=\"width100\" />";
                    }

                    //product text info
                    //<h1 class="pageH2 clrLightBlack semiBold mrg_B_5">Crocin Advance 650mg</h1>
                    //<p class="clrGrey semiBold mrg_B_10">Strip of 15 tablets.</p> 
                    //<span class="colrPink semiBold small"><span class="reqPrescription">Rx</span> Prescription Required</span>
                    //<span class="space10"></span>
                    //<p class="clrGrey fontRegular small line-ht-5">Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text</p>
                    //<span class="greyLine"></span>
                    //<span class="semiBold dispBlk mrg_B_10">Manufacturer : LIFE VISION</span>
                    //<span class="fontRegular colorGreen semiMedium dispBlk mrg_B_5">In Stock</span>
                    //<span class="fontRegular colrPink semiMedium dispBlk mrg_B_5">Currently Unavailable</span>
                    //<span class="themeClrSec fontRegular semiMedium dispBlk mrg_B_10">Product Code : CR650</span>
                    //<span class="clrGrey fontRegular">MRP : <span class="clrGrey strike fontRegular mrg_R_5">&#8377; 90.7 </span> (Inclusive of all taxes)</span>
                    //<span class="space5"></span>
                    //<span class="themeClrPrime large semiBold">&#8377; 25.7</span>
                    //<span class="space10"></span>
                    //<span class="prod-discount large">60% Off</span>
                    
                    strMarkup.Append("<h1 class=\"pageH4 clrLightBlack semiBold mrg_B_5\">" + row["ProductName"].ToString() + "</h1>");
                    string prodUnit = c.GetReqData("UnitProducts", "UnitName", "UnitID=" + row["FK_UnitID"]).ToString();
                    string prodManufacturer = c.GetReqData("Manufacturers", "MfgName", "MfgId=" + row["FK_MfgID"]).ToString();
                    strMarkup.Append("<p class=\"clrGrey semiBold mrg_B_10\">" + prodUnit + " of " + row["PackagingType"].ToString() + ".</p> ");
                    if (row["PrescriptionFlag"] != DBNull.Value && row["PrescriptionFlag"] != null && row["PrescriptionFlag"].ToString() != "")
                    {
                        if (row["PrescriptionFlag"].ToString() == "1")
                        {
                            strMarkup.Append("<span class=\"colrPink semiBold small\"><span class=\"reqPrescription\">Rx</span> Prescription Required</span>");
                            strMarkup.Append("<span class=\"space10\"></span>");
                        }
                    }

                    if (row["ProductShortDesc"] != DBNull.Value && row["ProductShortDesc"] != null && row["ProductShortDesc"].ToString() != "" && row["ProductShortDesc"].ToString() != "-")
                    {
                        strMarkup.Append("<p class=\"clrGrey fontRegular small line-ht-5\">" + row["ProductShortDesc"].ToString() + "</p>");
                    }
                    strMarkup.Append("<span class=\"greyLine\"></span>");

                    strMarkup.Append("<span class=\"semiBold dispBlk mrg_B_10\">Manufacturer : " + prodManufacturer + "</span>");

                    if (Convert.ToInt32(row["ProductStock"].ToString()) > 0)
                    {
                        strMarkup.Append("<span class=\"fontRegular colorGreen semiMedium dispBlk mrg_B_5\">In Stock</span>");
                    }
                    else
                    {
                        strMarkup.Append("<span class=\"fontRegular colrPink semiMedium dispBlk mrg_B_5\">Currently Unavailable</span>");
                    }

                    strMarkup.Append("<span class=\"themeClrSec fontRegular semiMedium dispBlk mrg_B_10\">Product Code : " + row["ProductSKU"].ToString() + "</span>");
                    strMarkup.Append("<span class=\"clrGrey fontRegular\">MRP : <span class=\"clrGrey strike fontRegular mrg_R_5\">&#8377; " + row["PriceMRP"].ToString() + "</span> (Inclusive of all taxes)</span>");
                    strMarkup.Append("<span class=\"space5\"></span>");
                    strMarkup.Append("<span class=\"themeClrPrime large semiBold\" id=\"pPrice\">&#8377; " + row["PriceSale"].ToString() + "</span>");
                    txtOrigPrice.Text = row["PriceSale"].ToString();
                    txtBasePrice.Text = row["PriceMRP"].ToString();
                    strMarkup.Append("<span class=\"space10\"></span>");
                    strMarkup.Append("<span class=\"prod-discount large\" id=\"pDis\">" + row["DiscountPercent"].ToString() + "% Off</span>");

                    arrProdInfo[1] = strMarkup.ToString();

                    if (row["ProductLongDesc"] != DBNull.Value && row["ProductLongDesc"] != null && row["ProductLongDesc"].ToString() != "" && row["ProductLongDesc"].ToString() != "-")
                    {
                        arrProdInfo[2] = "<p class=\"clrGrey line-ht-5\">" + row["ProductLongDesc"].ToString() + "</p>";
                    }
                    else
                    {
                        arrProdInfo[2] = "NA";
                    }

                    if (row["IsNotForOnlineSale"] != DBNull.Value && row["IsNotForOnlineSale"] != null && row["IsNotForOnlineSale"].ToString() != "")
                    {
                        if (row["IsNotForOnlineSale"].ToString() == "1")
                        {
                            prodAvailable = "<span class=\"semiMedium fontRegular colrPink dispBlk\">This Product is not for Online Sale, You can purchase it from Physical Store.</span>";
                        }
                    }

                    //<a href="#" class="blueAnch semiBold upperCase letter-sp-2 dspInlineBlk">Add To Cart</a>
                    if (Convert.ToInt32(row["ProductStock"].ToString()) != 0)
                    {
                        if (row["IsNotForOnlineSale"] != DBNull.Value && row["IsNotForOnlineSale"] != null && row["IsNotForOnlineSale"].ToString() != "")
                        {
                            if (row["IsNotForOnlineSale"].ToString() != "1")
                            {

                                // add to cart anchor starts
                                StringBuilder strCart = new StringBuilder();
                                strCart.Append("<span id=\"cartAnch-" + row["ProductID"] + "\">");
                                HttpCookie ordCookie = Request.Cookies["ordId"];     // Get Cookies Value
                                if (ordCookie != null) // Check whether cookies are not null
                                {
                                    string[] arrOrd = ordCookie.Value.Split('#'); // if cookies are not null, split its value by '#' and get its orderId

                                    int orderId = Convert.ToInt32(arrOrd[0].ToString());
                                    string cartItems = "";
                                    using (DataTable dtProducts = c.GetDataTable("Select FK_DetailProductID From OrdersDetails Where FK_DetailOrderID=" + orderId))
                                    {
                                        if (dtProducts.Rows.Count > 0)
                                        {
                                            foreach (DataRow prRow in dtProducts.Rows)
                                            {
                                                if (cartItems == "")
                                                {
                                                    cartItems = prRow["FK_DetailProductID"].ToString();
                                                }
                                                else
                                                {
                                                    cartItems = cartItems + "," + prRow["FK_DetailProductID"].ToString();
                                                }
                                            }
                                        }
                                    }

                                    string[] arrCartProducts = cartItems.Split(',');
                                    if (arrCartProducts.Contains(prodIdX.ToString()))  // if given product found in arry, mark it added, else display normal 'add to cart' anchor
                                        strCart.Append("<span class=\"pAdded\">Added To Request</span>");
                                    else
                                        strCart.Append("<a href=\"" + Master.rootPath + "add-to-cart/" + row["ProductID"].ToString() + "\" class=\"cartProd blueAnch semiBold upperCase letter-sp-2 dspInlineBlk\">Add Request</a>");
                                }
                                else
                                {
                                    //strCart.Append("<a href=\"#\" class=\"addToCartAnch cartProd semiBold upperCase letter-sp-2\">Add To Cart</a>");
                                    strCart.Append("<a href=\"" + Master.rootPath + "add-to-cart/" + row["ProductID"].ToString() + "\" class=\"cartProd blueAnch semiBold upperCase letter-sp-2 dspInlineBlk\">Add Request</a>");
                                }
                                strCart.Append("</span>");

                                arrProdInfo[3] = strCart.ToString();
                                // add to cart anchor ends
                            }
                        }



                        if (c.IsRecordExist("Select ProdOptionID From ProductOptions Where FK_ProductID=" + prodIdX + " AND DelMark=0 AND IsActive=1"))
                        {
                            string optionGroupId = c.GetReqData("ProductOptions", "FK_OptionGroupID", "FK_ProductID=" + prodIdX + " AND DelMark=0 AND IsActive=1").ToString();
                            arrProdInfo[5] = c.GetReqData("OptionGroups", "OptionGroupName", "OptionGroupID=" + optionGroupId).ToString();
                        }

                        //related products display
                        if (c.IsRecordExist("Select ProductCatID From ProductCategory Where delMark=0 AND ProductCatID=" + row["FK_SubCategoryID"] + " AND RelatedProdId IS NOT NULL AND RelatedProdId<>''"))
                        {
                            string relProdIds = c.GetReqData("ProductCategory", "RelatedProdId", "ProductCatID=" + row["FK_SubCategoryID"]).ToString();
                            StringBuilder strRelProd = new StringBuilder();

                            strRelProd.Append("<span class=\"space20\"></span>");
                            strRelProd.Append("<div class=\"col_1140\"><h3 class=\"pageH3 clrLightBlack semiBold mrg_B_10\">Related Products</h3></div>");

                            using (DataTable dtRelProd = MasterClass.Query("Select a.ProductID, a.ProductSKU, a.ProductName, a.PriceMRP, a.PriceSale, a.PackagingType, " +
                                    " CONVERT(DECIMAL(20, 2), (100 - (a.PriceSale * 100) / a.PriceMRP)) AS DiscountPercent, a.ProductLongDesc, a.ProductShortDesc, " +
                                    " a.ProductPhoto, a.FK_SubCategoryID, a.FK_UnitID, a.PackagingType, a.PrescriptionFlag From ProductsData a Where a.ProductActive=1 AND a.delMark=0 AND a.ProductID IN (" + relProdIds + ") Order By ProductName ASC"))
                            {
                                if (dtRelProd.Rows.Count > 0)
                                {
                                    strRelProd.Append("<div id=\"prodSlider\">");
                                    strRelProd.Append("<div class=\"MS-content\">");
                                    foreach (DataRow prow in dtRelProd.Rows)
                                    {
                                        strRelProd.Append("<div class=\"item\">");
                                        string subCatName = c.GetReqData("ProductCategory", "ProductCatName", "ProductCatID=" + prow["FK_SubCategoryID"].ToString()).ToString();
                                        string prodUrl = Master.rootPath + "products/" + c.UrlGenerator(subCatName) + "/" + c.UrlGenerator(prow["ProductName"].ToString().ToLower()) + "-" + prow["ProductID"].ToString();
                                        strRelProd.Append("<a href=\"" + prodUrl + "\" class=\"txtDecNone\">");
                                        strRelProd.Append("<div class=\"prodContainer posRelative\" title=\"" + prow["ProductName"].ToString() + "\">");
                                        strRelProd.Append("<div class=\"pad_15\">");
                                        if (prow["PrescriptionFlag"] != DBNull.Value && prow["PrescriptionFlag"] != null && prow["PrescriptionFlag"].ToString() != "")
                                        {
                                            if (prow["PrescriptionFlag"].ToString() == "1")
                                            {
                                                strRelProd.Append("<div class=\"medPrescription\" title=\"Prescription Required\">Rx</div>");
                                            }
                                        }
                                        strRelProd.Append("<div class=\"txtCenter\">");
                                        if (prow["ProductPhoto"] != DBNull.Value && prow["ProductPhoto"] != null && prow["ProductPhoto"].ToString() != "")
                                        {
                                            strRelProd.Append("<div class=\"proFixSize\">");
                                            strRelProd.Append("<img src=\"" + Master.rootPath + "upload/products/thumb/" + prow["ProductPhoto"].ToString() + "\" alt=\"" + prow["ProductName"].ToString() + "\" class=\"width100\" />");
                                            strRelProd.Append("</div>");
                                        }
                                        else
                                        {
                                            strRelProd.Append("<div class=\"proFixSize\">");
                                            strRelProd.Append("<img src=\"" + Master.rootPath + "upload/products/thumb/no-photo.png\" alt=\"" + prow["ProductName"].ToString() + "\" class=\"width100\" />");
                                            strRelProd.Append("</div>");
                                        }
                                        strRelProd.Append("</div>");
                                        strRelProd.Append("<span class=\"prodLine\"></span>");
                                        strRelProd.Append("<span class=\"space15\"></span>");
                                        string pName = prow["ProductName"].ToString().Length > 25 ? prow["ProductName"].ToString().Substring(0, 25) + "..." : prow["ProductName"].ToString();
                                        strRelProd.Append("<h4 class=\"prodName semiBold\">" + pName + "</h4>");
                                        strRelProd.Append("<span class=\"space10\"></span>");
                                        string prUnit = c.GetReqData("UnitProducts", "UnitName", "UnitID=" + prow["FK_UnitID"]).ToString();
                                        strRelProd.Append("<span class=\"small clrGrey line-ht-5 dspBlk semiBold\">" + prUnit + " Of " + prow["PackagingType"].ToString() + "</span>");
                                        strRelProd.Append("<span class=\"space10\"></span>");
                                        string shortDesc = prow["ProductShortDesc"].ToString().Length > 25 ? prow["ProductShortDesc"].ToString().Substring(0, 25) + "..." : prow["ProductShortDesc"].ToString();
                                        strRelProd.Append("<p class=\"clrGrey line-ht-5 tiny mrg_B_15\">" + shortDesc + "</p>");
                                        strRelProd.Append("<span class=\"prod-offer-price\">&#8377; " + prow["PriceSale"].ToString() + "</span>");
                                        strRelProd.Append("<span class=\"prod-price\">&#8377; " + prow["PriceMRP"].ToString() + "</span>");
                                        strRelProd.Append("<span class=\"prod-discount\">" + prow["DiscountPercent"].ToString() + "%</span>");
                                        strRelProd.Append("</div>");
                                        strRelProd.Append("</div>");
                                        strRelProd.Append("</a>");
                                        strRelProd.Append("</div>");
                                    }
                                    strRelProd.Append("</div>");
                                    strRelProd.Append("<div class=\"MS-controls\">");
                                    strRelProd.Append("<a class=\"MS-left\"><img src=\"" + Master.rootPath + "images/controls-prev.png\" /></a>");
                                    strRelProd.Append("<a class=\"MS-right\"><img src=\"" + Master.rootPath + "images/controls.png\" /></a>");
                                    strRelProd.Append("</div>");
                                    strRelProd.Append("</div>");
                                    arrProdInfo[6] = strRelProd.ToString();
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    private void FillQuantity()
    {
        for (int i = 1; i < 31; i++)
        {
            ddrQty.Items.Add(i.ToString());
            ddrQty.SelectedValue = "1";
        }
    }

    protected void ddrOption_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string[] arrProd = Page.RouteData.Values["prodId"].ToString().Split('-');
            int prodId = Convert.ToInt32(arrProd[arrProd.Length - 1]);
            GetProductInfo(prodId);
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }


    [WebMethod]
    public static string GetOptionId(string val, string productId)
    {
        iClass c = new iClass();
        double incrementPrice = 0;
        incrementPrice = Convert.ToDouble(c.GetReqData("ProductOptions", "PriceIncrement", "FK_OptionID=" + val + " AND FK_ProductID=" + productId));
        return incrementPrice.ToString();
    }
}