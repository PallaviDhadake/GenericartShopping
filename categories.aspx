<%@ Page Title="Health Product Categories | Genericart Online Generic Medicine Shopping" Language="C#" MasterPageFile="~/MasterParent.master" AutoEventWireup="true" CodeFile="categories.aspx.cs" Inherits="categories" %>
<%@ MasterType VirtualPath="~/MasterParent.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <span class="space40"></span>

    <div class="col_1140">
        <%= catProdStr %>

        <%= paginationHtml %>
        <div class="float_clear"></div>

        <%--<h1 class="pageH2 clrLightBlack semiBold mrg_B_10">Baby Care Products</h1>

        <div class="col_1_4">
            <div class="pad_10">
                <div class="prodContainer">
                    <div class="pad_15">
                        <div class="txtCenter"><img src="<%= Master.rootPath + "images/medicine-product.jpg" %>" alt="" /></div>
                        <span class="prodLine"></span>
                        <span class="space15"></span>
                        <a href="product-details.html" class="prodName semiBold">CROCIN COLD & FLU</a>
                        <span class="space10"></span>
                        <p class="clrGrey line-ht-5 tiny mrg_B_15">Lorem Ipsum is simply dummy text</p>
                        <span class="prod-offer-price">&#8377; 359</span>
                        <span class="prod-price">&#8377; 599</span>
                        <span class="prod-discount">40% Off</span>
                    </div>
                </div>
            </div>
        </div>
        <div class="float_clear"></div>--%>
    </div>
</asp:Content>

