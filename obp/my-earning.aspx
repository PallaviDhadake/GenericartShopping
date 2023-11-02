<%@ Page Title="My Earnings | OBP Genericart Medicines" Language="C#" MasterPageFile="~/obp/MasterObp.master" AutoEventWireup="true" CodeFile="my-earning.aspx.cs" Inherits="obp_my_earning" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">My Earnings</h2>
	<span class="space15"></span>

    	<%--gridview start--%>
		<asp:GridView ID="gvEarnings" runat="server" AutoGenerateColumns="False"
			CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None" Width="100%">

			<HeaderStyle CssClass="bg-dark" />
			<AlternatingRowStyle CssClass="alt" />
			<Columns>
				<asp:BoundField DataField="ObpComId">
					<HeaderStyle CssClass="HideCol" />
					<ItemStyle CssClass="HideCol" />
				</asp:BoundField>
				<asp:BoundField DataField="ObpComDate" HeaderText="Date">
					<ItemStyle Width="10%" />
				</asp:BoundField>
				<asp:BoundField DataField="ObpComType" HeaderText="Earn Type">
					<ItemStyle Width="10%" />
				</asp:BoundField>
				<asp:BoundField DataField="ObpRefUserId" HeaderText="Referral ID">
					<ItemStyle Width="10%" />
				</asp:BoundField>
				<asp:BoundField DataField="ObpRefName" HeaderText="Referral OBP Name">
					<ItemStyle Width="30%" />
				</asp:BoundField>
				<asp:BoundField DataField="ObpComLevel" HeaderText="Comm. Level">
					<ItemStyle Width="10%" />
				</asp:BoundField>
				<asp:BoundField DataField="ObpComPercent" HeaderText="Percent (%)">
					<ItemStyle Width="10%" />
				</asp:BoundField>
				<asp:BoundField DataField="ObpComAmount" HeaderText="Amount (&#8377;)">
					<ItemStyle Width="10%" />
				</asp:BoundField>
			</Columns>
			<EmptyDataTemplate>
				<span class="warning">No Earnings to Display :(</span>
			</EmptyDataTemplate>
			<PagerStyle CssClass="gvPager" />
		</asp:GridView>
		<%-- gridview End--%>
</asp:Content>

