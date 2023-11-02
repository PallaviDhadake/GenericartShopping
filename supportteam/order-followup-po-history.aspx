<%@ Page Title="" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="order-followup-po-history.aspx.cs" Inherits="supportteam_order_followup_po_history" %>

<%@ MasterType VirtualPath="~/supportteam/MasterSupport.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        window.onload = function () {
            duDatepicker('#<%= txtCalendar.ClientID %>', {
                auto: true, inline: true, format: 'dd/mm/yyyy',
            });
        }
    </script>
    <style>
        .clrblack { color: #000000}
        table { border-collapse: collapse; width: 100%;}
        td, th { border: 1px solid #dddddd; text-align: left; padding: 8px;}
         #followup .user-block .username, #followup .user-block .description {margin: 0px !important;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">Order Follow-up Po History</h2>
    <span class="space15"></span>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" EnableViewState="true">
        <ContentTemplate>
            <%-- Customres Information --%>
            <div class="row">
                <div class="col-md-12">
                    <div class="card card-primary">
                        <div class="card-header">
                            <h3 class="card-title">Customer's Information</h3>
                        </div>
                        <div class="card-body">
                            <div class="row">
                                <div class="col-6">
                                    <div class="user-block">
                                        <h5 class="text-blue"><%= arrCust[0] %></h5>
                                        <p class="text-sm">
                                            Mobile Number
                                    <b class="d-block"><%= arrCust[1] %></b>
                                        </p>

                                        <p class="text-sm">
                                            Favourite Shop
                                    <b class="d-block"><%= arrCust[2] %></b>
                                        </p>
                                    </div>
                                </div>
                                <div class="col-6">
                                    <div>
                                        <dl class="row">
                                            <dt class="col-sm-6">Total Order(s)</dt>
                                            <dd class="col-sm-6"><%= arrCust[3] %></dd>
                                            <dt class="col-sm-6">Deliverd Successfully</dt>
                                            <dd class="col-sm-6"><%= arrCust[4] %></dd>
                                            <dt class="col-sm-6">Inprocess</dt>
                                            <dd class="col-sm-6"><%= arrCust[5] %></dd>
                                            <dt class="col-sm-6">Cancelled</dt>
                                            <dd class="col-sm-6"><%= arrCust[6] %></dd>
                                        </dl>
                                    </div>
                                </div>
                            </div>

                            <%--  <div class="card-body">--%>

                            <div class="row">
                                <div class="col-12">
                                    <h3 class="text-info medium">GOBP Details</h3>
                                    <div class="p-2">
                                        <div>
                                            <dl class="row">
                                                <div class="user-block">
                                                    <span id="nogobp" runat="server" visible="false" class="medium clrblack"><%=arrCust[10] %></span>
                                                    <h5 class="text-blue"><%= arrCust[7] %> <span class="small clrblack"><%=arrCust[9] %></span> </h5>
                                                    <p class="text-sm" id="mobno" runat="server" visible="false">
                                                        Mobile Number
                                    <b class="d-block"><%= arrCust[8] %></b>
                                                    </p>
                                                </div>
                                            </dl>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%-- Customers Information End --%>

            <div class="row">
                <div class="col-12">
                    <div class="card card-primary card-outline card-tabs">
                        <div class="card-header p-0 pt-1 border-bottom-0">
                            <ul class="nav nav-tabs" id="custom-tabs-three-tab" role="tablist">
                                <li class="nav-item">
                                    <a class="nav-link" href="<%=orderslink %>">Orders</a>
                                </li>
                                <li class="nav-item">
                                   <a class="nav-link active" id="custom-tabs-three-profile-tab" data-toggle="pill" href="#custom-tabs-three-profile" role="tab" aria-controls="custom-tabs-three-profile" aria-selected="true">Follow Up History</a>
                                </li>
                            </ul>
                        </div>
                        <div class="card-body">
                            <div class="tab-content" id="custom-tabs-three-tabContent">
                                <div class="tab-pane fade" id="custom-tabs-three-home" role="tabpanel" aria-labelledby="custom-tabs-three-home-tab">
                                    <p class="">Last Order Overview</p>
                                </div>
                                <div class="tab-pane fade show active" id="custom-tabs-three-profile" role="tabpanel" aria-labelledby="custom-tabs-three-profile-tab">

                                    <div class="tab-content">
                                        <div class="tab-pane active" id="tab_1">
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
                                                    <div class="card card-purple">
                                                        <div class="card-header">
                                                            <h3 class="card-title">Today's Follow Up</h3>
                                                        </div>
                                                        <div class="card-body">
                                                            <div class="form-group">
                                                                <label>Follow-Up Remark : *</label>
                                                                <asp:DropDownList ID="ddrRemark" runat="server" CssClass="form-control" Width="100%">
                                                                    <asp:ListItem Value="0"><- Select -></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>

                                                            <div class="form-group">
                                                                <label>Remark Description : *</label>
                                                                <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" CssClass="form-control textarea" Width="100%" Height="120"></asp:TextBox>
                                                            </div>
                                                            <div class="form-group">
                                                                <label>Next Follow-Up Date : *</label>
                                                                <asp:TextBox ID="txtCalendar" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                                                            </div>
                                                            <div class="form-group">
                                                                <label>Next Follow-Up Time : *</label>
                                                                <asp:TextBox ID="txtTime" runat="server" CssClass="form-control" Width="100%" MaxLength="50" placeholder="00:00 AM/PM"></asp:TextBox>
                                                            </div>
                                                            <asp:Button ID="Button1" runat="server" Text="Save" CssClass="btn btn-md btn-success" OnClick="btnSave_Click" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

