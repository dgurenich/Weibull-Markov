using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Telerik.Web.UI;
using TWeibullMarkovLibrary;

namespace TWeibullMarkov
{
    public partial class UCActionPane : System.Web.UI.UserControl
    {

        private static readonly String _txtMissingValues = "Missing values";
        private static readonly String _fmtSumNE100Pct = "SUM {0} 100%";

        public String ActionHeader
        {
            set
            {
                ViewState["ActionHeader"] = value;
            }

            get
            {
                return ViewState["ActionHeader"].ToString();
            }
        }

        public Label ErrorLabel1
        {
            get
            {
                return errLabel1;
            }
        }

        public Label ErrorLabel2
        {
            get
            {
                return errLabel2;
            }
        }

        public Label ErrorLabel3
        {
            get
            {
                return errLabel3;
            }
        }

        public Label ErrorLabel4
        {
            get
            {
                return errLabel4;
            }
        }

        public Telerik.Web.UI.RadAjaxLoadingPanel AjaxLoadingPanel
        {
            get
            {
                return RadAjaxLoadingPanel1;
            }
        }


        protected CheckBox[] _cbApplic = null;
        protected RadNumericTextBox[] _tbCost = null;
        protected RadNumericTextBox[,] _tbProb = null;
        
        protected void Page_Load(object sender, EventArgs e)
        {
           
            _cbApplic = new CheckBox[] { CheckBox1, CheckBox2, CheckBox3, CheckBox4 };
            _tbCost = new RadNumericTextBox[] { RadCostTextBox1, RadCostTextBox2, RadCostTextBox3, RadCostTextBox4 };
            _tbProb = new RadNumericTextBox[,] {
                 { RadProbTextBox11, RadProbTextBox12, RadProbTextBox13, RadProbTextBox14 },
                 { RadProbTextBox21, RadProbTextBox22, RadProbTextBox23, RadProbTextBox24 },
                 { RadProbTextBox31, RadProbTextBox32, RadProbTextBox33, RadProbTextBox34 },
                 { RadProbTextBox41, RadProbTextBox42, RadProbTextBox43, RadProbTextBox44 }
                };

            if (!Page.IsPostBack)
            {
                errSumLabel.Visible = false;
                okLabel.Visible = false;
                LabelAction.Text = ActionHeader;
                CheckRow1();
                CheckRow2();
                CheckRow3();
                CheckRow4();
            }

            
        }

        /// <summary>
        /// Returns agency cost of the action in the i-th (zero based!) condition state
        /// </summary>
        /// <param name="i">Condition state (zero based!)</param>
        /// <returns></returns>
        public Double GetCost(Int32 i)
        {
            if (0 <= i && i < 4)
            {
                return _tbCost[i].Value.HasValue ? _tbCost[i].Value.Value : 0.0;
            }
            return 0.0;
        }


        /// <summary>
        /// Returns transition probability for a pair of states
        /// </summary>
        /// <param name="i">Source state (zero based!)</param>
        /// <param name="j">Target state (zero based!)</param>
        /// <returns>Transition probability as a fraction of one</returns>
        public Double GetTranProb(Int32 i, Int32 j)
        {
            if (0 <= i && i < 4 && 0 <= j && j < 4)
            {
                return _tbProb[i, j].Value.HasValue ? _tbProb[i, j].Value.Value / 100.0 : 0.0;
            }
            return 0.0;
        }

        /// <summary>
        /// Returns applicability of the action to condition state i (zero based!)
        /// </summary>
        /// <param name="i">Condition state index (zero based)</param>
        /// <returns>True if action is applicable, zero otherwise</returns>
        public Boolean IsApplicableToState(Int32 i)
        {
            if (0 <= i && i < 4)
            {
                return _cbApplic[i].Checked;
            }
            return false;
        }

