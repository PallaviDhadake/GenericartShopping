<%@ Page Title="Notification Image Gallery" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="notification-image-gallery.aspx.cs" Inherits="admingenshopping_notification_image_gallery" %>
<%@ MasterType VirtualPath="~/admingenshopping/MasterAdmin.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	
	<script>
		$(document).ready(function () {
			$('[id$=gvNotif]').DataTable({
				columnDefs: [
					 { orderable: false, targets: [0, 1, 2, 3] }
				],
				order: [[0, 'desc']]
			});
		});
	 </script>
	
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<h2 class="pgTitle">Notification Image Gallery</h2>
	<span class="space15"></span>

	<%= imgGallery %>


	<script>
		function myFunction(rowId) {
			//const paragraph = document.querySelector('span');
			const copyText = document.getElementById("url-" + rowId).innerText;
			//alert(copyText);
			var temp = document.createElement("INPUT");
			temp.value = copyText;
			document.body.appendChild(temp);
			temp.select();
			document.execCommand("copy");
			temp.remove();
			TostTrigger('success', 'Image Url Copied to Clipboard');
		}
	</script>
</asp:Content>

