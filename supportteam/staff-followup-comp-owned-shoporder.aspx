<%@ Page Title="Company Owned Shop Follow-up | Genericart Shopping Support Team " Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="staff-followup-comp-owned-shoporder.aspx.cs" Inherits="supportteam_staff_followup_comp_owned_shoporder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
        $(document).ready(function () {
            $('[id$=gvCompOwnShop]').DataTable({
                columnDefs: [
                    { orderable: false, targets: [0, 1, 2, 3] }
                ],
                order: [[3, 'desc']]
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Company Owned Shops</h2>
    <span class="space15"></span>

     <div id="viewCompOwnShop" runat="server">
   
        <span class="space15"></span>
        <%--gridview start--%>
        <asp:GridView ID="gvCompOwnShop" runat="server" AutoGenerateColumns="False"
            CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None" Width="100%" OnRowDataBound="gvCompOwnShop_RowDataBound">
            
            <HeaderStyle CssClass="bg-dark" />
            <AlternatingRowStyle CssClass="alt" />
            <Columns>
                <asp:BoundField DataField="CSID">
                    <HeaderStyle CssClass="HideCol" />
                    <ItemStyle CssClass="HideCol" />
                </asp:BoundField>
                 <asp:BoundField DataField="FK_FranchID">
                    <HeaderStyle CssClass="HideCol" />
                    <ItemStyle CssClass="HideCol" />
                </asp:BoundField>
               
                 <asp:BoundField DataField="FranchShopCode" HeaderText="Shop Code">
                    <ItemStyle Width="5%" />
                </asp:BoundField>
                  <asp:TemplateField HeaderText="Orders">
                    <ItemStyle Width="5%" />
                    <ItemTemplate>
                        <asp:Literal ID="litOrders" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
             
               
            </Columns>
            <EmptyDataTemplate>
                <span class="warning">No Shops to Display :(</span>
            </EmptyDataTemplate>
            <PagerStyle CssClass="gvPager" />
        </asp:GridView>
        <%-- gridview End--%>
    </div>

</asp:Content>

