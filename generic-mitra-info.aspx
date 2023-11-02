<%@ Page Language="C#" AutoEventWireup="true" CodeFile="generic-mitra-info.aspx.cs" Inherits="generic_mitra_info" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="IE=edge" http-equiv="X-UA-Compatible" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
    <link rel="shortcut icon" href="/favicon.ico" type="image/x-icon" />

    <title>Generic Mitra</title>

    <meta name="description" content="" />

    <link href="https://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700" rel="stylesheet" />

    <link href="css/shoppingGencart.css" rel="stylesheet" />
    <script src="js/jquery-2.2.4.min.js"></script>
</head>
<body>
    <div class="absTopArrow">
        <a href="#gMitra"><div class="topArrowIco"></div></a>
    </div>
    <form id="form1" runat="server">
        <!-- Header Starts -->
        <div id="gMitra" class="width100">
            <div class="pad_TB_10">
                <div class="col_800_center posRelative">
                    <div class="logo">
                        <a href="<%= rootPath + "generic-mitra-info" %>" title="Genericart Online Generic Medicine Shopping" class="txtDecNone">
                            <img src="images/genmitra-logo.png" alt="Genericart Generic Mitra" />
                        </a>
                    </div>
                    <div class="desktopInfo1">
                        <span class="space20"></span>
                        <span class="small clrBlack fontRegular dispBlk">Toll Free Number</span>
                        <a href="tel:9090308585" class="themeClrPrime txtDecNone semiBold small">9090308585</a>
                    </div>
                    <div class="float_clear"></div>

                    <div class="navBtnContainer">
                        <a id="navBtn" onclick="openNav()"></a>
                    </div>

                    <!-- Navigation Starts -->
                    <div id="mySideNav">
                        <a href="javascript:void(0)" class="closeBtnnew" onclick="closeNav()">&times;</a>
                        <ul id="sideNav" style="position: relative;">
                            <li><a href="<%= rootPath + "generic-mitra-info" %>">Home</a></li>
                            <li><a href="<%= rootPath + "generic-mitra-info#gAbout" %>">About Us</a></li>
                            <li><a href="<%= rootPath + "generic-mitra-info#gJoin" %>">How To Join</a></li>
                            <li><a href="<%= rootPath + "generic-mitra-info#gRole" %>">Roles &amp; Responsibilities</a></li>
                            <li><a href="<%= rootPath + "generic-mitra-info#gEarn" %>">How To Earn</a></li>
                            <li><a href="<%= rootPath + "genericmitra" %>" target="_blank">Generic Mitra Login</a></li>
                        </ul>
                        <div class="float_clear"></div>
                        <div id="mobNav">
                            <span class="space10"></span>
                            <div class="pad_30">
                                <a href="https://play.google.com/store/apps/details?id=com.autoqed.genericartmedicine" target="_blank" class="mrg_R_5" title="Download our app from Google Playstore">
                                    <img src="images/icons/android.png" class="android" /></a>
                                <a href="#" class="tooltip" title="Download On App Store"><span class="tooltiptext">Coming Soon !</span><img src="images/icons/ios.png" class="ios" /></a>
                                <span class="space20"></span>
                                <span class="tiny upperCase clrWhite letter-sp-3 mrg_B_15">Phone:</span>
                                <a href="tel:1800120061313" class="semiMedium clrWhite light txtDecNone">1800 1200 61313</a>
                                <span class="space30"></span>
                                <span class="tiny upperCase clrWhite letter-sp-3 mrg_B_15">Email:</span>
                                <a href="mailto:enquirygenericart@gmail.com" class="clrWhite breakWord txtDecNone">enquirygenericart&#64;gmail&#46;com</a>
                            </div>
                        </div>
                    </div>
                    <!-- Navigation Ends -->
                </div>
            </div>
        </div>
        <!-- Header Ends -->
        <!-- Banner Starts -->
        <span class="space10"></span>
        <div class="">
            <img src="images/gen-mitra-banner.jpg" alt="" class="width100 desktopImg" />
            <img src="images/gen-mitra-banner-mob.jpg" alt="" class="width100 mobImg" />
        </div>
        <!-- Banner Ends -->
        <!-- Info Starts -->
        <span class="space20"></span>
        <div class="col_800_center" id="genMitra">
            <div id="gAbout">
                <div class="pad_10">
                    <h1 class="semiMedium semiBold txtCenter line-ht-5 mrg_B_10">Genericart Medicine bringing you an opportunity to join hands with us as <span class="themeClrSec semiBold">Generic Mitra</span>.</h1>
                    <h2 class="semiMedium txtCenter mrg_B_20"><i class="fontRegular">Anyone, Anywhere, Anytime can Work as Generic Mitra</i></h2>
                    <div class="txtCenter"><a href="<%= rootPath + "register-genmitra" %>" target="_blank" class="readAnchBlue semiBold upperCase letter-sp-2">Join Now !</a></div>
                </div>

                <span class="space20"></span>

                <div class="width50">
                    <div class="txtCenter">
                        <span class="space30"></span>
                        <img src="images/gen-mitra-man.png" alt="" class="" />
                    </div>
                </div>
                <div class="width50">
                    <div class="pad_10">
                        <h3 class="regular semiBold mrg_B_10">You Must Have :</h3>
                        <ul class="checkList">
                            <li>Good Social Network and Interested to participate in social activities</li>
                            <li>Love talking to people and sharing information</li>
                            <li>Interested to earn with social cause</li>
                        </ul>

                        <span class="space10"></span>

                        <h3 class="regular semiBold mrg_B_10">Eligibility :</h3>
                        <ul class="starList">
                            <li>No Educational Qualification</li>
                            <li>No Age Limit</li>
                            <li>No time restrictions</li>
                        </ul>

                        <span class="space10"></span>

                        <h3 class="regular semiBold mrg_B_10">Who Can Join :</h3>
                        <ul class="starList">
                            <li>Housewife</li>
                            <li>Students</li>
                            <li>Retired Person</li>
                            <li>Professionals</li>
                        </ul>
                    </div>
                </div>
                <div class="float_clear"></div>
            </div>

            <span class="space20"></span>

            <div id="gJoin">
                <div class="width50Right">
                    <div class="txtCenter">
                        <img src="images/gen-mitra-girl.png" alt="" class="" />
                    </div>
                </div>
                <div class="width50Right">
                    <div class="pad_10">
                        <span class="space30"></span>
                        <h3 class="semiMedium semiBold mrg_B_10">Process to Join :</h3>
                        <ul class="processList">
                            <li>Visit our nearest Genericart Medicine Store, and enroll yourself as Generic Mitra </li>
                            <div class="txtCenter mrg_B_10">OR</div>
                            <li><a href="<%= rootPath + "register-genmitra" %>" target="_blank" class="readMore1">Click Here</a> to complete enrollment process</li>
                            <li>Submit Id Proof and Bank details </li>
                            <li>After Verification you will receive a Generic Mitra Code with Generic Mitra App link</li>
                            <li>Start working as Generic Mitra !</li>
                            <li>Happy Working !!</li>
                        </ul>
                    </div>
                </div>
                <div class="float_clear"></div>
            </div>

            <span class="space20"></span>

            <div id="gRole">
                <div class="width50">
                    <div class="txtCenter">
                        <img src="images/gen-mitra-responsibility.png" alt="" class="width100" />
                    </div>
                </div>
                <div class="width50">
                    <div class="pad_10">
                        <span class="space20"></span>
                        <h3 class="semiMedium semiBold mrg_B_10">Roles and Responsibilities of Generic Mitra  :</h3>
                        <ul class="starList">
                            <li>Search people in your circle who are in need of monthly medicine </li>
                            <li>Help them to place order with nearest Genericart Pharmacy Store Or  Help them order online through Genericart Medicine App </li>
                            <li>Connect them to our customer support team if more assistances is needed</li>
                            <li>Keep sharing information about new medicine arrivals</li>
                            <li>Keep sharing reminders for placing orders on monthly basis</li>
                        </ul>
                    </div>
                </div>
                <div class="float_clear"></div>
            </div>

            <span class="space20"></span>

            <div id="gEarn">
                <div class="width50Right">
                    <div class="txtCenter">
                        <img src="images/gen-mitra-how-to-earn.png" alt="" class="width100" />
                    </div>
                </div>
                <div class="width50Right">
                    <div class="pad_10">
                        <span class="space30"></span>
                        <h3 class="semiMedium semiBold mrg_B_10">How You Earn  :</h3>
                        <ul class="checkList">
                            <li>On every successfully delivered order you are eligible to get 5% referral benefit on order value.</li>
                            <li>This is a recurring commission which you will get on every order whenever the customer orders the medicine. </li>
                            <li>The referral benefit will be given to you as long as your referred customer keeps ordering with us</li>
                        </ul>
                    </div>
                </div>
                <div class="float_clear"></div>
            </div>
            <span class="space20"></span>
            <div class="txtCenter"><a href="<%= rootPath + "register-genmitra" %>" target="_blank" class="readAnchBlue semiBold upperCase letter-sp-2">Join Now !</a></div>
        </div>
        <span class="space20"></span>
        <!-- Info Ends -->
        <!-- Footer Starts -->
        <div class="footer">
            <div class="copyRight" id="copyright">
                <span class="space20"></span>
                <div class="col_1140">
                    <div class="col_1_3">
                        <div class="pad_10">
                            <span class="footerText">Copyrights © <%= currentYear %> All Rights Reserved, Genericart Medicines.</span>
                        </div>
                    </div>
                    <div class="col_1_3 txtCenter">
                        <div class="pad_10">
                            <%--<span class="footerText">Website By <a href="http://www.autoqed.com/" target="_blank" class="intellect" title="Website Design and Development Service By AutoQed">AutoQed</a></span>--%>
                        </div>
                    </div>
                    <div class="col_1_3 txtCenter">
                        <div class="pad_10">
                            <a href="https://www.facebook.com/genericartmedicine/" target="_blank" class="foo_fb socialIco" title="Follow us on Facebook"></a>
                            <a href="https://twitter.com/GenericartP" target="_blank" class="foo_twt socialIco" title="Follow us on Twitter"></a>
                            <a href="https://instagram.com/genericart_medicine_official?igshid=1gsdya7xv43h" target="_blank" class="foo_insta socialIco" title="Follow us on Instagram"></a>
                            <a href="#" target="_blank" class="foo_linkedin socialIco" title="Follow us on Youtube"></a>
                        </div>
                    </div>
                    <div class="float_clear"></div>
                </div>
                <span class="space20"></span>
            </div>
        </div>
        <!-- Footer Ends -->
    </form>

    <script>
        function openNav() {
            document.getElementById("mySideNav").style.width = "320px";
            document.getElementById("navBtn").style.zIndex = "0";
        }

        function closeNav() {
            document.getElementById("mySideNav").style.width = "0";
            document.getElementById("navBtn").style.zIndex = "5";
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
    <script type="text/javascript">
        $(window).scroll(function () {
            var topIcon = $('.topArrowIco'),
                scroll = $(window).scrollTop();
            //alert("Msg");
            if (scroll >= 250) topIcon.addClass('fixedArrow');

            else topIcon.removeClass('fixedArrow');
            //alert("Msg");
        });

    </script>
</body>
</html>
