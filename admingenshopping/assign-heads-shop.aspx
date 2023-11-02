<%@ Page Title="Shop List" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="assign-heads-shop.aspx.cs" Inherits="admingenshopping_assign_heads_shop" %>

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<h2 class="pgTitle">Shop List</h2>
	<span class="space15"></span>

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
</asp:Content>

