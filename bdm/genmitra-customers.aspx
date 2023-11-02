<%@ Page Title="Generic Mitra Customers" Language="C#" MasterPageFile="~/bdm/MasterBdm.master" AutoEventWireup="true" CodeFile="genmitra-customers.aspx.cs" Inherits="bdm_genmitra_customers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script>
		$(document).ready(function () {
			$('[id$=gvCust]').DataTable({
				columnDefs: [
					 { orderable: false, targets: [0, 1, 2, 3, 5, 4] }
				],
				order: [[0, 'desc']]
			});
		});
	 </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<h2 class="pgTitle">Generic Mitra Customers List</h2>
	<span class="space15"></span>

	<div class="row">
		<div class="form-group col-sm-4">
			<span class="text-sm text-bold mrgBtm10 dspBlk">Select Generic Mitra :</span>
			<asp:DropDownList ID="ddrGenMitra" runat="server" CssClass="form-control">
				<asp:ListItem Value="0"><- Select -></asp:ListItem>
			</asp:DropDownList>
		</div>
		<div class="col-sm-1">
			<span class="space30"></span>
			<asp:Button ID="btnShow" runat="server" CssClass="btn btn-md btn-primary" Text="Show List" OnClick="btnShow_Click" />
		</div>
	</div>

	<div id="viewDetails" runat="server">
		<span class="space15"></span>
		<div>
			<asp:GridView ID="gvCust" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
				AutoGenerateColumns="false" Width="100%">
				<RowStyle CssClass="" />
				<HeaderStyle CssClass="bg-dark" />
				<AlternatingRowStyle CssClass="alt" />
				<Columns>
					<asp:BoundField DataField="CustomrtID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="JoinDate" HeaderText="Date">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="CustomerName" HeaderText="Name">
						<ItemStyle Width="10%" />
					</asp:BoundField>

					<asp:BoundField DataField="CustomerMobile" HeaderText="Mobile">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="CustomerEmail" HeaderText="Email">
						<ItemStyle Width="8%" />
					</asp:BoundField>
					<asp:BoundField DataField="DeviceType" HeaderText="Registered From">
						<ItemStyle Width="10%" />
					</asp:BoundField>
				</Columns>
				<EmptyDataTemplate>
					<span class="warning">No Customer to Display :(</span>
				</EmptyDataTemplate>
				<PagerStyle CssClass="gvPager" />
			</asp:GridView>
		</div>
	</div>
</asp:Content>

