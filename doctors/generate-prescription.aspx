<%@ Page Language="C#" AutoEventWireup="true" CodeFile="generate-prescription.aspx.cs" Inherits="doctors_generate_prescription" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="IE=edge" http-equiv="X-UA-Compatible" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <link rel="shortcut icon" href="/favicon.ico" type="image/x-icon" />
    <title>Prescription</title>
    <link href="https://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700" rel="stylesheet" />
    <link href="../css/shoppingGencart.css" rel="stylesheet" />

    
</head>
<body>
    <div class="rxBox bgWhite box-shadow">
        <div class="pad_10">
            <div class="rxBorder">
                <div class="pad_15">
                    <%= rxStr %>
                    <%--<div class="rxLogo">
                        <img src="../images/rx.png" />
                    </div>
                    <div class="rxName">
                        <h3 class="clrBlack semiBold mrg_B_5 medium">Dr. Vankatesh Datar.</h3>
                        <span class="clrLightBlack semiBold regular">MBBS, MS(Surg)</span>
                        <span class="space5"></span>
                        <span class="clrGrey fontRegular">Physician</span>
                    </div>
                    <div class="float_clear"></div>
                    <span class="space15"></span>
                    <div class="rxLine"></div>
                    <span class="space15"></span>--%>
                   <%-- <div class="float_left">
                        <h4 class="clrDarkGrey mrg_B_5">Patient Name : Mr. Rakesh Sharma</h4>
                        <h4 class="clrDarkGrey mrg_B_5">Age : 35 Years</h4>
                        <h4 class="clrDarkGrey mrg_B_5">Gender : Male</h4>
                    </div>
                    <div class="float_right txtRight">
                        <h5 class="clrDarkGrey mrg_B_5">Date : 20 May 2021</h5>
                    </div>
                    <div class="float_clear"></div>
                    <span class="space20"></span>--%>

                    <%--<table class="rxtable">
                        <thead>
                            <tr>
                                <td>Medicine Name</td>
                                <td>Morning<br />Dose</td>
                                <td>Afternoon<br />Dose</td>
                                <td>Evening<br />Dose</td>
                            </tr>
                        </thead>
                        <tr>
                            <td>ACECLOFENAC 100 MG - 10 TAB <br /> (Note : Do not take on empty stomach)</td>
                            <td>1</td>
                            <td>0</td>
                            <td>1</td>
                        </tr>
                         <tr>
                            <td>Glimepiride 1 Mg <br /> (Note : Demo Note)</td>
                            <td>1</td>
                            <td>0</td>
                            <td>1</td>
                        </tr>
                        <tr>
                            <td>Folic Acid 5 Mg	 <br /> (Note : Take it empty stomach)</td>
                            <td>1</td>
                            <td>0</td>
                            <td>0</td>
                        </tr>
                        <tr>
                            <td>Glimepiride 1 Mg <br /> (Note : Demo Note)</td>
                            <td>1</td>
                            <td>0</td>
                            <td>1</td>
                        </tr>
                    </table>
                    <span class="space80"></span>
                    <span class="space50"></span>
                    <span class="fontRegular clrBlack">Doctor's Signature</span>
                    <span class="space10"></span>
                    <div class="rxBottomLine"></div>
                    <span class="space10"></span>
                    <div class="txtRight">
                        <span class="fontRegular clrBlack">Contact : +91 9090909090</span>
                    </div>--%>
                    <%--<p class="clrBlack fontRegular mrg_B_5">ACECLOFENAC 100 MG - 10 TAB</p>
                    <p class="clrBlack fontRegular mrg_B_5">1 --- 0 --- 1</p>
                    <p class="clrBlack   fontRegular">Do not take on empty stomach</p>--%>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
