<%@ Page Title="Generic Mitra Details" Language="C#" MasterPageFile="~/franchisee/MasterFranchisee.master" AutoEventWireup="true" CodeFile="generic-mitra-details.aspx.cs" Inherits="franchisee_generic_mitra_details" %>
<%@ MasterType VirtualPath="~/franchisee/MasterFranchisee.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script>
		$(document).ready(function () {
			$('[id$=gvGenMitra]').DataTable({
				columnDefs: [
					{ orderable: false, targets: [0, 1, 2, 3, 4, 6, 7, 8, 9] }
				],
				order: [[4, 'desc']]
			});
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<h2 class="pgTitle">Generic Mitra Details</h2>
	<span class="space15"></span>
	<div id="viewOrder" runat="server">
		<div>
			<asp:GridView ID="gvGenMitra" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
				AutoGenerateColumns="false" Width="100%" OnRowDataBound="gvGenMitra_RowDataBound" >
				<RowStyle CssClass="" />
				<HeaderStyle CssClass="bg-dark" />
				<AlternatingRowStyle CssClass="alt" />
				<Columns>
					<asp:BoundField DataField="GMitraID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="rDate" HeaderText="Reg. Date">
						<ItemStyle Width="8%" />
					</asp:BoundField>
					<asp:BoundField DataField="GMitraName" HeaderText="Name">
						<ItemStyle Width="15%" />
					</asp:BoundField>
					<asp:BoundField DataField="GMitraMobile" HeaderText="Mobile">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="custCount" HeaderText="No. Of Customers">
						<ItemStyle Width="8%" />
					</asp:BoundField>
					<asp:BoundField DataField="ordValue" HeaderText="Total Order Value">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:TemplateField HeaderText="20% Comission">
						<ItemStyle Width="10%" />
						<ItemTemplate>
							<asp:Literal ID="litComission" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Total Orders">
						<ItemStyle Width="5%" />
						<ItemTemplate>
							<asp:Literal ID="litOrders" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Status">
						<ItemStyle Width="5%" />
						<ItemTemplate>
							<asp:Literal ID="litStatus" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Pay Comission">
						<ItemStyle Width="10%" />
						<ItemTemplate>
							<asp:Literal ID="litPay" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="">
						<ItemStyle Width="5%" />
						<ItemTemplate>
							<asp:Literal ID="litAnch" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
				</Columns>
				<EmptyDataTemplate>
					<span class="warning">No Data to Display :(</span>
				</EmptyDataTemplate>
				<PagerStyle CssClass="gvPager" />
			</asp:GridView>
		</div>
	</div>
</asp:Content>

