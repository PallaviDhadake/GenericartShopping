<%@ Page Title="Product free offer" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="coupon-product.aspx.cs" Inherits="admingenshopping_coupon_product" %>
<%@ MasterType VirtualPath="~/admingenshopping/MasterAdmin.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />
	<link rel="stylesheet" href="/resources/demos/style.css" />
	<script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

	<style type="text/css">
		.optMargin {
			margin-right:15px;
			text-indent: 5px;
		}

		
	</style>
	
	 <script>
		 $(function () {
			 $('#<% =txtStartDate.ClientID%>').datepick({ dateFormat: 'dd/mm/yyyy' });
			 $('#<% =txtEndDate.ClientID%>').datepick({ dateFormat: 'dd/mm/yyyy' });
		 });
	</script>

	<script type="text/javascript">

		$(document).ready(function () {
			SearchText();
			SearchFreeProduct();
		});

		// Valaidate and save final data
		function SaveCoupon() {
			// Empty validation

	if (document.getElementById("<%=txtCpnCode.ClientID %>").value.trim() == "") {
		alert("Please enter Coupon Code!");
		return false;
	}
	if (document.getElementById("<%=txtStartDate.ClientID %>").value.trim() == "") {
		alert("Please enter start Date!");
		return false;
	}
	if (document.getElementById("<%=txtEndDate.ClientID %>").value.trim() == "") {
		alert("Please enter end Date!");
		return false;
	}
	if (document.getElementById("<%=txtCpnTitle.ClientID %>").value.trim() == "") {
		alert("Please enter Coupon Title!");
		return false;
	}
	if (document.getElementById("<%=txtCpnDesc.ClientID %>").value.trim() == "") {
		alert("Please enter Coupon Description!");
		return false;
	}
	if (document.getElementById("<%=txtCpnTC.ClientID %>").value.trim() == "") {
		alert("Please enter Terms & Condition!");
		return false;
	}
	if (document.getElementById("<%=txtPurchaseQty.ClientID %>").value.trim() == "") {
		alert("Please purchase quantity!");
		return false;
	}
	if (document.getElementById("<%=txtProductOffer.ClientID %>").value.trim() == "") {
		alert("Please enter offer product name!");
		return false;
	}
	var lableID = document.getElementById("<%=lblId.ClientID %>").innerText;

	if (lableID == '[New]') {
		var fileValid = document.getElementById("<%=fuImg.ClientID %>");
		if (fileValid.files.length == 0) {
			alert("Please select Coupon cover image!");
			return false;
		}
	}


	//Check whether valid dd/MM/yyyy Date Format.
	var dateStart = document.getElementById("<%=txtStartDate.ClientID %>").value;
	var dateEnd = document.getElementById("<%=txtEndDate.ClientID %>").value;
	var regex = /(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$/;
	if (regex.test(dateStart) == false || regex.test(dateEnd) == false) {
		alert("Please enter dd/mm/yyyy date format!");
		return false;
	}

	// Create Array object of form input fields ans send to WebMethod
	  
	//Disable our button
	$('#Button1').attr("disabled", true);

	var coupondata = {};
	coupondata.CouponID = lableID;
	coupondata.CouponImg = 'noimage.jpg';
	coupondata.CouponTitle = document.getElementById("<%=txtCpnTitle.ClientID %>").value.trim();
	coupondata.CouponInfo = document.getElementById("<%=txtCpnDesc.ClientID %>").value.trim();
	coupondata.CouponTerms = document.getElementById("<%=txtCpnTC.ClientID %>").value.trim();
	coupondata.CouponCode = document.getElementById("<%=txtCpnCode.ClientID %>").value.trim();
	coupondata.CouponType = 'product';
	coupondata.CouponRefType = 'Product';
	coupondata.CouponProductName = document.getElementById("<%=txtProduct.ClientID %>").value.trim();
	coupondata.CouponRefId = 0;
	coupondata.CouponProductOffer = document.getElementById("<%=txtProductOffer.ClientID %>").value.trim();
	coupondata.CouponProductId = 0;
	coupondata.CouponProductQty = document.getElementById("<%=txtPurchaseQty.ClientID %>").value.trim();
	coupondata.CouponPercentage = 0;
	coupondata.CouponCompareVal = 0;
	coupondata.CouponMinAmount = 0;
	coupondata.CouponMaxAmount = 0;
	coupondata.CouponMaxAllow = 1;
	coupondata.CouponUsedAmount = 0;
	coupondata.CouponStartDate = document.getElementById("<%=txtStartDate.ClientID %>").value.trim();
	coupondata.CouponEndDate = document.getElementById("<%=txtEndDate.ClientID %>").value.trim();
	coupondata.CouponDisplay = 'active';

	//var jsonData = JSON.stringify({
	//    cinfo: coupondata
	//});

	//alert(jsonData);

	$.ajax({
		type: "POST",
		url: "coupon-product.aspx/SaveCouponData",
		//data: jsonData,
		data: '{cinfo: ' + JSON.stringify(coupondata) + '}',
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		success: function (response) {
			//alert(response.d);

			//Enable our button
			$('#Button1').attr("disabled", false);

			switch (response.d) {
				case '1':
					// code block
					TostTrigger('success', 'Coupon saved successfully!');
					var fileValid = document.getElementById("<%=fuImg.ClientID %>");
					if (fileValid.files.length > 0) {
						UploadImage();
					}
					resetForm();
					break;
				case '2':
					// code block
					TostTrigger('warning', 'Duplicate coupon code entered.');
					break;
				case '3':
					// code block
					TostTrigger('warning', 'Invalid Product / Category name entered.');
					break;
				default:
					// code block
			}
			//window.location.reload();
		},
		error: function (response) {
			alert(response.d);
		}
	});

	return false;

}


// ================= Upload File Upload Control Image ================
function UploadImage() {
	$(function () {
		var handlerUrl = '<%=ResolveUrl("~/UploadFile.ashx") %>'

		var fileUpload = $('#<%=fuImg.ClientID%>').get(0);
		var files = fileUpload.files;
		var test = new FormData();
		for (var i = 0; i < files.length; i++) {
			test.append(files[i].name, files[i]);
		}
		var cpnCode = document.getElementById("<%=txtCpnCode.ClientID %>").value.trim();

		$.ajax({
			url: handlerUrl,
			type: "POST",
			contentType: false,
			processData: false,
			data: test,
			success: function (result) {
				if (result.split(':')[0] = "File Uploaded Successfully") {
					PageMethods.SaveAsCouponImage(cpnCode);

				}
				else {

				}
			},
			error: function (err) {
				alert(err.statusText);
			}
		});
	})
}


// =============== RESET Form Input fields ====================
function resetForm() {
	$('.resetInput').val('');
}



// ==================  Only Decimal numbers input validation ===============
$(document).ready(function () {

	$(".allow_numeric").on("input", function (evt) {
		var self = $(this);
		self.val(self.val().replace(/\D/g, ""));
		if ((evt.which < 48 || evt.which > 57)) {
			evt.preventDefault();
		}
	});

	$(".allow_decimal").on("input", function (evt) {
		var self = $(this);
		self.val(self.val().replace(/[^0-9\.]/g, ''));
		if ((evt.which != 46 || self.val().indexOf('.') != -1) && (evt.which < 48 || evt.which > 57)) {
			evt.preventDefault();
		}
	});

});


function startLoader() {
	$("#overlay").fadeIn(300);
}

function endLoader() {
	setTimeout(function () {
		$("#overlay").fadeOut(300);
	}, 500);
}



// Search Text of Product / Category Autocomplete
function SearchText() {
	var srchRequest = 'Product';
		 
	$("#<%= txtProduct.ClientID %>").autocomplete({
		source: function (request, response) {
			$.ajax({
				type: "POST",
				contentType: "application/json; charset=utf-8",
				url: "coupon-product.aspx/GetSearchControl",
				data: JSON.stringify({ srchType: srchRequest, prefix: document.getElementById('<%= txtProduct.ClientID %>').value }),
				dataType: "json",
				success: function (data) {
					//response(data.d);
					response($.map(data.d, function (item) {
						return {
							label: item.split('#')[0],
							//val: item.split('#')[1]
						}
					}))
				},
				error: function (result) {
					alert("No Match");
				}
			});
		}
	});
}


// Search Text of Product / Category Autocomplete
function SearchFreeProduct() {
	var srchRequest = '';
	srchRequest = 'Product';
	 

	$("#<%= txtProductOffer.ClientID %>").autocomplete({
		source: function (request, response) {
			$.ajax({
				type: "POST",
				contentType: "application/json; charset=utf-8",
				url: "coupon-product.aspx/GetSearchControl",
				data: JSON.stringify({ srchType: srchRequest, prefix: document.getElementById('<%= txtProductOffer.ClientID %>').value }),
				dataType: "json",
				success: function (data) {
					//response(data.d);
					response($.map(data.d, function (item) {
						return {
							label: item.split('#')[0],
							//val: item.split('#')[1]
						}
					}))
				},
				error: function (result) {
					alert("No Match");
				}
			});
		}
	});
}




// Dropdown list fillup function        

function fillDropdown(optionBtnVal) {
	//  alert(optionBtnVal);

	switch (optionBtnVal) {
		case 1:
			// code block
			var sqlQueryX = 'Select bannerId, imageName From BannerData';
			var displayX = 'imageName';
			var valueX = 'bannerId';
			break;
		case 3:
			// code block
			var sqlQueryX = 'Select top 10 ProductID, ProductName From ProductsData Where delmark=0 Order By ProductName';
			var displayX = 'ProductName';
			var valueX = 'ProductID';
			break;
		case 2:
			// code block
			var sqlQueryX = 'Select ProductCatID, ProductCatName From ProductCategory Where ParentCatID<>0 AND ChildCatFlag<>1 AND DelMark=0 Order By ProductCatName';
			var displayX = 'ProductCatName';
			var valueX = 'ProductCatID';
			break;
		default:
			// code block
	}

	//alert(sqlQueryX);

	$.ajax({
		type: "POST",
		url: "coupon-cash.aspx/FillDropdownList",
		data: JSON.stringify({ sqlQuery: sqlQueryX, displayField: displayX, valueField: valueX }),

		contentType: "application/json; charset=utf-8",
		dataType: "json",
		success: function (r) {
			//alert(r.d);
			var ddlCustomers = $("[id*=ddrProduct]");
			ddlCustomers.empty().append('<option selected="selected" value="0"><- Select -></option>');
			$.each(r.d, function () {
				ddlCustomers.append($("<option></option>").val(this['Value']).html(this['Text']));
			});

		}
	});
	//return false;

}

function fillMyList() {
	alert('called');
	$.ajax({
		type: "POST",
		url: "coupon-cash.aspx/FillDropdownList",
		//data: JSON.stringify({ OrderIdRef: orderRefX, OptionSelected: optValX }),

		contentType: "application/json; charset=utf-8",
		dataType: "json",
		success: function (r) {
			alert(r.d);
			//$('#modal-lg').modal('hide');
			//TostTrigger('success', 'Order:#' + orderRefX + ' Follow Up Submitted successfully');
		}
	});
	return false;
}


	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<div id="editBlog" runat="server">
	<h2 class="pgTitle">Offer : Product Discount</h2>
	<span class="space15"></span>
	
	<!-- Input control div starts -->
	 <div class="card card-primary">
		<div class="card-header">
			<h3 class="card-title"><%= pgTitle %></h3>
		</div>
		  <!-- card body -->
			<div class="card-body">
				<div class="colorLightBlue">
					<span>Id :</span>
					<asp:Label ID="lblId" runat="server" Text="[New]"></asp:Label>
				</div>

				<span class="space15"></span>
				<!-- form row start -->
				<div class="form-row">
					<div class="form-group col-md-6">
						<label>Coupon Code :*</label>
						<asp:TextBox ID="txtCpnCode" runat="server" CssClass="form-control resetInput resetInput" Width="100%" MaxLength="300"></asp:TextBox>
					</div>
				</div>
				<div class="form-row">
					<div class="form-group col-md-6">
						<label>Effective Date :*</label>
						<asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control resetInput resetInput" Width="100%" MaxLength="10" placeholder="Click here to open caldendar"></asp:TextBox>
					</div>
				</div>
				<div class="form-row">
					<div class="form-group col-md-6">
						<label>End Date :*</label>
						<asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control resetInput resetInput" Width="100%" MaxLength="10" placeholder="Click here to open caldendar"></asp:TextBox>
					</div>
				</div>
				 
				<div class="form-row">
					<div class="form-group col-md-6">
						<label>Title :*</label>
						<asp:TextBox ID="txtCpnTitle" runat="server" CssClass="form-control resetInput" Width="100%" MaxLength="300"></asp:TextBox>
					</div>
				</div>
				<div class="form-row">
					<div class="form-group col-md-6">
						<label>Description :*</label>
						<asp:TextBox ID="txtCpnDesc" runat="server" CssClass="form-control resetInput textarea" TextMode="MultiLine" Width="100%" Height="150"></asp:TextBox>
					</div>
				</div>
				<div class="form-row">
					<div class="form-group col-md-6">
						<label>Terms & Condition :*</label>
						<asp:TextBox ID="txtCpnTC" runat="server" CssClass="form-control resetInput textarea" TextMode="MultiLine" Width="100%" Height="150"></asp:TextBox>
					</div>
				</div>
				<div class="form-row">
					<div class="form-group col-md-6">
						<label>Product :*</label>
						<asp:TextBox ID="txtProduct" runat="server" CssClass="form-control resetInput" Width="100%" MaxLength="300"></asp:TextBox>
					</div>
				</div>
			
				<div class="form-row">
					<div class="form-group col-md-6">
						<label>Purchase Quantity :*</label>
						<asp:TextBox ID="txtPurchaseQty" runat="server" CssClass="form-control resetInput allow_numeric" Width="100%" MaxLength="5"></asp:TextBox>
					</div>
				</div>
				  <div class="form-row">
					<div class="form-group col-md-6">
						<label>Product Offer :*</label>
						<asp:TextBox ID="txtProductOffer" runat="server" CssClass="form-control resetInput" Width="100%" MaxLength="300"></asp:TextBox>
					</div>
				</div>
				<div class="form-row">
					<div class="form-group col-md-6">
						<label>Cover Image :</label>
						<asp:FileUpload ID="fuImg" runat="server" CssClass="form-control resetInput-file" />
						<span>Image recommended size: width= 600px, Height= 375px</span>
						<span class="space10"></span>
						
					</div>
				</div>
				<!--form row End -->
			</div>
			<!-- card body end--!>
	</div>
	<!-- Input control div ends -->
	<!-- Button controls starts -->
		<span class="space10"></span>
		<span class="space10"></span>
		 <input id="Button1" type="button" value="Submit" class="btn btn-primary" onclick="SaveCoupon();" />
		
		<asp:Button ID="btnDelete" runat="server" CssClass="btn btn-outline-info" Text="Delete" OnClientClick="return confirm('Are you sure to delete?');" />
		<asp:Button ID="btnCancel" runat="server" CssClass="btn btn-outline-dark" Text="Cancel" OnClick="btnCancel_Click" />
		<div class="float_clear"></div>
	<!-- Button controls ends -->
	</div>
	</div>
		 
		 <!-- Gridview to show saved data starts here -->
	<div id="viewBlog" runat="server">
		<a href="coupon-product.aspx?action=new" runat="server" class="btn btn-primary btn-md">Add New</a>
		<span class="space20"></span>
		<div class="formPanel table-responsive-md">
			<asp:GridView ID="gvCoupon"  runat="server" CssClass="table table-striped table-bordered table-hover" GridLines="None" 
				AutoGenerateColumns="false" OnPageIndexChanging="gvCoupon_PageIndexChanging" OnRowDataBound="gvCoupon_RowDataBound"   >
				<HeaderStyle CssClass="thead-dark" />
				<RowStyle CssClass="" />
				<AlternatingRowStyle CssClass="alt" />
				<Columns>
					<asp:BoundField DataField="coupon_id">
						<HeaderStyle CssClass="HideCol" />
						<ItemStyle  CssClass="HideCol"/>
					</asp:BoundField>
					<asp:BoundField DataField="cpnDate" HeaderText="Eff. Date">
						<ItemStyle Width="10%" />
					</asp:BoundField>
					<asp:BoundField DataField="coupon_code" HeaderText="Coupon Code">
						<ItemStyle Width="30%" />
					</asp:BoundField>
					<asp:BoundField DataField="coupon_head" HeaderText="Coupon Name">
						<ItemStyle Width="30%" />
					</asp:BoundField>
					<asp:TemplateField>
						<ItemStyle Width="5%" />
						<ItemTemplate>
							<asp:Literal ID="litAnch" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>                    
				</Columns>
				<EmptyDataTemplate>
					<span class="warning">Its Empty Here... :(</span>
				</EmptyDataTemplate>
				<PagerStyle CssClass="" />
			</asp:GridView>            
		</div>
	</div>
</asp:Content>

