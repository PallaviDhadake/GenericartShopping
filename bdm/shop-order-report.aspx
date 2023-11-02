<%@ Page Language="C#" AutoEventWireup="true" CodeFile="shop-order-report.aspx.cs" Inherits="bdm_shop_order_report" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<link href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700" rel="stylesheet">
	<link href="https://fonts.googleapis.com/css?family=Open+Sans" rel="stylesheet" />
	<script src="../admingenshopping/js/jquery-2.2.4.min.js" type="text/javascript"></script>
	<script src="../admingenshopping/js/iScripts.js" type="text/javascript"></script>
	<link href="../admingenshopping/css/jquery.dataTables.min.css" rel="stylesheet" />
	<script src="../admingenshopping/js/jquery.dataTables.min.js"></script>

	<script src="../admingenshopping/plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
	<!-- overlayScrollbars -->
	<script src="../admingenshopping/plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js"></script>
	<!-- AdminLTE App -->
	<script src="../admingenshopping/dist/js/adminlte.js"></script>
	<!-- OPTIONAL SCRIPTS -->
	<script src="../admingenshopping/dist/js/demo.js"></script>

	<!-- Custom style -->
	<link href="../admingenshopping/css/iAdmin.css" rel="stylesheet" />
	<link rel="stylesheet" href="../admingenshopping/plugins/fontawesome-free/css/all.min.css">
	<!-- overlayScrollbars -->
	<link rel="stylesheet" href="../admingenshopping/plugins/overlayScrollbars/css/OverlayScrollbars.min.css">
	<!-- Theme style -->
	<link rel="stylesheet" href="../admingenshopping/dist/css/adminlte.min.css">

	<script>
		$(document).ready(function () {
			$('[id$=gvOrder]').DataTable({
				columnDefs: [
					{ orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7] }
				],
				order: [[0, 'desc']]
			});
		});
	</script>
	<style>
		.pad_20{padding:20px;}
	</style>
</head>
<body>
	<form id="form1" runat="server">
		<div class="pad_20">
			<h2><%= shopName %></h2>

			<%= errMsg %>

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
					<asp:BoundField DataField="FK_OrderID" HeaderText="Order ID">
						<ItemStyle Width="10%" />
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
					<asp:TemplateField HeaderText="Status">
						<ItemStyle Width="5%" />
						<ItemTemplate>
							<asp:Literal ID="litStatus" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="DeviceType" HeaderText="Order From">
						<ItemStyle Width="10%" />
					</asp:BoundField>
				</Columns>
				<EmptyDataTemplate>
					<span class="warning">No Orders to Display :(</span>
				</EmptyDataTemplate>
				<PagerStyle CssClass="gvPager" />
			</asp:GridView>
		</div>
	</form>
</body>
</html>
