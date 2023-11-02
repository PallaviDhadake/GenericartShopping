<%@ Page Title="Dashboard | Genericart Shopping Purchase Control Panel" Language="C#" MasterPageFile="~/distributor/MasterDistributor.master" AutoEventWireup="true" CodeFile="dashboard.aspx.cs" Inherits="distributor_dashboard" %>

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
                <a href="online-payment-report.aspx">
                    <div class="small-box bg-gradient-primary">
                        <div class="inner">
                            <i class="fas fa-3x fa-shopping-cart"></i>
                            <span class="space10"></span>
                            <p>Online Orders Payment Report </p>
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
                <a href="online-payment-report-shopwise-detail.aspx">
                    <div class="small-box bg-gradient-purple">
                        <div class="inner">
                            <i class="fas fa-3x fa-cash-register"></i>
                            <span class="space10"></span>
                            <p>Payment Settlement </p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
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

