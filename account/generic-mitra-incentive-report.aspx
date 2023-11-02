<%@ Page Title="" Language="C#" MasterPageFile="~/account/MasterAccount.master" AutoEventWireup="true" CodeFile="generic-mitra-incentive-report.aspx.cs" Inherits="account_generic_mitra_incentive_report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<script src="//cdn.datatables.net/plug-ins/1.10.11/sorting/date-eu.js" type="text/javascript"></script>--%>
    <script>
        $(document).ready(function () {

            $('[id$=gvGenMitra]').DataTable({
                columnDefs: [
                    { orderable: false, targets: [0, 1, 2, 3, 4, 6, 7, 8, 9, 10] }
                ],
                order: [[0, 'desc']]
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">Generic Mitra Incentive Report</h2>
    <span class="space15"></span>

    <div class="row" id="daterange" runat="server">
        <div class="col-md-3">
            <asp:DropDownList ID="ddrMonth" runat="server" CssClass="form-control">
                <asp:ListItem Value="0"> -- Select Month -- </asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="col-md-3">
            <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="btn btn-md btn-primary" OnClick="btnShow_Click" />
        </div>
        <div class="col-md-3">
            <asp:Button ID="btnExportOrders" runat="server" CssClass="btn btn-md btn-info" Text="Export To Excel" OnClick="btnExportOrders_Click" />
        </div>
    </div>
    <span class="space30"></span>

    <div class="viewOrder" runat="server">
        <div>
            <asp:GridView ID="gvGenMitra" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
                AutoGenerateColumns="false" Width="100%" OnRowDataBound="gvGenMitra_RowDataBound" ShowFooter="true">
                <RowStyle CssClass="" />
                <HeaderStyle CssClass="bg-dark" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="GMitraID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="rDate" HeaderText="Reg. Date">
                        <ItemStyle Width="8%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="GMitraName" HeaderText="Name">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="GMitraMobile" HeaderText="Mobile">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="GMitraShopCode" HeaderText="Shop Code">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="franchiseeName" HeaderText="Franchisee Name">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="custCount" HeaderText="No. Of Customers">
                        <ItemStyle Width="8%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TotalOrder" HeaderText="Total Orders">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ordValue" HeaderText="Total Order Value">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Commission" HeaderText="20% Commission">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="View">
                        <ItemStyle Width="5%" />
                        <ItemTemplate>
                            <asp:Literal ID="litAnch" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>                
            </asp:GridView>
        </div>
    </div>
</asp:Content>

