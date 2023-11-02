<%@ Page Title="" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="reject-order-report.aspx.cs" Inherits="supportteam_reject_order_report" %>

<%@ MasterType VirtualPath="~/supportteam/MasterSupport.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        $(document).ready(function () {

            $('[id$=gvOrdFlup]').DataTable({
                columnDefs: [
                    { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13] }
                ],
                order: [[0, 'DESC']]
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">Reject Order Report</h2>
    <span class="space15"></span>

    <h2 style="font-size: 20px;color: lightseagreen">Data Range : <asp:Literal ID="litDate" runat="server"></asp:Literal></h2>

	<span class="space15"></span>
    <div id="viewOrder" runat="server">
        <div>
            <asp:GridView ID="gvOrdFlup" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
                AutoGenerateColumns="false" Width="100%" OnRowDataBound="gvOrdFlup_RowDataBound">
                <RowStyle CssClass="" />
                <HeaderStyle CssClass="bg-dark" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="FK_OrderCustomerID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OrderID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OrderStatus">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="transactionStatus">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CustomerName" HeaderText="Customer Name">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CustomerMobile" HeaderText="Mobile No">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ordInfo" HeaderText="Order Info">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="flCount" HeaderText="Followups">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="flLastDate" HeaderText="Last - Next Followup">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="transactionStatus">
                        <ItemStyle Width="10%" />
                        <ItemTemplate>
                            <asp:Literal ID="litTranStatus" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField DataField="transactionStatus" HeaderText="Transaction Status">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>--%>
                    <asp:BoundField DataField="FollowupNextDate">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField> 
                    <%--<asp:BoundField DataField="DateDiff" HeaderText="Days Passed">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>--%>
                    <%--<asp:BoundField DataField="flBy" HeaderText="Followup By">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>--%>
                    <asp:BoundField DataField="DeviceType" HeaderText="Device Type">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemStyle Width="10%" />
                        <ItemTemplate>
                            <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Followup">
                        <ItemStyle Width="10%" />
                        <ItemTemplate>
                            <asp:Literal ID="litFlUp" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <span class="warning">No data to Display :(</span>
                </EmptyDataTemplate>
                <PagerStyle CssClass="gvPager" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>
