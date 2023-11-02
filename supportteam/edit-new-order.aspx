<%@ Page Title="" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="edit-new-order.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="supportteam_edit_new_order" %>

<%@ MasterType VirtualPath="~/supportteam/MasterSupport.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />
    <link rel="stylesheet" href="/resources/demos/style.css" />
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script type="text/javascript">
        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            //if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
            if (iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
                return false;
            return true;
        }

    </script>

    <script>
        function() {
            duDatepicker('#<%= txtCalendar.ClientID %>', {
                auto: true, inline: true, format: 'dd/mm/yyyy',
            });
        }
    </script>

    <script type="text/javascript">
        function setupcalendar() {
            $(function () {
                duDatepicker('#<%= txtCalendar.ClientID %>', {
                    auto: true, inline: true, format: 'dd/mm/yyyy',
                });
            });
        }
    </script>

    <script type="text/javascript">
        function handleDropdownChange() {
            var ddl = document.getElementById('<%= ddrCourier.ClientID %>');
            var selectedValue = ddl.value;
            //alert(selectedValue);
            if (selectedValue == '5') {
                //alert(selectedValue);
                document.getElementById('courieroption').style.display = 'block';
            }
            else {
                //alert(selectedValue);
                document.getElementById('courieroption').style.display = 'none';
            }

            localStorage.setItem('ddrCourierItem', selectedValue);
            // Your custom logic here
            //console.log('Selected value: ' + selectedValue);
        }

        function restoreDdrItemValue() {
            var storedValue = localStorage.getItem('ddrCourierItem');
            if (storedValue == '5') {
                document.getElementById('courieroption').style.display = 'block';
            }
            else {
                document.getElementById('courieroption').style.display = 'none';
            }
        }
    </script>

    <script type="text/javascript">
        function setupAutocomplete() {
            $(function () {
                $("#<%= txtMedName.ClientID %>").autocomplete({

                    source: function (request, response) {
                        $.ajax({
                            url: '<%= ResolveUrl("../WebServices.aspx/GetSearchControl") %>',
                            data: "{ 'prefix': '" + request.term + "'}",
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                response($.map(data.d, function (item) {
                                    return {
                                        label: item.split('#')[0],
                                        val: item.split('#')[1]
                                    }
                                }))
                            },

                        });
                    },
                });
            });
        }
    </script>

    <script type="text/javascript">
        function pageLoad(sender, args) {
            if (args.get_isPartialLoad()) {
                setupAutocomplete();
                setupcalendar();
                handleDropdownChange();
                restoreDdrItemValue();
            }
        }
    </script>

    <script type="text/javascript">
        // Call setupAutocomplete on initial page load
        $(document).ready(function () {
            setupAutocomplete();
            setupcalendar();
            handleDropdownChange();
            restoreDdrItemValue();
        });
    </script>

    <script>
        $(function () {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        createDataTable();
                    }
                });
            };
    </script>
    <style>
        .clrblack {
            color: #000000
        }

        table {
            border-collapse: collapse;
            width: 100%;
        }

        td, th {
            border: 1px solid #dddddd;
            text-align: left;
            padding: 8px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">Edit Order</h2>
    <span class="space15"></span>
    <!-- Gridview to show saved data starts here -->
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" EnableViewState="true" UpdateMode="Conditional">
        <ContentTemplate>
            <%-- Customres Information --%>
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
                <div class="col-md-4">
                    <div class="card card-primary">
                        <div class="card-header">
                            <h3 class="card-title">Last Call Details</h3>
                        </div>
                        <div class="card-body">
                            <div class="row">
                                <div class="col-12">
                                    <div>
                                        <dl class="row">
                                            <dt class="col-sm-5">Date :</dt>
                                            <dd class="col-sm-6"><%= lastCall[0] %></dd>
                                            <dt class="col-sm-5">Time :</dt>
                                            <dd class="col-sm-6"><%= lastCall[1] %></dd>
                                            <dt class="col-sm-5">Call By :</dt>
                                            <dd class="col-sm-6"><%= lastCall[2] %></dd>
                                            <dt class="col-sm-5">Status :</dt>
                                            <dd class="col-sm-6"><%= lastCall[3] %></dd>
                                            <dt class="col-sm-5">Remark :</dt>
                                            <dd class="col-sm-6"><%= lastCall[4] %></dd>
                                            <dt class="col-sm-5">Next Follow Up :</dt>
                                            <dd class="col-sm-6"><%= lastCall[5] %></dd>
                                            <dt class="col-sm-5"></dt>
                                            <dd class="col-sm-6"><%= lastCall[6] %></dd>
                                        </dl>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%-- Customres Information End --%>

            <span class="space15"></span>
            <div id="viewProduct" runat="server">
                <div class="formPanel table-responsive-md">
                    <asp:GridView ID="gvProducts" runat="server" CssClass="table table-striped table-bordered table-hover" GridLines="None"
                        AutoGenerateColumns="false"
                        OnRowCommand="gvProducts_RowCommand">
                        <HeaderStyle CssClass="thead-dark" />
                        <RowStyle CssClass="" />
                        <AlternatingRowStyle CssClass="alt" />
                        <Columns>
                            <asp:BoundField DataField="FK_DetailProductID">
                                <HeaderStyle CssClass="HideCol" />
                                <ItemStyle CssClass="HideCol" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ProductName" HeaderText="Product Name">
                                <ItemStyle Width="20%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="OrdDetailSKU" HeaderText="Product Code">
                                <ItemStyle Width="5%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="OrdDetailQTY" HeaderText="last Qty">
                                <ItemStyle Width="5%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="OrigPrice" HeaderText="Price">
                                <ItemStyle Width="5%" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Qty">
                                <ItemStyle Width="8%" />
                                <ItemTemplate>
                                    <asp:TextBox ID="txtQty" runat="server" onkeypress="javascript:return isNumber(event);" CssClass="form-control"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Width="5%" />
                                <ItemTemplate>
                                    <asp:Button ID="btnAdd" runat="server" Text="Add" CommandName="add" CssClass="btn btn-primary" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <span class="warning">Its Empty Here... :(</span>
                        </EmptyDataTemplate>
                        <PagerStyle CssClass="" />
                    </asp:GridView>
                </div>
            </div>
            <span class="space20"></span>

            <h4 class="formTitle bg-purple">Other Items</h4>
            <div class="form-row">
                <div class="form-group col-md-6">
                    <label><span style="color: red">*</span>Medicine Name :</label>
                    <asp:TextBox ID="txtMedName" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                </div>
                <div class="form-group col-md-3">
                    <label><span style="color: red">*</span>Quantity :</label>
                    <asp:DropDownList ID="ddrQty" runat="server" CssClass="form-control">
                        <asp:ListItem Value="0"><- Select -></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group col-md-3">
                    <span class="space30"></span>
                    <span class="space5"></span>
                    <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary mr-lg-1" Text="ADD" OnClick="btnAdd_Click" />
                </div>
            </div>

            <span class="space20"></span>
            <%-- Add Viewstate data in gridview --%>
            <div id="inwardItem" runat="server">
                <h4 class="formTitle bg-info text-white">Specify Order Item</h4>
                <span class="space15"></span>
                <span class="space10"></span>
                <div id="OrderItem" runat="server" visible="false">
                    <div class="formPanel table-responsive-md">
                        <asp:GridView ID="gvOrdDetails" runat="server" CssClass="table table-striped table-bordered table-hover" GridLines="None"
                            AutoGenerateColumns="false"
                            OnRowCommand="gvOrdDetails_RowCommand">
                            <HeaderStyle CssClass="thead-dark" />
                            <RowStyle CssClass="" />
                            <AlternatingRowStyle CssClass="alt" />
                            <Columns>
                                <asp:BoundField DataField="FK_DetailProductID">
                                    <HeaderStyle CssClass="HideCol" />
                                    <ItemStyle CssClass="HideCol" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ProductName" HeaderText="Product Name">
                                    <ItemStyle Width="50%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="OrdDetailSKU" HeaderText="Product Code">
                                    <ItemStyle Width="15%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="OrdDetailPrice" HeaderText="Price">
                                    <ItemStyle Width="10%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="OrdDetailQTY" HeaderText="Quantity">
                                    <ItemStyle Width="10%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="OrdDetailAmount" HeaderText="Amount">
                                    <ItemStyle Width="10%" />
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <ItemStyle Width="5%" />
                                    <ItemTemplate>
                                        <asp:Button ID="btnDel" runat="server" Text="Delete" CommandName="del" CssClass="buttonDel" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="FK_DetailOrderID">
                                    <HeaderStyle CssClass="HideCol" />
                                    <ItemStyle CssClass="HideCol" />
                                </asp:BoundField>
                            </Columns>
                            <PagerStyle CssClass="" />
                        </asp:GridView>
                        <span class="text-indigo text-lg text-bold"><%= ordAmmount %></span>
                    </div>
                </div>
            </div>

            <%-- Payment and Courier Details --%>
            <div class="card card-olive">
                <div class="card-header">
                    <h3 class="card-title">Payment and Courier Details</h3>
                </div>
                <div class="card-body">
                    <span class="space15"></span>
                    <asp:CheckBox ID="chkCODType" runat="server" TextAlign="right" CssClass="space" Text="Is this COD (cash on delivery)?" />
                    <asp:CheckBox ID="chkSelfPickup" runat="server" TextAlign="right" CssClass="space" Text="self pickup" />
                    <br />
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <br />
                            <span class="themeClrSec fontRegular dspBlk mrg_B_10 semiMedium"><b>Upload Prescription</b></span>
                            <span class="reqPrescription"></span><span class="small semiBold colrPink mrg_B_20"><i>Upload Payment Slip </i></span>
                            <br />
                            <span class="reqPrescription"></span><span class="small semiBold colrPink mrg_B_20"><i>File size Less than 5 MB </i></span>
                            <span class="space30"></span>
                            <asp:FileUpload ID="fuPaymentSlip" runat="server" />
                            <br />
                            <%=paymentslip %>
                        </div>
                    </div>
                    <%--<span class="themeclrsec fontregular dspblk mrg_b_10 semimedium"><b>upload prescription</b></span>
                    <span class="reqprescription"></span><span class="small semibold colrpink mrg_b_20"><i>upload payment slip </i></span>
                    <br />
                    <span class="reqprescription"></span><span class="small semibold colrpink mrg_b_20"><i>file size less than 5 mb </i></span>
                    <span class="space30"> <%= paymentslip %>--%>

                    <span class="space20"></span>
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label>UPI ID / Transaction No. :</label>
                            <asp:TextBox ID="txtUPIID" runat="server" CssClass="form-control" Width="100%" MaxLength="50" placeholder="UPI ID / Transaction No."></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label>Courier Option :</label>
                            <asp:DropDownList ID="ddrCourier" runat="server" CssClass="form-control" AppendDataBoundItems="True" onchange="handleDropdownChange()">
                                <asp:ListItem Value="0">-- Select Courier Option --</asp:ListItem>
                                <asp:ListItem Value="1">DTDC</asp:ListItem>
                                <asp:ListItem Value="2">Tirupati Courier</asp:ListItem>
                                <asp:ListItem Value="3">Tej</asp:ListItem>
                                <asp:ListItem Value="4">Post Office</asp:ListItem>
                                <asp:ListItem Value="5">Other</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div id="courieroption" class="form-group col-md-6" style="display: none">
                            <label>Courier Option :</label>
                            <asp:TextBox ID="txtCourier" runat="server" CssClass="form-control" Width="100%" MaxLength="50" placeholder="Courier Option"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>

            <%-- Assign Shop Address --%>
            <div class="card card-olive">
                <div class="card-header">
                    <h3 class="card-title">Assign Shop and Address</h3>
                </div>
                <div class="card-body">
                    <div id="existingAddr" runat="server">
                        <span class="space30"></span>
                        <h3 class="text-blue pageH3">Select Shipping Address</h3>
                        <asp:DropDownList ID="ddrAddress" runat="server" CssClass="form-control" 
                            AutoPostBack="true">
                            <asp:ListItem Value="0">-- Select Address --</asp:ListItem>
                        </asp:DropDownList>
                        <span class="space10"></span>
                        <div class="text-center text-lg text-bold text-blue">OR</div>
                    </div>
                    <div id="newAddr" runat="server">
                        <span class="space10"></span>
                        <h3 class="text-blue pageH3">Add Shipping Address :</h3>
                        <div class="form-row">
                            <div class="form-group col-md-6">
                                <label>Address : <span style="color: red" class="ml-1">*</span></label>
                                <asp:TextBox ID="txtAddress1" runat="server" TextMode="MultiLine" Height="120" CssClass="form-control" Width="100%"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Country : <span style="color: red" class="ml-1">*</span></label>
                                <asp:TextBox ID="txtCountry" runat="server" CssClass="form-control" Width="100%" MaxLength="30"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>State : <span style="color: red" class="ml-1">*</span></label>
                                <asp:TextBox ID="txtState" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>City : <span style="color: red" class="ml-1">*</span></label>
                                <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" Width="100%" MaxLength="30"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Pincode : <span style="color: red" class="ml-1">*</span></label>
                                <asp:TextBox ID="txtPinCode" runat="server" CssClass="form-control" Width="100%" MaxLength="20"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Use this address as : <span style="color: red" class="ml-1">*</span></label>
                                <asp:DropDownList ID="ddrAddrName" runat="server" CssClass="form-control" AppendDataBoundItems="True" onchange="handleDropdownChange()">
                                    <asp:ListItem Value="0"><- Select -></asp:ListItem>
                                    <asp:ListItem Value="1">Home</asp:ListItem>
                                    <asp:ListItem Value="2">Office</asp:ListItem>
                                    <asp:ListItem Value="3">Other</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div id="ordAssign">

                        <div class="form-group">
                            <asp:CheckBox ID="chkConfirm" runat="server" TextAlign="Right" Text="" onclick="manageBox()" />
                            <label class="ml-2 mr-4"><span style="color: red"></span>Assign Order To Previous Shop ?</label>

                            <asp:CheckBox ID="chkMonthly" runat="server" TextAlign="Right" CssClass="Space" Text="" />
                            <label class="ml-2"><span style="color: red"></span>Mark as Monthly Order ?</label>
                        </div>

                        <div class="row">
                            <div class="form-group col-md-4">
                                <label>Select Shop To Assign Order : <span style="color: red" class="ml-1">*</span></label>
                                <asp:DropDownList ID="ddrShops" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="0"><- Select -></asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <div class="form-group col-md-4">
                                <label>Enter Shop Code To Assign Order : <span style="color: red"></span></label>
                                <asp:TextBox ID="txtShopCode" runat="server" CssClass="form-control" placeholder="Enter Shop Code"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-4">
                                <label>Next Followup Date : <span style="color: red" class="ml-1">*</span></label>
                                <asp:TextBox ID="txtCalendar" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Button controls starts -->
            <span class="space10"></span>
            <span class="space10"></span>
            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Submit Order" OnClick="btnSave_Click" />
            <div class="float_clear"></div>
            <!-- Button controls ends -->
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

