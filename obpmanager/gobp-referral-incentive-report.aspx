<%@ Page Title="" Language="C#" MasterPageFile="~/obpmanager/MasterObpManager.master" AutoEventWireup="true" CodeFile="gobp-referral-incentive-report.aspx.cs" Inherits="obpmanager_gobp_referral_incentive_report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        $(document).ready(function () {
            $('[id$=gvGOBPIncentive]').DataTable({
                columnDefs: [
                    { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7, 8] }
                ],
                order: [[0, 'DE']]
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">GOBP Referral Incentive Report</h2>
    <span class="space15"></span>

    <h2 style="color: blueviolet; font-size: 20px;">Enter GOBP UserId to check the Referral Incentive Report</h2>
    <div class="row" id="daterange" runat="server">
        <div class="col-md-3">
            <span class="text-sm text-bold mrgBtm10 dspBlk">GOBP UserName :</span>
            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="GOBP UserName" />
        </div>        
        <div class="col-md-3" style="margin-top: 6px;">
            <br />
            <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="btn btn-md btn-primary" OnClick="btnShow_Click" />
        </div>       
    </div>

    <span class="space30"></span>

    <div class="viewOrder" runat="server">
        <div>
            <asp:GridView ID="gvGOBPIncentive" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
                AutoGenerateColumns="false" Width="100%" OnRowDataBound="gvGOBPIncentive_RowDataBound" ShowFooter="true">
                <RowStyle CssClass="" />
                <HeaderStyle CssClass="bg-dark" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="OBPID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OBP_Username" HeaderText="OBP User ID">
                        <ItemStyle Width="30%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Name" HeaderText="OBP Name">
                        <ItemStyle Width="20%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OrderID" HeaderText="Order ID">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Date" HeaderText="Date">
                        <ItemStyle Width="10" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CommType" HeaderText="Commission Type">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CommLevel" HeaderText="OBP Commission Level">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ComPercent" HeaderText="Commission Percent">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ComAmount" HeaderText="Incentive Amount">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>                                 
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

