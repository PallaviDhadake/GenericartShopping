<%@ Page Title="" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="callers-dashboard.aspx.cs" Inherits="supportteam_callers_dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        $(document).ready(function () {
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
    <h2 class="pgTitle">Callers Dashboard</h2>
    <a id="BtnBackToList" href="dashboard.aspx" class="btn btn-md btn-primary" title="Dashboard">Back To Dashboard</a> &nbsp;
    <span class="space15"></span>

    <div class="container-fluid">
        <div class="row">
            <div class="col-lg-12">
                <h2 class="text-xl text-maroon">Todays Calls</h2>
            </div>
            <span class="space20"></span>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-purple">
                        <div class="inner">
                            <h3><%= arrFlup[23] %></h3>
                            <p style="font-size: 1.3em;">Total Calls</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-gradient-blue">
                        <div class="inner">
                            <h3><%= arrFlup[23] %></h3>
                            <p style="font-size: 1.3em;">Fresh Calls</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-gradient-green">
                        <div class="inner">
                            <h3><%= arrFlup[21] %></h3>
                            <p style="font-size: 1.3em;">Followup Calls</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-gradient-danger">
                        <div class="inner">
                            <h3><%= arrFlup[22] %></h3>
                            <p style="font-size: 1.3em;">Enquiry Calls</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <%--<div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-cyan">
                        <div class="inner">
                            <h3><%= arrFlUp[4] %></h3>
                            <p style="font-size: 1.3em;">Registered But Not Ordered</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>--%>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-danger">
                        <div class="inner">
                            <h3><%= arrFlup[24] %></h3>
                            <p style="font-size: 1.3em;">Order Amount</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-fuchsia">
                        <div class="inner">
                            <h3><%= arrFlup[25] %></h3>
                            <p style="font-size: 1.3em;">Converted Clients</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <%--<div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-cyan">
                        <div class="inner">
                            <h3><%= arrFlUp[4] %></h3>
                            <p style="font-size: 1.3em;">New Clients</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>--%>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-gradient-orange">
                        <div class="inner">
                            <h3 style="color: white"><%= arrFlup[26] %></h3>
                            <p style="font-size: 1.3em;color: white">Order Count</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>
        </div>

        <span class="space30"></span>
        <div class="row">
            <div class="col-lg-12">
                <h2 class="text-xl text-maroon">Monthly Calls</h2>
            </div>
            <span class="space20"></span>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-purple">
                        <div class="inner">
                            <h3><%= arrFlup[17] %></h3>
                            <p style="font-size: 1.3em;">Total Calls</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-gradient-blue">
                        <div class="inner">
                            <h3><%= arrFlup[17] %></h3>
                            <p style="font-size: 1.3em;">Fresh Calls</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-gradient-green">
                        <div class="inner">
                            <h3><%= arrFlup[15] %></h3>
                            <p style="font-size: 1.3em;">Followup Calls</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-gradient-danger">
                        <div class="inner">
                            <h3><%= arrFlup[16] %></h3>
                            <p style="font-size: 1.3em;">Enquiry Calls</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <%--<div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-cyan">
                        <div class="inner">
                            <h3><%= arrFlUp[4] %></h3>
                            <p style="font-size: 1.3em;">Registered But Not Ordered</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>--%>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-danger">
                        <div class="inner">
                            <h3><%= arrFlup[18] %></h3>
                            <p style="font-size: 1.3em;">Order Amount</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-fuchsia">
                        <div class="inner">
                            <h3><%= arrFlup[19] %></h3>
                            <p style="font-size: 1.3em;">Converted Clients</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <%--<div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-cyan">
                        <div class="inner">
                            <h3><%= arrFlUp[4] %></h3>
                            <p style="font-size: 1.3em;">New Clients</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>--%>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-gradient-orange">
                        <div class="inner">
                            <h3 style="color: white"><%= arrFlup[20] %></h3>
                            <p style="font-size: 1.3em; color: white">Order Count</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>
        </div>

        <span class="space30"></span>
        <div class="row">
            <div class="col-lg-12">
                <h2 class="text-xl text-maroon">Overall Total Calls Till Now</h2>
            </div>
            <span class="space20"></span>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-purple">
                        <div class="inner">
                            <h3><%= arrFlup[2] %></h3>
                            <p style="font-size: 1.3em;">Total Calls</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-gradient-blue">
                        <div class="inner">
                            <h3><%= arrFlup[5] %></h3>
                            <p style="font-size: 1.3em;">Fresh Calls</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-gradient-green">
                        <div class="inner">
                            <h3><%= arrFlup[0] %></h3>
                            <p style="font-size: 1.3em;">Followup Calls</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-gradient-danger">
                        <div class="inner">
                            <h3><%= arrFlup[1] %></h3>
                            <p style="font-size: 1.3em;">Enquiry Calls</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <%--<div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-cyan">
                        <div class="inner">
                            <h3><%= arrFlUp[4] %></h3>
                            <p style="font-size: 1.3em;">Registered But Not Ordered</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>--%>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-danger">
                        <div class="inner">
                            <h3><%= arrFlup[6] %></h3>
                            <p style="font-size: 1.3em;">Order Amount</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-fuchsia">
                        <div class="inner">
                            <h3><%= arrFlup[7] %></h3>
                            <p style="font-size: 1.3em;">Converted Clients</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>

            <%--<div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-cyan">
                        <div class="inner">
                            <h3><%= arrFlUp[4] %></h3>
                            <p style="font-size: 1.3em;">New Clients</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>--%>

            <div class="col-lg-3 col-6">
                <a href="#">
                    <div class="small-box bg-gradient-orange">
                        <div class="inner">
                            <h3 style="color: white"><%= arrFlup[8] %></h3>
                            <p style="font-size: 1.3em;color: white">Order Count</p>
                        </div>
                        <div class="icon">
                            <i class="fa fas fa-tasks"></i>
                        </div>
                        <span class="small-box-footer">More info <i class="fas fa-arrow-circle-right"></i></span>
                    </div>
                </a>
            </div>
        </div>
    </div>
</asp:Content>

