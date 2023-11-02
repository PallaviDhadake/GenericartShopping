<%@ Page Title="Registered Customer Follow-Up | Genericart Shopping Support Team" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="staff-followup-new.aspx.cs" Inherits="supportteam_staff_followup_new" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%-- <script>
        $(document).ready(function () {
            $('[id$=gvNewFollowUp]').DataTable({
                columnDefs: [
                    { orderable: false, targets: [0, 3, 4] }
                ],
                order: [[0, 'desc']]
               
            });
        });
    </script>--%>
    <script src="//cdn.datatables.net/plug-ins/1.10.11/sorting/date-eu.js" type="text/javascript"></script>
      <script type="text/javascript">

          $(document).ready(function () {

              let teamId = '<%= Session["adminSupport"].ToString() %>';
              let feedbackStatus = 0;

              var table = $('#datatable').DataTable({
                  "columnDefs": [
                        { "targets": 2, "type": "date-eu" },
                  ],
                  "order": [[2, "desc"]],
                columns: [
                    {
                        'data': 'CustomrtID',
                        "visible": false,
                    },
                    {
                        'data': 'FeedBackFlag',
                        "visible": false,
                    },
                    { 'data': 'CustomerJoinDate' },

                    {
                        'data': 'CustomerName',
                        'sortable': false,
                    },
                    {
                        'data': 'CustomerEmail',
                        'sortable': false,
                    },
                    {
                        'data': 'CustomerMobile',
                        'sortable': false,
                    },
                    {
                        'data': 'CustAddress',
                        'sortable': false,
                        'render': function (custAddress) {
                            if (custAddress == "") {
                                return 'NA'
                            }
                            else {
                                return custAddress;
                            }
                        }
                    },
                    {
                        'data': 'CustCity',
                        'sortable': false,
                        'render': function (CustCity) {
                            if (CustCity == "") {
                                return 'NA'
                            }
                            else {
                                return CustCity;
                            }
                        }
                    },

                    {
                        'sortable': false,
                        'render': function (data, type, row, meta) {
                            if (row.FeedBackFlag == 1) {
                                return '<span class=\"btn btn-sm btn-success\"><i class=\"fa fa-check\"></i> Follow Up Recored</span>'
                            }
                            else {
                                return '<a href=staff-followup-form.aspx?type=regcust&id=' + row.CustomrtID + ' class="btn btn-sm btn-primary" >Follow Up<a/>';
                            }

                        }
                    }
                ],
                bServerSide: true,

                sAjaxSource: '../WebServices/supportTeamWebServices.asmx/GetRegisteredCustomer',
                sServerMethod: 'post',

                "fnServerParams": function (aoData) {
                    aoData.push({ "name": "teamId", "value": "" + teamId + "" },
                        { "name": "feedbackStatus", "value": "" + feedbackStatus + ""});
                },


                "processing": true,
                'language': {
                    'loadingRecords': '&nbsp;',
                    'processing': '<div class="spinner"></div>',
                    "infoFiltered": ""
                },
                "responsive": true,
                "columnDefs": [
                    { "width": "1%", "targets": 0 },
                    { "width": "1%", "targets": 1 },
                    { "width": "2%", "targets": 2 },
                    { "width": "5%", "targets": 3 },
                    { "width": "1%", "targets": 4 },
                    { "width": "3%", "targets": 5 },
                    { "width": "1%", "targets": 6 },
                    { "width": "15%", "targets": 7 },
                    { "width": "12%", "targets": 8 }
                   

                ]
            });
            //function for geting column value
            table.rows(function (idx, data, node) {
                return data.tagged == 0;
            }).select();

              $('#btnSearch1').click(function (e) {
                  e.preventDefault();
                  feedbackStatus = document.getElementById("feedbackStatus").value;
                  table.ajax.reload();

              });

        });
      </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">Registered Customer New Follow Up</h2>
    <span class="space15"></span>


    <label>Select FeedBack Type: </label>
    <div class="row">
        <div class="form-group col-sm-4">
            <select name="feedbackStatus" id="feedbackStatus" class="form-control" autofocus>
                <option value="0" selected>In-Complete</option>
                <option value="1">Completed</option>
            </select>
        </div>
        <div class="col-sm-1">
            <button id="btnSearch1" class="btn btn-primary">Search</button>
        </div>

    </div>



    <div class="width100">
        <table id="datatable" class="table table-striped table-bordered table-hover w-100">
            <thead class="thead-dark">
                <tr>

                    <th>Customer Id</th>
                    <th>FeedBack Flag</th>
                    <th>Date</th>
                    <th>Name</th>
                    <th>Email</th>
                    <th>Mobile No</th>
                    <th>Address</th>
                    <th>City</th>
                    <th></th>
                </tr>
            </thead>
        </table>
    </div>





    <div id="viewNewFollowup" runat="server">
        <%-- <a href="staff-followup-new.aspx" class="btn btn-primary">New Follow-Up</a>--%>

        <%--gridview start--%>
        <asp:GridView ID="gvNewFollowUp" runat="server" AutoGenerateColumns="False"
            CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None" Width="100%" OnRowDataBound="gvNewFollowUp_RowDataBound">

            <HeaderStyle CssClass="bg-dark" />
            <AlternatingRowStyle CssClass="alt" />
            <Columns>
                <asp:BoundField DataField="CustomrtID">
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
        <%-- gridview End--%>
    </div>
</asp:Content>

