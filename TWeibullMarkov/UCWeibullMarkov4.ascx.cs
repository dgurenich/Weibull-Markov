using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using TWeibullMarkovLibrary;

namespace TWeibullMarkov
{
    public partial class UCWeibullMarkov4 : System.Web.UI.UserControl
    {

        public UCActionPane ActionPane1
        {
            get
            {
                return this.UCActionPane1;
            }
        }

        public UCActionPane ActionPane2
        {
            get
            {
                return this.UCActionPane2;
            }
        }

        public UCWeibullPane WeibullPane1
        {
            get
            {
                return UCWeibullPane1;
            }
        }

        public UCWeibullPane WeibullPane2
        {
            get
            {
                return UCWeibullPane2;
            }
        }

        public UCWeibullPane WeibullPane3
        {
            get
            {
                return UCWeibullPane3;
            }
        }

        public UCWeibullPane WeibullPane4
        {
            get
            {
                return UCWeibullPane4;
            }
        }

        public UCFailureCost FailureCostPane
        {
            get
            {
                return UCFailureCost1;
            }
        }

        public UCDiscounting DiscountingPane
        {
            get
            {
                return UCDiscounting1;
            }
        }

        public UCPolicy PolicyPane
        {
            get
            {
                return this.UCPolicy1;
            }
        }

        public Telerik.Web.UI.RadButton GeneratePolicyButton
        {
            get
            {
                return this.RadButtonPolicy;
            }
        }

        public Telerik.Web.UI.RadComboBox ExamplesComboBox
        {
            get
            {
                return this.RadComboBoxExamples;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack && RadComboBoxExamples.Items.Count <= 1)
            {
                 PopulateExamples();  
            }

        }

        public void PopulateExamples()
        {
            try
            {

                RadComboBoxExamples.Items.Clear();

                String[] files = System.IO.Directory.GetFiles(MapPath("~/App_Data/Examples"), "*.xml");
           
                if (files == null || files.Length < 1)
                {
                    RadComboBoxExamples.Text = "no examples available";
                    RadComboBoxExamples.ForeColor = System.Drawing.Color.Red;
                    RadComboBoxExamples.Items.Add(new Telerik.Web.UI.RadComboBoxItem("no examples available", null));
                    RadComboBoxExamples.Items[0].ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    RadComboBoxExamples.Items.Add(new Telerik.Web.UI.RadComboBoxItem("(select example)", null));
                }

                foreach (String s in files)
                {
                    String r = System.IO.Path.GetFileNameWithoutExtension(s);
                    RadComboBoxExamples.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r, s));
                }
                
            }
            catch (Exception ex)
            {
                RadComboBoxExamples.Items.Clear();
                RadComboBoxExamples.Items.Add(new Telerik.Web.UI.RadComboBoxItem(ex.Message, null));
                RadComboBoxExamples.ForeColor = System.Drawing.Color.Red;
                RadComboBoxExamples.Items[0].ForeColor = System.Drawing.Color.Red;
            }
        }

        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            if (source is UCPolicy)
            {
                if (args is TWeibullMarkovLibrary.MyEventArgs)
                {
                    if ((args as MyEventArgs).Cargo.ToString() == "GeneratePolicy")
                        RaiseBubbleEvent("Policy", args);
                }
            }
            return base.OnBubbleEvent(source, args);
        }


        public Boolean CheckActionPanes(out Boolean ok1, out Boolean ok2, out Boolean ok3)
        {
            ok1 = CheckActionPane(1);
            ok2 = CheckActionPane(2);

            ok3 = true;
            if (ok1 && ok2)
            {
                ok3 = false;
                for (Int32 i = 0; !ok3 && i < 4; i++)
                {
                    ok3 = (UCActionPane1.IsApplicableToState(i) || UCActionPane2.IsApplicableToState(i));
                }
            }

            return ok1 && ok2 && ok3;
        }


        public Boolean CheckWeibullPanes(out Boolean ok1, out Boolean ok2, out Boolean ok3, out Boolean ok4)
        {
            ok1 = UCWeibullPane1.Validate();
            ok2 = UCWeibullPane2.Validate();
            ok3 = UCWeibullPane3.Validate();
            ok4 = UCWeibullPane4.Validate();

            return (ok1 && ok2 && ok3 && ok4);
        }

        public Boolean CheckFailureCostPane()
        {
            Boolean ok = FailureCostPane.Validate();
            return ok;
        }


        public Boolean CheckActionPane(Int32 A)
        {
            Boolean ok = true;

            if (A == 1)
                ok = UCActionPane1.ValidateInput();
            else if (A == 2)
                ok = UCActionPane2.ValidateInput();
            return ok;
        }

        protected void RadButtonPolicy_Click(object sender, EventArgs e)
        {
            MyEventArgs myArgs = new MyEventArgs();
            myArgs.Cargo = "GeneratePolicy";
            RaiseBubbleEvent("Policy", myArgs);
        }

        public Boolean Initialize(String modelFilePath, out String errorMessage)
        {
            Boolean ok = true;
            errorMessage = null;
            WeibullMarkovModel model = null;

            try
            {
                String localSchemaPath = MapPath(UCPolicy.SCHEMA_FILE_PATH);

                if (System.IO.File.Exists(localSchemaPath))
                {
                    ok = TWeibullMarkovLibrary.Utilities.ValidateXMLvsXSD(modelFilePath, localSchemaPath, null, out errorMessage);
                }
                if (ok)
                {
                    // It is already validated, that's whay the second argument is null
                    model = WeibullMarkovModel.LoadFromXml(modelFilePath, null, out errorMessage);
                    ok = (model != null);
                }
                if (ok)
                {
                    UCWeibullPane1.Initialize(1, model);
                    UCWeibullPane2.Initialize(2, model);
                    UCWeibullPane3.Initialize(3, model);
                    UCWeibullPane4.Initialize(4, model);
                    UCActionPane1.Initialize(1, model);
                    UCActionPane2.Initialize(2, model);
                    UCFailureCost1.Initialize(model);
                    UCDiscounting1.Initialize(model);
                    UCPolicy1.Initialize(model);
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                ok = false;
            }

            if (!ok)
            {
                UCPolicy1.DisplayError(errorMessage);
            }

            return ok;
        }

        protected void RadComboBoxExamples_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            MyEventArgs myArgs = new MyEventArgs();
            if (!String.IsNullOrEmpty(RadComboBoxExamples.SelectedValue))
            {
                myArgs.Cargo = RadComboBoxExamples.SelectedValue;
                RaiseBubbleEvent("Example", myArgs);
            }
        }
    }
}