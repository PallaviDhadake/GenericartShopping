<%@ Page Title="Move State/City/District" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="move-state-city.aspx.cs" Inherits="admingenshopping_move_state_city" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="page-content">
        <asp:Button ID="btnMoveState" CssClass="btn btn-lg btn-primary" runat="server" Text="Move States" OnClick="btnMoveState_Click" />
            
        <br /><br />

        <asp:Button ID="btnMoveDist" CssClass="btn btn-lg btn-primary" runat="server" Text="Move Districts" OnClick="btnMoveDist_Click" />

         <br /><br />

        <asp:Button ID="btnMoveCity" CssClass="btn btn-lg btn-primary" runat="server" Text="Move City" OnClick="btnMoveCity_Click" />

        <br />
        <%= errMsg %>
    </div>
</asp:Content>

