<%@ Page Title="Order Report Master | Genericart Shopping Admin Panel" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="order-reports.aspx.cs" Inherits="admingenshopping_order_reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	
	<%--<script>
		$(document).ready(function () {
			$('[id$=gvOrder]').DataTable({
				columnDefs: [
					 { orderable: false, targets: [0, 1, 2, 3, 5, 4, 6, 8, 9, 10, 11, 12] }
				],
				order: [[0, 'desc']]
			});
		});
	 </script>--%>
	<link rel="stylesheet" href="https://pro.fontawesome.com/releases/v5.10.0/css/all.css" integrity="sha384-AYmEC3Yw5cVb3ZcuHtOA93w35dYTsvhLPVnYs9eStHfGJvOvKxVfELGroGkvsg+p" crossorigin="anonymous"/>
	 <script type="text/javascript">

		 $(document).ready(function () {
			 //var table = $('#datatable').DataTable(
			 //   {
			 //   	processing: true, ajax: {},
			 //   	"language": {
			 //   		processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading..n.</span> '
			 //   	},
			 //   }
			 // );
			 const params = new URLSearchParams(window.location.search);
			 const action = params.get('type');
			 //alert("type:" + action);

			 let gmStatus = 0;

			 switch (action) {
				 //case "null": gmStatus = 0; break;
				 case "new": gmStatus = 1; break;
				 case "new-fav": gmStatus = 111; break;
				 case "accepted": gmStatus = 3; break;
				 case "delivered": gmStatus = 7; break;
				 case "monthly": gmStatus = 222; break;
				 case "denied": gmStatus = 4; break;
				 case "deniedbyshop": gmStatus = 333; break;
				 case "cancelCust": gmStatus = 2; break;
				 case "returned": gmStatus = 10; break;
				 default: gmStatus = 0; break;
			 }
			 $.ajax({
				 url: '../WebServices/adminShoppingWebService.asmx/GetAllOrdersData',
				 data: { orderStatus: gmStatus },
				 method: 'post',
				 dataType: 'json',
				 success: function (data) {
					 //alert(data);
					 $('#datatable').dataTable({
						 data: data,
						 "order": [[0, "desc"]],
						 columns: [
							 { 'data': 'OrderID', "visible": false },
							 { 'data': 'OrderStatus', "visible": false },
							 { 'data': 'FK_OrderCustomerID', "visible": false },
							 { 'data': 'orStatus', "visible": false },
							 { 'data': 'OrderID', 'sortable': false },
							 { 'data': 'ordDate', 'sortable': false },
							 { 'data': 'CustomerName', 'sortable': false },
							 { 'data': 'CustomerMobile', 'sortable': false },
							 { 'data': 'OrdAmount', 'sortable': false },
							 { 'data': 'ProductCount', 'sortable': false },
							 { 'data': 'CartProducts', 'sortable': false },
							 {
								 'sortable': false,
								 'render': function (data, type, row, meta) {
									 let gmStatus = row.OrderStatus;
									 let favShopOrder = "<br/><span style=\"font-size:0.8em; color:#fa0ac2; line-height:0.5; font-weight:600;\">" + row.orStatus + "</span>";
									 
									 switch (gmStatus) {

										 case 1:
											 return "<div class=\"ordNew\">New</div>" + favShopOrder;
											 break;
										 case 2:
											 return "<div class=\"ordCancCust\">Cancel By Customer</div>" + favShopOrder;
											 break;
										 case 3:
											 return "<div class=\"ordAccepted\">Accepted</div>" + favShopOrder;
											 break;
										 case 4:
											 return "<div class=\"ordDenied\">Denied By Admin</div>" + favShopOrder;
											 break;
										 case 5:
											 return "<div class=\"ordProcessing\">Processing</div>" + favShopOrder;
											 break;
										 case 6:
											 return "<div class=\"ordShipped\">Shipped</div>" + favShopOrder;
											 break;
										 case 7:
											 return "<div class=\"ordDelivered\">Delivered</div>" + favShopOrder;
											 break;
										 case 8:
											 return "<div class=\"ordDenied\">Rejected By GMMH0001</div>" + favShopOrder;
											 break;
										 case 9:
											 return "<div class=\"ordProcessing\">Rejected by shop - Order Amount Low</div>" + favShopOrder;
											 break;
										 case 10:
											 return "<div class=\"ordDenied\">Returned By Customer</div>" + favShopOrder;
											 break;
                                         case 11:
                                             return "<div class=\"ordDenied\">Rejected By Doctor</div>" + favShopOrder;
                                             break;
                                         case 12:
                                             return "<div class=\"ordDenied\">No Response to Call</div>" + favShopOrder;
											 break;
                                         case 13:
                                             return "<div class=\"ordDenied\">Refund Request by Customer</div>" + favShopOrder;
                                             break;
                                         case 14:
                                             return "<div class=\"ordDenied\">Refund Request in Process</div>" + favShopOrder;
                                             break;
                                         case 15:
                                             return "<div class=\"ordDenied\">Refund Request Completed</div>" + favShopOrder;
                                             break;
                                         default:
                                             return "<div class=\"ordDenied\">NA</div>";

									 }
								 }
							 },
							 { 'data': 'DeviceType', 'sortable': false },
							 {
								 'sortable': false,
								 'render': function (data, type, row, meta) {
									 return '<a href=order-details.aspx?id=' + row.OrderID + ' class="gAnch" ><a/>';
								 }
							 }
						 ],
						 //serverSide: true,
						 "processing": true,
						     "language": {
						     "loadingRecords": '<i class="fa fa-spinner fa-spin fa-2x fa-fw"></i><span class="sr-only">Loading...</span>',
						     "processing": '<i class="fa fa-spinner fa-spin fa-2x fa-fw"></i><span class="sr-only">Loading...</span>',
						     "infoFiltered": ""
						 }
					 })
				 }
			 });
			 

		 });


     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<h2 class="pgTitle">Order Report Master</h2>
	<span class="greyLine"></span>
    <span class="medium themeClrBlue bold_weight ml-2">Date Range: (<%= fyDateRange %>)</span>
	<span class="space15"></span>
	<%--Suppliers GridView Start--%>
	<div id="viewOrder" runat="server">
		<span class="space15"></span>
		<table id="datatable" class="table table-striped table-bordered table-hover w-100">
			<thead class="thead-dark">
				<tr>
					<th>OrderID</th>
					<th>OrderStatus</th>
					<th>FK_OrderCustomerID</th>
					<th>orStatus</th>
					<th>Order ID</th>
					<th>Date</th>
					<th>Customer Name</th>
					<th>Customer Mobile</th>
					<th>Order Amount</th>
					<th>Product Count</th>
					<th>Cart Products</th>
					<th>Status</th>
					<th>DeviceType</th>
					<th></th>
				</tr>
			</thead>
		</table>
		<%--<div>
			<asp:GridView ID="gvOrder" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
				AutoGenerateColumns="false" OnRowDataBound="gvOrder_RowDataBound" Width="100%">
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
					<asp:BoundField DataField="FK_OrderCustomerID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="OrderID" HeaderText="Order ID">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="ordDate" HeaderText="Date">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="CustomerName" HeaderText="Name">
						<ItemStyle Width="15%" />
					</asp:BoundField>
					<asp:BoundField DataField="CustomerMobile" HeaderText="Mobile No.">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="OrdAmount" HeaderText="Amount">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="ProductCount" HeaderText="No. of Product">
						<ItemStyle Width="12%" />
					</asp:BoundField>
					<asp:BoundField DataField="CartProducts" HeaderText="Products">
						<ItemStyle Width="25%" />
					</asp:BoundField>
					<asp:TemplateField HeaderText="Status">
						<ItemStyle Width="10%" />
						<ItemTemplate>
							<asp:Literal ID="litStatus" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="DeviceType" HeaderText="Order From">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:TemplateField>
						<ItemStyle Width="5%" />
						<ItemTemplate>
							<asp:Literal ID="litAnch" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
				</Columns>
				<EmptyDataTemplate>
					<span class="warning">No Orders to Display :(</span>
				</EmptyDataTemplate>
				<PagerStyle CssClass="gvPager" />
			</asp:GridView>
		</div>--%>
	</div>
	<%--Suppliers GridView End--%>
</asp:Content>

