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
                Assert.AreEqual(emp.Result, "Workstation:(EPRUIZHW0249)");
                break;
            }
        }

        [TestMethod]
        public void WhereOperationWithSecondConstantParam()
        {
            var employees = new EntitySet<UserEntity>();

            foreach (var emp in employees.Where(e => e.Workstation == "EPRUIZHW0249"))
            {
                Assert.AreEqual(emp.Result, "Workstation:(EPRUIZHW0249)");
                break;
            }
        }

        [TestMethod]
        public void WhereOperationWithStartsWith()
        {
            var employees = new EntitySet<UserEntity>();

            foreach (var emp in employees.Where(e => e.Workstation.StartsWith("EPRUIZHW02")))
            {
                Assert.AreEqual(emp.Result, "Workstation:(EPRUIZHW02*)");
                break;
            }
        }

        [TestMethod]
        public void WhereOperationWithEndsWith()
        {
            var employees = new EntitySet<UserEntity>();

            foreach (var emp in employees.Where(e => e.Workstation.EndsWith("IZHW02")))
            {
                Assert.AreEqual(emp.Result, "Workstation:(*IZHW02)");
                break;
            }
        }

        [TestMethod]
        public void WhereOperationWithContains()
        {
            var employees = new EntitySet<UserEntity>();

            foreach (var emp in employees.Where(e => e.Workstation.Contains("RUIZHW")))
            {
                Assert.AreEqual(emp.Result, "Workstation:(*RUIZHW*)");
                break;
            }
        }
    }
}
