<%@ Page Title="" Language="C#" MasterPageFile="~/obpmanager/MasterObpManager.master" AutoEventWireup="true" CodeFile="obp-daywise-report.aspx.cs" Inherits="obpmanager_obp_daywise_report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <script>
         $(document).ready(function () {
             // Fire the click event on page load
             $('#menuToggle').trigger('click');
             //alert("end");
         });
</script>
    <script>
        $(document).ready(function () {

            $('[id$=gvGOBPDaywise]').DataTable({
                columnDefs: [
                    { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9] }
                ],
                order: [[0, 'DE']]
            });
        });
    </script>
    <style>
      
    </style>
    <script type="text/javascript">

        window.onload = function () {
            //alert("window load");

            duDatepicker('#<%= txtFDate.ClientID %>', {
				auto: true, inline: true, format: 'dd/mm/yyyy',
				
			});
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <h2 class="pgTitle">GOBP Day Wise Report</h2>
    <span class="space15"></span>

    <div class="row" id="daterange" runat="server">
        <div class="col-md-3">
            <%--<label> Date :*</label>--%>
				<asp:TextBox ID="txtFDate" runat="server" CssClass="form-control" Width="100%" placeholder="Click here to open calendar"></asp:TextBox>
        </div>
        <div class="col-md-3">
            <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="btn btn-md btn-primary" OnClick="btnShow_Click" />
        </div>
      
    </div>
    <span class="space30"></span>

    <div class="viewOrder" runat="server">
        <div>
            <asp:GridView ID="gvGOBPDaywise" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
                AutoGenerateColumns="false" Width="100%" OnRowDataBound="gvGOBPDaywise_RowDataBound">
                <RowStyle CssClass="" />
                <HeaderStyle CssClass="bg-dark" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="OBP_ID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OBP_UserID" HeaderText="OBP User ID">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>  
                     <asp:BoundField DataField="OBP_ApplicantName" HeaderText="Name">
                        <ItemStyle Width="25%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OrderID" HeaderText="Order No.">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                      <asp:BoundField DataField="CustomerName" HeaderText="Customer Name">
                        <ItemStyle Width="20%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ItemsCount" HeaderText="Items">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                     <asp:BoundField DataField="OrderAmt" HeaderText="Order Amt (Inc. Tax)">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OrderStatus" HeaderText="Status">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                     <asp:BoundField DataField="OrderType" HeaderText="Order Type">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                     <asp:BoundField DataField="CommissionAmt" HeaderText="Commission Amt">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                </Columns>
                  <EmptyDataTemplate>
                    <span class="warning">No sales data to display</span>
                </EmptyDataTemplate>
                <PagerStyle CssClass="gvPager" />
            </asp:GridView>
        </div>
        <span class="space20"></span>
        <span class="bold_weight large">Total Commission Amount : <%=totalcomamt %></span>
    </div>
</asp:Content>

