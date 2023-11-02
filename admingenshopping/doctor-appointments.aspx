<%@ Page Title="Doctors Appointments" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="doctor-appointments.aspx.cs" Inherits="admingenshopping_doctor_appointments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
        $(document).ready(function () {
            $('[id$=gvAppointment]').DataTable({
                columnDefs: [
                     { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11] }
                ],
                order: [[0, 'desc']]
            });
        });
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Doctor Appointments</h2>
    <span class="space15"></span>

    <div id="viewApp" runat="server">
        <span class="space15"></span>
        <div>
            <asp:GridView ID="gvAppointment" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
                AutoGenerateColumns="false" Width="100%" OnRowDataBound="gvAppointment_RowDataBound">
                <RowStyle CssClass="" />
                <HeaderStyle CssClass="bg-dark" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="DocAppID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DocAppStatus">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FK_DocID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="subDate" HeaderText="Date">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="appDate" HeaderText="Appointment Date">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DocAppName" HeaderText="Name">
                        <ItemStyle Width="20%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DocAppMobile" HeaderText="Mobile No.">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DocAppAge" HeaderText="Age">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Doctor">
                        <ItemStyle Width="15%" />
                        <ItemTemplate>
                            <asp:Literal ID="litDocName" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemStyle Width="10%" />
                        <ItemTemplate>
                            <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="DeviceType" HeaderText="App. From">
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
                    <span class="warning">No Data to Display :(</span>
                </EmptyDataTemplate>
                <PagerStyle CssClass="gvPager" />
            </asp:GridView>
        </div>
    </div>
    <div id="readApp" runat="server">
        <div class="card">
            <div class="card-header">
                <h3 class="large colorLightBlue">Appointment Details</h3>
                <span class="badge badge-pill badge-success text-lg"><%= deviceType %></span>
            </div>
            <div class="card-body">

                <table class="form_table">
                    <tr>
                        <td><span class="colorLightBlue">Appointment Id :</span></td>
                        <td>
                            <asp:Label ID="Label1" CssClass="colorLightBlue" runat="server" Text=""><%= ordData[0] %></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 25%"><span class="formLable bold_weight">Date:</span></td>
                        <td><span class="formLable"><%= ordData[1] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Name :</span></td>
                        <td><span class="formLable"><%= ordData[2] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Age :</span></td>
                        <td><span class="formLable"><%= ordData[3] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Gender :</span></td>
                        <td><span class="formLable"><%= ordData[4] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Email :</span></td>
                        <td><span class="formLable"><%= ordData[5] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Mobile :</span></td>
                        <td><span class="formLable"><%= ordData[6] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Address :</span></td>
                        <td><span class="formLable" style="display: block; width: 60% !important;"><%= ordData[7] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Pin Code :</span></td>
                        <td><span class="formLable"><%= ordData[8] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Appointment Details :</span></td>
                        <td><span class="formLable" style="display: block; width: 60% !important;"><%= ordData[9] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Previous Doctor Name :</span></td>
                        <td><span class="formLable"><%= ordData[10] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Appointment For :</span></td>
                        <td><span class="formLable"><%= ordData[11] %></span> </td>
                    </tr>
                </table>
            </div>
            
        </div>
        <span class="space15"></span>
        <a href="doctor-appointments.aspx" class="btn btn-sm btn-outline-dark">Back</a>
    </div>
</asp:Content>

