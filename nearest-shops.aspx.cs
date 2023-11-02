using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Web.Services;

public partial class nearest_shops : System.Web.UI.Page
{
	iClass c = new iClass();
	public string shopStr, citySate, zipcode;
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			string strQuery = "";
			if (Request.Cookies["userLoc"] != null)
			{
				citySate = Request.Cookies["userLoc"].Value.ToString();
				if (citySate != "0")
				{
					//string[] arrLocation = citySate.Split('#');
					//string pincode = arrLocation[6].ToString();
					//zipcode = pincode.ToString();
					//strQuery = "Select FranchID, FranchShopCode, FranchName, FranchAddress, FranchMobile From FranchiseeData Where FranchActive=1 AND SUBSTRING(FranchShopCode,1,4)<>'GMDR' AND SUBSTRING(FranchShopCode,1,4)<>'GMOS' AND FranchPinCode='" + pincode + "'";
					strQuery = "Select FranchID, FranchShopCode, FranchName, FranchAddress, FranchMobile From FranchiseeData Where FranchActive=1 AND FranchLegalBlock=0 AND SUBSTRING(FranchShopCode,1,4)<>'GMDR' AND SUBSTRING(FranchShopCode,1,4)<>'GMOS'";
				}
				else
				{
					strQuery = "Select FranchID, FranchShopCode, FranchName, FranchAddress, FranchMobile From FranchiseeData Where FranchActive=1 AND FranchLegalBlock=0 AND SUBSTRING(FranchShopCode,1,4)<>'GMDR' AND SUBSTRING(FranchShopCode,1,4)<>'GMOS'";
				}
				GetShops(strQuery);
			}
			else
			{
				strQuery = "Select FranchID, FranchShopCode, FranchName, FranchAddress, FranchMobile From FranchiseeData Where FranchActive=1 AND FranchLegalBlock=0 AND SUBSTRING(FranchShopCode,1,4)<>'GMDR' AND SUBSTRING(FranchShopCode,1,4)<>'GMOS'";

				GetShops(strQuery);
			}

