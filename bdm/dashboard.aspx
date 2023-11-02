<%@ Page Title="Dashboard | Genericart Shopping Management Control Panel" Language="C#" MasterPageFile="~/bdm/MasterBdm.master" AutoEventWireup="true" CodeFile="dashboard.aspx.cs" Inherits="bdm_dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">Dashboard</h2>
    <span class="space15"></span>

    <%--Dashboard card boxes start--%>
    <div class="container-fluid">
        <!-- Small boxes (Stat box) -->
        <div class="row">
            <div class="col-lg-3 col-6">
                <!-- small box -->
                <%--<a href="order-reports.aspx">--%>
                <a href="customer-order-report.aspx">
                    <div class="small-box bg-gradient-primary">
                        <div class="inner">
                            <h3><%= arrCounts[0] %></h3>

                            <p>Total Orders </p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>
            <!-- ./col -->

            <div class="col-lg-3 col-6">
                <!-- small box -->
                <a href="monthwise-orders-report.aspx">
                    <div class="small-box bg-gradient-warning">
                        <div class="inner">
                            <h3><%= arrCounts[1] %></h3>

                            <p>Total Orders Amount</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>
            <!-- ./col -->
            <div class="col-lg-3 col-6">
                <!-- small box -->
                <%--<a href="order-reports.aspx?type=delivered">--%>
                <a href="customer-order-report.aspx?type=delivered">
                    <div class="small-box bg-gradient-success">
                        <div class="inner">
                            <h3><%= arrCounts[2] %></h3>

                            <p>Total Delivered Orders </p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>
            <!-- ./col -->

            <div class="col-lg-3 col-6">
                <!-- small box -->
                <div class="small-box bg-gradient-teal">
                    <div class="inner">
                        <h3><%= arrCounts[3] %></h3>

                        <p>Delivered Orders Amount</p>
                    </div>
                    <div class="icon">
                        <i class="ion ion-bag"></i>
                    </div>
                    <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                </div>
            </div>
            <!-- ./col -->


            <div class="col-lg-3 col-6">
                <!-- small box -->
                <a href="medicine-order-report-shopwise.aspx">
                    <div class="small-box bg-gradient-info">
                        <div class="inner">
                            <i class="fas fa-3x fa-store-alt"></i>
                            <span class="space10"></span>
                            <p>Shop wise Orders </p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>
            <!-- ./col -->
            <div class="col-lg-3 col-6">
                <!-- small box -->
                <a href="medicine-order-report-shopwise.aspx?type=heads">
                    <div class="small-box bg-gradient-lightblue">
                        <div class="inner">
                            <i class="fas fa-3x fa-store-alt"></i>
                            <span class="space10"></span>
                            <p>ZH / DH wise Orders </p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>
            <!-- ./col -->

            <div class="col-lg-3 col-6">
                <!-- small box -->
                <a href="medicine-order-report.aspx">
                    <div class="small-box bg-navy">
                        <div class="inner">
                            <i class="fas fa-3x fa fa-cubes"></i>
                            <span class="space10"></span>
                            <p>Projected Stock</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-pie-graph"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>
            <!-- ./col -->

            <div class="col-lg-3 col-6">
                <!-- small box -->
                <div class="small-box bg-purple">
                    <div class="inner">
                        <h3><%= arrCounts[4] %></h3>

                        <p>Registered Customers</p>
                    </div>
                    <div class="icon">
                        <i class="ion ion-person-add"></i>
                    </div>
                    <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                </div>
            </div>
            <!-- ./col -->
            <div class="col-lg-3 col-6">
                <!-- small box -->
                <a href="fav-shop-customers.aspx">
                    <div class="small-box bg-olive">
                        <div class="inner">
                            <i class="fas fa-3x fa-user-alt"></i>
                            <span class="space10"></span>
                            <p>Favourite Shop Customers </p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>
            <!-- ./col -->
            <div class="col-lg-3 col-6">
                <!-- small box -->
                <a href="order-slabs.aspx">
                    <div class="small-box bg-gradient-info posRelative">
                        <div style="position: absolute; top: 5px; right: 10px; width: 70%; text-align: right;">
                            <span class="">Zero Orders</span><br />
                            <span class="">1 - 100 Orders</span><br />
                            <span class="">Above 100 Orders</span>
                        </div>
                        <div class="inner">
                            <i class="fas fa-3x fa-shopping-cart"></i>
                            <span class="space10"></span>
                            <p>Online Orders</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>
            <!-- ./col -->
            <div class="col-lg-3 col-6">
                <!-- small box -->
                <a href="franchisee-daily-sales-report.aspx">
                    <div class="small-box bg-indigo">
                        <div class="inner">
                            <i class="fas fa-3x fa fa-chart-line"></i>
                            <span class="space5"></span>
                            <p>Daily Sales Report</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-pie-graph"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>
            <!-- ./col -->
        </div>
    </div>
    <%--  Dashboard cards end--%>
</asp:Content>

