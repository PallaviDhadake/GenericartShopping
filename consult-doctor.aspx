<%@ Page Title="Consult Doctor" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="consult-doctor.aspx.cs" Inherits="consult_doctor" %>
<%@ MasterType VirtualPath="~/MasterParent.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Consult Doctor Starts -->
    <span class="space50"></span>
    <div class="col_1140" id="consult">
        <%=errMsg %>
        <div class="bgWhite border_r_5">
            <div class="col_1_2">
                <div class="pad_30 txtCenter">
                    <img src="images/consult-doctor.jpg" alt="" class="" />
                </div>
            </div>
            <div class="col_1_2">
                <div class="pad_30">
                    <h1 class="pageH2 clrLightBlack semiBold mrg_B_10">Steps To Consult Doctor</h1>
                    <ul class="basicList">
                        <li>Fill the desired form for doctors consultation.</li>
                        <li>Our team will contact you within a hour</li>
                        <li>Our team will let you know the time of consultation</li>
                        <li>You will get online / in person prescription after consultation according to  the mode of consultation you prefer.</li>
                    </ul>
                </div>
            </div>
            <div class="float_clear"></div>
        </div>
    </div>
    <span class="space50"></span>
    <!-- Consult Doctor Ends -->

    <!-- Search by concern Starts -->
    <div class="col_1280">
        <div class="col_1140">
            <h2 class="clrLightBlack semiBold pageH3 mrg_B_10">Search By Speciality</h2>
        </div>
        <section class="search-products slider">
            <%=splStr %>
           <%-- <div class="slide">
                <div class="prodContainer txtCenter">
                    <div class="pad_15">
                        <img src="images/icons/corona.png" alt="" />
                        <a href="<%= Master.rootPath + "doctors/cardiologist-1" %>" class="prodName semiBold">Corona</a>
                    </div>
                </div>
            </div>
            <div class="slide">
                <div class="prodContainer txtCenter">
                    <div class="pad_15">
                        <img src="images/icons/corona.png" alt="" />
                        <a href="#" class="prodName semiBold">Corona</a>
                    </div>
                </div>
            </div>
            <div class="slide">
                <div class="prodContainer txtCenter">
                    <div class="pad_15">
                        <img src="images/icons/corona.png" alt="" />
                        <a href="#" class="prodName semiBold">Corona</a>
                    </div>
                </div>
            </div>
            <div class="slide">
                <div class="prodContainer txtCenter">
                    <div class="pad_15">
                        <img src="images/icons/corona.png" alt="" />
                        <a href="#" class="prodName semiBold">Corona</a>
                    </div>
                </div>
            </div>
            <div class="slide">
                <div class="prodContainer txtCenter">
                    <div class="pad_15">
                        <img src="images/icons/corona.png" alt="" />
                        <a href="#" class="prodName semiBold">Corona</a>
                    </div>
                </div>
            </div>
            <div class="slide">
                <div class="prodContainer txtCenter">
                    <div class="pad_15">
                        <img src="images/icons/corona.png" alt="" />
                        <a href="#" class="prodName semiBold">Corona</a>
                    </div>
                </div>
            </div>
            <div class="slide">
                <div class="prodContainer txtCenter">
                    <div class="pad_15">
                        <img src="images/icons/corona.png" alt="" />
                        <a href="#" class="prodName semiBold">Corona</a>
                    </div>
                </div>
            </div>
            <div class="slide">
                <div class="prodContainer txtCenter">
                    <div class="pad_15">
                        <img src="images/icons/corona.png" alt="" />
                        <a href="#" class="prodName semiBold">Corona</a>
                    </div>
                </div>
            </div>--%>
        </section>
    </div>
    <span class="space50"></span>
    <!-- Search by concern Ends -->

    <!-- Doctors Slider Starts -->
    <div class="col_1280">
        <div class="col_1140">
            <h2 class="clrLightBlack semiBold pageH3 mrg_B_10">Top Doctors</h2>
        </div>
        <section class="search-doctors slider">

            <%= topDocStr %>

           <%-- <div class="slide">
                <div class="docBox box-shadow">
                    <div class="docImg">
                        <img src="images/doctor-img.jpg" alt="" class="width100" />
                    </div>
                    <div class="docInfo">
                        <div class="pad_10">
                            <a href="<%= Master.rootPath + "doctors-profile/dr-amish-tripathy-1" %>" class="docName semiBold mrg_B_3">Dr. Amish Tripathy</a>
                            <span class="clrGrey semiBold tiny">Cardiologist</span><br />
                            <span class="clrGrey semiBold tiny">15 Years Experience</span>
                            <span class="space10"></span>
                            <span class="prod-offer-price">&#8377; 359</span>
                            <span class="prod-price">&#8377; 599</span>
                            <span class="space5"></span>
                        </div>
                        <a href="<%= Master.rootPath + "book-appointment/dr-amish-tripathy-1" %>" class="docAnch semiBold upperCase">Consult</a>
                    </div>
                    <div class="float_clear"></div>
                </div>
            </div>
            
            <div class="slide">
                <div class="docBox box-shadow">
                    <div class="docImg">
                        <img src="images/doctor-img.jpg" alt="" class="width100" />
                    </div>
                    <div class="docInfo">
                        <div class="pad_10">
                            <a href="#" class="docName semiBold mrg_B_3">Dr. Amish Tripathy</a>
                            <span class="clrGrey semiBold tiny">Cardiologist</span><br />
                            <span class="clrGrey semiBold tiny">15 Years Experience</span>
                            <span class="space10"></span>
                            <span class="prod-offer-price">&#8377; 359</span>
                            <span class="prod-price">&#8377; 599</span>
                            <span class="space5"></span>
                        </div>
                        <a href="#" class="docAnch semiBold upperCase">Consult</a>
                    </div>
                    <div class="float_clear"></div>
                </div>
            </div>

            <div class="slide">
                <div class="docBox box-shadow">
                    <div class="docImg">
                        <img src="images/doctor-img.jpg" alt="" class="width100" />
                    </div>
                    <div class="docInfo">
                        <div class="pad_10">
                            <a href="#" class="docName semiBold mrg_B_3">Dr. Amish Tripathy</a>
                            <span class="clrGrey semiBold tiny">Cardiologist</span><br />
                            <span class="clrGrey semiBold tiny">15 Years Experience</span>
                            <span class="space10"></span>
                            <span class="prod-offer-price">&#8377; 359</span>
                            <span class="prod-price">&#8377; 599</span>
                            <span class="space5"></span>
                        </div>
                        <a href="#" class="docAnch semiBold upperCase">Consult</a>
                    </div>
                    <div class="float_clear"></div>
                </div>
            </div>

            <div class="slide">
                <div class="docBox box-shadow">
                    <div class="docImg">
                        <img src="images/doctor-img.jpg" alt="" class="width100" />
                    </div>
                    <div class="docInfo">
                        <div class="pad_10">
                            <a href="#" class="docName semiBold mrg_B_3">Dr. Amish Tripathy</a>
                            <span class="clrGrey semiBold tiny">Cardiologist</span><br />
                            <span class="clrGrey semiBold tiny">15 Years Experience</span>
                            <span class="space10"></span>
                            <span class="prod-offer-price">&#8377; 359</span>
                            <span class="prod-price">&#8377; 599</span>
                            <span class="space5"></span>
                        </div>
                        <a href="#" class="docAnch semiBold upperCase">Consult</a>
                    </div>
                    <div class="float_clear"></div>
                </div>
            </div>--%>
        </section>
    </div>
    <span class="space50"></span>
    <!-- Doctors Slider Ends -->

    <!-- Download App Starts -->
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

    <script type="text/javascript">
        $(document).ready(function () {
            $('.search-products').slick({
                slidesToShow: 6, slidesToScroll: 1, autoplay: false, autoplaySpeed: 1000, arrows: true, dots: false, pauseOnHover: true,
                responsive: [{ breakpoint: 1024, settings: { slidesToShow: 4 } }, { breakpoint: 816, settings: { slidesToShow: 3 } }, { breakpoint: 540, settings: { slidesToShow: 2 } }, { breakpoint: 480, settings: { slidesToShow: 1 } }]
            });
            $('.search-doctors').slick({
                slidesToShow: 3, slidesToScroll: 1, autoplay: false, autoplaySpeed: 1000, arrows: true, dots: false, pauseOnHover: true,
                responsive: [{ breakpoint: 1024, settings: { slidesToShow: 2 } }, { breakpoint: 768, settings: { slidesToShow: 1 } }]
            });
        });
    </script>
</asp:Content>

