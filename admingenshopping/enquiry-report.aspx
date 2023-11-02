<%@ Page Title="Saving Calculator Enquiry Report | Genericart Shopping Admin Panel" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="enquiry-report.aspx.cs" Inherits="admingenshopping_enquiry_report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script>
		$(document).ready(function () {
			$('[id$=gvOrder]').DataTable({
				columnDefs: [
					 { orderable: false, targets: [0, 1, 2, 4, 5, 7, 8, 9, 10] }
				],
				order: [[0, 'desc']]
			});
		});
	 </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<h2 class="pgTitle">Enquiry Report Master</h2>
	<span class="space15"></span>
	<%--Suppliers GridView Start--%>
	<div id="viewOrder" runat="server">
		<span class="space15"></span>
		<div>
			<asp:GridView ID="gvOrder" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" 
				GridLines="None" AutoGenerateColumns="false" OnRowDataBound="gvOrder_RowDataBound" Width="100%">
				<RowStyle CssClass="" />
				<HeaderStyle CssClass="bg-dark" />
				<AlternatingRowStyle CssClass="alt" />
				<Columns>
					<asp:BoundField DataField="CalcID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="EnqStatus">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="CalcID" HeaderText="Enquiry ID">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="CalDate" HeaderText="Date">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="CustomerName" HeaderText="Name">
						<ItemStyle Width="15%" />
					</asp:BoundField>
					<asp:BoundField DataField="CustomerMobile" HeaderText="Mobile No.">
						<ItemStyle Width="15%" />
					</asp:BoundField>
					<asp:BoundField DataField="ProductCount" HeaderText="No. of Medicines">
						<ItemStyle Width="12%" />
					</asp:BoundField>
					<asp:BoundField DataField="EnqProducts" HeaderText="Medicines">
						<ItemStyle Width="25%" />
					</asp:BoundField>
					<asp:TemplateField HeaderText="Status">
						<ItemStyle Width="5%" />
						<ItemTemplate>
							<asp:Literal ID="litStatus" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="DeviceType" HeaderText="Enq. From">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:TemplateField>
						<ItemStyle Width="5%" />
						<ItemTemplate>
							<asp:Literal ID="litAnch" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
				</Columns>
				<EmptyDataTemplate>
					<span class="warning">No Enquiries to Display :(</span>
				</EmptyDataTemplate>
				<PagerStyle CssClass="gvPager" />
			</asp:GridView>
		</div>
	</div>
	<%--Suppliers GridView End--%>
</asp:Content>

