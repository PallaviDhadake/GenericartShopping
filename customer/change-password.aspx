<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/customer/MasterCustomer.master" AutoEventWireup="true" CodeFile="change-password.aspx.cs" Inherits="customer_change_password" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="width50">
        <div class="pad_10">
            <div class="bgWhite border_r_5 box-shadow">
                <div class="pad_20">
                    <span class="space20"></span>
                    <h2 class="clrLightBlack semiBold semiMedium">Change Password</h2>
                    <span class="space20"></span>

                    <div class="w50 float_left mrg_B_15">
                        <div class="app_r_padding">
                            <span class="labelCap">Old Password :*</span>
                            <asp:TextBox ID="txtOldPass" runat="server" CssClass="textBox w95" MaxLength="50" TextMode="Password"></asp:TextBox>
                        </div>
                    </div>
                    <div class="float_clear"></div>
                    <div class="w50 float_left mrg_B_15">
                        <div class="app_r_padding">
                            <span class="labelCap">New password :*</span>
                            <asp:TextBox ID="txtNewPass" runat="server" CssClass="textBox w95" TextMode="Password"></asp:TextBox>
                        </div>
                    </div>
                    <div class="float_clear"></div>
                    <div class="w50 float_left mrg_B_15">
                        <div class="app_r_padding">
                            <span class="labelCap">Confirm password :*</span>
                            <asp:TextBox ID="txtConfirmPass" runat="server" CssClass="textBox w95" TextMode="Password"></asp:TextBox>
                        </div>
                    </div>
                    <div class="float_clear"></div>
                    <span class="space15"></span>
                    <asp:Button ID="btnSubmit" runat="server" Text="Change Password" CssClass="pinkAnch small semiBold dspInlineBlk" OnClick="btnSubmit_Click" />
                    <!-- Form ends   -->
                </div>
            </div>
        </div>
    </div>

</asp:Content>

