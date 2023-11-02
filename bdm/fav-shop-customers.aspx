<%@ Page Title="List of Shops with Fav Shop Customers Count" Language="C#" MasterPageFile="~/bdm/MasterBdm.master" AutoEventWireup="true" CodeFile="fav-shop-customers.aspx.cs" Inherits="management_fav_shop_customers" %>

<%@ MasterType VirtualPath="~/management/MasterManagement.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<%--<script>
		$(document).ready(function () {
			$('[id$=gvOrder]').DataTable({
				columnDefs: [
					 { orderable: false, targets: [0, 1, 2, 3, 4] }
				],
				order: [[4, 'desc']]
			});
		});
	</script>--%>
	<script type="text/javascript">
		$(function () {
			var prm = Sys.WebForms.PageRequestManager.getInstance();
			if (prm != null) {
				prm.add_endRequest(function (sender, e) {
					if (sender._postBackSettings.panelsToUpdate != null) {
						createDataTable();
					}
				});
			};

			createDataTable();
			function createDataTable() {
				$('#<%= gvOrder.ClientID %>').prepend($("<thead></thead>").append($('#<%= gvOrder.ClientID %>').find("tr:first"))).DataTable({

					columnDefs: [
						{ orderable: false, targets: [0, 1, 2, 3, 4] }
					],
					order: [[4, 'desc']]
				});

			}
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<h2 class="pgTitle">Favourite Shops Customers</h2>
	<span class="space15"></span>

	<div class="row">
		<div class="col-md-3 form-group" id="zhPanel" runat="server">
			<label>Select Zonal Head :*</label>
			<asp:DropDownList ID="ddrZh" runat="server" CssClass="form-control" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddrZh_SelectedIndexChanged">
				<asp:ListItem Value="0"><- Select -></asp:ListItem>
			</asp:DropDownList>
		</div>
		<div class="col-md-3 form-group" runat="server">
			<label>Select District Head :*</label>
			<asp:DropDownList ID="ddrDh" runat="server" CssClass="form-control" Width="100%">
				<asp:ListItem Value="0"><- Select -></asp:ListItem>
			</asp:DropDownList>
		</div>
		<div class="col-md-3 form-group">
			<span style="height: 6px; display: block; width: 100%;"></span>
			<br />
			<asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Show Report" OnClick="btnSave_Click" />
		</div>
	</div>
	<span class="space15"></span>

	<asp:GridView ID="gvOrder" runat="server"
		CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
		AutoGenerateColumns="false" Width="100%">
		<RowStyle CssClass="" />
		<HeaderStyle CssClass="bg-dark" />
		<AlternatingRowStyle CssClass="alt" />
		<Columns>
			<asp:BoundField DataField="FranchID">
				<HeaderStyle CssClass="HideCol" />
				<ItemStyle CssClass="HideCol" />
			</asp:BoundField>
			<asp:BoundField DataField="FranchName" HeaderText="Shop Name">
				<ItemStyle Width="25%" />
			</asp:BoundField>
			<asp:BoundField DataField="FranchShopCode" HeaderText="Shop Code">
				<ItemStyle Width="20%" />
			</asp:BoundField>
			<asp:BoundField DataField="CityName" HeaderText="City">
				<ItemStyle Width="10%" />
			</asp:BoundField>
			<asp:BoundField DataField="totalFavCust" HeaderText="Fav. Shop Customers">
				<ItemStyle Width="20%" />
			</asp:BoundField>
		</Columns>
		<EmptyDataTemplate>
			<span class="warning">No Data to Display :(</span>
		</EmptyDataTemplate>
		<PagerStyle CssClass="gvPager" />
	</asp:GridView>
</asp:Content>

