<%@ Page Title="My Cart" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="cart.aspx.cs" Inherits="cart" %>
<%@ MasterType VirtualPath="~/MasterParent.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<span class="space40"></span>
	<div class="col_1140  bgWhite border_r_5 pad_15" id="cArt">
		<%= errMsg %>
		<div id="cartMarkup" runat="server">
			<div class="posRelative" id="prodGrid">
				<asp:GridView ID="gvCartItems" runat="server" AutoGenerateColumns="False" AllowPaging="true" PageSize="10"
					CssClass="gvApp" GridLines="None" 
					OnPageIndexChanging="gvCartItems_PageIndexChanging" OnRowDataBound="gvCartItems_RowDataBound" >
					<RowStyle CssClass="row" />
					<AlternatingRowStyle CssClass="alt" />
					<Columns>
						<asp:BoundField DataField="ProductID">
							<HeaderStyle CssClass="HideCol" />
							<ItemStyle CssClass="HideCol" />
						</asp:BoundField>
						<asp:BoundField DataField="ProductPhoto">
							<HeaderStyle CssClass="HideCol" />
							<ItemStyle CssClass="HideCol" />
						</asp:BoundField>
						<asp:BoundField DataField="FK_SubCategoryID">
							<HeaderStyle CssClass="HideCol" />
							<ItemStyle CssClass="HideCol" />
						</asp:BoundField>
						<asp:TemplateField HeaderText="Product">
							<HeaderStyle CssClass="prodImg" />
							<ItemStyle Width="10%" CssClass="prodImg" />
							<ItemTemplate>
								<asp:Literal ID="litProdPhoto" runat="server"></asp:Literal>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:BoundField DataField="ProductName" HeaderText="Product Name">
							<ItemStyle Width="20%" />
						</asp:BoundField>
						<asp:TemplateField HeaderText="Product Type">
							<HeaderStyle CssClass="HideCol" />
							<ItemStyle Width="10%" CssClass="HideCol" />
							<ItemTemplate>
								<asp:Literal ID="litCategory" runat="server"></asp:Literal>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:BoundField DataField="PriceFinal" HeaderText="Price">
							<HeaderStyle CssClass="HideCol" />
							<ItemStyle Width="10%" CssClass="price HideCol"/>
						</asp:BoundField>
						<asp:TemplateField HeaderText="Quantity">
							<ItemStyle Width="10%" />
							<ItemTemplate>
								<asp:TextBox ID="txtQuantity" runat="server" CssClass="textBox w80"></asp:TextBox>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Total">
							<ItemStyle Width="10%" />
							<ItemTemplate>
								<asp:Label ID="lblTotal" runat="server" Text="0"></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Mark for Pill Reminder">
							<ItemStyle Width="5%" />
							<ItemTemplate>
								<asp:CheckBox ID="chkSelect" runat="server" />
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Remove">
							<ItemStyle Width="10%" />
							<ItemTemplate>
								<asp:Literal ID="litRemove" runat="server"></asp:Literal>
							</ItemTemplate>
						</asp:TemplateField>
						</Columns>
					<EmptyDataTemplate>
						<span class="medium clrBlack fontRegular">No Medicine Requests..!!</span>
					</EmptyDataTemplate>
					<PagerStyle CssClass="gvPager"/>
				</asp:GridView>
				<div class="absTotal">
					<asp:Button ID="btnUpdateQty" runat="server" Text="Save Cart" CssClass="buttonForm semiBold upperCase letter-sp-2" OnClick="btnUpdateQty_Click"  />
					<asp:Button ID="btnCheckout" runat="server" Text="Proceed To Checkout" CssClass="blueAnch semiBold upperCase letter-sp-2" OnClick="btnCheckout_Click" />
					<a href="<%= Master.rootPath + "categories/medicine-7/1" %>" class="pinkAnch semiBold upperCase letter-sp-2">Add More Products</a>
					<%--Total:
					<span id="ctl00_ContentPlaceHolder1_lblGrandTotal">66,999</span><br />
					Grand Total : <span id="ctl00_ContentPlaceHolder1_lblGstTotal">66,999</span>--%>
				</div>
				<div class="float_clear"></div>
				<span class="space10"></span>
				<%--<span class="fontRegular regular semiBold clrBlack dispBlk">Add Note To Your Order :</span><span class="subNotice">(Optional)</span>
				<asp:TextBox ID="txtNote" runat="server" TextMode="MultiLine" Height="60" Width="300" CssClass="textBox"></asp:TextBox>--%>
				<span class="noteTxt"></span>
				<span class="space10"></span>
				<div class="txtRight">
					<asp:CheckBox ID="chkMreq" runat="server" TextAlign="Right" Font-Bold="true" CssClass="themeBgPrime small pad_TB_10 pad_LR_15 border_r_5 clrWhite" Text=" Mark this as my Monthly Requirement Request ?" />
				</div>
				<span class="space10"></span>
				<span class="greyLine"></span>
			</div>

			
			<div class="col_1_2_right">
				<div class="totalBox">
					<div class="pad_40">
						<div class="bgWhite">
							<div class="pad_30">
								<span class="themeClrSec medium semiBold dispBlk mrg_B_10">SubTotal : &#8377; <asp:Label ID="lblSubTotal" runat="server" Text="0" CssClass="semiBold"></asp:Label></span>
								<i><span class="clrGrey fontRegular small">Shipping & taxes calculated at checkout</span></i>
								<span class="space30"></span>
								<%--<asp:Button ID="btnCheckout" runat="server" Text="Proceed To Checkout" CssClass="blueAnch semiBold upperCase letter-sp-2" OnClick="btnCheckout_Click" />--%>
								<span class="space20"></span>
								<%--<a href="<%= Master.rootPath + "categories/medicine-7/1" %>" class="pinkAnch semiBold upperCase letter-sp-2">Add More Products</a>--%>
								
								<%--<a href="#" class="addToCartAnch semiBold upperCase letter-sp-2">Proceed To Checkout</a>--%>
							</div>
						</div>
					</div>
				</div>
			</div>
			<div class="col_1_2_right">
				<div class="pad_R_30">
					<img src="<%= Master.rootPath + "images/cart-page-sidebar.jpg" %>" class="width100" />
				</div>
			</div>
			<div class="float_clear"></div>
		</div>
	</div>
	

	<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
	<script type="text/javascript">
		$(function () {
			//$("[id*=txtQuantity]").val("1");
		});
		$("body").on("change keyup", "[id*=txtQuantity]", function () {
			//Check whether Quantity value is valid Float number. 
			var quantity = parseFloat($.trim($(this).val()));
			if (isNaN(quantity)) {
				quantity = 0;
			}

			//Update the Quantity TextBox.
			$(this).val(quantity);

			//Calculate and update Row Total.
			var row = $(this).closest("tr");
			$("[id*=lblTotal]", row).html(parseFloat($(".price", row).html()) * parseFloat($(this).val()));

			//Calculate and update Grand Total.
			var grandTotal = 0;
			var gstTotal = 0;
			$("[id*=lblTotal]").each(function () {
				grandTotal = grandTotal + parseFloat($(this).html());
				//gstTotal = (grandTotal * 18) / 100;
			});
			var finalTotal = grandTotal;
			//var finalTotal = grandTotal + gstTotal;
			//$("[id*=lblGrandTotal]").html(grandTotal.toFixed(2)); // toFixed(n) - to display n no. of digits after decimal point 
			$("[id*=lblSubTotal]").html(finalTotal.toFixed(2));
		});
	</script>
</asp:Content>

