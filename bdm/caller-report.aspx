<%@ Page Title="" Language="C#" MasterPageFile="~/bdm/MasterBdm.master" AutoEventWireup="true" CodeFile="caller-report.aspx.cs" Inherits="bdm_caller_report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        window.onload = function () {
            //alert("window load");

            duDatepicker('#<%= txtDate.ClientID %>', {
                auto: true, inline: true, format: 'dd/mm/yyyy',
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">Caller Report</h2>

    <div id="viewCall" runat="server">
        <span class="space15"></span>

        <div class="row">
            <div class="form-group col-sm-4">
                <span class="text-sm text-bold mrgBtm10 dspBlk">Date :</span>
                <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
            </div>
            <div class="col-sm-1">
                <span class="space30"></span>
                <asp:Button ID="btnShow" runat="server" CssClass="btn btn-md btn-primary" Text="Show List" OnClick="btnShow_Click" />
            </div>
        </div>

        <span class="space15"></span>
        <asp:GridView ID="gvCall" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
            AutoGenerateColumns="false" OnRowDataBound="gvCall_RowDataBound" Width="100%">
            <RowStyle CssClass="" />
            <HeaderStyle CssClass="bg-dark" />
            <AlternatingRowStyle CssClass="alt" />
            <Columns>
                <asp:BoundField DataField="TeamID">
                    <HeaderStyle CssClass="HideCol" />
                    <ItemStyle CssClass="HideCol" />
                </asp:BoundField>
                <asp:BoundField DataField="TeamPersonName" HeaderText="Name">
                    <ItemStyle Width="15%" />
                </asp:BoundField>
                <asp:BoundField DataField="TeamMobile" HeaderText="Mobile No.">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="TotalFollowup" HeaderText="Total Followup Calls">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="TotalEnquiry" HeaderText="Total Enquiry Calls">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="TotalCalls" HeaderText="Total Calls">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:TemplateField>
                    <ItemStyle Width="5%" />
                    <ItemTemplate>
                        <asp:Literal ID="litAnch" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <span class="warning">No Records to Display :(</span>
            </EmptyDataTemplate>
            <PagerStyle CssClass="gvPager" />
        </asp:GridView>

        <table>
            <tr>
                <td><h4><span class="formLable bold_weight text-primary">Total Calls :</span></h4></td>
                <td><h4><span class="formLable text-primary"><%= callCounts %></span></h4></td>
            </tr>
        </table>
    </div>

    <div id="ViewFollowup" runat="server">
        <div class="pad_10">
            <table class="form_table">
                <tr>
                    <td><span class="colorLightBlue">Id :</span></td>
                    <td>
                        <asp:Label ID="lblId" CssClass="colorLightBlue" runat="server" Text="[New]"></asp:Label></td>
                </tr>
                <tr>
                    <td><span class="colorLightBlue">Date :</span></td>
                    <td>
                        <asp:Label ID="lblDate" CssClass="colorLightBlue" runat="server" Text="[New]"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 25%"><span class="formLable bold_weight">Name :</span></td>
                    <td><span class="formLable"><%= arrCall[0] %></span> </td>
                </tr>
                <tr>
                    <td><span class="formLable bold_weight">Mobile No. :</span></td>
                    <td><span class="formLable"><%= arrCall[1] %></span> </td>
                </tr>
            </table>

            <asp:GridView ID="gvFlup" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
				AutoGenerateColumns="false" Width="100%">
				<RowStyle CssClass="" />
				<HeaderStyle CssClass="bg-dark" />
				<AlternatingRowStyle CssClass="alt" />
				<Columns>
					<asp:BoundField DataField="FlupID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="FlupTime" HeaderText="Followup Time">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="CustomerName" HeaderText="Name">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="FlupRemarkStatus" HeaderText="Followup Status">
						<ItemStyle Width="15%" />
					</asp:BoundField>
					<asp:BoundField DataField="FlupRemark" HeaderText="Followup Detail">
						<ItemStyle Width="20%" />
					</asp:BoundField>
					<asp:BoundField DataField="FlupNextDate" HeaderText="Next Followup">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FlupNextTime" HeaderText="Time">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
				</Columns>
				<EmptyDataTemplate>
					<span class="warning">No Orders to Display :(</span>
				</EmptyDataTemplate>
				<PagerStyle CssClass="gvPager" />
			</asp:GridView>

            <asp:GridView ID="gvEnqFlup" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
				AutoGenerateColumns="false" Width="100%">
				<RowStyle CssClass="" />
				<HeaderStyle CssClass="bg-dark" />
				<AlternatingRowStyle CssClass="alt" />
				<Columns>
					<asp:BoundField DataField="FlupEnqID">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle CssClass="HideCol" />
					</asp:BoundField>
					<asp:BoundField DataField="FlupTime" HeaderText="Followup Time">
						<ItemStyle Width="5%" />
					</asp:BoundField>
					<asp:BoundField DataField="CustomerName" HeaderText="Name">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="FlupEnqRemarkStatus" HeaderText="Enq. Followup Status">
						<ItemStyle Width="15%" />
					</asp:BoundField>
					<asp:BoundField DataField="FlupEnqRemark" HeaderText="Enq. Followup Detail">
						<ItemStyle Width="20%" />
					</asp:BoundField>
					<asp:BoundField DataField="FlupEnqNextDate" HeaderText="Next Enq. Followup">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FlupEnqNextTime" HeaderText="Time">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
				</Columns>
				<EmptyDataTemplate>
					<span class="warning">No Orders to Display :(</span>
				</EmptyDataTemplate>
				<PagerStyle CssClass="gvPager" />
			</asp:GridView>
        </div>

        <div class="card card-primary">
            <div class="card-header">
                <h3 class="card-title">Followup Status Review</h3>
            </div>
            <div class="card-body">
                <div class="row">
                    <div class="col col-3">
                        <div class="info-box bg-light">
                            <div class="info-box-content">
                                <span class="info-box-text text-center text-muted text-blue">Not Connected</span>
                                <span class="info-box-number text-center mb-0"><%= arrCounts[2] %></span>
                            </div>
                        </div>
                    </div>

                    <div class="col col-3">
                        <div class="info-box bg-light">
                            <div class="info-box-content">
                                <span class="info-box-text text-center text-muted text-blue">Connected</span>
                                <span class="info-box-number text-center mb-0"><%= arrCounts[5] %></span>
                            </div>
                        </div>
                    </div>

                    <div class="col col-3">
                        <div class="info-box bg-light">
                            <div class="info-box-content">
                                <span class="info-box-text text-center text-muted text-blue">Repeted Order</span>
                                <span class="info-box-number text-center mb-0"><%= arrCounts[8] %></span>
                            </div>
                        </div>
                    </div>

                    <div class="col col-3">
                        <div class="info-box bg-light">
                            <div class="info-box-content">
                                <span class="info-box-text text-center text-muted text-blue">New Order</span>
                                <span class="info-box-number text-center mb-0"><%= arrCounts[11] %></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

