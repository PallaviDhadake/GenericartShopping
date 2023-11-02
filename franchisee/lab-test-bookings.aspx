<%@ Page Title="Lab Test Booking Appointments Report" Language="C#" MasterPageFile="~/franchisee/MasterFranchisee.master" AutoEventWireup="true" CodeFile="lab-test-bookings.aspx.cs" Inherits="franchisee_lab_test_bookings" %>
<%@ MasterType VirtualPath="~/franchisee/MasterFranchisee.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script>
		$(document).ready(function () {
			$('[id$=gvDetails]').DataTable({
				columnDefs: [
					 { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13] }
				],
				order: [[0, 'desc']]
			});
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	 <h2 class="pgTitle">Lab Test Booking Appointments Report</h2>
	<span class="space15"></span>
	<div id="viewDetails" runat="server">
		<a href="<%= appUrl %>" class="btn btn-md btn-primary" target="_blank">Add New Lab Appointment</a>
		<span class="space15"></span>
		<div>
			<asp:GridView ID="gvDetails" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
				AutoGenerateColumns="false" Width="100%" OnRowDataBound="gvDetails_RowDataBound">
				<RowStyle CssClass="" />
				<HeaderStyle CssClass="bg-dark" />
				<AlternatingRowStyle CssClass="alt" />
				<Columns>
					<asp:BoundField DataField="LabAppID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					 <asp:BoundField DataField="FK_LabTestID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="LabAppStatus">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="appDate" HeaderText="Appointment Date">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="LabAppName" HeaderText="Name">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="LabAppAge" HeaderText="Age">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="gender" HeaderText="Gender">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="LabAppMobile" HeaderText="Mobile">
						<ItemStyle Width="8%" />
					</asp:BoundField>
					<asp:BoundField DataField="LabAppEmail" HeaderText="Email">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="LabAppAddress" HeaderText="Address">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="LabAppPincode" HeaderText="Pincode">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<%--<asp:BoundField DataField="FK_LabTestID" HeaderText="Lab Test">
						<ItemStyle Width="10%" />
					</asp:BoundField>--%>
					<asp:TemplateField HeaderText="Lab Test">
						<ItemStyle Width="5%" />
						<ItemTemplate>
							<asp:Literal ID="litLabTest" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="DeviceType" HeaderText="Booking From">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:TemplateField HeaderText="Status">
						<ItemStyle Width="10%" />
						<ItemTemplate>
							<asp:Literal ID="litStatus" runat="server"></asp:Literal>
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
</asp:Content>

