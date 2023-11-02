<%@ Page Title="Add New GOBP | Genericart Medicines India Pvt Ltd" Language="C#" MasterPageFile="~/obp/MasterObp.master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeFile="add-gobp.aspx.cs" Inherits="obp_add_gobp" %>
<%@ MasterType VirtualPath="~/obp/MasterObp.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        $(document).ready(function () {
            $('[id$=gvAddGOBP]').DataTable({
                columnDefs: [
                    { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7] }
                ],
                order: [[0, 'desc']]
            });
        });
    </script>
    
    <script type="text/javascript">
         function setupcalendar() {
            <%-- $('#<% =txtBirthDate.ClientID%>').datepick({ onSelect: function (dates) { getAge() }, dateFormat: 'dd/mm/yyyy' });--%>
             $('#<%=txtTrDate.ClientID %>').datepick({ dateFormat: 'dd/mm/yyyy' });
         }
     </script>

    <script type="text/javascript">

        <%--function getAge() {
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
        }--%>
        function alpha(e) {
            var k;
            document.all ? k = e.keyCode : k = e.which;
            return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57));
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

    <style type="text/css">
        .Space label {
            margin-left: 5px;
            margin-right: 15px;
        }
        .invalid {
            border:solid 2px red !important;
        }
    </style>



   

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">Add GOBP</h2>
    <span class="space15"></span>
    <div id="myForm" runat="server" >
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>

            <div id="joinlevel" runat="server">
                <span class="pgTitle">Select Joining Level </span>
                <span class="space10"></span>
                <div class="row">
                    <asp:DropDownList ID="ddrJoinLevel" CssClass="form-control w-25 mr-3" runat="server">
                        <asp:ListItem Value="0">-- Show All --</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button ID="BtnShow" CssClass="btn btn-info" runat="server" Text="Show" OnClick="BtnShow_Click" />
                </div>
                <span class="space20"></span>
            </div>

            <div id="editdata" runat="server">
                <div class="colorLightBlue">
                    <span>Id :</span>
                    <asp:Label ID="lblId" runat="server" Text="[New]"></asp:Label>
                </div>
                <span class="space15"></span>
                <div class="form-row">
                    <div class="form-group col-md-6">
                        <label><span style="color: red;">*</span> &nbsp; Applicant Name :</label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control required-input" Width="100%" MaxLength="50" placeholder="Applicant Name" Required="true"></asp:TextBox>
                    </div>

                    <div class="form-group col-md-6">
                        <label>Firm Name : (If you have registered already, please enter firm name)</label>
                        <asp:TextBox ID="txtshopName" runat="server" CssClass="form-control" Width="100%" MaxLength="50" placeholder="Firm Name"></asp:TextBox>
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
                        <asp:TextBox ID="txtAdd" runat="server" CssClass="form-control required-input" Width="100%" MaxLength="50" TextMode="MultiLine" Height="80" placeholder="Address"></asp:TextBox>
                    </div>

                   <%-- <div class="form-group col-md-6">
                        <label><span style="color: red;">*</span> &nbsp; Birth Date : </label>
                        <asp:TextBox ID="txtBirthDate" runat="server" CssClass="form-control required-input" Width="100%" MaxLength="50" placeholder="Birth Date"></asp:TextBox>
                    </div>

                    <div class="form-group col-md-6">
                        <label><span style="color: red;">*</span> &nbsp; Age :</label>
                        <asp:TextBox ID="txtAge" runat="server" CssClass="form-control" Width="100%" MaxLength="50" placeholder="Age"></asp:TextBox>
                    </div>--%>

                    <div class="form-group col-md-6">
                        <label><span style="color: red;">*</span> &nbsp; Marital Status :</label><br />
                        <asp:RadioButton ID="rdbSingle" runat="server" GroupName="maritalStatus" Text=" Single" CssClass="radio Space" />
                        <asp:RadioButton ID="rdbMarried" runat="server" GroupName="maritalStatus" Text=" Married" CssClass="radio Space" />
                    </div>

                    <div class="form-group col-md-6">
                        <label><span style="color: red;">*</span> &nbsp; Email Id :</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control required-input" Width="100%" MaxLength="50" placeholder="Email"></asp:TextBox>
                    </div>

                    <div class="form-group col-md-6">
                        <label><span style="color: red;">*</span> &nbsp; Mobile No :</label>
                        <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control required-input" Width="100%" MaxLength="10" placeholder="Mobile No."></asp:TextBox>
                    </div>

                    <div class="form-group col-md-6">
                        <label><span style="color: red;">*</span> &nbsp; WhatsApp No :</label>
                        <asp:TextBox ID="txtWpNo" runat="server" CssClass="form-control required-input" Width="100%" MaxLength="10" placeholder="Whatapps No."></asp:TextBox>
                    </div>

                    <div class="form-group col-md-6">
                        <label><span style="color: red;">*</span> &nbsp;State :</label>
                        <asp:DropDownList ID="ddrState" runat="server" CssClass="form-control required-input" AutoPostBack="True" OnSelectedIndexChanged="ddrState_SelectedIndexChanged">
                            <asp:ListItem Value="0"><- Select -></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="form-group col-md-6">
                        <label><span style="color: red;">*</span> &nbsp; District :</label>
                        <asp:DropDownList ID="ddrDistrict" CssClass="form-control required-input" runat="server"
                            AutoPostBack="True" OnSelectedIndexChanged="ddrDistrict_SelectedIndexChanged">
                            <asp:ListItem Value="0"><-Select-></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="form-group col-md-6">
                        <label><span style="color: red;">*</span> &nbsp; City :</label>
                        <asp:DropDownList ID="ddrCity" CssClass="form-control required-input" runat="server" AutoPostBack="True">
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
                        <label>Owner Occupation :</label>
                        <asp:TextBox ID="txtownrOccuption" runat="server" CssClass="form-control" Width="100%" MaxLength="50" placeholder="Occupation"></asp:TextBox>
                    </div>

                    <div class="form-group col-md-6">
                        <label><span style="color: red;">*</span> &nbsp; Any Illegal Matters :</label><br />
                        <asp:RadioButton ID="rdbMatterYes" runat="server" GroupName="legalMatter" Text=" Yes" CssClass="radio Space" />
                        <asp:RadioButton ID="rdbMatterNo" runat="server" GroupName="legalMatter" Text=" No" CssClass="radio Space" />
                    </div>

                    <div class="form-group col-md-12">
                        <label><span style="color: red;">*</span> &nbsp; Residence From :</label><br />
                        <asp:RadioButton ID="rdbBelow5Yr" runat="server" GroupName="residence" Text=" 0 - 5 Years" CssClass="radio Space" />
                        <asp:RadioButton ID="rdb5Yr" runat="server" GroupName="residence" Text=" 5 - 10 Years" CssClass="radio Space" />
                        <asp:RadioButton ID="rdb10Yr" runat="server" GroupName="residence" Text=" More than 10 Years" CssClass="radio Space" />
                    </div>

                    <span class="space20"></span>

                    <div class="form-group col-md-6">

                        <label>Profile Pic :</label><br />
                        <span class="subNotice">Upload Profile Pic</span><br />
                        <span class="subNotice">File size Less than 5 MB</span>
                        <span class="space5"></span>
                        <asp:FileUpload ID="fuprofilePic" runat="server" CssClass="mrg_B_15" />
                        <span class="space5"></span>
                        <%=profilepic %>
                    </div>

                    <div class="form-group col-md-6">

                        <label>Resume  :</label><br />
                        <span class="subNotice">Upload Resume </span>
                        <br />
                        <span class="subNotice">File size Less than 5 MB</span>
                        <span class="space5"></span>
                        <asp:FileUpload ID="fuResume" runat="server" CssClass="mrg_B_15" />
                        <span class="space5"></span>
                        <%=resume %>
                    </div>
                    <%-- <div class="float_clear"></div>--%>

                    <div class="form-group col-md-6">

                        <label> Address  Proof :</label><br />
                        <%--<span class="labelCap">Address  Proof :<span class="redStar">*</span> </span>--%>
                        <span class="subNotice">please attach mobile photo or scanned copy of Adhar Card / PAN Card (Compulsory)</span>
                        <span class="subNotice">File size Less than 5 MB</span>
                        <span class="space5"></span>
                        <asp:FileUpload ID="fuAddProof" runat="server" CssClass="mrg_B_15" />
                        <span class="space5"></span>
                         <%=addproof%>
                        <span class="space5"></span>
                        <asp:FileUpload ID="fuAddProof1" runat="server" CssClass="mrg_B_15" />
                         <span class="space5"></span>
                        <%=addproof1%>
                    </div>

                    <div class="form-group col-md-6">
                        <label>Id  Proof :</label><br />
                        <%--<span class="labelCap">Id  Proof :<span class="redStar">*</span> </span>--%>
                        <span class="subNotice">please attach mobile photo or scanned copy of Adhar Card / PAN Card (Compulsory)</span>
                        <span class="subNotice">File size Less than 5 MB</span>
                        <span class="space5"></span>
                        <asp:FileUpload ID="fulIdProof" runat="server" CssClass="mrg_B_15" />
                        <span class="space5"></span>
                        <%=idproof%>
                        <span class="space5"></span>
                        <asp:FileUpload ID="fulIdProof1" runat="server" CssClass="mrg_B_15" />
                         <span class="space5"></span>
                        <%=idproof1%>

                    </div>
                </div>

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
                        <label> Bank Name :</label>
                        <asp:TextBox ID="txtBank" runat="server" CssClass="form-control" Width="100%" MaxLength="50" placeholder="Bank Name"></asp:TextBox>
                    </div>

                    <div class="form-group col-md-6">
                        <label><span style="color: red;">*</span> &nbsp; Transaction Date. :</label>
                        <asp:TextBox ID="txtTrDate" runat="server" CssClass="form-control required-input" Width="100%" MaxLength="50" placeholder="Click Here to open Calendar"></asp:TextBox>
                    </div>

                    <div class="form-group col-md-6">
                        <label> Account Holder Name :</label>
                        <asp:TextBox ID="txtHolderName" runat="server" CssClass="form-control" Width="100%" MaxLength="50" placeholder="Account Holder Name"></asp:TextBox>
                    </div>

                    <div class="form-group col-md-6">
                        <label><span style="color: red;">*</span> &nbsp; Amount :</label>
                        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control required-input" Width="100%" MaxLength="50" placeholder="Amount"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-6"></div>
                    <span class="space15"></span>
                   
                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary mr-2" Text="Submit" OnClick="btnSubmit_Click" />
                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-outline-dark" Text="Cancel" OnClick="btnCancel_Click1" />
            </div>
            </div>

            <!-- Grid view starts -->

       

    <div id="viewdata" runat="server">
        <a href="add-gobp.aspx?action=new" runat="server" class="btn btn-sm btn-primary">Add New</a>
        <span class="space15"></span>
        <div class="formPanel">
            <asp:GridView ID="gvAddGOBP" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
                AutoGenerateColumns="false" Width="100%" OnRowDataBound="gvAddGOBP_RowDataBound">
                <RowStyle CssClass="" />
                <HeaderStyle CssClass="bg-dark" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="OBP_ID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                     <asp:BoundField DataField="OBP_JoinLevel" HeaderText="Join Level">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OBP_UserID" HeaderText="User Id">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OBP_ApplicantName" HeaderText="Name">
                        <ItemStyle Width="20%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OBP_MobileNo" HeaderText="Mobile No">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OBP_City" HeaderText="City">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                     <asp:BoundField DataField="OBP_StatusFlag" HeaderText="Status">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:TemplateField>
                        <ItemStyle Width="5%" />
                        <ItemTemplate>
                            <asp:Literal ID="litAnch" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <span class="warning">Its Empty Here... :(</span>
                </EmptyDataTemplate>
                <PagerStyle CssClass="gvPager" />
            </asp:GridView>
        </div>
    </div>
             </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSubmit" />
        </Triggers>
    </asp:UpdatePanel>
    </div>

     
   
</asp:Content>

