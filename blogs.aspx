<%@ Page Title="Blogs | Genericart Online Generic Medicine Shopping" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="blogs.aspx.cs" Inherits="blogs" %>
<%@ MasterType VirtualPath="~/MasterParent.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <span class="space40"></span>
    <div class="col_1140 bgWhite border_r_5">
        <div class="col_800">
            <div class="pad_30">
                <%= newsStr %>
            </div>
        </div>
        <div class="float_clear"></div>
    </div>
</asp:Content>

