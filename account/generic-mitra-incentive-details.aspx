<%@ Page Title="" Language="C#" MasterPageFile="~/account/MasterAccount.master" AutoEventWireup="true" CodeFile="generic-mitra-incentive-details.aspx.cs" Inherits="account_generic_mitra_incentive_details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script>
		$(document).ready(function () {
			$('[id$=gvOrder]').DataTable({
				columnDefs: [
					{ orderable: false, targets: [0, 1, 2, 3, 4, 6, 7, 8, 9, 10, 11] }
				],
				order: [[0, 'desc']]
			});
		});
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<h2 class="pgTitle">Generic Mitra Order Report</h2>
	<span class="space15"></span>
	<%--Franchisee orders GridView Start--%>
	<div id="viewOrder" runat="server">
		<%--<a href="supplier-master.aspx?action=new" class="btn btn-primary btn-sm">Add New</a>--%>
		<span class="space15"></span>
		<div>
			<asp:GridView ID="gvOrder" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
				AutoGenerateColumns="false" OnRowDataBound="gvOrder_RowDataBound" Width="100%">
				<RowStyle CssClass="" />
				<HeaderStyle CssClass="bg-dark" />
				<AlternatingRowStyle CssClass="alt" />
				<Columns>
					<asp:BoundField DataField="OrdAssignID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="OrdAssignStatus">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="FK_OrderID" HeaderText="Order ID">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="SaleBill" HeaderText="Sale Bill No.">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="ordDate" HeaderText="Date">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="CustomerName" HeaderText="Name">
						<ItemStyle Width="15%" />
					</asp:BoundField>
					<asp:BoundField DataField="OrdAmount" HeaderText="Amount">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="ProductCount" HeaderText="No. Of Product">
						<ItemStyle Width="12%" />
					</asp:BoundField>
					 <asp:BoundField DataField="CartProducts" HeaderText="Products">
						<ItemStyle Width="30%" />
					</asp:BoundField>
					<asp:TemplateField HeaderText="Status">
						<ItemStyle Width="5%" />
						<ItemTemplate>
							<asp:Literal ID="litStatus" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="DeviceType" HeaderText="Order From">
						<ItemStyle Width="10%" />
					</asp:BoundField>  
					<asp:BoundField DataField="PaymentMode" HeaderText="Payment Mode">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<%--<asp:TemplateField>
						<ItemStyle Width="5%" />
						<ItemTemplate>
							<asp:Literal ID="litAnch" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField> 
					<asp:TemplateField HeaderText="Mark as Not Monthly Order">
						<ItemStyle Width="10%" />
						<ItemTemplate>							
							<asp:LinkButton ID="cmdRemove" runat="server" CssClass="btn" CommandName="gvRemove" OnClientClick="return confirm('Are you sure you want to remove this?');"><i class="fas fa-2x fa-calendar-times"></i></asp:LinkButton>
						</ItemTemplate>
					</asp:TemplateField> --%>
				</Columns>
				<EmptyDataTemplate>
					<span class="warning">No Orders to Display :(</span>
				</EmptyDataTemplate>
				<PagerStyle CssClass="gvPager" />
			</asp:GridView>
		</div>
	</div>
	<%--Franchisee Orders GridView End--%>
</asp:Content>

