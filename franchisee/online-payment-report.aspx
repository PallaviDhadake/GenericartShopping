<%@ Page Title="Online Payment Report" Language="C#" MasterPageFile="~/franchisee/MasterFranchisee.master" AutoEventWireup="true" CodeFile="online-payment-report.aspx.cs" Inherits="franchisee_online_payment_report" %>
<%@ MasterType VirtualPath="~/franchisee/MasterFranchisee.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script>
		$(document).ready(function () {
			$('[id$=gvOrder]').DataTable({
				columnDefs: [
					{ orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12] }
				],
				order: [[0, 'desc']]
			});
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<h2 class="pgTitle">Online Payment Report</h2>
	<span class="space15"></span>
	<div id="viewOrder" runat="server">
		<div>
			<asp:GridView ID="gvOrder" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
				AutoGenerateColumns="false" OnRowDataBound="gvOrder_RowDataBound" Width="100%">
				<RowStyle CssClass="" />
				<HeaderStyle CssClass="bg-dark" />
				<AlternatingRowStyle CssClass="alt" />
				<Columns>
					<asp:BoundField DataField="OrdAssignID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="OrdAssignStatus">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="Fk_FranchID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="FK_OrderID" HeaderText="Order ID">
						<ItemStyle Width="8%" />
					</asp:BoundField>
					<asp:BoundField DataField="ordDate" HeaderText="Order Date">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="CustomerName" HeaderText="Name">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="payDate" HeaderText="Payment Date">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="OrderPaymentTxnId" HeaderText="Payment Transction Id">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="OrdAmount" HeaderText="Order Amount">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="OLP_amount" HeaderText="Paid Amount">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:TemplateField HeaderText="Order Status">
						<ItemStyle Width="5%" />
						<ItemTemplate>
							<asp:Literal ID="litStatus" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="OPL_transtatus" HeaderText="Transction Status">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="OLP_device_type" HeaderText="Pay From">
						<ItemStyle Width="5%" />
					</asp:BoundField> 
				</Columns>
				<EmptyDataTemplate>
					<span class="warning">No data to Display :(</span>
				</EmptyDataTemplate>
				<PagerStyle CssClass="gvPager" />
			</asp:GridView>
		</div>
	</div>

	<div id="monthwiseOrd" runat="server">

		<div class="row">
			<div class="col-md-3">
				<span class="formLable dspBlk bold_weight mrgBtm10">Select Month: *</span>
				<asp:DropDownList ID="ddrMonth" runat="server" CssClass="form-control mr-2" Width="100%">
					<asp:ListItem Value="0"><- Select -></asp:ListItem>
				</asp:DropDownList>
			</div>
			<div class="col-md-3">
				<span class="formLable dspBlk bold_weight mrgBtm10">Enter Year: *</span>
				<asp:TextBox ID="txtYear" CssClass="form-control mr-2"  Width="100%" runat="server"></asp:TextBox>
			</div>
			<div class="col-md-3">
				<span class="space30"></span>
				<asp:Button ID="btnShow" runat="server" Text="Show" CssClass="btn btn-md btn-primary " OnClick="btnShow_Click"  />
			</div>
		</div>
		<span class="space20"></span>
		<span class="medium text-bold text-indigo"><%= repTitle %></span>
		<span class="space20"></span>
		<%= monthwisePaidOrders %>
	</div>
</asp:Content>

