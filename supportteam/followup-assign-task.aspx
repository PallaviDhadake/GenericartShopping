<%@ Page Title="" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="followup-assign-task.aspx.cs" Inherits="supportteam_followup_assign_task" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="TeamData" runat="server" visible="true">
        <div class="card-header">
            <h3 class="large colorLightBlue">Online Calling Team</h3>
        </div>
        <div class="card-body">
            <span class="space15"></span>
            <asp:GridView ID="gvTeam" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None" PageSize="30" AllowPaging="true"
                AutoGenerateColumns="false">
                <RowStyle CssClass="" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="TeamUserID" HeaderText="Team ID">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TeamPersonName" HeaderText="Name">
                        <ItemStyle Width="20%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TeamMobile" HeaderText="Mobile No.">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                </Columns>
                <EmptyDataTemplate>
                    <span class="warning">:(</span>
                </EmptyDataTemplate>
                <PagerStyle CssClass="gvPager" />
            </asp:GridView>
        </div>
    </div>

    <span class="space20"></span>
    <div class="card card-primary">
        <div class="card-header">
            <h3 class="card-title">Todays Calling Overview</h3>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col col-4">
                    <div class="info-box bg-light">
                        <div class="info-box-content">
                            <span class="info-box-text text-center text-muted text-blue">Todays Customers To Distribute</span>
                            <span class="info-box-number text-center mb-0"><%= arrOrd[1] %></span>
                        </div>
                    </div>
                </div>                
                <div class="col col-4">
                    <div class="info-box-content" style="padding-top: 20px;">
                        <asp:Button ID="btnDistribute" runat="server" Text="Distribute Calls" CssClass="btn btn-md btn-success" OnClick="btnDistribute_Click" Visible="true"/>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="TeamCall" runat="server" visible="true">
        <h2 class="pgTitle">Todays Calls Distribution</h2>
        <span class="space15"></span>
        <asp:GridView ID="gvCall" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover table-responsive-sm"
            GridLines="None" Width="100%">
            <Columns>
                <asp:BoundField DataField="TeamPersonName" HeaderText="Team Name" />
                <asp:BoundField DataField="AssignedOrders" HeaderText="Orders Assigned To Team" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

