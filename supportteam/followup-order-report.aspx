<%@ Page Title="Orders Followup Report" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="followup-order-report.aspx.cs" Inherits="supportteam_followup_order_report" %>

<%@ MasterType VirtualPath="~/supportteam/MasterSupport.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="//cdn.datatables.net/plug-ins/1.10.11/sorting/date-eu.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            $('[id$=gvOrdFlup]').DataTable({
                columnDefs: [
                    { "targets": 8, "type": "date-eu" },
					{ orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 9, 10, 11, 12, 13, 14] }
                ],
                order: [[8, 'desc']]
            });
        });
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">Followup Report</h2>
    <span class="space15"></span>

    <div class="row" id="dateRange" runat="server" visible="false">
        <div class="col-md-3">
            <asp:DropDownList ID="ddrMonth" runat="server" CssClass="form-control">
                <asp:ListItem><-Select-></asp:ListItem>
            </asp:DropDownList>
        </div>
        <%--<div class="col-md-3">
            <asp:TextBox ID="txtYear" runat="server" CssClass="form-control"></asp:TextBox>
        </div>--%>
        <div class="col-md-3">
            <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="btn btn-md btn-primary" OnClick="btnRefresh_Click" />
        </div>
    </div>
    <span class="space30"></span>

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
                    <asp:BoundField DataField="CustomerName" HeaderText="Customer Name">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CustomerMobile" HeaderText="Mobile No">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ordInfo" HeaderText="Order Info">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="SaleBillNo" HeaderText="Sale Bill No.">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="deliverydate" HeaderText="Dispatch - Delivery Date">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="flCount" HeaderText="Followups">
                        <ItemStyle Width="3%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="flLastDate" HeaderText="Last - Next Followup">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FollowupNextDate">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField> 
                    <asp:BoundField DataField="DateDiff" HeaderText="Days Passed">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="flBy" HeaderText="Followup By">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DeviceType" HeaderText="Device Type">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
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

