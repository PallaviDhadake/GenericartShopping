<%@ Page Title="Monthwise Orders Report" Language="C#" MasterPageFile="~/bdm/MasterBdm.master" AutoEventWireup="true" CodeFile="monthwise-orders-report.aspx.cs" Inherits="bdm_monthwise_orders_report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	
	<%--<script type="text/javascript">
		$(function () {
			var prm = Sys.WebForms.PageRequestManager.getInstance();
			if (prm != null) {
				prm.add_endRequest(function (sender, e) {
					if (sender._postBackSettings.panelsToUpdate != null) {
						createDataTable();
					}
				});
			};

			createDataTable();
			function createDataTable() {
				$('#<%= gvOrder.ClientID %>').prepend($("<thead></thead>").append($('#<%= gvOrder.ClientID %>').find("tr:first"))).DataTable({

					columnDefs: [
						 { orderable: false, targets: [0, 1, 2, 3] }
					],
					order: [[0, 'asc']],
					fixedHeader: {
					    header: true,
					    footer: true
					}
				});

			}
		});
	</script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<h2 class="pgTitle">Order Report Master</h2>
	<span class="space15"></span>
	<div id="viewOrder" runat="server">
		<div>
			<asp:GridView ID="gvOrder" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm"
				GridLines="None" ShowFooter="true"
				AutoGenerateColumns="false" Width="100%">
				<RowStyle CssClass="" />
				<HeaderStyle CssClass="bg-dark" />
				<AlternatingRowStyle CssClass="alt" />
				<Columns>
					<asp:BoundField DataField="NumOfMonth">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
						<FooterStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="OrderMonth" HeaderText="Month">
						<ItemStyle Width="30%" />
					</asp:BoundField>
					<asp:BoundField DataField="OrderCount" HeaderText="Total Orders">
						<ItemStyle Width="30%" />
					</asp:BoundField>
					<asp:BoundField DataField="OrderAmount" HeaderText="Total Order Amount">
						<ItemStyle Width="30%" />
					</asp:BoundField>
					<%--<asp:TemplateField>
						<ItemStyle Width="5%" />
						<ItemTemplate>
							<asp:Literal ID="litAnch" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>--%>
				</Columns>
				<EmptyDataTemplate>
					<span class="warning">No Orders to Display :(</span>
				</EmptyDataTemplate>
				<FooterStyle BackColor="Black" ForeColor="White" />
				<PagerStyle CssClass="gvPager" />
			</asp:GridView>
		</div>
	</div>
</asp:Content>

