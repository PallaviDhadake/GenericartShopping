<%@ Page Title="" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="gobp-registration.aspx.cs" Inherits="gobp_registration" %>
<%@ MasterType VirtualPath="~/MasterParent.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="<%=Master.rootPath + "css/redmond.datepick.css" %>" rel="stylesheet" />
    <script src="<%=Master.rootPath + "js/jquery.plugin.js" %>" type="text/javascript"></script>
    <script src="<%=Master.rootPath + "js/jquery.datepick.js" %>" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            <%--$('#<% =txtJoinDate.ClientID%>').datepick({ onSelect: function (dates) { getAge() }, dateFormat: 'dd/mm/yyyy' });--%>
            $('#<% =txtBirthDate.ClientID%>').datepick({ onSelect: function (dates) { getAge() }, dateFormat: 'dd/mm/yyyy' });
            $('#<%=txtTrDate.ClientID %>').datepick({ dateFormat: 'dd/mm/yyyy' });
        });
    </script>

    <script type="text/javascript">

        function getAge() {
            var dateString = document.getElementById("<% =txtBirthDate.ClientID%>").value;
             var arr = dateString.split('/');
             dateString = arr[1] + "/" + arr[0] + "/" + arr[2];
             var today = new Date();
             var birthDate = new Date(dateString);
             var age = today.getFullYear() - birthDate.getFullYear();
             var m = today.getMonth() - birthDate.getMonth();
             if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
                 age--;
             }
             document.getElementById("<%=txtAge.ClientID %>").value = age;
        }
        function alpha(e) {
            //alert("Alpha Called");
            var k;
            document.all ? k = e.keyCode : k = e.which;
            return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57));
        }
    </script>
    <style>
        .docAgree {
            display:block;
            text-decoration:none;
            padding:5px 0px 5px 37px;
            color:#000;
            font-size:18px;
            line-height:1.5;
            font-weight:600;
            background:url(images/icons/docs.png) no-repeat left center;
        }
        .docAgree:hover{
            color: #454545;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Page Header Starts -->
   <%-- <div class="pgHeader4">
        <div class="headerOverlay">
            <div class="col_1140">
                <div class="pg_TB_pad">
                    <h1 class="pageH1 clrWhite">Genericart Online Business Partner Registration</h1>
                    <ul class="bCrumb">
                        <li><a href="<%= Master.rootPath %>">Home</a></li>
                        <li>&raquo;</li>
                        <li>Genericart Online Business Partner Registration</li>
                    </ul>
                    <div class="float_clear"></div>
                </div>
            </div>
        </div>
    </div>--%>
    <!-- Page Header Ends -->
    <span class="space30"></span>
    <div class="col_1140">
        <div class="col_800">
            <h2 class="pageH2 themeClrPrime mrg_B_10">Registration Form</h2>
            <span class="space20"></span>
            <%--<div class="w50 float_left mrg_B_15">
                <div class="app_r_padding">
                    <span class="labelCap">Encrypt Id :<span class="redStar">*</span></span>
                    <asp:TextBox ID="txtEncryptId" CssClass="textBox w95" MaxLength="50" placeholder="Encrypt Id" runat="server"></asp:TextBox>
                </div>
            </div>--%>
            
            <%--<div class="w50 float_left mrg_B_15">
                <div class="app_r_padding">
                    <span class="labelCap">Join Date :<span class="redStar">*</span></span>
                    <asp:TextBox ID="txtJoinDate" CssClass="textBox w95" MaxLength="50" placeholder="Join Date" runat="server"></asp:TextBox>
                </div>
            </div>--%>
             

            <div class="w50 float_left mrg_B_15">
                <div class="app_r_padding">
                    <span class="labelCap">Select Franchisee Type :<span class="redStar">*</span></span>
                    <asp:DropDownList ID="ddrOpbType" runat="server" CssClass="cmbBox">
                        <asp:ListItem Value="0"><- Select -></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="float_clear"></div>
            
             <div class="w100 float_left mrg_B_15">
                <div class="app_r_padding">
                    <span class="labelCap">Applicant Name :<span class="redStar">*</span></span>
                    <asp:TextBox ID="txtName" CssClass="textBox w95" MaxLength="50" placeholder="Applicant Name" runat="server"></asp:TextBox>
                </div>
            </div>
             <div class="float_clear"></div>

            <div class="w100 float_left mrg_B_15">
                <div class="app_r_padding">
                    <span class="labelCap">Firm Name :<span class="redStar small fontRegular">(If you have registered already, please enter firm name)</span></span>
                    <asp:TextBox ID="txtshopName" CssClass="textBox w95" MaxLength="50" placeholder="Firm Name" runat="server"></asp:TextBox>
                </div>
            </div>
             <div class="float_clear"></div>

            <div class="w100 mrg_B_15">
                    <span class="labelCap">Type Of Firm :<span class="redStar">*</span></span>
                    <asp:RadioButton ID="rdbProprietor" runat="server" GroupName="firmType" Text=" Proprietor" CssClass="radio" />
                    <asp:RadioButton ID="rdbPartner" runat="server" GroupName="firmType" Text=" Partner" CssClass="radio" />
                    <asp:RadioButton ID="rdbTrust" runat="server" GroupName="firmType" Text=" Trust" CssClass="radio" />
                    <asp:RadioButton ID="rdbOther" runat="server" GroupName="firmType" Text=" Other" CssClass="radio" />
                </div>
             <div class="float_clear"></div>
            
            <div class="w100 float_left mrg_B_15">
                <div class="app_r_padding">
                    <span class="labelCap">Address :<span class="redStar">*</span></span>
                    <asp:TextBox ID="txtAdd" CssClass="textBox w95" MaxLength="300" TextMode="MultiLine" Height="120" placeholder="Address" runat="server"></asp:TextBox>
                </div>
            </div>
             <div class="float_clear"></div>


              <div class="w50 float_left mrg_B_15">
                <div class="app_r_padding">
                    <span class="labelCap">Birth Date  :<span class="redStar">*</span></span>
                    <asp:TextBox ID="txtBirthDate" CssClass="textBox w95" MaxLength="10" placeholder="Birth Date" runat="server"></asp:TextBox>
                </div>
            </div>
            
            <div class="w50 float_left mrg_B_15">
                <div class="app_r_padding">
                    <span class="labelCap">Age :<span class="redStar">*</span></span>
                    <asp:TextBox ID="txtAge" CssClass="textBox w95" MaxLength="5" placeholder="Age" runat="server"></asp:TextBox>
                </div>
            </div>
             <div class="float_clear"></div>

            <div class="w100 mrg_B_15">
                    <span class="labelCap">Marital Status :<span class="redStar">*</span></span>
                    <asp:RadioButton ID="rdbSingle" runat="server" GroupName="maritalStatus" Text=" Single" CssClass="radio" />
                    <asp:RadioButton ID="rdbMarried" runat="server" GroupName="maritalStatus" Text=" Married" CssClass="radio" />
                </div>
             <div class="float_clear"></div>

            <div class="w50 float_left mrg_B_15">
                <div class="app_r_padding">
                    <span class="labelCap">Email Id  :<span class="redStar">*</span></span>
                    <asp:TextBox ID="txtEmail" CssClass="textBox w95" MaxLength="50" placeholder="Email Id" runat="server"></asp:TextBox>
                </div>
            </div>
            
            <div class="w50 float_left mrg_B_15">
                <div class="app_r_padding">
                    <span class="labelCap">Mobile No :<span class="redStar">*</span></span>
                    <asp:TextBox ID="txtMobile" CssClass="textBox w95" MaxLength="10" placeholder="Mobile No" runat="server"></asp:TextBox>
                </div>
            </div>
             <div class="float_clear"></div>

             <div class="w50 float_left mrg_B_15">
                <div class="app_r_padding">
                    <span class="labelCap">WhatsApp No  :<span class="redStar">*</span></span>
                    <asp:TextBox ID="txtWpNo" CssClass="textBox w95" MaxLength="15" placeholder="WhatsApp No" runat="server"></asp:TextBox>
                </div>
            </div>

            <div class="w50 float_left mrg_B_15">
                    <div class="app_r_padding">
                        <span class="labelCap">State :<span class="redStar">*</span></span>
                        <asp:DropDownList ID="ddrState" CssClass="cmbBox w95" runat="server" 
                            AutoPostBack="true" OnSelectedIndexChanged="ddrState_SelectedIndexChanged" >
                            <asp:ListItem Value="0"><-Select-></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

            <div class="float_clear"></div>

            
                <div class="w50 float_left mrg_B_15">
                    <div class="app_r_padding" >
                        <span class="labelCap">District :<span class="redStar">*</span></span>
                        <asp:DropDownList ID="ddrDistrict" CssClass="cmbBox w95" runat="server" 
                            AutoPostBack="True" OnSelectedIndexChanged="ddrDistrict_SelectedIndexChanged">
                            <asp:ListItem Value="0"><-Select-></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <%--<div class="float_clear"></div>--%>

                <div class="w50 float_left mrg_B_15">
                    <div class="app_r_padding">
                        <span class="labelCap">City :<span class="redStar">*</span></span>
                        <asp:DropDownList ID="ddrCity" CssClass="cmbBox w95" runat="server" AutoPostBack="True">
                            <asp:ListItem Value="0"><-Select-></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            <div class="float_clear"></div>

             

            <%--<div class="w50 float_left mrg_B_15">
                <div class="app_r_padding">
                    <span class="labelCap">User Id  :<span class="redStar"></span></span>
                    <asp:TextBox ID="txtUserId" CssClass="textBox w95" MaxLength="30" placeholder="User Id" runat="server"></asp:TextBox>
                </div>
            </div>
            
            <div class="w50 float_left mrg_B_15">
                <div class="app_r_padding">
                    <span class="labelCap">User Password :<span class="redStar"></span></span>
                    <asp:TextBox ID="txtpass" CssClass="textBox w95" MaxLength="50" placeholder="User Password" runat="server"></asp:TextBox>
                </div>
            </div>
             <div class="float_clear"></div>--%>

             <%--<div class="w50 float_left mrg_B_15">
                    <span class="labelCap">Sales Insentive :<span class="redStar"></span></span>
                    <asp:RadioButton ID="RadioButton1" runat="server" GroupName="maritalStatus" Text=" Yes" CssClass="radio" />
                    <asp:RadioButton ID="RadioButton2" runat="server" GroupName="maritalStatus" Text=" No" CssClass="radio" />
                </div>
            
             <div class="w50 float_left mrg_B_15">
                    <span class="labelCap">Referral :<span class="redStar">*</span></span>
                    <asp:RadioButton ID="rdbReferralYes" runat="server" GroupName="maritalStatus" Text=" Yes" CssClass="radio" />
                    <asp:RadioButton ID="rdbReferralNo" runat="server" GroupName="maritalStatus" Text=" No" CssClass="radio" />
                </div>
            <div class="float_clear"></div>--%>

             <div class="w100 mrg_B_15">
                    <span class="labelCap">Education Of Owner :<span class="redStar">*</span></span>
                    <asp:RadioButton ID="rdbEduTenth" runat="server" GroupName="education" Text=" 10 - 12th" CssClass="radio" />
                    <asp:RadioButton ID="rdbEduGraduate" runat="server" GroupName="education" Text=" Graduate" CssClass="radio" />
                    <asp:RadioButton ID="rdbEduPG" runat="server" GroupName="education" Text=" Post Graduate" CssClass="radio" />
                    <asp:RadioButton ID="rdbEduOther" runat="server" GroupName="education" Text=" Other" CssClass="radio" />
                </div>
            
            <div class="w100  mrg_B_15">
                <div class="app_r_padding">
                    <span class="labelCap">Owner Occupation  :<span class="redStar">*</span></span>
                    <asp:TextBox ID="txtownrOccuption" CssClass="textBox w95" MaxLength="50" placeholder="Occupation" runat="server"></asp:TextBox>
                </div>
            </div>
             <div class="float_clear"></div>

            <div class="w50 float_left mrg_B_15">
                    <div class="app_r_padding">
                        <span class="labelCap">ProfilePic :</span>
                        <span class="subNotice">Upload Profile Pic</span>
                        <span class="subNotice">File size Less than 5 MB</span>
                            <asp:FileUpload ID="fuprofilePic" runat="server" CssClass="mrg_B_15" />
                    </div>
                </div>

            <div class="w50 float_left mrg_B_15">
                    <div class="app_r_padding">
                        <span class="labelCap">Resume :</span>
                        <span class="subNotice">Upload Resume </span>
                        <span class="subNotice">File size Less than 5 MB</span>
                            <asp:FileUpload ID="fuResume" runat="server" CssClass="mrg_B_15" />
                    </div>
                </div>
            <div class="float_clear"></div>

                <div class="w50 float_left mrg_B_15">
                    <div class="app_r_padding" >
                        <span class="labelCap">Address  Proof :<span class="redStar">*</span> </span>
                        <span class="subNotice">please attach mobile photo or scanned copy of Adhar Card / PAN Card (Compulsory)</span>
                        <span class="subNotice">File size Less than 5 MB</span>
                            <asp:FileUpload ID="fuAddProof" runat="server" CssClass="mrg_B_15" />
                        <asp:FileUpload ID="fuAddProof1" runat="server" CssClass="mrg_B_15" />
                    </div>
                </div>

             <div class="w50 float_left mrg_B_15">
                    <div class="app_r_padding" >
                        <span class="labelCap">Id  Proof :<span class="redStar">*</span> </span>
                        <span class="subNotice">please attach mobile photo or scanned copy of Adhar Card / PAN Card (Compulsory)</span>
                        <span class="subNotice">File size Less than 5 MB</span>
                            <asp:FileUpload ID="fulIdProof" runat="server" CssClass="mrg_B_15" />
                        <asp:FileUpload ID="fulIdProof1" runat="server" CssClass="mrg_B_15" />
                    </div>
                </div>

                <div class="float_clear"></div>

            <div class="w100 mrg_B_15">
                    <span class="labelCap">Any Illegal Matters :<span class="redStar">*</span></span>
                    <asp:RadioButton ID="rdbMatterYes" runat="server" GroupName="legalMatter" Text=" Yes" CssClass="radio" />
                    <asp:RadioButton ID="rdbMatterNo" runat="server" GroupName="legalMatter" Text=" No" CssClass="radio" />
                </div>
            <div class="float_clear"></div>

            <div class="w100 mrg_B_15">
                    <span class="labelCap">Residence From :<span class="redStar">*</span></span>
                    <asp:RadioButton ID="rdbBelow5Yr" runat="server" GroupName="residence" Text=" 0 - 5 Years" CssClass="radio" />
                    <asp:RadioButton ID="rdb5Yr" runat="server" GroupName="residence" Text=" 5 - 10 Years" CssClass="radio" />
                    <asp:RadioButton ID="rdb10Yr" runat="server" GroupName="residence" Text=" More than 10 Years" CssClass="radio" />
                </div>
             <div class="float_clear"></div>
            <%--<div class="w50 float_left mrg_B_15">
                <div class="app_r_padding">
                    <span class="labelCap">Shop Code  :<span class="redStar"></span></span>
                    <asp:TextBox ID="txtShopCode" CssClass="textBox w95" MaxLength="30" placeholder="Shop Code" runat="server"></asp:TextBox>
                </div>
            </div>

            <div class="float_clear"></div>--%>

            <span class="space20"></span>
                <%--<span class="space20"></span>--%>
               <h3 class="medium themeClrPrime mrg_B_5">Payment Details</h3>
                <h4 class="line-ht-5 info">If you have already paid GOBP registration Fees, then fill the following details, <br /> otherwise click here to make <a href="images/QR-Code-RazorPay.jpg" data-fancybox="imgGroup" class="rm">Online Payment</a></h4>
                <span class="space20"></span>

                <%-- New Payment Details --%>
                <div class="w50 float_left mrg_B_15">
                    <div class="app_r_padding">
                        <span class="labelCap">UTR No.:<span class="redStar">*</span></span>
                        <asp:TextBox ID="txtUTR" runat="server" CssClass="textBox" MaxLength="30" ></asp:TextBox>
                    </div>
                </div>
                <div class="w50 float_left mrg_B_15">
                    <div class="app_r_padding" >
                        <span class="labelCap">Bank Name :<span class="redStar">*</span></span>
                        <asp:TextBox ID="txtBank" CssClass="textBox" MaxLength="50" placeholder="" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="float_clear"></div>

            <div class="w50 float_left mrg_B_15">
                <div class="app_r_padding">
                    <span class="labelCap">Transaction Date.:<span class="redStar">*</span></span>
                    <asp:TextBox ID="txtTrDate" runat="server" CssClass="textBox" placeholder="Click Here to open Calendar"></asp:TextBox>
                </div>
            </div>

            <div class="w50 float_left mrg_B_15">
                <div class="app_r_padding">
                    <span class="labelCap">Account Holder Name:<span class="redStar">*</span></span>
                    <asp:TextBox ID="txtHolderName" runat="server" CssClass="textBox" placeholder="Amount Holder Name" MaxLength="50"></asp:TextBox>
                </div>
            </div>
            <div class="float_clear"></div>

            <div class="w50 float_left mrg_B_15">
                    <div class="app_r_padding">
                        <span class="labelCap">Amount:<span class="redStar">*</span></span>
                        <asp:TextBox ID="txtAmount" runat="server" CssClass="textBox" placeholder="Amount" ></asp:TextBox>
                    </div>
                </div>
            <%--<div class="w50 float_left mrg_B_15">
                <div class="app_r_padding">
                    <span class="labelCap">Account Holder Name:<span class="redStar">*</span></span>
                    <asp:TextBox ID="txtHolderName" runat="server" CssClass="textBox" placeholder="Amount Holder Data"></asp:TextBox>
                </div>
            </div>--%>
            <div class="float_clear"></div>

               <%--<div class="w50 float_left mrg_B_15">
                    <div class="app_r_padding">
                        <span class="labelCap">Amount:<span class="redStar">*</span></span>
                        <asp:TextBox ID="txtAmount" runat="server" CssClass="textBox" placeholder="Amount" ></asp:TextBox>
                    </div>
                </div>
             <div class="w50 float_left mrg_B_15">
                    <div class="app_r_padding">
                        <span class="labelCap">Total Purchase:<span class="redStar">*</span></span>
                        <asp:TextBox ID="txtTotPur" runat="server" CssClass="textBox" placeholder="Amount" ></asp:TextBox>
                    </div>
                </div>

                <div class="float_clear"></div>--%>

                

               <%-- <div class="w50 float_left mrg_B_15">
                    <div class="app_r_padding" >
                        <span class="space40"></span>
                        <asp:CheckBox ID="chkSoftPay" CssClass="chkList" TextAlign="Right" Text=" Software Payment Included?" runat="server" />
                    </div>
                </div>--%>
                <div class="float_clear"></div>

                
                <%-- New Payment Details Ends --%>

            <span class="space15"></span>      
                <a href="javascript:;" class="terms" id="test">Terms &amp; Conditions</a>
                <span class="space15"></span>
                <!-- Download Agreement Copy -->
                <a href="upload/GOBP-AGREEMENT-COPY.pdf" target="_blank" class="docAgree">Download GOBP Agreement Form</a>
                <span class="space15"></span>
                <asp:CheckBox ID="chkTerms" CssClass="chkList" TextAlign="Right" Text=" By Clicking this you are agree to our terms and conditions" runat="server" />
                    
                <div class="dis-none col_800_center" id="hidden-content">
                    <h2 class="pageH2 themeClrPrime mrg_B_5">Terms and Conditions</h2>
                    <ul class="greenList">
                        <li>I understand and agree that the statements in this proposal form shall be the basis of the contract between me and Genericart Medicine Pvt Ltd.</li>
                        <li>I further declare that the statements in this proposal are true and I have disclosed all information which might be material to the company. I declare that I have read the OBP agreement and understood the terms and conditions associated risks and benefits which I propose to take.</li>
                        <li>I declare that the amount paid has not been generated from the proceeds of any criminal activities / offences and I shall abide by and conform to the Prevention of Money Laundering Act, 2002 or any other applicable laws.</li> 
                        <li>I declare that the company has disclosed and explained all the information related to OBP and I declare that I have understood the same before signing this proposal form.</li>
                        <li>I also hereby agree and authorized the Company to access my data maintained by the Unique Identification Authority of India (UIDAI) for KYC verification and other eKYC services purpose.</li>
                        <li>I herewith declare that I have understood & read all your term, condition & FAQ of agreement as well as I am aware that the OBP fee is non refundable in any case. I am willing to purchase virtual franchise accordingly. Kindly prepare agreement per details mentioned above.</li>
                        <li>I herewith declare that I do not have any criminal background as well as there were no civil judicial cases pending/running against me.</li> 
                    </ul>
                    <span class="space10"></span>
                    <h4 class="semiBold mrg_B_5">I have understood all above points which are already present in agreement.</h4>
                    <p class="paraTxt">Terms &amp; Conditions accepted will be considered as “Signature”.</p>
                </div>
                    
                <span class="space15"></span>
                <%= errMsg %>
                <span class="space15"></span>
                <asp:Button ID="btnSubmit" runat="server" CssClass="buttonForm" Text="SUBMIT" OnClick="btnSubmit_Click" />

           
            <div class="float_clear"></div>
            <span class="space30"></span>

            <%--<asp:Button ID="Button1" CssClass="" runat="server" Text="Button" />--%>
        </div>

         <!-- Sidebar starts -->
        <div class="col_340">
            <div class="pad_L_30">
                <div class="sideBlueBase">
                    <div class="pad_TB_20">
                        <h4 class="sideTitle medium upperCase">Business</h4>
                    </div>
                    <img alt="Business Opportunity, Genericart Medicine" src="<%= Master.rootPath + "images/gobp-genericart.jpg" %>"  class="width100"/>
                    <div class="pad_20">
                        <span class="sideIntro">
                            Genericart Online Business Partner is a unique business platform where you can join us as a business partner who is involved in joining clients to company.
                            <br />This is a vertual franchise where you get the benefits on monthly basis.
                        </span>
                        <span class="space20"></span>
                        <a href="business-opportunity" class="sideAnch" title="Find Business Opportunity">Know More</a>
                    </div>
                </div>
            </div>
        </div>
        <!-- Sidebar ends -->
        <div class="float_clear"></div>

    </div>


     <script>
         $("#test").on('click', function () {

             $.fancybox.open({
                 src: '#hidden-content',
                 type: 'inline',
                 opts: {
                     afterShow: function (instance, current) {
                         console.info('done!');
                     }
                 }
             });

         });
     </script>

</asp:Content>

