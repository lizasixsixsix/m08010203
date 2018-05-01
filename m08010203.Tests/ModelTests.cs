using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace m08010203.Tests
{
    [TestClass]
    public class ModelTests
    {
        private NorthwindDB nw;

        [TestInitialize]
        public void Initialize()
        {
            this.nw = new NorthwindDB();
        }

        [TestCleanup]
        public void Cleanup()
        {
            nw.Dispose();
        }

        [TestMethod]
        public void SameCategoryOrders()
        {
            var catId = nw.Categories.First().CategoryID;

            var catOrds = nw.Orders
                .Where(o => o.Order_Details
                    .Any(d => d.Product.Category.CategoryID == catId))
                .Select(
                    o => new
                    {
                        customer = o.Customer.ContactName,
                        products = o.Order_Details.Select(d => d.Product.ProductName)
                    })
                .ToList();

            Assert.IsTrue(catOrds.Count() > 0);

            Console.WriteLine(catOrds
                .Select(o => o.customer + "\n"
                             + o.products.Aggregate((a, b) => a + "\t" + b))
                .Aggregate((a, b) => a + "\n\n" + b));
        }
    }
}
