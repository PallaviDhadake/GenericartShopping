<%@ Page Title="Saving calculator" Language="C#" MasterPageFile="~/obp/MasterObp.master" AutoEventWireup="true" CodeFile="saving-calc.aspx.cs" Inherits="obp_saving_calc" %>

<%@ MasterType VirtualPath="~/obp/MasterObp.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.4/jquery.min.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js" type="text/javascript"></script>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="Stylesheet" type="text/css" />

    <script type="text/javascript">
        $(document).ready(function () {
            SearchText();
            //alert("document ready call");
        });
        function SearchText() {
            var pgUrl = '<%=ResolveUrl("/obp/saving-calc.aspx") %>'
            PageMethods.set_path('/obp/saving-calc.aspx');
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
        .bgOrange {
            background: #d1e751;
        }

        .clrGreen {
            color: #569702;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1 class="pgTitle">Know Your Saving</h1>

    <div class="card card-info">
        <div class="card-header">
            <h3 class="card-title">Know Your Saving</h3>
        </div>
        <div class="card-body">
            <div class="form-row">
                <div class="form-group col-md-6">
                    <label>Mobile No. :*</label>
                    <asp:TextBox ID="txtMobile" CssClass="form-control" MaxLength="10" placeholder="Without Country Code" runat="server"></asp:TextBox>
                </div>
                <div class="form-group col-md-6"></div>
                <div class="form-group col-md-6">
                    <label>Enter / Select your Drug name and start comparing Price. :*</label>
                    <asp:TextBox ID="txtMedName" CssClass="form-control" placeholder="Type medicine name" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txtMedId" runat="server" CssClass="form-control" Enabled="false" Visible="false"></asp:TextBox>
                </div>
                <div class="form-group col-md-6"></div>
                <div class="form-group col-md-3">
                    <asp:Button ID="btnCalculate" runat="server" Text="Calculate" CssClass="btn btn-md btn-primary" OnClick="btnCalculate_Click" />
                </div>
            </div>
        </div>
    </div>
    
    <%= errMsg %>

    <span class="space50"></span>

    <div id="medTable" runat="server" visible="false">
        <table class="gvCalc">
            <%= medStr %>
        </table>

        <div class="row">
            <div class="col-md-2">
            <div class="pad_10">
                <div class="bg-primary">
                    <div class="pad_20">
                        <span class="clrWhite fontRegular">Total Net Savings</span>
                        <span class="space10"></span>
                        <span class="semiBold medium clrWhite"><%= totalPercentage %></span>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-2">
            <div class="pad_10">
                <div class="bg-primary">
                    <div class="pad_20">
                        <span class="clrWhite fontRegular">Total Savings</span>
                        <span class="space10"></span>
                        <span class="semiBold medium clrWhite"><%= totalSum %> </span>
                    </div>
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
        <div class="">
            <asp:Button ID="btnProceed" runat="server" Text="Proceed To Enquiry" CssClass="btn btn-lg btn-info" OnClick="btnProceed_Click" />
        </div>
    </div>

    <div id="AddressBox" runat="server" visible="false">
        <div id="existingAddr" runat="server">
            <span class="space30"></span>
            <h3 class="text-blue pageH3">Select Shipping Address</h3>
            <asp:DropDownList ID="ddrAddress" runat="server" CssClass="form-control">
                <asp:ListItem Value="0"><- Select -></asp:ListItem>
            </asp:DropDownList>
            <div class="text-center text-lg text-bold text-blue">OR</div>
        </div>
        <div id="newAddr" runat="server">
            <span class="space10"></span>
            <h3 class="text-blue pageH3">Add Shipping Address :</h3>
            <div class="form-row">
                <div class="form-group col-md-6">
                    <label>Address :*</label>
                    <asp:TextBox ID="txtAddress1" runat="server" TextMode="MultiLine" Height="120" CssClass="form-control" Width="100%"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Country :*</label>
                    <asp:TextBox ID="txtCountry" runat="server" CssClass="form-control" Width="100%" MaxLength="30"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>State :*</label>
                    <asp:TextBox ID="txtState" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>City :*</label>
                    <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" Width="100%" MaxLength="30"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Pincode :*</label>
                    <asp:TextBox ID="txtPinCode" runat="server" CssClass="form-control" Width="100%" MaxLength="20"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Use this address as :*</label>
                    <asp:DropDownList ID="ddrAddrName" runat="server" CssClass="form-control">
                        <asp:ListItem Value="0"><- Select -></asp:ListItem>
                        <asp:ListItem Value="1">Home</asp:ListItem>
                        <asp:ListItem Value="2">Office</asp:ListItem>
                        <asp:ListItem Value="3">Other</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <span class="space20"></span>
        <asp:Button ID="btnSubmitOrder" runat="server" Text="Submit Order" CssClass="btn btn-md btn-success" OnClick="btnSubmitOrder_Click" />
    </div>


</asp:Content>

