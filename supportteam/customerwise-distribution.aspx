<%@ Page Title="" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="customerwise-distribution.aspx.cs" Inherits="supportteam_customerwise_distribution" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="//cdn.datatables.net/plug-ins/1.10.11/sorting/date-eu.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            $('[id$=gvOrdFlup]').DataTable({
                columnDefs: [
                    { "targets": 8, "type": "date-eu" },
                    { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9] }
                ],
                order: [[8, 'desc']]
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">Followup Report</h2>
    <span class="space15"></span>

    <div id="viewOrder" runat="server">
        <asp:GridView ID="gvOrdFlup" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
            AutoGenerateColumns="false" Width="100%" OnRowDataBound="gvOrdFlup_RowDataBound">
            <RowStyle CssClass="" />
            <HeaderStyle CssClass="bg-dark" />
            <AlternatingRowStyle CssClass="alt" />
            <Columns>
                <asp:BoundField DataField="FlpAsnId">
                    <HeaderStyle CssClass="HideCol" />
                    <ItemStyle CssClass="HideCol" />
                </asp:BoundField>
                <asp:BoundField DataField="Fk_CustomerID">
                    <HeaderStyle CssClass="HideCol" />
                    <ItemStyle CssClass="HideCol" />
                </asp:BoundField>
                <asp:BoundField DataField="FK_OrderId">
                    <HeaderStyle CssClass="HideCol" />
                    <ItemStyle CssClass="HideCol" />
                </asp:BoundField>
                <asp:BoundField DataField="CustomerName" HeaderText="Customer Name">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="CustomerMobile" HeaderText="Customer Mobile">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="CallGoodTime" HeaderText="Time To Call">
                    <ItemStyle Width="5%" />
                </asp:BoundField>
                <asp:BoundField DataField="OrderCount" HeaderText="Order Count">
                    <ItemStyle Width="5%" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="View">
                    <ItemStyle Width="10%" />
                    <ItemTemplate>
                        <asp:Literal ID="litFlUp" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Followup Status">
                    <ItemStyle Width="15%" />
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlFlupStatus" runat="server" CssClass="form-control" Width="100%">
                            <asp:ListItem Value="1">Pending</asp:ListItem>
                            <asp:ListItem Value="2">Completed</asp:ListItem>
                            <asp:ListItem Value="3">Postpone</asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle Width="10%" />
                    <ItemTemplate>
                        <asp:Button ID="btnUpdate" runat="server" Text="Update Status" OnClick="btnUpdate_Click" CssClass="btn btn-sm btn-info" />            
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <span class="warning">No data to Display :(</span>
            </EmptyDataTemplate>
            <PagerStyle CssClass="gvPager" />
        </asp:GridView>
    </div>
</asp:Content>

