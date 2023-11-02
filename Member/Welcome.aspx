<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/Member/Welcome.aspx.cs" Inherits="Welcome" MasterPageFile="~/Member/MemberMain.master" %>

<asp:Content ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
        function printDiv(divID) {
            var divElements = document.getElementById(divID).innerHTML;
            var oldPage = document.body.innerHTML;
            document.body.innerHTML =
                "<html><body>" +
                divElements + "</body>";
            window.print();
            document.body.innerHTML = oldPage;
        }
    </script>

    <style>
        .responsive-wel {
            background-image: url( "../images/welcome-letter-border.jpg" );
            background-size: 100% 100%;
            width: 100%;
        }

        .welcomelet {
            width: 383px;
            height: 93px;
            margin-top: 45px;
        }

        .c7 {
            font-size: 16px;
            font-family: arial;
            font-weight: bold;
        }

        .c3 {
            font-size: 20px;
            color: #009900;
            font-family: arial;
            font-style: italic;
            font-weight: bold;
        }

        .c4 {
            color: #000000;
            font-family: Arial;
            line-height: 22px;
            vertical-align: middle;
        }

        .img2 {
            height: 100px;
            margin: 0 auto;
            width: 100px;
        }

        .PrintFunction {
            background-color: rgba(153, 7, 7, 1);
            border-radius: 8px;
            color: rgba(255, 255, 255, 1);
            display: block;
            font-family: "tahoma";
            font-size: 1.1em;
            font-weight: 700;
            margin: 15px;
            padding: 5px;
            text-align: center;
            text-decoration: none;
            transition: all 0.1s ease 0s;
            width: 139px;
        }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="breadcrumbs">
        <div class="col-sm-4">
            <div class="page-header float-left">
                <div class="page-title">
                    <h1>Welcome</h1>
                </div>
            </div>
        </div>
        <div class="col-sm-8">
            <div class="page-header float-right">
                <div class="page-title">
                    <ol class="breadcrumb text-right">
                        <li class="active">Welcome</li>
                    </ol>
                </div>
            </div>
        </div>
    </div>
    <div class="content mt-3">
        <asp:HiddenField ID="Hf" runat="server" />
        <div style="width: 100%;" align="center">
            <div style="width: 85%;">
                <div align="center" style="font-size: 22px; font-weight: bold; padding-bottom: 10px;">
                    Welcome Letter
                </div>
                <div id="printdiv" style="width: 100%; background-color: White; padding: 10px;">
                    <table width="100%" cellpadding="0" cellspacing="0" style="font-family: Verdana; line-height: 22px;">
                        <tr>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 40%;" valign="top">
                                <%--<img src="../images/logo.png" style="max-height: 150px;" />--%>
                            </td>
                            <td></td>
                            <td valign="top">
                                <%--<table style="width: 100%;">
                                    <tr>
                                        <td style="width: 100%; text-transform: uppercase; font-weight: bold; font-size: 21px;">
                                            <span style="color: #DA251D;">Ahart </span><span style="color: #005CA1;">Marketing Services Pvt Ltd</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-size: 15px;">C/O Brijesh Kumar, WN-4 Kamla Nagar, 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-size: 15px;">Chandauli Uttar Pradesh, India 232104
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-size: 15px;">Web : www.ahartmarketingservices.com
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="font-size: 15px;">Contact: +91 94551 51783
                                        </td>
                                    </tr>
                                </table>--%>
                                <img src="../images/logo.png" class="img-responsive" style="max-height: 150px;" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="padding-top: 30px;">
                                <span>Welcome
                                        <asp:Label ID="LblHeadName" runat="server" Text="" CssClass="c7"></asp:Label>
                                    You are Sucessfully Registered as a Member in <% = MasterClass.GetProjectName() %>.
                                    <br />
                                    <br />
                                    Your Details as as Follows..</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="padding-top: 20px; padding-bottom: 15px;">
                                <table style="width: 100%;" border="1" cellpadding="3px" cellspacing="0">
                                    <tr>
                                        <td style="font-weight: bold;" align="center">ID
                                        </td>
                                        <td style="font-weight: bold;" align="center">Name
                                        </td>
                                        <td style="font-weight: bold;" align="center">Registration Date
                                        </td>
                                        <td style="font-weight: bold;" align="center">Package
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="LblID" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="LblName" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="LblDOJ" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="LblPackage" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <div style="height: 20px;"></div>
                                <table style="width: 100%;" border="1" cellpadding="3px" cellspacing="0">
                                    <tr>
                                        <td style="font-weight: bold;" align="center">TopUp Date
                                        </td>
                                        <td style="font-weight: bold;" align="center" colspan="2">Address
                                        </td>
                                        <td style="font-weight: bold;" align="center">Sponsor Id
                                        </td>
                                        <td style="font-weight: bold;" align="center">Sponsor Name
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">&nbsp;<asp:Label ID="LblTopUpDate" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td align="center" colspan="2">
                                            <asp:Label ID="LblAddress" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="LblSponsorId" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="LblSponsorName" runat="server" Text=""></asp:Label>
                                        </td>
                                        <%--<td align="center">
                                            <asp:Label ID="LblPanNo" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="LblAadharNo" runat="server" Text=""></asp:Label>
                                        </td>--%>
                                    </tr>
                                </table>
                                <div style="height: 20px;"></div>
                                <table style="width: 100%;" cellpadding="3px" cellspacing="0">
                                    <tr>
                                        <td><b>Important - </b>Please be sure the change password on the distributer form to ensure the security of member account once again than you for joining our company we look forward to a long lasting and successful partnership together<br />
                                            <br />
                                            <div class="text-left" style="font-weight: bold;">
                                                Your sincerely 
                                                <br />
                                                <%= MasterClass.GetProjectName() %>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <tr>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>

                        <tr>
                            <td colspan="3">&nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="height: 30px;">
                    <a href="#" class="btn btn-primary" onclick="javascript:printDiv('printdiv')">Print</a>
                </div>
                <div style="clear: both">
                </div>
            </div>
            <div style="clear: both">
            </div>
        </div>
    </div>
</asp:Content>
