<%@ Page Title="Option Group Master" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="option-group-master.aspx.cs" Inherits="admingenshopping_option_group_master" %>
<%@ MasterType VirtualPath="~/admingenshopping/MasterAdmin.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
        $(document).ready(function () {
            $('[id$=gvOptionGroup]').DataTable({
                columnDefs: [
                    { orderable: false, targets: [0, 2] }
                ],
                order: [[0, 'desc']]
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Option Group Master</h2>
    <span class="space15"></span>

    <div id="editOptGroup" runat="server">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title"><%=pgTitle %></h3>
                    </div>
                    <div class="card-body">
                        <div class="col-sm-6">
                            <div class="colorLightBlue">
                                <span>Id :</span>
                                <asp:Label ID="lblId" runat="server" Text="[New]"></asp:Label>
                            </div>
                            <span class="space15"></span>
                            <div class="form-group">
                                <label for="optionName">Option Group Name :*</label>
                                <asp:TextBox ID="txtOptionGroup" runat="server" CssClass="form-control" Width="100%" MaxLength="30"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <span class="space10"></span>
                <%=errMsg %>
                <span class="space10"></span>
                <asp:Button ID="btnSave" runat="server"
                    CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                <asp:Button ID="btnDelete" runat="server"
                    CssClass="btn btn-outline-info" Text="Delete" OnClientClick="return confirm('Are you sure to delete?');"
                    OnClick="btnDelete_Click" />
                <asp:Button ID="btnCancel" runat="server"
                    CssClass="btn btn-outline-dark" Text="Cancel"
                    OnClick="btnCancel_Click" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="viewOptGroup" runat="server">
        <span class="space15"></span>
        <div>
            <asp:GridView ID="gvOptionGroup" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None" Width="100%"
                AutoGenerateColumns="false" OnPageIndexChanging="gvOptionGroup_PageIndexChanging" OnRowDataBound="gvOptionGroup_RowDataBound">
                <RowStyle CssClass="" />
                <HeaderStyle CssClass="bg-dark" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="OptionGroupID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OptionGroupName" HeaderText="Option Group Name">
                        <ItemStyle Width="20%" />
                    </asp:BoundField>
                    <asp:TemplateField>
                        <ItemStyle Width="5%" />
                        <ItemTemplate>
                            <asp:Literal ID="litAnch" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <span class="warning">No Option Group To Display :(</span>
                </EmptyDataTemplate>
                <PagerStyle CssClass="gvPager" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>

