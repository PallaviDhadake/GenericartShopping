<%@ Page Title="Order Details | Followup" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="followup-order-detail.aspx.cs" Inherits="supportteam_followup_order_detail" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        //$(document).ready(function () {
            
        //});

        function lastCallPopup() {
            // Show the modal on page load
            $('#modal-sm').modal('show');
        }
    </script>

    <style type="text/css">
        .Order {
            border: 5px solid #ff6a00;
            padding: 20px;
        }

        .text-sm {
            text-align: left;
            font-size: 15px;
        }
    </style>
    <script>
        $(document).ready(function () {
            $('[id$=gvOrderDetails]').DataTable({
                columnDefs: [
                    { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6] }
                ],
                order: [[0, 'desc']]
            });
        });
    </script>
    <script>
        window.onload = function () {
            duDatepicker('#<%= txtCalendar.ClientID %>', {
                auto: true, inline: true, format: 'dd/mm/yyyy',
            });
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            manageBox();
        });

        function manageBox() {
            let checkBox = document.getElementById("<%= chkConfirm.ClientID %>");
            let box = document.getElementById("ordAssign");
            //checkBox.checked = true;
            //alert("new" + box);
            if (checkBox.checked == true) {

                box.style.display = "none";
            } else if (checkBox.checked == false) {
                box.style.display = "block";
            }

        }


        checkBox.onclick = new function () {
            manageBox();
        }
    </script>
    <style>
        #followup .user-block .username, #followup .user-block .description {
            margin: 0px !important;
        }
    </style>

    <script>
        function SaveGoodTime(myCustomerId) {
            var input = document.getElementById('<%= txtTimeGood.ClientID %>').value;
            if (validateTimeInput(input) == false) {
                TostTrigger('warning', 'Please enter a valid time in the format HH:MM AM/PM');
                return;
            }
            //alert(myCustomerId);
            PageMethods.UpdateGoodTime(input, myCustomerId, onSuccess, onError);
        }

        function onSuccess(result) {
            // Handle the successful response from the server method
            TostTrigger('success', 'Time updated successfully.');
            document.getElementById('<%= txtTimeGood.ClientID %>').value = "";
            $('#modal-sm-time').modal('hide');
            document.getElementById('GoodTime').innerText = result;
        }

        function onError(error) {
            // Handle any errors that occur during the AJAX request
            alert("Error: " + error.get_message());
        }

        function validateTimeInput(txtTimeStr) {
            var input = txtTimeStr;
            var timePattern = /^(0[1-9]|1[0-2]):[0-5][0-9] (AM|PM)$/i;

            if (!timePattern.test(input)) {
                    return false;
                }
                return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>Follow Up : Order details view</h1>
    <h2 class="text-success text-bold text-lg mb-2 mt-2"><%= ordConvertBy %></h2>
    <div class="row">
        <div class="col-md-8">
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
                                <p class="text-sm">
                                    Good time to call
                                    <b id="GoodTime" class="d-block"><%= callTime %></b>
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
                    <%--<button type="button" class="btn btn-warning">Lock Call</button>--%>
                    <asp:Button ID="btnLock" runat="server" Text="Lock Call" CssClass="btn btn-md btn-warning" OnClick="btnLock_Click" />
                    <a href="#" class="btn btn-md btn-success" data-toggle="modal" data-target="#modal-sm-time">Good Time To Call</a>
                    <a href="<%= lookupUrl %>" target="_blank" class="btn btn-md btn-info">Customer Lookup</a>
                    <asp:Button ID="btnFlup" runat="server" Text="Customer Wise Followup" CssClass="btn btn-md btn-danger" OnClick="btnFlup_Click" />
                    <%= lockMsg %>
                    <span class="space10"></span>
                    <%= callMsg %>

                    <div class="Order" runat="server" id="OrderInfo" Visible="false">
                        <h2 class="text-sm">
                            <b class="d-block">Resent Order Found : &nbsp;&nbsp;</b>
                            <% =arrOrdInfo[0] %>
                        </h2>

                        <h2 class="text-sm">
                            <b class="d-block">Order ID : &nbsp;&nbsp;</b>
                            <% =arrOrdInfo[1] %>
                        </h2>

                        <h2 class="text-sm">
                            <b class="d-block">Order Amount : &nbsp;&nbsp;</b>
                            <% =arrOrdInfo[2] %>
                        </h2>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="card card-primary">
                <div class="card-header">
                    <h3 class="card-title">GOBP Details</h3>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-12">
                            <div>
                                <dl class="row">
                                    <dt class="col-sm-5">OBP Name</dt>
                                    <dd class="col-sm-7"><%= arrGobp[0] %></dd>
                                    <dt class="col-sm-5">OBP Mobile No.</dt>
                                    <dd class="col-sm-7"><%= arrGobp[1] %></dd>
                                    <dt class="col-sm-5">OBP Email</dt>
                                    <dd class="col-sm-7"><%= arrGobp[2] %></dd>
                                </dl>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="card card-primary">
        <div class="card-header">
            <h3 class="card-title">Order's Overview</h3>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col col-2">
                    <div class="info-box bg-light">
                        <div class="info-box-content">
                            <span class="info-box-text text-center text-muted text-blue">Order Number</span>
                            <span class="info-box-number text-center mb-0"><%= arrOrd[0] %></span>
                        </div>
                    </div>
                </div>

                <div class="col col-2">
                    <div class="info-box bg-light">
                        <div class="info-box-content">
                            <span class="info-box-text text-center text-muted text-blue">Date</span>
                            <span class="info-box-number text-center mb-0"><%= arrOrd[1] %></span>
                        </div>
                    </div>
                </div>

                <div class="col col-2">
                    <div class="info-box bg-light">
                        <div class="info-box-content">
                            <span class="info-box-text text-center text-muted text-blue">Amount</span>
                            <span class="info-box-number text-center mb-0"><%= arrOrd[2] %></span>
                        </div>
                    </div>
                </div>

                <div class="col col-2">
                    <div class="info-box bg-light">
                        <div class="info-box-content">
                            <span class="info-box-text text-center text-muted text-blue">Shop Assigned</span>
                            <span class="info-box-number text-center mb-0"><%= arrOrd[3] %></span>
                        </div>
                    </div>
                </div>

                <div class="col col-2">
                    <div class="info-box bg-light">
                        <div class="info-box-content">
                            <span class="info-box-text text-center text-muted text-blue">Status From Shop</span>
                            <span class="info-box-number text-center mb-0"><%= arrOrd[4] %></span>
                        </div>
                    </div>
                </div>

                <%--<div class="col col-2">
                    <div class="info-box bg-light">
                        <div class="info-box-content">
                            <span class="info-box-text text-center text-muted text-blue">Dispatched From</span>
                            <span class="info-box-number text-center mb-0"><%= arrOrd[8] %></span>
                        </div>
                    </div>
                </div>

                <div class="col col-2">
                    <div class="info-box bg-light">
                        <div class="info-box-content">
                            <span class="info-box-text text-center text-muted text-blue">Dispatched Address</span>
                            <span class="info-box-number text-center mb-0"><%= arrOrd[9] %></span>
                        </div>
                    </div>
                </div>--%>

                <div class="col col-2">
                    <div class="info-box bg-light">
                        <div class="info-box-content">
                            <span class="info-box-text text-center text-muted text-blue">Total Follow up</span>
                            <span class="info-box-number text-center mb-0"><%= arrOrd[5] %></span>
                        </div>
                    </div>
                </div>

            </div>

            <asp:GridView ID="gvOrderDetails" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
                AutoGenerateColumns="false" OnRowDataBound="gvOrderDetails_RowDataBound">
                <RowStyle CssClass="" />
                <HeaderStyle CssClass="bg-dark" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="OrdDetailID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FK_DetailProductID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ProductName" HeaderText="Product Name">
                        <ItemStyle Width="30%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OrdDetailSKU" HeaderText="Product Code">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OrdDetailQTY" HeaderText="Quantity">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OrigPrice" HeaderText="Price">
                        <ItemStyle Width="20%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OrdAmount" HeaderText="Amount">
                        <ItemStyle Width="20%" />
                    </asp:BoundField>
                </Columns>
                <EmptyDataTemplate>
                    <span class="warning">:(</span>
                </EmptyDataTemplate>
                <PagerStyle CssClass="gvPager" />
            </asp:GridView>

            <span class="space30"></span>


        </div>
    </div>

    <div class="">
        <ul class="nav nav-pills ml-auto p-2">
            <li class="nav-item"><a class="nav-link active" href="#tab_1" data-toggle="tab">Update Followup Feedback</a></li>
            <li class="nav-item"><a class="nav-link" href="#tab_2" data-toggle="tab">Repeat / New Order</a></li>

        </ul>
        <hr />
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
                                    <%--<div class="user-block">
                            <span class="username">
                                <a href="#">Chinmay Kulkarni</a>
                            </span>
                            <span class="description">Follow Up on - 12/Aug/2022  4:00PM</span>
                        </div>
                        <h6 class="text-indigo">Conn: Repeated Order</h6>
                        <p class="text-bold">
                            Lorem ipsum represents a long-held tradition for designers,
                typographers and the like. Some people hate it and argue for
                its demise, but others ignore.
                        </p>
                        <i class="nav-icon fas fa-clock"></i><span>Next Follow Up: 25/Aug/2022, Time: 03:00 PM</span>

                        <hr />
                        <div class="user-block">
                            <span class="username">
                                <a href="#">Sanjay Mokashi</a>
                            </span>
                            <span class="description">Follow Up on - 12/Aug/2022  4:00PM</span>
                        </div>
                        <h6 class="text-indigo">Conn: Next Call Scheduled</h6>
                        <p class="text-bold">
                            Lorem ipsum represents a long-held tradition for designers,
                typographers and the like. Some people hate it and argue for
                its demise, but others ignore.
                        </p>
                        <i class="nav-icon fas fa-clock"></i><span>Next Follow Up: 25/Aug/2022, Time: 03:00 PM</span>--%>
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
                                    <label>Customer's Response On Call :</label>
                                    <div id="RadioButtonGrp" runat="server">
                                    <div class="row">
                                    <div class="col-md-3">
                                        <div class="p-2">
                                            <img alt="" src="../images/icons/happy.png" style="width:28px" />
                                            <asp:RadioButton ID="rdbHappy" runat="server" GroupName="ResponseIco" Text="Happy" />
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="p-2">
                                             <img alt="" src="../images/icons/neutral.png" style="width:28px" />
                                            <asp:RadioButton ID="rdbNeutral" runat="server" GroupName="ResponseIco" Text="Neutral" />
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="p-2">
                                             <img alt="" src="../images/icons/upset.png" style="width:28px" />
                                            <asp:RadioButton ID="rdbUpset" runat="server" GroupName="ResponseIco" Text="Upset" />
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="p-2">
                                             <img alt="" src="../images/icons/angry.png" style="width:28px" />
                                            <asp:RadioButton ID="rdbAngry" runat="server" GroupName="ResponseIco" Text="Angry" />
                                        </div>
                                    </div>
                                    </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label>Next Follow-Up Date : *</label>
                                    <asp:TextBox ID="txtCalendar" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Next Follow-Up Time : *</label>
                                    <asp:TextBox ID="txtTime" runat="server" CssClass="form-control" Width="100%" MaxLength="50" placeholder="00:00 AM/PM"></asp:TextBox>
                                </div>
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-md btn-success" OnClick="btnSave_Click" />

                                <%--<button type="button" class="btn btn-default" data-toggle="modal" data-target="#modal-sm">Launch Small Modal</button>--%>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="tab-pane" id="tab_2">
                <asp:CheckBox ID="chkConfirm" runat="server" TextAlign="Right" Text=" Assign Order To Previous Shop ?" onclick="manageBox()" />
                <asp:CheckBox ID="chkMonthly" runat="server" TextAlign="Right" Text=" Mark as Monthly Order ?" />
                <div id="ordAssign">
                    <div class="row">
                        <div class="col-md-4">
                            <span class="col-form-label">Select Shop To Assign Order :</span>
                            <asp:DropDownList ID="ddrShops" runat="server" CssClass="form-control">
                                <asp:ListItem Value="0"><- Select -></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <span class="col-form-label">Enter Shop Code To Assign Order :</span>
                            <asp:TextBox ID="txtShopCode" runat="server" CssClass="form-control" placeholder="Enter Shop Code"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <span class="space10"></span>
                <asp:Button ID="btnRepeat" runat="server" Text="Repeat Order" CssClass="btn btn-md btn-primary" OnClick="btnRepeat_Click" OnClientClick="return confirm('Are you sure to repeat order?');" />
                <span class="space10"></span>
                <p class="text-md text-bold text-primary">Repeat Order :
                    <br />
                    Replica of above order will be placed, and its followup feedback will be updated automatically.</p>
                <hr />

                <asp:Button ID="btnEditRepeat" runat="server" Text="Edit & Repeat Order" CssClass="btn btn-md btn-info" Enabled="true" OnClick="btnEditRepeat_Click" />
                <span class="space20"></span>
                <p class="text-md text-bold text-info">Edit & Repeat Order :
                    <br />
                    In this you will redirected to another page where you can edit quantity of above order and place it again,
                    <br />
                    also its followup feedback will be updated automatically.</p>
                <hr />

                <%--<asp:Button ID="btnNew" runat="server" Text="New Order" CssClass="btn btn-md btn-warning" />--%>
                <%= newOrdUrl %>
                <span class="space20"></span>
                <p class="text-md text-bold text-warning">New Order :
                    <br />
                    In this you will redirected to another page where you have to place complete new order</p>
            </div>
        </div>

    </div>

    <%--<script type="text/javascript">
        $(document).ready(function () {
            document.getElementById("<%= btnRepeat.ClientID %>").addEventListener("click", ManageBtn);
            function ManageBtn() {
                document.getElementById("<%= btnRepeat.ClientID %>").click();
        }
        });
    </script>--%>

    <div class="modal fade" id="modal-sm">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Last Call Response</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <%--<p>One fine body&hellip;</p>--%>
                    <%=arrLastCall[3] %>
                    <%=arrLastCall[4] %>

                </div>
                <%--<div class="modal-footer justify-content-between">
                </div>--%>
            </div>
        </div>
    </div>

<div class="modal fade" id="modal-sm-time">
    <div class="modal-dialog modal-sm">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">Good Time to Call</h4>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <p>Enter Time here</p>
                <asp:TextBox ID="txtTimeGood" CssClass="form-control" Placeholder="00:00 AM/PM" runat="server"></asp:TextBox>
            </div>
            <div class="modal-footer justify-content-between">
                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                <button type="button" class="btn btn-primary" onclick="SaveGoodTime(<%=myCustId%>);">Save changes</button>
            </div>
        </div>
    </div>
</div>

</asp:Content>

