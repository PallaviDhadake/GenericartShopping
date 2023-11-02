<%@ Page Title="" Language="C#" MasterPageFile="~/franchisee/MasterFranchisee.master" AutoEventWireup="true" CodeFile="customer-mailing-report.aspx.cs" Inherits="franchisee_customer_mailing_report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">

        window.onload = function () {
            //alert("window load");

            duDatepicker('#<%= txtDate.ClientID %>', {
                auto: true, inline: true, format: 'dd/mm/yyyy',
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Address Label Print</h2>

    <div id="viewCustomer" runat="server">
        <span class="space15"></span>

        <div class="row">
            <div class="form-group col-sm-4">
                <span class="text-sm text-bold mrgBtm10 dspBlk">Date :</span>
                <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
            </div>
            <div class="col-sm-1">
                <span class="space30"></span>
                <asp:Button ID="btnShow" runat="server" CssClass="btn btn-md btn-primary" Text="Show List" OnClick="btnShow_Click" />
            </div>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <div class="col-sm-1">
                <span class="space30"></span>
                <asp:Button ID="btnPrintAdd" runat="server" CssClass="btn btn-md btn-primary" Text="Print Label" OnClick="btnPrintAdd_Click" />
            </div>
        </div>

        <span class="space15"></span>
        <span class="space15"></span>
        <asp:GridView ID="gvCall" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
            AutoGenerateColumns="false" Width="100%">
            <RowStyle CssClass="" />
            <HeaderStyle CssClass="bg-dark" />
            <AlternatingRowStyle CssClass="alt" />
            <Columns>
                <asp:BoundField DataField="CustomrtID">
                    <HeaderStyle CssClass="HideCol" />
                    <ItemStyle CssClass="HideCol" />
                </asp:BoundField>
                <asp:BoundField DataField="OrderID" HeaderText="Order ID" />
                <asp:BoundField DataField="Name" HeaderText="Name" />
                <asp:BoundField DataField="MobileNo" HeaderText="Mobile No." />
                <asp:BoundField DataField="OrderAmount" HeaderText="Order Amount" />
                <asp:BoundField DataField="MedicineCount" HeaderText="MedicineCount" /> 
                <asp:TemplateField HeaderText="Print Preview">
                    <ItemTemplate>
                        <a id="BtnPrint" href='<%# Eval("OrderID", "generate-mailing.aspx?id={0}") %>' target="_blank" class="btn btn-primary btn-sm">Print</a>
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

