<%@ Page Title="Customer's Favourite Shop Report" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="cust-fav-shop-report.aspx.cs" Inherits="admingenshopping_cust_fav_shop_report" %>
<%@ MasterType VirtualPath="~/admingenshopping/MasterAdmin.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
        $(document).ready(function () {
            $('[id$=gvFavShop]').DataTable({
                columnDefs: [
                     { orderable: false, targets: [0, 1, 2, 3, 4, 5] }
                ],
                order: [[4, 'asc']]
            });
        });
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Customer's Favourite Shop Report</h2>
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
                    <asp:BoundField DataField="CustomerFavShop">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CustomerName" HeaderText="Name">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CustomerMobile" HeaderText="Mobile No.">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FranchName" HeaderText="Shop Name">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FranchShopCode" HeaderText="Shop Code">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ordCount" HeaderText="Total Orders">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ordCountCust" HeaderText="Customer Orders">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                   <%-- <asp:TemplateField HeaderText="Total Orders">
                        <ItemStyle Width="10%" />
                        <ItemTemplate>
                            <asp:Literal ID="litOrders" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                </Columns>
                <EmptyDataTemplate>
                    <span class="warning">No Data to Display :(</span>
                </EmptyDataTemplate>
                <PagerStyle CssClass="gvPager" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>

