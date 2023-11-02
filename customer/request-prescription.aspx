<%@ Page Title="Request Prescription" Language="C#" MasterPageFile="~/customer/MasterCustomer.master" AutoEventWireup="true" CodeFile="request-prescription.aspx.cs" Inherits="customer_request_prescription" %>
<%@ MasterType VirtualPath="~/customer/MasterCustomer.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<!-- Request Rx Starts -->
		<div class="width50" id="qc">
			<div class="pad_10">
				<div class="bgWhite border_r_5 box-shadow">
					<div class="pad_15 posRelative">
						<h2 class="clrLightBlack semiBold semiMedium">Request Prescription</h2>
						<span class="space20"></span>

						<div class="proNavPanel">
							<ul class="proNav">
								<li><a id="trReq" href="javascript:toggleBox('Request', 'trReq');">Request Prescription</a></li>
								<li><a id="trView" href="javascript:toggleBox('ViewReq', 'trView');">My Prescriptions</a></li>
							</ul>
						</div>
						<span class="space20"></span>
						<div id="Request">
							<div class="w20 float_left mrg_B_15">
								<label class="container">
									Myself
								<asp:RadioButton ID="rdbMyself" runat="server" GroupName="appGroup" Checked="true" AutoPostBack="true" OnCheckedChanged="rdbMyself_CheckedChanged" />
									<span class="checkmark"></span>
								</label>
							</div>
							<div class="w50 float_left mrg_B_15">
								<label class="container">
									Family Member
								<asp:RadioButton ID="rdbFamily" runat="server" GroupName="appGroup" AutoPostBack="true" OnCheckedChanged="rdbFamily_CheckedChanged" />
									<span class="checkmark"></span>
								</label>
							</div>
							<div class="float_clear"></div>
							<!-- Form starts -->
							<div class="w100 mrg_B_15">
								<div class="app_r_padding">
									<span class="labelCap">Name :*</span>
									<asp:TextBox ID="txtName" CssClass="textBox w95" runat="server" MaxLength="50" placeholder="Enter your full Name"></asp:TextBox>
								</div>
							</div>
							<div class="float_clear"></div>

							<div class="w100 mrg_B_15">
								<div class="app_r_padding">
									<span class="labelCap">Mobile :*</span>
									<asp:TextBox ID="txtMobile" CssClass="textBox w95" runat="server" MaxLength="10" placeholder="Your mobile number"></asp:TextBox>
								</div>
							</div>
							<div class="w50 float_left mrg_B_15">
								<div class="app_r_padding">
									<span class="labelCap">Age :*</span>
									<asp:TextBox ID="txtAge" CssClass="textBox w95" runat="server" MaxLength="10" placeholder="Your Age"></asp:TextBox>
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
								</div>
							</div>
							<div class="float_clear"></div>
							<div class="w100 mrg_B_15">
								<div class="app_r_padding">
									<span class="labelCap">Address :*</span>
									<asp:TextBox ID="txtAddr" runat="server" CssClass="textBox" TextMode="MultiLine" Rows="3" Columns="20"></asp:TextBox>
								</div>
							</div>
							<div class="w100 mrg_B_15">
								<div class="app_r_padding">
									<span class="labelCap">Disease :*</span>
									<asp:TextBox ID="txtDisease" runat="server" CssClass="textBox" MaxLength="50"></asp:TextBox>
								</div>
							</div>
							<div class="w100 mrg_B_15">
								<div class="app_r_padding">
									<span class="labelCap">Medicine Name :*</span>
									<asp:TextBox ID="txtMedicine" runat="server" CssClass="textBox" TextMode="MultiLine" Rows="8" Columns="20" placeholder="Enter Each medicine name in new line"></asp:TextBox>
								</div>
							</div>
							<span class="space20"></span>
							<asp:Button ID="btnSave" runat="server" CssClass="blueAnch small semiBold dspInlineBlk" Text="Submit" OnClick="btnSave_Click" />
							<!-- Form ends -->
						</div>
						<div id="ViewReq">
							<asp:GridView ID="gvRxReq" runat="server" CssClass="gvApp" GridLines="None"
								AutoGenerateColumns="false" Width="100%" OnRowDataBound="gvRxReq_RowDataBound">
								<RowStyle CssClass="" />
								<HeaderStyle CssClass="bg-dark" />
								<AlternatingRowStyle CssClass="alt" />
								<Columns>
									<asp:BoundField DataField="PreReqID">
										<HeaderStyle CssClass="HideCol" />
										<ItemStyle CssClass="HideCol" />
									</asp:BoundField>
									<asp:BoundField DataField="PreReqStatus">
										<HeaderStyle CssClass="HideCol" />
										<ItemStyle CssClass="HideCol" />
									</asp:BoundField>
									<asp:BoundField DataField="reqDate" HeaderText="Date">
										<ItemStyle Width="5%" />
									</asp:BoundField>
									<asp:BoundField DataField="PreReqName" HeaderText="Name">
										<ItemStyle Width="20%" />
									</asp:BoundField>
									<asp:BoundField DataField="PreReqDisease" HeaderText="Disease">
										<ItemStyle Width="10%" />
									</asp:BoundField>
									<asp:TemplateField HeaderText="Status">
										<ItemStyle Width="5%" />
										<ItemTemplate>
											<asp:Literal ID="litStatus" runat="server"></asp:Literal>
										</ItemTemplate>
									</asp:TemplateField>
								</Columns>
								<EmptyDataTemplate>
									<span class="warning">No Data to Display :(</span>
								</EmptyDataTemplate>
							</asp:GridView>
						</div>
					</div>
				</div>
			</div>
		</div>
		<!-- Request Rx Ends -->

	<script type="text/javascript">
		$(function () {
			toggleBox('Request', 'trReq');
		});

		function toggleBox(divId, switchId) {
			document.getElementById("Request").style.display = "none";
			document.getElementById("ViewReq").style.display = "none";
			document.getElementById("trReq").className = "";
			document.getElementById("trView").className = "";
			$("#" + divId).fadeIn("5000");
			document.getElementById(switchId).className = "act";
		}
	</script>
</asp:Content>

