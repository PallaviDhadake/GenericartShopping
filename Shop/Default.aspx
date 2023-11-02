<%@ Page Language="C#" MasterPageFile="~/Shop/ShopMain.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .chat .item > .message {
            margin-top: 0px;
        }

        .slimScrollDiv {
            height: 432px !important;
        }

        .chat .item > .message > .name {
            color: #101010 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-header">
      <h1>
        Dashboard
        <small>Shop panel</small>
      </h1>
      <ol class="breadcrumb">
        <li><a href="Default.aspx"><i class="fa fa-dashboard"></i> Home</a></li>
        
      </ol>
    </section>
    <!-- Main content -->
    <section class="content">
    
      <div class="row">
       
        <section class=" col-md-offset-3 col-lg-6 connectedSortable ui-sortable">

       
                    <div class="box box-warning box-solid">
                        <div class="box-header with-border ui-sortable-handle" style="cursor: move;">
                            <h3 class="box-title">My Profile</h3>
                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                <!--<button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>-->
                            </div>
                        </div>
                        <div class="box-footer no-padding">
                            <ul class="nav nav-stacked">
                                <li><a href="">Shop ID <span class="pull-right badge bg-purple">
                                    <span id="ContentPlaceHolder1_userCodeSpan"><%= ShopId %></span></span></a></li>
                                <li><a href="">Name <span class="pull-right badge bg-green">
                                    <span id="ContentPlaceHolder1_nameSpan"><%= MemberName %></span></span></a></li>
                                <li><a href="">Mobile No <span class="pull-right badge bg-red">
                                    <span id="ContentPlaceHolder1_mobileSpan"><%= MobileNo %></span></span></a></li>
                               <li><a href="">Email Id <span class="pull-right badge bg-orange">
                                    <span id="ContentPlaceHolder1_emailSpan"><%= EmailId %></span></span></a></li>
                                <li><a href="">joining Date <span class="pull-right badge bg-aqua">
                                    <span id="ContentPlaceHolder1_joiningDateSpan"><%= JoiningDate %> </span></span></a></li>
                            </ul>
                        </div>
                    </div>
        </section>
          
        
      </div>
            <div style="padding: 10px 0px; text-align: center;"></div>
</asp:Content>
