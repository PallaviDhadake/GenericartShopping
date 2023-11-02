<%@ Page Title="Monthly Orders Followup Report" Language="C#" MasterPageFile="~/franchisee/MasterFranchisee.master" AutoEventWireup="true" CodeFile="monthly-order-followup.aspx.cs" Inherits="franchisee_monthly_order_followup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script src="//cdn.datatables.net/plug-ins/1.10.11/sorting/date-eu.js" type="text/javascript"></script>
	<script>
		$(document).ready(function () {
			$('[id$=gvOrder]').DataTable({
				columnDefs: [
					{ "targets": 5, "type": "date-eu" },
					{ orderable: false, targets: [0, 1, 2, 3, 4, 6, 7] }
				],
				order: [[8, 'asc']]
			});
		});
	</script>

	<script type="text/javascript">
		function followupOption(optValue){
			document.getElementById("txtOption").value = optValue;
		}
	</script>

	<style>
/* The container */
.container {
  display: block;
  position: relative;
  padding-left: 35px;
  margin-bottom: 12px;
  cursor: pointer;
  font-size: 22px;
  -webkit-user-select: none;
  -moz-user-select: none;
  -ms-user-select: none;
  user-select: none;
}

/* Hide the browser's default radio button */
.container input {
  position: absolute;
  opacity: 0;
  cursor: pointer;
}

/* Create a custom radio button */
.checkmark {
  position: absolute;
  top: 0;
  left: 0;
  height: 25px;
  width: 25px;
  background-color: #eee;
  border-radius: 50%;
}

/* On mouse-over, add a grey background color */
.container:hover input ~ .checkmark {
  background-color: #ccc;
}

/* When the radio button is checked, add a blue background */
.container input:checked ~ .checkmark {
  background-color: #2196F3;
}

/* Create the indicator (the dot/circle - hidden when not checked) */
.checkmark:after {
  content: "";
  position: absolute;
  display: none;
}

/* Show the indicator (dot/circle) when checked */
.container input:checked ~ .checkmark:after {
  display: block;
}

/* Style the indicator (dot/circle) */
.container .checkmark:after {
	top: 9px;
	left: 9px;
	width: 8px;
	height: 8px;
	border-radius: 50%;
	background: white;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

	<div class="proNavPanel">
		<span style="position:absolute; top:-15px; left:280px; z-index:555; " class="absCount"><%= gvCount1 %></span>
		<span style="position:absolute; top:-15px; left:500px; z-index:555; " class="absCount"><%= gvCount2 %></span>
		<ul class="proNav">
			<li><a class="act">Monthly Orders Followup</a></li>
			<li><a href="survey-followup-report.aspx">Survey Followup</a></li>
		</ul>
	</div>
	<span class="space20"></span>

	<h2 class="pgTitle">Monthly Orders Followup Report</h2>
	<span class="space15"></span>

	<div id="viewOrder" runat="server">
		<div>
			<asp:GridView ID="gvOrder" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
				AutoGenerateColumns="false" OnRowDataBound="gvOrder_RowDataBound" Width="100%">
				<RowStyle CssClass="" />
				<HeaderStyle CssClass="bg-dark" />
				<AlternatingRowStyle CssClass="alt" />
				<Columns>
					<asp:BoundField DataField="CustomrtID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="CustomerName" HeaderText="Name">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="CustomerMobile" HeaderText="Mobile No.">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="totalMonthlyOrders" HeaderText="Total Monthly Orders">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="recentOrderId" HeaderText="Recent OrderID">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="recentOrderDate" HeaderText="Order Date">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:TemplateField HeaderText="Days Passed">
						<ItemStyle Width="5%" />
						<ItemTemplate>
							<asp:Literal ID="litDays" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Followup Feedback">
						<ItemStyle Width="15%" />
						<ItemTemplate>
							<asp:Literal ID="litAnch" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="sortCol">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
				</Columns>
				<EmptyDataTemplate>
					<span class="warning">No Orders to Display :(</span>
				</EmptyDataTemplate>
				<PagerStyle CssClass="gvPager" />
			</asp:GridView>
		</div>
	</div>

	<!-- Modal Follow Up -->
	<div class="modal fade" id="modal-lg">
		<div class="modal-dialog modal-lg">
			<div class="modal-content">
			
				<div class="modal-header">
					<h4 class="modal-title">Order Follow Up</h4>
					<button type="button" class="close" data-dismiss="modal" aria-label="Close">
						<span aria-hidden="true">&times;</span>
					</button>
				</div>
				<div class="modal-body">
				<label class="container" style="color:#108203">Order Done, Call back in next month
					<input type="radio" checked="checked" name="radio" onclick="javascript: followupOption(3);">
					<span class="checkmark"></span>
				</label>
				<label class="container" style="color:#108203">Followup Again
					<input type="radio" name="radio" onclick="javascript: followupOption(4);">
					<span class="checkmark"></span>
				</label>
				<label class="container" style="color:#ff0000">Not interested for monthly order
					<input type="radio" name="radio" onclick="javascript: followupOption(2);">
					<span class="checkmark"></span>
				</label>
				<%--<label class="container" style="color:#ff0000">Phone not picked 
					<input type="radio" name="radio" onclick="javascript: followupOption(4);">
					<span class="checkmark"></span>
				</label>--%>
				<label class="container" style="color:#ff0000">Wrongly select as monthly order
					<input type="radio" name="radio" onclick="javascript: followupOption(1);">
					<span class="checkmark"></span>
				</label>

					<input id="txtOption" type="text" value="1"  style="display:none;"/>
					<input id="txtOrderRef" type="text" style="display:none;" />
				</div>

				<div class="modal-footer">
					<input id="btnSubmit" type="button" value="SUBMIT FOLLOW UP" class="btn btn-primary" onclick="javascript: PreFollowUp();" />
				</div>    
			</div>
			
		</div>
	</div>

	<script>
		$('#modal-lg').on('show.bs.modal', function (event) {
			var button = $(event.relatedTarget) // Button that triggered the modal
			var recipient = button.data('whatever') // Extract info from data-* attributes
			// If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
			// Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
			var modal = $(this)
			modal.find('.modal-title').text('Follow Up Order: #' + recipient)

			//modal.find('.modal-body input').val(recipient)
			modal.find('#txtOrderRef').val(recipient)
			
		})
	</script>


<script type = "text/javascript">
	function PreFollowUp() {

		var orderText = document.getElementById("txtOrderRef").value;
		var optionValText = document.getElementById("txtOption").value;

		//var obj = {};
		//obj.orderText = document.getElementById("txtOrderRef").value;
		//obj.optionValText = document.getElementById("txtOption").value;

		SubmitFollowUp(orderText, optionValText);
	}

	function SubmitFollowUp(orderRefX, optValX) {
		
		$.ajax({
			type: "POST",
			url: "monthly-order-followup.aspx/OrderFollowUp",
			data: JSON.stringify({ OrderIdRef: orderRefX, OptionSelected: optValX }),
			//data: {
			//    "OrderIdRef": orderRefX,
			//    "OptionSelected": optValX
			//},
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: function (r) {
				//alert(r.d);
				$('#modal-lg').modal('hide');
				TostTrigger('success', 'Order:#' + orderRefX + ' Follow Up Submitted successfully');
			}
		});
		return false;
	}
	   

</script>

</asp:Content>

