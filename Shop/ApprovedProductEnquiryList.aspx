<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/Shop/ApprovedProductEnquiryList.aspx.cs" Inherits="ApprovedProductEnquiryList" MasterPageFile="~/Shop/ShopMain.master" %>

<asp:Content ContentPlaceHolderID="head" runat="server">



    <style>
        .fade.in {
            opacity: 1;
        }

        .show {
            display: block !important;
            opacity: 1;
        }

            .show .modal-dialog {
                margin-top: 20vh;
            }

        .modal.in .modal-dialog {
            -webkit-transform: translate(0,0);
            -ms-transform: translate(0,0);
            transform: translate(0,0);
        }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="breadcrumbs">
        <div class="col-sm-4">
            <div class="page-header float-left">
                <div class="page-title">
                    <h1>Approved Product Enquiry List</h1>
                </div>
            </div>
        </div>
        <div class="col-sm-8">
            <div class="page-header float-right">
                <div class="page-title">
                    <ol class="breadcrumb text-right">
                        <li class="active">Approved Product Enquiry List</li>
                    </ol>
                </div>
            </div>
        </div>
    </div>
    <div class="content mt-3">
        <div class="col-md-12">
            <div class="box box-primary">
                <div class="box-body hidden">
                    <asp:HiddenField ID="HF1" runat="server" />
                    <label class="col-sm-3 col-md-3 col-xs-12">Date From</label>
                    <div class="col-sm-9 col-md-9 col-xs-12">
                        <div class="input-group">
                            <asp:TextBox ID="TxtDateFrom" placeholder="Select Date" CssClass="form-control" runat="server" AutoComplete="off"></asp:TextBox>
                            <span class="input-group-addon" id="btnGroupAddon"><i class="fa fa-calendar"></i></span>
                        </div>

                        <script>
                            var picker1 = new Pikaday(
                                {
                                    field: document.getElementById('<%= TxtDateFrom.ClientID %>'),
                                    firstDay: 1,
                                    minDate: new Date('01-01-1950'),
                                    maxDate: new Date('30-12-2020'),
                                    yearRange: [1950, 2020]
                                });
                        </script>
                    </div>
                </div>
                <div class="form-group row col-md-5 col-xs-12 hidden">
                    <label class="col-sm-2 col-md-2 col-xs-12">Date to</label>
                    <div class="col-sm-10 col-md-10 col-xs-12">
                        <div class="input-group">
                            <asp:TextBox ID="TxtDateTo" runat="server" placeholder="Select Date" autocomplete="off" CssClass="form-control"></asp:TextBox>
                            <span class="input-group-addon" id="Span1"><i class="fa fa-calendar"></i></span>
                        </div>
                        <script>
                            var picker1 = new Pikaday(
                                {
                                    field: document.getElementById('<%= TxtDateTo.ClientID %>'),
                                    firstDay: 1,
                                    minDate: new Date('01-01-1950'),
                                    maxDate: new Date('30-12-2020'),
                                    yearRange: [1950, 2020]
                                });
                        </script>
                    </div>
                </div>
                <div class="col-md-2 col-xs-12 hidden">
                    <asp:Button ID="BtnSearch" runat="server" OnClick="BtnSearch_Click" CssClass="btn btn-info center" Text="Search" />
                </div>
                <div class="col-xs-12 col-md-12">
                    <table id="example1" class="table table-striped responsive-utilities jambo_table">
                        <thead>
                            <tr class="headings">
                                <th>SI.No</th>
                                <th>Name</th>
                                <th>Date of Joining</th>
                                <th>Cart Id</th>
                                <th>Product</th>
                                <th>Quantity</th>
                                <th class=" no-link last"><span class="nobr">View</span></th>
                                <th class=" no-link last"><span class="nobr">Deliver</span></th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
                                <ItemTemplate>
                                    <tr class="even pointer">
                                        <td><%# Eval("Row") %></td>
                                        <td><%# Eval("Name") %></td>
                                        <td><%# Eval("Date") %></td>
                                        <td><%# Eval("CartId") %></td>
                                        <td><%# Eval("ProductName") %></td>
                                        <td><%# Eval("TotalQuantity") %></td>
                                        <td>
                                            <asp:LinkButton ID="LinkView" runat="server" CommandName="ViewThis" CommandArgument='<%# Eval("CartId") %>' CssClass="btn btn-info btn-xs">View</asp:LinkButton></td>
                                        <td>
                                            <asp:LinkButton ID="LinkDeliver" runat="server" CommandName="DeliverThis" CommandArgument='<%# Eval("CartId") %>' CssClass="btn btn-info btn-xs" OnClientClick="return confirm('Are you sure to Update Delivery Detail');">Deliver</asp:LinkButton></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>

    <div id="myModal" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content modal-lg">
                <div class="modal-body">
                    <table id="example1" class="table table-striped responsive-utilities jambo_table">
                        <thead>
                            <tr class="headings">
                                <th>SI.No</th>
                                <th>Product</th>
                                <th>Quantity</th>
                                <th class="text-right">Rate</th>
                                <th class="text-right">Amount</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="RepDetail" runat="server">
                                <ItemTemplate>
                                    <tr class="even pointer">
                                        <td><%# Eval("Row") %></td>
                                        <td><%# Eval("ItemName") %></td>
                                        <td><%# Eval("Quantity") %></td>
                                        <td class="text-right"><%# Eval("SaleRate") %></td>
                                        <td class="text-right"><%# Eval("Amount") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
                <div class="modal-footer">
                    <button type="submit" class="btn btn-primary">Hide</button>
                </div>
            </div>
        </div>
    </div>

    <div id="myModalDeliver" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content modal-lg">
                <div class="modal-header">
                    <h4>Update Delivery Detail</h4>
                </div>
                <div class="modal-body" style="line-height: 45px;">
                    <div class="col-md-4 col-xs-12">
                        Delivery Date<br />
                    </div>
                    <div class="col-md-6 col-xs-12">
                        <asp:TextBox ID="TxtDeliveryDate" runat="server" CssClass="form-control" Style="display: inline-block;" placeholder="Select Delivery Date"></asp:TextBox>
                        <script>
                            var picker = new Pikaday(
                                {
                                    field: document.getElementById('<%= TxtDeliveryDate.ClientID %>'),
                                    firstDay: 1,
                                    minDate: new Date('01-01-1950'),
                                    maxDate: new Date('30-12-2020'),
                                    yearRange: [1950, 2020]
                                });
                        </script>
                    </div>
                    <div style="clear: both;"></div>
                    <div class="col-md-4 col-xs-12">
                        Mode of Delivery<br />
                    </div>
                    <div class="col-md-6 col-xs-12">
                        <asp:TextBox ID="TxtModeofDelivery" runat="server" CssClass="form-control" placeholder="Enter Mode of Delivery">
                        </asp:TextBox>
                    </div>
                    <div style="clear: both;"></div>
                    <div class="col-md-4 col-xs-12">
                        Document Number<br />
                    </div>
                    <div class="col-md-6 col-xs-12">
                        <asp:TextBox ID="TxtDocumentNumber" runat="server" CssClass="form-control" placeholder="Enter Document Number"></asp:TextBox>
                    </div>
                    <div style="clear: both;"></div>
                    <div class="col-md-4 col-xs-12">
                        Delivery Person Name<br />
                    </div>
                    <div class="col-md-6 col-xs-12">
                        <asp:TextBox ID="TxtDeliveryPersonName" runat="server" CssClass="form-control" placeholder="Enter Delivery Person Name"></asp:TextBox>
                    </div>
                    <div style="clear: both;"></div>
                     <div class="col-md-4 col-xs-12">
                        Final Deliver<br />
                    </div>
                    <div class="col-md-6 col-xs-12">
                        <asp:CheckBox ID="ChkDeliver" runat="server" CssClass="form-control"></asp:CheckBox>
                    </div>
                    <div style="clear: both;"></div>
                    <div class="col-md-4 col-xs-12">
                        &nbsp;
                    </div>
                    <div class="col-md-6 col-xs-12">
                        <asp:Button ID="BtnDeliver" CssClass="btn btn-primary" runat="server" Text="Update Delivery Detail" OnClick="BtnDeliver_Click" />
                    </div>
                    <div style="clear: both;"></div>
                </div>
                <div class="modal-footer">
                    <button type="submit" class="btn btn-primary">Hide</button>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="HFCartId" runat="server" />

</asp:Content>
