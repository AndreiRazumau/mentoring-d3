using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnmanagedCalculator.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void TestAddMethod()
        {
            using (var calcWrapper = new CalculatorWrapperNsp.CalculatorWrapper())
            {
                Assert.AreEqual(10, calcWrapper.Add(2, 8));
            }
        }
    }
}
