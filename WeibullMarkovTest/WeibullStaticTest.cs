using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TWeibullMarkovLibrary;

namespace WeibullMarkovTest
{
    [TestClass]
    public class WeibullStaticTest
    {
        [TestMethod]
        public void AA_TestStaticWeibullMethods()
        {
            Double Eta, Beta;
            Boolean ok = true;
            String errorMessage = null;
            ok = TWeibullMarkovLibrary.Weibull.EstimateParameters(80, 100, 0.01, out Eta, out Beta, out errorMessage);
            Assert.IsTrue(ok, errorMessage);
            Assert.IsTrue(Math.Round(Eta, 8) == 83.53074276, "Eta <> 83.53074276");
            Assert.IsTrue(Math.Round(Beta, 8) == 8.48643187, "Beta <> 8.48643187");

            if (ok)
            {
                ok = TWeibullMarkovLibrary.Weibull.EstimateParameters("80", "100", "1", out Eta, out Beta, out errorMessage);
                Assert.IsTrue(ok, errorMessage);
                Assert.IsTrue(Math.Round(Eta, 8) == 83.53074276, "Eta <> 83.53074276");
                Assert.IsTrue(Math.Round(Beta, 8) == 8.48643187, "Beta <> 8.48643187");
            }

            if (ok)
            {
                Double F = TWeibullMarkovLibrary.Weibull.CumulativeFailure(Eta, Beta, 70);
                Assert.IsTrue(Math.Round(F, 6) == 0.200040, "F <> 0.200040");
                Double f = TWeibullMarkovLibrary.Weibull.OneYearFailure(Eta, Beta, 125);
                Assert.IsTrue(Math.Round(f, 8) == 0.86682264, "f <> 0.86682264");
                Double dfdt = TWeibullMarkovLibrary.Weibull.OneYearFailureFirstDerivative(Eta, Beta, 137);
                Assert.IsTrue(Math.Round(dfdt, 8) == 0.00397321, "dfdt <> 0.00397321");

            }
        }

       

        [TestMethod]
        public void AC_TimeTargetBetaGT1()
        {
            Double beta = 2.85894, eta = 7.50274, W=1000.0, deltaY = 0.005;
            Double alpha = 1.0/(1.0+ 0.0525);
            Int32 NY = 30;
            Int32 numIter = 0;
            String errorMessage = null;
            Int32 y = Weibull.TimeTargetBetaGTOne(eta, beta, NY, deltaY, W, alpha, true, out numIter, out errorMessage);
            Assert.IsTrue(y > 0, errorMessage);
            Assert.IsTrue(y == 32, "y<>32");
        }

        [TestMethod]
        public void AD_TimeTargetBetaLT1()
        {
            Double beta = 0.5, eta = 2.0, W = 1000.0, deltaY = 0.005;
            Double alpha = 1.0 / (1.0 + 0.0525);
            Int32 NY = 30;
            Int32 numIter = 0;
            String errorMessage = null;
            Int32 y = Weibull.TimeTargetBetaLTOne(eta, beta, NY, deltaY, W, alpha, true, out numIter, out errorMessage);
            Assert.IsTrue(y > 0, errorMessage);
            Assert.IsTrue(y == 123, "y<>123");
        }

        [TestMethod]
        public void AE_TimeTargetGT1()
        {
            Double beta = 2.85894, eta = 7.50274, W = 1000.0, deltaY = 0.005;
            Double alpha = 1.0 / (1.0 + 0.0525);
            Int32 NY = 30;
            Int32 numIter = 0;
            String errorMessage = null;
            Int32 y = Weibull.TimeTarget(eta, beta, NY, deltaY, W, alpha, true, out numIter, out errorMessage);
            Assert.IsTrue(y > 0, errorMessage);
            Assert.IsTrue(y == 32, "y<>32");
        }

        [TestMethod]
        public void AF_TimeTargetLT1()
        {
            Double beta = 0.5, eta = 2.0, W = 1000.0, deltaY = 0.005;
            Double alpha = 1.0 / (1.0 + 0.0525);
            Int32 NY = 30;
            Int32 numIter = 0;
            String errorMessage = null;
            Int32 y = Weibull.TimeTarget(eta, beta, NY, deltaY, W, alpha, true, out numIter, out errorMessage);
            Assert.IsTrue(y > 0, errorMessage);
            Assert.IsTrue(y == 123, "y<>123");
        }

        [TestMethod]
        public void AG_TimeTargetEQ1()
        {
            Double beta = 1.0, eta = 2.0, W = 1000.0, deltaY = 0.005;
            Double alpha = 1.0 / (1.0 + 0.0525);
            Int32 NY = 30;
            Int32 numIter = 0;
            String errorMessage = null;
            Int32 y = Weibull.TimeTarget(eta, beta, NY, deltaY, W, alpha, true, out numIter, out errorMessage);
            Assert.IsTrue(y < 0);
           
        }

        [TestMethod]
        public void BA_ShadowCostAtAgeZero()
        {
            Double beta = 2.85894, eta = 7.50274, W = 1000.0, deltaY = 0.005;
            Double alpha = 1.0 / (1.0 + 0.0525);
            Int32 NY = 30;
            Int32 numIter = 0;
            String errorMessage = null;
            Int32 y = Weibull.TimeTarget(eta, beta, NY, deltaY, W, alpha, true, out numIter, out errorMessage);
            Assert.IsTrue(y > 0, errorMessage);
            Assert.IsTrue(y == 32, "y<>32");
            Double shCost = Weibull.ShadowCostAtAgeZero(eta, beta, W, alpha, y, true, out errorMessage);
            Assert.IsTrue(shCost > 0.0, errorMessage);
        }
    }
}
