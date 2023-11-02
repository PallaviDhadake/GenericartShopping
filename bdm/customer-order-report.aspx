<%@ Page Title="" Language="C#" MasterPageFile="~/bdm/MasterBdm.master" AutoEventWireup="true" CodeFile="customer-order-report.aspx.cs" Inherits="bdm_customer_order_report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="//cdn.datatables.net/plug-ins/1.10.11/sorting/date-eu.js" type="text/javascript"></script>
   <%-- <script>
        $(document).ready(function () {
            $('[id$=gvOrder]').DataTable({
                columnDefs: [
                    { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12] }
                ],
                order: [[0, 'desc']]
            });
        });
    </script>--%>
    <script type="text/javascript">
        $(function () {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        createDataTable();
                    }
                });
            };

            createDataTable();
            function createDataTable() {
                $('#<%= gvOrder.ClientID %>').prepend($("<thead></thead>").append($('#<%= gvOrder.ClientID %>').find("tr:first"))).DataTable({
                    columnDefs: [
                        { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12] }
                    ],
                    order: [[0, 'desc']]
                });

            }
        });
    </script>

    <script type="text/javascript">

        window.onload = function () {
            //alert("window load");

            duDatepicker('#<%= txtFromDate.ClientID %>', {
                auto: true, inline: true, format: 'dd/mm/yyyy',
            });

            duDatepicker('#<%= txtToDate.ClientID %>', {
                auto: true, inline: true, format: 'dd/mm/yyyy',
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">Order Report Master</h2>
    <span class="space15"></span>

    <h2 style="font-size: 20px; color: lightseagreen">Data Range :
        <asp:Literal ID="litDate" runat="server"></asp:Literal></h2>

    <span class="space15"></span>
    <div id="viewOrder" runat="server">
        <span class="space15"></span>
        <div class="row">
            <div class="form-group col-sm-4">
                <span class="text-sm text-bold mrgBtm10 dspBlk">From Date :</span>
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
            </div>
            <div class="form-group col-sm-4">
                <span class="text-sm text-bold mrgBtm10 dspBlk">To Date :</span>
                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calender"></asp:TextBox>
            </div>
            <div class="col-sm-1">
                <span class="space30"></span>
                <asp:Button ID="btnShow" runat="server" CssClass="btn btn-md btn-primary" Text="Show List" OnClick="btnShow_Click" />
            </div>
        </div>

        <span class="space15"></span>
        <asp:GridView ID="gvOrder" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
            AutoGenerateColumns="false" OnRowDataBound="gvOrder_RowDataBound" Width="100%">
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
                <asp:BoundField DataField="OrderID" HeaderText="Order ID">
                    <ItemStyle Width="5%" />
                </asp:BoundField>
                <asp:BoundField DataField="ordDate" HeaderText="Date">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="CustomerName" HeaderText="Customer Name">
                    <ItemStyle Width="15%" />
                </asp:BoundField>
                <asp:BoundField DataField="CustomerMobile" HeaderText="Customer Mobile">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="OrderAmount" HeaderText="Order Amount">
                    <ItemStyle Width="5%" />
                </asp:BoundField>
                <asp:BoundField DataField="ProductCount" HeaderText="Product Count">
                    <ItemStyle Width="12%" />
                </asp:BoundField>
                <asp:BoundField DataField="CartProducts" HeaderText="Cart Products">
                    <ItemStyle Width="20%" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Status">
                    <ItemStyle Width="10%" />
                    <ItemTemplate>
                        <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="DeviceType" HeaderText="DeviceType">
                    <ItemStyle Width="5%" />
                </asp:BoundField>
                <asp:TemplateField>
                    <ItemStyle Width="5%" />
                    <ItemTemplate>
                        <asp:Literal ID="litAnch" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <span class="warning">No Records to Display :(</span>
            </EmptyDataTemplate>
            <PagerStyle CssClass="gvPager" />
        </asp:GridView>
    </div>
</asp:Content>

