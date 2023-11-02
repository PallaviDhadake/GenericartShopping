<%@ Page Title="Not Ordered Products List" Language="C#" MasterPageFile="~/franchisee/MasterFranchisee.master" AutoEventWireup="true" CodeFile="products-not-ordered.aspx.cs" Inherits="franchisee_products_not_ordered" %>
<%@ MasterType VirtualPath="~/franchisee/MasterFranchisee.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script>
		$(document).ready(function () {
			$('[id$=gvProducts]').DataTable({
				columnDefs: [
					{ orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9] }
				],
				order: [[0, 'desc']]
			});
		});
	</script>
	<script>
		window.onload = function () {
			//alert("window load");

			duDatepicker('#<%= txtFromDate.ClientID %>', {
				auto: true, inline: true, format: 'dd/mm/yyyy',
			});
			duDatepicker('#<%= txtToDate.ClientID %>', {
				auto: true, inline: true, format: 'dd/mm/yyyy',
			});
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<h2 class="pgTitle">Not Ordered Products List</h2>
	<span class="space15"></span>

	<div class="form-row">
		<div class="form-group col-md-3">
			<label>Select From Date :*</label>
			<asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
		</div>
		<div class="form-group col-md-3">
			<label>Select To Date :*</label>
			<asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
		</div>
		<div class="form-group col-md-3">  
			<span class="space7"></span><br />
			<asp:Button ID="btnShow" runat="server" CssClass="btn btn-md btn-info" Text="Show Report" OnClick="btnShow_Click" />
		</div>
	</div>
	<span class="space15"></span>
	<div id="viewProducts" runat="server">
		<div>
			<asp:GridView ID="gvProducts"  runat="server" CssClass="table table-striped table-bordered table-hover" GridLines="None" 
				AutoGenerateColumns="false" OnRowDataBound="gvProducts_RowDataBound"  >
				<HeaderStyle CssClass="thead-dark" />
				<RowStyle CssClass="" />
				<AlternatingRowStyle CssClass="alt" />
				<Columns>
					<asp:BoundField DataField="ProductID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle  CssClass="HideCol"/>
					</asp:BoundField>
					<asp:BoundField DataField="ProductName" HeaderText="Product Name">
						<ItemStyle Width="25%" />
					</asp:BoundField>
					<asp:BoundField DataField="ProductSKU" HeaderText="Code">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="ProductCatName" HeaderText="Category">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="UnitName" HeaderText="Unit">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="MfgName" HeaderText="Brand Name">
						<ItemStyle Width="12%" />
					</asp:BoundField>
					<asp:BoundField DataField="PriceMRP" HeaderText="MRP">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="PriceSale" HeaderText="Sale Rate">
						<ItemStyle Width="9%" />
					</asp:BoundField>
					<asp:BoundField DataField="ProductStock" HeaderText="Stock">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:TemplateField HeaderText="Status">
						<ItemStyle Width="3%" />
						<ItemTemplate>
							<asp:Literal ID="litStatus" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>                  
				</Columns>
				<EmptyDataTemplate>
					<span class="warning">Its Empty Here... :(</span>
				</EmptyDataTemplate>
				<PagerStyle CssClass="" />
			</asp:GridView>      
		</div>
	</div>
</asp:Content>

