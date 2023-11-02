<%@ Page Title="GOBP Master" Language="C#" MasterPageFile="~/orgmember/MasterOrgmember.master" AutoEventWireup="true" CodeFile="gobp-master.aspx.cs" Inherits="orgmember_gobp_master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <script>
        $(document).ready(function () {

            $('[id$=gvGobpData]').DataTable({
                columnDefs: [
                     { orderable: false, targets: [0, 1, 2, 3, 4] }
                ],
                order: [[0, 'desc']]
            });
        });
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <h2 class="pgTitle">GOBP Data</h2>
        <span class="space15"></span>
    <h3 class="semiBold clrAccepted"><%=DistrictHeadName %></h3>
    <span class="space15"></span>

    <asp:GridView ID="gvGobpData" runat="server" AutoGenerateColumns="False"
        CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None">
        <HeaderStyle CssClass="bg-dark" />
        <AlternatingRowStyle CssClass="alt" />
        <Columns>
            <asp:BoundField DataField="OBP_ID">
                <HeaderStyle CssClass="HideCol" />
                <ItemStyle CssClass="HideCol" />
            </asp:BoundField>
            <asp:BoundField DataField="OBP_ApplicantName" HeaderText="Name">
                <ItemStyle Width="25%" />
            </asp:BoundField>
            <asp:BoundField DataField="OBP_UserID" HeaderText="User Id">
                <ItemStyle Width="15%" />
            </asp:BoundField>
           <%-- <asp:BoundField DataField="orderAmount" HeaderText="Total Orders Amount">
                <ItemStyle Width="10%" />
            </asp:BoundField>--%>
             <asp:BoundField DataField="customers" HeaderText="Total Customers">
                <ItemStyle Width="10%" />
            </asp:BoundField>
             <asp:BoundField DataField="MonthCustomer" HeaderText="This Month Customers">
                <ItemStyle Width="10%" />
            </asp:BoundField>

        </Columns>
        <EmptyDataTemplate>
            <span class="warning">No Data to Display</span>
        </EmptyDataTemplate>
        <PagerStyle CssClass="gvPager" />
    </asp:GridView>
</asp:Content>

