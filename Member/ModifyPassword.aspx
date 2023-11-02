<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ModifyPassword.aspx.cs" Inherits="Modify" MasterPageFile="~/Member/MemberMain.master" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="breadcrumbs">
        <div class="col-sm-4">
            <div class="page-header float-left">
                <div class="page-title">
                    <h1>Modify Password</h1>
                </div>
            </div>
        </div>
        <div class="col-sm-8">
            <div class="page-header float-right">
                <div class="page-title">
                    <ol class="breadcrumb text-right">
                        <li class="active">Modify Password</li>
                    </ol>
                </div>
            </div>
        </div>
    </div>
    <div class="content mt-3">
        <div class="col-md-12">
            <div class="box box-primary">
                <div class="box-body">
                    <asp:UpdatePanel ID="UP1" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="PnlChangePass" DefaultButton="BtnModifyPassword" runat="server">
                                <div class="col-md-2">
                                    <span>Current Password</span>
                                </div>
                                <div class="col-md-4 col-xs-12">
                                    <asp:TextBox ID="TxtCurrentPassword" CssClass="form-control" placeholder="Current Password" runat="server" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="validator_changepassword" runat="server" ErrorMessage="Enter Current Password." ControlToValidate="TxtCurrentPassword" ValidationGroup="Changepass" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-2">
                                    <span>New Password</span>
                                </div>
                                <div class="col-md-4 col-xs-12">
                                    <asp:TextBox ID="TxtNewPassword" CssClass="form-control" placeholder="New Password" runat="server" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="validator_changepassword" runat="server" ErrorMessage="Enter New Password." ControlToValidate="TxtConfirmPassword" ValidationGroup="Changepass" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                                </div>
                                    <div class="col-md-2">
                                        <span>Confirm Password</span>
                                    </div>
                                <div class="col-md-4 col-xs-12">
                                    <asp:TextBox ID="TxtConfirmPassword" CssClass="form-control" runat="server" placeholder="Confirm Password" TextMode="Password"></asp:TextBox>
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Enter Correct Password." CssClass="validator_changepassword" ControlToCompare="TxtNewPassword" ControlToValidate="TxtConfirmPassword" Display="Dynamic" ValidationGroup="Changepass" SetFocusOnError="true"></asp:CompareValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="validator_changepassword" ErrorMessage="Enter Confirm Password." ControlToValidate="TxtConfirmPassword" ValidationGroup="Changepass" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    </td>
                                </div>
                                <div class="col-md-2">
                                    <asp:Button ID="BtnModifyPassword" runat="server" OnClick="Button1_Click" ValidationGroup="Changepass" Text="Modify Password" CssClass="btn btn-primary" />
                                    <asp:ValidationSummary ShowMessageBox="false" ShowSummary="false" runat="server" ValidationGroup="Changepass" />
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
