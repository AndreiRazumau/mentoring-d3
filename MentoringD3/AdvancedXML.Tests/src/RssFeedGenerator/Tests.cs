﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
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
        public void GenerateRSS()
        {
            var xsl = new XslCompiledTransform();
            var settings = new XsltSettings { EnableScript = true };
            xsl.Load("src/RssFeedGenerator/RssGenerator.xslt", settings, null);

            using (var fs = new FileStream("../../Resources/xml/books.rss", FileMode.OpenOrCreate, FileAccess.Write))
            {
                xsl.Transform("../../Resources/xml/books.xml", null, fs);
            }
        }
    }
}
