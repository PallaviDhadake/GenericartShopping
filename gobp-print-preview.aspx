<%@ Page Language="C#" AutoEventWireup="true" CodeFile="gobp-print-preview.aspx.cs" Inherits="gobp_print_preview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>GOBP Registration Proess</title>
    <%--<link href="../admingenshopping/css/iAdmin.css" rel="stylesheet" />--%>
    <style>
        .photoSize {
        	width: 140px;
        	height: 180px;
        	border: 2px solid #000;
        	position: absolute;
        	top: 50px;
        	right: 20px;
            margin-top: 30px;
        }        
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="col_1140">
            <div class="formPanel posRelative">
                <b><h4 class="formTitle themeDarkBg" style="text-align: center; margin-top: 30px; background-color: #23282d; color: #ffffff; padding: 0.7em;"><%=pgTitle %></h4></b>
                <span class="titleLine"></span>
                <br />
                <br />
                <div class="pad_10 ">
                    <%= errMsg %>
                    <div id="photo" runat="server">
                        <div class="photoSize">
                            <p class="txtCenter" style="padding: 60px 0; font-size: 0.9em; text-align:center ">Paste a
                                <br />
                                Photograph & do cross sign</p>
                        </div>
                        <div style="position: absolute; top: 250px; right: 20px; width: 28%; border: 2px solid #000; margin-top: 25px;">
                            <div class="pad_10" style="padding-bottom: 0.9em; padding-top: 0.9em; padding-left: 0.9em;">
                                <span class="bold_weight dspBlk small mrgBtm10">Name : <%= enqData[2] %></span>
                                <br />
                                <br />
                                <span class="space10"></span>
                                <span class="bold_weight dspBlk small mrgBtm10">Signature : </span>
                            </div>
                        </div>
                    </div>
                    <%--<div class="headInfo1">
                        <div class="pad_15 ">
                            <%= headInfo %>
                        </div>
                    </div>--%>
                    <%--<img src="images/genericart-logo.png" style="text-align: center" />--%>
                    <%--<img src="images/genericart-medicine-logo-name.png" />--%>
                    <span class="space10"></span>
                    <table class="form_table" >

                        <tr>
                        <td><span class="text-lg text-bold text-purple">Type:</span></td>
                        <td><span class="text-lg text-bold text-purple"><%= enqData[0] %></span></td>
                    </tr>
                    <tr>
                        <td style="width:25%" ><span class="formLable bold_weight">Join Date:</span></td>
                        <td ><span class="formLable"><%= enqData[1] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Applicant Name :</span></td>
                        <td><span class="formLable" style="display:block; width:60% !important;"><%= enqData[2] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Firm Name :</span></td>
                        <td><span class="formLable" style="display:block; width:60% !important;"><%= enqData[3] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Firm Type :</span></td>
                        <td><span class="formLable"><%= enqData[4] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Address :</span></td>
                        <td><span class="formLable" style="display:block; width:60% !important;"><%= enqData[5] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Date Of Birth :</span></td>
                        <td><span class="formLable"><%= enqData[6] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Age :</span></td>
                        <td><span class="formLable"><%= enqData[7] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Shop Code :</span></td>
                        <td><span class="formLable"><%= enqData[8] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">State :</span></td>
                        <td><span class="formLable"><%= enqData[9] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">District :</span></td>
                        <td><span class="formLable"><%= enqData[10] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">City :</span></td>
                        <td><span class="formLable"><%= enqData[11] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Mobile No :</span></td>
                        <td><span class="formLable"><%= enqData[12] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Email Id :</span></td>
                        <td><span class="formLable"><%= enqData[13] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Whatsapp No. :</span></td>
                        <td><span class="formLable"><%= enqData[14] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Education :</span></td>
                        <td><span class="formLable"><%= enqData[15] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Present Occupation of Owner :</span></td>
                        <td><span class="formLable"><%= enqData[16] %></span> </td>
                    </tr>
                    <%--<tr>
                        <td><span class="formLable bold_weight">Annual Income :</span></td>
                        <td><span class="formLable"><%= enqData[20] %></span> </td>
                    </tr>--%>
                    <tr>
                        <td><span class="formLable bold_weight">Any Illegal Matter :</span></td>
                        <td><span class="formLable"><%= enqData[17] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Residence From :</span></td>
                        <td><span class="formLable"><%= enqData[18] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Marital Status :</span></td>
                        <td><span class="formLable"><%= enqData[19] %></span> </td>
                    </tr>
                    <%--<tr>
                        <td><span class="formLable bold_weight">Distance From Nearest Existing Franchisee :</span></td>
                        <td><span class="formLable"><%= enqData[24] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Ready To Invest 8-10 Lakhs :</span></td>
                        <td><span class="formLable"><%= enqData[25] %></span> </td>
                    </tr>--%>

                    <!--New Payment Details-->
                    <tr>
                        <td><span class="formLable bold_weight">UTR NO:</span></td>
                        <td><span class="formLable"><%= enqData[20] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Bank Name:</span></td>
                        <td><span class="formLable"><%= enqData[21] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Transaction Date:</span></td>
                        <td><span class="formLable"><%= enqData[22] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Account Holder Name:</span></td>
                        <td><span class="formLable"><%= enqData[23] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Paid Amount:</span></td>
                        <td><span class="formLable"><%= enqData[24] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Address Proof :</span></td>
                        <td><span class="formLable"><%= enqData[25] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Id Proof :</span></td>
                        <td><span class="formLable"><%= enqData[26] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Profile :</span></td>
                        <td><span class="formLable"><%= enqData[27] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Resume :</span></td>
                        <td><span class="formLable"><%= enqData[28] %></span> </td>
                    </tr>

                        <tr>
                            <td colspan="2">
                                <%= enqData[35] %>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>

            <span class="space20"></span>
            <a href="javascript:printDoc()" class="buttonBlue float_left mrg_R_15">Print</a>
            <asp:Button ID="btnPrint" runat="server" CssClass="addNew themeClrBlue float_left" Text="Back" OnClick="btnPrint_Click" />
            <div class="float_clear"></div>

            <span class="space30"></span>
        </div>
    </form>

    <script type="text/javascript">
        function printDoc() {
            window.print();
        }
    </script>
</body>
</html>
