<%@ Page Title="" Language="C#" MasterPageFile="~/bdm/MasterBdm.master" AutoEventWireup="true" CodeFile="caller-overview-report.aspx.cs" Inherits="bdm_caller_overview_report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">

        window.onload = function () {
            //alert("window load");

            duDatepicker('#<%= txtDate.ClientID %>', {
                auto: true, inline: true, format: 'dd/mm/yyyy',
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Caller Report</h2>

    <div id="viewReport" runat="server">
        <span class="space15"></span>

        <div class="row">
            <div class="form-group col-sm-4">
                <span class="text-sm text-bold mrgBtm10 dspBlk">Date :</span>
                <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
            </div>
            <div class="col-sm-1">
                <span class="space30"></span>
                <asp:Button ID="btnShow" runat="server" CssClass="btn btn-md btn-primary" Text="Show List" OnClick="btnShow_Click" />
            </div>
        </div>

        <asp:GridView ID="gvCall" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
            AutoGenerateColumns="false" Width="100%">
            <RowStyle CssClass="" />
            <HeaderStyle CssClass="bg-dark" />
            <AlternatingRowStyle CssClass="alt" />
            <Columns>
                <asp:TemplateField HeaderText="Sr. no">
                    <ItemTemplate>
                        <%#Container.DataItemIndex+1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Name" HeaderText="Name" />
                <asp:BoundField DataField="FreshCalls" HeaderText="Fresh Calls" />
                <asp:BoundField DataField="FollowupCalls" HeaderText="Flup Calls" />
                <asp:BoundField DataField="EnquiryCalls" HeaderText="Enquiry Calls" />
                <asp:BoundField DataField="TodaysCalls" HeaderText="Todays Calls" />
                <asp:BoundField DataField="MonthlyCalls" HeaderText="Monthly Calls" />
                <asp:BoundField DataField="TodaysOrder" HeaderText="Conv. Order" />
                <asp:BoundField DataField="TotalAmount" HeaderText="Order Amount" />
                <asp:BoundField DataField="ConversionRatio" HeaderText="Conversion Ratio (%)" />
            </Columns>
            <EmptyDataTemplate>
                <span class="warning">No Records to Display :(</span>
            </EmptyDataTemplate>
            <PagerStyle CssClass="gvPager" />
        </asp:GridView>
    </div>
</asp:Content>

