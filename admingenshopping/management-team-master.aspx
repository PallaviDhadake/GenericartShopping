<%@ Page Title="" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="management-team-master.aspx.cs" Inherits="admingenshopping_management_team_master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
        $(document).ready(function () {
            $.ajax({
                url: '../WebServices/adminShoppingWebService.asmx/GetEmployees',
                method: 'post',
                dataType: 'json',
                success: function (data) {
                    //alert(data);
                    $('#datatable').dataTable({
                        data: data,
                        columns: [
                            { "data": "EmpId" },
                            { "data": "EmpName" },
                            { "data": "EmpCityName" },
                            { "data": "EmpDesignation" },
                            { "data": "EmpMobileNo" },
                            { "data": "EmpEmail" },
                        ]
                    });
                }
            });

            //$('#empdata').DataTable({
            //    processing: true,
            //    serverSide: true,
            //    sServerMethod: 'POST',
            //    ajax: {
            //        url: '../WebServices/adminShoppingWebService.asmx/GetEmployees',
            //        dataSrc: '',
            //        mDataProp: '',
            //        type: 'POST'
            //    },
            //    //data: mydata.data,
            //    aoColumns: [
            //    { mData: "EmpID" },
            //    { mData: "EmpName" },
            //    { mData: "EmpCityName" },
            //    { mData: "EmpDesignation" },
            //    { mData: "EmpMobileNo" },
            //    { mData: "EmpEmail" }
            //    ],
            //    order: [[0, 'asc']]
            //});

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table id="datatable">
        <thead>
            <tr>
                <th>EmpID</th>
                <th>EmpName</th>
                <th>EmpCityName</th>
                <th>EmpDesignation</th>
                <th>EmpMobileNo</th>
                <th>EmpEmail</th>
            </tr>
        </thead>
    </table>
</asp:Content>

