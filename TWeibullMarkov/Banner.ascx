<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Banner.ascx.cs" Inherits="TWeibullMarkov.Banner" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<style type="text/css">
    .style1
    {
        width: 100%;
    }
  
       
  
    .style3
    {
        width: 20px;
    }
  
   
  
</style>

<table class="style1" cellpadding="0" cellspacing="0">
    <tr>
        <td bgcolor="#006F53" width="20" rowspan="2">
            &nbsp;</td>
        <td bgcolor="#006F53" 
            
            style="font-family: 'Highway Gothic Expanded'; font-size: xx-large; color: #FFFFFF" 
            class="style3">
            &nbsp;</td>
        <td bgcolor="#006F53" 
            
            style="font-family: 'Highway Gothic Expanded'; font-size: 50px; color: #FFFFFF; padding-top: 5px;">
            Gurenich Consulting, llc</td>
    </tr>
    <tr>
        <td bgcolor="#006F53" 
            style="font-family: 'Highway Gothic'; font-size: small; color: #FFFFFF" 
            class="style3">
            &nbsp;</td>
        <td bgcolor="#006F53" 
            
            style="font-family: 'Highway Gothic Wide'; font-size: medium; color: #FFFFFF; padding-bottom: 5px;">
            &nbsp;</td>
    </tr>
    <tr>
        <td colspan="3">
            <telerik:RadMenu ID="RadMenu1" Runat="server" Width="100%">
                <Items>
                    <telerik:RadMenuItem runat="server" Text="Home">
                    </telerik:RadMenuItem>
                    <telerik:RadMenuItem runat="server" Text="Services">
                    </telerik:RadMenuItem>
                    <telerik:RadMenuItem runat="server" Text="Solutions">
                        <Items>
                            <telerik:RadMenuItem runat="server" 
                                Text="Weibull-Markov Decision Model for MR&amp;R Policy Optimization" 
                                ToolTip="Software tool for generation of optimal multi-period policy of maintenance, repair and rehabilitation (MR&amp;R) using Markov Decision Model (MDM) with Weibull-distributed deterioration probabilities.">
                            </telerik:RadMenuItem>
                        </Items>
                    </telerik:RadMenuItem>
                    <telerik:RadMenuItem runat="server" Text="Contact Us">
                    </telerik:RadMenuItem>
                </Items>
            </telerik:RadMenu>
        </td>
    </tr>
</table>

