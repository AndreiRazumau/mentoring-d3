using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml.Xsl;

namespace AdvancedXML.Tests.HTMLReport
{
    /// <summary>
    ///     Summary description for Tests
    /// </summary>
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void GenerateHTMLReport()
        {
            var xsl = new XslCompiledTransform();
            var settings = new XsltSettings { EnableScript = true };
            xsl.Load("src/HTMLReport/HtmlGenerator.xslt", settings, null);
            using (var stream = new FileStream("result.html", FileMode.OpenOrCreate))
            {
                xsl.Transform("../../Resources/xml/books.xml", null, stream);
            }
        }
    }
}
