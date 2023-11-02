<%@ Page Title="Disease Master | Genericart Shopping Support Team " Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="disease-master.aspx.cs" Inherits="supportteam_disease_master" %>

<%@ MasterType VirtualPath="~/admingenshopping/MasterAdmin.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        $(document).ready(function () {
            $('[id$=gvDisease]').DataTable({
                columnDefs: [
                     { orderable: false, targets: [0, 2] }
                ],
                order: [[0, 'desc']]
            });

            $('[id$=gvProducts]').DataTable({
                columnDefs: [
                     { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6] }
                ],
                order: [[0, 'desc']]
            });
        });

    </script>
    <script type="text/javascript">
        function CheckAll(oCheckbox) {
            var gvFr = document.getElementById("<%=gvProducts.ClientID %>");
            for (i = 1; i < gvFr.rows.length; i++) {
                gvFr.rows[i].cells[1].getElementsByTagName("INPUT")[0].checked = oCheckbox.checked;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">Concern Master</h2>
    <span class="space15"></span>
    <%--New/Edit Form Start--%>
    <div id="editDisease" runat="server">
        <div class="card card-primary">
            <div class="card-header">
                <h3 class="card-title"><%=pgTitle %></h3>
            </div>
            <div class="card-body">
                <div class="col-sm-7">
                    <div class="colorLightBlue">
                        <span>Id :</span>
                        <asp:Label ID="lblId" runat="server" Text="[New]"></asp:Label>
                    </div>
                    <span class="space15"></span>
                    <div class="form-group">
                        <label for="dName">Disease Name:*</label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control " MaxLength="50" Width="80%"></asp:TextBox>
                    </div>


                    <div class="form-group">
                        <label>Photo: </label>
                        <div class="input-group">
                            <asp:FileUpload ID="fuImg" runat="server" />
                        </div>
                    </div>
                    <%= disImg %>
                </div>
                <div id="assignProd" runat="server">
                        <span class="space20"></span>
                        <div class="form-row">
                            <div class="form-group col-md-6">
                                <label>Main Category :*</label>
                                <asp:DropDownList ID="ddrMainCategory" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddrMainCategory_SelectedIndexChanged">
                                    <asp:ListItem Value="0"><- Select -></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Sub Category :*</label>
                                <asp:DropDownList ID="ddrSubCategory" runat="server" CssClass="form-control" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddrSubCategory_SelectedIndexChanged">
                                    <asp:ListItem Value="0"><- Select -></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <span class="space20"></span>
                        <div class="table-responsive-md">
                            <asp:GridView ID="gvProducts"  runat="server" CssClass="table table-striped table-bordered table-hover" GridLines="None" 
                                AutoGenerateColumns="false" OnRowDataBound="gvProducts_RowDataBound" OnRowCommand="gvProducts_RowCommand" >
                                <HeaderStyle CssClass="thead-dark" />
                                <RowStyle CssClass="" />
                                <AlternatingRowStyle CssClass="alt" />
                                <Columns>
                                    <asp:BoundField DataField="ProductID">
                                        <HeaderStyle CssClass="HideCol" />
                                        <ItemStyle  CssClass="HideCol"/>
                                    </asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemStyle Width="5%" />
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" />

                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server"  />
                                            <asp:Button ID="cmdDelete" runat="server" CssClass="deleteProd" CommandName="gvDel" Text=" " OnClientClick="return confirm('Are you sure you want to delete this?');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ProductName" HeaderText="Product Name">
                                        <ItemStyle Width="25%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ProductSKU" HeaderText="Code">
                                        <ItemStyle Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ProductCatName" HeaderText="Category">
                                        <ItemStyle Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="UnitName" HeaderText="Unit">
                                        <ItemStyle Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MfgName" HeaderText="Brand Name">
                                        <ItemStyle Width="12%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PriceMRP" HeaderText="MRP">
                                        <ItemStyle Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PriceSale" HeaderText="Sale Rate">
                                        <ItemStyle Width="9%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ProductStock" HeaderText="Stock">
                                        <ItemStyle Width="5%" />
                                    </asp:BoundField>  
                                    <%--<asp:TemplateField>
                                        <ItemStyle Width="5%" />
                                        <ItemTemplate>
                                            <asp:Button ID="cmdDelete" runat="server" CssClass="deleteProd" CommandName="gvDel" Text=" " OnClientClick="return confirm('Are you sure you want to delete this?');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>    --%>            
                                </Columns>
                                <EmptyDataTemplate>
                                    <span class="warning">Its Empty Here... :(</span>
                                </EmptyDataTemplate>
                                <PagerStyle CssClass="" />
                            </asp:GridView>            
                        </div>
                        <span class="space20"></span>
                        <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-sm btn-primary" Text="Add Products" OnClick="btnAdd_Click" />
                    </div>
            </div>
        </div>
        <span class="space15"></span>
        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" Text="Submit" OnClick="btnSubmit_Click" />
        <asp:Button ID="btnDelete" runat="server"
            CssClass="btn btn-sm btn-outline-info" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this?');" OnClick="btnDelete_Click" />
        <asp:Button ID="btnCancel" runat="server"
            CssClass="btn btn-sm btn-outline-dark" Text="Cancel" OnClick="btnCancel_Click" />
        <div class="float_clear"></div>
    </div>
    <%--New/Edit Form Start--%>

    
    <div id="viewDisease" runat="server">
        <a href="disease-master.aspx?action=new" class="btn btn-primary btn-sm">Add New</a>
        <span class="space25"></span>
        <asp:GridView ID="gvDisease" runat="server" AutoGenerateColumns="False"
            CssClass="table table-striped table-bordered table-hover" GridLines="None" OnRowDataBound="gvDisease_RowDataBound"
            OnPageIndexChanging="gvDisease_PageIndexChanging" Width="100%">
            <RowStyle CssClass="" />
            <HeaderStyle CssClass="bg-dark" />
            <AlternatingRowStyle CssClass="alt" />
            <Columns>
                <asp:BoundField DataField="DiseaseId">
                    <HeaderStyle CssClass="HideCol" />
                    <ItemStyle CssClass="HideCol" />
                </asp:BoundField>
                <asp:BoundField DataField="DiseaseName" HeaderText="Disease Name">
                    <ItemStyle Width="50%" />
                </asp:BoundField>

                <asp:TemplateField>
                    <ItemStyle Width="20%" />
                    <ItemTemplate>
                        <asp:Literal ID="litAnch" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <span class="warning">No Disease to Display :(</span>
            </EmptyDataTemplate>
            <PagerStyle CssClass="gvPager" />
        </asp:GridView>
    </div>
    
</asp:Content>

