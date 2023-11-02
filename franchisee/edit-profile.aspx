<%@ Page Title="Edit Profile | Genericart Shopping Franchisee Control Panel" Language="C#" MasterPageFile="~/franchisee/MasterFranchisee.master" AutoEventWireup="true" CodeFile="edit-profile.aspx.cs" Inherits="franchisee_edit_profile" %>
<%@ MasterType VirtualPath="~/franchisee/MasterFranchisee.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Edit Profile Master</h2>
    <span class="space15"></span>
     <div id="editProfile" runat="server">
        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>--%>
                <%--card start--%>
                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title"><%= pgTitle %></h3>
                    </div>
                   <%-- card body--%>
                    <div class="card-body">
                        <%--<div class="colorLightBlue">
                            <span>Id :</span>
                            <asp:Label ID="lblId" runat="server" Text="[New]"></asp:Label>
                        </div>--%>

                        <span class="space15"></span>
                        <%--form row start--%>
                        <div class="form-row">
                            <div class="form-group col-md-6">
                                <label>Franchisee Name :*</label>
                                <asp:TextBox ID="txtFrName" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Franchisee Shop Code :</label>
                                <asp:TextBox ID="txtFrCode" runat="server" CssClass="form-control" Width="100%" MaxLength="30" ReadOnly="true"></asp:TextBox>
                            </div> 
                            <div class="form-group col-md-6">
                                <label>Registered Date:</label>
                                <asp:TextBox ID="txtFrDate" runat="server" CssClass="form-control" Width="100%" MaxLength="30" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Owner Name:*</label>
                                <asp:TextBox ID="txtFrOwnerName" runat="server" CssClass="form-control" Width="100%" MaxLength="30"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>PinCode :*</label>
                                <asp:TextBox ID="txtFrPincode" runat="server" CssClass="form-control" Width="100%" MaxLength="10"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Franchisee State :*</label>
                                <asp:DropDownList ID="ddrFrState" runat="server" CssClass="form-control" Width="100%">
                                    <asp:ListItem Value="0"><- Select -></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Email :*</label>
                                <asp:TextBox ID="txtFrEmail" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Mobile :*</label>
                                <asp:TextBox ID="txtFrMobile" runat="server" CssClass="form-control" Width="100%" MaxLength="10"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Franchisee Address:*</label>
                                <asp:TextBox ID="txtFrAddress" runat="server" CssClass="form-control textarea" TextMode="MultiLine" Height="150" Width="100%"></asp:TextBox>
                            </div>

                        </div>
                        <%--form row End--%>
                         <%--form row start--%>
                        <div class="form-row">

                             <div class="form-group col-md-6">
                                <label>Bank Name :</label>
                                <asp:TextBox ID="txtFrBankName" runat="server" CssClass="form-control" Width="100%" MaxLength="30"></asp:TextBox>
                            </div>
                             <div class="form-group col-md-6">
                                <label>Branch :</label>
                                <asp:TextBox ID="txtBankBranch" runat="server" CssClass="form-control" Width="100%" MaxLength="30"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Bank Account Name :</label>
                                <asp:TextBox ID="txtBankAccName" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                            </div>
                             <div class="form-group col-md-6">
                                <label>Bank Account No. :</label>
                                <asp:TextBox ID="txtBankAccNo" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                            </div>

                            <div class="form-group col-md-6">
                                <label>Bank IFSC :</label>
                                <asp:TextBox ID="txtBankIfsc" runat="server" CssClass="form-control" Width="100%" MaxLength="30"></asp:TextBox>
                            </div>

                           
                          
                           
                        </div>
                        <%--form row End--%>

                         <%--form row start--%>
                        <div class="form-row">
                            <div class="form-group col-md-6">
                                 <label>Adhar Card :</label>
                                 <asp:FileUpload ID="adharImg" runat="server" CssClass="form-control-file" />
                                <span class="space10"></span>
                                <%= adharPhoto %>
                            </div>
                            <div class="form-group col-md-6">
                                 <label>Pan Card :</label>
                                 <asp:FileUpload ID="panImg" runat="server" CssClass="form-control-file" />
                                <span class="space10"></span>
                                <%= panPhoto %>
                            </div>
                        </div>
                    </div>
                   <%-- card body end--%>
                </div>
                <%--card end--%>

                <!-- Button controls starts -->
                <span class="space10"></span>
                <span class="space10"></span>
               <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save"  OnClick="btnSave_Click" />
              
                <!-- Button controls ends      -->
            <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>

</asp:Content>

