<%@ Page Title="Cancelled By Customers Orders" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="customer-cancelled-orders.aspx.cs" Inherits="admingenshopping_customer_cancelled_orders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
        $(document).ready(function () {
            $('[id$=gvFavShop]').DataTable({
                columnDefs: [
                     { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7, 8] }
                ],
                order: [[4, 'desc']]
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Cancelled By Customer Orders</h2>
    <span class="space15"></span>
    <div id="viewOrder" runat="server">
        <span class="space15"></span>
        <div>
            <asp:GridView ID="gvFavShop" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" 
                GridLines="None" AutoGenerateColumns="false" Width="100%">
                <RowStyle CssClass="" />
                <HeaderStyle CssClass="bg-dark" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="FK_OrderCustomerID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CustomerName" HeaderText="Name">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CustomerMobile" HeaderText="Mobile No.">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CustomerEmail" HeaderText="Email Id">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OrderID" HeaderText="Order Id">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="orDate" HeaderText="Order Date">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OrderAmount" HeaderText="Order Amount">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ReasonTitle" HeaderText="Order Cancel Reason">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DeviceType" HeaderText="Device">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                </Columns>
                <EmptyDataTemplate>
                    <span class="warning">No Data to Display :(</span>
                </EmptyDataTemplate>
                <PagerStyle CssClass="gvPager" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>

