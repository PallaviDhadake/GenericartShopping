<%@ Page Title="Delivered Order Follow-Up | Genericart Shopping Support Team" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="staff-followup-delivered-order.aspx.cs" Inherits="supportteam_staff_followup_delivered_order" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   <%-- <script>
        $(document).ready(function () {
            $('[id$=gvDeliveredOrd]').DataTable({
                columnDefs: [
                    { orderable: false, targets: [0, 3, 4, 5] }
                ],
                order: [[0, 'desc']]
            });
        });
    </script>--%>

     <script type="text/javascript">

         $(document).ready(function () {


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
                 "order": [[4, "desc"]],
                 columns: [
                    
                     {
                         'data': 'FK_OrderCustomerID',
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
                             return '<a href=staff-followup-form.aspx?type=deliverord&id=' + row.FK_OrderCustomerID + ' class="btn btn-sm btn-primary" >Follow Up<a/>';
                         }
                     }
                 ],
                 bServerSide: true,
                 
                 sAjaxSource: '../WebServices/supportTeamWebServices.asmx/GetOrders',
                 sServerMethod: 'post',

                 "fnServerParams": function (aoData) {
                     aoData.push({ "name": "orderStatus", "value": "7" });
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

         });
     </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <h2 class="pgTitle">Delivered Orders List</h2>
    <span class="space15"></span>


    <div class="width100">
            <table id="datatable" class="table table-striped table-bordered table-hover w-100">
                <thead class="thead-dark">
                    <tr>
                        
                        <th>Customer Id</th>
                        <th>Name</th>
                        <th>Email</th>
                        <th>Mobile No</th>
                        <th>Total Orders</th>
                        <th>Recent Orders</th>
                        <th></th>
                    </tr>
                </thead>
            </table>
        </div>




   <%-- <div id="viewDeliveredOrdFollowup" runat="server">
      
        <asp:GridView ID="gvDeliveredOrd" runat="server" AutoGenerateColumns="False"
            CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None" Width="100%" OnRowDataBound="gvDeliveredOrd_RowDataBound">
            
            <HeaderStyle CssClass="bg-dark" />
            <AlternatingRowStyle CssClass="alt" />
            <Columns>
                <asp:BoundField DataField="FK_OrderCustomerID">
                    <HeaderStyle CssClass="HideCol" />
                    <ItemStyle CssClass="HideCol" />
                </asp:BoundField>

                <asp:BoundField DataField="OrderID">
                    <HeaderStyle CssClass="HideCol" />
                    <ItemStyle CssClass="HideCol" />
                </asp:BoundField>
                


                 <asp:BoundField DataField="CustomerName" HeaderText="Name">
                    <ItemStyle Width="5%" />
                </asp:BoundField>
                <asp:BoundField DataField="CustomerEmail" HeaderText="Email">
                    <ItemStyle Width="5%" />
                </asp:BoundField>
                <asp:BoundField DataField="CustomerMobile" HeaderText="Mobile No">
                    <ItemStyle Width="5%" />
                </asp:BoundField>
                

                <asp:TemplateField>
                    <ItemStyle Width="5%" />
                    <ItemTemplate>
                        <asp:Literal ID="litAnch" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <span class="warning">No Customer to Display :(</span>
            </EmptyDataTemplate>
            <PagerStyle CssClass="gvPager" />
        </asp:GridView>
     
    </div>--%>

</asp:Content>