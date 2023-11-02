<%@ Page Title="Staff Folloup Count | Genericart Shopping Support Team" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="staff-followup-count.aspx.cs" Inherits="supportteam_staff_followup_count" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <script>
        $(document).ready(function () {
            $('[id$=gvStaffFollowCount]').DataTable({
                columnDefs: [
                    { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6] }
                ],
                order: [[4, 'desc']]
            });
        });
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Staff Followup Count Master</h2>
    <span class="space15"></span>

    <div id="viewCount" runat="server">
        <span class="space15"></span>
        <div>
            <asp:GridView ID="gvStaffFollowCount" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
                AutoGenerateColumns="false" Width="100%" OnRowDataBound="gvStaffFollowCount_RowDataBound">
                <RowStyle CssClass="" />
                <HeaderStyle CssClass="bg-dark" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="TeamID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TeamTaskID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>

                    <asp:BoundField DataField="TeamPersonName" HeaderText="Name">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>

                    <asp:BoundField DataField="TeamUserID" HeaderText="User ID">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OverAllCount" HeaderText="OverAllCount">
                        <ItemStyle Width="8%" />
                    </asp:BoundField>
                  
                    <asp:BoundField DataField="TodaysCount" HeaderText="TodaysCount">
                        <ItemStyle Width="8%" />
                    </asp:BoundField>
                 
                 <asp:TemplateField HeaderText="Task">
                    <ItemStyle Width="10%" />
                    <ItemTemplate>
                        <asp:Literal ID="litTask" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                    
                </Columns>
                <EmptyDataTemplate>
                    <span class="warning">No Staff to Display :(</span>
                </EmptyDataTemplate>
                <PagerStyle CssClass="gvPager" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>

