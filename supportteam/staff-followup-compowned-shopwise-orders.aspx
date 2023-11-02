<%@ Page Title="Shopwise Orders Customer List | Genericart Shopping Support Team" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="staff-followup-compowned-shopwise-orders.aspx.cs" Inherits="supportteam_staff_followup_compowned_shopwise_orders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        window.onload = function () {
            duDatepicker('#fromDate', {
                auto: false, inline: true, format: 'dd/mm/yyyy',

            });
            duDatepicker('#toDate', {
                auto: false, inline: true, format: 'dd/mm/yyyy',

            });
        }
    </script>
    <script type="text/javascript">

        $(document).ready(function () {
            let orderStatus = 0;
            let fromDate = '';
            let toDate = '';
            let urlParams = new URLSearchParams(window.location.search);
            let frId = urlParams.get('franchId');
            let teamId = '<%= Session["adminSupport"].ToString() %>';

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
                "order": [[5, "desc"]],
                columns: [

                    {
                        'data': 'FK_OrderCustomerID',
                        "visible": false,
                    },
                    {
                        'data': 'FeedBackFlag',
                        "visible": false,
                    },

                    { 'data': 'CustomerName' },
                    { 'data': 'CustomerEmail' },
                    {
                        'data': 'CustomerMobile',
                        'sortable': false,
                    },
                    { 'data': 'totalOrdersCount' },
                    {
                        'data': 'recentOrderId',
                        'sortable': false,
                    },

                    {
                        'sortable': false,
                        'render': function (data, type, row, meta) {
                            if (row.FeedBackFlag == 1) {
                                return '<span class=\"btn btn-sm btn-success\"><i class=\"fa fa-check\"></i> Follow Up Recored</span>'
                            }
                            else {
                                return '<a href=staff-followup-form.aspx?type=shopwisecustid&id=' + row.FK_OrderCustomerID + '&frId=' + frId + ' class="btn btn-sm btn-primary" >Follow Up<a/>';
                            }
                           
                        }
                    }
                ],
                bServerSide: true,

                sAjaxSource: '../WebServices/supportTeamWebServices.asmx/GetShopwiseCustomerData',
                sServerMethod: 'post',

                "fnServerParams": function (aoData) {

                    aoData.push({ "name": "orderStatus", "value": "" + orderStatus + "" },
                        { "name": "frId", "value": "" + frId + "" },
                        { "name": "teamId", "value": "" + teamId + "" },
                        { "name": "fromDate", "value": "" + fromDate + "" },
                        { "name": "toDate", "value": "" + toDate + "" }
                    );
                },


                "processing": true,
                'language': {
                    'loadingRecords': '&nbsp;',
                    'processing': '<div class="spinner"></div>',
                    "infoFiltered": ""
                },
                "responsive": true,

                //"columnDefs": [
                //    {
                //    "targets": [0],
                //    "visible": false,
                //    "searchable": false
                //    }
                //]
            });
            //function for geting column value
            table.rows(function (idx, data, node) {
                return data.tagged == 0;
            }).select();

            $('#btnSearch1').click(function (e) {
                e.preventDefault();
                orderStatus = document.getElementById("orderType").value;
                table.ajax.reload();

            });
            $('#btnSearch2').click(function (e) {
                e.preventDefault();
                let oldFromDate = document.getElementById("fromDate").value;
                let oldToDate = document.getElementById("toDate").value;

                let newFromDate = oldFromDate.split("/").reverse().join("-");
                let newToDate = oldToDate.split("/").reverse().join("-");
                fromDate = newFromDate;
                toDate = newToDate;

                table.ajax.reload();

            });
            $('#btnReset').click(function (e) {
                e.preventDefault();
                fromDate = '';
                toDate = '';
                table.ajax.reload();

            });
        });


    </script>
   

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">All Customers List</h2>
    <span class="space15"></span>


    <label>Select Order Type: </label>
    <div class="row">
        <div class="form-group col-sm-4">
            <select name="orderType" id="orderType" class="form-control" autofocus>
                <%--<option value="none" selected disabled hidden>Select an Option</option>--%>
                <option value="0" selected>Pending</option>
                <option value="1">Accepted</option>
                <option value="2">Rejected</option>
                <option value="2">In-Process</option>
                <option value="6">Shipped</option>
                <option value="7">Delivered</option>
            </select>
        </div>
        <div class="col-sm-1">
            <button id="btnSearch1" class="btn btn-primary">Search</button>
        </div>
    </div>
    <span class="space20"></span>
    <div class="row">

        <%--Date To start--%>
        <div class="p-1">
            <label>Select From Date:</label>
        </div>
        <div class="form-group col-sm-2">
            <input type="text" id="fromDate" class="form-control" placeholder="click to open calender">
        </div>
        <%--Date To end--%>
        <%--Date To start--%>
        <div class="p-1">
            <label>Select To Date:</label>
        </div>
        <div class="form-group col-sm-2">
            <input type="text" id="toDate" class="form-control" placeholder="click to open calender">
        </div>
        <%--Date To end--%>
        <%--button--%>
        <div class=" col-sm-1">
            <button id="btnSearch2" class="btn btn-primary">Search</button>

        </div>
        <div class="col-sm-1">
            <button id="btnReset" class="btn btn-outline-dark">Reset</button>
        </div>
        <%--button end--%>
    </div>


    <div class="width100">
        <table id="datatable" class="table table-striped table-bordered table-hover w-100">
            <thead class="thead-dark">
                <tr>

                    <th>Customer Id</th>
                    <th>FeedBack Flag</th>
                    <th>Name</th>
                    <th>Email</th>
                    <th>Mobile No</th>
                    <th>Total Orders</th>
                    <th>Recend Orders</th>
                    <th></th>
                </tr>
            </thead>
        </table>
    </div>



</asp:Content>

