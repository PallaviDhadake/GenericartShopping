<%@ Page Title="" Language="C#" MasterPageFile="~/GOBPDH/MasterGOBPDH.master" AutoEventWireup="true" CodeFile="gobp-orders.aspx.cs" Inherits="GOBPDH_gobp_orders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
        $(document).ready(function () {

            $('[id$=gvGOBP]').DataTable({
                columnDefs: [
                     { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7] }
                ],
                order: [[0, 'desc']]
            });
        });
    </script>
    <style>
        #followup .user-block .username, #followup .user-block .description {
            margin: 0px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">GOBP Order List</h2>
    <span class="space15"></span>

    <div id="viewFrEnquiry" runat="server">
        <div class="">
            <asp:GridView ID="gvGOBP" runat="server" AutoGenerateColumns="False"
                CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None">
                <HeaderStyle CssClass="bg-dark" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="GOBPID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <%--<asp:BoundField DataField="GOBPID" HeaderText="GOBPID">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>--%>
                    <asp:BoundField DataField="Name" HeaderText="Name">
                        <ItemStyle Width="20%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="MobileNo" HeaderText="Mobile No.">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Customers" HeaderText="Total Customers">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="MonthOrder" HeaderText="Current Month Orders">
                        <ItemStyle Width="20%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="MonthAmount" HeaderText="Current Month Amount">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TotalOrders" HeaderText="Total Orders">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <%--<asp:TemplateField>
                        <ItemStyle Width="5%" />
                        <ItemTemplate>
                            <asp:Literal ID="litView" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    
                </Columns>
                <EmptyDataTemplate>
                    <span class="warning">No GOBP Enquries to Display</span>
                </EmptyDataTemplate>
                <PagerStyle CssClass="gvPager" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>

