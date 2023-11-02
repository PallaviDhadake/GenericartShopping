<%@ Page Title="" Language="C#" MasterPageFile="~/Shop/ShopMain.master" AutoEventWireup="true" CodeFile="PrescriptionList.aspx.cs" Inherits="PrescriptionList" %>

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
                    <h1>Prescription List</h1>
                </div>
            </div>
        </div>
        <div class="col-sm-8">
            <div class="page-header float-right">
                <div class="page-title">
                    <ol class="breadcrumb text-right">
                        <li class="active">Prescription List</li>
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
                                            <th>Status</th>
                                            <th>Prescription</th>
                                            <th>Accept</th>
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
                                                    <td>UnApproved</td>
                                                    <td><a href="../ImageUpload/<%# Eval("UploadPrescription") %>" target="_blank" class="btn btn-info btn-xs" >View</a></td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkActivate" runat="server" CommandName="Activate" Style="padding: 3px 8px;" CssClass="btn btn-info" CommandArgument='<%#Eval("Auto") %>' Text="Accept" OnClientClick="return confirm('Are you sure to Accept Order');">Accept</asp:LinkButton>
                                                    </td>
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
</asp:Content>

