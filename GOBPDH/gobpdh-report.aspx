<%@ Page Title="" Language="C#" MasterPageFile="~/GOBPDH/MasterGOBPDH.master" AutoEventWireup="true" CodeFile="gobpdh-report.aspx.cs" Inherits="GOBPDH_gobpdh_report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
    <h2 class="pgTitle">GOBP Order List</h2>
    <span class="space15"></span>

    <h2 style="font-size: 20px; color: lightseagreen">Data Range :
        <asp:Literal ID="litDate" runat="server"></asp:Literal></h2>

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

    <div id="viewFrEnquiry" runat="server">
        <div class="">
            <asp:GridView ID="gvGOBP" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvGOBP_RowDataBound" Width="100%"
                CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None">
                <HeaderStyle CssClass="bg-dark" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="GOBPID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="GOBPUser" HeaderText="GOBPID">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                     <asp:BoundField DataField="OBP_EmpId" HeaderText="Employee ID">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Name" HeaderText="Name">
                        <ItemStyle Width="20%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="MobileNo" HeaderText="Mobile No.">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Customers" HeaderText="Total Customers">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TotalOrders" HeaderText="Total Orders">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:TemplateField>
                        <ItemStyle Width="5%" />
                        <ItemTemplate>
                            <asp:Literal ID="litView" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
                <EmptyDataTemplate>
                    <span class="warning">No GOBP Enquries to Display</span>
                </EmptyDataTemplate>
                <PagerStyle CssClass="gvPager" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>
