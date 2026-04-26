using System;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MD.CMS.BusinessLogic.Core.Tests.DataAccess.Entities
{
    [TestClass]
    public class ContentTests
    {
        [TestMethod]
        public void UniqueId_ReturnsIdAndLcid_WhenContentIsNotNew()
        {
            var content = new Content
            {
                Id = "42",
                LCID = 2057
            };

            Assert.AreEqual("42-2057", content.UniqueId);
        }

        [TestMethod]
        public void UniqueId_ReturnsEmpty_WhenContentIsNew()
        {
            var content = new Content
            {
                Id = string.Empty
            };

            Assert.AreEqual(string.Empty, content.UniqueId);
        }

        [TestMethod]
        public void GetPermissionEntityId_ReturnsIdAndLcid()
        {
            var content = new Content
            {
                Id = "7",
                LCID = 1033
            };

            Assert.AreEqual("7-1033", content.GetPermissionEntityId());
        }

        [TestMethod]
        public void ShouldSerializeMethods_ReturnTrue()
        {
            var content = new Content();

            Assert.IsTrue(content.ShouldSerializeLCID());
            Assert.IsTrue(content.ShouldSerializeDateCreated());
            Assert.IsTrue(content.ShouldSerializeAuthorId());
            Assert.IsTrue(content.ShouldSerializeAuthor());
            Assert.IsTrue(content.ShouldSerializeFolderId());
            Assert.IsTrue(content.ShouldSerializeTaxonomyId());
            Assert.IsTrue(content.ShouldSerializeTitle());
            Assert.IsTrue(content.ShouldSerializePath());
            Assert.IsTrue(content.ShouldSerializeHtml());
            Assert.IsTrue(content.ShouldSerializeContentType());
            Assert.IsTrue(content.ShouldSerializeContentAlias());
            Assert.IsTrue(content.ShouldSerializeIsNew());
            Assert.IsTrue(content.ShouldSerializeTaxonomy());
            Assert.IsTrue(content.ShouldSerializeMenu());
            Assert.IsTrue(content.ShouldSerializeMetaDataFieldValues());
        }

        [TestMethod]
        public void Compare_ReturnsDateComparison_WhenBothArePresent()
        {
            var older = new Content { DateCreated = "2020-01-01 00:00:00" };
            var newer = new Content { DateCreated = "2020-01-02 00:00:00" };

            int comparison = Content.Compare(older, newer);

            Assert.IsTrue(comparison < 0);
        }

        [TestMethod]
        public void CompareTo_ReturnsOne_WhenOtherIsNull()
        {
            var content = new Content { DateCreated = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") };

            int comparison = content.CompareTo(null);

            Assert.AreEqual(1, comparison);
        }

        [TestMethod]
        public void CompareTo_ReturnsDateComparison_WhenOtherIsPresent()
        {
            var first = new Content { DateCreated = "2020-01-03 00:00:00" };
            var second = new Content { DateCreated = "2020-01-01 00:00:00" };

            int comparison = first.CompareTo(second);

            Assert.IsTrue(comparison > 0);
        }
    }
}
