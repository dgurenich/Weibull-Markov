<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCWeibullPane.ascx.cs"
    Inherits="TWeibullMarkov.UCWeibullPane" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Charting" TagPrefix="telerik" %>
<style type="text/css">
    .style1
    {
        width: 100%;
    }
    
    .loading
    {
        background-color: #fff;
        height: 265px;
        width: 464px;
    }
</style>
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">
        function OnComboBoxSelectedIndexChanged(sender, eventArgs) {
            var selectedItem = eventArgs.get_item();
            var selectedItemText = selectedItem != null ? selectedItem.get_text() : sender.get_text();
            var labelId = '<%=LabelAndSetPeriod.ClientID%>';
            var label = document.getElementById('<%=LabelAndSetPeriod.ClientID%>');

            if (selectedItemText.indexOf('%') > 0) {
                label.innerHTML = 'and set its survival period';
            }
            else {
                label.innerHTML = '';
            }
            return false;
        }
       
    </script>
</telerik:RadCodeBlock>
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
    <asp:Panel ID="Panel1" runat="server" BorderColor="#006F53" BorderStyle="Solid" BorderWidth="2px"
        Height="265px" Width="464px">
        <table class="style1">
            <tr>
                <td colspan="3" valign="bottom" style="font-weight: bold; border-bottom-style: solid;
                    border-bottom-width: thin; border-bottom-color: #99BB99;">
                    <%=StateHeader%>
                </td>
            </tr>
            <tr>
                <td style="padding-left: 4px">
                    Set survival period (years) for the 50% deterioration threshold</td>
                <td>
                    <telerik:RadNumericTextBox ID="RadNumericTextBox1" runat="server" MaxValue="300"
                        MinValue="1" ShowSpinButtons="True" ToolTip="Please enter the number of years after which 50% of the condition unit's quantity will still remain in the current condition state."
                        Value="1" Width="60px">
                        <NumberFormat DecimalDigits="1" />
                        <DisabledStyle ForeColor="#666666" />
                        <EnabledStyle HorizontalAlign="Right" />
                    </telerik:RadNumericTextBox>
                </td>
                <td align="left">
                    <asp:RequiredFieldValidator ID="rqfNum1" runat="server" ControlToValidate="RadNumericTextBox1"
                        Display="Dynamic" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0" class="style1">
                        <tr>
                            <td style="padding-left: 4px" nowrap="nowrap">
                                Select 2nd threshold
                            </td>
                            <td>
                                <telerik:RadComboBox ID="RadComboBox1" runat="server" OnSelectedIndexChanged="RadComboBox1_SelectedIndexChanged"
                                    ToolTip="Please choose the  quantity percentage threshold (10%, 5% or 1%) for the second period after which, respectively, 10%, 5% or just 1% of the quantity will stay in the current state.  Select &quot;Don't know&quot; if you are uncertain."
                                    Width="90px" CausesValidation="False" ForeColor="Black" AutoPostBack="True">
                                    <Items>
                                        <telerik:RadComboBoxItem runat="server" Owner="RadComboBox1" Text="10%" ToolTip="Please enter the number of years after which 10% of the condition unit's quantity will still remain in the given condition state."
                                            Value="10" />
                                        <telerik:RadComboBoxItem runat="server" Text="5%" ToolTip="Please enter the number of years after which 10% of the condition unit's quantity will still remain in the given condition state."
                                            Value="5" />
                                        <telerik:RadComboBoxItem runat="server" Text="1%" ToolTip="Please enter the number of years after which 1% of the condition unit's quantity will still remain in the given condition state."
                                            Value="1" />
                                        <telerik:RadComboBoxItem runat="server" Text="Don't know" Value="Don't know" />
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                            <td align="left" nowrap="nowrap" width="150">
                                <asp:Label ID="LabelAndSetPeriod" runat="server" Text=" and set its survival period"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <telerik:RadNumericTextBox ID="RadNumericTextBox2" runat="server" MaxValue="300"
                        MinValue="1" ShowSpinButtons="True" ToolTip="Please enter the number of years after which the selected percentage of the condition unit's quantity will still remain in the current condition state."
                        Value="2" Width="60px">
                        <NumberFormat DecimalDigits="1" />
                        <DisabledStyle ForeColor="#666666" />
                        <EnabledStyle HorizontalAlign="Right" />
                    </telerik:RadNumericTextBox>
                </td>
                <td align="left">
                    <asp:RequiredFieldValidator ID="rqfNum2" runat="server" ControlToValidate="RadNumericTextBox2"
                        Display="Dynamic" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                </td>
            </tr>
           
            <tr>
                <td colspan="3">
                    <table class="style1">
                        <tr>
                            <td valign="top">
                                <telerik:RadButton ID="RadButtonEstimate" runat="server" OnClick="RadButtonEstimate_Click"
                                    Text="Estimate" ToolTip="Click to estimate the Weibull model with the given input parameters."
                                    Width="150px" CausesValidation="False">
                                </telerik:RadButton>
                            </td>
                            <td rowspan="2">
                                <telerik:RadChart ID="RadChart1" runat="server" DefaultType="Line" Height="170px"
                                    Skin="Telerik" Width="295px" AlternateText="Transtion probability chart.">
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
                                        <telerik:ChartSeries DataXColumn="X" DataYColumn="S2Y" Name="One-Year">
                                            <Appearance BarWidthPercent="50">
                                                <FillStyle FillType="ComplexGradient" MainColor="DarkOliveGreen">
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
                                                <EmptyValue Mode="Zero">
                                                </EmptyValue>
                                                <Border Color="111, 174, 12" />
                                            </Appearance>
                                        </telerik:ChartSeries>
                                        <telerik:ChartSeries DataXColumn="X" DataYColumn="S1Y" Name="Cumulative" 
                                            Type="Line">
                                            <Appearance>
                                                <FillStyle FillType="ComplexGradient" MainColor="Red">
                                                    <FillSettings>
                                                        <ComplexGradient>
                                                            <telerik:GradientElement Color="136, 221, 246" />
                                                            <telerik:GradientElement Color="97, 203, 234" Position="0.5" />
                                                            <telerik:GradientElement Color="59, 161, 197" Position="1" />
                                                        </ComplexGradient>
                                                    </FillSettings>
                                                </FillStyle>
                                                <TextAppearance TextProperties-Color="51, 51, 51">
                                                </TextAppearance>
                                                <Border Color="67, 181, 229" />
                                            </Appearance>
                                        </telerik:ChartSeries>
                                    </Series>
                                    <Legend>
                                        <Appearance Dimensions-AutoSize="False" Dimensions-Height="100px" Dimensions-Margins="17.6%, 3%, 1px, 1px"
                                            Dimensions-Paddings="2px, 8px, 6px, 3px" Dimensions-Width="200px" 
                                            Position-AlignedPosition="TopRight">
                                            <ItemAppearance Position-AlignedPosition="Bottom" Position-Auto="False" Position-X="0"
                                                Position-Y="0">
                                            </ItemAppearance>
                                            <ItemTextAppearance AutoTextWrap="True" TextProperties-Color="Black" TextProperties-Font="Segoe UI, 6pt">
                                            </ItemTextAppearance>
                                            <ItemMarkerAppearance Figure="Square">
                                                <Border Width="0" />
                                            </ItemMarkerAppearance>
                                            <FillStyle MainColor="">
                                            </FillStyle>
                                            <Border Width="0" />
                                        </Appearance>
                                        <TextBlock>
                                            <Appearance TextProperties-Font="Segoe UI, 6pt">
                                            </Appearance>
                                        </TextBlock>
                                    </Legend>
                                    <PlotArea>
                                        <XAxis LayoutMode="Normal">
                                            <Appearance Color="182, 182, 182" MajorTick-Color="216, 216, 216">
                                                <MajorGridLines Color="216, 216, 216" PenStyle="Solid" />
                                                <LabelAppearance Position-AlignedPosition="Center" Position-Auto="False" Position-X="0"
                                                    Position-Y="0">
                                                </LabelAppearance>
                                                <TextAppearance TextProperties-Color="51, 51, 51" Position-AlignedPosition="Top"
                                                    TextProperties-Font="Verdana, 6pt">
                                                </TextAppearance>
                                            </Appearance>
                                            <AxisLabel Visible="True">
                                                <Appearance Visible="True" Position-AlignedPosition="Top" Position-Auto="False" Position-X="28"
                                                    Position-Y="148">
                                                </Appearance>
                                                <TextBlock Text="Years in current condition state">
                                                    <Appearance TextProperties-Color="51, 51, 51" TextProperties-Font="Verdana, 6pt, style=Bold"
                                                        Position-AlignedPosition="Left">
                                                    </Appearance>
                                                </TextBlock>
                                            </AxisLabel>
                                        </XAxis>
                                        <YAxis>
                                            <Appearance Color="182, 182, 182" MajorTick-Color="216, 216, 216" MinorTick-Color="223, 223, 223">
                                                <MajorGridLines Color="216, 216, 216" />
                                                <MinorGridLines Color="223, 223, 223" />
                                                <LabelAppearance Position-AlignedPosition="Right">
                                                </LabelAppearance>
                                                <TextAppearance TextProperties-Color="51, 51, 51" Position-AlignedPosition="Left"
                                                    TextProperties-Font="Segoe UI, 6pt">
                                                </TextAppearance>
                                            </Appearance>
                                            <AxisLabel>
                                                <TextBlock Text="Probability (%)">
                                                    <Appearance TextProperties-Color="51, 51, 51" TextProperties-Font="Segoe UI, 6pt">
                                                    </Appearance>
                                                </TextBlock>
                                            </AxisLabel>
                                        </YAxis>
                                        <YAxis2 Visible="False">
                                        </YAxis2>
                                        <Appearance Dimensions-AutoSize="False" Dimensions-Height="110px" Dimensions-Margins="13%, 5%, 5%, 12%"
                                            Dimensions-Width="245px">
                                            <FillStyle FillType="Solid" MainColor="White">
                                            </FillStyle>
                                            <Border Color="182, 182, 182" />
                                        </Appearance>
                                    </PlotArea>
                                    <ChartTitle>
                                        <Appearance Position-Auto="False" Position-X="0" Position-Y="5" Dimensions-AutoSize="False"
                                            Dimensions-Height="25px" Dimensions-Width="280px">
                                            <FillStyle MainColor="">
                                            </FillStyle>
                                        </Appearance>
                                        <TextBlock Text="Probability of going to next state (percent)">
                                            <Appearance Position-AlignedPosition="Center" TextProperties-Color="Black" TextProperties-Font="Segoe UI, 6pt, style=Bold"
                                                Position-Auto="False" Position-X="30" Position-Y="0">
                                            </Appearance>
                                        </TextBlock>
                                    </ChartTitle>
                                </telerik:RadChart>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                                <asp:CustomValidator ID="CustomValidator1" runat="server" Display="Dynamic" ErrorMessage="Second survival period must be longer than the first one."
                                    OnServerValidate="CustomValidator1_ServerValidate">Second survival period must be longer than the first one.</asp:CustomValidator>
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
                                        <asp:TableCell runat="server" ColumnSpan="3" Height="20"></asp:TableCell>
                                     </asp:TableRow>
                                     <asp:TableRow runat="server">
                                        <asp:TableCell runat="server" ColumnSpan="3" Height="50" Font-Size="Smaller"></asp:TableCell>
                                     </asp:TableRow>
                                </asp:Table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
</telerik:RadAjaxPanel>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="" Transparency="50"
    Width="464" Height="265" BackColor="#CCCCCC">
   <table width="100%">
        <tr>
            <td height="265" align="center" valign="middle">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/LoadingProgressBar.gif"
                    ImageAlign="Middle"/>
            </td>
        </tr>
    </table>
</telerik:RadAjaxLoadingPanel>
