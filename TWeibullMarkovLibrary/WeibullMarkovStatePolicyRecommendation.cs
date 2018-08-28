using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TWeibullMarkovLibrary
{

    /// <summary>
    /// Data object that holds optimal recommendation for the given condition state
    /// in the given age.
    /// </summary>
    public class WeibullMarkovStatePolicyRecommendation
    {
        public static readonly String RECOMMENDATION = "Recommendation";

        private static readonly String _AGE_YEAR = "age-year";
        private static readonly String _ACTION = "action";
        private static readonly String _UNIT_BENEFIT = "unit-benefit";

        /// <summary>
        /// Age in condition state, year starting at 1
        /// </summary>
        public Int32 Year;

        /// <summary>
        /// Number of the recommended action, 0 for Do Nothing
        /// </summary>
        public Int32 ActionNumber = 0;

        /// <summary>
        /// Unit benefit of the recommended action, 0.00 for Do Nothing
        /// </summary>
        public Double Benefit = 0.0;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="year">Age, year</param>
        /// <param name="actionNumber">Number of recommended action</param>
        /// <param name="benefit">Unit benefit</param>
        public WeibullMarkovStatePolicyRecommendation(Int32 year, Int32 actionNumber, Double benefit)
        {
            Year = year;
            ActionNumber = actionNumber;
            Benefit = benefit;
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
                xml = doc.CreateElement(RECOMMENDATION);
                XmlAttribute year = doc.CreateAttribute(_AGE_YEAR);
                year.Value = Year.ToString();
                xml.Attributes.Append(year);
                XmlAttribute action = doc.CreateAttribute(_ACTION);
                action.Value = ActionNumber.ToString();
                xml.Attributes.Append(action);
                XmlAttribute benefit = doc.CreateAttribute(_UNIT_BENEFIT);
                benefit.Value = Benefit.ToString();
                xml.Attributes.Append(benefit);
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
                xml = null;
            }

            return xml;
        }

        /// <summary>
        /// Constructs an object of the class from the given XML element
        /// </summary>
        /// <param name="xml">XML element</param>
        /// <param name="errorMessage">out Error message</param>
        /// <returns>Constructed object, null on failure</returns>
        public static WeibullMarkovStatePolicyRecommendation FromXmlElement(XmlElement xml, out String errorMessage)
        {
            WeibullMarkovStatePolicyRecommendation rec = null;
            errorMessage = null;

            try
            {
                if (xml.Name != RECOMMENDATION)
                    throw new Exception("Expected a <" + RECOMMENDATION + " ...> XML element");
                rec = new WeibullMarkovStatePolicyRecommendation(0, 0, 0.0);
                if (xml.HasAttributes)
                {
                    foreach (XmlAttribute attr in xml.Attributes)
                    {
                        if (attr.Name == _AGE_YEAR)
                            rec.Year = Int32.Parse(attr.Value.Trim());
                        else if (attr.Name == _ACTION)
                            rec.ActionNumber = Int32.Parse(attr.Value.Trim());
                        else if (attr.Name == _UNIT_BENEFIT)
                            rec.Benefit = Double.Parse(attr.Value.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                rec = null;
            }

            return rec;
        }


    }


   
   
}
