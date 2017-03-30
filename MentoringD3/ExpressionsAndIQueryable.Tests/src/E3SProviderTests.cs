using ExpressionsAndIQueryable.Tests.E3SClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ExpressionsAndIQueryable.Tests
{
    [TestClass]
    public class E3SProviderTests
    {
        [TestMethod]
        public void WithProvider()
        {
            var employees = new E3SEntitySet<E3SEntity>();

            foreach (var emp in employees.Where(e => e.Workstation == "EPRUIZHW0249"))
            {
                Console.WriteLine(emp.Result);
            }
        }
    }
}
