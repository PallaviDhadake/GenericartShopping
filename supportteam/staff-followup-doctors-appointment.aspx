<%@ Page Title="Doctors Appointment Follow-Up | Genericart Shopping Support Team" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="staff-followup-doctors-appointment.aspx.cs" Inherits="supportteam_staff_followup_doctors_appointment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%-- <script>
        $(document).ready(function () {
            $('[id$=gvDocApp]').DataTable({
                columnDefs: [
                    { orderable: false, targets: [0, 3] }
                ],
                order: [[0, 'desc']]
            });
        });
     </script>--%>

    <script type="text/javascript">

        $(document).ready(function () {

            //$.fn.dataTable.ext.errMode = 'none';
            //.on('error.dt', function (e, settings, techNote, message) {
            //    //toastr.warning('Oops Something went wrong')
            //    // console.log('An error has been reported by DataTables: ', message);
            //    console.log(techNote);
            //})
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
                        'data': 'DocAppID',
                        "visible": false,
                    },


                    { 'data': 'AppSubmitDate' },
                    { 'data': 'DocAppName' },
                    { 'data': 'DocAppDate' },
                    {
                        'data': 'DocAppMobile',
                        'sortable': false
                    },
                    {
                        'data': 'PrevDocName',
                        'sortable': false
                    },
                    {
                        'sortable': false,
                        'render': function (data, type, row, meta) {
                            return '<a href=staff-followup-form.aspx?type=docapp&id=' + row.DocAppID + ' class="btn btn-sm btn-primary" >Follow Up<a/>';
                        }
                    }
                ],
                bServerSide: true,
                sAjaxSource: '../WebServices/supportTeamWebServices.asmx/GetDoctorsAppointmentData',
                 sServerMethod: 'post',
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">Doctors Appointment List</h2>
    <span class="space15"></span>

    <div class="width100">
        <table id="datatable" class="table table-striped table-bordered table-hover w-100">
            <thead class="thead-dark">
                <tr>
                    <th>Id</th>
                    <th>App. Submit Date</th>
                    <th>Name</th>
                    <th>App. Date</th>
                    <th>Mobile</th>
                    <th>Prev Doctor</th>
                    <th></th>
                </tr>
            </thead>

        </table>
    </div>


    <div id="viewDocAppointment" runat="server">
        <%-- <a href="staff-followup-new.aspx" class="btn btn-primary">New Follow-Up</a>--%>

        <%--gridview start--%>
        <asp:GridView ID="gvDocApp" runat="server" AutoGenerateColumns="False"
            CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None" Width="100%" OnRowDataBound="gvDocApp_RowDataBound">

            <HeaderStyle CssClass="bg-dark" />
            <AlternatingRowStyle CssClass="alt" />
            <Columns>
                <asp:BoundField DataField="DocAppID">
                    <HeaderStyle CssClass="HideCol" />
                    <ItemStyle CssClass="HideCol" />
                </asp:BoundField>

                <asp:BoundField DataField="DocAppName" HeaderText="Name">
                    <ItemStyle Width="5%" />
                </asp:BoundField>
                <asp:BoundField DataField="DocAppMobile" HeaderText="Mobile No">
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
        <%-- gridview End--%>
    </div>
</asp:Content>

