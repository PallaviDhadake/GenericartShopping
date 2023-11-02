<%@ Page Title="Migrate Franchisee" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="migrate-franchisee.aspx.cs" Inherits="admingenshopping_migrate_franchisee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <div class="page-content">
        <asp:Button ID="BtnSubmit" CssClass="btn btn-lg btn-primary" runat="server" Text="Migrate Franchisee" OnClick="BtnSubmit_Click" />
            
        <br /><br />

        <asp:Button ID="btnUpdate" CssClass="btn btn-lg btn-primary" runat="server" Text="Update Latlongs" OnClick="btnUpdate_Click" />

        <br /><br />

        <asp:Button ID="btnUpdateHeads" CssClass="btn btn-lg btn-primary" runat="server" Text="Update Heads" OnClick="btnUpdateHeads_Click" />

        <br />
        <%= errMsg %>
    </div>
</asp:Content>

