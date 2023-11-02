<%@ Page Title="Medicine Survey Entry" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="survey-medicine-entry.aspx.cs" Inherits="admingenshopping_survey_medicine_entry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Medicine Survey Entry</h2>
    <span class="space15"></span>
    <div class="card card-primary">
        <div class="card-header">
            <h3 class="card-title">Medicine Survey Entry</h3>
        </div>
        <div class="card-body">
            <div class="col-sm-7">
                 <span class="bold_weight semiMedium themeClrBlue mrg_B_15 dspBlk">Last Uploaded On : <%= csvUploadDate %></span>
                <span class="formLable dspBlk mrgBtm10">Select Excel File :</span>
                <asp:FileUpload ID="fuFile" runat="server" />
                <span class="space10"></span>
            </div>
        </div>
    </div>
    <span class="space20"></span>   
    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-md btn-primary" OnClick="btnSubmit_Click"  />
    <span class="space40"></span>
    <%= errMsg %>
    <%= errMsg2 %>
</asp:Content>

