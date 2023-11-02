<%@ Page Title="Bludart Waybills" Language="C#" MasterPageFile="~/franchisee/MasterFranchisee.master" AutoEventWireup="true" CodeFile="bluedart-waybills.aspx.cs" Inherits="franchisee_bluedart_waybills" %>
<%@ MasterType VirtualPath="~/franchisee/MasterFranchisee.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script>
		$(document).ready(function () {
			$('[id$=gvWaybills]').DataTable({
				columnDefs: [
					{ orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7, 8] }
				],
				order: [[0, 'desc']]
			});
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<h2 class="pgTitle">Order Report Master</h2>
	<span class="space15"></span>
	<div id="viewOrder" runat="server">
		<div>
			<asp:GridView ID="gvWaybills" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
				AutoGenerateColumns="false" Width="100%" OnRowDataBound="gvWaybills_RowDataBound">
				<RowStyle CssClass="" />
				<HeaderStyle CssClass="bg-dark" />
				<AlternatingRowStyle CssClass="alt" />
				<Columns>
					<asp:BoundField DataField="WayBillID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="wDate" HeaderText="Waybill Date">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="FK_OrderID" HeaderText="Order ID">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="AWBNo" HeaderText="AWB No.">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="CCRCRDREF" HeaderText="Credit Ref No.">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="TokenNumber" HeaderText="Token No.">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="shipDate" HeaderText="Pickup Date & Time">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:TemplateField HeaderText="Waybill PDF">
						<ItemStyle Width="10%" />
						<ItemTemplate>
							<asp:Literal ID="litPdf" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="wBillCat" HeaderText="Type">
						<ItemStyle Width="3%" />
					</asp:BoundField>
				</Columns>
				<EmptyDataTemplate>
					<span class="warning">No Data to Display :(</span>
				</EmptyDataTemplate>
				<PagerStyle CssClass="gvPager" />
			</asp:GridView>
		</div>
	</div>
</asp:Content>

