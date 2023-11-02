<%@ Page Title="Customer Lookup" Language="C#" MasterPageFile="~/account/MasterAccount.master" AutoEventWireup="true" CodeFile="cust-lookup.aspx.cs" Inherits="account_cust_lookup" %>
<%@ MasterType VirtualPath="~/account/MasterAccount.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Customer Lookup Report</h2>
	<span class="space15"></span>
	<div class="row">
		<div class="form-group col-sm-3">
			<label class="text-sm">Customer Mobile No.:</label>
			<asp:TextBox ID="txtMob" runat="server" CssClass="form-control" placeholder="Registered Mobile No."></asp:TextBox>
		</div>
		<div class="col-sm-3">
			<span class="space30"></span>
			<asp:Button ID="btnShow" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnShow_Click"  />
		</div>
	</div>
        <div id="repBtn" runat="server">
            <a class="fancylink btn btn-md btn-info" href="<%= lookupUrl %>" >View Report</a>
        </div>
	<div class="row">
		<%= errMsg %>
	</div>
	<span class="space40"></span>


    <script type="text/javascript">
        $(document).ready(function () {
            $("a.fancylink").fancybox({
                type: 'iframe'
            });
        });

    </script>
</asp:Content>

