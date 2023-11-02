<%@ Page Title="Book Lab Test" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="book-lab-test.aspx.cs" Inherits="book_lab_test" %>
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
	<span class="space50"></span>
	<div class="col_1140" id="docProfile">
		<%=errMsg %>
		<div class="col_800_center">
			<div class="bgWhite border_r_5 box-shadow">
				<div class="pad_15">
					<h2 class="clrLightBlack semiBold pageH3 mrg_B_20 txtCenter">Book Lab Test</h2>

					<!-- Form starts -->
					<div class="w50 float_left mrg_B_15">
						<div class="app_r_padding">
							<span class="labelCap">Name :*</span>
							<asp:TextBox ID="txtName" CssClass="textBox w95" runat="server"  MaxLength="50" placeholder="Enter Full Name"></asp:TextBox>
						</div>
					</div>
					<div class="w50 float_left mrg_B_15">
						<div class="app_r_padding">
							<span class="labelCap">Age :*</span>
							<asp:TextBox ID="txtAge" CssClass="textBox w95" runat="server"  MaxLength="10" placeholder="Your Age"></asp:TextBox>
						</div>
					</div>
					<div class="float_clear"></div>
					<div class="w50 float_left mrg_B_15">
						<div class="app_r_padding">
							<span class="labelCap">Gender :*</span>
							<asp:DropDownList ID="ddrGender" CssClass="cmbBox" runat="server">
								<asp:ListItem Value="0"><- Select -></asp:ListItem>
								<asp:ListItem Value="1">Male</asp:ListItem>
								<asp:ListItem Value="2">Female</asp:ListItem>
							</asp:DropDownList>
						</div>
					</div>
					<div class="w50 float_left mrg_B_15">
						<div class="app_r_padding">
							<span class="labelCap">Mobile :*</span>
							<asp:TextBox ID="txtMobile" CssClass="textBox w95" runat="server" MaxLength="10"  placeholder="Your mobile number"></asp:TextBox>
						</div>
					</div>
					<div class="float_clear"></div>
					<div class="w100 mrg_B_15">
						<div class="app_r_padding">
							<span class="labelCap">Address :*</span>
							<asp:TextBox ID="txtAddress" runat="server" CssClass="textBox" TextMode="MultiLine" Rows="8" Columns="20"></asp:TextBox>
						</div>
					</div>
					<div class="w50 float_left mrg_B_15">
						<div class="app_r_padding">
							<span class="labelCap">Pincode :*</span>
							<asp:TextBox ID="txtPinCode" CssClass="textBox w95" runat="server"  MaxLength="12" placeholder="Enter Pincode"></asp:TextBox>
						</div>
					</div>
					<div class="w50 float_left mrg_B_15">
						<div class="app_r_padding">
							<span class="labelCap">Email :*</span>
							<asp:TextBox ID="txtEmail" CssClass="textBox w95" runat="server"  MaxLength="50" placeholder="Your email address"></asp:TextBox>
						</div>
					</div>
					<div class="float_clear"></div>
					<div class="w50 float_left mrg_B_15">
						<div class="app_r_padding">
							<span class="labelCap">Appointment Date :*</span>
							<asp:TextBox ID="txtDate" runat="server" CssClass="textBox" placeholder="Click here to Open Calendar"></asp:TextBox>
						</div>
					</div>
					<div class="w50 float_left mrg_B_15">
						<div class="app_r_padding">
							<span class="labelCap">Select Lab Test:*</span>
							<asp:DropDownList ID="ddrLab" CssClass="cmbBox" runat="server">
								<asp:ListItem Value="0"><- Select -></asp:ListItem>
							</asp:DropDownList>
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
					<asp:Button ID="btnSave" runat="server" CssClass="blueAnch small semiBold dspInlineBlk" Text="Book Lab Test" OnClick="btnSave_Click"/>
					<!-- Form ends -->
				</div>
			</div>
		</div>
		<div class="float_clear"></div>
	</div>
</asp:Content>

