<%@ Page Title="My Orders | Genericart Online Generic Medicine Shopping" Language="C#" MasterPageFile="~/customer/MasterCustomer.master" AutoEventWireup="true" CodeFile="my-orders.aspx.cs" Inherits="customer_my_orders" %>
<%@ MasterType VirtualPath="~/customer/MasterCustomer.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .cancel-links{text-decoration:none;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      
        <!-- My Orders Starts -->
        <div class="width50" id="orders">
            <div class="pad_10">
                <div class="bgWhite border_r_5 box-shadow">
                    <div class="pad_20">
                        <h2 class="clrLightBlack semiBold semiMedium">My Medicine Request</h2>
                        <span class="space20"></span>
                        
                       

                        <div class="proNavPanel">
                            <ul class="proNav">
                                <li><a id="trOngoing" href="javascript:toggleBox('Ongoing', 'trOngoing');">Ongoing</a></li>
                                <li><a id="trCompleted" href="javascript:toggleBox('Completed', 'trCompleted');">Completed</a></li>
                            </ul>
                        </div>
                        <span class="space20"></span>

                        <div id="Ongoing">
                            <%= orderStr %>
                            <%--<h3 class="clrLightBlack semiBold mrg_B_20">Not Yet Approved</h3>

                            <div class="bgWhite border_r_5 box-shadow posRelative">
                                <div class="pad_15">
                                    <div class="absCircle">
                                        <div class="pinkCircle"></div>
                                        <div class="float_clear"></div>
                                        <span class="tiny fontRegular clrDarkGrey">Pending</span>
                                    </div>
                                    <a href="order-details.html" class="orderAnch semiBold">Order Number : bbdfa656fsabkj</a>
                                    <span class="lineSeperator"></span>

                                    <div class="float_left width70">
                                        <span class="clrGrey fontRegular tiny dispBlk mrg_B_3">Order Date : 10/10/2020</span>
                                        <span class="clrGrey fontRegular tiny dispBlk">contains Crocin, dettol...</span>
                                    </div>
                                    <div class="float_right">
                                        <span class="tiny clrGrey fontRegular">Amt.</span>
                                        <span class="regular clrLightBlack semiBold">&#8377; 359</span>
                                    </div>
                                    <div class="float_clear"></div>

                                    <span class="space20"></span>

                                    <a href="#" class="blueAnch dspInlineBlk semiBold small mrg_R_5">Contact Us</a>
                                    <a href="#" class="pinkAnch dspInlineBlk semiBold small">Cancel Order</a>
                                </div>
                            </div>--%>
                        </div>

                        <div id="Completed">
                            <%= completeOrderStr %>
                            <%--<div class="bgWhite border_r_5 box-shadow posRelative">
                                <div class="pad_15">
                                    <div class="absCircle">
                                        <div class="greenCircle"></div>
                                    </div>
                                    <a href="order-details.html" class="orderAnch semiBold">Order Number : bbdfa656fsabkj</a>
                                    <span class="lineSeperator"></span>

                                    <div class="float_left width70">
                                        <span class="clrGrey fontRegular tiny dispBlk mrg_B_3">Order Date : 10/10/2020</span>
                                        <span class="clrGrey fontRegular tiny dispBlk">contains Crocin, dettol...</span>
                                    </div>
                                    <div class="float_right">
                                        <span class="tiny clrGrey fontRegular">Amt.</span>
                                        <span class="regular clrLightBlack semiBold">&#8377; 359</span>
                                    </div>
                                    <div class="float_clear"></div>

                                    <span class="space20"></span>

                                    <a href="#" class="blueAnch dspInlineBlk semiBold small mrg_R_5">Track Order</a>
                                    <a href="#" class="pinkAnch dspInlineBlk semiBold small">Cancel Order</a>
                                </div>
                            </div>--%>
                            
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- My Orders Ends -->


    <script type="text/javascript">
        $(document).ready(function () {
            $("a.cancel-links").fancybox({
                type: 'iframe'

            });
        });
    </script>


    <script type="text/javascript">
            $(function () {
                toggleBox('Ongoing', 'trOngoing');
            });

            function toggleBox(divId, switchId) {
                document.getElementById("Ongoing").style.display = "none";
                document.getElementById("Completed").style.display = "none";
                document.getElementById("trOngoing").className = "";
                document.getElementById("trCompleted").className = "";
                $("#" + divId).fadeIn("5000");
                document.getElementById(switchId).className = "act";
            }
        </script>

   

  

  
   
</asp:Content>

