<%@ Page Title="Product wise Order Report" Language="C#" MasterPageFile="~/bdm/MasterBdm.master" AutoEventWireup="true" CodeFile="medicine-order-report.aspx.cs" Inherits="management_medicine_order_report" %>
<%@ MasterType VirtualPath="~/management/MasterManagement.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
	<script type="text/javascript">
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
                        { orderable: false, targets: [0, 1, 2, 3, 4, 5] }
                    ],
                    order: [[4, 'desc']]
                });

            }
        });
    </script>
	<script type="text/javascript">

		window.onload = function () {
			//alert("window load");

			duDatepicker('#<%= txtFDate.ClientID %>', {
				auto: true, inline: true, format: 'dd/mm/yyyy',
				
			});
			duDatepicker('#<%= txtToDate.ClientID %>', {
				auto: true, inline: true, format: 'dd/mm/yyyy',

			});
		}
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<h2 class="pgTitle">Product wise Order Report</h2>
	<span class="space15"></span>

	<div id="viewOrder" runat="server">
		<div class="row">
			<div class="col-md-2 form-group">
				<label>From Date :*</label>
				<asp:TextBox ID="txtFDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
			</div>
			<div class="col-md-2 form-group">
				<label>From Date :*</label>
				<asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
			</div>
			<div class="col-md-2 form-group" id="zhPanel" runat="server">
				<label>Select Zonal Head :*</label>
				<asp:DropDownList ID="ddrZh" runat="server" CssClass="form-control" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddrZh_SelectedIndexChanged">
					<asp:ListItem Value="0"><- Select -></asp:ListItem>
				</asp:DropDownList>
			</div>
			<div class="col-md-2 form-group" id="dhPanel" runat="server">
				<label>Select District Head :*</label>
				<asp:DropDownList ID="ddrDh" runat="server" CssClass="form-control" Width="100%">
					<asp:ListItem Value="0"><- Select -></asp:ListItem>
				</asp:DropDownList>
			</div>
			<div class="col-md-2 form-group">
				<span style="height:6px; display:block; width:100%;"></span><br />
				<asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Show Report" OnClick="btnSave_Click" />
			</div>
		</div>
		<span class="space15"></span>
		<div>
			<%= errMsg %>
			<asp:GridView ID="gvOrder" runat="server" 
				CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
				AutoGenerateColumns="false" Width="100%">
				<RowStyle CssClass="" />
				<HeaderStyle CssClass="bg-dark" />
				<AlternatingRowStyle CssClass="alt" />
				<Columns>
					<asp:BoundField DataField="FK_DetailProductID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="ProductName" HeaderText="Product Name">
						<ItemStyle Width="25%" />
					</asp:BoundField>
					<asp:BoundField DataField="OrdDetailSKU" HeaderText="SKU Code">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="totalOrders" HeaderText="Orders">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="totalQty" HeaderText="Order Quantity">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="UnitName" HeaderText="Unit">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<%--<asp:BoundField DataField="MfgName" HeaderText="Manufacturer">
						<ItemStyle Width="12%" />
					</asp:BoundField>--%>
				</Columns>
				<EmptyDataTemplate>
					<span class="warning">No Data to Display :(</span>
				</EmptyDataTemplate>
				<PagerStyle CssClass="gvPager" />
			</asp:GridView>
		</div>
	</div>
</asp:Content>

