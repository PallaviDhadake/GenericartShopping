<%@ Page Title="Payment Settlement Report Daywise" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="payment-settlement-report-daywaise.aspx.cs" Inherits="supportteam_payment_settlement_report_daywaise" %>
<%@ MasterType VirtualPath="~/supportteam/MasterSupport.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script src="//cdn.datatables.net/plug-ins/1.10.11/sorting/date-eu.js" type="text/javascript"></script>
	<script>
	   $(document).ready(function () {
		   $('[id$=gvSettlement]').DataTable({
			   columnDefs: [
					{ "targets": 2, "type": "date-eu" },
					{ orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11] }
			   ],
			   order: [[2, 'desc']]
		   });


		   $('[id$=gvSettlecount]').DataTable({
			   columnDefs: [
					{ "targets": 1, "type": "date-eu" },
					{ orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10] }
			   ],
			   order: [[1, 'desc']]
		   });
	   });

	 </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<h2 class="pgTitle">Daywise Payment Settlement Report</h2>
	<span class="space15"></span>

	<table><tr><td><asp:Button ID="btnFetch" runat="server" Text="Fetch Settlement Status" CssClass="btn btn-md btn-info mr-2" OnClick="btnFetch_Click" /></td><td><%= apiResp %></td></tr></table>
	<span class="space20"></span>
	
	<%= errMsg %>
	<div id="settleGrid" runat="server">
		<asp:GridView ID="gvSettlement" runat="server" AutoGenerateColumns="False"
			CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None" 
			OnRowDataBound="gvSettlement_RowDataBound">
			<HeaderStyle CssClass="bg-dark" />
			<AlternatingRowStyle CssClass="alt" />
			<Columns>
				<asp:BoundField DataField="SettlementID">
					<HeaderStyle CssClass="HideCol" />
					<ItemStyle CssClass="HideCol" />
				</asp:BoundField>
				<asp:BoundField DataField="SettlementVerify">
					<HeaderStyle CssClass="HideCol" />
					<ItemStyle CssClass="HideCol" />
				</asp:BoundField>
				<asp:BoundField DataField="SettDate">
					<HeaderStyle CssClass="HideCol" />
					<ItemStyle CssClass="HideCol" />
				</asp:BoundField>
				<asp:BoundField DataField="sDate" HeaderText="Date">
					<ItemStyle Width="10%" />
				</asp:BoundField>
				<asp:BoundField DataField="OrderSettlementID" HeaderText="Settlement Id">
					<ItemStyle Width="10%" />
				</asp:BoundField>
				<asp:BoundField DataField="SettlementFee" HeaderText="Fee">
					<ItemStyle Width="10%" />
				</asp:BoundField>
				<asp:BoundField DataField="SettlemetAmount" HeaderText="Amount">
					<ItemStyle Width="10%" />
				</asp:BoundField>
				<asp:BoundField DataField="SettlementGST" HeaderText="GST">
					<ItemStyle Width="10%" />
				</asp:BoundField>
				<asp:BoundField DataField="SettlemetTotalAmount" HeaderText="Total Amount">
					<ItemStyle Width="10%" />
				</asp:BoundField>
				<asp:BoundField DataField="UTRNo" HeaderText="UTR No">
					<ItemStyle Width="10%" />
				</asp:BoundField>
				<asp:BoundField DataField="SettlementCount" HeaderText="Count">
					<ItemStyle Width="10%" />
				</asp:BoundField>
				<asp:TemplateField>
					<ItemStyle Width="10%" />
					<ItemTemplate>
						<asp:Button ID="cmdVerify" runat="server" CssClass="btn btn-md btn-primary" CommandName="gvVerify" Text="Mark as Verify" Visible="false" />
					</ItemTemplate>
				</asp:TemplateField>   
			</Columns>
			<EmptyDataTemplate>
				<span class="warning">No Data to Display</span>
			</EmptyDataTemplate>
			<FooterStyle BackColor="Black" ForeColor="White" />
			<PagerStyle CssClass="gvPager" />
		</asp:GridView>
	</div>

	<div id="settleCountGrid" runat="server">
		<asp:GridView ID="gvSettlecount" runat="server" AutoGenerateColumns="False"
			CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None">
			<HeaderStyle CssClass="bg-dark" />
			<AlternatingRowStyle CssClass="alt" />
			<Columns>
				<asp:BoundField DataField="OPL_id">
					<HeaderStyle CssClass="HideCol" />
					<ItemStyle CssClass="HideCol" />
				</asp:BoundField>
				<asp:BoundField DataField="opDate">
					<HeaderStyle CssClass="HideCol" />
					<ItemStyle CssClass="HideCol" />
				</asp:BoundField>
				<asp:BoundField DataField="oplDate" HeaderText="Date">
					<ItemStyle Width="10%" />
				</asp:BoundField>
				<asp:BoundField DataField="OLP_amount" HeaderText="Amount">
					<ItemStyle Width="10%" />
				</asp:BoundField>
				<asp:BoundField DataField="OPL_merchantTranId" HeaderText="Txn Id">
					<ItemStyle Width="10%" />
				</asp:BoundField>
				<asp:BoundField DataField="OPL_transtatus" HeaderText="Status">
					<ItemStyle Width="10%" />
				</asp:BoundField>
				<asp:BoundField DataField="OLP_order_no" HeaderText="Order Id">
					<ItemStyle Width="10%" />
				</asp:BoundField>
				<asp:BoundField DataField="OLP_RazorPayFee" HeaderText="RazorPay Fee">
					<ItemStyle Width="10%" />
				</asp:BoundField>
				<asp:BoundField DataField="OLP_RazorPayGST" HeaderText="RazorPay GST">
					<ItemStyle Width="10%" />
				</asp:BoundField>
				<asp:BoundField DataField="OLP_RazorPayAmount" HeaderText="RazorPay Amount">
					<ItemStyle Width="10%" />
				</asp:BoundField>
				<asp:BoundField DataField="OLP_device_type" HeaderText="Device">
					<ItemStyle Width="10%" />
				</asp:BoundField>
			</Columns>
			<EmptyDataTemplate>
				<span class="warning">No Data to Display</span>
			</EmptyDataTemplate>
			<FooterStyle BackColor="Black" ForeColor="White" />
			<PagerStyle CssClass="gvPager" />
		</asp:GridView>
		<span class="space20"></span>
		<a href="payment-settlement-report-daywaise.aspx" class="btn btn-md btn-dark">Back</a>
	</div>
</asp:Content>

