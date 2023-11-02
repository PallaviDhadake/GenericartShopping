<%@ Page Title="Welcome, GOBP | Genericart Medicines Pvt Ltd" Language="C#" MasterPageFile="~/obp/MasterGobp.master" AutoEventWireup="true" CodeFile="welcome-obp.aspx.cs" Inherits="obp_welcome_obp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="pad_10">
				<div class="bgWhite border_r_5 box-shadow">
					<div class="pad_20">
						<h2 class="clrLightBlack semiBold semiMedium">Welcome GOBP</h2>
						<span class="space20"></span>
						<div class="userImgLarge dis-tbl" style="<%= bgColor %>">
							<%--<img src="images/user.jpg" alt="" />--%>
							<div class="unameLarge tbl-cell">
								<span class="clrWhite semiBold"><%= firstChar %></span>
							</div>
						</div>
						<div class="userNameLarge">
							<h2 class="semiMedium clrLightBlack semiBold"><%= sessionName %></h2>
							<span class="tiny clrGrey fontRegular">Joined on <%= joinDate %></span>

							<span class="space20"></span>
							
						</div>
						<div class="float_clear"></div>
					    <span class="space30"></span>

						<!-- Fav Shop Display Starts -->
						<div id="StatsInfo">
							<span class="space30"></span>
							<h3 class="medium themeClrPrime semiBold mrg_B_10">Today's Orders Overview</h3>
							<div class="col_1_3_fluid">
								<div class="pad_15">
									<div class="gradStats">
										<h4><% = arrCounts[0] %> <span>Order Count</span></h4>
									</div>
								</div>
							</div>
							<div class="col_1_3_fluid">
								<div class="pad_15">
									<div class="gradStats">
										<h4><% = arrCounts[1] %> <span>Order Amount</span></h4>
									</div>
								</div>
							</div>
							<div class="col_1_3_fluid">
								<div class="pad_15">
									<div class="gradStats">
										<h4><% = arrCounts[2] %> <span>Incentive Amount</span></h4>
									</div>
								</div>
							</div>
							<div class="float_clear"></div>
							<span class="space20"></span>
							<div class="col_1_3_fluid">
								<div class="pad_15">
									<a href="add-customer.aspx" class="icon-link">
									  <span class="icon"></span>
									  <span class="link-text">Add Customer</span>
									</a>
								</div>
							</div>
							<div class="col_1_3_fluid">
								<div class="pad_15">
									<a href="<%= rootPath + "categories/medicine-7/1" %>" class="icon-link">
									  <span class="icon icoPlaceOrder"></span>
									  <span class="link-text">Place Order</span>
									</a>
								</div>
							</div>
							<div class="col_1_3_fluid">
								<div class="pad_15">
									<a href="#" class="icon-link">
									  <span class="icon icoOrdersTill"></span>
									  <span class="link-text">Orders Till Date</span>
									</a>
								</div>
							</div>
							<div class="float_clear"></div>
						</div>
						<!-- Fav Shop Display Ends -->
					</div>
				</div>
			</div>
</asp:Content>

