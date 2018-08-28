using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;

using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;


namespace TWeibullMarkovLibrary
{
    public class Utilities
    {

        private static Int32 _numErrors;
        private static Int32 _numWarnings;
        private static String _xmlErrMessage = String.Empty;

     
        /// <summary>
        ///   XML schema validator function
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "args"></param>
        private static void validator(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
            {
                _xmlErrMessage += args.Message;
                _xmlErrMessage += "\r\n";
                _numWarnings++;
            }

            else if (args.Severity == XmlSeverityType.Error)
            {
                _xmlErrMessage += args.Message;
                _xmlErrMessage += "\r\n";
                _numErrors++;
            }
        }


        /// <summary>
        ///   Validates XML file against the provided schema.
        /// </summary>
        /// <param name = "xmlPathName">Name of the XML file to load.</param>
        /// <param name = "xsdPathName">Name of the XSD file to validate against. null if no validation required.</param>
        /// <param name = "errorMessage">out Error Message</param>
        /// <returns>True on success, False on failure</returns>
        public static Boolean ValidateXMLvsXSD(String xmlPathName, String xsdPathName, ILog log, out String errorMessage)
        {
            Boolean ok = true;
            errorMessage = null;

            if (log != null)
                log.InfoFormat("Validating {0} against the schema {1}.... started", xmlPathName, xsdPathName);

            XmlDocument doc = null;
            XmlReader r = null;

            try
            {
                _numErrors = 0;
                _numWarnings = 0;
                _xmlErrMessage = String.Empty;

                if (!System.IO.File.Exists(xsdPathName))
                {
                    throw new Exception(String.Format("Schema file {0} not found.", xsdPathName));
                }
                var xrs = new XmlReaderSettings();
                xrs.Schemas.Add(null, xsdPathName);

                xrs.ValidationEventHandler += validator;
                xrs.ValidationFlags = xrs.ValidationFlags | XmlSchemaValidationFlags.ReportValidationWarnings;
                xrs.ValidationType = ValidationType.Schema;

                r = XmlReader.Create(xmlPathName, xrs);
                doc = new XmlDocument();
                doc.Load(r);
                r.Close();

                doc.Validate(validator);

                if (!String.IsNullOrEmpty(_xmlErrMessage))
                {
                    String err = String.Format("Errors: {0}\r\nWarnings: {1}\r\n{2}", _numErrors, _numWarnings,
                                               _xmlErrMessage);
                    throw new Exception(err);
                }

                ok = true;
            }
            catch (XmlException xmlEx)
            {
                errorMessage = xmlEx.Message;
                if (log != null)
                    log.Error(errorMessage, xmlEx);
                ok = false;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (log != null)
                    log.Error(errorMessage, ex);
                ok = false;
            }
            finally
            {
            }

            if (log != null)
                log.InfoFormat("Validating {0} against the schema {1}. ok={2}.  Error: ({3}).... ended", xmlPathName,
                           xsdPathName, ok, String.IsNullOrEmpty(errorMessage) ? "none" : errorMessage);

            if (r != null)
                if (r.ReadState != ReadState.Closed)
                {
                    r.Close();
                }

            GC.Collect();

            return (ok);
        }
    }
}
