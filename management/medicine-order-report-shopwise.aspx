<%@ Page Title="Shop wise Medicine Order Report" Language="C#" MasterPageFile="~/management/MasterManagement.master" AutoEventWireup="true" CodeFile="medicine-order-report-shopwise.aspx.cs" Inherits="management_medicine_order_report_shopwise" %>
<%@ MasterType VirtualPath="~/management/MasterManagement.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script>
		$(document).ready(function () {
			$('[id$=gvOrder]').DataTable({
				columnDefs: [
					 { orderable: false, targets: [0, 1, 2, 4, 5, 6, 7, 8, 9, 10, 11] }
				],
				order: [[3, 'desc']]
			});
		});
	 </script>
	<script type="text/javascript">

		window.onload = function () {
			//alert("window load");

			duDatepicker('#<%= txtFDate.ClientID %>', {
				auto: true, inline: true, format: 'dd/mm/yyyy',

			});
			duDatepicker('#<%= txtToDate.ClientID %>', {
				auto: true, inline: true, format: 'dd/mm/yyyy',

			});
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<h2 class="pgTitle">Shop wise Order Report</h2>
	<span class="space15"></span>

	<div id="viewOrder" runat="server">
		<div class="row">
			<div class="col-md-2 form-group">
				<label>From Date :*</label>
				<asp:TextBox ID="txtFDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
			</div>
			<div class="col-md-2 form-group">
				<label>From Date :*</label>
				<asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
			</div>
			<div class="col-md-2 form-group" id="zhPanel" runat="server" visible="false">
				<label>Select Zonal Head :*</label>
				<asp:DropDownList ID="ddrZh" runat="server" CssClass="form-control" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddrZh_SelectedIndexChanged">
					<asp:ListItem Value="0"><- Select -></asp:ListItem>
				</asp:DropDownList>
			</div>
			<div class="col-md-2 form-group" id="dhPanel" runat="server" visible="false">
				<label>Select District Head :*</label>
				<asp:DropDownList ID="ddrDh" runat="server" CssClass="form-control" Width="100%">
					<asp:ListItem Value="0"><- Select -></asp:ListItem>
				</asp:DropDownList>
			</div>
			<div class="col-md-2 form-group">
				<span style="height:6px; display:block; width:100%;"></span><br />
				<asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Show Report" OnClick="btnSave_Click" />
			</div>
		</div>
		<span class="space15"></span>
		<div>
			<%= errMsg %>
			<asp:GridView ID="gvOrder" runat="server" 
				CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
				AutoGenerateColumns="false" Width="100%">
				<RowStyle CssClass="" />
				<HeaderStyle CssClass="bg-dark" />
				<AlternatingRowStyle CssClass="alt" />
				<Columns>
					<asp:BoundField DataField="Fk_FranchID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="FranchName" HeaderText="Shop Name">
						<ItemStyle Width="25%" />
					</asp:BoundField>
					<asp:BoundField DataField="FranchShopCode" HeaderText="Shop Code">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="totalOrders" HeaderText="Total Orders">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="totalOrdAmount" HeaderText="Total Order Amount">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="deliveredOrd" HeaderText="Delivered">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="ordAmount" HeaderText="Delivered Order Amount">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="pendingOrd" HeaderText="Pending">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="inprocOrd" HeaderText="Accepted / Inprocess">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="shippedOrd" HeaderText="Shipped">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="rejectedOrd" HeaderText="Rejected">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="rejOrdAmount" HeaderText="Rejected Order Amount">
						<ItemStyle Width="10%" />
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

