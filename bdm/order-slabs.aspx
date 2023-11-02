<%@ Page Title="Order Slabs" Language="C#" MasterPageFile="~/bdm/MasterBdm.master" AutoEventWireup="true" CodeFile="order-slabs.aspx.cs" Inherits="bdm_order_slabs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
	<script>
		$(document).ready(function () {
			$('[id$=gvZeroOrders]').DataTable({
				columnDefs: [
					 { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6] }
				],
				order: [[2, 'asc']]
			});

			$('[id$=gvLess100]').DataTable({
				columnDefs: [
					 { orderable: false, targets: [0, 1, 2, 3, 4, 6] }
				],
				order: [[5, 'asc']]
			});

			$('[id$=gvAbove100]').DataTable({
				columnDefs: [
					 { orderable: false, targets: [0, 1, 2, 3, 4, 6] }
				],
				order: [[5, 'asc']]
			});
		});
	 </script>
	<script type="text/javascript">
		function getShopInfo(shopId) {
			PageMethods.GetShopDetails(shopId, onSucess, onError);
			//alert("page method called");
			function onSucess(result) {
				//alert(result);
				document.getElementById('sName').innerHTML = result[0];
				document.getElementById('sOwnName').innerHTML = result[1];
				document.getElementById('sCode').innerHTML = result[2];
				document.getElementById('sMob').innerHTML = result[3];
				document.getElementById('sEmail').innerHTML = result[4];
				document.getElementById('sAddr').innerHTML = result[5];
				document.getElementById('sZipCode').innerHTML = result[6];
			}

			function onError(result) {
				alert(result.get_message());
			}
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div class="card card-primary card-outline">
		<div class="card-header">
			<h3 class="card-title">
				<i class="fas fa-shopping-cart"></i>
				Order Slabs
			</h3>
		</div>
		<div class="card-body">
			<h4 class="mb-4">Shop's Order Slab Report</h4>
			<div class="row">
			    <div class="col-md-3 form-group">
				    <label>From Date :*</label>
				    <asp:TextBox ID="txtFDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
			    </div>
			    <div class="col-md-3 form-group">
				    <label>From Date :*</label>
				    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
			    </div>
			    <div class="col-md-2 form-group">
				    <span style="height:7px; display:block; width:100%;"></span><br />
				    <asp:Button ID="btnShow" runat="server" CssClass="btn btn-primary" Text="Show Report" OnClick="btnShow_Click" />
			    </div>
		    </div>
		    <span class="space15"></span>
			<ul class="nav nav-tabs" id="custom-content-above-tab" role="tablist">
				<li class="nav-item">
					<a class="nav-link active" id="custom-content-above-home-tab" data-toggle="pill" href="#custom-content-above-home" role="tab" aria-controls="custom-content-above-home" aria-selected="true">Zero Orders</a>
				</li>
				<li class="nav-item">
					<a class="nav-link" id="custom-content-above-profile-tab" data-toggle="pill" href="#custom-content-above-profile" role="tab" aria-controls="custom-content-above-profile" aria-selected="false">1 - 100 Orders</a>
				</li>
				<li class="nav-item">
					<a class="nav-link" id="custom-content-above-messages-tab" data-toggle="pill" href="#custom-content-above-messages" role="tab" aria-controls="custom-content-above-messages" aria-selected="false">Above 100 Orders</a>
				</li>
			</ul>
			<div class="tab-custom-content">
				<p class="lead mb-0 text-bold text-success"><%= ordTitle %></p>
			</div>
			<div class="tab-content" id="custom-content-above-tabContent">
				<div class="tab-pane fade show active" id="custom-content-above-home" role="tabpanel" aria-labelledby="custom-content-above-home-tab">
					<asp:GridView ID="gvZeroOrders" runat="server"
						CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
						AutoGenerateColumns="false" Width="100%" OnRowDataBound="gvZeroOrders_RowDataBound">
						<RowStyle CssClass="" />
						<HeaderStyle CssClass="bg-dark" />
						<AlternatingRowStyle CssClass="alt" />
						<Columns>
							<asp:BoundField DataField="FranchID">
								<HeaderStyle CssClass="HideCol" />
								<ItemStyle CssClass="HideCol" />
							</asp:BoundField>
							<asp:BoundField DataField="FranchShopCode" HeaderText="Shop Code">
								<ItemStyle Width="10%" />
							</asp:BoundField>
							<asp:BoundField DataField="FranchName" HeaderText="Shop Name">
								<ItemStyle Width="25%" />
							</asp:BoundField>
							<asp:BoundField DataField="DistrictName" HeaderText="District">
								<ItemStyle Width="10%" />
							</asp:BoundField>
							<asp:BoundField DataField="CityName" HeaderText="City">
								<ItemStyle Width="10%" />
							</asp:BoundField>
							<asp:BoundField DataField="ordersCount" HeaderText="Total Orders">
								<ItemStyle Width="10%" />
							</asp:BoundField>
							<asp:TemplateField HeaderText="">
								<ItemStyle Width="5%" />
								<ItemTemplate>
									<asp:Literal ID="litAnch" runat="server"></asp:Literal>
								</ItemTemplate>
							</asp:TemplateField>
						</Columns>
						<EmptyDataTemplate>
							<span class="warning">No Data to Display :(</span>
						</EmptyDataTemplate>
						<PagerStyle CssClass="gvPager" />
					</asp:GridView>
				</div>
				<div class="tab-pane fade" id="custom-content-above-profile" role="tabpanel" aria-labelledby="custom-content-above-profile-tab">
					<asp:GridView ID="gvLess100" runat="server"
						CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
						AutoGenerateColumns="false" Width="100%" OnRowDataBound="gvLess100_RowDataBound">
						<RowStyle CssClass="" />
						<HeaderStyle CssClass="bg-dark" />
						<AlternatingRowStyle CssClass="alt" />
						<Columns>
							<asp:BoundField DataField="Fk_FranchID">
								<HeaderStyle CssClass="HideCol" />
								<ItemStyle CssClass="HideCol" />
							</asp:BoundField>
							<asp:BoundField DataField="FranchShopCode" HeaderText="Shop Code">
								<ItemStyle Width="10%" />
							</asp:BoundField>
							<asp:BoundField DataField="FranchName" HeaderText="Shop Name">
								<ItemStyle Width="25%" />
							</asp:BoundField>
							<asp:BoundField DataField="DistrictName" HeaderText="District">
								<ItemStyle Width="10%" />
							</asp:BoundField>
							<asp:BoundField DataField="CityName" HeaderText="City">
								<ItemStyle Width="10%" />
							</asp:BoundField>
							<asp:BoundField DataField="ordersCount" HeaderText="Total Orders">
								<ItemStyle Width="10%" />
							</asp:BoundField>
							<asp:TemplateField HeaderText="">
								<ItemStyle Width="5%" />
								<ItemTemplate>
									<asp:Literal ID="litAnch" runat="server"></asp:Literal>
								</ItemTemplate>
							</asp:TemplateField>
						</Columns>
						<EmptyDataTemplate>
							<span class="warning">No Data to Display :(</span>
						</EmptyDataTemplate>
						<PagerStyle CssClass="gvPager" />
					</asp:GridView>
				</div>
				<div class="tab-pane fade" id="custom-content-above-messages" role="tabpanel" aria-labelledby="custom-content-above-messages-tab">
					<asp:GridView ID="gvAbove100" runat="server"
						CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
						AutoGenerateColumns="false" Width="100%" OnRowDataBound="gvAbove100_RowDataBound">
						<RowStyle CssClass="" />
						<HeaderStyle CssClass="bg-dark" />
						<AlternatingRowStyle CssClass="alt" />
						<Columns>
							<asp:BoundField DataField="Fk_FranchID">
								<HeaderStyle CssClass="HideCol" />
								<ItemStyle CssClass="HideCol" />
							</asp:BoundField>
							<asp:BoundField DataField="FranchShopCode" HeaderText="Shop Code">
								<ItemStyle Width="10%" />
							</asp:BoundField>
							<asp:BoundField DataField="FranchName" HeaderText="Shop Name">
								<ItemStyle Width="25%" />
							</asp:BoundField>
							<asp:BoundField DataField="DistrictName" HeaderText="District">
								<ItemStyle Width="10%" />
							</asp:BoundField>
							<asp:BoundField DataField="CityName" HeaderText="City">
								<ItemStyle Width="10%" />
							</asp:BoundField>
							<asp:BoundField DataField="ordersCount" HeaderText="Total Orders">
								<ItemStyle Width="10%" />
							</asp:BoundField>
							<asp:TemplateField HeaderText="">
								<ItemStyle Width="5%" />
								<ItemTemplate>
									<asp:Literal ID="litAnch" runat="server"></asp:Literal>
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
		</div>
	</div>

	<div class="modal fade" id="modal-lg">
		<div class="modal-dialog modal-lg">
			<div class="modal-content">
				<div class="modal-header">
					<h4 class="modal-title">Franchisee Details</h4>
					<button type="button" class="close" data-dismiss="modal" aria-label="Close">
						<span aria-hidden="true">&times;</span>
					</button>
				</div>
				<div class="modal-body">
					<div class="row">
						<div class="col-sm-6">
							<table class="form_table">
								<tr>
									<td style="width:40%;"><span class="formLable bold_weight">Shop Name :</span></td>
									<td style="width:60%;"><span class="formLable" id="sName"></span> </td>
								</tr>
								<tr>
									<td><span class="formLable bold_weight">Shop Owner Name :</span></td>
									<td><span class="formLable" id="sOwnName"></span> </td>
								</tr>
								<tr>
									<td><span class="formLable bold_weight">Shop Code :</span></td>
									<td><span class="formLable" id="sCode"></span> </td>
								</tr>
								<tr>
									<td><span class="formLable bold_weight">Mobile No :</span></td>
									<td><span class="formLable" id="sMob"></span> </td>
								</tr>
								<tr>
									<td><span class="formLable bold_weight">Email :</span></td>
									<td><span class="formLable" id="sEmail"></span> </td>
								</tr>
								<tr>
									<td><span class="formLable bold_weight">Address :</span></td>
									<td><span class="formLable" id="sAddr"></span> </td>
								</tr>
								<tr>
									<td><span class="formLable bold_weight">Pincode :</span></td>
									<td><span class="formLable" id="sZipCode"></span> </td>
								</tr>
							</table>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
</asp:Content>

