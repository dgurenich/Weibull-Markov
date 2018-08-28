using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using TWeibullMarkovLibrary;

namespace TWeibullMarkov
{
    public partial class UCDiscounting : System.Web.UI.UserControl
    {

        /// <summary>
        /// Returns 0.1% if the box is not filled.
        /// The validator should be on guard against such things.
        /// </summary>
        public Double AnnualRate
        {
            get
            {
                return RadNumericTextBox1.Value.HasValue ? RadNumericTextBox1.Value.Value : 0.001;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ComputeDiscountingFactor();
            }
        }

        protected void ComputeDiscountingFactor()
        {
            if (RadNumericTextBox1.Value.HasValue)
            {
                Double f = 1.0 / (1.0 + RadNumericTextBox1.Value.Value / 100.0);
                LabelDiscFactor.Text = f.ToString("f6");
                LabelDiscFactor.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                LabelDiscFactor.Text = "???";
                LabelDiscFactor.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void RadButton1_Click(object sender, EventArgs e)
        {
            ComputeDiscountingFactor();
        }

        public Boolean Validate()
        {
            RequiredFieldValidator1.Validate();
            ComputeDiscountingFactor();
            return RequiredFieldValidator1.IsValid;
        }

        public void Initialize(WeibullMarkovModel model)
        {
            RadNumericTextBox1.Value = Math.Min(RadNumericTextBox1.MaxValue,Math.Max(model.DiscRate, RadNumericTextBox1.MinValue));
            ComputeDiscountingFactor();
        }

    }
}