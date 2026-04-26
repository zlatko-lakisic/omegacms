using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MD.CMS.BusinessLogic.Core.Tests.DataAccess.Entities
{
    [TestClass]
    public class FolderTests
    {
        [TestMethod]
        public void IsHidden_ReturnsTrue_WhenNameStartsWithDot()
        {
            var folder = new Folder<Content> { Name = ".private" };

            Assert.IsTrue(folder.IsHidden);
        }

        [TestMethod]
        public void IsHidden_ReturnsFalse_WhenNameIsNormal()
        {
            var folder = new Folder<Content> { Name = "public" };

            Assert.IsFalse(folder.IsHidden);
        }

        [TestMethod]
        public void IsNew_ReturnsTrue_WhenIdIsDefault()
        {
            var folder = new Folder<Content> { Id = 0 };

            Assert.IsTrue(folder.IsNew);
        }

        [TestMethod]
        public void IsNew_ReturnsFalse_WhenIdIsAssigned()
        {
            var folder = new Folder<Content> { Id = 99 };

            Assert.IsFalse(folder.IsNew);
        }

        [TestMethod]
        public void ToString_ReturnsFolderName()
        {
            var folder = new Folder<Content> { Name = "News" };

            Assert.AreEqual("News", folder.ToString());
        }
    }
}