        protected void CheckRow1()
        {
            if (!CheckBox1.Checked)
            {
                if (!RadCostTextBox1.Value.HasValue)
                {
                    RadCostTextBox1.Value = 999999.00;
                }

                if (!RadProbTextBox11.Value.HasValue)
                {
                    RadProbTextBox11.Value = 100.0;
                }

                if (!RadProbTextBox12.Value.HasValue)
                {
                    RadProbTextBox12.Value = 0.0;
                }

                if (!RadProbTextBox13.Value.HasValue)
                {
                    RadProbTextBox13.Value = 0.0;
                }

                if (!RadProbTextBox14.Value.HasValue)
                {
                    RadProbTextBox14.Value = 0.0;
                }

                RadCostTextBox1.Enabled = false;
                RadProbTextBox11.Enabled = false;
                RadProbTextBox12.Enabled = false;
                RadProbTextBox13.Enabled = false;
                RadProbTextBox14.Enabled = false;
                errLabel1.Visible = false;
            }
            else
            {
                if (!RadCostTextBox1.Value.HasValue)
                {
                    RadCostTextBox1.Value = 1.00;
                }

                if (!RadProbTextBox11.Value.HasValue)
                {
                    RadProbTextBox11.Value = 100.0;
                }

                if (!RadProbTextBox12.Value.HasValue)
                {
                    RadProbTextBox12.Value = 0.0;
                }

                if (!RadProbTextBox13.Value.HasValue)
                {
                    RadProbTextBox13.Value = 0.0;
                }

                if (!RadProbTextBox14.Value.HasValue)
                {
                    RadProbTextBox14.Value = 0.0;
                }

                RadCostTextBox1.Enabled = true;
                RadProbTextBox11.Enabled = true;
                RadProbTextBox12.Enabled = true;
                RadProbTextBox13.Enabled = true;
                RadProbTextBox14.Enabled = true;

                ValidateRow1();
            }


        }


        protected void CheckRow2()
        {
            if (!CheckBox2.Checked)
            {
                if (!RadCostTextBox2.Value.HasValue)
                {
                    RadCostTextBox2.Value = 999999.00;
                }

                if (!RadProbTextBox21.Value.HasValue)
                {
                    RadProbTextBox21.Value = 100.0;
                }

                if (!RadProbTextBox22.Value.HasValue)
                {
                    RadProbTextBox22.Value = 0.0;
                }

                if (!RadProbTextBox23.Value.HasValue)
                {
                    RadProbTextBox23.Value = 0.0;
                }

                if (!RadProbTextBox24.Value.HasValue)
                {
                    RadProbTextBox24.Value = 0.0;
                }

                RadCostTextBox2.Enabled = false;
                RadProbTextBox21.Enabled = false;
                RadProbTextBox22.Enabled = false;
                RadProbTextBox23.Enabled = false;
                RadProbTextBox24.Enabled = false;
                errLabel2.Visible = false;
            }
            else
            {
                if (!RadCostTextBox2.Value.HasValue)
                {
                    RadCostTextBox2.Value = 100.00;
                }

                if (!RadProbTextBox21.Value.HasValue)
                {
                    RadProbTextBox21.Value = 0.0;
                }

                if (!RadProbTextBox22.Value.HasValue)
                {
                    RadProbTextBox22.Value = 0.0;
                }

                if (!RadProbTextBox23.Value.HasValue)
                {
                    RadProbTextBox23.Value = 0.0;
                }

                if (!RadProbTextBox24.Value.HasValue)
                {
                    RadProbTextBox24.Value = 0.0;
                }

                RadCostTextBox2.Enabled = true;
                RadProbTextBox21.Enabled = true;
                RadProbTextBox22.Enabled = true;
                RadProbTextBox23.Enabled = true;
                RadProbTextBox24.Enabled = true;

                ValidateRow2();
            }


        }


        protected void CheckRow3()
        {
            if (!CheckBox3.Checked)
            {
                if (!RadCostTextBox3.Value.HasValue)
                {
                    RadCostTextBox3.Value = 999999.00;
                }

                if (!RadProbTextBox31.Value.HasValue)
                {
                    RadProbTextBox31.Value = 100.0;
                }

                if (!RadProbTextBox32.Value.HasValue)
                {
                    RadProbTextBox32.Value = 0.0;
                }

                if (!RadProbTextBox33.Value.HasValue)
                {
                    RadProbTextBox33.Value = 0.0;
                }

                if (!RadProbTextBox34.Value.HasValue)
                {
                    RadProbTextBox34.Value = 0.0;
                }

                RadCostTextBox3.Enabled = false;
                RadProbTextBox31.Enabled = false;
                RadProbTextBox32.Enabled = false;
                RadProbTextBox33.Enabled = false;
                RadProbTextBox34.Enabled = false;

                errLabel3.Visible = false;
            }
            else
            {
                if (!RadCostTextBox3.Value.HasValue)
                {
                    RadCostTextBox3.Value = 1.00;
                }

                if (!RadProbTextBox31.Value.HasValue)
                {
                    RadProbTextBox31.Value = 100.0;
                }

                if (!RadProbTextBox32.Value.HasValue)
                {
                    RadProbTextBox32.Value = 0.0;
                }

                if (!RadProbTextBox33.Value.HasValue)
                {
                    RadProbTextBox33.Value = 0.0;
                }

                if (!RadProbTextBox34.Value.HasValue)
                {
                    RadProbTextBox34.Value = 0.0;
                }

                RadCostTextBox3.Enabled = true;
                RadProbTextBox31.Enabled = true;
                RadProbTextBox32.Enabled = true;
                RadProbTextBox33.Enabled = true;
                RadProbTextBox34.Enabled = true;

                ValidateRow3();
            }
        }

