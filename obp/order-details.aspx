<%@ Page Title="Order Details" Language="C#" MasterPageFile="~/obp/MasterObp.master" AutoEventWireup="true" CodeFile="order-details.aspx.cs" Inherits="obp_order_details" %>

<%@ MasterType VirtualPath="~/obp/MasterObp.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<style>
		.box {
			position: relative;
		}

		.total_price {
			position: absolute;
			bottom: -10px;
			right: 0;
			padding-right: 50px;
		}
	</style>
	<script>
		$(document).ready(function () {
			$('[id$=gvOrderDetails]').DataTable({
				columnDefs: [
					 { orderable: false, targets: [0, 1, 2, 3, 5, 4, 6, 7] }
				],
				order: [[0, 'desc']]
			});
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<h2 class="pgTitle">Order Details</h2>
	<span class="space15"></span>

	<div id="Div1" runat="server">
		<div class="card">
			<div class="card-header">
				<span class="badge badge-pill badge-success text-lg"><%= deviceType %></span>
				<h3 class="large colorLightBlue">Customer Details</h3>
			</div>
			<div class="card-body">
				<%= mreq %>
				<table class="form_table">
					<tr>
						<td><span class="formLable bold_weight">Customer Name :</span></td>
						<td><span class="formLable">
							<button type="button" class="btn btn-sm btn-default" data-toggle="modal" data-target="#modal-lg">
								<i class="fa fa-eye" aria-hidden="true"></i>
								<%= ordCustData[0] %>
							</button>
						</span></td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Customer Mobile No :</span></td>
						<td><span class="formLable"><%= ordCustData[1] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Customer Email :</span></td>
						<td><span class="formLable"><%= ordCustData[2] %></span> </td>
					</tr>
				</table>
			</div>
			<div class="card-header">
				<h3 class="large colorLightBlue">Order Details</h3>
			</div>
			<div class="card-body">

				<table class="form_table">
					<tr>
						<td><span class="colorLightBlue">Order Id :</span></td>
						<td>
							<asp:Label ID="Label1" CssClass="colorLightBlue" runat="server" Text=""><%= ordData[0] %></asp:Label>
						</td>
					</tr>
					<tr>
						<td style="width: 25%"><span class="formLable bold_weight">Date:</span></td>
						<td><span class="formLable"><%= ordData[1] %></span> </td>
					</tr>
					<tr>
						<td style="width: 25%"><span class="formLable bold_weight">Time:</span></td>
						<td><span class="formLable"><%= ordData[11] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Shipment Name :</span></td>
						<td><span class="formLable"><%= ordData[3] %></span> </td>
					</tr>

					<tr>
						<td><span class="formLable bold_weight">Shipment Address :</span></td>
						<td><span class="formLable" style="display: block; width: 60% !important;"><%= ordData[4] %></span> </td>
					</tr>
					<%--<tr>
						<td><span class="formLable bold_weight">Shipment Address 2 :</span></td>
						<td><span class="formLable"><%= ordData[5] %></span> </td>
					</tr>--%>
					<tr>
						<td><span class="formLable bold_weight">City :</span></td>
						<td><span class="formLable" style="display: block; width: 60% !important;"><%= ordData[6] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">State :</span></td>
						<td><span class="formLable" style="display: block; width: 60% !important;"><%= ordData[7] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Zip Code :</span></td>
						<td><span class="formLable"><%= ordData[8] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Country :</span></td>
						<td><span class="formLable"><%= ordData[9] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Contact Info :</span></td>
						<td><span class="formLable"><%= ordData[10] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Payment Mode :</span></td>
						<td><span class="formLable"><%= ordData[12] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Payment Status :</span></td>
						<td><span class="formLable"><%= ordData[13] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Order Note :</span></td>
						<td><span class="formLable"><%= ordData[14] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Delivery Type :</span></td>
						<td><span class="formLable"><%= ordData[16] %></span> </td>
					</tr>
					<div id="ordCanc" runat="server" visible="false">
						<tr>
							<td><span class="formLable bold_weight">Order Cancel Reason :</span></td>
							<td><span class="formLable" style="color: #ff0000; font-weight: 600;"><%= ordData[17] %></span> </td>
						</tr>
					</div>
					<div id="ordReturned" runat="server" visible="false">
						<tr>
							<td><span class="formLable bold_weight">Order Return Reason :</span></td>
							<td><span class="formLable" style="color: #ff0000; font-weight: 600;"><%= ordData[18] %></span> </td>
						</tr>
					</div>
				</table>
			</div>
			<div id="cartProd" runat="server">
				<div class="card-header">
					<h3 class="large colorLightBlue">Product Details</h3>
				</div>
				<div class="card-body">
					<%--OrderDetails GridView Start--%>
					<div id="viewOrder" runat="server">
						<span class="space15"></span>
						<div>
							<asp:GridView ID="gvOrderDetails" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
								AutoGenerateColumns="false" OnRowDataBound="gvOrderDetails_RowDataBound">
								<RowStyle CssClass="" />
								<HeaderStyle CssClass="bg-dark" />
								<AlternatingRowStyle CssClass="alt" />
								<Columns>
									<asp:BoundField DataField="OrdDetailID">
										<HeaderStyle CssClass="HideCol" />
										<ItemStyle CssClass="HideCol" />
									</asp:BoundField>
									<asp:BoundField DataField="FK_DetailProductID">
										<HeaderStyle CssClass="HideCol" />
										<ItemStyle CssClass="HideCol" />
									</asp:BoundField>
									<asp:BoundField DataField="ProductName" HeaderText="Product Name">
										<ItemStyle Width="30%" />
									</asp:BoundField>
									<asp:BoundField DataField="OrdDetailSKU" HeaderText="Product Code">
										<ItemStyle Width="15%" />
									</asp:BoundField>
									<asp:BoundField DataField="OrdDetailQTY" HeaderText="Quantity">
										<ItemStyle Width="15%" />
									</asp:BoundField>
									<asp:BoundField DataField="OrigPrice" HeaderText="Price">
										<ItemStyle Width="20%" />
									</asp:BoundField>
									<asp:BoundField DataField="OrdAmount" HeaderText="Amount">
										<ItemStyle Width="20%" />
									</asp:BoundField>
								</Columns>
								<EmptyDataTemplate>
									<span class="warning">:(</span>
								</EmptyDataTemplate>
								<PagerStyle CssClass="gvPager" />
							</asp:GridView>
						</div>
					</div>
					<%--Suppliers GridView End--%>
				</div>
				<span class="space50"></span>
				<span class="space10"></span>
				<div class="box row-fluid">
					<p class="total_price" style="text-align: right !important">
						<b>Sub Total = &#8377; <%= ordData[15] %></b>
						<br />
						<%--<b><%= shippingCharges %></b>--%>
						<b>Total Amount = &#8377; <%= ordData[2] %></b>
					</p>
				</div>
			</div>
			<%= prescriptionStr %>
		</div>
		<span class="space15"></span>
	</div>
	<a href="order-reports.aspx" class="btn btn-sm btn-outline-dark">Back</a>



	<div class="modal fade" id="modal-lg">
		<div class="modal-dialog modal-lg">
			<div class="modal-content">
				<div class="modal-header">
					<h4 class="modal-title">Customer Details</h4>
					<button type="button" class="close" data-dismiss="modal" aria-label="Close">
						<span aria-hidden="true">&times;</span>
					</button>
				</div>
				<div class="modal-body">
					<div class="row">
						<div class="col-sm-6">
							<table class="form_table">
								<tr>
									<td style="width: 30%;"><span class="formLable bold_weight">Name :</span></td>
									<td style="width: 70%;"><span class="formLable"><%= ordCustData[0] %></span> </td>
								</tr>
								<tr>
									<td><span class="formLable bold_weight">Mobile No :</span></td>
									<td><span class="formLable"><%= ordCustData[1] %></span> </td>
								</tr>
								<tr>
									<td><span class="formLable bold_weight">Email :</span></td>
									<td><span class="formLable"><%= ordCustData[2] %></span> </td>
								</tr>
								<tr>
									<td><span class="formLable bold_weight">City :</span></td>
									<td><span class="formLable"><%= ordCustData[3] %></span> </td>
								</tr>
								<tr>
									<td><span class="formLable bold_weight">State :</span></td>
									<td><span class="formLable"><%= ordCustData[4] %></span> </td>
								</tr>
								<tr>
									<td><span class="formLable bold_weight">Zip Code :</span></td>
									<td><span class="formLable"><%= ordCustData[5] %></span> </td>
								</tr>
								<tr>
									<td><span class="formLable bold_weight">Address :</span></td>
									<td><span class="formLable"><%= ordCustData[6] %></span> </td>
								</tr>
							</table>
						</div>

						<div class="col-sm-6">
							<table>
								<tr>
									<td><span class="formLable bold_weight text-primary">Total Orders :</span></td>
									<td><span class="formLable text-primary"><%= orderCount %></span> </td>
								</tr>
							</table>
						</div>
					</div>

				</div>
				<div class="modal-footer justify-content-between">
					<%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
					<button type="button" class="btn btn-primary">Save changes</button>--%>
				</div>
			</div>
			<!-- /.modal-content -->
		</div>
		<!-- /.modal-dialog -->
	</div>
	<!-- /.modal -->
</asp:Content>

