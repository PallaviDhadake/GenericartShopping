<%@ Page Title="" Language="C#" MasterPageFile="~/districthead/MasterDH.master" AutoEventWireup="true" CodeFile="registered-gobp.aspx.cs" Inherits="districthead_registered_gobp" %>

<%@ MasterType VirtualPath="~/districthead/MasterDH.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <script>
        $(document).ready(function () {

            $('[id$=gvGOBP]').DataTable({
                columnDefs: [
                     { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7] }
                ],
                order: [[0, 'desc']]
            });
        });
     </script>
    <style>
        #followup .user-block .username, #followup .user-block .description {
            margin: 0px !important;
        }
        .gobpTbl {border-collapse: collapse; width: 100%;}
        .gobpTbl td, th {border: 1px solid #716868;  text-align: left; padding: 15px;}
        .gobpTbl th{background:#8141ad; color:#ffffff}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <h2 class="pgTitle">Registered GOBP List</h2>
    <span class="space15"></span>

     <div id="viewFrEnquiry" runat="server">
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
                        <ItemStyle Width="20%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OBP_MobileNo" HeaderText="Mobile No">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="custCount" HeaderText="Total Customers">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="dh" HeaderText="DH">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                 <%--   <asp:BoundField DataField="dhContact" HeaderText="DH Contact">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>--%>
                    <asp:BoundField DataField="OBP_StatusFlag" HeaderText="Status">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                <%--    <asp:BoundField DataField="flCount" HeaderText="FL Count">
                        <ItemStyle Width="5%" />
                    </asp:BoundField>--%>
                    <asp:TemplateField>
                        <ItemStyle Width="5%" />
                        <ItemTemplate>
                            <asp:Literal ID="litView" runat="server"></asp:Literal>
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

    <div id="readFrEnquiry" runat="server">
        <div class="card card-primary">
            <div class="card-header">
                <h3 class="card-title">GOBP Registration Data</h3>
            </div>
            <div class="card-body">
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
                        <td><span class="formLable bold_weight">Applicant Name :</span></td>
                        <td><span class="formLable" style="display:block; width:60% !important;"><%= enqData[2] %></span> </td>
                    </tr>
                    <tr>
                        <td><span class="formLable bold_weight">GOBP Code :</span></td>
                        <td><span class="formLable"><%= enqData[8] %></span> </td>
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
                        <td><span class="formLable bold_weight">DH Name :</span></td>
                        <td><span class="formLable"><%= enqData[4] %></span> </td>
                    </tr>
                    <%-- <tr>
                        <td><span class="formLable bold_weight">DH Contact No:</span></td>
                        <td><span class="formLable"><%= enqData[5] %></span> </td>
                    </tr>--%>
                     <tr>
                        <td><span class="formLable bold_weight">Total No. Of Customers :</span></td>
                        <td><span class="formLable"><%= enqData[3] %></span> </td>
                    </tr>
                </table>
               
            </div>
        </div>
        <span class="space20"></span>
     

        <div class="">
           <%-- <span class="semiBold semiMedium">Pallavi Ramesh Dhadake</span>
            <span class="space15"></span>
            <span class="">8959564852</span>
            <span class="space15"></span>--%>
            <table class="gobpTbl">
                <tr>
                    <th class=""></th>
                    <th>Customers</th>
                    <th>Commission</th>
                </tr>
                <tr>
                    <td class="bold_weight">Total</td>
                    <td><%= custcountData[0] %></td>   <!--Customer -->
                    <td><%= custcountData[3] %></td> <!-- Commission -->
                </tr>
                <tr>
                    <td class="bold_weight">This Month</td>
                    <td><%= custcountData[1] %></td> <!--Customer -->
                    <td><%= custcountData[4] %></td> <!-- Commission -->
                </tr>
                  <tr>
                    <td class="bold_weight">Last Month</td>
                    <td><%= custcountData[2] %></td> <!--Customer -->
                    <td><%= custcountData[5] %></td> <!-- Commission -->
                </tr>
            </table>

             <span class="space30"></span>

               <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-md btn-dark" Text="Cancel" OnClick="btnCancel_Click" />
        
       
        </div>
    </div>

</asp:Content>

