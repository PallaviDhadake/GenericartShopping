<%@ Page Title="Registered Customer Follow-Up | Genericart Shopping Support Team" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="staff-followup-report.aspx.cs" Inherits="supportteam_staff_followup_report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
      <script>
        $(document).ready(function () {
            $('[id$=gvFeedback]').DataTable({
                columnDefs: [
                    { orderable: false, targets: [0, 3, 4, 5, 6] }
                ],
                order: [[0, 'desc']]
            });
        });
      </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle"><%= pageName %></h2>
    <span class="space15"></span>

     <div id="viewFeedback" runat="server">
        <a href="<%= pageLink %>" class="btn btn-primary">New Follow-Up</a>
        <span class="space15"></span>
        <%--gridview start--%>
        <asp:GridView ID="gvFeedback" runat="server" AutoGenerateColumns="False"
            CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None" Width="100%" OnRowDataBound="gvFeedback_RowDataBound">
            
            <HeaderStyle CssClass="bg-dark" />
            <AlternatingRowStyle CssClass="alt" />
            <Columns>
                <asp:BoundField DataField="FeedBkID">
                    <HeaderStyle CssClass="HideCol" />
                    <ItemStyle CssClass="HideCol" />
                </asp:BoundField>
                <asp:BoundField DataField="FeedBkRating">
                    <HeaderStyle CssClass="HideCol" />
                    <ItemStyle CssClass="HideCol" />
                </asp:BoundField>

                 <asp:BoundField DataField="FeedBkDate" HeaderText="Date">
                    <ItemStyle Width="5%" />
                </asp:BoundField>
                 <asp:BoundField DataField="CustomerName" HeaderText="Name">
                    <ItemStyle Width="5%" />
                </asp:BoundField>
                
                <asp:TemplateField HeaderText="Task">
                    <ItemStyle Width="5%" />
                    <ItemTemplate>
                        <asp:Literal ID="litTaskId" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Ratings">
                    <ItemStyle Width="5%" />
                    <ItemTemplate>
                        <asp:Literal ID="litRatings" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField>
                    <ItemStyle Width="5%" />
                    <ItemTemplate>
                        <asp:Literal ID="litAnch" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <span class="warning">No data to Display :(</span>
            </EmptyDataTemplate>
            <PagerStyle CssClass="gvPager" />
        </asp:GridView>
        <%-- gridview End--%>
    </div>

</asp:Content>

