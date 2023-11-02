﻿<%@ Page Title="Online Payment Report" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="online-payment-report.aspx.cs" Inherits="supportteam_online_payment_report" %>
<%@ MasterType VirtualPath="~/supportteam/MasterSupport.master" %>
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
	</div>
	<span class="space10"></span>	
</asp:Content>

