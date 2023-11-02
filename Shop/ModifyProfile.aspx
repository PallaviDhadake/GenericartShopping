<%@ Page Language="C#" MasterPageFile="~/Shop/ShopMain.master" AutoEventWireup="true" CodeFile="ModifyProfile.aspx.cs" Inherits="ModifyProfile" Title="Modify Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        @media (max-width :600px) {
            .col-xs-12 {
                margin-bottom: 10px;
            }
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
                    <div class="col-md-1 col-xs-12 text-bold">Name</div>
                    <div class="col-md-3 col-xs-12">
                        <asp:Label ID="TxtName" runat="server" CssClass="form-control"></asp:Label>
                    </div>
                    <div class="col-md-2 col-xs-12 text-bold">Father's Name</div>
                    <div class="col-md-3 col-xs-12">
                        <asp:TextBox ID="TxtFathersName" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div style="clear: both; height: 10px;"></div>
                    <div class="col-md-1 col-xs-12 text-bold">Sex</div>
                    <div class="col-md-2 col-xs-12">
                        <asp:Label ID="TxtSex" runat="server" Text="" CssClass="form-control"></asp:Label>
                    </div>
                    <div class="col-md-1 col-xs-12 text-bold">D O B</div>
                    <div class="col-md-2 col-xs-12">
                        <asp:TextBox ID="TxtDOB" runat="server" CssClass="form-control date-picker" data-date-format="dd-mm-yyyy"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="Req1" runat="server" ControlToValidate="TxtDOB" ErrorMessage="Select Date of Birth" SetFocusOnError="true" Display="Dynamic" ValidationGroup="EditProfile"></asp:RequiredFieldValidator>
                        <script>
                            var picker = new Pikaday(
                                {
                                    field: document.getElementById('<%= TxtDOB.ClientID %>'),
                                    firstDay: 1,
                                    minDate: new Date('01-01-1950'),
                                    maxDate: new Date('30-12-2020'),
                                    yearRange: [2000, 2020]
                                });
                        </script>
                    </div>
                    <div class="col-md-1 col-xs-12 text-bold">Doc No</div>
                    <div class="col-md-2 col-xs-12">
                        <asp:TextBox ID="TxtAadharNo" runat="server" CssClass="form-control" MaxLength="12"></asp:TextBox>
                    </div>
                    <div style="clear: both; height: 2px;"></div>
                    <div class="col-md-12 header text-bold">Contact Address</div>
                    <div class="col-md-1 col-xs-12">Address</div>
                    <div class="col-md-11 col-xs-12">
                        <asp:TextBox ID="TxtAddress" runat="server" TextMode="MultiLine" CssClass="form-control"> </asp:TextBox>
                    </div>
                    <div style="clear: both; height: 2px;"></div>
                    <div class="col-md-12 header">Contact Details</div>
                    <div class="col-md-1 col-xs-12 text-bold">Email Id</div>
                    <div class="col-md-3 col-xs-12">
                        <asp:Label ID="TxtEmailId" runat="server" CssClass="form-control"></asp:Label>
                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtEmailId" ErrorMessage="Ex.:abc@xyz.com" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="EditProfile" Display="Dynamic"></asp:RegularExpressionValidator>--%>
                    </div>
                    <div class="col-md-1 col-xs-12">Mobile No</div>
                    <div class="col-md-3 col-xs-12">
                        <asp:Label ID="TxtMobileNo" runat="server" CssClass="form-control"></asp:Label>
                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator15" CssClass="absolute_validator" runat="server" ErrorMessage="Please Enter Correct Mobile No." ControlToValidate="TxtMobileNo" ValidationExpression="^[0-9\-\(\)\, ]+$" ValidationGroup="EditProfile" Display="Dynamic">*</asp:RegularExpressionValidator>--%>
                    </div>
                    <div style="clear: both; height: 5px;"></div>
                    <div class="col-md-2 col-xs-12">
                        <asp:Button ID="BtnSubmit" runat="server" Text="Update" OnClick="TxtSubmit_Click" CssClass="btn btn-primary" ValidationGroup="EditProfile" Visible="false" />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="EditProfile" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div style="clear: both !important;"></div>
</asp:Content>
