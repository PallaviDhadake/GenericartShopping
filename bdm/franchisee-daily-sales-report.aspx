<%@ Page Title="Franchisee Daily Sales Report" Language="C#" MasterPageFile="~/bdm/MasterBdm.master" AutoEventWireup="true" CodeFile="franchisee-daily-sales-report.aspx.cs" Inherits="bdm_franchisee_daily_sales_report" %>
<%@ MasterType VirtualPath="~/bdm/MasterBdm.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
			$('[id$=gvCompanyShops]').DataTable({
				columnDefs: [
					 { orderable: false, targets: [0, 1, 2, 3, 4, 6, 7] }
				],
				order: [[5, 'desc']]
			});

			$('[id$=gvOtherShops]').DataTable({
			    "fnDrawCallback": function (oSettings) {
			        $("a.frStats").fancybox({
			            type: 'iframe'
			        });
			        //alert( 'DataTables has redrawn the table' );
			    },
				columnDefs: [
					 { orderable: false, targets: [0, 1, 2, 3, 4, 6, 7] }
				],
				order: [[5, 'desc']]
			});
		});
	 </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<div class="card card-primary card-outline">
		<div class="card-header">
			<h3 class="card-title">
				<i class="fas fa-chart-line"></i>
				Daily Sales Report
			</h3>
		</div>
		<div class="card-body">
			<h4 class="mb-4">Shop's Daily Sales Report</h4>
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
					<asp:Button ID="btnShow" runat="server" CssClass="btn btn-primary" Text="Show Report"  />
				</div>
			</div>
			<span class="space15"></span>
			<ul class="nav nav-tabs" id="custom-content-above-tab" role="tablist">
				<li class="nav-item">
					<a class="nav-link active" id="custom-content-above-home-tab" data-toggle="pill" href="#custom-content-above-home" role="tab" aria-controls="custom-content-above-home" aria-selected="true">Company Own Shop's Sales Report</a>
				</li>
				<li class="nav-item">
					<a class="nav-link" id="custom-content-above-profile-tab" data-toggle="pill" href="#custom-content-above-profile" role="tab" aria-controls="custom-content-above-profile" aria-selected="false">Other Shop's Sales Report</a>
				</li>
			</ul>
			<div class="tab-custom-content">
				<p class="lead mb-0 text-bold text-success"><%= repTitle %></p>
			</div>
			<div class="tab-content" id="custom-content-above-tabContent">
				<div class="tab-pane fade show active" id="custom-content-above-home" role="tabpanel" aria-labelledby="custom-content-above-home-tab">
					
					<p class="lead mb-4 text-bold text-indigo"><%= ordTotalComp %></p>
					<!-- company own shops report -->
					<asp:GridView ID="gvCompanyShops" runat="server"
						CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
						AutoGenerateColumns="false" Width="100%" OnRowDataBound="gvCompanyShops_RowDataBound">
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
							<asp:BoundField DataField="orderAmount" HeaderText="Total Orders Amount">
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
					<!-- other shops report -->
					<p class="lead mb-4 text-bold text-indigo"><%= ordTotalOther %></p>
					<asp:GridView ID="gvOtherShops" runat="server"
						CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
						AutoGenerateColumns="false" Width="100%"  OnRowDataBound="gvOtherShops_RowDataBound">
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
							<asp:BoundField DataField="orderAmount" HeaderText="Total Orders Amount">
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

	<script type="text/javascript">
		$(document).ready(function () {
			$("a.frStats").fancybox({
				type: 'iframe'
			});
		});
	</script>
</asp:Content>

