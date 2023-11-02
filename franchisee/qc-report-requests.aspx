<%@ Page Title="QC Report Requests" Language="C#" MasterPageFile="~/franchisee/MasterFranchisee.master" AutoEventWireup="true" CodeFile="qc-report-requests.aspx.cs" Inherits="franchisee_qc_report_requests" %>
<%@ MasterType VirtualPath="~/franchisee/MasterFranchisee.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script>
		$(document).ready(function () {
			$('[id$=gvRequests]').DataTable({
				columnDefs: [
					{ orderable: false, targets: [0, 1, 2, 4, 5, 6, 7, 8, 9, 10] }
				],
				order: [[0, 'desc']]
			});
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<h2 class="pgTitle">QC Report Requests</h2>
	<span class="space15"></span>
	<div id="viewReq" runat="server">
		<div>
			<asp:GridView ID="gvRequests" runat="server" 
				CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
				AutoGenerateColumns="false" Width="100%" OnRowDataBound="gvRequests_RowDataBound">
				<%--<RowStyle CssClass="" />--%>
				<HeaderStyle CssClass="bg-dark" />
				<AlternatingRowStyle CssClass="alt" />
				<Columns>
					<asp:BoundField DataField="QCReqID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="QCReqStatus">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="FK_OrderID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="reqDate" HeaderText="Date">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:TemplateField HeaderText="Customer">
						<ItemStyle Width="10%" />
						<ItemTemplate>
							<asp:Literal ID="litCust" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Mobile No.">
						<ItemStyle Width="5%" />
						<ItemTemplate>
							<asp:Literal ID="litMob" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="ProductName" HeaderText="Product">
						<ItemStyle Width="15%" />
					</asp:BoundField>
					<asp:BoundField DataField="QCReqBatchNo" HeaderText="Batch No.">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:TemplateField HeaderText="Status">
						<ItemStyle Width="5%" />
						<ItemTemplate>
							<asp:Literal ID="litStatus" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="DeviceType" HeaderText="Req. From">
						<ItemStyle Width="6%" />
					</asp:BoundField>
					<asp:TemplateField>
						<ItemStyle Width="5%" />
						<ItemTemplate>
							<asp:Literal ID="litAnch" runat="server"></asp:Literal>
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

	<div id="editReq" runat="server">
		<div class="card">
			<div class="card-header">
				<span class="badge badge-pill badge-success text-lg"><%= deviceType %></span>
				<h3 class="large colorLightBlue">Customer Details</h3>
			</div>
			<div class="card-body">
				<table class="form_table">
					<tr>
						<td><span class="formLable bold_weight">Customer Name :</span></td>
						<td><span class="formLable"><%= ordData[5] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Customer Mobile No :</span></td>
						<td><span class="formLable"><%= ordData[6] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Customer Email :</span></td>
						<td><span class="formLable"><%= ordData[7] %></span> </td>
					</tr>
				</table>
			</div>
			<div class="card-header">
				<h3 class="large colorLightBlue">Request Details</h3>
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
						<td style="width: 25%"><span class="formLable bold_weight">Order Date:</span></td>
						<td><span class="formLable"><%= ordData[1] %></span> </td>
					</tr>
					<tr>
						<td style="width: 25%"><span class="formLable bold_weight">Request Date:</span></td>
						<td><span class="formLable"><%= ordData[2] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Product Name :</span></td>
						<td><span class="formLable"><%= ordData[3] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Batch No. :</span></td>
						<td><span class="formLable"><%= ordData[4] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Upload Report :</span></td>
						<td>
							<asp:FileUpload ID="fuReport" runat="server" />
						</td>
					</tr>
					<tr>
						<td colspan="2"><%= qcRep %></td>
					</tr>
				</table>
			</div>
		</div>
		<asp:Button ID="btnUpload" runat="server" CssClass="btn btn-primary" Text="Upload Report" OnClick="btnUpload_Click"  />
		<a href="qc-report-requests.aspx" class="btn btn-outline-dark">Back</a>
	</div>
</asp:Content>

