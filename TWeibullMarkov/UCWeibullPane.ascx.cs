using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;
using TWeibullMarkovLibrary;

namespace TWeibullMarkov
{
    public partial class UCWeibullPane : System.Web.UI.UserControl
    {

        private DataTable _dt = ChartDataTableSource();

        private static readonly String _vs_eta = "eta";
        private static readonly String _vs_beta = "beta";
        private static readonly String _vs_T = "T";
        private static readonly String _vs_f = "f";
        private static readonly String _vs_ff = "ff";

        /// <summary>
        /// Weibull scale parameter
        /// </summary>
        public Double Eta
        {
            get
            {
                if (ViewState[_vs_eta] == null)
                    return 0.0;

                return Double.Parse(ViewState[_vs_eta].ToString());
            }
            set
            {
                ViewState[_vs_eta] = value;
            }
        }


        /// <summary>
        /// Weibull slope (shape) parameter
        /// </summary>
        public Double Beta
        {
            get
            {
                if (ViewState[_vs_beta] == null)
                    return 0.0;

                return Double.Parse(ViewState[_vs_beta].ToString());
            }
            set
            {
                ViewState[_vs_beta] = value;
            }
        }

        /// <summary>
        /// Last tabulated year
        /// </summary>
        public Double T
        {
            get
            {
                if (ViewState[_vs_T] == null)
                    return 0.0;

                return Double.Parse(ViewState[_vs_T].ToString());
            }
            set
            {
                ViewState[_vs_T] = value;
            }
        }

        /// <summary>
        /// Last tabulated value of one-year probability of transitioning, f(T)
        /// </summary>
        public Double f
        {
            get
            {
                if (ViewState[_vs_f] == null)
                    return 0.0;

                return Double.Parse(ViewState[_vs_f].ToString());
            }
            set
            {
                ViewState[_vs_f] = value;
            }
        }

        /// <summary>
        /// Last tabulated value of the first derivative of one-year probability, f'(T)
        /// </summary>
        public Double ff
        {
            get
            {
                if (ViewState[_vs_ff] == null)
                    return 0.0;

                return Double.Parse(ViewState[_vs_ff].ToString());
            }
            set
            {
                ViewState[_vs_ff] = value;
            }
        }


        public String StateHeader
        {
            get
            {
                return ViewState["StateHeader"] == null ? String.Empty : ViewState["StateHeader"].ToString();
            }

            set
            {
                ViewState["StateHeader"] = value;
            }
        }

        private Boolean IsModelEstimated
        {
            get
            {
                return ViewState["ModelEstimated"] == null ? false : (ViewState["ModelEstimated"].ToString() == "1");
            }

            set
            {
                ViewState["ModelEstimated"] = value ? "1" : "0";
            }
        }

        public String LastState
        {
            set
            {
                ViewState["LastState"] = value;
            }

            get
            {
                return ViewState["LastState"] == null ? "false" : ViewState["LastState"].ToString();
            }
        }

        public Double? t50
        {
            get
            {
                return RadNumericTextBox1.Value; 
            }
        }


        public Double? t9x
        {
            get
            {
                return RadNumericTextBox2.Visible ? RadNumericTextBox2.Value : null;
            }
        }

        public String x
        {
            get
            {
                return RadComboBox1.Text.IndexOf("%") > 0 ? RadComboBox1.SelectedValue : null;
            }
        }


        private Boolean IsLastState
        {
            get
            {
                Boolean b = false;
                if (LastState != null && Boolean.TryParse(LastState, out b))
                    return b;
                if (LastState != null && (LastState.ToUpper() == "1" || LastState.ToUpper() == "YES" || LastState.ToUpper() == "TRUE"))
                    return true;
                return false;
            }
        }


        public Label ErrorLabel
        {
            get
            {
                return lblError;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetControlsAvailability();
            }
            lblError.Text = String.Empty;
            lblError.Visible = false;
        }


        private static DataTable ChartDataTableSource()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("X", typeof(Double));
            dt.Columns.Add("S1Y", typeof(Double));
            dt.Columns.Add("S2Y", typeof(Double));
            return dt;
        }

