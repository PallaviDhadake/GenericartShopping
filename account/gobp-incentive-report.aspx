<%@ Page Title="" Language="C#" MasterPageFile="~/account/MasterAccount.master" AutoEventWireup="true" CodeFile="gobp-incentive-report.aspx.cs" Inherits="account_gobp_incentive_report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<script src="//cdn.datatables.net/plug-ins/1.10.11/sorting/date-eu.js" type="text/javascript"></script>--%>
    <script>
        $(document).ready(function () {

            $('[id$=gvGOBPIncentive]').DataTable({
                columnDefs: [
                    { orderable: false, targets: [0, 1, 2, 3, 4, 5] }
                ],
                order: [[0, 'desc']]
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">GOBP Incentive Report</h2>
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
            <asp:GridView ID="gvGOBPIncentive" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
                AutoGenerateColumns="false" Width="100%" OnRowDataBound="gvGOBPIncentive_RowDataBound" ShowFooter="true">
                <RowStyle CssClass="" />
                <HeaderStyle CssClass="bg-dark" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="OBP_ID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OBP_ApplicantName" HeaderText="Name">
                        <ItemStyle Width="30%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OBP_UserID" HeaderText="OBP User ID">
                        <ItemStyle Width="20%" />
                    </asp:BoundField>                   
                    <asp:BoundField DataField="custCount" HeaderText="Cust. Count">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TotalOrder" HeaderText="Order Count">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                     <asp:BoundField DataField="OrderTotalAmount" HeaderText="Order Amt (Inc. Tax)">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OrderAmount" HeaderText="Incentive Amount">
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

