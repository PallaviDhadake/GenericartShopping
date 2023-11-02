<%@ Page Title="Shop List" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="shop-list.aspx.cs" Inherits="admingenshopping_shop_list" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script>
		$(document).ready(function () {
			$('[id$=gvShops]').DataTable({
				columnDefs: [
					 { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7] }
				],
				order: [[0, 'desc']]
			});
		});
	</script>
	<script type="text/javascript">

		window.onload = function () {
			//alert("window load");

			duDatepicker('#<%= txtRegDate.ClientID %>', {
				auto: true, inline: true, format: 'dd/mm/yyyy',
			});
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<h2 class="pgTitle">Shop List</h2>
	<span class="space15"></span>
	<!-- Gridview to show saved data starts here -->
	<div id="viewFranch" runat="server">
		<div class="formPanel table-responsive-md">
			<asp:GridView ID="gvShops" runat="server" CssClass="table table-striped table-bordered table-hover" GridLines="None"
				AutoGenerateColumns="false" OnRowDataBound="gvShops_RowDataBound">
				<HeaderStyle CssClass="thead-dark" />
				<RowStyle CssClass="" />
				<AlternatingRowStyle CssClass="alt" />
				<Columns>
					<asp:BoundField DataField="FranchID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="FranchShopCode" HeaderText="Shop Code">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="FranchName" HeaderText="Shop Name">
						<ItemStyle Width="20%" />
					</asp:BoundField>
					<asp:BoundField DataField="FranchMobile" HeaderText="Mobile No">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="FranchAddress" HeaderText="Address">
						<ItemStyle Width="30%" />
					</asp:BoundField>
					<asp:BoundField DataField="CityName" HeaderText="City">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="FranchPassword" HeaderText="Password">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:TemplateField>
						<ItemStyle Width="5%" />
						<ItemTemplate>
							<asp:Literal ID="litAnch" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
					<%--<asp:BoundField DataField="cityName" HeaderText="City">
						<ItemStyle Width="10%"/>
					</asp:BoundField>--%>

					<%--<asp:TemplateField HeaderText="Status" >
						<ItemStyle Width="3%" />
						<ItemTemplate>
							<asp:Literal ID="litStatus" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>  --%>
				</Columns>
				<EmptyDataTemplate>
					<span class="warning">Its Empty Here... :(</span>
				</EmptyDataTemplate>
				<PagerStyle CssClass="" />
			</asp:GridView>
		</div>
	</div>

	<div id="editFranch" runat="server">
		<%--card start--%>
		<div class="card card-primary">
			<div class="card-header">
				<h3 class="card-title"><%=pgTitle %></h3>
			</div>
			<%-- card body--%>
			<div class="card-body">
				<div class="colorLightBlue">
					<span>Id :</span>
					<asp:Label ID="lblId" runat="server" Text="[New]"></asp:Label>
				</div>

				<span class="space15"></span>
				<%--form row start--%>
				<div class="form-row">
					<div class="form-group col-md-6">
						<label>Shop Name :*</label>
						<asp:TextBox ID="txtShopName" runat="server" CssClass="form-control" Width="100%" MaxLength="100"></asp:TextBox>
					</div>
					<div class="form-group col-md-6">
						<label>Shop Owner Name :*</label>
						<asp:TextBox ID="txtOwner" runat="server" CssClass="form-control" Width="100%" MaxLength="100"></asp:TextBox>
					</div>
					<div class="form-group col-md-6">
						<label>ShopCode :*</label>
						<asp:TextBox ID="txtShopCode" runat="server" CssClass="form-control" Width="100%" MaxLength="30"></asp:TextBox>
					</div>
					<div class="form-group col-md-6">
						<label>Registration Date :*</label>
						<asp:TextBox ID="txtRegDate" runat="server" CssClass="form-control" Width="100%" MaxLength="10"></asp:TextBox>
					</div>
					<div class="form-group col-md-6">
						<label>Mobile No :*</label>
						<asp:TextBox ID="txtMobileNo" runat="server" CssClass="form-control" Width="100%" MaxLength="10"></asp:TextBox>
					</div>
					<div class="form-group col-md-6">
						<label>Email Id :*</label>
						<asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
					</div>
					<div class="form-group col-md-6">
						<label>State :*</label>
						<asp:DropDownList ID="ddrState" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddrState_SelectedIndexChanged">
							<asp:ListItem Value="0"><- Select -></asp:ListItem>
						</asp:DropDownList>
					</div>
					<div class="form-group col-md-6">
						<label>District :*</label>
						<asp:DropDownList ID="ddrDist" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddrDist_SelectedIndexChanged">
							<asp:ListItem Value="0"><- Select -></asp:ListItem>
						</asp:DropDownList>
					</div>
					<div class="form-group col-md-6">
						<label>City :*</label>
						<asp:DropDownList ID="ddrCity" runat="server" CssClass="form-control" Width="100%">
							<asp:ListItem Value="0"><- Select -></asp:ListItem>
						</asp:DropDownList>
					</div>
					<div class="form-group col-md-6">
						<label>Pincode :*</label>
						<asp:TextBox ID="txtPinCode" runat="server" CssClass="form-control" Width="100%" MaxLength="10"></asp:TextBox>
					</div>

					<div class="form-group col-md-6">
						<label>Address :*</label>
						<asp:TextBox ID="txtAddress" runat="server" CssClass="form-control textarea" TextMode="MultiLine" Width="100%" Height="150" MaxLength="200"></asp:TextBox>
					</div>
					<div class="form-group col-md-6">
						
					</div>
					<div class="form-group col-md-6">
						<label>Latlongs :*</label>
						<asp:TextBox ID="txtLatLongs" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
					</div>
					<div class="form-group col-md-6">
						<label>Password :*</label>
						<asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" Width="100%" MaxLength="20"></asp:TextBox>
					</div>
					<div class="form-group col-md-6">
						<label>Zonal Head :*</label>
						<asp:DropDownList ID="ddrZH" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddrZH_SelectedIndexChanged">
							<asp:ListItem Value="0"><- Select -></asp:ListItem>
						</asp:DropDownList>
					</div>
					<div class="form-group col-md-6">
						<label>District Head :*</label>
						<asp:DropDownList ID="ddrDH" runat="server" CssClass="form-control" Width="100%">
							<asp:ListItem Value="0"><- Select -></asp:ListItem>
						</asp:DropDownList>
					</div>
					<%--<div class="form-group col-md-6">
						<label>Adhar Card: </label>
						<%= frAdhar %>
					</div>
					<div class="form-group col-md-6">
						<label>Pan Card: </label>
						<%= frPan %>
					</div>--%>
				</div>
				<%--form row End--%>

				
			</div>
			<%-- card body end--%>
			<div class="pad_LR_20">
				<h3 class="large colorLightBlue">Bank Details</h3>
			</div>
			<div class="card-body">
				<div class="form-row">
					<div class="form-group col-md-6">
						<label>Bank Name :*</label>
						<asp:TextBox ID="txtBankName" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
					</div>
					<div class="form-group col-md-6">
						<label>Branch Name :*</label>
						<asp:TextBox ID="txtBranch" runat="server" CssClass="form-control" Width="100%" MaxLength="30"></asp:TextBox>
					</div>
					<div class="form-group col-md-6">
						<label>Bank Account Name :*</label>
						<asp:TextBox ID="txtBankAccName" runat="server" CssClass="form-control" Width="100%" MaxLength="100"></asp:TextBox>
					</div>
					<div class="form-group col-md-6">
						<label>Bank Account Number :*</label>
						<asp:TextBox ID="txtAccNo" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
					</div>
					<div class="form-group col-md-6">
						<label>IFSC Number :*</label>
						<asp:TextBox ID="txtIfsc" runat="server" CssClass="form-control" Width="100%" MaxLength="30"></asp:TextBox>
					</div>
					<div class="form-check col-md-6">
						<span class="space30"></span>
						<asp:CheckBox ID="chkLegal" runat="server" TextAlign="Right" />
						<label class="form-check-label" for="featured"><strong>Is Legal Block ?</strong></label>
					</div>
				</div>
			</div>
		</div>
		<%--card end--%>

		<!-- Button controls starts -->
		<span class="space10"></span>
		<%=errMsg %>
		<span class="space10"></span>
		<asp:Button ID="btnSave" runat="server"
			CssClass="btn btn-md btn-primary" Text="Save" OnClick="btnSave_Click" />
		<asp:Button ID="btnDelete" runat="server"
			CssClass="btn btn-md btn-outline-info" Text="Delete" OnClientClick="return confirm('Are you sure to delete?');"
			OnClick="btnDelete_Click" />
		<asp:Button ID="btnCancel" runat="server"
			CssClass="btn btn-md btn-outline-dark" Text="Cancel"
			OnClick="btnCancel_Click" />
		<div class="float_clear"></div>
		<!-- Button controls ends -->
	</div>
</asp:Content>

