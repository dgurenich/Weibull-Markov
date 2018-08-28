<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TWeibullMarkov._Default" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register src="UCWeibullMarkov4.ascx" tagname="UCWeibullMarkov4" tagprefix="uc1" %>
<%@ Register src="Banner.ascx" tagname="Banner" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Gurenich Consulting</title>
  
</head>
<link href="Skins/MetroGreen/Ajax.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/Window.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/Button.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/Calendar.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/ColorPicker.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/ComboBox.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/DataPager.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/Dock.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/Editor.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/FileExplorer.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/Filter.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/FormDecorator.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/Grid.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/ImageEditor.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/Input.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/ListBox.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/ListView.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/Menu.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/Notification.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/OrgChart.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/PanelBar.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/Rating.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/RibbonBar.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/Rotator.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/Scheduler.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/SchedulerRecurrenceEditor.MetroGreen.css" rel="stylesheet"
    type="text/css" />
<link href="Skins/MetroGreen/SchedulerReminderDialog.MetroGreen.css" rel="stylesheet"
    type="text/css" />
<link href="Skins/MetroGreen/SiteMap.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/Slider.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/SocialShare.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/Splitter.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/TabStrip.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/TagCloud.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/ToolBar.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/ToolTip.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/TreeList.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/TreeView.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/Upload.MetroGreen.css" rel="stylesheet" type="text/css" />
<link href="Skins/MetroGreen/Widgets.MetroGreen.css" rel="stylesheet" type="text/css" />
<body>
    <form id="form1" runat="server" 
    style="font-family: Segoe UI, Helvetica, sans-serif; font-size: small; font-weight: normal; font-style: normal; font-variant: normal">
    <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server">
    </telerik:RadStyleSheetManager>
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" 
        EnableTheming="True">
    </telerik:RadScriptManager>
    <table width="100%">
    <tr>
    <td>
        <uc2:Banner ID="Banner1" runat="server" />
        </td>
    </tr>
    <tr>
    <td>
        <telerik:RadTabStrip ID="RadTabStrip2" runat="server" 
            MultiPageID="RadMultiPage1">
            <Tabs>
                <telerik:RadTab runat="server" Text="Getting Started" PageViewID="RadPageViewStarted">
                </telerik:RadTab>
                <telerik:RadTab runat="server" Owner="RadTabStrip2"  
                    Text="Dashboard" PageViewID="RadPageViewDashboard">
                </telerik:RadTab>
                <telerik:RadTab runat="server" Text="Downloads" PageViewID="RadPageViewDownloads">
                </telerik:RadTab>
                <telerik:RadTab runat="server" Text="Q&amp;A" PageViewID="RadPageViewQA">
                </telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
  
    <telerik:RadMultiPage ID="RadMultiPage1" Runat="server">
        <telerik:RadPageView ID="RadPageViewStarted" runat="server">
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageViewDashboard" runat="server">
            <uc1:UCWeibullMarkov4 ID="UCWeibullMarkov41" runat="server" />
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageViewDownloads" runat="server">
        </telerik:RadPageView>
         <telerik:RadPageView ID="RadPageViewQA" runat="server">
        </telerik:RadPageView>
    </telerik:RadMultiPage>
    </td>
    </tr>
    </table>
    
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" 
        onajaxrequest="RadAjaxManager1_AjaxRequest">
    </telerik:RadAjaxManager>
    </form>
</body>
</html>
