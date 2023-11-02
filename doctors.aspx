<%@ Page Title="Doctors" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="doctors.aspx.cs" Inherits="doctors" %>
<%@ MasterType VirtualPath="~/MasterParent.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Doctors Starts -->
    <span class="space50"></span>
    <div class="col_1140" id="docList">
        <%=docStr %>
       <%-- <h1 class="clrLightBlack semiBold pageH2 mrg_B_10">Cardiologists</h1>--%>

       <%-- <div class="col_1_3">
            <div class="pad_10">
                <div class="docBox box-shadow">
                    <div class="width50">
                        <div class="pad_15">
                            <img src="<%= Master.rootPath + "images/dr-profile.jpg" %>" alt="" class="width100 border_r_5" />
                        </div>
                    </div>
                    <div class="width50">
                        <div class="pad_15">
                            <a href="<%= Master.rootPath + "doctors-profile/dr-amish-tripathy-1" %>" class="docName semiBold mrg_B_3">Dr. Amish Tripathy</a>
                            <span class="clrGrey semiBold tiny">Cardiologist</span><br />
                            <span class="clrGrey semiBold tiny">15 Years Experience</span>
                            <span class="space10"></span>
                            <span class="prod-offer-price">&#8377; 359</span>
                            <span class="prod-price">&#8377; 599</span>
                            <span class="space5"></span>
                        </div>
                    </div>
                    <div class="float_clear"></div>
                    <div class="width50 txtCenter">
                        <a href="<%= Master.rootPath + "doctors-profile/dr-amish-tripathy-1" %>" class="docProfileAnch semiBold upperCase">Profile</a>
                    </div>
                    <div class="width50 txtCenter">
                        <a href="<%= Master.rootPath + "book-appointment/dr-amish-tripathy-1" %>" class="docAnch semiBold upperCase">Consult</a>
                    </div>
                    <div class="float_clear"></div>
                </div>
            </div>
        </div>
        <div class="col_1_3">
            <div class="pad_10">
                <div class="docBox box-shadow">
                    <div class="width50">
                        <div class="pad_15">
                            <img src="<%= Master.rootPath + "images/dr-profile.jpg" %>" alt="" class="width100 border_r_5" />
                        </div>
                    </div>
                    <div class="width50">
                        <div class="pad_15">
                            <a href="#" class="docName semiBold mrg_B_3">Dr. Amish Tripathy</a>
                            <span class="clrGrey semiBold tiny">Cardiologist</span><br />
                            <span class="clrGrey semiBold tiny">15 Years Experience</span>
                            <span class="space10"></span>
                            <span class="prod-offer-price">&#8377; 359</span>
                            <span class="prod-price">&#8377; 599</span>
                            <span class="space5"></span>
                        </div>
                    </div>
                    <div class="float_clear"></div>
                    <div class="width50 txtCenter">
                        <a href="doctors-profile.html" class="docProfileAnch semiBold upperCase">Profile</a>
                    </div>
                    <div class="width50 txtCenter">
                        <a href="consult-doctor.html" class="docAnch semiBold upperCase">Consult</a>
                    </div>
                    <div class="float_clear"></div>
                </div>
            </div>
        </div>
        <div class="col_1_3">
            <div class="pad_10">
                <div class="docBox box-shadow">
                    <div class="width50">
                        <div class="pad_15">
                            <img src="<%= Master.rootPath + "images/dr-profile.jpg" %>" alt="" class="width100 border_r_5" />
                        </div>
                    </div>
                    <div class="width50">
                        <div class="pad_15">
                            <a href="#" class="docName semiBold mrg_B_3">Dr. Amish Tripathy</a>
                            <span class="clrGrey semiBold tiny">Cardiologist</span><br />
                            <span class="clrGrey semiBold tiny">15 Years Experience</span>
                            <span class="space10"></span>
                            <span class="prod-offer-price">&#8377; 359</span>
                            <span class="prod-price">&#8377; 599</span>
                            <span class="space5"></span>
                        </div>
                    </div>
                    <div class="float_clear"></div>
                    <div class="width50 txtCenter">
                        <a href="doctors-profile.html" class="docProfileAnch semiBold upperCase">Profile</a>
                    </div>
                    <div class="width50 txtCenter">
                        <a href="consult-doctor.html" class="docAnch semiBold upperCase">Consult</a>
                    </div>
                    <div class="float_clear"></div>
                </div>
            </div>
        </div>
        <div class="float_clear"></div>

        <div class="col_1_3">
            <div class="pad_10">
                <div class="docBox box-shadow">
                    <div class="width50">
                        <div class="pad_15">
                            <img src="<%= Master.rootPath + "images/dr-profile.jpg" %>" alt="" class="width100 border_r_5" />
                        </div>
                    </div>
                    <div class="width50">
                        <div class="pad_15">
                            <a href="#" class="docName semiBold mrg_B_3">Dr. Amish Tripathy</a>
                            <span class="clrGrey semiBold tiny">Cardiologist</span><br />
                            <span class="clrGrey semiBold tiny">15 Years Experience</span>
                            <span class="space10"></span>
                            <span class="prod-offer-price">&#8377; 359</span>
                            <span class="prod-price">&#8377; 599</span>
                            <span class="space5"></span>
                        </div>
                    </div>
                    <div class="float_clear"></div>
                    <div class="width50 txtCenter">
                        <a href="doctors-profile.html" class="docProfileAnch semiBold upperCase">Profile</a>
                    </div>
                    <div class="width50 txtCenter">
                        <a href="consult-doctor.html" class="docAnch semiBold upperCase">Consult</a>
                    </div>
                    <div class="float_clear"></div>
                </div>
            </div>
        </div>
        <div class="col_1_3">
            <div class="pad_10">
                <div class="docBox box-shadow">
                    <div class="width50">
                        <div class="pad_15">
                            <img src="<%= Master.rootPath + "images/dr-profile.jpg" %>" alt="" class="width100 border_r_5" />
                        </div>
                    </div>
                    <div class="width50">
                        <div class="pad_15">
                            <a href="#" class="docName semiBold mrg_B_3">Dr. Amish Tripathy</a>
                            <span class="clrGrey semiBold tiny">Cardiologist</span><br />
                            <span class="clrGrey semiBold tiny">15 Years Experience</span>
                            <span class="space10"></span>
                            <span class="prod-offer-price">&#8377; 359</span>
                            <span class="prod-price">&#8377; 599</span>
                            <span class="space5"></span>
                        </div>
                    </div>
                    <div class="float_clear"></div>
                    <div class="width50 txtCenter">
                        <a href="doctors-profile.html" class="docProfileAnch semiBold upperCase">Profile</a>
                    </div>
                    <div class="width50 txtCenter">
                        <a href="consult-doctor.html" class="docAnch semiBold upperCase">Consult</a>
                    </div>
                    <div class="float_clear"></div>
                </div>
            </div>
        </div>
        <div class="col_1_3">
            <div class="pad_10">
                <div class="docBox box-shadow">
                    <div class="width50">
                        <div class="pad_15">
                            <img src="<%= Master.rootPath + "images/dr-profile.jpg" %>" alt="" class="width100 border_r_5" />
                        </div>
                    </div>
                    <div class="width50">
                        <div class="pad_15">
                            <a href="#" class="docName semiBold mrg_B_3">Dr. Amish Tripathy</a>
                            <span class="clrGrey semiBold tiny">Cardiologist</span><br />
                            <span class="clrGrey semiBold tiny">15 Years Experience</span>
                            <span class="space10"></span>
                            <span class="prod-offer-price">&#8377; 359</span>
                            <span class="prod-price">&#8377; 599</span>
                            <span class="space5"></span>
                        </div>
                    </div>
                    <div class="float_clear"></div>
                    <div class="width50 txtCenter">
                        <a href="doctors-profile.html" class="docProfileAnch semiBold upperCase">Profile</a>
                    </div>
                    <div class="width50 txtCenter">
                        <a href="consult-doctor.html" class="docAnch semiBold upperCase">Consult</a>
                    </div>
                    <div class="float_clear"></div>
                </div>
            </div>
        </div>
        <div class="float_clear"></div>--%>

    </div>
    <span class="space50"></span>
    <!-- Doctors Starts -->
</asp:Content>

