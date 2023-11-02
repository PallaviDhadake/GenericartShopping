<%@ Page Title="Add Product photos" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="product-photos.aspx.cs" Inherits="admingenshopping_product_photos" %>
<%@ MasterType VirtualPath="~/admingenshopping/MasterAdmin.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .closeAnch {
            background: url(images/icons/deleteIco.png) no-repeat center center;
            display: block;
            height: 20px;
            width: 20px;
            position: absolute;
            top: 5px;
            right: 5px;
        }

        .imgBox {
            float: left;
            position: relative;
        }

        .imgContainer {
            height: 200px !important;
            width: 200px;
            overflow: hidden !important;
        }

        .w100 {
            width: 100%;
        }

        .pad-5 {
            padding: 5px;
        }

        .border1 {
            border: 1px solid #ececec;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="pgTitle">Add Photos</h2>
    <span class="space15"></span>
    <div class="card card-primary">
        <div class="card-header">
            <h3 class="card-title">Add Photos to Album></h3>
        </div>
        <div class="card-body">
            <div class="col-sm-6">

                <div class="form-group">
                    <label>Select Product: </label>
                    <asp:DropDownList ID="ddrProduct" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddrProduct_SelectedIndexChanged">
                        <asp:ListItem Value="0"><- Select -></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label>Select Image : *</label>
                    <div class="input-group">
                        <asp:FileUpload ID="fuImg" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <span class="space10"></span>
    <%= errMsg %>
    <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary mr-2" Text="Submit" OnClick="btnSubmit_Click" />
    <a href="product-master.aspx" class="btn btn-outline-dark">Cancel</a>
    <div class="float_clear"></div>
    <span class="space20"></span>
    <div class="float_clear"></div>
    <div class="spacer"></div>
    <%=photoMarkup %>
    <div class="float_clear"></div>
</asp:Content>

