using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Discountapp.Infrastructure;

namespace Discountapp.UnitTest
{
    [TestClass]
    public class UnitTestExtensions
    {
        [TestMethod]
        public void CheckStringExtension_ControllerName()
        {
            var expected = "Test";
            var actual = "TestController".ControllerName();

            Assert.AreEqual(expected, actual);
        }
    }
}
