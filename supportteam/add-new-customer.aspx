<%@ Page Title="" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="add-new-customer.aspx.cs" Inherits="supportteam_add_new_customer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="NewCustomer" runat="server">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title">New Customer </h3>
                    </div>
                    <div class="card-body">
                        <div class="col-sm-7">
                            <div class="form-group">
                                <label for="dName"><span style="color: red;">*</span> Customer Name: </label>
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="50" Width="80%" placeholder="customer Name"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="dName"><span style="color: red;">*</span> Mobile Number: </label>
                                <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" MaxLength="10" Width="80%" placeholder="10 digits only"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="dName"><span style="color: red;">*</span> Address: </label>
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Height="80" Width="80%" placeholder="Address"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <span class="space15"></span>
                                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" Text="Submit" OnClick="btnSubmit_Click" />
                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-sm btn-danger" Text="Cancel" OnClick="btnCancel_Click" />                               
                                <div class="float-clear"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

