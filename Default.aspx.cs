using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Web.Script.Serialization;
using System.Net;
using System.Web.Services;
using System.Web.Script.Services;


public partial class _Default : System.Web.UI.Page
{
    public string prodUrl, rootPath, prodstr, catStr, blogsStr, diseaseStr, errMsg, citySate, currentYear, bannerStr, custLink, custTitle, shopExist, blogStr, cardiacStr;
    public string CartCount, MemberId = "", MemberEmail = "";
    public string[] arrStats = new string[5];
    MasterClass m = new MasterClass();
    iClass c = new iClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            rootPath = c.ReturnHttp();
            currentYear = DateTime.Now.Year.ToString();

            //Activate OBP Session directly for testing
            //Session["adminObp"] = 28;


            if (!IsPostBack)
            {
                if (Request.QueryString["act"] != null)
                {
                    if (Request.QueryString["act"] == "logout")
                    {
                        Session["adminMaster"] = null;
                        Response.Redirect(rootPath + "admingenshopping", false);
                    }

                    if (Request.QueryString["act"] == "custlogout")
                    {
                        Session["genericCust"] = null;
                        Response.Redirect(rootPath + "login", false);
                    }

                    if (Request.QueryString["act"] == "franchiseelogout")
                    {
                        Session["adminFranchisee"] = null;
                        Response.Redirect(rootPath + "franchisee", false);
                    }

                    if (Request.QueryString["act"] == "doclogout")
                    {
                        Session["adminDoctor"] = null;
                        Response.Redirect(rootPath + "doctors", false);
                    }

                    if (Request.QueryString["act"] == "dhlogout")
                    {
                        Session["adminDH"] = null;
                        Response.Redirect(rootPath + "districthead", false);
                    }

                    if (Request.QueryString["act"] == "zhlogout")
                    {
                        Session["adminZH"] = null;
                        Response.Redirect(rootPath + "zonalhead", false);
                    }

                    if (Request.QueryString["act"] == "supportTeamLogout")
                    {
                        Session["adminSupport"] = null;
                        Response.Redirect(rootPath + "supportteam", false);
                    }

                    if (Request.QueryString["act"] == "mglogout")
                    {
                        Session["adminMgmt"] = null;
                        Response.Redirect(rootPath + "management", false);
                    }

                    if (Request.QueryString["act"] == "genMitralogout")
                    {
                        Session["adminGenMitra"] = null;
                        Response.Redirect(rootPath + "genericmitra", false);
                    }

                    if (Request.QueryString["act"] == "bdmlogout")
                    {
                        Session["adminBdm"] = null;
                        Response.Redirect(rootPath + "bdm", false);
                    }

                    if (Request.QueryString["act"] == "purchlogout")
                    {
                        Session["adminPurch"] = null;
                        Response.Redirect(rootPath + "purchase", false);
                    }

                    if (Request.QueryString["act"] == "acclogout")
                    {
                        Session["adminAcc"] = null;
                        Response.Redirect(rootPath + "account", false);
                    }
                    if (Request.QueryString["act"] == "obplogout")
                    {
                        Session["adminObp"] = null;
                        Response.Redirect(rootPath + "obp", false);
                    }
                    if (Request.QueryString["act"] == "obpmanlogout")
                    {
                        Session["adminObpManager"] = null;
                        Response.Redirect(rootPath + "obpmanager", false);
                    }
                    if (Request.QueryString["act"] == "orgmemlogout")
                    {
                        Session["adminorgMember"] = null;
                        Response.Redirect(rootPath + "orgmember", false);
                    }
                    if (Request.QueryString["act"] == "GOBPDHlogout")
                    {
                        Session["adminGOBPDH"] = null;
                        Response.Redirect(rootPath + "GOBPDH", false);
                    }
                }
                
                // Diabetic Medicines  //Remove (Vinayak 23-sept-22)
                //GetProducts(15);

                // Cardiac Medicines  //Remove (Vinayak 23-sept-22)
                //GetProducts(17);

                GetCategories();
                GetDiseases();
                GetBanners();
                GetBlogs();
                GetStats();

                if (Request.Cookies["cityId"] != null)
                {
                    //txtCity.Text = Request.Cookies["cityId"].Value.ToString();
                }
                else
                {
                    if (Request.Cookies["userLoc"] == null)
                    {
                        string loc = GetLocation();
                        HttpCookie userLoc = new HttpCookie("userLoc");
                        userLoc.Value = loc.ToString();
                        Response.Cookies.Add(userLoc);
                        Response.Cookies["userLoc"].Expires = DateTime.Now.AddDays(30);

                        if (Request.Cookies["userLoc"] != null)
                        {
                            GetUserLocationData();
                        }
                    }
                    else
                    {
                        //citySate = Request.Cookies["userLoc"].Value.ToString();
                        //string[] arrLocation = citySate.Split('#');
                        //string city = arrLocation[0].ToString();
                        //string state = arrLocation[1].ToString();
                        //int cityId = Convert.ToInt32(c.GetReqData("CityData", "CityID", "CityName='" + city + "'"));
                        //int stateId = Convert.ToInt32(c.GetReqData("StatesData", "StateID", "StateName='" + state + "'"));
                        //txtCity.Text = city.ToString();
                        GetUserLocationData();
                    }
                }


                HttpCookie reqCookie = Request.Cookies["ordId"];
                if (reqCookie != null)
                {
                    string[] arrItems = reqCookie.Value.Split('#');
                    int orderId = Convert.ToInt32(arrItems[0].ToString());
                    int count = Convert.ToInt32(c.GetReqData("OrdersDetails", "Count(FK_DetailProductID)", "FK_DetailOrderID=" + orderId));
                    CartCount = count.ToString();
                }
                else
                {
                    CartCount = "0";
                }

                
            }

