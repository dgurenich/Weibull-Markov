using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;

namespace TWeibullMarkovLibrary
{
    /// <summary>
    /// Encapsulates the Weibull-Markov model for one condition state (element)
    /// </summary>
    public class WeibullMarkovModel
    {
        public static readonly String WEIBULL_MARKOV_MODEL = "WEIBULL-MARKOV-MODEL";

        private static readonly String _VERSION = "version";
        private static readonly String _NUM_KEYS = "num-keys";
        private static readonly String _NUM_STATES = "num-states";
        private static readonly String _NUM_ACTIONS = "num-actions";
        private static readonly String _FAIL_COST_ESTIMATE = "fcost-estimate";
        private static readonly String _FAIL_COST_VALUE = "fcost-value";
        private static readonly String _FAIL_COST_OVERRIDE = "fcost-override";
        private static readonly String _FAIL_COST_POLICY = "fcost-policy";
        private static readonly String _DISCOUNT_RATE_PCT = "disc-rate-pct";
        private static readonly String _KEYS = "Keys";
        private static readonly String _KEY = "Key";
        private static readonly String _NUMBER = "number";
        private static readonly String _VALUE = "value";
        private static readonly String _CONDITION_STATES = "Condition-States";
        private static readonly String _ver = "1.0";

        private static readonly Int32 _maxIter = 20;
       

        /// <summary>
        /// Number of condition states
        /// </summary>
        public Int32 NumStates = 0;

        /// <summary>
        /// Number of actions excluding Do Nothing
        /// </summary>
        public Int32 NumActions = 0;

        /// <summary>
        /// If True minimum failure cost is to be estimated by the model itself
        /// </summary>
        public Boolean FailCostEstimate = true;

        /// <summary>
        /// Unit failure cost as either preset or estimated
        /// </summary>
        public Double FailureCost = 0.0;

        /// <summary>
        /// When True the model is allowed to override the failure cost set by the user
        /// </summary>
        public Boolean FailCostOverride = false;

        /// <summary>
        /// Value of the failure cost that has been used for policy generation.  See the Solve method
        /// </summary>
        public Double? FailureCostUsedInPolicy = null;

        /// <summary>
        /// Annual discount rate
        /// </summary>
        public Double DiscRate = 0.0;

        /// <summary>
        /// Equivalent discounting factor
        /// </summary>
        private Double _discFactor = 1.0;

        /// <summary>
        /// List of condition states
        /// </summary>
        public List<WeibullMarkovConditionState> States = null;

        private static ILog _log = ConfigureLogger();

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

            log4net.ILog log = log4net.LogManager.GetLogger("WBM-LOGGER");

            return log;

        }

        public String[] Keys = null;

        /// <summary>
        /// Constructructor
        /// </summary>
        /// <param name="numStates">Number of states</param>
        /// <param name="numActions">Number of actions</param>
        /// <param name="failCostEstimate">Model estimates the failure cost</param>
        /// <param name="failCost">Pre-set value of the failure cost (if any)</param>
        /// <param name="failCostOverride">Model is allowed to override the preset value of the failure cost</param>
        /// <param name="discRate">Annual discount rate in percentage points, e.g. 4.5</param>
        public WeibullMarkovModel(Int32 numStates, Int32 numActions, Boolean failCostEstimate, Double? failCost, Boolean failCostOverride, Double discRate)
        {
            NumStates = numStates;
            NumActions = numActions;
            FailCostEstimate = failCostEstimate;
            if (failCost.HasValue)
                FailureCost = failCost.Value;
            FailCostOverride = failCostOverride;
            DiscRate = discRate;
            _discFactor = 1.0 / (1.0 + discRate/100.0);
            FailureCostUsedInPolicy = null;
         
        }

        ~WeibullMarkovModel()
        {
            if (States != null)
                States.Clear();
            Keys = null;
        }


        /// <summary>
        /// Allocates the array of key objects
        /// </summary>
        /// <param name="n">Number of key slots to allocate</param>
        /// <param name="errorMessage">out Error message</param>
        /// <returns>Allocated string array, null on failure</returns>
        public String[] AllocateKeys(Int32 n, out String errorMessage)
        {
            Keys = null;
            errorMessage = null;

            try
            {
                Keys = new String[n];
            }
            catch (Exception ex)
            {
                Keys = null;
                errorMessage = ex.Message;
            }
            return Keys;
        }

