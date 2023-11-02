<%@ Page Title="Customer Address" Language="C#" MasterPageFile="~/customer/MasterCustomer.master" AutoEventWireup="true" CodeFile="user-address.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="customer_user_address" %>
<%@ MasterType VirtualPath="~/customer/MasterCustomer.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript"
            src="https://maps.googleapis.com/maps/api/js?key=AIzaSyCvO0AHfg1cuND1KXbw3t5xZr5p4PVrEk4">
    </script>
    <script type="text/javascript">
        function initialize() {
            //alert(test);
            var myLatlng = new google.maps.LatLng(<%= latlongs %>);

            var mapOptions = {
                center: myLatlng,
                zoom: 17, scrollwheel: false, draggable: true,
            };

            var map = new google.maps.Map(document.getElementById("map-canvas"), mapOptions);
            var genericart = {
                path: 'M95.35,50.645c0,13.98-11.389,25.322-25.438,25.322c-14.051,0-25.438-11.342-25.438-25.322   c0-13.984,11.389-25.322,25.438-25.322C83.964,25.322,95.35,36.66,95.35,50.645 M121.743,50.645C121.743,22.674,98.966,0,70.866,0   C42.768,0,19.989,22.674,19.989,50.645c0,12.298,4.408,23.574,11.733,32.345l39.188,56.283l39.761-57.104   c1.428-1.779,2.736-3.654,3.916-5.625l0.402-0.574h-0.066C119.253,68.516,121.743,59.874,121.743,50.645',
                fillColor: '#209ae3',
                fillOpacity: 1,
                scale: 0.3
            };
            var marker = new google.maps.Marker({
                position: myLatlng,
                icon: genericart,
                map: map,
                title: "My Address",
                animation: google.maps.Animation.DROP
            });
            //alert("test");
            marker.addListener('click', toggleBounce);
            function toggleBounce() {
                if (marker.getAnimation() !== null) {
                    marker.setAnimation(null);
                } else {
                    marker.setAnimation(google.maps.Animation.BOUNCE);
                }
            }
        }
        google.maps.event.addDomListener(window, 'load', initialize);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <!-- Address Starts -->
        <div class="width50">
            <div class="pad_10">
                <div class="bgWhite border_r_5 box-shadow">
                    <div class="pad_20">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <h2 class="clrLightBlack semiBold semiMedium mrg_B_10">Address</h2>
                                <%= errMsg %>
                                <%--<div class="bgWhite border_r_5 box-shadow">
                                    <div id="map-canvas"></div>
                                    <div class="pad_20">
                                        <div class="float_left width80">
                                            <span class="home dispBlk mrg_B_10">Home</span>
                                            <span class="fontRegular tiny clrGrey dispBlk"><%= address %></span>
                                        </div>
                                        <div class="float_right">
                                            <span class="space10"></span>
                                            <span id="btnChange" class="bookAnch semiBold" style="font-size:1em; cursor:pointer;">Change</span>
                                        </div>
                                        <div class="float_clear"></div>
                                    </div>
                                </div>
                                <span class="space20"></span>--%>
                                <%= address %>

                                <span id="add"></span>
                                <%--<span id="btnAdd" class="dottedBtn small upperCase semiBold">Add New Address</span>--%>
                                <%--<asp:Button ID="btnAdd" runat="server" Text="Add New Address" CssClass="dottedBtn small upperCase semiBold w100" OnClick="btnAdd_Click"  />--%>
                                <a href="<%= Master.rootPath+"user-address?type=new#add" %>" class="dottedBtn small upperCase semiBold">Add New Address</a>
                                
                                <div id="addressForm" runat="server">
                                    <span class="space20"></span>
                                    <div class="w100 mrg_B_15">
                                        <div class="app_r_padding">
                                            <span class="labelCap">Address :*</span>
                                            <asp:TextBox ID="txtAddress" runat="server" CssClass="textBox w95" TextMode="MultiLine" Height="80"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="w50 float_left mrg_B_15">
                                        <div class="app_r_padding">
                                            <span class="labelCap">Country :*</span>
                                            <asp:TextBox ID="txtCountry" runat="server" CssClass="textBox w95" MaxLength="30"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="w50 float_left mrg_B_15">
                                        <div class="app_r_padding">
                                            <span class="labelCap">State :*</span>
                                            <asp:TextBox ID="txtState" runat="server" CssClass="textBox w95" MaxLength="50"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="float_clear"></div>
                                    <div class="w50 float_left mrg_B_15">
                                        <div class="app_r_padding">
                                            <span class="labelCap">City :*</span>
                                            <asp:TextBox ID="txtCity" runat="server" CssClass="textBox w95" MaxLength="50"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="w50 float_left mrg_B_15">
                                        <div class="app_r_padding">
                                            <span class="labelCap">Zip Code :*</span>
                                            <asp:TextBox ID="txtZipcode" runat="server" CssClass="textBox w95" MaxLength="15"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="float_clear"></div>
                                    <div class="w50 float_left mrg_B_15">
                                        <div class="app_r_padding">
                                            <span class="labelCap">Latitude :</span>
                                            <asp:TextBox ID="txtLatitude" runat="server" CssClass="textBox w95" MaxLength="50"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="w50 float_left mrg_B_15">
                                        <div class="app_r_padding">
                                            <span class="labelCap">Longitude :</span>
                                            <asp:TextBox ID="txtLongitude" runat="server" CssClass="textBox w95" MaxLength="50"></asp:TextBox>
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
                                    <%--<div class="w100 mrg_B_15">
                                        <div class="app_r_padding">
                                            <span class="labelCap">Google Map Latlongs :*</span>

                                            <input type="text" id="" class="textBox w95" placeholder="for example, 16.8305743,74.6409581" />
                                        </div>
                                    </div>--%>
                                    <asp:Button ID="btnSave" runat="server" Text="Update Address" CssClass="blueAnch small semiBold dspInlineBlk" OnClick="btnSave_Click" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
        <!-- Address Ends -->
    
    <script type="text/javascript">
        //document.getElementById('<%= addressForm.ClientID %>').style.display = "none";

        <%--var btn = document.getElementById('btnAdd');
        var btnchange = document.getElementById('btnChange');
        btn.onclick = function () {
            document.getElementById('<%= addressForm.ClientID %>').style.display = "block";
            
        }
        btnchange.onclick = function () {
            document.getElementById('<%= addressForm.ClientID %>').style.display = "block";
        }--%>
    </script>
</asp:Content>

