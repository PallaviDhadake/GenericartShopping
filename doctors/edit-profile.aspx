<%@ Page Title="Edit Profile" Language="C#" MasterPageFile="~/doctors/MasterDoctor.master" AutoEventWireup="true" CodeFile="edit-profile.aspx.cs" Inherits="doctors_edit_profile" %>
<%@ MasterType VirtualPath="~/doctors/MasterDoctor.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="pgTitle">Edit Profile</h2>
    <span class="space15"></span>
    <div id="editProfile" runat="server">
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
                        <asp:DropDownList ID="ddrCity" runat="server" CssClass="form-control" Width="100%">
                            <asp:ListItem Value="0"><- Select -></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Pincode :</label>
                        <asp:TextBox ID="txtPinCode" runat="server" CssClass="form-control" Width="100%" MaxLength="15"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-6">
                                
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
                        <asp:TextBox ID="txtDegree" runat="server" CssClass="form-control textarea" Width="100%" MaxLength="50"></asp:TextBox>
                    </div>

                    <div class="form-group col-md-6">
                        <label>Speciality :*</label>
                        <asp:DropDownList ID="ddrSpeciality" runat="server" CssClass="form-control" Width="100%">
                            <asp:ListItem Value="0"><- Select -></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Experience :*</label><span style="color: ButtonShadow; font-size: 0.9em;"> (In Years)</span>
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
        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-sm btn-primary" Text="Modify" OnClick="btnSave_Click" />
        
        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-sm btn-outline-dark" Text="Cancel" OnClick="btnCancel_Click" />
        <div class="float_clear"></div>
        <!-- Button controls ends -->
    </div>
</asp:Content>

