<%@ Page Title="" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="order-assign-report.aspx.cs" Inherits="supportteam_order_assign_report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="https://pro.fontawesome.com/releases/v5.10.0/css/all.css" integrity="sha384-AYmEC3Yw5cVb3ZcuHtOA93w35dYTsvhLPVnYs9eStHfGJvOvKxVfELGroGkvsg+p" crossorigin="anonymous"/>    

	<script>
        $(document).ready(function () {
            $('[id$=gvOrder]').DataTable({
                columnDefs: [
                    { "targets": 8, "type": "date-eu" },
                    { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 9, 10] }
                ],
                order: [[0, 'desc']]
            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<h2 class="pgTitle">Order Report Master</h2>
	<span class="greyLine"></span>
    <span class="medium themeClrBlue bold_weight ml-2">Date Range: (<%= fyDateRange %>)</span>
	<span class="space15"></span>
	<%--Suppliers GridView Start--%>
	<div id="viewOrder" runat="server">
		<span class="space15"></span>		
		<div>
			<asp:GridView ID="gvOrder" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
				AutoGenerateColumns="false" OnRowDataBound="gvOrder_RowDataBound" Width="100%">
				<RowStyle CssClass="" />
				<HeaderStyle CssClass="bg-dark" />
				<AlternatingRowStyle CssClass="alt" />
				<Columns>
					<asp:BoundField DataField="OrderID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="OrderStatus">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="FK_OrderCustomerID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="OrderID" HeaderText="Order ID">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="ordDate" HeaderText="Date">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="CustomerName" HeaderText="Name">
						<ItemStyle Width="15%" />
					</asp:BoundField>
					<asp:BoundField DataField="CustomerMobile" HeaderText="Mobile No.">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="OrdAmount" HeaderText="Amount">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<%--<asp:BoundField DataField="ProductCount" HeaderText="No. of Product">
						<ItemStyle Width="12%" />
					</asp:BoundField>
					<asp:BoundField DataField="CartProducts" HeaderText="Products">
						<ItemStyle Width="25%" />
					</asp:BoundField>--%>
					<asp:TemplateField HeaderText="Status">
						<ItemStyle Width="10%" />
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
					<span class="warning">No Orders to Display :(</span>
				</EmptyDataTemplate>
				<PagerStyle CssClass="gvPager" />
			</asp:GridView>
		</div>
	</div>	
</asp:Content>
