<%@ Page Title="Customer Login | Genericart Online Generic Medicine Shopping" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>
<%@ MasterType VirtualPath="~/MasterParent.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        var onloadCallback = function () {
            grecaptcha.render('recaptcha', {
                'sitekey': '6LcNBIUUAAAAADbX-_n6UhdJhtAxQDgiypcyZqSN'
            });
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <span class="space40"></span>
    <div class="col_1140">
        <div class="loginForm margin_auto">
            <div class="loginformContainer">
                <div class="pad_30">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" OnLoad="UpdatePanel1_Load">
                        <ContentTemplate>
                            <h2 class="pageH2 themeClrPrime upperCase txtCenter letter-sp-3 light mrg_B_5">login</h2>
                            <div class="w100 mrg_B_20">
                                <span class="labelCap themeClrPrime">Mobile / E-mail:*</span>
                                <asp:TextBox ID="txtUserId" CssClass="textBox usrName" MaxLength="50" placeholder="Enter Your Mobile No. or email address" runat="server"></asp:TextBox>
                            </div>
                            <div class="w100 mrg_B_20">
                                <span class="labelCap themeClrPrime">Password :*</span>
                                <asp:TextBox ID="txtPassword" CssClass="textBox usrPwd" MaxLength="20" TextMode="Password" placeholder="Enter Your Password" runat="server"></asp:TextBox>
                            </div>
                            <div class="w100">
                                <div id="recaptcha" style="-moz-transform:scale(0.77); -ms-transform:scale(0.77); -o-transform:scale(0.77);-webkit-transform:scale(0.77); transform:scale(0.77); -webkit-transform-origin:0 0; -moz-transform-origin:0 0; -ms-transform-origin:0 0; -o-transform-origin:0 0; transform-origin:0 0;"></div>
                                <script src="https://www.google.com/recaptcha/api.js?onload=onloadCallback&render=explicit" async defer></script>
                            </div>
                            <span class="space15"></span>
                            <%= errMsg %>
                            <asp:Button ID="btnLogin" runat="server" CssClass="btnLogin upperCase" Text="LogIn" OnClick="btnLogin_Click" />
                            <span class="space20"></span>
                            <a href="<%= Master.rootPath + "forgot-password" %>" class="forgetLink">Forgot Password ?</a>
                            <span class="space3"></span><span class="themeClrSec fontRegular small">OR</span><span class="space3"></span>
                            <%--<span class="themeClrSec fontRegular small">New User ?</span> &nbsp;<a href="<%= Master.rootPath + "registration" %>" class="forgetLink">Register Here</a>--%>
                            <span class="themeClrSec fontRegular small">New User ?</span> &nbsp;<a href="<%= regLink %>" class="forgetLink">Register Here</a>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

