<%@ Page Title="GOBP Join Level Tree View" Language="C#" MasterPageFile="~/obp/MasterObp.master" AutoEventWireup="true" CodeFile="gobp-treeView.aspx.cs" Inherits="obp_gobp_treeView" %>
<%@ MasterType VirtualPath="~/obp/MasterObp.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .mainjoinlevel{background:#ba3434; width:35px; height:35px; border-radius:50%; text-align:center !important; margin:0 auto !important;}

        .joinlevel{background:#ba3434; width:35px; height:35px; border-radius:50%; text-align:center !important; margin:0 auto !important; position:relative;}

        .joinlevel:before{content:''; width:500px; height:2px; background:#808080; position:absolute; left:50px; top:15px;}
        .joinlevel:after{content:''; width:500px; height:2px; background:#808080; position:absolute; right:50px; top:15px;}
        .border-radius{width:20px !important; height:10px !important; border-radius:50%;}
        .levelnum{margin-top:-5px !important;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">GOBP Tree View</h2>
    <span class="space15"></span>
    <div class="container text-center">
        <%=joinlevelstr %>
          <%--<div class="joinlevel">
                <div class="p-2">
                    <p class="text-white bold_weight levelnum" style="">1</p>
                </div>
            </div>
        <div class="row justify-content-center mt-3">
            <div class="clearfix"></div>
            <div class="col-3">
                <img src="../images/icons/gobp-treeView.png" class="img-fluid mr-3 mt-2" />
                <p class="bold_weight mb-0">OBP0012</p>
                <span class="bold_weight">ABDUL KHALIQ & ABDULLAH BAKRAN</span>
                <span class="space15"></span>
            </div>
            <div class="col-3">
                <img src="../images/icons/gobp-treeView.png" class="img-fluid mr-3 mt-2" />
                <p class="bold_weight mb-0">OBP0012</p>
                <span class="bold_weight">ABDUL KHALIQ & ABDULLAH BAKRAN</span>
                <span class="space15"></span>
            </div>
            <div class="col-3">
                <img src="../images/icons/gobp-treeView.png" class="img-fluid mr-3 mt-2" />
                <p class="bold_weight mb-0">OBP0012</p>
                <span class="bold_weight">ABDUL KHALIQ & ABDULLAH BAKRAN</span>
                <span class="space15"></span>
            </div>
        </div>


         <div class="joinlevel">
                <div class="p-2">
                    <p class="text-white bold_weight levelnum" style="">2</p>
                </div>
            </div>
        <div class="row justify-content-center mt-3">
            <div class="clearfix"></div>
            <div class="col-3">
                <img src="../images/icons/gobp-treeView.png" class="img-fluid mr-3 mt-2" />
                <p class="bold_weight mb-0">OBP0012</p>
                <span class="bold_weight">ABDUL KHALIQ & ABDULLAH BAKRAN</span>
                <span class="space15"></span>
            </div>
            <div class="col-3">
                <img src="../images/icons/gobp-treeView.png" class="img-fluid mr-3 mt-2" />
                <p class="bold_weight mb-0">OBP0012</p>
                <span class="bold_weight">ABDUL KHALIQ & ABDULLAH BAKRAN</span>
                <span class="space15"></span>
            </div>
            <div class="col-3">
                <img src="../images/icons/gobp-treeView.png" class="img-fluid mr-3 mt-2" />
                <p class="bold_weight mb-0">OBP0012</p>
                <span class="bold_weight">ABDUL KHALIQ & ABDULLAH BAKRAN</span>
                <span class="space15"></span>
            </div>
        </div>--%>

        <div class="row justify-content-center">
          <%--  <%=joinlevelstr %>--%>

            
               <%-- <span class="space20"></span>--%>
                <%--<div class="col-3">
                     <img src="../images/icons/gobp-treeView.png" class="img-fluid mr-3 mt-2" />
                <p class="bold_weight mb-0">OBP0012</p>
                <span class="bold_weight">ABDUL KHALIQ & ABDULLAH BAKRAN</span>
                <span class="space15"></span>
                </div>
                <div class="col-3">
                     <img src="../images/icons/gobp-treeView.png" class="img-fluid mr-3 mt-2" />
                <p class="bold_weight mb-0">OBP0012</p>
                <span class="bold_weight">ABDUL KHALIQ & ABDULLAH BAKRAN</span>
                <span class="space15"></span>
                </div>
                <div class="col-3">
                     <img src="../images/icons/gobp-treeView.png" class="img-fluid mr-3 mt-2" />
                <p class="bold_weight mb-0">OBP0012</p>
                <span class="bold_weight">ABDUL KHALIQ & ABDULLAH BAKRAN</span>
                <span class="space15"></span>
                </div>--%>
           

            <%--<div class="col">
                <div class="joinlevel">
                    <div class="p-2">
                        <p class="text-white bold_weight levelnum" style="">1</p>
                    </div>
                </div>
                <img src="../images/icons/gobp-treeView.png" class="img-fluid mr-3 mt-2" />
                <p class="bold_weight mb-0">OBP0012</p>
                <span class="bold_weight">ABDUL KHALIQ & ABDULLAH BAKRAN</span>
                <span class="space15"></span>
            </div>
            <div class="col-12">
                <div class="row justify-content-center">
                    <div class="col">
                         <div class="joinlevel">
                            <div class="p-2">
                               <p class="text-white bold_weight levelnum" style="">2</p>
                            </div>
                        </div>
                        <img src="../images/icons/gobp-treeView.png" class="img-fluid mr-3 mt-2" />
                        <p class="bold_weight mb-0">OBP0012</p>
                        <span class="bold_weight">ABDUL KHALIQ & ABDULLAH BAKRAN</span>
                        <span class="space15"></span>
                    </div>
                    <div class="col">
                         <div class="joinlevel">
                            <div class="p-2">
                               <p class="text-white bold_weight levelnum" style="">2</p>
                            </div>
                        </div>
                        <img src="../images/icons/gobp-treeView.png" class="img-fluid mr-3 mt-2" />
                        <p class="bold_weight mb-0">OBP0012</p>
                        <span class="bold_weight">ABDUL KHALIQ & ABDULLAH BAKRAN</span>
                        <span class="space15"></span>
                    </div>
                      <div class="col">
                         <div class="joinlevel">
                            <div class="p-2">
                               <p class="text-white bold_weight levelnum" style="">2</p>
                            </div>
                        </div>
                        <img src="../images/icons/gobp-treeView.png" class="img-fluid mr-3 mt-2" />
                        <p class="bold_weight mb-0">OBP0012</p>
                        <span class="bold_weight">ABDUL KHALIQ & ABDULLAH BAKRAN</span>
                        <span class="space15"></span>
                    </div>
                     <div class="col">
                         <div class="joinlevel">
                            <div class="p-2">
                               <p class="text-white bold_weight levelnum" style="">2</p>
                            </div>
                        </div>
                        <img src="../images/icons/gobp-treeView.png" class="img-fluid mr-3 mt-2" />
                        <p class="bold_weight mb-0">OBP0012</p>
                        <span class="bold_weight">ABDUL KHALIQ & ABDULLAH BAKRAN</span>
                        <span class="space15"></span>
                    </div>
                     <div class="col">
                         <div class="joinlevel">
                            <div class="p-2">
                               <p class="text-white bold_weight levelnum" style="">2</p>
                            </div>
                        </div>
                        <img src="../images/icons/gobp-treeView.png" class="img-fluid mr-3 mt-2" />
                        <p class="bold_weight mb-0">OBP0012</p>
                        <span class="bold_weight">ABDUL KHALIQ & ABDULLAH BAKRAN</span>
                        <span class="space15"></span>
                    </div>
                </div>
            </div>
            <div class="col-12  d-inline-block">
                <div class="row">
                    <div class="col">
                         <div class="joinlevel">
                            <div class="p-2">
                               <p class="text-white bold_weight levelnum" style="">3</p>
                            </div>
                        </div>
                        <img src="../images/icons/gobp-treeView.png" class="img-fluid mr-3 mt-2" />
                        <p class="bold_weight mb-0">OBP0012</p>
                        <span class="bold_weight">ABDUL KHALIQ & ABDULLAH BAKRAN</span>
                        <span class="space15"></span>
                    </div>
                     <div class="col">
                         <div class="joinlevel">
                            <div class="p-2">
                               <p class="text-white bold_weight levelnum" style="">3</p>
                            </div>
                        </div>
                        <img src="../images/icons/gobp-treeView.png" class="img-fluid mr-3 mt-2" />
                        <p class="bold_weight mb-0">OBP0012</p>
                        <span class="bold_weight">ABDUL KHALIQ & ABDULLAH BAKRAN</span>
                        <span class="space15"></span>
                    </div>
                     <div class="col">
                         <div class="joinlevel">
                            <div class="p-2">
                               <p class="text-white bold_weight levelnum" style="">3</p>
                            </div>
                        </div>
                        <img src="../images/icons/gobp-treeView.png" class="img-fluid mr-3 mt-2" />
                        <p class="bold_weight mb-0">OBP0012</p>
                        <span class="bold_weight">ABDUL KHALIQ & ABDULLAH BAKRAN</span>
                        <span class="space15"></span>
                    </div>
                </div>
            </div>
            <div class="col-12">
                <div class="row">
                    <div class="col">
                        <a href="#" class="text-decoration-none text-dark">
                        <img src="../images/icons/gobp-treeView.png" class="img-fluid mr-3" />
                      
                        <p class="bold_weight mb-0">OBP0012</p>
                        <span class="bold_weight">Pallavi Ramesh Dhadake</span>
                        <span class="space15"></span>
                        </a>
                    </div>
                    <div class="col">
                        <img src="../images/icons/gobp-treeView.png" class="img-fluid mr-3" />
                        <p class="bold_weight mb-0">OBP0012</p>
                        <span class="bold_weight">Pallavi Ramesh Dhadake</span>
                        <span class="space15"></span>
                    </div>
                    <div class="col">
                        <img src="../images/icons/gobp-treeView.png" class="img-fluid mr-3" />
                        <p class="bold_weight mb-0">OBP0012</p>
                        <span class="bold_weight">Pallavi Ramesh Dhadake</span>
                        <span class="space15"></span>
                    </div>
                    <div class="col">
                        <img src="../images/icons/gobp-treeView.png" class="img-fluid mr-3" />
                        <p class="bold_weight mb-0">OBP0012</p>
                        <span class="bold_weight">Pallavi Ramesh Dhadake</span>
                        <span class="space15"></span>
                    </div>
                    <div class="col">
                        <img src="../images/icons/gobp-treeView.png" class="img-fluid mr-3" />
                        <p class="bold_weight mb-0">OBP0012</p>
                        <span class="bold_weight">Pallavi Ramesh Dhadake</span>
                        <span class="space15"></span>
                    </div>
                </div>
            </div>--%>


           <%-- <div class="joinlevel">
                            <div class="p-2">
                               <p class="text-white bold_weight levelnum" style="">2</p>
                            </div>
                        </div>--%>

            <%-- ViewSorce Markup --%>
           <%-- <div class="col-12">
                <div class="row justify-content-center">
                    <div class="col">
                        <div class="joinlevel">
                            <div class="p-2">
                                <div class="text-white bold_weight levelnum">3</p></div>
                            </div>
                            <img src="../images/icons/gobp-treeView.png" class="img-fluid mr-3 mt-2"><p class="bold_weight mb-0">OBP00084</p>
                            <span class="bold_weight">demo</span><span class="space15"></span></div>
                    </div>
                </div>
                <div class="col-12">
                    <div class="row justify-content-center">
                        <div class="col">
                            <div class="joinlevel">
                                <div class="p-2">
                                    <div class="text-white bold_weight levelnum">3</p></div>
                                </div>
                                <img src="../images/icons/gobp-treeView.png" class="img-fluid mr-3 mt-2"><p class="bold_weight mb-0">OBP00084</p>
                                <span class="bold_weight">Pallavi Ramesh Dhadake</span><span class="space15"></span></div>
                        </div>
                    </div>--%>

            

            <%-- End markup --%>


        </div>
    </div>
</asp:Content>

