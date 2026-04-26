using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MD.CMS.BusinessLogic.Core.Tests.DataAccess.Entities
{
    [TestClass]
    public class TaxonomyTests
    {
        [TestMethod]
        public void IsNew_ReturnsTrue_WhenIdIsDefault()
        {
            var taxonomy = new Taxonomy { Id = 0 };

            Assert.IsTrue(taxonomy.IsNew);
        }

        [TestMethod]
        public void IsNew_ReturnsFalse_WhenIdIsAssigned()
        {
            var taxonomy = new Taxonomy { Id = 1 };

            Assert.IsFalse(taxonomy.IsNew);
        }

        [TestMethod]
        public void Children_ReturnsInitializedList_WhenChildrenWasNull()
        {
            var taxonomy = new Taxonomy();

            Assert.IsNotNull(taxonomy.Children);
            Assert.AreEqual(0, taxonomy.Children.Count);
        }

        [TestMethod]
        public void ToString_ReturnsId()
        {
            var taxonomy = new Taxonomy { Id = 55 };

            Assert.AreEqual("55", taxonomy.ToString());
        }
    }
}
