<%@ Page Title="Dashboard | Generic Mitra Admin Panel" Language="C#" MasterPageFile="~/genericmitra/MasterGenMitra.master" AutoEventWireup="true" CodeFile="dashboard.aspx.cs" Inherits="genericmitra_dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">Dashboard</h2>
    <span class="space15"></span>


    <div class="container-fluid">
        <!-- Small boxes (Stat box) -->
        <div class="row">
            <div class="col-lg-3 col-6">
                <!-- small box -->
                <a href="add-customer.aspx">
                    <div class="small-box bg-gradient-info">
                        <div class="inner">
                            <h3><%= arrCounts[0] %></h3>

                            <p>Total Customers</p>
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
                <a href="#">
                    <div class="small-box bg-gradient-success">
                        <div class="inner">
                            <h3><%= arrCounts[1] %></h3>

                            <p>Favourite Shops</p>
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
                <a href="order-reports.aspx">
                    <div class="small-box bg-gradient-warning">
                        <div class="inner">
                            <h3><%= arrCounts[2] %></h3>

                            <p>Total Order Amount in Rs.</p>
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
                <div class="small-box bg-gradient-primary">
                        <div class="inner">
                            <h3><%= arrCounts[3] %></h3>

                            <p>Total Comission Amount in Rs.</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <span class="small-box-footer"> -</span>
                    </div>
            </div>
            <!-- ./col -->
        </div>
    </div>
</asp:Content>

