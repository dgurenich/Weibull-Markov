<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCPolicy.ascx.cs" Inherits="TWeibullMarkov.UCPolicy" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<style type="text/css">
.LabelError
{
    padding: 5px;
    text-align: left;
}
</style>
<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
    <asp:Panel ID="Panel1" runat="server" BorderColor="#006F53" BorderStyle="Solid" BorderWidth="2px"
        Width="934px" Height="385px">
        <table class="style1">
            <tr>
                <td valign="top" style="padding: 5px 5px 5px 15px">
                    <asp:Label ID="labelInfo" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" height="340" valign="middle" width="100%">
                    <asp:Label ID="labelError" runat="server" BackColor="#FFFFCC" BorderColor="Red" 
                        BorderStyle="Solid" BorderWidth="2" ForeColor="Red" CssClass="LabelError"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
</telerik:RadAjaxPanel>
<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="" Transparency="50"
    Width="924" Height="20px">
    <div align="center" style="padding: 10px; width: 920px; height: 50px;">
        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/LoadingProgressBar.gif"
            ImageAlign="Middle" />
    </div>
</telerik:RadAjaxLoadingPanel>
