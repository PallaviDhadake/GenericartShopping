<%@ Page Title="Add Units | Genericart Shopping Admin Control Panel" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="unit-master.aspx.cs" Inherits="supportteam_unit_master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        $(document).ready(function () {
            $('[id$=gvUnit]').DataTable({
                columnDefs: [
                     { orderable: false, targets: [0, 2] }
                ],
                order: [[0, 'desc']]
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">Unit Master</h2>
    <span class="space15"></span>
    <%--New/Edit Form Start--%>
    <div id="editProf" runat="server">
        <div class="card card-primary">
            <div class="card-header">
                <h3 class="card-title"><%=pgTitle %></h3>
            </div>
            <div class="card-body">
                <div class="colorLightBlue">
                    <span>Id :</span>
                    <asp:Label ID="lblId" runat="server" Text="[New]"></asp:Label>
                </div>
                <span class="space15"></span>
                <div class="form-row">
                    <div class="form-group col-md-6">
                        <label>Unit Name :*</label>
                        <asp:TextBox ID="txtUnitName" runat="server" CssClass="form-control" Width="100%" MaxLength="30"></asp:TextBox>
                    </div>

                </div>

            </div>
            <%--card body end--%>
        </div>
        <span class="space10"></span>
        <%=errMsg %>
        <span class="space10"></span>
        <asp:Button ID="btnSave" runat="server"
            CssClass="btn btn-sm btn-primary" Text="Save" OnClick="btnSave_Click" />
        <asp:Button ID="btnDelete" runat="server"
            CssClass="btn btn-sm btn-outline-info" Text="Delete" OnClientClick="return confirm('Are you sure to delete?');"
            OnClick="btnDelete_Click" />
        <asp:Button ID="btnCancel" runat="server"
            CssClass="btn btn-sm btn-outline-dark" Text="Cancel"
            OnClick="btnCancel_Click" />
        <div class="float_clear"></div>
    </div>
    <%--New/Edit Form Start--%>

    <%--Suppliers GridView Start--%>
    <div id="viewprof" runat="server">
        <a href="unit-master.aspx?action=new" class="btn btn-primary btn-sm">Add New</a>
        <span class="space20"></span>
        <div>
            <asp:GridView ID="gvUnit" runat="server" CssClass="table table-striped table-bordered table-hover" GridLines="None" Width="100%"
                AutoGenerateColumns="false" OnRowDataBound="gvUnit_RowDataBound">
                <RowStyle CssClass="" />
                <HeaderStyle CssClass="bg-dark" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="UnitID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="UnitName" HeaderText="Name">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>

                    <asp:TemplateField>
                        <ItemStyle Width="5%" />
                        <ItemTemplate>
                            <asp:Literal ID="litAnch" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <span class="warning">No Unit to Display :(</span>
                </EmptyDataTemplate>
                <PagerStyle CssClass="gvPager" />
            </asp:GridView>
        </div>
    </div>
    <%--Suppliers GridView Start--%>
</asp:Content>

