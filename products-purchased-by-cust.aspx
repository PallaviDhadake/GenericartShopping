<%@ Page Language="C#" AutoEventWireup="true" CodeFile="products-purchased-by-cust.aspx.cs" Inherits="products_purchased_by_cust" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta http-equiv="x-ua-compatible" content="ie=edge" />

    <title>Customer Lookup</title>

    <!-- Font Awesome Icons -->
    <link rel="stylesheet" href="admingenshopping/plugins/fontawesome-free/css/all.min.css" />
    <!-- IonIcons -->
    <link rel="stylesheet" href="http://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css" />
    <!-- Theme style -->
    <link href="admingenshopping/dist/css/adminlte.css" rel="stylesheet" />
    <!-- Google Font: Source Sans Pro -->
    <link href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700" rel="stylesheet" />

    <link rel="stylesheet" type="text/css" href="admingenshopping/css/iAdmin.css" />

    <link href="https://fonts.googleapis.com/css?family=Open+Sans" rel="stylesheet" />
    <script src="js/jquery-2.2.4.min.js" type="text/javascript"></script>
    <script src="js/iScripts.js" type="text/javascript"></script>

    <link href="admingenshopping/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="admingenshopping/js/jquery.dataTables.min.js"></script>

    <link href="css/jquery.fancybox.css" rel="stylesheet" />
    <script src="js/jquery.fancybox.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {

            $('[id$=gvMedicine]').DataTable({
                columnDefs: [
                     { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7] }
                ],
                order: [[1, 'asc']]
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server" class="pad_15">
        <h2 class="pgTitle">Ordered Products List</h2>
        <span class="space15"></span>

        <div id="viewProducts" runat="server">
            <div>
                <asp:GridView ID="gvMedicine" runat="server" AutoGenerateColumns="False"
                    CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None">
                    <HeaderStyle CssClass="bg-dark" />
                    <AlternatingRowStyle CssClass="alt" />
                    <Columns>
                        <asp:BoundField DataField="FK_DetailProductID">
                            <HeaderStyle CssClass="HideCol" />
                            <ItemStyle CssClass="HideCol" />
                        </asp:BoundField>
                         <asp:BoundField DataField="OrdDetails" HeaderText="Order Details">
                            <ItemStyle Width="10%" />
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
                    </Columns>
                    <EmptyDataTemplate>
                        <span class="warning">No Items to Display</span>
                    </EmptyDataTemplate>
                    <PagerStyle CssClass="gvPager" />
                </asp:GridView>

                <span class="space20"></span>
                <a href="<%= backLink %>" class="btn btn-md btn-dark">Back</a>
                <%= errMsg %>
            </div>
        </div>
    </form>
</body>
</html>
