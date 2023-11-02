<%@ Page Title="Generi-Mitra | Genericart Shopping Admin Panel" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeFile="generi-mitra.aspx.cs" Inherits="admingenshopping_generi_mitra" %>
<%@ MasterType VirtualPath="~/admingenshopping/MasterAdmin.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .mycheckbox input[type="checkbox"] {margin-right: 5px;}
        .stDogerBlue {display:inline-block; padding:3px 4px; background-color:#007bff; border-radius:3px; color:#fff; font-size:0.8em}
    </style>
   
    <script type="text/javascript">

        $(document).ready(function () {
            let gmStatus;
            if (localStorage.getItem("gmStatus") === null) {
                 gmStatus = 0;
            }
            else {
                gmStatus = localStorage.getItem("gmStatus");
                let ddrGmStatusType = document.getElementById("gmStatusType");
                ddrGmStatusType.value = gmStatus;
            }
            var table = $('#datatable').on('xhr.dt', function (e, settings, json, xhr) {

                //You can get response content from xhr object. 
                //Status code: xhr.status 
                //Body ( JSON): xhr.responseJSON
                //Return true to disable the occurrence of error.dt event. 
                if (xhr.status == 500) {
                    toastr.error('Oops Something went wrong on the server');
                }
                if (xhr.status == 408) {
                    toastr.error('Request Timeout ');
                }

                return true;
            }).DataTable({
                "order": [[0, "desc"]],
                columns: [

                    {
                        'data': 'GMitraID',
                        "visible": false
                    },
                    {
                        'data': 'GMitraStatus',
                        "visible": false
                    },

                    { 'data': 'GMitraDate', 'sortable': false },
                    { 'data': 'GMitraName', 'sortable': false },
                    { 'data': 'GMitraEmail', 'sortable': false },

                    {
                        'data': 'GMitraMobile',
                        'sortable': false
                    },
                    { 'data': 'StateName', 'sortable': false },
                    { 'data': 'DistrictName', 'sortable': false },
                    { 'data': 'CityName', 'sortable': false },
                    {
                        'sortable': false,
                        'render': function (data, type, row, meta) {
                            let gmStatus = row.GMitraStatus;
                            switch (gmStatus) {

                                case 0:
                                    return "<span class=\"stDogerBlue\">New</span>";
                                    break;
                                case 1:
                                    return "<span class=\"stGreen\">Active</span>";
                                    break;
                                case 2:
                                    return "<span class=\"stGrey\">Blocked</span>";
                                    break;
                                case 3:
                                    return "<span class=\"stRed\">Deleted</span>";
                                    break;

                            }
                        }
                    },


                    {
                        'sortable': false,
                        'render': function (data, type, row, meta) {
                            return '<a href=generi-mitra.aspx?action=edit&id=' + row.GMitraID + ' class="gAnch" ><a/>';
                        }
                    }
                ],
                bServerSide: true,

                sAjaxSource: '../WebServices/adminShoppingWebService.asmx/GetGeneriMitraCustomerData',
                sServerMethod: 'post',

                "fnServerParams": function (aoData) {

                    aoData.push({ "name": "GmStatus", "value": "" + gmStatus + "" }
                    );
                },


                "processing": true,
                'language': {
                    'loadingRecords': '&nbsp;',
                    'processing': '<div class="spinner"></div>',
                    "infoFiltered": ""
                },
                "responsive": true


            });
            ////function for geting column value
            //table.rows(function (idx, data, node) {
            //    return data.tagged == 0;
            //}).select();

            $('#btnSearch1').click(function (e) {
                e.preventDefault();
                gmStatus = document.getElementById("gmStatusType").value;
                localStorage.setItem("gmStatus", ""+ gmStatus +"");
                table.ajax.reload();

            });
            const params = new URLSearchParams(window.location.search);
            const action = params.get('action');

            if (action === "edit") {

                //viewCust.hide();
                $('#viewCustomer').hide();
            }

        });


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">Generic Mitra Master</h2>
    <span class="space15"></span>

    <div id="viewCustomer">
        <label>Select Status Type: </label>
        <div class="row">
            <div class="form-group col-sm-4">
                <select name="gmStatusType" id="gmStatusType" class="form-control" autofocus>
                    <%--<option value="none" selected disabled hidden>Select an Option</option>--%>
                    <option value="0" selected>New</option>
                    <option value="1">Active</option>
                    <option value="2">Blocked</option>
                    <option value="3">Deleted</option>
                </select>
            </div>
            <div class="col-sm-1">
                <button id="btnSearch1" class="btn btn-primary">Search</button>
            </div>
        </div>
        <span class="space20"></span>



        <div class="width100">
            <table id="datatable" class="table table-striped table-bordered table-hover w-100">
                <thead class="thead-dark">
                    <tr>

                        <th>Customer Id</th>
                        <th>Gm Status</th>
                        <th>Date</th>
                        <th>Name</th>
                        <th>Email</th>
                        <th>Mobile No</th>
                        <th>State</th>
                        <th>District</th>
                        <th>City</th>
                        <th>Status</th>
                        <th></th>
                    </tr>
                </thead>
            </table>
        </div>

    </div>


    <%--  <div id="viewGeneriMitra" runat="server">
        <span class="space25"></span>
        <asp:GridView ID="gvGeneriMitra" runat="server" AutoGenerateColumns="False"
            CssClass="table table-striped table-bordered table-hover" GridLines="None" Width="100%" OnRowDataBound="gvGeneriMitra_RowDataBound">
            <RowStyle CssClass="" />
            <HeaderStyle CssClass="bg-dark" />
            <AlternatingRowStyle CssClass="alt" />
            <Columns>
                <asp:BoundField DataField="GMitraID">
                    <HeaderStyle CssClass="HideCol" />
                    <ItemStyle CssClass="HideCol" />
                </asp:BoundField>
                <asp:BoundField DataField="GMitraStatus">
                    <HeaderStyle CssClass="HideCol" />
                    <ItemStyle CssClass="HideCol" />
                </asp:BoundField>

                <asp:BoundField DataField="GMitraDate" HeaderText="Date">
                    <ItemStyle Width="10%" />
                </asp:BoundField>

                <asp:BoundField DataField="GMitraName" HeaderText="Name">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="GMitraMobile" HeaderText="Mobile">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="StateName" HeaderText="State">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="DistrictName" HeaderText="District">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="CityName" HeaderText="City">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                 <asp:TemplateField HeaderText="Status">
                    <ItemStyle Width="10%" />
                    <ItemTemplate>
                        <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>



                <asp:TemplateField>
                    <ItemStyle Width="10%" />
                    <ItemTemplate>
                        <asp:Literal ID="litAnch" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <span class="warning">No Generimitra to Display :(</span>
            </EmptyDataTemplate>
            <PagerStyle CssClass="gvPager" />
        </asp:GridView>
    </div>--%>


    <%--New/Edit Form Start--%>
    <div id="editGeneriMitra" runat="server">
        <div class="card card-primary">
            <div class="card-header">
                <h3 class="card-title"><%= pgTitle %></h3>
            </div>
            <%-- card body--%>
            <div class="card-body">
                <div class="colorLightBlue">
                    <span>Id :</span>
                    <asp:Label ID="lblId" runat="server"></asp:Label>
                </div>

                <span class="space15"></span>
                <%--form row start--%>
                <div class="form-row">
                    <div class="form-group col-md-6">
                        <label>Name :*</label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Width="100%" MaxLength="200"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Mobile :*</label>
                        <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                    </div>


                    <div class="form-group col-md-6">
                        <label>Email :*</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                    </div>

                    <div class="form-group col-md-6">
                        <label>Username :*</label>
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                    </div>

                    <div class="form-group col-md-6">
                        <label>Password :*</label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                    </div>


                    <div class="form-group col-md-6">
                        <label>Bank Name:*</label>
                        <asp:TextBox ID="txtBankName" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Bank Account Name:*</label>
                        <asp:TextBox ID="txtAccName" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Bank Account No:*</label>
                        <asp:TextBox ID="txtAccNo" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-6">
                        <label>IFSC Code:*</label>
                        <asp:TextBox ID="txtIfsc" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-6">
                        <label>PAN No:*</label>
                        <asp:TextBox ID="txtPan" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                    </div>

                    <div class="form-group col-md-6">
                        <label>State :*</label>
                        <asp:DropDownList ID="ddrState" runat="server" CssClass="form-control" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddrState_SelectedIndexChanged">
                            <asp:ListItem Value="0"><- Select -></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-md-6">
                        <label>District :*</label>
                        <asp:DropDownList ID="ddrDistrict" runat="server" CssClass="form-control" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddrDistrict_SelectedIndexChanged">
                            <asp:ListItem Value="0"><- Select -></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-md-6">
                        <label>City :*</label>
                        <asp:DropDownList ID="ddrCity" runat="server" CssClass="form-control" Width="100%">
                            <asp:ListItem Value="0"><- Select -></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-md-6">
                       <label>Parent Shop Code</label>
                        <asp:TextBox ID="txtShopCode" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Pancard :</label><span class="space10"></span>
                        <%= pancardImg %>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Adhar Card :</label><span class="space10"></span>
                        <%= adharcardImg %>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Cheque / Passbook  :</label><span class="space10"></span>
                        <%= bankdocImg %>
                    </div>

                </div>
                <div class="form-group">
                    <asp:CheckBox ID="chkApprove" runat="server" TextAlign="Right" Text="Approve with documents" CssClass="mycheckbox" />
                    <%--<label class="form-check-label"><strong>Approve ?</strong> </label>--%>
                </div>
                <%--form row End--%>
            </div>
            <%-- card body end--%>
        </div>
        <%--card end--%>

        <!-- Button controls starts -->
        <span class="space10"></span>
        <span class="space10"></span>
        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Modify Info" OnClick="btnSave_Click" />

        <asp:Button ID="btnBlock" runat="server" CssClass="btn btn-warning" OnClick="btnBlock_Click" />


        <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-outline-info" Text="Delete" OnClientClick="return confirm('Are you sure to delete?');" OnClick="btnDelete_Click" />
        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-outline-dark" Text="Cancel" OnClick="btnCancel_Click"/>
        <div class="float_clear"></div>
    </div>
    <%--New/Edit Form Start--%>
</asp:Content>

