<%@ Page Title="Customer Master" Language="C#" MasterPageFile="~/bdm/MasterBdm.master" AutoEventWireup="true" CodeFile="customer-master.aspx.cs" Inherits="bdm_customer_master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="NewCustomer" runat="server">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
         <div class="card card-primary">
            <div class="card-header">
                <h3 class="card-title">Customer Master (Only from survey data)</h3>
            </div>
              <div class="card-body">
                <div class="col-sm-7">
                    <div class="form-group">
                        <label for="dName">Customer Name:*</label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control " MaxLength="50" Width="80%"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="dName">Mobile Number:*</label>
                        <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control " MaxLength="10" Width="80%" placeholder="10 digits only"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <span class="space15"></span>
                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" Text="Submit" OnClick="btnSubmit_Click" />
                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-sm btn-outline-dark" Text="Cancel" OnClick="btnCancel_Click" />
                        <div class="float_clear"></div>
                    </div>
                </div>
            </div>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

