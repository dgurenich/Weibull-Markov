<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCDiscounting.ascx.cs" Inherits="TWeibullMarkov.UCDiscounting" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<style type="text/css">
    .style1
    {
        width: 100%;
    }
</style>
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
<asp:Panel ID="Panel1" runat="server" BorderColor="#006F53" BorderStyle="Solid" BorderWidth="2px"
   Width="934px" Height="52px">
    <table class="style1">
        <tr>
            <td colspan="2" style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #99BB99; font-weight: bold;">
                Discounting</td>
        </tr>
        <tr>
            <td nowrap="nowrap" align="left">
                Annual discount rate (percent)
                <telerik:RadNumericTextBox ID="RadNumericTextBox1" Runat="server" 
                    MaxValue="200" MinValue="0.01" ShowSpinButtons="True" Value="4.5" Width="70px">
                    <IncrementSettings Step="0.1" />
                    <EnabledStyle HorizontalAlign="Right" />
                </telerik:RadNumericTextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="RadNumericTextBox1" ErrorMessage="Missing value">Missing value</asp:RequiredFieldValidator>
            </td>
            <td nowrap="nowrap" align="right" style="padding-right: 10px">
                <telerik:RadButton ID="RadButton1" runat="server" CausesValidation="False" 
                    onclick="RadButton1_Click" Text="Compute Discounting Factor" 
                    ToolTip="Click to compute the discountig factor for the entered rate." 
                    Width="200px">
                </telerik:RadButton>
                Equivalent discounting factor:
                <asp:Label ID="LabelDiscFactor" runat="server" Font-Bold="True"></asp:Label></td>
        </tr>
    </table>
</asp:Panel>
</telerik:RadAjaxPanel>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin=""
    Transparency="50" Width="924" Height="20px" BackColor="#CCCCCC">
   <div align="center" style="padding: 10px; width: 920px; height: 50px;">
        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/LoadingProgressBar.gif"
            ImageAlign="Middle" />
    </div>
</telerik:RadAjaxLoadingPanel>