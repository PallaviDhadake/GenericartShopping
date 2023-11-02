<%@ Page Title="Prescription Received From Doctors" Language="C#" MasterPageFile="~/franchisee/MasterFranchisee.master" AutoEventWireup="true" CodeFile="received-prescriptions.aspx.cs" Inherits="franchisee_received_prescriptions" %>
<%@ MasterType VirtualPath="~/franchisee/MasterFranchisee.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script>
		$(document).ready(function () {
			$('[id$=gvRx]').DataTable({
				columnDefs: [
					 { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7, 8] }
				],
				order: [[0, 'desc']]
			});
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<h2 class="pgTitle">Prescriptions Received</h2>
	<span class="space15"></span>

	<span class="text-bold text-sm text-danger">Click on patient name to view details</span>
	<span class="space15"></span>


	<div id="viewRx" runat="server">
		<div class="formPanel table-responsive-md">
			<asp:GridView ID="gvRx" runat="server" CssClass="table table-striped table-bordered table-hover" GridLines="None"
				AutoGenerateColumns="false" OnRowDataBound="gvRx_RowDataBound"  >
				<HeaderStyle CssClass="thead-dark" />
				<RowStyle CssClass="" />
				<AlternatingRowStyle CssClass="alt" />
				<Columns>
					<asp:BoundField DataField="PrescFwdID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="PrescFwdStatus">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="PrescImg">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="FK_PreReqID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="frwdDate" HeaderText="Date">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="DocName" HeaderText="Forwarded By">
						<ItemStyle Width="15%" />
					</asp:BoundField>
					<asp:BoundField DataField="PreReqName" HeaderText="Patient Name">
						<ItemStyle Width="15%" />
					</asp:BoundField>
					<asp:TemplateField HeaderText="Download">
						<ItemStyle Width="10%" />
						<ItemTemplate>
							<asp:Literal ID="litDownloadRx" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Status">
						<ItemStyle Width="5%" />
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

	<div class="modal fade" id="modal-lg">
		<div class="modal-dialog modal-lg">
			<div class="modal-content">
				<div class="modal-header">
					<h4 class="modal-title">Customer Details</h4>
					<button type="button" class="close" data-dismiss="modal" aria-label="Close">
						<span aria-hidden="true">&times;</span>
					</button>
				</div>
				<div class="modal-body">
					<div class="row">
						<div class="col-sm-6">
							<span id="custMarkup"></span>
							<span class="space10"></span>
							<%--<table class="form_table">
								<tr>
									<td><span class="formLable bold_weight">Id :</span></td>
									<td><span class="formLable"><%= rxData[7] %></span> </td>
								</tr>
								<tr>
									<td style="width: 40%;"><span class="formLable bold_weight">Prescription Request Date :</span></td>
									<td style="width: 60%;"><span class="formLable"><%= rxData[6] %></span> </td>
								</tr>
								<tr>
									<td><span class="formLable bold_weight">Name :</span></td>
									<td><span class="formLable"><%= rxData[0] %></span> </td>
								</tr>
								<tr>
									<td><span class="formLable bold_weight">Mobile No :</span></td>
									<td><span class="formLable"><%= rxData[1] %></span> </td>
								</tr>
								<tr>
									<td><span class="formLable bold_weight">Age :</span></td>
									<td><span class="formLable"><%= rxData[2] %></span> </td>
								</tr>
								<tr>
									<td><span class="formLable bold_weight">Gender :</span></td>
									<td><span class="formLable"><%= rxData[3] %></span> </td>
								</tr>
								<tr>
									<td><span class="formLable bold_weight">Address :</span></td>
									<td><span class="formLable"><%= rxData[4] %></span> </td>
								</tr>
								<tr>
									<td><span class="formLable bold_weight">Disease :</span></td>
									<td><span class="formLable"><%= rxData[5] %></span> </td>
								</tr>
							</table>--%>
						</div>
					</div>
				</div>
			</div>
			<!-- /.modal-content -->
		</div>
		<!-- /.modal-dialog -->
	</div>
	<!-- /.modal -->



	<script type="text/javascript">
		function GetCustDetails(rxReqId) {
			//PageMethods.set_path('/received-prescriptions.aspx');
			PageMethods.GetRxCustInfo(rxReqId, onSucess, onError);
			//alert("page method called" + rxReqId);
			function onSucess(result) {
				//alert(result);
				document.getElementById("custMarkup").innerHTML = result;
			}

			function onError(result) {
				alert(result.get_message());
			}
		}

		function MarkAsComplete(presReqId) {
		    PageMethods.CompleteOrder(presReqId, onSucess, onError);
		    //alert("page method called" + presReqId);
		    function onSucess(result) {
		        //alert(result);
		        $('#modal-lg').modal('hide');
		        TostTrigger('success', 'Marked as Completed');
		        waitAndMove("<%= Master.rootPath + "franchisee/received-prescriptions.aspx" %>", 500);
		    }

		    function onError(result) {
		        alert(result.get_message());
		    }
		}

		
		<%--$("#modal-lg").on("hidden.bs.modal", function () {
			window.location = <%= Master.rootPath + "franchisee/received-prescriptions.aspx" %>;
		});--%>
	</script>
</asp:Content>

