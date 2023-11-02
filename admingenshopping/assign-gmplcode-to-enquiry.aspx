<%@ Page Title="" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="assign-gmplcode-to-enquiry.aspx.cs" Inherits="admingenshopping_assign_gmplcode_to_enquiry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <h2 class="pgTitle">Assign GMPL Code To Enquiry Items</h2>
    <span class="space15"></span>
    <asp:Button ID="btnAssign" runat="server" Text="Assign Code" CssClass="btn btn-md btn-primary" OnClick="btnAssign_Click" />
                
    <span class="space40"></span>
    <%= errMsg %>
    
</asp:Content>

