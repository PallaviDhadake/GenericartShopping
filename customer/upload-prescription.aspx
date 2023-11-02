<%@ Page Title="Upload Prescription" Language="C#" MasterPageFile="~/customer/MasterCustomer.master" AutoEventWireup="true" CodeFile="upload-prescription.aspx.cs" Inherits="customer_upload_prescription" %>
<%@ MasterType VirtualPath="~/customer/MasterCustomer.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<style>
		.closeAnch{background:url(../images/icons/close.png) no-repeat center center; display:block; height:20px; width:20px; position:absolute; top:5px; right:5px  }
		.imgBox{ float:left;position:relative; width:50%; margin-top:10px; }
		.imgContainer{ height:200px !important; width:200px; overflow:hidden !important; }
		.w100{ width:100% }
		.pad-5{ padding:5px }
		.border1{ border:1px solid #ececec }
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<!-- Upload Rx Starts -->
	<div class="width50" id="orders">
		<div class="pad_10">
			<div class="bgWhite border_r_5 box-shadow">
				<div class="pad_20">
					<h2 class="clrLightBlack semiBold semiMedium">Upload Prescription</h2>
					<span class="space20"></span>
					<asp:FileUpload ID="fuPrescription" runat="server" />
					<span class="space20"></span>
					<div class="blueAnch dspInlineBlk mrg_R_15">
						<asp:Button ID="btnUploadRx" runat="server" Text="Upload Prescription" CssClass="prescriButton small semiBold mrg_R_15" OnClick="btnUploadRx_Click" />
					</div>
					<div class="dspInlineBlk txtCenter">OR</div>
					<div class="whatsappAnch dspInlineBlk mrg_L_15">
						<a href="https://wa.me/919730484686?text=I%20need%20prescription%20for%20my%20medicine,%20please%20assist%20me" class="whatsappButton small semiBold txtDecNone" target="_blank" title="Click here to get prescription from Dr. Shruti">Request Prescription</a>
					</div>
					<span class="space20"></span>
					<%= prescriStr %>

					<span class="space20"></span>
					<a href="order-details?orderId=<%= orId %>" class="pinkAnch dspInlineBlk semiBold">Back</a>
				</div>
			</div>
		</div>
	</div>
	<!-- Upload Rx Ends -->
</asp:Content>

