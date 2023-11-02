<%@ Page Title="Staff Training Videos | Genericart Shopping Admin Panel" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="staff-training-videos.aspx.cs" Inherits="supportteam_staff_training_videos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
      <style>
        .bgVidOrange{background:#FF8C00;}
        .bgVidDarkGrey{background:#686868;}
        .bgVidGreen{background:#08bf08;}
        .clrWhite{color:#FFFFFF;}
        /*.videoRow{display:table; padding-top:10px; padding-bottom:10px;}*/
    </style>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <span class="medium dspBlk mrg_B_10">Select Video Language :</span>
    <asp:DropDownList ID="ddrLang" runat="server" CssClass="txtBox" Width="400" AutoPostBack="true" OnSelectedIndexChanged="ddrLang_SelectedIndexChanged">
        <asp:ListItem Value="0"><- Select -></asp:ListItem>
        <asp:ListItem Value="1">English</asp:ListItem>
        <asp:ListItem Value="2">Marathi</asp:ListItem>
        <asp:ListItem Value="3">Hindi</asp:ListItem>
        <asp:ListItem Value="4">Kannada</asp:ListItem>
    </asp:DropDownList>
    <span class="space40"></span>

    <div class="videoRow">
        <%= videoStr %>
    </div> 
</asp:Content>

