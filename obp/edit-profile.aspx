<%@ Page Title="Generic Mitra | Edit Profile" Language="C#" MasterPageFile="~/genericmitra/MasterGenMitra.master" AutoEventWireup="true" CodeFile="edit-profile.aspx.cs" Inherits="genericmitra_edit_profile" %>
<%@ MasterType VirtualPath="~/genericmitra/MasterGenMitra.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .subNotice{font-size:0.9em; color:#aaa; display:block; margin-bottom:10px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="editGeneriMitra" runat="server">
        <div class="card card-primary">
            <div class="card-header">
                <h3 class="card-title">Update Profile</h3>
            </div>
            <%-- card body--%>
            <div class="card-body">
                <div class="colorLightBlue">
                    <span>Id :</span>
                    <asp:Label ID="lblId" runat="server"></asp:Label>
                </div>

                <span class="space15"></span>
                <%--form row start--%>
                <div class="form-row">
                    <div class="form-group col-md-6">
                        <label>Name :*</label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Width="100%" MaxLength="200"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Mobile :*</label>
                        <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                    </div>


                    <div class="form-group col-md-6">
                        <label>Email :*</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                    </div>

                    <div class="form-group col-md-6">
                        <label>Username :*</label>
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" Width="100%" Enabled="false"></asp:TextBox>
                    </div>

                    <div class="form-group col-md-6">
                        <label>Password :*</label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                    </div>


                    <div class="form-group col-md-6">
                        <label>Bank Name:*</label>
                        <asp:TextBox ID="txtBankName" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Bank Account Name:*</label>
                        <asp:TextBox ID="txtAccName" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Bank Account No:*</label>
                        <asp:TextBox ID="txtAccNo" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-6">
                        <label>IFSC Code:*</label>
                        <asp:TextBox ID="txtIfsc" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-6">
                        <label>PAN No:*</label>
                        <asp:TextBox ID="txtPan" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                    </div>

                    <div class="form-group col-md-6">
                        <label>State :*</label>
                        <asp:DropDownList ID="ddrState" runat="server" CssClass="form-control" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddrState_SelectedIndexChanged">
                            <asp:ListItem Value="0"><- Select -></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-md-6">
                        <label>District :*</label>
                        <asp:DropDownList ID="ddrDistrict" runat="server" CssClass="form-control" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddrDistrict_SelectedIndexChanged">
                            <asp:ListItem Value="0"><- Select -></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-md-6">
                        <label>City :*</label>
                        <asp:DropDownList ID="ddrCity" runat="server" CssClass="form-control" Width="100%">
                            <asp:ListItem Value="0"><- Select -></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-md-6">
                       
                    </div>
                    <div class="form-group col-md-6">
                        <label>Pan Card :*</label><span class="subNotice">File size must be less than 1MB (image/pdf)</span>
                        <asp:FileUpload ID="fuPan" runat="server" />
                        <%= pancardImg %>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Adhar Card :*</label><span class="subNotice">File size must be less than 1MB (image/pdf)</span>
                        <asp:FileUpload ID="fuAdhar" runat="server" />
                        <%= adharcardImg %>
                    </div>
                    <div class="form-group col-md-6">
                        <label >Cheque / Passbook Photocopy :*</label><span class="subNotice">File size must be less than 1MB (image/pdf)</span>
                        <asp:FileUpload ID="fuPassbook" runat="server" />
                        <%= bankdocImg %>
                    </div>

                </div>
                <%--form row End--%>
            </div>
            <%-- card body end--%>
        </div>
        <%--card end--%>

        <!-- Button controls starts -->
        <span class="space10"></span>
        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Update Info" OnClick="btnSave_Click" />

        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-outline-dark" Text="Back" OnClick="btnCancel_Click"/>
    </div>
</asp:Content>

