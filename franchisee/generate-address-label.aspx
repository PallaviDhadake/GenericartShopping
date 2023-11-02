<%@ Page Language="C#" AutoEventWireup="true" CodeFile="generate-address-label.aspx.cs" Inherits="franchisee_generate_address_label" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="IE=edge" http-equiv="X-UA-Compatible" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <link rel="shortcut icon" href="/favicon.ico" type="image/x-icon" />
    <title>Address Label Report</title>
    <link href="https://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700" rel="stylesheet" />
    <link href="../css/shoppingGencart.css" rel="stylesheet" />    
</head>
<body>
    <div class="rxBox bgWhite box-shadow">
        <div class="pad_10">
            <div class="rxBorder">
                <div class="pad_15">
                    <%= rxStr %>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
