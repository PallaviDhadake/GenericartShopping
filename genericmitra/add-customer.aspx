<%@ Page Title="Add Customer | Generic Mitra Admin Panel" Language="C#" MasterPageFile="~/genericmitra/MasterGenMitra.master" AutoEventWireup="true" CodeFile="add-customer.aspx.cs" Inherits="genericmitra_add_customer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script src="//cdn.datatables.net/plug-ins/1.10.11/sorting/date-eu.js" type="text/javascript"></script>
	<script>
		$(document).ready(function () {
			$('[id$=gvCustomer]').DataTable({
				columnDefs: [
					{ "targets": 2, "type": "date-eu" },
					{ orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7, 8] }
				],
				order: [[2, 'desc']]
			});
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<h2 class="pgTitle">Add Customer</h2>
	<span class="space15"></span>

	<div id="viewCust" runat="server">
		<a href="add-customer.aspx?action=new" runat="server" class="btn btn-primary btn-md">Add New</a>
		<span class="space15"></span>

		<%--gridview start--%>
		<asp:GridView ID="gvCustomer" runat="server" AutoGenerateColumns="False"
			CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None" Width="100%" OnRowDataBound="gvCustomer_RowDataBound" OnRowCommand="gvCustomer_RowCommand">

			<HeaderStyle CssClass="bg-dark" />
			<AlternatingRowStyle CssClass="alt" />
			<Columns>
				<asp:BoundField DataField="CustomrtID">
					<HeaderStyle CssClass="HideCol" />
					<ItemStyle CssClass="HideCol" />
				</asp:BoundField>
				<asp:BoundField DataField="CustomerFavShop">
					<HeaderStyle CssClass="HideCol" />
					<ItemStyle CssClass="HideCol" />
				</asp:BoundField>

				<asp:BoundField DataField="CustomerJoinDate" HeaderText="Date">
					<ItemStyle Width="5%" />
				</asp:BoundField>
				<asp:BoundField DataField="CustomerName" HeaderText="Name">
					<ItemStyle Width="5%" />
				</asp:BoundField>

				<asp:BoundField DataField="CustomerMobile" HeaderText="Mobile">
					<ItemStyle Width="5%" />
				</asp:BoundField>

				<asp:TemplateField HeaderText="Fav Shop">
					<ItemStyle Width="15%" />
					<ItemTemplate>
						<asp:Literal ID="litFavShop" runat="server"></asp:Literal>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Fav Shop Code">
					<ItemStyle Width="10%" />
					<ItemTemplate>
						<asp:TextBox ID="txtFavShop" CssClass="form-control" MaxLength="50" Width="100" runat="server" Text=""></asp:TextBox>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemStyle Width="10%" />
					<ItemTemplate>
						<asp:Button ID="cmdUpdate" runat="server" CssClass="btn btn-sm btn-primary" CommandName="gvUpdate" Text="Update" />
						<asp:Button ID="cmdDelete" runat="server" CssClass="btn btn-sm btn-danger" CommandName="gvRemove" Text="Remove" ToolTip="Remove Favourit Shop" />
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="">
					<ItemStyle Width="5%" />
					<ItemTemplate>
						<asp:Literal ID="litAnch" runat="server"></asp:Literal>
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
			<EmptyDataTemplate>
				<span class="warning">No Customer to Display :(</span>
			</EmptyDataTemplate>
			<PagerStyle CssClass="gvPager" />
		</asp:GridView>
		<%-- gridview End--%>
	</div>


	<div id="editCust" runat="server">
		<asp:UpdatePanel ID="UpdatePanel1" runat="server">
			<ContentTemplate>
				<%--card start--%>
				<div class="card card-primary">
					<div class="card-header">
						<h3 class="card-title">Add Customer</h3>
					</div>
					<%-- card body--%>
					<div class="card-body">
						<div class="colorLightBlue">
							<span>Id :</span>
							<asp:Label ID="lblId" runat="server" Text="[New]"></asp:Label>
						</div>

						<span class="space15"></span>
						<%--form row start--%>
						<div class="form-row">


							<div class="form-group col-md-6">
								<label>Name :*</label>
								<asp:TextBox ID="txtName" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
							</div>
							<div class="form-group col-md-6">
								<label>Mobile No :*</label>
								<asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" Width="100%" MaxLength="10"></asp:TextBox>
							</div>
							<div class="form-group col-md-6">
								<label>Email :*</label>
								<asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
							</div>
							<div class="form-group col-md-6">
								<label>Country :*</label>
								<asp:TextBox ID="txtCountry" runat="server" CssClass="form-control" Width="100%" MaxLength="30"></asp:TextBox>
							</div>
							<div class="form-group col-md-6">
								<label>State :*</label>
								<asp:TextBox ID="txtState" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
							</div>

							<div class="form-group col-md-6">
								<label>City :*</label>
								<asp:TextBox ID="txtCity" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
							</div>
							<div class="form-group col-md-6">
								<label>Pin Code :*</label>
								<asp:TextBox ID="txtPinCode" runat="server" CssClass="form-control" Width="100%" MaxLength="15"></asp:TextBox>
							</div>

							<div class="form-group col-md-6">
								<label>Address Type  : *</label>
								<asp:DropDownList ID="ddrAddrType" runat="server" CssClass="form-control" Width="100%">
									<asp:ListItem Value="0"><- Select -></asp:ListItem>
									<asp:ListItem Text="Home" Value="1"></asp:ListItem>
									<asp:ListItem Text="Office" Value="2"></asp:ListItem>
									<asp:ListItem Text="Other" Value="3"></asp:ListItem>

								</asp:DropDownList>
							</div>

							<div class="form-group col-md-6">
								<label>Address:*</label>
								<asp:TextBox ID="txtAddress" runat="server" CssClass="form-control textarea" TextMode="MultiLine" Height="150" Width="100%"></asp:TextBox>
							</div>

							<div class="form-group col-md-6">
								<label>Shop Code :</label>
								<asp:TextBox ID="txtShopCode" runat="server" CssClass="form-control" Width="100%" MaxLength="15"></asp:TextBox>
							</div>

						</div>
						<%--form row End--%>
					</div>
					<%-- card body end--%>
				</div>
				<%--card end--%>

				<!-- Button controls starts -->
				<span class="space10"></span>
				<%--  <%=errMsg %>--%>
				<span class="space10"></span>
				<asp:Button ID="btnSave" runat="server"
					CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
				<asp:Button ID="btnCancel" runat="server"
					CssClass="btn btn-outline-dark" Text="Cancel" OnClick="btnCancel_Click" />
				<div class="float_clear"></div>
				<!-- Button controls ends -->
			</ContentTemplate>
		</asp:UpdatePanel>
	</div>

</asp:Content>

