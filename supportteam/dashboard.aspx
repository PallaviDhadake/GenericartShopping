<%@ Page Title="Dashboard | Genericart Shopping Support Team" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="dashboard.aspx.cs" Inherits="supportteam_dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
    $(document).ready(function() {
        // Call refresh page function after 30000 milliseconds (or 30 seconds).
        setInterval('refreshPage()', 30000);
    });

    function refreshPage() { 
        location.reload(); 
    }
    </script>

    <style>
        #BtnBackToList {
          display: inline-block;
          margin-left: 10px;
          float: right;
          margin-right: 10px;
        }
        
        .pgTitle {
          display: inline-block;
          margin-right: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">Dashboard</h2>
    <a id="BtnBackToList" href="callers-dashboard.aspx" class="btn btn-md btn-primary" title="Callers Overview">Callers Overview</a> &nbsp;
    <span class="space15"></span>

    <!-- To display support members -->
    <div class="container-fluid">
        <div class="row">            
            <div class="col-lg-3 col-6">
                <a href="order-report.aspx?type=new">
                    <div class="small-box bg-purple" style="height: 180px;">
                        <div class="inner">
                            <h3><%=arrFlUp[12] %></h3>
                            <p style="font-size: 1.3em;">New Order</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer" style="margin-top: 29px;">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="prescription-order-report.aspx?type=new">
                    <div class="small-box bg-purple" style="height: 180px;">
                        <div class="inner">
                            <h3><%=arrFlUp[13] %></h3>
                            <p style="font-size: 1.3em;">New Prescription Order</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer" style="margin-top: 29px;">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="order-assign-report.aspx?type=accept">
                    <div class="small-box bg-purple">
                        <div class="inner">
                            <h3><%=arrFlUp[15] %></h3>
                            <p style="font-size: 1.3em;">Order Accepted But Not Assigned</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="registered-not-orderd.aspx">
                    <div class="small-box bg-purple">
                        <div class="inner">
                            <h3><%= arrFlUp[8] %></h3>
                            <p style="font-size: 1.3em;">Registered But Not Ordered</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="saving-calc-enquiry.aspx">
                    <div class="small-box bg-purple" style="height: 180px;">
                        <div class="inner">
                            <h3><%= arrFlUp[9] %></h3>
                            <p style="font-size: 1.3em;">Enquiries</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer" style="margin-top: 29px;">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="customer-order-consistency.aspx">
                    <div class="small-box bg-purple">
                        <div class="inner">
                            <h3><%= arrFlUp[10] %></h3>
                            <p style="font-size: 1.3em;">Customer Order Consistency Report</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="reject-order-report.aspx">
                    <div class="small-box bg-purple" style="height: 180px;">
                        <div class="inner">
                            <h3><%=arrFlUp[11] %></h3>
                            <p style="font-size: 1.3em;">Reject Order Report</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer" style="margin-top: 29px;">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>            
        </div>
        <div class="row">
            <div class="col-lg-12">
                <h2 class="text-xl text-maroon">Company Own Shops</h2>
            </div>
            
            <div class="col-lg-4 col-6" id="today" runat="server" visible="true">
                <a href="customerwise-distribution.aspx?type=today&shop=own">
                    <div class="small-box bg-teal">
                        <div class="inner">
                            <h3><%= arrFlUp[5] %></h3>
                            <p style="font-size: 1.3em;">Today's Followup
                                <span style="font-size:0.7em !important; display:block;">Quarterly Followup of Day : <%= currDay %></span>
                            </p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-4 col-6" id="todistribute" runat="server" visible="false">
                <a href="fl-todays-order-report.aspx?type=today&shop=own">
                    <div class="small-box bg-teal">
                        <div class="inner">
                            <h3><%= arrFlUp[16] %></h3>
                            <p style="font-size: 1.3em;">Today's Followup
                                <span style="font-size:0.7em !important; display:block;">Quarterly Followup of Day : <%= currDay %></span>
                            </p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-4 col-6">
                <a href="followup-order-report.aspx?shop=own">
                    <div class="small-box bg-success">
                        <div class="inner">
                            <h3><%= arrFlUp[6] %></h3>
                            <p style="font-size: 1.3em;">Followup Pending Orders
                                <span style="font-size:0.7em !important; display:block;">Recent 3 months pending followup</span>
                            </p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-4 col-6">
                <a href="followup-order-report.aspx?type=inactive&shop=own">
                    <div class="small-box bg-warning">
                        <div class="inner">
                            <h3><%= arrFlUp[7] %></h3>
                            <p style="font-size: 1.3em;">Followup Inactive Customers
                                <span style="font-size:0.7em !important; display:block;">Older than 3 months pending followup</span>
                            </p>
                        
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-ban"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-12">
                <h2 class="text-xl text-maroon">Other Shops</h2>
            </div>            
            <div class="col-lg-4 col-6">
                <a href="followup-order-report.aspx?type=today&shop=other">
                    <div class="small-box bg-teal">
                        <div class="inner">
                            <h3><%= arrFlUp[3] %></h3>
                            <p style="font-size: 1.3em;">Today's Followup
                                <span style="font-size:0.7em !important; display:block;">Quarterly Followup of Day : <%= currDay %></span>
                            </p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-4 col-6">
                <a href="followup-order-report.aspx?shop=other">
                    <div class="small-box bg-success">
                        <div class="inner">
                            <h3><%= arrFlUp[0] %></h3>
                            <p style="font-size: 1.3em;">Followup Pending Orders
                                <span style="font-size:0.7em !important; display:block;">Recent 3 months pending followup</span>
                            </p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-4 col-6">
                <a href="followup-order-report.aspx?type=inactive&shop=other">
                    <div class="small-box bg-warning">
                        <div class="inner">
                            <h3><%= arrFlUp[1] %></h3>
                            <p style="font-size: 1.3em;">Followup Inactive Customers
                                <span style="font-size:0.7em !important; display:block;">Older than 3 months pending followup</span>
                            </p>
                        
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-ban"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-12">
                <h2 class="text-xl text-maroon">Refund Activities</h2>
            </div>
            <span class="space15"></span>

            <div class="col-lg-3 col-6">
                <a href="refund-request-report.aspx?type=Refund-Request">
                    <div class="small-box bg-gradient-success">
                        <div class="inner">
                            <h3><%=arrRefunds[0] %></h3>

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
                            <h3><%=arrRefunds[1] %></h3>

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
                            <h3><%=arrRefunds[2] %></h3>

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


    <div id="teamManager" runat="server">
        <%--Dashboard card boxes start--%>
        <div class="container-fluid">
            <!-- Small boxes (Stat box) -->
            <div class="row">
                <div class="col-lg-3 col-6">
                    <!-- small box -->
                    <a href="add-team-master.aspx?type=active">
                        <div class="small-box bg-gradient-info">
                            <div class="inner">
                                <h3><%= managersDataCount[0] %></h3>

                                <p>Active Staff </p>
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
                    <a href="add-team-master.aspx?type=blocked">
                        <div class="small-box bg-gradient-success">
                            <div class="inner">
                                <h3><%= managersDataCount[1] %></h3>

                                <p>Blocked Staff </p>
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
    </div>
    <div id="teamStaff" runat="server">
        <%--<div class="row">
            <div class="col-lg-3 col-6">
                <a href="staff-followup-report.aspx">
                    <div class="small-box bg-gradient-info">
                        <div class="inner">
                            <h3><%= dataCount %></h3>

                            <p><%= cardName %> </p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>
            <div class="col-lg-3 col-6">
                <div class="small-box bg-gradient-success">
                    <div class="inner">
                        <h3><%= todaysCount %></h3>

                        <p>Todays Folloup Count</p>
                        <span class="space20"></span>
                    </div>
                    <div class="icon">
                        <i class="ion ion-bag"></i>
                    </div>
                    <span class="small-box-footer">
                        <br />
                    </span>
                </div>

            </div>

        </div>--%>

    </div>

    <div id="purchaseDeptID" runat="server">
        <div class="col-lg-3 col-6">
            <!-- small box -->
            <a href="product-master.aspx">
                <div class="small-box bg-gradient-info">
                    <div class="inner">
                        <%--<h3><%= staffDataCount[0] %></h3>--%>
                        <h3><%= purchaseData[0] %></h3>

                        <p>Product Count</p>
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

    <div id="trainee" runat="server">
        <div class="col-lg-3 col-6">
            <!-- small box -->
            <a href="staff-training-videos.aspx">
                <div class="small-box bg-gradient-info">
                    <div class="inner">

                        <span class="space15"></span>

                        <p>Trainee Videos</p>
                    </div>
                    <div class="icon">
                        <i class="ion ion-bag"></i>
                    </div>
                    <%--<span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>--%>
                </div>
            </a>
        </div>
    </div>

    <div id="payment" runat="server">
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
                            <p>Payment Settlement Shopwise Report</p>
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

    </div>

    <div id="gaurang" runat="server" visible="false">
        <div class="container-fluid">
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
            </div>
        </div>
    </div>

</asp:Content>

