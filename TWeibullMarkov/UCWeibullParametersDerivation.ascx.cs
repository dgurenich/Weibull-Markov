using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TWeibullMarkov
{
    public partial class UCWeibullParametersDerivation : System.Web.UI.UserControl
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                RadNumericTextBox1.Enabled = true;
                RadNumericTextBox2.Enabled = true;
                RadComboBox1.Enabled = true;
            }
        }

        protected void RadComboBox1_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (e.Text.IndexOf('%') < 0)
            {
                RadNumericTextBox2.Visible = false;
                CompareValidator1.Enabled = false;
                LabelAndSetPeriod.Visible = false;
            }
            else
            {
                RadNumericTextBox2.Visible = true;
                CompareValidator1.Enabled = true;
                LabelAndSetPeriod.Visible = true;
            }
        }

        protected void RadButtonReset_Click(object sender, EventArgs e)
        {
            ModelTable.Visible = false;
            RadChart1.Visible = false;
        }

        protected void RadButtonEstimate_Click(object sender, EventArgs e)
        {
            ModelTable.Visible = true;
            RadChart1.Visible = true;
        }
    }
}