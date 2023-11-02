<%@ Page Language="C#" AutoEventWireup="true" CodeFile="payment.aspx.cs" Inherits="payment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="IE=edge" http-equiv="X-UA-Compatible" />
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
	<link rel="shortcut icon" href="/favicon.ico" type="image/x-icon" />

	<title>Genericart Online Generic Medicine Shopping</title>

	<meta name="description" content="" />

	<link href="https://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700" rel="stylesheet" />

    <link href="css/shoppingGencart.css" rel="stylesheet" />
	
	<script src="js/jquery-2.2.4.min.js"></script>
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
    <style type="text/css">
        input[type=submit] {
            position:absolute; top:50%; left:50%; transform:translate(-50%, -50%);
          background-color: #4CAF50; /* Green */
          border: none;
          color: white;
          padding: 15px 32px;
          text-align: center;
          text-decoration: none;
          display: inline-block;
          font-size: 20px; font-weight:600; text-transform:uppercase; letter-spacing:2px;
        }
    </style>
</head>
<body>
    <form action="Charge.aspx" method="post">
    <script
        src="https://checkout.razorpay.com/v1/checkout.js"
        data-key="rzp_live_XLBk42M2vXMaGC"
        data-amount="<%= ordAmount %>"
        data-name="Razorpay"
        data-description="Purchase Description"
        data-order_id="<%=orderId%>"
        data-image="https://razorpay.com/favicon.png"
        data-prefill.name="<%= custName %>"
        data-prefill.email="<%= custEmail %>"
        data-prefill.contact="<%= custMob %>"
        data-theme.color="#F37254"
    ></script>
        <div class="col_1140 txtCenter">
            <span class="space80"></span>
            <%--<img src="images/genericart-logo.png" alt="Genericart Online Generic Medicine Shopping" />--%>
            <a href="<%= rootPath %>" title="Genericart Online Generic Medicine Shopping" class="txtDecNone">
				<img src="images/genericart-logo.png" alt="Genericart Online Generic Medicine Shopping" />
			</a>
            <span class="space40"></span>
            <h2 class="medium line-ht-5 mrg_B_10">You will be redirected to payment page after clicking on pay now button.<br /> Your Order Amount is Rs. <%= oAmt %></h2>
            <input type="hidden" value="Hidden Element" name="hidden" />

            <%= errMsg %>
            <span>Order ID : </span><%= orderId %>
        </div>
    </form>
</body>
</html>
