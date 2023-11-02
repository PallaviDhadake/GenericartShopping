<%@ Page Title="Dashboard | Generic Mitra Admin Panel" Language="C#" MasterPageFile="~/obp/MasterObp.master" AutoEventWireup="true" CodeFile="dashboard.aspx.cs" Inherits="obp_dashboard" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        function docReminderPopup() {
            // Show the modal on page load
            $('#modal-sm').modal('show');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">Dashboard</h2>
    <span class="space15"></span>


    <div class="container-fluid">
        <!-- Small boxes (Stat box) -->
        <div class="row">
            <div class="col-lg-3 col-6">
                <!-- small box -->
                <a href="add-customer.aspx">
                    <div class="small-box bg-gradient-info">
                        <div class="inner">
                            <h3><%= arrCounts[0] %></h3>

                            <p>Total Customers</p>
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
                <a href="#">
                    <div class="small-box bg-gradient-success">
                        <div class="inner">
                            <h3><%= arrCounts[1] %></h3>

                            <p>Favourite Shops</p>
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
                <a href="order-reports.aspx">
                    <div class="small-box bg-gradient-warning">
                        <div class="inner">
                            <h3><%= arrCounts[2] %></h3>

                            <p>Total Order Amount in Rs.</p>
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
                <div class="small-box bg-gradient-primary">
                        <div class="inner">
                            <h3><%= arrCounts[3] %></h3>
                            <p>Total Comission Amount in Rs.</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <span class="small-box-footer"> -</span>
                    </div>
            </div>
            <!-- ./col -->
        </div>
        <div class="row">
            <!-- Graph starts -->
            <h2 class="large">Commission Performance in Current Financial Year</h2>
                <span class="space20"></span>
                <asp:Chart ID="chartPerform" runat="server" Width="800">
                    <Series>
                        <asp:Series Name="Total Comission" IsValueShownAsLabel="true" LabelBackColor="139, 199, 62" Color="0, 175, 239"></asp:Series>
                    </Series>
                    <Legends>
                        <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="False" Name="Default"
                            LegendStyle="Row" />
                    </Legends>
                    <ChartAreas>
                        <asp:ChartArea Name="Month"></asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
        </div>

        <!-- Document Modal popup starts -->
        <div class="modal fade" id="modal-sm">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Document(s) Incomplete Reminder</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="callout callout-danger">
                        <p>
                            Dear OBP, We found that your registration document(s) are incomplete. Their fullfillent is mandatory. We can not 
                            proceed for your commission payment, until you submit your document(s).
                        </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- Document Modal popup ends -->
    </div>
</asp:Content>

