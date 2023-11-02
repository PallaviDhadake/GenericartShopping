﻿<%@ Page Title="" Language="C#" MasterPageFile="~/obpmanager/MasterObpManager.master" AutoEventWireup="true" CodeFile="monthly-dashboard.aspx.cs" Inherits="obpmanager_monthly_dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="proNavPanel">
		<ul class="proNav">
			<li><a class="act">Monthly</a></li>
			<li><a href="yearly-dashboard.aspx">Yearly</a></li>
            <li><a href="overall-dashboard.aspx">Overall</a></li>
		</ul>
	</div>
	<span class="space20"></span>

    <div class="container-fluid">
        <div class="row">
            <div class="col-lg-12">
                <h2 class="pgTitle" style="color: royalblue; font-size: 20px;"><b>New GOBP's</b> <span style="color: black">(GOBP Registered in this month)</span></h2>
            </div>
            <span class="space20"></span>

            <div class="col-lg-3 col-6">
                <a href="registered-current-month-gobp.aspx?gobpregtype=newgobpmonth&type=thismonth">
                    <div class="small-box bg-gradient-info">
                        <div class="inner">
                            <h3><%= arrCounts[0] %></h3>

                            <p>Total GOBP</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="registered-current-month-gobp.aspx?gobpregtype=newgobpmonth&type=thismonthactive">
                    <div class="small-box bg-gradient-info">
                        <div class="inner">
                            <h3><%= arrCounts[1] %></h3>

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
                <a href="registered-current-month-gobp.aspx?gobpregtype=newgobpmonth&type=thismonthinactive">
                    <div class="small-box bg-gradient-info">
                        <div class="inner">
                            <h3><%= arrCounts[2] %></h3>

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

        <div class="row">
            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-gradient-info">
                        <div class="inner">
                            <h3><%= arrCounts[3] %></h3>

                            <p>Total Sales</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <%--<span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>--%>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-gradient-info">
                        <div class="inner">
                            <h3><%= arrCounts[4] %></h3>

                            <p>Order Count</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <%--<span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>--%>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-gradient-info">
                        <div class="inner">
                            <h3><%= arrCounts[5] %></h3>

                            <p>Avg. Prescription</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <%--<span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>--%>
                    </div>
                </a>
            </div>
        </div>

        <span class="space30"></span>

        <div class="row">
            <div class="col-lg-12">
                <h2 class="pgTitle" style="color: rebeccapurple; font-size: 20px;"><b>Existing GOBP's</b> <span style="color: black">(GOBP Registered before this month)</span></h2>
            </div>
            <span class="space20"></span>

            <div class="col-lg-3 col-6">
                <a href="registered-gobp.aspx?gobpregtype=existgobpmonth&type=existgobpmonth">
                    <div class="small-box bg-gradient-purple">
                        <div class="inner">
                            <h3><%= arrCounts[6] %></h3>

                            <p>Total GOBP</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="registered-gobp.aspx?gobpregtype=existgobpmonth&type=existgobpactive">
                    <div class="small-box bg-gradient-purple">
                        <div class="inner">
                            <h3><%= arrCounts[7] %></h3>

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
                <a href="registered-gobp.aspx?gobpregtype=existgobpmonth&type=existgobpinactive">
                    <div class="small-box bg-gradient-purple">
                        <div class="inner">
                            <h3><%= arrCounts[8] %></h3>

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

        <div class="row">
            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-gradient-purple">
                        <div class="inner">
                            <h3><%= arrCounts[9] %></h3>

                            <p>Total Sales</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <%--<span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>--%>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-gradient-purple">
                        <div class="inner">
                            <h3><%= arrCounts[10] %></h3>

                            <p>Order Count</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <%--<span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>--%>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-gradient-purple">
                        <div class="inner">
                            <h3><%= arrCounts[11] %></h3>

                            <p>Avg. Prescription</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <%--<span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>--%>
                    </div>
                </a>
            </div>
        </div>
    </div>
</asp:Content>

