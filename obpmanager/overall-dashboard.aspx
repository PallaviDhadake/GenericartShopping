<%@ Page Title="" Language="C#" MasterPageFile="~/obpmanager/MasterObpManager.master" AutoEventWireup="true" CodeFile="overall-dashboard.aspx.cs" Inherits="obpmanager_overall_dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="proNavPanel">
		<ul class="proNav">
			<li><a href="monthly-dashboard.aspx">Monthly</a></li>
			<li><a href="yearly-dashboard.aspx">Yearly</a></li>
            <li><a class="act">Overall</a></li>
		</ul>
	</div>
	<span class="space20"></span>

    <div class="container-fluid">
        <div class="row">
            <div class="col-lg-12">
                <h2 class="pgTitle" style="color: royalblue; font-size: 20px;"><b>Overall GOBP's</b></h2>
            </div>
            <span class="space20"></span>

            <div class="col-lg-3 col-6">
                <a href="registered-gobp.aspx?gobpregtype=overall">
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
                <a href="registered-gobp.aspx?gobpregtype=overall&type=active">
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
                <a href="registered-gobp.aspx?gobpregtype=overall&type=inactive">
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
    </div>
</asp:Content>

