<%@ Page Title="" Language="C#" MasterPageFile="~/account/MasterAccount.master" AutoEventWireup="true" CodeFile="qrcode-order-report.aspx.cs" Inherits="account_qrcode_order_report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="//cdn.datatables.net/plug-ins/1.10.11/sorting/date-eu.js" type="text/javascript"></script>
    <script>
		$(document).ready(function () {
			$('[id$=gvOrder]').DataTable({
				columnDefs: [
					{ orderable: false, targets: [0, 1, 2, 3, 4, 6, 7, 8, 9, 10, 11] }
				],
				order: [[0, 'desc']]
			});
		});
    </script>

	<%--<script type="text/javascript">
        window.onload = function () {
            //alert("window load");

            duDatepicker('#<%= txtFromDate.ClientID %>', {
                auto: true, inline: true, format: 'dd/mm/yyyy',
			});

			duDatepicker('#<%= txtToDate.ClientID %>', {
				auto: true, inline: true, format: 'dd/mm/yyyy',
			});
        }
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Order Report</h2>
	<span class="space15"></span>

	<span class="space15"></span>
	<div id="viewOrder" runat="server">
		<span class="space15"></span>
		<asp:GridView ID="gvOrder" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
			AutoGenerateColumns="false" Width="100%" OnRowDataBound="RowDataBound" >
			<RowStyle CssClass="" />
			<HeaderStyle CssClass="bg-dark" />
			<AlternatingRowStyle CssClass="alt" />
			<Columns>
                <asp:BoundField DataField="OrdAssignID">
                    <HeaderStyle CssClass="HideCol" />
                    <ItemStyle CssClass="HideCol" />
                </asp:BoundField>
				<asp:BoundField DataField="OrdStatus">
					<HeaderStyle CssClass="HideCol" />
					<ItemStyle CssClass="HideCol" />
				</asp:BoundField>
                <asp:BoundField DataField="OrderID" HeaderText="Order ID">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="OrderSalesBillNumber" HeaderText="Sale Bill No.">
                    <ItemStyle Width="5%" />
                </asp:BoundField>
                <asp:BoundField DataField="ordDate" HeaderText="Date">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="CustomerName" HeaderText="Name">
                    <ItemStyle Width="15%" />
                </asp:BoundField>
                <asp:BoundField DataField="OrdAmount" HeaderText="Amount">
                    <ItemStyle Width="5%" />
                </asp:BoundField>
                <asp:BoundField DataField ="UPIID" HeaderText="UPI ID/ Transaction No.">
                    <ItemStyle Width="5%" />
                </asp:BoundField>
                <asp:BoundField DataField="ProductCount" HeaderText="No. Of Product">
                    <ItemStyle Width="12%" />
                </asp:BoundField>
                <asp:BoundField DataField="CartProducts" HeaderText="Products">
                    <ItemStyle Width="30%" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Status">
                    <ItemStyle Width="5%" />
                    <ItemTemplate>
                        <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="DeviceType" HeaderText="Order From">
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
				<span class="warning">No Orders To Display :(</span>
			</EmptyDataTemplate>
			<PagerStyle CssClass="gvPaper" />
		</asp:GridView>
	</div>
</asp:Content>
