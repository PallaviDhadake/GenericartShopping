<%@ Page Title="Dashboard | Generic Mitra Admin Panel" Language="C#" MasterPageFile="~/obpmanager/MasterObpManager.master" AutoEventWireup="true" CodeFile="dashboard.aspx.cs" Inherits="obpmanager_dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">Dashboard</h2>
    <span class="space15"></span>


    <div class="container-fluid">
        <!-- Small boxes (Stat box) -->
        <div class="row">
            <div class="col-lg-12">
                <h2 class="text-xl text-maroon">Overall (Customer Wise)</h2>
            </div>
            <span class="space20"></span>

            <div class="col-lg-3 col-6">
                <!-- small box -->
                <a href="registered-gobp.aspx">
                    <div class="small-box bg-gradient-info">
                        <div class="inner">
                            <h3><%= arrCounts[0] %></h3>

                            <p>Total GOBP Registrations</p>
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
                <a href="registered-gobp.aspx?type=active">
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
                <a href="registered-gobp.aspx?type=inactive">
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
        </div>

        <span class="space20"></span>
        <div class="row">
            <div class="col-lg-12">
                <h2 class="text-xl text-maroon">Overall (Business Wise)</h2>
            </div>
            <span class="space20"></span>

            <div class="col-lg-3 col-6">
                <a href="gobp-orders.aspx?type=active">
                    <div class="small-box bg-gradient-danger">
                        <div class="inner">
                            <h3><%=arrCounts[6] %></h3>

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
                <a href="gobp-incentive-current-month.aspx?type=active">
                    <div class="small-box bg-fuchsia">
                        <div class="inner">
                            <h3><%=arrCounts[7] %></h3>

                            <p>Active GOBP Incentive</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="gobp-orders.aspx?type=inactive">
                    <div class="small-box bg-gradient-orange">
                        <div class="inner">
                            <h3><%=arrCounts[8] %></h3>

                            <p>Inactive GOBP</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>
        </div>

        <span class="space20"></span>
        <div class="row">
            <div class="col-lg-12">
                <h2 class="text-xl text-maroon">Current Month (Customer Wise)</h2>
            </div>
            <span class="space20"></span>

            <div class="col-lg-3 col-6">
                <a href="registered-current-month-gobp.aspx?type=thismonth">
                    <div class="small-box bg-gradient-info">
                        <div class="inner">
                            <h3><%=arrCounts[3] %></h3>

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
                <a href="registered-current-month-gobp.aspx?type=thismonthactive">
                    <div class="small-box bg-gradient-purple">
                        <div class="inner">
                            <h3><%=arrCounts[4] %></h3>

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
                <a href="registered-current-month-gobp.aspx?type=thismonthinactive">
                    <div class="small-box bg-gradient-green">
                        <div class="inner">
                            <h3><%=arrCounts[5] %></h3>

                            <p>Inactive GOBP</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>
        </div>

        <span class="space20"></span>
        <div class="row">
            <div class="col-lg-12">
                <h2 class="text-xl text-maroon">Current Month (Business Wise)</h2>
            </div>
            <span class="space20"></span>

            <div class="col-lg-3 col-6">
                <a href="gobp-current-month-order.aspx?type=active">
                    <div class="small-box bg-gradient-danger">
                        <div class="inner">
                            <h3><%=arrCounts[9] %></h3>

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
                <a href="gobp-incentive-current-month.aspx?type=inactive">
                    <div class="small-box bg-fuchsia">
                        <div class="inner">
                            <h3><%=arrCounts[10] %></h3>

                            <p>Active GOBP Incentives</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="gobp-current-month-order.aspx?type=inactive">
                    <div class="small-box bg-gradient-orange">
                        <div class="inner">
                            <h3><%=arrCounts[11] %></h3>

                            <p>Inactive GOBP</p>
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
</asp:Content>

