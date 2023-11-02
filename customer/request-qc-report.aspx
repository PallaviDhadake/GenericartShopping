<%@ Page Title="Request QC Report" Language="C#" MasterPageFile="~/customer/MasterCustomer.master" AutoEventWireup="true" CodeFile="request-qc-report.aspx.cs" Inherits="customer_request_qc_report" %>
<%@ MasterType VirtualPath="~/customer/MasterCustomer.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Request QC Starts -->
        <div class="width50" id="qc">
            <div class="pad_10">
                <div class="bgWhite border_r_5 box-shadow">
                    <div class="pad_20 posRelative">
                        <h2 class="clrLightBlack semiBold semiMedium">Request QC Report</h2>
                        <span class="space20"></span>
                        <%= ordStr %>
                        <%--<h2 class="clrLightBlack semiBold">Request Number : #11</h2>
                        <span class="lineSeperator"></span>

                        <div class="float_left width70">
                            <span class="clrGrey fontRegular tiny dispBlk mrg_B_3">Request Date : 10/10/2020</span>
                            <span class="clrGrey fontRegular tiny dispBlk">contains Crocin, dettol. <br />Lorem Ipsum is simply dummy text of the printing and typesetting industry.</span>
                        </div>
                        <div class="float_right">
                            <span class="tiny clrGrey fontRegular">Amt.</span>
                            <span class="regular clrLightBlack semiBold">&#8377; 359</span>
                        </div>
                        <div class="float_clear"></div>--%>

                        <span class="lineSeperator"></span>

                        <h2 class="pageH3 clrBlack semiBold mrg_B_10">Requested Products</h2>
                        <div id="prodGrid">
                            <asp:GridView ID="gvProducts" runat="server"
                                CssClass="gvApp" GridLines="None"
                                AutoGenerateColumns="false" Width="100%" OnRowDataBound="gvProducts_RowDataBound" OnRowCommand="gvProducts_RowCommand">
                                <RowStyle CssClass="" />
                                <HeaderStyle CssClass="bg-dark" />
                                <AlternatingRowStyle CssClass="alt" />
                                <Columns>
                                    <asp:BoundField DataField="ProductID">
                                        <HeaderStyle CssClass="HideCol" />
                                        <ItemStyle CssClass="HideCol" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ProductName" HeaderText="Product">
                                        <ItemStyle Width="50%" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Batch No.">
                                        <ItemStyle Width="10%" />
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtBatchNo" CssClass="textBox" MaxLength="100" Width="100" runat="server"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemStyle Width="5%" />
                                        <ItemTemplate>
                                            <asp:Button ID="cmdRequest" runat="server" CssClass="reqBtn" CommandName="gvRequest" Text="Request QC Report" />
                                            <asp:Button ID="cmdDownload" runat="server" CssClass="downloadBtn" CommandName="gvDownload" Text="Download Report" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <span class="warning">No Data to Display :(</span>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- Request QC Ends -->
</asp:Content>

