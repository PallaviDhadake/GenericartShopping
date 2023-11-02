<%@ Page Title="List of Shops with Fav Shop Customers Count" Language="C#" MasterPageFile="~/districthead/MasterDH.master" AutoEventWireup="true" CodeFile="fav-shop-customers.aspx.cs" Inherits="districthead_fav_shop_customers" %>

<%@ MasterType VirtualPath="~/districthead/MasterDH.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script>
		$(document).ready(function () {
			$('[id$=gvOrder]').DataTable({
				columnDefs: [
					 { orderable: false, targets: [0, 1, 2, 3, 4] }
				],
				order: [[4, 'desc']]
			});
		});
	 </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<h2 class="pgTitle">Favourite Shops Customers</h2>
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

