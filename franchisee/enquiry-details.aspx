<%@ Page Title="Enquiry Details | Genericart Shopping Franchisee Control Panel" Language="C#" MasterPageFile="~/franchisee/MasterFranchisee.master" AutoEventWireup="true" CodeFile="enquiry-details.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="franchisee_enquiry_details" %>
<%@ MasterType VirtualPath="~/franchisee/MasterFranchisee.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script type="text/javascript">

		window.onload = function () {
			//alert("window load");

			duDatepicker('#<%= txtDate.ClientID %>', {
				auto: true, inline: true, format: 'dd/mm/yyyy',
			});

		    duDatepicker('#<%= txtPickupDate.ClientID %>', {
		        auto: true, inline: true, format: 'dd/mm/yyyy',
		    });
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<h2 class="pgTitle">Enquiry Details</h2>
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
							<button type="button" class="btn btn-sm btn-default" data-toggle="modal" data-target="#modal-lg"><i class="fa fa-eye" aria-hidden="true"></i>
								 <%= ordCustData[0] %>
							</button></span> </td>
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
				<h3 class="large colorLightBlue">Enquiry Details</h3>
			</div>
			<div class="card-body">

				<table class="form_table">
					<tr>
						<td><span class="colorLightBlue">Enquiry Id :</span></td>
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
						<td><span class="formLable bold_weight">Shipment Address :</span></td>
						<td><span class="formLable" style="display: block; width: 60% !important;"><%= ordData[4] %></span> </td>
					</tr>
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
				</table>
			</div>
			<div id="cartProd" runat="server">
				<div class="card-header">
					<h3 class="large colorLightBlue">Medicine Details</h3>
				</div>
				<div class="card-body">
					<%--OrderDetails GridView Start--%>
					<div id="viewOrder" runat="server">
						<span class="space15"></span>
						<div>
							<asp:GridView ID="gvOrderDetails" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None" PageSize="30" AllowPaging="true"
								AutoGenerateColumns="false">
								<RowStyle CssClass="" />
								<AlternatingRowStyle CssClass="alt" />
								<Columns>
									<asp:BoundField DataField="BrandMedicine" HeaderText="Brand Medicine">
										<ItemStyle Width="20%" />
									</asp:BoundField>
									<asp:BoundField DataField="BrandPrice" HeaderText="Brand Price">
										<ItemStyle Width="10%" />
									</asp:BoundField>
									<asp:BoundField DataField="GenericMedicine" HeaderText="Generic Medicine">
										<ItemStyle Width="20%" />
									</asp:BoundField>
									<asp:BoundField DataField="GenericCode" HeaderText="Generic Code">
										<ItemStyle Width="12%" />
									</asp:BoundField>
									<asp:BoundField DataField="GenericPrice" HeaderText="Generic Price">
										<ItemStyle Width="12%" />
									</asp:BoundField>
									<asp:BoundField DataField="SavingAmount" HeaderText="Saving Amount">
										<ItemStyle Width="15%" />
									</asp:BoundField>
									<asp:BoundField DataField="SavingPercent" HeaderText="Net Savings">
										<ItemStyle Width="12%" />
									</asp:BoundField>
								</Columns>
								<EmptyDataTemplate>
									<span class="warning">:(</span>
								</EmptyDataTemplate>
								<PagerStyle CssClass="gvPager" />
							</asp:GridView>
						</div>
					</div>

					<div class="row" id="amountEnq" runat="server" visible="false">
						<div class="col-md-3">
							<label>Enter Final Order Amount :*</label>
							<asp:TextBox ID="txtFinalAmount" runat="server" CssClass="form-control" Width="100%" placeholder="Only Numeric Value"></asp:TextBox>
						</div>
						<div class="col-md-3">
							<span class="space30"></span>
							<asp:Button ID="btnUpdateAmount" runat="server" CssClass="btn btn-md btn-info" Text="Update Amount" OnClick="btnUpdateAmount_Click"  />
						</div>
					</div>
				</div>
				<span class="space50"></span>
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
							<label>Estimated Delivery Date :*</label>
							<asp:TextBox ID="txtDate" runat="server" CssClass="form-control" Width="100%" MaxLength="10" placeholder="Click here to Open Calendar"></asp:TextBox>
						</div>
					</div>
					<span class="space10"></span>
					<div class="form-row">
						<div class="form-group col-md-6">
							<asp:CheckBox ID="chkDelivered" runat="server" TextAlign="Right" Text=" Mark This Enquiry as Delivered ?" AutoPostBack="true" OnClientClick="return confirm('Are you sure to Mark this as delivered?');" OnCheckedChanged="chkDelivered_CheckedChanged" />
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
		</div>
		<span class="space15"></span>
	</div>
	<asp:Button ID="btnAccept" runat="server" CssClass="float-left btn btn-success  mr-lg-1" Text="Accept" OnClientClick="return confirm('Are you sure to Accept?');" OnClick="btnAccept_Click"/>
	<asp:Button ID="btnDispatch" runat="server" CssClass="btn btn-info float-left mr-lg-1" Text="Dispatch Order" OnClick="btnDispatch_Click" />
	<div id="btnReject" runat="server" class="float-left mr-lg-1"><a href="cancel-request-reason-enq.aspx?id=<%= ordId %>" class="btn btn-danger cancel-links" onclientclick="return confirm('Are you sure to Reject?');">Reject</a></div>
	
	 <%--<asp:Button ID="btnReject" runat="server" CssClass="btn btn-danger" Text="Reject" OnClientClick="return confirm('Are you sure to Reject?');" OnClick="btnRejected_Click"/>
	 <a href="enquiry-report.aspx" class="btn btn-outline-dark">Back</a>--%>
	
	
	<asp:Button ID="btnBack" runat="server" CssClass="btn btn-outline-dark float-left mr-lg-1" Text="Back" OnClick="btnBack_Click"  />




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
									<td style="width:30%;"><span class="formLable bold_weight">Name :</span></td>
									<td style="width:70%;"><span class="formLable"><%= ordCustData[0] %></span> </td>
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