			if (Request.QueryString["shop"] != null)
			{
				strQuery = "Select FranchID, FranchShopCode, FranchName, FranchAddress, FranchMobile From FranchiseeData Where FranchActive=1 AND FranchLegalBlock=0 AND SUBSTRING(FranchShopCode,1,4)<>'GMDR' AND SUBSTRING(FranchShopCode,1,4)<>'GMOS' AND FranchID=" + Request.QueryString["shop"];
				GetShops(strQuery);
				ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('success', 'Shop Marked as Favorite');", true);
			}
		}
	}

	private void GetShops(string sQuery)
	{
		try
		{
			//using (DataTable dtShops = c.GetDataTable("Select FranchID, FranchShopCode, FranchName, FranchAddress, FranchMobile From FranchiseeData Where FranchActive=1 AND SUBSTRING(FranchShopCode,1,4)<>'GMDR' AND SUBSTRING(FranchShopCode,1,4)<>'GMOS' AND FranchPinCode='" + pincode + "'"))
			using (DataTable dtShops = c.GetDataTable(sQuery))
			{
				if (dtShops.Rows.Count > 0)
				{
					StringBuilder strMarkup = new StringBuilder();
					int bCount = 1;
					foreach (DataRow row in dtShops.Rows)
					{
						strMarkup.Append("<div class=\"width50\">");
						strMarkup.Append("<div class=\"pad_15\">");
						strMarkup.Append("<div class=\"bgWhite box-shadow border_r_5\">");
						strMarkup.Append("<div class=\"pad_15\">");
						strMarkup.Append("<div class=\"shopImg\">");
						strMarkup.Append("<img src=\"" + Master.rootPath + "images/medical-shop.jpg\" />");
						strMarkup.Append("<span class=\"space5\"></span>");
						strMarkup.Append("<span id=\"shop-" + row["FranchID"].ToString() + "\">");

						if (Session["genericCust"] != null)
						{
							if (!c.IsRecordExist("Select CustomrtID From CustomersData Where CustomrtID=" + Session["genericCust"] + " AND CustomerFavShop=" + row["FranchID"].ToString()))
							{
								strMarkup.Append("<a href=\"" + Master.rootPath + "add-fav-shop/" + row["FranchID"].ToString() + "-" + Session["genericCust"] + "\" class=\"fShop txtDecNone\" title=\"Mark as favorite shop\">");
								strMarkup.Append("<div class=\"markFav\" ></div>");
								strMarkup.Append("</a>");
							}
							else
							{
								strMarkup.Append("<div class=\"markedFav\" ></div>");
							}
						}
						else
						{
							strMarkup.Append("<a href=\"" + Master.rootPath + "add-fav-shop/" + row["FranchID"].ToString() + "-" + Session["genericCust"] + "\" class=\"fShop txtDecNone\" title=\"Mark as favorite shop\">");
							//strMarkup.Append("<a href=\"" + Master.rootPath + "add-fav-shop/" + row["FranchID"].ToString() + "-1\" class=\"fShop txtDecNone\" title=\"Mark as favorite shop\">");
							strMarkup.Append("<div class=\"markFav\" ></div>");
							strMarkup.Append("</a>");
						}

						strMarkup.Append("</span>");
						strMarkup.Append("</div>");
						strMarkup.Append("<div class=\"shopInfo\">");
						strMarkup.Append("<h4 class=\"themeClrSec semiBold semiMedium mrg_B_3\">" + row["FranchName"].ToString() + "</h4>");
						strMarkup.Append("<span class=\"colrPink semiBold\">Shop Code : " + row["FranchShopCode"].ToString() + "</span>");
						strMarkup.Append("<span class=\"space10\"></span>");
						strMarkup.Append("<p class=\"clrGrey fontRegular line-ht-5 small mrg_B_10\">Address : " + row["FranchAddress"].ToString() + "</p>");
						strMarkup.Append("<a href=\"tel:" + row["FranchMobile"].ToString() + "\" class=\"conCall conIco fontRegular txtDecNone breakWord\">" + row["FranchMobile"].ToString() + "</a>");
						strMarkup.Append("<span class=\"space20\"></span>");
						strMarkup.Append("</div>");
						strMarkup.Append("<div class=\"float_clear\"></div>");
						strMarkup.Append("</div>");
						strMarkup.Append("</div>");
						strMarkup.Append("</div>");
						strMarkup.Append("</div>");

						if (bCount % 2 == 0)
						{
							strMarkup.Append("<div class=\"float_clear\"></div>");
						}

						bCount++;
					}
					strMarkup.Append("<div class=\"float_clear\"></div>");

					shopStr = strMarkup.ToString();
					Session["shopId"] = null;
				}
				else
				{
					shopStr = "<div class=\"themeBgSec\"><div class=\"pad_10\"><div class=\"clrWhite line-ht-5\">Currently We are not available in your area..!!</div></div></div>";
					return;
				}
			}
		}
		catch (Exception ex)
		{
			shopStr = "<div class=\"themeBgSec\"><div class=\"pad_10\"><div class=\"clrWhite line-ht-5\">" + ex.Message.ToString() + "</div></div></div>";
			return;
		}
	}

	protected void btnSearch_Click(object sender, EventArgs e)
	{
		try
		{
			string strQuery = "";
			string pincode = "";
			txtPinCode.Text = txtPinCode.Text.Trim().Replace("'", "");

			if (Request.Cookies["userLoc"] != null)
			{
				citySate = Request.Cookies["userLoc"].Value.ToString();
				if (citySate != "0")
				{
					//string[] arrLocation = citySate.Split('#');
					//pincode = arrLocation[6].ToString();
					//zipcode = pincode.ToString();
					//strQuery = "Select FranchID, FranchShopCode, FranchName, FranchAddress, FranchMobile From FranchiseeData Where FranchActive=1 AND SUBSTRING(FranchShopCode,1,4)<>'GMDR' AND SUBSTRING(FranchShopCode,1,4)<>'GMOS' AND FranchPinCode='" + pincode + "'";

					strQuery = "Select FranchID, FranchShopCode, FranchName, FranchAddress, FranchMobile From FranchiseeData Where FranchActive=1 AND FranchLegalBlock=0 AND SUBSTRING(FranchShopCode,1,4)<>'GMDR' AND SUBSTRING(FranchShopCode,1,4)<>'GMOS'";
				}
				else
				{
					strQuery = "Select FranchID, FranchShopCode, FranchName, FranchAddress, FranchMobile From FranchiseeData Where FranchActive=1 AND FranchLegalBlock=0 AND SUBSTRING(FranchShopCode,1,4)<>'GMDR' AND SUBSTRING(FranchShopCode,1,4)<>'GMOS'";
				}
			}
			else
			{
				strQuery = "Select FranchID, FranchShopCode, FranchName, FranchAddress, FranchMobile From FranchiseeData Where FranchActive=1 AND FranchLegalBlock=0 AND SUBSTRING(FranchShopCode,1,4)<>'GMDR' AND SUBSTRING(FranchShopCode,1,4)<>'GMOS'";
			}

			if (txtPinCode.Text == "" && txtShopName.Text == "")
			{
				GetShops(strQuery);
				ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Select atleast one field to proceed');", true);
				return;
			}
			

			if (txtPinCode.Text != "")
			{
				if (!c.IsNumeric(txtPinCode.Text))
				{
					GetShops(strQuery);
					ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('warning', 'Pincode must be numeric value');", true);
					return;
				}

				if (txtShopName.Text != "")
				{
					//strQuery = "Select FranchID, FranchShopCode, FranchName, FranchAddress, FranchMobile From FranchiseeData Where FranchActive=1 AND SUBSTRING(FranchShopCode,1,4)<>'GMDR' AND SUBSTRING(FranchShopCode,1,4)<>'GMOS' AND FranchID='" + Session["shopId"].ToString() + "'";
					strQuery = "Select FranchID, FranchShopCode, FranchName, FranchAddress, FranchMobile From FranchiseeData Where FranchActive=1 AND FranchLegalBlock=0 AND SUBSTRING(FranchShopCode,1,4)<>'GMDR' AND SUBSTRING(FranchShopCode,1,4)<>'GMOS' AND FranchPinCode='" + txtPinCode.Text + "' AND FranchName LIKE '" + txtShopName.Text + "%'";
				}
				else
				{
					strQuery = "Select FranchID, FranchShopCode, FranchName, FranchAddress, FranchMobile From FranchiseeData Where FranchActive=1 AND FranchLegalBlock=0 AND SUBSTRING(FranchShopCode,1,4)<>'GMDR' AND SUBSTRING(FranchShopCode,1,4)<>'GMOS' AND FranchPinCode='" + txtPinCode.Text + "'";
				}
			}
			else
			{
				if (txtShopName.Text != "")
				{
					//strQuery = "Select FranchID, FranchShopCode, FranchName, FranchAddress, FranchMobile From FranchiseeData Where FranchActive=1 AND SUBSTRING(FranchShopCode,1,4)<>'GMDR' AND SUBSTRING(FranchShopCode,1,4)<>'GMOS' AND FranchID='" + Session["shopId"].ToString() + "'";
					strQuery = "Select FranchID, FranchShopCode, FranchName, FranchAddress, FranchMobile From FranchiseeData Where FranchActive=1 AND FranchLegalBlock=0 AND SUBSTRING(FranchShopCode,1,4)<>'GMDR' AND SUBSTRING(FranchShopCode,1,4)<>'GMOS' AND FranchName='" + txtShopName.Text + "'";
				}
			}

			GetShops(strQuery);
		}
		catch (Exception ex)
		{
			ScriptManager.RegisterClientScriptBlock(this, GetType(), "myScript", "TostTrigger('error', 'Error Occoured While Processing');", true);
			c.ErrorLogHandler(this.ToString(), "btnSearch_Click", ex.Message.ToString());
			return;
		}
	}


	[WebMethod]
	public static List<string> GetShopInfo(string shopName)
	{
		iClass c = new iClass();
		List<string> shops = new List<string>();
		using (DataTable dtShops = c.GetDataTable("Select FranchName, FranchID From FranchiseeData Where FranchActive=1 AND FranchLegalBlock=0 AND FranchName LIKE'" + shopName + "%' Order By FranchName"))
		{
			if (dtShops.Rows.Count > 0)
			{
				foreach (DataRow row in dtShops.Rows)
				{
					shops.Add(string.Format("{0}#{1}", row["FranchName"], row["FranchID"]));
				}
			}
			else
			{
				shops.Add("Match not found");
			}

			return shops;
		}
	}

	[WebMethod]
	public static string GetShopId(string shopId)
	{
		//return "intellect" + medId.ToString();

		HttpContext.Current.Session["shopId"] = shopId;

		return shopId;
	}
}