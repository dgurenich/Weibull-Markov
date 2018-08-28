using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TWeibullMarkovLibrary
{
    /// <summary>
    /// Data object that encapsulates all information pertinent to a condition state;
    /// </summary>
    public class WeibullMarkovConditionState
    {

        public static readonly String CONDITION_STATE = "Condition-State";

        private static readonly String _NUMBER = "number";
        private static readonly String _ETA = "eta";
        private static readonly String _BETA = "beta";
        private static readonly String _ACTIONS = "Actions";
        private static readonly String _POLICY = "Policy";
        private static readonly String _DO_NOTHING_COST = "dn-cost";
        private static readonly String _T_TABULATED = "t";
        private static readonly String _F_PROB_AT_T = "f";
        private static readonly String _DFDT_AT_T = "dfdt";
        private static readonly String _T50 = "t50";
        private static readonly String _T9X = "t9x";
        private static readonly String _X = "x";

        /// <summary>
        /// Condition state number
        /// </summary>
        public Int32 Number = 0;

        /// <summary>
        /// Weibull scale parameter
        /// </summary>
        public Double Eta = 1.0;

        /// <summary>
        /// Weibull slope parameter
        /// </summary>
        public Double Beta = 1.0;

        /// <summary>
        /// Immediate cost of do-nothing.  Normally equas zero
        /// </summary>
        public Double DoNothingCost = 0.0;

        private Double? _T = null;

        /// <summary>
        /// Time (T) for which the value of f(t) was last tabulated
        /// </summary>
        public Double T
        {
            get 
                {
                if (!_T.HasValue)
                  ComputeTFF();
                return _T.Value; 
                }
            set { _T = value; }
        }


        private Double? _f = null;

        /// <summary>
        /// Last tablualed value of f(t)
        /// </summary>
        public Double f
        {
            get {
                if (!_f.HasValue)
                {
                    ComputeTFF();
                }
                return _f.Value; }
            set { _f = value; }
        }

        private Double? _ff = null;

        /// <summary>
        /// Last tabulated value of the first derivative, df(t)/dt
        /// </summary>
        public Double ff
        {
            get {
                if (!_ff.HasValue)
                    ComputeTFF();
                return _ff.Value; 
                }
            set { _ff = value; }
        }


        /// <summary>
        /// Used only for communication with the UI
        /// </summary>
        public Double? t50 = null;
        public Double? t9X = null;
        public String x = null;

        /// <summary>
        /// List of actions asscoiated with the state
        /// </summary>
        public List<WeibullMarkovAction> Actions = new List<WeibullMarkovAction>();

        /// <summary>
        /// List of policy recommendations associated with the state
        /// </summary>
        public List<WeibullMarkovStatePolicyRecommendation> Recommendations = new List<WeibullMarkovStatePolicyRecommendation>();


        /// <summary>
        /// Shadow cost computed at the current iteration
        /// </summary>
        public Double ShadowCost = 0.0;

        /// <summary>
        /// Shadow cost computed at the previous iteration
        /// </summary>
        public Double PrevShadowCost = -1.0;

        /// <summary>
        /// Action assigned at the current situation
        /// </summary>
        public Int32 AssignedAction = 0;

        /// <summary>
        /// Action assigned at the previous iteration
        /// </summary>
        public Int32 PrevAssignedAction = -1;

        /// <summary>
        /// Shadow cost of Do Nothing for age-zero (year 1) computed at the current iteration
        /// </summary>
        public Double DoNothingShadowCostAgeZero = 0.0;

        /// <summary>
        /// Shadow cost of Do Nothing for age-zero (year 1) computed at the current iteration
        /// </summary>
        public Double PrevDoNothingShadowCostAgeZero = -1.0;

        /// <summary>
        /// Minimum shadow cost among all do-something actions
        /// </summary>
        public Double MinActionShadowCost = -1.0;

        /// <summary>
        /// Number of the do-something action with the minimum shadow cost among all
        /// </summary>
        public Int32 MinShadowCostAction = -1;

        /// <summary>
        /// Number of items in the CostStream array
        /// </summary>
        public Int32 ShadowCostStreamLength = -1;

        /// <summary>
        /// Array of Shadow costs for the state's Do-Nothing action
        /// </summary>
        public Double[] ShadowCostStream = null;

        /// <summary>
        /// True if no change since previous iteration.
        /// </summary>
        public Boolean NoChange
        {
            get
            {
                Boolean b = (PrevAssignedAction == AssignedAction
                    && Math.Round(PrevShadowCost, 2) == Math.Round(ShadowCost, 2)
                    && Math.Round(PrevDoNothingShadowCostAgeZero, 2) == Math.Round(DoNothingShadowCostAgeZero, 2));

                if (b)
                {
                    foreach (WeibullMarkovAction action in Actions)
                    {
                        b = b && action.NoChange;
                        if (!b)
                            break;
                    }
                }

                return b;
            }
        }

   
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="e">Eta parameter</param>
        /// <param name="b">Beta parameter</param>
        public WeibullMarkovConditionState(Double e, Double b)
        {
            Eta = e;
            Beta = b;
            DoNothingCost = 0.0;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="e">Eta parameter</param>
        /// <param name="b">Beta parameter</param>
        /// <param name="c">Do nothing cost</param>
        public WeibullMarkovConditionState(Double e, Double b, Double c)
        {
            Eta = e;
            Beta = b;
            DoNothingCost = c;
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="e">Eta parameter</param>
        /// <param name="b">Beta parameter</param>
        /// <param name="c">Do nothing cost</param>
        /// <param name="at">Last tabulated t</param>
        /// <param name="af">Last tabulated f(t)</param>
        /// <<param name="aff">Last tabulated f'(t)</param>
        public WeibullMarkovConditionState(Double e, Double b, Double c, Double at, Double af, Double aff)
        {
            Eta = e;
            Beta = b;
            DoNothingCost = c;
            _T = at;
            _f = af;
            _ff = aff;
        }

        ~WeibullMarkovConditionState()
        {
            ClearActions();
            ClearRecommendations();
        }


        /// <summary>
        /// Removes allactions from the list
        /// </summary>
        public void ClearActions()
        {
            Actions.Clear();
        }

        /// <summary>
        /// Computes T,f and FF for S=0.01
        /// <returns>Computed value of F, which must be 0.99. Just for checking.</returns>
        /// </summary>
        public Double ComputeTFF()
        {
            T = Eta * Math.Exp(Math.Log(Math.Log(100.0)) / Beta);
            Double F = Weibull.CumulativeFailure(Eta, Beta, T);

            // XML accepts integer T only
            T = Math.Round(T, 0);
            f = Weibull.OneYearFailure(Eta, Beta, T);
            ff = Weibull.OneYearFailureFirstDerivative(Eta, Beta, T);
          
            return F;
        }
     
        /// <summary>
        /// Creates a new action object and adds it to the list
        /// </summary>
        /// <param name="number">Action number</param>
        /// <param name="numStates">Number of target states for the action</param>
        /// <param name="isApplicable">True of actio is applicable</param>
        /// <param name="cost">Unit action cost</param>
        /// <returns>Created action object</returns>
        public WeibullMarkovAction AddAction(Int32 number, Int32 numStates, Boolean isApplicable, Double cost)
        {
            WeibullMarkovAction action = new WeibullMarkovAction(number, numStates, isApplicable, cost);
            Actions.Add(action);
            return action;
        }

        /// <summary>
        /// Removes all policy recommendations
        /// </summary>
        public void ClearRecommendations()
        {
            Recommendations.Clear();
        }

        /// <summary>
        /// Adds policy recommendation for the state
        /// </summary>
        /// <param name="year">Age in the state (year)</param>
        /// <param name="actionNumber">Number of the recommended action: 1, 2, etc.</param>
        /// <param name="benefit">Unit benefit of the recommended action</param>
        /// <returns>Created recommendation object</returns>
        public WeibullMarkovStatePolicyRecommendation AddRecommendation(Int32 year, Int32 actionNumber, Double benefit)
        {
            WeibullMarkovStatePolicyRecommendation rec = new WeibullMarkovStatePolicyRecommendation(year, actionNumber, benefit);
            Recommendations.Add(rec);
            return rec;
        }


        /// <summary>
        /// Converts the object to XMl element
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
                xml = doc.CreateElement(CONDITION_STATE);
                XmlAttribute number = doc.CreateAttribute(_NUMBER);
                number.Value = Number.ToString();
                xml.Attributes.Append(number);

                XmlAttribute eta = doc.CreateAttribute(_ETA);
                eta.Value = Eta.ToString("g");
                xml.Attributes.Append(eta);

                XmlAttribute beta = doc.CreateAttribute(_BETA);
                beta.Value = Beta.ToString("g");
                xml.Attributes.Append(beta);

                XmlAttribute doNothingCost = doc.CreateAttribute(_DO_NOTHING_COST);
                doNothingCost.Value = DoNothingCost.ToString("f2");
                xml.Attributes.Append(doNothingCost);

                if (t50.HasValue)
                {
                    XmlAttribute at50 = doc.CreateAttribute(_T50);
                    at50.Value = t50.Value.ToString();
                    xml.Attributes.Append(at50);
                    if (t9X.HasValue && x != null)
                    {
                        XmlAttribute at9x = doc.CreateAttribute(_T9X);
                        at9x.Value = t9X.Value.ToString();
                        xml.Attributes.Append(at9x);

                        XmlAttribute ax = doc.CreateAttribute(_X);
                        ax.Value = x;
                        xml.Attributes.Append(ax);
                    }
                }

                if (T > 1.0 && _ff.HasValue && _f.HasValue)    // Otherwise it does not make sense
                {
                    XmlAttribute tTabulated = doc.CreateAttribute(_T_TABULATED);
                    tTabulated.Value = T.ToString();
                    xml.Attributes.Append(tTabulated);


                    XmlAttribute f = doc.CreateAttribute(_F_PROB_AT_T);
                    f.Value = _f.Value.ToString("g");
                    xml.Attributes.Append(f);

                    XmlAttribute ff = doc.CreateAttribute(_DFDT_AT_T);
                    ff.Value = _ff.Value.ToString("g");
                    xml.Attributes.Append(ff);
                }

                if (Actions != null && Actions.Count > 0)
                {
                    XmlElement n1 = doc.CreateElement(_ACTIONS);
                    foreach (WeibullMarkovAction action in Actions)
                    {
                        XmlElement n = action.ToXmlElement(doc, out errorMessage);
                        if (n == null)
                            throw new Exception(errorMessage);
                        n1.AppendChild(n);
                    }
                    xml.AppendChild(n1);
                }

                if (Recommendations != null && Recommendations.Count > 0)
                {
                    XmlElement n2 = doc.CreateElement(_POLICY);
                    foreach (WeibullMarkovStatePolicyRecommendation rec in Recommendations)
                    {
                        XmlElement r = rec.ToXmlElement(doc, out errorMessage);
                        if (r == null)
                            throw new Exception(errorMessage);
                        n2.AppendChild(r);
                    }
                    xml.AppendChild(n2);
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                xml = null;
            }

            return xml;
        }

        /// <summary>
        /// Constructs an object from the XML element
        /// </summary>
        /// <param name="xml">XML element</param>
        /// <param name="errorMessage">out Error message</param>
        /// <returns>Constructed object, null on failure</returns>
        public static WeibullMarkovConditionState FromXmlElement(XmlElement xml, out String errorMessage)
        {
            WeibullMarkovConditionState state = null;
            errorMessage = null;

            try
            {
                if (xml.Name != CONDITION_STATE)
                    throw new Exception("A <" + CONDITION_STATE + " ...> XML element expected.");
                state = new WeibullMarkovConditionState(0.0, 0.0);
                String number = xml.GetAttribute(_NUMBER);
                if (number == null)
                    throw new Exception("The '" + _NUMBER + "' attribute is missing in the State XML element");
                state.Number = Int32.Parse(number);
                String eta = xml.GetAttribute(_ETA);
                if (eta == null)
                    throw new Exception("The '" + _ETA + "' attribute is missing in the State XML element");
                state.Eta = Double.Parse(eta);
                String beta = xml.GetAttribute(_BETA);
                if (beta == null)
                    throw new Exception("The '" + _BETA + "' attribute is missing in the State XML element");
                state.Beta = Double.Parse(beta);

                String doNothCost = xml.GetAttribute(_DO_NOTHING_COST);
                if (!String.IsNullOrEmpty(doNothCost))
                    state.DoNothingCost = Double.Parse(doNothCost);

                String tTabulated = xml.GetAttribute(_T_TABULATED);
                if (!String.IsNullOrEmpty(tTabulated))
                    state.T = Double.Parse(tTabulated);

                String f = xml.GetAttribute(_F_PROB_AT_T);
                if (!String.IsNullOrEmpty(f))
                    state.f = Double.Parse(f);

                String ff = xml.GetAttribute(_DFDT_AT_T);
                if (!String.IsNullOrEmpty(ff))
                    state.ff = Double.Parse(ff);

                String at50 = xml.GetAttribute(_T50);
                if (!String.IsNullOrEmpty(at50))
                {
                    state.t50 = Double.Parse(at50);
                    String at9x = xml.GetAttribute(_T9X);
                    String ax = xml.GetAttribute(_X);
                    if (!String.IsNullOrEmpty(at9x) && !String.IsNullOrEmpty(ax))
                    {
                        state.t9X = Double.Parse(at9x);
                        state.x = ax;
                    }
                }

                if (xml.HasChildNodes)
                {
                    foreach (XmlNode n in xml.ChildNodes)
                    {
                        if (n.Name == _ACTIONS)
                        {
                            if (n.HasChildNodes)
                            {
                                foreach (XmlNode n1 in n.ChildNodes)
                                {
                                    if (n1.Name == WeibullMarkovAction.ACTION)
                                    {
                                        WeibullMarkovAction action = WeibullMarkovAction.FromXmlElement(n1 as XmlElement, out errorMessage);
                                        if (action == null)
                                            throw new Exception(errorMessage);
                                        state.Actions.Add(action);
                                    }
                                }
                            }
                        }

                        if (n.Name == _POLICY)
                        {
                            foreach (XmlNode n2 in n.ChildNodes)
                            {
                                if (n2.Name == WeibullMarkovStatePolicyRecommendation.RECOMMENDATION) 
                                {
                                    WeibullMarkovStatePolicyRecommendation rec = WeibullMarkovStatePolicyRecommendation.FromXmlElement(n2 as XmlElement, out errorMessage);
                                    if (rec == null)
                                        throw new Exception(errorMessage);
                                    state.Recommendations.Add(rec);
                                }
                                
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                state = null;
            }

            return state;
        }

        /// <summary>
        /// Assigns value for the shadow cost of Do Nothing for age zero (year 1).
        /// Saves the previous value.
        /// </summary>
        /// <param name="a">Shadow cost value to assign.</param>
        public void AssignDoNothingShadowCostAgeZero(Double a)
        {
            PrevDoNothingShadowCostAgeZero = DoNothingShadowCostAgeZero;
            DoNothingShadowCostAgeZero = a;
        }

        /// <summary>
        /// Assigns action, computes the shadow cost and saves the previous values.
        /// </summary>
        /// <param name="i">Number of the action to assign. 0 for Do Nothing.</param>
        public void AssignAction(Int32 i)
        {
            PrevAssignedAction = AssignedAction;
            AssignedAction = i;
            if (i == 0)
            {
                PrevShadowCost = ShadowCost;
                ShadowCost = DoNothingShadowCostAgeZero;
            }
            else
            {
                foreach (WeibullMarkovAction action in Actions)
                {
                    if (action.Number == i)
                    {
                        PrevShadowCost = ShadowCost;
                        ShadowCost = action.ShadowCost;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Sets the action that has the minimum shadow cost among all
        /// </summary>
        /// <param name="i">Number of the action</param>
        public void AssignMinShadowCostAction(Int32 i)
        {
            foreach (WeibullMarkovAction action in Actions)
                {
                    if (action.Number == i)
                    {
                        MinShadowCostAction = i;
                        MinActionShadowCost =  action.ShadowCost;
                        break;
                    }
                }
        }


        /// <summary>
        /// Resets MDM solution values preparing for the next iteration cycle
        /// </summary>
        public void ResetSolution()
        {
            Recommendations.Clear();

            PrevAssignedAction = -1;
            AssignedAction = 0;
            PrevDoNothingShadowCostAgeZero = -1.0;
            DoNothingShadowCostAgeZero = 0.0;
            PrevShadowCost = -1.0;
            ShadowCost = 0.0;
         
            foreach(WeibullMarkovAction action in Actions)
            {
               action.ResetSolution();
            }
        }
    }
}
