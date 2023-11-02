<%@ Page Title="Upload Medicine Products" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="upload-products.aspx.cs" Inherits="admingenshopping_upload_products" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="breadcrumbs" id="breadcrumbs">
        <ul class="breadcrumb">
            <li>
                <i class="ace-icon fa fa-home home-icon"></i>
                <a href="#">Home</a>
            </li>
            <li class="active">Add Products</li>
        </ul>
    </div>
    <div class="page-content">
        <label>Select File</label><br />
            <asp:FileUpload ID="fuFile" runat="server" CssClass="" />
            <br />
        <br />
            <asp:Button ID="BtnSubmit" CssClass="btn btn-primary" runat="server" Text="Submit" OnClick="BtnSubmit_Click" />
        <br />
        <%= errMsg %>
    </div>
</asp:Content>

