<%@ Page Title="Order Details | Genericart Shopping Franchisee Control Panel" Language="C#" MasterPageFile="~/franchisee/MasterFranchisee.master" AutoEventWireup="true" CodeFile="order-details.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="franchisee_order_details" %>
<%@ MasterType VirtualPath="~/franchisee/MasterFranchisee.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

	<%--<script src="https://cdn.jsdelivr.net/npm/sweetalert2@10"></script>
	<script src="../admingenshopping/plugins/sweetalert2/sweetalert2.min.js"></script>
	<link href="../admingenshopping/plugins/sweetalert2/sweetalert2.min.css" rel="stylesheet" />--%>
	<style>
		.box {
			position: relative;
		}

		.total_price {
			position: absolute;
			bottom: 0;
			right: 0;
			padding-right: 50px;
		}
		.coins {display:block; background:url(../images/icons/coin.png) no-repeat left center; padding:10px 0px 10px 32px; font-size:16px; font-weight:700;margin-left:25px}
		.shipping {display:block; background:url(../images/icons/delivery-truck.png) no-repeat left center; padding:10px 0px 10px 32px; font-size:16px; font-weight:700;margin-left:25px}

		.form-control1 {
		    display: block;
		    width: 100%;
		    padding: .375rem .75rem;
		    font-size: 1rem;
		    line-height: 1.5;
		    color: #000;
		    background-color: #dfd74f;
		    background-clip: padding-box;
		    border: 1px solid #ced4da;
		    border-radius: .25rem;
		    transition: border-color .15s ease-in-out,box-shadow .15s ease-in-out
		}
	</style>

	<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />
	<link rel="stylesheet" href="/resources/demos/style.css" />
	<script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
	<script type="text/javascript">
		$(function () {
			//alert("get med load call");
			$("#<%= txtMedName.ClientID %>").autocomplete({

				source: function (request, response) {
					$.ajax({
						url: '<%= ResolveUrl("../WebServices.aspx/GetSearchControl") %>',
						data: "{ 'prefix': '" + request.term + "'}",
						dataType: "json",
						type: "POST",
						contentType: "application/json; charset=utf-8",
						success: function (data) {
							response($.map(data.d, function (item) {
								return {
									label: item.split('#')[0],
									val: item.split('#')[1]
								}
							}))
						},

					});
				},
			});
		});
	</script>

	<script type="text/javascript">

		window.onload = function () {
			//alert("window load");

			duDatepicker('#<%= txtDate.ClientID %>', {
				auto: true, inline: true, format: 'dd/mm/yyyy',
			});

			duDatepicker('#<%= txtDeliveryDate.ClientID%>', {
				auto: true, inline: true, format: 'dd/mm/yyyy',
			});

			duDatepicker('#<%= txtPickupDate.ClientID %>', {
				auto: true, inline: true, format: 'dd/mm/yyyy',
			});
		}
	</script>
	<%--<script>
		$(document).ready(function () {
			$('[id$=gvOrderDetails]').DataTable({
				columnDefs: [
					 { orderable: false, targets: [0, 1, 2, 3, 5, 4, 6, 7] }
				],
				order: [[0, 'desc']]
			});
		});
	 </script>--%>
	<script type="text/javascript">
		$(function () {
			var prm = Sys.WebForms.PageRequestManager.getInstance();
			if (prm != null) {
				prm.add_endRequest(function (sender, e) {
					if (sender._postBackSettings.panelsToUpdate != null) {
						createDataTable();
					}
				});
			};
			
			createDataTable();
			function createDataTable() {
				$('#<%= gvOrderDetails.ClientID %>').prepend($("<thead></thead>").append($('#<%= gvOrderDetails.ClientID %>').find("tr:first"))).DataTable({

					columnDefs: [
						{ orderable: false, targets: [0, 1, 2, 3, 5, 4, 6, 7] }
					],
					order: [[0, 'desc']]
				});

			}
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
                <span class="badge badge-pill badge-info text-lg"><%= gobpName %></span>
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
						<td style="width: 25%"><span class="formLable bold_weight">Sale Bill No. :</span></td>
						<td><span class="formLable"><%=ordData[21] %></span></td>
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
						<td><span class="formLable" style="display: block; width: 80% !important;"><%= ordData[4] %></span> </td>
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
						<td style="vertical-align:top;"><span class="formLable bold_weight">Payment Status :</span></td>
						<td style="width:80% !important">
							<span class="formLable"><%= ordData[13] %></span> <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="btn btn-sm btn-info ml-2" Visible="false" OnClick="btnRefresh_Click" /> <br /> <%= apiResponse %>
							<div id="offlinePay" runat="server" visible="false">
								<span class="space10"></span>
								<span class="text-indigo text-bold mb-1">Offline Payment Details</span>
								<div class="form-row">
									<div class="col-md-4">
										<asp:RadioButton ID="chkQR" runat="server" GroupName="PayMode" TextAlign="Right" Text=" Paid By QR Code" />
									</div>
									<div class="col-md-4">
										<asp:RadioButton ID="chkGPay" runat="server" GroupName="PayMode" TextAlign="Right" Text=" Paid By GPay" />
									</div>
									<div class="col-md-4">
										<asp:RadioButton ID="chkCash" runat="server" GroupName="PayMode" TextAlign="Right" Text=" Paid By Cash" />
									</div>
								</div>
								<div class="form-row">
									<div class="col-md-6">
										<asp:TextBox ID="txtUTRNo" runat="server" CssClass="form-control" Width="100%" placeholder="Enter UTR Number"></asp:TextBox>
									</div>
									<div class="col-md-6">
										<asp:Button ID="btnAddOfflinePayment" runat="server" Text="Mark as Paid" CssClass="btn btn-md btn-dark" OnClick="btnAddOfflinePayment_Click" />
									</div>
								</div>
							</div>
						</td>
					</tr>

					<tr>
						<td><span class="formLable bold_weight">UPI ID / Transaction No. :</span></td>
						<td><span class="formLable"><%=ordData[20] %></span></td>
					</tr>

					<tr>
						<td><span class="formLable bold_weight">Order Note :</span></td>
						<td><span class="formLable"><%= ordData[14] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Delivery Type :</span></td>
						<td><span class="formLable"><%= ordData[16] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Courier Option :</span></td>
						<td><span class="formLable"><%=ordData[23] %></span></td>
					</tr>


					<div id="ordCanc" runat="server" visible="false">
						<tr>
							<td><span class="formLable bold_weight">Order Cancel Reason :</span></td>
							<td><span class="formLable" style="color:#ff0000; font-weight:600;"><%= ordData[17] %></span> </td>
						</tr>
					</div>
				</table>
			</div>

			<div id="medUpdate" runat="server" visible="false">
				<div class="pad_20">
					<div class="form-row">
						<div class="form-group col-md-6">
							<label>Medicine Name :*</label>
							<asp:TextBox ID="txtMedName" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
						</div>
						<div class="form-group col-md-3">
							<label>Quantity :*</label>
							<asp:DropDownList ID="ddrQty" runat="server" CssClass="form-control">
								<asp:ListItem Value="0"><- Select -></asp:ListItem>
							</asp:DropDownList>
						</div>
						<div class="form-group col-md-3">
							<span class="space30"></span>
							<span class="space5"></span>
							<asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary mr-lg-1" Text="ADD" OnClick="btnAdd_Click"  />
						</div>
					</div>
				</div>
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
								AutoGenerateColumns="false" OnRowDataBound="gvOrderDetails_RowDataBound" OnRowCommand="gvOrderDetails_RowCommand">
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
									<asp:TemplateField HeaderText="">
										<ItemStyle Width="5%" />
										<ItemTemplate>
											<asp:Button ID="cmdDelete" runat="server" CssClass="gDel" CommandName="gvDel" Text="" OnClientClick="return confirm('Are you sure to delete?');" />
										</ItemTemplate>
									</asp:TemplateField>
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
					<span class="coins">Coins Consume: <%= ordData[18] %></span>
					<span class="shipping">Shipping Charges: <%= ordData[19] %></span>
				</div>
			</div>

			<%= prescriptionStr %>

			<%= paymentslip %>

			<div id="rxAmount" runat="server">
				<div class="pad_20">
					<div class="form-row">
						<div class="form-group col-md-6">
							<label>Order Amount :*</label>
							<asp:TextBox ID="txtRxOrdAmount" runat="server" CssClass="form-control" Width="100%" MaxLength="100"></asp:TextBox>
						</div>
					</div>
				</div>
			</div>

			<!-- =============== PickUp Request Section Starts ==================== -->
			<div id="pickupReq" runat="server" visible="false">
				<span class="lineSpace"></span>
				<div class="card-header">
					<h3 class="colorLightBlue">Bluedart Pickup Request</h3>
				</div>
				<div class="card-body">
					<div class="form-row">
						<div class="form-group col-md-4">
							<label>Select Payment Type :*</label>
							<asp:DropDownList ID="ddrPay" runat="server" CssClass="form-control">
								<asp:ListItem Value="0"><- Select -></asp:ListItem>
								<asp:ListItem Value="1">Online Payment</asp:ListItem>
								<asp:ListItem Value="2">Cash On Delivery</asp:ListItem>
							</asp:DropDownList>
						</div>
					</div>
					<div class="form-row">
						<div class="form-group col-md-3">
							<label>Enter Parcel Weight in KG :*</label>
							<asp:TextBox ID="txtWeight" runat="server" CssClass="form-control" Width="100%" placeholder="Only Numeric Value"></asp:TextBox>
						</div>
						<div class="form-group col-md-3">
							<label>Enter Parcel Width in cm :*</label>
							<asp:TextBox ID="txtWidth" runat="server" CssClass="form-control" Width="100%" placeholder="Only Numeric Value"></asp:TextBox>
						</div>
						<div class="form-group col-md-3">
							<label>Enter Parcel Height in cm :*</label>
							<asp:TextBox ID="txtHeight" runat="server" CssClass="form-control" Width="100%" placeholder="Only Numeric Value"></asp:TextBox>
						</div>
						<div class="form-group col-md-3">
							<label>Enter Parcel Breadth in cm :*</label>
							<asp:TextBox ID="txtBreadth" runat="server" CssClass="form-control" Width="100%" placeholder="Only Numeric Value"></asp:TextBox>
						</div>
					</div>
					<div class="form-row">
						<div class="form-group col-md-4">
							<label>Shipment Pickup Date :*</label>
							<asp:TextBox ID="txtPickupDate" runat="server" CssClass="form-control" Width="100%" MaxLength="10" placeholder="Click here to Open Calendar"></asp:TextBox>
						</div>
						<div class="form-group col-md-4">
							<label>Shipment Pickup Time :*</label>
							<asp:TextBox ID="txtPickupTime" runat="server" CssClass="form-control" Width="100%" MaxLength="5" placeholder="ex. 01:00 (24 Hr format)"></asp:TextBox>
						</div>
						<div class="form-group col-md-6">
							<asp:Button ID="btnSendReq" runat="server" CssClass="btn btn-md btn-info" Text="Send PickUp Request" OnClick="btnSendReq_Click" />
						</div>
					</div>
					<%= pickupReqStatus %>
				</div>
				<span class="lineSpace"></span>
			</div>
			<!-- =============== PickUp Request Section Ends ==================== -->

			<div id="dispatchForm" runat="server">
				<div class="card-header">
					<h3 class="large colorLightBlue" id="dispatch">Add Dispatch Details</h3>
				</div>
				<div class="card-body">
					<div class="form-row">
						<div class="form-group col-md-6">
							<label>Shipment / Courier Name :*</label>
							<asp:TextBox ID="txtShipmentName" runat="server" CssClass="form-control" Width="100%" MaxLength="100"></asp:TextBox>
						</div>
						<div class="form-group col-md-6">
							<label>Tracking No. :*</label>
							<asp:TextBox ID="txtTrackingNo" runat="server" CssClass="form-control" Width="100%" MaxLength="100"></asp:TextBox>
						</div>
						<div class="form-group col-md-6">
							<label>Dispatched Date :*</label>
							<asp:TextBox ID="txtDate" runat="server" CssClass="form-control" Width="100%" MaxLength="10" placeholder="Click here to Open Calender"></asp:TextBox>
						</div>
						<div class="form-group col-md-6">
							<label>Delivery Date :*</label>
							<asp:TextBox ID="txtDeliveryDate" runat="server" CssClass="form-control" Width="100%" MaxLength="10" placeholder="Click here to Open Calendar"></asp:TextBox>
						</div>
						<div class="form-group col-md-6">
							<label>Sale Bill No. </label>
							<asp:TextBox ID="txtSaleBill" runat="server" CssClass="form-control1" Width="100%" MaxLength="50" placeholder="Sale Bill No."></asp:TextBox>
						</div>
					</div>
					<span class="space10"></span>
					<div class="form-row">
						<div class="form-group col-md-6">
							<asp:CheckBox ID="chkDelivered" runat="server" TextAlign="Right" Text=" Mark This Order as Delivered ?" AutoPostBack="true" OnClientClick="return confirm('Are you sure to Mark this as delivered?');" OnCheckedChanged="chkDelivered_CheckedChanged" />
							<%--<label class="form-check-label"><strong>Mark This Order as Delivered ?</strong> </label>--%>
						</div>
					</div>
					<%--<asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Submit Dispatch Details"  />--%>
				</div>
			</div>

			<!-- ======================= Add return details starts ============================= -->
			<div id="returnOrd" runat="server">
				<div class="card-header">
					<h3 class="large colorLightBlue" id="retOrd">Add Order Return Details (if any)</h3>
				</div>
				<div class="card-body">
					<div class="form-row">
						<div class="form-group col-md-6">
							<label>Return Reason :*</label>
							<asp:TextBox ID="txtRetReason" runat="server" CssClass="form-control textarea" Width="100%" TextMode="MultiLine" Height="120"></asp:TextBox>
						</div>
					</div>
					<span class="space10"></span>
					<asp:Button ID="btnRet" runat="server" CssClass="btn btn-primary" Text="Mark as Returned" OnClick="btnRet_Click"  />
				</div>
			</div>
			<!-- ======================= Add return details ends ============================= -->
			<%--<div id="cancReason" runat="server" visible="false">
				<div class="card-header">
					<h3 class="large colorLightBlue">Order Rejection Detail</h3>
				</div>
				<div class="card-body">
					<div class="form-row">
						<div class="form-group col-md-6">
							<label>Please Select Reason to Cancel Order :*</label>
							<asp:DropDownList ID="ddrReasons" runat="server" CssClass="form-control">
								<asp:ListItem Value="0"><- Select -></asp:ListItem>
							</asp:DropDownList>
						</div>
					</div>
					<asp:Button ID="btnCancOrder" runat="server" CssClass="btn btn-primary" Text="Submit Reason" OnClick="btnCancOrder_Click" />
				</div>
			</div>--%>
		</div>
		<span class="space15"></span>
	</div>


	<%--OnClick="btnAccept_Click"--%>
	<asp:Button ID="btnAccept" runat="server" CssClass="btn btn-success float-left mr-lg-1" Text="Accept" OnClientClick="return confirm('Are you sure to Accept?');" OnClick="btnAccept_Click" />
	<asp:Button ID="btnDispatch" runat="server" CssClass="btn btn-info float-left mr-lg-1" Text="Dispatch Order" OnClick="btnDispatch_Click" />
	<asp:Button ID="btnPrint" runat="server" CssClass="btn btn-info float-left mr-lg-1" Text="Print" OnClick="btnPrint_Click" />
	<%--<asp:Button ID="btnReject" runat="server" CssClass="btn btn-danger" Text="Reject" OnClientClick="return confirm('Are you sure to Reject?');" OnClick="btnRejected_Click"/>--%>

	<div id="btnReject" runat="server" class="float-left mr-lg-1"><a href="cancel-request-reason.aspx?id=<%= ordId %>" class="btn btn-danger cancel-links" onclientclick="return confirm('Are you sure to Reject?');">Reject</a></div>
	
	<asp:Button ID="btnBack" runat="server" CssClass="btn btn-outline-dark float-left mr-lg-1" Text="Back" OnClick="btnBack_Click"  />


	<%--customer information modal--%>
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

	<script type="text/javascript">
		$(document).ready(function () {
			$("a.cancel-links").fancybox({
				type: 'iframe'

			});
		});
	</script>
</asp:Content>

