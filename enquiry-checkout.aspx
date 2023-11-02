<%@ Page Title="Checkout" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="enquiry-checkout.aspx.cs" Inherits="enquiry_checkout" %>
<%@ MasterType VirtualPath="~/MasterParent.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .popupBox{background:#fff; width:80%; display:inline-block;}
    </style>
    <script src="<%= Master.rootPath + "js/jquery.popupoverlay.js" %>" type="text/javascript"></script>

    <script>
        $(document).ready(function () {
            ManageCustAddress();
        });

        function ManageCustAddress() {
            var appLink = getElementArray("a", "addrBorder");
            if (appLink.length > 0) {
                for (i = 0; i < appLink.length; i++) {
                    //appLink[i].style = "border:2px solid #ccc;";
                    appLink[i].onclick = function ()    // Assign Click Event
                    {
                        //alert("test1");
                        var strUrl = this.href.split('/'); //Get anchor href(Virtual Url)
                        var addrOrder = strUrl[strUrl.length - 1].split('-');
                        var addrId = addrOrder[0];
                        var calcId = addrOrder[1];
                        //alert("Address is : "+ addrId + "," + orderId);
                        document.getElementById("addr-" + addrId).innerHTML = "<a class=\"pAnchor\">Processing...</a>";
                        ShoppingWebService.UpdateEnqAddress(addrId, calcId, function (result) {
                            //alert("domain : " + window.location.host);
                            var navUrl;
                            if (window.location.href.indexOf("localhost") > -1) {
                                navUrl = "http://" + window.location.host + "/GenericartShopping/";
                            }
                            else {
                                //navUrl = "http://www.temp.com/";
                                navUrl = "http://" + window.location.host + "/";
                            }

                            if (result == 0) {
                                //document.getElementById("addr-" + addrId).innerHTML = "<a href=\"" + navUrl + "add-addr/" + addrId + "\" class=\"addrBorder txtDecNone\">Add To Cart</a>";
                            }
                            else {
                                //document.getElementById("addr-" + addrId).innerHTML = "<span class=\"pAdded\">Address Added</span>";
                                window.location.reload();
                            }

                            ManageCustAddress();
                        });
                        return false;
                    }
                }
            }
        }
    </script>
    

    <script>
        $(document).ready(function () {
            $('#slide').popup({
                focusdelay: 400,
                outline: false,
                vertical: 'center'
            });
            //$('#slide').popup('show');
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <span class="space40"></span>
    <div class="col_1140 bgWhite border_r_5">
        <div class="col_800">
            <div class="pad_15">
                 <!-- Information Form Starts -->
                <div id="informationForm" runat="server">
                    
                    <h2 class="pageH3 themeClrPrime mrg_B_10">Contact Information :</h2>
                    <div class="w100 mrg_B_15">
                        <div class="app_r_padding">
                            <asp:TextBox ID="txtMobile" CssClass="textBox" placeholder="Your Mobile Number" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <span class="space20"></span>
                    <h2 class="pageH3 themeClrPrime mrg_B_10">Shipping Address :</h2>
                    <div class="w100 mrg_B_15">
                        <div class="app_r_padding">
                            <span class="labelCap clrBlack">Name :*</span>
                            <asp:TextBox ID="txtName" CssClass="textBox" MaxLength="50" placeholder="Enter Your First Name" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="w50 mrg_B_15 float_left">
                        <asp:CheckBox ID="chkAddNew" runat="server" CssClass="option-input radio" TextAlign="Right" Text=" Add New Address" AutoPostBack="true" OnCheckedChanged="chkAddNew_CheckedChanged" />
                    </div>
                    <div class="float_clear"></div>
                    <div id="newAddr" runat="server">
                        <div class="w100 mrg_B_15">
                            <span class="labelCap clrBlack">Address :*</span>
                            <asp:TextBox ID="txtAddress1" CssClass="textBox" TextMode="MultiLine" Height="120" runat="server"></asp:TextBox>
                        </div>
                        <div class="w50 float_left mrg_B_15">
                            <div class="app_r_padding">
                                <span class="labelCap clrBlack">Country :*</span>
                                <asp:TextBox ID="txtCountry" CssClass="textBox" MaxLength="30" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="w50 float_left mrg_B_15">
                            <div class="app_r_padding">
                                <span class="labelCap clrBlack">State :*</span>
                                <asp:TextBox ID="txtState" CssClass="textBox" MaxLength="50"  runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="float_clear"></div>

                        <div class="w50 float_left mrg_B_15">
                            <div class="app_r_padding">
                                <span class="labelCap clrBlack">City :*</span>
                                <asp:TextBox ID="txtCity" CssClass="textBox" MaxLength="30"  runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="w50 float_left mrg_B_15">
                            <div class="app_r_padding">
                                <span class="labelCap clrBlack">Pincode :*</span>
                                <asp:TextBox ID="txtPinCode" CssClass="textBox" MaxLength="20"  runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="float_clear"></div>
                        <div class="w50 float_left mrg_B_15">
                            <div class="app_r_padding">
                                <span class="labelCap">Use this address as :*</span>
                                <asp:DropDownList ID="ddrAddrName" runat="server" CssClass="textBox">
                                    <asp:ListItem Value="0"><- Select -></asp:ListItem>
                                    <asp:ListItem Value="1">Home</asp:ListItem>
                                    <asp:ListItem Value="2">Office</asp:ListItem>
                                    <asp:ListItem Value="3">Other</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="float_clear"></div>
                    </div>

                    <div id="existingAddr" runat="server">
                        <h3 class="themeClrPrime pageH3">Select Shipping Address</h3>
                        <%= addrStr %>
                        <span class="space30"></span>
                    </div>

                    <span class="space10"></span>
                    <asp:Button ID="btnShipping" runat="server" CssClass="buttonForm upperCase letter-sp-2 fontRegular" Text="Submit Enquiry" OnClick="btnShipping_Click"  />
                </div>
                <!-- Information Form Ends -->
            </div>
        </div>
        <div class="col_340">
            <div class="pad_15">
                <img src="<%= Master.rootPath + "images/covid-sidebar.jpg" %>" class="width100" />
            </div>
        </div>
        <div class="float_clear"></div>
    </div>

    <div id="slide" class="well posRelative">
        <div class="txtCenter">
            <div class="popupBox border_r_5">
                <div class="pad_20">
                    <div role="main">
                        <img src="images/genericart-logo.png" />
                        <span class="space10"></span>
                        <p class="medium semiBold themeClrPrime">Thank you for Submitting Enquiry..!! </p>
                        <span class="space15"></span> 
                        <p class="regular semiBold">Our nearest franchisee store will contact you soon..!!</p>
                    </div>
                </div>
            </div>
            <div class="float_clear"></div>
        </div>
    </div>
</asp:Content>

