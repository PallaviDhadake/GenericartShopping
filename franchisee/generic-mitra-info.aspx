<%@ Page Title="GenericMitra Information | Ecommerce Genericart Portal" Language="C#" MasterPageFile="~/franchisee/MasterFranchisee.master" AutoEventWireup="true" CodeFile="generic-mitra-info.aspx.cs" Inherits="franchisee_generic_mitra_info" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Generic Mitra Information</h2>
	<span class="space15"></span>
    <div class="card">
    <div class="card-body p-0">
        <table class="table table-sm">
            <thead>
                <tr>
                    <th style="width:20% ">Information</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>Full Name:</td>
                    <td><span class="fs-3"><%=gmInfo[0] %></span></td>
                </tr>
                <tr>
                    <td>Mobile No.:</td>
                    <td><%=gmInfo[1] %></td>
                </tr>
                <tr>
                    <td>Email Id:</td>
                    <td><%=gmInfo[2] %></td>
                </tr>
                <tr>
                    <td>State:</td>
                    <td><%=gmInfo[3] %></td>
                </tr>
                 <tr>
                    <td>District:</td>
                    <td><%=gmInfo[4] %></td>
                </tr>
                 <tr>
                    <td>City:</td>
                    <td><%=gmInfo[5] %></td>
                </tr>
                 <tr>
                    <td>PAN Card:</td>
                    <td><span class="badge bg-success"><%=gmInfo[6] %></span></td>
                </tr>
                 <tr>
                    <td>Aadhar Card:</td>
                    <td><span class="badge bg-success"><%=gmInfo[7] %></span></td>
                </tr>
                 <tr>
                    <td>Bank Document:</td>
                    <td><span class="badge bg-success"><%=gmInfo[8] %></span></td>
                </tr>
            </tbody>
        </table>
    </div>
</div>
</asp:Content>

