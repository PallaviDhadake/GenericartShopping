<%@ Page Title="Customer Details" Language="C#"  MasterPageFile="~/customer/MasterCustomer.master" AutoEventWireup="true" CodeFile="user-info.aspx.cs" Inherits="customer_user_info" %>
<%@ MasterType VirtualPath="~/customer/MasterCustomer.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<!-- User info Starts -->
	<!-- Basic Info Starts -->
		<div class="width50">
			<div class="pad_10">
				<div class="bgWhite border_r_5 box-shadow">
					<div class="pad_20">
						<h2 class="clrLightBlack semiBold semiMedium">Basic Info</h2>
						<span class="space20"></span>
						<div class="userImgLarge dis-tbl" style="<%= bgColor %>">
							<%--<img src="images/user.jpg" alt="" />--%>
							<div class="unameLarge tbl-cell">
								<span class="clrWhite semiBold"><%= firstChar %></span>
							</div>
						</div>
						<div class="userNameLarge">
							<h2 class="semiMedium clrLightBlack semiBold"><%= sessionName %></h2>
							<span class="tiny clrGrey fontRegular">Joined on <%= joinDate %></span>

							<span class="space20"></span>
							
						</div>
						<div class="float_clear"></div>

						<span class="space30"></span>

						<!-- Form starts -->
						<div class="w100 mrg_B_15">
							<div class="app_r_padding">
								<span class="labelCap">Name :*</span>
								<asp:TextBox ID="txtName" runat="server" CssClass="textBox" MaxLength="50" placeholder="Enter Your Name"></asp:TextBox>
							</div>
						</div>
						<div class="w50 float_left mrg_B_15">
							<div class="app_r_padding">
								<span class="labelCap">Email :*</span>
								<asp:TextBox ID="txtEmail" runat="server" CssClass="textBox w95" MaxLength="50" placeholder="Your email address"></asp:TextBox>
							</div>
						</div>
						<div class="w50 float_left mrg_B_15">
							<div class="app_r_padding">
								<span class="labelCap">Mobile No. :*</span>
								<asp:TextBox ID="txtMobile" runat="server" CssClass="textBox w95" MaxLength="10" placeholder="Mobile No. without country code"></asp:TextBox>
							</div>
						</div>
						<div class="float_clear"></div>
						<div class="w50 float_left mrg_B_15">
							<div class="app_r_padding">
								<span class="labelCap">Date Of Birth :*</span>
								<asp:TextBox ID="txtDOB" runat="server" CssClass="textBox w95" placeholder="Click here to Open Calendar"></asp:TextBox>
							</div>
						</div>
						<div class="w50 float_left mrg_B_15">
							<div class="app_r_padding">
								<span class="labelCap">Age :*</span>
								<asp:TextBox ID="txtAge" runat="server" CssClass="textBox w95" placeholder="your age"></asp:TextBox>
							</div>
						</div>
						<div class="float_clear"></div>
						<span class="space15"></span>
						<asp:Button ID="btnSubmit" runat="server" Text="Update Info" CssClass="pinkAnch small semiBold dspInlineBlk" OnClick="btnSubmit_Click" />
						<!-- Form ends -->

						<!-- Fav Shop Display Starts -->
						<div id="favShop" runat="server" visible="false">
							<span class="space30"></span>
							<h3 class="medium themeClrPrime semiBold mrg_B_10">Your Favourite Shop</h3>
							<div class="bgLightGrey border_r_3">
								<div class="pad_15">
									<div class="favShopImg">
										<img src="../images/icons/shop.png" class="width100" />
									</div>
									<div class="favShop"><%= arrFavShop[0] %></div>
									<div class="float_clear"></div>
									<span class="space10"></span>
									<p class="small clrDarkGrey line-ht-5"><%= arrFavShop[1] %></p>
									<span class="space20"></span>
									<a href="tel:<%= arrFavShop[2] %>" class="callShop">Call : <%= arrFavShop[2] %></a>
								</div>
							</div>
						</div>
						<!-- Fav Shop Display Ends -->
					</div>
				</div>
			</div>
		</div>
		<!-- Basic Info Ends -->
	<!-- User info Ends -->

	<script type="text/javascript">

		window.onload = function () {
			//alert("window load");

			duDatepicker('#<%= txtDOB.ClientID %>', {
				auto: true, inline: true, format: 'dd/mm/yyyy',
				events: {
					dateChanged: function (data) {
						getAge();
					},
				}
			});
		}
	</script>

	<script type="text/javascript" >

		function getAge() {
			var dateString = document.getElementById("<%= txtDOB.ClientID%>").value;
			var arr = dateString.split('/');
			dateString = arr[1] + "/" + arr[0] + "/" + arr[2];
			var today = new Date();
			var birthDate = new Date(dateString);
			var age = today.getFullYear() - birthDate.getFullYear();
			var m = today.getMonth() - birthDate.getMonth();
			if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
				age--;
			}
			document.getElementById("<%= txtAge.ClientID %>").value = age;
		}
		function alpha(e) {
			//alert("Alpha Called");
			var k;
			document.all ? k = e.keyCode : k = e.which;
			return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57));
		}
	</script>
</asp:Content>

