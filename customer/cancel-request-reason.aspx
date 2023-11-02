<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cancel-request-reason.aspx.cs" Inherits="customer_cancel_request_reason" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="IE=edge" http-equiv="X-UA-Compatible" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <title></title>
    <link href="../css/shoppingGencart.css" rel="stylesheet" />
    <script src="../js/jquery-2.2.4.min.js"></script>

    <style>
        .popupContainer {
            background-color: antiquewhite;
            padding: 20px;
        }
    </style>
</head>
<body style="width: 500px!important; overflow-x: hidden;">
    <form id="form1" runat="server">
        <div class="popupContainer">
            <div class="">
                <span class="space50"></span>
                <span class="semiBold">Please Select Reason :</span>
                <span class="space20"></span>
                <asp:DropDownList ID="ddrReasons" runat="server" CssClass="textBox w50">
                    <asp:ListItem Value="0"><- Select -></asp:ListItem>
                </asp:DropDownList>
            </div>
            <span class="space20"></span>
            <%= errMsg %>
            <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" CssClass="blueAnch" />
            <span class="space50"></span>

        </div>
    </form>
</body>
</html>
