<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCWeibullParametersDerivation.ascx.cs"
    Inherits="TWeibullMarkov.UCWeibullParametersDerivation" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Charting" TagPrefix="telerik" %>
<style type="text/css">
    .style1
    {
        width: 100%;
    }
</style>
<script type="text/javascript" language="javascript">

   

</script>
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" height="250px" width="450px">
<asp:Panel ID="Panel1" runat="server" BorderColor="#009900" BorderStyle="Solid" BorderWidth="4px"
    Height="250px" Width="450px">
    <table class="style1">
        <tr>
            <td colspan="2" valign="bottom" style="font-weight: bold">
                <%=StateHeader%>
            </td>
        </tr>
        <tr>
            <td style="padding-left: 4px">
                Set survival period (years) for the 1st threshold&nbsp; (50%)
            </td>
            <td>
                <telerik:RadNumericTextBox ID="RadNumericTextBox1" runat="server" MaxValue="300"
                    MinValue="1" ShowSpinButtons="True" ToolTip="Please enter the number of years after which 50% of the condition unit's quantity will still remain in the current condition state."
                    Value="1" Width="70px">
                    <NumberFormat DecimalDigits="2" />
                    <EnabledStyle HorizontalAlign="Right" />
                </telerik:RadNumericTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" class="style1">
                    <tr>
                        <td style="text-align: left">
                            <table class="style1">
                                <tr>
                                    <td nowrap="nowrap">
                                        Select 2nd threshold
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="RadComboBox1" runat="server" AutoPostBack="True" OnClientSelectedIndexChanged="OnComboIndexChangedHandler"
                                            OnSelectedIndexChanged="RadComboBox1_SelectedIndexChanged" ToolTip="Please choose the  quantity percentage threshold (10%, 5% or 1%) for the second period after which, respectively, 10%, 5% or just 1% of the quantity will stay in the current state.  Select &quot;Don't know&quot; if you are uncertain."
                                            ValidationGroup="vgroupSecondPeriod" Width="90px">
                                            <Items>
                                                <telerik:RadComboBoxItem runat="server" Owner="RadComboBox1" Text="10%" ToolTip="Please enter the number of years after which 10% of the condition unit's quantity will still remain in the given condition state."
                                                    Value="10%" />
                                                <telerik:RadComboBoxItem runat="server" Text="5%" ToolTip="Please enter the number of years after which 10% of the condition unit's quantity will still remain in the given condition state."
                                                    Value="5%" />
                                                <telerik:RadComboBoxItem runat="server" Text="1%" ToolTip="Please enter the number of years after which 1% of the condition unit's quantity will still remain in the given condition state."
                                                    Value="1%" />
                                                <telerik:RadComboBoxItem runat="server" Text="Don't know" Value="Don't know" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="left" nowrap="nowrap" width="100">
                            <asp:Label ID="LabelAndSetPeriod" runat="server" Text=" and set its period"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <telerik:RadNumericTextBox ID="RadNumericTextBox2" runat="server" MaxValue="300"
                    MinValue="1" ShowSpinButtons="True" ToolTip="Please enter the number of years after which the selected percentage of the condition unit's quantity will still remain in the current condition state."
                    Value="2" Width="70px" ValidationGroup="vgroupSecondPeriod">
                    <NumberFormat DecimalDigits="2" />
                    <EnabledStyle HorizontalAlign="Right" />
                </telerik:RadNumericTextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="RadNumericTextBox1"
                    ControlToValidate="RadNumericTextBox2" Display="Dynamic" ErrorMessage="Survival period for the second threshold must be longer than for the first."
                    Operator="GreaterThan" ValidationGroup="vgroupSecondPeriod">Survival period for the second threshold must be longer than the first one&#39;s.</asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table cellpadding="0" cellspacing="0" class="style1">
                    <tr>
                        <td valign="top">
                            <telerik:RadButton ID="RadButtonEstimate" runat="server" Text="Estimate" 
                                OnClick="RadButtonEstimate_Click" 
                                ToolTip="Click to Estimate the Weibull model for the given inputs.">
                            </telerik:RadButton>
                            <telerik:RadButton ID="RadButtonReset" runat="server" Text="Reset" OnClick="RadButtonReset_Click"
                                ToolTip="Push this button to remove the estimated model parameters and transition chart off the panel.">
                            </telerik:RadButton>
                        </td>
                        <td rowspan="4" align="right">
                            <telerik:RadChart ID="RadChart1" runat="server" Height="150px" Width="240px" DefaultType="Line"
                                Skin="Telerik">
                                <Appearance Corners="Round, Round, Round, Round, 7">
                                    <FillStyle FillType="ComplexGradient">
                                        <FillSettings GradientMode="Horizontal">
                                            <ComplexGradient>
                                                <telerik:GradientElement Color="236, 236, 236" />
                                                <telerik:GradientElement Color="248, 248, 248" Position="0.5" />
                                                <telerik:GradientElement Color="236, 236, 236" Position="1" />
                                            </ComplexGradient>
                                        </FillSettings>
                                    </FillStyle>
                                    <Border Color="130, 130, 130" />
                                </Appearance>
                                <Series>
                                    <telerik:ChartSeries Name="One-Year" Type="Line">
                                        <Appearance>
                                            <FillStyle FillType="ComplexGradient" MainColor="#009933">
                                                <FillSettings>
                                                    <ComplexGradient>
                                                        <telerik:GradientElement Color="213, 247, 255" />
                                                        <telerik:GradientElement Color="193, 239, 252" Position="0.5" />
                                                        <telerik:GradientElement Color="157, 217, 238" Position="1" />
                                                    </ComplexGradient>
                                                </FillSettings>
                                            </FillStyle>
                                            <TextAppearance TextProperties-Color="51, 51, 51">
                                            </TextAppearance>
                                        </Appearance>
                                    </telerik:ChartSeries>
                                    <telerik:ChartSeries Name="Cumulative" Type="Line">
                                        <Appearance>
                                            <FillStyle FillType="ComplexGradient" MainColor="#FF0066">
                                                <FillSettings>
                                                    <ComplexGradient>
                                                        <telerik:GradientElement Color="218, 254, 122" />
                                                        <telerik:GradientElement Color="198, 244, 80" Position="0.5" />
                                                        <telerik:GradientElement Color="153, 205, 46" Position="1" />
                                                    </ComplexGradient>
                                                </FillSettings>
                                            </FillStyle>
                                            <TextAppearance TextProperties-Color="51, 51, 51">
                                            </TextAppearance>
                                            <Border Color="111, 174, 12" />
                                        </Appearance>
                                    </telerik:ChartSeries>
                                </Series>
                                <Legend>
                                    <Appearance Dimensions-AutoSize="False" Dimensions-Height="62px" Dimensions-Margins="17.6%, 3%, 1px, 1px"
                                        Dimensions-Paddings="2px, 8px, 6px, 3px" Dimensions-Width="200px" Overflow="Row"
                                        Position-AlignedPosition="Bottom" Position-Auto="False" Position-X="20" Position-Y="125">
                                        <ItemAppearance Position-AlignedPosition="Bottom" Position-Auto="False" Position-X="0"
                                            Position-Y="0">
                                        </ItemAppearance>
                                        <ItemTextAppearance AutoTextWrap="True" TextProperties-Color="Black" TextProperties-Font="Verdana, 6pt">
                                        </ItemTextAppearance>
                                        <ItemMarkerAppearance Figure="Square">
                                            <Border Width="0" />
                                        </ItemMarkerAppearance>
                                        <FillStyle MainColor="">
                                        </FillStyle>
                                        <Border Width="0" />
                                    </Appearance>
                                </Legend>
                                <PlotArea>
                                    <XAxis>
                                        <Appearance Color="182, 182, 182" MajorTick-Color="216, 216, 216">
                                            <MajorGridLines Color="216, 216, 216" PenStyle="Solid" />
                                            <TextAppearance TextProperties-Color="51, 51, 51">
                                            </TextAppearance>
                                        </Appearance>
                                        <AxisLabel Visible="True">
                                            <Appearance Visible="True">
                                            </Appearance>
                                            <TextBlock>
                                                <Appearance TextProperties-Color="51, 51, 51">
                                                </Appearance>
                                            </TextBlock>
                                        </AxisLabel>
                                    </XAxis>
                                    <YAxis>
                                        <Appearance Color="182, 182, 182" MajorTick-Color="216, 216, 216" MinorTick-Color="223, 223, 223">
                                            <MajorGridLines Color="216, 216, 216" />
                                            <MinorGridLines Color="223, 223, 223" />
                                            <TextAppearance TextProperties-Color="51, 51, 51">
                                            </TextAppearance>
                                        </Appearance>
                                        <AxisLabel Visible="True">
                                            <Appearance Visible="True">
                                            </Appearance>
                                            <TextBlock>
                                                <Appearance TextProperties-Color="51, 51, 51">
                                                </Appearance>
                                            </TextBlock>
                                        </AxisLabel>
                                    </YAxis>
                                    <YAxis2 Visible="False">
                                    </YAxis2>
                                    <Appearance Dimensions-AutoSize="False" Dimensions-Height="105px" Dimensions-Margins="18%, 10%, 12%, 10%"
                                        Dimensions-Width="205px">
                                        <FillStyle FillType="Solid" MainColor="White">
                                        </FillStyle>
                                        <Border Color="182, 182, 182" />
                                    </Appearance>
                                </PlotArea>
                                <ChartTitle>
                                    <Appearance Position-Auto="False" Position-X="8" Position-Y="0">
                                        <FillStyle MainColor="">
                                        </FillStyle>
                                    </Appearance>
                                    <TextBlock Text="Same State Staying Probability (%)">
                                        <Appearance Position-AlignedPosition="Top" Position-Auto="False" Position-X="12"
                                            Position-Y="4" TextProperties-Color="Black" TextProperties-Font="Tahoma, 7pt">
                                        </Appearance>
                                    </TextBlock>
                                </ChartTitle>
                            </telerik:RadChart>
                        </td>
                    </tr>
                    <tr>
                        <td valign="middle" height="100">
                            <asp:Table ID="ModelTable" runat="server">
                                <asp:TableRow runat="server">
                                    <asp:TableCell runat="server" Font-Names="Symbol">b</asp:TableCell>
                                    <asp:TableCell runat="server">(slope)</asp:TableCell>
                                    <asp:TableCell runat="server">beta_value</asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow runat="server">
                                    <asp:TableCell runat="server" Font-Names="Symbol">h</asp:TableCell>
                                    <asp:TableCell runat="server">(scale)</asp:TableCell>
                                    <asp:TableCell runat="server">eta_value</asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow runat="server">
                                    <asp:TableCell runat="server" Font-Names="Symbol">l</asp:TableCell>
                                    <asp:TableCell runat="server">(hazard rate)</asp:TableCell>
                                    <asp:TableCell runat="server">lambda_value</asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="RadComboBox1">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadNumericTextBox2" />
                                <telerik:AjaxUpdatedControl ControlID="CompareValidator1" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="RadButtonEstimate">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadNumericTextBox1" />
                                <telerik:AjaxUpdatedControl ControlID="RadComboBox1" />
                                <telerik:AjaxUpdatedControl ControlID="RadNumericTextBox2" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="RadButtonReset">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadNumericTextBox1" />
                                <telerik:AjaxUpdatedControl ControlID="RadComboBox1" />
                                <telerik:AjaxUpdatedControl ControlID="RadNumericTextBox2" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                    </AjaxSettings>
                </telerik:RadAjaxManagerProxy>
            </td>
        </tr>
    </table>
</asp:Panel>

</telerik:RadAjaxPanel>

