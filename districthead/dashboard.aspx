<%@ Page Title="Dashboard | Genericart Shopping District Head Control Panel" Language="C#" MasterPageFile="~/districthead/MasterDH.master" AutoEventWireup="true" CodeFile="dashboard.aspx.cs" Inherits="districthead_dashboard" %>

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
                <a href="fav-shop-customers.aspx">
                    <div class="small-box bg-gradient-blue">
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
                <a href="registered-gobp.aspx">
                    <div class="small-box bg-gradient-purple">
                        <div class="inner">
                            <h3><%=arrCounts[0] %></h3>

                            <p>Total GOBP</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>        

        </div>
    </div>
    <%--  Dashboard cards end--%>
</asp:Content>

