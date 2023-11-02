<%@ Page Title="" Language="C#" MasterPageFile="~/account/MasterAccount.master" AutoEventWireup="true" CodeFile="customer-details.aspx.cs" Inherits="account_customer_details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="//cdn.datatables.net/plug-ins/1.10.11/sorting/date-eu.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            $('[id$=gvCustDetail]').DataTable({
                columnDefs: [
                    { "targets": 8, "type": "date-eu" },
                    { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6] }
                ],
                order: [[8, 'desc']]
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">GOBP Customer Details</h2>
    <span class="space15"></span>

    <div class="row" id="daterange" runat="server">
        <div class="col-md-3">
            <asp:Button ID="btnExportOrders" runat="server" CssClass="btn btn-md btn-info" Text="Export To Excel" OnClick="btnExportOrders_Click" />
        </div>
    </div>
    <span class="space15"></span>

    <span class="text-lg text-bold text-purple">GOBP Name : </span>
    <span class="text-lg text-bold text-purple"><%= custData[0] %></span>
    <span class="space10"></span>

    <span class="text-lg text-bold text-purple">Month : </span>
    <span class="text-lg text-bold text-purple"><%=custData[1] %></span>
    <span class="space15"></span>

    <div class="viewOrder" runat="server">
        <div>
            <asp:GridView ID="gvGOBPIncentive" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
                AutoGenerateColumns="false" Width="100%" ShowFooter="true" OnRowDataBound="gvGOBPIncentive_RowDataBound">
                <RowStyle CssClass="" />
                <HeaderStyle CssClass="bg-dark" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="CustomerName" HeaderText="Customer Name">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OrderID" HeaderText="Order ID">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="SaleBill" HeaderText="Sale Bill No.">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TotalSaleBill" HeaderText="Total Sale Bill">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ProductList" HeaderText="Product List">
                        <ItemStyle Width="20%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="BillAmount" HeaderText="Bill Amount">
                        <ItemStyle Width="10" />
                    </asp:BoundField>                    
                    <asp:BoundField DataField="TotalGST" HeaderText="Total GST">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Amount" HeaderText="Amount">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="GOBPIncentive" HeaderText="GOBP Incentive %">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="IncentiveAmount" HeaderText="Incentive Amount">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>             
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

