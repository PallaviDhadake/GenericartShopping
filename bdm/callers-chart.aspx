<%@ Page Title="Caller Chart | Online Sales Team Head" Language="C#" MasterPageFile="~/bdm/MasterBdm.master" AutoEventWireup="true" CodeFile="callers-chart.aspx.cs" Inherits="bdm_callers_chart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <style type="text/css">
        .Space label
        {
            margin-left: 5px;
            margin-right: 15px;
			
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <h2 class="pgTitle">Callers Performance Chart</h2>
    <span class="space15"></span>
    <div class="row">
        <div class="form-group col-md-6">
                <label>Callers Team : </label>
                     <asp:DropDownList ID="ddrCallers" runat="server" CssClass="form-control" Width="100%" AutoPostBack="false" >
                        <asp:ListItem Value="0"><- All Callers -></asp:ListItem>
                    </asp:DropDownList>
                <span class="space5"></span>
         </div>
        <div class="form-group col-md-6">
            <label>Calling Type :</label><span class="space10"></span>
            <asp:RadioButton ID="rdbAll" runat="server" GroupName="callType" Text=" All Calls" CssClass="radio Space" Checked="true" />
            <asp:RadioButton ID="rdbConverted" runat="server" GroupName="callType" Text=" Order Converted Calls" CssClass="radio Space" />
        </div>

        <span class="space10"></span>
        <div class="form-group col-md-6">
            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Show Chart" OnClick="btnSave_Click"  />
        </div>
        <span class="space10"></span>

        <asp:Chart ID="chartPerform" runat="server" Width="800">
            <Series>
                <asp:Series Name="Total Calls Count" IsValueShownAsLabel="true" LabelBackColor="139, 199, 62" Color="0, 175, 239"></asp:Series>
            </Series>
            <Legends>
                <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="False" Name="Default" LegendStyle="Row" />
            </Legends>
            <ChartAreas>
                <asp:ChartArea Name="Month"></asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
    </div>
</asp:Content>

