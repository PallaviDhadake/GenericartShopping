<%@ Page Title="Customer Lookup" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="cust-lookup.aspx.cs" Inherits="supportteam_cust_lookup" %>

<%@ MasterType VirtualPath="~/supportteam/MasterSupport.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://pro.fontawesome.com/releases/v5.10.0/css/all.css" integrity="sha384-AYmEC3Yw5cVb3ZcuHtOA93w35dYTsvhLPVnYs9eStHfGJvOvKxVfELGroGkvsg+p" crossorigin="anonymous" />

    <script>
        $(document).ready(function () {
            $('[id$=gvEditOrder]').DataTable({
                columnDefs: [
                    { "targets": 8, "type": "date-eu" },
                    { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 9, 10, 11, 12] }
                ],
                order: [[8, 'desc']]
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">Customer Lookup Report</h2>
    <span class="space15"></span>
    <div class="row">
        <div class="form-group col-sm-3">
            <label class="text-sm">Customer Mobile No.:</label>
            <asp:TextBox ID="txtMob" runat="server" CssClass="form-control" placeholder="Registered Mobile No."></asp:TextBox>
        </div>
        <div class="col-sm-3">
            <span class="space30"></span>
            <asp:Button ID="btnShow" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnShow_Click" />
        </div>
    </div>
    <div id="repBtn" runat="server">
        <a class="fancylink btn btn-md btn-info" href="<%= lookupUrl %>">View Report</a>
        <a class="btn btn-md btn-secondary" href="<%= poUrl %>" target="_blank">Submit PO/ New Order</a>
    </div>
    <div class="row">
        <%= errMsg %>
    </div>
    <span class="space40"></span>

    <h2 class="pgTitle"><% = editorder %></h2>

    <div id="viewOrder" runat="server">
        <span class="space15"></span>
        <div>
            <asp:GridView ID="gvEditOrder" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
                AutoGenerateColumns="false" OnRowDataBound="gvEditOrder_RowDataBound" Width="100%">
                <RowStyle CssClass="" />
                <HeaderStyle CssClass="bg-dark" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="OrderID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OrderStatus">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FK_OrderCustomerID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ordDate" HeaderText="Date">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OrderID" HeaderText="Order ID">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CustomerName" HeaderText="Name">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CustomerMobile" HeaderText="Mobile No.">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FlLastDate" HeaderText="Last Followup Date">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ProductName" HeaderText="Items Inside">
                        <ItemStyle Width="25%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OrdAmount" HeaderText="Total Amount">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DateDiff" HeaderText="Days Passed">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemStyle Width="10%" />
                        <ItemTemplate>
                            <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemStyle Width="10%" />
                        <ItemTemplate>
                            <asp:Button ID="cmdEditOrd" runat="server" CssClass="btn btn-sm btn-primary" Text="Edit & Repeat Order" OnClick="EditOrder_Click" />
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

    <script type="text/javascript">
        $(document).ready(function () {
            $("a.fancylink").fancybox({
                type: 'iframe'
            });
        });

    </script>
</asp:Content>

