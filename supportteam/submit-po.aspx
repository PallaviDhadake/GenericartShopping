<%@ Page Title="Submit Customer's Purchase Order" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeFile="submit-po.aspx.cs" Inherits="supportteam_submit_po" %>

<%@ MasterType VirtualPath="~/supportteam/MasterSupport.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />
    <link rel="stylesheet" href="/resources/demos/style.css" />
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script type="text/javascript">
        $(function () {
            //alert("get med load call");
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
    </script>

    <script type="text/javascript">
        $(function () {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        createDataTable();
                    }
                });
            };

            createDataTable();
            function createDataTable() {
                $('#<%= gvOrderDetails.ClientID %>').prepend($("<thead></thead>").append($('#<%= gvOrderDetails.ClientID %>').find("tr:first"))).DataTable({

                    columnDefs: [
                        { orderable: false, targets: [0, 1, 2, 3, 5, 4, 6, 7, 8] }
                    ],
                    order: [[0, 'desc']]
                });

            }
        });
    </script>

    <script>
        window.onload = function () {
            duDatepicker('#<%= txtCalendar.ClientID %>', {
                auto: true, inline: true, format: 'dd/mm/yyyy',
            });

            restoreDdrItemValue();
        }
    </script>

    <style type="text/css">
        .Space label {
            margin-left: 5px;
            margin-right: 15px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle"><%= pageTitle %></h2>
    <span class="space15"></span>

    <div id="EnqMedicines" runat="server">
        <div class="card-header">
            <h3 class="large colorLightBlue">Enquiry Medicines Overview</h3>
        </div>
        <div class="card-body">
            <span class="space15"></span>
            <asp:GridView ID="gvEnqMedicine" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None" PageSize="30" AllowPaging="true"
                AutoGenerateColumns="false">
                <RowStyle CssClass="" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="BrandMedicine" HeaderText="Brand Medicine">
                        <ItemStyle Width="20%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="BrandPrice" HeaderText="Brand Price">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="GenericMedicine" HeaderText="Generic Medicine">
                        <ItemStyle Width="20%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="GenericCode" HeaderText="Generic Code">
                        <ItemStyle Width="12%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="GenericPrice" HeaderText="Generic Price">
                        <ItemStyle Width="12%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="SavingAmount" HeaderText="Saving Amount">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="SavingPercent" HeaderText="Net Savings">
                        <ItemStyle Width="12%" />
                    </asp:BoundField>
                </Columns>
                <EmptyDataTemplate>
                    <span class="warning">:(</span>
                </EmptyDataTemplate>
                <PagerStyle CssClass="gvPager" />
            </asp:GridView>
        </div>
    </div>

    <h3 class="large colorLightBlue" style="margin-left: 20px;">New Order</h3>
    <div class="card card-primary">
        <div class="card-header">
            <h3 class="card-title">Submit PO</h3>
        </div>
        <div class="card-body">
            <div class="form-row">
                <div class="form-group col-md-6">
                    <label>Medicine Name :*</label>
                    <asp:TextBox ID="txtMedName" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                </div>
                <div class="form-group col-md-3">
                    <label>Quantity :*</label>
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
            <div>
                <asp:GridView ID="gvOrderDetails" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
                    AutoGenerateColumns="false" OnRowDataBound="gvOrderDetails_RowDataBound" OnRowCommand="gvOrderDetails_RowCommand">
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
                        <asp:TemplateField HeaderText="">
                            <ItemStyle Width="5%" />
                            <ItemTemplate>
                                <asp:Button ID="cmdDelete" runat="server" CssClass="gDel" CommandName="gvDel" Text="" OnClientClick="return confirm('Are you sure to delete?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="FK_DetailOrderID">
                            <HeaderStyle CssClass="HideCol" />
                            <ItemStyle CssClass="HideCol" />
                        </asp:BoundField>
                    </Columns>
                    <EmptyDataTemplate>
                        <span class="warning">:(</span>
                    </EmptyDataTemplate>
                    <PagerStyle CssClass="gvPager" />
                </asp:GridView>
                <span class="text-indigo text-lg text-bold"><%= ordAmmount %></span>
            </div>
            <span class="space15"></span>
            <asp:CheckBox ID="chkCODType" runat="server" TextAlign="Right" CssClass="Space" Text="Is this COD (Cash On Delivery)?" />
            <asp:CheckBox ID="chkSelfPickup" runat="server" TextAlign="Right" CssClass="Space" Text="Self Pickup" />
            <br />
            <span class="themeClrSec fontRegular dspBlk mrg_B_10 semiMedium"><b>Upload Payment Slip & Prescription</b></span>
            <span class="reqPrescription"></span><span class="small semiBold colrPink mrg_B_20"><i>Upload Payment Slip & Prescription </i></span>
            <br />
            <span class="reqPrescription"></span><span class="small semiBold colrPink mrg_B_20"><i>File size Less than 5 MB </i></span>
            <span class="space30"></span>
            <asp:FileUpload ID="fuPaymentSlip" runat="server" />
            <br />
            <br />
            <div class="form-row">
                <div class="form-group col-md-4">
                    <asp:Button ID="btnPaymentSlip" runat="server" Text="Upload Payment Slip & Prescription" CssClass="btn btn-primary mr-lg-1" OnClick="btnPaymentSlip_Click" />
                </div>
                <div class="form-group col-md-4">
                    <asp:Button ID="btnRmPaymentSlip" runat="server" Text="Remove Payment Slip & Prescription" CssClass="btn btn-primary mr-lg-1" OnClick="btnRmPaymentSlip_Click" />
                </div>
            </div>
            <span class="space20"></span>
            <%= paymentslip %>

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
                <div id="courieroption" class="form-group col-md-6" style="display:none">
                    <label>Courier Option :</label>
                    <asp:TextBox ID="txtCourier" runat="server" CssClass="form-control" Width="100%" MaxLength="50" placeholder="Courier Option"></asp:TextBox>
                </div>
            </div>

            <div id="AddressBox" runat="server" visible="false">
                <div id="existingAddr" runat="server">
                    <span class="space30"></span>
                    <h3 class="text-blue pageH3">Select Shipping Address</h3>
                    <asp:DropDownList ID="ddrAddress" runat="server" CssClass="form-control">
                        <asp:ListItem Value="0"><- Select -></asp:ListItem>
                    </asp:DropDownList>
                    <div class="text-center text-lg text-bold text-blue">OR</div>
                </div>
                <div id="newAddr" runat="server">
                    <span class="space10"></span>
                    <h3 class="text-blue pageH3">Add Shipping Address :</h3>
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label>Address :*</label>
                            <asp:TextBox ID="txtAddress1" runat="server" TextMode="MultiLine" Height="120" CssClass="form-control" Width="100%"></asp:TextBox>
                        </div>
                        <div class="form-group col-md-6">
                            <label>Country :*</label>
                            <asp:TextBox ID="txtCountry" runat="server" CssClass="form-control" Width="100%" MaxLength="30"></asp:TextBox>
                        </div>
                        <div class="form-group col-md-6">
                            <label>State :*</label>
                            <asp:TextBox ID="txtState" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                        </div>
                        <div class="form-group col-md-6">
                            <label>City :*</label>
                            <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" Width="100%" MaxLength="30"></asp:TextBox>
                        </div>
                        <div class="form-group col-md-6">
                            <label>Pincode :*</label>
                            <asp:TextBox ID="txtPinCode" runat="server" CssClass="form-control" Width="100%" MaxLength="20"></asp:TextBox>
                        </div>
                        <div class="form-group col-md-6">
                            <label>Use this address as :*</label>
                            <asp:DropDownList ID="ddrAddrName" runat="server" CssClass="form-control">
                                <asp:ListItem Value="0"><- Select -></asp:ListItem>
                                <asp:ListItem Value="1">Home</asp:ListItem>
                                <asp:ListItem Value="2">Office</asp:ListItem>
                                <asp:ListItem Value="3">Other</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div id="ordAssign">
                    <asp:CheckBox ID="chkMonthly" runat="server" TextAlign="Right" CssClass="Space" Text=" Mark as Monthly Order ?" />
                    <div class="row">
                        <div class="col-md-4">
                            <span class="col-form-label text-bold">Select Shop To Assign Order :</span>
                            <asp:DropDownList ID="ddrShops" runat="server" CssClass="form-control">
                                <asp:ListItem Value="0"><- Select -></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <span class="col-form-label text-bold">Enter Shop Code To Assign Order :</span>
                            <asp:TextBox ID="txtShopCode" runat="server" CssClass="form-control" placeholder="Enter Shop Code"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <span class="col-form-label text-bold"><span style="color: red">*</span>Next Followup Date : </span>
                            <asp:TextBox ID="txtCalendar" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>

            <span class="space10"></span>

            <span class="space20"></span>
            <asp:Button ID="btnSubmitOrder" runat="server" Text="Submit Order" CssClass="btn btn-md btn-success" OnClick="btnSubmitOrder_Click" />
        </div>
    </div>

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
</asp:Content>

