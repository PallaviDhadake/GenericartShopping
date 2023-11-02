<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/official/MasterOfficial.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="official_Dashboard" %>
<%@ MasterType VirtualPath="~/official/MasterOfficial.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <h2 class="pgTitle">Dashboard</h2>
    <span class="space15"></span>
     <%--Dashboard card boxes start--%>
    <div class="container-fluid">
        <!-- Small boxes (Stat box) -->
        <div class="row">
            <div class="col-lg-3 col-6">
                <div class="small-box bg-purple">
                    <div class="inner">
                        <h3><%=arrCounts[0]%></h3>
                        <p>New GOBP Registration</p>
                    </div>
                    <div class="icon">
                        <i class="ion ion-bag"></i>

                    </div>
                    <a href="gobp-registration-master.aspx?type=new" class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i> </a>
                </div>
            </div>
            <!-- ./col -->

            <div class="col-lg-3 col-6">
                <div class="small-box bg-yellow">
                    <div class="inner">
                        <h3><%=arrCounts[1]%></h3>
                        <p>Active GOBP </p>
                    </div>
                    <div class="icon">
                        <i class="ion ion-bag"></i>

                    </div>
                    <a href="gobp-registration-master.aspx?type=active" class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i> </a>
                </div>
            </div>
            <!-- ./col -->

            
        </div>
    </div>
    <%--  Dashboard cards end--%>
</asp:Content>

