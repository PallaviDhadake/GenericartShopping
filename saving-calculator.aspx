<%@ Page Title="Saving Calculator" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="saving-calculator.aspx.cs" Inherits="saving_calculator" %>
<%@ MasterType VirtualPath="~/MasterParent.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <%--<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.4/jquery.min.js" type = "text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js" type = "text/javascript"></script>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel = "Stylesheet" type="text/css" />--%>

    <script type="text/javascript">
        $(document).ready(function () {
            SearchText();
            //alert("document ready call");
        });
        function SearchText() {
            var pgUrl = '<%=ResolveUrl("saving-calculator.aspx") %>'
            PageMethods.set_path('/saving-calculator.aspx');
            $("#<%= txtMedName.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        //url: "saving-calculator.aspx/GetMedName",
                        url: pgUrl + "/GetMedName",
                        data: "{'medName':'" + document.getElementById('<%= txtMedName.ClientID %>').value + "'}",
                        dataType: "json",
                        success: function (data) {
                            //response(data.d);
                            //alert(data.d);
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split('#')[0],
                                    val: item.split('#')[1]
                                }
                            }))
                        },
                        error: function (result) {
                            alert("No Match");
                        }
                    });
                },
                select: function (e, i) {
                    $("#<%= txtMedId.ClientID %>").val(i.item.val);
                    PageMethods.GetMedInfo(i.item.val, onSucess, onError);
                    //alert(i.item.val);
                    function onSucess(result) {
                        //alert(result);
                    }

                    function onError(result) {
                        alert(result.get_message());
                    }
                },
                minLength: 1
            });
        }
    </script> 
    <style>
        /*.bgOrange{background:#ffa600;}*/
        .bgOrange{background:#d1e751;}
        .clrGreen{color:#569702;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <span class="space40"></span>
    <div class="col_1140 bgWhite border_r_5">
        <h1 class="pageH1 themeClrPrime semiBold txtCenter">Saving Calculator</h1>
        <div class="col_800_center">
            <div class="nwsBox-shadow">
                <div class="pad_25">
                    <img src="<%= Master.rootPath + "images/saving-calc-header-ecomm.jpg" %>" alt="Save more than 50% with Generic medicines" class="width100" />
                    <span class="space40"></span>
                    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>--%>
                            <div class="w100 mrg_B_15">
                                <span class="fontRegular semiMedium mrg_B_3" style="display:block;">Enter your Mobile No. :*</span>
                                <span class="small semiBold colrPink mrg_B_15" style="display:block;">If you already have an account with us, please enter your registered mobile number</span>
                                <asp:TextBox ID="txtMobile" CssClass="textBox w95" MaxLength="10" placeholder="Without Country Code" runat="server"></asp:TextBox>
                            </div>
                            <span class="space10"></span>
                            <div class="w100 mrg_B_15">
                                <span class="fontRegular semiMedium mrg_B_15" style="display:block;">Enter / Select your Drug name and start comparing Price.</span>
                                <asp:TextBox ID="txtMedName" CssClass="textBox w95" placeholder="Type medicine name" runat="server"></asp:TextBox>
                                <asp:TextBox ID="txtMedId" runat="server" CssClass="textBox" Enabled="false" Visible="false"></asp:TextBox>
                            </div>
                            <span class="space15"></span>
                            <%= errMsg %>
                            <div class="txtCenter"><asp:Button ID="btnCalculate" runat="server" Text="Calculate" CssClass="buttonForm semiBold upperCase letter-sp-2" OnClick="btnCalculate_Click" /></div>

                            <span class="space50"></span>

                            <div id="medTable" runat="server" visible="false">
                                <table class="gvCalc">
                                    <%--<tr>
                                        <td>Branded</td>
                                        <td>PARACETAMOL (INTAS) 500 MG</td>
                                        <td>50</td>
                                        <td rowspan="2">30</td>
                                        <td rowspan="2">70%</td>
                                    </tr>
                                    <tr>
                                        <td>Generic</td>
                                        <td>GMP0308</td>
                                        <td>20</td>
                                    </tr>--%>
                                    <%= medStr %>
                                </table>
                            
                                <div class="width30Right txtCenter">
                                    <div class="pad_10">
                                        <div class="themeBgSec">
                                            <div class="pad_20">
                                                <span class="clrWhite fontRegular">Total Net Savings</span>
                                                <span class="space10"></span>
                                                <span class="semiBold medium clrWhite"><%= totalPercentage %></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="width30Right txtCenter">
                                    <div class="pad_10">
                                        <div class="themeBgSec">
                                            <div class="pad_20">
                                                <span class="clrWhite fontRegular">Total Savings</span>
                                                <span class="space10"></span>
                                                <span class="semiBold medium clrWhite"><%= totalSum %> </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="float_clear"></div>
                                <div class="txtRight">
                                    <div class="pad_10">
                                        <asp:CheckBox ID="chkMreq" runat="server" TextAlign="Right" CssClass="themeBgPrime small pad_TB_10 pad_LR_15 clrWhite" Text=" Mark this as my Monthly Requirement ?" />
                                    </div>
                                </div>
                                <span class="space30"></span>
                                <div class="txtCenter">
                                    <asp:Button ID="btnProceed" runat="server" Text="Proceed To Enquiry" CssClass="buttonForm semiBold upperCase letter-sp-2" OnClick="btnProceed_Click" />
                                </div>
                            </div>
                        <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

