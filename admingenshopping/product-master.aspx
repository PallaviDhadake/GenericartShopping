<%@ Page Title="Products Master | Genericart Shopping Admin Panel" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="product-master.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="admingenshopping_product_master" %>
<%@ MasterType VirtualPath="~/admingenshopping/MasterAdmin.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="css/shoppingGencart.css" rel="stylesheet" />
    <script src="<%= Master.rootPath + "js/jquery.popupoverlay.js" %>" type="text/javascript"></script>
    <style type="text/css">
        #mednote{font-weight:400; color:#ff0000; line-height:1.5; font-size:0.9em;}
         .popupBox{background:#fff; width:80%; display:inline-block;}
    </style>
    <script>
       $(document).ready(function () {
           $('[id$=gvProducts]').DataTable({
               columnDefs: [
                    { orderable: false, targets: [0, 2, 3, 4, 5, 9, 10] }
               ],
               order: [[0, 'desc']]
           });
       });
     </script>
    <script>
        $(document).ready(function () {
            document.getElementById("mednote").style.display = "none";
        });
        function CheckPrice() {
            var mrpPrice = document.getElementById('<%= txtPriceMrp.ClientID %>').value;
            var sellPrice = document.getElementById('<%= txtPriceSale.ClientID %>').value;
            if (parseFloat(sellPrice) > parseFloat(mrpPrice)) {
                document.getElementById("mednote").style.display = "block";
                document.getElementById("mednote").innerHTML = "Your selling price is greater than actual MRP Price";
                document.getElementById('<%= txtPriceSale.ClientID %>').style.border = "1px solid #f00";
            }
            else {
                document.getElementById("mednote").style.display = "none";
                document.getElementById('<%= txtPriceSale.ClientID %>').style.border = "1px solid #ccc";
            }
        }
    </script>
    <script>
        $(document).ready(function () { document.getElementById("slide").style.display = "none"; });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Product Master</h2>
    <span class="space15"></span>

    <div id="editProduct" runat="server">
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
                                <label>Product Name :*</label>
                                <asp:TextBox ID="txtProdName" runat="server" CssClass="form-control" Width="100%" MaxLength="500"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Product Code SKU :*</label>
                                <asp:TextBox ID="txtProdSKU" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Product Entry Code :*</label>
                                <asp:TextBox ID="txtProdEnCode" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Manufacturer : *</label>
                                <asp:DropDownList ID="ddrMfr" runat="server" CssClass="form-control" Width="100%">
                                    <asp:ListItem Value="0"><- Select -></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Unit : *</label>
                                <asp:DropDownList ID="ddrUnit" runat="server" CssClass="form-control" Width="100%">
                                    <asp:ListItem Value="0"><- Select -></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group col-md-6">
                                <label>MRP Price :*</label>
                                <asp:TextBox ID="txtPriceMrp" runat="server" CssClass="form-control" Width="100%" onBlur="CheckPrice();"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Selling Price :*</label>
                                <asp:TextBox ID="txtPriceSale" runat="server" CssClass="form-control" Width="100%" onBlur="CheckPrice();"></asp:TextBox>
                                <span id="mednote">note</span>
                            </div>

                            <div class="form-group col-md-6">
                                <label>Tax Percent :*</label>
                                <asp:DropDownList ID="ddrPercent" runat="server" CssClass="form-control" Width="100%" BackColor="AliceBlue" AutoPostBack="true" OnSelectedIndexChanged="ddrPercent_SelectedIndexChanged">
                                    <asp:ListItem Value="-1"><- Select -></asp:ListItem>
                                    <asp:ListItem Value="0">0%</asp:ListItem>
                                    <asp:ListItem Value="5">5%</asp:ListItem>
                                    <asp:ListItem Value="12">12%</asp:ListItem>
                                    <asp:ListItem Value="18">18%</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Taxless Price :*</label>
                                <asp:TextBox ID="txtTaxLess" runat="server" CssClass="form-control" Width="100%" Enabled="false"></asp:TextBox>
                            </div>
                            
                            <div class="form-group col-md-6">
                                <label>Main Category :*</label>
                                <asp:DropDownList ID="ddrMainCategory" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddrMainCategory_SelectedIndexChanged">
                                    <asp:ListItem Value="0"><- Select -></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Sub Category :*</label>
                                <asp:DropDownList ID="ddrSubCategory" runat="server" CssClass="form-control" Width="100%" >
                                    <asp:ListItem Value="0"><- Select -></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Product Packaging :*</label>
                                <asp:TextBox ID="txtPackaging" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Stock Qty :*</label>
                                <asp:TextBox ID="txtStock" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Short Description :*</label>
                                <asp:TextBox ID="txtShortDesc" runat="server" CssClass="form-control textarea" TextMode="MultiLine" Height="150" Width="100%"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Long Description :*</label>
                                <asp:TextBox ID="txtLongDesc" runat="server" CssClass="form-control textarea" TextMode="MultiLine" Width="100%" Height="150"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>SEO Meta Description :</label>
                                <asp:TextBox ID="txtMetaDesc" runat="server" CssClass="form-control textarea" TextMode="MultiLine" Width="100%" Height="150"></asp:TextBox>
                            </div>
                           
                            <div class="form-check col-md-6">
                                <span class="space30"></span>
                                <div>
                                    <asp:CheckBox ID="chkPrescription" runat="server" TextAlign="Right"/>
                                    <label class="form-check-label"><strong>Prescription Required ?</strong> </label>
                                </div>
                                <div>
                                    <asp:CheckBox ID="chkFeatured" runat="server" TextAlign="Right"  />
                                    <label class="form-check-label"><strong>Mark as Featured ?</strong> </label>
                                </div>
                                
                                <div>
                                    <asp:CheckBox ID="chkBestSeller" runat="server" TextAlign="Right" />
                                    <label class="form-check-label" for="bSeller"><strong>Mark as Best Seller ?</strong> </label>
                                </div>

                                <div>
                                    <asp:CheckBox ID="chkActive" runat="server" TextAlign="Right" />
                                    <label class="form-check-label" for="bSeller"><strong>Is Active ?</strong> </label>
                                </div>
                                <div>
                                    <asp:CheckBox ID="chkReminder" runat="server" TextAlign="Right" />
                                    <label class="form-check-label" for="bSeller"><strong>Mark Product For Pill Reminder ?</strong> </label>
                                </div>
                                <div>
                                    <asp:CheckBox ID="chkNotOnline" runat="server" TextAlign="Right" />
                                    <label class="form-check-label" for="bSeller"><strong>Is Not For Online Sale ?</strong> </label>
                                </div>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Product Photo :</label>
                                <asp:FileUpload ID="fuImg" runat="server" CssClass="form-control-file" />
                                <span class="space10"></span>
                                <%= prodPhoto %><span class="space5"></span>
                                <asp:Button ID="btnRemove" runat="server" CssClass="btn btn-secondary" Text="Remove Photo" OnClick="btnRemove_Click" OnClientClick="return confirm('Are you sure to remove photo?');" />
                                <span class="space5"></span>
                                <asp:Button ID="btnPhoto" runat="server" CssClass="btn btn-info" Text="Add Multiple Photos" Visible="false" OnClick="btnPhoto_Click" />
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
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-outline-info" Text="Delete" OnClientClick="return confirm('Are you sure to delete?');" OnClick="btnDelete_Click" />
                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-outline-dark" Text="Cancel" OnClick="btnCancel_Click" />
                <div class="float_clear"></div>
                <!-- Button controls ends -->
            <%--</ContentTemplate>
        </asp:UpdatePanel>--%>

        <!-- popup starts -->
        <div id="slide" class="well posRelative">
            <div class="txtCenter">
                <div class="popupBox border_r_5">
                    <div class="pad_20">
                        <div role="main">
                            <p class="medium semiBold text-success"><%= prodMsg %> </p>
                            <span class="space15"></span> 
                            <p class="medium semiBold themeClrPrime">Do you want to add other photos of product ?? </p>
                            <span class="space15"></span> 
                            <a href="<%= rdrUrl %>" class="btn btn-md btn-success mr-2">Yes</a>
                            <a href="product-master.aspx"  class="btn btn-md btn-danger">No</a>
                        </div>
                    </div>
                </div>
                <div class="float_clear"></div>
            </div>
        </div>
        <!-- popup ends -->
    </div>

    <!-- Gridview to show saved data starts here -->
    <div id="viewProduct" runat="server">
        <a href="product-master.aspx?action=new" runat="server" class="btn btn-primary btn-md">Add New</a>
        <span class="space20"></span>
        <div class="formPanel table-responsive-md">
            <asp:GridView ID="gvProducts"  runat="server" CssClass="table table-striped table-bordered table-hover" GridLines="None" 
                AutoGenerateColumns="false" 
                OnPageIndexChanging="gvProducts_PageIndexChanging" OnRowDataBound="gvProducts_RowDataBound"  >
                <HeaderStyle CssClass="thead-dark" />
                <RowStyle CssClass="" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="ProductID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle  CssClass="HideCol"/>
                    </asp:BoundField>
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
                    <asp:TemplateField HeaderText="Status">
                        <ItemStyle Width="3%" />
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
                    <span class="warning">Its Empty Here... :(</span>
                </EmptyDataTemplate>
                <PagerStyle CssClass="" />
            </asp:GridView>            
        </div>
    </div>
</asp:Content>

