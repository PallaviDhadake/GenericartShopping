<%@ Page Title="Saving Calculator List" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="saving-calculator-list.aspx.cs" Inherits="admingenshopping_saving_calculator_list" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
        $(document).ready(function () {
            $('[id$=gvCalc]').DataTable({
                columnDefs: [
                     { orderable: false, targets: [0, 2, 3, 5, 7, 8] }
                ],
                order: [[0, 'desc']]
            });
        });
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle" >Saving Calculator Data</h2>
    <span class="space20"></span>

    <asp:GridView ID="gvCalc" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
        AutoGenerateColumns="false" Width="100%">
        <RowStyle CssClass="" />
        <HeaderStyle CssClass="bg-dark" />
        <AlternatingRowStyle CssClass="alt" />
        <Columns>
            <asp:BoundField DataField="CalcID">
                <HeaderStyle CssClass="HideCol" />
                <ItemStyle CssClass="HideCol" />
            </asp:BoundField>
            <asp:BoundField DataField="cDate" HeaderText="Date">
                <ItemStyle Width="5%" />
            </asp:BoundField>
            <asp:BoundField DataField="MobileNumber" HeaderText="Mobile No.">
                <ItemStyle Width="5%"/>
            </asp:BoundField>
            <asp:BoundField DataField="BrandMedicine" HeaderText="Medicine Name">
                <ItemStyle Width="10%"/>
            </asp:BoundField>
            <asp:BoundField DataField="BrandPrice" HeaderText="Brand Price">
                <ItemStyle Width="8%"/>
            </asp:BoundField>
            <asp:BoundField DataField="GenericCode" HeaderText="Content">
                <ItemStyle Width="10%"/>
            </asp:BoundField>
            <asp:BoundField DataField="GenericPrice" HeaderText="Generic Price">
                <ItemStyle Width="10%"/>
            </asp:BoundField>
            <asp:BoundField DataField="SavingAmount" HeaderText="Total Saving">
                <ItemStyle Width="10%"/>
            </asp:BoundField>
            <asp:BoundField DataField="netSaving" HeaderText="Total Net Saving">
                <ItemStyle Width="10%"/>
            </asp:BoundField>
        </Columns>
        <EmptyDataTemplate>
            <span class="warning">No Orders to Display :(</span>
        </EmptyDataTemplate>
        <PagerStyle CssClass="gvPager" />
    </asp:GridView>


    <%= errMsg %>
</asp:Content>

