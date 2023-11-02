<%@ Page Title="" Language="C#" MasterPageFile="~/supportteam/MasterSupport.master" AutoEventWireup="true" CodeFile="sampleData.aspx.cs" Inherits="supportteam_sampleData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   
    <script type="text/javascript">
        $(document).ready(function () {
            //Once the document is ready call Bind DataTable
            BindDataTable()
        });

        function BindDataTable() {
            $('#tblDataTable').DataTable({
                "processing": true,
                "serverSide": true,
                "ajax": {
                    url: 'sampleData.aspx/GetDataForDataTable',
                    type: 'POST'
                    
                },
                "columns": [
                    { "data": "CustomerName" },
                    { "data": "CustomerMobile" },
                ]
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
            <!--Structure of the table with only header-->
            <table id="tblDataTable" class="display">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Phone Number</th>
                       
                    </tr>      
                </thead>
            </table>
        </div>
</asp:Content>

