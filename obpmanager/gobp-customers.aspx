<%@ Page Title="GOBP Custoers" Language="C#" MasterPageFile="~/obpmanager/MasterObpManager.master" AutoEventWireup="true" CodeFile="gobp-customers.aspx.cs" Inherits="obpmanager_gobp_customers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        $(document).ready(function () {

            $('[id$=gvGOBP]').DataTable({
                columnDefs: [
                     { orderable: false, targets: [0, 1, 2, 3] }
                ],
                order: [[0, 'desc']]
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">GOBP Customers</h2>
    <span class="space15"></span>

    <asp:GridView ID="gvGOBP" runat="server" AutoGenerateColumns="False"
        CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None">
        <HeaderStyle CssClass="bg-dark" />
        <AlternatingRowStyle CssClass="alt" />
        <Columns>
            <asp:BoundField DataField="CustomrtID">
                <HeaderStyle CssClass="HideCol" />
                <ItemStyle CssClass="HideCol" />
            </asp:BoundField>
            <asp:BoundField DataField="CustomerName" HeaderText="Name">
                <ItemStyle Width="25%" />
            </asp:BoundField>
            <asp:BoundField DataField="CustomerMobile" HeaderText="Mobile No">
                <ItemStyle Width="15%" />
            </asp:BoundField>
            <asp:BoundField DataField="custPurchase" HeaderText="Total Business">
                <ItemStyle Width="10%" />
            </asp:BoundField>

        </Columns>
        <EmptyDataTemplate>
            <span class="warning">No GOBP Customers to Display</span>
        </EmptyDataTemplate>
        <PagerStyle CssClass="gvPager" />
    </asp:GridView>
</asp:Content>

