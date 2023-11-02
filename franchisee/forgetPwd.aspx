<%@ Page Language="C#" AutoEventWireup="true" CodeFile="forgetPwd.aspx.cs" Inherits="franchisee_forgetPwd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Genericart shopping franchisee forget Password</title>

    <link href="../admingenshopping/css/jquery_notification.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../admingenshopping/plugins/fontawesome-free/css/all.min.css"/>
    <!-- Ionicons -->
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css"/>
    <!-- icheck bootstrap -->
    <link rel="stylesheet" href="../admingenshopping/plugins/icheck-bootstrap/icheck-bootstrap.min.css"/>
    <!-- Theme style -->
    <link rel="stylesheet" href="../admingenshopping/dist/css/adminlte.min.css"/>
    <!-- Google Font: Source Sans Pro -->
    <link href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700" rel="stylesheet"/>

   

    <script src="../admingenshopping/plugins/jquery/jquery.min.js"></script>
    <script src="../admingenshopping/plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
    <script src="../admingenshopping/dist/js/adminlte.min.js"></script>
    <script src="../admingenshopping/js/jquery_notification_v.1.js" type="text/javascript"></script>
    <script src="../js/iScript.js"></script>
     <!-- Toast Notification files -->
    <link href="<%= rootPath + "../css/toastr.css" %>" rel="stylesheet" />
    <script src="<%= rootPath + "../js/toastr.js" %>"></script>
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
<body class="hold-transition login-page">
    <form id="form1" runat="server">
        <div>
            <div class="login-box">
                <div class="login-logo">
                    <img src="../admingenshopping/images/customIcon/genericart-logo.png" alt="Genericart Logo" />
                    <h3 class="titleTxt txtCenter">Genericart Shopping<br /></h3>
                </div>
                <!-- /.login-logo -->
                <div class="card">
                    <div class="card-body login-card-body">
                        <p class="login-box-msg">Enter your mobile number</p>
                       
                            <div class="input-group mb-3">
                                <asp:TextBox ID="txtMobile" runat="server" class="form-control" placeholder="Mobile no."></asp:TextBox>
                                <div class="input-group-append">
                                    <div class="input-group-text">
                                        <span class="fas fa-envelope"></span>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-12">
                                    <asp:Button ID="cmdRecover" runat="server" Text="Send" class="btn btn-primary btn-block" OnClick="cmdRecover_Click" />

                                </div>
                                <!-- /.col -->
                                <%=errMsg %>
                            </div>
                       
                        <p class="mt-3 mb-1">
                            <a href="default.aspx" class="fPass" title="Franchisee Login">Log In</a>
                        </p>
                    </div>
                    <!-- /.login-card-body -->
                </div>
            </div>
        </div>
    </form>

   <%-- <script src="../admingenshopping/plugins/jquery/jquery.min.js"></script>
    <script src="../admingenshopping/plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
    <script src="../admingenshopping/dist/js/adminlte.min.js"></script>
    <script src="../admingenshopping/js/jquery_notification_v.1.js" type="text/javascript"></script>
    <script src="../js/iScript.js"></script>
    <script src="<%= rootPath + "../js/toastr.js" %>"></script>
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
     </script>--%>
</body>
</html>
