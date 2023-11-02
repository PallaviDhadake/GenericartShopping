<%@ Page Title="" Language="C#" MasterPageFile="~/obp/MasterObp.master" AutoEventWireup="true" CodeFile="validator.aspx.cs" Inherits="obp_validator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
       

        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <label for="name">Name:</label>
                <asp:TextBox ID="txtName" runat="server" CssClass="form-input" Required="true"></asp:TextBox>

                <label for="email">Email:</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-input" Required="true"></asp:TextBox>

                <label for="message">Message:</label>
                <asp:TextBox ID="txtMessage" runat="server" CssClass="form-input" TextMode="MultiLine" Required="true"></asp:TextBox>

                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>

