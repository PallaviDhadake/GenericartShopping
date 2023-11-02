<%@ Page Title="Generic Products | Genericart Online Generic Medicine Shopping" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="products.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="products" %>
<%@ MasterType VirtualPath="~/MasterParent.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="<%= Master.rootPath + "js/multislider.js" %>"></script>
    <link href="<%= Master.rootPath + "css/lightslider.css" %>" rel="stylesheet" />
    <script src="<%= Master.rootPath + "js/lightslider.js" %>"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#image-gallery').lightSlider({
                gallery: true,
                item: 1,
                thumbItem: 4,
                slideMargin: 0,
                speed: 500,
                auto: false,
                loop: false,
                onSliderLoad: function () {
                    $('#image-gallery').removeClass('cS-hidden');
                }
            });
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            ManageCartEvent();
        });

        function ManageCartEvent() {
            var appLink = getElementArray("a", "cartProd");
            if (appLink.length > 0) {
                for (i = 0; i < appLink.length; i++) {
                    appLink[i].onclick = function ()    // Assign Click Event
                    {
                        //alert("test1");
                        var strUrl = this.href.split('/'); //Get anchor href(Virtual Url)
                        var prodId = strUrl[strUrl.length - 1];
                        var qty = document.getElementById("<%= ddrQty.ClientID %>").value;

                        var optDiv = document.getElementById("<%= prodOption.ClientID %>");
                        var optId = 0;
                        if (optDiv != null) {
                            
                            optId = document.getElementById("<%= txtOptionId.ClientID %>").value;
                            if (optId == 0) {
                                TostTrigger('warning', 'Select Option');
                                return false;
                            }
                        }
                        else {
                            optId = 0;
                        }

                        document.getElementById("cartAnch-" + prodId).innerHTML = "<a class=\"pAnchor\">Processing...</a>";

                        ShoppingWebService.UpdateCartList(prodId, qty, optId, function (result) {
                            var navUrl;
                            if (window.location.href.indexOf("localhost") > -1) {
                                navUrl = "http://" + window.location.host + "/GenericartShopping/";
                            }
                            else {
                                //navUrl = "http://www.temp.com/";
                                navUrl = "http://" + window.location.host + "/";
                            }

                            if (result == 0) {
                                document.getElementById("cartAnch-" + prodId).innerHTML = "<a href=\"" + navUrl + "add-to-cart/" + prodId + "\" class=\"cartProd blueAnch semiBold upperCase letter-sp-2 dspInlineBlk\">Add Request</a>";
                            }
                            else {
                                document.getElementById("cartAnch-" + prodId).innerHTML = "<span class=\"pAdded\">Added To Request</span>";
                                TostTrigger('success', 'Product Added To Request..!!');
                                waitAndMove("<%= Master.rootPath + "cart" %>", 2000);
                            }

                            ManageCartEvent();

                            ShoppingWebService.CartListCount(function (result) {
                                document.getElementById("cartCount").innerHTML = result;
                                document.getElementById("mobCartCount").innerHTML = result;
                            });
                        });
                        return false;
                    }
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Product Details Starts -->
    <span class="space50"></span>
    <div class="col_1140 bgWhite border_r_5 box-shadow" id="prodDetails">
        <%= errMsg %>
        <div class="col_1_2">
            <div class="pad_15">
                <span class="space20"></span>
                <%= arrProdInfo[0] %>
                <%--<img src="<%= Master.rootPath + "upload/products/product-photo-1.jpg" %>" alt="" class="width100" />--%>
                <span class="space20"></span>
            </div>
        </div>
        <div class="col_1_2">
            <div class="pad_15">
                <span class="space20"></span>
                <%--<h1 class="pageH2 clrLightBlack semiBold mrg_B_5">Crocin Advance 650mg</h1>
                <p class="clrGrey semiBold mrg_B_10">Strip of 15 tablets.</p> 
                <span class="colrPink semiBold small"><span class="reqPrescription">Rx</span> Prescription Required</span>
                <span class="space10"></span>
                <p class="clrGrey fontRegular small line-ht-5">Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text</p>
                <span class="greyLine"></span>
                <span class="semiBold dispBlk mrg_B_10">Manufacturer : LIFE VISION</span>
                <span class="fontRegular colorGreen semiMedium dispBlk mrg_B_5">In Stock</span>
                <span class="fontRegular colrPink semiMedium dispBlk mrg_B_5">Currently Unavailable</span>
                <span class="themeClrSec fontRegular semiMedium dispBlk mrg_B_10">Product Code : CR650</span>
                <span class="clrGrey fontRegular">MRP : <span class="clrGrey strike fontRegular mrg_R_5">&#8377; 90.7 </span> (Inclusive of all taxes)</span>
                <span class="space5"></span>
                <span class="themeClrPrime large semiBold">&#8377; 25.7</span>
                <span class="space10"></span>
                <span class="prod-discount large">60% Off</span>--%>
                <%= arrProdInfo[1] %>
                <span class="space20"></span>
                <span class="semiBold clrBlack dispBlk mrg_B_10">Quantity :</span>
                <asp:DropDownList ID="ddrQty" runat="server" CssClass="htmlcmbBox w25">
                    <asp:ListItem Value="0"><- Select -></asp:ListItem>
                </asp:DropDownList>
                <div id="prodOption" runat="server" visible="false">
                    <span class="space20"></span>
                    <span class="semiBold clrBlack dispBlk mrg_B_10"><%= arrProdInfo[5] %></span>
                    <%--<asp:DropDownList ID="ddrOption" runat="server" CssClass="htmlcmbBox w25" AutoPostBack="true" OnSelectedIndexChanged="ddrOption_SelectedIndexChanged">
                        <asp:ListItem Value="0"><- Select -></asp:ListItem>
                    </asp:DropDownList>--%>
                    <%= arrProdInfo[4] %>
                    <%--<asp:TextBox ID="txtOptionId" runat="server" CssClass="textBox" Enabled="false" style="display:none"></asp:TextBox>--%>
                    <asp:TextBox ID="txtOptionId" runat="server" CssClass="textBox" Enabled="false" style="display:none"></asp:TextBox>
                    <asp:TextBox ID="txtBasePrice" runat="server" CssClass="textBox" Enabled="false" style="display:none"></asp:TextBox>
                    <asp:TextBox ID="txtOrigPrice" runat="server" CssClass="textBox" Enabled="false" style="display:none"></asp:TextBox>
                    <asp:TextBox ID="txtIncrPrice" runat="server" CssClass="textBox" Enabled="false" style="display:none"></asp:TextBox>
                </div>
                <span class="space20"></span>
                <%= prodAvailable %>
                <%--<a href="#" class="pinkAnch semiBold upperCase letter-sp-2 dspInlineBlk mrg_R_15">Buy Now</a>--%>
                <%--<a href="#" class="blueAnch semiBold upperCase letter-sp-2 dspInlineBlk">Add To Cart</a>--%>
                <%= arrProdInfo[3] %>
                <span class="space20"></span>
            </div>
        </div>
        <div class="float_clear"></div>
    </div>
    <span class="space15"></span>
    <div class="col_1140 bgWhite border_r_5 box-shadow" id="cart1">
        <div class="pad_15">
            <h2 class="pageH3 clrLightBlack semiBold">Product Details</h2>
            <span class="lineSeperator"></span>
            <%--<p class="clrGrey line-ht-5">Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>
            <span class="space15"></span>
            <ul class="basicList">
                <li>description</li>
                <li>description</li>
                <li>description</li>
                <li>description</li>
                <li>description</li>
            </ul>--%>
            <%= arrProdInfo[2] %>
        </div>
    </div>
    <div class="col_1280" >
        <%= arrProdInfo[6] %>
    </div>
    <span class="space50"></span>
    <!-- Product Details Ends -->

    <script type="text/javascript">
        function GetOption(optId, prId) {
            PageMethods.set_path('<%=ResolveUrl("products.aspx") %>');
            
            //alert("onclick called" + optId);
            PageMethods.GetOptionId(optId, prId, onSucess, onError);
            //alert("page method called");
            function onSucess(result) {
                //alert(result);
                document.getElementById("<%= txtIncrPrice.ClientID %>").value = result;
                var basePrice = document.getElementById("<%= txtBasePrice.ClientID %>").value;
                var origPrice = document.getElementById("<%= txtOrigPrice.ClientID %>").value;
                var incrPrice = document.getElementById("<%= txtIncrPrice.ClientID %>").value;
                var finalPrice = parseFloat(origPrice) + parseFloat(incrPrice);
                document.getElementById("pPrice").innerHTML = "<span class=\"themeClrPrime semiBold\" id=\"pPrice\">&#8377; " + finalPrice.toFixed(2) + "</span>";
                var temp = parseFloat(finalPrice) * 100 /  parseFloat(basePrice);
                var prodDisc = (100 - parseFloat(temp));
                document.getElementById("pDis").innerHTML = "<span class=\"prod-discount\" id=\"pDis\">" + prodDisc.toFixed(2) + "% Off</span>";
                $("#tree ul li").removeClass("act");
                $("#" + optId).closest('li').addClass("act");
                //waitAndMove(window.location, 1000);
            }

            function onError(result) {
                alert(result.get_message());
            }

            document.getElementById('<%= txtOptionId.ClientID %>').value = optId;
        }
    </script>

    <script>
        $('#prodSlider').multislider({
            duration: 750,
            interval: false
        });
    </script>
</asp:Content>

