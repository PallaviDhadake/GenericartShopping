<%@ Page Title="Contact Us | Genericart Online Generic Medicine Shopping" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="contact-us.aspx.cs" Inherits="contact_us" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script type="text/javascript">
		var onloadCallback = function () {
			grecaptcha.render('recaptcha', {
				'sitekey': '6LcNBIUUAAAAADbX-_n6UhdJhtAxQDgiypcyZqSN'
			});
		};
	</script>
	<script type="text/javascript"
			src="https://maps.googleapis.com/maps/api/js?key=AIzaSyCvO0AHfg1cuND1KXbw3t5xZr5p4PVrEk4">
	</script>
	<script type="text/javascript">
		function initialize() {

			var myLatlng = new google.maps.LatLng(16.834294, 74.648223);

			var mapOptions = {
				center: myLatlng,
				zoom: 17, scrollwheel: false, draggable: true,
			};

			var map = new google.maps.Map(document.getElementById("map-canvas"), mapOptions);
			var genericart = {
				path: 'M95.35,50.645c0,13.98-11.389,25.322-25.438,25.322c-14.051,0-25.438-11.342-25.438-25.322   c0-13.984,11.389-25.322,25.438-25.322C83.964,25.322,95.35,36.66,95.35,50.645 M121.743,50.645C121.743,22.674,98.966,0,70.866,0   C42.768,0,19.989,22.674,19.989,50.645c0,12.298,4.408,23.574,11.733,32.345l39.188,56.283l39.761-57.104   c1.428-1.779,2.736-3.654,3.916-5.625l0.402-0.574h-0.066C119.253,68.516,121.743,59.874,121.743,50.645',
				fillColor: '#00afef',
				fillOpacity: 1,
				scale: 0.3
			};
			var marker = new google.maps.Marker({
				position: myLatlng,
				icon: genericart,
				map: map,
				title: "Genericart Medicine",
				animation: google.maps.Animation.DROP
			});
			//alert("test");
			marker.addListener('click', toggleBounce);
			function toggleBounce() {
				if (marker.getAnimation() !== null) {
					marker.setAnimation(null);
				} else {
					marker.setAnimation(google.maps.Animation.BOUNCE);
				}
			}
		}
		google.maps.event.addDomListener(window, 'load', initialize);
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<span class="space40"></span>
	<div id="map-canvas"></div>
	<span class="space40"></span>
	<div class="col_1140">
		<div class="col_800 bgWhite border_r_5 pad_15 box-shadow">
			<asp:UpdatePanel ID="UpdatePanel1" runat="server" OnLoad="UpdatePanel1_Load">
				<ContentTemplate>
					<h2 class="pageH2 themeClrPrime mrg_B_10">Get in touch with Us</h2>
					<div class="w100 mrg_B_15">
						<span class="labelCap">Name :*</span>
						<asp:TextBox ID="txtName" CssClass="textBox w95" MaxLength="50" placeholder="Full Name (Surname first)" runat="server"></asp:TextBox>
					</div>
					<div class="w50 float_left mrg_B_15">
						<div class="app_r_padding">
							<span class="labelCap">Email Id :*</span>
							<asp:TextBox ID="txtEmail" CssClass="textBox w95" MaxLength="30" placeholder="Your Email Address" runat="server"></asp:TextBox>
						</div>
					</div>
					<div class="w50 float_left mrg_B_15">
						<div class="app_r_padding" >
							<span class="labelCap">Mobile No :*</span>
							<asp:TextBox ID="txtMobile" CssClass="textBox w95" MaxLength="10" placeholder="Without Country Code" runat="server"></asp:TextBox>
						</div>
					</div>
					<div class="float_clear"></div>
					<div class="w100 mrg_B_15" >
						<span class="labelCap">Message :*</span>
						<asp:TextBox ID="txtMsg" CssClass="textBox w95" TextMode="MultiLine" Height="150" runat="server"></asp:TextBox>
					</div>
					<div class="w100">
						<div id="recaptcha" style="-moz-transform:scale(0.77); -ms-transform:scale(0.77); -o-transform:scale(0.77);-webkit-transform:scale(0.77); transform:scale(0.77); -webkit-transform-origin:0 0; -moz-transform-origin:0 0; -ms-transform-origin:0 0; -o-transform-origin:0 0; transform-origin:0 0;"></div>
						<script src="https://www.google.com/recaptcha/api.js?onload=onloadCallback&render=explicit" async defer></script>
					</div>
					<asp:Button ID="btnSubmit" runat="server" CssClass="buttonForm" Text="SEND" OnClick="btnSubmit_Click" />
				</ContentTemplate>
			</asp:UpdatePanel>
		</div>
		<div class="float_clear"></div>
	</div>
</asp:Content>

