<%@ Page Title="Assign Enquiry To Franchisee" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="assign-enquiry.aspx.cs" Inherits="admingenshopping_assign_enquiry" %>
<%@ MasterType VirtualPath="~/admingenshopping/MasterAdmin.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
        function delConfirm(btn) {
            btn.value = 'processing...';
            return confirm("Are you sure to Assign enquiry to selected shop?");
        }
    </script>
    <script>
        $(document).ready(function () {
            $('[id$=gvShopList]').DataTable({
                columnDefs: [
                     { orderable: false, targets: [0, 2, 3, 5, 6] }
                ]
            });
        });
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Assign Enquiry</h2>
    <span class="space15"></span>

    <div class="card">
        <div class="card-header">
            <h3 class="large colorLightBlue">Enquiry Details</h3>
        </div>
        <div class="card-body">
            <table class="form_table">
                <tr>
                    <td><span class="formLable bold_weight">Enquiry Id :</span></td>
                    <td><span class="formLable">
                        <button type="button" class="btn btn-sm btn-default" data-toggle="modal" data-target="#modal-lg"><i class="fa fa-eye" aria-hidden="true"></i>
                                <%= ordData[0] %>
                        </button></span> 
                    </td>
                </tr>
                <tr>
                    <td><span class="formLable bold_weight">Date :</span></td>
                    <td><span class="formLable"><%= ordData[1] %></span> </td>
                </tr>
                <tr>
                    <td><span class="formLable bold_weight">City :</span></td>
                    <td><span class="formLable"><%= ordData[2] %></span> </td>
                </tr>
                <tr>
                    <td><span class="formLable bold_weight">Zip Code :</span></td>
                    <td><span class="formLable"><%= ordData[3] %></span> </td>
                </tr>
            </table>
        </div>

        <!-- Order Assignment Details Starts -->
        <%= ordData[7] %>
        <%--<div class="card-header">
            <h3 class="medium colorLightBlue">Order Assigning Details</h3>
        </div>
        <div class="card-body">
            <table class="table table-bordered">
                <tr>
                    <th>OrderId</th>
                    <th>Assigned To</th>
                    <th>Shop Code</th>
                    <th>Order Status</th>
                </tr>
                <tr>
                    <td>1</td>
                    <td>Shop Name</td>
                    <td>GMMH0001</td>
                    <td class="clrGreen">Accepted</td>
                </tr>
            </table>
        </div>--%>
        <!-- Order Assignment Ends -->

        
        <div id="shopList" runat="server">
            <div class="card-header">
                <h3 class="medium colorLightBlue">Available Shops List to Assign Enquiry</h3>
                <div class="w-auto float-left pad_10">
                    <asp:RadioButton ID="rdbRelevant" runat="server" GroupName="ShopList" Text=" Show Relevant" Checked="true" AutoPostBack="true" OnCheckedChanged="rdbRelevant_CheckedChanged" />
                </div>
                <div class="w-25 float-left pad_10">
                    <asp:RadioButton ID="rdbAll" runat="server" GroupName="ShopList" Text=" Show All" AutoPostBack="true" OnCheckedChanged="rdbAll_CheckedChanged" />
                </div>
                <div class="float_clear"></div>
            </div>
            <div class="card-body">
                <!-- Dislay GV to assign order to franchisee starts -->
                <asp:GridView ID="gvShopList" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" 
                    GridLines="None" PageSize="1" AutoGenerateColumns="false" 
                    OnRowDataBound="gvShopList_RowDataBound" OnRowCommand="gvShopList_RowCommand">
                    <RowStyle CssClass="" />
                    <AlternatingRowStyle CssClass="alt" />
                    <Columns>
                        <asp:BoundField DataField="FranchID">
                            <HeaderStyle CssClass="HideCol" />
                            <ItemStyle CssClass="HideCol" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FranchName" HeaderText="Shop Name">
                            <ItemStyle Width="30%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FranchShopCode" HeaderText="Shop Code">
                            <ItemStyle Width="12%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FranchMobile" HeaderText="Mobile No.">
                            <ItemStyle Width="12%" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Total Enquiry">
                            <ItemStyle Width="15%" />
                            <ItemTemplate>
                                <asp:Literal ID="litTotalOrd" runat="server"></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Last Enquiry Date">
                            <ItemStyle Width="15%" />
                            <ItemTemplate>
                                <asp:Literal ID="litLastOrdDate" runat="server"></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Width="30%" />
                            <ItemTemplate>
                                <asp:Button ID="cmdAssign" runat="server" CssClass="buttonDel" CommandName="gvAssign" Text="Assign Enquiry"   OnClientClick="return delConfirm(this);"/>
                            </ItemTemplate>
                        </asp:TemplateField>     
                    </Columns>
                    <EmptyDataTemplate>
                        <span class="warning">:(</span>
                    </EmptyDataTemplate>
                    <PagerStyle CssClass="gvPager" />
                </asp:GridView>
                <!-- Dislay GV to assign order to franchisee ends -->
            </div>
        </div>
        

    </div>
    <span class="space10"></span>
    <a href="<%= rdrUrl %>" class="btn btn-sm btn-outline-dark">Back</a>


    <!-- Modal Box for displaying Ordered Products -->
    <div class="modal fade" id="modal-lg">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Product Details</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <asp:GridView ID="gvOrderDetails" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None" PageSize="30" AllowPaging="true"
                                AutoGenerateColumns="false">
                                <RowStyle CssClass="" />
                                <AlternatingRowStyle CssClass="alt" />
                                <Columns>
                                    <asp:BoundField DataField="BrandMedicine" HeaderText="Brand Medicine">
                                        <ItemStyle Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BrandPrice" HeaderText="Brand Price">
                                        <ItemStyle Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="GenericMedicine" HeaderText="Generic Medicine">
                                        <ItemStyle Width="30%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="GenericPrice" HeaderText="Generic Price">
                                        <ItemStyle Width="12%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SavingAmount" HeaderText="Saving Amount">
                                        <ItemStyle Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SavingPercent" HeaderText="Net Savings">
                                        <ItemStyle Width="15%" />
                                    </asp:BoundField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <span class="warning">:(</span>
                                </EmptyDataTemplate>
                                <PagerStyle CssClass="gvPager" />
                            </asp:GridView>
                    </div>
                    <span class="space10"></span>

                </div>
                <div class="modal-footer justify-content-between">
                    <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary">Save changes</button>--%>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    <!-- /.modal -->
</asp:Content>

