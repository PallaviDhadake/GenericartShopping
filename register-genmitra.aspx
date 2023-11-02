<%@ Page Title="Generi Mitra Registration | Genericart Online Generic Medicine Shopping" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="register-genmitra.aspx.cs" Inherits="register_genmitra" MaintainScrollPositionOnPostback="true" %>

<%@ MasterType VirtualPath="~/MasterParent.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <span class="space40"></span>
    <div class="col_1140">
        <div class="regForm margin_auto">
            <div class="loginformContainer">
                <div class="pad_30">
                    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>--%>
                            <h2 class="pageH2 themeClrPrime upperCase txtCenter letter-sp-3 light mrg_B_10">Register Here</h2>
                            <!-- Name and Mobile Reg Proceed Starts -->
                            <%-- <div id="proceedData" runat="server" visible="false">--%>
                            <div class="w100 mrg_B_15">
                                <div class="app_r_padding">
                                    <span class="labelCap clrBlack">Name :*</span>
                                    <asp:TextBox ID="txtName" CssClass="textBox" MaxLength="50" placeholder="Enter Your Name" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="w50 float_left mrg_B_15">
                                <div class="app_r_padding">
                                    <span class="labelCap clrBlack">Mobile :*</span>
                                    <asp:TextBox ID="txtMobile" CssClass="textBox" MaxLength="10" placeholder="Enter your 10 digit mobile number" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="w50 float_left mrg_B_15">
                                <div class="app_r_padding">
                                    <span class="labelCap clrBlack">Email :*</span>
                                    <asp:TextBox ID="txtEmail" CssClass="textBox" MaxLength="50" placeholder="Enter Your Email Address" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="float_clear"></div>

                            <div class="w50 float_left mrg_B_15">
                                <div class="app_r_padding">
                                    <span class="labelCap clrBlack">State :*</span>
                                    <asp:DropDownList ID="ddrState" runat="server" CssClass="textBox" Width="100%" OnSelectedIndexChanged="ddrState_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="0"><- Select  -></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="w50 float_left mrg_B_15">
                                <div class="app_r_padding">
                                    <span class="labelCap clrBlack">District :*</span>
                                    <asp:DropDownList ID="ddrDistrict" runat="server" CssClass="textBox" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddrDistrict_SelectedIndexChanged">
                                        <asp:ListItem Value="0"><- Select -></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                             <div class="float_clear"></div>

                            <div class="w50 float_left mrg_B_15">
                                <div class="app_r_padding">
                                    <span class="labelCap clrBlack">City :</span>
                                    <asp:DropDownList ID="ddrCity" runat="server" CssClass="textBox" Width="100%">
                                        <asp:ListItem Value="0"><- Select -></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                           <%-- <div class="w100 mrg_B_15">
                                <span class="labelCap clrBlack">Password :*</span>
                                <asp:TextBox ID="txtPassword" CssClass="textBox" MaxLength="20" TextMode="Password" runat="server"></asp:TextBox>
                            </div>--%>

                            <div class="w50 float_left mrg_B_15">
                                <div class="app_r_padding">
                                    <span class="labelCap clrBlack">Bank Name :*</span>
                                    <asp:TextBox ID="txtBankName" CssClass="textBox" MaxLength="30" placeholder="Enter Bank Name" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="float_clear"></div>

                            <div class="w50 float_left mrg_B_15">
                                <div class="app_r_padding">
                                    <span class="labelCap clrBlack">Bank Account Name :</span>
                                    <asp:TextBox ID="txtAccName" CssClass="textBox" MaxLength="50" placeholder="Enter Your Bank Account Name" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            

                            <div class="w50 float_left mrg_B_15">
                                <div class="app_r_padding">
                                    <span class="labelCap clrBlack">Bank Account No. :</span>
                                    <asp:TextBox ID="txtAccNo" CssClass="textBox" MaxLength="50" placeholder="Enter Bank Account No" runat="server"></asp:TextBox>
                                </div>
                            </div>
                             <div class="float_clear"></div>
                            <div class="w50 float_left mrg_B_15">
                                <div class="app_r_padding">
                                    <span class="labelCap clrBlack">IFSC Code :</span>
                                    <asp:TextBox ID="txtIfsc" CssClass="textBox" MaxLength="50" placeholder="Enter Your IFSC Code" runat="server"></asp:TextBox>
                                </div>
                            </div>
                           

                            <div class="w50 float_left mrg_B_15">
                                <div class="app_r_padding">
                                    <span class="labelCap clrBlack">PAN No. :</span>
                                    <asp:TextBox ID="txtPan" CssClass="textBox" MaxLength="30" placeholder="Enter PAN no" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            
                            <div class="float_clear"></div>


                            <div class="w50 float_left mrg_B_15">
                                <div class="app_r_padding">
                                    <span class="labelCap clrBlack">Pan Card :</span><span class="subNotice">File size must be less than 1MB (image/pdf)</span>
                                    <asp:FileUpload ID="fuPan" runat="server" />
                                </div>
                            </div>
                           

                            <div class="w50 float_left mrg_B_15">
                                <div class="app_r_padding">
                                    <span class="labelCap clrBlack">Adhar Card :</span><span class="subNotice">File size must be less than 1MB (image/pdf)</span>
                                    <asp:FileUpload ID="fuAdhar" runat="server" />
                                </div>
                            </div>
                            
                            <div class="float_clear"></div>

                            <div class="w50 float_left mrg_B_15">
                                <div class="app_r_padding">
                                    <span class="labelCap clrBlack">Cheque / Passbook Photocopy :</span><span class="subNotice">File size must be less than 1MB (image/pdf)</span>
                                    <asp:FileUpload ID="fuPassbook" runat="server" />
                                </div>
                            </div>
                            
                            <div class="float_clear"></div>

                            <span class="space10"></span>
                            <%= errMsg %>
                            <span class="space20"></span>

                            <asp:Button ID="btnRegister" runat="server" CssClass="btnReg upperCase letter-sp-2" Text="Register" OnClick="btnRegister_Click"/>
                            <%--</div>--%>
                            <!-- Name and Mobile Reg Proceed Ends -->

                            <span class="greyLine"></span>
                            <%--   <div class="txtCenter">
                                <span class="clrBlack fontRegular semiMedium">Already Have an Account ?</span>
                                <span class="space20"></span>
                                <a href="<%= Master.rootPath + "login" %>" class="btnReg upperCase txtDecNone letter-sp-3">login</a>
                            </div>--%>
                        <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

