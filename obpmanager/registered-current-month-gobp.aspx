﻿<%@ Page Title="" Language="C#" MasterPageFile="~/obpmanager/MasterObpManager.master" AutoEventWireup="true" CodeFile="registered-current-month-gobp.aspx.cs" Inherits="obpmanager_registered_current_month_gobp" %>
<%@ MasterType VirtualPath="~/obpmanager/MasterObpManager.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
        $(document).ready(function () {

            $('[id$=gvGOBP]').DataTable({
                columnDefs: [
                     { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9] }
                ],
                order: [[0, 'desc']]
            });
        });
    </script>
    <style>
        #followup .user-block .username, #followup .user-block .description {
            margin: 0px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <h2 class="pgTitle">Registered GOBP List</h2>
    <span class="space15"></span>

    <div class="row" id="daterange" runat="server">
        <div class="col-md-3">
            <asp:DropDownList ID="ddrMonth" runat="server" CssClass="form-control">
                <asp:ListItem Value="0"> -- Select Month -- </asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="col-md-3">
            <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="btn btn-md btn-primary" OnClick="btnShow_Click" />
        </div>        
    </div>
    <span class="space20"></span>

     <div id="viewFrEnquiry" runat="server">
        <div class="">
            <asp:GridView ID="gvGOBP" runat="server" AutoGenerateColumns="False"
                CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
                OnRowDataBound="gvGOBP_RowDataBound">
                <HeaderStyle CssClass="bg-dark" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="OBP_ID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="joinDate" HeaderText="Join Date">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OBP_ApplicantName" HeaderText="Applicant Name">
                        <ItemStyle Width="20%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OBP_MobileNo" HeaderText="Mobile No">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="custCount" HeaderText="Total Customers">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="dh" HeaderText="DH">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="dhContact" HeaderText="DH Contact">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OBP_StatusFlag" HeaderText="Status">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="flCount" HeaderText="FL Count">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                    <asp:TemplateField>
                        <ItemStyle Width="5%" />
                        <ItemTemplate>
                            <asp:Literal ID="litView" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                </Columns>
                <EmptyDataTemplate>
                    <span class="warning">No GOBP Enquries to Display</span>
                </EmptyDataTemplate>
                <PagerStyle CssClass="gvPager" />
            </asp:GridView>
        </div>
    </div>

    <div id="readFrEnquiry" runat="server">
        <div class="card card-primary">
            <div class="card-header">
                <h3 class="card-title">GOBP Registration Data</h3>
            </div>
            <div class="card-body">
                <table class="form_table">
                    <tr>
                        <td><span class="colorLightBlue">Id :</span></td>
                        <td><asp:Label ID="lblId" CssClass="colorLightBlue" runat="server" Text="[New]"></asp:Label></td>
                    </tr>
                    <tr>
                        <td><span class="text-lg text-bold text-purple">Type:</span></td>
                        <td><span class="text-lg text-bold text-purple"><%= enqData[0] %></span></td>
                    </tr>
                    <tr>
                        <td style="width:25%" ><span class="formLable bold_weight">Join Date:</span></td>
                        <td ><span class="formLable"><%= enqData[1] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Applicant Name :</span></td>
                        <td><span class="formLable" style="display:block; width:60% !important;"><%= enqData[2] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">GOBP Code :</span></td>
                        <td><span class="formLable"><%= enqData[8] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Mobile No :</span></td>
                        <td><span class="formLable"><%= enqData[12] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Email Id :</span></td>
                        <td><span class="formLable"><%= enqData[13] %></span> </td>
                    </tr>
                     <tr>
                        <td><span class="formLable bold_weight">DH Name :</span></td>
                        <td><span class="formLable"><%= enqData[4] %></span> </td>
                    </tr>
                     <tr>
                        <td><span class="formLable bold_weight">DH Contact No:</span></td>
                        <td><span class="formLable"><%= enqData[5] %></span> </td>
                    </tr>
                     <tr>
                        <td><span class="formLable bold_weight">Total No. Of Customers :</span></td>
                        <td><span class="formLable"><%= enqData[3] %></span> </td>
                    </tr>
                </table>

                <div class="row">
                    <div class="col col-3">
                        <div class="info-box bg-light">
                            <div class="info-box-content">
                                <span class="info-box-text text-center text-muted text-blue">Total Customers</span>
                                <span class="info-box-number text-center mb-0"><%=countData[0] %></span>
                            </div>
                        </div>
                    </div>

                    <div class="col col-3">
                        <div class="info-box bg-light">
                            <div class="info-box-content">
                                <span class="info-box-text text-center text-muted text-blue">Active Customers</span>
                                <span class="info-box-number text-center mb-0"><%=countData[1] %></span>
                            </div>
                        </div>
                    </div>

                    <div class="col col-3">
                        <div class="info-box bg-light">
                            <div class="info-box-content">
                                <span class="info-box-text text-center text-muted text-blue">Total Orders</span>
                                <span class="info-box-number text-center mb-0"></span>
                            </div>
                        </div>
                    </div>

                    <div class="col col-3">
                        <div class="info-box bg-light">
                            <div class="info-box-content">
                                <span class="info-box-text text-center text-muted text-blue">Total Order Amount</span>
                                <span class="info-box-number text-center mb-0"></span>
                            </div>
                        </div>
                    </div>

                    <div class="col col-3">
                        <div class="info-box bg-light">
                            <div class="info-box-content">
                                <span class="info-box-text text-center text-muted text-blue">Incentive Amount</span>
                                <span class="info-box-number text-center mb-0"></span>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="form-row">
                    <div class="form-group col-md-6">
                        <label>Bank Name  :*</label>
                        <asp:TextBox ID="txtBank" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Bank Account Type :*</label>
                        <asp:TextBox ID="txtAccType" runat="server" CssClass="form-control" Width="100%" MaxLength="10"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Account Holder Name :*</label>
                        <asp:TextBox ID="txtHolderName" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Account No. :*</label>
                        <asp:TextBox ID="txtAccNo" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Bank IFSC :*</label>
                        <asp:TextBox ID="txtIFSC" runat="server" CssClass="form-control" Width="100%" MaxLength="30"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <span class="space20"></span>
        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-md btn-dark" Text="Cancel" OnClick="btnCancel_Click" />
        
        <span class="space30"></span>

        <div class="row">
            <div class="col-6">
                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title">Follow Up History</h3>
                    </div>
                    <div class="card-body">
                        <div class="post" id="followup">
                            <%= followupHistory %>
                        </div>

                    </div>
                </div>
            </div>
            <div class="col-6">
                <div class="card card-success">
                    <div class="card-header">
                        <h3 class="card-title">Today's Follow Up</h3>
                    </div>
                    <div class="card-body">                        
                        <div class="form-group">
                            <label>Remark Description : *</label>
                            <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" CssClass="form-control textarea" Width="100%" Height="150"></asp:TextBox>
                        </div>                       
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-md btn-success" OnClick="btnSave_Click"/>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

