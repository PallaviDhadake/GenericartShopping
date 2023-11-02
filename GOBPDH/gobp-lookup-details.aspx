<%@ Page Title="" Language="C#" MasterPageFile="~/GOBPDH/MasterGOBPDH.master" AutoEventWireup="true" CodeFile="gobp-lookup-details.aspx.cs" Inherits="GOBPDH_gobp_lookup_details" %>

<%@ MasterType VirtualPath="~/GOBPDH/MasterGOBPDH.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .mainjoinlevel {
            background: #ba3434;
            width: 35px;
            height: 35px;
            border-radius: 50%;
            text-align: center !important;
            margin: 0 auto !important;
        }

        .joinlevel {
            background: #ba3434;
            width: 35px;
            height: 35px;
            border-radius: 50%;
            text-align: center !important;
            margin: 0 auto !important;
            position: relative;
        }

            .joinlevel:before {
                content: '';
                width: 500px;
                height: 2px;
                background: #808080;
                position: absolute;
                left: 50px;
                top: 15px;
            }

            .joinlevel:after {
                content: '';
                width: 500px;
                height: 2px;
                background: #808080;
                position: absolute;
                right: 50px;
                top: 15px;
            }

        .border-radius {
            width: 20px !important;
            height: 10px !important;
            border-radius: 50%;
        }

        .levelnum {
            margin-top: -5px !important;
        }
       
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="readFrEnquiry" runat="server">
        <div class="card card-primary">
            <div class="card-header">
                <h3 class="card-title">GOBP Registration Data</h3>
            </div>
            <div class="card-body">
                <table class="form_table">
                    <tr>
                        <td><span class="colorLightBlue">Id :</span></td>
                        <td>
                            <asp:Label ID="lblId" CssClass="colorLightBlue" runat="server" Text="[New]"></asp:Label></td>
                    </tr>
                </table>

                <div class="row">
                    <div class="col-md-7">
                        <table class="form_table">
                            <tr>
                                <td><span class="text-lg text-bold text-purple">Type:</span></td>
                                <td><span class="text-lg text-bold text-purple"><%= arrGobpData[27] %></span></td>
                            </tr>
                            <tr>
                                <td style="width: 25%"><span class="formLable bold_weight">Join Date:</span></td>
                                <td><span class="formLable"><%= arrGobpData[28] %></span> </td>
                            </tr>
                            <tr>
                                <td><span class="formLable bold_weight">Applicant Name :</span></td>
                                <td><span class="formLable" style="display: block; width: 60% !important;"><%= arrGobpData[0] %></span> </td>
                            </tr>
                            <tr>
                                <td><span class="formLable bold_weight">GOBP Code :</span></td>
                                <td><span class="formLable"><%= arrGobpData[29] %></span> </td>
                            </tr>
                            <tr>
                                <td><span class="formLable bold_weight">Mobile No :</span></td>
                                <td><span class="formLable"><%= arrGobpData[6] %></span> </td>
                            </tr>
                            <tr>
                                <td><span class="formLable bold_weight">Email Id :</span></td>
                                <td><span class="formLable"><%= arrGobpData[5] %></span> </td>
                            </tr>
                            <tr>
                                <td><span class="formLable bold_weight">DH Name :</span></td>
                                <td><span class="formLable"><%= arrGobpData[30] %></span> </td>
                            </tr>
                            <tr>
                                <td><span class="formLable bold_weight">DH Contact No:</span></td>
                                <td><span class="formLable"><%= arrGobpData[31] %></span> </td>
                            </tr>
                            <tr>
                                <td><span class="formLable bold_weight">Firm Type :</span></td>
                                <td><span class="formLable"><%= arrGobpData[1] %></span> </td>
                            </tr>
                            <tr>
                                <td><span class="formLable bold_weight">Address :</span></td>
                                <td><span class="formLable"><%= arrGobpData[2] %></span> </td>
                            </tr>
                            <tr>
                                <td><span class="formLable bold_weight">Age :</span></td>
                                <td><span class="formLable"><%= arrGobpData[3] %></span> </td>
                            </tr>
                            <tr>
                                <td><span class="formLable bold_weight">Marital Status :</span></td>
                                <td><span class="formLable"><%= arrGobpData[4] %></span> </td>
                            </tr>
                            <tr>
                                <td><span class="formLable bold_weight">What's App No :</span></td>
                                <td><span class="formLable"><%= arrGobpData[7] %></span> </td>
                            </tr>
                            <tr>
                                <td><span class="formLable bold_weight">State :</span></td>
                                <td><span class="formLable"><%= arrGobpData[8] %></span> </td>
                            </tr>
                             <tr>
                                <td><span class="formLable bold_weight">District :</span></td>
                                <td><span class="formLable"><%= arrGobpData[9] %></span> </td>
                            </tr>
                            <tr>
                                <td><span class="formLable bold_weight">City :</span></td>
                                <td><span class="formLable"><%= arrGobpData[10] %></span> </td>
                            </tr>
                        </table>
                    </div>
                    <div class="col-md-5">
                        <table class="form_table">
                            <tr>
                                <td><span class="formLable bold_weight">Owner Education :</span></td>
                                <td><span class="formLable"><%= arrGobpData[11] %></span> </td>
                            </tr>
                            <tr>
                                <td><span class="formLable bold_weight">Owner Occupation :</span></td>
                                <td><span class="formLable"><%= arrGobpData[12] %></span> </td>
                            </tr>
                            <tr>
                                <td><span class="formLable bold_weight">Any Legal Matter  :</span></td>
                                <td><span class="formLable"><%= arrGobpData[13] %></span> </td>
                            </tr>
                            <tr>
                                <td><span class="formLable bold_weight">Residence Form  :</span></td>
                                <td><span class="formLable"><%= arrGobpData[14] %></span> </td>
                            </tr>
                            <tr>
                                <td><span class="formLable bold_weight">UTR Number :</span></td>
                                <td><span class="formLable"><%= arrGobpData[21] %></span> </td>
                            </tr>
                            <tr>
                                <td><span class="formLable bold_weight">Bank Name :</span></td>
                                <td><span class="formLable"><%= arrGobpData[22] %></span> </td>
                            </tr>
                            <tr>
                                <td><span class="formLable bold_weight">Account Holder Name :</span></td>
                                <td><span class="formLable"><%= arrGobpData[23] %></span> </td>
                            </tr>
                            <tr>
                                <td><span class="formLable bold_weight">Ammount :</span></td>
                                <td><span class="formLable"><%= arrGobpData[24] %></span> </td>
                            </tr>
                            <%--  <tr>
                        <td><span class="formLable bold_weight">Birth Date :</span></td>
                        <td><span class="formLable"><%= arrGobpData[26] %></span> </td>
                    </tr>--%>
                            <tr>
                                <td><span class="formLable bold_weight">Transaction Date :</span></td>
                                <td><span class="formLable"><%= arrGobpData[25] %></span> </td>
                            </tr>
                            <tr>
                                <td><span class="formLable bold_weight">Is MLM :</span></td>
                                <td><span class="formLable"><%= arrGobpData[32] %></span> </td>
                            </tr>
                            <tr>
                                <td><span class="formLable bold_weight">Join Level :</span></td>
                                <td><span class="formLable"><%= arrGobpData[36] %></span> </td>
                            </tr>
                            <tr>
                                <td><span class="formLable bold_weight">Zonal Head User Id :</span></td>
                                <td><span class="formLable"><%= arrGobpData[33] %></span> </td>
                            </tr>
                            <tr>
                                <td><span class="formLable bold_weight">Refferal User Id :</span></td>
                                <td><span class="formLable"><%= arrGobpData[34] %></span> </td>
                            </tr>
                            <tr>
                                <td><span class="formLable bold_weight">Shop Code :</span></td>
                                <td><span class="formLable"><%= arrGobpData[38] %></span> </td>
                            </tr>
                            <tr>
                                <td><span class="formLable bold_weight">Shop Name :</span></td>
                                <td><span class="formLable"><%= arrGobpData[37] %></span> </td>
                            </tr>

                        </table>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <table>
                            <tr>
                                <span class="space10"></span>
                                <span class="bold_weight">Profile Photo</span>
                                <span class="space10"></span>
                                <%= arrGobpData[15] %>
                                <span class="space10"></span>

                                <%-- <td><span class="formLable bold_weight">Profile Photo :</span></td>
                        <td><span class="formLable"><%= arrGobpData[15] %></span> </td>--%>
                            </tr>
                            <tr>
                                <span class="space10"></span>
                                <span class="bold_weight">Resume</span>
                                <span class="space10"></span>
                                <%= arrGobpData[16] %>
                                <span class="space10"></span>
                                <%--  <td><span class="formLable bold_weight">Resume :</span></td>
                        <td><span class="formLable"><%= arrGobpData[16] %></span> </td>--%>
                            </tr>
                            <tr>
                                <span class="space10"></span>
                                <span class="bold_weight">Address Proof 1</span>
                                <span class="space10"></span>
                                <%= arrGobpData[18] %>
                                <span class="space10"></span>
                                <%--<td><span class="formLable bold_weight">Address Proof 1  :</span></td>
                        <td><span class="formLable"><%= arrGobpData[18] %></span> </td>--%>
                            </tr>
                        </table>
                    </div>
                    <div class="col-md-6">
                        <table>

                            <tr>
                                <span class="space10"></span>
                                <span class="bold_weight">Address Proof 2</span>
                                <span class="space10"></span>
                                <%= arrGobpData[35] %>

                                <%--<td><span class="formLable bold_weight">Address Proof 2  :</span></td>
                        <td><span class="formLable"><%= arrGobpData[35] %></span> </td>--%>
                            </tr>
                            <tr>
                                <span class="space10"></span>
                                <span class="bold_weight">Id Proof 1</span>
                                <span class="space10"></span>
                                <%= arrGobpData[19] %>
                                <span class="space10"></span>
                                <%--<td><span class="formLable bold_weight">Id Proof 1 :</span></td>
                        <td><span class="formLable"><%= arrGobpData[19] %></span> </td>--%>
                            </tr>
                            <tr>
                                <span class="space10"></span>
                                <span class="bold_weight">Id Proof 2</span>
                                <span class="space10"></span>
                                <%= arrGobpData[20] %>
                                <span class="space10"></span>

                                <%--<td><span class="formLable bold_weight">Id Proof 2 :</span></td>
                        <td><span class="formLable"><%= arrGobpData[20] %></span> </td>--%>
                            </tr>
                        </table>
                    </div>

                </div>


                <%-- <div class="row">
                    <div class="col col-4">
                        <div class="info-box bg-light">
                            <div class="info-box-content">
                                <span class="info-box-text text-center text-muted text-blue">Total Customers</span>
                                <span class="info-box-number text-center mb-0"><%=countData[0] %></span>
                            </div>
                        </div>
                    </div>
                    
                    <div class="col col-4">
                        <div class="info-box bg-light">
                            <div class="info-box-content">
                                <span class="info-box-text text-center text-muted text-blue">Total Orders</span>
                                <span class="info-box-number text-center mb-0"><%=countData[1] %></span>
                            </div>
                        </div>
                    </div>
                    
                    <div class="col col-4">
                        <div class="info-box bg-light">
                            <div class="info-box-content">
                                <span class="info-box-text text-center text-muted text-blue">Total Incentive Amount</span>
                                <span class="info-box-number text-center mb-0"><%=countData[2] %></span>
                            </div>
                        </div>
                    </div>
                </div>--%>
                <span class="space30"></span>

                <%-- <div class="card-body">
                    <%= arrShopInfo[0] %>
                </div>--%>

                <%--<div class="form-row">
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
                </div>--%>
            </div>
        </div>
        <span class="space20"></span>

        <div class="card-body">
            <%= arrShopInfo[0] %>
        </div>

        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-md btn-dark" Text="Cancel" OnClick="btnCancel_Click" />

        <span class="space30"></span>

        <%-- Gobp join level tree view --%>
        <div class="container text-center">
            <%=joinlevelstr %>
        </div>
        <%-- Gobp join level tree view end--%>

        <%-- <div class="row">
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
        </div>--%>
    </div>

</asp:Content>

