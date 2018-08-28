using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;

namespace TWeibullMarkovLibrary
{
    public class Weibull
    {

        private static ILog _log = ConfigureLogger();
        private static Int32 _maxIterNewton = 30;  // Maximum number of iterations for the Newton method

        /// <summary>
        ///   Logger which can be assigned from 
        /// </summary>
        public static ILog Log
        {
            get
            {
                return _log;
            }

        }


        //LOGGER
        /// <summary>
        /// Static function that configures the log4net logger object.
        /// </summary>
        /// <returns>Interface to the logger.</returns>
        static log4net.ILog ConfigureLogger()
        {
            // Programmatic configuration
            // follows (with some streamlining) the example from Brendan Long and Ron Grabowski
            // org.apache.logging.log4net-user
            // These config statements create a RollingFile Appender.  Rolling File Appenders rollover on each execution of the test harness, 
            // in this case, following the Composite RollingMode.  Alternative log4net appenders may be added  or replace this default appender at the programmer's discretion.

            // PatternLayout layout = new PatternLayout("%d [%t] %-5p %c - %m%n");

            PatternLayout layout = new PatternLayout("%d %-5p %c - %m%n");
            log4net.Appender.RollingFileAppender appender = new RollingFileAppender();

            appender.Layout = layout;
            appender.AppendToFile = true;
            appender.MaxFileSize = 10000000;
            appender.RollingStyle = RollingFileAppender.RollingMode.Composite;
            appender.StaticLogFileName = true;

            appender.File = Properties.Settings.Default.log4netDir; // all logs will be created in the subdirectory logs below where the test harness is executing

            // Configure filter to accept log messages of any level.
            log4net.Filter.LevelMatchFilter traceFilter = new log4net.Filter.LevelMatchFilter();
            traceFilter.LevelToMatch = log4net.Core.Level.Debug;
            appender.ClearFilters();
            appender.AddFilter(traceFilter);

            appender.ImmediateFlush = true;
            appender.ActivateOptions();

            // Attach appender into hierarchy
            log4net.Repository.Hierarchy.Logger root =
                ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root;
            root.AddAppender(appender);
            root.Repository.Configured = true;

            log4net.ILog log = log4net.LogManager.GetLogger("WEIBULL-LOGGER");

            return log;

        }


        private static Double _ln2 = Math.Log(2.0);
        private static Int32 _maxIter = 100;
        private static Double _maxTime = 1000;      // Thousand years is safe enough period to shoot ahead
        private static Double _miniPenny = 0.0025;  // One quarter of a cent is small enough

        public static Boolean EstimateParameters(Double t50, Double? t9X, Double? x, out Double eta, out Double beta, out String errorMessage)
        {
            Boolean ok = true;
            errorMessage = null;
            eta = 1.0;
            beta = 1.0;

            try
            {
                if (t9X.HasValue && x.HasValue)
                {
                    if (t50 <= 0.0)
                    {
                        throw new Exception("50% survival period must be greater than zero.");
                    }
                    if (x.Value >= 0.5)
                    {
                        throw new Exception("Second survival threshold must be higher than 50%.");
                    }
                    if (t9X.Value <= t50 && x.Value < 0.5)
                    {
                        String err = String.Format("Survival period for the second survival threshold ({0}%) must be greater than for 50%", x.Value * 100.0);
                        throw new Exception(err);
                    }

                    Double a = 1.0 / x.Value;
                    Double b = _ln2 / Math.Log(a);
                    Double nom = Math.Log(b);
                    Double denom = Math.Log(t50) - Math.Log(t9X.Value);
                    beta = nom / denom;
                    eta = t50 / Math.Exp(Math.Log(_ln2) / beta);
                }
                else
                {
                    beta = 1.0;
                    eta = t50 / Math.Exp(Math.Log(_ln2));
                }
            }
            catch (Exception ex)
            {
                ok = false;
                errorMessage = ex.Message;
                eta = -1.0;
                beta = -1.0;
            }
            return (ok);
        }


