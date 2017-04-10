using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml.Xsl;

namespace AdvancedXML.Tests.RssFeedGenerator
{
    /// <summary>
    ///     Summary description for Tests
    /// </summary>
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void SuccesfulValidation()
        {
            var xsl = new XslCompiledTransform();
            var settings = new XsltSettings { EnableScript = true };
            xsl.Load("src/RssFeedGenerator/RssGenerator.xslt", settings, null);
            xsl.Transform("../../Resources/xml/books.xml", null, Console.Out);
        }
    }
}
