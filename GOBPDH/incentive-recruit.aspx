<%@ Page Title="Recruitment Incentive Report" Language="C#" MasterPageFile="~/GOBPDH/MasterGOBPDH.master" AutoEventWireup="true" CodeFile="incentive-recruit.aspx.cs" Inherits="GOBPDH_incentive_recruit" %>
<%@ MasterType VirtualPath="~/obpmanager/MasterObpManager.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Recruitment Incentive Report</h2>
    <span class="space15"></span>
    <div>
        <asp:GridView ID="gvGOBP" runat="server" AutoGenerateColumns="False"
                CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
                >
                <HeaderStyle CssClass="bg-dark" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="OBP_ID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="joinDate" HeaderText="Join Date">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                     <asp:BoundField DataField="OBP_JoinLevel" HeaderText="Join Level">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                     <asp:BoundField DataField="OBP_UserID" HeaderText="User ID">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OBP_ApplicantName" HeaderText="Applicant Name">
                        <ItemStyle Width="20%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OBP_MobileNo" HeaderText="Mobile No">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="custCount" HeaderText="Total GOBPs">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="dh" HeaderText="DH">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="dhContact" HeaderText="DH Contact">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OBP_StatusFlag" HeaderText="Status">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="flCount" HeaderText="FL Count">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                    <asp:TemplateField>
                        <ItemStyle Width="5%" />
                        <ItemTemplate>
                            <asp:Literal ID="litView" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                </Columns>
                <EmptyDataTemplate>
                    <span class="warning">No GOBP Enquries to Display</span>
                </EmptyDataTemplate>
                <PagerStyle CssClass="gvPager" />
            </asp:GridView>
    </div>
</asp:Content>

