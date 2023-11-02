<%@ Page Title="Shop List" Language="C#" MasterPageFile="~/doctors/MasterDoctor.master" AutoEventWireup="true" CodeFile="shop-list.aspx.cs" Inherits="doctors_shop_list" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
	<script>
		function delConfirm(btn) {
			btn.value = 'processing...';
			return confirm("Are you sure to send prescription to selected shop?");
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<h2 class="text-lg text-primary text-bold"><%= PageTitle %></h2>
	<span class="space15"></span>
	<h2 class="pgTitle">Shop List</h2>
	<span class="space15"></span>
	<!-- Gridview to show saved data starts here -->
	<div id="viewFranch" runat="server">
		<div class="formPanel table-responsive-md">
			<asp:GridView ID="gvShops" runat="server" CssClass="table table-striped table-bordered table-hover" GridLines="None"
				AutoGenerateColumns="false" OnRowDataBound="gvShops_RowDataBound" OnRowCommand="gvShops_RowCommand" >
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
					<asp:BoundField DataField="FranchPinCode" HeaderText="Pincode">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					
					<asp:TemplateField>
						<ItemStyle Width="30%" />
						<ItemTemplate>
							<asp:Button ID="cmdForword" runat="server" CssClass="btn btn-sm btn-primary" CommandName="gvForowrd" Text="Forward Prescription" OnClientClick="return delConfirm(this);" />
						</ItemTemplate>
					</asp:TemplateField>
					<%--<asp:BoundField DataField="FranchPassword" HeaderText="Password">
						<ItemStyle Width="10%" />
					</asp:BoundField>--%>
				</Columns>
				<EmptyDataTemplate>
					<span class="warning">Its Empty Here... :(</span>
				</EmptyDataTemplate>
				<PagerStyle CssClass="" />
			</asp:GridView>
		</div>
	</div>

</asp:Content>

