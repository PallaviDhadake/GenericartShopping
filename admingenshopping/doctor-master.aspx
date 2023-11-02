<%@ Page Title="Doctor Master page | Genericart Shopping Admin Master" Language="C#" MasterPageFile="~/admingenshopping/MasterAdmin.master" AutoEventWireup="true" CodeFile="doctor-master.aspx.cs" Inherits="admingenshopping_doctor_master" %>
<%@ MasterType VirtualPath="~/admingenshopping/MasterAdmin.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
        $(document).ready(function () {
            $('[id$=gvDoctor]').DataTable({
                columnDefs: [
                     { orderable: false, targets: [0, 2, 3, 6] }
                ],
                order: [[0, 'desc']]
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <h2 class="pgTitle">Doctor Master</h2>
    <span class="space15"></span>
    <div id="editDoctor" runat="server">
                <%--card start--%>
                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title"><%=pgTitle %></h3>
                    </div>
                    <%-- card body--%>
                    <div class="card-body">
                        <div class="colorLightBlue">
                            <span>Id :</span>
                            <asp:Label ID="lblId" runat="server" Text="[New]"></asp:Label>
                        </div>

                        <span class="space15"></span>
                        <%--form row start--%>
                        <div class="form-row">
                            <div class="form-group col-md-6">
                                <label>Name :*</label>
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Width="100%" MaxLength="30"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Mobile No :*</label>
                                <asp:TextBox ID="txtMobileNo" runat="server" CssClass="form-control" Width="100%" MaxLength="10"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Email Id :*</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Reg No. :*</label>
                                <asp:TextBox ID="txtRegNo" runat="server" CssClass="form-control" Width="100%" MaxLength="50"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>State :*</label>
                                <asp:DropDownList ID="ddrState" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddrState_SelectedIndexChanged">
                                    <asp:ListItem Value="0"><- Select -></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group col-md-6">
                                <label>District :*</label>
                                <asp:DropDownList ID="ddrDist" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddrDist_SelectedIndexChanged">
                                    <asp:ListItem Value="0"><- Select -></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group col-md-6">
                                <label>City :*</label>
                                <asp:DropDownList ID="ddrCity" runat="server" CssClass="form-control" Width="100%" >
                                    <asp:ListItem Value="0"><- Select -></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Pincode :</label>
                                <asp:TextBox ID="txtPinCode" runat="server" CssClass="form-control" Width="100%" MaxLength="15"></asp:TextBox>
                            </div>
                            
                            
                            <div class="form-group col-md-6">
                                <label>Address :</label>
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control textarea" TextMode="MultiLine" Width="100%" Height="150" MaxLength="200"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>About :</label>
                                <asp:TextBox ID="txtAbout" runat="server" CssClass="form-control textarea" Width="100%" TextMode="MultiLine" Height="150"></asp:TextBox>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Degree :*</label>
                                <asp:TextBox ID="txtDegree" runat="server" CssClass="form-control textarea" Width="100%"  MaxLength="50"></asp:TextBox>
                            </div>
                            
                            <div class="form-group col-md-6">
                                <label>Speciality :*</label>
                                <asp:DropDownList ID="ddrSpeciality" runat="server" CssClass="form-control" Width="100%">
                                    <asp:ListItem Value="0"><- Select -></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group col-md-6">
                                <label>Experience :*</label><span style="color:ButtonShadow; font-size:0.9em;"> (In Years)</span>
                                <%--<asp:TextBox ID="txtExp" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>--%>
                                <asp:DropDownList ID="ddrExperience" runat="server" CssClass="form-control" Width="100%">
                                    <asp:ListItem Value="0"><- Select -></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            
                            <div class="form-group col-md-6">
                                <label>Consultation Fees :*</label>
                                <asp:TextBox ID="txtFees" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                            </div>

                            <div class="form-group col-md-6">
                                <label>Photo: </label>
                                <div class="input-group">
                                    <asp:FileUpload ID="fuImg" runat="server" />
                                </div>
                            </div>
                            
                            <div class="form-check col-md-6">
                                <asp:CheckBox ID="chkFeatured" runat="server" TextAlign="Right" />
                                <label class="form-check-label" for="featured"><strong>Mark as Featured ?</strong></label>

                                <span class="space10"></span>

                                <asp:CheckBox ID="chkActive" runat="server" TextAlign="Right" />
                                <label class="form-check-label" for="active"><strong>Mark as Active ?</strong> </label>
                            </div>
                        </div>
                        <%--form row End--%>

                        <%= docImg %>
                    </div>
                    <%-- card body end--%>
                </div>
                <%--card end--%>

                <!-- Button controls starts -->
                <span class="space10"></span>
                <%=errMsg %>
                <span class="space10"></span>
                <asp:Button ID="btnSave" runat="server"
                    CssClass="btn btn-sm btn-primary" Text="Save" OnClick="btnSave_Click" />
                <asp:Button ID="btnDelete" runat="server"
                    CssClass="btn btn-sm btn-outline-info" Text="Delete" OnClientClick="return confirm('Are you sure to delete?');"
                    OnClick="btnDelete_Click" />
                <asp:Button ID="btnCancel" runat="server"
                    CssClass="btn btn-sm btn-outline-dark" Text="Cancel"
                    OnClick="btnCancel_Click" />
                <div class="float_clear"></div>
                <!-- Button controls ends -->
           
    </div>

    <!-- Gridview to show saved data starts here -->
    <div id="viewDoctor" runat="server">
        <a href="doctor-master.aspx?action=new" runat="server" class="btn btn-sm btn-primary">Add New</a>
        <span class="space15"></span>
        <div class="formPanel">
            <asp:GridView ID="gvDoctor" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive-sm" GridLines="None"
                AutoGenerateColumns="false" OnPageIndexChanging="gvDoctor_PageIndexChanging" OnRowDataBound="gvDoctor_RowDataBound" Width="100%">
                <RowStyle CssClass="" />
                <HeaderStyle CssClass="bg-dark" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="DoctorID">
                        <HeaderStyle CssClass="HideCol" />
                        <ItemStyle CssClass="HideCol" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DocName" HeaderText="Name">
                        <ItemStyle Width="20%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DocMobileNum" HeaderText="Mobile No">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DocDegree" HeaderText="Degree">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DocExperience" HeaderText="Experience">
                        <ItemStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ConsultationFees" HeaderText="Consultation Charges">
                        <ItemStyle Width="15%" />
                    </asp:BoundField>
                    <asp:TemplateField>
                        <ItemStyle Width="5%" />
                        <ItemTemplate>
                            <asp:Literal ID="litAnch" runat="server"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <span class="warning">Its Empty Here... :(</span>
                </EmptyDataTemplate>
                <PagerStyle CssClass="gvPager" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>

