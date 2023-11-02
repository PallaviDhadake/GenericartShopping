<%@ Page Title="Alert Message to Support Team" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="team-alert.aspx.cs" Inherits="supportteam_team_alert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Alert Message!</h2>
    <span class="space15"></span>
    <div class="callout callout-warning">
        <h5>Same day repeat call to customer alert</h5>
        <p>This message is because this customer received a call from our team today. Calling multiple times can get him / her annoying.</p>
        <h3>Recent call overview</h3>
        <dl>
            <dt>Name of Customer:</dt>
            <dd><%=callInfo[0] %></dd>
            <dt>Called By:</dt>
            <dd><%=callInfo[1] %></dd>
            <dt>Calling Date:</dt>
            <dd><%=callInfo[2] %></dd>
            <dt>Calling Time:</dt>
            <dd><%=callInfo[3] %></dd>
        </dl>
        <span class="space15"></span>
        <a href="dashboard.aspx" class="btn btn-secondary text-white text-decoration-none">Go Back to Dashboard</a>
    </div>
</asp:Content>

