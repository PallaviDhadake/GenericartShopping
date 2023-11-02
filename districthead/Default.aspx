<%@ Page Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="districthead_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Genericart Shopping District Head Login </title>

    <script src="../admingenshopping/js/jquery-2.2.4.min.js"></script>
    
    <script src="../admingenshopping/js/jquery.plugin.js"></script>
    <script src="../admingenshopping/js/jquery_notification_v.1.js"></script>

    <link href="../admingenshopping/plugins/fontawesome-free/css/all.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css" />
    <link href="../admingenshopping/plugins/icheck-bootstrap/icheck-bootstrap.min.css" rel="stylesheet" />
    <link href="../admingenshopping/dist/css/adminlte.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700" rel="stylesheet" />

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
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="login-box">
                    <div class="login-logo">
                        <img src="../admingenshopping/images/customIcon/genericart-logo.png" alt="Genericart Logo"/>
                         <h3 class="titleTxt txtCenter">Genericart Shopping<br /></h3>
                    </div>
                    <!-- /.login-logo -->
                    <div class="card">
                        <div class="card-body login-card-body">
                            <p class="login-box-msg"><b>District Head LogIn</b></p>
                            <div class="input-group mb-3">
                                <asp:TextBox ID="txtShopCode" runat="server" class="form-control" Placeholder="User Id"></asp:TextBox>
                                <div class="input-group-append">
                                    <div class="input-group-text">
                                        <span class="fas fa-envelope"></span>
                                    </div>
                                </div>
                            </div>
                            <div class="input-group mb-3">
                                <asp:TextBox ID="txtPwd" runat="server" class="form-control" TextMode="Password" Placeholder="Password"></asp:TextBox>
                                <div class="input-group-append">
                                    <div class="input-group-text">
                                        <span class="fas fa-lock"></span>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-8">
                                    <div>
                                    </div>
                                </div>
                                <!-- /.col -->
                                <div class="col-4">
                                    <asp:Button ID="cmdSign" runat="server" Text="Sign In" class="btn btn-primary btn-block" OnClick="cmdSign_Click" />
                                </div>
                                <!-- /.col -->
                            </div>
                            <p class="mb-1">
                                <a href="forgetPwd.aspx" class="fPass" title="Forgot password recovery">Forgot password?</a>
                            </p>
                        </div>
                        <%=errMsg %>
                        <!-- /.login-card-body -->
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </form>

    <script src="../admingenshopping/plugins/jquery/jquery.min.js"></script>
    
    <!-- Bootstrap 4 -->
    <script src="../admingenshopping/plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
   
    <!-- AdminLTE App -->
    <script src="../admingenshopping/dist/js/adminlte.min.js"></script>
   <%-- <script src="dist/js/adminlte.min.js"></script>--%>
</body>
</html>
