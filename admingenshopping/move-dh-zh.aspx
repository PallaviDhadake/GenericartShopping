<%@ Page Title="Move DH / ZH" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="move-dh-zh.aspx.cs" Inherits="admingenshopping_move_dh_zh" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="page-content">
        <asp:Button ID="btnMoveDh" CssClass="btn btn-lg btn-primary" runat="server" Text="Move DH" OnClick="btnMoveDh_Click"  />
            
        <br /><br />

        <asp:Button ID="btnMoveZh" CssClass="btn btn-lg btn-primary" runat="server" Text="Move ZH" OnClick="btnMoveZh_Click"  />

        <br /><br />

        <asp:Button ID="btnZhDist" CssClass="btn btn-lg btn-primary" runat="server" Text="Move ZH Districts" OnClick="btnZhDist_Click"  />

        <br /><br />

        <asp:Button ID="btnMgmt" CssClass="btn btn-lg btn-primary" runat="server" Text="Move Management Team" OnClick="btnMgmt_Click"  />

        <br />
        <%= errMsg %>
    </div>
</asp:Content>

