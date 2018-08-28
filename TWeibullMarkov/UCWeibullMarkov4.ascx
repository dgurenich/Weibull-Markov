<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCWeibullMarkov4.ascx.cs"
    Inherits="TWeibullMarkov.UCWeibullMarkov4" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="UCWeibullPane.ascx" TagName="UCWeibullPane" TagPrefix="uc1" %>
<%@ Register Src="UCActionPane.ascx" TagName="UCActionPane" TagPrefix="uc2" %>
<%@ Register Src="UCDiscounting.ascx" TagName="UCDiscounting" TagPrefix="uc3" %>
<%@ Register Src="UCFailureCost.ascx" TagName="UCFailureCost" TagPrefix="uc4" %>
<%@ Register Src="UCPolicy.ascx" TagName="UCPolicy" TagPrefix="uc5" %>
<style type="text/css">
    .style1
    {
    }
    
    
    .style5
    {
        font-size: x-large;
    }
    .style6
    {
        font-size: x-large;
    }
    
    </style>
<table style="padding: 0px; margin: 0px;" cellpadding="0" cellspacing="0">
    <tr>
        <td colspan="2">
            <table style="border-style: solid; border-width: 0px 0px 0px 0px; border-color: #006F53;
                padding: 0px; margin: 0px; border-collapse: collapse;">
                <tr>
                    <td colspan="4">
                    <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" LoadingPanelID="RadAjaxLoadingPanel2">
                        <table style="border: 2px solid #006F53; padding: 0px; margin: 0px;" width="100%">
                            <tr>
                                <td align="center" 
                                    style="background-color: #006F53; font-family: 'Highway Gothic Expanded'; font-size: xx-large; color: #FFFFFF;" 
                                    width="50" height="50">1
                                    </td>
                                <td class="style5" style="padding-left: 10px">
                                    <span class="style6">Weibull Deterioration Models for Condition States</span>
                                </td>
                                <td style="padding-right: 10px" align="right" nowrap="nowrap">
                                    Start from example:&nbsp;<telerik:RadComboBox ID="RadComboBoxExamples" 
                                        ToolTip="Start building your model from a pre-cooked example."
                                        runat="server" AutoPostBack="True" CausesValidation="False" Width="330px" 
                                        onselectedindexchanged="RadComboBoxExamples_SelectedIndexChanged">
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                        </table>
                       </telerik:RadAjaxPanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <uc1:UCWeibullPane ID="UCWeibullPane1" runat="server" StateHeader="Condition State 1 (best)" />
                     </td>
                    <td>
                        <uc1:UCWeibullPane ID="UCWeibullPane2" runat="server" StateHeader="Condition State 2" />
                    </td>
                    <td>
                        <uc1:UCWeibullPane ID="UCWeibullPane3" runat="server" StateHeader="Condition State 3" />
                    </td>
                    <td>
                        <uc1:UCWeibullPane ID="UCWeibullPane4" runat="server" StateHeader="Condition State 4 (worst)"
                            LastState="yes" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td valign="top">
            <table style="border-style: solid; border-width: 0px 0px 0px 0px; border-color: #006F53;
                padding: 0px; margin: 0px; padding: 0px; margin: 0px; border-collapse: collapse;"
                width="940">
                <tr>
                    <td colspan="2">
                        <table style="border: 2px solid #006F53; padding: 0px; margin: 0px;" width="100%">
                            <tr>
                                <td  align="center" 
                                    style="background-color: #006F53; font-family: 'Highway Gothic'; font-size: xx-large; color: #FFFFFF;" 
                                    width="50" height="50">
                                    2</td>
                                <td class="style5" style="padding-left: 10px">
                                    Cost and Imrovement Models
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <uc2:UCActionPane ID="UCActionPane1" runat="server" ActionHeader="Action 1" />
                    </td>
                    <td>
                        <uc2:UCActionPane ID="UCActionPane2" runat="server" ActionHeader="Action 2" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <uc4:UCFailureCost ID="UCFailureCost1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <uc3:UCDiscounting ID="UCDiscounting1" runat="server" />
                    </td>
                </tr>
            </table>
        </td>
        <td valign="top">
            <table style="border-style: solid; border-width: 0px 0px 0px 0px; border-color: #006F53;
                padding: 0px; margin: 0px; border-collapse: collapse;" width="940px">
                <tr>
                    <td valign="top">
                        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
                            <table style="border: 2px solid #006F53; padding: 0px; margin: 0px;" width="100%">
                                <tr>
                                    <td align="center" 
                                        style="background-color: #006F53; font-family: 'Highway Gothic Expanded'; color: #FFFFFF; font-size: xx-large;" 
                                        width="50" height="50">
                                        3
                                    </td>
                                    <td class="style5" style="padding-left: 10px">
                                        Optimal Markov Policy
                                    </td>
                                    <td align="right" width="460" style="padding-right: 10px">
                                        <telerik:RadButton ID="RadButtonPolicy" runat="server" Text="Generate Optimal Policy"
                                            ToolTip="Click to generate the optimal Markov policy based on the given inputs."
                                            OnClick="RadButtonPolicy_Click" CausesValidation="False">
                                        </telerik:RadButton>
                                    </td>
                                </tr>
                            </table>
                        </telerik:RadAjaxPanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <uc5:UCPolicy ID="UCPolicy1" runat="server" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Transparency="50"
    Width="900" Height="70" BackColor="#CCCCCC">
    <div align="center" style="padding: 25px; width: 900px; height: 70px;">
        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/LoadingProgressBar.gif"
            ImageAlign="Middle" />
    </div>
</telerik:RadAjaxLoadingPanel>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel2" runat="server" Transparency="50"
    Width="1800" Height="70" BackColor="#CCCCCC">
    <div align="center" style="padding: 25px; width: 1800px; height: 70px;">
        <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/LoadingProgressBar.gif"
            ImageAlign="Middle" />
    </div>
</telerik:RadAjaxLoadingPanel>