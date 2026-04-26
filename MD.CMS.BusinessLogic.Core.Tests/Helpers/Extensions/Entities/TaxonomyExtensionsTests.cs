using System.Collections.Generic;
using System.Linq;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MD.CMS.BusinessLogic.Core.Tests.Helpers.Extensions.Entities
{
    [TestClass]
    public class TaxonomyExtensionsTests
    {
        [TestMethod]
        public void GetJson_MapsTaxonomyHierarchyToNestedJson()
        {
            var child = new Taxonomy
            {
                Id = 12,
                ParentId = 10,
                Name = "Child"
            };

            var parent = new Taxonomy
            {
                Id = 10,
                ParentId = 0,
                Name = "Parent",
                Children = new List<Taxonomy> { child }
            };

            var result = new List<Taxonomy> { parent }.GetJson().ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Parent", result[0].Name);
            Assert.AreEqual("10", result[0].Value);
            Assert.AreEqual("0", result[0].ParentId);
            Assert.IsNotNull(result[0].Children);
            Assert.AreEqual(1, result[0].Children.Count());
            Assert.AreEqual("Child", result[0].Children.First().Name);
        }
    }
}
