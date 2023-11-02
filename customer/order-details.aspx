<%@ Page Title="My Orders" Language="C#" MasterPageFile="~/customer/MasterCustomer.master" AutoEventWireup="true" CodeFile="order-details.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="customer_order_details" %>
<%@ MasterType VirtualPath="~/customer/MasterCustomer.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .closeAnch{background:url(../images/icons/close.png) no-repeat center center; display:block; height:20px; width:20px; position:absolute; top:5px; right:5px  }
        .imgBox{ float:left;position:relative; width:50%; margin-top:10px; }
        .imgContainer{ height:200px !important; width:200px; overflow:hidden !important; }
        .w100{ width:100% }
        .pad-5{ padding:5px }
        .border1{ border:1px solid #ececec }
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Order Details Starts -->
        <div class="width50" id="orders">
            <div class="pad_10">
                <div class="bgWhite border_r_5 box-shadow">
                    <div class="pad_20 posRelative">
                        <a href="#rateStar" class="absRate">Give Us Rating</a>
                        <h2 class="clrLightBlack semiBold semiMedium">Medicine Request Details</h2>
                        <span class="space20"></span>
                        <div id="uploadRx" runat="server" visible="false">

                        </div>
                        <%= ordStr %>
                        <%--<h2 class="clrLightBlack semiBold">Order Number : bbdfa656fsabkj</h2>
                        <span class="lineSeperator"></span>

                        <div class="float_left width70">
                            <span class="clrGrey fontRegular tiny dispBlk mrg_B_3">Order Date : 10/10/2020</span>
                            <span class="clrGrey fontRegular tiny dispBlk">contains Crocin, dettol. <br />Lorem Ipsum is simply dummy text of the printing and typesetting industry.</span>
                        </div>
                        <div class="float_right">
                            <span class="tiny clrGrey fontRegular">Amt.</span>
                            <span class="regular clrLightBlack semiBold">&#8377; 359</span>
                        </div>
                        <div class="float_clear"></div>

                        <span class="space20"></span>

                        <ul class="timeline">
                            <li class="active">
                                <span>12 Oct, 2020</span>
                                <p>We have received your order, as soon as have packed your package, we will send it off for delivery.</p>
                            </li>
                            <li class="active">
                                <span>13 Oct, 2020</span>
                                <p>Your package has been dispatched.</p>
                            </li>
                            <li>
                                <span>15 Oct, 2020</span>
                                <p>Your package has been delivered.</p>
                            </li>
                        </ul>

                        <span class="lineSeperator"></span>

                        <h3 class="clrLightBlack semiBold mrg_B_10 regular">Delivery Address</h3>
                        <div class="float_left width70">
                            <span class="clrGrey fontRegular small dispBlk">P-160, ST/8, Lake Town, Block A, Lake Town, Kolkata 700089</span>
                        </div>
                        <div class="float_right">
                            <a href="#" class="bookAnch semiBold" style="font-size:0.9em;">Change</a>
                        </div>
                        <div class="float_clear"></div>

                        <span class="lineSeperator"></span>--%>

                        <h3 class="clrLightBlack semiBold regular" id="rateStar">Don't forget to Rate</h3>
                        <span class="clrGrey small fontRegular">For further improvements</span>
                        <span class="space10"></span>
                        <div class="rate">
                            <input type="radio" id="star5" name="rate" value="5" onclick="GetRating(this);"/>
                            <label for="star5" title="5 stars">5 stars</label>
                            <input type="radio" id="star4" name="rate" value="4" onclick="GetRating(this);" />
                            <label for="star4" title="4 stars">4 stars</label>
                            <input type="radio" id="star3" name="rate" value="3" onclick="GetRating(this);" />
                            <label for="star3" title="3 stars">3 stars</label>
                            <input type="radio" id="star2" name="rate" value="2" onclick="GetRating(this);" />
                            <label for="star2" title="2 stars">2 stars</label>
                            <input type="radio" id="star1" name="rate" value="1" onclick="GetRating(this);" />
                            <label for="star1" title="1 star">1 star</label>
                        </div>
                        <div class="float_clear"></div>
                        <asp:TextBox ID="txtRating" runat="server" CssClass="textBox" Enabled="false" style="display:none;"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <!-- Order Details Ends -->

    <script type="text/javascript">
        function GetRating(rating) {
            PageMethods.set_path('/customer/order-details.aspx');
            //alert("onclick called" + rating.value);
            PageMethods.GetOrderRating(rating.value, onSucess, onError);
            //alert("page method called");
            function onSucess(result) {
                //alert(result);
                TostTrigger('success', 'Thank you for Rating..!!');
                //window.location = ("" + window.location);
                waitAndMove(window.location, 1000);
            }

            function onError(result) {
                alert(result.get_message());
            }

            
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            var ratingVal = document.getElementById("<%= txtRating.ClientID %>").value;
            setRating(ratingVal);
        });

        function setRating(val) {
            //alert("setRating called" + val);
            switch (val) {
                case "1": document.getElementById("star1").checked = true; break;
                case "2": document.getElementById("star2").checked = true; break;
                case "3": document.getElementById("star3").checked = true; break;
                case "4": document.getElementById("star4").checked = true; break;
                case "5": document.getElementById("star5").checked = true; break;
            }
            
        }
    </script>
    <script type="text/javascript">
        // Select all links with hashes
        $('a[href*="#"]')
          // Remove links that don't actually link to anything
          .not('[href="#"]')
          .not('[href="#0"]')
          .click(function (event) {
              // On-page links
              if (
                location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '')
                &&
                location.hostname == this.hostname
              ) {
                  // Figure out element to scroll to
                  var target = $(this.hash);
                  target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
                  // Does a scroll target exist?
                  if (target.length) {
                      // Only prevent default if animation is actually gonna happen
                      event.preventDefault();
                      $('html, body').animate({
                          scrollTop: target.offset().top
                      }, 1000, function () {
                          // Callback after animation
                          // Must change focus!
                          var $target = $(target);
                          //$target.focus();
                          if ($target.is(":focus")) { // Checking if the target was focused
                              return false;
                          } else {
                              $target.attr('tabindex', '-1'); // Adding tabindex for elements not focusable
                              //$target.focus(); // Set focus again
                          };
                      });
                  }
              }
          });

    </script>
</asp:Content>

