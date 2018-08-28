<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCFailureCost.ascx.cs"
    Inherits="TWeibullMarkov.UCFailureCost" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<style type="text/css">
    
   
    
</style>
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
    <asp:Panel ID="Panel1" runat="server" BorderColor="#006F53" BorderStyle="Solid" BorderWidth="2px"
        Width="934px" Height="78">
        <table width="100%">
            <tr>
                <td style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #99BB99;
                    font-weight: bold;" colspan="2">
                    Unit Failure Cost
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:RadioButton ID="RadioButton1" runat="server" Checked="True" GroupName="rbGroupFailCost"
                        Text="Let model estimate" ToolTip="Select this option if you want the model to estimate the minimum failure cost that will trigger an improvement action in the worst condition state starting from year one."
                        AutoPostBack="True" OnCheckedChanged="RadioButton1_CheckedChanged" />
                </td>
            </tr>
            <tr>
                <td nowrap="nowrap" valign="middle">
                    <asp:RadioButton ID="RadioButton2" runat="server" GroupName="rbGroupFailCost" Text="Use this dollar value for the failure cost: "
                        ToolTip="Select this option if you want the model to use the specific unit failure cost (in dollars) that you must enter."
                        AutoPostBack="True" OnCheckedChanged="RadioButton2_CheckedChanged" />
                        <telerik:RadNumericTextBox ID="RadNumericTextBox1" runat="server" MaxValue="10000000"
                        MinValue="0" ShowSpinButtons="True" Value="1" ToolTip="Please enter the unit failure cost here  in dollars.">
                        <DisabledStyle ForeColor="#666666" />
                        <EnabledStyle HorizontalAlign="Right" />
                    </telerik:RadNumericTextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ControlToValidate="RadNumericTextBox1" ErrorMessage="Missing value">Missing value</asp:RequiredFieldValidator>
                </td>
                <td align="right" nowrap="nowrap" valign="middle" style="padding-right: 10px">
                    <asp:CheckBox ID="CheckBox1" runat="server" 
                        Text="Model may override the entered  failure cost" 
                        ToolTip="Check this box if you want to allow the model to increase the entered value of the unit faulre cost if it is not sufficient to trigger an action in the last condition state.  This option is not available together with the 'Let model estimate' option." />
                </td>
            </tr>
        </table>
    </asp:Panel>
</telerik:RadAjaxPanel>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="" Transparency="50"
    Width="934" Height="78" BackColor="#CCCCCC">
    <table width="100%">
        <tr>
            <td height="78" align="center" valign="middle">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/LoadingProgressBar.gif"
                    ImageAlign="Middle"/>
            </td>
        </tr>
    </table>
</telerik:RadAjaxLoadingPanel>
