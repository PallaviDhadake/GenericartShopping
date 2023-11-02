<%@ Page Language="C#" MasterPageFile="~/Member/MemberMain.master" AutoEventWireup="true" CodeFile="~/Member/PurchaseList.aspx.cs" Inherits="PurchaseList" Title="All Sales List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        function showModal() {
            $("#myModal").modal('show');
        }

        $(function () {
            $("#btnShow").click(function () {
                showModal();
            });
        });
    </script>

    <style>
        @media print {
            .noprint {
                display: none;
            }
        }
    </style>

    <script language="javascript" type="text/javascript">
        function printDiv(divID) {
            //Get the HTML of div
            var divElements = document.getElementById(divID).innerHTML;
            //Get the HTML of whole page
            var oldPage = document.body.innerHTML;

            //Reset the page's HTML with div's HTML only
            document.body.innerHTML =
                "<html><head><title></title></head><body>" +
                divElements + "</body>";

            //Print Page
            window.print();

            //Restore orignal HTML
            document.body.innerHTML = oldPage;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="breadcrumbs">
        <div class="col-sm-4">
            <div class="page-header float-left">
                <div class="page-title">
                    <h1>Purchase List</h1>
                </div>
            </div>
        </div>
        <div class="col-sm-8">
            <div class="page-header float-right">
                <div class="page-title">
                    <ol class="breadcrumb text-right">
                        <li class="active">Purchase List</li>
                    </ol>
                </div>
            </div>
        </div>
    </div>
    <div class="content mt-3">
        <div class="col-md-12 col-xs-12" style="margin-bottom: 25px;">
            <div id="printdivMain">
                <div class="col-md-3">
                    <label for="exampleInputEmail1">Date From</label>
                    <asp:TextBox ID="TxtDateFrom" CssClass="form-control date-picker" data-date-format="dd-mm-yyyy" runat="server" autocomplete="off"></asp:TextBox>
                    <script>
                        var picker = new Pikaday(
                            {
                                field: document.getElementById('<%= TxtDateFrom.ClientID %>'),
                                firstDay: 1,
                                minDate: new Date('01-01-1950'),
                                maxDate: new Date('30-12-2020'),
                                yearRange: [2000, 2020]
                            });
                    </script>
                </div>
                <div class="col-md-3">
                    <label for="exampleInputEmail1">Date To</label>
                    <asp:TextBox ID="TxtDateTo" CssClass="form-control date-picker" data-date-format="dd-mm-yyyy" runat="server" autocomplete="off"></asp:TextBox>
                    <script>
                        var picker = new Pikaday(
                            {
                                field: document.getElementById('<%= TxtDateTo.ClientID %>'),
                                firstDay: 1,
                                minDate: new Date('01-01-1950'),
                                maxDate: new Date('30-12-2020'),
                                yearRange: [2000, 2020]
                            });
                    </script>
                </div>
                <div class="col-md-3">
                    <label for="exampleInputEmail1">&nbsp;</label><br />
                    <asp:Button ID="BtnSearch" runat="server" OnClick="BtnSearch_Click" Text="Search" CssClass="btn btn-primary" />
                </div>
                <div style="clear: both; height: 15px;"></div>
                <div class="col-md-12 col-xs-12 table-responsive">
                    <table id="dynamic-table" class="table table-striped table-bordered table-hover">
                        <thead>
                            <tr>
                                <th>SI.No</th>
                                <th>Receive Date</th>
                               <%-- <th>Seller Id</th>
                                <th>Seller Name</th>--%>
                                <th>Bill no</th>
                                <th>Bill Date</th>
                                <%--<th class="text-right">BV</th>
                                <th class="text-right">Bill Amount</th>--%>
                                <th class="text-right">Net Amount</th>
                                <th class="text-center">Status</th>
                                <%--<th>View</th>--%>
                                <%-- <th>Customer Wise Print</th>--%>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("row")%></td>
                                        <td><%# Eval("ReceiveDate") %></td>
                                      <%--  <td><%# Eval("Id") %></td>
                                        <td><%# Eval("EmployeeName") %></td>--%>
                                        <td><%# Eval("billno") %></td>
                                        <td><%# Eval("billdate") %></td>
                                       <%-- <td class="text-right"><%# Eval("businessvalue") %></td>
                                        <td class="text-right"><%# Eval("billamount") %></td>--%>
                                        <td class="text-right"><%# Eval("netamount") %></td>
                                        <td class="text-center"><%# Eval("StatusShow") %></td>
                                        <%--<td>
                                            <asp:LinkButton ID="LinkEdit" runat="server" CommandName="View" CommandArgument='<%#Eval("Auto") %>' Text="View">View</asp:LinkButton></td>--%>
                                        <%-- <td>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandName="CustomerWisePrint" CommandArgument='<%#Eval("Auto") %>' Text="View">Customer Wise Print</asp:LinkButton></td>--%>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <tr>
                                        <td style="font-weight: bold;">Total</td>
                                        <td></td>
                                        <%--<td></td>
                                        <td></td>--%>
                                        <td></td>
                                        <td></td>
                                       <%-- <td style="text-align: right; font-weight: bold;">
                                            <asp:Label ID="LblTotalBV" runat="server" />
                                        </td>
                                        <td></td>--%>
                                        <td style="text-align: right; font-weight: bold;">
                                            <asp:Label ID="lblTotal" runat="server" />
                                        </td>
                                        <td></td>
                                       <%-- <td></td>--%>
                                    </tr>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>

            </div>
            <%--<div align="center">
                        <a href="#" id="hrefPrintMain">Print</a>
                    </div>--%>
        </div>
    </div>
</asp:Content>
