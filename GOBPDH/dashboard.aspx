<%@ Page Title="" Language="C#" MasterPageFile="~/GOBPDH/MasterGOBPDH.master" AutoEventWireup="true" CodeFile="dashboard.aspx.cs" Inherits="GOBPDH_dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Dashboard</h2>
    <span class="space15"></span>

    <div class="container-fluid">
        <div class="row">
            <div class="col-lg-3 col-6">
                <a href="registered-gobp.aspx">
                    <div class="small-box bg-gradient-info">
                        <div class="inner">
                            <h3><%=arrCounts[0] %> </h3>

                            <p>Total GOBP Registration</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>
            
            <div class="col-lg-3 col-6">
                <a href="registered-gobp.aspx?type=financial">
                    <div class="small-box bg-blue">
                        <div class="inner">
                            <h3><%=arrCounts[3] %></h3>

                            <p>Financial Year Registration</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-gradient-olive">
                        <div class="inner">
                            <h3><%=arrCounts[4] %></h3>

                            <p>State Wise GOBP Report</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-3 col-6">
                <a href="order-report.aspx">
                    <div class="small-box bg-gradient-fuchsia">
                        <div class="inner">
                            <h3><%=arrCounts[13] %></h3>

                            <p>Total Order</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="order-report.aspx?type=month">
                    <div class="small-box bg-gradient-indigo">
                        <div class="inner">
                            <h3><%=arrCounts[14] %></h3>

                            <p>This Month Order</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="order-report.aspx?type=financial">
                    <div class="small-box bg-gradient-danger">
                        <div class="inner">
                            <h3><%=arrCounts[15] %></h3>

                            <p>Finance. Year Order</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-3 col-6">
                <a href="gobp-orders.aspx?type=active">
                    <div class="small-box bg-gradient-purple">
                        <div class="inner">
                            <h3><%=arrCounts[1] %></h3>

                            <p>Active GOBP</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-gradient-green">
                        <div class="inner">
                            <h3><%=arrCounts[2] %></h3>

                            <p>Inactive GOBP</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="gobp-orders.aspx?type=thismonth">
                    <div class="small-box bg-gradient-orange">
                        <div class="inner">
                            <h3 style="color: white"><%=arrCounts[5] %></h3>

                            <p style="color: white">Active GOBP this Month</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="#">
                        <div class="small-box bg-gradient-pink">
                            <div class="inner">
                                <h3><%=arrCounts[6] %></h3>

                                <p>Top 50 GOBP</p>
                            </div>
                            <div class="icon">
                                <i class="ion ion-bag"></i>
                            </div>
                            <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                        </div>
                    </div>
                </a>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-12">
                <h2 class="text-xl text-maroon">Sales Generated</h2>
            </div>
            <span class="space20"></span>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-gradient-green">
                        <div class="inner">
                            <h3><%=arrCounts[7] %></h3>

                            <p>Yesterday's Sales</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>                        
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-gradient-green">
                        <div class="inner">
                            <h3><%=arrCounts[8] %></h3>

                            <p>This Month Sales</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>                        
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-gradient-green">
                        <div class="inner">
                            <h3><%=arrCounts[9] %></h3>

                            <p>Finance. Year Sales</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>                        
                    </div>
                </a>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-12">
                <h2 class="text-xl text-maroon">Payout Generated</h2>
            </div>
            <span class="space20"></span>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-gradient-info">
                        <div class="inner">
                            <h3><%=arrCounts[10] %></h3>

                            <p>Yesterday's Payout</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>                        
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-gradient-info">
                        <div class="inner">
                            <h3><%=arrCounts[11] %></h3>

                            <p>This Month Payout</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>                        
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-gradient-info">
                        <div class="inner">
                            <h3><%=arrCounts[12] %></h3>

                            <p>Finance. Year Payout</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>                        
                    </div>
                </a>
            </div>
        </div>
    </div>
</asp:Content>

