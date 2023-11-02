<%@ Page Title="Online Payment Report" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="online-payment-report.aspx.cs" Inherits="admingenshopping_online_payment_report" %>
<%@ MasterType VirtualPath="~/admingenshopping/MasterAdmin.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script type="text/javascript">
		$(document).ready(function () {
			var table = $('#datatable').DataTable({
				responsive: false,
				"order": [[0, "desc"]],
				columns: [
					{ 'data': 'OrderID', "visible": false },
					{ 'data': 'OrderStatus', "visible": false },
					{ 'data': 'OrderID', 'sortable': false },
					//{
					//	'sortable': false,
					//	'render': function (data, type, row, meta) {
					//		return '<a href=order-details.aspx?id=' + row.OrderID + ' class="" target=\"_blank\">' + row.OrderID + '<a/>';
					//	}
					//},
					{ 'data': 'ordDate', 'sortable': false },
					{ 'data': 'custInfo', 'sortable': false },
					{ 'data': 'ordAmount', 'sortable': false },
					{ 'data': 'ordPaidAmount', 'sortable': false },
					{ 'data': 'FranchName', 'sortable': false, },
					{ 'data': 'FranchShopcode', 'sortable': false, },
					//{ 'data': 'Shopstatus', 'sortable': false, },
					{
						'sortable': false,
						'render': function (data, type, row, meta) {

							// Code by Vinayak 12-May-2022 (If "OrderStatus" found "2" i.e. Cancelled By Customer into OrdersData table, then just ignore ShopStatus and return)
							let orderStatus = row.OrderStatus;
							if (orderStatus == 2) {
								return "<div class=\"ordDenied\">Cancel By Customer</div>";
							}

							let shStatus = row.Shopstatus;

							switch (shStatus) {

								case "Pending":
									return "<div class=\"ordNew\">" + shStatus + "</div>";
									break;
								case "Accepted":
									return "<div class=\"ordAccepted\">" + shStatus + "</div>";
									break;
								case "Rejected":
									return "<div class=\"ordDenied\">" + shStatus + "</div>";
									break;
								case "Inprocess":
									return "<div class=\"ordProcessing\">" + shStatus + "</div>";
									break;
								case "Shipped":
									return "<div class=\"ordShipped\">" + shStatus + "</div>";
									break;
								case "Delivered":
									return "<div class=\"ordDelivered\">" + shStatus + "</div>";
									break;
							}



						}
					},
					{ 'data': 'OrderPaymentTxnId', 'sortable': false, },
					{ 'data': 'OPL_transtatus', 'sortable': false, },
					{ 'data': 'OLP_device_type', 'sortable': false, },
				],
				bServerSide: true,
				sAjaxSource: '../WebServices/adminShoppingWebService.asmx/GetOnlinePayReport',
				sServerMethod: 'post',
				"processing": true,
				'language': {
					'loadingRecords': '&nbsp;',
					'processing': '<div class="spinner"><span class="sr-only">Loading...</span></div>',
					"infoFiltered": ""
				}
			});
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<h2 class="pgTitle">Online Payment Report</h2>
	<span class="space15"></span>
	<div id="viewOrder" runat="server">
		<table id="datatable" class="table table-striped table-bordered table-hover w-100">
			<thead class="thead-dark">
				<tr>
					<th>OrderID</th>
					<th>OrderStatus</th>
					<th>Order ID</th>
					<th>Date</th>
					<th>Customer</th>
					<th>Order Amount</th>
					<th>Paid Amount</th>
					<th>Shop Name</th>
					<th>Shop Code</th>
					<th>Status</th>
					<th>Transction ID</th>
					<th>Transction Status</th>
					<th>Paid From</th>
				</tr>
			</thead>
		</table>
		<%--<div>
			<asp:GridView ID="gvOrder" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
				AutoGenerateColumns="false" Width="100%">
				<RowStyle CssClass="" />
				<HeaderStyle CssClass="bg-dark" />
				<AlternatingRowStyle CssClass="alt" />
				<Columns>
					<asp:BoundField DataField="OrderID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="OrderStatus">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="OrderID" HeaderText="Order ID">
						<ItemStyle Width="8%" />
					</asp:BoundField>
					<asp:BoundField DataField="ordDate" HeaderText="Order Date">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="custInfo" HeaderText="Name">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="ordAmount" HeaderText="Order Amount">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="FranchName" HeaderText="Shop Name">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="FranchShopcode" HeaderText="Shop Code">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="Shopstatus" HeaderText="Shop Status">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="OrderPaymentTxnId" HeaderText="Transction ID">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="OPL_transtatus" HeaderText="Transction Status">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="OLP_device_type" HeaderText="Pay From">
						<ItemStyle Width="5%" />
					</asp:BoundField> 
				</Columns>
				<EmptyDataTemplate>
					<span class="warning">No data to Display :(</span>
				</EmptyDataTemplate>
				<PagerStyle CssClass="gvPager" />
			</asp:GridView>
		</div>--%>
	</div>
</asp:Content>

