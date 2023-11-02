<%@ Page Title="" Language="C#" MasterPageFile="~/Shop/ShopMain.master" AutoEventWireup="true" CodeFile="ApprovedPrescription.aspx.cs" Inherits="ApprovedPrescription" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .table.table-bordered tbody td, .table > thead:first-child > tr:first-child > th {
            font-size: 12px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="breadcrumbs">
        <div class="col-sm-4">
            <div class="page-header float-left">
                <div class="page-title">
                    <h1>Approved Prescription</h1>
                </div>
            </div>
        </div>
        <div class="col-sm-8">
            <div class="page-header float-right">
                <div class="page-title">
                    <ol class="breadcrumb text-right">
                        <li class="active">Approved Prescription</li>
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
                    <div class="row text-left">
                        <div class="col-md-12 col-sm-12 col-xs-12 m-b-15 table-responsive" style="font-size: 12px; overflow-x: visible">
                            <div class="row table-responsive">
                                <asp:Label ID="LblMassageShow" runat="server"></asp:Label>
                                <table id="example1" class="table table-bordered table-striped table-hover">
                                    <thead>
                                        <tr>
                                            <th>Sn</th>
                                            <th>Name</th>
                                            <th>Date of Joining</th>
                                            <th>Cart Id</th>
                                            <th>Prescription</th>
                                            <th>Update Detail</th>
                                            <th>Customer Status</th>
                                            <th>Status</th>
                                            <th>Deliver</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
                                            <ItemTemplate>
                                                <tr>
                                                    <td><%# Container.ItemIndex + 1 %></td>
                                                    <td><%# Eval("Name") %></td>
                                                    <td><%# Eval("Date", "{0:dd/MM/yyyy}") %></td>
                                                    <td><%# Eval("CartId") %></td>
                                                    <td><a href="../ImageUpload/<%# Eval("UploadPrescription") %>" target="_blank" class="btn btn-info btn-xs">View</a></td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkUpdate" runat="server" CommandName="UpdateThis" CommandArgument='<%# Eval("Auto") %>' CssClass="btn btn-info btn-xs">Update Detail</asp:LinkButton></td>
                                                    <td><%# Eval("CustomerStatus") %></td>
                                                    <td>UnApproved</td>
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
            </div>
        </div>
    </div>

    <div id="myModal" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content modal-lg">
                <div class="modal-body">
                    <div class="col-md-12 col-xs-12">
                        <div style="clear: both; height: 15px;"></div>
                        Update Detail for Cart No
                        <asp:Label ID="LblCartNo" runat="server"></asp:Label>
                        <div style="clear: both; height: 10px;"></div>
                        <br />
                        <asp:TextBox ID="TxtShopDetail" placeholder="Enter Detail" CssClass="form-control" runat="server" AutoComplete="off" TextMode="MultiLine" Rows="5"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="Req1" runat="server" ValidationGroup="UploadDetail" ControlToValidate="TxtShopDetail" Display="Dynamic" SetFocusOnError="true" ErrorMessage="Enter Description" CssClass="text-red"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-12 col-xs-12" style="margin-top: 10px;">
                        <asp:Button ID="BtnUpdateShopDetail" CssClass="btn btn-primary" runat="server" Text="Update Detail" ValidationGroup="UploadDetail" OnClick="BtnUpdateShopDetail_Click"></asp:Button>
                        <asp:ValidationSummary ID="VS1" runat="server" ValidationGroup="UploadDetail" ShowMessageBox="false" ShowSummary="false" />
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="submit" class="btn btn-primary">Close</button>
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
</asp:Content>

