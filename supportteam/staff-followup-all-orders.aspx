<%@ Page Title="All Orders Follow-Up | Genericart Shopping Support Team" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="staff-followup-all-orders.aspx.cs" Inherits="supportteam_staff_followup_all_orders" %>

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
              let orderStatus;
              let fromDate = '';
              let toDate = '';
              if (localStorage.getItem("orderStatus") === null) {
                  orderStatus = 1;
              }
              else {
                  orderStatus = localStorage.getItem("orderStatus");
                  let ddrOrdersType = document.getElementById("orderType");
                  ddrOrdersType.value = orderStatus;
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
                              return '<a href=staff-followup-form.aspx?type=allord&id=' + row.FK_OrderCustomerID + ' class="btn btn-sm btn-primary" >Follow Up<a/>';
                          }
                      }
                  ],
                  bServerSide: true,

                  sAjaxSource: '../WebServices/supportTeamWebServices.asmx/GetOrders',
                  sServerMethod: 'post',

                  "fnServerParams": function (aoData) {

                      aoData.push({ "name": "orderStatus", "value": "" + orderStatus + "" },
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
                  "responsive": true


              });
              //function for geting column value
              table.rows(function (idx, data, node) {
                  return data.tagged == 0;
              }).select();

              $('#btnSearch1').click(function (e) {
                  e.preventDefault();
                  orderStatus = document.getElementById("orderType").value;
                  localStorage.setItem("orderStatus", "" + orderStatus + "");
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
    <h2 class="pgTitle">All Orders List</h2>
    <span class="space15"></span>


    <label>Select Order Type: </label>
    <div class="row">
        <div class="form-group col-sm-4">
            <select name="orderType" id="orderType" class="form-control" autofocus>
                <%--<option value="none" selected disabled hidden>Select an Option</option>--%>
                <option value=1 selected>New</option>
                <option value=3>Accepted</option>
                <option value=5>In-Process</option>
                <option value=6>Shipped</option>
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








    <%--<label>Select Order Type :</label>
    <div class="row">
        <div class="form-group col-sm-4">
            <asp:DropDownList ID="ddrOrdStatus" runat="server" CssClass="form-control" Width="100%">
                <asp:ListItem Value="0"><--Select--></asp:ListItem>
                <asp:ListItem Value="1">New</asp:ListItem>
                <asp:ListItem Value="3">Accepted</asp:ListItem>
                <asp:ListItem Value="5">In-Process</asp:ListItem>
                <asp:ListItem Value="6">Shipped</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="col-sm-1">

            <asp:Button ID="btnSearch" runat="server"
                CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click"/>
        </div>

    </div>--%>


  <%--  <span class="space15"></span>
    <div id="viewAllOrdFollowUp" runat="server">
       
        <asp:GridView ID="gvAllOrd" runat="server" AutoGenerateColumns="False"
            CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None" Width="100%" OnRowDataBound="gvAllOrd_RowDataBound">

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