        /// <summary>
        /// Adds a condition state
        /// </summary>
        /// <param name="eta">Eta</param>
        /// <param name="beta">Beta</param>
        /// <param name="errorMessage">out Error message</param>
        /// <returns>Created condition state object, null on failure</returns>
        /// <remarks>Attention! Condition states must be added in order</remarks>
        public WeibullMarkovConditionState AddConditionState(Double eta, Double beta, Double doNothCos, Double T, Double f, Double ff, out String errorMessage)
        {
            Boolean ok = true;
            errorMessage = null;

            if (eta <= 0.0)
            {
                ok = false;
                errorMessage = "The scale (Eta) parameter must be positive.";
            }

            if (ok && beta <= 0.0)
            {
                ok = false;
                errorMessage = "The slope (Beta) parameter must be positive.";
            }

            if (ok && NumStates <= 0)
            {
                ok = false;
                errorMessage = "The number of states for the model has not been set or was set as a negative number.";
            }

            if (ok && NumActions <= 0)
            {
                ok = false;
                errorMessage = "The number of actions for the model has not been set or was set as a negative number.";
            }

            if (ok && States != null && States.Count >= NumStates)
            {
                ok = false;
                errorMessage = "Attempt has been made to add a condition state above the declared number " + NumStates.ToString();
            }

            if (ok)
            {
                if (States == null)
                    States = new List<WeibullMarkovConditionState>();
                WeibullMarkovConditionState state = new WeibullMarkovConditionState(eta, beta, doNothCos, T, f, ff);
                state.Number = States.Count + 1;    // States must be added in their natural order: 1, 2, 3, etc.
                States.Add(state);
                return state;
            }

            return null;
        }

        /// <summary>
        /// Converts the object to XML element
        /// </summary>
        /// <param name="doc">Beholding XML document</param>
        /// <param name="errorMessage">out Error message</param>
        /// <returns>Constructed XML element, null on failure</returns>
        public XmlElement ToXmlElement(XmlDocument doc, out String errorMessage)
        {
            XmlElement xml = null;
            errorMessage = null;

            try
            {
                xml = doc.CreateElement(WEIBULL_MARKOV_MODEL);

                XmlAttribute version = doc.CreateAttribute(_VERSION);
                version.Value = _ver;
                xml.Attributes.Append(version);

                XmlAttribute stateNumber = doc.CreateAttribute(_NUM_STATES);
                stateNumber.Value = NumStates.ToString();
                xml.Attributes.Append(stateNumber);

                XmlAttribute actionNumber = doc.CreateAttribute(_NUM_ACTIONS);
                actionNumber.Value = NumActions.ToString();
                xml.Attributes.Append(actionNumber);

                XmlAttribute failCostEst = doc.CreateAttribute(_FAIL_COST_ESTIMATE);
                failCostEst.Value = FailCostEstimate ? "1" : "0";
                xml.Attributes.Append(failCostEst);

                if (!FailCostEstimate)
                {
                    XmlAttribute failCost = doc.CreateAttribute(_FAIL_COST_VALUE);
                    failCost.Value = FailureCost.ToString();
                    xml.Attributes.Append(failCost);

                    XmlAttribute failCostOverride = doc.CreateAttribute(_FAIL_COST_OVERRIDE);
                    failCostOverride.Value = FailCostOverride ? "1" : "0";
                    xml.Attributes.Append(failCostOverride);
                }

                if (FailureCostUsedInPolicy.HasValue)
                {
                    XmlAttribute failCostPolicy = doc.CreateAttribute(_FAIL_COST_POLICY);
                    failCostPolicy.Value = FailureCostUsedInPolicy.Value.ToString("f2");
                    xml.Attributes.Append(failCostPolicy);
                }

                XmlAttribute discRatePct = doc.CreateAttribute(_DISCOUNT_RATE_PCT);
                discRatePct.Value = DiscRate.ToString();
                xml.Attributes.Append(discRatePct);

                if (Keys != null && Keys.Length > 0)
                {
                    XmlAttribute numkeys = doc.CreateAttribute(_NUM_KEYS);
                    numkeys.Value = Keys.Length.ToString();
                    xml.Attributes.Append(numkeys);

                    XmlElement n = doc.CreateElement(_KEYS);
                    for (Int32 i = 0; i < Keys.Length; i++)
                    {
                        XmlElement n2 = doc.CreateElement(_KEY);
                        XmlAttribute keyNumber = doc.CreateAttribute(_NUMBER);
                        keyNumber.Value = (i + 1).ToString();
                        n2.Attributes.Append(keyNumber);
                        XmlAttribute keyValue = doc.CreateAttribute(_VALUE);
                        keyValue.Value = Keys[i];
                        n2.Attributes.Append(keyValue);
                        n.AppendChild(n2);
                    }
                    xml.AppendChild(n);
                }

                if (States != null && States.Count > 0)
                {
                    XmlElement n = doc.CreateElement(_CONDITION_STATES);
                    foreach (WeibullMarkovConditionState state in States)
                    {
                        XmlElement n2 = state.ToXmlElement(doc, out errorMessage);
                        if (n2 == null)
                            throw new Exception(errorMessage);
                        n.AppendChild(n2);
                    }
                    xml.AppendChild(n);
                }
            }
            catch (Exception ex)
            {
                xml = null;
                errorMessage = ex.Message;
            }
            

            return xml;
        }

