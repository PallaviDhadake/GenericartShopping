<%@ Page Title="Upload GOBP Excel CSV data | Online Business Partner" Language="C#" MasterPageFile="~/obp/MasterObp.master" AutoEventWireup="true" CodeFile="add-customer-excel.aspx.cs" Inherits="obp_add_customer_excel" %>
<%@ MasterType VirtualPath="~/obp/MasterObp.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <div class="col-lg-6">
            <div class="card card-primary">
                <div class="card-header">
                    <h3 class="card-title">Upload OBP .CSV List</h3><br />
                    <p>Only English language text will be supported</p>
                </div>
                <div class="card-body">
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label>Select Excel File :</label>
                            <asp:FileUpload ID="fuObpList" runat="server" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <asp:Button ID="btnUploadMedProd" runat="server" Text="Upload List" CssClass="btn btn-md btn-info" OnClick="btnUploadMedProd_Click"  />
                        </div>
                    </div>
                    <%=errMsg2 %>
                </div>
            </div>
        </div>
        <span class="space15"></span>
         <div>
             <p>Example of CSV format as below.</p>
             <img alt="" src="../images/customer-csv.png" style="width:400px" />
         </div>
</asp:Content>