        protected void CheckRow4()
        {
            if (!CheckBox4.Checked)
            {
                if (!RadCostTextBox4.Value.HasValue)
                {
                    RadCostTextBox4.Value = 999999.00;
                }

                if (!RadProbTextBox41.Value.HasValue)
                {
                    RadProbTextBox41.Value = 100.0;
                }

                if (!RadProbTextBox42.Value.HasValue)
                {
                    RadProbTextBox42.Value = 0.0;
                }

                if (!RadProbTextBox43.Value.HasValue)
                {
                    RadProbTextBox43.Value = 0.0;
                }

                if (!RadProbTextBox44.Value.HasValue)
                {
                    RadProbTextBox44.Value = 0.0;
                }

                RadCostTextBox4.Enabled = false;
                RadProbTextBox41.Enabled = false;
                RadProbTextBox42.Enabled = false;
                RadProbTextBox43.Enabled = false;
                RadProbTextBox44.Enabled = false;

                errLabel4.Visible = false;
            }
            else
            {
                if (!RadCostTextBox4.Value.HasValue)
                {
                    RadCostTextBox4.Value = 1.00;
                }

                if (!RadProbTextBox41.Value.HasValue)
                {
                    RadProbTextBox41.Value = 100.0;
                }

                if (!RadProbTextBox42.Value.HasValue)
                {
                    RadProbTextBox42.Value = 0.0;
                }

                if (!RadProbTextBox43.Value.HasValue)
                {
                    RadProbTextBox43.Value = 0.0;
                }

                if (!RadProbTextBox44.Value.HasValue)
                {
                    RadProbTextBox44.Value = 0.0;
                }

                RadCostTextBox4.Enabled = true;
                RadProbTextBox41.Enabled = true;
                RadProbTextBox42.Enabled = true;
                RadProbTextBox43.Enabled = true;
                RadProbTextBox44.Enabled = true;

                ValidateRow4();
            }
        }



        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckRow1();
        }


