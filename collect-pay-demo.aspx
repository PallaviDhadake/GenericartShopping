<%@ Page Language="C#" AutoEventWireup="true" CodeFile="collect-pay-demo.aspx.cs" Inherits="collect_pay_demo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="IE=edge" http-equiv="X-UA-Compatible" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <link rel="shortcut icon" href="/favicon.ico" type="image/x-icon" />

    <title>Genericart Online Generic Medicine Shopping</title>

    <meta name="description" content="" />

    <link href="https://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700" rel="stylesheet" />

    <link href="css/shoppingGencart.css" rel="stylesheet" />
    <script src="js/jquery-2.2.4.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <span class="space50"></span>
        <div class="col_1140">
            <div class="col_800 bgWhite border_r_5 pad_15 box-shadow">

                <h2 class="pageH2 themeClrPrime mrg_B_10">Online Payment</h2>
                <div class="w100 mrg_B_15">
                    <span class="labelCap">UPI ID :*</span>
                    <asp:TextBox ID="txtUpi" CssClass="textBox w95" MaxLength="50" placeholder="xxxxxxxxxx@sbi" runat="server"></asp:TextBox>
                </div>
                <div class="w50 float_left mrg_B_15">
                    <div class="app_r_padding">
                        <span class="labelCap">Order Amount :*</span>
                        <asp:TextBox ID="txtAmount" CssClass="textBox w95" MaxLength="30" placeholder="" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="float_clear"></div>
                <asp:Button ID="btnSubmit" runat="server" CssClass="buttonForm" Text="SEND" OnClick="btnSubmit_Click1" /><br />
                <%= errMsg %>
            </div>
            <div class="float_clear"></div>
        </div>
        <span class="space50"></span>
    </form>
</body>
</html>
