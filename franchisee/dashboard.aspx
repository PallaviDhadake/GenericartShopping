<%@ Page Title="Dashboard | Genericart Shopping Franchisee Control Panel " Language="C#" MasterPageFile="~/franchisee/MasterFranchisee.master" AutoEventWireup="true" CodeFile="dashboard.aspx.cs" Inherits="franchisee_dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">Dashboard</h2>
    <span class="space15"></span>

    <%--Dashboard card boxes start--%>
    <div class="container-fluid">
        <!-- Small boxes (Stat box) -->


          <div class="row">
            <div class="col-lg-12">
                <h2 class="pgTitle">Generic Mitra</h2>
                <span class="space15"></span>
            </div>
            <div class="col-lg-3 col-6">
                <!-- small box -->
                <a href="https://ecommerce.genericartmedicine.com/register-genmitra?frId=<%= shopId %>" target="_blank">
                    <div class="small-box bg-olive">
                        <div class="inner">
                            <i class="fas fa-3x fa fa-user"></i>
                            <span class="space5"></span>
                            <p>Generic Mitra Enrollment</p>
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
                <a href="generic-mitra-details.aspx">
                    <div class="small-box bg-indigo">
                        <div class="inner">
                            <h3><%=arrCounts[8] %></h3>

                            <p>Total Generic Mitra</p>
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


        <hr />
        <div class="row">
            <div class="col-lg-3 col-6">
                <!-- small box -->
                <a href="orders-report.aspx?type=new">
                    <div class="small-box bg-gradient-info">
                        <div class="inner">
                            <h3><%= arrCounts[0] %></h3>

                            <p>New Orders </p>
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
                <a href="orders-report.aspx?type=rx">
                    <div class="small-box bg-gradient-purple">
                        <div class="inner">
                            <h3><%=arrCounts[5] %></h3>

                            <p>Prescription Orders</p>
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
                <a href="received-prescriptions.aspx?type=new">
                    <div class="small-box bg-olive">
                        <div class="inner">
                            <h3><%=arrCounts[7] %></h3>

                            <p>Genericart Dr/ Dr. Prax order</p>
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
                <a href="enquiry-report.aspx?type=new">
                    <div class="small-box bg-indigo">
                        <div class="inner">
                            <h3><%=arrCounts[6] %></h3>

                            <p>New Enquiries</p>
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
                <a href="orders-report.aspx?type=accepted">
                    <div class="small-box bg-gradient-success">
                        <div class="inner">
                            <h3><%=arrCounts[1] %></h3>

                            <p>Accepted &amp; Yet to Dispatch orders</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-stats-bars"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>
            <!-- ./col -->
            <div class="col-lg-3 col-6">
                <!-- small box -->
                <a href="orders-report.aspx?type=rejected">
                    <div class="small-box bg-gradient-warning">
                        <div class="inner">
                            <h3><%=arrCounts[2] %></h3>

                            <p>Rejected  Order</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-person-add"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>
            <!-- ./col -->
            <div class="col-lg-3 col-6">
                <!-- small box -->
                <a href="orders-report.aspx?type=dispatched">
                    <div class="small-box bg-gradient-danger">
                        <div class="inner">
                            <h3><%=arrCounts[3] %></h3>

                            <p>Dispatched Order</p>
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
                <a href="orders-report.aspx?type=delivered">
                    <div class="small-box bg-gradient-success">
                        <div class="inner">
                            <h3><%=arrCounts[4] %></h3>

                            <p>Delivered Order</p>
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
                <a href="orders-report.aspx?type=returned">
                    <div class="small-box bg-gradient-danger">
                        <div class="inner">
                            <h3><%=arrCounts[9] %></h3>

                            <p>Returned Order</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-pie-graph"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>
            <!-- ./col -->

            <div class="col-lg-3 col-6" id="bluedart" runat="server" visible="false">
                <!-- small box -->
                <a href="bluedart-waybills.aspx">
                    <div class="small-box bg-gradient-info">
                        <div class="inner">
                            <i class="fas fa-3x fa-shipping-fast"></i>
                            <span class="space5"></span>
                            <p>Bluedart Bookings</p>
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

        <div class="row">
            <div class="col-lg-3 col-6">
                <!-- small box -->
                <a href="shopwise-order-rating.aspx">
                    <div class="small-box bg-orange">
                        <div class="inner">
                            <i class="fas fa-3x fa fa-star-half-alt"></i>
                            <span class="space5"></span>
                            <p>Your Shop Rating</p>
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
                <a href="medicine-order-report.aspx">
                    <div class="small-box bg-navy">
                        <div class="inner">
                            <i class="fas fa-3x fa fa-cubes"></i>
                            <span class="space5"></span>
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
                <a href="orders-report.aspx?type=monthly">
                    <div class="small-box bg-teal">
                        <div class="inner">
                            <i class="fas fa-3x fa fa-shopping-cart"></i>
                            <span class="space5"></span>
                            <p>Monthly Orders</p>
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
                <a href="monthly-order-followup.aspx">
                    <div class="small-box bg-primary">
                        <div class="inner">
                            <i class="fas fa-3x fa fa-shopping-cart"></i>
                            <span class="space5"></span>
                            <p>Monthly Clients Followup</p>
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

