<%@ Page Title="Blogs Master" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="blogs-master.aspx.cs" Inherits="admingenshopping_blogs_master" %>
<%@ MasterType VirtualPath="~/admingenshopping/MasterAdmin.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
        $(function () {
            $('#<% =txtNDate.ClientID%>').datepick({ dateFormat: 'dd/mm/yyyy' });
        });
    </script>
    <script>
        $(document).ready(function () {
            $('[id$=gvBlogs]').DataTable({
                columnDefs: [
                     { orderable: false, targets: [0, 2, 3] }
                ],
                order: [[0, 'desc']]
            });
        });
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Blogs Master</h2>
    <span class="space15"></span>

    <div id="editBlog" runat="server">
        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>--%>
                <%--card start--%>
                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title"><%= pgTitle %></h3>
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
                                <label>Date :*</label>
                                <asp:TextBox ID="txtNDate" runat="server" CssClass="form-control" Width="100%" MaxLength="10" placeholder="Click here to open caldendar"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-md-6">
                                <label>Title :*</label>
                                <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" Width="100%" MaxLength="300"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-md-6">
                                <label>Description :*</label>
                                <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control textarea" TextMode="MultiLine" Width="100%" Height="250"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-md-6">
                                <label>Blog Image :</label>
                                <asp:FileUpload ID="fuImg" runat="server" CssClass="form-control-file" />
                                <span class="space10"></span>
                                <%= blogImg %>
                            </div>
                        </div>
                        <%--form row End--%>
                    </div>
                   <%-- card body end--%>
                </div>
                <%--card end--%>

                <!-- Button controls starts -->
                <span class="space10"></span>
                <span class="space10"></span>
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click"/>
                <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-outline-info" Text="Delete" OnClientClick="return confirm('Are you sure to delete?');" OnClick="btnDelete_Click"/>
                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-outline-dark" Text="Cancel" OnClick="btnCancel_Click" />
                <div class="float_clear"></div>
                <!-- Button controls ends -->
            <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>

    <!-- Gridview to show saved data starts here -->
    <div id="viewBlog" runat="server">
        <a href="blogs-master.aspx?action=new" runat="server" class="btn btn-primary btn-md">Add New</a>
        <span class="space20"></span>
        <div class="formPanel table-responsive-md">
            <asp:GridView ID="gvBlogs"  runat="server" CssClass="table table-striped table-bordered table-hover" GridLines="None" 
                AutoGenerateColumns="false" OnPageIndexChanging="gvBlogs_PageIndexChanging" OnRowDataBound="gvBlogs_RowDataBound"   >
                <HeaderStyle CssClass="thead-dark" />
                <RowStyle CssClass="" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="NewsId">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle  CssClass="HideCol"/>
                    </asp:BoundField>
                    <asp:BoundField DataField="nDate" HeaderText="Date">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NewsTitle" HeaderText="Title">
                        <ItemStyle Width="30%" />
                    </asp:BoundField>
                    <asp:TemplateField>
                        <ItemStyle Width="5%" />
                        <ItemTemplate>
                            <asp:Literal ID="litAnch" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                </Columns>
                <EmptyDataTemplate>
                    <span class="warning">Its Empty Here... :(</span>
                </EmptyDataTemplate>
                <PagerStyle CssClass="" />
            </asp:GridView>            
        </div>
    </div>
</asp:Content>