            if (Session["genericCust"] != null)
            {
                custLink = rootPath + "customer/user-info";
                string cName = c.GetReqData("CustomersData", "CustomerName", "CustomrtID=" + Session["genericCust"]).ToString();
                string[] arrcName = cName.ToString().Split(' ');
                custTitle = "Hello, " + arrcName[0].ToString();
            }
            else
            {
                custLink = rootPath + "login";
                custTitle = "Login";
            }
        }
        catch (Exception ex)
        {
            //errMsg = c.ErrNotification(3, ex.Message.ToString());
            errMsg = c.ErrNotification(3, "Page Load error");
            return;
        }
    }

    private void GetUserLocationData()
    {
        try
        {
            citySate = Request.Cookies["userLoc"].Value.ToString();
            if (citySate != "0")
            {
                //string[] arrLocation = citySate.Split('#');
                //string city = arrLocation[0].ToString();
                //string state = arrLocation[1].ToString();
                //int cityId = Convert.ToInt32(c.GetReqData("CityData", "CityID", "CityName='" + city + "'"));
                //int stateId = Convert.ToInt32(c.GetReqData("StatesData", "StateID", "StateName='" + state + "'"));
                //txtCity.Text = city.ToString();

                //string pincode = arrLocation[6].ToString();
                //if (!c.IsRecordExist("Select FranchID From FranchiseeData Where FranchPinCode='" + pincode + "' AND FranchActive=1"))
                //{
                //    shopExist = "<div class=\"absMsg\">Please enter your location, or <a href=\"" + rootPath + "nearest-shops\" class=\"clrWhite\">Click Here</a> to search nearest shop !</div>";
                //    shopExist = "<div class=\"absMsg\">Currently We are not available in your area..!!</div>";
                //    //Please enter your location, or search nearest shop !
                //}
                //else
                //{
                //    shopExist = "<div class=\"absMsg\">Please enter your location, or <a href=\"" + rootPath + "nearest-shops\" class=\"clrWhite\">Click Here</a> to search nearest shop !</div>";
                //}
                //shopExist = "<div class=\"absMsg\">Please enter your location, or <a href=\"" + rootPath + "nearest-shops\" class=\"clrWhite\">Click Here</a> to search nearest shop !</div>";
                shopExist = "<div class=\"absMsg\"><a href=\"" + rootPath + "nearest-shops\" class=\"clrWhite\">Click Here</a> to search nearest shop !</div>";
            }
            else
            {
                shopExist = "<div class=\"absMsg\"><a href=\"" + rootPath + "nearest-shops\" class=\"clrWhite\">Click Here</a> to search nearest shop !</div>";
                //shopExist = "<div class=\"absMsg\">Please enter your location, or <a href=\"" + rootPath + "nearest-shops\" class=\"clrWhite\">Click Here</a> to search nearest shop !</div>";
                //shopExist = "<div class=\"absMsg\">Currently We are not available in your area..!!</div>";
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            //errMsg = c.ErrNotification(3, "Get Location Error");
            return;
        }
    }

    private void GetProducts(int catIdX)
    {
        try
        {
            //using (DataTable dtProd = MasterClass.Query("SELECT ROW_NUMBER() over (order by Item.Name) as Row, Item.Name, Item.Auto, Substring(Item.Name, 0, 28) + '..' As PName, ItemGroupAuto, UnitAuto, BrandAuto, PurchaseRate, MRP, SaleRate, Description, BusinessValue, Item.Status, ItemGroup.Name as ItemGroupName, Brand.Name as BrandName, Unit.Name as UnitName, Validity, BarCode, ItemUrl, CONVERT(DECIMAL(20, 2), (100 - (SaleRate * 100) / MRP)) AS DiscountPercent, Substring(ShortDescription, 0, 35) + '..' As ShortDescription FROM Item left join itemgroup on Item.ItemGroupAuto = ItemGroup.auto left join Unit on Item.UnitAuto = Unit.Auto left join Brand on Item.BrandAuto = Brand.Auto where isnull(Item.Status, 0) = 1 and ItemGroupAuto = 1 order by item.Name"))
            //using (DataTable dtProd = MasterClass.Query("Select TOP 10 a.ProductID, a.ProductSKU, a.ProductName, a.PriceMRP, a.PriceSale, a.PackagingType, " +
            //    " CONVERT(DECIMAL(20, 2), (100 - (a.PriceSale * 100) / a.PriceMRP)) AS DiscountPercent, a.ProductLongDesc, a.ProductShortDesc, " +
            //    " a.ProductPhoto, a.FK_SubCategoryID, a.FK_UnitID, a.PackagingType, a.PrescriptionFlag From ProductsData a Where a.ProductActive=1 AND a.FK_SubCategoryID=7 AND a.delMark=0 Order By ProductName ASC"))
            using (DataTable dtProd = MasterClass.Query("Select TOP 10 a.ProductID, a.ProductSKU, a.ProductName, a.PriceMRP, a.PriceSale, a.PackagingType, " +
                " CONVERT(DECIMAL(20, 2), (100 - (a.PriceSale * 100) / a.PriceMRP)) AS DiscountPercent, a.ProductLongDesc, a.ProductShortDesc, " +
                " a.ProductPhoto, a.FK_SubCategoryID, a.FK_UnitID, a.PackagingType, a.PrescriptionFlag From ProductsData a Where a.ProductActive=1 AND a.FK_SubCategoryID=" + catIdX + " AND a.delMark=0 Order By ProductName ASC"))
            {
                if (dtProd.Rows.Count > 0)
                {
                    StringBuilder strMarkup = new StringBuilder();

                    foreach (DataRow row in dtProd.Rows)
                    {
                        strMarkup.Append("<div class=\"item\">");
                        string subCatName = c.GetReqData("ProductCategory", "ProductCatName", "ProductCatID=" + row["FK_SubCategoryID"].ToString()).ToString();
                        string prodUrl = rootPath + "products/" + c.UrlGenerator(subCatName) + "/" + c.UrlGenerator(row["ProductName"].ToString().ToLower()) + "-" + row["ProductID"].ToString();
                        strMarkup.Append("<a href=\"" + prodUrl + "\" class=\"txtDecNone\">");
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
                            strMarkup.Append("<img src=\"" + rootPath + "upload/products/thumb/" + row["ProductPhoto"].ToString() + "\" alt=\"" + row["ProductName"].ToString() + "\" class=\"width100\" />");
                            strMarkup.Append("</div>");
                        }
                        else
                        {
                            strMarkup.Append("<div class=\"proFixSize\">");
                            strMarkup.Append("<img src=\"" + rootPath + "upload/products/thumb/no-photo.png\" alt=\"" + row["ProductName"].ToString() + "\" class=\"width100\" />");
                            strMarkup.Append("</div>");
                        }
                        strMarkup.Append("</div>");
                        strMarkup.Append("<span class=\"prodLine\"></span>");
                        strMarkup.Append("<span class=\"space15\"></span>");
                        //string subCatName = c.GetReqData("ProductCategory", "ProductCatName", "ProductCatID=" + row["FK_SubCategoryID"].ToString()).ToString();
                        //string prodUrl = rootPath + "products/" + c.UrlGenerator(subCatName) + "/" + c.UrlGenerator(row["ProductName"].ToString().ToLower()) + "-" + row["ProductID"].ToString();
                        string pName = row["ProductName"].ToString().Length > 20 ? row["ProductName"].ToString().Substring(0, 20) + "..." : row["ProductName"].ToString();
                        strMarkup.Append("<h4 class=\"prodName semiBold\">" + pName + "</h4>");
                        strMarkup.Append("<span class=\"space10\"></span>");
                        string prodUnit = c.GetReqData("UnitProducts", "UnitName", "UnitID=" + row["FK_UnitID"]).ToString();
                        strMarkup.Append("<span class=\"small clrGrey line-ht-5 dspBlk semiBold\">" + prodUnit + " Of " + row["PackagingType"].ToString() + "</span>");
                        strMarkup.Append("<span class=\"space10\"></span>");
                        string shortDesc = row["ProductShortDesc"].ToString().Length > 20 ? row["ProductShortDesc"].ToString().Substring(0, 20) + "..." : row["ProductShortDesc"].ToString();
                        strMarkup.Append("<p class=\"clrGrey line-ht-5 tiny mrg_B_15\">" + shortDesc + "</p>");
                        strMarkup.Append("<span class=\"prod-offer-price\">&#8377; " + row["PriceSale"].ToString() + "</span>");
                        strMarkup.Append("<span class=\"prod-price\">&#8377; " + row["PriceMRP"].ToString() + "</span>");
                        strMarkup.Append("<span class=\"prod-discount\">" + row["DiscountPercent"].ToString() + "%</span>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</a>");
                        strMarkup.Append("</div>");
                    }
                    if (catIdX == 15)
                        prodstr = strMarkup.ToString();
                    else
                        cardiacStr = strMarkup.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            prodstr = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    private void GetCategories()
    {
        try
        {
            //using (DataTable dtCat = MasterClass.Query("SELECT Auto ,Name ,Status ,MainItemGroupAuto ,ItemGroupUrl FROM dbo.ItemGroup WHERE MainItemGroupAuto = 1 ORDER by Auto"))
            //using (DataTable dtCat = MasterClass.Query("Select ProductCatID, ProductCatName From ProductCategory Where ParentCatID<>0 AND delMark=0 ORDER by ProductCatID DESC"))
            using (DataTable dtCat = MasterClass.Query("Select HealthProdId, HealthProdName, HealthProdCoverPhoto From HealthProductsData Where delMark=0 ORDER by HealthProdName ASC"))
            {
                if (dtCat.Rows.Count > 0)
                {
                    StringBuilder strMarkup = new StringBuilder();

                    foreach (DataRow row in dtCat.Rows)
                    {
                        //strMarkup.Append("<div class=\"slide\">");
                        //string catUrl = rootPath + "categories/" + c.UrlGenerator(row["ProductCatName"].ToString().ToLower()) + "-" + row["ProductCatID"].ToString() + "/1";
                        //strMarkup.Append("<a href=\"" + catUrl + "\" class=\"txtDecNone\">");
                        //strMarkup.Append("<div class=\"prodContainer txtCenter\">");
                        //strMarkup.Append("<div class=\"genericClass top-right-border\">");
                        //strMarkup.Append("<div class=\"pad_30\">");
                        ////strMarkup.Append("<img src=\"" + rootPath + "upload/products/thumb/no-photo.png\" class=\"width100\" />");
                        //string imgName = "";
                        //using (DataTable dtCatProd = c.GetDataTable("Select TOP 1 ProductID, ProductPhoto From ProductsData Where delMark=0 AND ProductActive=1 AND FK_SubCategoryID=" + row["ProductCatID"] + " AND ProductPhoto IS NOT NULL AND ProductPhoto<>'no-photo.png' Order By ProductID DESC"))
                        //{
                        //    if (dtCatProd.Rows.Count > 0)
                        //    {
                        //        DataRow cpRow = dtCatProd.Rows[0];
                        //        imgName = cpRow["ProductPhoto"].ToString();
                        //    }
                        //    else
                        //    {
                        //        imgName = "no-photo.png";
                        //    }
                        //}
                        //strMarkup.Append("<img src=\"" + rootPath + "upload/products/thumb/" + imgName + "\" class=\"width100\" />");
                        //strMarkup.Append("</div>");
                        //strMarkup.Append("</div>");
                        //strMarkup.Append("<div class=\"pad_10\">");
                        ////string catUrl = rootPath + "categories/" + c.UrlGenerator(row["ProductCatName"].ToString().ToLower()) + "-" + row["ProductCatID"].ToString() + "/1";
                        //strMarkup.Append("<h5 class=\"prodName semiBold clrBlack mrg_B_5 dispBlk\">" + row["ProductCatName"].ToString() + "</h5>");
                        //strMarkup.Append("<span class=\"small fontRegular clrBlack\">&#8377; 300 Onwards</span>");
                        //strMarkup.Append("</div>");
                        //strMarkup.Append("</div>");
                        //strMarkup.Append("</a>");
                        //strMarkup.Append("</div>");

                        //strMarkup.Append("<div class=\"slide\">");
                        strMarkup.Append("<div class=\"item\">");
                        string catUrl = rootPath + "health-products/" + c.UrlGenerator(row["HealthProdName"].ToString().ToLower()) + "-" + row["HealthProdId"].ToString() + "/1";
                        strMarkup.Append("<a href=\"" + catUrl + "\" class=\"txtDecNone\">");
                        strMarkup.Append("<div class=\"prodContainer txtCenter\" style=\"height:275px;\">");
                        strMarkup.Append("<div class=\"genericClass top-right-border\">");
                        strMarkup.Append("<div class=\"pad_30\">");
                        //strMarkup.Append("<img src=\"" + rootPath + "upload/products/thumb/no-photo.png\" class=\"width100\" />");
                        if (row["HealthProdCoverPhoto"] != DBNull.Value && row["HealthProdCoverPhoto"] != null && row["HealthProdCoverPhoto"].ToString() != "")
                        {
                            strMarkup.Append("<img src=\"" + rootPath + "upload/healthProducts/" + row["HealthProdCoverPhoto"].ToString() + "\" alt=\"" + row["HealthProdName"].ToString() + "\" class=\"width100\" />");
                        }
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("<div class=\"pad_10\">");
                        //string catUrl = rootPath + "categories/" + c.UrlGenerator(row["ProductCatName"].ToString().ToLower()) + "-" + row["ProductCatID"].ToString() + "/1";
                        strMarkup.Append("<h5 class=\"prodName semiBold clrBlack mrg_B_5 dispBlk\">" + row["HealthProdName"].ToString() + "</h5>");
                        //strMarkup.Append("<span class=\"small fontRegular clrBlack\">&#8377; 300 Onwards</span>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</a>");
                        strMarkup.Append("</div>");
                    }
                    catStr = strMarkup.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            catStr = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    private void GetDiseases()
    {
        try
        {
            //using (DataTable dtDisease = MasterClass.Query("SELECT Auto, Name, Status, DiseaseUrl FROM dbo.Disease ORDER BY Auto"))
            using (DataTable dtDisease = MasterClass.Query("Select DiseaseId, DiseaseName, DiseaseCoverPhoto From DiseaseData Where delMark=0 ORDER BY DiseaseName"))
            {
                if (dtDisease.Rows.Count > 0)
                {
                    StringBuilder strMarkup = new StringBuilder();

                    foreach (DataRow row in dtDisease.Rows)
                    {
                        strMarkup.Append("<div class=\"item\">");
                        string disStr = rootPath + "diseases/" + c.UrlGenerator(row["DiseaseName"].ToString()) + "-" + row["DiseaseId"].ToString() + "/1";
                        strMarkup.Append("<a href=\"" + disStr + "\" class=\"txtDecNone\">");
                        strMarkup.Append("<div class=\"prodContainer txtCenter\" style=\"height:270px;\">");
                        strMarkup.Append("<div class=\"pad_15\">");
                        if (row["DiseaseCoverPhoto"] != DBNull.Value && row["DiseaseCoverPhoto"] != null && row["DiseaseCoverPhoto"].ToString() != "")
                        {
                            strMarkup.Append("<img src=\"" + rootPath + "upload/diseases/" + row["DiseaseCoverPhoto"].ToString() + "\" alt=\"" + row["DiseaseName"].ToString() + "\" class=\"width100\" />");
                        }
                        //string disStr = rootPath + "diseases/" + c.UrlGenerator(row["DiseaseName"].ToString()) + "-" + row["DiseaseId"].ToString() + "/1";
                        strMarkup.Append("<span class=\"space5\"></span>");
                        strMarkup.Append("<h5 class=\"prodName semiBold\">" + row["DiseaseName"].ToString() + "</h5>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</a>");
                        strMarkup.Append("</div>");
                    }
                    diseaseStr = strMarkup.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            diseaseStr = m.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            GetProducts(15);
            GetProducts(17);
            GetCategories();
            GetDiseases();
            GetBanners();
            if (TxtSearch.Text != "")
            {
                //DataTable dt = MasterClass.Query("Select ProductID, ProductName, FK_SubCategoryID FROM ProductsData where ProductName = '" + TxtSearch.Text + "' and isnull(ProductActive, 0) = 1 ");
                using (DataTable dtProdList = c.GetDataTable("Select ProductID, ProductName, FK_SubCategoryID FROM ProductsData where ProductName = '" + TxtSearch.Text + "' and delMark=0 AND isnull(ProductActive, 0) = 1 "))
                {
                    if (dtProdList.Rows.Count > 0)
                    {
                        DataRow row = dtProdList.Rows[0];
                        string subCatName = c.GetReqData("ProductCategory", "ProductCatName", "ProductCatID=" + row["FK_SubCategoryID"].ToString()).ToString();
                        string prodUrl = rootPath + "products/" + c.UrlGenerator(subCatName) + "/" + c.UrlGenerator(row["ProductName"].ToString().ToLower()) + "-" + row["ProductID"].ToString();
                        Response.Redirect(prodUrl, false);
                    }
                    else
                    {
                        TxtSearch.Text = "";
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Selected Product does not exist');", true);
                        TxtSearch.Focus();
                    }
                }
            }
            else
            {
                TxtSearch.Text = "";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Please select product to continue');", true);
                TxtSearch.Focus();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "BtnSearch_Click", ex.Message.ToString());
            return;
        }
    }

    private string GetLocation()
    {
        try
        {
            //string ipAddress = "45.249.252.98";
            string ipAddress = GetIPAddress();
            string strQuery;
            string key = "3e991d59cdd1caea8b82ed3370145f2d019f136a037afe971e4527dcf7d88c37";
            HttpWebRequest HttpWReq;
            HttpWebResponse HttpWResp;

            strQuery = "http://api.ipinfodb.com/v3/ip-city/?" + "ip=" + ipAddress + "&key=" + key + "&format=json";

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            HttpWReq = (HttpWebRequest)WebRequest.Create(strQuery);
            HttpWReq.Method = "GET";
            HttpWResp = (HttpWebResponse)HttpWReq.GetResponse();

            System.IO.StreamReader reader = new System.IO.StreamReader(HttpWResp.GetResponseStream());
            string content = reader.ReadToEnd();

            dynamic item = serializer.Deserialize<object>(content);
            string city = item["cityName"];
            string countryc = item["countryCode"];
            string countryn = item["countryName"];
            string region = item["regionName"];
            string lat = item["latitude"];
            string longi = item["longitude"];
            string timez = item["timeZone"];
            string zip = item["zipCode"];

            if (city.ToString().Length > 2 && region.ToString().Length > 2 && lat.ToString().Length > 2 && longi.ToString().Length > 2 && zip.ToString().Length > 2)
            {
                return city + "#" + region + "#" + countryn + "#" + lat + "#" + longi + "#" + timez + "#" + zip;
            }
            else
            {
                return "0";
            }
        }
        catch (Exception ex)
        {
            return ex.Message.ToString();
        }
    }

    public string GetIPAddress()
    {
        string ipaddress;
        ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (ipaddress == "" || ipaddress == null)
            ipaddress = Request.ServerVariables["REMOTE_ADDR"];

        return ipaddress;
    }


    [WebMethod]
    public static string GetCity(string cityName)
    {
        //Set the Cookie value.
        HttpContext.Current.Response.Cookies["cityId"].Value = cityName;
        HttpContext.Current.Response.Cookies["cityId"].Expires = DateTime.Now.AddDays(30);
        return cityName;
    }

    private void GetBanners()
    {
        try
        {
            using (DataTable dtBanner = c.GetDataTable("Select bannerId, imageName, bannerLink From BannerData Where delMark=0 Order By displayOrder"))
            {
                if (dtBanner.Rows.Count > 0)
                {
                    StringBuilder strMarkup = new StringBuilder();
                    foreach (DataRow row in dtBanner.Rows)
                    {
                        strMarkup.Append("<div>");
                        if (row["bannerLink"] != DBNull.Value && row["bannerLink"] != null && row["bannerLink"].ToString() != "")
                        {
                            strMarkup.Append("<a href=\"" + row["bannerLink"].ToString() + "\"><img src=\"" + rootPath + "upload/banner/" + row["imageName"].ToString() + "\" alt=\"Genericart Shopping\" class=\"width100\" /></a>");
                        }
                        else
                        {
                            strMarkup.Append("<img src=\"" + rootPath + "upload/banner/" + row["imageName"].ToString() + "\" alt=\"Genericart Shopping\" class=\"width100\" />");
                        }
                        strMarkup.Append("</div>");
                    }
                    bannerStr = strMarkup.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetBanners", ex.Message.ToString());
            return;
        }
    }

    private void GetBlogs()
    {
        try
        {
            using (DataTable dtBlogs = c.GetDataTable("Select NewsId, NewsDate, NewsTitle, NewsDesc, NewsImage, ReadCount From NewsData Order By NewsId DESC"))
            {
                if (dtBlogs.Rows.Count > 0)
                {
                    StringBuilder strMarkup = new StringBuilder();
                    foreach (DataRow row in dtBlogs.Rows)
                    {
                        strMarkup.Append("<div class=\"item\">");
                        strMarkup.Append("<div class=\"prodContainer\">");
                        strMarkup.Append("<div style=\"height:155px; overflow:hidden; background:#ccc;\"><img src=\"" + rootPath + "upload/news/thumb/" + row["NewsImage"].ToString() + "\" alt=\"" + row["NewsTitle"].ToString() + "\" class=\"width100 top-right-border\" /></div>");
                        strMarkup.Append("<div class=\"pad_15\">");
                        string nTitle = row["NewsTitle"].ToString().Length > 30 ? row["NewsTitle"].ToString().Substring(0, 30) + "..." : row["NewsTitle"].ToString();
                        string nUrl = rootPath + "blogs/" + c.UrlGenerator(row["NewsTitle"].ToString().ToLower() + '-' + row["NewsId"].ToString());
                        strMarkup.Append("<a href=\"" + nUrl + "\" class=\"blogName semiBold dispBlk mrg_B_10\">" + nTitle + "</a>");
                        string nDesc = row["NewsDesc"].ToString().Length > 90 ? row["NewsDesc"].ToString().Substring(0, 80) + "..." : row["NewsDesc"].ToString();
                        strMarkup.Append("<p class=\"small fontRegular clrGrey line-ht-5 mrg_B_10\">" + nDesc + "</p>");
                        strMarkup.Append("<a href=\"" + nUrl + "\" class=\"readMore\" style=\"font-size:0.9em !important;\">Continue Reading</a>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                        strMarkup.Append("</div>");
                    }
                    blogsStr = strMarkup.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
            c.ErrorLogHandler(this.ToString(), "GetBlogs", ex.Message.ToString());
            return;
        }
    }

    public string OpenConnection1()
    {
        return System.Web.Configuration.WebConfigurationManager.ConnectionStrings["GenCartDATAReg"].ConnectionString;
    }

    public long returnAggregate(string strQuery)
    {
        try
        {
            long rValue = 0;
            SqlConnection con = new SqlConnection(OpenConnection1());
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = strQuery;

            object result = cmd.ExecuteScalar();

            if (result.GetType() != typeof(DBNull))
            {
                rValue = Convert.ToInt32(result);
            }
            else
            {
                rValue = 0;

            }

            con.Close();
            con = null;
            cmd.Dispose();
            return rValue;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void GetStats()
    {
        try
        {
            //state
            arrStats[0] = returnAggregate("Select Count(Distinct stateId) From FranchiseeData Where delMark=0").ToString();
            //arrStats[0] = "7";

            //Franchisee
            //arrStats[1] = returnAggregate("Select Count(frId) From FranchiseeData Where delMark=0 AND isClosed=0 AND legalBlock=0 AND frStatus=1").ToString(); old
            //arrStats[1] = "938";

            //string arrCounts0 = c.GetReqData("FranchiseeData", "Count(frId)", "delMark=0 And isClosed=0 And (frShopCode Is Null OR frShopCode='') And frAnniversary is Null ").ToString();
            string arrCounts0 = returnAggregate("Select Count(frId) From FranchiseeData Where delMark=0 And isClosed=0 And (frShopCode Is Null OR frShopCode='') And frAnniversary is Null ").ToString();
            string arrCounts11 = returnAggregate("Select COUNT(frId) From FranchiseeData Where delMark=0 And frShopCode Is NOT Null And frShopCode<>'' And frAnniversary is NOT Null And isClosed=0 AND Substring(frShopCode,1,4)<>'GMOS' AND Substring(frShopCode,1,4)<>'GMDR' AND Substring(frShopCode,1,4)<>'TRFR' AND Substring(frShopCode,1,4)<>'TRAN' AND Substring(frShopCode,1,4)<>'CANC'  AND transferFlag IS NOT NULL AND transferFlag=0").ToString();
            string arrCounts14 = returnAggregate("Select COUNT(frId) From FranchiseeData Where delMark=0 And frShopCode Is NOT Null And frShopCode<>'' And frAnniversary is Null And isClosed=0 AND Substring(frShopCode,1,4)<>'GMOS' AND Substring(frShopCode,1,4)<>'GMDR' AND Substring(frShopCode,1,4)<>'TRFR' AND Substring(frShopCode,1,4)<>'TRAN' AND Substring(frShopCode,1,4)<>'CANC'").ToString();
            string arrCounts15 = returnAggregate("Select COUNT(frId) From FranchiseeData Where isClosed=1 AND delMark=0").ToString();
            //arrStats[2] = c.returnAggregate("Select Count(frId) From FranchiseeData Where delMark=0 AND isClosed=0 AND legalBlock=0 AND frStatus=1").ToString(); old

            arrStats[1] = (Convert.ToInt32(arrCounts0) + Convert.ToInt32(arrCounts11) + Convert.ToInt32(arrCounts14) + Convert.ToInt32(arrCounts15)).ToString();

            int clientBenifited = (Convert.ToInt32(arrStats[1]) * 5598) / 100000;
            arrStats[2] = clientBenifited.ToString();
            //arrStats[2] = "52";
        }
        catch (Exception ex)
        {
            //errMsg = c.ErrNotification(3, "Get Stats Error");
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }
}