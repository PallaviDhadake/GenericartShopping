<%@ Page Title="" Language="C#" MasterPageFile="~/GOBPDH/MasterGOBPDH.master" AutoEventWireup="true" CodeFile="gobpdh-detail.aspx.cs" Inherits="GOBPDH_gobpdh_detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .box {
            position: relative;
        }

        .total_price {
            position: absolute;
            bottom: -10px;
            right: 0;
            padding-right: 50px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">Order Details</h2>
    <span class="space15"></span>

    <div class="card card-primary">
        <div class="card-header">
            <h3 class="card-title">GOBP Registration Data</h3>
        </div>
        <div class="card-body">
            <table class="form_table">
                <tr>
                    <td><span class="colorLightBlue">Id :</span></td>
                    <td>
                        <asp:Label ID="lblId" CssClass="colorLightBlue" runat="server" Text="[New]"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 25%"><span class="formLable bold_weight">Join Date:</span></td>
                    <td><span class="formLable"><%= enqData[1] %></span> </td>
                </tr>
                <tr>
                    <td><span class="formLable bold_weight">Applicant Name :</span></td>
                    <td><span class="formLable" style="display: block; width: 60% !important;"><%= enqData[2] %></span> </td>
                </tr>
                <tr>
                    <td><span class="formLable bold_weight">GOBP Code :</span></td>
                    <td><span class="formLable"><%= enqData[8] %></span> </td>
                </tr>
                 <tr>
                    <td><span class="formLable bold_weight">Employee Code :</span></td>
                    <td><span class="formLable"><%= enqData[14] %></span> </td>
                </tr>
                <tr>
                    <td><span class="formLable bold_weight">Mobile No :</span></td>
                    <td><span class="formLable"><%= enqData[12] %></span> </td>
                </tr>
                <tr>
                    <td><span class="formLable bold_weight">Email Id :</span></td>
                    <td><span class="formLable"><%= enqData[13] %></span> </td>
                </tr>
            </table>
        </div>
    </div>

    <div class="card card-primary">
        <div class="card-header">
            <h3 class="card-title">GOBP Order Details</h3>
        </div>
        <div class="card-body">
            <asp:GridView ID="gvOrder" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvOrder_RowDataBound" Width="100%"
                CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None">
                <HeaderStyle CssClass="bg-dark" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="OrderID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OrderStatus">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Date" HeaderText="Date">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OrderID" HeaderText="Order ID">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Name" HeaderText="Customer Name">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Items" HeaderText="Total Items">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OrderAmount" HeaderText="Order Amount">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemStyle Width="10%" />
                        <ItemTemplate>
                            <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <span class="warning">No GOBP Enquries to Display</span>
                </EmptyDataTemplate>
                <PagerStyle CssClass="gvPager" />
            </asp:GridView>
        </div>
    </div>

    <span class="space20"></span>
        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-md btn-dark" Text="Back" OnClick="btnCancel_Click" />

</asp:Content>

