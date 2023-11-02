<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        RegisterRoutes(System.Web.Routing.RouteTable.Routes);

    }
    void RegisterRoutes(System.Web.Routing.RouteCollection routes)
    {
        routes.MapPageRoute("prod-route", "products/{prCatId}/{prodId}", "~/products.aspx", false, new System.Web.Routing.RouteValueDictionary { { "prCatId", string.Empty }, { "prodId", string.Empty } });
        routes.MapPageRoute("categories-route", "categories/{catId}/{pageId}", "~/categories.aspx", false, new System.Web.Routing.RouteValueDictionary { { "catId", string.Empty }, { "pageId", string.Empty } });
        routes.MapPageRoute("diseases-route", "diseases/{disId}/{pgId}", "~/diseases.aspx", false, new System.Web.Routing.RouteValueDictionary { { "disId", string.Empty }, { "pgId", string.Empty } });
        routes.MapPageRoute("hp-route", "health-products/{hprId}/{hpPgId}", "~/health-products.aspx", false, new System.Web.Routing.RouteValueDictionary { { "hprId", string.Empty }, { "hpPgId", string.Empty } });
        routes.MapPageRoute("blogs-route", "blogs/{blogId}", "~/blogs.aspx", false, new System.Web.Routing.RouteValueDictionary { { "blogId", string.Empty } });
        routes.MapPageRoute("prodQview-route", "product-quick-view/{prodName}/{prodId}", "~/product-quick-view.aspx", false, new System.Web.Routing.RouteValueDictionary { { "prodName", string.Empty }, { "prodId", string.Empty } });

        routes.MapPageRoute("doctors-route", "doctors/{specId}", "~/doctors.aspx", false, new System.Web.Routing.RouteValueDictionary { { "specId", string.Empty } });
        routes.MapPageRoute("docProfile-route", "doctors-profile/{docId}", "~/doctors-profile.aspx", false, new System.Web.Routing.RouteValueDictionary { { "docId", string.Empty } });
        routes.MapPageRoute("bookAppointment-route", "book-appointment/{doctorId}", "~/book-appointment.aspx", false, new System.Web.Routing.RouteValueDictionary { { "doctorId", string.Empty } });

        routes.MapPageRoute("abt-route", "about-us", "~/about-us.aspx");
        routes.MapPageRoute("contact-route", "contact-us", "~/contact-us.aspx");
        routes.MapPageRoute("search-route", "search", "~/search.aspx");
        routes.MapPageRoute("testimonials-route", "testimonials", "~/testimonials.aspx");
        routes.MapPageRoute("consultDoc-route", "consult-doctor", "~/consult-doctor.aspx");

        routes.MapPageRoute("privacy-route", "privacy-policy", "~/privacy-policy.aspx");
        routes.MapPageRoute("saleTerms-route", "terms-of-sale", "~/terms-of-sale.aspx");
        routes.MapPageRoute("useTerms-route", "terms-of-use", "~/terms-of-use.aspx");
        routes.MapPageRoute("terms-route", "terms-and-conditions", "~/terms-and-conditions.aspx");
        routes.MapPageRoute("disclaimer-route", "disclaimer", "~/disclaimer.aspx");
        routes.MapPageRoute("faq-route", "faq", "~/faq.aspx");

        routes.MapPageRoute("labTest-route", "lab-test", "~/lab-test.aspx");
        routes.MapPageRoute("uploadRxx-route", "upload-prescription", "~/upload-prescription.aspx");
        routes.MapPageRoute("genMitra-route", "generic-mitra-info", "~/generic-mitra-info.aspx");
        routes.MapPageRoute("regGenMitra-route", "register-genmitra", "~/register-genmitra.aspx");

        routes.MapPageRoute("cart-route", "cart", "~/cart.aspx");
        routes.MapPageRoute("wishlist-route", "wishlist", "~/wishlist.aspx");
        routes.MapPageRoute("login-route", "login", "~/login.aspx");
        routes.MapPageRoute("register-route", "registration/{regId}", "~/registration.aspx", false, new System.Web.Routing.RouteValueDictionary { { "regId", string.Empty } });
        routes.MapPageRoute("checkout-route", "checkout/{chkId}", "~/checkout.aspx", false, new System.Web.Routing.RouteValueDictionary { { "chkId", string.Empty } });
        routes.MapPageRoute("savingCalc-route", "saving-calculator", "~/saving-calculator.aspx");
        routes.MapPageRoute("forgot-pwd-route", "forgot-password", "~/forgot-pwd.aspx");
        routes.MapPageRoute("nearShops-route", "nearest-shops", "~/nearest-shops.aspx");
        routes.MapPageRoute("enqCheckout-route", "enquiry-checkout", "~/enquiry-checkout.aspx");
        routes.MapPageRoute("bookLabtest-route", "book-lab-test", "~/book-lab-test.aspx");

        routes.MapPageRoute("payment-route", "payment", "~/payment.aspx");

        // Customer Folder
        routes.MapPageRoute("userInfo-route", "customer/user-info", "~/customer/user-info.aspx");
        routes.MapPageRoute("userAddr-route", "customer/user-address", "~/customer/user-address.aspx");
        routes.MapPageRoute("myOrders-route", "customer/my-orders", "~/customer/my-orders.aspx");
        routes.MapPageRoute("ordDetails-route", "customer/order-details", "~/customer/order-details.aspx");
        routes.MapPageRoute("myAppointment-route", "customer/my-appointment", "~/customer/my-appointment.aspx");
        routes.MapPageRoute("uploadRx-route", "customer/upload-prescription", "~/customer/upload-prescription.aspx");
        routes.MapPageRoute("changePass-route", "customer/change-password", "~/customer/change-password.aspx");
        routes.MapPageRoute("reqQc-route", "customer/request-qc-report", "~/customer/request-qc-report.aspx");
        routes.MapPageRoute("reqRx-route", "customer/request-prescription", "~/customer/request-prescription.aspx");


        //routes.MapPageRoute("adds-route", "ads-details/{advId}", "~/ads-details.aspx", false, new System.Web.Routing.RouteValueDictionary { { "advId", string.Empty } });

    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started
        Session.Timeout = 300;
    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

    protected void Application_BeginRequest(Object sender, EventArgs e)
    {
        //if (!Request.IsSecureConnection)
        //{
        //    string path = string.Format("https{0}", Request.Url.AbsoluteUri.Substring(4));
        //    Response.Redirect(path);
        //}
    }

</script>
