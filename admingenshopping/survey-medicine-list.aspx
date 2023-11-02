<%@ Page Title="Survey Medicines List" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="survey-medicine-list.aspx.cs" Inherits="admingenshopping_survey_medicine_list" %>

<%@ MasterType VirtualPath="~/admingenshopping/MasterAdmin.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        #mednote{font-weight:400; color:#ff0000; line-height:1.5; font-size:0.9em;}
    </style>
    <script>
       $(document).ready(function () {
           $('[id$=gvSurveyMed]').DataTable({
               columnDefs: [
                    { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7, 8] }
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
            var genPrice = document.getElementById('<%= txtGenericPrice.ClientID %>').value;
            var brandPrice = document.getElementById('<%= txtBrandPrice.ClientID %>').value;
            if (parseFloat(genPrice) > parseFloat(brandPrice)) {
                document.getElementById("mednote").style.display = "block";
                document.getElementById("mednote").innerHTML = "Your generic price is greater than brand Price";
                document.getElementById('<%= txtGenericPrice.ClientID %>').style.border = "1px solid #f00";
            }
            else {
                document.getElementById("mednote").style.display = "none";
                document.getElementById('<%= txtGenericPrice.ClientID %>').style.border = "1px solid #ccc";
            }
        }
    </script>
    <script type="text/javascript">
        function CheckAll(oCheckbox) {
            var gvFr = document.getElementById("<%=gvSurveyMed.ClientID %>");
            for (i = 1; i < gvFr.rows.length; i++) {
                gvFr.rows[i].cells[1].getElementsByTagName("INPUT")[0].checked = oCheckbox.checked;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">Survey Medicines List</h2>
    <span class="space15"></span>

    <div id="editMed" runat="server">
        <div class="card card-primary">
            <div class="card-header">
                <h3 class="card-title"><%= pgTitle %></h3>
            </div>
            <div class="card-body">
                <div class="colorLightBlue">
                    <span>Id :</span>
                    <asp:Label ID="lblId" runat="server" Text="[New]"></asp:Label>
                </div>

                <span class="space15"></span>
                <%--form row start--%>
                <div class="form-row">
                    <div class="form-group col-md-6">
                        <label>Content Name :*</label>
                        <asp:TextBox ID="txtContent" runat="server" CssClass="form-control" Width="100%" MaxLength="500"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Brand Name :*</label>
                        <asp:TextBox ID="txtBrandName" runat="server" CssClass="form-control" Width="100%" MaxLength="500"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Company Name : *</label>
                        <asp:TextBox ID="txtBrandCompany" runat="server" CssClass="form-control" Width="100%" MaxLength="300"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Packaging : *</label>
                        <asp:TextBox ID="txtPackaging" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Generic Price :*</label>
                        <asp:TextBox ID="txtGenericPrice" runat="server" CssClass="form-control" Width="100%" onBlur="CheckPrice();"></asp:TextBox>
                        <span id="mednote">note</span>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Brand Price :*</label>
                        <asp:TextBox ID="txtBrandPrice" runat="server" CssClass="form-control" Width="100%" onBlur="CheckPrice();"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Generic Code :*</label>
                        <asp:TextBox ID="txtGenCode" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <span class="space10"></span>
        <span class="space10"></span>
        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSubmit_Click"  />
        <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-outline-info" Text="Delete" OnClientClick="return confirm('Are you sure to delete?');" OnClick="btnDelete_Click"  />
        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-outline-dark" Text="Cancel" OnClick="btnBack_Click" />
        <div class="float_clear"></div>
    </div>

    <div id="viewMed" runat="server">
        <div class="posRelative">
            <div style="position:absolute; top:30px; right:10px;" class="text-primary text-lg text-bold">Total Medicines : <%= totalCount %></div>
        </div>
        <table>
            <tr>
                <td>
                    <asp:RadioButton ID="rdbBasic" runat="server" GroupName="searchMed" TextAlign="Right" Text=" Basic Search" CssClass="mrgRgt10" Checked="true" />
                </td>
                <td>
                    <asp:RadioButton ID="rdbAdvance" runat="server" GroupName="searchMed" TextAlign="Right" Text=" Advanced Search" />
                </td>
            </tr>
        </table>
        <span class="space20"></span>
        <div class="form-row">
            <div class="form-group col-md-6">
                <asp:TextBox ID="txtMedContent" runat="server" CssClass="form-control" Width="100%" MaxLength="500" placeholder="Enter Medicine Content"></asp:TextBox>
            </div>
            <div class="form-group col-md-2">
                <asp:TextBox ID="txtGmpCode" runat="server" CssClass="form-control" Width="100%" MaxLength="500" placeholder="Enter GMP Code"></asp:TextBox>
            </div>
            <div class="form-group col-md-2">
                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click"  />
            </div>
        </div>
        <span class="space20"></span>
        <%--<div style="width:1140px; height:600px; display:block; margin:0 auto; overflow-x:scroll;">--%>
            <asp:GridView ID="gvSurveyMed" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-striped table-bordered table-hover" GridLines="None" OnRowDataBound="gvSurveyMed_RowDataBound" >
                <HeaderStyle CssClass="thead-dark" />
                <RowStyle CssClass="" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="MedicineRowID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                     <asp:TemplateField>
                        <ItemStyle Width="1%" />
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ContentName" HeaderText="Content">
                        <HeaderStyle CssClass="" />
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="BrandName" HeaderText="Brand Name">
                        <ItemStyle Width="5%"/>
                    </asp:BoundField>
                    <asp:BoundField DataField="CompanyName" HeaderText="Brand Company Name">
                        <ItemStyle Width="15%"/>
                    </asp:BoundField>
                    <asp:BoundField DataField="Packaging" HeaderText="Packaging">
                        <ItemStyle Width="5%"/>
                    </asp:BoundField>
                    <asp:BoundField DataField="PriceBrand" HeaderText="Brand Price">
                        <ItemStyle Width="5%"/>
                    </asp:BoundField>
                    <asp:BoundField DataField="PriceGeneric" HeaderText="Generic Price">
                        <ItemStyle Width="5%"/>
                    </asp:BoundField>
                    <asp:BoundField DataField="genCode" HeaderText="Generic Code">
                        <ItemStyle Width="5%"/>
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
        <%--</div>--%>
        <span class="space30"></span>

        <asp:Button ID="btnDeleteMed" runat="server" Text="Delete Medicine" CssClass="btn btn-md btn-info" OnClick="btnDeleteMed_Click" OnClientClick="return confirm('Are you sure to delete?');" />
    </div>
</asp:Content>

