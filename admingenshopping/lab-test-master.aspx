<%@ Page Title="Lab Test Master" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="lab-test-master.aspx.cs" Inherits="admingenshopping_lab_test_master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
        $(document).ready(function () {
            $('[id$=gvLabs]').DataTable({
                columnDefs: [
                     { orderable: false, targets: [0, 2] }
                ],
                order: [[0, 'desc']]
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Lab Test Master</h2>
    <span class="space15"></span>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--New/Edit Form Start--%>
            <div id="editProf" runat="server">
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
                                <label for="cName">Lab Test Name :*</label>
                                <asp:TextBox ID="txtLabName" runat="server" CssClass="form-control" Width="100%" MaxLength="100"></asp:TextBox>
                            </div>
                        </div>
                    </div>
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
            </div>
            <%--New/Edit Form End--%>

            <div id="viewprof" runat="server">
                <span class="space15"></span>
                <div>
                    <asp:GridView ID="gvLabs" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None" Width="100%"
                        AutoGenerateColumns="false" OnRowDataBound="gvLabs_RowDataBound">
                        <RowStyle CssClass="" />
                        <HeaderStyle CssClass="bg-dark" />
                        <AlternatingRowStyle CssClass="alt" />
                        <Columns>
                            <asp:BoundField DataField="LabTestID">
                                <HeaderStyle CssClass="HideCol" />
                                <ItemStyle CssClass="HideCol" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LabTestName" HeaderText="Lab Name">
                                <ItemStyle Width="30%" />
                            </asp:BoundField>
                            <asp:TemplateField>
                                <ItemStyle Width="5%" />
                                <ItemTemplate>
                                    <asp:Literal ID="litAnch" runat="server"></asp:Literal></ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <span class="warning">No Data to Display :(</span>
                        </EmptyDataTemplate>
                        <PagerStyle CssClass="gvPager" />
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

