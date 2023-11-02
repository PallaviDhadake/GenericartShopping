<%@ Page Title="Prescription Requests" Language="C#" MasterPageFile="~/doctors/MasterDoctor.master" AutoEventWireup="true" CodeFile="prescription-requests.aspx.cs" Inherits="doctors_prescription_requests" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/doctors/MasterDoctor.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	 <script>
		 $(document).ready(function () {
			 $('[id$=gvRx]').DataTable({
				 columnDefs: [
					  { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11] }
				 ],
				 order: [[0, 'desc']]
			 });

			 $('[id$=gvRxItems]').DataTable({
				 columnDefs: [
					  { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10] }
				 ],
				 order: [[0, 'desc']]
			 });
		 });
	 </script>
	
	<%--<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.4/jquery.min.js" type = "text/javascript"></script>
	<script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js" type = "text/javascript"></script>
	<link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel = "Stylesheet" type="text/css" />--%>

	<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />
	<link rel="stylesheet" href="/resources/demos/style.css" />
	<script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

	<script type="text/javascript">
		$(document).ready(function () {
			SearchText();
			//alert("document ready call");
		});
		function SearchText() {
			
			//PageMethods.set_path('/saving-calculator.aspx');
			$("#<%= txtMedName.ClientID %>").autocomplete({
				source: function (request, response) {
					$.ajax({
						type: "POST",
						contentType: "application/json; charset=utf-8",
						//url: "saving-calculator.aspx/GetMedName",
						url: "prescription-requests.aspx/GetMedName",
						data: "{'medName':'" + document.getElementById('<%= txtMedName.ClientID %>').value + "'}",
						dataType: "json",
						success: function (data) {
							//response(data.d);
							//alert(data.d);
							response($.map(data.d, function (item) {
								return {
									label: item.split('#')[0],
									val: item.split('#')[1]
								}
							}))
						},
						error: function (result) {
							alert("No Match");
						}
					});
				},
				select: function (e, i) {
					$("#<%= txtMedId.ClientID %>").val(i.item.val);
					PageMethods.GetMedInfo(i.item.val, onSucess, onError);
					//alert(i.item.val);
					function onSucess(result) {
						//alert(result);
					}

					function onError(result) {
						alert(result.get_message());
					}
				},
				minLength: 1
			});
			}
	</script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<h2 class="pgTitle">Requested Prescriptions</h2>
	<span class="space15"></span>

	<div id="viewRx" runat="server">
		<span class="space15"></span>
		<div>
			<asp:GridView ID="gvRx" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
				AutoGenerateColumns="false" OnRowDataBound="gvRx_RowDataBound" Width="100%">
				<RowStyle CssClass="" />
				<HeaderStyle CssClass="bg-dark" />
				<AlternatingRowStyle CssClass="alt" />
				<Columns>
					<asp:BoundField DataField="PreReqID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="PreReqStatus">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="FK_CustomerID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="reqDate" HeaderText="Date">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="PreReqName" HeaderText="Name">
						<ItemStyle Width="15%" />
					</asp:BoundField>
					<asp:BoundField DataField="PreReqMobile" HeaderText="Mobile No.">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="PreReqAge" HeaderText="Age">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="PreReqDisease" HeaderText="Disease">
						<ItemStyle Width="25%" />
					</asp:BoundField>
					<asp:TemplateField HeaderText="Status">
						<ItemStyle Width="5%" />
						<ItemTemplate>
							<asp:Literal ID="litStatus" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="DeviceType" HeaderText="Req. From">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:TemplateField>
						<ItemStyle Width="5%" />
						<ItemTemplate>
							<asp:Literal ID="litView" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField>
						<ItemStyle Width="5%" />
						<ItemTemplate>
							<asp:Literal ID="litForword" runat="server"></asp:Literal>
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

	<div id="editRx" runat="server">
		<div class="card">
			<div class="card-header">
				<h3 class="large colorLightBlue">Prescription Request Details</h3>
				<span class="badge badge-pill badge-success text-lg"><%= deviceType %></span>
			</div>
			<div class="card-body">

				<table class="form_table">
					<tr>
						<td><span class="colorLightBlue">Request Id :</span></td>
						<td>
							<asp:Label ID="Label1" CssClass="colorLightBlue" runat="server" Text=""><%= ordData[0] %></asp:Label>
						</td>
					</tr>
					<tr>
						<td style="width: 25%"><span class="formLable bold_weight">Date:</span></td>
						<td><span class="formLable"><%= ordData[1] %></span> </td>
					</tr>
					<tr>
						<td style="width: 25%"><span class="formLable bold_weight">Customer Name:</span></td>
						<td><span class="formLable"><%= ordData[2] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Mobile No. :</span></td>
						<td><span class="formLable"><%= ordData[3] %></span> </td>
					</tr>

					<tr>
						<td><span class="formLable bold_weight">Age :</span></td>
						<td><span class="formLable"><%= ordData[4] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Gender :</span></td>
						<td><span class="formLable"><%= ordData[5] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Address :</span></td>
						<td><span class="formLable"><%= ordData[10] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Disease :</span></td>
						<td><span class="formLable"><%= ordData[6] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Medicines :</span></td>
						<td><span class="formLable" style="display: block; width: 60% !important;"><%= ordData[7] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Request For :</span></td>
						<td><span class="formLable"><%= ordData[8] %></span> </td>
					</tr>
					<tr>
						<td><span class="formLable bold_weight">Request From :</span></td>
						<td><span class="formLable"><%= ordData[9] %></span> </td>
					</tr>
				</table>
			</div>
			 <div class="card-header">
				<h3 class="large colorLightBlue">Create Prescription</h3>
			</div>
			<div class="card-body">
				<div class="form-row">
					<div class="form-group col-md-6">
						<label>Medicine Name :*</label>
						<asp:TextBox ID="txtMedName" runat="server" CssClass="form-control" Width="100%" MaxLength="200"></asp:TextBox>
						<asp:TextBox ID="txtMedId" runat="server" CssClass="form-control" Enabled="false" Visible="false"></asp:TextBox>
					</div>
					<div class="form-group col-md-6">
						<label>Quantity :*</label>
						<asp:TextBox ID="txtQty" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
					</div>
					<div class="form-group col-md-6">
						<label>Dose 1 :*</label>
						<asp:TextBox ID="txtDose1" runat="server" CssClass="form-control" Width="100%" MaxLength="10"></asp:TextBox>
					</div>
					<div class="form-group col-md-6">
						<label>Dose 2 :*</label>
						<asp:TextBox ID="txtDose2" runat="server" CssClass="form-control" Width="100%" MaxLength="10"></asp:TextBox>
					</div>
					<div class="form-group col-md-6">
						<label>Dose 3 :*</label>
						<asp:TextBox ID="txtDose3" runat="server" CssClass="form-control" Width="100%" MaxLength="10"></asp:TextBox>
					</div>
					<div class="form-group col-md-6">
						<label>Note :*</label>
						<asp:TextBox ID="txtNote" runat="server" CssClass="form-control" Width="100%" MaxLength="100"></asp:TextBox>
					</div>
					<div class="form-group col-md-6">
						<label>Dosage Days :*</label>
						<asp:TextBox ID="txtDays" runat="server" CssClass="form-control" Width="100%" MaxLength="100"></asp:TextBox>
					</div>
				</div>
				<span class="space10"></span>
				<asp:Button ID="btnAdd" runat="server" CssClass="btn btn-md btn-primary" Text="Submit" OnClick="btnAdd_Click" />
				<span class="space30"></span>
				<asp:GridView ID="gvRxItems" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
					AutoGenerateColumns="false" Width="100%" OnRowCommand="gvRxItems_RowCommand">
					<RowStyle CssClass="" />
					<HeaderStyle CssClass="bg-dark" />
					<AlternatingRowStyle CssClass="alt" />
					<Columns>
						<asp:BoundField DataField="PreItemId">
							<HeaderStyle CssClass="HideCol" />
							<ItemStyle CssClass="HideCol" />
						</asp:BoundField>
						<asp:BoundField DataField="FK_PreReqID">
							<HeaderStyle CssClass="HideCol" />
							<ItemStyle CssClass="HideCol" />
						</asp:BoundField>
						<asp:BoundField DataField="FK_PreProductID">
							<HeaderStyle CssClass="HideCol" />
							<ItemStyle CssClass="HideCol" />
						</asp:BoundField>
						<asp:BoundField DataField="ProductName" HeaderText="Medicine Name">
							<ItemStyle Width="20%" />
						</asp:BoundField>
						<asp:BoundField DataField="PreItemQty" HeaderText="Quantity">
							<ItemStyle Width="10%" />
						</asp:BoundField>
						 <asp:BoundField DataField="PreItemDose1" HeaderText="Dose 1">
							<ItemStyle Width="10%" />
						</asp:BoundField>
						 <asp:BoundField DataField="PreItemDose2" HeaderText="Dose 2">
							<ItemStyle Width="10%" />
						</asp:BoundField>
						 <asp:BoundField DataField="PreItemDose3" HeaderText="Dose 3">
							<ItemStyle Width="10%" />
						</asp:BoundField>
						<asp:BoundField DataField="PreItemNote" HeaderText="Note">
							<ItemStyle Width="15%" />
						</asp:BoundField>
						<asp:BoundField DataField="PreItemDays" HeaderText="Days">
							<ItemStyle Width="10%" />
						</asp:BoundField>
						<asp:TemplateField HeaderText="Delete">
							<ItemStyle Width="5%" />
							<ItemTemplate>
								<asp:Button ID="cmdDelete" runat="server" CssClass="gDel" CommandName="gvDel" Text="" OnClientClick="return confirm('Are you sure to delete?');" />
							</ItemTemplate>
						</asp:TemplateField>
					</Columns>
					<EmptyDataTemplate>
						<span class="warning">No Data to Display :(</span>
					</EmptyDataTemplate>
					<PagerStyle CssClass="gvPager" />
				</asp:GridView>
				<span class="space20"></span>
				<span class="text-bold text-danger">Note : Before uploading prescription you have to generate prescription using following button named "Generate Prescriptiom"</span>
				<span class="space20"></span>
				<div class="form-row">
					<div class="form-group col-md-6">
						<label>Select Prescription File :*</label>
						<asp:FileUpload ID="fuRx" runat="server" CssClass="form-control-file" />
					</div>
				</div>
				<span class="space10"></span>
				<asp:Button ID="btnUpload" runat="server" CssClass="btn btn-md btn-primary" Text="Upload Prescription" OnClick="btnUpload_Click" />
				<span class="space10"></span>
				<%= uploadedRx %>
			</div>
		</div>
		<span class="space15"></span>
		<%--<asp:Button ID="btnAssign" runat="server" CssClass="btn btn-md btn-info" Text="Generate Prescription" />--%>
		<a href="generate-prescription.aspx?rxId=<%= rxId %>" class="btn btn-md btn-info" target="_blank">Generate Prescription</a>
		<a href="prescription-requests.aspx" class="btn btn-md btn-outline-dark">Back</a>
	</div>
</asp:Content>

