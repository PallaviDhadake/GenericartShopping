<%@ Page Title="Add Country | Genericart Shopping Addmin Control Panel" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="addcountry.aspx.cs" Inherits="admingenshopping_addcountry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
        $(document).ready(function () {
            $('[id$=gvCountry]').DataTable();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <h2 class="pgTitle">Country Master</h2>
    <span class="space15"></span>
   <%-- <%--New/Edit Form Start--%>
    <div id="editCountry" runat="server">
        <div class="card card-primary">
            <div class="card-header">
                <h3 class="card-title"><%=pgTitle %></h3>
            </div>
            <div class="card-body">
                <div class="col-sm-7">
                    <div class="colorLightBlue">
                        <span>Id :</span>
                        <asp:Label ID="lblId" runat="server" Text="[New]"></asp:Label>
                    </div>
                    <span class="space15"></span>
                    <div class="form-group">
                        <label for="cName">Country Name:*</label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control " MaxLength="50" Width="80%"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <span class="space15"></span>
        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-sm btn-primary" Text="Submit" OnClick="btnSave_Click" />
        <asp:Button ID="btnDelete" runat="server"
            CssClass="btn btn-sm btn-outline-info" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this?');" OnClick="btnDelete_Click" />
        <asp:Button ID="btnCancel" runat="server"
            CssClass="btn btn-sm btn-outline-dark" Text="Cancel" OnClick="btnCancel_Click" />
        <div class="float_clear"></div>
    </div>
    <%--New/Edit Form End--%>




     <%--Testimonials GridView Start--%>
    <div id="viewCountry" runat="server">
        <a href="addcountry.aspx?action=new" class="btn btn-primary btn-sm">Add New</a>
        <span class="space25"></span>
        <asp:GridView ID="gvCountry" runat="server" AutoGenerateColumns="False"
            CssClass="table table-striped table-bordered table-hover" GridLines="None" OnRowDataBound="gvCountry_RowDataBound" Width="100%">
            <RowStyle CssClass="" />
            <HeaderStyle CssClass="bg-dark" />
            <AlternatingRowStyle CssClass="alt" />
            <Columns>
                <asp:BoundField DataField="CountryID"><HeaderStyle CssClass="HideCol" /><ItemStyle CssClass="HideCol" /></asp:BoundField>
                <asp:BoundField DataField="sn" HeaderText="SI. NO."><ItemStyle Width="10%" /></asp:BoundField>
                <asp:BoundField DataField="CountryName" HeaderText="Country"><ItemStyle Width="20%" /></asp:BoundField>
                <asp:TemplateField>
                    <ItemStyle Width="20%" />
                    <ItemTemplate>
                        <asp:Literal ID="litAnch" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <span class="warning">No Country to Display :(</span>
            </EmptyDataTemplate>
            <PagerStyle CssClass="gvPager" />
        </asp:GridView>
    </div>
    <%--Testimonials GridView Start--%>
</asp:Content>

