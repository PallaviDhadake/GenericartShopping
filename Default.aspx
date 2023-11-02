<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<!-- Google Tag Manager -->
	<script>(function (w, d, s, l, i) {
	w[l] = w[l] || []; w[l].push({
		'gtm.start':
		new Date().getTime(), event: 'gtm.js'
	}); var f = d.getElementsByTagName(s)[0],
	j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true; j.src =
	'https://www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f);
})(window, document, 'script', 'dataLayer', 'GTM-T56SD66');</script>
	<!-- End Google Tag Manager -->

	<meta content="IE=edge" http-equiv="X-UA-Compatible" />
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
	<link rel="shortcut icon" href="/favicon.ico" type="image/x-icon" />

	<title>Genericart Online Generic Medicine Shopping</title>

	<meta name="description" content="" />

	<link href="https://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700" rel="stylesheet" />

	<!-- Global site tag (gtag.js) - Google Analytics -->
	<script async src="https://www.googletagmanager.com/gtag/js?id=UA-226104277-1"></script>
	<script>
		window.dataLayer = window.dataLayer || [];
		function gtag() { dataLayer.push(arguments); }
		gtag('js', new Date());

		gtag('config', 'UA-226104277-1');
	</script>

	<link href="css/shoppingGencart.css" rel="stylesheet" />
	<link href="css/jquery.bxslider.css" rel="stylesheet" />
	<script src="js/jquery-2.2.4.min.js"></script>
	<script src="js/jquery.bxslider.min.js"></script>
	<script src="js/slick.js"></script>
	<script src="js/multislider.js"></script>
	<link href="css/toastr.css" rel="stylesheet" />
	<script src="js/toastr.js"></script>
	<script type="text/javascript">
		function TostTrigger(EventName, MsgText) {
			// code to be executed
			Command: toastr[EventName](MsgText)
			toastr.options = {
				"closeButton": true,
				"debug": false,
				"newestOnTop": false,
				"progressBar": false,
				"positionClass": "toast-top-full-width",
				"preventDuplicates": false,
				"onclick": null,
				"showDuration": "300",
				"hideDuration": "1000",
				"timeOut": "5000",
				"extendedTimeOut": "1000",
				"showEasing": "swing",
				"hideEasing": "linear",
				"showMethod": "slideDown",
				"hideMethod": "fadeOut"
			}

		}
	</script>

	<meta name="google-site-verification" content="cuF5bcjXPVniE4WEgun9DxjZkEt6ulQyXIhYkTDzpkg" />
