<%@ Page Language="C#" AutoEventWireup="true" CodeFile="customer-lookup.aspx.cs" Inherits="customer_lookup" %>

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
    <script src="js/App/jquery-2.2.4.min.js" type="text/javascript"></script>
    <script src="js/App/iScripts.js" type="text/javascript"></script>

    <link href="admingenshopping/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="admingenshopping/js/jquery.dataTables.min.js"></script>

    <link href="css/jquery.fancybox.css" rel="stylesheet" />
    <script src="js/jquery.fancybox.js"></script>
</head>
<body>
   <form id="form1" runat="server">
        <div class="content-header">
            <div class="container-fluid">
                <div class="card card-primary">
					<div class="card-header">
						<h3 class="card-title">View Details of <%= arrShopInfo[0] %></h3>
					</div>
					<div class="card-body">
						<div class="row bg-white">
							<div class="col-md-6">
								<div class="card-header">
									<h3 class="large colorLightBlue">Customer Details</h3>
								</div>
								<div class="card-body">
									<table class="ordTbl">
										<tr>
											<td style="width:35%;"><span class="text-bold">Customer Name :</span></td>
											<td style="width:70%;"><span class="text-olive text-bold"><%= arrShopInfo[0] %></span></td>
										</tr>
										<tr>
											<td><span class="text-bold">Email :</span></td>
											<td><span class="text-olive text-bold"><%= arrShopInfo[1] %></span></td>
										</tr>
										<tr>
											<td><span class="text-bold">Mobile No. :</span></td>
											<td><span class="text-olive text-bold"><%= arrShopInfo[2] %></span></td>
										</tr>
										<tr>
											<td><span class="text-bold">Date of Birth :</span></td>
											<td><span class="text-olive text-bold"><%= arrShopInfo[3] %></span></td>
										</tr>
										<tr>
											<td><span class="text-bold">Joining Date :</span></td>
											<td><span class="text-olive text-bold"><%= arrShopInfo[4] %></span></td>
										</tr>
										<tr>
											<td><span class="text-bold">Favourite Shop :</span></td>
											<td><span class="text-olive text-bold"><%= arrShopInfo[5] %></span></td>
										</tr>
										<%--<tr>
											<td><span class="text-bold">Total Orders : </span></td>
											<td><span class="text-olive text-bold"><%= arrShopInfo[6] %></span></td>
										</tr>
										<tr>
											<td><span class="text-bold">Last Order Date :</span></td>
											<td><span class="text-olive text-bold"><%= arrShopInfo[7] %></span></td>
										</tr>
										<tr>
											<td><span class="text-bold">Products Purchased Till Now :</span></td>
											<td><span class="text-olive text-bold"><%= arrShopInfo[8] %></span></td>
										</tr>--%>
									</table>
								</div>
							</div>
                            <div class="col-md-6">
								<div class="card-header">
									<h3 class="large colorLightBlue">Order Details</h3>
								</div>
								<div class="card-body">
									<span class="text-bold">Total Orders : <span class="text-olive"><%= arrShopInfo[6] %></span></span>
									<span class="space15"></span>
									<span class="text-bold">Last Order Date :<span class="text-olive"> <%= arrShopInfo[7] %></span></span>
									<span class="space15"></span>
									<span class="text-bold">Products Purchased Till Now :<span class="text-olive"> <%= arrShopInfo[8] %></span></span>
                                    <span class="space15"></span>
									<span class="text-bold">Total Order Amount :<span class="text-olive"> <%= arrShopInfo[9] %></span></span>
                                    <span class="space15"></span>
									<span class="text-bold">Average Order Amount :<span class="text-olive"> <%= arrShopInfo[10] %></span></span>
                                    <span class="space15"></span>
									<span class="text-bold">Yearly Orders Detail :</span><br />
                                    <span class="space15"></span>
                                    <%= arrShopInfo[11] %>
								</div>
                            </div>
						</div>
					</div>
				</div>
                <%= errMsg %>
            </div>
        </div>
    </form>
</body>
</html>
