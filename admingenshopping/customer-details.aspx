<%@ Page Title="Customer Details Page | Genericart Shopping Admin Panel" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="customer-details.aspx.cs" EnableEventValidation="false" ValidateRequest="false" Inherits="admingenshopping_customer_details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
  
     <script type="text/javascript">
         $(document).ready(function () {
             var table = $('#datatable').DataTable({
                 responsive: false,
                 "order": [[1, "desc"]],
                 columns: [
                     { 'data': 'CustomrtID', "visible": false },
                     { 'data': 'JoinDate' },
                     { 'data': 'CustomerName' },
                     { 'data': 'CustomerMobile', 'sortable': false },
                     { 'data': 'CustomerEmail', 'sortable': false,  },
                     { 'data': 'DeviceType', 'sortable': false, },
                     { 'data': 'CustomerPassword', 'sortable': false, },
                     {
                         'data': 'CustomerName',
                         'sortable': false,
                         'render': function (CustNames) {

                             return '<input type=text name="custName" class="form-control" value="' + CustNames + '">';
                         }
                     },
                     {
                         'sortable': false,
                         'render': function (CustNames) {
                             return '<button type="button" class="btn btn-primary btn-sm">Update</button>';
                         }
                     }

                 ],
                 bServerSide: true,
                 sAjaxSource: '../WebServices/adminShoppingWebService.asmx/GetCustomerData',
                 sServerMethod: 'post',
                  "processing": true,
                 'language': {
                     'loadingRecords': '&nbsp;',
                     'processing': '<div class="spinner"></div>',
                     "infoFiltered": ""
                 },
                 "columnDefs": [
                     { "width": "5%", "targets": 0 },
                     { "width": "5%", "targets": 1 },
                     { "width": "5%", "targets": 2 },
                     { "width": "5%", "targets": 3 },
                     { "width": "2%", "targets": 4 },
                     { "width": "3%", "targets": 5 },
                     { "width": "1%", "targets": 6 },
                     { "width": "15%", "targets": 7 },
                     { "width": "1%", "targets": 8 }

                 ]
             });
            
            
             $('#datatable tbody').on('click', 'button', function () {
                 //below line returns object of the data in the row
                 let data = table.row($(this).parents('tr')).data(); 
                 let id = data.CustomrtID;
                // console.log(id);
                
                 let customerObj = {};
                
                 customerObj.customerName = $(this).closest('tr').find("input:text").val(); 
                 customerObj.customerId = id;
               

                 $.ajax({
                     type: "POST",
                     url: "customer-details.aspx/UpdateCustomerData",
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     data: JSON.stringify(customerObj),
                     success: function (success) {
                         toastr.success('Data Updated successfully');
                     },
                     error: function (xhr, errorType, exception) {
                         toastr.error('Oops somthig went wrong');;
                     }

                 });

                 table.ajax.reload();

             });

         });
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">Customer Details Master</h2>
    <span class="space15"></span>

    <div class="table-responsive-sm table-responsive-md table-responsive-lg">
        <table id="datatable" class="w-100 table table-striped table-bordered table-hover">
            <thead class="thead-dark">
                <tr>
                    <th>Id</th>
                    <th>Date</th>
                    <th>Name</th>
                    <th>Mobile</th>
                    <th>Email</th>
                    <th>DeviceType</th>
                    <th>Password</th>
                    <th></th>
                    <th></th>
                </tr>
            </thead>

        </table>
    </div>




    <%--Members GridView Start--%>
  <%--  <div id="viewDetails" runat="server">
        <span class="space15"></span>
        <div>
            <asp:GridView ID="gvDetails" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
                AutoGenerateColumns="false" Width="100%" OnRowDataBound="gvDetails_RowDataBound" OnRowCommand="gvDetails_RowCommand">
                <RowStyle CssClass="" />
                <HeaderStyle CssClass="bg-dark" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="CustomrtID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="JoinDate" HeaderText="Date">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CustomerName" HeaderText="Name">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>

                    <asp:BoundField DataField="CustomerMobile" HeaderText="Mobile">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CustomerEmail" HeaderText="Email">
                        <ItemStyle Width="8%" />
                    </asp:BoundField>
                  
                    <asp:BoundField DataField="CustomerPassword" HeaderText="Password">
                        <ItemStyle Width="8%" />
                    </asp:BoundField>
                 
                    <asp:BoundField DataField="DeviceType" HeaderText="Registered From">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Name">
                        <ItemStyle Width="10%" />
                        <ItemTemplate>
                            <asp:TextBox ID="txtCustName" CssClass="form-control" MaxLength="50" Width="100" runat="server" Text=""></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField>
                        <ItemStyle Width="10%" />
                        <ItemTemplate>
                            <asp:Button ID="cmdUpdate" runat="server" CssClass="btn btn-sm btn-primary" CommandName="gvUpdate" Text="Update" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <span class="warning">No Customer to Display :(</span>
                </EmptyDataTemplate>
                <PagerStyle CssClass="gvPager" />
            </asp:GridView>
        </div>
    </div>--%>
    <%--Members GridView End--%>
</asp:Content>

