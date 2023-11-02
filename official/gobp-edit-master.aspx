<%@ Page Title="" Language="C#" MasterPageFile="~/official/MasterOfficial.master" AutoEventWireup="true" CodeFile="gobp-edit-master.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="official_gobp_edit_master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <script type="text/javascript">
        $(function () {
            $('#<% =txtBirthDate.ClientID%>').datepick({ onSelect: function (dates) { getAge() }, dateFormat: 'dd/mm/yyyy' });
            $('#<%=txtTrDate.ClientID %>').datepick({ dateFormat: 'dd/mm/yyyy' });
        });
     </script>

    <style type="text/css">
        .Space label
        {
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

    <script>
        window.onload = function () {
            restoreDdrItemValue();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">GOBP Registrations</h2>
    <span class="space15"></span>

    <div class="card card-primary">
        <div class="card-header">
            <h3 class="card-title">Edit GOBP Data</h3>
        </div>
        <div class="card-body">
            <div class="colorLightBlue">
                <span>Id :</span>
                <asp:Label ID="lblId" runat="server" Text="[New]"></asp:Label>
            </div>

            <span class="space15"></span>
            <div class="form-row">
                <div class="form-group col-md-6">
                    <label>Join Date :*</label>
                    <asp:TextBox ID="txtJoinDate" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                </div>
                <div class="form-group col-md-6">
                    <label>Franchisee Type : *</label>
                    <asp:DropDownList ID="ddrOpbType" runat="server" CssClass="form-control" Width="100%">
                        <asp:ListItem Value="0"><- Select -></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group col-md-6">
                    <label>Applicant Name :*</label>
                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Firm Name :</label>
                    <asp:TextBox ID="txtshopName" runat="server" CssClass="form-control" Width="100%" MaxLength="70"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Firm Type : *</label><span class="space10"></span>
                    <asp:RadioButton ID="rdbProprietor" runat="server" GroupName="firmType" Text=" Proprietor" CssClass="radio" />
                    <asp:RadioButton ID="rdbPartner" runat="server" GroupName="firmType" Text=" Partner" CssClass="radio" />
                    <asp:RadioButton ID="rdbTrust" runat="server" GroupName="firmType" Text=" Trust" CssClass="radio" />
                    <asp:RadioButton ID="rdbOther" runat="server" GroupName="firmType" Text=" Other" CssClass="radio" />
                </div>
                <div class="form-group col-md-6">
                    <label>Address :*</label>
                    <asp:TextBox ID="txtAdd" runat="server" CssClass="form-control textarea" TextMode="MultiLine" Height="150" Width="100%"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Birth Date :*</label>
                    <asp:TextBox ID="txtBirthDate" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                </div>

                <div class="form-group col-md-6">
                    <label>Age :*</label>
                    <asp:TextBox ID="txtAge" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Marital Status :*</label><span class="space10"></span>
                    <asp:RadioButton ID="rdbSingle" runat="server" GroupName="maritalStatus" Text=" Single" CssClass="radio" />
                    <asp:RadioButton ID="rdbMarried" runat="server" GroupName="maritalStatus" Text=" Married" CssClass="radio" />
                </div>
                <div class="form-group col-md-6">
                    <label>Email Id  :*</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Mobile No :*</label>
                    <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" Width="100%" MaxLength="10"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>WhatsApp No :*</label>
                    <asp:TextBox ID="txtWpNo" runat="server" CssClass="form-control" Width="100%" MaxLength="15"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>State :*</label>
                    <asp:DropDownList ID="ddrState" runat="server" CssClass="form-control" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddrState_SelectedIndexChanged" >
                        <asp:ListItem Value="0"><- Select -></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group col-md-6">
                    <label>District :*</label>
                    <asp:DropDownList ID="ddrDistrict" runat="server" CssClass="form-control" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddrDistrict_SelectedIndexChanged" >
                        <asp:ListItem Value="0"><- Select -></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group col-md-6">
                    <label>City :*</label>
                    <asp:DropDownList ID="ddrCity" runat="server" CssClass="form-control" Width="100%">
                        <asp:ListItem Value="0"><- Select -></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group col-md-6">
                    <label>User Id :</label>
                    <asp:TextBox ID="txtUserId" runat="server" CssClass="form-control" Width="100%" MaxLength="30" BackColor="#ffd6e1" Enabled="false"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Password  :</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                </div>
                <%--<div class="form-group col-md-6">
                    <label>Shop Code  :</label>
                    <asp:TextBox ID="txtShopCode" runat="server" CssClass="form-control" Width="100%" MaxLength="50" BackColor="#ffd6e1" Enabled="false"></asp:TextBox>
                </div>--%>
                <div class="form-group col-md-6">
                    <label>Education Of Owner :</label><span class="space10"></span>
                    <asp:RadioButton ID="rdbEduTenth" runat="server" GroupName="education" Text=" 10 - 12th" CssClass="radio" />
                    <asp:RadioButton ID="rdbEduGraduate" runat="server" GroupName="education" Text=" Graduate" CssClass="radio" />
                    <asp:RadioButton ID="rdbEduPG" runat="server" GroupName="education" Text=" Post Graduate" CssClass="radio" />
                    <asp:RadioButton ID="rdbEduOther" runat="server" GroupName="education" Text=" Other" CssClass="radio" />
                </div>
                <%--<div class="float_clear"></div>--%>
                <div class="form-group col-md-6">
                    <label>Owner Occupation  :*</label>
                    <asp:TextBox ID="txtownrOccuption" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Any Illegal Matters  :*</label><span class="space10"></span>
                    <asp:RadioButton ID="rdbMatterYes" runat="server" GroupName="legalMatter" Text=" Yes" CssClass="radio" />
                    <asp:RadioButton ID="rdbMatterNo" runat="server" GroupName="legalMatter" Text=" No" CssClass="radio" />
                </div>
                <div class="form-group col-md-6">
                    <label>Residence From  :*</label><span class="space10"></span>
                    <asp:RadioButton ID="rdbBelow5Yr" runat="server" GroupName="residence" Text=" 0 - 5 Years" CssClass="radio" />
                    <asp:RadioButton ID="rdb5Yr" runat="server" GroupName="residence" Text=" 5 - 10 Years" CssClass="radio" />
                    <asp:RadioButton ID="rdb10Yr" runat="server" GroupName="residence" Text=" More than 10 Years" CssClass="radio" />
                </div>
                <div class="form-group col-md-6">
                    <label>UTR No :*</label>
                    <asp:TextBox ID="txtUTR" runat="server" CssClass="form-control" Width="100%" MaxLength="30"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Bank Name  :*</label>
                    <asp:TextBox ID="txtBank" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Transaction Date :*</label>
                    <asp:TextBox ID="txtTrDate" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Account Holder Name :*</label>
                    <asp:TextBox ID="txtHolderName" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Amount :*</label>
                    <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Sales Incentive :</label>
                    <asp:TextBox ID="txtSalesInc" runat="server" CssClass="form-control" Width="100%" MaxLength="30"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Referral  :</label>
                    <asp:TextBox ID="txtReferal" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Total Purchase  :</label>
                    <asp:TextBox ID="txtPurchase" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Remark  :</label>
                    <asp:TextBox ID="txtRemark" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                </div>
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
                    <label>Address  Proof  :*</label>
                    <span class="subNotice">please attach mobile photo or scanned copy of Adhar Card / PAN Card (Compulsory)</span>
                    <span class="subNotice">File size Less than 5 MB</span>
                    <asp:FileUpload ID="fuAddProof" runat="server" CssClass="mrg_B_15" />
                    <asp:FileUpload ID="fuAddProof1" runat="server" CssClass="mrg_B_15" />
                    <span class="space5"></span>
                    <%= addrProof %>
                </div>
                <div class="form-group col-md-6">
                    <label>Id Proof :*</label>
                    <span class="subNotice">please attach mobile photo or scanned copy of Adhar Card / PAN Card (Compulsory)</span>
                    <span class="subNotice">File size Less than 5 MB</span>
                    <asp:FileUpload ID="fulIdProof" runat="server" CssClass="mrg_B_15" />
                    <asp:FileUpload ID="fulIdProof1" runat="server" CssClass="mrg_B_15" />
                    <span class="space5"></span>
                    <%= idProof %>
                </div>
                
                
                <!-- ========= Bank Account Details =========== -->
                <div class="form-group col-md-6">
                    <h3 class="text-primary">Bank Account Details</h3>
                </div>
                 <div class="form-group col-md-6"></div>

                <div class="form-group col-md-6">
                    <label>Bank Account Type  :*</label>
                    <asp:RadioButton ID="rdbSaving" runat="server" GroupName="BankAccType" Text=" Saving" CssClass="radio Space"  Checked="True" />
                    <asp:RadioButton ID="rdbCurrent" runat="server" GroupName="BankAccType" Text=" Current" CssClass="radio Space" />
                </div>
                <div class="form-group col-md-6">
                </div>
                <div class="form-group col-md-6">
                    <label>Bank Name :*</label>
                    <asp:TextBox ID="txtBankNameInfo" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Account Name :*</label>
                    <asp:TextBox ID="txtAccName" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Account Number :*</label>
                    <asp:TextBox ID="txtAccNumber" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>IFSC Code :*</label>
                    <asp:TextBox ID="txtIFSC" runat="server" CssClass="form-control" Width="100%" MaxLength="30"></asp:TextBox>
                </div>

                <div class="form-group col-md-6">
                    <h3 class="text-primary">Assign District Head</h3>
                </div>
                 <div class="form-group col-md-6"></div>

                <div class="form-group col-md-6">
                    <label>District Head : *</label>
                   <%=myDistrictHead %>
                     <asp:DropDownList ID="ddrDistrictHead" runat="server" CssClass="form-control" Width="100%" AutoPostBack="false" onchange="handleDropdownChange()" >
                        <asp:ListItem Value="0"><- Select -></asp:ListItem>
                    </asp:DropDownList>
                <span class="space5"></span>
                </div>

                <div id="parentoption" class="form-group col-md-6">
                    <label>Parent GOBP :</label>
                    <asp:TextBox ID="txtParentGobp" runat="server" CssClass="form-control" Width="100%" MaxLength="50" BackColor="#F3EE8E" maxlenght="15"></asp:TextBox>
                </div>
            </div>
            <!-- Corporate Commission Input starts -->
                <div class="form-row">
                    <div class="form-group col-md-6">
                        <h3 class="text-primary">Corporate GOBP Commission</h3>
                    </div>
                    <div class="form-group col-md-6"></div>

                     <div class="form-group col-md-6">
                        <label>Medicine Product Commission (%) :</label>
                       <asp:TextBox ID="txtCorpComm" runat="server" CssClass="form-control" Width="100%" MaxLength="15" onkeypress="return validateNumber(event)" placeholder="Numeric values only"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-6"></div>
                </div>
                <!-- Corporate Commission Input ends -->
            
            <div class="form-row">
                 <div class="form-group col-md-6">
                    <asp:CheckBox ID="chkactivateAc" runat="server" Text="&nbsp; Activate Account" TextAlign="Right" />
                </div>
            </div>
        </div>
    </div>
    <span class="space10"></span>
    <%= errMsg %>
    <span class="space10"></span>
    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Update Info" OnClick="btnSave_Click"  />
    <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-outline-info" Text="Delete" OnClientClick="return confirm('Are you sure to delete?');" OnClick="btnDelete_Click" />
    <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-outline-dark" Text="Cancel" OnClick="btnCancel_Click"  />
   <%-- <asp:Button ID="btnEcomm" runat="server"
        CssClass="btn btn-warning mrgRgt10" Text="Update Info to ECOMM" OnClick="btnEcomm_Click"/>--%>

    <script type="text/javascript">
        function handleDropdownChange() {
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
        }

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