        protected void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            CheckRow2();
        }

        protected void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            CheckRow3();
        }

        protected void CheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            CheckRow4();
        }

        public Boolean ValidateInput()
        {
            Boolean ok = true, ok1 = true, ok2 = true, ok3 = true, ok4 = true;


            if (CheckBox1.Checked)
                ok1 = ValidateRow1();

            if (CheckBox2.Checked)
                ok2 = ValidateRow2();

            if (CheckBox3.Checked)
                ok3 = ValidateRow3();

            if (CheckBox4.Checked)
                ok4 = ValidateRow4();

            ok = ok1 && ok2 && ok3 && ok4;

            okLabel.Visible = ok;
            errSumLabel.Visible = !ok;

            return ok;
        }

        protected Boolean ValidateRow1()
        {
            Boolean ok = true;
            if (CheckBox1.Checked)
            {
                if (!RadCostTextBox1.Value.HasValue || !RadProbTextBox11.Value.HasValue || !RadProbTextBox12.Value.HasValue
                    || !RadProbTextBox13.Value.HasValue || !RadProbTextBox14.Value.HasValue)
                {
                    ok = false;
                    errLabel1.Text = _txtMissingValues;
                }
                else
                {
                    Double sum = RadProbTextBox11.Value.Value + RadProbTextBox12.Value.Value + RadProbTextBox13.Value.Value + RadProbTextBox14.Value.Value;
                    if (Math.Round(sum, 1) != 100.0)
                    {
                        ok = false;
                        errLabel1.Text = String.Format(_fmtSumNE100Pct, sum < 100.0 ? '<' : '>');
                    }
                }
            }
            errLabel1.Visible = !ok;
            return ok;
        }

        protected Boolean ValidateRow2()
        {
            Boolean ok = true;
            if (CheckBox2.Checked)
            {
                if (!RadCostTextBox2.Value.HasValue || !RadProbTextBox21.Value.HasValue || !RadProbTextBox22.Value.HasValue
                    || !RadProbTextBox23.Value.HasValue || !RadProbTextBox24.Value.HasValue)
                {
                    ok = false;
                    errLabel2.Text = _txtMissingValues;
                }
                else
                {
                    Double sum = RadProbTextBox21.Value.Value + RadProbTextBox22.Value.Value + RadProbTextBox23.Value.Value + RadProbTextBox24.Value.Value;
                    if (Math.Round(sum, 1) != 100.0)
                    {
                        ok = false;
                        errLabel2.Text = String.Format(_fmtSumNE100Pct, sum < 100.0 ? '<' : '>');
                    }
                }
            }
            errLabel2.Visible = !ok;
            return ok;
        }

        protected Boolean ValidateRow3()
        {
            Boolean ok = true;
            if (CheckBox3.Checked)
            {
                if (!RadCostTextBox3.Value.HasValue || !RadProbTextBox31.Value.HasValue || !RadProbTextBox32.Value.HasValue
                    || !RadProbTextBox33.Value.HasValue || !RadProbTextBox34.Value.HasValue)
                {
                    ok = false;
                    errLabel3.Text = _txtMissingValues;
                }
                else
                {
                    Double sum = RadProbTextBox31.Value.Value + RadProbTextBox32.Value.Value + RadProbTextBox33.Value.Value + RadProbTextBox34.Value.Value;
                    if (Math.Round(sum, 1) != 100.0)
                    {
                        ok = false;
                        errLabel3.Text = String.Format(_fmtSumNE100Pct, sum < 100.0 ? '<' : '>');
                    }
                }
            }
            errLabel3.Visible = !ok;
            return ok;
        }

        protected Boolean ValidateRow4()
        {
            Boolean ok = true;
            if (CheckBox4.Checked)
            {
                if (!RadCostTextBox4.Value.HasValue || !RadProbTextBox41.Value.HasValue || !RadProbTextBox42.Value.HasValue
                    || !RadProbTextBox43.Value.HasValue || !RadProbTextBox44.Value.HasValue)
                {
                    ok = false;
                    errLabel4.Text = _txtMissingValues;
                }
                else
                {
                    Double sum = RadProbTextBox41.Value.Value + RadProbTextBox42.Value.Value + RadProbTextBox43.Value.Value + RadProbTextBox44.Value.Value;
                    if (Math.Round(sum, 1) != 100.0)
                    {
                        ok = false;
                        errLabel4.Text = String.Format(_fmtSumNE100Pct, sum < 100.0 ? '<' : '>');
                    }
                }
            }
            errLabel4.Visible = !ok;
            return ok;
        }

        protected void RadButton1_Click(object sender, EventArgs e)
        {
            ValidateInput();
        }

        public void Initialize(Int32 actionNumber, WeibullMarkovModel model)
        {
            for (Int32 i = 0; i < 4; i++)
            {
                _cbApplic[i].Checked = false;
                _tbCost[i].Value = 999999.00;
                for (Int32 j=0; j<4; j++) 
                {
                    _tbProb[i, j].Value = (j == 0 ? 100.0 : 0.0);
                }
            }

            for (Int32 i = 0; i < model.States.Count; i++)
            {
                foreach (WeibullMarkovAction action in model.States[i].Actions)
                {
                    if (action.Number == actionNumber && action.IsApplicable)
                    {
                        _cbApplic[i].Checked = true;
                        _tbCost[i].Value = action.Cost;
                        for (Int32 j = 0; j < 4; j++)
                        {
                            _tbProb[i,j].Value = 100.0 * action.TranProb[j];
                        }
                    }
                }
            }

            CheckRow1();
            CheckRow2();
            CheckRow3();
            CheckRow4();

            ValidateInput();
        }
    }
}