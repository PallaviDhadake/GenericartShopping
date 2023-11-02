<%@ Page Title="" Language="C#" MasterPageFile="~/obp/MasterObp.master" AutoEventWireup="true" CodeFile="edit-obp-profile.aspx.cs" Inherits="obp_edit_obp_profile" %>
<%@ MasterType VirtualPath="~/obp/MasterObp.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function setupcalendar() {
            <%-- $('#<% =txtBirthDate.ClientID%>').datepick({ onSelect: function (dates) { getAge() }, dateFormat: 'dd/mm/yyyy' });--%>
             $('#<%=txtTrDate.ClientID %>').datepick({ dateFormat: 'dd/mm/yyyy' });
        }
    </script>

    <style type="text/css">
        .Space label {
            margin-left: 5px;
            margin-right: 15px;
        }
    </style>
    <script>
        function validateNumber(e) {
            const pattern = /^[0-9]$/;
            return pattern.test(e.key)
        }
    </script>


    <script type="text/javascript">
        function pageLoad(sender, args) {
            if (args.get_isPartialLoad()) {
                setupcalendar();
            }
        }
    </script>

    <script type="text/javascript">
        // Call setupAutocomplete on initial page load
        $(document).ready(function () {
            setupcalendar();
        });
    </script>

  <%--  <script>
        window.onload = function () {
            restoreDdrItemValue();
        }
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">Edit GOBP Profile</h2>
    <span class="space15"></span>
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>
    <div class="card card-primary">
        <div class="card-header">
            <h3 class="card-title">Edit GOBP Data</h3>
        </div>
        <div class="card-body">
            <div class="colorLightBlue">
                <%-- <span>Id :</span>--%>
                <asp:Label ID="lblId" runat="server" Text="[New]"></asp:Label>
            </div>

            <span class="space15"></span>
            <div class="form-row">
              
                <div class="form-group col-md-6">
                    <label><span style="color: red;">*</span> &nbsp; Applicant Name :</label>
                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Firm Name :</label>
                    <asp:TextBox ID="txtshopName" runat="server" CssClass="form-control" Width="100%" MaxLength="70"></asp:TextBox>
                </div>

                <div class="form-group col-md-6">
                    <label>Type Of Firm :</label><br />
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
                    <asp:TextBox ID="txtAdd" runat="server" CssClass="form-control textarea" TextMode="MultiLine" Height="150" Width="100%"></asp:TextBox>
                </div>
             
                <div class="form-group col-md-6">
                        <label><span style="color: red;">*</span> &nbsp; Marital Status :</label><br />
                        <asp:RadioButton ID="rdbSingle" runat="server" GroupName="maritalStatus" Text=" Single" CssClass="radio Space" />
                        <asp:RadioButton ID="rdbMarried" runat="server" GroupName="maritalStatus" Text=" Married" CssClass="radio Space" />
                    </div>

                <div class="form-group col-md-6">
                    <label><span style="color: red;">*</span> &nbsp; Email Id  :</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label><span style="color: red;">*</span> &nbsp; Mobile No :</label>
                    <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" Width="100%" Enabled="false" MaxLength="10"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label><span style="color: red;">*</span> &nbsp; WhatsApp No :</label>
                    <asp:TextBox ID="txtWpNo" runat="server" CssClass="form-control" Width="100%" MaxLength="15"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label><span style="color: red;">*</span> &nbsp; State :</label>
                    <asp:DropDownList ID="ddrState" runat="server" CssClass="form-control" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddrState_SelectedIndexChanged">
                        <asp:ListItem Value="0"><- Select -></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group col-md-6">
                    <label><span style="color: red;">*</span> &nbsp; District :</label>
                    <asp:DropDownList ID="ddrDistrict" runat="server" CssClass="form-control" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddrDistrict_SelectedIndexChanged">
                        <asp:ListItem Value="0"><- Select -></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group col-md-6">
                    <label><span style="color: red;">*</span> &nbsp; City :</label>
                    <asp:DropDownList ID="ddrCity" runat="server" CssClass="form-control" Width="100%">
                        <asp:ListItem Value="0"><- Select -></asp:ListItem>
                    </asp:DropDownList>
                </div>
               

                 <div class="form-group col-md-6">
                        <label><span style="color: red;">*</span> &nbsp; Education Of Owner :</label><br />
                        <asp:RadioButton ID="rdbEduTenth" runat="server" GroupName="education" Text=" 10 - 12th" CssClass="radio Space" />
                        <asp:RadioButton ID="rdbEduGraduate" runat="server" GroupName="education" Text=" Graduate" CssClass="radio Space" />
                        <asp:RadioButton ID="rdbEduPG" runat="server" GroupName="education" Text=" Post Graduate" CssClass="radio Space" />
                        <asp:RadioButton ID="rdbEduOther" runat="server" GroupName="education" Text=" Other" CssClass="radio Space" />
                    </div>

                <%--<div class="float_clear"></div>--%>
                <div class="form-group col-md-6">
                    <label><span style="color: red;">*</span> &nbsp; Owner Occupation  :</label>
                    <asp:TextBox ID="txtownrOccuption" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
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
                    </div>

                <%-- Upload Documnets starts --%>
                <div class="form-group col-md-6">
                    <label>ProfilePic  :</label>
                    <span class="subNotice">Upload Profile Pic</span>
                    <span class="subNotice">File size Less than 5 MB</span>
                    <asp:FileUpload ID="fuprofilePic" runat="server" CssClass="mrg_B_15" />
                    <span class="space5"></span>
                    <%= profile %>
                </div>
                <div class="form-group col-md-6">
                    <label>Resume :</label>
                    <span class="subNotice">Upload Resume </span>
                    <span class="subNotice">File size Less than 5 MB</span>
                    <asp:FileUpload ID="fuResume" runat="server" CssClass="mrg_B_15" />
                    <span class="space5"></span>
                    <%= resume %>
                </div>
                <div class="form-group col-md-6">
                    <label>Address  Proof  :</label>
                    <span class="subNotice">please attach mobile photo or scanned copy of Adhar Card / PAN Card (Compulsory)</span>
                    <span class="subNotice">File size Less than 5 MB</span>
                    <asp:FileUpload ID="fuAddProof" runat="server" CssClass="mrg_B_15" />
                     <span class="space5"></span>
                    <asp:FileUpload ID="fuAddProof1" runat="server" CssClass="mrg_B_15" />
                    <span class="space5"></span>
                    <%= addrProof %>
                </div>
                <div class="form-group col-md-6">
                    <label>Id Proof :</label>
                    <span class="subNotice">please attach mobile photo or scanned copy of Adhar Card / PAN Card (Compulsory)</span>
                    <span class="subNotice">File size Less than 5 MB</span>
                    <asp:FileUpload ID="fulIdProof" runat="server" CssClass="mrg_B_15" />
                     <span class="space5"></span>
                    <asp:FileUpload ID="fulIdProof1" runat="server" CssClass="mrg_B_15" />
                    <span class="space5"></span>
                    <%= idProof %>
                </div>
                <%-- Upload Documnets starts --%>
            </div>


            <%-- Bank Details starts --%>
            <div class="form-row">
                <span class="space15"></span>
                <h3 class="pgTitle" style="color: blue;">Payment Details</h3>
                <div class="form-row">
                    <div class="form-group col-md-12">
                        <h4 class="line-ht-5 info">If you have already paid GOBP registration Fees, then fill the following details,
                <br />
                            otherwise click here to make <a href="../images/QR-Code-Genericart.jpg" data-fancybox="imgGroup" class="rm">Online Payment</a></h4>
                    </div>
                    <span class="space20"></span>
                    <div class="form-group col-md-6">
                        <label><span style="color: red;">*</span> &nbsp; UTR No. :</label>
                        <asp:TextBox ID="txtUTR" runat="server" CssClass="form-control required-input" Width="100%" MaxLength="50" placeholder="UTR No."></asp:TextBox>
                    </div>

                    <div class="form-group col-md-6">
                        <label>Bank Name :</label>
                        <asp:TextBox ID="txtBank" runat="server" CssClass="form-control" Width="100%" MaxLength="50" placeholder="Bank Name"></asp:TextBox>
                    </div>

                    <div class="form-group col-md-6">
                        <label><span style="color: red;">*</span> &nbsp; Transaction Date. :</label>
                        <asp:TextBox ID="txtTrDate" runat="server" CssClass="form-control required-input" Width="100%" MaxLength="50" placeholder="Click Here to open Calendar"></asp:TextBox>
                    </div>

                    <div class="form-group col-md-6">
                        <label>Account Holder Name :</label>
                        <asp:TextBox ID="txtHolderName" runat="server" CssClass="form-control" Width="100%" MaxLength="50" placeholder="Account Holder Name"></asp:TextBox>
                    </div>

                    <div class="form-group col-md-6">
                        <label><span style="color: red;">*</span> &nbsp; Amount :</label>
                        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control required-input" Width="100%" MaxLength="50" placeholder="Amount"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-6"></div>
                    <span class="space15"></span>
                </div>


            
            </div>

        <%-- Bank Details ends --%>




        
    </div>
    </div>
    <span class="space10"></span>
    <%= errMsg %>
    <span class="space10"></span>
    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Update Info" OnClick="btnSave_Click" />
    <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-outline-info" Text="Delete" OnClientClick="return confirm('Are you sure to delete?');" OnClick="btnDelete_Click" />
    <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-outline-dark" Text="Cancel" OnClick="btnCancel_Click" />
 


             </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript">
        <%--function handleDropdownChange() {
            var ddl = document.getElementById('<%= ddrDistrictHead.ClientID %>');
            var selectedValue = ddl.value;
            //alert(selectedValue);
            // 122: Lakshaman Ambi MLM Type DH
            if (selectedValue == '122') {
                document.getElementById('parentoption').style.display = 'block';
            }
            else {
                document.getElementById('parentoption').style.display = 'none';
            }

            localStorage.setItem('ddrDistrictHead', selectedValue);
        }--%>

        function restoreDdrItemValue() {
            var storedValue = localStorage.getItem('ddrDistrictHead');
            if (storedValue == '122') {
                document.getElementById('parentoption').style.display = 'block';
            }
            else {
                document.getElementById('parentoption').style.display = 'none';
            }
        }
    </script>
</asp:Content>

