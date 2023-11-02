<%@ Page Title="District Heads" Language="C#" MasterPageFile="~/orgmember/MasterOrgmember.master" AutoEventWireup="true" CodeFile="dh-master.aspx.cs" Inherits="orgmember_dh_master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
        $(document).ready(function () {

            $('[id$=gvDisHead]').DataTable({
                columnDefs: [
                     { orderable: false, targets: [0, 1, 2, 3, 4, 5] }
                ],
                order: [[0, 'desc']]
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">District Heads</h2>
    <span class="space15"></span>

    <asp:GridView ID="gvDisHead" runat="server" AutoGenerateColumns="False"
        CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None" OnRowDataBound="gvDisHead_RowDataBound">
        <HeaderStyle CssClass="bg-dark" />
        <AlternatingRowStyle CssClass="alt" />
        <Columns>
            <asp:BoundField DataField="DistHdId">
                <HeaderStyle CssClass="HideCol" />
                <ItemStyle CssClass="HideCol" />
            </asp:BoundField>
            <asp:BoundField DataField="DistHdName" HeaderText="Name">
                <ItemStyle Width="25%" />
            </asp:BoundField>
            <asp:BoundField DataField="DistHdCityName" HeaderText="City">
                <ItemStyle Width="15%" />
            </asp:BoundField>
            <asp:BoundField DataField="DistHdMobileNo" HeaderText="Mobile No">
                <ItemStyle Width="10%" />
            </asp:BoundField>
             <asp:BoundField DataField="ObpCount" HeaderText="Total GOBP">
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
            <span class="warning">No District Heads to Display</span>
        </EmptyDataTemplate>
        <PagerStyle CssClass="gvPager" />
    </asp:GridView>
</asp:Content>

