<%@ Page Title="Product Option Master" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="product-option-master.aspx.cs" Inherits="admingenshopping_product_option_master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
        $(document).ready(function () {
            $('[id$=gvProducts]').DataTable({
                columnDefs: [
                    { orderable: false, targets: [0, 1, 2, 3, 5] }
                ],
                order: [[0, 'desc']]
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Product Option Master</h2>
    <span class="space15"></span>

    <div id="editProdOptions" runat="server">
       <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>--%>
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
                        <div class="row">
                            <div class="form-group col-sm-6">
                                <label>Select Product : *</label>
                                <asp:DropDownList ID="ddrProduct" runat="server" CssClass="form-control" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddrProduct_SelectedIndexChanged">
                                    <asp:ListItem Value="0"><- Select -></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group col-sm-6">
                                <label>Select Option Group : *</label>
                                <asp:DropDownList ID="ddrOptionGroup" runat="server" CssClass="form-control" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddrOptionGroup_SelectedIndexChanged">
                                    <asp:ListItem Value="0"><- Select -></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group col-sm-6">
                                <label>Select Option : *</label>
                                <asp:DropDownList ID="ddrOptions" runat="server" CssClass="form-control" Width="100%">
                                    <asp:ListItem Value="0"><- Select -></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group col-sm-6">
                                <label>Price Increament :*</label>
                                <asp:TextBox ID="txtPriceIncreament" runat="server" CssClass="form-control" Width="100%" ></asp:TextBox>
                            </div>
                            <div class=" form-group col-sm-6">
                                <asp:CheckBox ID="chkIsActive" runat="server" TextAlign="Right" />
                                <label class="form-check-label"><strong>IsActive ?</strong> </label>
                            </div>
                        </div>

                        <%--form row End--%>
                    </div>
                    <%-- card body end--%>
                </div>
                <%--card end--%>

                <!-- Button controls starts -->
                <span class="space10"></span>
                <%=errMsg %>
                <span class="space10"></span>
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-outline-info" Text="Delete" OnClientClick="return confirm('Are you sure to delete?');" OnClick="btnDelete_Click" />
                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-outline-dark" Text="Cancel" OnClick="btnCancel_Click" />
                <div class="float_clear"></div>
                <!-- Button controls ends -->
           <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>

    <div id="viewProdOptions" runat="server">
        <%--<a href="product-option-master.aspx?action=new" runat="server" class="btn btn-primary">Add New</a>--%>
        <span class="space15"></span>
        <div class="formPanel">
            <asp:GridView ID="gvProducts" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
                AutoGenerateColumns="false" OnRowDataBound="gvProducts_RowDataBound" Width="100%">
                <RowStyle CssClass="" />
                <HeaderStyle CssClass="bg-dark" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="ProdOptionID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ProductName" HeaderText="Product Name">
                        <ItemStyle Width="20%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OptionGroupName" HeaderText="Option Group">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OptionName" HeaderText="Options">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PriceIncrement" HeaderText="Price">
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
                    <span class="warning">Its Empty Here... :(</span>
                </EmptyDataTemplate>
                <PagerStyle CssClass="gvPager" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>

