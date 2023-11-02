<%@ Page Title="Send Notification to All" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="send-notification.aspx.cs" Inherits="supportteam_send_notification" %>
<%@ MasterType VirtualPath="~/supportteam/MasterSupport.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <h2 class="pgTitle">Send Notification to All</h2>
    <span class="space15"></span>

    <div class="card card-primary">
        <div class="card-header">
            <h3 class="card-title">Send Notification</h3>
        </div>
        <div class="card-body">
            <div class="col-sm-6">
                <label>Enter Notification Title :</label>
                <asp:TextBox ID="txtNotifTitle" runat="server" CssClass="form-control" Width="100%" MaxLength="250"></asp:TextBox>
                <span class="space20"></span>
            </div>
            <div class="col-sm-6">
                <label>Enter Notification Content :</label>
                <asp:TextBox ID="txtNotifMsg" runat="server" CssClass="form-control textarea" Width="100%" TextMode="MultiLine" Height="150"></asp:TextBox>
                <span class="space20"></span>
            </div>
            <div class="col-sm-6">
                <label>Notification Image :</label><br />
                <asp:FileUpload ID="fuFile" runat="server" />
                <span class="space20"></span>

                <span class="text-bold text-lg">OR</span><br />

                <label>Enter Image Url :</label>
                <asp:TextBox ID="txtUrl" runat="server" CssClass="form-control textarea" Width="100%"></asp:TextBox>
                <span class="space20"></span>
            </div>
            <div class="col-sm-6">
                <asp:CheckBox ID="chkDb" runat="server" TextAlign="Right" Text=" Save this notification to database ?" />
                <span class="space20"></span>
            </div>
        </div>
    </div>
    <span class="space20"></span>   
    <asp:Button ID="btnSubmit" runat="server" Text="Send Notification" CssClass="btn btn-md btn-primary" OnClick="btnSubmit_Click"  />
    <span class="space10"></span>
    <%= errMsg %>
</asp:Content>

