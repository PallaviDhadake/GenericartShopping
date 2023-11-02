using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using System.Configuration;
using System.Text;
using System.Web.Script.Serialization;
using System.Net;
using System.Web.Services;
using System.Web.Script.Services;

public partial class MasterParent : System.Web.UI.MasterPage
{
    iClass c = new iClass();
    public string errMsg, currentYear, CartCount, rootPath, citySate, custLink, custTitle, shopExist, hideMeCalss;
    protected void Page_Load(object sender, EventArgs e)
    {
        // Important code line
        this.Page.Header.DataBind();

        if (Session["adminObp"] != null)
        {
            hideMeCalss="hideMe";
        }

        try
        {
            currentYear = DateTime.Now.Year.ToString();
            shopExist = "<div class=\"absMsg\"><a href=\"" + rootPath + "nearest-shops\" class=\"clrWhite\">Click Here</a> to search nearest shop !</div>";
            if (!IsPostBack)
            {
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

                
            }

            if (Session["genericCust"] != null)
            {
                custLink = rootPath + "customer/user-info";
                //custTitle = "Hello, " + c.GetReqData("CustomersData", "CustomerName", "CustomrtID=" + Session["genericCust"]).ToString();
                string cName = c.GetReqData("CustomersData", "CustomerName", "CustomrtID=" + Session["genericCust"]).ToString();
                string[] arrcName = cName.ToString().Split(' ');
                custTitle = "Hello, " + arrcName[0].ToString();
            }
            else
            {
                custLink = rootPath + "login";
                custTitle = "Login";
                //Response.Redirect(rootPath + "login", false);
            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
            return;
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        rootPath = c.ReturnHttp();
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
        ScriptManager1.Services.Add(new ServiceReference(rootPath + "WebServices/ShoppingWebService.asmx"));
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
            }
            else
            {

            }
        }
        catch (Exception ex)
        {
            errMsg = c.ErrNotification(3, ex.Message.ToString());
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

            //return city + "#" + region + "#" + countryn + "#" + lat + "#" + longi + "#" + timez + "#" + zip;
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

    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (TxtSearch.Text != "")
            {
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

}
