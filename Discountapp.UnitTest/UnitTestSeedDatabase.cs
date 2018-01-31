using System;
using Discountapp.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Discountapp.UnitTest
{
    [TestClass]
    public class UnitTestSeedDatabase
    {
        [TestMethod]
        public void TestSeedingOfDatabase()
        {
            var data = new DiscountappMemoryContext();

            Equals(true, data.Cities.Count > 0);
        }
    }
}
