<%@ Page Title="Customers List" Language="C#" MasterPageFile="~/franchisee/MasterFranchisee.master" AutoEventWireup="true" CodeFile="fav-shop-cust-list.aspx.cs" Inherits="franchisee_fav_shop_cust_list" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="//cdn.datatables.net/plug-ins/1.10.11/sorting/date-eu.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            $('[id$=gvFavShop]').DataTable({
                columnDefs: [
                    { "targets": 1, "type": "date-eu" },
                     { orderable: false, targets: [0, 1, 2, 3, 4] }
                ],
                order: [[1, 'desc']]
            });
        });
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Favorite Shop Customer List</h2>
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
                    <asp:BoundField DataField="CustomrtID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="regDate" HeaderText="Date">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CustomerName" HeaderText="Name">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CustomerMobile" HeaderText="Mobile No.">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ordCountCust" HeaderText="Customer Orders">
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

