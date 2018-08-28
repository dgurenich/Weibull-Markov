using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using TWeibullMarkovLibrary;

namespace TWeibullMarkov
{
    public partial class UCFailureCost : System.Web.UI.UserControl
    {

        public Boolean Estimate
        {
            get
            {
                return RadioButton1.Checked;
            }
        }

        public Boolean Override
        {
            get
            {
                return RadioButton2.Checked && CheckBox1.Checked;
            }
        }

        /// <summary>
        /// Returns null if failure cost box is not filled.
        /// This may happen if only Estimate == true
        /// </summary>
        public Double? FailureCost
        {
            get
            {
                return RadNumericTextBox1.Value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SetControlsAvailability();
            }
         
        }

        protected void SetControlsAvailability()
        {
            if (!RadioButton2.Checked)
            {
                RadNumericTextBox1.Enabled = false;
                CheckBox1.Enabled = false;
            }
            else
            {
                RadNumericTextBox1.Enabled = true;
                CheckBox1.Enabled = true;
            }
        }

        protected void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            SetControlsAvailability();
        }

        protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            SetControlsAvailability();
        }

        public Boolean Validate()
        {
            Boolean ok = true;
            if (RadioButton2.Checked)
            {
                RequiredFieldValidator1.Validate();
                ok = RequiredFieldValidator1.IsValid;
            }

         
            return ok;
        }

        public void Initialize(WeibullMarkovModel model)
        {
            if (model.FailCostEstimate)
            {
                RadioButton1.Checked = true;
                RadioButton2.Checked = false;
                RadNumericTextBox1.Value = 0.0;
                CheckBox1.Checked = false;
            }
            else
            {
                RadioButton1.Checked = false;
                RadioButton2.Checked = true;
                RadNumericTextBox1.Value = model.FailureCost;
                CheckBox1.Checked = model.FailCostOverride;
            }
            SetControlsAvailability();
        }
    }
}