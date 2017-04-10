using System.Reflection;
using System.Xml;
using System.Xml.Schema;

namespace AdvancedXML.Tests.SchemaValidation
{
    /// <summary>
    ///     Validate XML file.
    /// </summary>
    public class SchemaValidator
    {
        #region [Private members]

        private ValidationResult _result;

        #endregion

        /// <summary>
        ///     Validate input xml file.
        /// </summary>
        /// <param name="filePath">The xml file path.</param>
        public ValidationResult Validate(string filePath)
        {
            var settings = new XmlReaderSettings();
            this._result = new ValidationResult();
            using (var xsdStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AdvancedXML.Tests.Resources.xsd.books.xsd"))
            {
                if (xsdStream != null)
                {
                    settings.ValidationType = ValidationType.Schema;
                    settings.Schemas.Add(XmlSchema.Read(xsdStream, null));
                }
            }

            settings.ValidationEventHandler +=
                delegate (object sender, ValidationEventArgs e)
                {
                    this._result.Messages.Enqueue($"{e.Message} Line Number: {e.Exception.LineNumber}, Line Position: {e.Exception.LinePosition}");
                    this._result.IsSuccess = false;
                };

            settings.ValidationFlags = settings.ValidationFlags | XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationType = ValidationType.Schema;

            XmlReader reader = XmlReader.Create(filePath, settings);
            while (reader.Read());

            return this._result;
        }
    }
}
