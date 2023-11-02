<%@ Page Title="List of Generic Mitra" Language="C#" MasterPageFile="~/bdm/MasterBdm.master" AutoEventWireup="true" CodeFile="generi-mitra.aspx.cs" Inherits="bdm_generi_mitra" %>
<%@ MasterType VirtualPath="~/bdm/MasterBdm.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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

            $('#btnSearch1').click(function (e) {
                e.preventDefault();
                gmStatus = document.getElementById("gmStatusType").value;
                localStorage.setItem("gmStatus", "" + gmStatus + "");
                table.ajax.reload();

            });
        });


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Generic Mitra</h2>
    <span class="space15"></span>

    <div id="viewCustomer">
        <label>Select Status Type: </label>
        <div class="row">
            <div class="form-group col-sm-4">
                <select name="gmStatusType" id="gmStatusType" class="form-control" autofocus>
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
                    </tr>
                </thead>
            </table>
        </div>

    </div>
</asp:Content>