        public static Boolean EstimateParameters(String s50, String s9X, String sX, out Double eta, out Double beta, out String errorMessage)
        {
            Boolean ok = true;
            errorMessage = null;
            eta = 1.0;
            beta = 1.0;

            try
            {
                Double t50 = 0;
                Double? t9X = null;
                Double? x = null;

                if (String.IsNullOrEmpty(s50) || !Double.TryParse(s50, out t50))
                {
                    ok = false;
                    throw new Exception(String.Format("'{0}' is not a number", s50));
                }

                Double d = 0.0;
                Double b = 0.0;
                if (!String.IsNullOrEmpty(s9X) && !String.IsNullOrEmpty(sX) && Double.TryParse(sX, out d))
                {

                    if (!Double.TryParse(s9X, out b))
                    {
                        throw new Exception(String.Format("'{0}' is not a number", s9X));
                    }
                    else
                    {
                        t9X = b;
                    }

                    if (d < 0) 
                    {
                        throw new Exception("Second survival threshold may not be less than zero.");
                    }
                    else if (d >= 1.0)  // Assume that sX is specified in percentage points
                    {
                        x = d / 100.0;
                    }
                    else {
                        x = d;    // Assume that sX is specified as a fraction of one
                    }
                }

                ok = EstimateParameters(t50, t9X, x, out eta, out beta, out errorMessage);
               
            }
            catch (Exception ex)
            {
                ok = false;
                errorMessage = ex.Message;
                eta = -1.0;
                beta = -1.0;
            }
            return (ok);
        }



        public static Double CumulativeSurvival(Double eta, Double beta, Double t)
        {
            if (t <= .0)
            {
                return 1.0;
            }
            return Math.Exp(-Math.Pow(t / eta, beta));
        }

        public static Double CumulativeFailure(Double eta, Double beta, Double t)
        {
            return 1.0 - CumulativeSurvival(eta, beta, t);
        }

        public static Double OneYearSurvival(Double eta, Double beta, Double t)
        {
            if (t < 1)
            {
                return 1.0;
            }
            Double A = -Math.Pow(t / eta, beta);
            Double B = -Math.Pow((t - 1.0) / eta, beta);
            return Math.Exp(A - B);
        }

        

        public static Double OneYearFailure(Double eta, Double beta, Double t)
        {
            return (1.0 - OneYearSurvival(eta, beta, t));
        }

        public static Double OneYearFailureFirstDerivative(Double eta, Double beta, Double t)
        {
            if (t < 1)
            {
                return 0.0;     // Just to avoid exception
            }
            Double R = beta / eta;
            Double b = beta - 1.0;
            Double C = Math.Pow(t / eta, b);
            Double D =  Math.Pow((t - 1) / eta, b);
            Double s = OneYearSurvival(eta, beta, t);
            return R * (C - D) * s;
        }

        public static Double OneYearSurvivalFirstDerivative(Double eta, Double beta, Double t)
        {
            return -OneYearFailureFirstDerivative(eta, beta, t);
        }

       

