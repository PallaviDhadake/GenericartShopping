<%@ Page Title="Check Pincode Delivery" Language="C#" MasterPageFile="~/franchisee/MasterFranchisee.master" AutoEventWireup="true" CodeFile="consume-api.aspx.cs" Inherits="franchisee_consume_api" %>
<%@ MasterType VirtualPath="~/franchisee/MasterFranchisee.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Check Pincode For Delivery</h2>
    <span class="space15"></span>

    <div id="editCountry" runat="server">
        <div class="card card-primary">
            <div class="card-header">
                <h3 class="card-title">Check Pincode Delivery</h3>
            </div>
            <div class="card-body">
                <div class="col-sm-7">
                    <div class="form-group">
                        <label for="cName">Enter Pincode:*</label>
                        <asp:TextBox ID="txtPincode" runat="server" CssClass="form-control " MaxLength="50" Width="80%"></asp:TextBox>
                    </div>
                </div>
                 <div class="col-sm-7">
                    <div class="form-group">
                        <label for="cName">Address:*</label>
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control textarea" TextMode="MultiLine" Height="120" Width="80%"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <span class="space15"></span>
        <asp:Button ID="btnCheck" runat="server" CssClass="btn btn-sm btn-primary" Text="Check Status" OnClick="btnCheck_Click"  />
        <asp:Button ID="btnAddress" runat="server" CssClass="btn btn-sm btn-primary" Text="Check Address" OnClick="btnAddress_Click"  />
        
        <div class="float_clear"></div>

        <%= errMsg %>
    </div>
</asp:Content>

