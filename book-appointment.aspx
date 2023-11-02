<%@ Page Title="Book Appointment" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="book-appointment.aspx.cs" Inherits="book_appointment" %>
<%@ MasterType VirtualPath="~/MasterParent.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	
	<script type="text/javascript">

		window.onload = function () {
			//alert("window load");

			duDatepicker('#<%= txtDate.ClientID %>', {
				auto: true, inline: true, format: 'dd/mm/yyyy',
			});
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<!-- Appointment Starts -->
	<span class="space50"></span>
	<div class="col_1140" id="docProfile">
		<%=errMsg %>
		<div class="col_800">
			<div class="bgWhite border_r_5 box-shadow">
				<div class="pad_15">
					<h2 class="clrLightBlack semiBold pageH3 mrg_B_10">Appointment Form</h2>

					<span class="small semiBold colrPink">*Note: Psychiatric cases will not be taken in general consultation</span>
					<span class="space30"></span>
					<div class="w20 float_left mrg_B_15">
						<label class="container">
							Myself
							<asp:RadioButton ID="rdbMyself" runat="server" GroupName="appGroup" Checked="true" />  
							<%--<input type="radio" checked="checked" name="radio">--%>
							<span class="checkmark"></span>
						</label>
					</div>
					<div class="w50 float_left mrg_B_15">
						<label class="container">
							Family Member
							<asp:RadioButton ID="rdbFamily" runat="server" GroupName="appGroup" />  
							<%--<input type="radio" name="radio">--%>
							<span class="checkmark"></span>
						</label>
					</div>
					<div class="float_clear"></div>

					<!-- Form starts -->
					<div class="w100 mrg_B_15">
						<div class="app_r_padding">
							<span class="labelCap">Name :*</span>
							<asp:TextBox ID="txtName" CssClass="textBox w95" runat="server" MaxLength="50" placeholder="Enter your full Name"></asp:TextBox>
							<%--<input type="text" id="txtFName" class="textBox w95" placeholder="First Name" />--%>
						</div>
					</div>
					<div class="float_clear"></div>
					<div class="w50 float_left mrg_B_15">
						<div class="app_r_padding">
							<span class="labelCap">Email :*</span>
							<asp:TextBox ID="txtEmail" CssClass="textBox w95" runat="server" MaxLength="50" placeholder="Your email address"></asp:TextBox>
							<%--<input type="text" id="txtEmail" class="textBox w95" placeholder="Your email address" />--%>
						</div>
					</div>
					<div class="w50 float_left mrg_B_15">
						<div class="app_r_padding">
							<span class="labelCap">Mobile :*</span>
							<asp:TextBox ID="txtMobile" CssClass="textBox w95" runat="server" MaxLength="10"  placeholder="Your mobile number"></asp:TextBox>
							<%--<input type="text" id="txtMobile" class="textBox w95" placeholder="Your mobile number" />--%>
						</div>
					</div>
					<div class="float_clear"></div>
					<div class="w50 float_left mrg_B_15">
						<div class="app_r_padding">
							<span class="labelCap">Age :*</span>
							<asp:TextBox ID="txtAge" CssClass="textBox w95" runat="server"  MaxLength="10" placeholder="Your Age"></asp:TextBox>
							<%--<input type="text" id="txtAge" class="textBox w95" placeholder="Your Age" />--%>
						</div>
					</div>
					<div class="w50 float_left mrg_B_15">
						<div class="app_r_padding">
							<span class="labelCap">Gender :*</span>
							<asp:DropDownList ID="ddrGender" CssClass="cmbBox" runat="server">
								<asp:ListItem Value="0"><- Select -></asp:ListItem>
								<asp:ListItem Value="1">Male</asp:ListItem>
								<asp:ListItem Value="2">Female</asp:ListItem>
							</asp:DropDownList>
							<%--<select id="ddrGender" class="cmbBox">
								<option value="0">Male</option>
								<option value="1">Female</option>
							</select>--%>
						</div>
					</div>
					<div class="float_clear"></div>
					<div class="w100 mrg_B_15">
						<div class="app_r_padding">
							<span class="labelCap">Address :*</span>
							<asp:TextBox ID="txtAddress" runat="server" CssClass="textBox" TextMode="MultiLine" Rows="8" Columns="20"></asp:TextBox>
		
							<%--<textarea id="txtProblem" rows="8" cols="20" class="textBox"></textarea>--%>
						</div>
					</div>
					<div class="w50 float_left mrg_B_15">
						<div class="app_r_padding">
							<span class="labelCap">Pincode :</span>
							<asp:TextBox ID="txtPinCode" runat="server" CssClass="textBox"  MaxLength="10"></asp:TextBox>
						</div>
					</div>
					<div class="float_clear"></div>
					<div class="w100 mrg_B_15">
						<div class="app_r_padding">
							<span class="labelCap">Describe Your Problem with Duration :*</span>
							<asp:TextBox ID="txtProblem" runat="server" CssClass="textBox" TextMode="MultiLine" Rows="8" Columns="20" placeholder="Describe Your Problem with Duration"></asp:TextBox>
		
							<%--<textarea id="txtProblem" rows="8" cols="20" class="textBox"></textarea>--%>
						</div>
					</div>
					<div class="w50 float_left mrg_B_15">
						<div class="app_r_padding">
							<span class="labelCap">Appointment Date :*</span>
							<asp:TextBox ID="txtDate" runat="server" CssClass="textBox" placeholder="Click here to Open Calendar"></asp:TextBox>
						</div>
					</div>
					<div class="w50 float_left mrg_B_15">
						<div class="app_r_padding">
							<span class="labelCap">Earlier Consulted Doctor Name :*</span>
							<asp:TextBox ID="txtDocName" runat="server" CssClass="textBox"  MaxLength="50"></asp:TextBox>
						</div>
					</div>
					<div class="float_clear"></div>
					<div class="w50 float_left mrg_B_15">
						<div class="app_r_padding">
							<span class="labelCap">Reference / Shop Code :</span>
							<asp:TextBox ID="txtShopCode" runat="server" CssClass="textBox"  MaxLength="50"></asp:TextBox>
						</div>
					</div>
					<div class="float_clear"></div>
					<span class="space20"></span>
					<asp:Button ID="btnSave" runat="server" CssClass="blueAnch small semiBold dspInlineBlk" Text="Book Appointment" OnClick="btnSave_Click"/>
					<%--<input id="btnSave" type="button" value="Book Appointment" class="blueAnch small semiBold dspInlineBlk" />--%>
					<!-- Form ends -->
				</div>
			</div>
		</div>
		<div class="col_340">
			<div class="pad_L_15">

				<%=docStr %>

<%--				<div class="bgWhite border_r_5 box-shadow">
					<div class="width50">
						<div class="pad_15">
							<img src="<%= Master.rootPath + "images/dr-profile.jpg" %>" alt="" class="width100 border_r_5" />
						</div>
					</div>
					<div class="width50">
						<div class="pad_TB_15">
							<h1 class="docName semiBold">Dr. Amish Tripathy</h1>
							<span class="themeClrPrime semiBold tiny">Cardiologist</span><br />
							<span class="clrGrey semiBold tiny">15 Years Experience</span>
							<span class="space5"></span>
							<p class="tiny clrGrey line-ht-5 fontRegular">Lorem Ipsum is simply dummy text of the...</p>
							<span class="space5"></span>
							<a href="#" class="readMore" style="font-size:0.8em; font-weight:600;">Check Profile</a>
						</div>
					</div>
					<div class="float_clear"></div>
				</div>--%>


				<%--<span class="space10"></span>--%>
				<div class="bgWhite border_r_5 box-shadow">
					<div class="pad_15">
						<h3 class="clrLightBlack semiBold mrg_B_10">Booking Steps</h3>
						<ul class="timeline">
							<li class="active">
								<span class="regular">Step 1</span>
								<p class="clrGrey fontRegular line-ht-5">Fill the desired form for doctors consultation.</p>
							</li>
							<li class="active">
								<span class="regular">Step 2</span>
								<p class="clrGrey fontRegular line-ht-5">Our team will contact you within a hour</p>
							</li>
							<li class="active">
								<span class="regular">Step 3</span>
								<p class="clrGrey fontRegular line-ht-5">Our team will let you know the time of consultation</p>
							</li>
							<li class="active">
								<span class="regular">Step 3</span>
								<p class="clrGrey fontRegular line-ht-5">You will get online / in person prescription after consultation according to  the mode of consultation you prefer.</p>
							</li>
						</ul>
					</div>
				</div>
				<span class="space10"></span>

				<%--<%=consultStr %>--%>
				<div class="bgWhite border_r_5 box-shadow">
					<div class="pad_15">
						<h2 class="clrLightBlack semiBold">Appointment Summary</h2>
						<span class="lineSeperator"></span>

						<span class="semiBold clrLightBlack small float_left">Consultation Fees : </span><span class="fontRegular small float_right">&#8377; 100</span>
						<div class="float_clear"></div>
						<span class="lineSeperator"></span>
						<span class="semiBold clrLightBlack small float_left">Total Value : </span><span class="fontRegular small float_right">&#8377; 100</span>
						<div class="float_clear"></div>
					</div>
				</div>


			</div>
		</div>
		<div class="float_clear"></div>
	</div>
	<span class="space50"></span>
	<!-- Appointment Ends -->

	
</asp:Content>

