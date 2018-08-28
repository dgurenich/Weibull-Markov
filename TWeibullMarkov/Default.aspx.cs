using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using TWeibullMarkovLibrary;

namespace TWeibullMarkov
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
            RadAjaxManager1.AjaxSettings.AddAjaxSetting(UCWeibullMarkov41.GeneratePolicyButton, UCWeibullMarkov41.ActionPane1);
            RadAjaxManager1.AjaxSettings.AddAjaxSetting(UCWeibullMarkov41.GeneratePolicyButton, UCWeibullMarkov41.ActionPane2);
            RadAjaxManager1.AjaxSettings.AddAjaxSetting(UCWeibullMarkov41.GeneratePolicyButton, UCWeibullMarkov41.WeibullPane1);
            RadAjaxManager1.AjaxSettings.AddAjaxSetting(UCWeibullMarkov41.GeneratePolicyButton, UCWeibullMarkov41.WeibullPane2);
            RadAjaxManager1.AjaxSettings.AddAjaxSetting(UCWeibullMarkov41.GeneratePolicyButton, UCWeibullMarkov41.WeibullPane3);
            RadAjaxManager1.AjaxSettings.AddAjaxSetting(UCWeibullMarkov41.GeneratePolicyButton, UCWeibullMarkov41.WeibullPane4);
            RadAjaxManager1.AjaxSettings.AddAjaxSetting(UCWeibullMarkov41.GeneratePolicyButton, UCWeibullMarkov41.FailureCostPane);
            RadAjaxManager1.AjaxSettings.AddAjaxSetting(UCWeibullMarkov41.GeneratePolicyButton, UCWeibullMarkov41.DiscountingPane);
            RadAjaxManager1.AjaxSettings.AddAjaxSetting(UCWeibullMarkov41.GeneratePolicyButton, UCWeibullMarkov41.PolicyPane);
                        
            RadAjaxManager1.AjaxSettings.AddAjaxSetting(UCWeibullMarkov41.ExamplesComboBox, UCWeibullMarkov41.WeibullPane1);
            RadAjaxManager1.AjaxSettings.AddAjaxSetting(UCWeibullMarkov41.ExamplesComboBox, UCWeibullMarkov41.WeibullPane2);
            RadAjaxManager1.AjaxSettings.AddAjaxSetting(UCWeibullMarkov41.ExamplesComboBox, UCWeibullMarkov41.WeibullPane3);
            RadAjaxManager1.AjaxSettings.AddAjaxSetting(UCWeibullMarkov41.ExamplesComboBox, UCWeibullMarkov41.WeibullPane4);
            RadAjaxManager1.AjaxSettings.AddAjaxSetting(UCWeibullMarkov41.ExamplesComboBox, UCWeibullMarkov41.ActionPane1);
            RadAjaxManager1.AjaxSettings.AddAjaxSetting(UCWeibullMarkov41.ExamplesComboBox, UCWeibullMarkov41.ActionPane2);
            RadAjaxManager1.AjaxSettings.AddAjaxSetting(UCWeibullMarkov41.ExamplesComboBox, UCWeibullMarkov41.FailureCostPane);
            RadAjaxManager1.AjaxSettings.AddAjaxSetting(UCWeibullMarkov41.ExamplesComboBox, UCWeibullMarkov41.DiscountingPane);
            RadAjaxManager1.AjaxSettings.AddAjaxSetting(UCWeibullMarkov41.ExamplesComboBox, UCWeibullMarkov41.PolicyPane);
            
         }   

        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            if (source is String && source.ToString() == "Policy") 
            {
                if (args is MyEventArgs)
                {
                    if ((args as MyEventArgs).Cargo.ToString() == "GeneratePolicy")
                    {
                        RadAjaxManager1.RaisePostBackEvent("Policy:GeneratePolicy");
                    }
                }
            }
            else if (source is String && source.ToString() == "Example")
            {
                if (args is MyEventArgs)
                {
                    RadAjaxManager1.RaisePostBackEvent("Example:" + (args as MyEventArgs).Cargo.ToString());
                }
            }

            return base.OnBubbleEvent(source, args);
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {
            if (e.Argument.StartsWith("Example:"))
            {
                String errorMessage = null;
                String filePath = e.Argument.Substring(8, e.Argument.Length - 8);
                Boolean ok = UCWeibullMarkov41.Initialize(filePath, out errorMessage);
                if (!ok)
                    UCWeibullMarkov41.PolicyPane.DisplayError(errorMessage);
            }
            else if (e.Argument.StartsWith("Policy") && e.Argument.EndsWith("GeneratePolicy"))
            {
                Boolean aok1 = true, aok2 = true, aok3 = true;
                Boolean tok1 = true, tok2 = true, tok3 = true, tok4 = true;

                Boolean aok = UCWeibullMarkov41.CheckActionPanes(out aok1, out aok2, out aok3);
                Boolean tok = UCWeibullMarkov41.CheckWeibullPanes(out tok1, out tok2, out tok3, out tok4);
                Boolean fok = UCWeibullMarkov41.FailureCostPane.Validate();
                Boolean dok = UCWeibullMarkov41.DiscountingPane.Validate();

                Boolean ok = aok && tok && fok && dok;

                if (!ok)
                    UCWeibullMarkov41.PolicyPane.DisplayError(tok1, tok2, tok3, tok4, aok1, aok2, fok, dok, aok3);
                else
                    UCWeibullMarkov41.PolicyPane.EstimateModel(UCWeibullMarkov41.WeibullPane1,
                                                                UCWeibullMarkov41.WeibullPane2,
                                                                UCWeibullMarkov41.WeibullPane3,
                                                                UCWeibullMarkov41.WeibullPane4,
                                                                UCWeibullMarkov41.ActionPane1,
                                                                UCWeibullMarkov41.ActionPane2,
                                                                UCWeibullMarkov41.FailureCostPane,
                                                                UCWeibullMarkov41.DiscountingPane);

            }
        }
    }
}