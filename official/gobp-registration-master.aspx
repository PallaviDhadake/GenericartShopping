<%@ Page Title="" Language="C#" MasterPageFile="~/official/MasterOfficial.master" AutoEventWireup="true" CodeFile="gobp-registration-master.aspx.cs" Inherits="official_gobp_registration_master" %>
<%@ MasterType VirtualPath="~/official/MasterOfficial.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <script type="text/javascript">
         $(function () {
             var prm = Sys.WebForms.PageRequestManager.getInstance();
             if (prm != null) {
                 prm.add_endRequest(function (sender, e) {
                     if (sender._postBackSettings.panelsToUpdate != null) {
                         createDataTable();
                     }
                 });
             };

             createDataTable();
             function createDataTable() {
                 $('#<%= gvGOBP.ClientID %>').prepend($("<thead></thead>").append($('#<%= gvGOBP.ClientID %>').find("tr:first"))).DataTable({
                columnDefs: [
                    { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7, 8] }
                ],
                order: [[0, 'desc']]
            });

        }
    });
</script>

    <style>
        .photoSize-gobp {
        	width: 140px;
        	height: 180px;
        	border: 2px solid #000;
        	position: absolute;
        	top: 80px;
        	right: 20px;
            margin-top: 30px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <h2 class="pgTitle">New GOBP Registration</h2>
    <span class="space15"></span>

    <div id="viewFrEnquiry" runat="server">
        <a href="<%= Master.rootPath + "gobp-registration.aspx" %>" target="_blank" class="btn btn-primary">Add New</a>
        <span class="space15"></span>

        <div class="">
            <asp:GridView ID="gvGOBP" runat="server" AutoGenerateColumns="False"
                CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
                OnRowDataBound="gvGOBP_RowDataBound">
                <HeaderStyle CssClass="bg-dark" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="OBP_ID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="joinDate" HeaderText="Join Date">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OBP_ApplicantName" HeaderText="Applicant Name">
                        <ItemStyle Width="25%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OBP_UserID" HeaderText="GOBP User Id">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OBPAmount" HeaderText="OBP Fees">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>                   
                    <asp:BoundField DataField="OBP_City" HeaderText="City">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OBP_MobileNo" HeaderText="Mobile No">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                  <%--  <asp:BoundField DataField="OBP_UserID" HeaderText="User ID">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>--%>
                    <%--<asp:BoundField DataField="OBP_StatusFlag" HeaderText="Status">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>--%>
                    <asp:TemplateField>
                        <ItemStyle Width="5%" />
                        <ItemTemplate>
                            <asp:Literal ID="litView" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemStyle Width="5%" />
                        <ItemTemplate>
                            <asp:Literal ID="litEdit" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="Sync With Ecom">
                    <ItemStyle Width="10%" />
                    <ItemTemplate>
                        <asp:Literal ID="litEcom" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>--%>

                </Columns>
                <EmptyDataTemplate>
                    <span class="warning">No GOBP Enquries to Display</span>
                </EmptyDataTemplate>
                <PagerStyle CssClass="gvPager" />
            </asp:GridView>
        </div>
    </div>

    <div id="readFrEnquiry" runat="server">
        <div class="formPanel posRelative">
            <h4 class="formTitle themeDarkBg">Read GOBP Registration Data</h4>
            <span class="titleLine"></span>
            <div class="pad_10 ">
                <div id="photo" runat="server" visible="false" style="background:#ccc;">
                    <%--<span class="space20"></span>--%>
                    <div class="photoSize-gobp">
                        <p class="txtCenter" style="padding:60px 0; font-size:0.9em; text-align:center">Paste a <br />Photograph & do cross sign</p>
                    </div>
                    <div style="position: absolute; top:280px; right:20px; width:28%; border:2px solid #000; margin-top: 25px;">
                        <div class="pad_10">
                            <span class="bold_weight dspBlk small mrgBtm10">Name : <%= enqData[2] %></span>
                            <br />
                            <br />
                            <span class="space10"></span>
                            <span class="bold_weight dspBlk small mrgBtm10">Signature : </span>
                        </div>
                    </div>
                </div>
                <%--<div class="<%= clsName %>">
                    <div class="pad_15 ">
                        <%= headInfo %>
                    </div>
                </div>--%>
                <table class="form_table">
                    <tr>
                        <td><span class="colorLightBlue">Id :</span></td>
                        <td><asp:Label ID="lblId" CssClass="colorLightBlue" runat="server" Text="[New]"></asp:Label></td>
                    </tr>
                    <tr>
                        <td><span class="text-lg text-bold text-purple">Type:</span></td>
                        <td><span class="text-lg text-bold text-purple"><%= enqData[0] %></span></td>
                    </tr>
                    <tr>
                        <td style="width:25%" ><span class="formLable bold_weight">Join Date:</span></td>
                        <td ><span class="formLable"><%= enqData[1] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">GOBP Code :</span></td>
                        <td><span class="formLable"><%= enqData[8] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Applicant Name :</span></td>
                        <td><span class="formLable" style="display:block; width:60% !important;"><%= enqData[2] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Firm Name :</span></td>
                        <td><span class="formLable" style="display:block; width:60% !important;"><%= enqData[3] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Firm Type :</span></td>
                        <td><span class="formLable"><%= enqData[4] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Address :</span></td>
                        <td><span class="formLable" style="display:block; width:60% !important;"><%= enqData[5] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Date Of Birth :</span></td>
                        <td><span class="formLable"><%= enqData[6] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Age :</span></td>
                        <td><span class="formLable"><%= enqData[7] %></span> </td>
                    </tr>
                    
                    <tr>
                        <td><span class="formLable bold_weight">State :</span></td>
                        <td><span class="formLable"><%= enqData[9] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">District :</span></td>
                        <td><span class="formLable"><%= enqData[10] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">City :</span></td>
                        <td><span class="formLable"><%= enqData[11] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Mobile No :</span></td>
                        <td><span class="formLable"><%= enqData[12] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Email Id :</span></td>
                        <td><span class="formLable"><%= enqData[13] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Whatsapp No. :</span></td>
                        <td><span class="formLable"><%= enqData[14] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Education :</span></td>
                        <td><span class="formLable"><%= enqData[15] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Present Occupation of Owner :</span></td>
                        <td><span class="formLable"><%= enqData[16] %></span> </td>
                    </tr>
                    <%--<tr>
                        <td><span class="formLable bold_weight">Annual Income :</span></td>
                        <td><span class="formLable"><%= enqData[20] %></span> </td>
                    </tr>--%>
                    <tr>
                        <td><span class="formLable bold_weight">Any Illegal Matter :</span></td>
                        <td><span class="formLable"><%= enqData[17] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Residence From :</span></td>
                        <td><span class="formLable"><%= enqData[18] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Marital Status :</span></td>
                        <td><span class="formLable"><%= enqData[19] %></span> </td>
                    </tr>
                    <%--<tr>
                        <td><span class="formLable bold_weight">Distance From Nearest Existing Franchisee :</span></td>
                        <td><span class="formLable"><%= enqData[24] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Ready To Invest 8-10 Lakhs :</span></td>
                        <td><span class="formLable"><%= enqData[25] %></span> </td>
                    </tr>--%>

                    <!--New Payment Details-->
                    <tr>
                        <td><span class="formLable bold_weight">UTR NO:</span></td>
                        <td><span class="formLable"><%= enqData[20] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Bank Name:</span></td>
                        <td><span class="formLable"><%= enqData[21] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Transaction Date:</span></td>
                        <td><span class="formLable"><%= enqData[22] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Account Holder Name:</span></td>
                        <td><span class="formLable"><%= enqData[23] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Paid Amount:</span></td>
                        <td><span class="formLable"><%= enqData[24] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Address Proof :</span></td>
                        <td><span class="formLable"><%= enqData[25] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Id Proof :</span></td>
                        <td><span class="formLable"><%= enqData[26] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Profile :</span></td>
                        <td><span class="formLable"><%= enqData[27] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">Resume :</span></td>
                        <td><span class="formLable"><%= enqData[28] %></span> </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <%= enqData[35] %>
                        </td>
                    </tr>
                </table>
            </div>
        </div>

        <span class="space20"></span>

        <%--<asp:Button ID="btnSave" runat="server" CssClass="buttonBlue float_left mrgRgt10" Text="Mark as Read / Unread" OnClick="btnSave_Click" />
        <%=editAnch %>--%>
        <asp:Button ID="btnPrint" runat="server" 
            CssClass="buttonBlue float_left mrgRgt10" Text="Print Preview" 
            onclick="btnPrint_Click" />
        

        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-dark" Text="Cancel" OnClick="btnCancel_Click" />
        
        <span class="space30"></span>
    </div>
</asp:Content>