        /// <summary>
        /// Computes the Y-cap value for the case when beta>1.  Constructs the equation (M-6) and then uses the Newton's method to solve it.
        /// </summary>
        /// <param name="eta">Scale</param>
        /// <param name="beta">Slope</param>
        /// <param name="Y">Planning horizon length</param>
        /// <param name="deltaY">Precision at time Y, usually is set to 0.005 (one half of one cent)</param>
        /// <param name="W">Non-discounted asyptotic level (failure coset or the next state's shadow cost at age zero</param>
        /// <param name="alpha">Discounting multiplier</param>
        /// <param name="logIt">When set to True causes the function to log the iterations</param>
        /// <param name="numIter">Number of iterations (returned) done by the Newton's method</param>
        /// <param name="errorMessage">Error message (returned), null if no error</param>
        /// <returns>Target value for Y-cap, -1 if error</returns>
        public static Int32 TimeTargetBetaGTOne(Double eta, Double beta, Int32 Y, Double deltaY, Double W, Double alpha, 
                Boolean logIt, out Int32 numIter, out String errorMessage)
        {
            String fn = "TimeTargetBetaGTOne";
            Int32 YCap = -1;
            errorMessage = null;
            numIter = 0;

            try
            {
                if (beta <= 1.0)
                {
                    throw new Exception("The slope parameter (beta) must be greater than 1.");
                }

                if (eta <= 0.0)
                {
                    throw new Exception("The scale parameter (eta) must be greater than 0.");
                }

                if (Y < 1)
                {
                    throw new Exception("The length of the planning horizon (Y) must be greater than 0.");
                }

                if (deltaY <= 0.0)
                {
                    throw new Exception("Precision thershold (deltaY) must be greater than 0.");
                }

                if (W <= 0.0)
                {
                    throw new Exception("The non-discounted asymptotic level for the shadow cost (W) must be gereater than 0");
                }

                if (alpha <= 0.0 || 1.0 < alpha)
                {
                    throw new Exception("The discounting multiplier (alpha) must be a value between 0 and 1");
                }

                Double SY = Weibull.CumulativeSurvival(eta, beta, (Double)Y);
                Double a = Math.Log(deltaY / W);
                Double b = Math.Log(SY);
                Double c = Math.Log(alpha);
                Double d = Math.Pow(eta, beta);
                Double e = a + b + Y * c;   // [ square bracketed ] expression in M-6
                Double f = d * e;           // free term of M-6

                if (logIt && Log != null)
                {
                    Log.Info(fn);
                    Log.Info("Inputs:");
                    Log.InfoFormat("eta (scale) = {0}", eta);
                    Log.InfoFormat("beta (slope)= {0}", beta);
                    Log.InfoFormat("W = {0}", W);
                    Log.InfoFormat("deltaY = {0}", deltaY);
                    Log.InfoFormat("alpha (discounting multiplier) = {0}", alpha);
                    Log.InfoFormat("Y (planning horizon) = {0}", Y);
                    Log.Info("Intermediate terms:");
                    Log.InfoFormat("S(Y) = {0}", SY);
                    Log.InfoFormat("ln(deltaY/W) = {0}", a);
                    Log.InfoFormat("ln[S(Y)] = {0}", b);
                    Log.InfoFormat("ln(alpha) = {0}", c);
                    Log.InfoFormat("eta^beta = {0}", d);
                    Log.InfoFormat("[ ] = {0}", e);
                    Log.InfoFormat("free term of (M-6) = {0}", f);
                    Log.Info("Iterations:");
                    Log.InfoFormat("{0,6}\t{1,12}\t{2,12}\t{3,12}\t{4,12}\t{5,12}", "NIter", "Z", "F(Z)", "F'(Z)", "Step", "Solved");
                }

                Boolean rootFound = false;
                Double Z = (Double)(Y+1); // When beta>1 we start from Y+1
               
                do
                {
                    Double F = Math.Pow(Z, beta) - c * d * Z + f;           // F(Z) of M-6
                    Double G = beta * Math.Pow(Z, beta - 1.0) - c * d;      // F'(Z) of M-6
                    rootFound = Math.Abs(F) < 1e-10;
                    Double dZ = -F / G;                                     // Step

                    rootFound = Math.Abs(F) < 1e-10 && Math.Abs(dZ) < 1e-10;

                    if (logIt && Log != null) 
                    {
                        Log.InfoFormat("{0,6}\t{1,12:e4}\t{2,12:e4}\t{3,12:e4}\t{4,12:e4}\t{5,12}", numIter, Z, F, G, dZ, rootFound);
                    }

                     Z += dZ;
                     numIter++;
                } while (!rootFound && numIter < _maxIterNewton);

                if (!rootFound)
                {
                    throw new Exception(String.Format("Root not found after {0} iterations.", numIter));
                }

                Boolean bMaxed = false;
                YCap = (Int32)(Z + 1.0) - 1;    // When beta>1 round it up and the deduct 1
                if (YCap < Y + 1)
                {
                    YCap = Y + 1;
                    bMaxed = true;
                }
                if (logIt && Log != null)
                {
                    Log.InfoFormat("Y-cap = {0}, maxed={1}", YCap, bMaxed);
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                YCap = -1;
                if (Log != null)
                {
                    Log.ErrorFormat("Function: {0}. Error: {1}", fn, errorMessage);
                }
            }

            return YCap;
        }


        /// <summary>
        /// Computes the Y-cap value for the case when beta<1.  Constructs the equation (M-8) and then uses the Newton's method to solve it.
        /// </summary>
        /// <param name="eta">Scale</param>
        /// <param name="beta">Slope</param>
        /// <param name="Y">Planning horizon length</param>
        /// <param name="deltaY">Precision at time Y, usually is set to 0.005 (one half of one cent)</param>
        /// <param name="W">Non-discounted asyptotic level (failure coset or the next state's shadow cost at age zero</param>
        /// <param name="alpha">Discounting multiplier</param>
        /// <param name="logIt">When set to True causes the function to log the iterations</param>
        /// <param name="numIter">Number of iterations (returned) done by the Newton's method</param>
        /// <param name="errorMessage">Error message (returned), null if no error</param>
        /// <returns>Target value for Y-cap, -1 if error</returns>
        public static Int32 TimeTargetBetaLTOne(Double eta, Double beta, Int32 Y, Double deltaY, Double W, Double alpha,
                Boolean logIt, out Int32 numIter, out String errorMessage)
        {
            String fn = "TimeTargetBetaLTOne";
            Int32 YCap = -1;
            errorMessage = null;
            numIter = 0;

            try
            {
                if (beta >= 1.0)
                {
                    throw new Exception("The slope parameter (beta) must be less than 1.");
                }

                if (eta <= 0.0)
                {
                    throw new Exception("The scale parameter (eta) must be greater than 0.");
                }

                if (Y < 1)
                {
                    throw new Exception("The length of the planning horizon (Y) must be greater than 0.");
                }

                if (deltaY <= 0.0)
                {
                    throw new Exception("Precision thershold (deltaY) must be greater than 0.");
                }

                if (W <= 0.0)
                {
                    throw new Exception("The non-discounted asymptotic level for the shadow cost (W) must be gereater than 0");
                }

                if (alpha <= 0.0 || 1.0 < alpha)
                {
                    throw new Exception("The discounting multiplier (alpha) must be a value between 0 and 1");
                }

                if (Log != null)
                {
                    Log.Warn("Dealing with 'infant mortality'");
                }

                Double SY = Weibull.CumulativeSurvival(eta, beta, (Double)Y);
                Double a = deltaY / W;
                Double b = Math.Pow(alpha, Y-1.0);
                Double c = Math.Log(alpha);
                Double d = beta / Math.Pow(eta, beta);


                Double f = -a * b * SY;           // free term of M-8

                if (logIt && Log != null)
                {
                    Log.Info(fn);
                    Log.Info("Inputs:");
                    Log.InfoFormat("eta (scale) = {0}", eta);
                    Log.InfoFormat("beta (slope)= {0}", beta);
                    Log.InfoFormat("W = {0}", W);
                    Log.InfoFormat("deltaY = {0}", deltaY);
                    Log.InfoFormat("alpha (discounting multiplier) = {0}", alpha);
                    Log.InfoFormat("Y (planning horizon) = {0}", Y);
                    Log.Info("Intermediate terms:");
                    Log.InfoFormat("S(Y) = {0}", SY);
                    Log.InfoFormat("deltaY/W = {0}", a);
                    Log.InfoFormat("alpha^(Y-1) = {0}", b);
                    Log.InfoFormat("ln(alpha) = {0}", c);
                    Log.InfoFormat("beta/eta^beta = {0}", d);
                    Log.InfoFormat("free term of (M-8) = {0}", f);
                    Log.Info("Iterations:");
                    Log.InfoFormat("{0,6}\t{1,12}\t{2,12}\t{3,12}\t{4,12}\t{5,12}", "NIter", "Z", "F(Z)", "F'(Z)", "Step", "Solved");
                }

                Boolean rootFound = false;
                Double Z = (Double)(Y); // When beta<1 start from Y, not from Y+1

                do
                {
                    Double SZ = Weibull.CumulativeSurvival(eta,beta, Z);
                    Double F = Math.Pow(alpha,Z)*beta/eta*Math.Pow(Z/eta, beta-1)*SZ + f;           // F(Z) of M-8
                    Double G = d * Math.Pow(alpha, Z) * Math.Pow(Z, beta - 2.0) * (Z * c + beta - d * Math.Pow(Z, beta) - 1) * SZ;  // F'(Z) of M-9
                    rootFound = Math.Abs(F) < 1e-10;
                    Double dZ = -F / G;                                     // Step

                    rootFound = Math.Abs(F) < 1e-10 && Math.Abs(dZ) < 1e-10;

                    if (logIt && Log != null)
                    {
                        Log.InfoFormat("{0,6}\t{1,12:e4}\t{2,12:e4}\t{3,12:e4}\t{4,12:e4}\t{5,12}", numIter, Z, F, G, dZ, rootFound);
                    }

                    Z += dZ;
                    numIter++;
                } while (!rootFound && numIter < _maxIterNewton);

                if (!rootFound)
                {
                    throw new Exception(String.Format("Root not found after {0} iterations.", numIter));
                }

             

                Boolean bMaxed = false;
                YCap = (Int32)(Z + 1.0);    // When beta<1 - round up and DO NOT deduct 1
                if (YCap < Y+1)
                {
                    YCap = Y+1;
                    bMaxed = true;
                }
                if (logIt && Log != null)
                {
                    Log.InfoFormat("Y-cap = {0}, maxed={1}", YCap, bMaxed);
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                YCap = -1;
                if (Log != null)
                {
                    Log.ErrorFormat("Function: {0}. Error: {1}", fn, errorMessage);
                }
            }

            return YCap;
        }

        /// <summary>
        /// Computes the Y-cap value for beta<>1.  Constructs the equation (M-6) or (M-8) and then uses the Newton's method to solve it.
        /// </summary>
        /// <param name="eta">Scale</param>
        /// <param name="beta">Slope</param>
        /// <param name="Y">Planning horizon length</param>
        /// <param name="deltaY">Precision at time Y, usually is set to 0.005 (one half of one cent)</param>
        /// <param name="W">Non-discounted asyptotic level (failure coset or the next state's shadow cost at age zero</param>
        /// <param name="alpha">Discounting multiplier</param>
        /// <param name="logIt">When set to True causes the function to log the iterations</param>
        /// <param name="numIter">Number of iterations (returned) done by the Newton's method</param>
        /// <param name="errorMessage">Error message (returned), null if no error</param>
        /// <returns>Target value for Y-cap, -1 if error</returns>
        public static Int32 TimeTarget(Double eta, Double beta, Int32 Y, Double deltaY, Double W, Double alpha,
             Boolean logIt, out Int32 numIter, out String errorMessage)
        {
            errorMessage = null;
            numIter = 0;
            Int32 YCap = -1;
            if (beta == 1.0)
            {
                errorMessage = "Time target does not exist when the slope parameter (beta) equals one.";
                Log.Error(errorMessage);
            }
            else if (beta > 1.0)
            {
                Log.InfoFormat("Estimating Y-Cap for beta>1");
                YCap = TimeTargetBetaGTOne(eta, beta, Y, deltaY, W, alpha, logIt, out numIter, out errorMessage);
            }
            else
            {
                Log.InfoFormat("Estimating Y-Cap for beta<1");
                YCap = TimeTargetBetaLTOne(eta, beta, Y, deltaY, W, alpha, logIt, out numIter, out errorMessage);
            }

            return YCap;
        }


        public static Double ShadowCostAtAgeZero(Double eta, Double beta, Double W, Double alpha, Int32 YCap, Boolean logIt, out String errorMessage)
        {
            errorMessage = null;
            Double Cost = -1.0;
            try
            {
                Cost = W * alpha;
                Int32 i = YCap;
                if (logIt && Log != null)
                {
                    Log.Info("ShadowCostAtAgeZero");
                    Log.InfoFormat("{0,6}\t{1,10}", "Age", "Cost");
                    Log.InfoFormat("{0,6}\t{1,10:f2}", i, Cost);
                }
                while (i > 0)
                {
                    Double f = OneYearFailure(eta, beta, ((Double)i));
                    Double s = 1.0 - f;
                    Cost = alpha * (s * Cost + f * W);
                    i--;
                    Log.InfoFormat("{0,6}\t{1,10:f2}", i, Cost);
                }
                if (logIt && Log != null)
                {
                    Log.InfoFormat("V(action=0) = {0:f2}", Cost);
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                Cost = -1.0;
                if (Log != null)
                    Log.Error(errorMessage);
            }
            return Cost;
        }


        #region USE_STREAM
        /// <summary>
        /// Calculates the shadow cost in age-zero (year 1) given the target cost and the length of the horizon
        /// </summary>
        /// <param name="Eta">Scale parameter</param>
        /// <param name="Beta">Slope parameter</param>
        /// <param name="A">Target cost (non-discounted)</param>
        /// <param name="alpha">Discounting factor</param>
        /// <param name="iForecast">Horizon length</param>
        /// <param name="CostStream">Stream of costs to populate</param>
        /// <param name="errorMessage">out Error message</param>
        /// <returns>Shadow cost at age zero</returns>
        public static Double ShadowCostAtAgeZero(Double Eta, Double Beta, Double A, Double alpha, Int32 iForecast, ref Double[] CostStream, out String errorMessage)
        {
            errorMessage = null;
            Double Cost = alpha * A;

            Log.DebugFormat("ShadowCostAtAgeZero\tEta={0}\tBeta={1}\tA={2}\talpha={3}\tiForecast={4}", Eta, Beta, A, alpha, iForecast);
            try
            {
                if (CostStream != null)
                    CostStream = null;
                CostStream = new Double[iForecast];
                Int32 i = iForecast;
                while (i > 0)
                {
                    Double f = OneYearFailure(Eta, Beta, ((Double)i));
                    Double s = 1.0 - f;
                    Cost = alpha * (s * Cost + f * A);
                    CostStream[i - 1] = Cost;
                    Double CostPrev = (Cost / alpha - f * A) / s;
                    Log.DebugFormat("Age={0}\tf={1}\ts={2}\tCost={3}\tCostPrev={4}", i-1, f, s, Cost, CostPrev);
                    i--;
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                Log.Error(errorMessage);
                Cost = -1.0;
            }

            Log.DebugFormat("ShadowCostAtAgeZero\tCost={0}", Cost);

            return Cost;
        }
       
        public static Double ShadowCostAtAgeZero(Double Eta, Double Beta, Double A, Double alpha, out Int32 iTarget, ref Double[] CostStream, out String errorMessage)
        {
            errorMessage = null;
            Double Cost = -1.0;
            Int32 numIter = 0;
            iTarget = 0;

            try
            {
                Double sTarget = _miniPenny / A / alpha;
                Double fTarget = 1.0 - sTarget;
                Int32 T = TimeTarget(Eta, Beta, 30, _miniPenny, A, alpha, false, out numIter, out errorMessage);
                if (T <= 0)
                    throw new Exception(errorMessage);
                iTarget = (Int32)T;
                Cost = ShadowCostAtAgeZero(Eta, Beta, A, alpha, iTarget, ref CostStream, out errorMessage);
                if (Cost < 0.0)
                    throw new Exception(errorMessage);
            }
            catch (Exception ex)
            {
                Cost = -1.0;
                errorMessage = ex.Message;
                Log.Error(errorMessage);
            }

            return Cost;
        }
        #endregion
    }
}
