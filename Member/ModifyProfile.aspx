<%@ Page Language="C#" MasterPageFile="~/Member/MemberMain.master" AutoEventWireup="true" CodeFile="ModifyProfile.aspx.cs" Inherits="ModifyProfile" Title="Modify Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .header {
            font-size: 16px;
            font-weight: bold;
            margin-bottom: 10px;
            margin-top: 10px;
        }

        .col-xs-12 {
            margin-bottom: 7px;
        }

        .col-md-1 {
            padding: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="breadcrumbs">
        <div class="col-sm-4">
            <div class="page-header float-left">
                <div class="page-title">
                    <h1>Profile</h1>
                </div>
            </div>
        </div>
        <div class="col-sm-8">
            <div class="page-header float-right">
                <div class="page-title">
                    <ol class="breadcrumb text-right">
                        <li class="active">Profile</li>
                    </ol>
                </div>
            </div>
        </div>
    </div>
    <div class="content mt-3">
        <div class="col-md-12">
            <div class="box box-primary">
                <div class="box-body">
                    <asp:HiddenField ID="Hf" runat="server" />
                    <div class="col-md-12 header">Personal Details</div>
                    <div class="col-md-1 col-xs-12">Name :</div>
                    <div class="col-md-3 col-xs-12">
                        <asp:Label ID="TxtName" runat="server" CssClass="form-control"></asp:Label>
                    </div>
                    <div class="col-md-1 col-xs-12">Father's Name : </div>
                    <div class="col-md-3 col-xs-12">
                        <asp:TextBox ID="TxtFathersName" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-1 col-xs-12">Introducer's Id : </div>
                    <div class="col-md-3 col-xs-12">
                        <asp:TextBox ID="TxtIntroducerID" runat="server" Text="" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div style="clear: both; height: 10px;"></div>
                    <div class="col-md-1 col-xs-12">Sex : </div>
                    <div class="col-md-3 col-xs-12">
                        <asp:TextBox ID="TxtSex" runat="server" Text="" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-1 col-xs-12">D O B : </div>
                    <div class="col-md-3 col-xs-12">
                        <asp:TextBox ID="TxtDOB" runat="server" CssClass="form-control date-picker" data-date-format="dd-mm-yyyy"></asp:TextBox>
                        <script>
                            var picker = new Pikaday(
                                {
                                    field: document.getElementById('<%= TxtDOB.ClientID %>'),
                                    firstDay: 1,
                                    minDate: new Date('01-01-1950'),
                                    maxDate: new Date('30-12-2020'),
                                    yearRange: [2010, 2030]
                                });
                        </script>
                        <asp:RequiredFieldValidator ID="Req1" runat="server" ControlToValidate="TxtDOB" ErrorMessage="Select Date of Birth" SetFocusOnError="true" Display="Dynamic" ValidationGroup="EditProfile"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-1 col-xs-12">PAN : </div>
                    <div class="col-md-3 col-xs-12">
                        <asp:TextBox ID="TxtPAN" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                    </div>
                    <div class="col-md-2 col-xs-12">Aadhar No : </div>
                    <div class="col-md-3 col-xs-12">
                        <asp:TextBox ID="TxtAadharNo" runat="server" CssClass="form-control" MaxLength="12"></asp:TextBox>
                    </div>
                    <div class="col-md-2 col-xs-12">Profile Photo : </div>
                    <div class="col-md-2 col-xs-12">
                        <asp:FileUpload ID="ImageUpload" runat="server" onchange="readURL(this)" />
                    </div>
                    <div class="col-md-2 col-xs-12">
                        <img id="imgprvw" alt="uploaded image preview" src="<%=ImageShow %>" style="height: 50px;" />
                    </div>
                    <div class="col-md-12 header">Contact Address</div>
                    <div class="col-md-1 col-xs-12">Address : </div>
                    <div class="col-md-11 col-xs-12">
                        <asp:TextBox ID="TxtAddress" runat="server" TextMode="MultiLine" CssClass="form-control"> </asp:TextBox>
                    </div>
                    <div style="clear: both; height: 10px;"></div>
                    <div class="col-md-1 col-xs-12">Country : </div>
                    <div class="col-md-2 col-xs-12">
                        <asp:DropDownList ID="TxtCountry" runat="server" CssClass="form-control" OnSelectedIndexChanged="TxtCountry_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-1 col-xs-12">State : </div>
                    <div class="col-md-2 col-xs-12">
                        <asp:DropDownList ID="TxtState" runat="server" CssClass="form-control" OnSelectedIndexChanged="TxtState_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-1 col-xs-12">City:</div>
                    <div class="col-md-2 col-xs-12">
                        <asp:DropDownList ID="TxtCity" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-1 col-xs-12">Pin Code:</div>
                    <div class="col-md-2 col-xs-12">
                        <asp:TextBox ID="TxtPinCode" runat="server" CssClass="form-control" MaxLength="6"></asp:TextBox>
                    </div>
                    <div class="col-md-12 header">Contact Details</div>
                    <div class="col-md-1 col-xs-12">Email Id:</div>
                    <div class="col-md-3 col-xs-12">
                        <asp:TextBox ID="TxtEmailId" runat="server" CssClass="form-control"></asp:TextBox>
                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtEmailId" ErrorMessage="Ex.:abc@xyz.com" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="EditProfile" Display="Dynamic"></asp:RegularExpressionValidator>--%>
                    </div>
                    <div class="col-md-1 col-xs-12">Mobile No:</div>
                    <div class="col-md-3 col-xs-12">
                        <asp:TextBox ID="TxtMobileNo" runat="server" CssClass="form-control"></asp:TextBox>
                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator15" CssClass="absolute_validator" runat="server" ErrorMessage="Please Enter Correct Mobile No." ControlToValidate="TxtMobileNo" ValidationExpression="^[0-9\-\(\)\, ]+$" ValidationGroup="EditProfile" Display="Dynamic">*</asp:RegularExpressionValidator>--%>
                    </div>
                    <div class="col-md-12 header">Bank Details</div>
                    <div class="col-md-2 col-xs-12">Bank Name</div>
                    <div class="col-md-3 col-xs-12">
                        <asp:TextBox ID="TxtBankName" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-2 col-xs-12">Branch Name</div>
                    <div class="col-md-3 col-xs-12">
                        <asp:TextBox ID="TxtBranchName" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div style="clear: both; height: 10px;"></div>
                    <div class="col-md-2 col-xs-12">Account No : </div>
                    <div class="col-md-3 col-xs-12">
                        <asp:TextBox ID="TxtAccountNo" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-2 col-xs-12">IFSC Code : </div>
                    <div class="col-md-3 col-xs-12">
                        <asp:TextBox ID="TxtIFSCCode" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-12 header">
                        Co-Applicant's Details
                    </div>
                    <div class="col-md-2 col-xs-12">Nominee Name : </div>
                    <div class="col-md-3 col-xs-12">
                        <asp:TextBox ID="TxtNomineeName" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-1 col-xs-12">Relation : </div>
                    <div class="col-md-3 col-xs-12">
                        <asp:DropDownList ID="CmbRelation" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2 col-xs-12">
                        <asp:Button ID="BtnSubmit" runat="server" Text="Update" OnClick="TxtSubmit_Click" CssClass="btn btn-primary" ValidationGroup="EditProfile" />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="EditProfile" />
                    </div>
                    <div style="clear: both; height: 20px;"></div>
                </div>
            </div>
        </div>
    </div>

    <script>
        function readURL(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#imgprvw').attr('src', e.target.result);
                }

                reader.readAsDataURL(input.files[0]);
            }
        }

    </script>
</asp:Content>
