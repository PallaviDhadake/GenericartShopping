<%@ Page Title="Upload Customers" Language="C#" MasterPageFile="~/obp/MasterObp.master" AutoEventWireup="true" CodeFile="upload-customers-data.aspx.cs" Inherits="obp_upload_customers_data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Upload Purchase / Sales Report</h2>
    <span class="space15"></span>
    <div class="card card-primary">
        <div class="card-header">
            <h3 class="card-title">Upload Customers Data</h3>
        </div>
        <div class="card-body">
            <div class="col-sm-7">
                <span class="formLable dspBlk mrgBtm10">Select Excel File :</span>
                <asp:FileUpload ID="fuFileCust" runat="server" />
                <span class="space20"></span>
                <asp:Button ID="btnUploadCust" runat="server" Text="Submit" CssClass="btn btn-md btn-info" OnClick="btnUploadCust_Click"  />
            </div>
        </div>
    </div>
    <span class="space20"></span>
    <%= errMsg %>
    <%= errMsg2 %>
    <span class="space20"></span>
</asp:Content>

