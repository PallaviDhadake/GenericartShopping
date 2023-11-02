<%@ Page Title="" Language="C#" MasterPageFile="~/obp/MasterObp.master" AutoEventWireup="true" CodeFile="gobp-info.aspx.cs" Inherits="obp_gobp_info" %>
<%@ MasterType VirtualPath="~/obp/MasterObp.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="card">
			<div class="card-header">
				<h3 class="large colorLightBlue">GOBP Details</h3>
			</div>
			<div class="card-body">
				<table class="form_table">
					<tr>
						<td style="width: 25%"><span class="formLable bold_weight">GOBPID:</span></td>
						<td><span class="formLable"><%= obpData[9] %></span> </td>
					</tr>
					<tr>
						<td style="width: 25%"><span class="formLable bold_weight">Name:</span></td>
						<td><span class="formLable"><%= obpData[0] %></span> </td>
					</tr>
					<tr>
						<td style="width: 25%"><span class="formLable bold_weight">Mobile:</span></td>
						<td><span class="formLable"><%= obpData[1] %></span> </td>
					</tr>
					<tr>
						<td style="width: 25%"><span class="formLable bold_weight">Email:</span></td>
						<td><span class="formLable"><%= obpData[2] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">City :</span></td>
						<td><span class="formLable"><%= obpData[3] %></span> </td>
					</tr>

					<tr>
						<td><span class="formLable bold_weight">OBPReferral :</span></td>
						<td><span class="formLable"><%= obpData[4] %></span> </td>
					</tr>
					
					<tr>
						<td><span class="formLable bold_weight">District Head :</span></td>
						<td><span class="formLable"><%= obpData[5] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">MLM Type :</span></td>
						<td><span class="formLable"><%= obpData[6] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Joining Level :</span></td>
						<td><span class="formLable"><%= obpData[7] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">OBP Refferal UserId :</span></td>
						<td><span class="formLable"><%= obpData[8] %></span> </td>
					</tr>
				</table>
				<hr />
				<div class="row">
					<div class="col-md-4">
						<div class="p-3 shadow">
							<span class="text-primary medium font-weight-bold">My Earnings</span>
							<span class="space10"></span>
							<span class="medium font-weight-bold">Total : <span class="semiMedium text-dark"><%=MyEarnings %></span> </span>
							<span class="space10"></span>
							<span class="medium font-weight-bold">This Month : <span class="semiMedium text-dark"><%=thismonearning %></span> </span>
						</div>
					</div>
					<div class="col-md-4">
						<div class="p-3 shadow">
							<span class="text-primary medium font-weight-bold">Total Customers</span>
							<span class="space10"></span>
							<span class="medium font-weight-bold">Total : <span class="semiMedium text-dark"><%=totalCust %></span> </span>
							<span class="space10"></span>
							<span class="medium font-weight-bold">This Month : <span class="semiMedium text-dark"><%=thismontcust %></span> </span>
						</div>
					</div>
					<div class="col-md-4">
						<div class="p-3 shadow">
							<span class="text-primary medium font-weight-bold">Total Orders</span>
							<span class="space10"></span>
							<span class="medium font-weight-bold">Total : <span class="semiMedium text-dark"><%=totalOrders %></span> </span>
							<span class="space10"></span>
							<span class="medium font-weight-bold">This Month : <span class="semiMedium text-dark"><%=thismontorder %></span> </span>
						</div>
					</div>
				</div>
			</div>
		</div>
		<span class="space15"></span>
	<a href="add-gobp.aspx" class="btn btn-sm btn-outline-dark">Back</a>
</asp:Content>

