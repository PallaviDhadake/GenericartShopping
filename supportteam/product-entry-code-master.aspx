<%@ Page Title="" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="product-entry-code-master.aspx.cs" Inherits="supportteam_product_entry_code_master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <script>
         $(document).ready(function () {
             //alert("Datatabel Called");

             $('[id$=gvProducts]').DataTable({
                 columnDefs: [
                     { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7, 8] }
                 ],
                 order: [[0, 'desc']]
             });
         });

     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <span class="bold_weight medium regular">Entered Product's "Entry Code"  <%=entercode %> Out of <%=allprodcode %> </span>
    <span class="space20"></span>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="viewProduct" runat="server">
                <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover" GridLines="None" OnRowDataBound="gvProducts_RowDataBound"
                       >
                     <HeaderStyle CssClass="thead-dark"/>
                        <RowStyle CssClass="" />
                        <AlternatingRowStyle CssClass="alt" />
                    <Columns>
                        <asp:BoundField DataField="ProductID">
                            <HeaderStyle CssClass="HideCol" />
                            <ItemStyle CssClass="HideCol" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ProductName" HeaderText="Product Name">
                            <ItemStyle Width="20%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ProductSKU" HeaderText="Code">
                            <ItemStyle Width="15%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UnitName" HeaderText="Unit">
                            <ItemStyle Width="15%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PriceMRP" HeaderText="MRP">
                            <ItemStyle Width="10%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PriceSale" HeaderText="Sale Rate">
                            <ItemStyle Width="10%" />
                        </asp:BoundField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Literal ID="litTextBox" runat="server"></asp:Literal>
                              <%--<input type="text" id="txtProdCode" class="form-control" placeholder="Enter Prod Entry Code" />--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Literal ID="litButton" runat="server"></asp:Literal>
                                <%--<button class="btn btn-primary" onclick="UpdateProdCode('textboxId', 'prodId')">Update</button>--%>

                                <%--<button class="btn btn-primary" onclick="UpdateProdCode(this.previousElementSibling, 'prodId')">Update</button>--%>


                                <%--<button class="btn btn-primary" onclick="UpdateProdCode(this);">Update</button>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status">
                            <ItemStyle Width="5%" />
                            <ItemTemplate>
                                <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

   <%-- <input id="myTextBox" type="text" />

    <input id="txtProd18166" type="text" class="form-control" placeholder="Enter Prod Entry Code" />

    <a class='btn btn-primary' onclick="myTestFun('myTextBox');">Update Anchor</a>
    <a class='btn btn-primary' onclick="UpdateProdCode('txtProd18166', '1816');">OLd Anchor</a>--%>

  

    <script type="text/javascript">
        function UpdateProdCode(textboxId, prodId) {
            //alert(textboxId + " " + prodId);
            let myTxtVal = document.getElementById(textboxId).value;
            // alert(myTxtVal);
            let ProdtxtBox = document.getElementById(textboxId);


            if (myTxtVal === "") {
                alert("Product Entry Code is mandatory");
                return;
            }
            $.ajax({
                type: "POST",
                url: "product-entry-code-master.aspx/SaveProdCode",
                data: JSON.stringify({ prodIdx: prodId, prodEntryCode: myTxtVal }),

                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    switch (response.d) {
                        case "1":
                            // SUCCESSFUL Entry
                            alert("Entry Code updated successfully.");
                            ProdtxtBox.style.border = "2px solid Green";
                            break;
                        case "2":
                            // DUPLICATION attempted
                            alert("Duplication attempted.");
                            ProdtxtBox.style.border = "2px solid red";
                            //var dataTable = $('[id$=gvProducts]').DataTable(); // Initialize the DataTable and store the instance in a variable
                            //dataTable.draw();
                            break;
                        // more cases...
                        default:
                        // code to execute if expression doesn't match any case
                    }

                },
                failure: function (response) {
                    // alert(response.d);
                }
            });
        }
    </script>
</asp:Content>

