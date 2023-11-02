<%@ Page Title="Lab Tests" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="lab-test.aspx.cs" Inherits="lab_test" %>
<%@ MasterType VirtualPath="~/MasterParent.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Lab Test Starts -->
    <span class="space50"></span>
    <div class="col_1140" id="consult">
        <div class="bgWhite border_r_5">
            <div class="col_1_2">
                <div class="pad_30 txtCenter">
                    <img src="images/steps-for-lab-test.jpg" alt="" class="" />
                </div>
            </div>
            <div class="col_1_2">
                <div class="pad_30">
                    <h1 class="pageH2 clrLightBlack semiBold mrg_B_10">Steps For a Lab Test</h1>
                    <ul class="basicList">
                        <li>Fill the required lab test booking form.</li>
                        <li>Add required lab investigation in the form.</li>
                        <li>Submit it.</li>
                        <li>Our service provider will contact you.</li>
                    </ul>
                    <span class="space30"></span>
                    <%--<a href="https://www.thyrocare.com/wellness/Emailer/DM_landing_page/DM_Emailer.aspx?VAL=UDEwMDIzODAsUDEwMDIzODEsUDEwMDIzODIsUDEwMDIzODM=&dQsQa_code=7264978898" class="blueAnch dispBlk txtCenter semiBold upperCase letter-sp-2" target="_blank">Book a Lab Test</a>--%>
                    <a href="<%= Master.rootPath + "book-lab-test" %>" class="blueAnch dispBlk txtCenter semiBold upperCase letter-sp-2" target="_blank">Book a Lab Test</a>
                </div>
            </div>
            <div class="float_clear"></div>
        </div>
    </div>
    <span class="space50"></span>
    <!-- Lab Test Ends -->

    <!-- Download App Starts -->
        <span class="space50"></span>
        <div class="col_1140">
            <h3 class="clrLightBlack semiBold pageH3 txtCenter">Download App Now</h3>
            <span class="space50"></span>

            <div class="col_1_2 txtCenter">
                <img src="images/app-mock-image.png" />
            </div>
            <div class="col_1_2">
                <div class="width50">
                    <div class="pad_20">
                        <div class="iconBox txtCenter">
                            <img src="images/icons/vision.png" />
                        </div>
                        <span class="space20"></span>
                        <h4 class="semiMedium clrLightBlack semiBold mrg_B_10">Our Vision</h4>
                        <p class="paraTxt small">Our Vision is to be leading national chain of drugstore offering quality and affordable generic medicine with superior customer service</p>
                    </div>
                </div>
                <div class="width50">
                    <div class="pad_20">
                        <div class="iconBox txtCenter">
                            <img src="images/icons/mission.png" />
                        </div>
                        <span class="space20"></span>
                        <h4 class="semiMedium clrLightBlack semiBold mrg_B_10">Our Mission</h4>
                        <p class="paraTxt small">Our mission is to open 25000+ stores across India.</p>
                    </div>
                </div>
                <div class="float_clear"></div>

                <span class="space10"></span>

                <div class="width50">
                    <div class="pad_20">
                        <div class="iconBox txtCenter">
                            <img src="images/icons/values.png" />
                        </div>
                        <span class="space20"></span>
                        <h4 class="semiMedium clrLightBlack semiBold mrg_B_10">Our Values</h4>
                        <p class="paraTxt small">We Commit to truth and honesty in what we say and do, and practice high standards of fairness and ethics at work and in our relationships.</p>
                    </div>
                </div>
                <div class="width50">
                    <div class="pad_20">
                        <div class="iconBox txtCenter">
                            <img src="images/icons/support.png" />
                        </div>
                        <span class="space20"></span>
                        <h4 class="semiMedium clrLightBlack semiBold mrg_B_10">Our Support</h4>
                        <p class="paraTxt small">The right to use the <span class="upperCase fontRegular">Swast Aushadhi seva generic medicine store, genericart</span> trademark and logo location and market study assistance.</p>
                    </div>
                </div>
                <div class="float_clear"></div>

                <span class="space10"></span>

                <div class="width50">
                    <div class="pad_20">
                        <a href="#" target="_blank">
                            <div class="bgBlack txtCenter border_r_5">
                                <div class="pad_5">
                                    <img src="images/icons/apple-app-store-icon.png" alt="" />
                                </div>
                            </div>
                        </a>
                    </div>
                </div>
                <div class="width50">
                    <div class="pad_20">
                        <a href="https://play.google.com/store/apps/details?id=com.autoqed.genericartmedicine" target="_blank">
                            <div class="bgBlack txtCenter border_r_5">
                                <div class="pad_5">
                                    <img src="images/icons/goole-play.png" alt="" class="" />
                                </div>
                            </div>
                        </a>
                    </div>
                </div>
                <div class="float_clear"></div>
            </div>
            <div class="float_clear"></div>
        </div>
        <span class="space50"></span>
        <!-- Download App Ends -->
</asp:Content>

