<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCActionPane.ascx.cs"
    Inherits="TWeibullMarkov.UCActionPane" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<style type="text/css">
 
</style>
<telerik:RadAjaxPanel ID="RadAjaxPanelAction" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
    <asp:Panel ID="Panel1" runat="server" BorderColor="#006F53" BorderStyle="Solid" BorderWidth="2px"
        Height="243px" Width="464px">
        <table>
            <tr>
                <td style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #99BB99;
                    font-weight: bold;" colspan="7">
                    <asp:Label ID="LabelAction" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td rowspan="2" valign="bottom" style="border-bottom-style: solid; border-bottom-width: thin;
                    border-bottom-color: #99BB99; border-right-style: solid; border-right-width: thin;
                    border-right-color: #99BB99;" width="50">
                    Applica-bility to condition states
                </td>
                <td style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #99BB99;
                    border-right-style: solid; border-right-width: thin; border-right-color: #99BB99;"
                    rowspan="2" valign="bottom">
                    Unit cost ($)
                </td>
                <td colspan="4" style="border-right: thin solid #99BB99; border-bottom: thin solid #99BB99;
                    text-align: left;" align="center">
                    Probability of transitioning to other condition states when action is taken (percent)
                </td>
                <td style="border-style: none;" width="110px" valign="bottom">
                    <telerik:RadButton ID="RadButton1" runat="server" Text="Validate" 
                        
                        ToolTip="Click to validate the inputs before submitting them for the optimal policy generation." 
                        Width="100%" CausesValidation="False" onclick="RadButton1_Click">
                    </telerik:RadButton>
                </td>
            </tr>
            <tr>
                <td style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #99BB99;
                    border-right-style: solid; border-right-width: thin; border-right-color: #99BB99;
                    padding-left: 20px;">
                    1
                </td>
                <td style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #99BB99;
                    border-right-style: solid; border-right-width: thin; border-right-color: #99BB99;
                    padding-left: 20px;">
                    2
                </td>
                <td style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #99BB99;
                    border-right-style: solid; border-right-width: thin; border-right-color: #99BB99;
                    padding-left: 20px;">
                    3
                </td>
                <td style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #99BB99;
                    border-right-style: solid; border-right-width: thin; border-right-color: #99BB99;
                    padding-left: 20px;">
                    4
                </td>
                <td style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #99BB99" 
                    valign="bottom" width="110px">
                    <asp:Label ID="errSumLabel" runat="server" ForeColor="Red" Text="Errors:"></asp:Label>
                    <asp:Label ID="okLabel" runat="server" ForeColor="DarkGreen" Text="No errors"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" style="border-bottom-style: solid; border-right-style: solid;
                    border-right-width: thin; border-bottom-width: thin; border-right-color: #99BB99;
                    border-bottom-color: #99BB99" height="32">
                    <asp:CheckBox ID="CheckBox1" runat="server" Text="1" ToolTip="Please check this box is if action can be applied to the condition state.  Leave it un-checked otherwise."
                        OnCheckedChanged="CheckBox1_CheckedChanged" AutoPostBack="True" />
                </td>
                <td style="border-bottom-style: solid; border-right-style: solid; border-right-width: thin;
                    border-bottom-width: thin; border-right-color: #99BB99; border-bottom-color: #99BB99">
                    <telerik:RadNumericTextBox ID="RadCostTextBox1" runat="server" Width="80px" ShowSpinButtons="True"
                        MaxValue="10000000" MinValue="1" ToolTip="If action can be applied to the condition state please enter its unit agency cost here.">
                        <DisabledStyle ForeColor="#666666" />
                        <EnabledStyle HorizontalAlign="Right" />
                    </telerik:RadNumericTextBox>
                </td>
                <td style="border-bottom-style: solid; border-right-style: solid; border-right-width: thin;
                    border-bottom-width: thin; border-right-color: #99BB99; border-bottom-color: #99BB99">
                    <telerik:RadNumericTextBox ID="RadProbTextBox11" runat="server" Width="50px" ShowSpinButtons="True"
                        MaxValue="100" MinValue="0" ToolTip="Please enter the probability of transitioning to condition state 1 if action is applied.">
                        <NumberFormat DecimalDigits="1" />
                        <DisabledStyle ForeColor="#666666" />
                        <EnabledStyle HorizontalAlign="Right" />
                    </telerik:RadNumericTextBox>
                </td>
                <td style="border-bottom-style: solid; border-right-style: solid; border-right-width: thin;
                    border-bottom-width: thin; border-right-color: #99BB99; border-bottom-color: #99BB99">
                    <telerik:RadNumericTextBox ID="RadProbTextBox12" runat="server" MaxValue="100" MinValue="0"
                        ShowSpinButtons="True" Width="50px" ToolTip="Please enter the probability of transitioning to condition state 2 if action is applied.">
                        <NumberFormat DecimalDigits="1" />
                        <DisabledStyle ForeColor="#666666" />
                        <EnabledStyle HorizontalAlign="Right" />
                    </telerik:RadNumericTextBox>
                </td>
                <td style="border-bottom-style: solid; border-right-style: solid; border-right-width: thin;
                    border-bottom-width: thin; border-right-color: #99BB99; border-bottom-color: #99BB99">
                    <telerik:RadNumericTextBox ID="RadProbTextBox13" runat="server" MaxValue="100" MinValue="0"
                        ShowSpinButtons="True" Width="50px" ToolTip="Please enter the probability of transitioning to condition state 3 if action is applied.">
                        <NumberFormat DecimalDigits="1" />
                        <DisabledStyle ForeColor="#666666" />
                        <EnabledStyle HorizontalAlign="Right" />
                    </telerik:RadNumericTextBox>
                </td>
                <td style="border-bottom-style: solid; border-right-style: solid; border-right-width: thin;
                    border-bottom-width: thin; border-right-color: #99BB99; border-bottom-color: #99BB99">
                    <telerik:RadNumericTextBox ID="RadProbTextBox14" runat="server" MaxValue="100" MinValue="0"
                        ShowSpinButtons="True" Width="50px" ToolTip="Please enter the probability of transitioning to condition state 4 if action is applied.">
                        <NumberFormat DecimalDigits="1" />
                        <DisabledStyle ForeColor="#666666" />
                        <EnabledStyle HorizontalAlign="Right" />
                    </telerik:RadNumericTextBox>
                </td>
                <td style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #99BB99">
                    <asp:Label ID="errLabel1" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" style="border-bottom-style: solid; border-right-style: solid;
                    border-right-width: thin; border-bottom-width: thin; border-right-color: #99BB99;
                    border-bottom-color: #99BB99" height="32">
                    <asp:CheckBox ID="CheckBox2" runat="server" Text="2" OnCheckedChanged="CheckBox2_CheckedChanged"
                        ToolTip="Please check this box is if action can be applied to the condition state.  Leave it un-checked otherwise. "
                        AutoPostBack="True" />
                </td>
                <td style="border-bottom-style: solid; border-right-style: solid; border-right-width: thin;
                    border-bottom-width: thin; border-right-color: #99BB99; border-bottom-color: #99BB99">
                    <telerik:RadNumericTextBox ID="RadCostTextBox2" runat="server" MaxValue="10000000"
                        MinValue="1" ShowSpinButtons="True" ToolTip="If action can be applied to the condition state please enter its unit agency cost here."
                        Width="80px">
                        <DisabledStyle ForeColor="#666666" />
                        <EnabledStyle HorizontalAlign="Right" />
                    </telerik:RadNumericTextBox>
                </td>
                <td style="border-bottom-style: solid; border-right-style: solid; border-right-width: thin;
                    border-bottom-width: thin; border-right-color: #99BB99; border-bottom-color: #99BB99">
                    <telerik:RadNumericTextBox ID="RadProbTextBox21" runat="server" MaxValue="100" MinValue="0"
                        ShowSpinButtons="True" ToolTip="Please enter the probability of transitioning to condition state 1 if action is applied."
                        Width="50px">
                        <NumberFormat DecimalDigits="1" />
                        <DisabledStyle ForeColor="#666666" />
                        <EnabledStyle HorizontalAlign="Right" />
                    </telerik:RadNumericTextBox>
                </td>
                <td style="border-bottom-style: solid; border-right-style: solid; border-right-width: thin;
                    border-bottom-width: thin; border-right-color: #99BB99; border-bottom-color: #99BB99">
                    <telerik:RadNumericTextBox ID="RadProbTextBox22" runat="server" MaxValue="100" MinValue="0"
                        ShowSpinButtons="True" ToolTip="Please enter the probability of transitioning to condition state 2 if action is applied."
                        Width="50px">
                        <NumberFormat DecimalDigits="1" />
                        <DisabledStyle ForeColor="#666666" />
                        <EnabledStyle HorizontalAlign="Right" />
                    </telerik:RadNumericTextBox>
                </td>
                <td style="border-bottom-style: solid; border-right-style: solid; border-right-width: thin;
                    border-bottom-width: thin; border-right-color: #99BB99; border-bottom-color: #99BB99">
                    <telerik:RadNumericTextBox ID="RadProbTextBox23" runat="server" MaxValue="100" MinValue="0"
                        ShowSpinButtons="True" ToolTip="Please enter the probability of transitioning to condition state 3 if action is applied."
                        Width="50px">
                        <NumberFormat DecimalDigits="1" />
                        <DisabledStyle ForeColor="#666666" />
                        <EnabledStyle HorizontalAlign="Right" />
                    </telerik:RadNumericTextBox>
                </td>
                <td style="border-bottom-style: solid; border-right-style: solid; border-right-width: thin;
                    border-bottom-width: thin; border-right-color: #99BB99; border-bottom-color: #99BB99">
                    <telerik:RadNumericTextBox ID="RadProbTextBox24" runat="server" MaxValue="100" MinValue="0"
                        ShowSpinButtons="True" ToolTip="Please enter the probability of transitioning to condition state 4 if action is applied."
                        Width="50px">
                        <NumberFormat DecimalDigits="1" />
                        <DisabledStyle ForeColor="#666666" />
                        <EnabledStyle HorizontalAlign="Right" />
                    </telerik:RadNumericTextBox>
                </td>
                <td style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #99BB99">
                    <asp:Label ID="errLabel2" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" style="border-bottom-style: solid; border-right-style: solid;
                    border-right-width: thin; border-bottom-width: thin; border-right-color: #99BB99;
                    border-bottom-color: #99BB99" height="32">
                    <asp:CheckBox ID="CheckBox3" runat="server" Text="3" OnCheckedChanged="CheckBox3_CheckedChanged"
                        ToolTip="Please check this box is if action can be applied to the condition state.  Leave it un-checked otherwise."
                        AutoPostBack="True" />
                </td>
                <td style="border-bottom-style: solid; border-right-style: solid; border-right-width: thin;
                    border-bottom-width: thin; border-right-color: #99BB99; border-bottom-color: #99BB99">
                    <telerik:RadNumericTextBox ID="RadCostTextBox3" runat="server" MaxValue="10000000"
                        MinValue="1" ShowSpinButtons="True" ToolTip="If action can be applied to the condition state please enter its unit agency cost here."
                        Width="80px">
                        <DisabledStyle ForeColor="#666666" />
                        <EnabledStyle HorizontalAlign="Right" />
                    </telerik:RadNumericTextBox>
                </td>
                <td style="border-bottom-style: solid; border-right-style: solid; border-right-width: thin;
                    border-bottom-width: thin; border-right-color: #99BB99; border-bottom-color: #99BB99">
                    <telerik:RadNumericTextBox ID="RadProbTextBox31" runat="server" MaxValue="100" MinValue="0"
                        ShowSpinButtons="True" ToolTip="Please enter the probability of transitioning to condition state 1 if action is applied."
                        Width="50px">
                        <NumberFormat DecimalDigits="1" />
                        <DisabledStyle ForeColor="#666666" />
                        <EnabledStyle HorizontalAlign="Right" />
                    </telerik:RadNumericTextBox>
                </td>
                <td style="border-bottom-style: solid; border-right-style: solid; border-right-width: thin;
                    border-bottom-width: thin; border-right-color: #99BB99; border-bottom-color: #99BB99">
                    <telerik:RadNumericTextBox ID="RadProbTextBox32" runat="server" MaxValue="100" MinValue="0"
                        ShowSpinButtons="True" ToolTip="Please enter the probability of transitioning to condition state 2 if action is applied."
                        Width="50px">
                        <NumberFormat DecimalDigits="1" />
                        <DisabledStyle ForeColor="#666666" />
                        <EnabledStyle HorizontalAlign="Right" />
                    </telerik:RadNumericTextBox>
                </td>
                <td style="border-bottom-style: solid; border-right-style: solid; border-right-width: thin;
                    border-bottom-width: thin; border-right-color: #99BB99; border-bottom-color: #99BB99">
                    <telerik:RadNumericTextBox ID="RadProbTextBox33" runat="server" MaxValue="100" MinValue="0"
                        ShowSpinButtons="True" ToolTip="Please enter the probability of transitioning to condition state 3 if action is applied."
                        Width="50px">
                        <NumberFormat DecimalDigits="1" />
                        <DisabledStyle ForeColor="#666666" />
                        <EnabledStyle HorizontalAlign="Right" />
                    </telerik:RadNumericTextBox>
                </td>
                <td style="border-bottom-style: solid; border-right-style: solid; border-right-width: thin;
                    border-bottom-width: thin; border-right-color: #99BB99; border-bottom-color: #99BB99">
                    <telerik:RadNumericTextBox ID="RadProbTextBox34" runat="server" MaxValue="100" MinValue="0"
                        ShowSpinButtons="True" ToolTip="Please enter the probability of transitioning to condition state 4 if action is applied."
                        Width="50px">
                        <NumberFormat DecimalDigits="1" />
                        <DisabledStyle ForeColor="#666666" />
                        <EnabledStyle HorizontalAlign="Right" />
                    </telerik:RadNumericTextBox>
                </td>
                <td style="border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: #99BB99">
                    <asp:Label ID="errLabel3" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" style="border-right-style: solid; border-right-width: thin; border-right-color: #99BB99;"
                    height="32">
                    <asp:CheckBox ID="CheckBox4" runat="server" Text="4" OnCheckedChanged="CheckBox4_CheckedChanged"
                        ToolTip="Please check this box is if action can be applied to the condition state.  Leave it un-checked otherwise."
                        AutoPostBack="True" />
                </td>
                <td style="border-right-style: solid; border-right-width: thin; border-right-color: #99BB99;">
                    <telerik:RadNumericTextBox ID="RadCostTextBox4" runat="server" MaxValue="10000000"
                        MinValue="1" ShowSpinButtons="True" ToolTip="If action can be applied to the condition state please enter its unit agency cost here."
                        Width="80px">
                        <DisabledStyle ForeColor="#666666" />
                        <EnabledStyle HorizontalAlign="Right" />
                    </telerik:RadNumericTextBox>
                </td>
                <td style="border-right-style: solid; border-right-width: thin; border-right-color: #99BB99;">
                    <telerik:RadNumericTextBox ID="RadProbTextBox41" runat="server" MaxValue="100" MinValue="0"
                        ShowSpinButtons="True" ToolTip="Please enter the probability of transitioning to condition state 1 if action is applied."
                        Width="50px">
                        <NumberFormat DecimalDigits="1" />
                        <DisabledStyle ForeColor="#666666" />
                        <EnabledStyle HorizontalAlign="Right" />
                    </telerik:RadNumericTextBox>
                </td>
                <td style="border-right-style: solid; border-right-width: thin; border-right-color: #99BB99;">
                    <telerik:RadNumericTextBox ID="RadProbTextBox42" runat="server" MaxValue="100" MinValue="0"
                        ShowSpinButtons="True" ToolTip="Please enter the probability of transitioning to condition state 2 if action is applied."
                        Width="50px">
                        <NumberFormat DecimalDigits="1" />
                        <DisabledStyle ForeColor="#666666" />
                        <EnabledStyle HorizontalAlign="Right" />
                    </telerik:RadNumericTextBox>
                </td>
                <td style="border-right-style: solid; border-right-width: thin; border-right-color: #99BB99;">
                    <telerik:RadNumericTextBox ID="RadProbTextBox43" runat="server" MaxValue="100" MinValue="0"
                        ShowSpinButtons="True" ToolTip="Please enter the probability of transitioning to condition state 3 if action is applied."
                        Width="50px">
                        <NumberFormat DecimalDigits="1" />
                        <DisabledStyle ForeColor="#666666" />
                        <EnabledStyle HorizontalAlign="Right" />
                    </telerik:RadNumericTextBox>
                </td>
                <td style="border-right-style: solid; border-right-width: thin; border-right-color: #99BB99;">
                    <telerik:RadNumericTextBox ID="RadProbTextBox44" runat="server" MaxValue="100" MinValue="0"
                        ShowSpinButtons="True" ToolTip="Please enter the probability of transitioning to condition state 4 if action is applied."
                        Width="50px">
                        <NumberFormat DecimalDigits="1" />
                        <DisabledStyle ForeColor="#666666" />
                        <EnabledStyle HorizontalAlign="Right" />
                    </telerik:RadNumericTextBox>
                </td>
                <td>
                    <asp:Label ID="errLabel4" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
</telerik:RadAjaxPanel>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="" Transparency="50"
    Width="464" Height="243" BackColor="#CCCCCC">
    <table width="100%">
        <tr>
            <td height="243" align="center" valign="middle">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/LoadingProgressBar.gif"
                    ImageAlign="Middle"/>
            </td>
        </tr>
    </table>
</telerik:RadAjaxLoadingPanel>
