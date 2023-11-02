<%@ Page Title="" Language="C#" MasterPageFile="~/GOBPDH/MasterGOBPDH.master" AutoEventWireup="true" CodeFile="gobp-registartion.aspx.cs" Inherits="GOBPDH_gobp_registartion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
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
            var k;
            document.all ? k = e.keyCode : k = e.which;
            return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57));
        }
    </script>

    <style type="text/css">
        .Space label {
            margin-left: 5px;
            margin-right: 15px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">Registration Form</h2>
    <span class="space15"></span>
    <div class="form-row">
        <div class="form-group col-md-6">
            <label><span style="color: red;">*</span> &nbsp; Select Franchisee Type :</label>
            <asp:DropDownList ID="ddrOpbType" runat="server" CssClass="form-control">
                <asp:ListItem Value="0"><- Select -></asp:ListItem>
            </asp:DropDownList>
        </div>

        <div class="form-group col-md-6">
            <label><span style="color: red;">*</span> &nbsp; Applicant Name :</label>
            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Width="100%" MaxLength="50" placeholder="Applicant Name"></asp:TextBox>
        </div>

        <div class="form-group col-md-6">
            <label><span style="color: red;">*</span> &nbsp; Firm Name : (If you have registered already, please enter firm name)</label>
            <asp:TextBox ID="txtshopName" runat="server" CssClass="form-control" Width="100%" MaxLength="50" placeholder="Firm Name"></asp:TextBox>
        </div>

        <div class="form-group col-md-6">
            <label><span style="color: red;">*</span> &nbsp; Type Of Firm :</label><br />
            <asp:RadioButton ID="rdbProprietor" runat="server" GroupName="firmType" Text=" Proprietor" CssClass="radio Space" />
            &nbsp;&nbsp;
            <asp:RadioButton ID="rdbPartner" runat="server" GroupName="firmType" Text=" Partner" CssClass="radio Space" />
            &nbsp;&nbsp;
            <asp:RadioButton ID="rdbTrust" runat="server" GroupName="firmType" Text=" Trust" CssClass="radio Space" />
            &nbsp;&nbsp;
            <asp:RadioButton ID="rdbOther" runat="server" GroupName="firmType" Text=" Other" CssClass="radio Space" />
        </div>

        <div class="form-group col-md-6">
            <label><span style="color: red;">*</span> &nbsp; Address :</label>
            <asp:TextBox ID="txtAdd" runat="server" CssClass="form-control" Width="100%" MaxLength="50" TextMode="MultiLine" Height="120" placeholder="Address"></asp:TextBox>
        </div>

        <div class="form-group col-md-6">
            <label><span style="color: red;">*</span> &nbsp; Birth Date : </label>
            <asp:TextBox ID="txtBirthDate" runat="server" CssClass="form-control" Width="100%" MaxLength="50" placeholder="Birth Date"></asp:TextBox>
        </div>

        <div class="form-group col-md-6">
            <label><span style="color: red;">*</span> &nbsp; Age :</label>
            <asp:TextBox ID="txtAge" runat="server" CssClass="form-control" Width="100%" MaxLength="50" placeholder="Age"></asp:TextBox>
        </div>

        <div class="form-group col-md-6">
            <label><span style="color: red;">*</span> &nbsp; Marital Status :</label><br />
            <asp:RadioButton ID="rdbSingle" runat="server" GroupName="maritalStatus" Text=" Single" CssClass="radio Space" />
            <asp:RadioButton ID="rdbMarried" runat="server" GroupName="maritalStatus" Text=" Married" CssClass="radio Space" />
        </div>

        <div class="form-group col-md-6">
            <label><span style="color: red;">*</span> &nbsp; Email Id :</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Width="100%" MaxLength="50" placeholder="Email"></asp:TextBox>
        </div>

        <div class="form-group col-md-6">
            <label><span style="color: red;">*</span> &nbsp; Mobile No :</label>
            <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" Width="100%" MaxLength="10" placeholder="Mobile No."></asp:TextBox>
        </div>

        <div class="form-group col-md-6">
            <label><span style="color: red;">*</span> &nbsp; WhatsApp No :</label>
            <asp:TextBox ID="txtWpNo" runat="server" CssClass="form-control" Width="100%" MaxLength="10" placeholder="Whatapps No."></asp:TextBox>
        </div>

        <div class="form-group col-md-6">
            <label><span syle="color: red;">*</span> &nbsp; State :</label>
            <asp:DropDownList ID="ddrState" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddrState_SelectedIndexChanged">
                <asp:ListItem Value="0"><- Select -></asp:ListItem>
            </asp:DropDownList>
        </div>

        <div class="form-group col-md-6">
            <label><span style="color: red;">*</span> &nbsp; District :</label>
            <asp:DropDownList ID="ddrDistrict" CssClass="form-control" runat="server"
                AutoPostBack="True" OnSelectedIndexChanged="ddrDistrict_SelectedIndexChanged">
                <asp:ListItem Value="0"><-Select-></asp:ListItem>
            </asp:DropDownList>
        </div>

        <div class="form-group col-md-6">
            <label><span style="color: red;">*</span> &nbsp; City :</label>
            <asp:DropDownList ID="ddrCity" CssClass="form-control" runat="server" AutoPostBack="True">
                <asp:ListItem Value="0"><-Select-></asp:ListItem>
            </asp:DropDownList>
        </div>

        <div class="form-group col-md-6">
            <label><span style="color: red;">*</span> &nbsp; Education Of Owner :</label><br />
            <asp:RadioButton ID="rdbEduTenth" runat="server" GroupName="education" Text=" 10 - 12th" CssClass="radio Space" />
            <asp:RadioButton ID="rdbEduGraduate" runat="server" GroupName="education" Text=" Graduate" CssClass="radio Space" />
            <asp:RadioButton ID="rdbEduPG" runat="server" GroupName="education" Text=" Post Graduate" CssClass="radio Space" />
            <asp:RadioButton ID="rdbEduOther" runat="server" GroupName="education" Text=" Other" CssClass="radio Space" />
        </div>

        <div class="form-group col-md-6">
            <label><span style="color: red;">*</span> &nbsp; Owner Occupation :</label>
            <asp:TextBox ID="txtownrOccuption" runat="server" CssClass="form-control" Width="100%" MaxLength="50" placeholder="Occupation"></asp:TextBox>
        </div>

        <div class="form-group col-md-6">
            <label><span style="color: red;">*</span> &nbsp; Any Illegal Matters :</label><br />
            <asp:RadioButton ID="rdbMatterYes" runat="server" GroupName="legalMatter" Text=" Yes" CssClass="radio Space" />
            <asp:RadioButton ID="rdbMatterNo" runat="server" GroupName="legalMatter" Text=" No" CssClass="radio Space" />
        </div>

        <div class="form-group col-md-6">
            <label><span style="color: red;">*</span> &nbsp; Residence From :</label><br />
            <asp:RadioButton ID="rdbBelow5Yr" runat="server" GroupName="residence" Text=" 0 - 5 Years" CssClass="radio Space" />
            <asp:RadioButton ID="rdb5Yr" runat="server" GroupName="residence" Text=" 5 - 10 Years" CssClass="radio Space" />
            <asp:RadioButton ID="rdb10Yr" runat="server" GroupName="residence" Text=" More than 10 Years" CssClass="radio Space" />
        </div>

        <div class="form-group col-md-6">
            <label>Parent GOBP :(Enter GOBP Refferal User Id If Any) </label>
            <asp:TextBox ID="txtParentGobp" runat="server" CssClass="form-control" Width="100%" MaxLength="50" BackColor="#F3EE8E" maxlenght="15" placeholder="Enter GOBP User Id"></asp:TextBox>
        </div>
        </div>
        <span class="space20"></span>

    <div class="form-row">
        <h3 class="pgTitle" style="color: blue;">Payment Details</h3>
        <div class="form-group col-md-12">
            <h4 class="line-ht-5 info">If you have already paid GOBP registration Fees, then fill the following details,
                <br />
                otherwise click here to make <a href="../images/QR-Code-RazorPay.jpg" data-fancybox="imgGroup" class="rm">Online Payment</a></h4>
        </div>
        <span class="space20"></span>
        <div class="form-group col-md-6">
            <label><span style="color: red;">*</span> &nbsp; UTR No. :</label>
            <asp:TextBox ID="txtUTR" runat="server" CssClass="form-control" Width="100%" MaxLength="50" placeholder="UTR No."></asp:TextBox>
        </div>

        <div class="form-group col-md-6">
            <label><span style="color: red;">*</span> &nbsp; Bank Name :</label>
            <asp:TextBox ID="txtBank" runat="server" CssClass="form-control" Width="100%" MaxLength="50" placeholder="Bank Name"></asp:TextBox>
        </div>

        <div class="form-group col-md-6">
            <label><span style="color: red;">*</span> &nbsp; Transaction Date. :</label>
            <asp:TextBox ID="txtTrDate" runat="server" CssClass="form-control" Width="100%" MaxLength="50" placeholder="Click Here to open Calendar"></asp:TextBox>
        </div>

        <div class="form-group col-md-6">
            <label><span style="color: red;">*</span> &nbsp; Account Holder Name :</label>
            <asp:TextBox ID="txtHolderName" runat="server" CssClass="form-control" Width="100%" MaxLength="50" placeholder="Account Holder Name"></asp:TextBox>
        </div>

        <div class="form-group col-md-6">
            <label><span style="color: red;">*</span> &nbsp; Amount :</label>
            <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" Width="100%" MaxLength="50" placeholder="Amount"></asp:TextBox>
        </div>
        <div class="form-group col-md-6"></div>
        <span class="space15"></span>
        <div class="form-group col-md-12">
            <a href="javascript:;" class="terms" id="test">Terms &amp; Conditions</a>
        </div>
        <span class="space15"></span>
        <div class="form-group col-md-12">
            <asp:CheckBox ID="chkTerms" CssClass="chkList Space" TextAlign="Right" Text=" By Clicking this you are agree to our terms and conditions" runat="server" />
        </div>

        <div class="form-group col-md-12">
            <div class="dis-none col_800_center" id="hidden-content">
                <h2 class="pgTitle" style="color: blue;">Terms and Conditions</h2>
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
        </div>

        <span class="space15"></span>
        <%= errMsg %>
        <span class="space15"></span>
        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="SUBMIT" OnClick="btnSubmit_Click" />


        <div class="float_clear"></div>
        <span class="space30"></span>
   </div>
</asp:Content>

