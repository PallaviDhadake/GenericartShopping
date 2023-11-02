<%@ Page Title="Most Demamding Products List" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="most-demanding-products-list.aspx.cs" Inherits="admingenshopping_most_demanding_products_list" %>
<%@ MasterType VirtualPath="~/admingenshopping/MasterAdmin.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
       $(document).ready(function () {
           $('[id$=gvProducts]').DataTable({
               columnDefs: [
                    { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7, 8] }
               ],
               order: [[7, 'asc']]
           });
       });
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Most Demanding Products List</h2>
    <span class="space15"></span>

    <div id="viewProduct" runat="server">
        <div class="formPanel table-responsive-md">
            <asp:GridView ID="gvProducts"  runat="server" CssClass="table table-striped table-bordered table-hover" GridLines="None" 
                AutoGenerateColumns="false" OnRowDataBound="gvProducts_RowDataBound" OnRowCommand="gvProducts_RowCommand"   >
                <HeaderStyle CssClass="thead-dark" />
                <RowStyle CssClass="" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="ProductID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle  CssClass="HideCol"/>
                    </asp:BoundField>
                    <asp:BoundField DataField="ProductPhoto">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle  CssClass="HideCol"/>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Product Image">
                        <ItemStyle Width="5%" />
                        <ItemTemplate>
                            <asp:Literal ID="litProduct" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField> 
                    <asp:BoundField DataField="ProductName" HeaderText="Product Name">
                        <ItemStyle Width="25%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ProductSKU" HeaderText="Code">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ProductCatName" HeaderText="Category">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="MfgName" HeaderText="Brand Name">
                        <ItemStyle Width="12%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ProductDisplayOrder" HeaderText="Display Order">
                        <ItemStyle Width="15%"/>
                    </asp:BoundField>
                   <asp:TemplateField>
                        <ItemStyle Width="10%" />
                        <ItemTemplate>
                            <asp:Button ID="moveUp" runat="server" CssClass="gMoveUp" CommandName="Up" />
                            <asp:Button ID="moveDown" runat="server" CssClass="gMoveDown" CommandName="Down"  />
                        </ItemTemplate>
                    </asp:TemplateField>                
                </Columns>
                <EmptyDataTemplate>
                    <span class="warning">Its Empty Here... :(</span>
                </EmptyDataTemplate>
                <PagerStyle CssClass="" />
            </asp:GridView>            
        </div>

        <asp:Button ID="btnSetOrder" runat="server" Text="Set Display Order" CssClass="btn btn-sm btn-primary" OnClick="btnSetOrder_Click" />
    </div>
</asp:Content>

