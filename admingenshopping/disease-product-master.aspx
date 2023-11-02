<%@ Page Title="Disease Product Master | Genericart Shopping Admin Master" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="disease-product-master.aspx.cs" Inherits="admingenshopping_disease_product_master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

	 <%--<link href="//netdna.bootstrapcdn.com/bootstrap/3.1.1/css/bootstrap.min.css" rel="stylesheet"/>
	<script src="https://code.jquery.com/ui/1.11.4/jquery-ui.min.js" type="text/javascript"></script>
	<link href="https://code.jquery.com/ui/1.11.4/themes/start/jquery-ui.css" rel="Stylesheet" type="text/css" />--%>

	<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />
	<link rel="stylesheet" href="/resources/demos/style.css" />
	<script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

	  <script type="text/javascript">
		  $(document).ready(function () {
			  SearchText();
		  });
		  function SearchText() {
			  $("#<%= txtProduct.ClientID %>").autocomplete({
				  source: function (request, response) {
					  $.ajax({
						  type: "POST",
						  contentType: "application/json; charset=utf-8",
						  url: "disease-product-master.aspx/GetSearchControl",
						  data: "{'prefix':'" + document.getElementById('<%= txtProduct.ClientID %>').value + "'}",
						  dataType: "json",
						  success: function (data) {
							  //response(data.d);
							  response($.map(data.d, function (item) {
								  return {
									  label: item.split('#')[0],
									  //val: item.split('#')[1]
								  }
							  }))
						  },
						  error: function (result) {
							  alert("No Match");
						  }
					  });
				  }
			  });
		  }
	  </script>

	<script src="js/bootstrap-tokenfield.js"></script>
	<link href="css/bootstrap-tokenfield.css" rel="stylesheet" />
   <script type="text/javascript">
	   $(document).ready(function () {
		   SearchD();
	   });
	   function SearchD() {

		   $("#<%= txtDisease.ClientID %>").tokenfield({
			   autocomplete: {
				   source: function (request, response) {
					   $.ajax({
						   url: '<%=ResolveUrl("disease-product-master.aspx/GetDiseases") %>',
							data: "{ 'prefix': '" + request.term + "'}",
							dataType: "json",
							type: "POST",
							contentType: "application/json; charset=utf-8",
							success: function (data) {
								response($.map(data.d, function (item) {
									return {
										label: item.split('#')[0],

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
   
			 <%--New/Edit Form Start--%>
			<div id="editDiseaseProd" runat="server">
				<div class="card card-primary">
					<div class="card-header">
						<h3 class="card-title"></h3>
					</div>
					<div class="card-body">
						<div class="">
							<div class="colorLightBlue">
								<span>Id :</span>
								<asp:Label ID="lblId" runat="server" Text="[New]"></asp:Label>
							</div>
							<span class="space15"></span>
						 
							<div class="form-group">
								<label for="pName">Product Name :*</label>
								<asp:TextBox ID="txtProduct" runat="server" AutoPostBack="true" OnTextChanged="txtProduct_TextChanged" CssClass="form-control" Width="100%" MaxLength="200"></asp:TextBox>
							</div>

							

							 <div class="form-group">
								<label for="dName">Disease Name :*</label>
								<%-- <input type="text" id="txtDisease" value=""/>--%>
								<asp:TextBox ID="txtDisease" runat="server" CssClass="form-control" Width="100%" MaxLength="200"></asp:TextBox>
							</div>
						</div>
					</div>
				</div>
				<span class="space10"></span>
			   <%-- <%=errMsg %>--%>
				<span class="space10"></span>
				<asp:Button ID="btnSave" runat="server"
					CssClass="btn btn-sm btn-primary" Text="Save" OnClick="btnSave_Click"/>
				
			</div>
			<%--New/Edit Form End--%>

			<%--disease product  GridView Start--%>
			<div id="viewDiseaseProd" runat="server">
				<span class="space20"></span>
				<div>
					<asp:GridView ID="gvDiseaseProd" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
						OnRowCommand="gvDiseaseProd_RowCommand"
						AutoGenerateColumns="false" Width="100%">
						<RowStyle CssClass="" />
						<HeaderStyle CssClass="bg-dark" />
						<AlternatingRowStyle CssClass="alt" />
						<Columns>
							<asp:BoundField DataField="FK_ProductID"><HeaderStyle CssClass="HideCol" /><ItemStyle CssClass="HideCol" /></asp:BoundField>
							<asp:BoundField DataField="FK_DiseaseID"><HeaderStyle CssClass="HideCol" /><ItemStyle CssClass="HideCol" /></asp:BoundField>
							<asp:BoundField DataField="ProductName" HeaderText="Product Name"><ItemStyle Width="30%" /></asp:BoundField>
							<asp:BoundField DataField="DiseaseName" HeaderText="Disease Name"><ItemStyle Width="30%" /></asp:BoundField>
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
			<%--disease product  GridView Start--%>
	  
</asp:Content>

