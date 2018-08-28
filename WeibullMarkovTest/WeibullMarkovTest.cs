using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TWeibullMarkovLibrary;

using log4net;

namespace WeibullMarkovTest
{
    [TestClass]
    public class WeibullMarkovTest
    {
        String modelXmlFileName = "WeibullMarkovTest.xml";
        String modelXsdFileName = @"C:\SVN_Sandbox\Weibull-Markov\TWeibullMarkov\App_Data\WeibullMarkovModel.xsd";

        [TestMethod]
        public void A1_CreateAndSaveModel()
        {
            String errorMessage = null;

            try
            {
                WeibullMarkovModel model = new WeibullMarkovModel(5, 3, true, null, false, 0.07);
                String[] keys = model.AllocateKeys(4, out errorMessage);
                Assert.IsNotNull(keys, errorMessage);
                Assert.IsTrue(keys.Length == 4, "keys.Length <> 4");
                keys[0] = "раз";
                keys[1] = "два";
                keys[2] = "три";
                keys[3] = "четыре";
                WeibullMarkovConditionState state1 = model.AddConditionState(27.10497512, 3.012514254, 0, 0, 0, 0, out errorMessage);
                Assert.IsNotNull(state1, errorMessage);
                WeibullMarkovConditionState state2 = model.AddConditionState(31.32526401, 3.266009211, 0, 0, 0, 0, out errorMessage);
                Assert.IsNotNull(state2, errorMessage);
                WeibullMarkovConditionState state3 = model.AddConditionState(19.18160534, 5.764593918, 0, 0, 0, 0, out errorMessage);
                Assert.IsNotNull(state3, errorMessage);
                WeibullMarkovConditionState state4 = model.AddConditionState(10.81636701, 4.670420484, 0, 0, 0, 0, out errorMessage);
                Assert.IsNotNull(state4, errorMessage);

                Assert.IsTrue(model.States.Count==4, "Wrong number of condition states");

             

                WeibullMarkovAction action21 = state2.AddAction(1, 4, true, 1.92);
                action21.TranProb[0] = 0.99;
                action21.TranProb[1] = 0.01;

                WeibullMarkovAction action31 = state3.AddAction(1, 4, true, 23.04);
                action31.TranProb[0] = 0.9;
                action31.TranProb[1] = 0.05;
                action31.TranProb[2] = 0.03;
                action31.TranProb[3] = 0.02;

                WeibullMarkovAction action32 = state3.AddAction(2, 4, true, 190.04);
                action32.TranProb[0] = 1.00;
              

                WeibullMarkovAction action41 = state4.AddAction(1, 4, true, 232.28);
                action41.TranProb[0] = 0.7;
                action41.TranProb[1] = 0.2;
                action41.TranProb[2] = 0.05;
                action41.TranProb[3] = 0.05;

                WeibullMarkovAction action42 = state4.AddAction(2, 4, true, 600.0);
                action42.TranProb[0] = 1.00;

                Double F = model.States[0].ComputeTFF();
                Assert.IsTrue(F == 0.99, "F<>0.99");

                Boolean ok = model.SaveToXml(modelXmlFileName, out errorMessage);
                Assert.IsTrue(ok, errorMessage);

                if (ok && !String.IsNullOrEmpty(modelXsdFileName))
                {
                    ok = TWeibullMarkovLibrary.Utilities.ValidateXMLvsXSD(modelXmlFileName, modelXsdFileName, null, out errorMessage);
                    Assert.IsTrue(ok, errorMessage);
                }
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
            
        }

        [Ignore]
        [TestMethod]
        public void A2_SolveModelSimple()
        {
            String errorMessage = null;
            Boolean ok = true;
            String modelXmlFileName = @"C:\SVN_Sandbox\Weibull-Markov\TWeibullMarkov\App_Data\Examples\Paper Example 3.xml";
            String modelXsdFileName = @"C:\SVN_Sandbox\Weibull-Markov\TWeibullMarkov\App_Data\WeibullMarkovModel.xsd";

            try
            {
                WeibullMarkovModel model = WeibullMarkovModel.LoadFromXml(modelXmlFileName, modelXsdFileName, out errorMessage);
                Assert.IsNotNull(model, errorMessage);
                ok = (model != null);
                if (ok)
                {
                    Int32 numIter = 0;
                    Boolean failCostOverridden = false;
                    Double failureCost = -1.0;
                    ok = model.Solve(30, out numIter, out failCostOverridden, out failureCost, out errorMessage);
                    if (ok)
                    {
                        ok = model.SaveToXml("SolvedModel.xml", out errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                ok = false;
            }

            Assert.IsTrue(ok, errorMessage);
        }

    }
}
