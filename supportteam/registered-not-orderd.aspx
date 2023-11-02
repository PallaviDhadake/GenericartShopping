<%@ Page Title="Registered but not orderd customers list" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="registered-not-orderd.aspx.cs" Inherits="supportteam_registered_not_orderd" %>
<%@ MasterType VirtualPath="~/supportteam/MasterSupport.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        window.onload = function () {
            duDatepicker('#<%= txtNStartDate.ClientID %>', {
                auto: true, inline: true, format: 'dd/mm/yyyy',
            });
            duDatepicker('#<%= txtNEndDate.ClientID %>', {
                auto: true, inline: true, format: 'dd/mm/yyyy',
            });
        }
    </script>

    <script>
        $(document).ready(function () {
            $('[id$=gvNotOrdCust]').DataTable({
                columnDefs: [
					{ orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7] }
                ],
                order: [[0, 'desc']]
            });

            // Call refresh page function after 30000 milliseconds (or 30 seconds).
            //setInterval('submitForm();', 30000);
        });

        function submitForm() {
            document.getElementById("<%=btnShow.ClientID %>").click();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="card card-info">
        <div class="card-header">
            <h3 class="card-title">Registered but Not Ordered Customers List</h3>
        </div>
        <div class="card-body">
            <div class="form-row">
                <div class="form-group col-md-6">
                    <label>Select From Date :*</label>
                    <asp:TextBox ID="txtNStartDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Select To Date :*</label>
                    <asp:TextBox ID="txtNEndDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
                </div>
            </div>
            <asp:Button ID="btnShow" runat="server" CssClass="btn btn-md btn-info" Text="Submit" OnClick="Submit"  />
        </div>
    </div>

    <div id="notOrdCust" runat="server">
        <asp:GridView ID="gvNotOrdCust" runat="server" AutoGenerateColumns="False"
            CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None" OnRowDataBound="gvNotOrdCust_RowDataBound">
            <HeaderStyle CssClass="bg-dark" />
            <AlternatingRowStyle CssClass="alt" />
            <Columns>
                <asp:BoundField DataField="CustomrtID" HeaderText="Customer ID">
                    <HeaderStyle CssClass="HideCol" />
                    <ItemStyle CssClass="HideCol" />
                    <FooterStyle CssClass="HideCol" />
                </asp:BoundField>
                <asp:BoundField DataField="RBNO_NextFollowup" HeaderText="Followup Date">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="CustomerName" HeaderText="Customer Name">
                    <ItemStyle Width="20%" />
                </asp:BoundField>
                <asp:BoundField DataField="CustomerMobile" HeaderText="Mobile No.">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="CustomerEmail" HeaderText="Email Addres">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="DeviceType" HeaderText="Registered From">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="flcount" HeaderText="Total Followup">
                    <ItemStyle Width="5%" />
                </asp:BoundField>
                 <asp:TemplateField HeaderText="Fav. Shop">
                        <ItemStyle Width="10%" />
                        <ItemTemplate>
                            <asp:Literal ID="litFavShop" runat="server"></asp:Literal>
                        </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Followup">
                        <ItemStyle Width="10%" />
                        <ItemTemplate>
                            <asp:Literal ID="litFlUp" runat="server"></asp:Literal>
                        </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <span class="warning">No Data to Display</span>
            </EmptyDataTemplate>
            <FooterStyle BackColor="Black" ForeColor="White" />
            <PagerStyle CssClass="gvPager" />
        </asp:GridView>
    </div>
    <span class="space20"></span>
    <%= errMsg %>
</asp:Content>

