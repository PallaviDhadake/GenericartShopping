<%@ Page Title="Follow-Up Form | Genericart Shopping Support Team" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="staff-followup-form.aspx.cs" Inherits="supportteam_staff_followup_form" %>
<%@ MasterType VirtualPath="~/supportteam/MasterSupport.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<style>
		.bg-malta-blue{background-color:#B7C9D8;}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<div id="editFollowUpForm" runat="server">
		<asp:UpdatePanel ID="UpdatePanel1" runat="server">
			<ContentTemplate>
				<%--card start--%>
				<div class="card card-primary">
					<div class="card-header">
						<h3 class="card-title">Add FeedBack</h3>
					</div>
					<%-- card body--%>
					<div class="card-body">
						<%--form row start--%>
						<div class="row">
							<div class="col-md-6">
								<div class="form-group">
									<label>Ratings :*</label>
									<span class="space5"></span>

									<asp:RadioButton ID="RadioButton1" runat="server" GroupName="ratings" />
									<label class="form-check-label mr-1" for="RadioButton1"><strong>Excellent</strong></label>

									<asp:RadioButton ID="RadioButton2" runat="server" GroupName="ratings" />
									<label class="form-check-label mr-1" for="RadioButton2"><strong>Very Good</strong></label>

									<asp:RadioButton ID="RadioButton3" runat="server" GroupName="ratings" />
									<label class="form-check-label mr-1" for="RadioButton3"><strong>Good</strong></label>

									<asp:RadioButton ID="RadioButton4" runat="server" GroupName="ratings" />
									<label class="form-check-label mr-1" for="RadioButton4"><strong>Fair</strong></label>

									<asp:RadioButton ID="RadioButton5" runat="server" GroupName="ratings" />
									<label class="form-check-label mr-1" for="RadioButton5"><strong>Poor</strong></label>

									<asp:RadioButton ID="RadioButton6" runat="server" GroupName="ratings" />
									<label class="form-check-label mr-1" for="RadioButton6"><strong>Very Poor</strong></label>

								</div>

								<div class="form-group">
									<label for="testimonials">Feedback:*</label>
									<asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" Width="100%" Height="100" CssClass="form-control textarea"></asp:TextBox>
								</div>

								<div class="form-group">
									<asp:CheckBox ID="chkFranch" runat="server" TextAlign="Right" Text=" Interested For Franchisee ?" />
								</div>
							</div>
							<%--customer details--%>
							<div class="col-md-6">

								<div class="card card-widget widget-user-2">
									<!-- Add the bg color to the header using any of the bg-* classes -->
									<div class="widget-user-header bg-malta-blue">
									 
										<h3 class=""><%= taskName %></h3>
										<h5 class="">Customer Details</h5>
									</div>
									<div class="card-footer p-0">
										<ul class="nav flex-column">
											<li class="nav-item">
												<div class="nav-link">Customer Name <span class="ml-2"><%= custumerInfo[0] %></span></div> 
											</li>
											<li class="nav-item">
												 <div class="nav-link">Customer Mobile <span class="ml-2"><%= custumerInfo[1] %></span></div> 
											</li>
                                            <li class="nav-item">
												 <div class="nav-link"><a href="<%= custLookupLink %>" class="btn btn-md btn-info lookup">More Info</a></div> 
											</li>
										</ul>
									</div>
								</div>
								<span class="space20"></span>
								<asp:LinkButton runat="server" ID="btnCall" CssClass="btn btn-lg btn-success" OnClick="btnCall_Click">
									<i class="fas fa-phone-alt" aria-hidden="true"></i>&nbsp;&nbsp; Call Now
								</asp:LinkButton>
								<span class="space10"></span>
								<%= apiResponse %>
							</div>
								<%--customer details end--%>
							<span class="space20"></span>
							<a href="<%= poUrl %>" target="_blank" class="btn btn-md btn-info">Submit PO</a>
						</div>
						<%--form row End--%>
					</div>
					<%-- card body end--%>
				</div>
				<%--card end--%>

				<!-- Button controls starts -->
				<span class="space10"></span>
			  <%--  <%=errMsg %>--%>
				<span class="space10"></span>
				<asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click"/>
				<asp:Button ID="btnCancel" runat="server"
					CssClass="btn btn-outline-dark" Text="Cancel" OnClick="btnCancel_Click"/>
				<div class="float_clear"></div>
				<!-- Button controls ends -->
			</ContentTemplate>
		</asp:UpdatePanel>
	</div>

    <script type="text/javascript">
         $(document).ready(function () {
             $("a.lookup").fancybox({
                 type: 'iframe'
             });
         });
    </script>
</asp:Content>

