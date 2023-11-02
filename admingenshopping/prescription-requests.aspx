<%@ Page Title="Prescription Requests" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="prescription-requests.aspx.cs" Inherits="admingenshopping_prescription_requests" %>
<%@ MasterType VirtualPath="~/admingenshopping/MasterAdmin.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script>
		$(document).ready(function () {
			$('[id$=gvReq]').DataTable({
				columnDefs: [
					 { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10] }
				],
				order: [[0, 'desc']]
			});
		});
	 </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<h2 class="pgTitle">Requested Prescriptions</h2>
	<span class="space15"></span>
	<div id="viewReq" runat="server">
		<div>
			<asp:GridView ID="gvReq" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
				AutoGenerateColumns="false" Width="100%" OnRowDataBound="gvReq_RowDataBound">
				<RowStyle CssClass="" />
				<HeaderStyle CssClass="bg-dark" />
				<AlternatingRowStyle CssClass="alt" />
				<Columns>
					<asp:BoundField DataField="PreReqID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="PreReqStatus">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="FK_CustomerID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="reqDate" HeaderText="Date">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="PreReqName" HeaderText="Name">
						<ItemStyle Width="15%" />
					</asp:BoundField>
					<asp:BoundField DataField="PreReqMobile" HeaderText="Mobile No.">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="PreReqAge" HeaderText="Age">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="PreReqDisease" HeaderText="Disease">
						<ItemStyle Width="25%" />
					</asp:BoundField>
					<asp:TemplateField HeaderText="Status">
						<ItemStyle Width="5%" />
						<ItemTemplate>
							<asp:Literal ID="litStatus" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="DeviceType" HeaderText="Req. From">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:TemplateField>
						<ItemStyle Width="5%" />
						<ItemTemplate>
							<asp:Literal ID="litView" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
				</Columns>
				<EmptyDataTemplate>
					<span class="warning">No Data to Display :(</span>
				</EmptyDataTemplate>
				<PagerStyle CssClass="gvPager" />
			</asp:GridView>
		</div>
	</div>

	<div id="readReq" runat="server">
		<div class="card">
			<div class="card-header">
				<h3 class="large colorLightBlue">Prescription Request Details</h3>
				<span class="badge badge-pill badge-success text-lg"><%= deviceType %></span>
			</div>
			<div class="card-body">

				<table class="form_table">
					<tr>
						<td><span class="colorLightBlue">Request Id :</span></td>
						<td>
							<asp:Label ID="Label1" CssClass="colorLightBlue" runat="server" Text=""><%= ordData[0] %></asp:Label>
						</td>
					</tr>
					<tr>
						<td style="width: 25%"><span class="formLable bold_weight">Date:</span></td>
						<td><span class="formLable"><%= ordData[1] %></span> </td>
					</tr>
					<tr>
						<td style="width: 25%"><span class="formLable bold_weight">Customer Name:</span></td>
						<td><span class="formLable"><%= ordData[2] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Mobile No. :</span></td>
						<td><span class="formLable"><%= ordData[3] %></span> </td>
					</tr>

					<tr>
						<td><span class="formLable bold_weight">Age :</span></td>
						<td><span class="formLable"><%= ordData[4] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Gender :</span></td>
						<td><span class="formLable"><%= ordData[5] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Address :</span></td>
						<td><span class="formLable"><%= ordData[10] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Disease :</span></td>
						<td><span class="formLable"><%= ordData[6] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Medicines :</span></td>
						<td><span class="formLable" style="display: block; width: 60% !important;"><%= ordData[7] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Request For :</span></td>
						<td><span class="formLable"><%= ordData[8] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Request From :</span></td>
						<td><span class="formLable"><%= ordData[9] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Select Doctor :</span></td>
						<td>
							<asp:DropDownList ID="ddrDoc" runat="server" CssClass="form-control">
								<asp:ListItem Value="0"><- Select -></asp:ListItem>
							</asp:DropDownList>
						</td>
					</tr>
				</table>
			</div>
		</div>
		<span class="space15"></span>
		<asp:Button ID="btnAssign" runat="server" CssClass="btn btn-md btn-primary" Text="Assign Prescription" OnClick="btnAssign_Click" />
		<asp:Button ID="btnDeny" runat="server" CssClass="btn btn-md btn-danger" Text="Deny" OnClick="btnDeny_Click"  />
		<a href="prescription-requests.aspx" class="btn btn-md btn-outline-dark">Back</a>
	</div>
</asp:Content>

