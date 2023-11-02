<%@ Page Title="Dashboard | Genericart Shopping Admin Control Panel" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="dashboard.aspx.cs" Inherits="admingenshopping_dashboard" %>
<%@ MasterType VirtualPath="~/admingenshopping/MasterAdmin.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Dashboard</h2>
    <span class="space15"></span>
    <%= nearestShopMarkup %>
    <%= nearestShopMarkupEnq %>
<%--Dashboard card boxes start--%>
    <div class="container-fluid">
        <!-- Small boxes (Stat box) -->
        <div class="row">
            <div class="col-lg-3 col-6">
                <!-- small box -->
                <div class="small-box bg-info">
                    <div class="inner">
                        <h3><%= arrCounts[6] %></h3>

                        <p>Total Orders </p>
                    </div>
                    <div class="icon">
                        <i class="ion ion-bag"></i>
                    </div>
                    <a href="order-reports.aspx" class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <!-- ./col -->
            <div class="col-lg-3 col-6">
                <!-- small box -->
                <div class="small-box bg-primary">
                    <div class="inner">
                        <h3><%= arrCounts[0] %></h3>

                        <p>New Orders </p>
                    </div>
                    <div class="icon">
                        <i class="ion ion-bag"></i>
                    </div>
                    <a href="order-reports.aspx?type=new" class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <!-- ./col -->
            <div class="col-lg-3 col-6">
                <!-- small box -->
                <div class="small-box bg-primary">
                    <div class="inner">
                        <h3><%= arrCounts[8] %></h3>

                        <p>New Favourite Shop Orders </p>
                    </div>
                    <div class="icon">
                        <i class="ion ion-bag"></i>
                    </div>
                    <a href="order-reports.aspx?type=new-fav" class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <!-- ./col -->

            <div class="col-lg-3 col-6">
                <!-- small box -->
                <div class="small-box bg-olive">
                    <div class="inner">
                        <h3><%= arrCounts[4] %></h3>

                        <p>New Prescription Orders </p>
                    </div>
                    <div class="icon">
                        <i class="ion ion-bag"></i>
                    </div>
                    <a href="prescription-orders-report.aspx?type=new" class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <!-- ./col -->

            <div class="col-lg-3 col-6">
                <!-- small box -->
                <div class="small-box bg-olive">
                    <div class="inner">
                        <h3><%= arrCounts[9] %></h3>

                        <p>New Fav Shop Prescription Orders </p>
                    </div>
                    <div class="icon">
                        <i class="ion ion-bag"></i>
                    </div>
                    <a href="prescription-orders-report.aspx?type=new-fav" class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <!-- ./col -->

            <div class="col-lg-3 col-6">
                <!-- small box -->
                <div class="small-box bg-teal">
                    <div class="inner">
                        <h3><%= arrCounts[5] %></h3>

                        <p>New Enquiries </p>
                    </div>
                    <div class="icon">
                        <i class="ion ion-bag"></i>
                    </div>
                    <a href="enquiry-report.aspx?type=new" class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <!-- ./col -->
        </div>
        <div class="row">
            
            <div class="col-lg-3 col-6">
                <!-- small box -->
                <div class="small-box bg-success">
                    <div class="inner">
                        <h3><%=arrCounts[1] %></h3>
                        <span class="space5"></span>
                        <p>Products</p>
                    </div>
                    <div class="icon">
                        <i class="ion ion-stats-bars"></i>
                    </div>
                    <a href="product-master.aspx" class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <!-- ./col -->
            
            <%--<div class="col-lg-3 col-6">
                <!-- small box -->
                <div class="small-box bg-danger">
                    <div class="inner">
                        <h3><%= arrCounts[3] %></h3>

                        <p>Manufacturers</p>
                    </div>
                    <div class="icon">
                        <i class="ion ion-pie-graph"></i>
                    </div>
                    <a href="manufacturers.aspx" class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></a>
                </div>
            </div>--%>
            <!-- ./col -->

            <div class="col-lg-3 col-6">
                <!-- small box -->
                <div class="small-box bg-purple">
                    <div class="inner">
                        <i class="fas fa-3x fa-pills"></i>
                        <span class="space10"></span>
                        <p>Product wise Orders </p>
                    </div>
                    <div class="icon">
                        <i class="ion ion-bag"></i>
                    </div>
                    <a href="medicine-order-report.aspx" class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></a>
                </div>
            </div>

            <div class="col-lg-3 col-6">
                <!-- small box -->
                <div class="small-box bg-gradient-orange">
                    <div class="inner">
                        <i class="fas fa-3x fa-store-alt"></i>
                        <span class="space10"></span>
                        <p>Shop wise Orders </p>
                    </div>
                    <div class="icon">
                        <i class="ion ion-bag"></i>
                    </div>
                    <a href="medicine-order-report-shopwise.aspx" class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></a>
                </div>
            </div>

            <div class="col-lg-3 col-6">
                <!-- small box -->
                <div class="small-box bg-lightblue">
                    <div class="inner">
                        <i class="fas fa-3x fa-store-alt"></i>
                        <span class="space10"></span>
                        <p>DH / ZH wise Orders </p>
                    </div>
                    <div class="icon">
                        <i class="ion ion-bag"></i>
                    </div>
                    <a href="medicine-order-report-shopwise.aspx?type=heads" class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <div class="col-lg-3 col-6">
                <!-- small box -->
                <div class="small-box bg-warning">
                    <div class="inner">
                        <h3><%= arrCounts[2] %></h3>

                        <p>Registered Customers</p>
                    </div>
                    <div class="icon">
                        <i class="ion ion-person-add"></i>
                    </div>
                    <a href="customer-details.aspx" class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <!-- ./col -->
            <div class="col-lg-3 col-6">
                <!-- small box -->
                <div class="small-box bg-danger">
                    <div class="inner">
                        <h3><%= arrCounts[7] %></h3>

                        <p>New Generic Mitra</p>
                    </div>
                    <div class="icon">
                        <i class="ion ion-person-add"></i>
                    </div>
                    <a href="generi-mitra.aspx" class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></a>
                </div>
            </div>
        </div>

        <div class="row">
            <%--<div class="col-12 col-sm-6 col-md-3">
                <a href="../assign_log.txt" target="_blank">
                    <div class="info-box">
                        <span class="info-box-icon bg-info elevation-1"><i class="fas fa-sync-alt"></i></span>

                        <div class="info-box-content">
                            <span class="info-box-text">Auto Routed Orders</span>
                        </div>
                    </div>
                </a>
            </div>
            <div class="col-12 col-sm-6 col-md-3">
                <a href="../hourly_assign_log.txt" target="_blank">
                    <div class="info-box">
                        <span class="info-box-icon bg-success elevation-1"><i class="fas fa-clock"></i></span>

                        <div class="info-box-content">
                            <span class="info-box-text">4 Hour Reverted Orders</span>
                        </div>
                    </div>
                </a>
            </div>--%>

            <!-- enq -->
            <%--<div class="col-12 col-sm-6 col-md-3">
                <a href="../enq_assign_log.txt" target="_blank">
                    <div class="info-box">
                        <span class="info-box-icon bg-info elevation-1"><i class="fas fa-sync-alt"></i></span>

                        <div class="info-box-content">
                            <span class="info-box-text">Auto Routed Enquiries</span>
                        </div>
                    </div>
                </a>
            </div>
            <div class="col-12 col-sm-6 col-md-3">
                <a href="../hourly_enq_assign_log.txt" target="_blank">
                    <div class="info-box">
                        <span class="info-box-icon bg-success elevation-1"><i class="fas fa-clock"></i></span>

                        <div class="info-box-content">
                            <span class="info-box-text">4 Hour Reverted Enquiries</span>
                        </div>
                    </div>
                </a>
            </div>--%>
        </div>

        <!-- Info boxes -->
        <%--<div class="row">
            <div class="col-12 col-sm-6 col-md-3">
                <div class="info-box">
                    <span class="info-box-icon bg-info elevation-1"><i class="fas fa-cog"></i></span>

                    <div class="info-box-content">
                        <span class="info-box-text">New Enquiry</span>
                        <span class="info-box-number">
                            0
                                       
                        </span>
                    </div>
                </div>
            </div>
            <div class="col-12 col-sm-6 col-md-3">
                <div class="info-box mb-3">
                    <span class="info-box-icon bg-success elevation-1"><i class="fas fa-thumbs-up"></i></span>

                    <div class="info-box-content">
                        <span class="info-box-text">Incomplete Enquiry</span>
                        <span class="info-box-number">154</span>
                    </div>
                </div>
            </div>
            <div class="clearfix hidden-md-up"></div>

            <div class="col-12 col-sm-6 col-md-3">
                <div class="info-box mb-3">
                    <span class="info-box-icon bg-warning elevation-1"><i class="fas fa-users"></i></span>

                    <div class="info-box-content">
                        <span class="info-box-text">Monthly Enquiry</span>
                        <span class="info-box-number">760</span>
                    </div>
                </div>
            </div>
            <div class="col-12 col-sm-6 col-md-3">
                <div class="info-box mb-3">
                    
                    <span class="info-box-icon bg-danger elevation-1"><i class="fas fa-shopping-cart"></i></span>

                    <div class="info-box-content">
                        <span class="info-box-text">Monthly Orders</span>
                        <span class="info-box-number">2,000</span>
                    </div>
                </div>
            </div>
        </div>--%>
    </div>
    <%--  Dashboard cards end--%>
</asp:Content>

