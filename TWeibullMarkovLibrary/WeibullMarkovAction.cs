using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TWeibullMarkovLibrary
{
    /// <summary>
    /// Data object that encapsulates the information about remedial action applied to a condition state.
    /// </summary>
    public class WeibullMarkovAction
    {

        public static readonly String ACTION = "Action";

        private static readonly String _DESCRIPTION = "Description";
        private static readonly String _APPLICABILITY = "applicability";
        private static readonly String _NUM_TARGET_STATES = "num-target-states";
        private static readonly String _UNIT_COST = "unit-cost";
        private static readonly String _TARGET_STATE = "Target-State";
        private static readonly String _TRANSITION_PROBABILITIES = "Transition-Probabilities";
        private static readonly String _NUMBER = "number";
        private static readonly String _PROBABILITY = "probability";

        /// <summary>
        /// Action number: 1, 2, etc.
        /// </summary>
        public Int32 Number;


        /// <summary>
        /// Optional description
        /// </summary>
        public String Description = null;

        /// <summary>
        /// True if action is applicable
        /// </summary>
        public Boolean IsApplicable = false;

        /// <summary>
        /// Unit cost
        /// </summary>
        public Double Cost = 0.0;

        /// <summary>
        /// Probability of transitioning to other condition states
        /// </summary>
        public Double[] TranProb = null;

        /// <summary>
        /// Shadow cost computed at the current iteration
        /// </summary>
        public Double ShadowCost = 0.0;

        /// <summary>
        /// Shadow cost computed at the previous iteration
        /// </summary>
        public Double PrevShadowCost = -1.0;


        /// <summary>
        /// Returns True if shadow cost computed at the current iteration is equal to the cent to the shadow
        /// cost computed at the previous iteration.
        /// </summary>
        public Boolean NoChange
        {
            get
            {
                return (Math.Round(PrevShadowCost, 2) == Math.Round(ShadowCost, 2));
            }
        }


       
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="number">Action number</param>
        /// <param name="numStates">Number of condition states</param>
        /// <param name="isApplicable">True of action is applicable</param>
        /// <param name="cost">Unit cost</param>
        public WeibullMarkovAction(Int32 number, Int32 numStates, Boolean isApplicable, Double cost)
        {
            Number = number;
            IsApplicable = isApplicable;
            if (numStates > 0)
                TranProb = new Double[numStates];
            else
                TranProb = null;
            Cost = cost;
        }

        /// <summary>
        /// Converts object to the XML element
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
                xml = doc.CreateElement(ACTION);
                XmlAttribute number = doc.CreateAttribute(_NUMBER);
                number.Value = Number.ToString();
                xml.Attributes.Append(number);
                if (!String.IsNullOrEmpty(Description))
                {
                    XmlAttribute description = doc.CreateAttribute(_DESCRIPTION);
                    description.Value = Description;
                    xml.Attributes.Append(description);
                }
                XmlAttribute applicable = doc.CreateAttribute(_APPLICABILITY);
                applicable.Value = IsApplicable ? "1" : "0";
                xml.Attributes.Append(applicable);
                XmlAttribute numStates = doc.CreateAttribute(_NUM_TARGET_STATES);
                numStates.Value = TranProb == null ? "0" : TranProb.Length.ToString();
                xml.Attributes.Append(numStates);
                XmlAttribute cost = doc.CreateAttribute(_UNIT_COST);
                cost.Value = Cost.ToString();
                xml.Attributes.Append(cost);

                if (TranProb != null && TranProb.Length > 0)
                {
                    XmlNode tp = doc.CreateElement(_TRANSITION_PROBABILITIES);
                    for (Int32 i = 0; i < TranProb.Length; i++)
                    {
                        Double p = TranProb[i];
                        XmlElement toState = doc.CreateElement(_TARGET_STATE);
                        XmlAttribute stateN = doc.CreateAttribute(_NUMBER);
                        stateN.Value = (i + 1).ToString();
                        toState.Attributes.Append(stateN);
                        XmlAttribute prob = doc.CreateAttribute(_PROBABILITY);
                        prob.Value = p.ToString();
                        toState.Attributes.Append(prob);
                        tp.AppendChild(toState);
                    }
                    xml.AppendChild(tp);
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
        /// Constructs an object from XML element
        /// </summary>
        /// <param name="xml">XML element</param>
        /// <param name="errorMessage">out Error message</param>
        /// <returns>Constructed element, null on failure</returns>
        public static WeibullMarkovAction FromXmlElement(XmlElement xml, out String errorMessage)
        {
            WeibullMarkovAction action = null;
            errorMessage = null;

            try
            {   
                if (xml.Name != ACTION)
                    throw new Exception("Expected a </" + ACTION + " ...> XML element");

                Int32 numStates = 0;

                if (xml.HasChildNodes)
                {
                    XmlNode n = xml.FirstChild;
                    if (n.Name == _TRANSITION_PROBABILITIES)
                    {
                        if (n.HasChildNodes)
                        {
                            numStates = n.ChildNodes.Count;
                        }
                    }
                    else
                    {
                        throw new Exception("XML node <" + _TRANSITION_PROBABILITIES + "> expected.");
                    }
                }

                action = new WeibullMarkovAction(0, numStates, false, 0.0);

                if (xml.HasAttributes)
                {
                    foreach (XmlAttribute attr in xml.Attributes)
                    {
                        if (attr.Name == _NUMBER)
                            action.Number = Int32.Parse(attr.Value);
                        else if (attr.Name == _APPLICABILITY)
                            action.IsApplicable = attr.Value.Trim().ToUpper() == "1" || attr.Value.Trim().ToUpper() == "Y";
                        else if (attr.Name == _NUM_TARGET_STATES)
                            numStates = Int32.Parse(attr.Value.Trim());
                        else if (attr.Name == _UNIT_COST)
                            action.Cost = Double.Parse(attr.Value.Trim());
                        else if (attr.Name == _DESCRIPTION)
                            action.Description = attr.Value;
                    }

                    if (numStates > 0 && action.TranProb == null)
                    {
                        throw new Exception(String.Format("The declared number of target condition states is {0} but none are present in the XML", numStates)); 
                    }
                    else if (action.TranProb != null && action.TranProb.Length != numStates) 
                    {
                        throw new Exception(String.Format("The declared number of target condition states ({0}) different from the number of their XML nodes ({1})", numStates, action.TranProb.Length)); 
                    }
                }

                if (xml.HasChildNodes)
                {
                    XmlNode n = xml.FirstChild;
                    if (n.Name == _TRANSITION_PROBABILITIES)
                    {
                        foreach (XmlElement n2 in n.ChildNodes)
                        {
                            if (n2.Name == _TARGET_STATE)
                            {
                                if (n2.HasAttributes)
                                {
                                    String s = n2.GetAttribute(_NUMBER);
                                    if (String.IsNullOrEmpty(s))
                                        throw new Exception("The '" + _NUMBER + "' attribute is missing in the Target-State XML element.");
                                    Int32 i = Int32.Parse(s);
                                    if (i < 1 || i > numStates)
                                    {
                                        throw new Exception(String.Format("The value of the '" + _NUMBER + "' attribute ({0}) for the Target-State is out of range (1 - {1})", s, numStates));
                                    }
                                    i--;
                                    s = n2.GetAttribute(_PROBABILITY);
                                    if (String.IsNullOrEmpty(s))
                                        throw new Exception("The '" + _PROBABILITY + "' attribute is missing in the Target-State XML element.");
                                    action.TranProb[i] = Double.Parse(s.Trim());
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                action = null;
            }
            return action;
        }

        /// <summary>
        /// Assigns value for the shadow cost and saves the previous value.
        /// </summary>
        /// <param name="shCost">Shadow cost value to assign</param>
        public void AssignShadowCost(Double shCost)
        {
            PrevShadowCost = ShadowCost;
            ShadowCost = shCost;
        }


        /// <summary>
        /// Resets MDM solution values preparing for the next iteration cycle
        /// </summary>
        public void ResetSolution()
        {
            PrevShadowCost = -1.0;
            ShadowCost = 0.0;
        }
    }
}
