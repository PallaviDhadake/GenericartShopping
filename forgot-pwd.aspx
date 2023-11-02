<%@ Page Title="Password Recovery | Genericart Online Generic Medicine Shopping" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="forgot-pwd.aspx.cs" Inherits="forgot_pwd" %>
<%@ MasterType VirtualPath="~/MasterParent.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <span class="space40"></span>
    <div class="col_1140">
        <div class="loginForm margin_auto">
            <div class="loginformContainer">
                <div class="pad_30">
                  <%--  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>--%>
                            <h2 class="pageH3 themeClrPrime upperCase txtCenter letter-sp-3 light mrg_B_5">Password Recovery</h2>
                            <div class="w100 mrg_B_20">
                                <span class="labelCap themeClrPrime">Mobile No. :*</span>
                                <asp:TextBox ID="txtUserId" CssClass="textBox usrName" MaxLength="10" placeholder="Enter Your Mobile No." runat="server"></asp:TextBox>
                            </div>
                           
                            <span class="space15"></span>
                            <%= errMsg %>
                            <asp:Button ID="btnRecover" runat="server" CssClass="btnLogin upperCase" Text="Send" OnClick="btnRecover_Click"/>
                       <%--   
                        </ContentTemplate>
                    </asp:UpdatePanel>--%>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