        /// <summary>
        /// Constructs an object from XML element
        /// </summary>
        /// <param name="xml">XML element</param>
        /// <param name="errorMessage">out Error message</param>
        /// <returns>Constructed element, null on failure</returns>
        public static WeibullMarkovModel FromXmlElement(XmlElement xml, out String errorMessage)
        {
            WeibullMarkovModel model = null;
            errorMessage = null;

            try
            {
                if (xml.Name != WEIBULL_MARKOV_MODEL)
                    throw new Exception ("A <" + WEIBULL_MARKOV_MODEL + "...> XML element expected");
                model = new WeibullMarkovModel(0, 0, true, null, false, 0.0);

                String numStates = xml.GetAttribute(_NUM_STATES);
                if (String.IsNullOrEmpty(numStates))
                    throw new Exception("The '" + _NUM_STATES + "' attribute is missing in the WEIBULL-MARKOV-MODEL XML element");
                model.NumStates = Int32.Parse(numStates);

                String numActions = xml.GetAttribute(_NUM_ACTIONS);
                if (String.IsNullOrEmpty(numActions))
                    throw new Exception("The '" + _NUM_ACTIONS + "' attribute is missing in the WEIBULL-MARKOV-MODEL XML element");
                model.NumActions = Int32.Parse(numActions);

                String failCostEstimate = xml.GetAttribute(_FAIL_COST_ESTIMATE);
                if (String.IsNullOrEmpty(failCostEstimate))
                    throw new Exception("The '" + _FAIL_COST_ESTIMATE + "' attribute is missing in the WEIBULL-MARKOV-MODEL XML element");
                model.FailCostEstimate = failCostEstimate == "1" || failCostEstimate.ToUpper() == "Y";

                String discRatePct = xml.GetAttribute(_DISCOUNT_RATE_PCT);
                model.DiscRate = Double.Parse(discRatePct);
                model._discFactor = 1.0 / (1.0 + model.DiscRate/100.0);

                if (!model.FailCostEstimate)
                {
                    String failCost = xml.GetAttribute(_FAIL_COST_VALUE);
                    if (String.IsNullOrEmpty(failCost))
                        throw new Exception("The '" + _FAIL_COST_VALUE + "' attribute is missing in the WEIBULL-MARKOV-MODEL XML element");
                    model.FailureCost = Double.Parse(failCost);
                    String failCostOverride = xml.GetAttribute(_FAIL_COST_OVERRIDE);
                    if (String.IsNullOrEmpty(failCostOverride))
                        throw new Exception("The '" + _FAIL_COST_OVERRIDE + "'attribute is missing in the WEIBULL-MARKOV-MODEL XML element");
                    model.FailCostOverride = failCostOverride == "1" || failCostOverride.ToUpper() == "Y";
                }

                String failCostPolicy = xml.GetAttribute(_FAIL_COST_POLICY);
                if (!String.IsNullOrEmpty(failCostPolicy))
                    model.FailureCostUsedInPolicy = Double.Parse(failCostPolicy);

                String numKeys = xml.GetAttribute(_NUM_KEYS);
                if (!String.IsNullOrEmpty(numKeys))
                {
                    Int32 n = Int32.Parse(numKeys);
                    if (n > 0)
                    {
                        String[] keys = model.AllocateKeys(n, out errorMessage);
                        if (keys == null)
                            throw new Exception(errorMessage);
                    }
                }

                if (xml.HasChildNodes)
                {
                    foreach(XmlNode n1 in xml.ChildNodes) 
                    {
                        if (n1.Name == _CONDITION_STATES)
                        {
                            model.States = null;
                            model.States = new List<WeibullMarkovConditionState>(model.NumStates);
                            for (Int32 i = 0; i < model.NumStates; i++)
                            {
                                model.States.Add(null); // We reserve slots in the list so that the states were always going in order of their numbers in the list
                            }
                            if (n1.HasChildNodes)
                            {
                                foreach (XmlNode n2 in n1.ChildNodes)
                                {
                                    if (n2.Name == WeibullMarkovConditionState.CONDITION_STATE)
                                    {
                                        WeibullMarkovConditionState state = WeibullMarkovConditionState.FromXmlElement(n2 as XmlElement, out errorMessage);
                                        if (state == null)
                                            throw new Exception(errorMessage);
                                        if (state.Number < 1 || state.Number > model.NumStates)
                                        {
                                            String err = String.Format("The number of the condition state ({0}) is out of range (1 - {1})", state.Number, model.NumStates);
                                            throw new Exception(err);
                                        }
                                        model.States[state.Number - 1] = state;
                                    }
                                }
                            }
                        }

                        else if (n1.Name == _KEYS && model.Keys!= null && model.Keys.Length >0)
                        {
                            if (n1.HasChildNodes)
                            {
                                foreach (XmlNode n2 in n1.ChildNodes)
                                {
                                    if (n2.Name == _KEY)
                                    {
                                        String number = (n2 as XmlElement).GetAttribute(_NUMBER);
                                        if (!String.IsNullOrEmpty(number))
                                        {
                                            throw new Exception("'" + _NUMBER + "' attribute missing in the <" + _KEY + " ...> XML element.");
                                        }
                                        String val = (n2 as XmlElement).GetAttribute(_VALUE);
                                        if (!String.IsNullOrEmpty(val))
                                        {
                                            throw new Exception("'" + _VALUE + "' attribute missing in the <" + _KEY + " ...> XML element.");
                                        }
                                        Int32 num = Int32.Parse(number);
                                        if (num < 1 || model.Keys.Length < num)
                                        {
                                            throw new Exception(String.Format("Key number {0} is outof range (1 - {1}).", num, model.Keys.Length));
                                        }
                                        model.Keys[num-1] = val;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                model = null;
            }

            return model;
        }


        /// <summary>
        /// Saves model object to an XML file
        /// </summary>
        /// <param name="xmlFilePath">Path name of the file to write the output to</param>
        /// <param name="errorMessage">out Error message</param>
        /// <returns>True on success, False on failure</returns>
        public Boolean SaveToXml(String xmlFilePath, out String errorMessage)
        {
            Boolean ok = true;
            errorMessage = null;

            Log.InfoFormat("Saving Weibull-Markov model to the file {0} - started", xmlFilePath);

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration d = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                xmlDoc.AppendChild(d);

                XmlComment comment = xmlDoc.CreateComment(Properties.Settings.Default.XMLComment);
                xmlDoc.AppendChild(comment);

                XmlElement root = this.ToXmlElement(xmlDoc, out errorMessage);
                if (root == null)
                    throw new Exception(errorMessage);

                // Adding schema attribute
                XmlNode s = xmlDoc.CreateNode(XmlNodeType.Attribute, "xsi", "noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                s.Value = Properties.Settings.Default.XSDFile;
                root.Attributes.SetNamedItem(s);

                xmlDoc.AppendChild(root);

                xmlDoc.Save(xmlFilePath);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                ok = false;
                Log.Error(errorMessage);
            }

            Log.InfoFormat("Saving Weibull-Markov model to the file {0} - ended. OK={1}", xmlFilePath, ok);

            return ok;
        }


        /// <summary>
        /// Creates a new WeibullMarkovModel object loading from the XML file
        /// </summary>
        /// <param name="xmlFileName">Name (path) of the XML file</param>
        /// <param name="xsdFileName">Optional (may be null) name of the schema (XSD) file to falidate the XML file against.</param>
        /// <param name="errorMessage">out Error message</param>
        /// <returns>Created model object, null on failure</returns>
        public static WeibullMarkovModel LoadFromXml(String xmlFileName, String xsdFileName, out String errorMessage)
        {
            errorMessage = null;
            WeibullMarkovModel model = null; ;

            try
            {
                Log.InfoFormat("Loading Weibull-Markov model from {0} - started", xmlFileName);
                if (!String.IsNullOrEmpty(xsdFileName))
                {
                    Boolean ok = Utilities.ValidateXMLvsXSD(xmlFileName, xsdFileName, Log, out errorMessage);
                    if (!ok)
                        throw new Exception(errorMessage);
                }

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFileName);

                XmlElement rootElement = xmlDoc.DocumentElement;

                model = WeibullMarkovModel.FromXmlElement(rootElement, out errorMessage);
                if (model == null)
                    throw new Exception(errorMessage);
                    
           }
            catch (Exception ex)
            {
                model = null;
                errorMessage = ex.Message;
                Log.Error(errorMessage);
            }

            Log.InfoFormat("Loading Weibull-Markov model from {0} - ended.  OK={1}", xmlFileName, model != null);

            return model;
        }


        /// <summary>
        /// Resets MDM solution values preparing for the next iteration cycle
        /// </summary>
        /// </summary>
        public void ResetSolution()
        {
            foreach (WeibullMarkovConditionState state in States)
            {
                state.ResetSolution();
            }
        }

        /// <summary>
        /// Solves Markov Decision Model (MDM)
        /// </summary>
        /// <param name="numYears">Planning horizon for the optimal policy (number of years to look ahead for)</param>
        /// <param name="numIter">out Number of iterations run</param>
        /// <param name="errorMessage">out Error message</param>
        /// <param name="failCostOverridden">out True if failure cost was overridden</param>
        /// <param name="failureCost">out Estimated or overriding value of the failure cost</param>
        /// <returns>True on success, False on failure</returns>
        public Boolean Solve(Int32 numYears, out Int32 numIter, out Boolean failCostOverridden, out Double failureCost, out String errorMessage)
        {
            Boolean ok = true;
            numIter = 0;
            errorMessage = null;
            failCostOverridden = false;
            failureCost = FailureCost;

            Log.InfoFormat("Solving Weibull-Markov Decision model - started. Planning horizon: {0}", numYears);
            
            try
            {
                Log.Info("Resetting.");

                _discFactor = 1.0 / (1.0 + DiscRate/100.0);

                Log.InfoFormat("Discounting factor: {0}", _discFactor);

                FailureCostUsedInPolicy = null;

                ResetSolution();
                Int32 NS = States.Count;
                Boolean noChange = true;

                Log.InfoFormat("Checking inputs");

                // Checking inputs
                for (Int32 i = NS - 1; i >= 0; i--)
                {
                    WeibullMarkovConditionState state = States[i];

                    if (state == null)
                    {
                        String err = String.Format("Slot number {0} (zero-based) in the condition state list is null or empty.", i);
                        throw new Exception(err);
                    }

                    if ( i == NS-1 && (state.Actions == null || state.Actions.Count < 1))
                    {
                        String err = String.Format("Last condition state (state number {0}) has no actions specified for it other than Do Nothing.", state.Number);
                        throw new Exception(err);
                    }

                    if (state.Beta == 1.0 || state.ff == 0.0)
                    {
                        Double f = Weibull.OneYearFailure(state.Eta, state.Beta, 1.0);
                        if (f <= 0.0)
                        {
                            String err = String.Format("No optimal solution can be found for condition state {0} because its failure probability is constant over time and equals zero or remains negligibly small.", state.Number);
                            throw new Exception(err);
                        }
                    }
                }

                Log.InfoFormat("Main iteration cycle - started");
                Log.InfoFormat("Nummber of condition states: {0}", NS);

                do
                {
                    noChange = true;
                    numIter++;

                    Log.InfoFormat("Iteration: {0}", numIter);

                    for (Int32 i = NS - 1; i >= 0; i--)
                    {
                        WeibullMarkovConditionState state = States[i];

                        Log.InfoFormat("Iter: {0}\tCondition state: {1}\t{2}\tEta={3}\tBeta={4}\tdfdt({5})={6}", 
                            numIter, state.Number, (i == NS - 1) ? "(last state)" : String.Empty, state.Eta, state.Beta, state.T, state.ff);

                        Log.InfoFormat("Iter: {0}\tCondition state: {1}\tNumber of actions: {2}", numIter, state.Number, state.Actions == null ? 0 : state.Actions.Count);

                        Int32 minCostActionNumber = -1;
                        Double minActionShadowCost = -1.0;

                        // Compute shadow costs for the actions and find the action with the minimum shadow cost
                        if (state.Actions != null && state.Actions.Count > 0)
                        {
                            foreach (WeibullMarkovAction action in state.Actions)
                            {
                                if (action.IsApplicable)
                                {
                                    Double shCost = action.Cost;
                                    for (Int32 j = 0; j < action.TranProb.Length; j++)
                                    {
                                        Double frac = action.TranProb[j];
                                        shCost += _discFactor * frac * States[j].ShadowCost;
                                    }

                                    action.AssignShadowCost(shCost);

                                    Log.InfoFormat("Iter: {0}\tCondition state: {1}\tAction: {2}\tAgency cost: {3:f2}\tShadow cost: {4:f2}",
                                        numIter, state.Number, action.Number, action.Cost, action.ShadowCost);

                                    if (minCostActionNumber < 0 || Math.Round(minActionShadowCost, 2) > Math.Round(shCost, 2))
                                    {
                                        minCostActionNumber = action.Number;
                                        minActionShadowCost = shCost;
                                    }
                                }
                            }
                        }

                       
                        Log.InfoFormat("Iter: {0}\tCondition state: {1}\tMinShadowCostAction: {2}\tMinActionShadowCost: {3:f2}\tDo-Nothing direct (agency) cost: {4:f2}",
                            numIter, 
                            state.Number, 
                            minCostActionNumber, 
                            minActionShadowCost,
                            state.DoNothingCost);

                        if (i == NS - 1)    // If it is the last condition state
                        {
                            Log.InfoFormat("Iter: {0}\tLast state processing", numIter);

                            if (!this.FailCostEstimate)
                            {
                                Double d = Weibull.ShadowCostAtAgeZero(state.Eta, state.Beta, FailureCost, _discFactor, out state.ShadowCostStreamLength, ref state.ShadowCostStream, out errorMessage);
                               
                                Log.InfoFormat("Iter: {0}\tLast state processing\tDN-ShadowCost-in-Year-One: {1} - computed", numIter, d);

                                if (d < 0.0)
                                {
                                    throw new Exception(errorMessage);
                                }

                                d += state.DoNothingCost;

                                if (d < minActionShadowCost && 0 < minCostActionNumber && FailCostOverride)
                                {
                                    d = minActionShadowCost + 0.01;
                                    failCostOverridden = true;
                                    Log.InfoFormat("Iter: {0}\tLast state processing\tDN-ShadowCost-in-Year-One: {1} - as to override", numIter, d);
                                }
                                else
                                {
                                   Log.InfoFormat("Iter: {0}\tLast state processing\tDN-ShadowCost-in-Year-One: {1} - as computed", numIter, d);
                                }

                                state.AssignDoNothingShadowCostAgeZero(d);
                            }
                            else
                            {
                                Double d = minActionShadowCost + 0.01;
                                state.AssignDoNothingShadowCostAgeZero(d);
                                Log.InfoFormat("Iter: {0}\tLast state processing\tDN-ShadowCost-in-Year-One: {1} - estimated", numIter, d);
                            }
                        }
                        else  // ..., i.e. if it is NOT the last state
                        {
                            Double d = Weibull.ShadowCostAtAgeZero(state.Eta, state.Beta, States[i + 1].ShadowCost, _discFactor, out state.ShadowCostStreamLength, ref state.ShadowCostStream, out errorMessage);
                            d += state.DoNothingCost;
                            state.AssignDoNothingShadowCostAgeZero(d);
                            Log.InfoFormat("Iter: {0}\tCondition state: {1}\tDN-ShadowCost-in-Year-One: {2} - as computed", numIter, state.Number, d);
                        }

                        if (minCostActionNumber > 0)
                        {
                            state.AssignMinShadowCostAction(minCostActionNumber);
                        }

                        // Assign action that has the minimum shadow cost (including do nothing)
                        if (minCostActionNumber < 0 || state.DoNothingShadowCostAgeZero < minActionShadowCost)
                        {
                            state.AssignAction(0);
                        }
                        else
                        {
                            state.AssignAction(minCostActionNumber);
                        }

                        Log.InfoFormat("Iter: {0}\tCondition state: {1}\tAssigned action: {2}\tShadow cost: {3}", numIter, state.Number, state.AssignedAction, state.ShadowCost);

                        noChange = noChange && state.NoChange;

                        Log.InfoFormat("Iter: {0}\tCondition state: {1}\tConvergence achieved: {2}", numIter, state.Number, noChange);
                    }

                } while (!noChange && numIter <= _maxIter);

                Log.InfoFormat("Main iteration cycle ended. Converted: {0}.  Iterations run: {1}", noChange, numIter); 

                if (!noChange)
                {
                    String err = String.Format("Conversion of the main iteration cycle has not been achieved after {0} iterations.", numIter);
                    throw new Exception(err);
                }

                if (FailCostEstimate || failCostOverridden)
                {
                    Log.InfoFormat("Calculating the failure cost.");

                    WeibullMarkovConditionState lastS = States[NS-1];

                    if (lastS.ff == 0.0 || lastS.Beta == 1.0)
                    {
                        // f cannot be zero because it has been checked against it in the beginning
                        Double f = Weibull.OneYearFailure(lastS.Eta, lastS.Beta, 1.0);
                        Double d = lastS.DoNothingShadowCostAgeZero - lastS.DoNothingCost;
                        failureCost = d * (1.0 - _discFactor * (1.0 - f)) / f;
                        // we are done because f does not change over time

                        Log.InfoFormat("For constant deterioration rate failure cost is: {0:f2}", failureCost);
                    }
                    else
                    {
                        if (!FailCostEstimate)
                        {
                            failureCost = FailureCost;  // Use user's input as the first approximation
                        }
                        else //..., i.e. if it has to be calculated from scratch (FailCostEstimate==True)
                        {
                            Double f = Weibull.OneYearFailure(lastS.Eta, lastS.Beta, 1.0);
                            // To calculate the first approximation we use 1% of failure if actual fialure probability is
                            // less than 0.01 at age zero (in year 1)
                            f = Math.Max(f, 0.01);
                            Double d = lastS.DoNothingShadowCostAgeZero - lastS.DoNothingCost;
                            failureCost = d * (1.0 - _discFactor * (1.0 - f)) / f;
                        }

                        Log.InfoFormat("Failure cost approximation: {0:f2}", failureCost);

                        // Make sure that the crude estimate of failure cost delivers tangible (i.e. >=0.01) cost at age zero (year 1).
                        Double ageZeroCost = 0.0;
                        Int32 n = 0;

                        while (n < _maxIter)
                        {
                            ageZeroCost = Weibull.ShadowCostAtAgeZero(lastS.Eta, lastS.Beta, failureCost, _discFactor, out lastS.ShadowCostStreamLength, ref lastS.ShadowCostStream, out errorMessage);
                            if (ageZeroCost < 0.0)
                                throw new Exception(errorMessage);
                            if (Math.Round(ageZeroCost, 3) <= 0.005)  // If we are loosing precision multiply failure cost by 10 and try again
                            {
                                failureCost *= 10.0;
                            }
                            else
                            {
                                break;
                            }
                            n++;

                            Log.InfoFormat("Failure cost approximation iteration {0}. Shadow cost at age zero if approximated failure cost used: {0}", n, ageZeroCost);
                        }

                        if (n >= _maxIter)
                        {
                            String err = String.Format("Failure cost could not be estimated after {0} iterations.", n);
                            throw new Exception(err);
                        }

                        // From this moment on we do not pay attention to the direct do-nothing cost and compare only their
                        // shadow-cost (i.e. failure-cost related components).

                        // Adjsut it making use of linearity
                        Log.InfoFormat("Shadow cost at age-zero - used by the policy: {0:f2}", lastS.DoNothingShadowCostAgeZero);
                        Log.InfoFormat("Failure-related shadow cost at age-zero - used by the policy: {0:f2}", lastS.DoNothingShadowCostAgeZero - lastS.DoNothingCost);

                        failureCost *= (lastS.DoNothingShadowCostAgeZero - lastS.DoNothingCost);
                        failureCost /= ageZeroCost;

                        Log.InfoFormat("Adjusted failure cost: {0:f2}", failureCost);


                        ageZeroCost = Weibull.ShadowCostAtAgeZero(lastS.Eta, lastS.Beta, failureCost, _discFactor, out lastS.ShadowCostStreamLength, ref lastS.ShadowCostStream, out errorMessage);

                        Log.InfoFormat("Failure-related shadow cost at age zero - adjusted: {0:f2}",  ageZeroCost);

                        if (ageZeroCost < 0.0)
                        {
                            throw new Exception(errorMessage);
                        }

                        if (Math.Round(ageZeroCost, 2) != Math.Round(lastS.DoNothingShadowCostAgeZero, 2))
                        {
                            Log.WarnFormat("Computed failure cost ({0:f2}) does not deliver the required shadow cost in year one ({1:f2}). The value it delivers is {2:f2}",
                                failureCost, lastS.DoNothingShadowCostAgeZero, ageZeroCost);
                        }
                    }
                }

                Log.InfoFormat("Generating policy - started.");
                FailureCostUsedInPolicy = failureCost;
                Log.InfoFormat("Failure cost used: {0}", FailureCostUsedInPolicy);
                Log.InfoFormat("Alpha-adjusted failure cost used: {0}", FailureCostUsedInPolicy * _discFactor);

                for (Int32 j = 0; j < NS; j++)
                {
                    WeibullMarkovConditionState state = States[j];
                    state.Recommendations.Clear();

                    Double dTarget = (j == NS - 1) ? FailureCostUsedInPolicy.Value : States[j + 1].DoNothingShadowCostAgeZero;
                    dTarget *= _discFactor;

                    Log.InfoFormat("Condition state: {0}\tTargetCost: {1}", state.Number, state.ff >= 0.0 ? dTarget : 0.0);

                    Int32 yMax = Math.Min(numYears, state.ShadowCostStreamLength);

                    // The first pass is to establish yMax, the second one is to add recommendations

                    for (Int32 iPass = 0; iPass < 2; iPass++)
                    {
                        for (Int32 y = 0; y < yMax; y++)
                        {
                            Double d = state.ShadowCostStream[y];
                            Int32 A = 0;
                            Double B = 0.0;
                            if (0 < state.MinShadowCostAction && Math.Round(d + state.DoNothingCost - state.MinActionShadowCost, 2) >= 0.01)
                            {
                                A = state.MinShadowCostAction;
                                B = Math.Round(d - state.MinActionShadowCost, 2);
                            }

                            if (iPass > 0)
                            {
                                WeibullMarkovStatePolicyRecommendation R = new WeibullMarkovStatePolicyRecommendation(y + 1, A, B);
                                state.Recommendations.Add(R);
                            }

                            Log.InfoFormat("Pass: {7}\tState: {0}\tYear={1,3:d}\tShC={2,9:f2}\tMinAct={3,3}\tMinActShC={4,9:f2}\tActRec={5}\tUnitBenf={6,9:f2}",
                              state.Number, y + 1, d, state.MinShadowCostAction, state.MinActionShadowCost, A, B, iPass);

                            if (state.ff == 0.0 || state.Beta == 1.0)
                            {
                                yMax = 1;
                                break;
                            }

                            if (iPass == 0 && A == 0 && state.ff >= 0.0 && y == yMax - 1)
                            {
                                // If f-probability is growing but it is still do nothing in year yMax then cut it at the first year;
                                yMax = 1;
                                break;
                            }
                         
                            // Saturation conditions
                            if (iPass==0 && y > 0 && state.ff >= 0.0 && Math.Round(d, 2) >= Math.Round(dTarget, 2))
                            {
                                yMax = A==0 ? 1 : y + 1;
                                break;
                            }

                            if (iPass == 0 && y > 0 && state.ff < 0.0 && Math.Round(d, 2) <= 0.0)
                            {
                                yMax = y + 1;
                                break;
                            }

                            if (iPass == 0 && A == 0 && state.ff <= 0.0)
                            {
                                yMax = y + 1;
                                break;  // If f-probability is decreasing and it is do-nothing in year Y then it can only be  do-nothing later
                            }
                        }
                    }
                }

                Log.InfoFormat("Generating policy - ended.");

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                ok = false;
                Log.Error(errorMessage);
            }

            Log.InfoFormat("Solving Weibull-Markov Decision model - ended. OK={0}", ok);

            return ok;
        }
    }
}
