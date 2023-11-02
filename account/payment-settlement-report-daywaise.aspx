<%@ Page Title="Payment Settlement Report Daywise" Language="C#" MasterPageFile="~/account/MasterAccount.master" AutoEventWireup="true" CodeFile="payment-settlement-report-daywaise.aspx.cs" Inherits="account_payment_settlement_report_daywaise" %>
<%@ MasterType VirtualPath="~/account/MasterAccount.master" %>
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
					{ orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11] }
			   ],
			   order: [[1, 'desc']]
		   });

		   $('[id$=gvRefundSettleCount]').DataTable({
			   columnDefs: [
					{ "targets": 1, "type": "date-eu" },
					{ orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11] }
			   ],
			   order: [[1, 'desc']]
		   });
	   });

	 </script>

	<script type="text/javascript">
		function CallSettlement() {
			if ($("#<%=txtSettleID.ClientID%>")[0].value =="")
			{
				alert('Please enter Settlement ID');
				return;
			}

			$.ajax({
				type: "POST",
				url: "payment-settlement-report-daywaise.aspx/FetchSettlement",
				data: '{Id: "' + $("#<%=txtSettleID.ClientID%>")[0].value + '" }',
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: OnSuccess,
			failure: function (response) {
			    //alert(response.d);
			    TostTrigger('error', response.d);
			}
		});
	}
	function OnSuccess(response) {
	    //alert(response.d);
	    if(response.d == 'success'){
	        TostTrigger('success', "Settlement ID Fetched.");
	    }
	    else {
	        //alert(response.d);
	        TostTrigger('error', "Error occured while fetching data.");
	    }
	    
	    setTimeout(function () { location.reload(); }, 3000);
	}
</script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<h2 class="pgTitle" runat="server" id="heaadH2">Daywise Payment Settlement Report</h2>
	<%--<h2 class="pgTitle"><%= pgTitle %></h2>--%>
	<span class="space15"></span>

	<table><tr>
		<td><asp:Button ID="btnFetch" runat="server" Text="Fetch Settlement Status" CssClass="btn btn-md btn-info mr-2" OnClick="btnFetch_Click" /></td><td><%= apiResp %></td>
		<td><!-- Button trigger modal -->
<button type="button" class="btn btn-primary" data-toggle="modal" data-target="#exampleModal">
  Fetch Settlement by ID
</button></td>
		   </tr>
		<tr><td></td></tr>
	</table>
	<span class="space20"></span>
	
	<%= errMsg %>
	<div id="settleGrid" runat="server">
		<asp:GridView ID="gvSettlement" runat="server" AutoGenerateColumns="False"
			CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None" OnRowDataBound="gvSettlement_RowDataBound" OnRowCommand="gvSettlement_RowCommand">
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
						<asp:Button ID="cmdVerify" runat="server" CssClass="btn btn-md btn-primary" CommandName="gvVerify" Text="Mark as Verify" />
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
		<h2 class="pgTitle">Payment Settlement</h2>
		<span class="space15"></span>
		<asp:GridView ID="gvSettlecount" runat="server" AutoGenerateColumns="False"
			CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None" 
			OnRowDataBound="gvSettlecount_RowDataBound">
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
				<asp:TemplateField HeaderText="Shop Info">
					<ItemStyle Width="20%" />
					<ItemTemplate>
						<asp:Literal ID="litShop" runat="server"></asp:Literal>
					</ItemTemplate>
				</asp:TemplateField>
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
		<h2 class="pgTitle">Refund Settlement</h2>
		<span class="space15"></span>

		<asp:GridView ID="gvRefundSettleCount" runat="server" AutoGenerateColumns="False"
			CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None" OnRowDataBound="gvRefundSettleCount_RowDataBound" >
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
				<asp:TemplateField HeaderText="Shop Info">
					<ItemStyle Width="20%" />
					<ItemTemplate>
						<asp:Literal ID="litShop" runat="server"></asp:Literal>
					</ItemTemplate>
				</asp:TemplateField>
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

	<!-- Modal -->
<div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
  <div class="modal-dialog" role="document">
	<div class="modal-content">
	  <div class="modal-header">
		<h5 class="modal-title" id="exampleModalLabel">Fetch Settlement by ID</h5>
		<button type="button" class="close" data-dismiss="modal" aria-label="Close">
		  <span aria-hidden="true">&times;</span>
		</button>
	  </div>
	  <div class="modal-body">
		<div class="form-group">
			<label for="exampleInputEmail1">Enter Settlement ID</label>
			<asp:TextBox ID="txtSettleID" CssClass="form-control" placeholder="e.g. setl_JpecFOhqDwCUXX" runat="server"></asp:TextBox>
		  </div>
	  </div>
	  <div class="modal-footer">
		<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
		<button type="button" class="btn btn-primary" onclick="CallSettlement()">Submit</button>
	  </div>
	</div>
  </div>
</div>
</asp:Content>

