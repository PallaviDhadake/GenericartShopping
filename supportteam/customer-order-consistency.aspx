<%@ Page Title="Customer Order Consistency" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="customer-order-consistency.aspx.cs" Inherits="supportteam_customer_order_consistency" %>
<%@ MasterType VirtualPath="~/supportteam/MasterSupport.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        #datatable .thead-dark{background:#171717; color:#fff;}
    </style>
    <script>
        $(document).ready(function () {
            $('[id$=datatable]').DataTable({
                columnDefs: [
                     { orderable: false, targets: [0, 1, 2, 3, 4] }
                ],
                order: [[0, 'asc']]
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Customer Order Consistency Report</h2>
	<span class="space15"></span>

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-4">
                <span class="formLable dspBlk">Select Quarter :</span>
                <asp:DropDownList ID="ddrQuarter" CssClass="form-control" runat="server" >
                    <asp:ListItem Value="0"><-Select-></asp:ListItem>
                    <asp:ListItem Value="1">First Quarter (APR-JUN)</asp:ListItem>
                    <asp:ListItem Value="2">Second Quarter (JUL-SEP)</asp:ListItem>
                    <asp:ListItem Value="3">Third Quarter (OCT-DEC)</asp:ListItem>
                    <asp:ListItem Value="4">Fourth Quarter (JAN-MAR)</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-4">
                <span class="formLable dspBlk">Select Year :</span>
                <asp:DropDownList ID="ddrYear" CssClass="form-control" runat="server" >
                </asp:DropDownList>
            </div>
            <div class="col-md-4">
                <span class="space20"></span>
                <asp:Button ID="btnShow" runat="server" Text="Show Report" CssClass="btn btn-md btn-primary" OnClick="btnShow_Click" />
            </div>
        </div>
    </div>
    <span class="space30"></span>
    <div id="viewOrder" runat="server">
		<%--<table id="datatable" class="table table-striped table-bordered table-hover w-100">
			<thead class="thead-dark">
				<tr>
					<th>Customer Name</th>
					<th>Mobile No.</th>
					<th>Order ID</th>
					<th>Date</th>
					<th>Customer</th>
				</tr>
			</thead>
		</table>--%>
        <%= reportMarkup %>
	</div>
</asp:Content>

