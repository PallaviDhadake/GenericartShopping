<%@ Page Title="Add Team | Genericart Shopping Support Team" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="add-team-master.aspx.cs" Inherits="supportteam_add_team_master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        $(document).ready(function () {
            $('[id$=gvTeam]').DataTable({
                columnDefs: [
                    { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7, 8] }
                ],
                order: [[0, 'desc']]
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle"><%= pageHeadName %></h2>
    <span class="space15"></span>

    <div id="viewTeam" runat="server">
        <%--<a href="add-team-master.aspx?action=new" class="btn btn-primary">Add New</a>--%>
        <span class="space15"></span>
        <%--gridview start--%>
        <asp:GridView ID="gvTeam" runat="server" AutoGenerateColumns="False"
            CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None" Width="100%" OnRowDataBound="gvTeam_RowDataBound">

            <HeaderStyle CssClass="bg-dark" />
            <AlternatingRowStyle CssClass="alt" />
            <Columns>
                <asp:BoundField DataField="TeamID">
                    <HeaderStyle CssClass="HideCol" />
                    <ItemStyle CssClass="HideCol" />
                </asp:BoundField>
                <asp:BoundField DataField="TeamUserStatus">
                    <HeaderStyle CssClass="HideCol" />
                    <ItemStyle CssClass="HideCol" />
                </asp:BoundField>
                <asp:BoundField DataField="TeamTaskID">
                    <HeaderStyle CssClass="HideCol" />
                    <ItemStyle CssClass="HideCol" />
                </asp:BoundField>
                <asp:BoundField DataField="TeamUserID" HeaderText="User ID">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="TeamRegDate" HeaderText="Date">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="TeamPersonName" HeaderText="Name">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Task">
                    <ItemStyle Width="10%" />
                    <ItemTemplate>
                        <asp:Literal ID="litTask" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status">
                    <ItemStyle Width="10%" />
                    <ItemTemplate>
                        <asp:Literal ID="litStatus" runat="server"></asp:Literal>
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
                <span class="warning">No Team Member to Display :(</span>
            </EmptyDataTemplate>
            <PagerStyle CssClass="gvPager" />
        </asp:GridView>
        <%-- gridview End--%>
    </div>

    <div id="editTeam" runat="server">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <%--card start--%>
                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title"><%=pgTitle %></h3>
                    </div>
                    <%-- card body--%>
                    <div class="card-body">
                        <div class="colorLightBlue">
                            <span>Id :</span>
                            <asp:Label ID="lblId" runat="server" Text="[New]"></asp:Label>
                        </div>

                        <span class="space15"></span>
                        <%--form row start--%>
                        <div class="form-row">


                            <div class="form-group col-md-6">
                                <label>Name :*</label>
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Mobile No :*</label>
                                <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" Width="100%" MaxLength="10"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>UserName :</label>
                                <asp:TextBox ID="txtUserName" runat="server" Width="100%" CssClass="form-control" ReadOnly="true" />
                            </div>

                            <div class="form-group col-md-6">
                                <label>Password :</label>
                                <asp:TextBox ID="txtPassword" runat="server" Width="100%" CssClass="form-control" ReadOnly="true" />
                            </div>

                            <div class="form-group col-md-6">
                                <label>Task : *</label>
                                <asp:DropDownList ID="ddrTask" runat="server" CssClass="form-control" Width="100%">
                                    <asp:ListItem Value="0"><- Select -></asp:ListItem>
                                    <asp:ListItem Value="1">Registered customer list</asp:ListItem>
                                    <asp:ListItem Value="2">Delivered order list </asp:ListItem>
                                    <asp:ListItem Value="3">All order list</asp:ListItem>
                                    <asp:ListItem Value="4">Lab appointment list</asp:ListItem>
                                    <asp:ListItem Value="5">Doctor appointment list</asp:ListItem>
                                    <asp:ListItem Value="6">Prescription request list</asp:ListItem>
                                    <asp:ListItem Value="7">Purchase Department</asp:ListItem>
                                    <asp:ListItem Value="8">Company Owned Shop Orders</asp:ListItem>
                                    <asp:ListItem Value="9">Trainee</asp:ListItem>
                                    <asp:ListItem Value="10">Online Payment Settlement</asp:ListItem>
                                </asp:DropDownList>
                            </div>

                        </div>
                        <%--form row End--%>
                    </div>
                    <%-- card body end--%>
                </div>
                <%--card end--%>

                <!-- Button controls starts -->
                <span class="space10"></span>
                <%--  <%=errMsg %>--%>
                <span class="space10"></span>
                <asp:Button ID="btnSave" runat="server"
                    CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                <asp:Button ID="btnBlock" runat="server"
                    CssClass="btn btn-outline-danger" Text="Block" OnClientClick="return confirm('Are you sure to block?');" OnClick="btnBlock_Click" />
                <asp:Button ID="btnActive" runat="server"
                    CssClass="btn btn-outline-success" Text="Active" OnClick="btnActive_Click" />
                <asp:Button ID="btnDelete" runat="server"
                    CssClass="btn btn-outline-info" Text="Delete" OnClientClick="return confirm('Are you sure to delete?');" OnClick="btnDelete_Click" />
                <asp:Button ID="btnCancel" runat="server"
                    CssClass="btn btn-outline-dark" Text="Cancel" OnClick="btnCancel_Click" />
                <div class="float_clear"></div>
                <!-- Button controls ends -->
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

