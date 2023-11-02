<%@ Page Title="Lab Appointment Follow-Up | Genericart Shopping Support Team" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="staff-followup-lab-appointment.aspx.cs" Inherits="supportteam_staff_followup_lab_appointment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <%--<script>
        $(document).ready(function () {
            $('[id$=gvLabApp]').DataTable({
                columnDefs: [
                    { orderable: false, targets: [0, 3, 4] }
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
                  "columnDefs": [
                        { "targets": 1, "type": "date-eu" },
                  ],
                  "order": [[1, "desc"]],
                  columns: [
                      {
                          'data': 'LabAppID',
                          "visible": false,
                      },
                      { 'data': 'LabAppDate' },
                      { 'data': 'LabAppName' },
                      { 'data': 'LabAppMobile' },
                      { 'data': 'LabTestName' },
                      {
                          'sortable': false,
                          'render': function (data, type, row, meta) {
                              return '<a href=staff-followup-form.aspx?type=labapp&id=' + row.LabAppID +' class="btn btn-sm btn-primary" >Follow Up<a/>';
                          }
                      }
                  ],
                  bServerSide: true,
                  sAjaxSource: '../WebServices/supportTeamWebServices.asmx/GetLabAppointment',
                  sServerMethod: 'post',
                  "processing": true,
                  'language': {
                      'loadingRecords': '&nbsp;',
                      'processing': '<div class="spinner"></div>'
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
     <h2 class="pgTitle">Lab Appointment List</h2>
    <span class="space15"></span>


      <div class="width100">
            <table id="datatable" class="table table-striped table-bordered table-hover w-100">
                <thead class="thead-dark">
                    <tr>
                        <th>Id</th>
                        <th>Date</th>
                        <th>Name</th>
                        <th>Mobile</th>
                        <th>Test Name</th>
                        <th></th>
                    </tr>
                </thead>
                
            </table>
        </div>









   <%-- <div id="viewLabAppointment" runat="server">
      
        <asp:GridView ID="gvLabApp" runat="server" AutoGenerateColumns="False"
            CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None" Width="100%" OnRowDataBound="gvLabApp_RowDataBound">

            <HeaderStyle CssClass="bg-dark" />
            <AlternatingRowStyle CssClass="alt" />
            <Columns>
                <asp:BoundField DataField="LabAppID">
                    <HeaderStyle CssClass="HideCol" />
                    <ItemStyle CssClass="HideCol" />
                </asp:BoundField>


                <asp:BoundField DataField="LabAppName" HeaderText="Name">
                    <ItemStyle Width="5%" />
                </asp:BoundField>
                <asp:BoundField DataField="LabTestName" HeaderText="Test Name">
                    <ItemStyle Width="5%" />
                </asp:BoundField>
                <asp:BoundField DataField="LabAppMobile" HeaderText="Mobile No">
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
                <span class="warning">No data to Display :(</span>
            </EmptyDataTemplate>
            <PagerStyle CssClass="gvPager" />
        </asp:GridView>
       
    </div>--%>
</asp:Content>