        protected void SetControlsAvailability()
        {
            ModelTable.Visible = IsModelEstimated;
            RadChart1.Visible = IsModelEstimated;
            RadComboBox1.Enabled = !IsModelEstimated;
            RadNumericTextBox1.Enabled = !IsModelEstimated;
            RadNumericTextBox2.Enabled = !IsModelEstimated;
            RadButtonEstimate.Text = IsModelEstimated ? "Start Over" : "Estimate";
            RadButtonEstimate.ToolTip = IsModelEstimated ? "Click to change inputs." : "Click to estimate Weibull deterioration model with the given inputs.";
        }


        protected void RadComboBox1_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (e.Text.IndexOf('%') < 0)
            {
                RadNumericTextBox2.Visible = false;
                LabelAndSetPeriod.Visible = false;
            }
            else
            {
                RadNumericTextBox2.Visible = true;
                LabelAndSetPeriod.Visible = true;
            }

            if (IsModelEstimated)
            {
                IsModelEstimated = false;
                SetControlsAvailability();
            }
        }



        protected void RadButtonEstimate_Click(object sender, EventArgs e)
        {

            String errorMessage = null;

            T = f = ff = 0;

            if (IsModelEstimated)
            {
                IsModelEstimated = false;
            }
            else
            {
                IsModelEstimated = false;
                Boolean ok = CheckInputs(out errorMessage);

                Double eta = 0, beta = 0;

                ok = TWeibullMarkovLibrary.Weibull.EstimateParameters(RadNumericTextBox1.Text, RadNumericTextBox2.Text, RadComboBox1.SelectedValue, out eta, out beta, out errorMessage);


                if (ok && beta <= 0.0)  // DIG 2/2/2012  This should never happen
                {
                    ok = false;
                    errorMessage = String.Format(@"Weibull model cannot be created for the given input: the 50% survival period is too short in comparison to the {0} period. 
Your options are: increase the first period, decrease the second period, or go with the default slope parameter (choose ""Don't know"" for the second threshold).", RadComboBox1.Text.Trim());
                }

                if (ok)
                {
                    Eta = eta;
                    Beta = beta;
                    ok = Tabulate(out errorMessage);
                }

                if (ok)
                {
                    lblError.Text = String.Empty;
                    lblError.Visible = false;
                    ModelTable.Rows[0].Cells[2].Text = String.Format(" = {0:g6}", Beta);
                    ModelTable.Rows[1].Cells[2].Text = String.Format(" = {0:g6}", Eta);
                    if (Beta < 1.0)
                    {
                        ModelTable.Rows[0].Cells[2].Text += "*";
                        ModelTable.Rows[0].Cells[2].ForeColor = System.Drawing.Color.Red;
                        ModelTable.Rows[0].Cells[2].ToolTip = @"The model has been estimated, but the slope parameter is less than one, which is typical for deterioration patterns known as ""infant mortality"", when deterioration tends to decrease over time rather than increase.  This is not an error, but please be aware that for the advanced age the policy may prescribe less aggressive treatment than for early years, which may look counter-intuitive.";
                        ModelTable.Rows[3].Cells[0].Text = "* Please fly the cursor over the red number for details.";
                    }
                    else if (Beta == 1.0)
                    {
                        ModelTable.Rows[0].Cells[2].Text += "*";
                        ModelTable.Rows[0].Cells[2].ForeColor = System.Drawing.Color.Red;
                        ModelTable.Rows[0].Cells[2].ToolTip = @"When slope parameter (beta) equals 1 deterioration rate is constant over time.  The policy recommendations for the condition state in such case do not change from year to year.";
                        ModelTable.Rows[3].Cells[0].Text = "* Please fly the cursor over the red number for details.";
                    }
                    else
                    {
                        ModelTable.Rows[0].Cells[2].ForeColor = System.Drawing.Color.Black;
                        ModelTable.Rows[0].Cells[2].ToolTip = String.Empty;
                        ModelTable.Rows[3].Cells[0].Text = String.Empty;
                    }
                    RadChart1.DataSource = null;
                    RadChart1.DataSource = _dt;
                    RadChart1.DataBind();
                    RadChart1.PlotArea.SeriesCollection().GetSeries(0).Appearance.LabelAppearance.Visible = false;
                    RadChart1.PlotArea.SeriesCollection().GetSeries(1).Appearance.LabelAppearance.Visible = false;
                    if (IsLastState)
                    {
                        RadChart1.ChartTitle.TextBlock.Text = "Probability of failure (percent)";
                    }
                }
                else
                {
                    lblError.Text = errorMessage;
                    lblError.Visible = true;
                }
                IsModelEstimated = ok;
            }
            SetControlsAvailability();
        }

        protected Boolean Tabulate(out String errorMessage)
        {
            Boolean ok = true;
            errorMessage = null;

            try
            {

                while (_dt.Rows.Count > 0)
                    _dt.Rows.RemoveAt(_dt.Rows.Count - 1);
                Int32 i = 0;
                T = 0;
                f = ff = 0;

                do
                {

                    T = (Double)i;

                    Double F = TWeibullMarkovLibrary.Weibull.CumulativeFailure(Eta, Beta, T);
                    f = TWeibullMarkovLibrary.Weibull.OneYearFailure(Eta, Beta, T);
                    ff = TWeibullMarkovLibrary.Weibull.OneYearFailureFirstDerivative(Eta, Beta, T);

                    DataRow dr = _dt.NewRow();

                    dr[0] = T;
                    dr[1] = F * 100.0;
                    dr[2] = f * 100.0;


                    _dt.Rows.Add(dr);



                    if ((F > 0.995) && i >= 5)    // Tabulte for 5 years minimum
                    {
                        if (i <= 40 && i % 5 == 0)
                        {
                            break;
                        }
                        else if (i % 10 == 0)
                        {
                            break;
                        }

                    }

                    i++;


                } while (i <= 100);
            }
            catch (Exception ex)
            {
                ok = false;
                errorMessage = ex.Message;
            }

            return ok;
        }

        protected Boolean CheckInputs(out String errorMessage)
        {
            Boolean ok = true;
            errorMessage = null;

            if (RadNumericTextBox2.Visible
                && RadNumericTextBox2.Value.HasValue
                && RadNumericTextBox2.Value.HasValue
                && RadNumericTextBox2.Value <= RadNumericTextBox1.Value)
            {
                errorMessage = String.Format("{0} survival period must be longer than 50%.", RadComboBox1.Text.Trim());
                ok = false;
            }


            return ok;
        }


        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (RadNumericTextBox2.Visible
                && RadNumericTextBox2.Value.HasValue
                && RadNumericTextBox2.Value.HasValue
                && RadNumericTextBox2.Value <= RadNumericTextBox1.Value)
            {
                (source as CustomValidator).Text = String.Format("{0} survival period must be longer than 50%.", RadComboBox1.Text.Trim());
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
        }

        public Boolean Validate()
        {
            Boolean ok = true;
            if (IsModelEstimated)
            {
                lblError.Text = String.Empty;

                ok = true;
            }
            else
            {
                ok = false;
                if (!RadNumericTextBox1.Value.HasValue || (RadNumericTextBox2.Visible && !RadNumericTextBox2.Value.HasValue))
                {
                    lblError.Text = "Please fill-in the missing values and estimate the model.";
                }
                else
                {
                    lblError.Text = "Please estimate the Weibull model (click on the 'Estimate' button).";
                }
            }

            
            if (ok && IsLastState)
            {
                RadChart1.ChartTitle.TextBlock.Text = "Probability of failure (percent)";
            }
            

            lblError.Visible = !ok;
            return ok;
        }

        public void Initialize(Int32 stateNumber, WeibullMarkovModel model)
        {
            foreach (WeibullMarkovConditionState state in model.States)
            {
                if (state.Number == stateNumber)
                {
                    RadNumericTextBox1.Value = state.t50.Value;
                    if (state.x != null && state.t9X.HasValue)
                    {
                        RadNumericTextBox2.Value = state.t9X.Value;
                        RadNumericTextBox2.Visible = true;
                        LabelAndSetPeriod.Visible = true;
                    }
                    else
                    {
                        LabelAndSetPeriod.Visible = false;
                        RadNumericTextBox2.Visible = false;
                    }

                    if (state.x != null)
                    {
                        RadComboBoxItem item = RadComboBox1.FindItemByValue(state.x);
                        RadComboBox1.SelectedIndex = (item == null ? 0 : item.Index);
                    }
                    else
                    {
                        RadComboBox1.SelectedIndex = 0;
                    }

                    break;
                }
            }

            if (stateNumber == 4)
            {
                LastState = "true";
            }

            IsModelEstimated = false;
            RadButtonEstimate_Click(RadButtonEstimate, null);
        }
    }
}