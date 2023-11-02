<%@ Page Title="Add Related Products to Sub Category" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="related-products.aspx.cs" Inherits="admingenshopping_related_products" %>

<%@ MasterType VirtualPath="~/admingenshopping/MasterAdmin.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
	<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />
	<link rel="stylesheet" href="/resources/demos/style.css" />
	<link href="css/bootstrap-tokenfield.css" rel="stylesheet" />
	<script src="js/bootstrap-tokenfield.js"></script>
	<script type="text/javascript">
		$(document).ready(function () {
			SearchD();
			//alert("demo");
		});
		function SearchD() {

			$("#<%= txtProduct.ClientID %>").tokenfield({
			   autocomplete: {
				   source: function (request, response) {
					   $.ajax({
						   url: '<%=ResolveUrl("related-products.aspx/GetProducts") %>',
						   data: "{ 'prefix': '" + request.term + "'}",
						   dataType: "json",
						   type: "POST",
						   contentType: "application/json; charset=utf-8",
						   success: function (data) {
							   response($.map(data.d, function (item) {
								   return {
									   label: item.split('#')[0]
								   }
							   }))
						   }
					   });
				   },
				   delay: 100
			   }
		   })

	   }
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<h2 class="pgTitle">Add Related Products to Sub Category</h2>
	<span class="space15"></span>

	<div id="editProd" runat="server">
		<div class="card card-primary">
			<div class="card-header">
				<h3 class="card-title"><%= subCat %> Related Products</h3>
			</div>
			<div class="card-body">
				<div class="">
					<div class="colorLightBlue">
						<span>Id :</span>
						<asp:Label ID="lblId" runat="server" Text="[New]"></asp:Label>
					</div>
					<span class="space15"></span>

					<div class="form-group">
						<label for="pName">Sub Category :*</label>
						<asp:TextBox ID="txtSubCat" runat="server" AutoPostBack="true" CssClass="form-control" Width="100%" MaxLength="200" Enabled="false"></asp:TextBox>
					</div>
					<div class="form-group">
						<label for="dName">Product Name :*</label>
						<asp:TextBox ID="txtProduct" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
					</div>
				</div>
			</div>
		</div>
		<span class="space10"></span>
		<asp:Button ID="btnSave" runat="server" CssClass="btn btn-md btn-primary" Text="Save Info" OnClick="btnSave_Click" />
		<%= errMsg %>
	</div>

	<div id="viewProd" runat="server">
		<span class="space20"></span>
		<div>
			<asp:GridView ID="gvSubCatProd" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" 
				GridLines="None" AutoGenerateColumns="false" Width="100%" OnRowCommand="gvSubCatProd_RowCommand">
				<RowStyle CssClass="" />
				<HeaderStyle CssClass="bg-dark" />
				<AlternatingRowStyle CssClass="alt" />
				<Columns>
					<asp:BoundField DataField="ProductID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="ProductName" HeaderText="Product Name">
						<ItemStyle Width="30%" />
					</asp:BoundField>
					<asp:BoundField DataField="ProductSKU" HeaderText="Product Code">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:TemplateField>
						<ItemStyle Width="5%" />
						<ItemTemplate>
							<asp:Button ID="cmdDelete" runat="server" CssClass="buttonDel" CommandName="gvDel" Text="Delete" />
						</ItemTemplate>
					</asp:TemplateField>
				</Columns>
				<EmptyDataTemplate>
					<span class="warning">No Data to Display :(</span>
				</EmptyDataTemplate>
				<PagerStyle CssClass="gvPager" />
			</asp:GridView>
		</div>
	</div>
</asp:Content>

