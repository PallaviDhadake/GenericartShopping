<%@ Page Title="Dashboard | Genericart Shopping Account Control Panel" Language="C#" MasterPageFile="~/account/MasterAccount.master" AutoEventWireup="true" CodeFile="dashboard.aspx.cs" Inherits="account_dashboard" %>

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
                            <p>Payment Settlement Report Shopwise</p>
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
                <a href="payment-settlement-report-daywaise.aspx">
                    <div class="small-box bg-gradient-teal">
                        <div class="inner">
                            <i class="fas fa-3x fa-cash-register"></i>
                            <span class="space10"></span>
                            <p>Payment Settlement Daywise Report</p>
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

        <div class="row">
            <div class="col-lg-3 col-6">
                <a href="order_paid_report.aspx?type=Accepted">
                    <div class="small-box bg-gradient-success">
                        <div class="inner">
                            <h3><%=arrCounts[0] %></h3>

                            <p>Accepted &amp; Yet to Dispatch Order</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-stats-bars"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="order_paid_report.aspx?type=Inprocess">
                    <div class="small-box bg-gradient-warning">
                        <div class="inner">
                            <h3><%=arrCounts[1] %></h3>

                            <p>Inproccess Order</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-stats-bars"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="order_paid_report.aspx?type=Dispatched">
                    <div class="small-box bg-gradient-danger">
                        <div class="inner">
                            <h3><%=arrCounts[2] %></h3>

                            <p>Dispatched Order</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-pie-graph"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="order_paid_report.aspx?type=Delivered">
                    <div class="small-box bg-gradient-info">
                        <div class="inner">
                            <h3><%=arrCounts[3] %></h3>

                            <p>Delivered Order</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-pie-graph"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>        
            <span class="space20"></span>
        </div>

        <div class="row">
            <div class="col-lg-12">
                <h2 class="text-xl text-maroon">Refund Activities</h2>
            </div>
            <span class="space30"></span>

            <div class="col-lg-3 col-6">
                <a href="refund-request-report.aspx?type=Refund-Request">
                    <div class="small-box bg-gradient-success">
                        <div class="inner">
                            <h3><%=arrCounts[4] %></h3>

                            <p>Refund Request By Customer</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-pie-graph"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="refund-request-report.aspx?type=Request-Inprocess">
                    <div class="small-box bg-gradient-warning">
                        <div class="inner">
                            <h3><%=arrCounts[5] %></h3>

                            <p>Refund Request Under Process</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-pie-graph"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="refund-request-report.aspx?type=Request-Completed">
                    <div class="small-box bg-gradient-danger">
                        <div class="inner">
                            <h3><%=arrCounts[6] %></h3>

                            <p>Refund Completed</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-pie-graph"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>
        </div>
    </div>
    <%--  Dashboard cards end--%>
</asp:Content>