</head>
<body class="bodyBg">

	<!-- Google Tag Manager (noscript) -->
	<noscript><iframe src="https://www.googletagmanager.com/ns.html?id=GTM-T56SD66" height="0" width="0" style="display:none;visibility:hidden"></iframe></noscript>
	<!-- End Google Tag Manager (noscript) -->

	<%--<a href="https://ecommerce.genericartmedicine.com/generic-mitra-info" class="genMitaAnch semiBold letter-sp-2">Join as Generic Mitra</a>--%>
	<form id="form1" runat="server">
		<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
	<!-- Header Starts -->
		<div id="header" class="width100">
			<%= errMsg %>
			<%= shopExist %>
			<%--<a href="http://genericartmedicine.com/business-opportunity" target="_blank"><div class="absBiz clrWhite upperCase semiBold letter-sp-2">Business Opportunity</div></a>--%>
			<!-- Top Grey Strip Starts -->
			<div class="width100 bgLightGrey" id="topStrip">
				<div class="pad_5">
					<div class="col_1140">
						<div class="float_left">
							<span class="small clrBlack fontRegular dispBlk">For Medicine Purchase</span>
							<a href="tel:9090308585" class="themeClrPrime txtDecNone semiBold small">9090308585</a>
						</div>
						<div class="float_right">
							<ul class="iconList">
								<li><a href="<%= custLink %>" class="userIcon tooltip"><span class="tooltiptext"><%= custTitle %></span></a></li>
								<%--<li><a href="#" class="notificationIcon" title="Notifications"></a></li>--%>
								<li><a href="nearest-shops" class="medicalIcon tooltip"><span class="tooltiptext">Nearest Shops</span></a></li>
								<li><a href="cart" class="cartIcon" title="My Medicines"><div class="absCartItems dis-tbl"><span class="tbl-cell" id="mobCartCount"><%= CartCount %></span></div></a></li>
								<%--<li style="margin-left: -15px;"><a href="Cart.aspx" style="font-weight: 800">(<%= CartCount %>)</a></li>--%>
							</ul>
						</div>
						<div class="float_clear"></div>
					</div>
				</div>
			</div>
			<!-- Top Grey Strip Ends -->
			<div class="pad_TB_20">
				<div class="col_1140 posRelative">
					<div class="logo">
						<a href="<%= rootPath %>" title="Genericart Online Generic Medicine Shopping" class="txtDecNone">
							<img src="images/genericart-logo.png" alt="Genericart Online Generic Medicine Shopping" />
						</a>
						<div class="desktopInfo">
							<span class="space20"></span>
							<span class="small clrBlack fontRegular dispBlk">Toll Free Number</span>
							<a href="tel:9090308585" class="themeClrPrime txtDecNone semiBold small">9090308585</a>
						</div>
					</div>
					<div class="mob_clear"></div>

					<%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
						<ContentTemplate>--%>
							<div class="searchContainer">
								<%--<div class="width20">
									<span class="small clrGrey fontRegular dispBlk semiBold" style="padding-left: 5px;">Deliver To</span>
									<asp:TextBox ID="txtCity" runat="server" CssClass="srchCmbBox" />
								</div>--%>
								<div class="width80">
									<asp:TextBox ID="TxtSearch" runat="server" CssClass="searchTxtBox" placeholder="Search with medicine content" />
								</div>
								<div class="width20 minusMargin">
									<asp:Button ID="BtnSearch" runat="server" CssClass="searchBtn semiBold upperCase letter-sp-2" OnClick="BtnSearch_Click" Text="Search" />
								</div>
								<div class="float_clear"></div>
							</div>
						<%--</ContentTemplate>
					</asp:UpdatePanel>--%>

					<div class="float_right desktopInfo">
						<ul class="iconList">
							<li><a href="<%= custLink %>" class="userIcon tooltip"><span class="tooltiptext"><%= custTitle %></span></a></li>
							<%--<li><a href="#" class="notificationIcon" title="Notifications"></a></li>--%>
							<li><a href="nearest-shops" class="medicalIcon tooltip"><span class="tooltiptext">Nearest Shops</span></a></li>
							<li><a href="cart" class="cartIcon" title="My Medicines"><div class="absCartItems dis-tbl"><span class="tbl-cell" id="cartCount"><%= CartCount %></span></div></a></li>
							<%--<li style="margin-left: -15px;"><a href="Cart.aspx" style="font-weight: 800">(<%= CartCount %>)</a></li>--%>
						</ul>
					</div>
					<div class="float_clear"></div>

					<%--<div class="absOSIcons desktopInfo">
						<a href="#" class="mrg_R_5" title="Download our app from Google Playstore"><img src="images/icons/android.png" class="android" /></a>
						<a href="#" class="" title="Download On App Store"><img src="images/icons/ios.png" class="ios" /></a>
					</div>--%>

					<!-- Navigation Starts -->
					<div id="nav">
						<div id="topNavPanel">
							<div class="col_1140">
								<span class="space10 mobNavLogo"></span>
								<ul id="topNav">
									<a href="javascript:void(0)" class="closeBtn" onclick="closeNav()">&times;</a>
									<li>
										<a href="<%= rootPath + "categories/medicine-7/1" %>">
											<div class="navIcon">
												<img src="images/icons/medicines.png" class="blackIcon width100" />
											</div>
											<div class="navIconInfo"><p class="semiBold">Medicines</p> <span>Search Medicines</span></div>
											<div class="float_clear"></div>
										</a>
									</li>
									<li>
										<%--<a href="https://docs.google.com/forms/d/e/1FAIpQLSda8d0lx5CErdNQ17ZHdDBXGv90eovqWZUHXpu_RoQ66_Exbg/viewform" target="_blank">--%>
										<%--<a href="consult-doctor" target="_blank">--%>
										<a href="book-appointment" target="_blank">
											<div class="navIcon">
												<img src="images/icons/doctors.png" class="blackIcon width100" />
											</div>
											<div class="navIconInfo"><p class="semiBold">Doctors</p><span>Consult Doctor</span></div>
											<div class="float_clear"></div>
										</a>
									</li>
									<%--<li>
										<a href="book-lab-test">
											<div class="navIcon">
												<img src="images/icons/labtest.png" class="blackIcon width100" />
												<img src="images/icons/labtest-blue.png" class="blueIcon" />
											</div>
											<div class="navIconInfo"><p class="semiBold">Lab</p><span>Book Lab Test</span></div>
											<div class="float_clear"></div>
										</a>
									</li>--%>
									<li>
										<a href="upload-prescription">
											<div class="navIcon">
												<img src="images/icons/prescription.png" class="blackIcon width100" />
											</div>
											<div class="navIconInfo"><p class="semiBold">Prescription</p><span>Upload Prescription</span></div>
											<div class="float_clear"></div>
										</a>
									</li>
									<li>
										<a href="saving-calculator">
											<div class="navIcon">
												<img src="images/icons/calculator.png" class="blackIcon width100" />
											</div>
											<div class="navIconInfo"><p class="semiBold blink_me">Know Your Savings</p><span>Saving Calculator</span></div>
											<div class="float_clear"></div>
										</a>
									</li>
                                    <li>
										<a href="http://genericartmedicine.com/business-opportunity">
											<div class="navIcon">
												<img src="images/icons/business-ico.png" class="blackIcon width100" />
											</div>
											<div class="navIconInfo"><p class="semiBold">Business</p><span>Opportunity</span></div>
											<div class="float_clear"></div>
										</a>
									</li>
								</ul>
								<div class="float_clear"></div>
								<div id="mobNav">
									<span class="space10"></span>
									<div class="pad_30">
										<a href="https://play.google.com/store/apps/details?id=com.autoqed.genericartmedicine" target="_blank" class="mrg_R_5" title="Download our app from Google Playstore"><img src="images/icons/android.png" class="android" /></a>
										<a href="#" class="tooltip" title="Download On App Store"><span class="tooltiptext">Coming Soon !</span><img src="images/icons/ios.png" class="ios" /></a>
										<span class="space20"></span>
										<span class="tiny upperCase clrWhite letter-sp-3 mrg_B_15">Phone:</span>
										<a href="tel:1800120061313" class="medium clrWhite light txtDecNone">1800 1200 61313</a>
										<span class="space30"></span>
										<span class="tiny upperCase clrWhite letter-sp-3 mrg_B_15">Email:</span>
										<a href="mailto:enquirygenericart@gmail.com" class="clrWhite breakWord txtDecNone">enquirygenericart&#64;gmail&#46;com</a>
									</div>
								</div>
							</div>
						</div>
						<div class="float_clear"></div>
						<a id="navBtn" onclick="openNav()"></a>
					</div>
					 <!-- Navigation Ends -->
				</div>
			</div>
		</div>
		<!-- Header Ends -->
		<span class="fixedSpacer"></span>
		<!-- Banner Starts -->
		
		<span class="space50"></span>
		<div class="col_1140 posRelative">
			<div class="bxslider">
				<%= bannerStr %>
			   <%-- <div>
					<img src="images/banner/banner-1.jpg" alt="" class="width100" />
				</div>
				<div>
					<img src="images/banner/banner-2.jpg" alt="" class="width100" />
				</div>
				<div>
					<img src="images/banner/banner-3.jpg" alt="" class="width100" />
				</div>
				<div>
					<img src="images/banner/banner-4.jpg" alt="" class="width100" />
				</div>--%>
			</div>
		</div>
		<span class="space50"></span>
		<!-- Banner Ends -->
		<!-- Product Cat Starts -->
		<div class="col_1280" id="cat">
			<div class="col_1140">
				<h2 class="clrLightBlack semiBold semi-large mrg_B_10">Health Products</h2>
			</div>
			<div id="catSlider">
				<div class="MS-content">
					<%= catStr %>
					
					<%--<div class="item">
						<div class="prodContainer txtCenter">
							<div class="bgLightYellow top-right-border">
								<div class="pad_30">
									<img src="images/products/product-1.png" class="width100" />
								</div>
							</div>
							<div class="pad_10">
								<a href="#" class="prodName semiBold clrBlack mrg_B_5 dispBlk">Skin Care</a>
								<span class="small fontRegular">&#8377; 300 Onwards</span>
							</div>
						</div>
					</div>--%>
				</div>
				<div class="MS-controls">
					<a class="MS-left"><img src="images/controls-prev.png" /></a>
					<a class="MS-right"><img src="images/controls.png" /></a>
				</div>
			</div>
				
		</div>
		<span class="space50"></span>
		<!-- Product Cat Ends -->
		<!-- Diabetic Medicine Products Starts -->
		<div class="col_1280">
			<div class="col_1140">
				<h2 class="clrLightBlack semiBold semi-large mrg_B_10">Medicine Products</h2>
				<span class="space20"></span>
				<div class="col_1_2">
					<div class="pad_15">
						<a href="categories/diabetic-15/1" title="Best Generic Medicine for Diabetes Disease"><img src="images/diatetic-medicine-products.jpg" alt="Best Generic Medicine for Diabetes disease" class="width100" /></a>
					</div>
				</div>
				 <div class="col_1_2">
					<div class="pad_15">
						<a href="categories/cardiac-17/1" title="Best Generic Medicine for Cardiac Disease"><img src="images/cardiac-medicine-products.jpg" alt="Best Generic Medicine for Blood Pressure disease" class="width100" /></a>
					</div>
				</div>
				<div class="float_clear"></div>
			</div>

			<!-- Removed Diabetic Product slidser =========
			<div id="prodSlider">
				<div class="MS-content">
					<%= prodstr %>
					
					<%--<div class="item">
						<div class="prodContainer">
							<div class="pad_15">
								<div class="txtCenter"><img src="images/medicine-product.jpg" alt="" /></div>
								<span class="prodLine"></span>
								<span class="space15"></span>
								<a href="product-details.html" class="prodName semiBold">CROCIN COLD & FLU</a>
								<span class="space10"></span>
								<p class="clrGrey line-ht-5 tiny mrg_B_15">Lorem Ipsum is simply dummy text</p>
								<span class="prod-offer-price">&#8377; 359</span>
								<span class="prod-price">&#8377; 599</span>
								<span class="prod-discount">40% Off</span>
							</div>
						</div>
					</div>--%>
				</div>
				<div class="MS-controls">
					<a class="MS-left"><img src="images/controls-prev.png" /></a>
					<a class="MS-right"><img src="images/controls.png" /></a>
				</div>
			</div>
		   ====================  -->

		</div>
		<%--<span class="space50"></span>--%>
		<!-- Diabetic Medicine Products Ends -->
		<!-- Cardiac Medicine Products Starts ===========
		<div class="col_1280">
			<div class="col_1140">
				<h2 class="clrLightBlack semiBold semi-large mrg_B_10">Cardiac Medicine Products</h2>
			</div>
			<div id="cardiacSlider">
				<div class="MS-content">
					<%= cardiacStr %>
				</div>
				<div class="MS-controls">
					<a class="MS-left"><img src="images/controls-prev.png" /></a>
					<a class="MS-right"><img src="images/controls.png" /></a>
				</div>
			</div>
		</div>
		============== -->

		<span class="space50"></span>
		<!-- Cardiac Medicine Products Ends -->
		<!-- Search by concern Starts -->
		<div class="col_1280">
			<div class="col_1140">
				<h3 class="clrLightBlack semiBold semi-large mrg_B_10">Search By Concern</h3>
			</div>
			<div id="disSlider">
				<div class="MS-content">
					<%= diseaseStr %>
					
					<%--<div class="item">
						<div class="prodContainer txtCenter">
						<div class="pad_15">
							<img src="images/icons/corona.png" alt="" />
							<a href="#" class="prodName semiBold">Corona</a>
						</div>
					</div>
					</div>--%>
				</div>
				<div class="MS-controls">
					<a class="MS-left"><img src="images/controls-prev.png" /></a>
					<a class="MS-right"><img src="images/controls.png" /></a>
				</div>
			</div>
		</div>
		<span class="space50"></span>
		<!-- Search by concern Ends -->
		<!-- Services Starts -->
		<div class="col_1140" id="genServ">
			<h3 class="clrLightBlack semiBold txtCenter semi-large mrg_B_10">Other Services</h3>
			<div class="col_800_center posRelative">
				<div class="pad_R_15">
					<a href="consult-doctor" class="txtDecNone">
						<div class="bgPink border_r_5">
							<div class="width50">
								<div class="pad_50">
									<div class="ribbon"><div class="pad_15 txtCenter semiBold colrPink"></div></div>
									<span class="space80"></span>
									<h3 class="clrWhite semiBold semi-large">Doctor Appointment</h3>
								</div>
							</div>
							<div class="width50">
								<span class="space80"></span>
								<img src="images/doctors-appoinment.png" alt="" class="width100" />
							</div>
							<div class="float_clear"></div>
						</div>
					</a>
				</div>
			</div>
			<%--<div class="col_1_2 posRelative">
				<div class="pad_L_15">
					<a href="book-lab-test" class="txtDecNone">
						<div class="themeBgPrime border_r_5">
							<div class="width50">
								<div class="ribbon"><div class="pad_15 txtCenter semiBold themeClrPrime"></div></div>
								<div class="pad_50">
									<span class="space80"></span>
									<h3 class="clrWhite semiBold semi-large">Order Lab Test</h3>
								</div>
							</div>
							<div class="width50">
								<span class="space80"></span>
								<img src="images/lab-test.png" alt="" class="width100" />
							</div>
							<div class="float_clear"></div>
						</div>
					</a>
				</div>
			</div>--%>
			<div class="float_clear"></div>
		</div>
		<span class="space50"></span>
		<!-- Services Starts -->
		<!-- Our Thoughts Starts -->
		<div class="col_1280">
			<div class="col_1140">
				<h2 class="clrLightBlack semiBold semi-large mrg_B_20 txtCenter">Our Thoughts</h2>
			</div>
			<div id="blogSlider">
				<div class="MS-content">
					<%= blogsStr %>
					
					<%--<div class="item">
						<div class="prodContainer">
						<img src="images/blogs/thumb/blog-1.jpg" alt="" class="width100 top-right-border" />
						<div class="pad_15">
							<a href="blog-details.html" class="blogName semiBold dispBlk mrg_B_10">Lorem ipsum dolor sit amet</a>
							<p class="small fontRegular clrGrey line-ht-5 mrg_B_10">Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut</p>
							<a href="blog-details.html" class="readMore" style="font-size:0.9em !important;">Continue Reading</a>
						</div>
					</div>
					</div>--%>
				</div>
				<div class="MS-controls">
					<a class="MS-left"><img src="images/controls-prev.png" /></a>
					<a class="MS-right"><img src="images/controls.png" /></a>
				</div>
			</div>
			
		</div>
		<span class="space50"></span>
		<!-- Our Thoughts Ends -->
		<!-- Credibility Starts -->
		<div class="themeBgPrime">
			<span class="space50"></span>
			<div class="col_1140 txtCenter">
				<h3 class="clrWhite semiBold semi-large mrg_B_15">Our Credibility</h3>
				<div class="col_1_4">
					<div class="pad_10">
						<div class="greyBox">
							<div class="pad_40">
								<span class="themeClrPrime large semiBold">10</span><span class="themeClrPrime semiMedium semiBold"> Years</span><br />
								<span class="themeClrPrime semiBold medium breakWord">Establishment</span>
							</div>
						</div>
					</div>
				</div>
				<div class="col_1_4">
					<div class="pad_10">
						<div class="greyBox">
							<div class="pad_40">
								<span class="themeClrPrime large semiBold"><%= arrStats[0] %></span><span class="themeClrPrime semiMedium semiBold"> States</span><br />
								<span class="themeClrPrime semiBold medium breakWord">Present</span>
							</div>
						</div>
					</div>
				</div>
				<div class="col_1_4">
					<div class="pad_10">
						<div class="greyBox">
							<div class="pad_40">
								<span class="themeClrPrime large semiBold"><%= arrStats[1]  %></span><br />
								<span class="themeClrPrime semiBold medium breakWord">Branches</span>
							</div>
						</div>
					</div>
				</div>
				<div class="col_1_4">
					<div class="pad_10">
						<div class="greyBox">
							<div class="pad_40">
								<span class="themeClrPrime large semiBold"><%= arrStats[2]  %>+ lakh</span><br />
								<span class="themeClrPrime semiBold medium breakWord">Customers</span>
							</div>
						</div>
					</div>
				</div>
				<div class="float_clear"></div>
			</div>
			<span class="space50"></span>
		</div>
		<!-- Credibility Ends -->
		<!-- Download App Starts -->
		<span class="space50"></span>
		<div class="col_1140">
			<h3 class="clrLightBlack semiBold pageH3 txtCenter">Download App Now</h3>
			<span class="space50"></span>

			<div class="col_1_2 txtCenter">
				<img src="images/app-mock-image.png" />
			</div>
			<div class="col_1_2">
				<div class="width50">
					<div class="pad_20">
						<div class="iconBox txtCenter">
							<img src="images/icons/vision.png" />
						</div>
						<span class="space20"></span>
						<h4 class="semiMedium clrLightBlack semiBold mrg_B_10">Our Vision</h4>
						<p class="paraTxt small">Our Vision is to be leading national chain of drugstore offering quality and affordable generic medicine with superior customer service</p>
					</div>
				</div>
				<div class="width50">
					<div class="pad_20">
						<div class="iconBox txtCenter">
							<img src="images/icons/mission.png" />
						</div>
						<span class="space20"></span>
						<h4 class="semiMedium clrLightBlack semiBold mrg_B_10">Our Mission</h4>
						<p class="paraTxt small">Our mission is to open 25000+ stores across India.</p>
					</div>
				</div>
				<div class="float_clear"></div>

				<span class="space10"></span>

				<div class="width50">
					<div class="pad_20">
						<div class="iconBox txtCenter">
							<img src="images/icons/values.png" />
						</div>
						<span class="space20"></span>
						<h4 class="semiMedium clrLightBlack semiBold mrg_B_10">Our Values</h4>
						<p class="paraTxt small">We Commit to truth and honesty in what we say and do, and practice high standards of fairness and ethics at work and in our relationships.</p>
					</div>
				</div>
				<div class="width50">
					<div class="pad_20">
						<div class="iconBox txtCenter">
							<img src="images/icons/support.png" />
						</div>
						<span class="space20"></span>
						<h4 class="semiMedium clrLightBlack semiBold mrg_B_10">Our Support</h4>
						<p class="paraTxt small">The right to use the <span class="upperCase fontRegular">Swast Aushadhi seva generic medicine store, genericart</span> trademark and logo location and market study assistance.</p>
					</div>
				</div>
				<div class="float_clear"></div>

				<span class="space10"></span>

				<div class="width50">
					<div class="pad_20">
						<a href="#" target="_blank">
							<div class="bgBlack txtCenter border_r_5">
								<div class="pad_5">
									<img src="images/icons/apple-app-store-icon.png" alt="" />
								</div>
							</div>
						</a>
					</div>
				</div>
				<div class="width50">
					<div class="pad_20">
						<a href="https://play.google.com/store/apps/details?id=com.autoqed.genericartmedicine" target="_blank">
							<div class="bgBlack txtCenter border_r_5">
								<div class="pad_5">
									<img src="images/icons/goole-play.png" alt="" class="" />
								</div>
							</div>
						</a>
					</div>
				</div>
				<div class="float_clear"></div>
			</div>
			<div class="float_clear"></div>
		</div>
		<span class="space50"></span>
		<!-- Download App Ends -->
		<!-- Footer Starts -->
		<div class="footer posRelative">
			<%--<a href="http://genericartmedicine.com/business-opportunity" target="_blank"><div class="absBiz clrWhite upperCase semiBold letter-sp-2">Business Opportunity</div></a>--%>
			<span class="space50"></span>
			<div class="col_1140">
				<div class="col_2_3">
					<div class="width33">
						<div class="pad_15">
							<img src="images/genericart-logo-footer.png" alt="" />
							<span class="space20"></span>
							<span class="small clrBlack fontRegular dispBlk">Toll Free Number</span>
							<a href="tel:9090308585" class="themeClrPrime txtDecNone semiBold small">9090308585</a>
						</div>
					</div>
					<div class="width33">
						<div class="pad_15">
							<h4 class="footerCaption mrg_B_10 semiBold upperCase letter-sp-2">Company</h4>
							<ul class="footerNav">
								<li><a href="<%= rootPath %>">Home</a></li>
								<li><a href="about-us">About Genericart</a></li>
								<li><a href="privacy-policy">Privacy Policy</a></li>
								<li><a href="terms-and-conditions">Terms &amp; Conditions</a></li>
								<li><a href="registration">Create Account</a></li>
								<li><a href="<%= rootPath + "franchisee" %>">Shop Login</a></li>
							</ul>
						</div>
					</div>
					<div class="width33">
						<div class="pad_15">
							<h4 class="footerCaption mrg_B_10 semiBold upperCase letter-sp-2">Our Services</h4>
							<ul class="footerNav">
								<li><a href="categories/medicine-7/1">Order Medicine</a></li>
								<%--<li><a href="lab-test">Book a Lab Test</a></li>--%>
								<li><a href="upload-prescription">Upload Prescription</a></li>
								<li><a href="saving-calculator">Know Your Savings</a></li>
								<%--<li><a href="consult-doctor">Consult a Doctor</a></li>--%>
							</ul>
						</div>
					</div>
					<div class="float_clear"></div>
				</div>
				<div class="col_1_3">
					<div class="pad_15">
						<h4 class="footerCaption mrg_B_10 semiBold upperCase letter-sp-2">Subscribe To Our Newsletter</h4>
						<div class="width70">
							<input id="txtEmail" type="text" class="searchTxtBox mrg_R_15" placeholder="Your Email Address" />
						</div>
						<div class="width30">
							<input id="btnSubscribe" type="button" value="Subscribe" class="searchBtn fontRegular upperCase mrg_L_15" />
						</div>
						<div class="float_clear"></div>

						<span class="space30"></span>

						<a href="#" target="_blank" class="foo_fb socialIco" title="Follow us on facebook"></a>
						<a href="#" target="_blank" class="foo_insta socialIco" title="Follow us on Instagram"></a>
						<a href="#" target="_blank" class="foo_linkedin socialIco" title="Follow us on linkedin"></a>
						<a href="#" target="_blank" class="foo_twt socialIco" title="Follow us on twitter"></a>

						<span class="space20"></span>
						<a href="https://play.google.com/store/apps/details?id=com.autoqed.genericartmedicine" target="_blank" class="mrg_R_5" title="Download our app from Google Playstore"><img src="images/icons/android.png" class="android" /></a>
						<a href="#" class="tooltip" title="Download On App Store"><span class="tooltiptext">Coming Soon !</span><img src="images/icons/ios.png" class="ios" /></a>
					</div>
				</div>
				<div class="float_clear"></div>
			</div>
			<span class="space50"></span>
			<span class="footerLine"></span>
			<div class="copyRight">
				<div class="col_1140">
					<div class="pad_15">
						<span class="small fontRegular float_left">&copy; <%= currentYear %> | Genericart Medicine Store, All Rights Reserved</span>
						<!--<span class="small fontRegular float_right">Website By <a href="http://www.intellect-systems.com" target="_blank" class="intellect" title="Website Design and Development Service By Intellect Systems">Intellect Systems</a></span>-->
						<div class="float_clear"></div>
					</div>
				</div>
			</div>
		</div>
		<!-- Footer Ends -->
	</form>

	<script type="text/javascript">
		function openNav() {
			document.getElementById("topNavPanel").style.width = "320px";
			document.getElementById("navBtn").style.zIndex = "0";
		}

		function closeNav() {
			document.getElementById("topNavPanel").style.width = "0";
			document.getElementById("navBtn").style.zIndex = "5";
		}
	</script>
	<script type="text/javascript">
		$('.bxslider').bxSlider({
			auto: true,
			autoControls: false,
			stopAutoOnClick: true,
			pager: true,
			touchEnabled: false
		});
	</script>
	<script>
		$('#catSlider').multislider({
			duration: 750,
			interval: 3000
		});
		$('#prodSlider').multislider({
			duration: 750,
			interval: false
		});
		$('#cardiacSlider').multislider({
			duration: 750,
			interval: false
		});
		$('#disSlider').multislider({
			duration: 750,
			interval: 3000
		});
		$('#blogSlider').multislider({
			duration: 750,
			interval: false
		});
	</script>
	
	
	<!-- Google Autocomplete -->
	<script type="text/javascript" src="js/plugins.js"></script>
	<script type="text/javascript" src="js/scripts.js"></script>
	

   <%-- <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js" type="text/javascript"></script>
	<link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="Stylesheet" type="text/css" />--%>
	<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />
	<link rel="stylesheet" href="/resources/demos/style.css" />
	<script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
	<script type="text/javascript">
		$(function () {
			//alert("get med load call");
			$("[id$=TxtSearch]").autocomplete({
				
				source: function (request, response) {
					$.ajax({
						url: '<%= ResolveUrl("WebServices.aspx/GetSearchControl") %>',
						data: "{ 'prefix': '" + request.term + "'}",
						dataType: "json",
						type: "POST",
						contentType: "application/json; charset=utf-8",
						success: function (data) {
							response($.map(data.d, function (item) {
								return {
									label: item.split('#')[0],
									val: item.split('#')[1]
								}
							}))
						},
						
					});
				},
			});
		});
	</script>
	<script type="text/javascript">
		$(function () {
			$("[id$=txtCity]").autocomplete({

				source: function (request, response) {
					$.ajax({
						url: '<%= ResolveUrl("WebServices.aspx/GetCities") %>',
						data: "{ 'prefix': '" + request.term + "'}",
						dataType: "json",
						type: "POST",
						contentType: "application/json; charset=utf-8",
						success: function (data) {
							response($.map(data.d, function (item) {
								return {
									label: item.split('-')[0],
									val: item.split('-')[1]
								}
							}))
						},

					});
				},
				select: function (e, i) {
					$("[id$=txtCity]").val(i.item.label);
					PageMethods.GetCity(i.item.label, onSucess, onError);
					//alert(i.item.label);
					function onSucess(result) {
						//alert(result);
					}

					function onError(result) {
						alert(result.get_message());
					}
				},
				minLength: 1
			});
		});
	</script>
</body>
</html>
