<%@ Page Title="Products Options" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="option-data.aspx.cs" Inherits="admingenshopping_option_data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Options Master</h2>
    <span class="space15"></span>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--New/Edit Form Start--%>
            <div id="editOption" runat="server">
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
                                <label>Select option Group Name :*</label>
                                <asp:DropDownList ID="ddrOptGroup" runat="server" CssClass="form-control" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddrOptGroup_SelectedIndexChanged">
                                    <asp:ListItem Value="0"><- Select -></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="optName">Option Name :*</label>
                                <asp:TextBox ID="txtOptName" runat="server" CssClass="form-control" Width="100%" MaxLength="30"></asp:TextBox>
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
            </div>
            <%--New/Edit Form End--%>

            <%--Suppliers GridView Start   OnPageIndexChanging="gvOption_PageIndexChanging"   --%>
            <div id="viewOption" runat="server">
                <span class="space20"></span>
                <div>
                    <asp:GridView ID="gvOption" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
                        AutoGenerateColumns="false" OnRowCommand="gvOption_RowCommand" OnRowDataBound="gvOption_RowDataBound" Width="100%">
                        <RowStyle CssClass="" />
                        <HeaderStyle CssClass="bg-dark" />
                        <AlternatingRowStyle CssClass="alt" />
                        <Columns>
                            <asp:BoundField DataField="OptionID">
                                <HeaderStyle CssClass="HideCol" />
                                <ItemStyle CssClass="HideCol" />
                            </asp:BoundField>
                            <asp:BoundField DataField="OptionName" HeaderText="Option">
                                <ItemStyle Width="30%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="optionGroup" HeaderText="Option Group">
                                <ItemStyle Width="30%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="OptionDisplayOrder" HeaderText="Display Order">
                                <ItemStyle Width="15%" />
                            </asp:BoundField>
                            <asp:TemplateField>
                                <ItemStyle Width="10%" />
                                <ItemTemplate>
                                    <asp:Button ID="moveUp" runat="server" CssClass="gMoveUp" CommandName="Up" />
                                    <asp:Button ID="moveDown" runat="server" CssClass="gMoveDown" CommandName="Down" />
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
                            <span class="warning">No Options to Display :(</span>
                        </EmptyDataTemplate>
                        <PagerStyle CssClass="gvPager" />
                    </asp:GridView>
                </div>
            </div>
            <%--Suppliers GridView Start--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

