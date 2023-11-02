<%@ Page Title="Medicine Not Found List" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="medicine-not-found-list.aspx.cs" Inherits="admingenshopping_medicine_not_found_list" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script>
		 $(document).ready(function () {
			 $('[id$=gvMedList]').DataTable({
				 columnDefs: [
					  { orderable: false, targets: [0, 1, 2, 3, 4, 5] }
				 ],
				 order: [[0, 'desc']]
			 });

		 });
	 </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<h2 class="pgTitle">Medicine Not Found List</h2>
	<span class="space15"></span>

	<div id="viewRx" runat="server">
		<span class="space15"></span>
		<div>
			<asp:GridView ID="gvMedList" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
				AutoGenerateColumns="false" Width="100%">
				<RowStyle CssClass="" />
				<HeaderStyle CssClass="bg-dark" />
				<AlternatingRowStyle CssClass="alt" />
				<Columns>
					<asp:BoundField DataField="RequestID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="reqDate" HeaderText="Date">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="RequestName" HeaderText="Name">
						<ItemStyle Width="15%" />
					</asp:BoundField>
					<asp:BoundField DataField="RequestMobile" HeaderText="Mobile No.">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="RequestMedicine" HeaderText="Medicine">
						<ItemStyle Width="15%" />
					</asp:BoundField>
					<asp:BoundField DataField="DeviceType" HeaderText="Req. From">
						<ItemStyle Width="5%" />
					</asp:BoundField>
				</Columns>
				<EmptyDataTemplate>
					<span class="warning">No Data to Display :(</span>
				</EmptyDataTemplate>
				<PagerStyle CssClass="gvPager" />
			</asp:GridView>
		</div>
	</div>
</asp:Content>

