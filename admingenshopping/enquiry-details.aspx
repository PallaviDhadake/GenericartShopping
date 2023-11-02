<%@ Page Title="Enquiry Details | Genericart Shopping Admin Panel" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="enquiry-details.aspx.cs" Inherits="admingenshopping_enquiry_details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
					<div id="ordReturned" runat="server" visible="false">
						<tr>
							<td><span class="formLable bold_weight">Order Return Reason :</span></td>
							<td><span class="formLable" style="color:#ff0000; font-weight:600;"><%= ordData[18] %></span> </td>
						</tr>
					</div>
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
					<%--Suppliers GridView End--%>
				</div>
				<span class="space50"></span>
			</div>
		</div>
		<span class="space15"></span>
	</div>
	<%--<asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" Text="Accept" />
	<asp:Button ID="btnDeny" runat="server" CssClass="btn btn-sm btn-danger" Text="Deny"   />
	--%>
	<asp:Button ID="btnAssignOrder" runat="server" CssClass="btn btn-md btn-info" Text="Assign Enquiry" OnClick="btnAssignOrder_Click"   />
	<a href="<%= rdrUrl %>" class="btn btn-md btn-outline-dark">Back</a>

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
</asp:Content>

