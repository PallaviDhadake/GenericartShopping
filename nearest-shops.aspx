<%@ Page Title="Nearest Shops Around Me | Genericart Online Generic Medicine Shopping" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="nearest-shops.aspx.cs" Inherits="nearest_shops" %>
<%@ MasterType VirtualPath="~/MasterParent.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<style>
		.shopImg{width:120px; height:120px; float:left;}
		.shopImg img{border-radius:50%; width:100%;}
		.shopInfo{margin-left:140px;}
	</style>

	<script type="text/javascript">
		$(document).ready(function () {
			SearchText();
		});
		function SearchText() {
			PageMethods.set_path('/nearest-shops.aspx');
			$("#<%= txtShopName.ClientID %>").autocomplete({
				source: function (request, response) {
					$.ajax({
						type: "POST",
						contentType: "application/json; charset=utf-8",
						url: "nearest-shops.aspx/GetShopInfo",
						data: "{'shopName':'" + document.getElementById('<%= txtShopName.ClientID %>').value + "'}",
						dataType: "json",
						success: function (data) {
							//alert(data.d);
							response($.map(data.d, function (item) {
								return {
									label: item.split('#')[0],
									val: item.split('#')[1]
								}
							}))
						},
						error: function (result) {
							alert("No Match");
						}
					});
				},
				select: function (e, i) {
					$("#<%= txtShopId.ClientID %>").val(i.item.val);

					PageMethods.GetShopId(i.item.val, onSucess, onError);
					function onSucess(result) {
						//alert(result);
					}

					function onError(result) {
						alert(result.get_message());
					}
				},
				minLength: 1
			});
		}
	</script>
	<script type="text/javascript">
		$(document).ready(function () {
			ManageFavShop();
		});

		function ManageFavShop() {
			var appLink = getElementArray("a", "fShop");
			if (appLink.length > 0) {
				for (i = 0; i < appLink.length; i++) {
					appLink[i].onclick = function ()  // Assign click event
					{
						//alert("click on like success");
					    var strUrl = this.href.split('/');  //Get anchor href(Virtual Url)
					    var franchCust = strUrl[strUrl.length - 1].split('-');
					    var frId = franchCust[0];
					    var custId = franchCust[1];

						var sessionVal = '<%=Session["genericCust"] != null%>';
						//alert(sessionVal);
						if (sessionVal == 'False') {
							
						    var rdrUrl = "<%= Master.rootPath + "login?ref=favShop&shop=" %>" + frId;
							//alert(rdrUrl);
							window.location.href = rdrUrl;
							return false;
						}
						else {
							

							//document.getElementById("shop-" + frId).innerHtml = "<a class=\"pAnchor\">Processing...</a>";
							ShoppingWebService.UpdateFavShop(frId, custId, function (result) {
								//alert("domain : " + window.location.host);
								//alert(result);
								var navUrl;
								if (window.location.href.indexOf("localhost") > -1) {
									navUrl = "http://" + window.location.host + "/GenericartShopping/";
								}
								else {
									//navUrl = "http://www.temp.com/";
									navUrl = "http://" + window.location.host + "/";
								}

								if (result == 0) {
									document.getElementById("shop-" + frId).innerHTML = "<a href=\"" + navUrl + "add-fav-shop/" + frId + "-" + custId + "\" title=\"Mark as favorite shop\" class=\"fShop txtDecNone\"><div class=\"markFav\" ></div></a>";
								}
								else {
								    //setTimeout(function () { if(top!=self) {top.location.href = '" + pageUrl + "';} }, 2000);
								    TostTrigger('success', 'Shop Marked as Favorite..!!') ;
								    setTimeout(function () { document.getElementById('<%= btnSearch.ClientID %>').click() }, 2000);
								    document.getElementById("shop-" + frId).innerHTML = "<div class=\"markedFav\" ></div>";
								}

								ManageFavShop();
							});
							return false;
						}
					}
				}
			}
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<span class="space40"></span>
	<div class="col_1140 border_r_5" id="shops">
		<div class="pad_TB_15 pad_LR_30">
			<div class="pad_LR_15">
				<h1 class="pageH2 clrLightBlack semiBold mrg_B_10">Nearest Shops Around Me</h1>

				<span class="regular semiBold clrLightBlack">Your Current Pincode : <%= zipcode %></span>
				<span class="space30"></span>

				<div class="width25 float_left mrg_B_15">
					<div class="app_r_padding">
						<asp:TextBox ID="txtPinCode" CssClass="searchTxtBox" placeholder="Enter Pincode" runat="server"></asp:TextBox>
					</div>
				</div>
				<div class="width50 float_left mrg_B_15">
					<div class="app_r_padding">
						<asp:TextBox ID="txtShopName" CssClass="searchTxtBox" placeholder="Enter Shop Name" runat="server"></asp:TextBox>
						<asp:TextBox ID="txtShopId" runat="server" CssClass="searchTxtBox" Enabled="false" Visible="false"></asp:TextBox>
					</div>
				</div>
				<div class="width20 float_left mrg_B_15">
					<div class="app_r_padding">
						<asp:Button ID="btnSearch" runat="server" CssClass="searchBtn semiBold upperCase letter-sp-2" Text="Search" OnClick="btnSearch_Click"  />
					</div>
				</div>
				<div class="float_clear"></div>
			</div>

			<span class="space10"></span>
			<%= shopStr %>
			<%--<div class="width50">
				<div class="pad_15">
					<div class="bgWhite box-shadow border_r_5">
						<div class="pad_15">
							<div class="shopImg">
								<img src="images/medical-shop.jpg" />
							</div>
							<div class="shopInfo">
								<h4 class="themeClrSec semiBold semiMedium mrg_B_3">Swast Aushadhi Generic Medicine Store</h4>
								<span class="colrPink semiBold">Shop Code : GMMH0001</span>
								<span class="space10"></span>
								<p class="clrGrey fontRegular line-ht-5 small mrg_B_10">Address : YAMAN-2, 1st Floor, Near New English School, Pandharpur Road, Miraj-416410</p>
								<a href="tel:+918408027474" class="conCall conIco fontRegular txtDecNone breakWord">+91 8408027474</a>
							</div>
							<div class="float_clear"></div>
						</div>
					</div>
				</div>
			</div>
			<div class="width50">
				<div class="pad_15">
					<div class="bgWhite box-shadow border_r_5">
						<div class="pad_15">
							<div class="shopImg">
								<img src="images/medical-shop.jpg" />
							</div>
							<div class="shopInfo">
								<h4 class="themeClrSec semiBold semiMedium mrg_B_3">Swast Aushadhi Generic Medicine Store</h4>
								<span class="colrPink semiBold">Shop Code : GMMH0001</span>
								<span class="space10"></span>
								<p class="clrGrey fontRegular line-ht-5 small mrg_B_10">Address : YAMAN-2, 1st Floor, Near New English School, Pandharpur Road, Miraj-416410</p>
								<a href="tel:+918408027474" class="conCall conIco fontRegular txtDecNone breakWord">+91 8408027474</a>
							</div>
							<div class="float_clear"></div>
						</div>
					</div>
				</div>
			</div>
			<div class="float_clear"></div>--%>
		</div>
	</div>
</asp:Content>

