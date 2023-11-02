<%@ Page Title="Doctors Profile" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="doctors-profile.aspx.cs" Inherits="doctors_profile" %>
<%@ MasterType VirtualPath="~/MasterParent.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Profile Starts -->
    <span class="space50"></span>
    <div class="col_1140" id="docProfile">
        <%= errMsg %>
        <div class="col_340">

            <%=docStr %>

      <%--      <div class="bgWhite border_r_5 box-shadow">
                <div class="width50">
                    <div class="pad_15">
                        <img src="<%= Master.rootPath + "images/dr-profile.jpg" %>" alt="" class="width100 border_r_5" />
                    </div>
                </div>
                <div class="width50">
                    <div class="pad_15">
                        <h1 class="docName semiBold mrg_B_3">Dr. Amish Tripathy</h1>
                        <span class="themeClrPrime semiBold tiny">Cardiologist</span><br />
                        <span class="clrGrey semiBold tiny">15 Years Experience</span>
                    </div>
                </div>
                <div class="float_clear"></div>
                <div class="pad_15">
                    <h2 class="themeClrPrime semiMedium semiBold mrg_B_5">Qualification</h2>
                    <p class="clrGrey line-ht-5 small">M.B.B.S, Lorem ipsum dolor <br />M.D. Cardiology <br />D. M. Cardiac Surgery</p>

                    <span class="space20"></span>

                    <h2 class="themeClrPrime semiMedium semiBold mrg_B_5">About Him</h2>
                    <p class="clrGrey line-ht-5 small">Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s,</p>

                    <span class="space20"></span>
                    <a href="<%= Master.rootPath + "book-appointment/dr-amish-tripathy-1" %>" class="blueAnch dspBlk small upperCase letter-sp-2 txtCenter semiBold">Book Appointment</a>
                </div>
            </div>--%>


        </div>
        <div class="col_800">
            <div class="pad_L_15">
                <div class="pad_LR_15">
                    <h2 class="clrLightBlack semiBold pageH3">Happy Customers</h2>
                </div>

                <div class="width50">
                    <div class="pad_15">
                        <div class="bgWhite border_r_5 box-shadow">
                            <div class="pad_15">
                                <div class="testImg">
                                    <img src="<%= Master.rootPath + "images/no-photo.png" %>" />
                                </div>
                                <div class="testPerson">
                                    <h3 class="clrLightBlack semiBold">Pankaj Sharma</h3>
                                    <span class="clrLightBlack fontRegular small">Age 31</span>
                                </div>
                                <div class="float_clear"></div>
                                <span class="space20"></span>
                                <p class="clrGrey line-ht-5 small">Thank you so much for providing such wonderful service! The doctors and staff were very friendly and provided me with an exceptional experience.</p>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="width50">
                    <div class="pad_15">
                        <div class="bgWhite border_r_5 box-shadow">
                            <div class="pad_15">
                                <div class="testImg">
                                    <img src="<%= Master.rootPath + "images/no-photo.png" %>" />
                                </div>
                                <div class="testPerson">
                                    <h3 class="clrLightBlack semiBold">Sandip Rathi</h3>
                                    <span class="clrLightBlack fontRegular small">Age 33</span>
                                </div>
                                <div class="float_clear"></div>
                                <span class="space20"></span>
                                <p class="clrGrey line-ht-5 small">It is awesome! Getting my prescription filled was fast and simple. It was great not having to wait so that I could get back to my day.</p>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="float_clear"></div>

                <div class="width50">
                    <div class="pad_15">
                        <div class="bgWhite border_r_5 box-shadow">
                            <div class="pad_15">
                                <div class="testImg">
                                    <img src="<%= Master.rootPath + "images/no-photo.png" %>" />
                                </div>
                                <div class="testPerson">
                                    <h3 class="clrLightBlack semiBold">Mahesh Chavan</h3>
                                    <span class="clrLightBlack fontRegular small">Age 28</span>
                                </div>
                                <div class="float_clear"></div>
                                <span class="space20"></span>
                                <p class="clrGrey line-ht-5 small"> I wanted to express how impressed I was with the level of care I received from everyone I encountered.</p>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="width50">
                    <div class="pad_15">
                        <div class="bgWhite border_r_5 box-shadow">
                            <div class="pad_15">
                                <div class="testImg">
                                    <img src="<%= Master.rootPath + "images/no-photo.png" %>" />
                                </div>
                                <div class="testPerson">
                                    <h3 class="clrLightBlack semiBold">Sunil Shah</h3>
                                    <span class="clrLightBlack fontRegular small">Age 39</span>
                                </div>
                                <div class="float_clear"></div>
                                <span class="space20"></span>
                                <p class="clrGrey line-ht-5 small">I have always been able to talk to the doctor and get my problems resolved. Coming here is like dealing with trusted family. Would not consider changing doctors.</p>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="float_clear"></div>
            </div>
        </div>
        <div class="float_clear"></div>
    </div>
    <span class="space50"></span>
    <!-- Profile Ends -->
</asp:Content>

