<%@ Page Title="Enquiries" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="saving-calc-enquiry.aspx.cs" Inherits="supportteam_saving_calc_enquiry" %>
<%@ MasterType VirtualPath="~/supportteam/MasterSupport.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        window.onload = function () {
            duDatepicker('#<%= txtNStartDate.ClientID %>', {
                auto: true, inline: true, format: 'dd/mm/yyyy',
            });
            duDatepicker('#<%= txtNEndDate.ClientID %>', {
                auto: true, inline: true, format: 'dd/mm/yyyy',
            });
        }
    </script>

    <script>
        $(document).ready(function () {
            $('[id$=gvEnq]').DataTable({
                columnDefs: [
					{ orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7, 8] }
                ],
                order: [[0, 'desc']]
            });
        });
	</script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="card card-info">
        <div class="card-header">
            <h3 class="card-title">Enquiries</h3>
        </div>
        <div class="card-body">
            <div class="form-row">
                <div class="form-group col-md-6">
                    <label>Select From Date :*</label>
                    <asp:TextBox ID="txtNStartDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <label>Select To Date :*</label>
                    <asp:TextBox ID="txtNEndDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
                </div>
            </div>
            <asp:Button ID="btnShow" runat="server" CssClass="btn btn-md btn-info" Text="Submit" OnClick="btnShow_Click"  />
        </div>
    </div>

     <div id="enqDetails" runat="server">
        <asp:GridView ID="gvEnq" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvEnq_RowDataBound"
            CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None">
            <HeaderStyle CssClass="bg-dark" />
            <AlternatingRowStyle CssClass="alt" />
            <Columns>
                <asp:BoundField DataField="CalcID" HeaderText="Enquiry ID">
                    <HeaderStyle CssClass="HideCol" />
                    <ItemStyle CssClass="HideCol" />
                    <FooterStyle CssClass="HideCol" />
                </asp:BoundField>
                 <asp:BoundField DataField="flpDate" HeaderText="Follow up Date" ItemStyle-BackColor="#bbdb95">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="enqDate" HeaderText="Enquiry Date">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="CustomerName" HeaderText="Customer Name">
                    <ItemStyle Width="15%" />
                </asp:BoundField>
                <asp:BoundField DataField="CustomerMobile" HeaderText="Mobile No.">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="DeviceType" HeaderText="Enquiry From">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="EnqStatus" HeaderText="Enquiry Status">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="MonthlyEnq" HeaderText="Monthly Enquiry">
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
                <span class="warning">No Data to Display</span>
            </EmptyDataTemplate>
            <FooterStyle BackColor="Black" ForeColor="White" />
            <PagerStyle CssClass="gvPager" />
        </asp:GridView>
    </div>
    <span class="space20"></span>
    <%= errMsg %>
</asp:Content>

