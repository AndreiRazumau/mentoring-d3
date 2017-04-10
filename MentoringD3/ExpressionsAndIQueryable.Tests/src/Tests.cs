using ExpressionsAndIQueryable.Tests.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ExpressionsAndIQueryable.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void WhereOperationWithFirstConstantParam()
        {
            var employees = new EntitySet<UserEntity>();

            foreach (var emp in employees.Where(e => "EPRUIZHW0249" == e.Workstation))
            {
                Assert.AreEqual("Workstation:(EPRUIZHW0249)", emp.Result);
                break;
            }
        }

        [TestMethod]
        public void WhereOperationWithSecondConstantParam()
        {
            var employees = new EntitySet<UserEntity>();

            foreach (var emp in employees.Where(e => e.Workstation == "EPRUIZHW0249"))
            {
                Assert.AreEqual("Workstation:(EPRUIZHW0249)", emp.Result);
                break;
            }
        }

        [TestMethod]
        public void WhereOperationWithStartsWith()
        {
            var employees = new EntitySet<UserEntity>();

            foreach (var emp in employees.Where(e => e.Workstation.StartsWith("EPRUIZHW02")))
            {
                Assert.AreEqual("Workstation:(EPRUIZHW02*)", emp.Result);
                break;
            }
        }

        [TestMethod]
        public void WhereOperationWithEndsWith()
        {
            var employees = new EntitySet<UserEntity>();

            foreach (var emp in employees.Where(e => e.Workstation.EndsWith("IZHW02")))
            {
                Assert.AreEqual("Workstation:(*IZHW02)", emp.Result);
                break;
            }
        }

        [TestMethod]
        public void WhereOperationWithContains()
        {
            var employees = new EntitySet<UserEntity>();

            foreach (var emp in employees.Where(e => e.Workstation.Contains("RUIZHW")))
            {
                Assert.AreEqual("Workstation:(*RUIZHW*)", emp.Result);
                break;
            }
        }

        [TestMethod]
        public void WhereOperationAndWhereOperationWithContains()
        {
            var employees = new EntitySet<UserEntity>();

            foreach (var emp in employees.Where(e => e.Workstation.Contains("RUIZHW") && e.Workstation.Contains("XZ")))
            {
                Assert.AreEqual("(Workstation:(*XZ*))&&(Workstation:(*RUIZHW*))", emp.Result);
                break;
            }
        }
    }
}
