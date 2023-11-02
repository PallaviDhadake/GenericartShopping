<%@ Page Language="C#" MasterPageFile="~/Member/MemberMain.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .hidden {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="breadcrumbs">
        <div class="col-sm-4">
            <div class="page-header float-left">
                <div class="page-title">
                    <h1>Dashboard</h1>
                </div>
            </div>
        </div>
        <div class="col-sm-8">
            <div class="page-header float-right">
                <div class="page-title">
                    <ol class="breadcrumb text-right">
                        <li class="active">Dashboard</li>
                    </ol>
                </div>
            </div>
        </div>
    </div>
    <div class="content mt-3">
        <div class="col-sm-12">
            <!-- PAGE CONTENT BEGINS -->
            <div class="alert alert-block alert-success">
                <button type="button" class="close" data-dismiss="alert">
                    <i class="ace-icon fa fa-times"></i>
                </button>
                <%--<i class="ace-icon fa fa-check green"></i>--%>
                    Welcome to User Panel <%--- <strong class="green">Ahart Marketing Services Pvt Ltd<small></small></strong>--%>
            </div>
            <div class="row hidden">
                <div class="space-6"></div>

                <div class="col-xl-3 col-lg-6">
                    <div class="card">
                        <div class="card-body">
                            <div class="stat-widget-one">
                                <div class="stat-icon dib"><i class="ti-money text-success border-success"></i></div>
                                <div class="stat-content dib">
                                    <div class="stat-text">Total Amount</div>
                                    <div class="stat-digit"><%= MemberAmount %></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xl-3 col-lg-6">
                    <div class="card">
                        <div class="card-body">
                            <div class="stat-widget-one">
                                <div class="stat-icon dib"><i class="ti-user text-primary border-primary"></i></div>
                                <div class="stat-content dib">
                                    <div class="stat-text">Left Member</div>
                                    <div class="stat-digit"><%= LeftMember %></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xl-3 col-lg-6">
                    <div class="card">
                        <div class="card-body">
                            <div class="stat-widget-one">
                                <div class="stat-icon dib"><i class="ti-layout-grid2 text-primary border-primary"></i></div>
                                <div class="stat-content dib">
                                    <div class="stat-text">Right Member</div>
                                    <div class="stat-digit"><%= RightMember %></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xl-3 col-lg-6">
                    <div class="card">
                        <div class="card-body">
                            <div class="stat-widget-one">
                                <div class="stat-icon dib"><i class="ti-money text-success border-success"></i></div>
                                <div class="stat-content dib">
                                    <div class="stat-text">Binary Income</div>
                                    <div class="stat-digit"><%= BinaryIncome.ToString("0.00") %></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xl-3 col-lg-6">
                    <div class="card">
                        <div class="card-body">
                            <div class="stat-widget-one">
                                <div class="stat-icon dib"><i class="ti-money text-success border-success"></i></div>
                                <div class="stat-content dib">
                                    <div class="stat-text">Sponcership Income</div>
                                    <div class="stat-digit"><%= SponcershipIncome.ToString("0.00") %></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xl-3 col-lg-6">
                    <div class="card">
                        <div class="card-body">
                            <div class="stat-widget-one">
                                <div class="stat-icon dib"><i class="ti-money text-success border-success"></i></div>
                                <div class="stat-content dib">
                                    <div class="stat-text">Login Income</div>
                                    <div class="stat-digit"><%= LoginIncome.ToString("0.00") %></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xl-3 col-lg-6">
                    <div class="card">
                        <div class="card-body">
                            <div class="stat-widget-one">
                                <div class="stat-icon dib"><i class="ti-money text-success border-success"></i></div>
                                <div class="stat-content dib">
                                    <div class="stat-text">ROI Income Generated</div>
                                    <div class="stat-digit"><%= ROIIncomeGenerated.ToString("0.00") %></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xl-3 col-lg-6">
                    <div class="card">
                        <div class="card-body">
                            <div class="stat-widget-one">
                                <div class="stat-icon dib"><i class="ti-money text-success border-success"></i></div>
                                <div class="stat-content dib">
                                    <div class="stat-text">ROI Income Earned</div>
                                    <div class="stat-digit"><%= ROIIncomeEarned.ToString("0.00") %></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xl-3 col-lg-6">
                    <div class="card">
                        <div class="card-body">
                            <div class="stat-widget-one">
                                <div class="stat-icon dib"><i class="ti-money text-success border-success"></i></div>
                                <div class="stat-content dib">
                                    <div class="stat-text">FRC Income</div>
                                    <div class="stat-digit"><%= FRCIncome.ToString("0.00") %></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xl-3 col-lg-6">
                    <div class="card">
                        <div class="card-body">
                            <div class="stat-widget-one">
                                <div class="stat-icon dib"><i class="ti-money text-danger border-danger"></i></div>
                                <div class="stat-content dib">
                                    <div class="stat-text">Admin Charge</div>
                                    <div class="stat-digit"><%= AdminCharge %></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xl-3 col-lg-6">
                    <div class="card">
                        <div class="card-body">
                            <div class="stat-widget-one">
                                <div class="stat-icon dib"><i class="ti-money text-danger border-danger"></i></div>
                                <div class="stat-content dib">
                                    <div class="stat-text">TDS</div>
                                    <div class="stat-digit"><%= TDS %></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="hr hr32 hr-dotted"></div>
        </div>
        <section class="content" style="overflow: hidden;">
    <div class="row"><section class="col-lg-12 ">
    <div class="row">
    <section class="col-lg-12 ">
    <div class="box box-info">
            <div class="box-header with-border">
              <h3 class="box-title">My Details</h3>
            </div>
            <div class="box-body">
              <div class="table-responsive">
                <table class="table no-margin">
                  <thead>
                    <tr>
                     <th>User ID</th>  
                      <th>Registration Date</th>
                      <th>ID Status</th>
                      <th>Activation Date</th>
                   
                      </tr>
                  </thead>
                  <tbody>
                    <tr>
                      <td><%= MemberId %></td>

                      <td><%= EntryDate %></td>

                      <td><%= StatusShow %></td>

                      <td><%= DOJ %></td>

                    </tr>
                  </tbody>
                </table>
              </div>
            </div>
            </div>
            </section>
    </div>
    </section></div>
    </section>
</asp:Content>
