<%@ Page Title="Export Orders, Enquiries to Excel File" Language="C#" MasterPageFile="~/bdm/MasterBdm.master" AutoEventWireup="true" CodeFile="export-excel-files.aspx.cs" Inherits="bdm_export_excel_files" %>
<%@ MasterType VirtualPath="~/bdm/MasterBdm.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">

        window.onload = function () {
            //alert("window load");

            duDatepicker('#<%= txtFromDate.ClientID %>', {
                auto: true, inline: true, format: 'dd/mm/yyyy',
            });
            duDatepicker('#<%= txtToDate.ClientID %>', {
                auto: true, inline: true, format: 'dd/mm/yyyy',
            });


            duDatepicker('#<%= txtEnqFromDate.ClientID %>', {
                auto: true, inline: true, format: 'dd/mm/yyyy',
            });
            duDatepicker('#<%= txtEnqToDate.ClientID %>', {
                auto: true, inline: true, format: 'dd/mm/yyyy',
            });

            duDatepicker('#<%= txtCustFrDate.ClientID %>', {
                auto: true, inline: true, format: 'dd/mm/yyyy',
            });
            duDatepicker('#<%= txtCustToDate.ClientID %>', {
                auto: true, inline: true, format: 'dd/mm/yyyy',
            });

            duDatepicker('#<%= txtCallFrDate.ClientID %>', {
                auto: true, inline: true, format: 'dd/mm/yyyy',
            });
            duDatepicker('#<%= txtCallToDate.ClientID %>', {
                auto: true, inline: true, format: 'dd/mm/yyyy',
            });

            duDatepicker('#<%= txtNStartDate.ClientID %>', {
                auto: true, inline: true, format: 'dd/mm/yyyy',
            });
            duDatepicker('#<%= txtNEndDate.ClientID %>', {
                auto: true, inline: true, format: 'dd/mm/yyyy',
            });

            duDatepicker('#<%= txtObpStartDate.ClientID %>', {
                auto: true, inline: true, format: 'dd/mm/yyyy',
            });
            duDatepicker('#<%= txtObpEndDate.ClientID %>', {
                auto: true, inline: true, format: 'dd/mm/yyyy',
            });

            duDatepicker('#<%= txtGMStartDate.ClientID %>', {
                auto: true, inline: true, format: 'dd/mm/yyyy',
            });
            duDatepicker('#<%= txtGMEndDate.ClientID %>', {
                auto: true, inline: true, format: 'dd/mm/yyyy',
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Export Reports to Excel File</h2>
    <span class="space15"></span>

    <div class="card card-primary">
        <div class="card-header">
            <h3 class="card-title">Export Orders to Excel</h3>
        </div>
        <div class="card-body">
            <div class="form-row">
                <div class="form-group col-md-6">
                    <label>Select From Date :*</label>
                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Select To Date :*</label>
                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
                </div>
            </div>
            <asp:Button ID="btnExportOrders" runat="server" CssClass="btn btn-md btn-info" Text="Export Orders" OnClick="btnExportOrders_Click" />
        </div>
    </div>
    <%= errMsg %>
    <div id="OrderGrid" runat="server" visible="false">
        <asp:GridView ID="gvOrder" runat="server" AutoGenerateColumns="False"
            CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None">
            <HeaderStyle CssClass="bg-dark" />
            <AlternatingRowStyle CssClass="alt" />
            <Columns>
                <asp:BoundField DataField="OrderID" HeaderText="Order ID">
                    <HeaderStyle CssClass="HideCol" />
                    <ItemStyle CssClass="HideCol" />
                    <FooterStyle CssClass="HideCol" />
                </asp:BoundField>
                <asp:BoundField DataField="OrderDate" HeaderText="Order Date">
                    <ItemStyle Width="20%" />
                </asp:BoundField>
                <asp:BoundField DataField="OrderAmount" HeaderText="Order Amount">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="CustomerName" HeaderText="Customer Name">
                    <ItemStyle Width="5%" />
                </asp:BoundField>
                <asp:BoundField DataField="CustomerMobile" HeaderText="Mobile No.">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="FranchName" HeaderText="Shop Name">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="FranchShopCode" HeaderText="Shop Code">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="adminstatus" HeaderText="Admin Status">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="Shopstatus" HeaderText="Shop Status">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="MonthlyOrder" HeaderText="Monthly Order">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="DeviceType" HeaderText="Order From">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
            </Columns>
            <EmptyDataTemplate>
                <span class="warning">No Data to Display</span>
            </EmptyDataTemplate>
            <FooterStyle BackColor="Black" ForeColor="White" />
            <PagerStyle CssClass="gvPager" />
        </asp:GridView>
    </div>

    <span class="space20"></span>
    <!-- =============================================================================================================================== -->

    <div class="card card-purple">
        <div class="card-header">
            <h3 class="card-title">Export Enquiries to Excel</h3>
        </div>
        <div class="card-body">
            <div class="form-row">
                <div class="form-group col-md-6">
                    <label>Select From Date :*</label>
                    <asp:TextBox ID="txtEnqFromDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Select To Date :*</label>
                    <asp:TextBox ID="txtEnqToDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
                </div>
            </div>
            <asp:Button ID="btnExportEnq" runat="server" CssClass="btn btn-md btn-info" Text="Export Enquiries" OnClick="btnExportEnq_Click" />
        </div>
    </div>

    <div id="enqGrid" runat="server" visible="false">
        <asp:GridView ID="gvEnq" runat="server" AutoGenerateColumns="False"
            CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None">
            <HeaderStyle CssClass="bg-dark" />
            <AlternatingRowStyle CssClass="alt" />
            <Columns>
                <asp:BoundField DataField="CalcID" HeaderText="Enquiry ID">
                    <HeaderStyle CssClass="HideCol" />
                    <ItemStyle CssClass="HideCol" />
                    <FooterStyle CssClass="HideCol" />
                </asp:BoundField>
                <asp:BoundField DataField="CalcDate" HeaderText="Enquiry Date">
                    <ItemStyle Width="20%" />
                </asp:BoundField>
                <asp:BoundField DataField="CustomerName" HeaderText="Customer Name">
                    <ItemStyle Width="5%" />
                </asp:BoundField>
                <asp:BoundField DataField="CustomerMobile" HeaderText="Mobile No.">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="DeviceType" HeaderText="Enquiry From">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="EnqStatus" HeaderText="Enquiry Status">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="MonthlyEnq" HeaderText="Monthly Enquiry">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
            </Columns>
            <EmptyDataTemplate>
                <span class="warning">No Data to Display</span>
            </EmptyDataTemplate>
            <FooterStyle BackColor="Black" ForeColor="White" />
            <PagerStyle CssClass="gvPager" />
        </asp:GridView>
    </div>
    <span class="space20"></span>
    <!-- =============================================================================================================================== -->


    <div class="card card-olive">
        <div class="card-header">
            <h3 class="card-title">Export Registered Customers List to Excel</h3>
        </div>
        <div class="card-body">
            <div class="form-row">
                <div class="form-group col-md-6">
                    <label>Select From Date :*</label>
                    <asp:TextBox ID="txtCustFrDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Select To Date :*</label>
                    <asp:TextBox ID="txtCustToDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
                </div>
            </div>
            <asp:Button ID="btnExportCustomers" runat="server" CssClass="btn btn-md btn-info" Text="Export Customers List" OnClick="btnExportCustomers_Click" />
        </div>
    </div>

    <div id="custGridView" runat="server" visible="false">
        <asp:GridView ID="gvCust" runat="server" AutoGenerateColumns="False"
            CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None">
            <HeaderStyle CssClass="bg-dark" />
            <AlternatingRowStyle CssClass="alt" />
            <Columns>
                <asp:BoundField DataField="CustomrtID" HeaderText="Customer ID">
                    <HeaderStyle CssClass="HideCol" />
                    <ItemStyle CssClass="HideCol" />
                    <FooterStyle CssClass="HideCol" />
                </asp:BoundField>
                <asp:BoundField DataField="CustomerJoinDate" HeaderText="Registration Date">
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
            </Columns>
            <EmptyDataTemplate>
                <span class="warning">No Data to Display</span>
            </EmptyDataTemplate>
            <FooterStyle BackColor="Black" ForeColor="White" />
            <PagerStyle CssClass="gvPager" />
        </asp:GridView>
    </div>
    <span class="space20"></span>

    <!-- ======================================================================================================================== -->

    <div class="card card-secondary">
        <div class="card-header">
            <h3 class="card-title">Export Call Me to Excel</h3>
        </div>
        <div class="card-body">
            <div class="form-row">
                <div class="form-group col-md-6">
                    <label>Select From Date :*</label>
                    <asp:TextBox ID="txtCallFrDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Select To Date :*</label>
                    <asp:TextBox ID="txtCallToDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
                </div>
            </div>
            <asp:Button ID="btnExportCallList" runat="server" CssClass="btn btn-md btn-info" Text="Export Call Me List" OnClick="btnExportCallList_Click" />
        </div>
    </div>

    <div id="callMeGrid" runat="server" visible="false">
        <asp:GridView ID="gvCallMe" runat="server" AutoGenerateColumns="False"
            CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None">
            <HeaderStyle CssClass="bg-dark" />
            <AlternatingRowStyle CssClass="alt" />
            <Columns>
                <asp:BoundField DataField="RequestDate" HeaderText="Date">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="RequestName" HeaderText="Name">
                    <ItemStyle Width="20%" />
                </asp:BoundField>
                <asp:BoundField DataField="RequestMobile" HeaderText="Mobile No.">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="RequestMedicine" HeaderText="Medicine">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="DeviceType" HeaderText="Request From">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
            </Columns>
            <EmptyDataTemplate>
                <span class="warning">No Data to Display</span>
            </EmptyDataTemplate>
            <FooterStyle BackColor="Black" ForeColor="White" />
            <PagerStyle CssClass="gvPager" />
        </asp:GridView>
    </div>
    <span class="space20"></span>
    <!-- =========================================================================================================================================================== -->

    <div class="card card-red">
        <div class="card-header">
            <h3 class="card-title">Export Registered but Not Ordered Customers List to Excel</h3>
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
            <asp:Button ID="btnExportNotOrderedList" runat="server" CssClass="btn btn-md btn-info" Text="Export Not Ordered Customers List" OnClick="btnExportNotOrderedList_Click" />
        </div>
    </div>

    <div id="notOrdCust" runat="server" visible="false">
        <asp:GridView ID="gvNotOrdCust" runat="server" AutoGenerateColumns="False"
            CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None">
            <HeaderStyle CssClass="bg-dark" />
            <AlternatingRowStyle CssClass="alt" />
            <Columns>
                <asp:BoundField DataField="CustomrtID" HeaderText="Customer ID">
                    <HeaderStyle CssClass="HideCol" />
                    <ItemStyle CssClass="HideCol" />
                    <FooterStyle CssClass="HideCol" />
                </asp:BoundField>
                <asp:BoundField DataField="CustomerJoinDate" HeaderText="Registration Date">
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
            </Columns>
            <EmptyDataTemplate>
                <span class="warning">No Data to Display</span>
            </EmptyDataTemplate>
            <FooterStyle BackColor="Black" ForeColor="White" />
            <PagerStyle CssClass="gvPager" />
        </asp:GridView>
    </div>
    <span class="space20"></span>

    <!-- =========================================================================================================================================================== -->

    <div class="card card-pink">
        <div class="card-header">
            <h3 class="card-title">Export OBP Orders List to Excel</h3>
        </div>
        <div class="card-body">
            <div class="form-row">
                <div class="form-group col-md-6">
                    <label>Select From Date :*</label>
                    <asp:TextBox ID="txtObpStartDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Select To Date :*</label>
                    <asp:TextBox ID="txtObpEndDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
                </div>
            </div>
            <asp:Button ID="btnObpOrders" runat="server" CssClass="btn btn-md btn-info" Text="Export OBP Orders List" OnClick="btnObpOrders_Click"  />
        </div>
    </div>

    <div id="obpOrd" runat="server" visible="false">
        <asp:GridView ID="gvObpOrd" runat="server" AutoGenerateColumns="False"
            CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None">
            <HeaderStyle CssClass="bg-dark" />
            <AlternatingRowStyle CssClass="alt" />
            <Columns>
                <asp:BoundField DataField="OrderID" HeaderText="Order ID">
                    <HeaderStyle CssClass="HideCol" />
                    <ItemStyle CssClass="HideCol" />
                    <FooterStyle CssClass="HideCol" />
                </asp:BoundField>
                <asp:BoundField DataField="OrderDate" HeaderText="Order Date">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="OrderAmount" HeaderText="Order Amount">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="CustomerName" HeaderText="Custome rName">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="CustomerMobile" HeaderText="Mobile No">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="FranchName" HeaderText="Shop Name">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="FranchShopCode" HeaderText="Shop Code">
                    <ItemStyle Width="5%" />
                </asp:BoundField>
                <asp:BoundField DataField="adminstatus" HeaderText="Admin Status">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="Shopstatus" HeaderText="Shop Status">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="OrderType" HeaderText="Order Type">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="MonthlyOrder" HeaderText="Monthly Order">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="PaymentMode" HeaderText="Payment Mode">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="PayStatus" HeaderText="Payment Status">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="OBP_ApplicantName" HeaderText="OBP Name">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="OBP_MobileNo" HeaderText="OBP Mobile No.">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="OBP_UserID" HeaderText="OBP Code">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="DeviceType" HeaderText="Device Type">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
            </Columns>
            <EmptyDataTemplate>
                <span class="warning">No Data to Display</span>
            </EmptyDataTemplate>
            <FooterStyle BackColor="Black" ForeColor="White" />
            <PagerStyle CssClass="gvPager" />
        </asp:GridView>
    </div>
    <span class="space20"></span>

    <!-- =========================================================================================================================================================== -->

    <div class="card card-purple">
        <div class="card-header">
            <h3 class="card-title">Export Generic Mitra Orders List to Excel</h3>
        </div>
        <div class="card-body">
            <div class="form-row">
                <div class="form-group col-md-6">
                    <label>Select From Date :*</label>
                    <asp:TextBox ID="txtGMStartDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Select To Date :*</label>
                    <asp:TextBox ID="txtGMEndDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
                </div>
            </div>
            <asp:Button ID="btnGMOrders" runat="server" CssClass="btn btn-md btn-info" Text="Export Generic Mitra Orders List" OnClick="btnGMOrders_Click"   />
        </div>
    </div>

    <div id="Div1" runat="server" visible="false">
        <asp:GridView ID="gvGenMitraOrd" runat="server" AutoGenerateColumns="False"
            CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None">
            <HeaderStyle CssClass="bg-dark" />
            <AlternatingRowStyle CssClass="alt" />
            <Columns>
                <asp:BoundField DataField="OrderID" HeaderText="Order ID">
                    <HeaderStyle CssClass="HideCol" />
                    <ItemStyle CssClass="HideCol" />
                    <FooterStyle CssClass="HideCol" />
                </asp:BoundField>
                <asp:BoundField DataField="OrderDate" HeaderText="Order Date">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="OrderAmount" HeaderText="Order Amount">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="CustomerName" HeaderText="Custome rName">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="CustomerMobile" HeaderText="Mobile No">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="FranchName" HeaderText="Shop Name">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="FranchShopCode" HeaderText="Shop Code">
                    <ItemStyle Width="5%" />
                </asp:BoundField>
                <asp:BoundField DataField="adminstatus" HeaderText="Admin Status">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="Shopstatus" HeaderText="Shop Status">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="OrderType" HeaderText="Order Type">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="MonthlyOrder" HeaderText="Monthly Order">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="PaymentMode" HeaderText="Payment Mode">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="PayStatus" HeaderText="Payment Status">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="DeviceType" HeaderText="Device Type">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
            </Columns>
            <EmptyDataTemplate>
                <span class="warning">No Data to Display</span>
            </EmptyDataTemplate>
            <FooterStyle BackColor="Black" ForeColor="White" />
            <PagerStyle CssClass="gvPager" />
        </asp:GridView>
    </div>
    <span class="space20"></span>
</asp:Content>

