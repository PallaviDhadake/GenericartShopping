<%@ Page Title="Banner Master | Genericart Admin Panel" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="banner-master.aspx.cs" Inherits="supportteam_banner_master" %>
<%@ MasterType VirtualPath="~/supportteam/MasterSupport.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />
	<link rel="stylesheet" href="/resources/demos/style.css" />
	<script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

	  <script type="text/javascript">
		  $(document).ready(function () {
			  SearchText();
		  });
		  function SearchText() {
			  $("#<%= txtProd.ClientID %>").autocomplete({
				  source: function (request, response) {
					  $.ajax({
						  type: "POST",
						  contentType: "application/json; charset=utf-8",
						  url: "banner-master.aspx/GetSearchControl",
						  data: "{'prefix':'" + document.getElementById('<%= txtProd.ClientID %>').value + "'}",
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

			  // category
			  $("#<%= txtCat.ClientID %>").autocomplete({
				  source: function (request, response) {
					  $.ajax({
						  type: "POST",
						  contentType: "application/json; charset=utf-8",
						  url: "banner-master.aspx/GetCategories",
						  data: "{'prefix':'" + document.getElementById('<%= txtCat.ClientID %>').value + "'}",
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<h2 class="pgTitle">Banner Master</h2>
  
	 <%--New/Edit Form Start--%>
	<div id="editBanner" runat="server">
		<div class="card card-primary">
			<div class="card-header">
				<h3 class="card-title"><%=pgTitle %></h3>
			</div>
			<div class="card-body">
				<div class="col-sm-7">
					<div class="colorLightBlue">
						<span>Id :</span>
						<asp:Label ID="lblId" runat="server" Text="[New]"></asp:Label>
					</div>
					<span class="space15"></span>

					<div class="form-group">
						<label>Banner Link</label>
						<div class="input-group">
							<asp:TextBox ID="txtLink" runat="server" CssClass="form-control" Width="100%" MaxLength="500"></asp:TextBox>
						</div>
					</div>
					<div class="form-group">
						<label>Select Link</label>
						<div class="input-group">
							<asp:RadioButton ID="rdbAndroid" runat="server" CssClass="form-control" GroupName="mobLink" TextAlign="Right" Text=" Android Link" AutoPostBack="true" OnCheckedChanged="rdbAndroid_CheckedChanged" />
							<asp:RadioButton ID="rdbProd" runat="server" CssClass="form-control" GroupName="mobLink" TextAlign="Right" Text=" Product Link"  AutoPostBack="true" OnCheckedChanged="rdbProd_CheckedChanged" />
							<asp:RadioButton ID="rdbCat" runat="server" CssClass="form-control" GroupName="mobLink" TextAlign="Right" Text=" Category Link"  AutoPostBack="true" OnCheckedChanged="rdbCat_CheckedChanged"/>
						</div>
					</div>
					<div class="form-group">
						<label>Android Link</label>
						<div class="input-group">
							<asp:DropDownList ID="ddrLink" runat="server" CssClass="form-control" Width="100%">
								<asp:ListItem Value="0"><- Select -></asp:ListItem>
								<asp:ListItem Value="1">Search-Shop</asp:ListItem>
								<asp:ListItem Value="2">Know-Your-Savings</asp:ListItem>
								<asp:ListItem Value="3">Upload-Prescription</asp:ListItem>
								<asp:ListItem Value="4">Doctor-Consultation</asp:ListItem>
								<asp:ListItem Value="5">QC-Report </asp:ListItem>
								<asp:ListItem Value="6">Medicine-Reminder</asp:ListItem>
								<asp:ListItem Value="7">generic-mitra-info</asp:ListItem>
								<asp:ListItem Value="8">business-opportunity</asp:ListItem>
							</asp:DropDownList>
						</div>
					</div>
					<div class="form-group">
						<label>Product Link</label>
						<div class="input-group">
							<asp:TextBox ID="txtProd" runat="server" CssClass="form-control" Width="100%" MaxLength="200"></asp:TextBox>
						</div>
					</div>
					<div class="form-group">
						<label>Category Link</label>
						<div class="input-group">
							<asp:TextBox ID="txtCat" runat="server" CssClass="form-control" Width="100%" MaxLength="200"></asp:TextBox>
						</div>
					</div>
					<div class="form-group">
						<label>Banner Photo: <span>*Image must be 1140 (width) x 460 (height)</span></label>
						<div class="input-group">
							<asp:FileUpload ID="fuImg" runat="server" />
						</div>
					</div>
				</div>
			</div>
		</div>
		<span class="space15"></span>
		<asp:Button ID="btnSave" runat="server" CssClass="btn btn-sm btn-primary" Text="Save" OnClick="btnSave_Click" />
		<asp:Button ID="btnDelete" runat="server"
			CssClass="btn btn-sm btn-outline-info" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this?');" OnClick="btnDelete_Click" />
		<asp:Button ID="btnCancel" runat="server"
			CssClass="btn btn-sm btn-outline-dark" Text="Cancel" OnClick="btnCancel_Click" />
		<div class="float_clear"></div>
	</div>
	<%--New/Edit Form Start--%>

	 <div id="viewBanner" runat="server">
		<a href="banner-master.aspx?action=new" class="btn btn-primary btn-sm">Add New</a>
		<span class="space15"></span>
		<%= errMsg %>
		<div class="formPanel">
			<asp:GridView ID="gvBanner" runat="server" CssClass="table table-bordered" 
				AutoGenerateColumns="false" GridLines="None" 
				OnPageIndexChanging="gvBanner_PageIndexChanging" OnRowDataBound="gvBanner_RowDataBound" 
				OnRowCommand="gvBanner_RowCommand" >
				<HeaderStyle CssClass="bg-dark" />
				<Columns>
					<asp:BoundField DataField="bannerId">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="imageName">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:TemplateField HeaderText="Banner">
						<ItemStyle Width="10%"/>
						<ItemTemplate>
							<asp:Literal ID="litBanner" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="displayOrder" HeaderText="Display Order">
						<ItemStyle Width="15%"/>
					</asp:BoundField>
					<asp:TemplateField>
						<ItemStyle Width="10%" />
						<ItemTemplate>
							<asp:Button ID="moveUp" runat="server" CssClass="gMoveUp" CommandName="Up" />
							<asp:Button ID="moveDown" runat="server" CssClass="gMoveDown" CommandName="Down"  />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField>
						<ItemStyle Width="10%" />
						<ItemTemplate>
							<asp:Button ID="cmdDelete" runat="server" CssClass="buttonDel" CommandName="gvDel" Text="Remove Banner" />
						</ItemTemplate>
					</asp:TemplateField>   
					<asp:TemplateField>
						<ItemStyle Width="5%" />
						<ItemTemplate>
							<asp:Literal ID="litAnch" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>             
				</Columns>
				<EmptyDataTemplate>
					<span class="warning">Its Empty Here... :(</span>
				</EmptyDataTemplate>
				<PagerStyle CssClass="gvPager"/>
			</asp:GridView>
		</div>
		<span class="space30"></span>
	</div>


</asp:Content>

