<%@ Page Title="Customer Registration | Genericart Online Generic Medicine Shopping" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="registration.aspx.cs" Inherits="registration" %>
<%@ MasterType VirtualPath="~/MasterParent.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <span class="space40"></span>
    <div class="col_1140">
        <div class="regForm margin_auto">
            <div class="loginformContainer">
                <div class="pad_30">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <%--<h2 class="pageH2 themeClrPrime upperCase txtCenter letter-sp-3 light mrg_B_10">Register Here</h2>--%>
                            <h2 class="pageH2 themeClrPrime upperCase txtCenter letter-sp-3 light mrg_B_10">Add Your Details</h2>
                            <!-- Name and Mobile Reg Proceed Starts -->
                            <div id="proceedData" runat="server" visible="false">
                                <div class="w100 mrg_B_15">
                                    <div class="app_r_padding">
                                        <span class="labelCap clrBlack">Name :*</span>
                                        <asp:TextBox ID="txtName" CssClass="textBox" MaxLength="50" placeholder="Enter Your Name" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="w100 float_left mrg_B_15">
                                    <div class="app_r_padding">
                                        <span class="labelCap clrBlack">Mobile :*</span>
                                        <asp:TextBox ID="txtMobile" CssClass="textBox" MaxLength="10" placeholder="Enter your 10 digit mobile number" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="float_clear"></div>
                                <%--<div class="w50 float_left mrg_B_15">
                                    <div class="app_r_padding">
                                        <span class="labelCap clrBlack">Email :*</span>
                                        <asp:TextBox ID="txtEmail" CssClass="textBox" MaxLength="50" placeholder="Enter Your Email Address" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="float_clear"></div>--%>

                                <%--<div class="w50 float_left mrg_B_15">
                                    <div class="app_r_padding">
                                        <span class="labelCap clrBlack">Country :*</span>
                                        <asp:TextBox ID="txtCountry" CssClass="textBox" MaxLength="30" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="w50 float_left mrg_B_15">
                                    <div class="app_r_padding">
                                        <span class="labelCap clrBlack">State :*</span>
                                        <asp:TextBox ID="txtState" CssClass="textBox" MaxLength="50"  runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="float_clear"></div>

                                <div class="w50 float_left mrg_B_15">
                                    <div class="app_r_padding">
                                        <span class="labelCap clrBlack">City :*</span>
                                        <asp:TextBox ID="txtCity" CssClass="textBox" MaxLength="30"  runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="w50 float_left mrg_B_15">
                                    <div class="app_r_padding">
                                        <span class="labelCap clrBlack">Pincode :*</span>
                                        <asp:TextBox ID="txtPinCode" CssClass="textBox" MaxLength="20"  runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="float_clear"></div>

                                <div class="w100 mrg_B_15">
                                    <span class="labelCap clrBlack">Address:*</span>
                                    <asp:TextBox ID="txtAddress1" CssClass="textBox" TextMode="MultiLine" Height="120" runat="server"></asp:TextBox>
                                </div>--%>
                                <%--<div class="w100 mrg_B_15">
                                    <span class="labelCap clrBlack">Password :*</span>
                                    <asp:TextBox ID="txtPassword" CssClass="textBox" MaxLength="20" TextMode="Password" runat="server"></asp:TextBox>
                                </div>--%>
                                <span class="space10"></span>
                                <%= proceedMsg %>
                                <span class="space10"></span>
                                <asp:Button ID="btnProceed" runat="server" CssClass="btnReg upperCase letter-sp-2" Text="Submit" OnClick="btnProceed_Click" />
                            </div>
                            <!-- Name and Mobile Reg Proceed Ends -->

                            <!-- Verify OTP Starts -->
                            <div id="otpVerify" runat="server" visible="false">
                                <span class="space20"></span>
                                <span class="regular fontRegular clrBlack">Verification Code is sent to your Mobile Number, <br /> Enter the verification code in below textbox</span>
                                <span class="space20"></span>
                                <div class="w100 mrg_B_15">
                                    <span class="labelCap clrBlack">Verification Code :*</span>
                                    <asp:TextBox ID="txtVerify" CssClass="textBox" MaxLength="4" placeholder="Enter Verification Code" runat="server"></asp:TextBox>
                                </div>
                                <span class="space15"></span>
                                <%= verifyMsg %>
                                <span class="space20"></span>
                                <asp:Button ID="btnVerify" runat="server" Text="Verify" CssClass="buttonForm float_left mrg_R_15" OnClick="btnVerify_Click" />
                                <asp:Button ID="btnResend" runat="server" Text="Resend" CssClass="buttonForm float_left mrg_R_15" OnClick="btnResend_Click" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonForm float_left" OnClick="btnCancel_Click" />
                                <div class="float_clear"></div>
                            </div>
                            <!-- Verify OTP Ends -->

                            <span class="space10"></span>
                            <%= errMsg %>
                            <span class="space20"></span>

                            <span class="greyLine"></span>
                            <div class="txtCenter">
                                <span class="clrBlack fontRegular semiMedium">Already Have an Account ?</span>
                                <span class="space20"></span>
                                <a href="<%= loginUrl %>" class="btnReg upperCase txtDecNone letter-sp-3">login</a>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

