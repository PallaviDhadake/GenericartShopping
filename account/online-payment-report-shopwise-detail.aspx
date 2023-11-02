<%@ Page Title="Day wise Shopwise Online Payment Report" Language="C#" MasterPageFile="~/account/MasterAccount.master" AutoEventWireup="true" CodeFile="online-payment-report-shopwise-detail.aspx.cs" Inherits="account_online_payment_report_shopwise_detail" %>
<%@ MasterType VirtualPath="~/account/MasterAccount.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script>
		$(document).ready(function () {
			$('[id$=gvOrder]').DataTable({
				columnDefs: [
					{ orderable: false, targets: [0, 1, 2, 3, 4] }
				],
				order: [[1, 'asc']]
			});
		});
	</script>
	<script type="text/javascript">
		function ApprovePayment(date, frId, amount, orderNumber) {
			//alert(date + ", " + frId + ", " + amount);

		    PageMethods.UpdatePayout(date, frId, amount, orderNumber, onSucess, onError);
			//alert("page method called");
			function onSucess(result) {
				//alert(result);
				if (result != 1) {
					switch (result) {
						case 0: TostTrigger('warning', 'Invalid From Date'); break;
						case 2: TostTrigger('warning', 'Invalid To Date'); break;
						case 3: TostTrigger('error', 'Error Occoured While Processing'); break;
					}
				}
				else {
					TostTrigger('success', 'Payment Approved');
				}
				waitAndMove(window.location, 2000);
			}

			function onError(result) {
				alert(result.get_message());
			}
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<h2 class="pgTitle">Day wise &amp; Shop wise Online Payment Report</h2>
	<span class="space15"></span>

	<div id="viewOrder" runat="server">
		<div>
			<asp:GridView ID="gvOrder" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
				AutoGenerateColumns="false" OnRowDataBound="gvOrder_RowDataBound" Width="100%">
				<RowStyle CssClass="" />
				<HeaderStyle CssClass="bg-dark" />
				<AlternatingRowStyle CssClass="alt" />
				<Columns>
					<asp:BoundField DataField="FranchID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="FranchName" HeaderText="Shop Name">
						<ItemStyle Width="30%" />
					</asp:BoundField>
					<asp:BoundField DataField="FranchShopCode" HeaderText="Shop Code">
						<ItemStyle Width="15%" />
					</asp:BoundField>
					<asp:BoundField DataField="unpaidAmount" HeaderText="Unpaid Amount">
						<ItemStyle Width="15%" />
					</asp:BoundField>
					<asp:TemplateField HeaderText="">
						<ItemStyle Width="5%" />
						<ItemTemplate>
							<asp:Literal ID="litView" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
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

