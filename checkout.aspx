<%@ Page Title="Checkout | Genericart Online Generic Medicine Shopping" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="checkout.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="checkout" %>
<%@ MasterType VirtualPath="~/MasterParent.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .lineSeperator{width:100%; height:1px; display:block; background:#e5e5e5; margin:15px 0;}
        .closeAnch{background:url(../images/icons/close.png) no-repeat center center; display:block; height:20px; width:20px; position:absolute; top:5px; right:5px  }
        .imgBox{ float:left;position:relative; width:25%; margin-top:10px; }
        .imgContainer{ height:200px !important; width:200px; overflow:hidden !important; }
        .w100{ width:100% }
        .pad-5{ padding:5px }
        .border1{ border:1px solid #ececec }
    </style>
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
                        var orderId = addrOrder[1];
                        //alert("Address is : "+ addrId + "," + orderId);
                        document.getElementById("addr-" + addrId).innerHTML = "<a class=\"pAnchor\">Processing...</a>";
                        ShoppingWebService.UpdateAddress(addrId, orderId, function (result) {
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <span class="space40"></span>
    <div class="col_1140 bgWhite border_r_5">
        
        <div id="uploadPrescription" runat="server">
            <div class="upRx">
                <div class="pad_15">
                    <span class="themeClrSec fontRegular dspBlk mrg_B_10 semiMedium">Upload Prescription</span>
                    <span class="reqPrescription">Rx</span><span class="small semiBold colrPink mrg_B_20"> Some of medicines from your cart requires prescription</span>
                    <span class="space50"></span>
                    <asp:FileUpload ID="fuPrescription" runat="server" />
                    <span class="space20"></span>
                    <div class="blueAnch dspInlineBlk mrg_R_15">
                        <asp:Button ID="btnUploadRx" runat="server" Text="Upload Prescription" CssClass="prescriButton small semiBold mrg_R_15" OnClick="btnUploadRx_Click" />
                    </div>
                    <div class="dspInlineBlk txtCenter">OR</div>
                    <div class="whatsappAnch dspInlineBlk mrg_L_15">
                        <a href="https://wa.me/919730484686?text=I%20need%20prescription%20for%20my%20medicine,%20please%20assist%20me" class="whatsappButton small semiBold txtDecNone" target="_blank" title="Click here to get prescription from Dr. Shruti">Request Prescription</a>
                    </div>
                    <span class="space20"></span>
                    <%= prescriStr %>
                    
                </div>
            </div>
        </div>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <div class="col_800">
                    <div class="pad_15">
                        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>--%>
                                <!-- Information Form Starts -->
                                <div id="informationForm" runat="server">
                                    <ul class="navTabs">
                                        <li><a href="<%= Master.rootPath + "cart" %>">Cart</a></li>
                                        <li>&raquo;</li>
                                        <li>Information</li>
                                        <li>&raquo;</li>
                                        <li><a href="<%= Master.rootPath + "checkout/payment" %>">Payment</a></li>
                                    </ul>

                                    <span class="space20"></span>


                                    <h2 class="pageH3 themeClrPrime mrg_B_10">Contact Information :</h2>
                                    <%--<div class="w100 mrg_B_15">
                                        <asp:TextBox ID="txtEmail" CssClass="textBox" placeholder="Your Email Address" runat="server"></asp:TextBox>
                                    </div>--%>
                                    <div class="w100 mrg_B_15">
                                        <asp:TextBox ID="txtMobile" CssClass="textBox" placeholder="Your Mobile Number" runat="server"></asp:TextBox>
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
                                        <%--<div class="w100 mrg_B_15">
                                            <span class="labelCap clrBlack">Address 2:</span>
                                            <asp:TextBox ID="txtAddress2" CssClass="textBox" TextMode="MultiLine" Height="120" runat="server"></asp:TextBox>
                                        </div>--%>

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
                                        <%--<div class="w50 float_left">
                                            <div class="pad_15">
                                                <div class="box-shadow bgWhite border_r_5 pad_10">
                                                    <asp:RadioButton ID="rdbHotel5Star" CssClass="option-input radio" GroupName="hotel" runat="server" Checked="true" />
                                                    <span style="display:block !important; margin-top:20px; font-size:0.9em;">Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s,</span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="w50 float_left">
                                            <div class="pad_15">
                                                <div class="box-shadow bgWhite border_r_5 pad_10">
                                                    <asp:RadioButton ID="RadioButton1" CssClass="option-input radio" GroupName="hotel" runat="server" />
                                                    <span style="display:block !important; margin-top:20px; font-size:0.9em;">Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s,</span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="float_clear"></div>--%>
                                        <span class="space30"></span>
                                    </div>

                                    <span class="space10"></span>
                                    <asp:Button ID="btnShipping" runat="server" CssClass="buttonForm upperCase letter-sp-2 fontRegular" Text="Proceed To Payment" OnClick="btnShipping_Click"  />
                                </div>
                                <!-- Information Form Ends -->
                        
                                <!-- Payment Info Starts -->
                                <div id="payment" runat="server">
                                    <ul class="navTabs">
                                        <li><a href="<%= Master.rootPath + "cart" %>">Cart</a></li>
                                        <li>&raquo;</li>
                                        <li><a href="<%= Master.rootPath + "checkout/" %>">Information</a></li>
                                        <li>&raquo;</li>
                                        <li>Payment</li>
                                    </ul>

                                    <span class="space20"></span>

                                    <div class="orderInfoBox">
                                        <div class="pad_15">
                                            <span class="semiBold clrBlack">Contact Info : </span><span><%= arrConInfo[0] %></span>
                                            <span class="lineSeperator"></span>
                                            <span class="semiBold clrBlack">Ship To : </span><span> <%= arrConInfo[1] %></span>
                                        </div>
                                    </div>

                                    <span class="space40"></span>

                                    <h2 class="pageH3 themeClrPrime mrg_B_3">Payment :</h2>
                                    <span class="fontRegular dspBlk">All transactions are secure and encrypted.</span>
                                    <span class="space10"></span>
                                    <span class="clrDarkGrey fontRegular dspBlk mrg_B_5">1. Order above Rs 500, and get free home delivery !</span>
                                    <%--<span class="clrDarkGrey fontRegular dspBlk mrg_B_5">2. COD charges are applicable for cash on delivery.</span>--%>
                                    <span class="clrDarkGrey fontRegular dspBlk mrg_B_5">2. Shipping charges are applicable for orders outside Maharashtra.</span>
                                    <span class="clrDarkGrey fontRegular dspBlk mrg_B_5">3. Shiping charges will be conveyed by our nearest pharmacy store for orders below Rs.500</span>
                                    <span class="space10"></span>
                                    <div class="orderInfoBox">
                                        <div class="pad_15">
                                            <asp:RadioButton ID="rdbCash" runat="server" TextAlign="Right" GroupName="paymentMode" Text=" COD (Cash On Delivery)" Checked="false" />
                                            <span class="lineSeperator"></span>
                                            <asp:RadioButton ID="rdbOnline" runat="server" TextAlign="Right" GroupName="paymentMode" Text=" Online Payment" Checked="true"  />
                                            <%--<span class="lineSeperator"></span>
                                            <asp:RadioButton ID="rdbDebit" runat="server" TextAlign="Right" GroupName="paymentMode" Text=" Debit Card" Enabled="false" />
                                            <span class="lineSeperator"></span>
                                            <asp:RadioButton ID="rdbNetBanking" runat="server" TextAlign="Right" GroupName="paymentMode" Text=" Net Banking" Enabled="false" />
                                            <span class="lineSeperator"></span>
                                            <asp:RadioButton ID="rdbOther" runat="server" TextAlign="Right" GroupName="paymentMode" Text=" Other Wallets" Enabled="false" />--%>
                                        </div>
                                    </div>

                                    <span class="space30"></span>

                                    <h2 class="pageH3 themeClrPrime mrg_B_3">Delivery Method :</h2>
                                    <span class="space20"></span>
                                    <div class="orderInfoBox">
                                        <div class="pad_15"> 
                                            <asp:RadioButton ID="rdbHome" runat="server" TextAlign="Right" GroupName="deliveryMode" Text=" Home Delivery" AutoPostBack="true" OnCheckedChanged="rdbHome_CheckedChanged" Checked="true" />
                                            <br /><i><span class="tiny semiBold clrGrey">Charges of Rs. 30 will be applicable on Home Delivery</span></i>
                                            <span class="lineSeperator"></span>
                                            <asp:RadioButton ID="rdbPickup" runat="server" TextAlign="Right" GroupName="deliveryMode" Text=" Self Pickup" AutoPostBack="true" OnCheckedChanged="rdbPickup_CheckedChanged" />
                                            <br /><i><span class="tiny semiBold clrGrey">If you select self pickup method, shipping charges will be free</span></i>
                                        </div>
                                    </div>

                                    <span class="space30"></span>

                                    <h2 class="pageH4 themeClrPrime">Add Note To Your Request :</h2>
                                    <i><span class="fontRegular clrGrey small">(optional)</span></i>
                                    <span class="space10"></span>
                                    <asp:TextBox ID="txtNote" runat="server" CssClass="textBox" TextMode="MultiLine" Height="120"></asp:TextBox>

                                    <span class="space30"></span>
                                    <%= errMsg %>
                                    <asp:Button ID="btnPlaceOrder" runat="server" CssClass="buttonForm upperCase letter-sp-2 fontRegular" Text="Submit Request" OnClick="btnPlaceOrder_Click"   />
                                </div>
                                <!-- Payment Info Ends -->
                            <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </div>
                </div>
                <div class="col_340">
                    <div class="pad_15">
                        <div id="paymentDetails" runat="server">
                            <span class="space40"></span>
                            <div class="orderInfoBox">
                                <div class="pad_15">
                                    <span class="semiBold clrBlack float_left">Sub Total : </span><span class="fontRegular float_right">&#8377; <%= arrConInfo[2] %></span>
                                    <div class="float_clear"></div>
                                    <span class="space10"></span>
                                    <span class="semiBold clrBlack float_left">Shipping Charges : </span><span class="fontRegular float_right">&#8377; <%= arrConInfo[3] %></span>
                                    <div class="float_clear"></div>
                                    <span class="lineSeperator"></span>
                                    <span class="semiBold clrBlack float_left">Grand Total : </span><span class="fontRegular float_right">&#8377; <%= arrConInfo[4] %></span>
                                    <div class="float_clear"></div>
                                </div>
                            </div>
                            <span class="space10"></span>
                            <span class="themeClrSec semiBold small">Free Shipping on Requests Above &#8377; 500</span>
                        </div>

                        <span class="space20"></span>
                        <img src="<%= Master.rootPath + "images/covid-sidebar.jpg" %>" class="width100" />
                    </div>
                </div>
                <div class="float_clear"></div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

