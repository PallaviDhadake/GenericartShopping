<%@ Page Language="C#" AutoEventWireup="true" CodeFile="import-netpaisa-data.aspx.cs" Inherits="import_netpaisa_data" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/toastr.css" rel="stylesheet" />
    <script src="js/toastr.js"></script>

    <script type="text/javascript">
		function TostTrigger(EventName, MsgText) {
			// code to be executed
			Command: toastr[EventName](MsgText)
			toastr.options = {
				"closeButton": true,
				"debug": false,
				"newestOnTop": false,
				"progressBar": false,
				"positionClass": "toast-top-full-width",
				"preventDuplicates": false,
				"onclick": null,
				"showDuration": "300",
				"hideDuration": "1000",
				"timeOut": "5000",
				"extendedTimeOut": "1000",
				"showEasing": "swing",
				"hideEasing": "linear",
				"showMethod": "slideDown",
				"hideMethod": "fadeOut"
            }

		}
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btnImport" runat="server" Text="Import Data" OnClick="btnImport_Click" />
            <%=errMsg2 %>
			<asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
			<asp:Label ID="lblStateName" runat="server" Text="Label"></asp:Label>
        </div>
    </form>
</body>
</html>
