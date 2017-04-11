using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AdvancedXML.Tests.SchemaValidation
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
            var validator = new SchemaValidator();
            var result = validator.Validate("Resources/xml/books.xml");
            Assert.AreEqual(true, result.IsSuccess);
        }

        [TestMethod]
        public void FailedValidation()
        {
            var validator = new SchemaValidator();
            var result = validator.Validate("Resources/xml/wrong-books.xml");
            Assert.AreEqual(false, result.IsSuccess);

            /*
                How did you ensure that errors returned by "validator" are the errors you have expected to happen?
            */

            foreach (var message in result.Messages)
            {
                Console.WriteLine(message);
            }
        }
    }
}
