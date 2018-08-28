using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using TWeibullMarkovLibrary;

namespace TWeibullMarkov
{
    public partial class UCPolicy : System.Web.UI.UserControl
    {

        public static readonly String SCHEMA_FILE_PATH = "~/App_Data/WeibullMarkovModel.xsd";

        protected void Page_Load(object sender, EventArgs e)
        {
            labelError.Visible = false;
            labelInfo.Visible = false;
        }

       

        public void DisplayError(Boolean tok1, Boolean tok2, Boolean tok3, Boolean tok4, Boolean aok1, Boolean aok2, Boolean fok, Boolean dok, Boolean aok3)
        {
            labelError.Text = "Please correct all errors in the following panes and try generating the policy again.<br/><br/>";
            if (!tok1)
                labelError.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Condition State 1<br/>";
            if (!tok2)
                labelError.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Condition State 2<br/>";
            if (!tok3)
                labelError.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Condition State 3<br/>";
            if (!tok4)
                labelError.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Condition State 4<br/>";
            if (!aok1)
                labelError.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Action 1<br/>";
            if (!aok2)
                labelError.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Action 2<br/>";
            if (!fok)
                labelError.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Failure cost<br/>";
            if (!fok)
                labelError.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Discounting<br/>";
            if (!aok3)
                labelError.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; - Actions 1 and 2: at least one action must be applicable in at least one condition state<br/>";
            labelInfo.Text = String.Empty;
            labelError.Visible = true;
            labelInfo.Visible = false;
         }


        public void DisplayError(String err)
        {
            labelError.Text = err;
            labelError.Visible = true;
            labelInfo.Visible = false;
        }


        public void EstimateModel(UCWeibullPane tp1, UCWeibullPane tp2, UCWeibullPane tp3, UCWeibullPane tp4, 
                                  UCActionPane a1, UCActionPane a2, UCFailureCost failCost, UCDiscounting disc)
        {

            Boolean ok = true;

            try
            {
                String errorMessage = null;

                Double discRate = disc.AnnualRate;
                Boolean fcEstimate = failCost.Estimate;
                Boolean fcOverride = failCost.Override;
                Double? fcCost = failCost.FailureCost;

                UCWeibullPane[] tp = new UCWeibullPane[] { tp1, tp2, tp3, tp4 };

                TWeibullMarkovLibrary.WeibullMarkovModel model = new WeibullMarkovModel(4, 3, fcEstimate, fcCost, fcOverride, discRate);

                // We are assuming throughout that the do nothing cost is zero

                TWeibullMarkovLibrary.WeibullMarkovConditionState[] state = new WeibullMarkovConditionState[4];
               

                for (int i = 0; ok && i < 4; i++)
                {

                    state[i] = model.AddConditionState(tp[i].Eta, tp[i].Beta, 0.0, tp[i].T, tp[i].f, tp[i].ff, out errorMessage);
                    if (state[i] == null)
                        throw new Exception(errorMessage);

                    state[i].t50 = tp[i].t50;
                    state[i].t9X = tp[i].t9x;
                    state[i].x = tp[i].x;

                    if (a1.IsApplicableToState(i))
                    {
                      TWeibullMarkovLibrary.WeibullMarkovAction action = state[i].AddAction(1, 4, true, a1.GetCost(i));
                      for (Int32 j = 0; j < 4; j++)
                      {
                          action.TranProb[j] = a1.GetTranProb(i, j);
                      }
                    }

                    if (a2.IsApplicableToState(i))
                    {
                        TWeibullMarkovLibrary.WeibullMarkovAction action = state[i].AddAction(2, 4, true, a2.GetCost(i));
                        for (Int32 j = 0; j < 4; j++)
                        {
                            action.TranProb[j] = a2.GetTranProb(i, j);
                        }
                    }
                }

                if (ok)
                {
                    String fileName = MapPath("~/App_Data/WeibullMarkovModel.xml");
                    ok = model.SaveToXml(fileName, out errorMessage);
                    if (!ok)
                        throw new Exception(errorMessage);

                    String localSchemaPath = MapPath(SCHEMA_FILE_PATH);
                    if (System.IO.File.Exists(localSchemaPath))
                    {
                        ok = TWeibullMarkovLibrary.Utilities.ValidateXMLvsXSD(fileName, localSchemaPath, null, out errorMessage);
                        if (!ok)
                        {
                            throw new Exception(errorMessage);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                labelError.Text = ex.Message;
                labelInfo.Text = String.Empty;
                labelError.Visible = true;
                labelInfo.Visible = false;
            }
        }

        

        public void Initialize(WeibullMarkovModel model)
        {
           

            try
            {
            }
            catch (Exception)
            {
               
            }
         
        }
    }
}